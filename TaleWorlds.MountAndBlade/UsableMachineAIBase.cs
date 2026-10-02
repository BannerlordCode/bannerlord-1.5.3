using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000167 RID: 359
	public abstract class UsableMachineAIBase
	{
		// Token: 0x0600128F RID: 4751 RVA: 0x0003A197 File Offset: 0x00038397
		protected UsableMachineAIBase(UsableMachine usableMachine)
		{
			this.UsableMachine = usableMachine;
			this._lastActiveWaitStandingPoint = this.UsableMachine.WaitEntity;
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x0003A1B7 File Offset: 0x000383B7
		public virtual bool HasActionCompleted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0003A1BA File Offset: 0x000383BA
		protected internal virtual Agent.AIScriptedFrameFlags GetScriptedFrameFlags(Agent agent)
		{
			return Agent.AIScriptedFrameFlags.None;
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0003A1BD File Offset: 0x000383BD
		public void Tick(Agent agentToCompareTo, Formation formationToCompareTo, Team potentialUsersTeam, float dt)
		{
			this.OnTick(agentToCompareTo, formationToCompareTo, potentialUsersTeam, dt);
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x0003A1CC File Offset: 0x000383CC
		protected virtual void OnTick(Agent agentToCompareTo, Formation formationToCompareTo, Team potentialUsersTeam, float dt)
		{
			foreach (StandingPoint standingPoint in this.UsableMachine.StandingPoints)
			{
				Agent userAgent = standingPoint.UserAgent;
				if ((agentToCompareTo == null || userAgent == agentToCompareTo) && userAgent != null && userAgent.IsAIControlled && (formationToCompareTo == null || userAgent.Formation == formationToCompareTo) && (this.HasActionCompleted || (potentialUsersTeam != null && this.UsableMachine.IsDisabledForBattleSideAI(potentialUsersTeam.Side)) || userAgent.IsRunningAway))
				{
					this.HandleAgentStopUsingStandingPoint(userAgent, standingPoint);
				}
				if (standingPoint.HasAIMovingTo)
				{
					Agent movingAgent = standingPoint.MovingAgent;
					if ((agentToCompareTo == null || movingAgent == agentToCompareTo) && (formationToCompareTo == null || (movingAgent != null && movingAgent.IsAIControlled && movingAgent.Formation == formationToCompareTo)))
					{
						if (standingPoint.IsDeactivated || this.HasActionCompleted || (potentialUsersTeam != null && this.UsableMachine.IsDisabledForBattleSideAI(potentialUsersTeam.Side)) || movingAgent.IsRunningAway)
						{
							this.HandleAgentStopUsingStandingPoint(movingAgent, standingPoint);
						}
						else
						{
							if (standingPoint.HasAlternative() && this.UsableMachine.IsInRangeToCheckAlternativePoints(movingAgent))
							{
								StandingPoint bestPointAlternativeTo = this.UsableMachine.GetBestPointAlternativeTo(standingPoint, movingAgent);
								if (bestPointAlternativeTo != standingPoint)
								{
									standingPoint.OnMoveToStopped(movingAgent);
									movingAgent.AIMoveToGameObjectEnable(bestPointAlternativeTo, this.UsableMachine, this.GetScriptedFrameFlags(movingAgent));
									if (standingPoint == this.UsableMachine.CurrentlyUsedAmmoPickUpPoint)
									{
										this.UsableMachine.CurrentlyUsedAmmoPickUpPoint = bestPointAlternativeTo;
										continue;
									}
									continue;
								}
							}
							if ((standingPoint.LockUserFrames || standingPoint.LockUserPositions) && standingPoint.HasUserPositionsChanged(movingAgent))
							{
								WorldFrame userFrameForAgent = standingPoint.GetUserFrameForAgent(movingAgent);
								movingAgent.SetScriptedPositionAndDirection(ref userFrameForAgent.Origin, userFrameForAgent.Rotation.f.AsVec2.RotationInRadians, false, this.GetScriptedFrameFlags(movingAgent));
							}
							if (!standingPoint.IsDisabled && !standingPoint.HasUser && !movingAgent.IsPaused && movingAgent.CanReachAndUseObject(standingPoint, standingPoint.GetUserFrameForAgent(movingAgent).Origin.GetGroundVec3().DistanceSquared(movingAgent.Position)))
							{
								movingAgent.UseGameObject(standingPoint, -1);
								movingAgent.SetScriptedFlags(movingAgent.GetScriptedFlags() & ~standingPoint.DisableScriptedFrameFlags);
							}
						}
					}
				}
				for (int i = standingPoint.GetDefendingAgentCount() - 1; i >= 0; i--)
				{
					Agent agent = standingPoint.DefendingAgents[i];
					if ((agentToCompareTo == null || agent == agentToCompareTo) && (formationToCompareTo == null || (agent != null && agent.IsAIControlled && agent.Formation == formationToCompareTo)) && (this.HasActionCompleted || (potentialUsersTeam != null && !this.UsableMachine.IsDisabledForBattleSideAI(potentialUsersTeam.Side)) || agent.IsRunningAway))
					{
						this.HandleAgentStopUsingStandingPoint(agent, standingPoint);
					}
				}
			}
			if (this._lastActiveWaitStandingPoint != this.UsableMachine.WaitEntity)
			{
				foreach (Formation formation in potentialUsersTeam.FormationsIncludingSpecialAndEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && this.UsableMachine.IsUsedByFormation(f) && f.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.FollowEntity && f.GetReadonlyMovementOrderReference().TargetEntity == this._lastActiveWaitStandingPoint))
				{
					if (this is SiegeTowerAI)
					{
						formation.SetMovementOrder(this.NextOrder);
					}
					else
					{
						formation.SetMovementOrder(MovementOrder.MovementOrderFollowEntity(this.UsableMachine.WaitEntity));
					}
				}
				this._lastActiveWaitStandingPoint = this.UsableMachine.WaitEntity;
			}
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0003A53C File Offset: 0x0003873C
		[Conditional("DEBUG")]
		private void TickForDebug()
		{
			if (Input.DebugInput.IsHotKeyDown("UsableMachineAiBaseHotkeyShowMachineUsers"))
			{
				foreach (StandingPoint standingPoint in this.UsableMachine.StandingPoints)
				{
					bool hasAIMovingTo = standingPoint.HasAIMovingTo;
					Agent userAgent = standingPoint.UserAgent;
				}
			}
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0003A5AC File Offset: 0x000387AC
		public static Agent GetSuitableAgentForStandingPoint(UsableMachine usableMachine, StandingPoint standingPoint, IEnumerable<Agent> agents, List<Agent> usedAgents)
		{
			if (usableMachine.AmmoPickUpPoints.Contains(standingPoint) && usableMachine.StandingPoints.Any<StandingPoint>((StandingPoint standingPoint2) => (standingPoint2.IsDeactivated || standingPoint2.HasUser || standingPoint2.HasAIMovingTo) && !standingPoint2.GameEntity.HasTag(usableMachine.AmmoPickUpTag) && standingPoint2 is StandingPointWithWeaponRequirement))
			{
				return null;
			}
			IEnumerable<Agent> enumerable = agents.Where<Agent>((Agent a) => !usedAgents.Contains(a) && a.IsAIControlled && a.IsActive() && !a.IsRunningAway && !a.InteractingWithAnyGameObject() && !standingPoint.IsDisabledForAgent(a) && (a.Formation == null || !a.IsDetachedFromFormation));
			if (!enumerable.Any<Agent>())
			{
				return null;
			}
			return enumerable.MaxBy<Agent, float>((Agent a) => standingPoint.GetUsageScoreForAgent(a));
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0003A63C File Offset: 0x0003883C
		public static Agent GetSuitableAgentForStandingPoint(UsableMachine usableMachine, StandingPoint standingPoint, List<ValueTuple<Agent, float>> agents, List<Agent> usedAgents, float weight)
		{
			if (usableMachine.IsStandingPointNotUsedOnAccountOfBeingAmmoLoad(standingPoint))
			{
				return null;
			}
			Agent agent = null;
			float num = float.MinValue;
			foreach (ValueTuple<Agent, float> valueTuple in agents)
			{
				Agent item = valueTuple.Item1;
				if (!usedAgents.Contains(item) && item.IsAIControlled && item.IsActive() && !item.IsRunningAway && !item.InteractingWithAnyGameObject() && !standingPoint.IsDisabledForAgent(item) && (item.Formation == null || !item.IsDetachedFromFormation || item.DetachmentWeight * 0.4f > weight))
				{
					float usageScoreForAgent = standingPoint.GetUsageScoreForAgent(item);
					if (num < usageScoreForAgent)
					{
						num = usageScoreForAgent;
						agent = item;
					}
				}
			}
			return agent;
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001297 RID: 4759 RVA: 0x0003A704 File Offset: 0x00038904
		protected virtual MovementOrder NextOrder
		{
			get
			{
				return MovementOrder.MovementOrderStop;
			}
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0003A70C File Offset: 0x0003890C
		public virtual void TeleportUserAgentsToMachine(List<Agent> agentList)
		{
			int num = 0;
			bool flag;
			do
			{
				num++;
				flag = false;
				foreach (Agent agent in agentList)
				{
					if (agent.IsAIControlled && agent.AIMoveToGameObjectIsEnabled())
					{
						flag = true;
						WorldFrame userFrameForAgent = this.UsableMachine.GetTargetStandingPointOfAIAgent(agent).GetUserFrameForAgent(agent);
						Vec2 vec = userFrameForAgent.Rotation.f.AsVec2.Normalized();
						if ((agent.Position.AsVec2 - userFrameForAgent.Origin.AsVec2).LengthSquared > 0.0001f || (agent.GetMovementDirection() - vec).LengthSquared > 0.0001f)
						{
							agent.TeleportToPosition(userFrameForAgent.Origin.GetGroundVec3());
							agent.SetMovementDirection(in vec);
							if (GameNetwork.IsServerOrRecorder)
							{
								GameNetwork.BeginBroadcastModuleEvent();
								GameNetwork.WriteMessage(new AgentTeleportToFrame(agent.Index, userFrameForAgent.Origin.GetGroundVec3(), vec));
								GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
							}
						}
					}
				}
			}
			while (flag && num < 10);
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x0003A850 File Offset: 0x00038A50
		public void StopUsingStandingPoint(StandingPoint standingPoint)
		{
			Agent agent = (standingPoint.HasUser ? standingPoint.UserAgent : (standingPoint.HasAIMovingTo ? standingPoint.MovingAgent : null));
			this.HandleAgentStopUsingStandingPoint(agent, standingPoint);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x0003A888 File Offset: 0x00038A88
		protected Agent.StopUsingGameObjectFlags GetStopUsingStandingPointFlags(Agent agent, StandingPoint standingPoint)
		{
			Agent.StopUsingGameObjectFlags stopUsingGameObjectFlags = Agent.StopUsingGameObjectFlags.None;
			if (agent.Team == null || agent.IsRunningAway)
			{
				stopUsingGameObjectFlags |= Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject;
			}
			else
			{
				if (this.UsableMachine.AutoAttachUserToFormation(agent.Team.Side))
				{
					stopUsingGameObjectFlags |= Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject;
				}
				if (this.UsableMachine.HasToBeDefendedByUser(agent.Team.Side))
				{
					stopUsingGameObjectFlags |= Agent.StopUsingGameObjectFlags.DefendAfterStoppingUsingGameObject;
				}
			}
			return stopUsingGameObjectFlags;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0003A8E6 File Offset: 0x00038AE6
		protected virtual void HandleAgentStopUsingStandingPoint(Agent agent, StandingPoint standingPoint)
		{
			agent.StopUsingGameObjectMT(true, this.GetStopUsingStandingPointFlags(agent, standingPoint));
		}

		// Token: 0x0400048C RID: 1164
		protected readonly UsableMachine UsableMachine;

		// Token: 0x0400048D RID: 1165
		private GameEntity _lastActiveWaitStandingPoint;
	}
}
