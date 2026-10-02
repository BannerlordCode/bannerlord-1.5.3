using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x020000FF RID: 255
	public class PawnPuluc : PawnBase
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0005E931 File Offset: 0x0005CB31
		public float Height
		{
			get
			{
				if (PawnPuluc._height == 0f)
				{
					PawnPuluc._height = (base.Entity.GetBoundingBoxMax() - base.Entity.GetBoundingBoxMin()).z;
				}
				return PawnPuluc._height;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0005E969 File Offset: 0x0005CB69
		public override Vec3 PosBeforeMoving
		{
			get
			{
				return this.PosBeforeMovingBase - new Vec3(0f, 0f, this.Height * (float)this.PawnsBelow.Count, -1f);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0005E99D File Offset: 0x0005CB9D
		public override bool IsPlaced
		{
			get
			{
				return (this.InPlay || this.IsInSpawn) && this.IsTopPawn;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0005E9B7 File Offset: 0x0005CBB7
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x0005E9BF File Offset: 0x0005CBBF
		public int X
		{
			get
			{
				return this._x;
			}
			set
			{
				this._x = value;
				if (value >= 0 && value < 11)
				{
					this.IsInSpawn = false;
					return;
				}
				this.IsInSpawn = true;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x0005E9E0 File Offset: 0x0005CBE0
		public List<PawnPuluc> PawnsBelow { get; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x0005E9E8 File Offset: 0x0005CBE8
		public bool InPlay
		{
			get
			{
				return this.X >= 0 && this.X < 11;
			}
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0005E9FF File Offset: 0x0005CBFF
		public PawnPuluc(GameEntity entity, bool playerOne)
			: base(entity, playerOne)
		{
			this.PawnsBelow = new List<PawnPuluc>();
			this.SpawnPos = base.CurrentPos;
			this.X = -1;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0005EA35 File Offset: 0x0005CC35
		public override void Reset()
		{
			base.Reset();
			this.X = -1;
			this.State = PawnPuluc.MovementState.MovingForward;
			this.IsTopPawn = true;
			this.IsInSpawn = true;
			this.CapturedBy = null;
			this.PawnsBelow.Clear();
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0005EA6C File Offset: 0x0005CC6C
		public override void AddGoalPosition(Vec3 goal)
		{
			if (this.IsTopPawn)
			{
				goal.z += this.Height * (float)this.PawnsBelow.Count;
				int count = this.PawnsBelow.Count;
				for (int i = 0; i < count; i++)
				{
					this.PawnsBelow[i].AddGoalPosition(goal - new Vec3(0f, 0f, (float)(i + 1) * this.Height, -1f));
				}
			}
			base.GoalPositions.Add(goal);
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0005EAFC File Offset: 0x0005CCFC
		public override void MovePawnToGoalPositions(bool instantMove, float speed, bool dragged = false)
		{
			if (base.GoalPositions.Count == 0)
			{
				return;
			}
			base.MovePawnToGoalPositions(instantMove, speed, dragged);
			if (this.IsTopPawn)
			{
				foreach (PawnPuluc pawnPuluc in this.PawnsBelow)
				{
					pawnPuluc.MovePawnToGoalPositions(instantMove, speed, dragged);
				}
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0005EB70 File Offset: 0x0005CD70
		public override void SetPawnAtPosition(Vec3 position)
		{
			base.SetPawnAtPosition(position);
			if (this.IsTopPawn)
			{
				int num = 1;
				foreach (PawnPuluc pawnPuluc in this.PawnsBelow)
				{
					pawnPuluc.SetPawnAtPosition(new Vec3(position.x, position.y, position.z - this.Height * (float)num, -1f));
					num++;
				}
			}
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0005EBFC File Offset: 0x0005CDFC
		public override void EnableCollisionBody()
		{
			base.EnableCollisionBody();
			foreach (PawnPuluc pawnPuluc in this.PawnsBelow)
			{
				pawnPuluc.Entity.BodyFlag &= ~BodyFlags.Disabled;
			}
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0005EC60 File Offset: 0x0005CE60
		public override void DisableCollisionBody()
		{
			base.DisableCollisionBody();
			foreach (PawnPuluc pawnPuluc in this.PawnsBelow)
			{
				pawnPuluc.Entity.BodyFlag |= BodyFlags.Disabled;
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0005ECC4 File Offset: 0x0005CEC4
		public void MovePawnBackToSpawn(bool instantMove, float speed, bool fake = false)
		{
			this.X = -1;
			this.State = PawnPuluc.MovementState.MovingForward;
			this.IsTopPawn = true;
			this.IsInSpawn = true;
			base.Captured = false;
			this.CapturedBy = null;
			this.PawnsBelow.Clear();
			if (!fake)
			{
				this.AddGoalPosition(this.SpawnPos);
				this.MovePawnToGoalPositions(instantMove, speed, false);
			}
		}

		// Token: 0x04000588 RID: 1416
		public PawnPuluc.MovementState State;

		// Token: 0x04000589 RID: 1417
		public PawnPuluc CapturedBy;

		// Token: 0x0400058A RID: 1418
		public Vec3 SpawnPos;

		// Token: 0x0400058B RID: 1419
		public bool IsInSpawn = true;

		// Token: 0x0400058C RID: 1420
		public bool IsTopPawn = true;

		// Token: 0x0400058D RID: 1421
		private static float _height;

		// Token: 0x0400058E RID: 1422
		private int _x;

		// Token: 0x02000236 RID: 566
		public enum MovementState
		{
			// Token: 0x040009DC RID: 2524
			MovingForward,
			// Token: 0x040009DD RID: 2525
			MovingBackward,
			// Token: 0x040009DE RID: 2526
			ChangingDirection
		}
	}
}
