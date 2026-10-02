using System;
using SandBox.BoardGames.Objects;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Tiles
{
	// Token: 0x020000F9 RID: 249
	public class TileMuTorere : Tile1D
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x0005DEB6 File Offset: 0x0005C0B6
		public int XLeftTile { get; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x0005DEBE File Offset: 0x0005C0BE
		public int XRightTile { get; }

		// Token: 0x06000C8D RID: 3213 RVA: 0x0005DEC6 File Offset: 0x0005C0C6
		public TileMuTorere(GameEntity entity, BoardGameDecal decal, int x, int xLeft, int xRight)
			: base(entity, decal, x)
		{
			this.XLeftTile = xLeft;
			this.XRightTile = xRight;
		}
	}
}
