using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200011A RID: 282
	[Serializable]
	public class FriendInfo
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00008106 File Offset: 0x00006306
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x0000810E File Offset: 0x0000630E
		public PlayerId Id { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00008117 File Offset: 0x00006317
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x0000811F File Offset: 0x0000631F
		public FriendStatus Status { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x00008128 File Offset: 0x00006328
		// (set) Token: 0x06000637 RID: 1591 RVA: 0x00008130 File Offset: 0x00006330
		public string Name { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x00008139 File Offset: 0x00006339
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x00008141 File Offset: 0x00006341
		public bool IsOnline { get; set; }
	}
}
