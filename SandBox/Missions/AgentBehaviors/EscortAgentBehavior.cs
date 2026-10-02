using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000AA RID: 170
	public class EscortAgentBehavior : AgentBehavior
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x0002FAF7 File Offset: 0x0002DCF7
		public Agent EscortedAgent
		{
			get
			{
				return this._escortedAgent;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0002FAFF File Offset: 0x0002DCFF
		public Agent TargetAgent
		{
			get
			{
				return this._targetAgent;
			}
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0002FB07 File Offset: 0x0002DD07
		public EscortAgentBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._targetAgent = null;
			this._escortedAgent = null;
			this._myLastStateWasRunning = false;
			this._initialMaxSpeedLimit = 1f;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0002FB30 File Offset: 0x0002DD30
		public void Initialize(Agent escortedAgent, Agent targetAgent, EscortAgentBehavior.OnTargetReachedDelegate onTargetReached = null)
		{
			this._escortedAgent = escortedAgent;
			this._targetAgent = targetAgent;
			this._targetMachine = null;
			this._targetPosition = null;
			this._onTargetReached = onTargetReached;
			this._escortFinished = false;
			this._initialMaxSpeedLimit = base.OwnerAgent.GetMaximumSpeedLimit();
			this._state = EscortAgentBehavior.State.Escorting;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0002FB84 File Offset: 0x0002DD84
		public void Initialize(Agent escortedAgent, UsableMachine targetMachine, EscortAgentBehavior.OnTargetReachedDelegate onTargetReached = null)
		{
			this._escortedAgent = escortedAgent;
			this._targetAgent = null;
			this._targetMachine = targetMachine;
			this._targetPosition = null;
			this._onTargetReached = onTargetReached;
			this._escortFinished = false;
			this._initialMaxSpeedLimit = base.OwnerAgent.GetMaximumSpeedLimit();
			this._state = EscortAgentBehavior.State.Escorting;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0002FBD8 File Offset: 0x0002DDD8
		public void Initialize(Agent escortedAgent, Vec3? targetPosition, EscortAgentBehavior.OnTargetReachedDelegate onTargetReached = null)
		{
			this._escortedAgent = escortedAgent;
			this._targetAgent = null;
			this._targetMachine = null;
			this._targetPosition = targetPosition;
			this._onTargetReached = onTargetReached;
			this._escortFinished = false;
			this._initialMaxSpeedLimit = base.OwnerAgent.GetMaximumSpeedLimit();
			this._state = EscortAgentBehavior.State.Escorting;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0002FC28 File Offset: 0x0002DE28
		public override void Tick(float dt, bool isSimulation)
		{
			if (this._escortedAgent == null || !this._escortedAgent.IsActive() || this._targetAgent == null || !this._targetAgent.IsActive())
			{
				this._state = EscortAgentBehavior.State.NotEscorting;
			}
			if (this._escortedAgent != null && this._state != EscortAgentBehavior.State.NotEscorting)
			{
				this.ControlMovement();
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0002FC7C File Offset: 0x0002DE7C
		public bool IsEscortFinished()
		{
			return this._escortFinished;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0002FC84 File Offset: 0x0002DE84
		private void ControlMovement()
		{
			int nearbyEnemyAgentCount = base.Mission.GetNearbyEnemyAgentCount(this._escortedAgent.Team, this._escortedAgent.Position.AsVec2, 5f);
			if (this._state != EscortAgentBehavior.State.NotEscorting && nearbyEnemyAgentCount > 0)
			{
				this._state = EscortAgentBehavior.State.NotEscorting;
				base.OwnerAgent.ResetLookAgent();
				base.Navigator.ClearTarget();
				base.OwnerAgent.DisableScriptedMovement();
				base.OwnerAgent.SetMaximumSpeedLimit(this._initialMaxSpeedLimit, false);
				Debug.Print("[Escort agent behavior] Escorted agent got into a fight... Disable!", 0, Debug.DebugColor.White, 17592186044416UL);
				return;
			}
			float num = (base.OwnerAgent.HasMount ? 2.2f : 1.2f);
			float num2 = base.OwnerAgent.Position.DistanceSquared(this._escortedAgent.Position);
			float num3;
			WorldPosition worldPosition;
			float num4;
			if (this._targetAgent != null)
			{
				num3 = base.OwnerAgent.Position.DistanceSquared(this._targetAgent.Position);
				worldPosition = this._targetAgent.GetWorldPosition();
				MatrixFrame matrixFrame = this._targetAgent.Frame;
				num4 = matrixFrame.rotation.f.AsVec2.RotationInRadians;
			}
			else if (this._targetMachine != null)
			{
				MatrixFrame globalFrame = this._targetMachine.GameEntity.GetGlobalFrame();
				num3 = base.OwnerAgent.Position.DistanceSquared(globalFrame.origin);
				worldPosition = globalFrame.origin.ToWorldPosition();
				num4 = globalFrame.rotation.f.AsVec2.RotationInRadians;
			}
			else if (this._targetPosition != null)
			{
				num3 = base.OwnerAgent.Position.DistanceSquared(this._targetPosition.Value);
				worldPosition = this._targetPosition.Value.ToWorldPosition();
				num4 = (this._targetPosition.Value - base.OwnerAgent.Position).AsVec2.RotationInRadians;
			}
			else
			{
				Debug.FailedAssert("At least one target must be specified for the escort behavior.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\AgentBehaviors\\EscortAgentBehavior.cs", "ControlMovement", 160);
				num3 = 0f;
				worldPosition = base.OwnerAgent.GetWorldPosition();
				num4 = 0f;
			}
			if (this._escortFinished)
			{
				bool flag = false;
				base.OwnerAgent.SetMaximumSpeedLimit(this._initialMaxSpeedLimit, false);
				if (this._onTargetReached != null)
				{
					flag = this._onTargetReached(base.OwnerAgent, ref this._escortedAgent, ref this._targetAgent, ref this._targetMachine, ref this._targetPosition);
				}
				if (flag && this._escortedAgent != null && (this._targetAgent != null || this._targetMachine != null || this._targetPosition != null))
				{
					this._state = EscortAgentBehavior.State.Escorting;
				}
				else
				{
					this._state = EscortAgentBehavior.State.NotEscorting;
				}
			}
			switch (this._state)
			{
			case EscortAgentBehavior.State.ReturnToEscortedAgent:
				if (num2 < 25f)
				{
					this._state = EscortAgentBehavior.State.Wait;
				}
				else
				{
					WorldPosition worldPosition2 = this._escortedAgent.GetWorldPosition();
					MatrixFrame matrixFrame = this._escortedAgent.Frame;
					this.SetMovePos(worldPosition2, matrixFrame.rotation.f.AsVec2.RotationInRadians, num);
				}
				break;
			case EscortAgentBehavior.State.Wait:
				if (num2 < 25f)
				{
					this._state = EscortAgentBehavior.State.Escorting;
					Debug.Print("[Escort agent behavior] Escorting!", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				else if (num2 > 100f)
				{
					this._state = EscortAgentBehavior.State.ReturnToEscortedAgent;
					Debug.Print("[Escort agent behavior] Escorted agent is too far away! Return to escorted agent!", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				else
				{
					WorldPosition worldPosition3 = base.OwnerAgent.GetWorldPosition();
					MatrixFrame matrixFrame = base.OwnerAgent.Frame;
					this.SetMovePos(worldPosition3, matrixFrame.rotation.f.AsVec2.RotationInRadians, 0f);
				}
				break;
			case EscortAgentBehavior.State.Escorting:
				if (num2 >= 25f)
				{
					this._state = EscortAgentBehavior.State.Wait;
					Debug.Print("[Escort agent behavior] Stop walking! Wait", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				else
				{
					this.SetMovePos(worldPosition, num4, 3f);
				}
				break;
			}
			if (this._state == EscortAgentBehavior.State.Escorting && num3 < 16f && num2 < 16f && !Campaign.Current.ConversationManager.IsConversationInProgress)
			{
				this._escortFinished = true;
			}
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x000300C0 File Offset: 0x0002E2C0
		private void SetMovePos(WorldPosition targetPosition, float targetRotation, float rangeThreshold)
		{
			Agent.AIScriptedFrameFlags aiscriptedFrameFlags = Agent.AIScriptedFrameFlags.NoAttack;
			if (base.Navigator.CharacterHasVisiblePrefabs)
			{
				this._myLastStateWasRunning = false;
			}
			else
			{
				float num = base.OwnerAgent.Position.AsVec2.Distance(targetPosition.AsVec2);
				float length = this._escortedAgent.Velocity.AsVec2.Length;
				if (num - rangeThreshold <= 0.5f * (this._myLastStateWasRunning ? 1f : 1.2f) && length <= base.OwnerAgent.Monster.WalkingSpeedLimit * (this._myLastStateWasRunning ? 1f : 1.2f))
				{
					this._myLastStateWasRunning = false;
				}
				else
				{
					base.OwnerAgent.SetMaximumSpeedLimit(num - rangeThreshold + length, false);
					this._myLastStateWasRunning = true;
				}
			}
			if (!this._myLastStateWasRunning)
			{
				aiscriptedFrameFlags |= Agent.AIScriptedFrameFlags.DoNotRun;
			}
			base.Navigator.SetTargetFrame(targetPosition, targetRotation, rangeThreshold, -10f, aiscriptedFrameFlags, false);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x000301B3 File Offset: 0x0002E3B3
		public override float GetAvailability(bool isSimulation)
		{
			return (float)((this._state == EscortAgentBehavior.State.NotEscorting) ? 0 : 1);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000301C4 File Offset: 0x0002E3C4
		protected override void OnDeactivate()
		{
			this._escortedAgent = null;
			this._targetAgent = null;
			this._targetMachine = null;
			this._targetPosition = null;
			this._onTargetReached = null;
			this._state = EscortAgentBehavior.State.NotEscorting;
			base.OwnerAgent.DisableScriptedMovement();
			base.OwnerAgent.ResetLookAgent();
			base.Navigator.ClearTarget();
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00030224 File Offset: 0x0002E424
		public override string GetDebugInfo()
		{
			return string.Concat(new object[]
			{
				"Escort ",
				this._escortedAgent.Name,
				" (id:",
				this._escortedAgent.Index,
				")",
				(this._targetAgent != null) ? string.Concat(new object[]
				{
					" to ",
					this._targetAgent.Name,
					" (id:",
					this._targetAgent.Index,
					")"
				}) : ((this._targetMachine != null) ? string.Concat(new object[]
				{
					" to ",
					this._targetMachine,
					"(id:",
					this._targetMachine.Id,
					")"
				}) : ((this._targetPosition != null) ? (" to position: " + this._targetPosition.Value) : " to NO TARGET"))
			});
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00030344 File Offset: 0x0002E544
		public static void AddEscortAgentBehavior(Agent ownerAgent, Agent targetAgent, EscortAgentBehavior.OnTargetReachedDelegate onTargetReached)
		{
			AgentNavigator agentNavigator = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator;
			InterruptingBehaviorGroup interruptingBehaviorGroup = ((agentNavigator != null) ? agentNavigator.GetBehaviorGroup<InterruptingBehaviorGroup>() : null);
			if (interruptingBehaviorGroup == null)
			{
				return;
			}
			bool flag = interruptingBehaviorGroup.GetBehavior<EscortAgentBehavior>() == null;
			EscortAgentBehavior escortAgentBehavior = interruptingBehaviorGroup.GetBehavior<EscortAgentBehavior>() ?? interruptingBehaviorGroup.AddBehavior<EscortAgentBehavior>();
			if (flag)
			{
				interruptingBehaviorGroup.SetScriptedBehavior<EscortAgentBehavior>();
			}
			escortAgentBehavior.Initialize(Agent.Main, targetAgent, onTargetReached);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0003039C File Offset: 0x0002E59C
		public static void RemoveEscortBehaviorOfAgent(Agent ownerAgent)
		{
			AgentNavigator agentNavigator = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator;
			InterruptingBehaviorGroup interruptingBehaviorGroup = ((agentNavigator != null) ? agentNavigator.GetBehaviorGroup<InterruptingBehaviorGroup>() : null);
			if (interruptingBehaviorGroup == null)
			{
				return;
			}
			if (interruptingBehaviorGroup.GetBehavior<EscortAgentBehavior>() != null)
			{
				interruptingBehaviorGroup.RemoveBehavior<EscortAgentBehavior>();
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x000303D4 File Offset: 0x0002E5D4
		public static bool CheckIfAgentIsEscortedBy(Agent ownerAgent, Agent escortedAgent)
		{
			AgentNavigator agentNavigator = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator;
			InterruptingBehaviorGroup interruptingBehaviorGroup = ((agentNavigator != null) ? agentNavigator.GetBehaviorGroup<InterruptingBehaviorGroup>() : null);
			EscortAgentBehavior escortAgentBehavior = ((interruptingBehaviorGroup != null) ? interruptingBehaviorGroup.GetBehavior<EscortAgentBehavior>() : null);
			return escortAgentBehavior != null && escortAgentBehavior.EscortedAgent == escortedAgent;
		}

		// Token: 0x040003B5 RID: 949
		private const float StartWaitingDistanceSquared = 25f;

		// Token: 0x040003B6 RID: 950
		private const float ReturnToEscortedAgentDistanceSquared = 100f;

		// Token: 0x040003B7 RID: 951
		private const float EscortFinishedDistanceSquared = 16f;

		// Token: 0x040003B8 RID: 952
		private const float TargetProximityThreshold = 3f;

		// Token: 0x040003B9 RID: 953
		private const float MountedMoveProximityThreshold = 2.2f;

		// Token: 0x040003BA RID: 954
		private const float OnFootMoveProximityThreshold = 1.2f;

		// Token: 0x040003BB RID: 955
		private EscortAgentBehavior.State _state;

		// Token: 0x040003BC RID: 956
		private Agent _escortedAgent;

		// Token: 0x040003BD RID: 957
		private Agent _targetAgent;

		// Token: 0x040003BE RID: 958
		private UsableMachine _targetMachine;

		// Token: 0x040003BF RID: 959
		private Vec3? _targetPosition;

		// Token: 0x040003C0 RID: 960
		private bool _myLastStateWasRunning;

		// Token: 0x040003C1 RID: 961
		private float _initialMaxSpeedLimit;

		// Token: 0x040003C2 RID: 962
		private EscortAgentBehavior.OnTargetReachedDelegate _onTargetReached;

		// Token: 0x040003C3 RID: 963
		private bool _escortFinished;

		// Token: 0x020001B5 RID: 437
		// (Invoke) Token: 0x06000F6E RID: 3950
		public delegate bool OnTargetReachedDelegate(Agent agent, ref Agent escortedAgent, ref Agent targetAgent, ref UsableMachine targetMachine, ref Vec3? targetPosition);

		// Token: 0x020001B6 RID: 438
		private enum State
		{
			// Token: 0x040007F6 RID: 2038
			NotEscorting,
			// Token: 0x040007F7 RID: 2039
			ReturnToEscortedAgent,
			// Token: 0x040007F8 RID: 2040
			Wait,
			// Token: 0x040007F9 RID: 2041
			Escorting
		}
	}
}
