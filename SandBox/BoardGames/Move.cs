using System;
using SandBox.BoardGames.Pawns;
using SandBox.BoardGames.Tiles;

namespace SandBox.BoardGames
{
	// Token: 0x020000EE RID: 238
	public struct Move
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x000559E1 File Offset: 0x00053BE1
		public bool IsValid
		{
			get
			{
				return this.Unit != null && this.GoalTile != null;
			}
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x000559F6 File Offset: 0x00053BF6
		public Move(PawnBase unit, TileBase goalTile)
		{
			this.Unit = unit;
			this.GoalTile = goalTile;
		}

		// Token: 0x040004FA RID: 1274
		public static readonly Move Invalid = new Move
		{
			Unit = null,
			GoalTile = null
		};

		// Token: 0x040004FB RID: 1275
		public PawnBase Unit;

		// Token: 0x040004FC RID: 1276
		public TileBase GoalTile;
	}
}
