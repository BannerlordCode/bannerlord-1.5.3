using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000178 RID: 376
	public class MatchHistoryDataContainer : MultiplayerLocalDataContainer<MatchHistoryData>
	{
		// Token: 0x06000AA8 RID: 2728 RVA: 0x00011363 File Offset: 0x0000F563
		public MatchHistoryDataContainer()
		{
			this._matchesToRemove = new List<MatchHistoryData>();
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00011376 File Offset: 0x0000F576
		protected override string GetSaveDirectoryName()
		{
			return "Data";
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0001137D File Offset: 0x0000F57D
		protected override string GetSaveFileName()
		{
			return "History.json";
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00011384 File Offset: 0x0000F584
		protected override void OnBeforeRemoveEntry(MatchHistoryData item, out bool canRemoveEntry)
		{
			base.OnBeforeRemoveEntry(item, out canRemoveEntry);
			this._matchesToRemove.Remove(item);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0001139C File Offset: 0x0000F59C
		protected override void OnBeforeAddEntry(MatchHistoryData item, out bool canAddEntry)
		{
			bool flag;
			base.OnBeforeAddEntry(item, out flag);
			MBReadOnlyList<MatchHistoryData> entries = base.GetEntries();
			bool flag2 = false;
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].MatchId == item.MatchId)
				{
					MatchHistoryDataContainer.PrintDebugLog("Found existing match with id trying to replace: " + entries[i].MatchId);
					base.RemoveEntry(entries[i]);
					this._matchesToRemove.Add(entries[i]);
					base.InsertEntry(item, i);
					MatchHistoryDataContainer.PrintDebugLog("Replaced existing match: (" + entries[i].MatchId + ") with: " + item.MatchId);
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				int num = this.GetEntryCountOfMatchType(item.MatchType) + 1 - 10;
				if (num > 0)
				{
					MatchHistoryDataContainer.PrintDebugLog(string.Format("Max match count is reached, removing ({0}) matches with type: {1}", num, item.MatchType));
					List<MatchHistoryData> oldestMatches = this.GetOldestMatches(item.MatchType, num);
					for (int j = 0; j < oldestMatches.Count; j++)
					{
						if (!this._matchesToRemove.Contains(oldestMatches[j]))
						{
							base.RemoveEntry(oldestMatches[j]);
							this._matchesToRemove.Add(oldestMatches[j]);
						}
					}
				}
			}
			canAddEntry = !flag2;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x000114F8 File Offset: 0x0000F6F8
		protected override List<MatchHistoryData> DeserializeInCompatibilityMode(string serializedJson)
		{
			List<MatchHistoryData> list = new List<MatchHistoryData>();
			try
			{
				MBList<MatchHistoryData> mblist = JsonConvert.DeserializeObject<MBList<MatchHistoryData>>(serializedJson);
				for (int i = 0; i < mblist.Count; i++)
				{
					list.Add(mblist[i]);
				}
			}
			catch
			{
				Debug.FailedAssert("Failed to resolve match history in compatibility mode. Resetting the file.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\LocalData\\MatchHistoryDataContainer.cs", "DeserializeInCompatibilityMode", 228);
			}
			return list;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00011560 File Offset: 0x0000F760
		public bool TryGetHistoryData(string matchId, out MatchHistoryData historyData)
		{
			historyData = null;
			MBReadOnlyList<MatchHistoryData> entries = base.GetEntries();
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].MatchId == matchId)
				{
					historyData = entries[i];
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000115A8 File Offset: 0x0000F7A8
		private List<MatchHistoryData> GetOldestMatches(string matchType, int count = 1)
		{
			DateTime maxValue = DateTime.MaxValue;
			List<MatchHistoryData> list = new List<MatchHistoryData>();
			MBReadOnlyList<MatchHistoryData> entries = base.GetEntries();
			entries.OrderBy<MatchHistoryData, DateTime>((MatchHistoryData e) => e.MatchDate);
			int num = 0;
			foreach (MatchHistoryData matchHistoryData in entries)
			{
				if (matchHistoryData == null)
				{
					Debug.FailedAssert("Trying to remove null match history data", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\LocalData\\MatchHistoryDataContainer.cs", "GetOldestMatches", 267);
				}
				else
				{
					if (matchHistoryData.MatchType == matchType)
					{
						list.Add(matchHistoryData);
						num++;
					}
					if (num == count)
					{
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00011668 File Offset: 0x0000F868
		private int GetEntryCountOfMatchType(string matchType)
		{
			int num = 0;
			using (List<MatchHistoryData>.Enumerator enumerator = base.GetEntries().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.MatchType == matchType)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x000116C8 File Offset: 0x0000F8C8
		private static void PrintDebugLog(string text)
		{
			Debug.Print("[MATCH_HISTORY]: " + text, 0, Debug.DebugColor.Yellow, 17592186044416UL);
		}

		// Token: 0x0400054E RID: 1358
		private const int MaxMatchCountPerMatchType = 10;

		// Token: 0x0400054F RID: 1359
		private List<MatchHistoryData> _matchesToRemove;
	}
}
