using System;
using System.Collections.Generic;
using SandBox.Conversation.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Conversation
{
	// Token: 0x020000CC RID: 204
	public static class ConversationMission
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x0003B80F File Offset: 0x00039A0F
		public static Agent OneToOneConversationAgent
		{
			get
			{
				return Campaign.Current.ConversationManager.OneToOneConversationAgent as Agent;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x0003B825 File Offset: 0x00039A25
		public static CharacterObject OneToOneConversationCharacter
		{
			get
			{
				return Campaign.Current.ConversationManager.OneToOneConversationCharacter;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x0003B836 File Offset: 0x00039A36
		public static Agent CurrentSpeakerAgent
		{
			get
			{
				return Campaign.Current.ConversationManager.SpeakerAgent as Agent;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0003B84C File Offset: 0x00039A4C
		public static IEnumerable<Agent> ConversationAgents
		{
			get
			{
				foreach (IAgent agent in Campaign.Current.ConversationManager.ConversationAgents)
				{
					yield return agent as Agent;
				}
				IEnumerator<IAgent> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0003B855 File Offset: 0x00039A55
		public static void StartConversationWithAgent(Agent agent)
		{
			MissionConversationLogic missionBehavior = Mission.Current.GetMissionBehavior<MissionConversationLogic>();
			if (missionBehavior == null)
			{
				return;
			}
			missionBehavior.StartConversation(agent, true, false);
		}
	}
}
