using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000158 RID: 344
	public class RecentPlayerInfo
	{
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x0000E7F0 File Offset: 0x0000C9F0
		// (set) Token: 0x060009C2 RID: 2498 RVA: 0x0000E7F8 File Offset: 0x0000C9F8
		public string PlayerId { get; set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x0000E801 File Offset: 0x0000CA01
		// (set) Token: 0x060009C4 RID: 2500 RVA: 0x0000E809 File Offset: 0x0000CA09
		public string PlayerName { get; set; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x0000E812 File Offset: 0x0000CA12
		// (set) Token: 0x060009C6 RID: 2502 RVA: 0x0000E81A File Offset: 0x0000CA1A
		public int ImportanceScore { get; set; }

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0000E823 File Offset: 0x0000CA23
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x0000E82B File Offset: 0x0000CA2B
		public DateTime InteractionTime { get; set; }
	}
}
