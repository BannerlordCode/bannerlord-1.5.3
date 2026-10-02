using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000130 RID: 304
	public class BehaviorSergeantMPMountedRanged : BehaviorComponent
	{
		// Token: 0x06000EDE RID: 3806 RVA: 0x0002451C File Offset: 0x0002271C
		public BehaviorSergeantMPMountedRanged(Formation formation)
			: base(formation)
		{
			this._flagpositions = base.Formation.Team.Mission.ActiveMissionObjects.FindAllWithType<FlagCapturePoint>().ToList<FlagCapturePoint>();
			this._flagDominationGameMode = base.Formation.Team.Mission.GetMissionBehavior<MissionMultiplayerFlagDomination>();
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x00024578 File Offset: 0x00022778
		private MovementOrder UncapturedFlagMoveOrder()
		{
			if (this._flagpositions.Any<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) != base.Formation.Team))
			{
				FlagCapturePoint flagCapturePoint = this._flagpositions.Where<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) != base.Formation.Team).MinBy<FlagCapturePoint, float>((FlagCapturePoint fp) => base.Formation.Team.QuerySystem.GetLocalEnemyPower(fp.Position.AsVec2));
				return MovementOrder.MovementOrderMove(new WorldPosition(base.Formation.Team.Mission.Scene, UIntPtr.Zero, flagCapturePoint.Position, false));
			}
			if (this._flagpositions.Any<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) == base.Formation.Team))
			{
				Vec3 position = this._flagpositions.Where<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) == base.Formation.Team).MinBy<FlagCapturePoint, float>((FlagCapturePoint fp) => fp.Position.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition)).Position;
				return MovementOrder.MovementOrderMove(new WorldPosition(base.Formation.Team.Mission.Scene, UIntPtr.Zero, position, false));
			}
			return MovementOrder.MovementOrderStop;
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00024668 File Offset: 0x00022868
		protected override void CalculateCurrentOrder()
		{
			if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation == null || base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition) > 2500f)
			{
				base.CurrentOrder = this.UncapturedFlagMoveOrder();
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				return;
			}
			FlagCapturePoint flagCapturePoint = null;
			if (this._flagpositions.Any<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) != base.Formation.Team && !fp.IsContested))
			{
				flagCapturePoint = this._flagpositions.Where<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) != base.Formation.Team && !fp.IsContested).MinBy<FlagCapturePoint, float>((FlagCapturePoint fp) => base.Formation.CachedAveragePosition.DistanceSquared(fp.Position.AsVec2));
			}
			if (!base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.IsInfantryFormation && flagCapturePoint != null)
			{
				WorldPosition worldPosition = new WorldPosition(base.Formation.Team.Mission.Scene, UIntPtr.Zero, flagCapturePoint.Position, false);
				base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				return;
			}
			if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.IsRangedFormation)
			{
				base.CurrentOrder = MovementOrder.MovementOrderChargeToTarget(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				return;
			}
			Vec2 vec = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition;
			float num = vec.Normalize();
			WorldPosition cachedMedianPosition = base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition;
			if (num > base.Formation.QuerySystem.MissileRangeAdjusted)
			{
				cachedMedianPosition.SetVec2(cachedMedianPosition.AsVec2 - vec * (base.Formation.QuerySystem.MissileRangeAdjusted - base.Formation.Depth * 0.5f));
			}
			else if (num < base.Formation.QuerySystem.MissileRangeAdjusted * 0.4f)
			{
				cachedMedianPosition.SetVec2(cachedMedianPosition.AsVec2 - vec * (base.Formation.QuerySystem.MissileRangeAdjusted * 0.7f));
			}
			else
			{
				vec = vec.RightVec();
				cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition + vec * 20f);
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x000248FC File Offset: 0x00022AFC
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.CurrentOrder.OrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget && base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.IsRangedFormation)
			{
				base.Formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
				return;
			}
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0002498C File Offset: 0x00022B8C
		protected override void OnBehaviorActivatedAux()
		{
			this._flagpositions.RemoveAll((FlagCapturePoint fp) => fp.IsDeactivated);
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00024A1D File Offset: 0x00022C1D
		protected override float GetAiWeight()
		{
			if (base.Formation.QuerySystem.IsRangedCavalryFormation)
			{
				return 1.2f;
			}
			return 0f;
		}

		// Token: 0x04000391 RID: 913
		private List<FlagCapturePoint> _flagpositions;

		// Token: 0x04000392 RID: 914
		private MissionMultiplayerFlagDomination _flagDominationGameMode;
	}
}
