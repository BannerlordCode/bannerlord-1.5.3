using System;
using TaleWorlds.Core.ImageIdentifiers;

namespace TaleWorlds.Core.ViewModelCollection.ImageIdentifiers
{
	// Token: 0x0200001F RID: 31
	public class CraftingPieceImageIdentifierVM : ImageIdentifierVM
	{
		// Token: 0x0600019F RID: 415 RVA: 0x0000599A File Offset: 0x00003B9A
		public CraftingPieceImageIdentifierVM(CraftingPiece craftingPiece, string pieceUsageId)
		{
			base.ImageIdentifier = new CraftingPieceImageIdentifier(craftingPiece, pieceUsageId);
		}
	}
}
