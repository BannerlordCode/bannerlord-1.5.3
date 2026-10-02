using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000115 RID: 277
	public class BehaviorDefend : BehaviorComponent
	{
		// Token: 0x06000E16 RID: 3606 RVA: 0x0001D46D File Offset: 0x0001B66D
		public BehaviorDefend(Formation formation)
			: base(formation)
		{
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0001D488 File Offset: 0x0001B688
		protected override void CalculateCurrentOrder()
		{
			Vec2 vec;
			if (this.TacticalDefendPosition != null)
			{
				vec = ((!this.TacticalDefendPosition.IsInsurmountable) ? this.TacticalDefendPosition.Direction : (base.Formation.Team.QuerySystem.AverageEnemyPosition - this.TacticalDefendPosition.Position.AsVec2).Normalized());
			}
			else if (base.Formation.CachedClosestEnemyFormation == null)
			{
				vec = base.Formation.Direction;
			}
			else
			{
				vec = ((base.Formation.Direction.DotProduct((base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized()) < 0.5f) ? (base.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) : base.Formation.Direction).Normalized();
			}
			if (this.TacticalDefendPosition != null)
			{
				if (!this.TacticalDefendPosition.IsInsurmountable)
				{
					base.CurrentOrder = MovementOrder.MovementOrderMove(this.TacticalDefendPosition.Position);
				}
				else
				{
					Vec2 vec2 = this.TacticalDefendPosition.Position.AsVec2 + this.TacticalDefendPosition.Width * 0.5f * vec;
					WorldPosition position = this.TacticalDefendPosition.Position;
					position.SetVec2(vec2);
					base.CurrentOrder = MovementOrder.MovementOrderMove(position);
				}
				this.CurrentFacingOrder = ((!this.TacticalDefendPosition.IsInsurmountable) ? FacingOrder.FacingOrderLookAtDirection(vec) : FacingOrder.FacingOrderLookAtEnemy);
				return;
			}
			if (this.DefensePosition.IsValid)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(this.DefensePosition);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
				return;
			}
			WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
			cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
			base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0001D6AC File Offset: 0x0001B8AC
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.CachedAveragePosition.DistanceSquared(base.CurrentOrder.GetPosition(base.Formation)) < 100f)
			{
				if (base.Formation.QuerySystem.HasShield)
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderShieldWall);
				}
				else if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2) > 100f && base.Formation.QuerySystem.UnderRangedAttackRatio > 0.2f - ((base.Formation.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Loose) ? 0.1f : 0f))
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
				}
				if (this.TacticalDefendPosition != null)
				{
					float num;
					if (this.TacticalDefendPosition.TacticalPositionType == TacticalPosition.TacticalPositionTypeEnum.ChokePoint)
					{
						num = this.TacticalDefendPosition.Width;
					}
					else
					{
						int countOfUnits = base.Formation.CountOfUnits;
						float num2 = base.Formation.Interval * (float)(countOfUnits - 1) + base.Formation.UnitDiameter * (float)countOfUnits;
						num = MathF.Min(this.TacticalDefendPosition.Width, num2 / 3f);
					}
					base.Formation.SetFormOrder(FormOrder.FormOrderCustom(num), true);
					return;
				}
			}
			else
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0001D864 File Offset: 0x0001BA64
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0001D8CA File Offset: 0x0001BACA
		public override void ResetBehavior()
		{
			base.ResetBehavior();
			this.DefensePosition = WorldPosition.Invalid;
			this.TacticalDefendPosition = null;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0001D8E4 File Offset: 0x0001BAE4
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x0400034C RID: 844
		public WorldPosition DefensePosition = WorldPosition.Invalid;

		// Token: 0x0400034D RID: 845
		public TacticalPosition TacticalDefendPosition;
	}
}
