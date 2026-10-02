using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200012E RID: 302
	public class BehaviorSergeantMPLastFlagLastStand : BehaviorComponent
	{
		// Token: 0x06000ECA RID: 3786 RVA: 0x00023DBE File Offset: 0x00021FBE
		public BehaviorSergeantMPLastFlagLastStand(Formation formation)
			: base(formation)
		{
			this._flagpositions = Mission.Current.ActiveMissionObjects.FindAllWithType<FlagCapturePoint>().ToList<FlagCapturePoint>();
			this._flagDominationGameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerFlagDomination>();
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00023DF8 File Offset: 0x00021FF8
		protected override void CalculateCurrentOrder()
		{
			base.CurrentOrder = ((this._flagpositions.Count > 0) ? MovementOrder.MovementOrderMove(new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, this._flagpositions[0].Position, false)) : MovementOrder.MovementOrderStop);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00023E4C File Offset: 0x0002204C
		public override void TickOccasionally()
		{
			this._flagpositions.RemoveAll((FlagCapturePoint fp) => fp.IsDeactivated);
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00023EAC File Offset: 0x000220AC
		protected override void OnBehaviorActivatedAux()
		{
			this._flagpositions.RemoveAll((FlagCapturePoint fp) => fp.IsDeactivated);
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00023F3C File Offset: 0x0002213C
		protected override float GetAiWeight()
		{
			if (this._lastEffort)
			{
				return 10f;
			}
			this._flagpositions.RemoveAll((FlagCapturePoint fp) => fp.IsDeactivated);
			FlagCapturePoint flagCapturePoint = this._flagpositions.FirstOrDefault<FlagCapturePoint>();
			if (this._flagpositions.Count != 1 || this._flagDominationGameMode.GetFlagOwnerTeam(flagCapturePoint) == null || !this._flagDominationGameMode.GetFlagOwnerTeam(flagCapturePoint).IsEnemyOf(base.Formation.Team))
			{
				return 0f;
			}
			float timeUntilBattleSideVictory = this._flagDominationGameMode.GetTimeUntilBattleSideVictory(this._flagDominationGameMode.GetFlagOwnerTeam(flagCapturePoint).Side);
			if (timeUntilBattleSideVictory <= 60f)
			{
				return 10f;
			}
			float num = base.Formation.CachedAveragePosition.Distance(flagCapturePoint.Position.AsVec2);
			float movementSpeedMaximum = base.Formation.QuerySystem.MovementSpeedMaximum;
			if (num / movementSpeedMaximum * 8f > timeUntilBattleSideVictory)
			{
				this._lastEffort = true;
				return 10f;
			}
			return 0f;
		}

		// Token: 0x0400038C RID: 908
		private List<FlagCapturePoint> _flagpositions;

		// Token: 0x0400038D RID: 909
		private bool _lastEffort;

		// Token: 0x0400038E RID: 910
		private MissionMultiplayerFlagDomination _flagDominationGameMode;
	}
}
