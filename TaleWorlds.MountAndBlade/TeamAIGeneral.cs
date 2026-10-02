using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200018A RID: 394
	public class TeamAIGeneral : TeamAIComponent
	{
		// Token: 0x0600153C RID: 5436 RVA: 0x0004E158 File Offset: 0x0004C358
		public TeamAIGeneral(Mission currentMission, Team currentTeam, float thinkTimerTime = 10f, float applyTimerTime = 1f)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0004E168 File Offset: 0x0004C368
		public override void OnUnitAddedToFormationForTheFirstTime(Formation formation)
		{
			if (GameNetwork.IsServer)
			{
				formation.ForceCalculateCaches();
				if (formation.AI.GetBehavior<BehaviorCharge>() == null)
				{
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
					formation.AI.AddAiBehavior(new BehaviorSergeantMPInfantry(formation));
					formation.AI.AddAiBehavior(new BehaviorSergeantMPLastFlagLastStand(formation));
					formation.AI.AddAiBehavior(new BehaviorSergeantMPMounted(formation));
					formation.AI.AddAiBehavior(new BehaviorSergeantMPMountedRanged(formation));
					formation.AI.AddAiBehavior(new BehaviorSergeantMPRanged(formation));
					return;
				}
			}
			else if (!GameNetwork.IsClientOrReplay)
			{
				formation.ForceCalculateCaches();
				if (formation.AI.GetBehavior<BehaviorCharge>() == null)
				{
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
					formation.AI.AddAiBehavior(new BehaviorAdvance(formation));
					formation.AI.AddAiBehavior(new BehaviorCautiousAdvance(formation));
					formation.AI.AddAiBehavior(new BehaviorCavalryScreen(formation));
					formation.AI.AddAiBehavior(new BehaviorDefend(formation));
					formation.AI.AddAiBehavior(new BehaviorDefensiveRing(formation));
					formation.AI.AddAiBehavior(new BehaviorFireFromInfantryCover(formation));
					formation.AI.AddAiBehavior(new BehaviorFlank(formation));
					formation.AI.AddAiBehavior(new BehaviorHoldHighGround(formation));
					formation.AI.AddAiBehavior(new BehaviorHorseArcherSkirmish(formation));
					formation.AI.AddAiBehavior(new BehaviorMountedSkirmish(formation));
					formation.AI.AddAiBehavior(new BehaviorProtectFlank(formation));
					formation.AI.AddAiBehavior(new BehaviorScreenedSkirmish(formation));
					formation.AI.AddAiBehavior(new BehaviorSkirmish(formation));
					formation.AI.AddAiBehavior(new BehaviorSkirmishBehindFormation(formation));
					formation.AI.AddAiBehavior(new BehaviorSkirmishLine(formation));
					formation.AI.AddAiBehavior(new BehaviorVanguard(formation));
					formation.AI.AddAiBehavior(new BehaviorShootFromCliff(formation));
				}
			}
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0004E488 File Offset: 0x0004C688
		private void UpdateVariables()
		{
			TeamQuerySystem querySystem = this.Team.QuerySystem;
			this._numberOfEnemiesInShootRange = 0;
			this._numberOfEnemiesCloseToAttack = 0;
			Vec2 averagePosition = querySystem.AveragePosition;
			foreach (Agent agent in this.Mission.Agents)
			{
				if (!agent.IsMount && agent.Team.IsValid && agent.Team.IsEnemyOf(this.Team))
				{
					float num = agent.Position.DistanceSquared(new Vec3(averagePosition.x, averagePosition.y, 0f, -1f));
					if (num < 40000f)
					{
						this._numberOfEnemiesInShootRange++;
					}
					if (num < 1600f)
					{
						this._numberOfEnemiesCloseToAttack++;
					}
				}
			}
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0004E57C File Offset: 0x0004C77C
		public override void OnDeploymentFinished()
		{
			foreach (Formation formation in this.Team.FormationsIncludingEmpty)
			{
				formation.OnDeploymentFinished();
			}
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0004E5D4 File Offset: 0x0004C7D4
		protected override void DebugTick(float dt)
		{
		}

		// Token: 0x040005B9 RID: 1465
		private int _numberOfEnemiesInShootRange;

		// Token: 0x040005BA RID: 1466
		private int _numberOfEnemiesCloseToAttack;
	}
}
