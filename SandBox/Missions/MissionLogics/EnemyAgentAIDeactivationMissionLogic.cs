using System;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200006A RID: 106
	public class EnemyAgentAIDeactivationMissionLogic : MissionLogic
	{
		// Token: 0x06000464 RID: 1124 RVA: 0x0001A9A5 File Offset: 0x00018BA5
		public EnemyAgentAIDeactivationMissionLogic()
		{
			Game.Current.EventManager.RegisterEvent<LocationCharacterAgentSpawnedMissionEvent>(new Action<LocationCharacterAgentSpawnedMissionEvent>(this.OnLocationCharacterAgentSpawned));
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001A9C8 File Offset: 0x00018BC8
		protected override void OnEndMission()
		{
			Game.Current.EventManager.UnregisterEvent<LocationCharacterAgentSpawnedMissionEvent>(new Action<LocationCharacterAgentSpawnedMissionEvent>(this.OnLocationCharacterAgentSpawned));
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001A9E8 File Offset: 0x00018BE8
		private void OnLocationCharacterAgentSpawned(LocationCharacterAgentSpawnedMissionEvent locationCharacterAgentSpawnedEvent)
		{
			Agent agent = locationCharacterAgentSpawnedEvent.Agent;
			if (agent.Team == Mission.Current.PlayerEnemyTeam)
			{
				DailyBehaviorGroup behaviorGroup = agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
				if (!behaviorGroup.HasBehavior<IdleAgentBehavior>())
				{
					behaviorGroup.AddBehavior<IdleAgentBehavior>();
				}
				behaviorGroup.SetScriptedBehavior<IdleAgentBehavior>();
			}
		}
	}
}
