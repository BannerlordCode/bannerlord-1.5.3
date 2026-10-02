using System;
using SandBox.Missions.AgentBehaviors;
using SandBox.Objects.Usables;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Source.Missions.AgentBehaviors
{
	// Token: 0x02000056 RID: 86
	public class BoardGameAgentBehavior : AgentBehavior
	{
		// Token: 0x0600036B RID: 875 RVA: 0x00014160 File Offset: 0x00012360
		public BoardGameAgentBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0001416C File Offset: 0x0001236C
		public override void Tick(float dt, bool isSimulation)
		{
			switch (this._state)
			{
			case BoardGameAgentBehavior.State.Idle:
				if (base.Navigator.TargetUsableMachine != this._chair && !this._chair.IsAgentFullySitting(base.OwnerAgent))
				{
					base.Navigator.SetTarget(this._chair, false, Agent.AIScriptedFrameFlags.None);
					this._state = BoardGameAgentBehavior.State.MovingToChair;
					return;
				}
				break;
			case BoardGameAgentBehavior.State.MovingToChair:
				if (this._chair.IsAgentFullySitting(base.OwnerAgent))
				{
					this._state = BoardGameAgentBehavior.State.Idle;
					return;
				}
				break;
			case BoardGameAgentBehavior.State.Finish:
				if (base.OwnerAgent.IsUsingGameObject && this._waitTimer == null)
				{
					base.Navigator.ClearTarget();
					this._waitTimer = new Timer(base.Mission.CurrentTime, 3f, true);
					return;
				}
				if (this._waitTimer != null)
				{
					if (this._waitTimer.Check(base.Mission.CurrentTime))
					{
						this.RemoveBoardGameBehaviorInternal();
						return;
					}
				}
				else
				{
					this.RemoveBoardGameBehaviorInternal();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0001425F File Offset: 0x0001245F
		public override void ConversationTick()
		{
			base.Navigator.ClearTarget();
			this._state = BoardGameAgentBehavior.State.Idle;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00014273 File Offset: 0x00012473
		protected override void OnDeactivate()
		{
			base.Navigator.ClearTarget();
			this._chair = null;
			this._state = BoardGameAgentBehavior.State.Idle;
			this._waitTimer = null;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00014295 File Offset: 0x00012495
		public override string GetDebugInfo()
		{
			return "BoardGameAgentBehavior";
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0001429C File Offset: 0x0001249C
		public override float GetAvailability(bool isSimulation)
		{
			return 1f;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x000142A4 File Offset: 0x000124A4
		private void RemoveBoardGameBehaviorInternal()
		{
			InterruptingBehaviorGroup behaviorGroup = base.OwnerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<InterruptingBehaviorGroup>();
			if (behaviorGroup.GetBehavior<BoardGameAgentBehavior>() != null)
			{
				behaviorGroup.RemoveBehavior<BoardGameAgentBehavior>();
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x000142D8 File Offset: 0x000124D8
		public static void AddTargetChair(Agent ownerAgent, Chair chair)
		{
			InterruptingBehaviorGroup behaviorGroup = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<InterruptingBehaviorGroup>();
			bool flag = behaviorGroup.GetBehavior<BoardGameAgentBehavior>() == null;
			BoardGameAgentBehavior boardGameAgentBehavior = behaviorGroup.GetBehavior<BoardGameAgentBehavior>() ?? behaviorGroup.AddBehavior<BoardGameAgentBehavior>();
			boardGameAgentBehavior._chair = chair;
			boardGameAgentBehavior._state = BoardGameAgentBehavior.State.Idle;
			boardGameAgentBehavior._waitTimer = null;
			if (flag)
			{
				behaviorGroup.SetScriptedBehavior<BoardGameAgentBehavior>();
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0001432C File Offset: 0x0001252C
		public static void RemoveBoardGameBehaviorOfAgent(Agent ownerAgent)
		{
			BoardGameAgentBehavior behavior = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<InterruptingBehaviorGroup>().GetBehavior<BoardGameAgentBehavior>();
			if (behavior != null)
			{
				behavior._chair = null;
				behavior._state = BoardGameAgentBehavior.State.Finish;
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00014360 File Offset: 0x00012560
		public static bool IsAgentMovingToChair(Agent ownerAgent)
		{
			if (ownerAgent == null)
			{
				return false;
			}
			InterruptingBehaviorGroup behaviorGroup = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<InterruptingBehaviorGroup>();
			BoardGameAgentBehavior boardGameAgentBehavior = ((behaviorGroup != null) ? behaviorGroup.GetBehavior<BoardGameAgentBehavior>() : null);
			return boardGameAgentBehavior != null && boardGameAgentBehavior._state == BoardGameAgentBehavior.State.MovingToChair;
		}

		// Token: 0x040001B6 RID: 438
		private const int FinishDelayAsSeconds = 3;

		// Token: 0x040001B7 RID: 439
		private Chair _chair;

		// Token: 0x040001B8 RID: 440
		private BoardGameAgentBehavior.State _state;

		// Token: 0x040001B9 RID: 441
		private Timer _waitTimer;

		// Token: 0x02000164 RID: 356
		private enum State
		{
			// Token: 0x040006DC RID: 1756
			Idle,
			// Token: 0x040006DD RID: 1757
			MovingToChair,
			// Token: 0x040006DE RID: 1758
			Finish
		}
	}
}
