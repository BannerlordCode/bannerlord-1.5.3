using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200012F RID: 303
	public class BehaviorSergeantMPMounted : BehaviorComponent
	{
		// Token: 0x06000ECF RID: 3791 RVA: 0x0002404C File Offset: 0x0002224C
		public BehaviorSergeantMPMounted(Formation formation)
			: base(formation)
		{
			this._flagpositions = base.Formation.Team.Mission.ActiveMissionObjects.FindAllWithType<FlagCapturePoint>().ToList<FlagCapturePoint>();
			this._flagDominationGameMode = base.Formation.Team.Mission.GetMissionBehavior<MissionMultiplayerFlagDomination>();
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x000240A8 File Offset: 0x000222A8
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

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00024198 File Offset: 0x00022398
		protected override void CalculateCurrentOrder()
		{
			if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation == null || base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition) > 2500f)
			{
				base.CurrentOrder = this.UncapturedFlagMoveOrder();
				return;
			}
			FlagCapturePoint flagCapturePoint = null;
			if (this._flagpositions.Any<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) != base.Formation.Team && !fp.IsContested))
			{
				flagCapturePoint = this._flagpositions.Where<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) != base.Formation.Team && !fp.IsContested).MinBy<FlagCapturePoint, float>((FlagCapturePoint fp) => base.Formation.CachedAveragePosition.DistanceSquared(fp.Position.AsVec2));
			}
			if ((!base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.IsRangedFormation || base.Formation.QuerySystem.FormationPower / base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.FormationPower / base.Formation.Team.QuerySystem.RemainingPowerRatio <= 0.7f) && flagCapturePoint != null)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(new WorldPosition(base.Formation.Team.Mission.Scene, UIntPtr.Zero, flagCapturePoint.Position, false));
				return;
			}
			base.CurrentOrder = MovementOrder.MovementOrderChargeToTarget(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x000242F8 File Offset: 0x000224F8
		public override void TickOccasionally()
		{
			this._flagpositions.RemoveAll((FlagCapturePoint fp) => fp.IsDeactivated);
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00024348 File Offset: 0x00022548
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x000243AD File Offset: 0x000225AD
		protected override float GetAiWeight()
		{
			if (base.Formation.QuerySystem.IsCavalryFormation)
			{
				return 1.2f;
			}
			return 0f;
		}

		// Token: 0x0400038F RID: 911
		private List<FlagCapturePoint> _flagpositions;

		// Token: 0x04000390 RID: 912
		private MissionMultiplayerFlagDomination _flagDominationGameMode;
	}
}
