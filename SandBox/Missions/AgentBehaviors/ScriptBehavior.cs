using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000B3 RID: 179
	public class ScriptBehavior : AgentBehavior
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x00032831 File Offset: 0x00030A31
		public ScriptBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00032850 File Offset: 0x00030A50
		public static void AddUsableMachineTarget(Agent ownerAgent, UsableMachine targetUsableMachine)
		{
			DailyBehaviorGroup behaviorGroup = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			ScriptBehavior scriptBehavior = behaviorGroup.GetBehavior<ScriptBehavior>() ?? behaviorGroup.AddBehavior<ScriptBehavior>();
			bool flag = behaviorGroup.ScriptedBehavior != scriptBehavior;
			scriptBehavior._targetUsableMachine = targetUsableMachine;
			scriptBehavior._state = ScriptBehavior.State.GoToUsableMachine;
			scriptBehavior._sentToTarget = false;
			if (flag)
			{
				behaviorGroup.SetScriptedBehavior<ScriptBehavior>();
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000328A8 File Offset: 0x00030AA8
		public static void AddAgentTarget(Agent ownerAgent, Agent targetAgent)
		{
			DailyBehaviorGroup behaviorGroup = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			ScriptBehavior scriptBehavior = behaviorGroup.GetBehavior<ScriptBehavior>() ?? behaviorGroup.AddBehavior<ScriptBehavior>();
			bool flag = behaviorGroup.ScriptedBehavior != scriptBehavior;
			scriptBehavior._targetAgent = targetAgent;
			scriptBehavior._state = ScriptBehavior.State.GoToAgent;
			scriptBehavior._sentToTarget = false;
			if (flag)
			{
				behaviorGroup.SetScriptedBehavior<ScriptBehavior>();
			}
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00032900 File Offset: 0x00030B00
		public static void AddWorldFrameTarget(Agent ownerAgent, WorldFrame targetWorldFrame)
		{
			DailyBehaviorGroup behaviorGroup = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			ScriptBehavior scriptBehavior = behaviorGroup.GetBehavior<ScriptBehavior>() ?? behaviorGroup.AddBehavior<ScriptBehavior>();
			bool flag = behaviorGroup.ScriptedBehavior != scriptBehavior;
			scriptBehavior._targetFrame = targetWorldFrame;
			scriptBehavior._state = ScriptBehavior.State.GoToTargetFrame;
			scriptBehavior._sentToTarget = false;
			if (flag)
			{
				behaviorGroup.SetScriptedBehavior<ScriptBehavior>();
			}
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00032958 File Offset: 0x00030B58
		public static void AddTargetWithDelegate(Agent ownerAgent, ScriptBehavior.SelectTargetDelegate selectTargetDelegate, ScriptBehavior.OnTargetReachedWaitDelegate onTargetReachWaitDelegate, ScriptBehavior.OnTargetReachedDelegate onTargetReachedDelegate, float initialWaitInSeconds = 0f)
		{
			DailyBehaviorGroup behaviorGroup = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			ScriptBehavior scriptBehavior = behaviorGroup.GetBehavior<ScriptBehavior>() ?? behaviorGroup.AddBehavior<ScriptBehavior>();
			bool flag = behaviorGroup.ScriptedBehavior != scriptBehavior;
			scriptBehavior._selectTargetDelegate = selectTargetDelegate;
			scriptBehavior._onTargetReachedDelegate = onTargetReachedDelegate;
			scriptBehavior._onTargetReachWaitDelegate = onTargetReachWaitDelegate;
			scriptBehavior._initialWaitInSeconds = initialWaitInSeconds;
			scriptBehavior._isInitiallyWaiting = initialWaitInSeconds > 0f;
			scriptBehavior._state = ScriptBehavior.State.NoTarget;
			scriptBehavior._sentToTarget = false;
			if (flag)
			{
				behaviorGroup.SetScriptedBehavior<ScriptBehavior>();
			}
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x000329D5 File Offset: 0x00030BD5
		public bool IsNearTarget(Agent targetAgent)
		{
			return this._targetAgent == targetAgent && (this._state == ScriptBehavior.State.NearAgent || this._state == ScriptBehavior.State.NearStationaryTarget);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x000329F8 File Offset: 0x00030BF8
		public override void Tick(float dt, bool isSimulation)
		{
			if (this._isInitiallyWaiting)
			{
				if (this._waitTimer == null)
				{
					this._waitTimer = new MissionTimer(this._initialWaitInSeconds);
					return;
				}
				if (this._waitTimer.Check(false))
				{
					this._isInitiallyWaiting = false;
					this._waitTimer = null;
					return;
				}
			}
			else
			{
				if (this._state == ScriptBehavior.State.NoTarget)
				{
					if (this._selectTargetDelegate == null)
					{
						if (this.BehaviorGroup.ScriptedBehavior == this)
						{
							this.BehaviorGroup.DisableScriptedBehavior();
						}
						return;
					}
					this.SearchForNewTarget();
				}
				switch (this._state)
				{
				case ScriptBehavior.State.GoToUsableMachine:
					if (!this._sentToTarget)
					{
						base.Navigator.SetTarget(this._targetUsableMachine, false, Agent.AIScriptedFrameFlags.None);
						this._sentToTarget = true;
						return;
					}
					if (base.OwnerAgent.IsUsingGameObject && base.OwnerAgent.Position.DistanceSquared(this._targetUsableMachine.GameEntity.GetGlobalFrame().origin) < 1f)
					{
						if (this.CheckForSearchNewTarget(ScriptBehavior.State.NearStationaryTarget))
						{
							base.OwnerAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							return;
						}
						this.RemoveTargets();
						return;
					}
					break;
				case ScriptBehavior.State.GoToAgent:
					if (this._targetAgent.IsActive())
					{
						float interactionDistanceToUsable = base.OwnerAgent.GetInteractionDistanceToUsable(this._targetAgent);
						if (base.OwnerAgent.Position.DistanceSquared(this._targetAgent.Position) >= interactionDistanceToUsable * interactionDistanceToUsable)
						{
							AgentNavigator navigator = base.Navigator;
							WorldPosition worldPosition = this._targetAgent.GetWorldPosition();
							MatrixFrame matrixFrame = this._targetAgent.Frame;
							navigator.SetTargetFrame(worldPosition, matrixFrame.rotation.f.AsVec2.RotationInRadians, this._customTargetReachedRangeThreshold, this._customTargetReachedRotationThreshold, Agent.AIScriptedFrameFlags.None, false);
							return;
						}
						if (!this.CheckForSearchNewTarget(ScriptBehavior.State.NearAgent))
						{
							AgentNavigator navigator2 = base.Navigator;
							WorldPosition worldPosition2 = base.OwnerAgent.GetWorldPosition();
							MatrixFrame matrixFrame = base.OwnerAgent.Frame;
							navigator2.SetTargetFrame(worldPosition2, matrixFrame.rotation.f.AsVec2.RotationInRadians, this._customTargetReachedRangeThreshold, this._customTargetReachedRotationThreshold, Agent.AIScriptedFrameFlags.None, false);
							this.RemoveTargets();
							return;
						}
					}
					else if (!this.CheckForSearchNewTarget(ScriptBehavior.State.NearAgent))
					{
						AgentNavigator navigator3 = base.Navigator;
						WorldPosition worldPosition3 = base.OwnerAgent.GetWorldPosition();
						MatrixFrame matrixFrame = base.OwnerAgent.Frame;
						navigator3.SetTargetFrame(worldPosition3, matrixFrame.rotation.f.AsVec2.RotationInRadians, this._customTargetReachedRangeThreshold, this._customTargetReachedRotationThreshold, Agent.AIScriptedFrameFlags.None, false);
						this.RemoveTargets();
						return;
					}
					break;
				case ScriptBehavior.State.GoToTargetFrame:
					if (!this._sentToTarget)
					{
						base.Navigator.SetTargetFrame(this._targetFrame.Origin, this._targetFrame.Rotation.f.AsVec2.RotationInRadians, this._customTargetReachedRangeThreshold, this._customTargetReachedRotationThreshold, Agent.AIScriptedFrameFlags.DoNotRun, false);
						this._sentToTarget = true;
						return;
					}
					if (base.Navigator.IsTargetReached() && !this.CheckForSearchNewTarget(ScriptBehavior.State.NearStationaryTarget) && this._waitTimer == null)
					{
						this.RemoveTargets();
						return;
					}
					break;
				case ScriptBehavior.State.NearAgent:
				{
					if (base.OwnerAgent.Position.DistanceSquared(this._targetAgent.Position) >= 1f)
					{
						this._state = ScriptBehavior.State.GoToAgent;
						return;
					}
					AgentNavigator navigator4 = base.Navigator;
					WorldPosition worldPosition4 = base.OwnerAgent.GetWorldPosition();
					MatrixFrame matrixFrame = base.OwnerAgent.Frame;
					navigator4.SetTargetFrame(worldPosition4, matrixFrame.rotation.f.AsVec2.RotationInRadians, this._customTargetReachedRangeThreshold, this._customTargetReachedRotationThreshold, Agent.AIScriptedFrameFlags.None, false);
					this.RemoveTargets();
					break;
				}
				default:
					return;
				}
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00032D6C File Offset: 0x00030F6C
		private bool CheckForSearchNewTarget(ScriptBehavior.State endState)
		{
			bool flag = false;
			bool flag2 = false;
			if (this._onTargetReachWaitDelegate != null && !this._isWaiting)
			{
				this._onTargetReachWaitDelegate(base.OwnerAgent, ref this._waitTimeInSeconds);
				this._isWaiting = this._waitTimeInSeconds > 0f;
			}
			if (this._isWaiting)
			{
				if (this._waitTimer == null)
				{
					this._waitTimer = new MissionTimer(this._waitTimeInSeconds);
				}
				else if (this._waitTimer.Check(false))
				{
					this._isWaiting = false;
					this._waitTimer = null;
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				if (this._onTargetReachedDelegate != null)
				{
					flag2 = this._onTargetReachedDelegate(base.OwnerAgent, ref this._targetAgent, ref this._targetUsableMachine, ref this._targetFrame);
				}
				if (flag2)
				{
					this.SearchForNewTarget();
				}
				else
				{
					this._state = endState;
				}
				return flag2;
			}
			return false;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00032E40 File Offset: 0x00031040
		private void SearchForNewTarget()
		{
			Agent agent = null;
			UsableMachine usableMachine = null;
			WorldFrame invalid = WorldFrame.Invalid;
			float customTargetReachedRangeThreshold = this._customTargetReachedRangeThreshold;
			float customTargetReachedRotationThreshold = this._customTargetReachedRotationThreshold;
			if (this._selectTargetDelegate(base.OwnerAgent, ref agent, ref usableMachine, ref invalid, ref customTargetReachedRangeThreshold, ref customTargetReachedRotationThreshold))
			{
				if (agent != null)
				{
					this._targetAgent = agent;
					this._state = ScriptBehavior.State.GoToAgent;
					this._sentToTarget = false;
				}
				else if (usableMachine != null)
				{
					this._targetUsableMachine = usableMachine;
					this._state = ScriptBehavior.State.GoToUsableMachine;
					this._sentToTarget = false;
				}
				else
				{
					this._targetFrame = invalid;
					this._state = ScriptBehavior.State.GoToTargetFrame;
					this._sentToTarget = false;
				}
				this._customTargetReachedRangeThreshold = customTargetReachedRangeThreshold;
				this._customTargetReachedRotationThreshold = customTargetReachedRotationThreshold;
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00032EDB File Offset: 0x000310DB
		public override float GetAvailability(bool isSimulation)
		{
			return (float)((this._state == ScriptBehavior.State.NoTarget) ? 0 : 1);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00032EEA File Offset: 0x000310EA
		protected override void OnDeactivate()
		{
			base.Navigator.ClearTarget();
			this.RemoveTargets();
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00032EFD File Offset: 0x000310FD
		private void RemoveTargets()
		{
			this._targetUsableMachine = null;
			this._targetAgent = null;
			this._targetFrame = WorldFrame.Invalid;
			this._state = ScriptBehavior.State.NoTarget;
			this._selectTargetDelegate = null;
			this._onTargetReachedDelegate = null;
			this._sentToTarget = false;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00032F34 File Offset: 0x00031134
		public override string GetDebugInfo()
		{
			return "Scripted";
		}

		// Token: 0x040003F5 RID: 1013
		private UsableMachine _targetUsableMachine;

		// Token: 0x040003F6 RID: 1014
		private Agent _targetAgent;

		// Token: 0x040003F7 RID: 1015
		private WorldFrame _targetFrame;

		// Token: 0x040003F8 RID: 1016
		private ScriptBehavior.State _state;

		// Token: 0x040003F9 RID: 1017
		private bool _sentToTarget;

		// Token: 0x040003FA RID: 1018
		private float _waitTimeInSeconds;

		// Token: 0x040003FB RID: 1019
		private bool _isWaiting;

		// Token: 0x040003FC RID: 1020
		private MissionTimer _waitTimer;

		// Token: 0x040003FD RID: 1021
		private float _customTargetReachedRangeThreshold = 1f;

		// Token: 0x040003FE RID: 1022
		private float _customTargetReachedRotationThreshold = 1f;

		// Token: 0x040003FF RID: 1023
		private float _initialWaitInSeconds;

		// Token: 0x04000400 RID: 1024
		private bool _isInitiallyWaiting;

		// Token: 0x04000401 RID: 1025
		private ScriptBehavior.SelectTargetDelegate _selectTargetDelegate;

		// Token: 0x04000402 RID: 1026
		private ScriptBehavior.OnTargetReachedDelegate _onTargetReachedDelegate;

		// Token: 0x04000403 RID: 1027
		private ScriptBehavior.OnTargetReachedWaitDelegate _onTargetReachWaitDelegate;

		// Token: 0x020001C2 RID: 450
		// (Invoke) Token: 0x06000F9C RID: 3996
		public delegate bool SelectTargetDelegate(Agent agent, ref Agent targetAgent, ref UsableMachine targetUsableMachine, ref WorldFrame targetFrame, ref float customTargetReachedRangeThreshold, ref float customTargetReachedRotationThreshold);

		// Token: 0x020001C3 RID: 451
		// (Invoke) Token: 0x06000FA0 RID: 4000
		public delegate bool OnTargetReachedDelegate(Agent agent, ref Agent targetAgent, ref UsableMachine targetUsableMachine, ref WorldFrame targetFrame);

		// Token: 0x020001C4 RID: 452
		// (Invoke) Token: 0x06000FA4 RID: 4004
		public delegate void OnTargetReachedWaitDelegate(Agent agent, ref float waitTimeInSeconds);

		// Token: 0x020001C5 RID: 453
		private enum State
		{
			// Token: 0x04000816 RID: 2070
			NoTarget,
			// Token: 0x04000817 RID: 2071
			GoToUsableMachine,
			// Token: 0x04000818 RID: 2072
			GoToAgent,
			// Token: 0x04000819 RID: 2073
			GoToTargetFrame,
			// Token: 0x0400081A RID: 2074
			NearAgent,
			// Token: 0x0400081B RID: 2075
			NearStationaryTarget
		}
	}
}
