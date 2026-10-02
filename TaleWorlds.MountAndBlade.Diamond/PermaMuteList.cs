using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000141 RID: 321
	public static class PermaMuteList
	{
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0000CB8A File Offset: 0x0000AD8A
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x0000CB91 File Offset: 0x0000AD91
		public static bool HasMutedPlayersLoaded { get; private set; }

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x0000CB9C File Offset: 0x0000AD9C
		private static PlatformFilePath PermaMuteFilePath
		{
			get
			{
				PlatformDirectoryPath platformDirectoryPath = new PlatformDirectoryPath(PlatformFileType.User, "Data");
				return new PlatformFilePath(platformDirectoryPath, "Muted.json");
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0000CBC4 File Offset: 0x0000ADC4
		[TupleElementNames(new string[] { "Id", "Name" })]
		public static IReadOnlyList<ValueTuple<string, string>> MutedPlayers
		{
			[return: TupleElementNames(new string[] { "Id", "Name" })]
			get
			{
				List<ValueTuple<string, string>> list;
				if (!PermaMuteList.HasMutedPlayersLoaded || !PermaMuteList._mutedPlayers.TryGetValue(PermaMuteList.CurrentPlayerId, out list))
				{
					return new List<ValueTuple<string, string>>();
				}
				return list;
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0000CBFE File Offset: 0x0000ADFE
		public static void SetPermanentMuteAvailableCallback(Func<bool> getPermanentMuteAvailable)
		{
			PermaMuteList._getPermanentMuteAvailable = getPermanentMuteAvailable;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0000CC08 File Offset: 0x0000AE08
		public static async Task LoadMutedPlayers(PlayerId currentPlayerId)
		{
			PermaMuteList.CurrentPlayerId = currentPlayerId.ToString();
			if (FileHelper.FileExists(PermaMuteList.PermaMuteFilePath))
			{
				try
				{
					Dictionary<string, List<ValueTuple<string, string>>> dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<ValueTuple<string, string>>>>(await FileHelper.GetFileContentStringAsync(PermaMuteList.PermaMuteFilePath));
					if (dictionary == null)
					{
						Debug.Print("PermaMuteList: file deserialized to null (corrupted). Deleting.", 0, Debug.DebugColor.White, 17592186044416UL);
						try
						{
							FileHelper.DeleteFile(PermaMuteList.PermaMuteFilePath);
							goto IL_00ED;
						}
						catch (Exception ex)
						{
							Debug.Print(string.Format("PermaMuteList: could not delete corrupted file. {0}", ex.Message), 0, Debug.DebugColor.White, 17592186044416UL);
							goto IL_00ED;
						}
					}
					PermaMuteList._mutedPlayers = dictionary;
					PermaMuteList.HasMutedPlayersLoaded = true;
					IL_00ED:;
				}
				catch (Exception ex2)
				{
					Debug.Print(string.Format("PermaMuteList: could not load muted players. {0}", ex2.Message), 0, Debug.DebugColor.White, 17592186044416UL);
				}
			}
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0000CC50 File Offset: 0x0000AE50
		public static async void SaveMutedPlayers()
		{
			try
			{
				byte[] array = Common.SerializeObjectAsJson(PermaMuteList._mutedPlayers);
				await FileHelper.SaveFileAsync(PermaMuteList.PermaMuteFilePath, array);
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Could not save muted players. " + ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\PermaMuteList.cs", "SaveMutedPlayers", 84);
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0000CC84 File Offset: 0x0000AE84
		public static bool IsPlayerMuted(PlayerId player)
		{
			Func<bool> getPermanentMuteAvailable = PermaMuteList._getPermanentMuteAvailable;
			if ((getPermanentMuteAvailable == null || getPermanentMuteAvailable()) && PermaMuteList.CurrentPlayerId != null)
			{
				string text = player.ToString();
				Dictionary<string, List<ValueTuple<string, string>>> mutedPlayers = PermaMuteList._mutedPlayers;
				lock (mutedPlayers)
				{
					List<ValueTuple<string, string>> list;
					if (!PermaMuteList._mutedPlayers.TryGetValue(PermaMuteList.CurrentPlayerId, out list))
					{
						return false;
					}
					using (List<ValueTuple<string, string>>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.Item1 == text)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0000CD4C File Offset: 0x0000AF4C
		public static void MutePlayer(PlayerId player, string name)
		{
			Func<bool> getPermanentMuteAvailable = PermaMuteList._getPermanentMuteAvailable;
			if (getPermanentMuteAvailable == null || getPermanentMuteAvailable())
			{
				Dictionary<string, List<ValueTuple<string, string>>> mutedPlayers = PermaMuteList._mutedPlayers;
				lock (mutedPlayers)
				{
					List<ValueTuple<string, string>> list;
					if (!PermaMuteList._mutedPlayers.TryGetValue(PermaMuteList.CurrentPlayerId, out list))
					{
						list = new List<ValueTuple<string, string>>();
						PermaMuteList._mutedPlayers.Add(PermaMuteList.CurrentPlayerId, list);
					}
					list.Add(new ValueTuple<string, string>(player.ToString(), name));
				}
			}
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0000CDDC File Offset: 0x0000AFDC
		public static void RemoveMutedPlayer(PlayerId player)
		{
			Func<bool> getPermanentMuteAvailable = PermaMuteList._getPermanentMuteAvailable;
			if (getPermanentMuteAvailable == null || getPermanentMuteAvailable())
			{
				string text = player.ToString();
				Dictionary<string, List<ValueTuple<string, string>>> mutedPlayers = PermaMuteList._mutedPlayers;
				lock (mutedPlayers)
				{
					List<ValueTuple<string, string>> list;
					if (PermaMuteList._mutedPlayers.TryGetValue(PermaMuteList.CurrentPlayerId, out list))
					{
						int num = -1;
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i].Item1 == text)
							{
								num = i;
								break;
							}
						}
						if (num >= 0)
						{
							list.RemoveAt(num);
						}
					}
				}
			}
		}

		// Token: 0x040003C9 RID: 969
		[TupleElementNames(new string[] { "Id", "Name" })]
		private static Dictionary<string, List<ValueTuple<string, string>>> _mutedPlayers = new Dictionary<string, List<ValueTuple<string, string>>>();

		// Token: 0x040003CA RID: 970
		private static string CurrentPlayerId;

		// Token: 0x040003CB RID: 971
		private static Func<bool> _getPermanentMuteAvailable;
	}
}
