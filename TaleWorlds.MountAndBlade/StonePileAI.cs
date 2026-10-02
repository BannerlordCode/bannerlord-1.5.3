using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200015F RID: 351
	public class StonePileAI : UsableMachineAIBase
	{
		// Token: 0x06001273 RID: 4723 RVA: 0x00039BC1 File Offset: 0x00037DC1
		public StonePileAI(StonePile stonePile)
			: base(stonePile)
		{
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x00039BCC File Offset: 0x00037DCC
		public static Agent GetSuitableAgentForStandingPoint(StonePile usableMachine, StandingPoint standingPoint, List<Agent> agents, List<Agent> usedAgents)
		{
			float num = float.MinValue;
			Agent agent = null;
			foreach (Agent agent2 in agents)
			{
				if (StonePileAI.IsAgentAssignable(agent2) && !standingPoint.IsDisabledForAgent(agent2) && standingPoint.GetUsageScoreForAgent(agent2) > num)
				{
					num = standingPoint.GetUsageScoreForAgent(agent2);
					agent = agent2;
				}
			}
			return agent;
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00039C44 File Offset: 0x00037E44
		public static Agent GetSuitableAgentForStandingPoint(StonePile stonePile, StandingPoint standingPoint, List<ValueTuple<Agent, float>> agents, List<Agent> usedAgents, float weight)
		{
			float num = float.MinValue;
			Agent agent = null;
			foreach (ValueTuple<Agent, float> valueTuple in agents)
			{
				Agent item = valueTuple.Item1;
				if (StonePileAI.IsAgentAssignable(item) && !standingPoint.IsDisabledForAgent(item) && standingPoint.GetUsageScoreForAgent(item) > num)
				{
					num = standingPoint.GetUsageScoreForAgent(item);
					agent = item;
				}
			}
			return agent;
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00039CC0 File Offset: 0x00037EC0
		public static bool IsAgentAssignable(Agent agent)
		{
			return agent != null && agent.IsAIControlled && agent.IsActive() && !agent.IsRunningAway && !agent.InteractingWithAnyGameObject() && (agent.Formation == null || !agent.IsDetachedFromFormation);
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00039CFA File Offset: 0x00037EFA
		protected override void HandleAgentStopUsingStandingPoint(Agent agent, StandingPoint standingPoint)
		{
			agent.DisableScriptedCombatMovement();
			base.HandleAgentStopUsingStandingPoint(agent, standingPoint);
		}
	}
}
