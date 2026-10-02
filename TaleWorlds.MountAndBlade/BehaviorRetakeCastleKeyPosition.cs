using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000127 RID: 295
	public class BehaviorRetakeCastleKeyPosition : BehaviorComponent
	{
		// Token: 0x06000E93 RID: 3731 RVA: 0x0002251D File Offset: 0x0002071D
		public BehaviorRetakeCastleKeyPosition(Formation formation)
			: base(formation)
		{
			this._behaviorState = BehaviorRetakeCastleKeyPosition.BehaviorState.UnSet;
			this._behaviorSide = formation.AI.Side;
			this.ResetOrderPositions();
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00022544 File Offset: 0x00020744
		protected override void CalculateCurrentOrder()
		{
			base.CalculateCurrentOrder();
			base.CurrentOrder = ((this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0002256C File Offset: 0x0002076C
		private FormationAI.BehaviorSide DetermineGatheringSide()
		{
			IEnumerable<SiegeLane> enumerable = TeamAISiegeComponent.SiegeLanes.Where<SiegeLane>((SiegeLane sl) => sl.LaneSide != this._behaviorSide && sl.LaneState != SiegeLane.LaneStateEnum.Conceited && sl.DefenderOrigin.IsValid);
			if (enumerable.Any<SiegeLane>())
			{
				int nearestSafeSideDistance = enumerable.Min<SiegeLane>((SiegeLane pgl) => SiegeQuerySystem.SideDistance(1 << (int)this._behaviorSide, 1 << (int)pgl.LaneSide));
				return enumerable.Where<SiegeLane>((SiegeLane pgl) => SiegeQuerySystem.SideDistance(1 << (int)this._behaviorSide, 1 << (int)pgl.LaneSide) == nearestSafeSideDistance).MinBy<SiegeLane, float>((SiegeLane pgl) => pgl.DefenderOrigin.GetGroundVec3().DistanceSquared(base.Formation.CachedMedianPosition.GetGroundVec3())).LaneSide;
			}
			return this._behaviorSide;
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x000225EC File Offset: 0x000207EC
		private void ConfirmGatheringSide()
		{
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._gatheringSide);
			if (siegeLane == null || siegeLane.LaneState >= SiegeLane.LaneStateEnum.Conceited)
			{
				this.ResetOrderPositions();
			}
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00022624 File Offset: 0x00020824
		private void ResetOrderPositions()
		{
			this._behaviorState = BehaviorRetakeCastleKeyPosition.BehaviorState.UnSet;
			this._gatheringSide = this.DetermineGatheringSide();
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._gatheringSide);
			ICastleKeyPosition castleKeyPosition = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
			WorldFrame worldFrame = ((castleKeyPosition != null) ? castleKeyPosition.DefenseWaitFrame : siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>().DefenseWaitFrame);
			ICastleKeyPosition castleKeyPosition2 = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
			this._gatheringTacticalPos = ((castleKeyPosition2 != null) ? castleKeyPosition2.WaitPosition : null);
			if (this._gatheringTacticalPos != null)
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(this._gatheringTacticalPos.Position);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			else if (worldFrame.Origin.IsValid)
			{
				worldFrame.Rotation.f.Normalize();
				this._gatherOrder = MovementOrder.MovementOrderMove(worldFrame.Origin);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			else
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(base.Formation.CachedMedianPosition);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			SiegeLane siegeLane2 = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide);
			ICastleKeyPosition castleKeyPosition3 = siegeLane2.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
			this._attackOrder = MovementOrder.MovementOrderMove((castleKeyPosition3 != null) ? castleKeyPosition3.MiddleFrame.Origin : siegeLane2.DefensePoints.FirstOrDefault<ICastleKeyPosition>().MiddleFrame.Origin);
			this._attackFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			base.CurrentOrder = ((this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
			this.CurrentFacingOrder = ((this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking) ? this._attackFacingOrder : this._gatheringFacingOrder);
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0002281D File Offset: 0x00020A1D
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this.ResetOrderPositions();
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0002282C File Offset: 0x00020A2C
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (this._behaviorState != BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking)
			{
				this.ConfirmGatheringSide();
			}
			bool flag = true;
			if (this._behaviorState != BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking)
			{
				flag = base.Formation.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(this._gatherOrder.CreateNewOrderWorldPositionMT(base.Formation, WorldPosition.WorldPositionEnforcedCache.NavMeshVec3).GetNavMeshVec3()) < 100f || base.Formation.CachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents / ((base.Formation.QuerySystem.IdealAverageDisplacement != 0f) ? base.Formation.QuerySystem.IdealAverageDisplacement : 1f) <= 3f;
			}
			BehaviorRetakeCastleKeyPosition.BehaviorState behaviorState = (flag ? BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking : BehaviorRetakeCastleKeyPosition.BehaviorState.Gathering);
			if (behaviorState != this._behaviorState)
			{
				this._behaviorState = behaviorState;
				base.CurrentOrder = ((this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
				this.CurrentFacingOrder = ((this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking) ? this._attackFacingOrder : this._gatheringFacingOrder);
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Gathering && this._gatheringTacticalPos != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._gatheringTacticalPos.Width), true);
			}
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00022988 File Offset: 0x00020B88
		protected override void OnBehaviorActivatedAux()
		{
			this._behaviorState = BehaviorRetakeCastleKeyPosition.BehaviorState.UnSet;
			this._behaviorSide = base.Formation.AI.Side;
			this.ResetOrderPositions();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x00022A0B File Offset: 0x00020C0B
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00022A12 File Offset: 0x00020C12
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x0400037C RID: 892
		private BehaviorRetakeCastleKeyPosition.BehaviorState _behaviorState;

		// Token: 0x0400037D RID: 893
		private MovementOrder _gatherOrder;

		// Token: 0x0400037E RID: 894
		private MovementOrder _attackOrder;

		// Token: 0x0400037F RID: 895
		private FacingOrder _gatheringFacingOrder;

		// Token: 0x04000380 RID: 896
		private FacingOrder _attackFacingOrder;

		// Token: 0x04000381 RID: 897
		private TacticalPosition _gatheringTacticalPos;

		// Token: 0x04000382 RID: 898
		private FormationAI.BehaviorSide _gatheringSide;

		// Token: 0x0200044A RID: 1098
		private enum BehaviorState
		{
			// Token: 0x040019F0 RID: 6640
			UnSet,
			// Token: 0x040019F1 RID: 6641
			Gathering,
			// Token: 0x040019F2 RID: 6642
			Attacking
		}
	}
}
