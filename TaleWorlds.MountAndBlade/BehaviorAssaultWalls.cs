using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000110 RID: 272
	public class BehaviorAssaultWalls : BehaviorComponent
	{
		// Token: 0x06000DD3 RID: 3539 RVA: 0x0001ACAC File Offset: 0x00018EAC
		private void ResetOrderPositions()
		{
			this._primarySiegeWeapons = this._teamAISiegeComponent.PrimarySiegeWeapons.ToList<IPrimarySiegeWeapon>();
			this._primarySiegeWeapons.RemoveAll(delegate(IPrimarySiegeWeapon uM)
			{
				SiegeWeapon siegeWeapon;
				IPrimarySiegeWeapon primarySiegeWeapon2;
				return uM.WeaponSide != this._behaviorSide || (siegeWeapon = uM as SiegeWeapon) == null || (siegeWeapon.IsDeactivated && !siegeWeapon.IsDestroyed && ((primarySiegeWeapon2 = siegeWeapon as IPrimarySiegeWeapon) == null || !primarySiegeWeapon2.HasCompletedAction()));
			});
			IEnumerable<ICastleKeyPosition> enumerable = TeamAISiegeComponent.SiegeLanes.Where<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide).SelectMany<SiegeLane, ICastleKeyPosition>((SiegeLane sila) => sila.DefensePoints);
			this._innerGate = this._teamAISiegeComponent.InnerGate;
			this._isGateLane = this._teamAISiegeComponent.OuterGate.DefenseSide == this._behaviorSide;
			if (this._isGateLane)
			{
				this._wallSegment = null;
			}
			else
			{
				WallSegment wallSegment = enumerable.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp is WallSegment && (dp as WallSegment).IsBreachedWall) as WallSegment;
				if (wallSegment != null)
				{
					this._wallSegment = wallSegment;
				}
				else
				{
					IPrimarySiegeWeapon primarySiegeWeapon = this._primarySiegeWeapons.MaxBy<IPrimarySiegeWeapon, float>((IPrimarySiegeWeapon psw) => psw.SiegeWeaponPriority);
					this._wallSegment = primarySiegeWeapon.TargetCastlePosition as WallSegment;
				}
			}
			this._stopOrder = MovementOrder.MovementOrderStop;
			this._chargeOrder = MovementOrder.MovementOrderCharge;
			bool flag = this._teamAISiegeComponent.OuterGate != null && this._behaviorSide == this._teamAISiegeComponent.OuterGate.DefenseSide;
			this._attackEntityOrderOuterGate = ((flag && !this._teamAISiegeComponent.OuterGate.IsDeactivated && this._teamAISiegeComponent.OuterGate.State != CastleGate.GateState.Open) ? MovementOrder.MovementOrderAttackEntity(GameEntity.CreateFromWeakEntity(this._teamAISiegeComponent.OuterGate.GameEntity), false) : MovementOrder.MovementOrderStop);
			this._attackEntityOrderInnerGate = ((flag && this._teamAISiegeComponent.InnerGate != null && !this._teamAISiegeComponent.InnerGate.IsDeactivated && this._teamAISiegeComponent.InnerGate.State != CastleGate.GateState.Open) ? MovementOrder.MovementOrderAttackEntity(GameEntity.CreateFromWeakEntity(this._teamAISiegeComponent.InnerGate.GameEntity), false) : MovementOrder.MovementOrderStop);
			WorldPosition origin = this._teamAISiegeComponent.OuterGate.MiddleFrame.Origin;
			this._castleGateMoveOrder = MovementOrder.MovementOrderMove(origin);
			if (this._isGateLane)
			{
				this._wallSegmentMoveOrder = this._castleGateMoveOrder;
			}
			else
			{
				WorldPosition origin2 = this._wallSegment.MiddleFrame.Origin;
				this._wallSegmentMoveOrder = MovementOrder.MovementOrderMove(origin2);
			}
			this._facingOrder = FacingOrder.FacingOrderLookAtEnemy;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0001AF24 File Offset: 0x00019124
		public BehaviorAssaultWalls(Formation formation)
			: base(formation)
		{
			base.BehaviorCoherence = 0f;
			this._behaviorSide = formation.AI.Side;
			this._teamAISiegeComponent = (TeamAISiegeComponent)formation.Team.TeamAI;
			this._behaviorState = BehaviorAssaultWalls.BehaviorState.Deciding;
			this.ResetOrderPositions();
			base.CurrentOrder = this._stopOrder;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0001AF84 File Offset: 0x00019184
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			return behaviorString;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0001AFE0 File Offset: 0x000191E0
		private BehaviorAssaultWalls.BehaviorState CheckAndChangeState()
		{
			switch (this._behaviorState)
			{
			case BehaviorAssaultWalls.BehaviorState.Deciding:
				if (!this._isGateLane && this._wallSegment == null)
				{
					return BehaviorAssaultWalls.BehaviorState.Charging;
				}
				if (!this._isGateLane)
				{
					return BehaviorAssaultWalls.BehaviorState.ClimbWall;
				}
				if (this._teamAISiegeComponent.OuterGate.IsGateOpen && this._teamAISiegeComponent.InnerGate.IsGateOpen)
				{
					return BehaviorAssaultWalls.BehaviorState.Charging;
				}
				return BehaviorAssaultWalls.BehaviorState.AttackEntity;
			case BehaviorAssaultWalls.BehaviorState.ClimbWall:
			{
				if (this._wallSegment == null)
				{
					return BehaviorAssaultWalls.BehaviorState.Charging;
				}
				bool flag = false;
				if (this._behaviorSide < FormationAI.BehaviorSide.BehaviorSideNotSet)
				{
					SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes[(int)this._behaviorSide];
					flag = siegeLane.IsUnderAttack() && !siegeLane.IsDefended();
				}
				flag = flag || base.Formation.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(this._wallSegment.MiddleFrame.Origin.GetNavMeshVec3()) < base.Formation.Depth * base.Formation.Depth;
				if (flag)
				{
					return BehaviorAssaultWalls.BehaviorState.TakeControl;
				}
				return BehaviorAssaultWalls.BehaviorState.ClimbWall;
			}
			case BehaviorAssaultWalls.BehaviorState.AttackEntity:
				if (this._teamAISiegeComponent.OuterGate.IsGateOpen && this._teamAISiegeComponent.InnerGate.IsGateOpen)
				{
					return BehaviorAssaultWalls.BehaviorState.Charging;
				}
				return BehaviorAssaultWalls.BehaviorState.AttackEntity;
			case BehaviorAssaultWalls.BehaviorState.TakeControl:
				if (base.Formation.CachedClosestEnemyFormation == null)
				{
					return BehaviorAssaultWalls.BehaviorState.Stop;
				}
				if (TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide).IsDefended())
				{
					return BehaviorAssaultWalls.BehaviorState.TakeControl;
				}
				if (!this._teamAISiegeComponent.OuterGate.IsGateOpen || !this._teamAISiegeComponent.InnerGate.IsGateOpen)
				{
					return BehaviorAssaultWalls.BehaviorState.MoveToGate;
				}
				return BehaviorAssaultWalls.BehaviorState.Charging;
			case BehaviorAssaultWalls.BehaviorState.MoveToGate:
				if (this._teamAISiegeComponent.OuterGate.IsGateOpen && this._teamAISiegeComponent.InnerGate.IsGateOpen)
				{
					return BehaviorAssaultWalls.BehaviorState.Charging;
				}
				return BehaviorAssaultWalls.BehaviorState.MoveToGate;
			case BehaviorAssaultWalls.BehaviorState.Charging:
				if ((!this._isGateLane || !this._teamAISiegeComponent.OuterGate.IsGateOpen || !this._teamAISiegeComponent.InnerGate.IsGateOpen) && this._behaviorSide < FormationAI.BehaviorSide.BehaviorSideNotSet)
				{
					if (!TeamAISiegeComponent.SiegeLanes[(int)this._behaviorSide].IsOpen && !TeamAISiegeComponent.IsFormationInsideCastle(base.Formation, true, 0.4f))
					{
						return BehaviorAssaultWalls.BehaviorState.Deciding;
					}
					if (base.Formation.CachedClosestEnemyFormation == null)
					{
						return BehaviorAssaultWalls.BehaviorState.Stop;
					}
				}
				return BehaviorAssaultWalls.BehaviorState.Charging;
			default:
				if (base.Formation.CachedClosestEnemyFormation != null)
				{
					return BehaviorAssaultWalls.BehaviorState.Deciding;
				}
				return BehaviorAssaultWalls.BehaviorState.Stop;
			}
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0001B21C File Offset: 0x0001941C
		protected override void CalculateCurrentOrder()
		{
			switch (this._behaviorState)
			{
			case BehaviorAssaultWalls.BehaviorState.Deciding:
				base.CurrentOrder = this._stopOrder;
				return;
			case BehaviorAssaultWalls.BehaviorState.ClimbWall:
			{
				base.CurrentOrder = this._wallSegmentMoveOrder;
				WorldFrame worldFrame = this._wallSegment.MiddleFrame;
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(-worldFrame.Rotation.f.AsVec2.Normalized());
				this.CurrentArrangementOrder = ArrangementOrder.ArrangementOrderLine;
				return;
			}
			case BehaviorAssaultWalls.BehaviorState.AttackEntity:
				base.CurrentOrder = ((!this._teamAISiegeComponent.OuterGate.IsGateOpen) ? this._attackEntityOrderOuterGate : this._attackEntityOrderInnerGate);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				this.CurrentArrangementOrder = ArrangementOrder.ArrangementOrderLine;
				return;
			case BehaviorAssaultWalls.BehaviorState.TakeControl:
			{
				base.CurrentOrder = ((base.Formation.CachedClosestEnemyFormation != null) ? MovementOrder.MovementOrderChargeToTarget(base.Formation.CachedClosestEnemyFormation.Formation) : MovementOrder.MovementOrderCharge);
				WorldFrame worldFrame = this._wallSegment.MiddleFrame;
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(-worldFrame.Rotation.f.AsVec2.Normalized());
				this.CurrentArrangementOrder = ArrangementOrder.ArrangementOrderLine;
				return;
			}
			case BehaviorAssaultWalls.BehaviorState.MoveToGate:
			{
				base.CurrentOrder = this._castleGateMoveOrder;
				WorldFrame worldFrame = this._innerGate.MiddleFrame;
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(-worldFrame.Rotation.f.AsVec2.Normalized());
				this.CurrentArrangementOrder = ArrangementOrder.ArrangementOrderLine;
				return;
			}
			case BehaviorAssaultWalls.BehaviorState.Charging:
				base.CurrentOrder = this._chargeOrder;
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				this.CurrentArrangementOrder = ArrangementOrder.ArrangementOrderLoose;
				return;
			case BehaviorAssaultWalls.BehaviorState.Stop:
				base.CurrentOrder = this._chargeOrder;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0001B3D6 File Offset: 0x000195D6
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this.ResetOrderPositions();
			this._behaviorState = BehaviorAssaultWalls.BehaviorState.Deciding;
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0001B3EC File Offset: 0x000195EC
		public override void TickOccasionally()
		{
			BehaviorAssaultWalls.BehaviorState behaviorState = this.CheckAndChangeState();
			this._behaviorState = behaviorState;
			this.CalculateCurrentOrder();
			foreach (IPrimarySiegeWeapon primarySiegeWeapon in this._primarySiegeWeapons)
			{
				UsableMachine usableMachine = primarySiegeWeapon as UsableMachine;
				if (!usableMachine.IsDeactivated && !primarySiegeWeapon.HasCompletedAction() && !usableMachine.IsUsedByFormation(base.Formation))
				{
					base.Formation.StartUsingMachine(primarySiegeWeapon as UsableMachine, false);
				}
			}
			if (this._behaviorState == BehaviorAssaultWalls.BehaviorState.MoveToGate || this._behaviorState == BehaviorAssaultWalls.BehaviorState.Stop || this._behaviorState == BehaviorAssaultWalls.BehaviorState.Charging || this._behaviorState == BehaviorAssaultWalls.BehaviorState.TakeControl)
			{
				CastleGate castleGate = this._teamAISiegeComponent.InnerGate;
				if (castleGate != null && !castleGate.IsGateOpen && !castleGate.IsDestroyed)
				{
					if (!castleGate.IsUsedByFormation(base.Formation))
					{
						base.Formation.StartUsingMachine(castleGate, false);
					}
				}
				else
				{
					castleGate = this._teamAISiegeComponent.OuterGate;
					if (castleGate != null && !castleGate.IsGateOpen && !castleGate.IsDestroyed && !castleGate.IsUsedByFormation(base.Formation))
					{
						base.Formation.StartUsingMachine(castleGate, false);
					}
				}
			}
			else
			{
				if (base.Formation.Detachments.Contains(this._teamAISiegeComponent.OuterGate))
				{
					base.Formation.StopUsingMachine(this._teamAISiegeComponent.OuterGate, false);
				}
				if (base.Formation.Detachments.Contains(this._teamAISiegeComponent.InnerGate))
				{
					base.Formation.StopUsingMachine(this._teamAISiegeComponent.InnerGate, false);
				}
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(this.CurrentArrangementOrder);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0001B5D8 File Offset: 0x000197D8
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x0001B63E File Offset: 0x0001983E
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0001B648 File Offset: 0x00019848
		protected override float GetAiWeight()
		{
			float num = 0f;
			if (this._teamAISiegeComponent != null)
			{
				if (this._primarySiegeWeapons.Any<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => psw.HasCompletedAction()) || this._wallSegment != null)
				{
					if (this._teamAISiegeComponent.IsCastleBreached())
					{
						num = 0.75f;
					}
					else
					{
						num = 0.25f;
					}
				}
				else if (this._teamAISiegeComponent.OuterGate.DefenseSide == this._behaviorSide)
				{
					num = 0.1f;
				}
			}
			return num;
		}

		// Token: 0x04000322 RID: 802
		private BehaviorAssaultWalls.BehaviorState _behaviorState;

		// Token: 0x04000323 RID: 803
		private List<IPrimarySiegeWeapon> _primarySiegeWeapons;

		// Token: 0x04000324 RID: 804
		private WallSegment _wallSegment;

		// Token: 0x04000325 RID: 805
		private CastleGate _innerGate;

		// Token: 0x04000326 RID: 806
		private TeamAISiegeComponent _teamAISiegeComponent;

		// Token: 0x04000327 RID: 807
		private MovementOrder _attackEntityOrderInnerGate;

		// Token: 0x04000328 RID: 808
		private MovementOrder _attackEntityOrderOuterGate;

		// Token: 0x04000329 RID: 809
		private MovementOrder _chargeOrder;

		// Token: 0x0400032A RID: 810
		private MovementOrder _stopOrder;

		// Token: 0x0400032B RID: 811
		private MovementOrder _castleGateMoveOrder;

		// Token: 0x0400032C RID: 812
		private MovementOrder _wallSegmentMoveOrder;

		// Token: 0x0400032D RID: 813
		private FacingOrder _facingOrder;

		// Token: 0x0400032E RID: 814
		protected ArrangementOrder CurrentArrangementOrder;

		// Token: 0x0400032F RID: 815
		private bool _isGateLane;

		// Token: 0x02000438 RID: 1080
		private enum BehaviorState
		{
			// Token: 0x040019B1 RID: 6577
			Deciding,
			// Token: 0x040019B2 RID: 6578
			ClimbWall,
			// Token: 0x040019B3 RID: 6579
			AttackEntity,
			// Token: 0x040019B4 RID: 6580
			TakeControl,
			// Token: 0x040019B5 RID: 6581
			MoveToGate,
			// Token: 0x040019B6 RID: 6582
			Charging,
			// Token: 0x040019B7 RID: 6583
			Stop
		}
	}
}
