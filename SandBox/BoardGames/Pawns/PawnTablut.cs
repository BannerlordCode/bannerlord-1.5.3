using System;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x02000101 RID: 257
	public class PawnTablut : PawnBase
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x0005EE2A File Offset: 0x0005D02A
		public override bool IsPlaced
		{
			get
			{
				return this.X >= 0 && this.X < 9 && this.Y >= 0 && this.Y < 9;
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0005EE54 File Offset: 0x0005D054
		public PawnTablut(GameEntity entity, bool playerOne)
			: base(entity, playerOne)
		{
			this.X = -1;
			this.Y = -1;
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0005EE6C File Offset: 0x0005D06C
		public override void Reset()
		{
			base.Reset();
			this.X = -1;
			this.Y = -1;
		}

		// Token: 0x04000595 RID: 1429
		public int X;

		// Token: 0x04000596 RID: 1430
		public int Y;
	}
}
