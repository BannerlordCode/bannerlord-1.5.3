using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.AI.AgentComponents
{
	// Token: 0x0200040F RID: 1039
	public class ScriptedMovementComponent : AgentComponent
	{
		// Token: 0x060038AE RID: 14510 RVA: 0x000E9DE8 File Offset: 0x000E7FE8
		public ScriptedMovementComponent(Agent agent, bool isCharacterToTalkTo = false, float dialogueProximityOffset = 0f)
			: base(agent)
		{
			this._isCharacterToTalkTo = isCharacterToTalkTo;
			this._agentSpeedLimit = this.Agent.GetMaximumSpeedLimit();
			if (!this._isCharacterToTalkTo)
			{
				this.Agent.SetMaximumSpeedLimit(MBRandom.RandomFloatRanged(0.2f, 0.3f), true);
				this._dialogueTriggerProximity += dialogueProximityOffset;
			}
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x000E9E50 File Offset: 0x000E8050
		public void SetTargetAgent(Agent targetAgent)
		{
			this._targetAgent = targetAgent;
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x000E9E5C File Offset: 0x000E805C
		public override void OnTick(float dt)
		{
			if (this.Agent.Mission.AllowAiTicking && this.Agent.IsAIControlled && this._targetAgent != null)
			{
				bool flag = this._targetAgent.State != AgentState.Routed && this._targetAgent.State != AgentState.Deleted;
				if (!this._isInDialogueRange)
				{
					float num = this._targetAgent.Position.DistanceSquared(this.Agent.Position);
					this._isInDialogueRange = num <= this._dialogueTriggerProximity * this._dialogueTriggerProximity;
					if (this._isInDialogueRange)
					{
						this.Agent.SetScriptedFlags(this.Agent.GetScriptedFlags() & ~Agent.AIScriptedFrameFlags.DoNotRun);
						this.Agent.DisableScriptedMovement();
						if (flag)
						{
							this.Agent.SetLookAgent(this._targetAgent);
						}
						this.Agent.SetMaximumSpeedLimit(this._agentSpeedLimit, false);
						return;
					}
					WorldPosition worldPosition = this._targetAgent.Position.ToWorldPosition();
					this.Agent.SetScriptedPosition(ref worldPosition, false, Agent.AIScriptedFrameFlags.DoNotRun);
					return;
				}
				else if (!flag)
				{
					this.Agent.SetLookAgent(null);
				}
			}
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x000E9F83 File Offset: 0x000E8183
		public bool ShouldConversationStartWithAgent()
		{
			return this._targetAgent != null && this._isInDialogueRange && this._isCharacterToTalkTo;
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x000E9F9D File Offset: 0x000E819D
		public void Reset()
		{
			this._targetAgent = null;
			this._isInDialogueRange = false;
		}

		// Token: 0x04001862 RID: 6242
		private bool _isInDialogueRange;

		// Token: 0x04001863 RID: 6243
		private readonly bool _isCharacterToTalkTo;

		// Token: 0x04001864 RID: 6244
		private readonly float _dialogueTriggerProximity = 10f;

		// Token: 0x04001865 RID: 6245
		private readonly float _agentSpeedLimit;

		// Token: 0x04001866 RID: 6246
		private Agent _targetAgent;
	}
}
