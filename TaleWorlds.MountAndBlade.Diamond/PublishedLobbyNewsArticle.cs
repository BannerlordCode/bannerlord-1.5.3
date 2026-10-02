using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000156 RID: 342
	public class PublishedLobbyNewsArticle
	{
		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0000E1F8 File Offset: 0x0000C3F8
		// (set) Token: 0x060009A6 RID: 2470 RVA: 0x0000E200 File Offset: 0x0000C400
		public string Title { get; set; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0000E209 File Offset: 0x0000C409
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x0000E211 File Offset: 0x0000C411
		public int Type { get; set; }

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0000E21A File Offset: 0x0000C41A
		// (set) Token: 0x060009AA RID: 2474 RVA: 0x0000E222 File Offset: 0x0000C422
		public string Description { get; set; }

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0000E22B File Offset: 0x0000C42B
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x0000E233 File Offset: 0x0000C433
		public string DateStart { get; set; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0000E23C File Offset: 0x0000C43C
		// (set) Token: 0x060009AE RID: 2478 RVA: 0x0000E244 File Offset: 0x0000C444
		public string DateEnd { get; set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x0000E24D File Offset: 0x0000C44D
		// (set) Token: 0x060009B0 RID: 2480 RVA: 0x0000E255 File Offset: 0x0000C455
		public bool Pinned { get; set; }
	}
}
