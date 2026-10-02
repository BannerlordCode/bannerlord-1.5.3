using System;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Engine;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000072 RID: 114
	public class LocationCharacterAgentSpawnedMissionEvent : EventBase
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x0001B508 File Offset: 0x00019708
		public LocationCharacterAgentSpawnedMissionEvent(LocationCharacter locationCharacter, Agent agent, WeakGameEntity spawnedOnGameEntity)
		{
			this.LocationCharacter = locationCharacter;
			this.Agent = agent;
			this.SpawnedOnGameEntity = spawnedOnGameEntity;
		}

		// Token: 0x0400026B RID: 619
		public readonly LocationCharacter LocationCharacter;

		// Token: 0x0400026C RID: 620
		public readonly Agent Agent;

		// Token: 0x0400026D RID: 621
		public readonly WeakGameEntity SpawnedOnGameEntity;
	}
}
