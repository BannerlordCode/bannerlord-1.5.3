using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F8 RID: 248
	[Serializable]
	public class BattlePlayerStatsSiege : BattlePlayerStatsBase
	{
		// Token: 0x060004FA RID: 1274 RVA: 0x000058FB File Offset: 0x00003AFB
		public BattlePlayerStatsSiege()
		{
			base.GameType = "Siege";
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x0000590E File Offset: 0x00003B0E
		// (set) Token: 0x060004FC RID: 1276 RVA: 0x00005916 File Offset: 0x00003B16
		public int WallsBreached { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x0000591F File Offset: 0x00003B1F
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x00005927 File Offset: 0x00003B27
		public int SiegeEngineKills { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00005930 File Offset: 0x00003B30
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x00005938 File Offset: 0x00003B38
		public int SiegeEnginesDestroyed { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00005941 File Offset: 0x00003B41
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x00005949 File Offset: 0x00003B49
		public int ObjectiveGoldGained { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00005952 File Offset: 0x00003B52
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x0000595A File Offset: 0x00003B5A
		public int Score { get; set; }
	}
}
