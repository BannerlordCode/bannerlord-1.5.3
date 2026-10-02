using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000093 RID: 147
	public struct ShipSlotAndPieceName
	{
		// Token: 0x060008BA RID: 2234 RVA: 0x0001D14D File Offset: 0x0001B34D
		public ShipSlotAndPieceName(string slotName, string pieceName)
		{
			this.SlotName = slotName;
			this.PieceName = pieceName;
		}

		// Token: 0x0400046B RID: 1131
		public string SlotName;

		// Token: 0x0400046C RID: 1132
		public string PieceName;
	}
}
