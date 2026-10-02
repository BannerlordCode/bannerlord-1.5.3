using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000157 RID: 343
	public static class RecentPlayersManager
	{
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0000E268 File Offset: 0x0000C468
		private static PlatformFilePath RecentPlayerFilePath
		{
			get
			{
				PlatformDirectoryPath platformDirectoryPath = new PlatformDirectoryPath(PlatformFileType.User, "Data");
				return new PlatformFilePath(platformDirectoryPath, "RecentPlayers.json");
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x0000E28D File Offset: 0x0000C48D
		public static MBReadOnlyList<RecentPlayerInfo> RecentPlayers
		{
			get
			{
				return RecentPlayersManager._recentPlayers;
			}
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0000E300 File Offset: 0x0000C500
		public static async void Initialize()
		{
			await RecentPlayersManager.LoadRecentPlayers();
			RecentPlayersManager.DecayPlayers();
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0000E334 File Offset: 0x0000C534
		private static async Task LoadRecentPlayers()
		{
			if (RecentPlayersManager.IsRecentPlayersCacheDirty)
			{
				if (Common.PlatformFileHelper.FileExists(RecentPlayersManager.RecentPlayerFilePath))
				{
					try
					{
						TaskAwaiter<string> taskAwaiter = FileHelper.GetFileContentStringAsync(RecentPlayersManager.RecentPlayerFilePath).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
						}
						RecentPlayersManager._recentPlayers = JsonConvert.DeserializeObject<MBList<RecentPlayerInfo>>(taskAwaiter.GetResult());
						if (RecentPlayersManager._recentPlayers == null)
						{
							RecentPlayersManager._recentPlayers = new MBList<RecentPlayerInfo>();
							throw new Exception("_recentPlayers were null.");
						}
					}
					catch (Exception ex)
					{
						Debug.FailedAssert("Could not recent players. " + ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\RecentPlayersManager.cs", "LoadRecentPlayers", 83);
						try
						{
							FileHelper.DeleteFile(RecentPlayersManager.RecentPlayerFilePath);
						}
						catch (Exception ex2)
						{
							Debug.FailedAssert("Could not delete recent players file. " + ex2.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\RecentPlayersManager.cs", "LoadRecentPlayers", 90);
						}
					}
				}
				RecentPlayersManager.IsRecentPlayersCacheDirty = false;
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0000E374 File Offset: 0x0000C574
		public static async Task<MBReadOnlyList<RecentPlayerInfo>> GetRecentPlayerInfos()
		{
			await RecentPlayersManager.LoadRecentPlayers();
			return RecentPlayersManager.RecentPlayers;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0000E3B1 File Offset: 0x0000C5B1
		public static PlayerId[] GetRecentPlayerIds()
		{
			return RecentPlayersManager._recentPlayers.Select<RecentPlayerInfo, PlayerId>((RecentPlayerInfo p) => PlayerId.FromString(p.PlayerId)).ToArray<PlayerId>();
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0000E3E4 File Offset: 0x0000C5E4
		public static void AddOrUpdatePlayerEntry(PlayerId playerId, string playerName, InteractionType interactionType, int forcedIndex)
		{
			if (forcedIndex == -1)
			{
				object lockObject = RecentPlayersManager._lockObject;
				lock (lockObject)
				{
					RecentPlayersManager.InteractionTypeInfo interactionTypeInfo = RecentPlayersManager.InteractionTypeScoreDictionary[interactionType];
					RecentPlayerInfo recentPlayerInfo = RecentPlayersManager.TryGetPlayer(playerId);
					if (recentPlayerInfo != null)
					{
						if (interactionTypeInfo.ProcessType == RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Cumulative)
						{
							recentPlayerInfo.ImportanceScore += interactionTypeInfo.Score;
						}
						else if (interactionTypeInfo.ProcessType == RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Fixed)
						{
							recentPlayerInfo.ImportanceScore += Math.Max(interactionTypeInfo.Score, recentPlayerInfo.ImportanceScore);
						}
						recentPlayerInfo.PlayerName = playerName;
						recentPlayerInfo.InteractionTime = DateTime.Now;
					}
					else
					{
						recentPlayerInfo = new RecentPlayerInfo();
						recentPlayerInfo.PlayerId = playerId.ToString();
						recentPlayerInfo.ImportanceScore = interactionTypeInfo.Score;
						recentPlayerInfo.InteractionTime = DateTime.Now;
						recentPlayerInfo.PlayerName = playerName;
						RecentPlayersManager._recentPlayers.Add(recentPlayerInfo);
					}
					Action<PlayerId, InteractionType> onRecentPlayerInteraction = RecentPlayersManager.OnRecentPlayerInteraction;
					if (onRecentPlayerInteraction != null)
					{
						onRecentPlayerInteraction(playerId, interactionType);
					}
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060009BA RID: 2490 RVA: 0x0000E4E8 File Offset: 0x0000C6E8
		// (remove) Token: 0x060009BB RID: 2491 RVA: 0x0000E51C File Offset: 0x0000C71C
		public static event Action<PlayerId, InteractionType> OnRecentPlayerInteraction;

		// Token: 0x060009BC RID: 2492 RVA: 0x0000E550 File Offset: 0x0000C750
		private static void DecayPlayers()
		{
			object lockObject = RecentPlayersManager._lockObject;
			lock (lockObject)
			{
				List<RecentPlayerInfo> list = new List<RecentPlayerInfo>();
				DateTime now = DateTime.Now;
				foreach (RecentPlayerInfo recentPlayerInfo in RecentPlayersManager._recentPlayers)
				{
					recentPlayerInfo.ImportanceScore -= (int)(now - recentPlayerInfo.InteractionTime).TotalHours;
					if (recentPlayerInfo.ImportanceScore <= 0)
					{
						list.Add(recentPlayerInfo);
					}
				}
				foreach (RecentPlayerInfo recentPlayerInfo2 in list)
				{
					RecentPlayersManager._recentPlayers.Remove(recentPlayerInfo2);
				}
			}
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0000E64C File Offset: 0x0000C84C
		public static void TrimPlayers()
		{
			if (RecentPlayersManager._recentPlayers.Count > 200)
			{
				object lockObject = RecentPlayersManager._lockObject;
				lock (lockObject)
				{
					List<RecentPlayerInfo> list = RecentPlayersManager._recentPlayers.OrderByDescending<RecentPlayerInfo, int>((RecentPlayerInfo p) => p.ImportanceScore).Take<RecentPlayerInfo>(160).ToList<RecentPlayerInfo>();
					RecentPlayersManager._recentPlayers.Clear();
					RecentPlayersManager._recentPlayers.AddRange(list);
				}
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0000E6E4 File Offset: 0x0000C8E4
		public static void Serialize()
		{
			try
			{
				byte[] array = Common.SerializeObjectAsJson(RecentPlayersManager._recentPlayers);
				FileHelper.SaveFile(RecentPlayersManager.RecentPlayerFilePath, array);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0000E724 File Offset: 0x0000C924
		public static IEnumerable<PlayerId> GetPlayersOrdered()
		{
			return from p in RecentPlayersManager._recentPlayers
				orderby p.InteractionTime descending
				select PlayerId.FromString(p.PlayerId);
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0000E780 File Offset: 0x0000C980
		private static RecentPlayerInfo TryGetPlayer(PlayerId playerId)
		{
			string text = playerId.ToString();
			foreach (RecentPlayerInfo recentPlayerInfo in RecentPlayersManager._recentPlayers)
			{
				if (recentPlayerInfo.PlayerId == text)
				{
					return recentPlayerInfo;
				}
			}
			return null;
		}

		// Token: 0x0400042F RID: 1071
		private const string RecentPlayersDirectoryName = "Data";

		// Token: 0x04000430 RID: 1072
		private const string RecentPlayersFileName = "RecentPlayers.json";

		// Token: 0x04000431 RID: 1073
		private const int MaxRecentPlayersSize = 200;

		// Token: 0x04000432 RID: 1074
		private const int DownsizedRecentPlayersSize = 160;

		// Token: 0x04000433 RID: 1075
		private static bool IsRecentPlayersCacheDirty = true;

		// Token: 0x04000434 RID: 1076
		private static readonly object _lockObject = new object();

		// Token: 0x04000435 RID: 1077
		private static MBList<RecentPlayerInfo> _recentPlayers = new MBList<RecentPlayerInfo>();

		// Token: 0x04000436 RID: 1078
		private static readonly Dictionary<InteractionType, RecentPlayersManager.InteractionTypeInfo> InteractionTypeScoreDictionary = new Dictionary<InteractionType, RecentPlayersManager.InteractionTypeInfo>
		{
			{
				InteractionType.Killed,
				new RecentPlayersManager.InteractionTypeInfo(5, RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Cumulative)
			},
			{
				InteractionType.KilledBy,
				new RecentPlayersManager.InteractionTypeInfo(5, RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Cumulative)
			},
			{
				InteractionType.InGameTogether,
				new RecentPlayersManager.InteractionTypeInfo(24, RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Fixed)
			},
			{
				InteractionType.InPartyTogether,
				new RecentPlayersManager.InteractionTypeInfo(48, RecentPlayersManager.InteractionTypeInfo.InteractionProcessType.Fixed)
			}
		};

		// Token: 0x020001DB RID: 475
		private class InteractionTypeInfo
		{
			// Token: 0x17000377 RID: 887
			// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0001793E File Offset: 0x00015B3E
			// (set) Token: 0x06000B92 RID: 2962 RVA: 0x00017946 File Offset: 0x00015B46
			public int Score { get; private set; }

			// Token: 0x17000378 RID: 888
			// (get) Token: 0x06000B93 RID: 2963 RVA: 0x0001794F File Offset: 0x00015B4F
			// (set) Token: 0x06000B94 RID: 2964 RVA: 0x00017957 File Offset: 0x00015B57
			public RecentPlayersManager.InteractionTypeInfo.InteractionProcessType ProcessType { get; private set; }

			// Token: 0x06000B95 RID: 2965 RVA: 0x00017960 File Offset: 0x00015B60
			public InteractionTypeInfo(int score, RecentPlayersManager.InteractionTypeInfo.InteractionProcessType type)
			{
				this.Score = score;
				this.ProcessType = type;
			}

			// Token: 0x020001EA RID: 490
			public enum InteractionProcessType
			{
				// Token: 0x04000762 RID: 1890
				Cumulative,
				// Token: 0x04000763 RID: 1891
				Fixed
			}
		}
	}
}
