using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200012C RID: 300
	public class BehaviorScreenedSkirmish : BehaviorComponent
	{
		// Token: 0x06000EB9 RID: 3769 RVA: 0x00022FD0 File Offset: 0x000211D0
		public BehaviorScreenedSkirmish(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			this._mainFormation = formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x00023038 File Offset: 0x00021238
		protected override void CalculateCurrentOrder()
		{
			Vec2 vec3;
			if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && this._mainFormation != null)
			{
				Vec2 vec = (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
				Vec2 vec2 = (this._mainFormation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
				vec3 = ((vec.DotProduct(vec2) > 0.5f) ? this._mainFormation.FacingOrder.GetDirection(this._mainFormation, null) : vec);
			}
			else
			{
				vec3 = base.Formation.Direction;
			}
			WorldPosition worldPosition;
			if (this._mainFormation == null)
			{
				worldPosition = base.Formation.CachedMedianPosition;
				worldPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			else
			{
				worldPosition = this._mainFormation.CachedMedianPosition;
				worldPosition.SetVec2(worldPosition.AsVec2 - vec3 * ((this._mainFormation.Depth + base.Formation.Depth) * 0.5f));
			}
			if (!base.CurrentOrder.CreateNewOrderWorldPositionMT(base.Formation, WorldPosition.WorldPositionEnforcedCache.None).IsValid || (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && (!base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.IsRangedCavalryFormation || base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.GetNavMeshVec3().AsVec2) >= base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.MissileRangeAdjusted * base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.MissileRangeAdjusted || base.CurrentOrder.CreateNewOrderWorldPositionMT(base.Formation, WorldPosition.WorldPositionEnforcedCache.NavMeshVec3).GetNavMeshVec3().DistanceSquared(worldPosition.GetNavMeshVec3()) >= base.Formation.Depth * base.Formation.Depth)))
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			}
			if (!this.CurrentFacingOrder.GetDirection(base.Formation, null).IsValid || this.CurrentFacingOrder.OrderEnum == FacingOrder.FacingOrderEnum.LookAtEnemy || base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation == null || base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.GetNavMeshVec3().AsVec2) >= base.Formation.QuerySystem.MissileRangeAdjusted * base.Formation.QuerySystem.MissileRangeAdjusted || (!base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.IsRangedCavalryFormation && this.CurrentFacingOrder.GetDirection(base.Formation, null).DotProduct(vec3) <= MBMath.Lerp(0.5f, 1f, 1f - MBMath.ClampFloat(base.Formation.Width, 1f, 20f) * 0.05f, 1E-05f)))
			{
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec3);
			}
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x000233B0 File Offset: 0x000215B0
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			bool flag = cachedClosestEnemyFormation == null || this._mainFormation.CachedMedianPosition.AsVec2.DistanceSquared(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) <= base.Formation.CachedAveragePosition.DistanceSquared(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) || base.Formation.CachedAveragePosition.DistanceSquared(base.CurrentOrder.GetPosition(base.Formation)) <= (this._mainFormation.Depth + base.Formation.Depth) * (this._mainFormation.Depth + base.Formation.Depth) * 0.25f;
			if (flag != this._isFireAtWill)
			{
				this._isFireAtWill = flag;
				base.Formation.SetFiringOrder(this._isFireAtWill ? FiringOrder.FiringOrderFireAtWill : FiringOrder.FiringOrderHoldYourFire);
			}
			if (this._mainFormation != null && MathF.Abs(this._mainFormation.Width - base.Formation.Width) > 10f)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._mainFormation.Width), true);
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0002352C File Offset: 0x0002172C
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00023594 File Offset: 0x00021794
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			if (this._mainFormation != null)
			{
				behaviorString.SetTextVariable("AI_SIDE", GameTexts.FindText("str_formation_ai_side_strings", this._mainFormation.AI.Side.ToString()));
				behaviorString.SetTextVariable("CLASS", GameTexts.FindText("str_formation_class_string", this._mainFormation.PhysicalClass.GetName()));
			}
			return behaviorString;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0002360C File Offset: 0x0002180C
		protected override float GetAiWeight()
		{
			MovementOrder currentOrder = base.CurrentOrder;
			if ((in currentOrder) == MovementOrder.MovementOrderStop)
			{
				this.CalculateCurrentOrder();
			}
			if (this._mainFormation == null || !this._mainFormation.AI.IsMainFormation)
			{
				this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			}
			if (this._behaviorSide != base.Formation.AI.Side)
			{
				this._behaviorSide = base.Formation.AI.Side;
			}
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			if (this._mainFormation == null || base.Formation.AI.IsMainFormation || cachedClosestEnemyFormation == null)
			{
				return 0f;
			}
			FormationQuerySystem querySystem = base.Formation.QuerySystem;
			float num = MBMath.Lerp(0.1f, 1f, MBMath.ClampFloat(querySystem.RangedUnitRatio + querySystem.RangedCavalryUnitRatio, 0f, 0.5f) * 2f, 1E-05f);
			float num2 = this._mainFormation.Direction.Normalized().DotProduct((cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - this._mainFormation.CachedMedianPosition.AsVec2).Normalized());
			float num3 = MBMath.LinearExtrapolation(0.5f, 1.1f, (num2 + 1f) / 2f);
			float num4 = base.Formation.CachedAveragePosition.Distance(cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) / cachedClosestEnemyFormation.MovementSpeedMaximum;
			float num5 = MBMath.Lerp(0.5f, 1.2f, (8f - MBMath.ClampFloat(num4, 4f, 8f)) / 4f, 1E-05f);
			return num * base.Formation.QuerySystem.MainFormationReliabilityFactor * num3 * num5;
		}

		// Token: 0x04000387 RID: 903
		private Formation _mainFormation;

		// Token: 0x04000388 RID: 904
		private bool _isFireAtWill = true;
	}
}
