using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F9 RID: 249
	[Serializable]
	public class BattlePlayerStatsSkirmish : BattlePlayerStatsBase
	{
		// Token: 0x06000505 RID: 1285 RVA: 0x00005963 File Offset: 0x00003B63
		public BattlePlayerStatsSkirmish()
		{
			base.GameType = "Skirmish";
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00005976 File Offset: 0x00003B76
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x0000597E File Offset: 0x00003B7E
		public int MVPs { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00005987 File Offset: 0x00003B87
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x0000598F File Offset: 0x00003B8F
		public int Score { get; set; }
	}
}
