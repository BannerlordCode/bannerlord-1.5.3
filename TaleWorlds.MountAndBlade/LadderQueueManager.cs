using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000353 RID: 851
	public class LadderQueueManager : MissionObject
	{
		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x000BD090 File Offset: 0x000BB290
		// (set) Token: 0x06003027 RID: 12327 RVA: 0x000BD098 File Offset: 0x000BB298
		public bool IsDeactivated { get; private set; }

		// Token: 0x06003028 RID: 12328 RVA: 0x000BD0A1 File Offset: 0x000BB2A1
		protected internal override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x000BD0B5 File Offset: 0x000BB2B5
		public void DeactivateImmediate()
		{
			this.IsDeactivated = true;
			this._deactivationDelayTimerElapsed = true;
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x000BD0C8 File Offset: 0x000BB2C8
		public void Deactivate()
		{
			this.IsDeactivated = true;
			this._deactivateTimer.Reset(Mission.Current.CurrentTime, 10f);
			int num = Math.Min(2, this._queuedAgentCount);
			int num2 = 0;
			int num3 = 0;
			while (num3 < this._queuedAgents.Count && num > 0)
			{
				if (this._queuedAgents[num3] != null)
				{
					num2++;
					if (num2 == num)
					{
						this.RemoveAgentFromQueueAtIndex(num3);
						num--;
						num3 = -1;
						num2 = 0;
					}
				}
				num3++;
			}
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x000BD144 File Offset: 0x000BB344
		public void Activate()
		{
			this.IsDeactivated = false;
			this._deactivationDelayTimerElapsed = false;
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000BD154 File Offset: 0x000BB354
		public void Initialize(int managedNavigationFaceId, MatrixFrame managedFrame, Vec3 managedDirection, BattleSideEnum managedSide, int maxUserCount, float arcAngle, float queueBeginDistance, float queueRowSize, float costPerRow, float baseCost, bool blockUsage, float agentSpacing, float zDifferenceToStopUsing, float distanceToStopUsing2d, bool doesManageMultipleIDs, int managedNavigationFaceAlternateID1, int managedNavigationFaceAlternateID2, int maxClimberCount, int maxRunnerCount)
		{
			this.ManagedNavigationFaceId = managedNavigationFaceId;
			this._managedEntitialFrame = managedFrame;
			this._managedEntitialDirection = managedDirection.AsVec2.Normalized();
			MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
			this._managedGlobalFrame = matrixFrame.TransformToParent(in managedFrame);
			this._managedGlobalWorldPosition = new WorldPosition(base.GameEntity.GetScenePointer(), UIntPtr.Zero, this._managedGlobalFrame.origin, false);
			this._managedGlobalWorldPosition.GetGroundVec3();
			matrixFrame = base.GameEntity.GetGlobalFrame();
			this._managedGlobalDirection = matrixFrame.rotation.TransformToParent(in managedDirection).AsVec2.Normalized();
			this._lastCachedGameEntityGlobalPosition = base.GameEntity.GetGlobalFrame().origin;
			this._managedSide = managedSide;
			this._maxUserCount = maxUserCount;
			this._arcAngle = arcAngle;
			this._queueBeginDistance = queueBeginDistance;
			this._queueRowSize = queueRowSize;
			this._costPerRow = costPerRow;
			this._baseCost = baseCost;
			this._blockUsage = blockUsage;
			this._agentSpacing = agentSpacing;
			this._zDifferenceToStopUsing = zDifferenceToStopUsing;
			this._distanceToStopUsing2d = distanceToStopUsing2d;
			this._doesManageMultipleIDs = doesManageMultipleIDs;
			this.ManagedNavigationFaceAlternateID1 = managedNavigationFaceAlternateID1;
			this.ManagedNavigationFaceAlternateID2 = managedNavigationFaceAlternateID2;
			this._maxClimberCount = maxClimberCount;
			this._maxRunnerCount = maxRunnerCount;
			this._lastUserCostPenaltyPerLadder = new ValueTuple<float, bool>[3];
			this._deactivateTimer = new Timer(0f, 0f, true);
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x000BD2C6 File Offset: 0x000BB4C6
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x000BD2E0 File Offset: 0x000BB4E0
		private void UpdateGlobalFrameCache()
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			if (this._lastCachedGameEntityGlobalPosition != globalFrame.origin)
			{
				this._lastCachedGameEntityGlobalPosition = globalFrame.origin;
				this._managedGlobalFrame = globalFrame.TransformToParent(in this._managedEntitialFrame);
				this._managedGlobalWorldPosition = new WorldPosition(base.GameEntity.GetScenePointer(), UIntPtr.Zero, this._managedGlobalFrame.origin, false);
				this._managedGlobalWorldPosition.GetGroundVec3MT();
				Vec3 vec = new Vec3(this._managedEntitialDirection, 0f, -1f);
				this._managedGlobalDirection = globalFrame.rotation.TransformToParent(in vec).AsVec2.Normalized();
			}
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x000BD3A4 File Offset: 0x000BB5A4
		private void OnTickParallelAux(float dt)
		{
			if (this.IsDeactivated && !this._deactivationDelayTimerElapsed && this._deactivateTimer.Check(Mission.Current.CurrentTime))
			{
				this._deactivationDelayTimerElapsed = true;
			}
			if (this._deactivationDelayTimerElapsed)
			{
				this._userAgents.Clear();
				for (int i = 0; i < this._queuedAgents.Count; i++)
				{
					if (this._queuedAgents[i] != null && this._queuedAgents[i].IsActive())
					{
						this.RemoveAgentFromQueueAtIndex(i);
					}
				}
				this._queuedAgents.Clear();
				return;
			}
			this.UpdateGlobalFrameCache();
			Vec3 groundVec = this._managedGlobalWorldPosition.GetGroundVec3();
			this._timeSinceLastUpdate += dt;
			if (this._timeSinceLastUpdate < this._updatePeriod)
			{
				return;
			}
			if (this._neighborLadderQueueManager != null && (float)this._neighborLadderQueueManager._queuedAgentCount < (float)this._queuedAgentCount * 0.4f && this._neighborLadderQueueManager.CostAddition < this.CostAddition * 0.6667f)
			{
				this.FlushQueueManager();
			}
			this._usingAgentResetTime -= this._timeSinceLastUpdate;
			this._timeSinceLastUpdate = 0f;
			this._updatePeriod = 0.2f + MBRandom.RandomFloat * 0.1f;
			StackArray.StackArray3Float stackArray3Float = default(StackArray.StackArray3Float);
			int num = 0;
			for (int j = this._userAgents.Count - 1; j >= 0; j--)
			{
				Agent agent = this._userAgents[j];
				bool flag = false;
				int currentNavigationFaceId = agent.GetCurrentNavigationFaceId();
				float num2 = ((this._zDifferenceToStopUsing > 0.01f) ? ((agent.Position.z - groundVec.z) / this._zDifferenceToStopUsing) : 1.01f);
				if (!agent.IsActive())
				{
					flag = true;
				}
				else if (this._usingAgentResetTime < 0f && (num2 > 1f || (agent.Position.AsVec2 - groundVec.AsVec2).LengthSquared > this._distanceToStopUsing2d * this._distanceToStopUsing2d))
				{
					if (currentNavigationFaceId == this.ManagedNavigationFaceId || (this._doesManageMultipleIDs && (currentNavigationFaceId == this.ManagedNavigationFaceAlternateID1 || currentNavigationFaceId == this.ManagedNavigationFaceAlternateID2)))
					{
						flag = true;
					}
					else if (!this.ShouldAgentUseTheLadder(agent))
					{
						flag = true;
					}
				}
				if (flag)
				{
					HumanAIComponent humanAIComponent = this._userAgents[j].HumanAIComponent;
					if (humanAIComponent != null)
					{
						humanAIComponent.AdjustSpeedLimit(this._userAgents[j], -1f, false);
					}
					this._userAgents[j].SetIsLadderQueueUsing(false);
					this._userAgents.RemoveAt(j);
				}
				else
				{
					bool flag2 = false;
					if (currentNavigationFaceId == this.ManagedNavigationFaceId)
					{
						ref StackArray.StackArray3Float ptr = ref stackArray3Float;
						ptr[0] = ptr[0] + ((num2 < 1f) ? (1f - num2 * num2 * num2) : 0f);
						num++;
						flag2 = true;
					}
					else if (this._doesManageMultipleIDs)
					{
						if (currentNavigationFaceId == this.ManagedNavigationFaceAlternateID1)
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[1] = ptr[1] + ((num2 < 1f) ? (1f - num2 * num2 * num2) : 0f);
							num++;
							flag2 = true;
						}
						else if (currentNavigationFaceId == this.ManagedNavigationFaceAlternateID2)
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[2] = ptr[2] + ((num2 < 1f) ? (1f - num2 * num2 * num2) : 0f);
							num++;
							flag2 = true;
						}
					}
					if (!flag2)
					{
						if (this._userAgents[j].HasPathThroughNavigationFaceIdFromDirectionMT(this.ManagedNavigationFaceId, this._managedGlobalDirection))
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[0] = ptr[0] + 0.3f;
						}
						else if (this._userAgents[j].HasPathThroughNavigationFaceIdFromDirectionMT(this.ManagedNavigationFaceAlternateID1, this._managedGlobalDirection))
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[1] = ptr[1] + 0.3f;
						}
						else if (this._userAgents[j].HasPathThroughNavigationFaceIdFromDirectionMT(this.ManagedNavigationFaceAlternateID2, this._managedGlobalDirection))
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[2] = ptr[2] + 0.3f;
						}
					}
				}
			}
			if (this._neighborLadderQueueManager != null)
			{
				for (int k = this._neighborLadderQueueManager._userAgents.Count - 1; k >= 0; k--)
				{
					int currentNavigationFaceId2 = this._neighborLadderQueueManager._userAgents[k].GetCurrentNavigationFaceId();
					if (currentNavigationFaceId2 == this.ManagedNavigationFaceId)
					{
						ref StackArray.StackArray3Float ptr = ref stackArray3Float;
						ptr[0] = ptr[0] + 0.3f;
						num++;
					}
					else if (this._doesManageMultipleIDs)
					{
						if (currentNavigationFaceId2 == this.ManagedNavigationFaceAlternateID1)
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[1] = ptr[1] + 0.3f;
							num++;
						}
						else if (currentNavigationFaceId2 == this.ManagedNavigationFaceAlternateID2)
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[2] = ptr[2] + 0.3f;
							num++;
						}
					}
				}
			}
			for (int l = 0; l < 3; l++)
			{
				if (!this._lastUserCostPenaltyPerLadder[l].Item1.ApproximatelyEqualsTo(stackArray3Float[l], 1E-05f))
				{
					this._lastUserCostPenaltyPerLadder[l].Item1 = stackArray3Float[l];
					this._lastUserCostPenaltyPerLadder[l].Item2 = true;
				}
			}
			for (int m = this._queuedAgents.Count - 1; m >= 0; m--)
			{
				if (this._queuedAgents[m] != null)
				{
					if (!this.ConditionsAreMet(this._queuedAgents[m], Agent.AIScriptedFrameFlags.GoToPosition))
					{
						this.RemoveAgentFromQueueAtIndex(m);
					}
					else
					{
						float num3 = MBRandom.RandomFloat * (float)this._maxUserCount;
						if (num3 > 0.7f)
						{
							int num4;
							int num5;
							this.GetParentIndicesForQueueIndex(m, out num4, out num5);
							if (num4 >= 0 && this._queuedAgents[num4] == null && num3 > ((num5 >= 0) ? 0.85f : 0.7f))
							{
								this.MoveAgentFromQueueIndexToQueueIndex(m, num4);
							}
							else if (num5 >= 0 && this._queuedAgents[num5] == null)
							{
								this.MoveAgentFromQueueIndexToQueueIndex(m, num5);
							}
						}
					}
				}
			}
			int num6 = this._queuedAgents.Count - 1;
			while (num6 >= 0 && this._queuedAgents[num6] == null)
			{
				this._queuedAgents.RemoveAt(num6--);
			}
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(Mission.Current, groundVec.AsVec2, 30f, false);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
				if (this.ConditionsAreMet(lastFoundAgent, Agent.AIScriptedFrameFlags.None) && lastFoundAgent.Position.DistanceSquared(groundVec) < 900f && !this._queuedAgents.Contains(lastFoundAgent) && lastFoundAgent.HasPathThroughNavigationFacesIDFromDirectionMT(this.ManagedNavigationFaceId, this.ManagedNavigationFaceAlternateID1, this.ManagedNavigationFaceAlternateID2, Vec2.Zero))
				{
					if (this._neighborLadderQueueManager == null)
					{
						this.AddAgentToQueue(lastFoundAgent);
					}
					else if (!this._neighborLadderQueueManager._userAgents.Contains(lastFoundAgent))
					{
						this.AddAgentToQueue(lastFoundAgent);
					}
					else
					{
						lastFoundAgent.SetIsLadderQueueUsing(true);
						this._userAgents.Add(lastFoundAgent);
					}
				}
				AgentProximityMap.FindNext(Mission.Current, ref proximityMapSearchStruct);
			}
			int num7 = this._userAgents.Count - num;
			int num8 = Math.Min(this._maxClimberCount - num, this._maxRunnerCount - num7);
			if (!this._blockUsage && num8 > 0)
			{
				float num9 = float.MaxValue;
				int num10 = -1;
				for (int n = 0; n < this._queuedAgents.Count; n++)
				{
					if (this._queuedAgents[n] != null)
					{
						float lengthSquared = (this._queuedAgents[n].Position - groundVec).LengthSquared;
						if (lengthSquared < num9)
						{
							num9 = lengthSquared;
							num10 = n;
						}
					}
				}
				if (num10 >= 0)
				{
					this._queuedAgents[num10].SetIsLadderQueueUsing(true);
					this._userAgents.Add(this._queuedAgents[num10]);
					this._queuedAgents[num10].HumanAIComponent.AdjustSpeedLimit(this._queuedAgents[num10], 0.2f, true);
					this._usingAgentResetTime = 2f;
					this.RemoveAgentFromQueueAtIndex(num10);
				}
			}
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x000BDBFC File Offset: 0x000BBDFC
		protected internal override void OnTickParallel(float dt)
		{
			if (this._neighborLadderQueueManager == null)
			{
				this.OnTickParallelAux(dt);
				return;
			}
			LadderQueueManager ladderQueueManager = ((base.Id.Id < this._neighborLadderQueueManager.Id.Id) ? this : this._neighborLadderQueueManager);
			lock (ladderQueueManager)
			{
				this.OnTickParallelAux(dt);
			}
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x000BDC70 File Offset: 0x000BBE70
		protected internal override void OnTick(float dt)
		{
			if (!GameNetwork.IsClientOrReplay && !this.IsDeactivated && !this._blockUsage)
			{
				if (this.ManagedNavigationFaceId > 1 && this._lastUserCostPenaltyPerLadder[0].Item2)
				{
					this._lastUserCostPenaltyPerLadder[0].Item2 = false;
					this.CostAddition = this.GetNavigationFaceCostPerClimber(this._lastUserCostPenaltyPerLadder[0].Item1);
					Mission.Current.SetNavigationFaceCostWithIdAroundPosition(this.ManagedNavigationFaceId, this._managedGlobalWorldPosition.GetGroundVec3(), this.CostAddition);
				}
				if (this.ManagedNavigationFaceAlternateID1 > 1 && this._lastUserCostPenaltyPerLadder[1].Item2)
				{
					this._lastUserCostPenaltyPerLadder[1].Item2 = false;
					Mission.Current.SetNavigationFaceCostWithIdAroundPosition(this.ManagedNavigationFaceAlternateID1, this._managedGlobalWorldPosition.GetGroundVec3(), this.GetNavigationFaceCostPerClimber(this._lastUserCostPenaltyPerLadder[1].Item1));
				}
				if (this.ManagedNavigationFaceAlternateID2 > 1 && this._lastUserCostPenaltyPerLadder[2].Item2)
				{
					this._lastUserCostPenaltyPerLadder[2].Item2 = false;
					Mission.Current.SetNavigationFaceCostWithIdAroundPosition(this.ManagedNavigationFaceAlternateID2, this._managedGlobalWorldPosition.GetGroundVec3(), this.GetNavigationFaceCostPerClimber(this._lastUserCostPenaltyPerLadder[2].Item1));
				}
			}
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x000BDDCC File Offset: 0x000BBFCC
		private bool ConditionsAreMet(Agent agent, Agent.AIScriptedFrameFlags flags)
		{
			return agent.IsAIControlled && agent.IsActive() && agent.Team != null && agent.Team.Side == this._managedSide && agent.MovementLockedState == AgentMovementLockedState.None && !agent.IsUsingGameObject && !agent.InteractingWithAnyGameObject() && !agent.IsDetachedFromFormation && agent.Position.z - this._managedGlobalWorldPosition.GetGroundZ() < this._zDifferenceToStopUsing && !this._userAgents.Contains(agent) && agent.GetScriptedFlags() == flags && agent.GetCurrentNavigationFaceId() != this.ManagedNavigationFaceId && (!this._doesManageMultipleIDs || (agent.GetCurrentNavigationFaceId() != this.ManagedNavigationFaceAlternateID1 && agent.GetCurrentNavigationFaceId() != this.ManagedNavigationFaceAlternateID2));
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x000BDEA7 File Offset: 0x000BC0A7
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this._userAgents.Clear();
			this.IsDeactivated = true;
			this._deactivationDelayTimerElapsed = true;
			this._queuedAgents.Clear();
			this._queuedAgentCount = 0;
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x000BDEDC File Offset: 0x000BC0DC
		private void GetParentIndicesForQueueIndex(int queueIndex, out int parentIndex1, out int parentIndex2)
		{
			parentIndex1 = -1;
			parentIndex2 = -1;
			Vec2i coordinatesForQueueIndex = this.GetCoordinatesForQueueIndex(queueIndex);
			int num = coordinatesForQueueIndex.Y - 1;
			if (num >= 0)
			{
				int num2 = MathF.Max(this.GetRowSize(num) - 1, 1);
				int num3 = MathF.Max(this.GetRowSize(coordinatesForQueueIndex.Y) - 1, 1);
				float num4 = (float)coordinatesForQueueIndex.X * (float)num2 / (float)num3;
				parentIndex1 = (int)num4;
				float num5 = MathF.Abs(num4 - (float)parentIndex1);
				if (num5 > 0.2f)
				{
					if (num5 > 0.8f)
					{
						parentIndex1++;
					}
					else
					{
						parentIndex2 = parentIndex1 + 1;
					}
				}
				parentIndex1 = this.GetQueueIndexForCoordinates(new Vec2i(parentIndex1, num));
				if (parentIndex2 >= 0)
				{
					parentIndex2 = this.GetQueueIndexForCoordinates(new Vec2i(parentIndex2, num));
				}
			}
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x000BDF94 File Offset: 0x000BC194
		private float GetScoreForAddingAgentToQueueIndex(Vec3 agentPosition, int queueIndex, out int scoreOfQueueIndex)
		{
			scoreOfQueueIndex = queueIndex;
			float num = float.MinValue;
			if (this._queuedAgents.Count <= queueIndex || this._queuedAgents[queueIndex] == null)
			{
				int num2;
				int num3;
				this.GetParentIndicesForQueueIndex(queueIndex, out num2, out num3);
				if (num2 < 0 || (this._queuedAgents.Count > num2 && this._queuedAgents[num2] != null) || (num3 >= 0 && this._queuedAgents.Count > num3 && this._queuedAgents[num3] != null))
				{
					Vec2i coordinatesForQueueIndex = this.GetCoordinatesForQueueIndex(queueIndex);
					num = (float)coordinatesForQueueIndex.Y * this._queueRowSize * -3f;
					num -= (agentPosition.AsVec2 - this.GetQueuePositionForCoordinates(coordinatesForQueueIndex, -1).AsVec2).Length;
				}
				if (num2 >= 0 && (this._queuedAgents.Count <= num2 || this._queuedAgents[num2] == null))
				{
					int num4;
					float scoreForAddingAgentToQueueIndex = this.GetScoreForAddingAgentToQueueIndex(agentPosition, num2, out num4);
					if (num < scoreForAddingAgentToQueueIndex)
					{
						scoreOfQueueIndex = num4;
						num = scoreForAddingAgentToQueueIndex;
					}
				}
				if (num3 >= 0 && (this._queuedAgents.Count <= num3 || this._queuedAgents[num3] == null))
				{
					int num5;
					float scoreForAddingAgentToQueueIndex2 = this.GetScoreForAddingAgentToQueueIndex(agentPosition, num3, out num5);
					if (num < scoreForAddingAgentToQueueIndex2)
					{
						scoreOfQueueIndex = num5;
						num = scoreForAddingAgentToQueueIndex2;
					}
				}
			}
			return num;
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x000BE0CC File Offset: 0x000BC2CC
		private void AddAgentToQueue(Agent agent)
		{
			int y = this.GetCoordinatesForQueueIndex(this._queuedAgents.Count).Y;
			int rowSize = this.GetRowSize(y);
			Vec3 position = agent.Position;
			int num = -1;
			float num2 = float.MinValue;
			for (int i = 0; i < rowSize; i++)
			{
				int num3;
				float scoreForAddingAgentToQueueIndex = this.GetScoreForAddingAgentToQueueIndex(position, this.GetQueueIndexForCoordinates(new Vec2i(i, y)), out num3);
				if (scoreForAddingAgentToQueueIndex > num2)
				{
					num2 = scoreForAddingAgentToQueueIndex;
					num = num3;
				}
			}
			while (this._queuedAgents.Count <= num)
			{
				this._queuedAgents.Add(null);
			}
			this._queuedAgents[num] = agent;
			WorldPosition queuePositionForIndex = this.GetQueuePositionForIndex(num, agent.Index);
			agent.SetScriptedPosition(ref queuePositionForIndex, false, Agent.AIScriptedFrameFlags.None);
			agent.SetIsInLadderQueue(true);
			this._queuedAgentCount++;
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000BE198 File Offset: 0x000BC398
		private void RemoveAgentFromQueueAtIndex(int queueIndex)
		{
			this._queuedAgentCount--;
			if (!this._queuedAgents[queueIndex].IsUsingGameObject && (!this._queuedAgents[queueIndex].IsAIControlled || !this._queuedAgents[queueIndex].AIMoveToGameObjectIsEnabled()))
			{
				this._queuedAgents[queueIndex].DisableScriptedMovement();
				this._queuedAgents[queueIndex].SetIsInLadderQueue(false);
			}
			this._queuedAgents[queueIndex] = null;
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x000BE21C File Offset: 0x000BC41C
		private float GetNavigationFaceCost(int rowIndex)
		{
			return this._baseCost + (float)MathF.Max(rowIndex - 1, 0) * this._costPerRow;
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x000BE236 File Offset: 0x000BC436
		private float GetNavigationFaceCostPerClimber(float costPenalty)
		{
			return this._baseCost + costPenalty * this._costPerRow;
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x000BE248 File Offset: 0x000BC448
		private void MoveAgentFromQueueIndexToQueueIndex(int fromQueueIndex, int toQueueIndex)
		{
			this._queuedAgents[toQueueIndex] = this._queuedAgents[fromQueueIndex];
			this._queuedAgents[fromQueueIndex] = null;
			WorldPosition queuePositionForIndex = this.GetQueuePositionForIndex(toQueueIndex, this._queuedAgents[toQueueIndex].Index);
			this._queuedAgents[toQueueIndex].SetScriptedPosition(ref queuePositionForIndex, false, Agent.AIScriptedFrameFlags.None);
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x000BE2A8 File Offset: 0x000BC4A8
		private int GetRowSize(int rowIndex)
		{
			float num = this._arcAngle * (this._queueBeginDistance + this._queueRowSize * (float)rowIndex);
			return 1 + (int)(num / this._agentSpacing);
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x000BE2D8 File Offset: 0x000BC4D8
		private int GetQueueIndexForCoordinates(Vec2i coordinates)
		{
			int num = coordinates.X;
			for (int i = 0; i < coordinates.Y; i++)
			{
				num += this.GetRowSize(i);
			}
			return num;
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x000BE308 File Offset: 0x000BC508
		private Vec2i GetCoordinatesForQueueIndex(int queueIndex)
		{
			Vec2i vec2i = default(Vec2i);
			for (;;)
			{
				int rowSize = this.GetRowSize(vec2i.Y);
				if (rowSize > queueIndex)
				{
					break;
				}
				queueIndex -= rowSize;
				vec2i.Y++;
			}
			vec2i.X = queueIndex;
			return vec2i;
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x000BE34C File Offset: 0x000BC54C
		private WorldPosition GetQueuePositionForCoordinates(Vec2i coordinates, int randomSeed)
		{
			MatrixFrame managedGlobalFrame = this._managedGlobalFrame;
			WorldPosition managedGlobalWorldPosition = this._managedGlobalWorldPosition;
			float num = 0f;
			int rowSize = this.GetRowSize(coordinates.Y);
			if (rowSize > 1)
			{
				num = this._arcAngle * ((float)coordinates.X / (float)(rowSize - 1) - 0.5f);
			}
			managedGlobalFrame.rotation.RotateAboutForward(num);
			managedGlobalFrame.origin += managedGlobalFrame.rotation.u * (this._queueBeginDistance + this._queueRowSize * (float)coordinates.Y);
			if (randomSeed >= 0)
			{
				Random random = new Random(coordinates.X * 100000 + coordinates.Y * 10000000 + randomSeed);
				managedGlobalFrame.rotation.RotateAboutForward(random.NextFloat() * 3.1415927f * 2f);
				managedGlobalFrame.origin += managedGlobalFrame.rotation.u * random.NextFloat() * 0.3f;
			}
			managedGlobalWorldPosition.SetVec2(managedGlobalFrame.origin.AsVec2);
			return managedGlobalWorldPosition;
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x000BE472 File Offset: 0x000BC672
		private WorldPosition GetQueuePositionForIndex(int queueIndex, int randomSeed)
		{
			return this.GetQueuePositionForCoordinates(this.GetCoordinatesForQueueIndex(queueIndex), randomSeed);
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x000BE484 File Offset: 0x000BC684
		public void FlushQueueManager()
		{
			int num = this._queuedAgentCount / 2;
			for (int i = this._queuedAgents.Count - 1; i >= num; i--)
			{
				if (this._queuedAgents[i] != null)
				{
					this.RemoveAgentFromQueueAtIndex(i);
				}
			}
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x000BE4C7 File Offset: 0x000BC6C7
		public void AssignNeighborQueueManager(LadderQueueManager neighborLadderQueueManager)
		{
			this._neighborLadderQueueManager = neighborLadderQueueManager;
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x000BE4D0 File Offset: 0x000BC6D0
		private bool IsFormationPositionOtherSideOfTheCastle(Agent agent)
		{
			UIntPtr navMesh = agent.Formation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.NavMeshVec3).GetNavMesh();
			UIntPtr navMesh2 = agent.GetWorldPosition().GetNavMesh();
			ref PathFaceRecord pathFaceRecordFromNavMeshFacePointer = base.Scene.GetPathFaceRecordFromNavMeshFacePointer(navMesh);
			PathFaceRecord pathFaceRecordFromNavMeshFacePointer2 = base.Scene.GetPathFaceRecordFromNavMeshFacePointer(navMesh2);
			return pathFaceRecordFromNavMeshFacePointer.FaceGroupIndex % 10 != pathFaceRecordFromNavMeshFacePointer2.FaceGroupIndex % 10;
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x000BE530 File Offset: 0x000BC730
		private bool ShouldAgentUseTheLadder(Agent agent)
		{
			if (!agent.IsFormationFrameEnabled)
			{
				return agent.HasPathThroughNavigationFacesIDFromDirectionMT(this.ManagedNavigationFaceId, this.ManagedNavigationFaceAlternateID1, this.ManagedNavigationFaceAlternateID2, Vec2.Zero);
			}
			return this.IsFormationPositionOtherSideOfTheCastle(agent);
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x000BE55F File Offset: 0x000BC75F
		public void OnFormationFrameChanged(Agent agent, bool hasFrame, WorldPosition frame)
		{
			if (agent.IsInLadderQueue && this._queuedAgents.Contains(agent) && (!this.ConditionsAreMet(agent, Agent.AIScriptedFrameFlags.GoToPosition) || !this.ShouldAgentUseTheLadder(agent)))
			{
				this.RemoveAgentFromQueueAtIndex(this._queuedAgents.IndexOf(agent));
			}
		}

		// Token: 0x0400139A RID: 5018
		public int ManagedNavigationFaceId;

		// Token: 0x0400139B RID: 5019
		public int ManagedNavigationFaceAlternateID1;

		// Token: 0x0400139C RID: 5020
		public int ManagedNavigationFaceAlternateID2;

		// Token: 0x0400139D RID: 5021
		public float CostAddition;

		// Token: 0x0400139E RID: 5022
		private readonly List<Agent> _userAgents = new List<Agent>();

		// Token: 0x0400139F RID: 5023
		private readonly List<Agent> _queuedAgents = new List<Agent>();

		// Token: 0x040013A0 RID: 5024
		private MatrixFrame _managedEntitialFrame;

		// Token: 0x040013A1 RID: 5025
		private Vec2 _managedEntitialDirection;

		// Token: 0x040013A2 RID: 5026
		private Vec3 _lastCachedGameEntityGlobalPosition;

		// Token: 0x040013A3 RID: 5027
		private MatrixFrame _managedGlobalFrame;

		// Token: 0x040013A4 RID: 5028
		private WorldPosition _managedGlobalWorldPosition;

		// Token: 0x040013A5 RID: 5029
		private Vec2 _managedGlobalDirection;

		// Token: 0x040013A6 RID: 5030
		private BattleSideEnum _managedSide;

		// Token: 0x040013A7 RID: 5031
		private bool _blockUsage;

		// Token: 0x040013A8 RID: 5032
		private int _maxUserCount;

		// Token: 0x040013A9 RID: 5033
		private int _queuedAgentCount;

		// Token: 0x040013AA RID: 5034
		private float _arcAngle = 2.3561945f;

		// Token: 0x040013AB RID: 5035
		private float _queueBeginDistance = 1f;

		// Token: 0x040013AC RID: 5036
		private float _queueRowSize = 0.8f;

		// Token: 0x040013AD RID: 5037
		private float _agentSpacing = 1f;

		// Token: 0x040013AE RID: 5038
		private float _timeSinceLastUpdate;

		// Token: 0x040013AF RID: 5039
		private float _updatePeriod;

		// Token: 0x040013B0 RID: 5040
		private float _usingAgentResetTime;

		// Token: 0x040013B1 RID: 5041
		private float _costPerRow;

		// Token: 0x040013B2 RID: 5042
		private float _baseCost;

		// Token: 0x040013B3 RID: 5043
		private float _zDifferenceToStopUsing = 2f;

		// Token: 0x040013B4 RID: 5044
		private float _distanceToStopUsing2d = 5f;

		// Token: 0x040013B5 RID: 5045
		private bool _doesManageMultipleIDs;

		// Token: 0x040013B6 RID: 5046
		private int _maxClimberCount = 18;

		// Token: 0x040013B7 RID: 5047
		private int _maxRunnerCount = 6;

		// Token: 0x040013B8 RID: 5048
		private Timer _deactivateTimer;

		// Token: 0x040013B9 RID: 5049
		private bool _deactivationDelayTimerElapsed = true;

		// Token: 0x040013BA RID: 5050
		private LadderQueueManager _neighborLadderQueueManager;

		// Token: 0x040013BB RID: 5051
		private ValueTuple<float, bool>[] _lastUserCostPenaltyPerLadder;
	}
}
