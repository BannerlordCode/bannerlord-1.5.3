using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Source.Objects;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000AD RID: 173
	public class FollowAgentBehavior : AgentBehavior
	{
		// Token: 0x06000743 RID: 1859 RVA: 0x000310B2 File Offset: 0x0002F2B2
		public FollowAgentBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._selectedAgent = null;
			this._deactivatedAgent = null;
			this._myLastStateWasRunning = false;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000310D0 File Offset: 0x0002F2D0
		public void SetTargetAgent(Agent agent)
		{
			this._selectedAgent = agent;
			this._state = FollowAgentBehavior.State.Idle;
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("navigation_mesh_deactivator");
			if (gameEntity != null)
			{
				int disableFaceWithId = gameEntity.GetFirstScriptOfType<NavigationMeshDeactivator>().DisableFaceWithId;
				if (disableFaceWithId != -1)
				{
					base.OwnerAgent.SetAgentExcludeStateForFaceGroupId(disableFaceWithId, false);
				}
			}
			this.TryMoveStateTransition(true);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0003112E File Offset: 0x0002F32E
		public override void Tick(float dt, bool isSimulation)
		{
			if (this._selectedAgent != null)
			{
				this.ControlMovement();
			}
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00031140 File Offset: 0x0002F340
		private void ControlMovement()
		{
			if (base.Navigator.TargetPosition.IsValid && base.Navigator.IsTargetReached())
			{
				base.OwnerAgent.DisableScriptedMovement();
				base.OwnerAgent.SetMaximumSpeedLimit(-1f, false);
				if (this._state == FollowAgentBehavior.State.OnMove)
				{
					this._idleDistance = base.OwnerAgent.Position.AsVec2.Distance(this._selectedAgent.Position.AsVec2);
				}
				this._state = FollowAgentBehavior.State.Idle;
			}
			int nearbyEnemyAgentCount = base.Mission.GetNearbyEnemyAgentCount(base.OwnerAgent.Team, base.OwnerAgent.Position.AsVec2, 5f);
			if (this._state != FollowAgentBehavior.State.Fight && nearbyEnemyAgentCount > 0)
			{
				base.OwnerAgent.SetWatchState(Agent.WatchState.Alarmed);
				base.OwnerAgent.ResetLookAgent();
				base.Navigator.ClearTarget();
				base.OwnerAgent.DisableScriptedMovement();
				this._state = FollowAgentBehavior.State.Fight;
				Debug.Print("[Follow agent behavior] Fight!", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			switch (this._state)
			{
			case FollowAgentBehavior.State.Idle:
				this.TryMoveStateTransition(false);
				return;
			case FollowAgentBehavior.State.OnMove:
				this.MoveToFollowingAgent(false);
				break;
			case FollowAgentBehavior.State.Fight:
				if (nearbyEnemyAgentCount == 0)
				{
					base.OwnerAgent.SetWatchState(Agent.WatchState.Patrolling);
					base.OwnerAgent.SetLookAgent(this._selectedAgent);
					this._state = FollowAgentBehavior.State.Idle;
					Debug.Print("[Follow agent behavior] Stop fighting!", 0, Debug.DebugColor.White, 17592186044416UL);
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x000312C0 File Offset: 0x0002F4C0
		private void TryMoveStateTransition(bool forceMove)
		{
			if (this._selectedAgent != null)
			{
				if ((base.OwnerAgent.GetScriptedFlags() & Agent.AIScriptedFrameFlags.Crouch) != (this._selectedAgent.GetScriptedFlags() & Agent.AIScriptedFrameFlags.Crouch))
				{
					base.OwnerAgent.SetCrouchMode(this._selectedAgent.CrouchMode);
				}
				if (base.OwnerAgent.Position.AsVec2.Distance(this._selectedAgent.Position.AsVec2) > 4f + this._idleDistance)
				{
					this._state = FollowAgentBehavior.State.OnMove;
					this.MoveToFollowingAgent(forceMove);
				}
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0003135C File Offset: 0x0002F55C
		private void MoveToFollowingAgent(bool forcedMove)
		{
			Vec2 asVec = this._selectedAgent.Velocity.AsVec2;
			if (this._updatePositionThisFrame || forcedMove || asVec.IsNonZero())
			{
				this._updatePositionThisFrame = false;
				WorldPosition worldPosition = this._selectedAgent.GetWorldPosition();
				Vec2 vec = (asVec.IsNonZero() ? asVec.Normalized() : this._selectedAgent.GetMovementDirection());
				Vec2 vec2 = vec.LeftVec();
				Vec2 vec3 = this._selectedAgent.Position.AsVec2 - base.OwnerAgent.Position.AsVec2;
				float lengthSquared = vec3.LengthSquared;
				int num = ((Vec2.DotProduct(vec3, vec2) > 0f) ? 1 : (-1));
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				foreach (Agent agent in base.Mission.Agents)
				{
					CampaignAgentComponent component = agent.GetComponent<CampaignAgentComponent>();
					if (((component != null) ? component.AgentNavigator : null) != null)
					{
						DailyBehaviorGroup behaviorGroup = component.AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
						FollowAgentBehavior followAgentBehavior = ((behaviorGroup != null) ? behaviorGroup.GetBehavior<FollowAgentBehavior>() : null);
						if (followAgentBehavior != null && followAgentBehavior._selectedAgent != null && followAgentBehavior._selectedAgent == this._selectedAgent)
						{
							Vec2 vec4 = this._selectedAgent.Position.AsVec2 - agent.Position.AsVec2;
							int num6 = ((Vec2.DotProduct(vec4, vec2) > 0f) ? 1 : (-1));
							if (vec4.LengthSquared < lengthSquared)
							{
								if (num6 == num)
								{
									if (agent.HasMount)
									{
										num3++;
									}
									else
									{
										num2++;
									}
								}
								if (Vec2.DotProduct(vec4, vec) > 0.3f)
								{
									if (agent.HasMount)
									{
										num5++;
									}
									else
									{
										num4++;
									}
								}
							}
						}
					}
				}
				float num7 = (this._selectedAgent.HasMount ? 1.25f : 0.6f);
				float num8 = (base.OwnerAgent.HasMount ? 1.25f : 0.6f);
				float num9 = (this._selectedAgent.HasMount ? 1.5f : 1f);
				float num10 = (base.OwnerAgent.HasMount ? 1.5f : 1f);
				Vec2 vec5 = vec * (2f + 0.5f * (num8 + num7) + (float)num2 * 0.6f + (float)num3 * 1.25f);
				Vec2 vec6 = (float)num * vec2 * (0.5f * (num10 + num9) + (float)num2 * 1f + (float)num3 * 1.5f);
				Vec2 vec7 = this._selectedAgent.Position.AsVec2 - vec5 - vec6;
				bool flag = false;
				AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(Mission.Current, vec7, 0.5f, false);
				while (proximityMapSearchStruct.LastFoundAgent != null)
				{
					Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
					if (lastFoundAgent.Index != base.OwnerAgent.Index && lastFoundAgent.Index != this._selectedAgent.Index)
					{
						flag = true;
						break;
					}
					AgentProximityMap.FindNext(Mission.Current, ref proximityMapSearchStruct);
				}
				float num11 = (base.OwnerAgent.HasMount ? 2.2f : 1.2f);
				if (!flag)
				{
					WorldPosition worldPosition2 = worldPosition;
					worldPosition2 = new WorldPosition(base.Mission.Scene, UIntPtr.Zero, worldPosition2.GetGroundVec3(), false);
					worldPosition2.SetVec2(vec7);
					if (worldPosition2.GetNavMesh() != UIntPtr.Zero && base.Mission.Scene.IsLineToPointClear(ref worldPosition2, ref worldPosition, base.OwnerAgent.Monster.BodyCapsuleRadius))
					{
						WorldPosition worldPosition3 = worldPosition2;
						worldPosition3.SetVec2(worldPosition3.AsVec2 + vec * 1.5f);
						if (worldPosition3.GetNavMesh() != UIntPtr.Zero && base.Mission.Scene.IsLineToPointClear(ref worldPosition3, ref worldPosition2, base.OwnerAgent.Monster.BodyCapsuleRadius))
						{
							this.SetMovePos(worldPosition3, this._selectedAgent.MovementDirectionAsAngle, num11, Agent.AIScriptedFrameFlags.NoAttack);
						}
						else
						{
							this.SetMovePos(worldPosition2, this._selectedAgent.MovementDirectionAsAngle, num11, Agent.AIScriptedFrameFlags.NoAttack);
						}
					}
					else
					{
						flag = true;
					}
				}
				if (flag)
				{
					float num12 = num11 + (float)num4 * 0.6f + (float)num5 * 1.25f;
					this.SetMovePos(worldPosition, this._selectedAgent.MovementDirectionAsAngle, num12, Agent.AIScriptedFrameFlags.NoAttack);
				}
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00031800 File Offset: 0x0002FA00
		private void SetMovePos(WorldPosition pos, float rotationInRadians, float rangeThreshold, Agent.AIScriptedFrameFlags flags)
		{
			bool flag = base.Mission.Mode == MissionMode.Stealth;
			if (base.Navigator.CharacterHasVisiblePrefabs)
			{
				this._myLastStateWasRunning = false;
			}
			else
			{
				if (flag && this._selectedAgent.CrouchMode)
				{
					flags |= Agent.AIScriptedFrameFlags.Crouch;
				}
				if (flag && this._selectedAgent.WalkMode)
				{
					base.OwnerAgent.SetMaximumSpeedLimit(this._selectedAgent.CrouchMode ? this._selectedAgent.Monster.CrouchWalkingSpeedLimit : this._selectedAgent.Monster.WalkingSpeedLimit, false);
					this._myLastStateWasRunning = false;
				}
				else
				{
					float num = base.OwnerAgent.Position.AsVec2.Distance(pos.AsVec2);
					if (num - rangeThreshold <= 0.5f * (this._myLastStateWasRunning ? 1f : 1.2f) && this._selectedAgent.Velocity.AsVec2.Length <= base.OwnerAgent.Monster.WalkingSpeedLimit * (this._myLastStateWasRunning ? 1f : 1.2f))
					{
						this._myLastStateWasRunning = false;
					}
					else
					{
						base.OwnerAgent.SetMaximumSpeedLimit(num - rangeThreshold + this._selectedAgent.Velocity.AsVec2.Length, false);
						this._myLastStateWasRunning = true;
					}
				}
			}
			if (!this._myLastStateWasRunning)
			{
				flags |= Agent.AIScriptedFrameFlags.DoNotRun;
			}
			base.Navigator.SetTargetFrame(pos, rotationInRadians, rangeThreshold, -10f, flags, flag);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0003198A File Offset: 0x0002FB8A
		public override void OnAgentRemoved(Agent agent)
		{
			if (agent == this._selectedAgent)
			{
				base.OwnerAgent.ResetLookAgent();
				this._selectedAgent = null;
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x000319A7 File Offset: 0x0002FBA7
		protected override void OnActivate()
		{
			if (this._deactivatedAgent != null)
			{
				this.SetTargetAgent(this._deactivatedAgent);
				this._deactivatedAgent = null;
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x000319C4 File Offset: 0x0002FBC4
		protected override void OnDeactivate()
		{
			this._state = FollowAgentBehavior.State.Idle;
			this._deactivatedAgent = this._selectedAgent;
			this._selectedAgent = null;
			base.OwnerAgent.DisableScriptedMovement();
			base.OwnerAgent.ResetLookAgent();
			base.Navigator.ClearTarget();
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00031A04 File Offset: 0x0002FC04
		public override string GetDebugInfo()
		{
			return string.Concat(new object[]
			{
				"Follow ",
				this._selectedAgent.Name,
				" (id:",
				this._selectedAgent.Index,
				")"
			});
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00031A55 File Offset: 0x0002FC55
		public override float GetAvailability(bool isSimulation)
		{
			return (float)((this._selectedAgent == null) ? 0 : 100);
		}

		// Token: 0x040003D5 RID: 981
		private const float _moveReactionProximityThreshold = 4f;

		// Token: 0x040003D6 RID: 982
		private const float _longitudinalClearanceOffset = 2f;

		// Token: 0x040003D7 RID: 983
		private const float _onFootMoveProximityThreshold = 1.2f;

		// Token: 0x040003D8 RID: 984
		private const float _mountedMoveProximityThreshold = 2.2f;

		// Token: 0x040003D9 RID: 985
		private const float _onFootAgentLongitudinalOffset = 0.6f;

		// Token: 0x040003DA RID: 986
		private const float _onFootAgentLateralOffset = 1f;

		// Token: 0x040003DB RID: 987
		private const float _mountedAgentLongitudinalOffset = 1.25f;

		// Token: 0x040003DC RID: 988
		private const float _mountedAgentLateralOffset = 1.5f;

		// Token: 0x040003DD RID: 989
		private float _idleDistance;

		// Token: 0x040003DE RID: 990
		private Agent _selectedAgent;

		// Token: 0x040003DF RID: 991
		private FollowAgentBehavior.State _state;

		// Token: 0x040003E0 RID: 992
		private Agent _deactivatedAgent;

		// Token: 0x040003E1 RID: 993
		private bool _myLastStateWasRunning;

		// Token: 0x040003E2 RID: 994
		private bool _updatePositionThisFrame;

		// Token: 0x020001BF RID: 447
		private enum State
		{
			// Token: 0x0400080D RID: 2061
			Idle,
			// Token: 0x0400080E RID: 2062
			OnMove,
			// Token: 0x0400080F RID: 2063
			Fight
		}
	}
}
