using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200004A RID: 74
	public struct PieceData
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x00015A3C File Offset: 0x00013C3C
		// (set) Token: 0x06000638 RID: 1592 RVA: 0x00015A44 File Offset: 0x00013C44
		public CraftingPiece.PieceTypes PieceType { get; private set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x00015A4D File Offset: 0x00013C4D
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x00015A55 File Offset: 0x00013C55
		public int Order { get; private set; }

		// Token: 0x0600063B RID: 1595 RVA: 0x00015A5E File Offset: 0x00013C5E
		public PieceData(CraftingPiece.PieceTypes pieceType, int order)
		{
			this.PieceType = pieceType;
			this.Order = order;
		}
	}
}
