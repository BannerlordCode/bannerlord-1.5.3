using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000137 RID: 311
	[Serializable]
	public class MatchmakingWaitTimeStats
	{
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x0000C721 File Offset: 0x0000A921
		// (set) Token: 0x0600088A RID: 2186 RVA: 0x0000C728 File Offset: 0x0000A928
		public static MatchmakingWaitTimeStats Empty { get; private set; } = new MatchmakingWaitTimeStats();

		// Token: 0x0600088C RID: 2188 RVA: 0x0000C73C File Offset: 0x0000A93C
		public MatchmakingWaitTimeStats()
		{
			this._regionStats = new List<MatchmakingWaitTimeRegionStats>();
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0000C74F File Offset: 0x0000A94F
		public void AddRegionStats(MatchmakingWaitTimeRegionStats regionStats)
		{
			this._regionStats.Add(regionStats);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0000C760 File Offset: 0x0000A960
		public MatchmakingWaitTimeRegionStats GetRegionStats(string region)
		{
			foreach (MatchmakingWaitTimeRegionStats matchmakingWaitTimeRegionStats in this._regionStats)
			{
				if (matchmakingWaitTimeRegionStats.Region.ToLower() == region.ToLower())
				{
					return matchmakingWaitTimeRegionStats;
				}
			}
			return null;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0000C7CC File Offset: 0x0000A9CC
		public int GetWaitTime(string region, string gameType, WaitTimeStatType statType)
		{
			int num = 0;
			if (!string.IsNullOrEmpty(region) && !string.IsNullOrEmpty(gameType))
			{
				MatchmakingWaitTimeRegionStats regionStats = this.GetRegionStats(region);
				if (regionStats != null)
				{
					num = regionStats.GetWaitTime(gameType, statType);
				}
			}
			return num;
		}

		// Token: 0x0400038D RID: 909
		private List<MatchmakingWaitTimeRegionStats> _regionStats;
	}
}
