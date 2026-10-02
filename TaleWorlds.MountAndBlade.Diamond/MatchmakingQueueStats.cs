using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000134 RID: 308
	[Serializable]
	public class MatchmakingQueueStats
	{
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x0000C2AD File Offset: 0x0000A4AD
		// (set) Token: 0x06000865 RID: 2149 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
		public static MatchmakingQueueStats Empty { get; private set; } = new MatchmakingQueueStats();

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x0000C2BC File Offset: 0x0000A4BC
		[JsonIgnore]
		public int TotalCount
		{
			get
			{
				int num = 0;
				foreach (MatchmakingQueueRegionStats matchmakingQueueRegionStats in this.RegionStats)
				{
					num += matchmakingQueueRegionStats.TotalCount;
				}
				return num;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x0000C314 File Offset: 0x0000A514
		[JsonIgnore]
		public int AverageWaitTime
		{
			get
			{
				int num = 0;
				int num2 = 0;
				if (this.RegionStats.Count > 0)
				{
					foreach (MatchmakingQueueRegionStats matchmakingQueueRegionStats in this.RegionStats)
					{
						num2 += matchmakingQueueRegionStats.AverageWaitTime;
					}
					num = num2 / this.RegionStats.Count;
				}
				return num;
			}
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0000C398 File Offset: 0x0000A598
		public MatchmakingQueueStats()
		{
			this.RegionStats = new List<MatchmakingQueueRegionStats>();
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0000C3AB File Offset: 0x0000A5AB
		public void AddRegionStats(MatchmakingQueueRegionStats matchmakingQueueRegionStats)
		{
			this.RegionStats.Add(matchmakingQueueRegionStats);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0000C3BC File Offset: 0x0000A5BC
		public MatchmakingQueueRegionStats GetRegionStats(string region)
		{
			foreach (MatchmakingQueueRegionStats matchmakingQueueRegionStats in this.RegionStats)
			{
				if (matchmakingQueueRegionStats.Region.ToLower() == region.ToLower())
				{
					return matchmakingQueueRegionStats;
				}
			}
			return null;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0000C428 File Offset: 0x0000A628
		public int GetQueueCountOf(string region, string[] gameTypes)
		{
			int num = 0;
			if (!string.IsNullOrEmpty(region) && gameTypes != null)
			{
				MatchmakingQueueRegionStats regionStats = this.GetRegionStats(region);
				if (regionStats != null)
				{
					num = regionStats.GetQueueCountOf(gameTypes);
				}
			}
			return num;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0000C458 File Offset: 0x0000A658
		public string[] GetRegionNames()
		{
			string[] array = new string[this.RegionStats.Count];
			for (int i = 0; i < this.RegionStats.Count; i++)
			{
				array[i] = this.RegionStats[i].Region;
			}
			return array;
		}

		// Token: 0x04000382 RID: 898
		[JsonProperty]
		public List<MatchmakingQueueRegionStats> RegionStats;
	}
}
