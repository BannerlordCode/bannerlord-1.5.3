using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000092 RID: 146
	public struct ShipVisualSlotInfo
	{
		// Token: 0x060008B9 RID: 2233 RVA: 0x0001D13D File Offset: 0x0001B33D
		public ShipVisualSlotInfo(string visualSlotId, string visualPieceId)
		{
			this.VisualSlotTag = visualSlotId;
			this.VisualPieceId = visualPieceId;
		}

		// Token: 0x04000469 RID: 1129
		public string VisualSlotTag;

		// Token: 0x0400046A RID: 1130
		public string VisualPieceId;
	}
}
