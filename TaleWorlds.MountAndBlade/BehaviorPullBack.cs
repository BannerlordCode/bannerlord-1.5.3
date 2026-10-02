using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000124 RID: 292
	public class BehaviorPullBack : BehaviorComponent
	{
		// Token: 0x06000E83 RID: 3715 RVA: 0x00021865 File Offset: 0x0001FA65
		public BehaviorPullBack(Formation formation)
			: base(formation)
		{
			this.CalculateCurrentOrder();
			base.BehaviorCoherence = 0.2f;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00021880 File Offset: 0x0001FA80
		protected override void CalculateCurrentOrder()
		{
			WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			if (cachedClosestEnemyFormation == null)
			{
				cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
			}
			else
			{
				Vec2 vec = (base.Formation.CachedAveragePosition - cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2).Normalized();
				cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition + 50f * vec);
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00021917 File Offset: 0x0001FB17
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00021930 File Offset: 0x0001FB30
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00021998 File Offset: 0x0001FB98
		protected override float GetAiWeight()
		{
			if (base.Formation.Team.TeamAI is TeamAISiegeComponent && !(base.Formation.Team.TeamAI is TeamAISallyOutAttacker) && !(base.Formation.Team.TeamAI is TeamAISallyOutDefender))
			{
				return this.GetSiegeAIWeight();
			}
			FormationQuerySystem querySystem = base.Formation.QuerySystem;
			FormationQuerySystem formationQuerySystem = querySystem.ClosestSignificantlyLargeEnemyFormation;
			if (formationQuerySystem == null || formationQuerySystem.Formation.CachedClosestEnemyFormation != querySystem || formationQuerySystem.MovementSpeedMaximum - querySystem.MovementSpeedMaximum > 2f)
			{
				formationQuerySystem = base.Formation.CachedClosestEnemyFormation;
				if (formationQuerySystem == null || formationQuerySystem.Formation.CachedClosestEnemyFormation != querySystem || formationQuerySystem.MovementSpeedMaximum - querySystem.MovementSpeedMaximum > 2f)
				{
					return 0f;
				}
			}
			float num = base.Formation.CachedAveragePosition.Distance(formationQuerySystem.Formation.CachedMedianPosition.AsVec2) / formationQuerySystem.MovementSpeedMaximum;
			float num2 = MBMath.ClampFloat(num, 4f, 10f);
			float num3 = MBMath.Lerp(0.1f, 1f, 1f - (num2 - 4f) / 6f, 1E-05f);
			float num4 = 0f;
			foreach (Team team in Mission.Current.Teams)
			{
				if (team.IsEnemyOf(base.Formation.Team))
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						if (formation.CountOfUnits > 0 && formation != formationQuerySystem.Formation)
						{
							float num5 = formation.CachedMedianPosition.AsVec2.Distance(formationQuerySystem.Formation.CachedMedianPosition.AsVec2) / formation.QuerySystem.MovementSpeedMaximum;
							if (num5 <= num + 4f && (num > 8f || formation.CachedClosestEnemyFormation == base.Formation.QuerySystem))
							{
								bool flag = false;
								if (num <= 8f)
								{
									foreach (Team team2 in base.Formation.Team.Mission.Teams)
									{
										if (team2.IsFriendOf(base.Formation.Team))
										{
											foreach (Formation formation2 in team2.FormationsIncludingSpecialAndEmpty)
											{
												if (formation2.CountOfUnits > 0 && formation2 != base.Formation && formation2.CachedClosestEnemyFormation == formation.QuerySystem && formation2.CachedMedianPosition.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition) / formation2.QuerySystem.MovementSpeedMaximum < num5 + 4f)
												{
													flag = true;
													break;
												}
											}
											if (flag)
											{
												break;
											}
										}
									}
								}
								if (!flag)
								{
									num4 += formation.QuerySystem.FormationMeleeFightingPower * formation.QuerySystem.GetClassWeightedFactor(1f, 1f, 1f, 1f);
								}
							}
						}
					}
				}
			}
			float num6 = 0f;
			foreach (Team team3 in Mission.Current.Teams)
			{
				if (team3.IsFriendOf(base.Formation.Team))
				{
					foreach (Formation formation3 in team3.FormationsIncludingSpecialAndEmpty)
					{
						if (formation3.CountOfUnits > 0 && formation3 != base.Formation && formation3.CachedClosestEnemyFormation == formationQuerySystem && formation3.CachedMedianPosition.AsVec2.Distance(formation3.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2) / formation3.QuerySystem.MovementSpeedMaximum < 4f)
						{
							num6 += formation3.QuerySystem.FormationMeleeFightingPower * formation3.QuerySystem.GetClassWeightedFactor(1f, 1f, 1f, 1f);
						}
					}
				}
			}
			return MBMath.ClampFloat((1f + num4 + formationQuerySystem.Formation.QuerySystem.FormationMeleeFightingPower * formationQuerySystem.GetClassWeightedFactor(1f, 1f, 1f, 1f)) / (base.Formation.GetFormationMeleeFightingPower() * querySystem.GetClassWeightedFactor(1f, 1f, 1f, 1f) + num6 + 1f) * querySystem.Team.RemainingPowerRatio / 3f, 0.1f, 1.21f) * num3;
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00021F68 File Offset: 0x00020168
		private float GetSiegeAIWeight()
		{
			return 0f;
		}
	}
}
