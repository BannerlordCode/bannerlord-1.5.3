using System;
using SandBox.BoardGames.Objects;
using SandBox.BoardGames.Pawns;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.BoardGames.Tiles
{
	// Token: 0x020000F8 RID: 248
	public abstract class TileBase
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0005DE21 File Offset: 0x0005C021
		public GameEntity Entity { get; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0005DE29 File Offset: 0x0005C029
		public BoardGameDecal ValidMoveDecal { get; }

		// Token: 0x06000C87 RID: 3207 RVA: 0x0005DE31 File Offset: 0x0005C031
		protected TileBase(GameEntity entity, BoardGameDecal decal)
		{
			this.Entity = entity;
			this.ValidMoveDecal = decal;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0005DE47 File Offset: 0x0005C047
		public virtual void Reset()
		{
			this.PawnOnTile = null;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0005DE50 File Offset: 0x0005C050
		public void Tick(float dt)
		{
			int num = (this._showTile ? 1 : (-1));
			this._tileFadeTimer += (float)num * dt * 5f;
			this._tileFadeTimer = MBMath.ClampFloat(this._tileFadeTimer, 0f, 1f);
			this.ValidMoveDecal.SetAlpha(this._tileFadeTimer);
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0005DEAD File Offset: 0x0005C0AD
		public void SetVisibility(bool isVisible)
		{
			this._showTile = isVisible;
		}

		// Token: 0x0400055B RID: 1371
		public PawnBase PawnOnTile;

		// Token: 0x0400055C RID: 1372
		private bool _showTile;

		// Token: 0x0400055D RID: 1373
		private float _tileFadeTimer;

		// Token: 0x0400055E RID: 1374
		private const float TileFadeDuration = 0.2f;
	}
}
