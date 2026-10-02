using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000139 RID: 313
	[Serializable]
	public class MatchmakingWaitTimeRegionStats
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0000C800 File Offset: 0x0000AA00
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x0000C808 File Offset: 0x0000AA08
		public string Region { get; private set; }

		// Token: 0x06000892 RID: 2194 RVA: 0x0000C811 File Offset: 0x0000AA11
		public MatchmakingWaitTimeRegionStats(string region)
		{
			this.Region = region;
			this._gameTypeAverageWaitTimes = new Dictionary<string, Dictionary<WaitTimeStatType, int>>();
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0000C82C File Offset: 0x0000AA2C
		public void SetGameTypeAverage(string gameType, WaitTimeStatType statType, int average)
		{
			Dictionary<WaitTimeStatType, int> dictionary;
			if (!this._gameTypeAverageWaitTimes.TryGetValue(gameType, out dictionary))
			{
				dictionary = new Dictionary<WaitTimeStatType, int>();
				this._gameTypeAverageWaitTimes.Add(gameType, dictionary);
			}
			this._gameTypeAverageWaitTimes[gameType][statType] = average;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0000C86F File Offset: 0x0000AA6F
		public bool HasStatsForGameType(string gameType)
		{
			return gameType != null && this._gameTypeAverageWaitTimes.ContainsKey(gameType);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0000C884 File Offset: 0x0000AA84
		public int GetWaitTime(string gameType, WaitTimeStatType statType)
		{
			Dictionary<WaitTimeStatType, int> dictionary;
			int num;
			if (this._gameTypeAverageWaitTimes.TryGetValue(gameType, out dictionary) && dictionary.TryGetValue(statType, out num))
			{
				return num;
			}
			return int.MaxValue;
		}

		// Token: 0x04000393 RID: 915
		private Dictionary<string, Dictionary<WaitTimeStatType, int>> _gameTypeAverageWaitTimes;
	}
}
