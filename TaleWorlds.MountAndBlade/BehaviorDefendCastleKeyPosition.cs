using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000116 RID: 278
	public class BehaviorDefendCastleKeyPosition : BehaviorComponent
	{
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x0001D8EB File Offset: 0x0001BAEB
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0001D8F2 File Offset: 0x0001BAF2
		public BehaviorDefendCastleKeyPosition(Formation formation)
			: base(formation)
		{
			this._teamAISiegeDefender = formation.Team.TeamAI as TeamAISiegeComponent;
			this._behaviorState = BehaviorDefendCastleKeyPosition.BehaviorState.UnSet;
			this._laddersOnThisSide = new List<SiegeLadder>();
			this.ResetOrderPositions();
			this._hasFormedShieldWall = true;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0001D930 File Offset: 0x0001BB30
		protected override void CalculateCurrentOrder()
		{
			base.CalculateCurrentOrder();
			base.CurrentOrder = ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyOrder : this._waitOrder);
			this.CurrentFacingOrder = ((base.Formation.CachedClosestEnemyFormation != null && TeamAISiegeComponent.IsFormationInsideCastle(base.Formation.CachedClosestEnemyFormation.Formation, true, 0.4f)) ? FacingOrder.FacingOrderLookAtEnemy : ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyFacingOrder : this._waitFacingOrder));
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0001D9B0 File Offset: 0x0001BBB0
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			return behaviorString;
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0001DA0C File Offset: 0x0001BC0C
		private void ResetOrderPositions()
		{
			this._behaviorSide = base.Formation.AI.Side;
			this._innerGate = null;
			this._outerGate = null;
			this._laddersOnThisSide.Clear();
			WorldFrame worldFrame;
			WorldFrame worldFrame2;
			if (this._teamAISiegeDefender.OuterGate.DefenseSide == this._behaviorSide)
			{
				CastleGate outerGate = this._teamAISiegeDefender.OuterGate;
				this._innerGate = this._teamAISiegeDefender.InnerGate;
				this._outerGate = this._teamAISiegeDefender.OuterGate;
				worldFrame = outerGate.MiddleFrame;
				worldFrame2 = outerGate.DefenseWaitFrame;
				this._tacticalMiddlePos = outerGate.MiddlePosition;
				this._tacticalWaitPos = outerGate.WaitPosition;
			}
			else
			{
				WallSegment wallSegment = this._teamAISiegeDefender.WallSegments.Where<WallSegment>((WallSegment ws) => ws.DefenseSide == this._behaviorSide && ws.IsBreachedWall).FirstOrDefault<WallSegment>();
				if (wallSegment != null)
				{
					worldFrame = wallSegment.MiddleFrame;
					worldFrame2 = wallSegment.DefenseWaitFrame;
					this._tacticalMiddlePos = wallSegment.MiddlePosition;
					this._tacticalWaitPos = wallSegment.WaitPosition;
				}
				else
				{
					IEnumerable<IPrimarySiegeWeapon> enumerable = this._teamAISiegeDefender.PrimarySiegeWeapons.Where<IPrimarySiegeWeapon>(delegate(IPrimarySiegeWeapon sw)
					{
						SiegeWeapon siegeWeapon;
						return sw.WeaponSide == this._behaviorSide && (((siegeWeapon = sw as SiegeWeapon) != null && !siegeWeapon.IsDestroyed && !siegeWeapon.IsDeactivated) || sw.HasCompletedAction());
					});
					if (!enumerable.Any<IPrimarySiegeWeapon>())
					{
						worldFrame = WorldFrame.Invalid;
						worldFrame2 = WorldFrame.Invalid;
						this._tacticalMiddlePos = null;
						this._tacticalWaitPos = null;
					}
					else
					{
						this._laddersOnThisSide = enumerable.OfType<SiegeLadder>().ToList<SiegeLadder>();
						ICastleKeyPosition castleKeyPosition = enumerable.FirstOrDefault<IPrimarySiegeWeapon>().TargetCastlePosition as ICastleKeyPosition;
						worldFrame = castleKeyPosition.MiddleFrame;
						worldFrame2 = castleKeyPosition.DefenseWaitFrame;
						this._tacticalMiddlePos = castleKeyPosition.MiddlePosition;
						this._tacticalWaitPos = castleKeyPosition.WaitPosition;
					}
				}
			}
			if (this._tacticalMiddlePos != null)
			{
				this._readyOrderPosition = this._tacticalMiddlePos.Position;
				this._readyOrder = MovementOrder.MovementOrderMove(this._readyOrderPosition);
				this._readyFacingOrder = FacingOrder.FacingOrderLookAtDirection(this._tacticalMiddlePos.Direction);
			}
			else if (worldFrame.Origin.IsValid)
			{
				worldFrame.Rotation.f.Normalize();
				this._readyOrderPosition = worldFrame.Origin;
				this._readyOrder = MovementOrder.MovementOrderMove(this._readyOrderPosition);
				this._readyFacingOrder = FacingOrder.FacingOrderLookAtDirection(worldFrame.Rotation.f.AsVec2);
			}
			else
			{
				this._readyOrderPosition = WorldPosition.Invalid;
				this._readyOrder = MovementOrder.MovementOrderStop;
				this._readyFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			if (this._tacticalWaitPos != null)
			{
				this._waitOrder = MovementOrder.MovementOrderMove(this._tacticalWaitPos.Position);
				this._waitFacingOrder = FacingOrder.FacingOrderLookAtDirection(this._tacticalWaitPos.Direction);
			}
			else if (worldFrame2.Origin.IsValid)
			{
				worldFrame2.Rotation.f.Normalize();
				this._waitOrder = MovementOrder.MovementOrderMove(worldFrame2.Origin);
				this._waitFacingOrder = FacingOrder.FacingOrderLookAtDirection(worldFrame2.Rotation.f.AsVec2);
			}
			else
			{
				this._waitOrder = MovementOrder.MovementOrderStop;
				this._waitFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			base.CurrentOrder = ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyOrder : this._waitOrder);
			this.CurrentFacingOrder = ((base.Formation.CachedClosestEnemyFormation != null && TeamAISiegeComponent.IsFormationInsideCastle(base.Formation.CachedClosestEnemyFormation.Formation, true, 0.4f)) ? FacingOrder.FacingOrderLookAtEnemy : ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyFacingOrder : this._waitFacingOrder));
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0001DD61 File Offset: 0x0001BF61
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this.ResetOrderPositions();
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0001DD70 File Offset: 0x0001BF70
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			bool flag = false;
			if (this._teamAISiegeDefender != null && !base.Formation.IsDeployment)
			{
				for (int i = 0; i < TeamAISiegeComponent.SiegeLanes.Count; i++)
				{
					SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes[i];
					if (siegeLane.LaneSide == this._behaviorSide)
					{
						if (siegeLane.IsOpen)
						{
							flag = true;
						}
						else
						{
							for (int j = 0; j < siegeLane.PrimarySiegeWeapons.Count; j++)
							{
								IPrimarySiegeWeapon primarySiegeWeapon = siegeLane.PrimarySiegeWeapons[j];
								SiegeLadder siegeLadder;
								if ((siegeLadder = primarySiegeWeapon as SiegeLadder) != null)
								{
									if (siegeLadder.IsUsed)
									{
										flag = true;
										break;
									}
								}
								else if ((primarySiegeWeapon as SiegeWeapon).GetComponent<SiegeWeaponMovementComponent>().HasApproachedTarget)
								{
									flag = true;
									break;
								}
							}
						}
					}
				}
			}
			BehaviorDefendCastleKeyPosition.BehaviorState behaviorState = (flag ? BehaviorDefendCastleKeyPosition.BehaviorState.Ready : BehaviorDefendCastleKeyPosition.BehaviorState.Waiting);
			bool flag2 = false;
			if (behaviorState != this._behaviorState)
			{
				this._behaviorState = behaviorState;
				base.CurrentOrder = ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyOrder : this._waitOrder);
				this.CurrentFacingOrder = ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyFacingOrder : this._waitFacingOrder);
				flag2 = true;
			}
			if (Mission.Current.MissionTeamAIType == Mission.MissionTeamAITypeEnum.Siege)
			{
				if (this._outerGate != null && this._outerGate.State == CastleGate.GateState.Open && !this._outerGate.IsDestroyed)
				{
					if (!this._outerGate.IsUsedByFormation(base.Formation))
					{
						base.Formation.StartUsingMachine(this._outerGate, false);
					}
				}
				else if (this._innerGate != null && this._innerGate.State == CastleGate.GateState.Open && !this._innerGate.IsDestroyed && !this._innerGate.IsUsedByFormation(base.Formation))
				{
					base.Formation.StartUsingMachine(this._innerGate, false);
				}
				foreach (SiegeLadder siegeLadder2 in this._laddersOnThisSide)
				{
					if (!siegeLadder2.IsDisabledForBattleSide(BattleSideEnum.Defender) && !siegeLadder2.IsUsedByFormation(base.Formation))
					{
						base.Formation.StartUsingMachine(siegeLadder2, false);
					}
				}
			}
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready && this._tacticalMiddlePos != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._tacticalMiddlePos.Width), true);
			}
			else if (this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Waiting && this._tacticalWaitPos != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._tacticalWaitPos.Width), true);
			}
			if (flag2 || !this._hasFormedShieldWall)
			{
				bool flag3;
				if (this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready && this._readyOrderPosition.IsValid)
				{
					Vec3 navMeshVec = base.Formation.CachedMedianPosition.GetNavMeshVec3();
					flag3 = this._readyOrderPosition.DistanceSquaredWithLimit(in navMeshVec, MathF.Min(base.Formation.Depth, base.Formation.Width) * 1.2f) <= (this._hasFormedShieldWall ? (MathF.Min(base.Formation.Depth, base.Formation.Width) * MathF.Min(base.Formation.Depth, base.Formation.Width)) : (MathF.Min(base.Formation.Depth, base.Formation.Width) * MathF.Min(base.Formation.Depth, base.Formation.Width) * 0.25f));
				}
				else
				{
					flag3 = true;
				}
				bool flag4 = flag3;
				if (flag4 != this._hasFormedShieldWall)
				{
					this._hasFormedShieldWall = flag4;
					base.Formation.SetArrangementOrder(this._hasFormedShieldWall ? ArrangementOrder.ArrangementOrderShieldWall : ArrangementOrder.ArrangementOrderLine);
				}
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0001E148 File Offset: 0x0001C348
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			this.ResetOrderPositions();
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0001E156 File Offset: 0x0001C356
		public override void ResetBehavior()
		{
			base.ResetBehavior();
			this.ResetOrderPositions();
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0001E164 File Offset: 0x0001C364
		protected override void OnBehaviorActivatedAux()
		{
			this.ResetOrderPositions();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			this._hasFormedShieldWall = true;
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0001E1D1 File Offset: 0x0001C3D1
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x0400034E RID: 846
		private TeamAISiegeComponent _teamAISiegeDefender;

		// Token: 0x0400034F RID: 847
		private CastleGate _innerGate;

		// Token: 0x04000350 RID: 848
		private CastleGate _outerGate;

		// Token: 0x04000351 RID: 849
		private List<SiegeLadder> _laddersOnThisSide;

		// Token: 0x04000352 RID: 850
		private BehaviorDefendCastleKeyPosition.BehaviorState _behaviorState;

		// Token: 0x04000353 RID: 851
		private MovementOrder _waitOrder;

		// Token: 0x04000354 RID: 852
		private MovementOrder _readyOrder;

		// Token: 0x04000355 RID: 853
		private FacingOrder _waitFacingOrder;

		// Token: 0x04000356 RID: 854
		private FacingOrder _readyFacingOrder;

		// Token: 0x04000357 RID: 855
		private TacticalPosition _tacticalMiddlePos;

		// Token: 0x04000358 RID: 856
		private TacticalPosition _tacticalWaitPos;

		// Token: 0x04000359 RID: 857
		private bool _hasFormedShieldWall;

		// Token: 0x0400035A RID: 858
		private WorldPosition _readyOrderPosition;

		// Token: 0x0200043D RID: 1085
		private enum BehaviorState
		{
			// Token: 0x040019C8 RID: 6600
			UnSet,
			// Token: 0x040019C9 RID: 6601
			Waiting,
			// Token: 0x040019CA RID: 6602
			Ready
		}
	}
}
