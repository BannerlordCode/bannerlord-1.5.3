using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F5 RID: 245
	[Serializable]
	public class BattlePlayerStatsBattle : BattlePlayerStatsBase
	{
		// Token: 0x060004E5 RID: 1253 RVA: 0x00005829 File Offset: 0x00003A29
		public BattlePlayerStatsBattle()
		{
			base.GameType = "Battle";
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x0000583C File Offset: 0x00003A3C
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x00005844 File Offset: 0x00003A44
		public int RoundsWon { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x0000584D File Offset: 0x00003A4D
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x00005855 File Offset: 0x00003A55
		public int RoundsLost { get; set; }
	}
}
