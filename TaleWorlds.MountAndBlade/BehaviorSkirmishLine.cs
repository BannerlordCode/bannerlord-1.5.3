using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000137 RID: 311
	public class BehaviorSkirmishLine : BehaviorComponent
	{
		// Token: 0x06000F1A RID: 3866 RVA: 0x00026BC0 File Offset: 0x00024DC0
		public BehaviorSkirmishLine(Formation formation)
			: base(formation)
		{
			this._behaviorSide = FormationAI.BehaviorSide.BehaviorSideNotSet;
			this._mainFormation = formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00026C18 File Offset: 0x00024E18
		protected override void CalculateCurrentOrder()
		{
			this._targetFormation = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation ?? base.Formation.CachedClosestEnemyFormation;
			Vec2 vec;
			WorldPosition worldPosition;
			if (this._targetFormation == null || this._mainFormation == null)
			{
				vec = base.Formation.Direction;
				worldPosition = base.Formation.CachedMedianPosition;
				worldPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			else
			{
				if (this._mainFormation.AI.ActiveBehavior is BehaviorCautiousAdvance)
				{
					vec = this._mainFormation.Direction;
				}
				else
				{
					vec = ((base.Formation.Direction.DotProduct((this._targetFormation.Formation.CachedMedianPosition.AsVec2 - this._mainFormation.CachedMedianPosition.AsVec2).Normalized()) < 0.5f) ? (this._targetFormation.Formation.CachedMedianPosition.AsVec2 - this._mainFormation.CachedMedianPosition.AsVec2) : base.Formation.Direction).Normalized();
				}
				Vec2 vec2 = this._mainFormation.OrderPosition - this._mainFormation.CachedMedianPosition.AsVec2;
				float num = this._mainFormation.CachedMovementSpeed * 7f;
				float length = vec2.Length;
				if (length > 0f)
				{
					float num2 = num / length;
					if (num2 < 1f)
					{
						vec2 *= num2;
					}
				}
				worldPosition = this._mainFormation.CachedMedianPosition;
				worldPosition.SetVec2(worldPosition.AsVec2 + vec * 8f + vec2);
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			if (!this.CurrentFacingOrder.GetDirection(base.Formation, null).IsValid || this.CurrentFacingOrder.OrderEnum == FacingOrder.FacingOrderEnum.LookAtEnemy || (this._targetFormation != null && (base.Formation.CachedAveragePosition.DistanceSquared(this._targetFormation.Formation.CachedMedianPosition.GetNavMeshVec3().AsVec2) >= base.Formation.QuerySystem.MissileRangeAdjusted * base.Formation.QuerySystem.MissileRangeAdjusted || (!this._targetFormation.IsRangedCavalryFormation && this.CurrentFacingOrder.GetDirection(base.Formation, null).DotProduct(vec) <= MBMath.Lerp(0.5f, 1f, 1f - MBMath.ClampFloat(base.Formation.Width, 1f, 20f) * 0.05f, 1E-05f)))))
			{
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
			}
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00026EF4 File Offset: 0x000250F4
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

		// Token: 0x06000F1D RID: 3869 RVA: 0x00026F6C File Offset: 0x0002516C
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00026FBC File Offset: 0x000251BC
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._mainFormation != null && base.Formation.Width > this._mainFormation.Width * 1.5f)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._mainFormation.Width * 1.2f), true);
			}
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0002703C File Offset: 0x0002523C
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWider, true);
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x000270A4 File Offset: 0x000252A4
		protected override float GetAiWeight()
		{
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
			float num2 = base.Formation.CachedAveragePosition.Distance((querySystem.ClosestSignificantlyLargeEnemyFormation ?? cachedClosestEnemyFormation).Formation.CachedMedianPosition.AsVec2) / (querySystem.ClosestSignificantlyLargeEnemyFormation ?? cachedClosestEnemyFormation).MovementSpeedMaximum;
			float num3 = MBMath.Lerp(0.5f, 1.2f, (MBMath.ClampFloat(num2, 4f, 8f) - 4f) / 4f, 1E-05f);
			return num * querySystem.MainFormationReliabilityFactor * num3;
		}

		// Token: 0x040003A5 RID: 933
		private Formation _mainFormation;

		// Token: 0x040003A6 RID: 934
		private FormationQuerySystem _targetFormation;
	}
}
