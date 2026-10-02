using System;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x020000FD RID: 253
	public class PawnKonane : PawnBase
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0005E883 File Offset: 0x0005CA83
		public override bool IsPlaced
		{
			get
			{
				return this.X >= 0 && this.X < BoardGameKonane.BoardWidth && this.Y >= 0 && this.Y < BoardGameKonane.BoardHeight;
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0005E8B3 File Offset: 0x0005CAB3
		public PawnKonane(GameEntity entity, bool playerOne)
			: base(entity, playerOne)
		{
			this.X = -1;
			this.Y = -1;
			this.PrevX = -1;
			this.PrevY = -1;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0005E8D9 File Offset: 0x0005CAD9
		public override void Reset()
		{
			base.Reset();
			this.X = -1;
			this.Y = -1;
			this.PrevX = -1;
			this.PrevY = -1;
		}

		// Token: 0x04000583 RID: 1411
		public int X;

		// Token: 0x04000584 RID: 1412
		public int Y;

		// Token: 0x04000585 RID: 1413
		public int PrevX;

		// Token: 0x04000586 RID: 1414
		public int PrevY;
	}
}
