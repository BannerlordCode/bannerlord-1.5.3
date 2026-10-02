using System;
using SandBox.BoardGames.Pawns;

namespace SandBox.BoardGames
{
	// Token: 0x020000ED RID: 237
	public struct TileBaseInformation
	{
		// Token: 0x06000B84 RID: 2948 RVA: 0x000559D7 File Offset: 0x00053BD7
		public TileBaseInformation(ref PawnBase pawnOnTile)
		{
			this.PawnOnTile = pawnOnTile;
		}

		// Token: 0x040004F9 RID: 1273
		public PawnBase PawnOnTile;
	}
}
