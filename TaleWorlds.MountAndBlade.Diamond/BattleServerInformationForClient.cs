using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000FE RID: 254
	[Serializable]
	public struct BattleServerInformationForClient
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x00006EDD File Offset: 0x000050DD
		// (set) Token: 0x0600056A RID: 1386 RVA: 0x00006EE5 File Offset: 0x000050E5
		public string MatchId { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x00006EEE File Offset: 0x000050EE
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x00006EF6 File Offset: 0x000050F6
		public string ServerAddress { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x00006EFF File Offset: 0x000050FF
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x00006F07 File Offset: 0x00005107
		public ushort ServerPort { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00006F10 File Offset: 0x00005110
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x00006F18 File Offset: 0x00005118
		public int PeerIndex { get; set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00006F21 File Offset: 0x00005121
		// (set) Token: 0x06000572 RID: 1394 RVA: 0x00006F29 File Offset: 0x00005129
		public int TeamNo { get; set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00006F32 File Offset: 0x00005132
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x00006F3A File Offset: 0x0000513A
		public int SessionKey { get; set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00006F43 File Offset: 0x00005143
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x00006F4B File Offset: 0x0000514B
		public string SceneName { get; set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00006F54 File Offset: 0x00005154
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x00006F5C File Offset: 0x0000515C
		public string GameType { get; set; }
	}
}
