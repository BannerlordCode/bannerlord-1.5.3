using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000190 RID: 400
	public abstract class TeamAISiegeComponent : TeamAIComponent
	{
		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x0004EF4C File Offset: 0x0004D14C
		// (set) Token: 0x06001553 RID: 5459 RVA: 0x0004EF53 File Offset: 0x0004D153
		public static List<SiegeLane> SiegeLanes { get; private set; }

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x0004EF5B File Offset: 0x0004D15B
		// (set) Token: 0x06001555 RID: 5461 RVA: 0x0004EF62 File Offset: 0x0004D162
		public static SiegeQuerySystem QuerySystem { get; protected set; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x0004EF6A File Offset: 0x0004D16A
		public CastleGate OuterGate { get; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001557 RID: 5463 RVA: 0x0004EF72 File Offset: 0x0004D172
		public List<IPrimarySiegeWeapon> PrimarySiegeWeapons { get; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x0004EF7A File Offset: 0x0004D17A
		public CastleGate InnerGate { get; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001559 RID: 5465 RVA: 0x0004EF82 File Offset: 0x0004D182
		public MBReadOnlyList<SiegeLadder> Ladders
		{
			get
			{
				return this._ladders;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x0004EF8A File Offset: 0x0004D18A
		// (set) Token: 0x0600155B RID: 5467 RVA: 0x0004EF92 File Offset: 0x0004D192
		public bool AreLaddersReady { get; private set; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x0004EF9B File Offset: 0x0004D19B
		// (set) Token: 0x0600155D RID: 5469 RVA: 0x0004EFA3 File Offset: 0x0004D1A3
		public List<int> DifficultNavmeshIDs { get; private set; }

		// Token: 0x0600155E RID: 5470 RVA: 0x0004EFAC File Offset: 0x0004D1AC
		protected TeamAISiegeComponent(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
			this.CastleGates = currentMission.ActiveMissionObjects.FindAllWithType<CastleGate>().ToList<CastleGate>();
			this.WallSegments = currentMission.ActiveMissionObjects.FindAllWithType<WallSegment>().ToList<WallSegment>();
			this.OuterGate = this.CastleGates.FirstOrDefault<CastleGate>((CastleGate g) => g.GameEntity.HasTag("outer_gate"));
			this.InnerGate = this.CastleGates.FirstOrDefault<CastleGate>((CastleGate g) => g.GameEntity.HasTag("inner_gate"));
			this.SceneSiegeWeapons = Mission.Current.MissionObjects.FindAllWithType<SiegeWeapon>().ToList<SiegeWeapon>();
			this._ladders = this.SceneSiegeWeapons.OfType<SiegeLadder>().ToMBList<SiegeLadder>();
			this.Ram = this.SceneSiegeWeapons.FirstOrDefault<SiegeWeapon>((SiegeWeapon ssw) => ssw is BatteringRam) as BatteringRam;
			this.SiegeTowers = this.SceneSiegeWeapons.OfType<SiegeTower>().ToList<SiegeTower>();
			this.PrimarySiegeWeapons = new List<IPrimarySiegeWeapon>();
			this.PrimarySiegeWeapons.AddRange(this._ladders);
			if (this.Ram != null)
			{
				this.PrimarySiegeWeapons.Add(this.Ram);
			}
			this.PrimarySiegeWeapons.AddRange(this.SiegeTowers);
			this.PrimarySiegeWeaponNavMeshFaceIDs = new HashSet<int>();
			using (List<IPrimarySiegeWeapon>.Enumerator enumerator = this.PrimarySiegeWeapons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IPrimarySiegeWeapon primarySiegeWeapon;
					List<int> list;
					if ((primarySiegeWeapon = enumerator.Current) != null && primarySiegeWeapon.GetNavmeshFaceIds(out list))
					{
						this.PrimarySiegeWeaponNavMeshFaceIDs.UnionWith(list);
					}
				}
			}
			this.CastleKeyPositions = new List<MissionObject>();
			this.CastleKeyPositions.AddRange(this.CastleGates);
			this.CastleKeyPositions.AddRange(this.WallSegments);
			TeamAISiegeComponent.SiegeLanes = new List<SiegeLane>();
			int i;
			int j;
			for (i = 0; i < 3; i = j + 1)
			{
				TeamAISiegeComponent.SiegeLanes.Add(new SiegeLane((FormationAI.BehaviorSide)i, TeamAISiegeComponent.QuerySystem));
				TeamAISiegeComponent.SiegeLanes[i].SetPrimarySiegeWeapons((from psw in this.PrimarySiegeWeapons
					where psw.WeaponSide == (FormationAI.BehaviorSide)i
					select psw into um
					select (um)).ToList<IPrimarySiegeWeapon>());
				TeamAISiegeComponent.SiegeLanes[i].SetDefensePoints((from ckp in this.CastleKeyPositions
					where ((ICastleKeyPosition)ckp).DefenseSide == (FormationAI.BehaviorSide)i
					select ckp into dp
					select (ICastleKeyPosition)dp).ToList<ICastleKeyPosition>());
				TeamAISiegeComponent.SiegeLanes[i].RefreshLane();
				j = i;
			}
			TeamAISiegeComponent.SiegeLanes.ForEach(delegate(SiegeLane sl)
			{
				sl.SetSiegeQuerySystem(TeamAISiegeComponent.QuerySystem);
			});
			this.DifficultNavmeshIDs = new List<int>();
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0004F2E8 File Offset: 0x0004D4E8
		protected internal override void Tick(float dt)
		{
			if (!this._noProperLaneRemains)
			{
				int num = 0;
				SiegeLane siegeLane = null;
				foreach (SiegeLane siegeLane2 in TeamAISiegeComponent.SiegeLanes)
				{
					siegeLane2.RefreshLane();
					siegeLane2.DetermineLaneState();
					if (siegeLane2.IsBreach)
					{
						num++;
					}
					else
					{
						siegeLane = siegeLane2;
					}
				}
				if (siegeLane != null && num >= 2 && !siegeLane.IsOpen && siegeLane.LaneState >= SiegeLane.LaneStateEnum.Used)
				{
					siegeLane.SetLaneState(SiegeLane.LaneStateEnum.Unused);
				}
				if (TeamAISiegeComponent.SiegeLanes.Count != 0)
				{
					goto IL_01D0;
				}
				this._noProperLaneRemains = true;
				using (IEnumerator<FormationAI.BehaviorSide> enumerator2 = (from ckp in this.CastleKeyPositions.Where<MissionObject>(delegate(MissionObject ckp)
					{
						CastleGate castleGate;
						return (castleGate = ckp as CastleGate) != null && castleGate.DefenseSide != FormationAI.BehaviorSide.BehaviorSideNotSet;
					})
					select ((CastleGate)ckp).DefenseSide).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						FormationAI.BehaviorSide difficultLaneSide = enumerator2.Current;
						SiegeLane siegeLane3 = new SiegeLane(difficultLaneSide, TeamAISiegeComponent.QuerySystem);
						siegeLane3.SetPrimarySiegeWeapons(new List<IPrimarySiegeWeapon>());
						siegeLane3.SetDefensePoints((from ckp in this.CastleKeyPositions
							where ((ICastleKeyPosition)ckp).DefenseSide == difficultLaneSide && ckp is CastleGate
							select ckp into dp
							select dp as ICastleKeyPosition).ToList<ICastleKeyPosition>());
						siegeLane3.RefreshLane();
						siegeLane3.DetermineLaneState();
						TeamAISiegeComponent.SiegeLanes.Add(siegeLane3);
					}
					goto IL_01D0;
				}
			}
			foreach (SiegeLane siegeLane4 in TeamAISiegeComponent.SiegeLanes)
			{
				siegeLane4.RefreshLane();
				siegeLane4.DetermineLaneState();
			}
			IL_01D0:
			base.Tick(dt);
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0004F4F4 File Offset: 0x0004D6F4
		public static void OnMissionFinalize()
		{
			if (TeamAISiegeComponent.SiegeLanes != null)
			{
				TeamAISiegeComponent.SiegeLanes.Clear();
				TeamAISiegeComponent.SiegeLanes = null;
			}
			TeamAISiegeComponent.QuerySystem = null;
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0004F514 File Offset: 0x0004D714
		public bool CalculateIsChargePastWallsApplicable(FormationAI.BehaviorSide side)
		{
			if (Mission.Current.MissionTeamAIType == Mission.MissionTeamAITypeEnum.SallyOut)
			{
				return false;
			}
			if (side == FormationAI.BehaviorSide.BehaviorSideNotSet && this.InnerGate != null && !this.InnerGate.IsGateOpen)
			{
				return false;
			}
			foreach (SiegeLane siegeLane in TeamAISiegeComponent.SiegeLanes)
			{
				if (side == FormationAI.BehaviorSide.BehaviorSideNotSet)
				{
					if (!siegeLane.IsOpen)
					{
						return false;
					}
				}
				else if (side == siegeLane.LaneSide)
				{
					return siegeLane.IsOpen && (siegeLane.IsBreach || (siegeLane.HasGate && (this.InnerGate == null || this.InnerGate.IsGateOpen)));
				}
			}
			return true;
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0004F5E0 File Offset: 0x0004D7E0
		public void SetAreLaddersReady(bool areLaddersReady)
		{
			this.AreLaddersReady = areLaddersReady;
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0004F5E9 File Offset: 0x0004D7E9
		public bool CalculateIsAnyLaneOpenToGetInside()
		{
			return TeamAISiegeComponent.SiegeLanes.Any<SiegeLane>((SiegeLane sl) => sl.IsOpen);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0004F614 File Offset: 0x0004D814
		public bool CalculateIsAnyLaneOpenToGoOutside()
		{
			return TeamAISiegeComponent.SiegeLanes.Any<SiegeLane>(delegate(SiegeLane sl)
			{
				if (!sl.IsOpen)
				{
					return false;
				}
				if (!sl.IsBreach && !sl.HasGate)
				{
					return sl.PrimarySiegeWeapons.Any<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => psw is SiegeTower);
				}
				return true;
			});
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0004F63F File Offset: 0x0004D83F
		public bool IsPrimarySiegeWeaponNavmeshFaceId(int id)
		{
			return this.PrimarySiegeWeaponNavMeshFaceIDs.Contains(id);
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0004F650 File Offset: 0x0004D850
		public static bool IsFormationGroupInsideCastle(MBList<Formation> formationGroup, bool includeOnlyPositionedUnits, float thresholdPercentage = 0.4f)
		{
			int num = 0;
			foreach (Formation formation in formationGroup)
			{
				num += (includeOnlyPositionedUnits ? formation.Arrangement.PositionedUnitCount : formation.CountOfUnits);
			}
			float num2 = (float)num * thresholdPercentage;
			foreach (Formation formation2 in formationGroup)
			{
				if (formation2.CountOfUnits > 0)
				{
					num2 -= (float)formation2.CountUnitsOnNavMeshIDMod10(1, includeOnlyPositionedUnits);
					if (num2 <= 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0004F718 File Offset: 0x0004D918
		public static bool IsFormationInsideCastle(Formation formation, bool includeOnlyPositionedUnits, float thresholdPercentage = 0.4f)
		{
			int num = (includeOnlyPositionedUnits ? formation.Arrangement.PositionedUnitCount : formation.CountOfUnits);
			float num2 = (float)num * thresholdPercentage;
			if (num == 0)
			{
				return !(formation.Team.TeamAI is TeamAISiegeAttacker) && !(formation.Team.TeamAI is TeamAISallyOutDefender) && (formation.Team.TeamAI is TeamAISiegeDefender || formation.Team.TeamAI is TeamAISallyOutAttacker);
			}
			if (includeOnlyPositionedUnits)
			{
				return (float)formation.QuerySystem.InsideCastleUnitCountPositioned >= num2;
			}
			return (float)formation.QuerySystem.InsideCastleUnitCountIncludingUnpositioned >= num2;
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0004F7B8 File Offset: 0x0004D9B8
		public bool IsCastleBreached()
		{
			int num = 0;
			int num2 = 0;
			foreach (Formation formation in this.Mission.AttackerTeam.FormationsIncludingSpecialAndEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					num2++;
					if (TeamAISiegeComponent.IsFormationInsideCastle(formation, true, 0.4f))
					{
						num++;
					}
				}
			}
			if (this.Mission.AttackerAllyTeam != null)
			{
				foreach (Formation formation2 in this.Mission.AttackerAllyTeam.FormationsIncludingSpecialAndEmpty)
				{
					if (formation2.CountOfUnits > 0)
					{
						num2++;
						if (TeamAISiegeComponent.IsFormationInsideCastle(formation2, true, 0.4f))
						{
							num++;
						}
					}
				}
			}
			return (float)num >= (float)num2 * 0.7f;
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0004F8B4 File Offset: 0x0004DAB4
		public override void OnDeploymentFinished()
		{
			foreach (SiegeLadder siegeLadder in this._ladders.Where<SiegeLadder>((SiegeLadder l) => !l.IsDisabled))
			{
				this.DifficultNavmeshIDs.Add(siegeLadder.OnWallNavMeshId);
			}
			foreach (SiegeTower siegeTower in this.SiegeTowers)
			{
				this.DifficultNavmeshIDs.AddRange(siegeTower.CollectGetDifficultNavmeshIDs());
			}
			foreach (Formation formation in this.Team.FormationsIncludingEmpty)
			{
				formation.OnDeploymentFinished();
			}
		}

		// Token: 0x040005C0 RID: 1472
		public const int InsideCastleNavMeshID = 1;

		// Token: 0x040005C1 RID: 1473
		public const int SiegeTokenForceSize = 15;

		// Token: 0x040005C2 RID: 1474
		private const float FormationInsideCastleThresholdPercentage = 0.4f;

		// Token: 0x040005C3 RID: 1475
		private const float CastleBreachThresholdPercentage = 0.7f;

		// Token: 0x040005C6 RID: 1478
		public readonly IEnumerable<WallSegment> WallSegments;

		// Token: 0x040005C7 RID: 1479
		public readonly List<SiegeWeapon> SceneSiegeWeapons;

		// Token: 0x040005C8 RID: 1480
		protected readonly IEnumerable<CastleGate> CastleGates;

		// Token: 0x040005C9 RID: 1481
		protected readonly List<SiegeTower> SiegeTowers;

		// Token: 0x040005CA RID: 1482
		protected readonly HashSet<int> PrimarySiegeWeaponNavMeshFaceIDs;

		// Token: 0x040005CB RID: 1483
		protected BatteringRam Ram;

		// Token: 0x040005CC RID: 1484
		protected List<MissionObject> CastleKeyPositions;

		// Token: 0x040005CD RID: 1485
		private readonly MBList<SiegeLadder> _ladders;

		// Token: 0x040005CE RID: 1486
		private bool _noProperLaneRemains;
	}
}
