using System;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x02000100 RID: 256
	public class PawnSeega : PawnBase
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x0005ED20 File Offset: 0x0005CF20
		public override bool IsPlaced
		{
			get
			{
				return this.X >= 0 && this.X < BoardGameSeega.BoardWidth && this.Y >= 0 && this.Y < BoardGameSeega.BoardHeight;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x0005ED50 File Offset: 0x0005CF50
		// (set) Token: 0x06000CE0 RID: 3296 RVA: 0x0005ED58 File Offset: 0x0005CF58
		public bool MovedThisTurn { get; private set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x0005ED61 File Offset: 0x0005CF61
		// (set) Token: 0x06000CE2 RID: 3298 RVA: 0x0005ED69 File Offset: 0x0005CF69
		public int PrevX
		{
			get
			{
				return this._prevX;
			}
			set
			{
				this._prevX = value;
				if (value >= 0)
				{
					this.MovedThisTurn = true;
					return;
				}
				this.MovedThisTurn = false;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x0005ED85 File Offset: 0x0005CF85
		// (set) Token: 0x06000CE4 RID: 3300 RVA: 0x0005ED8D File Offset: 0x0005CF8D
		public int PrevY
		{
			get
			{
				return this._prevY;
			}
			set
			{
				this._prevY = value;
				if (value >= 0)
				{
					this.MovedThisTurn = true;
					return;
				}
				this.MovedThisTurn = false;
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0005EDA9 File Offset: 0x0005CFA9
		public PawnSeega(GameEntity entity, bool playerOne)
			: base(entity, playerOne)
		{
			this.X = -1;
			this.Y = -1;
			this.PrevX = -1;
			this.PrevY = -1;
			this.MovedThisTurn = false;
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0005EDD6 File Offset: 0x0005CFD6
		public override void Reset()
		{
			base.Reset();
			this.X = -1;
			this.Y = -1;
			this.PrevX = -1;
			this.PrevY = -1;
			this.MovedThisTurn = false;
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0005EE01 File Offset: 0x0005D001
		public void UpdateMoveBackAvailable()
		{
			if (this.MovedThisTurn)
			{
				this.MovedThisTurn = false;
				return;
			}
			this.PrevX = -1;
			this.PrevY = -1;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0005EE21 File Offset: 0x0005D021
		public void AISetMovedThisTurn(bool moved)
		{
			this.MovedThisTurn = moved;
		}

		// Token: 0x04000590 RID: 1424
		public int X;

		// Token: 0x04000591 RID: 1425
		public int Y;

		// Token: 0x04000592 RID: 1426
		private int _prevX;

		// Token: 0x04000593 RID: 1427
		private int _prevY;
	}
}
