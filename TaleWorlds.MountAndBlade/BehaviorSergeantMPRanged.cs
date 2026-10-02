using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000131 RID: 305
	public class BehaviorSergeantMPRanged : BehaviorComponent
	{
		// Token: 0x06000EED RID: 3821 RVA: 0x00024B8C File Offset: 0x00022D8C
		public BehaviorSergeantMPRanged(Formation formation)
			: base(formation)
		{
			this._flagpositions = base.Formation.Team.Mission.ActiveMissionObjects.FindAllWithType<FlagCapturePoint>().ToList<FlagCapturePoint>();
			this._flagDominationGameMode = base.Formation.Team.Mission.GetMissionBehavior<MissionMultiplayerFlagDomination>();
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00024BE8 File Offset: 0x00022DE8
		protected override void CalculateCurrentOrder()
		{
			bool flag = false;
			Formation formation = null;
			float num = float.MaxValue;
			foreach (Team team in base.Formation.Team.Mission.Teams)
			{
				if (team.IsEnemyOf(base.Formation.Team))
				{
					for (int i = 0; i < Math.Min(team.FormationsIncludingSpecialAndEmpty.Count, 8); i++)
					{
						Formation formation2 = team.FormationsIncludingSpecialAndEmpty[i];
						if (formation2.CountOfUnits > 0)
						{
							flag = true;
							if (formation2.QuerySystem.IsCavalryFormation || formation2.QuerySystem.IsRangedCavalryFormation)
							{
								float num2 = formation2.CachedMedianPosition.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition);
								if (num2 < num)
								{
									num = num2;
									formation = formation2;
								}
							}
						}
					}
				}
			}
			if (base.Formation.Team.FormationsIncludingEmpty.AnyQ<Formation>((Formation f) => f.CountOfUnits > 0 && f != base.Formation && f.QuerySystem.IsInfantryFormation))
			{
				this._attachedInfantry = base.Formation.Team.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && f != base.Formation && f.QuerySystem.IsInfantryFormation).MinBy<Formation, float>((Formation f) => f.CachedMedianPosition.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition));
				Formation formation3 = null;
				if (flag)
				{
					if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition) <= 4900f)
					{
						formation3 = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation;
					}
					else if (formation != null)
					{
						formation3 = formation;
					}
				}
				Vec2 vec = ((formation3 == null) ? this._attachedInfantry.Direction : (formation3.CachedMedianPosition.AsVec2 - this._attachedInfantry.CachedMedianPosition.AsVec2).Normalized());
				WorldPosition cachedMedianPosition = this._attachedInfantry.CachedMedianPosition;
				cachedMedianPosition.SetVec2(cachedMedianPosition.AsVec2 - vec * ((this._attachedInfantry.Depth + base.Formation.Depth) / 2f));
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
				return;
			}
			if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition) <= 4900f)
			{
				Vec2 vec2 = (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
				float num3 = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2.Distance(base.Formation.CachedAveragePosition);
				WorldPosition cachedMedianPosition2 = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition;
				if (num3 > base.Formation.QuerySystem.MissileRangeAdjusted)
				{
					cachedMedianPosition2.SetVec2(cachedMedianPosition2.AsVec2 - vec2 * (base.Formation.QuerySystem.MissileRangeAdjusted - base.Formation.Depth * 0.5f));
				}
				else if (num3 < base.Formation.QuerySystem.MissileRangeAdjusted * 0.4f)
				{
					cachedMedianPosition2.SetVec2(cachedMedianPosition2.AsVec2 - vec2 * (base.Formation.QuerySystem.MissileRangeAdjusted * 0.4f));
				}
				else
				{
					cachedMedianPosition2.SetVec2(base.Formation.CachedAveragePosition);
				}
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition2);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec2);
				return;
			}
			if (this._flagpositions.Any<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) != base.Formation.Team))
			{
				Vec3 position = this._flagpositions.Where<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) != base.Formation.Team).MinBy<FlagCapturePoint, float>((FlagCapturePoint fp) => fp.Position.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition)).Position;
				if (base.CurrentOrder.OrderEnum == MovementOrder.MovementOrderEnum.Invalid || base.CurrentOrder.GetPosition(base.Formation) != position.AsVec2)
				{
					Vec2 vec3;
					if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation == null)
					{
						vec3 = base.Formation.Direction;
					}
					else
					{
						vec3 = (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
					}
					WorldPosition worldPosition = new WorldPosition(base.Formation.Team.Mission.Scene, UIntPtr.Zero, position, false);
					base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
					this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec3);
					return;
				}
			}
			else
			{
				if (this._flagpositions.Any<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) == base.Formation.Team))
				{
					Vec3 position2 = this._flagpositions.Where<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) == base.Formation.Team).MinBy<FlagCapturePoint, float>((FlagCapturePoint fp) => fp.Position.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition)).Position;
					base.CurrentOrder = MovementOrder.MovementOrderMove(new WorldPosition(base.Formation.Team.Mission.Scene, UIntPtr.Zero, position2, false));
					this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
					return;
				}
				WorldPosition cachedMedianPosition3 = base.Formation.CachedMedianPosition;
				cachedMedianPosition3.SetVec2(base.Formation.CachedAveragePosition);
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition3);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x000251E8 File Offset: 0x000233E8
		public override void TickOccasionally()
		{
			this._flagpositions.RemoveAll((FlagCapturePoint fp) => fp.IsDeactivated);
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00025248 File Offset: 0x00023448
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x000252AE File Offset: 0x000234AE
		protected override float GetAiWeight()
		{
			if (base.Formation.QuerySystem.IsRangedFormation)
			{
				return 1.2f;
			}
			return 0f;
		}

		// Token: 0x04000393 RID: 915
		private List<FlagCapturePoint> _flagpositions;

		// Token: 0x04000394 RID: 916
		private Formation _attachedInfantry;

		// Token: 0x04000395 RID: 917
		private MissionMultiplayerFlagDomination _flagDominationGameMode;
	}
}
