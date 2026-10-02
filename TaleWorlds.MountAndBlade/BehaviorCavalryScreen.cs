using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000112 RID: 274
	public class BehaviorCavalryScreen : BehaviorComponent
	{
		// Token: 0x06000DE8 RID: 3560 RVA: 0x0001C170 File Offset: 0x0001A370
		public BehaviorCavalryScreen(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			this._mainFormation = formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0001C1D0 File Offset: 0x0001A3D0
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0001C220 File Offset: 0x0001A420
		protected override void CalculateCurrentOrder()
		{
			if (this._mainFormation == null || base.Formation.AI.IsMainFormation || (base.Formation.AI.Side != FormationAI.BehaviorSide.Left && base.Formation.AI.Side != FormationAI.BehaviorSide.Right))
			{
				this._flankingEnemyCavalryFormation = null;
				WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
				cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
				return;
			}
			float currentTime = Mission.Current.CurrentTime;
			if (this._threatFormationCacheTime + 5f < currentTime)
			{
				this._threatFormationCacheTime = currentTime;
				Vec2 vec = ((base.Formation.AI.Side == FormationAI.BehaviorSide.Left) ? this._mainFormation.Direction.LeftVec() : this._mainFormation.Direction.RightVec()).Normalized() - this._mainFormation.Direction.Normalized();
				this._flankingEnemyCavalryFormation = null;
				float num = float.MinValue;
				foreach (Team team in Mission.Current.Teams)
				{
					if (team.IsEnemyOf(base.Formation.Team))
					{
						foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
						{
							if (formation.CountOfUnits > 0)
							{
								Vec2 vec2 = formation.CachedMedianPosition.AsVec2 - this._mainFormation.CachedMedianPosition.AsVec2;
								if (vec.Normalized().DotProduct(vec2.Normalized()) > 0.9238795f)
								{
									float formationPower = formation.QuerySystem.FormationPower;
									if (formationPower > num)
									{
										num = formationPower;
										this._flankingEnemyCavalryFormation = formation;
									}
								}
							}
						}
					}
				}
			}
			WorldPosition worldPosition;
			if (this._flankingEnemyCavalryFormation == null)
			{
				worldPosition = base.Formation.CachedMedianPosition;
				worldPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			else
			{
				Vec2 vec3 = this._flankingEnemyCavalryFormation.CachedMedianPosition.AsVec2 - this._mainFormation.CachedMedianPosition.AsVec2;
				float num2 = vec3.Normalize() * 0.5f;
				worldPosition = this._mainFormation.CachedMedianPosition;
				worldPosition.SetVec2(worldPosition.AsVec2 + num2 * vec3);
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0001C4E0 File Offset: 0x0001A6E0
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0001C4FC File Offset: 0x0001A6FC
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderSkein);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0001C564 File Offset: 0x0001A764
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

		// Token: 0x06000DEE RID: 3566 RVA: 0x0001C614 File Offset: 0x0001A814
		protected override float GetAiWeight()
		{
			if (this._mainFormation == null || !this._mainFormation.AI.IsMainFormation)
			{
				this._mainFormation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation);
			}
			if (this._flankingEnemyCavalryFormation == null)
			{
				return 0f;
			}
			return 1.2f;
		}

		// Token: 0x0400033B RID: 827
		private Formation _mainFormation;

		// Token: 0x0400033C RID: 828
		private Formation _flankingEnemyCavalryFormation;

		// Token: 0x0400033D RID: 829
		private float _threatFormationCacheTime;

		// Token: 0x0400033E RID: 830
		private const float _threatFormationCacheExpireTime = 5f;
	}
}
