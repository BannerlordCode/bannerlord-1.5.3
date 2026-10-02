using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x020000FB RID: 251
	public class PawnBaghChal : PawnBase
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0005E03D File Offset: 0x0005C23D
		public override bool IsPlaced
		{
			get
			{
				return this.X >= 0 && this.X < BoardGameBaghChal.BoardWidth && this.Y >= 0 && this.Y < BoardGameBaghChal.BoardHeight;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0005E06D File Offset: 0x0005C26D
		public MatrixFrame InitialFrame { get; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x0005E075 File Offset: 0x0005C275
		public bool IsTiger { get; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0005E07D File Offset: 0x0005C27D
		public bool IsGoat
		{
			get
			{
				return !this.IsTiger;
			}
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0005E088 File Offset: 0x0005C288
		public PawnBaghChal(GameEntity entity, bool playerOne, bool isTiger)
			: base(entity, playerOne)
		{
			this.X = -1;
			this.Y = -1;
			this.PrevX = -1;
			this.PrevY = -1;
			this.IsTiger = isTiger;
			this.InitialFrame = base.Entity.GetFrame();
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0005E0C6 File Offset: 0x0005C2C6
		public override void Reset()
		{
			base.Reset();
			this.X = -1;
			this.Y = -1;
			this.PrevX = -1;
			this.PrevY = -1;
		}

		// Token: 0x04000567 RID: 1383
		public int X;

		// Token: 0x04000568 RID: 1384
		public int Y;

		// Token: 0x04000569 RID: 1385
		public int PrevX;

		// Token: 0x0400056A RID: 1386
		public int PrevY;
	}
}
