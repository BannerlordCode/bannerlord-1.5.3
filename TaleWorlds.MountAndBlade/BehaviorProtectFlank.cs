using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000122 RID: 290
	public class BehaviorProtectFlank : BehaviorComponent
	{
		// Token: 0x06000E76 RID: 3702 RVA: 0x0002107C File Offset: 0x0001F27C
		public BehaviorProtectFlank(Formation formation)
			: base(formation)
		{
			this._protectFlankState = BehaviorProtectFlank.BehaviorState.HoldingFlank;
			this._behaviorSide = formation.AI.Side;
			this._mainFormation = formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			this.CalculateCurrentOrder();
			base.CurrentOrder = this._movementOrder;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x000210F0 File Offset: 0x0001F2F0
		protected override void CalculateCurrentOrder()
		{
			if (this._mainFormation == null || base.Formation.AI.IsMainFormation || base.Formation.CachedClosestEnemyFormation == null)
			{
				base.CurrentOrder = MovementOrder.MovementOrderStop;
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				return;
			}
			if (this._protectFlankState == BehaviorProtectFlank.BehaviorState.HoldingFlank || this._protectFlankState == BehaviorProtectFlank.BehaviorState.Returning)
			{
				Vec2 direction = this._mainFormation.Direction;
				Vec2 vec = (base.Formation.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - this._mainFormation.CachedMedianPosition.AsVec2).Normalized();
				Vec2 vec2;
				if (this._behaviorSide == FormationAI.BehaviorSide.Right || this.FlankSide == FormationAI.BehaviorSide.Right)
				{
					vec2 = this._mainFormation.CurrentPosition + vec.RightVec().Normalized() * (this._mainFormation.Width + base.Formation.Width + 10f);
					vec2 -= vec * (this._mainFormation.Depth + base.Formation.Depth);
				}
				else if (this._behaviorSide == FormationAI.BehaviorSide.Left || this.FlankSide == FormationAI.BehaviorSide.Left)
				{
					vec2 = this._mainFormation.CurrentPosition + vec.LeftVec().Normalized() * (this._mainFormation.Width + base.Formation.Width + 10f);
					vec2 -= vec * (this._mainFormation.Depth + base.Formation.Depth);
				}
				else
				{
					vec2 = this._mainFormation.CurrentPosition + vec * ((this._mainFormation.Depth + base.Formation.Depth) * 0.5f + 10f);
				}
				WorldPosition cachedMedianPosition = this._mainFormation.CachedMedianPosition;
				cachedMedianPosition.SetVec2(vec2);
				this._movementOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
				base.CurrentOrder = this._movementOrder;
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(direction);
			}
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00021308 File Offset: 0x0001F508
		private void CheckAndChangeState()
		{
			Vec2 position = this._movementOrder.GetPosition(base.Formation);
			switch (this._protectFlankState)
			{
			case BehaviorProtectFlank.BehaviorState.HoldingFlank:
			{
				FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
				if (cachedClosestEnemyFormation != null)
				{
					float num = 50f + (base.Formation.Depth + cachedClosestEnemyFormation.Formation.Depth) / 2f;
					if (cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2.DistanceSquared(position) < num * num)
					{
						this._chargeToTargetOrder = MovementOrder.MovementOrderChargeToTarget(cachedClosestEnemyFormation.Formation);
						base.CurrentOrder = this._chargeToTargetOrder;
						this._protectFlankState = BehaviorProtectFlank.BehaviorState.Charging;
						return;
					}
				}
				break;
			}
			case BehaviorProtectFlank.BehaviorState.Charging:
			{
				FormationQuerySystem cachedClosestEnemyFormation2 = base.Formation.CachedClosestEnemyFormation;
				if (cachedClosestEnemyFormation2 == null)
				{
					base.CurrentOrder = this._movementOrder;
					this._protectFlankState = BehaviorProtectFlank.BehaviorState.Returning;
					return;
				}
				float num2 = 60f + (base.Formation.Depth + cachedClosestEnemyFormation2.Formation.Depth) / 2f;
				if (base.Formation.CachedAveragePosition.DistanceSquared(position) > num2 * num2)
				{
					base.CurrentOrder = this._movementOrder;
					this._protectFlankState = BehaviorProtectFlank.BehaviorState.Returning;
					return;
				}
				break;
			}
			case BehaviorProtectFlank.BehaviorState.Returning:
				if (base.Formation.CachedAveragePosition.DistanceSquared(position) < 400f)
				{
					this._protectFlankState = BehaviorProtectFlank.BehaviorState.HoldingFlank;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00021460 File Offset: 0x0001F660
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x000214B0 File Offset: 0x0001F6B0
		public override void TickOccasionally()
		{
			this.CheckAndChangeState();
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._protectFlankState == BehaviorProtectFlank.BehaviorState.HoldingFlank && base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2) > 1600f && base.Formation.QuerySystem.UnderRangedAttackRatio > 0.2f - ((base.Formation.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Loose) ? 0.1f : 0f))
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
				return;
			}
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x000215A0 File Offset: 0x0001F7A0
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00021608 File Offset: 0x0001F808
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			if (this._mainFormation != null)
			{
				behaviorString.SetTextVariable("AI_SIDE", GameTexts.FindText("str_formation_ai_side_strings", this._mainFormation.AI.Side.ToString()));
				behaviorString.SetTextVariable("CLASS", GameTexts.FindText("str_formation_class_string", this._mainFormation.PhysicalClass.GetName()));
			}
			return behaviorString;
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x000216C8 File Offset: 0x0001F8C8
		protected override float GetAiWeight()
		{
			if (this._mainFormation == null || !this._mainFormation.AI.IsMainFormation)
			{
				this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			}
			if (this._mainFormation == null || base.Formation.AI.IsMainFormation)
			{
				return 0f;
			}
			return 1.2f;
		}

		// Token: 0x04000377 RID: 887
		private Formation _mainFormation;

		// Token: 0x04000378 RID: 888
		public FormationAI.BehaviorSide FlankSide;

		// Token: 0x04000379 RID: 889
		private BehaviorProtectFlank.BehaviorState _protectFlankState;

		// Token: 0x0400037A RID: 890
		private MovementOrder _movementOrder;

		// Token: 0x0400037B RID: 891
		private MovementOrder _chargeToTargetOrder;

		// Token: 0x02000448 RID: 1096
		private enum BehaviorState
		{
			// Token: 0x040019E8 RID: 6632
			HoldingFlank,
			// Token: 0x040019E9 RID: 6633
			Charging,
			// Token: 0x040019EA RID: 6634
			Returning
		}
	}
}
