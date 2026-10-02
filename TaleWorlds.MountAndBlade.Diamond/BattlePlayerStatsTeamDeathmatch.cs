using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000FA RID: 250
	[Serializable]
	public class BattlePlayerStatsTeamDeathmatch : BattlePlayerStatsBase
	{
		// Token: 0x0600050A RID: 1290 RVA: 0x00005998 File Offset: 0x00003B98
		public BattlePlayerStatsTeamDeathmatch()
		{
			base.GameType = "TeamDeathmatch";
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x000059AB File Offset: 0x00003BAB
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x000059B3 File Offset: 0x00003BB3
		public int Score { get; set; }
	}
}
