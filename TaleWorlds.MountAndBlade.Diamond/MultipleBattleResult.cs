using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000FC RID: 252
	[Serializable]
	public class MultipleBattleResult
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00005DC5 File Offset: 0x00003FC5
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x00005DCD File Offset: 0x00003FCD
		public List<BattleResult> BattleResults { get; set; }

		// Token: 0x06000521 RID: 1313 RVA: 0x00005DD6 File Offset: 0x00003FD6
		public MultipleBattleResult()
		{
			this.BattleResults = new List<BattleResult>();
			this._currentBattleIndex = -1;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00005DF0 File Offset: 0x00003FF0
		public void CreateNewBattleResult(string gameType)
		{
			BattleResult battleResult = new BattleResult();
			this.BattleResults.Add(battleResult);
			this._currentBattleIndex++;
			if (this._currentBattleIndex > 0)
			{
				foreach (KeyValuePair<string, BattlePlayerEntry> keyValuePair in this.BattleResults[this._currentBattleIndex - 1].PlayerEntries)
				{
					battleResult.AddOrUpdatePlayerEntry(PlayerId.FromString(keyValuePair.Key), keyValuePair.Value.TeamNo, gameType, Guid.Empty, -1);
				}
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00005E9C File Offset: 0x0000409C
		public BattleResult GetCurrentBattleResult()
		{
			return this.BattleResults[this._currentBattleIndex];
		}

		// Token: 0x040001BA RID: 442
		private int _currentBattleIndex;
	}
}
