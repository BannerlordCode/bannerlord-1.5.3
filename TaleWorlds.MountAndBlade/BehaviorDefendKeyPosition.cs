using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000117 RID: 279
	public class BehaviorDefendKeyPosition : BehaviorComponent
	{
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x0001E22F File Offset: 0x0001C42F
		// (set) Token: 0x06000E2A RID: 3626 RVA: 0x0001E23C File Offset: 0x0001C43C
		public WorldPosition DefensePosition
		{
			get
			{
				return this._behaviorPosition.Value;
			}
			set
			{
				this._defensePosition = value;
			}
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0001E248 File Offset: 0x0001C448
		public BehaviorDefendKeyPosition(Formation formation)
			: base(formation)
		{
			this._behaviorPosition = new QueryData<WorldPosition>(() => Mission.Current.FindBestDefendingPosition(this.EnemyClusterPosition, this._defensePosition), 5f);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0001E294 File Offset: 0x0001C494
		protected override void CalculateCurrentOrder()
		{
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			Vec2 vec;
			if (cachedClosestEnemyFormation == null)
			{
				vec = base.Formation.Direction;
			}
			else
			{
				vec = ((base.Formation.Direction.DotProduct((cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized()) < 0.5f) ? (cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) : base.Formation.Direction).Normalized();
			}
			if (this.DefensePosition.IsValid)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(this.DefensePosition);
			}
			else
			{
				WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
				cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			}
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0001E3A0 File Offset: 0x0001C5A0
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.QuerySystem.HasShield && base.Formation.CachedAveragePosition.DistanceSquared(base.CurrentOrder.GetPosition(base.Formation)) < base.Formation.Depth * base.Formation.Depth * 4f)
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderShieldWall);
				return;
			}
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0001E44E File Offset: 0x0001C64E
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0001E481 File Offset: 0x0001C681
		protected override float GetAiWeight()
		{
			return 10f;
		}

		// Token: 0x0400035B RID: 859
		private WorldPosition _defensePosition = WorldPosition.Invalid;

		// Token: 0x0400035C RID: 860
		public WorldPosition EnemyClusterPosition = WorldPosition.Invalid;

		// Token: 0x0400035D RID: 861
		private readonly QueryData<WorldPosition> _behaviorPosition;
	}
}
