using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000191 RID: 401
	public class TeamAISiegeDefender : TeamAISiegeComponent
	{
		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x0600156A RID: 5482 RVA: 0x0004F9C0 File Offset: 0x0004DBC0
		public List<ArcherPosition> ArcherPositions { get; }

		// Token: 0x0600156B RID: 5483 RVA: 0x0004F9C8 File Offset: 0x0004DBC8
		public TeamAISiegeDefender(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
			TeamAISiegeComponent.QuerySystem = new SiegeQuerySystem(this.Team, TeamAISiegeComponent.SiegeLanes);
			TeamAISiegeComponent.QuerySystem.Expire();
			IEnumerable<GameEntity> enumerable = from ap in currentMission.Scene.FindEntitiesWithTag("archer_position")
				where ap.Parent == null || ap.Parent.IsVisibleIncludeParents()
				select ap;
			this.ArcherPositions = enumerable.Select<GameEntity, ArcherPosition>((GameEntity ap) => new ArcherPosition(ap, TeamAISiegeComponent.QuerySystem, BattleSideEnum.Defender)).ToList<ArcherPosition>();
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0004FA64 File Offset: 0x0004DC64
		public override void OnUnitAddedToFormationForTheFirstTime(Formation formation)
		{
			if (formation.AI.GetBehavior<BehaviorCharge>() == null)
			{
				formation.ForceCalculateCaches();
				if (formation.FormationIndex == FormationClass.NumberOfRegularFormations)
				{
					formation.AI.AddAiBehavior(new BehaviorGeneral(formation));
				}
				else if (formation.FormationIndex == FormationClass.Bodyguard)
				{
					formation.AI.AddAiBehavior(new BehaviorProtectGeneral(formation));
				}
				formation.AI.AddAiBehavior(new BehaviorCharge(formation));
				formation.AI.AddAiBehavior(new BehaviorPullBack(formation));
				formation.AI.AddAiBehavior(new BehaviorRegroup(formation));
				formation.AI.AddAiBehavior(new BehaviorReserve(formation));
				formation.AI.AddAiBehavior(new BehaviorRetreat(formation));
				formation.AI.AddAiBehavior(new BehaviorStop(formation));
				formation.AI.AddAiBehavior(new BehaviorTacticalCharge(formation));
				formation.AI.AddAiBehavior(new BehaviorDefendCastleKeyPosition(formation));
				formation.AI.AddAiBehavior(new BehaviorEliminateEnemyInsideCastle(formation));
				formation.AI.AddAiBehavior(new BehaviorRetakeCastleKeyPosition(formation));
				formation.AI.AddAiBehavior(new BehaviorRetreatToKeep(formation));
				formation.AI.AddAiBehavior(new BehaviorSallyOut(formation));
				formation.AI.AddAiBehavior(new BehaviorUseMurderHole(formation));
				formation.AI.AddAiBehavior(new BehaviorShootFromCastleWalls(formation));
				formation.AI.AddAiBehavior(new BehaviorSparseSkirmish(formation));
			}
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0004FBC0 File Offset: 0x0004DDC0
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			foreach (SiegeTower siegeTower in this.SiegeTowers)
			{
				base.DifficultNavmeshIDs.AddRange(siegeTower.CollectGetDifficultNavmeshIDsForDefenders());
			}
			List<SiegeLane> list = TeamAISiegeComponent.SiegeLanes.ToList<SiegeLane>();
			TeamAISiegeComponent.SiegeLanes.Clear();
			int i;
			int i2;
			for (i = 0; i < 3; i = i2 + 1)
			{
				TeamAISiegeComponent.SiegeLanes.Add(new SiegeLane((FormationAI.BehaviorSide)i, TeamAISiegeComponent.QuerySystem));
				SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes[i];
				siegeLane.SetPrimarySiegeWeapons(base.PrimarySiegeWeapons.Where<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => psw.WeaponSide == (FormationAI.BehaviorSide)i).ToList<IPrimarySiegeWeapon>());
				siegeLane.SetDefensePoints((from ckp in this.CastleKeyPositions
					where (ckp as ICastleKeyPosition).DefenseSide == (FormationAI.BehaviorSide)i
					select ckp into dp
					select dp as ICastleKeyPosition).ToList<ICastleKeyPosition>());
				siegeLane.RefreshLane();
				siegeLane.DetermineLaneState();
				siegeLane.DetermineOrigins();
				if (i < list.Count)
				{
					for (int j = 0; j < Mission.Current.Teams.Count; j++)
					{
						siegeLane.SetLastAssignedFormation(j, list[i].GetLastAssignedFormation(j));
					}
				}
				i2 = i;
			}
			TeamAISiegeComponent.QuerySystem = new SiegeQuerySystem(this.Team, TeamAISiegeComponent.SiegeLanes);
			TeamAISiegeComponent.QuerySystem.Expire();
			TeamAISiegeComponent.SiegeLanes.ForEach(delegate(SiegeLane sl)
			{
				sl.SetSiegeQuerySystem(TeamAISiegeComponent.QuerySystem);
			});
			this.ArcherPositions.ForEach(delegate(ArcherPosition ap)
			{
				ap.OnDeploymentFinished(TeamAISiegeComponent.QuerySystem, BattleSideEnum.Defender);
			});
		}

		// Token: 0x040005D4 RID: 1492
		public const float InsideEnemyThresholdRatio = 0.5f;

		// Token: 0x040005D6 RID: 1494
		public Vec3 MurderHolePosition;
	}
}
