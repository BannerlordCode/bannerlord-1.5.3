using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200011B RID: 283
	public class BehaviorEliminateEnemyInsideCastle : BehaviorComponent
	{
		// Token: 0x06000E4A RID: 3658 RVA: 0x0001F1DA File Offset: 0x0001D3DA
		public BehaviorEliminateEnemyInsideCastle(Formation formation)
			: base(formation)
		{
			this._behaviorState = BehaviorEliminateEnemyInsideCastle.BehaviorState.UnSet;
			this._behaviorSide = formation.AI.Side;
			this.ResetOrderPositions();
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0001F201 File Offset: 0x0001D401
		protected override void CalculateCurrentOrder()
		{
			base.CalculateCurrentOrder();
			base.CurrentOrder = ((this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0001F228 File Offset: 0x0001D428
		private void DetermineMostImportantInvadingEnemyFormation()
		{
			float num = float.MinValue;
			this._targetEnemyFormation = null;
			foreach (Team team in base.Formation.Team.Mission.Teams)
			{
				if (team.IsEnemyOf(base.Formation.Team))
				{
					for (int i = 0; i < Math.Min(team.FormationsIncludingSpecialAndEmpty.Count, 8); i++)
					{
						Formation formation = team.FormationsIncludingSpecialAndEmpty[i];
						if (formation.CountOfUnits > 0 && TeamAISiegeComponent.IsFormationInsideCastle(formation, true, 0.4f))
						{
							float formationPower = formation.QuerySystem.FormationPower;
							if (formationPower > num)
							{
								num = formationPower;
								this._targetEnemyFormation = formation;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0001F308 File Offset: 0x0001D508
		private void ConfirmGatheringSide()
		{
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide);
			if (siegeLane == null || siegeLane.LaneState >= SiegeLane.LaneStateEnum.Conceited)
			{
				this.ResetOrderPositions();
			}
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0001F340 File Offset: 0x0001D540
		private FormationAI.BehaviorSide DetermineGatheringSide()
		{
			this.DetermineMostImportantInvadingEnemyFormation();
			if (this._targetEnemyFormation == null)
			{
				if (this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking)
				{
					this._behaviorState = BehaviorEliminateEnemyInsideCastle.BehaviorState.UnSet;
				}
				return this._behaviorSide;
			}
			int connectedSides = TeamAISiegeComponent.QuerySystem.DeterminePositionAssociatedSide(this._targetEnemyFormation.CachedMedianPosition.GetNavMeshVec3());
			IEnumerable<SiegeLane> enumerable = TeamAISiegeComponent.SiegeLanes.Where<SiegeLane>((SiegeLane sl) => sl.LaneState != SiegeLane.LaneStateEnum.Conceited && !SiegeQuerySystem.AreSidesRelated(sl.LaneSide, connectedSides));
			FormationAI.BehaviorSide behaviorSide = this._behaviorSide;
			if (enumerable.Any<SiegeLane>())
			{
				if (enumerable.Count<SiegeLane>() > 1)
				{
					int leastDangerousLaneState = enumerable.Min<SiegeLane>((SiegeLane pgl) => (int)pgl.LaneState);
					IEnumerable<SiegeLane> enumerable2 = enumerable.Where<SiegeLane>((SiegeLane pgl) => pgl.LaneState == (SiegeLane.LaneStateEnum)leastDangerousLaneState);
					behaviorSide = ((enumerable2.Count<SiegeLane>() > 1) ? enumerable2.MinBy<SiegeLane, int>((SiegeLane ldl) => SiegeQuerySystem.SideDistance(1 << connectedSides, 1 << (int)ldl.LaneSide)).LaneSide : enumerable2.First<SiegeLane>().LaneSide);
				}
				else
				{
					behaviorSide = enumerable.First<SiegeLane>().LaneSide;
				}
			}
			return behaviorSide;
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0001F458 File Offset: 0x0001D658
		private void ResetOrderPositions()
		{
			this._behaviorSide = this.DetermineGatheringSide();
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide);
			WorldFrame? worldFrame;
			if (siegeLane == null)
			{
				worldFrame = null;
			}
			else
			{
				List<ICastleKeyPosition> defensePoints = siegeLane.DefensePoints;
				if (defensePoints == null)
				{
					worldFrame = null;
				}
				else
				{
					ICastleKeyPosition castleKeyPosition = defensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
					worldFrame = ((castleKeyPosition != null) ? new WorldFrame?(castleKeyPosition.DefenseWaitFrame) : null);
				}
			}
			WorldFrame worldFrame2 = worldFrame ?? WorldFrame.Invalid;
			object obj;
			if (siegeLane == null)
			{
				obj = null;
			}
			else
			{
				List<ICastleKeyPosition> defensePoints2 = siegeLane.DefensePoints;
				if (defensePoints2 == null)
				{
					obj = null;
				}
				else
				{
					ICastleKeyPosition castleKeyPosition2 = defensePoints2.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
					obj = ((castleKeyPosition2 != null) ? castleKeyPosition2.WaitPosition : null);
				}
			}
			object obj2;
			if ((obj2 = obj) == null)
			{
				if (siegeLane == null)
				{
					obj2 = null;
				}
				else
				{
					List<ICastleKeyPosition> defensePoints3 = siegeLane.DefensePoints;
					obj2 = ((defensePoints3 != null) ? defensePoints3.FirstOrDefault<ICastleKeyPosition>().WaitPosition : null);
				}
			}
			this._gatheringTacticalPos = obj2;
			if (this._gatheringTacticalPos != null)
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(this._gatheringTacticalPos.Position);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			else if (worldFrame2.Origin.IsValid)
			{
				worldFrame2.Rotation.f.Normalize();
				this._gatherOrder = MovementOrder.MovementOrderMove(worldFrame2.Origin);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			else
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(base.Formation.CachedMedianPosition);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			this._attackOrder = MovementOrder.MovementOrderChargeToTarget(this._targetEnemyFormation);
			this._attackFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			base.CurrentOrder = ((this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
			this.CurrentFacingOrder = ((this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking) ? this._attackFacingOrder : this._gatheringFacingOrder);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0001F64B File Offset: 0x0001D84B
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this.ResetOrderPositions();
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0001F65C File Offset: 0x0001D85C
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (this._behaviorState != BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking)
			{
				this.ConfirmGatheringSide();
			}
			bool flag;
			if (this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking)
			{
				flag = this._targetEnemyFormation != null;
			}
			else
			{
				flag = this._targetEnemyFormation != null && (base.Formation.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(this._gatherOrder.CreateNewOrderWorldPositionMT(base.Formation, WorldPosition.WorldPositionEnforcedCache.NavMeshVec3).GetNavMeshVec3()) < 100f || base.Formation.CachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents / ((base.Formation.QuerySystem.IdealAverageDisplacement != 0f) ? base.Formation.QuerySystem.IdealAverageDisplacement : 1f) <= 3f);
			}
			BehaviorEliminateEnemyInsideCastle.BehaviorState behaviorState = (flag ? BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking : BehaviorEliminateEnemyInsideCastle.BehaviorState.Gathering);
			if (behaviorState != this._behaviorState)
			{
				this._behaviorState = behaviorState;
				base.CurrentOrder = ((this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
				this.CurrentFacingOrder = ((this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking) ? this._attackFacingOrder : this._gatheringFacingOrder);
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Gathering && this._gatheringTacticalPos != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._gatheringTacticalPos.Width), true);
			}
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0001F7D0 File Offset: 0x0001D9D0
		protected override void OnBehaviorActivatedAux()
		{
			this._behaviorState = BehaviorEliminateEnemyInsideCastle.BehaviorState.UnSet;
			this._behaviorSide = base.Formation.AI.Side;
			this.ResetOrderPositions();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0001F853 File Offset: 0x0001DA53
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0001F85A File Offset: 0x0001DA5A
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x04000366 RID: 870
		private BehaviorEliminateEnemyInsideCastle.BehaviorState _behaviorState;

		// Token: 0x04000367 RID: 871
		private MovementOrder _gatherOrder;

		// Token: 0x04000368 RID: 872
		private MovementOrder _attackOrder;

		// Token: 0x04000369 RID: 873
		private FacingOrder _gatheringFacingOrder;

		// Token: 0x0400036A RID: 874
		private FacingOrder _attackFacingOrder;

		// Token: 0x0400036B RID: 875
		private TacticalPosition _gatheringTacticalPos;

		// Token: 0x0400036C RID: 876
		private Formation _targetEnemyFormation;

		// Token: 0x02000441 RID: 1089
		private enum BehaviorState
		{
			// Token: 0x040019D4 RID: 6612
			UnSet,
			// Token: 0x040019D5 RID: 6613
			Gathering,
			// Token: 0x040019D6 RID: 6614
			Attacking
		}
	}
}
