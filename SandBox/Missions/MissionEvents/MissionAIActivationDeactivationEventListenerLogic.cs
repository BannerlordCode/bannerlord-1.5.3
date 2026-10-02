using System;
using System.Collections.Generic;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.Missions.MissionEvents
{
	// Token: 0x0200009E RID: 158
	public class MissionAIActivationDeactivationEventListenerLogic : MissionLogic
	{
		// Token: 0x0600069A RID: 1690 RVA: 0x0002C891 File Offset: 0x0002AA91
		public MissionAIActivationDeactivationEventListenerLogic()
		{
			Game.Current.EventManager.RegisterEvent<GenericMissionEvent>(new Action<GenericMissionEvent>(this.OnGenericMissionEventTriggered));
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0002C8B4 File Offset: 0x0002AAB4
		protected override void OnEndMission()
		{
			Game.Current.EventManager.UnregisterEvent<GenericMissionEvent>(new Action<GenericMissionEvent>(this.OnGenericMissionEventTriggered));
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0002C8D4 File Offset: 0x0002AAD4
		private void OnGenericMissionEventTriggered(GenericMissionEvent missionEvent)
		{
			if (missionEvent.EventId == "activate_agent_ai")
			{
				string[] array = missionEvent.Parameter.Split(new char[] { ' ' });
				SandBoxHelpers.MissionHelper.DisableGenericMissionEventScript(array[0], missionEvent);
				string[] activationTags = new string[array.Length - 1];
				Array.Copy(array, 1, activationTags, 0, activationTags.Length);
				using (List<Agent>.Enumerator enumerator = Mission.Current.Agents.GetEnumerator())
				{
					Func<string, bool> <>9__0;
					while (enumerator.MoveNext())
					{
						Agent agent = enumerator.Current;
						if (agent.AgentVisuals.IsValid())
						{
							string[] tags = agent.AgentVisuals.GetEntity().Tags;
							Func<string, bool> func;
							if ((func = <>9__0) == null)
							{
								func = (<>9__0 = (string x) => activationTags.ContainsQ(x));
							}
							if (tags.AnyQ<string>(func))
							{
								this.CheckRemoveScriptedBehaviorFromAgent(agent);
							}
						}
					}
					return;
				}
			}
			if (missionEvent.EventId == "deactivate_agent_ai")
			{
				string[] array2 = missionEvent.Parameter.Split(new char[] { ' ' });
				SandBoxHelpers.MissionHelper.DisableGenericMissionEventScript(array2[0], missionEvent);
				string[] deactivationTags = new string[array2.Length - 1];
				Array.Copy(array2, 1, deactivationTags, 0, deactivationTags.Length);
				Func<string, bool> <>9__1;
				foreach (Agent agent2 in Mission.Current.Agents)
				{
					if (agent2.AgentVisuals.IsValid())
					{
						string[] tags2 = agent2.AgentVisuals.GetEntity().Tags;
						Func<string, bool> func2;
						if ((func2 = <>9__1) == null)
						{
							func2 = (<>9__1 = (string x) => deactivationTags.ContainsQ(x));
						}
						if (tags2.AnyQ<string>(func2))
						{
							this.CheckAddScriptedBehaviorToAgent(agent2);
						}
					}
				}
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0002CAD0 File Offset: 0x0002ACD0
		private void CheckRemoveScriptedBehaviorFromAgent(Agent agent)
		{
			DailyBehaviorGroup behaviorGroup = agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			if (behaviorGroup.HasBehavior<IdleAgentBehavior>())
			{
				behaviorGroup.RemoveBehavior<IdleAgentBehavior>();
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0002CAFC File Offset: 0x0002ACFC
		private void CheckAddScriptedBehaviorToAgent(Agent agent)
		{
			DailyBehaviorGroup behaviorGroup = agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			if (!behaviorGroup.HasBehavior<IdleAgentBehavior>())
			{
				behaviorGroup.AddBehavior<IdleAgentBehavior>();
			}
			behaviorGroup.SetScriptedBehavior<IdleAgentBehavior>();
		}

		// Token: 0x0400038D RID: 909
		public const string ActivationEventId = "activate_agent_ai";

		// Token: 0x0400038E RID: 910
		public const string DeactivationEventId = "deactivate_agent_ai";
	}
}
