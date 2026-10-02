using System;
using SandBox.BoardGames.Objects;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Tiles
{
	// Token: 0x020000F6 RID: 246
	public class Tile1D : TileBase
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0005DDDF File Offset: 0x0005BFDF
		public int X { get; }

		// Token: 0x06000C81 RID: 3201 RVA: 0x0005DDE7 File Offset: 0x0005BFE7
		public Tile1D(GameEntity entity, BoardGameDecal decal, int x)
			: base(entity, decal)
		{
			this.X = x;
		}
	}
}
