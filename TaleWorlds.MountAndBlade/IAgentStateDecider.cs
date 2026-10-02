using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000267 RID: 615
	public interface IAgentStateDecider : IMissionBehavior
	{
		// Token: 0x060022E9 RID: 8937
		AgentState GetAgentState(Agent affectedAgent, float deathProbability, out bool usedSurgery);
	}
}
