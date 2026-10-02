using System;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200012B RID: 299
	public class BehaviorSallyOut : BehaviorComponent
	{
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x00022C80 File Offset: 0x00020E80
		private bool _calculateAreGatesOutsideOpen
		{
			get
			{
				return (this._teamAISiegeDefender.OuterGate == null || this._teamAISiegeDefender.OuterGate.IsGateOpen) && (this._teamAISiegeDefender.InnerGate == null || this._teamAISiegeDefender.InnerGate.IsGateOpen);
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x00022CCD File Offset: 0x00020ECD
		private bool _calculateShouldStartAttacking
		{
			get
			{
				return this._calculateAreGatesOutsideOpen || !TeamAISiegeComponent.IsFormationInsideCastle(base.Formation, true, 0.4f);
			}
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00022CED File Offset: 0x00020EED
		public BehaviorSallyOut(Formation formation)
			: base(formation)
		{
			this._teamAISiegeDefender = formation.Team.TeamAI as TeamAISiegeDefender;
			this._behaviorSide = formation.AI.Side;
			this.ResetOrderPositions();
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00022D23 File Offset: 0x00020F23
		protected override void CalculateCurrentOrder()
		{
			base.CalculateCurrentOrder();
			base.CurrentOrder = (this._calculateShouldStartAttacking ? this._attackOrder : this._gatherOrder);
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00022D48 File Offset: 0x00020F48
		private void ResetOrderPositions()
		{
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == FormationAI.BehaviorSide.Middle);
			WorldFrame? worldFrame;
			if (siegeLane == null)
			{
				worldFrame = null;
			}
			else
			{
				ICastleKeyPosition castleKeyPosition = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
				worldFrame = ((castleKeyPosition != null) ? new WorldFrame?(castleKeyPosition.DefenseWaitFrame) : null);
			}
			WorldFrame worldFrame2 = worldFrame ?? WorldFrame.Invalid;
			TacticalPosition tacticalPosition;
			if (siegeLane == null)
			{
				tacticalPosition = null;
			}
			else
			{
				ICastleKeyPosition castleKeyPosition2 = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
				tacticalPosition = ((castleKeyPosition2 != null) ? castleKeyPosition2.WaitPosition : null);
			}
			this._gatheringTacticalPos = tacticalPosition;
			if (this._gatheringTacticalPos != null)
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(this._gatheringTacticalPos.Position);
			}
			else if (worldFrame2.Origin.IsValid)
			{
				worldFrame2.Rotation.f.Normalize();
				this._gatherOrder = MovementOrder.MovementOrderMove(worldFrame2.Origin);
			}
			else
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(base.Formation.CachedMedianPosition);
			}
			this._attackOrder = MovementOrder.MovementOrderCharge;
			base.CurrentOrder = (this._calculateShouldStartAttacking ? this._attackOrder : this._gatherOrder);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00022EBC File Offset: 0x000210BC
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			if (!this._calculateAreGatesOutsideOpen)
			{
				CastleGate castleGate = ((this._teamAISiegeDefender.InnerGate != null && !this._teamAISiegeDefender.InnerGate.IsGateOpen) ? this._teamAISiegeDefender.InnerGate : this._teamAISiegeDefender.OuterGate);
				if (!castleGate.IsUsedByFormation(base.Formation))
				{
					base.Formation.StartUsingMachine(castleGate, false);
				}
			}
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00022F44 File Offset: 0x00021144
		protected override void OnBehaviorActivatedAux()
		{
			this._behaviorSide = base.Formation.AI.Side;
			this.ResetOrderPositions();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x00022FBF File Offset: 0x000211BF
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00022FC6 File Offset: 0x000211C6
		protected override float GetAiWeight()
		{
			return 10f;
		}

		// Token: 0x04000383 RID: 899
		private readonly TeamAISiegeDefender _teamAISiegeDefender;

		// Token: 0x04000384 RID: 900
		private MovementOrder _gatherOrder;

		// Token: 0x04000385 RID: 901
		private MovementOrder _attackOrder;

		// Token: 0x04000386 RID: 902
		private TacticalPosition _gatheringTacticalPos;
	}
}
