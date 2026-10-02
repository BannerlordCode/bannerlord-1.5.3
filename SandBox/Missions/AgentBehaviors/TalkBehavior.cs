using System;
using SandBox.Conversation.MissionLogics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000B5 RID: 181
	public class TalkBehavior : AgentBehavior
	{
		// Token: 0x06000784 RID: 1924 RVA: 0x00033016 File Offset: 0x00031216
		public TalkBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._startConversation = true;
			this._doNotMove = true;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00033030 File Offset: 0x00031230
		public override void Tick(float dt, bool isSimulation)
		{
			if (!this._startConversation || base.Mission.MainAgent == null || !base.Mission.MainAgent.IsActive() || base.Mission.Mode == MissionMode.Conversation || base.Mission.Mode == MissionMode.Battle || base.Mission.Mode == MissionMode.Barter)
			{
				return;
			}
			float interactionDistanceToUsable = base.OwnerAgent.GetInteractionDistanceToUsable(base.Mission.MainAgent);
			if (base.OwnerAgent.Position.DistanceSquared(base.Mission.MainAgent.Position) < (interactionDistanceToUsable + 3f) * (interactionDistanceToUsable + 3f) && base.Navigator.CanSeeAgent(base.Mission.MainAgent))
			{
				AgentNavigator navigator = base.Navigator;
				WorldPosition worldPosition = base.OwnerAgent.GetWorldPosition();
				MatrixFrame matrixFrame = base.OwnerAgent.Frame;
				navigator.SetTargetFrame(worldPosition, matrixFrame.rotation.f.AsVec2.RotationInRadians, 1f, -10f, Agent.AIScriptedFrameFlags.DoNotRun, false);
				MissionConversationLogic missionBehavior = base.Mission.GetMissionBehavior<MissionConversationLogic>();
				if (missionBehavior != null && missionBehavior.IsReadyForConversation)
				{
					missionBehavior.OnAgentInteraction(base.Mission.MainAgent, base.OwnerAgent, -1);
					this._startConversation = false;
					return;
				}
			}
			else if (!this._doNotMove)
			{
				AgentNavigator navigator2 = base.Navigator;
				WorldPosition worldPosition2 = Agent.Main.GetWorldPosition();
				MatrixFrame matrixFrame = Agent.Main.Frame;
				navigator2.SetTargetFrame(worldPosition2, matrixFrame.rotation.f.AsVec2.RotationInRadians, 1f, -10f, Agent.AIScriptedFrameFlags.DoNotRun, false);
			}
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x000331C8 File Offset: 0x000313C8
		public override float GetAvailability(bool isSimulation)
		{
			if (isSimulation)
			{
				return 0f;
			}
			if (this._startConversation && base.Mission.MainAgent != null && base.Mission.MainAgent.IsActive())
			{
				float num = base.OwnerAgent.GetInteractionDistanceToUsable(base.Mission.MainAgent) + 3f;
				if (base.OwnerAgent.Position.DistanceSquared(base.Mission.MainAgent.Position) < num * num && base.Mission.Mode != MissionMode.Conversation && !base.Mission.MainAgent.IsEnemyOf(base.OwnerAgent))
				{
					return 1f;
				}
			}
			return 0f;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00033281 File Offset: 0x00031481
		public override string GetDebugInfo()
		{
			return "Talk";
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00033288 File Offset: 0x00031488
		protected override void OnDeactivate()
		{
			base.Navigator.ClearTarget();
			this.Disable();
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0003329B File Offset: 0x0003149B
		public void Disable()
		{
			this._startConversation = false;
			this._doNotMove = true;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x000332AB File Offset: 0x000314AB
		public void Enable(bool doNotMove)
		{
			this._startConversation = true;
			this._doNotMove = doNotMove;
		}

		// Token: 0x04000407 RID: 1031
		private bool _doNotMove;

		// Token: 0x04000408 RID: 1032
		private bool _startConversation;
	}
}
