using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F7 RID: 247
	[Serializable]
	public class BattlePlayerStatsDuel : BattlePlayerStatsBase
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x000058A4 File Offset: 0x00003AA4
		public BattlePlayerStatsDuel()
		{
			base.GameType = "Duel";
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x000058B7 File Offset: 0x00003AB7
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x000058BF File Offset: 0x00003ABF
		public int DuelsWon { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x000058C8 File Offset: 0x00003AC8
		// (set) Token: 0x060004F5 RID: 1269 RVA: 0x000058D0 File Offset: 0x00003AD0
		public int InfantryWins { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x000058D9 File Offset: 0x00003AD9
		// (set) Token: 0x060004F7 RID: 1271 RVA: 0x000058E1 File Offset: 0x00003AE1
		public int ArcherWins { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x000058EA File Offset: 0x00003AEA
		// (set) Token: 0x060004F9 RID: 1273 RVA: 0x000058F2 File Offset: 0x00003AF2
		public int CavalryWins { get; set; }
	}
}
