using System;
using SandBox.BoardGames.Objects;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Tiles
{
	// Token: 0x020000F7 RID: 247
	public class Tile2D : TileBase
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x0005DDF8 File Offset: 0x0005BFF8
		public int X { get; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0005DE00 File Offset: 0x0005C000
		public int Y { get; }

		// Token: 0x06000C84 RID: 3204 RVA: 0x0005DE08 File Offset: 0x0005C008
		public Tile2D(GameEntity entity, BoardGameDecal decal, int x, int y)
			: base(entity, decal)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
