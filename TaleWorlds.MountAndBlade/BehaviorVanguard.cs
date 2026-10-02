using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013D RID: 317
	public class BehaviorVanguard : BehaviorComponent
	{
		// Token: 0x06000F43 RID: 3907 RVA: 0x00028928 File Offset: 0x00026B28
		public BehaviorVanguard(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			this._mainFormation = formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00028988 File Offset: 0x00026B88
		protected override void CalculateCurrentOrder()
		{
			if (this._mainFormation != null && this._mainFormation.CountOfUnits == 0)
			{
				this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			}
			Vec2 vec;
			WorldPosition worldPosition;
			if (this._mainFormation != null)
			{
				vec = this._mainFormation.Direction;
				Vec2 vec2 = (base.Formation.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - this._mainFormation.CachedMedianPosition.AsVec2).Normalized();
				worldPosition = this._mainFormation.CachedMedianPosition;
				worldPosition.SetVec2(this._mainFormation.CurrentPosition + vec2 * ((this._mainFormation.Depth + base.Formation.Depth) * 0.5f + 10f));
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

		// Token: 0x06000F45 RID: 3909 RVA: 0x00028ACC File Offset: 0x00026CCC
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x00028B1C File Offset: 0x00026D1C
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2) > 1600f && base.Formation.QuerySystem.UnderRangedAttackRatio > 0.2f - ((base.Formation.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.Loose) ? 0.1f : 0f))
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
				return;
			}
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x00028BFC File Offset: 0x00026DFC
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00028C64 File Offset: 0x00026E64
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			if (this._mainFormation != null)
			{
				behaviorString.SetTextVariable("AI_SIDE", GameTexts.FindText("str_formation_ai_side_strings", this._mainFormation.AI.Side.ToString()));
				behaviorString.SetTextVariable("CLASS", GameTexts.FindText("str_formation_class_string", this._mainFormation.PhysicalClass.GetName()));
			}
			return behaviorString;
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00028D14 File Offset: 0x00026F14
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

		// Token: 0x040003BA RID: 954
		private Formation _mainFormation;
	}
}
