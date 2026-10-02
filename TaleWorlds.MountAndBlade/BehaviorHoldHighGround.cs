using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200011F RID: 287
	public class BehaviorHoldHighGround : BehaviorComponent
	{
		// Token: 0x06000E67 RID: 3687 RVA: 0x0002033E File Offset: 0x0001E53E
		public BehaviorHoldHighGround(Formation formation)
			: base(formation)
		{
			this._isAllowedToChangePosition = true;
			this.RangedAllyFormation = null;
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0002035C File Offset: 0x0001E55C
		protected override void CalculateCurrentOrder()
		{
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			WorldPosition worldPosition;
			Vec2 vec;
			if (cachedClosestEnemyFormation != null)
			{
				worldPosition = base.Formation.CachedMedianPosition;
				if (base.Formation.AI.ActiveBehavior != this)
				{
					this._isAllowedToChangePosition = true;
				}
				else
				{
					float num = Math.Max((this.RangedAllyFormation != null) ? (this.RangedAllyFormation.QuerySystem.MissileRangeAdjusted * 0.8f) : 0f, 30f);
					this._isAllowedToChangePosition = base.Formation.CachedAveragePosition.DistanceSquared(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) > num * num;
				}
				if (this._isAllowedToChangePosition)
				{
					worldPosition.SetVec2(base.Formation.QuerySystem.HighGroundCloseToForeseenBattleGround);
					this._lastChosenPosition = worldPosition;
				}
				else
				{
					worldPosition = this._lastChosenPosition;
				}
				vec = ((base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.HighGroundCloseToForeseenBattleGround) > 25f) ? (base.Formation.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - worldPosition.AsVec2).Normalized() : ((base.Formation.Direction.DotProduct((cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized()) < 0.5f) ? (cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) : base.Formation.Direction).Normalized());
			}
			else
			{
				vec = base.Formation.Direction;
				worldPosition = base.Formation.CachedMedianPosition;
				worldPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0002055F File Offset: 0x0001E75F
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0002058C File Offset: 0x0001E78C
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x000205F2 File Offset: 0x0001E7F2
		protected override float GetAiWeight()
		{
			if (base.Formation.CachedClosestEnemyFormation == null)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x04000370 RID: 880
		public Formation RangedAllyFormation;

		// Token: 0x04000371 RID: 881
		private bool _isAllowedToChangePosition;

		// Token: 0x04000372 RID: 882
		private WorldPosition _lastChosenPosition;
	}
}
