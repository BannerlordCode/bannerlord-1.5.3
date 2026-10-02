using System;
using SandBox.Conversation;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Issues.IssueQuestTasks
{
	// Token: 0x020000BF RID: 191
	public class BeginConversationInitiatedByAIQuestTask : QuestTaskBase
	{
		// Token: 0x060007DD RID: 2013 RVA: 0x00034F94 File Offset: 0x00033194
		public BeginConversationInitiatedByAIQuestTask(Agent agent, Action onSucceededAction, Action onFailedAction, Action onCanceledAction, DialogFlow dialogFlow = null)
			: base(dialogFlow, onSucceededAction, onFailedAction, onCanceledAction)
		{
			this._conversationAgent = agent;
			base.IsLogged = false;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00034FB0 File Offset: 0x000331B0
		public void MissionTick(float dt)
		{
			if (Mission.Current.MainAgent == null || this._conversationAgent == null)
			{
				return;
			}
			if (!this._conversationOpened && Mission.Current.Mode != MissionMode.Conversation)
			{
				this.OpenConversation(this._conversationAgent);
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00034FE8 File Offset: 0x000331E8
		private void OpenConversation(Agent agent)
		{
			ConversationMission.StartConversationWithAgent(agent);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00034FF0 File Offset: 0x000331F0
		protected override void OnFinished()
		{
			this._conversationAgent = null;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00034FF9 File Offset: 0x000331F9
		public override void SetReferences()
		{
			CampaignEvents.MissionTickEvent.AddNonSerializedListener(this, new Action<float>(this.MissionTick));
		}

		// Token: 0x04000429 RID: 1065
		private bool _conversationOpened;

		// Token: 0x0400042A RID: 1066
		private Agent _conversationAgent;
	}
}
