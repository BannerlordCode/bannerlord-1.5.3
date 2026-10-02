using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000135 RID: 309
	[Serializable]
	public class MatchmakingQueueRegionStats
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x0000C4A1 File Offset: 0x0000A6A1
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x0000C4A9 File Offset: 0x0000A6A9
		[JsonProperty]
		public string Region { get; set; }

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x0000C4B4 File Offset: 0x0000A6B4
		[JsonIgnore]
		public int TotalCount
		{
			get
			{
				int num = 0;
				foreach (MatchmakingQueueGameTypeStats matchmakingQueueGameTypeStats in this.GameTypeStats)
				{
					num += matchmakingQueueGameTypeStats.Count;
				}
				return num;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0000C50C File Offset: 0x0000A70C
		// (set) Token: 0x06000872 RID: 2162 RVA: 0x0000C514 File Offset: 0x0000A714
		[JsonProperty]
		public int MaxWaitTime { get; set; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x0000C51D File Offset: 0x0000A71D
		// (set) Token: 0x06000874 RID: 2164 RVA: 0x0000C525 File Offset: 0x0000A725
		[JsonProperty]
		public int MinWaitTime { get; set; }

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0000C52E File Offset: 0x0000A72E
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x0000C536 File Offset: 0x0000A736
		[JsonProperty]
		public int MedianWaitTime { get; set; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0000C53F File Offset: 0x0000A73F
		// (set) Token: 0x06000878 RID: 2168 RVA: 0x0000C547 File Offset: 0x0000A747
		[JsonProperty]
		public int AverageWaitTime { get; set; }

		// Token: 0x06000879 RID: 2169 RVA: 0x0000C550 File Offset: 0x0000A750
		public MatchmakingQueueRegionStats(string region)
		{
			this.Region = region;
			this.GameTypeStats = new List<MatchmakingQueueGameTypeStats>();
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0000C56C File Offset: 0x0000A76C
		public MatchmakingQueueGameTypeStats GetQueueCountObjectOf(string[] gameTypes)
		{
			if (gameTypes != null)
			{
				foreach (MatchmakingQueueGameTypeStats matchmakingQueueGameTypeStats in this.GameTypeStats)
				{
					if (matchmakingQueueGameTypeStats.EqualWith(gameTypes))
					{
						return matchmakingQueueGameTypeStats;
					}
				}
			}
			return null;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0000C5CC File Offset: 0x0000A7CC
		public void AddStats(MatchmakingQueueGameTypeStats matchmakingQueueGameTypeStats)
		{
			this.GameTypeStats.Add(matchmakingQueueGameTypeStats);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0000C5DC File Offset: 0x0000A7DC
		public int GetQueueCountOf(string[] gameTypes)
		{
			int num = 0;
			if (gameTypes != null)
			{
				foreach (MatchmakingQueueGameTypeStats matchmakingQueueGameTypeStats in this.GameTypeStats)
				{
					if (matchmakingQueueGameTypeStats.HasAnyGameType(gameTypes))
					{
						num += matchmakingQueueGameTypeStats.Count;
					}
				}
			}
			return num;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0000C640 File Offset: 0x0000A840
		public void SetWaitTimeStats(int averageWaitTime, int maxWaitTime, int minWaitTime, int medianWaitTime)
		{
			this.AverageWaitTime = averageWaitTime;
			this.MaxWaitTime = maxWaitTime;
			this.MinWaitTime = minWaitTime;
			this.MedianWaitTime = medianWaitTime;
		}

		// Token: 0x04000384 RID: 900
		[JsonProperty]
		public List<MatchmakingQueueGameTypeStats> GameTypeStats;
	}
}
