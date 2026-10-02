using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F6 RID: 246
	[Serializable]
	public class BattlePlayerStatsCaptain : BattlePlayerStatsBase
	{
		// Token: 0x060004EA RID: 1258 RVA: 0x0000585E File Offset: 0x00003A5E
		public BattlePlayerStatsCaptain()
		{
			base.GameType = "Captain";
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00005871 File Offset: 0x00003A71
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x00005879 File Offset: 0x00003A79
		public int CaptainsKilled { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x00005882 File Offset: 0x00003A82
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x0000588A File Offset: 0x00003A8A
		public int MVPs { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00005893 File Offset: 0x00003A93
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x0000589B File Offset: 0x00003A9B
		public int Score { get; set; }
	}
}
