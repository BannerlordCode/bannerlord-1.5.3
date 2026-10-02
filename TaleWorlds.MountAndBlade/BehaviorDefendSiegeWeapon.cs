using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000118 RID: 280
	public class BehaviorDefendSiegeWeapon : BehaviorComponent
	{
		// Token: 0x06000E31 RID: 3633 RVA: 0x0001E4A0 File Offset: 0x0001C6A0
		public BehaviorDefendSiegeWeapon(Formation formation)
			: base(formation)
		{
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0001E4BA File Offset: 0x0001C6BA
		public void SetDefensePositionFromTactic(WorldPosition defensePosition)
		{
			this._defensePosition = defensePosition;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0001E4C3 File Offset: 0x0001C6C3
		public void SetDefendedSiegeWeaponFromTactic(SiegeWeapon siegeWeapon)
		{
			this._defendedSiegeWeapon = siegeWeapon;
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0001E4CC File Offset: 0x0001C6CC
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			return behaviorString;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0001E528 File Offset: 0x0001C728
		protected override void CalculateCurrentOrder()
		{
			float num = 5f;
			Vec2 vec;
			if (this._tacticalDefendPosition != null)
			{
				vec = ((!this._tacticalDefendPosition.IsInsurmountable) ? this._tacticalDefendPosition.Direction : (base.Formation.Team.QuerySystem.AverageEnemyPosition - this._tacticalDefendPosition.Position.AsVec2).Normalized());
			}
			else if (base.Formation.CachedClosestEnemyFormation == null)
			{
				vec = base.Formation.Direction;
			}
			else
			{
				FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
				if (this._defendedSiegeWeapon != null)
				{
					vec = cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - this._defendedSiegeWeapon.GameEntity.GlobalPosition.AsVec2;
					num = vec.Normalize();
					num = MathF.Min(num, 5f);
					float num2 = ((this._defendedSiegeWeapon.WaitEntity != null) ? (this._defendedSiegeWeapon.WaitEntity.GlobalPosition - this._defendedSiegeWeapon.GameEntity.GlobalPosition).Length : 3f);
					num = MathF.Max(num, num2);
				}
				else
				{
					vec = ((base.Formation.Direction.DotProduct((cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized()) < 0.5f) ? (cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition) : base.Formation.Direction).Normalized();
				}
			}
			if (this._tacticalDefendPosition != null)
			{
				if (!this._tacticalDefendPosition.IsInsurmountable)
				{
					base.CurrentOrder = MovementOrder.MovementOrderMove(this._tacticalDefendPosition.Position);
				}
				else
				{
					Vec2 vec2 = this._tacticalDefendPosition.Position.AsVec2 + this._tacticalDefendPosition.Width * 0.5f * vec;
					WorldPosition position = this._tacticalDefendPosition.Position;
					position.SetVec2(vec2);
					base.CurrentOrder = MovementOrder.MovementOrderMove(position);
				}
				this.CurrentFacingOrder = ((!this._tacticalDefendPosition.IsInsurmountable) ? FacingOrder.FacingOrderLookAtDirection(vec) : FacingOrder.FacingOrderLookAtEnemy);
				return;
			}
			if (this._defensePosition.IsValid)
			{
				WorldPosition defensePosition = this._defensePosition;
				defensePosition.SetVec2(this._defensePosition.AsVec2 + vec * num);
				base.CurrentOrder = MovementOrder.MovementOrderMove(defensePosition);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
				return;
			}
			WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
			cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
			base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0001E820 File Offset: 0x0001CA20
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.CachedAveragePosition.DistanceSquared(base.CurrentOrder.GetPosition(base.Formation)) < 100f)
			{
				if (base.Formation.QuerySystem.HasShield)
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
				}
				else if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2) > 100f && base.Formation.QuerySystem.UnderRangedAttackRatio > 0.2f - ((base.Formation.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Loose) ? 0.1f : 0f))
				{
					base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
				}
				if (this._tacticalDefendPosition != null)
				{
					float num;
					if (this._tacticalDefendPosition.TacticalPositionType == TacticalPosition.TacticalPositionTypeEnum.ChokePoint)
					{
						num = this._tacticalDefendPosition.Width;
					}
					else
					{
						int countOfUnits = base.Formation.CountOfUnits;
						float num2 = base.Formation.Interval * (float)(countOfUnits - 1) + base.Formation.UnitDiameter * (float)countOfUnits;
						num = MathF.Min(this._tacticalDefendPosition.Width, num2 / 3f);
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

		// Token: 0x06000E37 RID: 3639 RVA: 0x0001E9D8 File Offset: 0x0001CBD8
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0001EA3E File Offset: 0x0001CC3E
		public override void ResetBehavior()
		{
			base.ResetBehavior();
			this._defensePosition = WorldPosition.Invalid;
			this._tacticalDefendPosition = null;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0001EA58 File Offset: 0x0001CC58
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x0400035E RID: 862
		private WorldPosition _defensePosition = WorldPosition.Invalid;

		// Token: 0x0400035F RID: 863
		private TacticalPosition _tacticalDefendPosition;

		// Token: 0x04000360 RID: 864
		private SiegeWeapon _defendedSiegeWeapon;
	}
}
