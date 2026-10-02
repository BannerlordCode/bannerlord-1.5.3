using System;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200001E RID: 30
	public class ContainerItemDescription
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000C1E5 File Offset: 0x0000A3E5
		// (set) Token: 0x06000253 RID: 595 RVA: 0x0000C1ED File Offset: 0x0000A3ED
		public string WidgetId { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000C1F6 File Offset: 0x0000A3F6
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000C1FE File Offset: 0x0000A3FE
		public int WidgetIndex { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000C207 File Offset: 0x0000A407
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000C20F File Offset: 0x0000A40F
		public float WidthStretchRatio { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000C218 File Offset: 0x0000A418
		// (set) Token: 0x06000259 RID: 601 RVA: 0x0000C220 File Offset: 0x0000A420
		public float HeightStretchRatio { get; set; }

		// Token: 0x0600025A RID: 602 RVA: 0x0000C229 File Offset: 0x0000A429
		public ContainerItemDescription()
		{
			this.WidgetId = "";
			this.WidgetIndex = -1;
			this.WidthStretchRatio = 1f;
			this.HeightStretchRatio = 1f;
		}
	}
}
