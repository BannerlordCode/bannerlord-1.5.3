using System;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace SandBox.Issues.IssueQuestTasks
{
	// Token: 0x020000C0 RID: 192
	public class FollowAgentQuestTask : QuestTaskBase
	{
		// Token: 0x060007E2 RID: 2018 RVA: 0x00035012 File Offset: 0x00033212
		public FollowAgentQuestTask(Agent followedAgent, GameEntity targetEntity, Action onSucceededAction, Action onCanceledAction, DialogFlow dialogFlow = null)
			: base(dialogFlow, onSucceededAction, null, onCanceledAction)
		{
			this._followedAgent = followedAgent;
			this._followedAgentChar = (CharacterObject)this._followedAgent.Character;
			this._targetEntity = targetEntity;
			this.StartAgentMovement();
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0003504A File Offset: 0x0003324A
		public FollowAgentQuestTask(Agent followedAgent, Agent targetAgent, Action onSucceededAction, Action onCanceledAction, DialogFlow dialogFlow = null)
			: base(dialogFlow, onSucceededAction, null, onCanceledAction)
		{
			this._followedAgent = followedAgent;
			this._targetAgent = targetAgent;
			this.StartAgentMovement();
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0003506C File Offset: 0x0003326C
		private void StartAgentMovement()
		{
			if (this._targetEntity != null)
			{
				UsableMachine firstScriptOfType = this._targetEntity.GetFirstScriptOfType<UsableMachine>();
				ScriptBehavior.AddUsableMachineTarget(this._followedAgent, firstScriptOfType);
				return;
			}
			if (this._targetAgent != null)
			{
				ScriptBehavior.AddAgentTarget(this._followedAgent, this._targetAgent);
			}
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x000350BC File Offset: 0x000332BC
		public void MissionTick(float dt)
		{
			ScriptBehavior scriptBehavior = (ScriptBehavior)this._followedAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehavior<ScriptBehavior>();
			if (scriptBehavior != null && scriptBehavior.IsNearTarget(this._targetAgent) && this._followedAgent.GetCurrentVelocity().LengthSquared < 0.0001f && this._followedAgent.Position.DistanceSquared(Mission.Current.MainAgent.Position) < 16f)
			{
				base.Finish(QuestTaskBase.FinishStates.Success);
			}
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00035144 File Offset: 0x00033344
		protected override void OnFinished()
		{
			this._followedAgent = null;
			this._followedAgentChar = null;
			this._targetEntity = null;
			this._targetAgent = null;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00035162 File Offset: 0x00033362
		public override void SetReferences()
		{
			CampaignEvents.MissionTickEvent.AddNonSerializedListener(this, new Action<float>(this.MissionTick));
		}

		// Token: 0x0400042B RID: 1067
		private Agent _followedAgent;

		// Token: 0x0400042C RID: 1068
		private CharacterObject _followedAgentChar;

		// Token: 0x0400042D RID: 1069
		private GameEntity _targetEntity;

		// Token: 0x0400042E RID: 1070
		private Agent _targetAgent;
	}
}
