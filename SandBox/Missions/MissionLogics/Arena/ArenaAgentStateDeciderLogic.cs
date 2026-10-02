using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Arena
{
	// Token: 0x0200009A RID: 154
	public class ArenaAgentStateDeciderLogic : MissionLogic, IAgentStateDecider, IMissionBehavior
	{
		// Token: 0x06000665 RID: 1637 RVA: 0x0002B274 File Offset: 0x00029474
		public AgentState GetAgentState(Agent effectedAgent, float deathProbability, out bool usedSurgery)
		{
			usedSurgery = false;
			return AgentState.Unconscious;
		}
	}
}
