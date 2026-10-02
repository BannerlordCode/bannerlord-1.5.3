using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000126 RID: 294
	public class BehaviorReserve : BehaviorComponent
	{
		// Token: 0x06000E8D RID: 3725 RVA: 0x000220E1 File Offset: 0x000202E1
		public BehaviorReserve(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00022104 File Offset: 0x00020304
		protected override void CalculateCurrentOrder()
		{
			Formation formation = base.Formation.Team.FormationsIncludingEmpty.FirstOrDefaultQ<Formation>((Formation f) => f.CountOfUnits > 0 && f != base.Formation && f.AI.IsMainFormation);
			WorldPosition worldPosition;
			if (formation != null)
			{
				worldPosition = formation.CachedMedianPosition;
				Vec2 vec = (base.Formation.QuerySystem.Team.AverageEnemyPosition - formation.CachedMedianPosition.AsVec2).Normalized();
				worldPosition.SetVec2(worldPosition.AsVec2 - vec * (40f + base.Formation.Depth));
			}
			else
			{
				Vec2 vec2 = Vec2.Zero;
				int num = 0;
				foreach (Formation formation2 in base.Formation.Team.FormationsIncludingSpecialAndEmpty)
				{
					if (formation2.CountOfUnits > 0 && formation2 != base.Formation)
					{
						vec2 += formation2.CachedMedianPosition.AsVec2;
						num++;
					}
				}
				if (num <= 0)
				{
					base.CurrentOrder = MovementOrder.MovementOrderStop;
					return;
				}
				WorldPosition worldPosition2 = WorldPosition.Invalid;
				float num2 = float.MaxValue;
				vec2 *= 1f / (float)num;
				foreach (Formation formation3 in base.Formation.Team.FormationsIncludingSpecialAndEmpty)
				{
					if (formation3.CountOfUnits > 0 && formation3 != base.Formation)
					{
						float num3 = vec2.DistanceSquared(formation3.CachedMedianPosition.AsVec2);
						if (num3 < num2)
						{
							num2 = num3;
							worldPosition2 = formation3.CachedMedianPosition;
						}
					}
				}
				Vec2 vec3 = (base.Formation.QuerySystem.Team.AverageEnemyPosition - worldPosition2.AsVec2).Normalized();
				worldPosition = worldPosition2;
				worldPosition.SetVec2(worldPosition.AsVec2 - vec3 * (20f + base.Formation.Depth));
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00022348 File Offset: 0x00020548
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00022364 File Offset: 0x00020564
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWider, true);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x000223CC File Offset: 0x000205CC
		protected override float GetAiWeight()
		{
			if (!base.Formation.AI.IsMainFormation)
			{
				foreach (Formation formation in base.Formation.Team.FormationsIncludingSpecialAndEmpty)
				{
					if (base.Formation != formation && formation.CountOfUnits > 0)
					{
						using (List<Team>.Enumerator enumerator2 = Mission.Current.Teams.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Team team = enumerator2.Current;
								if (team.IsEnemyOf(base.Formation.Team))
								{
									using (List<Formation>.Enumerator enumerator3 = team.FormationsIncludingSpecialAndEmpty.GetEnumerator())
									{
										while (enumerator3.MoveNext())
										{
											if (enumerator3.Current.CountOfUnits > 0)
											{
												return 0.04f;
											}
										}
									}
								}
							}
							break;
						}
					}
				}
			}
			return 0f;
		}
	}
}
