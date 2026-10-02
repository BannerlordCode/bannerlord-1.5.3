using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000120 RID: 288
	public class BehaviorHorseArcherSkirmish : BehaviorComponent
	{
		// Token: 0x06000E6C RID: 3692 RVA: 0x0002060C File Offset: 0x0001E80C
		public BehaviorHorseArcherSkirmish(Formation formation)
			: base(formation)
		{
			this.CalculateCurrentOrder();
			base.BehaviorCoherence = 0.5f;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0002062D File Offset: 0x0001E82D
		protected override float GetAiWeight()
		{
			if (!this._isEnemyReachable)
			{
				return 0.09f;
			}
			return 0.9f;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00020644 File Offset: 0x0001E844
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x000206AC File Offset: 0x0001E8AC
		protected override void CalculateCurrentOrder()
		{
			WorldPosition worldPosition = base.Formation.CachedMedianPosition;
			this._isEnemyReachable = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && (!(base.Formation.Team.TeamAI is TeamAISiegeComponent) || !TeamAISiegeComponent.IsFormationInsideCastle(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation, false, 0.4f));
			Vec2 cachedAveragePosition = base.Formation.CachedAveragePosition;
			if (!this._isEnemyReachable)
			{
				worldPosition.SetVec2(cachedAveragePosition);
			}
			else
			{
				WorldPosition cachedMedianPosition = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition;
				int num = 0;
				Vec2 vec = Vec2.Zero;
				foreach (Formation formation in base.Formation.Team.FormationsIncludingSpecialAndEmpty)
				{
					if (formation != base.Formation && formation.CountOfUnits > 0)
					{
						num++;
						vec += formation.CachedMedianPosition.AsVec2;
					}
				}
				if (num > 0)
				{
					vec /= (float)num;
				}
				else
				{
					vec = cachedAveragePosition;
				}
				WorldPosition medianTargetFormationPosition = base.Formation.QuerySystem.Team.MedianTargetFormationPosition;
				Vec2 vec2 = (medianTargetFormationPosition.AsVec2 - vec).Normalized();
				float missileRangeAdjusted = base.Formation.QuerySystem.MissileRangeAdjusted;
				if (this._rushMode)
				{
					float num2 = cachedAveragePosition.DistanceSquared(cachedMedianPosition.AsVec2);
					if (num2 > base.Formation.QuerySystem.MissileRangeAdjusted * base.Formation.QuerySystem.MissileRangeAdjusted)
					{
						worldPosition = medianTargetFormationPosition;
						worldPosition.SetVec2(worldPosition.AsVec2 - vec2 * (missileRangeAdjusted - (10f + base.Formation.Depth * 0.5f)));
					}
					else if (base.Formation.CachedClosestEnemyFormation.IsCavalryFormation || num2 <= 400f || base.Formation.QuerySystem.UnderRangedAttackRatio >= 0.4f)
					{
						worldPosition = base.Formation.QuerySystem.Team.MedianPosition;
						worldPosition.SetVec2(vec - (((num > 0) ? 30f : 80f) + base.Formation.Depth) * vec2);
						this._rushMode = false;
					}
					else
					{
						worldPosition = base.Formation.QuerySystem.Team.MedianPosition;
						Vec2 vec3 = (cachedMedianPosition.AsVec2 - cachedAveragePosition).Normalized();
						worldPosition.SetVec2(cachedMedianPosition.AsVec2 - vec3 * (missileRangeAdjusted - (10f + base.Formation.Depth * 0.5f)));
					}
				}
				else
				{
					if (num > 0)
					{
						worldPosition = base.Formation.QuerySystem.Team.MedianPosition;
						worldPosition.SetVec2(vec - (30f + base.Formation.Depth) * vec2);
					}
					else
					{
						worldPosition = base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition;
						worldPosition.SetVec2(worldPosition.AsVec2 - 80f * vec2);
					}
					if (worldPosition.AsVec2.DistanceSquared(cachedAveragePosition) <= 400f)
					{
						worldPosition = medianTargetFormationPosition;
						worldPosition.SetVec2(worldPosition.AsVec2 - vec2 * (missileRangeAdjusted - (10f + base.Formation.Depth * 0.5f)));
						this._rushMode = true;
					}
				}
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00020A78 File Offset: 0x0001EC78
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x04000373 RID: 883
		private bool _rushMode;

		// Token: 0x04000374 RID: 884
		private bool _isEnemyReachable = true;
	}
}
