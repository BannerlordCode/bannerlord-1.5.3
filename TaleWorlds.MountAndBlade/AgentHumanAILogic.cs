using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000275 RID: 629
	public class AgentHumanAILogic : MissionLogic
	{
		// Token: 0x0600236D RID: 9069 RVA: 0x0007D900 File Offset: 0x0007BB00
		public override void OnAgentCreated(Agent agent)
		{
			base.OnAgentCreated(agent);
			if (agent.IsAIControlled && agent.IsHuman)
			{
				agent.AddComponent(new HumanAIComponent(agent));
			}
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x0007D928 File Offset: 0x0007BB28
		protected internal override void OnAgentControllerChanged(Agent agent, AgentControllerType oldController)
		{
			base.OnAgentControllerChanged(agent, oldController);
			if (agent.IsHuman)
			{
				if (agent.Controller == AgentControllerType.AI)
				{
					agent.AddComponent(new HumanAIComponent(agent));
					return;
				}
				if (oldController == AgentControllerType.AI && agent.HumanAIComponent != null)
				{
					agent.RemoveComponent(agent.HumanAIComponent);
				}
			}
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x0007D974 File Offset: 0x0007BB74
		public override void OnAgentMount(Agent agent)
		{
			base.OnAgentMount(agent);
			Mission.Current.UpdateMountReservationsAfterRiderMounts(agent, agent.MountAgent);
		}
	}
}
