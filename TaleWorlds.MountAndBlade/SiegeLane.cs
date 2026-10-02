using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000186 RID: 390
	public class SiegeLane
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060014E9 RID: 5353 RVA: 0x0004CD60 File Offset: 0x0004AF60
		// (set) Token: 0x060014EA RID: 5354 RVA: 0x0004CD68 File Offset: 0x0004AF68
		public SiegeLane.LaneStateEnum LaneState { get; private set; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060014EB RID: 5355 RVA: 0x0004CD71 File Offset: 0x0004AF71
		public FormationAI.BehaviorSide LaneSide { get; }

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x0004CD79 File Offset: 0x0004AF79
		// (set) Token: 0x060014ED RID: 5357 RVA: 0x0004CD81 File Offset: 0x0004AF81
		public List<IPrimarySiegeWeapon> PrimarySiegeWeapons { get; private set; }

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x0004CD8A File Offset: 0x0004AF8A
		// (set) Token: 0x060014EF RID: 5359 RVA: 0x0004CD92 File Offset: 0x0004AF92
		public bool IsOpen { get; private set; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x0004CD9B File Offset: 0x0004AF9B
		// (set) Token: 0x060014F1 RID: 5361 RVA: 0x0004CDA3 File Offset: 0x0004AFA3
		public bool IsBreach { get; private set; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x0004CDAC File Offset: 0x0004AFAC
		// (set) Token: 0x060014F3 RID: 5363 RVA: 0x0004CDB4 File Offset: 0x0004AFB4
		public bool HasGate { get; private set; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x0004CDBD File Offset: 0x0004AFBD
		// (set) Token: 0x060014F5 RID: 5365 RVA: 0x0004CDC5 File Offset: 0x0004AFC5
		public List<ICastleKeyPosition> DefensePoints { get; private set; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x0004CDCE File Offset: 0x0004AFCE
		// (set) Token: 0x060014F7 RID: 5367 RVA: 0x0004CDD6 File Offset: 0x0004AFD6
		public WorldPosition DefenderOrigin { get; private set; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x0004CDDF File Offset: 0x0004AFDF
		// (set) Token: 0x060014F9 RID: 5369 RVA: 0x0004CDE7 File Offset: 0x0004AFE7
		public WorldPosition AttackerOrigin { get; private set; }

		// Token: 0x060014FA RID: 5370 RVA: 0x0004CDF0 File Offset: 0x0004AFF0
		public SiegeLane(FormationAI.BehaviorSide laneSide, SiegeQuerySystem siegeQuerySystem)
		{
			this.LaneSide = laneSide;
			this.IsOpen = false;
			this.PrimarySiegeWeapons = new List<IPrimarySiegeWeapon>();
			this.DefensePoints = new List<ICastleKeyPosition>();
			this.IsBreach = false;
			this._siegeQuerySystem = siegeQuerySystem;
			this._lastAssignedFormations = new Formation[Mission.Current.Teams.Count];
			this.HasGate = false;
			this.LaneState = SiegeLane.LaneStateEnum.Active;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0004CE60 File Offset: 0x0004B060
		public bool CalculateIsLaneUnusable()
		{
			if (this.IsOpen)
			{
				return false;
			}
			if (this.HasGate)
			{
				for (int i = 0; i < this.DefensePoints.Count; i++)
				{
					CastleGate castleGate;
					if ((castleGate = this.DefensePoints[i] as CastleGate) != null && castleGate.IsGateOpen && castleGate.GameEntity.HasTag("outer_gate"))
					{
						return false;
					}
				}
			}
			for (int j = 0; j < this.PrimarySiegeWeapons.Count; j++)
			{
				IPrimarySiegeWeapon primarySiegeWeapon = this.PrimarySiegeWeapons[j];
				UsableMachine usableMachine;
				SiegeTower siegeTower;
				BatteringRam batteringRam;
				if (((usableMachine = primarySiegeWeapon as UsableMachine) == null || usableMachine.GameEntity.IsValid) && ((siegeTower = primarySiegeWeapon as SiegeTower) == null || !siegeTower.IsDestroyed) && (primarySiegeWeapon.HasCompletedAction() || (batteringRam = primarySiegeWeapon as BatteringRam) == null || !batteringRam.IsDestroyed))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0004CF40 File Offset: 0x0004B140
		public Formation GetLastAssignedFormation(int teamIndex)
		{
			if (teamIndex >= 0)
			{
				return this._lastAssignedFormations[teamIndex];
			}
			return null;
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0004CF50 File Offset: 0x0004B150
		public void SetLaneState(SiegeLane.LaneStateEnum newLaneState)
		{
			this.LaneState = newLaneState;
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0004CF59 File Offset: 0x0004B159
		public void SetLastAssignedFormation(int teamIndex, Formation formation)
		{
			if (teamIndex >= 0)
			{
				this._lastAssignedFormations[teamIndex] = formation;
			}
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x0004CF68 File Offset: 0x0004B168
		public void SetSiegeQuerySystem(SiegeQuerySystem siegeQuerySystem)
		{
			this._siegeQuerySystem = siegeQuerySystem;
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0004CF74 File Offset: 0x0004B174
		public float CalculateLaneCapacity()
		{
			bool flag = false;
			for (int i = 0; i < this.DefensePoints.Count; i++)
			{
				WallSegment wallSegment;
				if ((wallSegment = this.DefensePoints[i] as WallSegment) != null && wallSegment.IsBreachedWall)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return 60f;
			}
			if (this.HasGate)
			{
				bool flag2 = true;
				for (int j = 0; j < this.DefensePoints.Count; j++)
				{
					CastleGate castleGate;
					if ((castleGate = this.DefensePoints[j] as CastleGate) != null && !castleGate.IsGateOpen)
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					return 60f;
				}
			}
			float num = 0f;
			for (int k = 0; k < this.PrimarySiegeWeapons.Count; k++)
			{
				SiegeWeapon siegeWeapon = this.PrimarySiegeWeapons[k] as SiegeWeapon;
				if ((this.PrimarySiegeWeapons[k].HasCompletedAction() || !siegeWeapon.IsDeactivated) && !siegeWeapon.IsDestroyed)
				{
					num += this.PrimarySiegeWeapons[k].SiegeWeaponPriority;
				}
			}
			return num;
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0004D088 File Offset: 0x0004B288
		public SiegeLane.LaneDefenseStates GetDefenseState()
		{
			switch (this.LaneState)
			{
			case SiegeLane.LaneStateEnum.Safe:
			case SiegeLane.LaneStateEnum.Unused:
				return SiegeLane.LaneDefenseStates.Empty;
			case SiegeLane.LaneStateEnum.Used:
			case SiegeLane.LaneStateEnum.Abandoned:
				return SiegeLane.LaneDefenseStates.Token;
			case SiegeLane.LaneStateEnum.Active:
			case SiegeLane.LaneStateEnum.Contested:
			case SiegeLane.LaneStateEnum.Conceited:
				return SiegeLane.LaneDefenseStates.Full;
			default:
				return SiegeLane.LaneDefenseStates.Full;
			}
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0004D0C8 File Offset: 0x0004B2C8
		private bool IsPowerBehindLane()
		{
			switch (this.LaneSide)
			{
			case FormationAI.BehaviorSide.Left:
				return this._siegeQuerySystem.LeftRegionMemberCount >= 30;
			case FormationAI.BehaviorSide.Middle:
				return this._siegeQuerySystem.MiddleRegionMemberCount >= 30;
			case FormationAI.BehaviorSide.Right:
				return this._siegeQuerySystem.RightRegionMemberCount >= 30;
			default:
				MBDebug.ShowWarning("Lane without side");
				return false;
			}
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0004D134 File Offset: 0x0004B334
		public bool IsUnderAttack()
		{
			switch (this.LaneSide)
			{
			case FormationAI.BehaviorSide.Left:
				return this._siegeQuerySystem.LeftCloseAttackerCount >= 15;
			case FormationAI.BehaviorSide.Middle:
				return this._siegeQuerySystem.MiddleCloseAttackerCount >= 15;
			case FormationAI.BehaviorSide.Right:
				return this._siegeQuerySystem.RightCloseAttackerCount >= 15;
			default:
				MBDebug.ShowWarning("Lane without side");
				return false;
			}
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0004D1A0 File Offset: 0x0004B3A0
		public bool IsDefended()
		{
			switch (this.LaneSide)
			{
			case FormationAI.BehaviorSide.Left:
				return this._siegeQuerySystem.LeftDefenderCount >= 15;
			case FormationAI.BehaviorSide.Middle:
				return this._siegeQuerySystem.MiddleDefenderCount >= 15;
			case FormationAI.BehaviorSide.Right:
				return this._siegeQuerySystem.RightDefenderCount >= 15;
			default:
				MBDebug.ShowWarning("Lane without side");
				return false;
			}
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x0004D20C File Offset: 0x0004B40C
		public void DetermineLaneState()
		{
			if (this.LaneState != SiegeLane.LaneStateEnum.Conceited || this.IsDefended())
			{
				if (this.CalculateIsLaneUnusable())
				{
					this.LaneState = SiegeLane.LaneStateEnum.Safe;
				}
				else if (Mission.Current.IsTeleportingAgents)
				{
					this.LaneState = SiegeLane.LaneStateEnum.Active;
				}
				else if (!this.IsOpen)
				{
					bool flag = true;
					foreach (IPrimarySiegeWeapon primarySiegeWeapon in this.PrimarySiegeWeapons)
					{
						if (!(primarySiegeWeapon is IMoveableSiegeWeapon) || primarySiegeWeapon.HasCompletedAction() || ((SiegeWeapon)primarySiegeWeapon).IsUsed)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						this.LaneState = SiegeLane.LaneStateEnum.Unused;
					}
					else
					{
						this.LaneState = ((!this.IsPowerBehindLane()) ? SiegeLane.LaneStateEnum.Used : SiegeLane.LaneStateEnum.Active);
					}
				}
				else if (!this.IsPowerBehindLane())
				{
					this.LaneState = SiegeLane.LaneStateEnum.Abandoned;
				}
				else
				{
					this.LaneState = ((!this.IsUnderAttack() || this.IsDefended()) ? SiegeLane.LaneStateEnum.Contested : SiegeLane.LaneStateEnum.Conceited);
				}
				if (this.HasGate && this.LaneState < SiegeLane.LaneStateEnum.Active && TeamAISiegeComponent.QuerySystem.InsideAttackerCount >= 15)
				{
					this.LaneState = SiegeLane.LaneStateEnum.Active;
				}
			}
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x0004D334 File Offset: 0x0004B534
		public WorldPosition GetCurrentAttackerPosition()
		{
			if (this.IsBreach)
			{
				return this.DefenderOrigin;
			}
			if (this._attackerMovableWeapon != null)
			{
				return this._attackerMovableWeapon.WaitFrame.origin.ToWorldPosition();
			}
			return this.AttackerOrigin;
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x0004D36C File Offset: 0x0004B56C
		public void DetermineOrigins()
		{
			this._attackerMovableWeapon = null;
			if (this.IsBreach)
			{
				WallSegment wallSegment = this.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp is WallSegment && (dp as WallSegment).IsBreachedWall) as WallSegment;
				this.DefenderOrigin = wallSegment.MiddleFrame.Origin;
				this.AttackerOrigin = wallSegment.AttackerWaitFrame.Origin;
				return;
			}
			this.HasGate = this.DefensePoints.Any<ICastleKeyPosition>((ICastleKeyPosition dp) => dp is CastleGate);
			IEnumerable<IPrimarySiegeWeapon> enumerable;
			if (this.PrimarySiegeWeapons.Count != 0)
			{
				IEnumerable<IPrimarySiegeWeapon> primarySiegeWeapons = this.PrimarySiegeWeapons;
				enumerable = primarySiegeWeapons;
			}
			else
			{
				enumerable = from sw in Mission.Current.MissionObjects.FindAllWithType<SiegeWeapon>().Where<SiegeWeapon>(delegate(SiegeWeapon sw)
					{
						IPrimarySiegeWeapon primarySiegeWeapon;
						return (primarySiegeWeapon = sw as IPrimarySiegeWeapon) != null && primarySiegeWeapon.WeaponSide == this.LaneSide;
					})
					select sw as IPrimarySiegeWeapon;
			}
			IEnumerable<IPrimarySiegeWeapon> enumerable2 = enumerable;
			IMoveableSiegeWeapon moveableSiegeWeapon;
			if ((moveableSiegeWeapon = enumerable2.FirstOrDefault<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => psw is IMoveableSiegeWeapon) as IMoveableSiegeWeapon) != null)
			{
				this._attackerMovableWeapon = moveableSiegeWeapon as SiegeWeapon;
				this.DefenderOrigin = ((moveableSiegeWeapon as IPrimarySiegeWeapon).TargetCastlePosition as ICastleKeyPosition).MiddleFrame.Origin;
				this.AttackerOrigin = moveableSiegeWeapon.GetInitialFrame().origin.ToWorldPosition();
				return;
			}
			SiegeLadder siegeLadder = enumerable2.FirstOrDefault<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => psw is SiegeLadder) as SiegeLadder;
			this.DefenderOrigin = (siegeLadder.TargetCastlePosition as ICastleKeyPosition).MiddleFrame.Origin;
			this.AttackerOrigin = siegeLadder.InitialWaitPosition.GetGlobalFrame().origin.ToWorldPosition();
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x0004D53C File Offset: 0x0004B73C
		public void RefreshLane()
		{
			for (int i = this.PrimarySiegeWeapons.Count - 1; i >= 0; i--)
			{
				SiegeWeapon siegeWeapon;
				if ((siegeWeapon = this.PrimarySiegeWeapons[i] as SiegeWeapon) != null && siegeWeapon.IsDisabled)
				{
					this.PrimarySiegeWeapons.RemoveAt(i);
				}
			}
			bool flag = false;
			for (int j = 0; j < this.DefensePoints.Count; j++)
			{
				WallSegment wallSegment;
				if ((wallSegment = this.DefensePoints[j] as WallSegment) != null && wallSegment.IsBreachedWall)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.IsOpen = true;
				this.IsBreach = true;
				return;
			}
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = true;
			for (int k = 0; k < this.DefensePoints.Count; k++)
			{
				ICastleKeyPosition castleKeyPosition = this.DefensePoints[k];
				CastleGate castleGate;
				if (flag4 && (castleGate = castleKeyPosition as CastleGate) != null)
				{
					flag2 = true;
					flag3 = true;
					if (!castleGate.IsDestroyed && castleGate.State != CastleGate.GateState.Open)
					{
						flag4 = false;
						break;
					}
				}
				else if (!flag3 && !(castleKeyPosition is WallSegment))
				{
					flag3 = true;
				}
			}
			bool flag5 = false;
			if (!flag3)
			{
				for (int l = 0; l < this.PrimarySiegeWeapons.Count; l++)
				{
					IPrimarySiegeWeapon primarySiegeWeapon = this.PrimarySiegeWeapons[l];
					if (primarySiegeWeapon.HasCompletedAction() && !(primarySiegeWeapon as UsableMachine).IsDestroyed)
					{
						flag5 = true;
						break;
					}
				}
			}
			this.IsOpen = (flag2 && flag4) || flag5;
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x0004D6A0 File Offset: 0x0004B8A0
		public void SetPrimarySiegeWeapons(List<IPrimarySiegeWeapon> primarySiegeWeapons)
		{
			this.PrimarySiegeWeapons = primarySiegeWeapons;
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x0004D6AC File Offset: 0x0004B8AC
		public void SetDefensePoints(List<ICastleKeyPosition> defensePoints)
		{
			this.DefensePoints = defensePoints;
			foreach (ICastleKeyPosition castleKeyPosition in this.PrimarySiegeWeapons.Select<IPrimarySiegeWeapon, ICastleKeyPosition>((IPrimarySiegeWeapon psw) => psw.TargetCastlePosition as ICastleKeyPosition))
			{
				if (castleKeyPosition != null && !this.DefensePoints.Contains(castleKeyPosition))
				{
					this.DefensePoints.Add(castleKeyPosition);
				}
			}
		}

		// Token: 0x0400058C RID: 1420
		private readonly Formation[] _lastAssignedFormations;

		// Token: 0x0400058D RID: 1421
		private SiegeQuerySystem _siegeQuerySystem;

		// Token: 0x0400058E RID: 1422
		private SiegeWeapon _attackerMovableWeapon;

		// Token: 0x020004DD RID: 1245
		public enum LaneStateEnum
		{
			// Token: 0x04001C88 RID: 7304
			Safe,
			// Token: 0x04001C89 RID: 7305
			Unused,
			// Token: 0x04001C8A RID: 7306
			Used,
			// Token: 0x04001C8B RID: 7307
			Active,
			// Token: 0x04001C8C RID: 7308
			Abandoned,
			// Token: 0x04001C8D RID: 7309
			Contested,
			// Token: 0x04001C8E RID: 7310
			Conceited
		}

		// Token: 0x020004DE RID: 1246
		public enum LaneDefenseStates
		{
			// Token: 0x04001C90 RID: 7312
			Empty,
			// Token: 0x04001C91 RID: 7313
			Token,
			// Token: 0x04001C92 RID: 7314
			Full
		}
	}
}
