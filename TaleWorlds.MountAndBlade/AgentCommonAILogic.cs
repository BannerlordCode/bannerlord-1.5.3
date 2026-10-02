using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000274 RID: 628
	public class AgentCommonAILogic : MissionLogic
	{
		// Token: 0x0600236A RID: 9066 RVA: 0x0007D88C File Offset: 0x0007BA8C
		public override void OnAgentCreated(Agent agent)
		{
			base.OnAgentCreated(agent);
			if (agent.IsAIControlled)
			{
				agent.AddComponent(new CommonAIComponent(agent));
			}
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x0007D8AC File Offset: 0x0007BAAC
		protected internal override void OnAgentControllerChanged(Agent agent, AgentControllerType oldController)
		{
			base.OnAgentControllerChanged(agent, oldController);
			if (agent.IsActive())
			{
				if (agent.Controller == AgentControllerType.AI)
				{
					agent.AddComponent(new CommonAIComponent(agent));
					return;
				}
				if (oldController == AgentControllerType.AI && agent.CommonAIComponent != null)
				{
					agent.RemoveComponent(agent.CommonAIComponent);
				}
			}
		}
	}
}
