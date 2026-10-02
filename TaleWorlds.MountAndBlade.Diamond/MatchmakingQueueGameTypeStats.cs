using System;
using System.Linq;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000136 RID: 310
	[Serializable]
	public class MatchmakingQueueGameTypeStats
	{
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0000C65F File Offset: 0x0000A85F
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x0000C667 File Offset: 0x0000A867
		[JsonProperty]
		public string[] GameTypes { get; set; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x0000C670 File Offset: 0x0000A870
		// (set) Token: 0x06000881 RID: 2177 RVA: 0x0000C678 File Offset: 0x0000A878
		[JsonProperty]
		public int Count { get; set; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x0000C681 File Offset: 0x0000A881
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x0000C689 File Offset: 0x0000A889
		[JsonProperty]
		public int TotalWaitTime { get; set; }

		// Token: 0x06000884 RID: 2180 RVA: 0x0000C692 File Offset: 0x0000A892
		public MatchmakingQueueGameTypeStats()
		{
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0000C69A File Offset: 0x0000A89A
		public MatchmakingQueueGameTypeStats(string[] gameTypes)
		{
			this.GameTypes = gameTypes;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0000C6A9 File Offset: 0x0000A8A9
		public bool HasGameType(string gameType)
		{
			return this.GameTypes.Contains(gameType);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0000C6B8 File Offset: 0x0000A8B8
		public bool EqualWith(string[] gameTypes)
		{
			if (this.GameTypes.Length == gameTypes.Length)
			{
				foreach (string text in gameTypes)
				{
					if (!this.HasGameType(text))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
		internal bool HasAnyGameType(string[] gameTypes)
		{
			foreach (string text in gameTypes)
			{
				if (this.HasGameType(text))
				{
					return true;
				}
			}
			return false;
		}
	}
}
