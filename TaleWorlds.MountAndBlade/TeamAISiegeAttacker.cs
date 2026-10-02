using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200018F RID: 399
	public class TeamAISiegeAttacker : TeamAISiegeComponent
	{
		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x0004EC32 File Offset: 0x0004CE32
		public MBReadOnlyList<ArcherPosition> ArcherPositions
		{
			get
			{
				return this._archerPositions;
			}
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0004EC3C File Offset: 0x0004CE3C
		public TeamAISiegeAttacker(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
			IEnumerable<GameEntity> enumerable = currentMission.Scene.FindEntitiesWithTag("archer_position_attacker");
			this._archerPositions = enumerable.Select<GameEntity, ArcherPosition>((GameEntity ap) => new ArcherPosition(ap, TeamAISiegeComponent.QuerySystem, BattleSideEnum.Attacker)).ToMBList<ArcherPosition>();
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0004EC98 File Offset: 0x0004CE98
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
				formation.AI.AddAiBehavior(new BehaviorAssaultWalls(formation));
				formation.AI.AddAiBehavior(new BehaviorShootFromSiegeTower(formation));
				formation.AI.AddAiBehavior(new BehaviorUseSiegeMachines(formation));
				formation.AI.AddAiBehavior(new BehaviorWaitForLadders(formation));
				formation.AI.AddAiBehavior(new BehaviorSparseSkirmish(formation));
				formation.AI.AddAiBehavior(new BehaviorSkirmish(formation));
				formation.AI.AddAiBehavior(new BehaviorRetreatToKeep(formation));
			}
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0004EDE0 File Offset: 0x0004CFE0
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			foreach (SiegeTower siegeTower in this.SiegeTowers)
			{
				base.DifficultNavmeshIDs.AddRange(siegeTower.CollectGetDifficultNavmeshIDsForAttackers());
			}
			foreach (ArcherPosition archerPosition in this._archerPositions)
			{
				archerPosition.OnDeploymentFinished(TeamAISiegeComponent.QuerySystem, BattleSideEnum.Attacker);
			}
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0004EE88 File Offset: 0x0004D088
		public override void OnFormationFrameChanged(Agent agent, bool isFrameEnabled, WorldPosition frame)
		{
			base.OnFormationFrameChanged(agent, isFrameEnabled, frame);
			foreach (SiegeTower siegeTower in this.SiegeTowers)
			{
				if (agent.IsInLadderQueue || siegeTower.HasCompletedAction())
				{
					siegeTower.OnFormationFrameChanged(agent, isFrameEnabled, frame);
				}
			}
			foreach (SiegeLadder siegeLadder in base.Ladders)
			{
				if (agent.IsInLadderQueue || siegeLadder.State == SiegeLadder.LadderState.OnWall)
				{
					siegeLadder.OnFormationFrameChanged(agent, isFrameEnabled, frame);
				}
			}
		}

		// Token: 0x040005BF RID: 1471
		private readonly MBList<ArcherPosition> _archerPositions;
	}
}
