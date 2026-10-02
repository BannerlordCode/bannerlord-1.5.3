using System;
using System.Collections.Generic;
using SandBox.Missions.MissionLogics;
using SandBox.Objects.AnimationPoints;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000B6 RID: 182
	public class WalkingBehavior : AgentBehavior
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x000332BB File Offset: 0x000314BB
		private bool CanWander
		{
			get
			{
				return (this._isIndoor && this._indoorWanderingIsActive) || (!this._isIndoor && this._outdoorWanderingIsActive);
			}
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x000332E0 File Offset: 0x000314E0
		public WalkingBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			this._wanderTarget = null;
			this._isIndoor = CampaignMission.Current.Location.IsIndoor;
			this._indoorWanderingIsActive = true;
			this._outdoorWanderingIsActive = true;
			this._wasSimulation = false;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00033336 File Offset: 0x00031536
		public void SetIndoorWandering(bool isActive)
		{
			this._indoorWanderingIsActive = isActive;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0003333F File Offset: 0x0003153F
		public void SetOutdoorWandering(bool isActive)
		{
			this._outdoorWanderingIsActive = isActive;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00033348 File Offset: 0x00031548
		public override void Tick(float dt, bool isSimulation)
		{
			if (Mission.Current.CurrentState != Mission.State.EndingNextFrame)
			{
				if (this._wanderTarget == null || base.Navigator.TargetUsableMachine == null || this._wanderTarget.IsDisabled || !this._wanderTarget.IsStandingPointAvailableForAgent(base.OwnerAgent))
				{
					this._wanderTarget = this.FindTarget();
					this._lastTarget = this._wanderTarget;
					if (this._wanderTarget == null)
					{
						string specialTargetTag = base.OwnerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SpecialTargetTag;
						if (specialTargetTag != null && this._missionAgentHandler.HasUsablePointWithTag(specialTargetTag))
						{
							if (!this._isWaitingNearOccupiedTarget)
							{
								List<UsableMachine> allUsablePointsWithTag = this._missionAgentHandler.GetAllUsablePointsWithTag(specialTargetTag);
								if (allUsablePointsWithTag != null && allUsablePointsWithTag.Count > 0)
								{
									UsableMachine usableMachine = allUsablePointsWithTag[0];
									Vec3 globalPosition = usableMachine.GameEntity.GlobalPosition;
									Vec3 vec = base.OwnerAgent.Position - globalPosition;
									vec.z = 0f;
									if (vec.Length < 0.01f)
									{
										vec = new Vec3(1f, 0f, 0f, -1f);
									}
									vec.Normalize();
									Vec3 vec2 = globalPosition + vec * 2f;
									WorldPosition worldPosition = new WorldPosition(usableMachine.GameEntity.Scene, vec2);
									base.Navigator.SetTargetFrame(worldPosition, 0f, 1f, -10f, Agent.AIScriptedFrameFlags.DoNotRun, false);
									this._isWaitingNearOccupiedTarget = true;
									return;
								}
							}
							else
							{
								if (this._waitTimer == null)
								{
									this._waitTimer = new Timer(base.Mission.CurrentTime, 3f, true);
									return;
								}
								if (this._waitTimer.Check(base.Mission.CurrentTime))
								{
									this._waitTimer = null;
									this._isWaitingNearOccupiedTarget = false;
								}
							}
							return;
						}
					}
					else
					{
						this._isWaitingNearOccupiedTarget = false;
					}
				}
				else if (base.Navigator.GetDistanceToTarget(this._wanderTarget) < 5f)
				{
					bool flag = this._wasSimulation && !isSimulation && this._wanderTarget != null && this._waitTimer != null && MBRandom.RandomFloat < (this._isIndoor ? 0f : (Settlement.CurrentSettlement.IsVillage ? 0.6f : 0.1f));
					if (this._waitTimer == null)
					{
						if (!this._wanderTarget.GameEntity.HasTag("npc_idle"))
						{
							this.SetTimerForTheAgent(isSimulation);
						}
					}
					else if (this._waitTimer.Check(base.Mission.CurrentTime) || flag)
					{
						if (this.CanWander)
						{
							this._waitTimer = null;
							UsableMachine usableMachine2 = this.FindTarget();
							if (usableMachine2 == null || this.IsChildrenOfSameParent(usableMachine2, this._wanderTarget))
							{
								this.SetTimerForTheAgent(isSimulation);
							}
							else
							{
								this._lastTarget = this._wanderTarget;
								this._wanderTarget = usableMachine2;
							}
						}
						else
						{
							this._waitTimer.Reset(100f);
						}
					}
				}
				else if (this._wanderTarget != null)
				{
					using (List<StandingPoint>.Enumerator enumerator = this._wanderTarget.StandingPoints.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.HasUser)
							{
								this._wanderTarget = null;
								break;
							}
						}
					}
				}
				if (base.OwnerAgent.CurrentlyUsedGameObject != null && base.Navigator.GetDistanceToTarget(this._lastTarget) > 1f)
				{
					base.Navigator.SetTarget(this._lastTarget, this._lastTarget == this._wanderTarget, Agent.AIScriptedFrameFlags.None);
				}
				base.Navigator.SetTarget(this._wanderTarget, false, Agent.AIScriptedFrameFlags.None);
				this._wasSimulation = isSimulation;
			}
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x000336FC File Offset: 0x000318FC
		private void SetTimerForTheAgent(bool isSimulation)
		{
			AnimationPoint animationPoint;
			float num = (((animationPoint = base.OwnerAgent.CurrentlyUsedGameObject as AnimationPoint) != null) ? animationPoint.GetRandomWaitInSeconds() : 10f);
			if (isSimulation && MBRandom.RandomFloat < 0.33f)
			{
				num /= 10f + MBRandom.RandomFloat * 10f;
			}
			this._waitTimer = new Timer(base.Mission.CurrentTime, (num < 0f) ? 2.1474836E+09f : num, true);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00033778 File Offset: 0x00031978
		private bool IsChildrenOfSameParent(UsableMachine machine, UsableMachine otherMachine)
		{
			WeakGameEntity weakGameEntity = machine.GameEntity;
			while (weakGameEntity.Parent.IsValid)
			{
				weakGameEntity = weakGameEntity.Parent;
			}
			WeakGameEntity weakGameEntity2 = otherMachine.GameEntity;
			while (weakGameEntity2.Parent.IsValid)
			{
				weakGameEntity2 = weakGameEntity2.Parent;
			}
			return weakGameEntity == weakGameEntity2;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x000337D0 File Offset: 0x000319D0
		public override void ConversationTick()
		{
			if (this._waitTimer != null)
			{
				this._waitTimer.Reset(base.Mission.CurrentTime);
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x000337F0 File Offset: 0x000319F0
		public override float GetAvailability(bool isSimulation)
		{
			if (this._isWaitingNearOccupiedTarget)
			{
				return 1f;
			}
			if (this.FindTarget() == null)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00033813 File Offset: 0x00031A13
		public override void SetCustomWanderTarget(UsableMachine customUsableMachine)
		{
			this._wanderTarget = customUsableMachine;
			if (this._waitTimer != null)
			{
				this._waitTimer = null;
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0003382C File Offset: 0x00031A2C
		private UsableMachine FindRandomWalkingTarget(bool forWaiting)
		{
			if (forWaiting && (this._wanderTarget ?? base.Navigator.TargetUsableMachine) != null)
			{
				return null;
			}
			string text = base.OwnerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SpecialTargetTag;
			if (text == null)
			{
				text = "npc_common";
			}
			else if (!this._missionAgentHandler.HasUsablePointWithTag(text))
			{
				text = "npc_common_limited";
			}
			return this._missionAgentHandler.FindUnusedPointWithTagForAgent(base.OwnerAgent, text);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0003389C File Offset: 0x00031A9C
		private UsableMachine FindTarget()
		{
			return this.FindRandomWalkingTarget(this._isIndoor && !this._indoorWanderingIsActive);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x000338B8 File Offset: 0x00031AB8
		private float GetTargetScore(UsableMachine usableMachine)
		{
			if (base.OwnerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SpecialTargetTag != null && !usableMachine.GameEntity.HasTag(base.OwnerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SpecialTargetTag))
			{
				return 0f;
			}
			StandingPoint vacantStandingPointForAI = usableMachine.GetVacantStandingPointForAI(base.OwnerAgent);
			if (vacantStandingPointForAI == null || vacantStandingPointForAI.IsDisabledForAgent(base.OwnerAgent))
			{
				return 0f;
			}
			float num = 1f;
			Vec3 vec = vacantStandingPointForAI.GetUserFrameForAgent(base.OwnerAgent).Origin.GetGroundVec3() - base.OwnerAgent.Position;
			if (vec.Length < 2f)
			{
				num *= vec.Length / 2f;
			}
			return num * (0.8f + MBRandom.RandomFloat * 0.2f);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00033990 File Offset: 0x00031B90
		public override void OnSpecialTargetChanged()
		{
			if (this._wanderTarget == null)
			{
				return;
			}
			if (!base.Navigator.SpecialTargetTag.IsEmpty<char>() && !this._wanderTarget.GameEntity.HasTag(base.Navigator.SpecialTargetTag))
			{
				this._wanderTarget = null;
				base.Navigator.SetTarget(this._wanderTarget, false, Agent.AIScriptedFrameFlags.None);
				return;
			}
			if (base.Navigator.SpecialTargetTag.IsEmpty<char>() && !this._wanderTarget.GameEntity.HasTag("npc_common"))
			{
				this._wanderTarget = null;
				base.Navigator.SetTarget(this._wanderTarget, false, Agent.AIScriptedFrameFlags.None);
			}
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00033A3C File Offset: 0x00031C3C
		public override string GetDebugInfo()
		{
			string text = "Walk ";
			if (this._waitTimer != null)
			{
				text = string.Concat(new object[]
				{
					text,
					"(Wait ",
					(int)this._waitTimer.ElapsedTime(),
					"/",
					this._waitTimer.Duration,
					")"
				});
			}
			else if (this._wanderTarget == null)
			{
				text += "(search for target!)";
			}
			return text;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00033ABD File Offset: 0x00031CBD
		protected override void OnDeactivate()
		{
			base.Navigator.ClearTarget();
			this._wanderTarget = null;
			this._waitTimer = null;
			this._isWaitingNearOccupiedTarget = false;
		}

		// Token: 0x04000409 RID: 1033
		private readonly MissionAgentHandler _missionAgentHandler;

		// Token: 0x0400040A RID: 1034
		private readonly bool _isIndoor;

		// Token: 0x0400040B RID: 1035
		private UsableMachine _wanderTarget;

		// Token: 0x0400040C RID: 1036
		private UsableMachine _lastTarget;

		// Token: 0x0400040D RID: 1037
		private Timer _waitTimer;

		// Token: 0x0400040E RID: 1038
		private bool _indoorWanderingIsActive;

		// Token: 0x0400040F RID: 1039
		private bool _outdoorWanderingIsActive;

		// Token: 0x04000410 RID: 1040
		private bool _wasSimulation;

		// Token: 0x04000411 RID: 1041
		private bool _isWaitingNearOccupiedTarget;
	}
}
