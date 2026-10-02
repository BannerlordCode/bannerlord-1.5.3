using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic
{
	// Token: 0x020003E8 RID: 1000
	public class BattleMissionAgentInteractionLogic : MissionLogic
	{
		// Token: 0x0600378D RID: 14221 RVA: 0x000E7174 File Offset: 0x000E5374
		public override bool IsThereAgentAction(Agent userAgent, Agent otherAgent)
		{
			return otherAgent.IsMount && otherAgent.IsActive() && (otherAgent.RiderAgent == userAgent || (otherAgent.RiderAgent == null && (userAgent.GetAgentFlags() & AgentFlag.CanRide) == AgentFlag.CanRide));
		}
	}
}
