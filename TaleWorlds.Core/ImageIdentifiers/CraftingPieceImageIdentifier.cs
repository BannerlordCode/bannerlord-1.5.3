using System;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E6 RID: 230
	public class CraftingPieceImageIdentifier : ImageIdentifier
	{
		// Token: 0x06000B95 RID: 2965 RVA: 0x00025751 File Offset: 0x00023951
		public CraftingPieceImageIdentifier(CraftingPiece craftingPiece, string pieceUsageId)
		{
			base.Id = ((craftingPiece != null) ? (craftingPiece.StringId + "$" + pieceUsageId) : "");
			base.AdditionalArgs = "";
			base.TextureProviderName = "CraftingPieceImageTextureProvider";
		}
	}
}
