using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200010D RID: 269
	public class HumanAIComponent : AgentComponent
	{
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00018AE4 File Offset: 0x00016CE4
		// (set) Token: 0x06000D9C RID: 3484 RVA: 0x00018AEC File Offset: 0x00016CEC
		public Agent FollowedAgent { get; private set; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00018AF5 File Offset: 0x00016CF5
		// (set) Token: 0x06000D9E RID: 3486 RVA: 0x00018AFD File Offset: 0x00016CFD
		public bool ShouldCatchUpWithFormation
		{
			get
			{
				return this._shouldCatchUpWithFormation;
			}
			private set
			{
				if (this._shouldCatchUpWithFormation != value)
				{
					this._shouldCatchUpWithFormation = value;
					this.Agent.SetShouldCatchUpWithFormation(value);
				}
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x00018B1B File Offset: 0x00016D1B
		public bool IsDefending
		{
			get
			{
				return this._objectInterestKind == HumanAIComponent.UsableObjectInterestKind.Defending;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x00018B26 File Offset: 0x00016D26
		public bool HasTimedScriptedFrame
		{
			get
			{
				return this._scriptedFrameTimer > 0f;
			}
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00018B38 File Offset: 0x00016D38
		public HumanAIComponent(Agent agent)
			: base(agent)
		{
			this._behaviorValues = new HumanAIComponent.BehaviorValues[7];
			this._lastBehaviorValueSet = HumanAIComponent.BehaviorValueSet.Overriden;
			this.SetBehaviorValueSet(HumanAIComponent.BehaviorValueSet.Default);
			this.Agent.SetAllBehaviorParams(this._behaviorValues);
			this._hasNewBehaviorValues = false;
			Agent agent2 = this.Agent;
			agent2.OnAgentWieldedItemChange = (Action)Delegate.Combine(agent2.OnAgentWieldedItemChange, new Action(this.DisablePickUpForAgentIfNeeded));
			Agent agent3 = this.Agent;
			agent3.OnAgentMountedStateChanged = (Action)Delegate.Combine(agent3.OnAgentMountedStateChanged, new Action(this.DisablePickUpForAgentIfNeeded));
			this._itemPickUpTickTimer = new MissionTimer(2.5f + MBRandom.RandomFloat);
			this._mountSearchTimer = new MissionTimer(2f + MBRandom.RandomFloat);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00018C20 File Offset: 0x00016E20
		public void OverrideBehaviorParams(HumanAIComponent.AISimpleBehaviorKind behavior, float y1, float x2, float y2, float x3, float y3)
		{
			this._lastBehaviorValueSet = HumanAIComponent.BehaviorValueSet.Overriden;
			this._behaviorValues[(int)behavior].y1 = y1;
			this._behaviorValues[(int)behavior].x2 = x2;
			this._behaviorValues[(int)behavior].y2 = y2;
			this._behaviorValues[(int)behavior].x3 = x3;
			this._behaviorValues[(int)behavior].y3 = y3;
			this._hasNewBehaviorValues = true;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00018C98 File Offset: 0x00016E98
		private void SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind behavior, float y1, float x2, float y2, float x3, float y3)
		{
			this._behaviorValues[(int)behavior].y1 = y1;
			this._behaviorValues[(int)behavior].x2 = x2;
			this._behaviorValues[(int)behavior].y2 = y2;
			this._behaviorValues[(int)behavior].x3 = x3;
			this._behaviorValues[(int)behavior].y3 = y3;
			this._hasNewBehaviorValues = true;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00018D09 File Offset: 0x00016F09
		public void SyncBehaviorParamsIfNecessary()
		{
			if (this._hasNewBehaviorValues)
			{
				this.Agent.SetAllBehaviorParams(this._behaviorValues);
				this._hasNewBehaviorValues = false;
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00018D2C File Offset: 0x00016F2C
		public void DisablePickUpForAgentIfNeeded()
		{
			this._disablePickUpForAgent = true;
			if (this.Agent.MountAgent == null)
			{
				if (this.Agent.HasLostShield())
				{
					this._disablePickUpForAgent = false;
				}
				else
				{
					for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
					{
						MissionWeapon missionWeapon = this.Agent.Equipment[equipmentIndex];
						if (!missionWeapon.IsEmpty && missionWeapon.IsAnyConsumable())
						{
							this._disablePickUpForAgent = false;
							break;
						}
					}
				}
			}
			if (this._disablePickUpForAgent && this.Agent.Formation != null && MissionGameModels.Current.BattleBannerBearersModel.IsBannerSearchingAgent(this.Agent))
			{
				this._disablePickUpForAgent = false;
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00018DCF File Offset: 0x00016FCF
		public override void OnTickParallel(float dt)
		{
			if (this.Agent.Mission.AllowAiTicking && this.Agent.IsAIControlled)
			{
				this.SyncBehaviorParamsIfNecessary();
			}
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00018DF8 File Offset: 0x00016FF8
		private void ItemPickupTick()
		{
			if (this._itemToPickUp != null)
			{
				if (!this._itemToPickUp.IsAIMovingTo(this.Agent) || this.Agent.Mission.MissionEnded)
				{
					this._itemToPickUp = null;
				}
				else if (!this._itemToPickUp.GameEntity.IsValid)
				{
					this.Agent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
			}
			if (this._itemPickUpTickTimer.Check(true) && !this.Agent.Mission.MissionEnded)
			{
				EquipmentIndex primaryWieldedItemIndex = this.Agent.GetPrimaryWieldedItemIndex();
				WeaponComponentData weaponComponentData = ((primaryWieldedItemIndex == EquipmentIndex.None) ? null : this.Agent.Equipment[primaryWieldedItemIndex].CurrentUsageItem);
				bool flag = weaponComponentData != null && weaponComponentData.IsRangedWeapon;
				if (!this._disablePickUpForAgent && MissionGameModels.Current.ItemPickupModel.IsAgentEquipmentSuitableForPickUpAvailability(this.Agent) && this.Agent.CanBeAssignedForScriptedMovement() && this.Agent.IsAlarmed() && (this.Agent.GetAgentFlags() & AgentFlag.CanAttack) != AgentFlag.None && !this.IsInImportantCombatAction() && !this.Agent.IsInWater())
				{
					Agent targetAgent = this.Agent.GetTargetAgent();
					if ((targetAgent == null || (targetAgent.Position.DistanceSquared(this.Agent.Position) > 400f && (!flag || this.IsAnyConsumableDepleted() || targetAgent.Position.DistanceSquared(this.Agent.Position) >= this.Agent.GetMissileRange() * 1.2f || this.Agent.GetLastTargetVisibilityState() != AITargetVisibilityState.TargetIsClear))) && this._itemToPickUp == null)
					{
						float maximumForwardUnlimitedSpeed = this.Agent.GetMaximumForwardUnlimitedSpeed();
						Vec3 vec = this.Agent.Position - new Vec3(maximumForwardUnlimitedSpeed, maximumForwardUnlimitedSpeed, 1f, -1f);
						Vec3 vec2 = this.Agent.Position + new Vec3(maximumForwardUnlimitedSpeed, maximumForwardUnlimitedSpeed, 1.8f, -1f);
						this._itemToPickUp = this.SelectPickableItem(vec, vec2);
						if (this._itemToPickUp != null)
						{
							this.RequestMoveToItem(this._itemToPickUp);
						}
					}
				}
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00019034 File Offset: 0x00017234
		public override void OnTick(float dt)
		{
			if (this.Agent.Mission.AllowAiTicking && this.Agent.IsAIControlled)
			{
				if (!this._forceDisableItemPickup)
				{
					this.ItemPickupTick();
				}
				if (!this._forceDisableItemPickup && this._itemToPickUp != null && !this.Agent.IsRunningAway && this.Agent.AIMoveToGameObjectIsEnabled())
				{
					float num = (this._itemToPickUp.IsBanner() ? MissionGameModels.Current.BattleBannerBearersModel.GetBannerInteractionDistance(this.Agent) : MissionGameModels.Current.AgentStatCalculateModel.GetInteractionDistance(this.Agent));
					num *= 3f;
					ref WorldFrame ptr = ref this._itemToPickUp.GetUserFrameForAgent(this.Agent);
					Vec3 vec = this.Agent.Position;
					float num2 = ptr.Origin.DistanceSquaredWithLimit(in vec, num * num + 1E-05f);
					if (this.Agent.CanReachAndUseObject(this._itemToPickUp, num2))
					{
						this.Agent.UseGameObject(this._itemToPickUp, -1);
					}
				}
				if (this.Agent.CommonAIComponent != null && this.Agent.MountAgent == null && !this.Agent.CommonAIComponent.IsRetreating && this._mountSearchTimer.Check(true) && this.Agent.GetRidingOrder() == 1)
				{
					Agent agent = this.FindReservedMount();
					bool flag;
					if (agent != null && agent.State == AgentState.Active && agent.RiderAgent == null)
					{
						Vec3 vec = this.Agent.Position;
						flag = vec.DistanceSquared(agent.Position) >= 256f;
					}
					else
					{
						flag = true;
					}
					if (flag)
					{
						if (agent != null)
						{
							this.UnreserveMount(agent);
						}
						Agent agent2 = this.FindClosestMountAvailable();
						if (agent2 != null)
						{
							this.ReserveMount(agent2);
						}
					}
				}
				if (this._scriptedFrameTimer > 0f)
				{
					this._scriptedFrameTimer -= dt;
					if (this._scriptedFrameTimer < 0f)
					{
						this.Agent.DisableScriptedMovement();
					}
				}
			}
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00019234 File Offset: 0x00017434
		private Agent FindClosestMountAvailable()
		{
			float num = 6400f;
			Agent agent = null;
			float num2 = 6400f;
			Agent agent2 = null;
			foreach (KeyValuePair<Agent, MissionTime> keyValuePair in Mission.Current.MountsWithoutRiders)
			{
				Agent key = keyValuePair.Key;
				if (key.IsActive() && key.RiderAgent == null && !key.IsRunningAway && MissionGameModels.Current.AgentStatCalculateModel.CanAgentRideMount(this.Agent, key))
				{
					float num3 = this.Agent.Position.DistanceSquared(key.Position);
					if (num2 > num3)
					{
						agent2 = key;
						num2 = num3;
						if (agent2.CommonAIComponent.ReservedRiderAgentIndex < 0)
						{
							num = num3;
							agent = key;
						}
					}
					else if (num > num3 && agent2.CommonAIComponent.ReservedRiderAgentIndex < 0)
					{
						num = num3;
						agent = key;
					}
				}
			}
			if (agent2 != agent)
			{
				if (agent != null && num > 0.01f && num2 / num >= 0.4f)
				{
					agent2 = agent;
				}
				else
				{
					Agent agent3 = Mission.Current.FindAgentWithIndex(agent2.CommonAIComponent.ReservedRiderAgentIndex);
					float num4 = agent3.Position.DistanceSquared(agent2.Position);
					if (num4 > 0.01f && num2 / num4 < ((agent != null) ? 0.4f : 0.7f))
					{
						agent3.HumanAIComponent.UnreserveMount(agent2);
					}
					else
					{
						agent2 = agent;
					}
				}
			}
			return agent2;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000193B0 File Offset: 0x000175B0
		private Agent FindReservedMount()
		{
			Agent agent = null;
			int selectedMountIndex = this.Agent.GetSelectedMountIndex();
			if (selectedMountIndex >= 0)
			{
				foreach (KeyValuePair<Agent, MissionTime> keyValuePair in Mission.Current.MountsWithoutRiders)
				{
					Agent key = keyValuePair.Key;
					if (key.Index == selectedMountIndex)
					{
						agent = key;
						break;
					}
				}
			}
			return agent;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0001942C File Offset: 0x0001762C
		internal void ReserveMount(Agent mount)
		{
			this.Agent.SetSelectedMountIndex(mount.Index);
			mount.CommonAIComponent.OnMountReserved(this.Agent.Index);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00019455 File Offset: 0x00017655
		internal void UnreserveMount(Agent mount)
		{
			this.Agent.SetSelectedMountIndex(-1);
			mount.CommonAIComponent.OnMountUnreserved();
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00019470 File Offset: 0x00017670
		public override void OnAgentRemoved()
		{
			Agent agent = this.FindReservedMount();
			if (agent != null)
			{
				this.UnreserveMount(agent);
			}
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00019490 File Offset: 0x00017690
		public override void OnComponentRemoved()
		{
			Agent agent = this.FindReservedMount();
			if (agent != null)
			{
				this.UnreserveMount(agent);
			}
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x000194B0 File Offset: 0x000176B0
		public bool IsInImportantCombatAction()
		{
			Agent.ActionCodeType currentActionType = this.Agent.GetCurrentActionType(1);
			return currentActionType == Agent.ActionCodeType.ReadyMelee || currentActionType == Agent.ActionCodeType.ReadyRanged || currentActionType == Agent.ActionCodeType.ReleaseMelee || currentActionType == Agent.ActionCodeType.ReleaseRanged || currentActionType == Agent.ActionCodeType.ReleaseThrowing || currentActionType == Agent.ActionCodeType.DefendShield;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x000194EC File Offset: 0x000176EC
		private bool IsAnyConsumableDepleted()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
			{
				MissionWeapon missionWeapon = this.Agent.Equipment[equipmentIndex];
				if (!missionWeapon.IsEmpty && missionWeapon.IsAnyConsumable() && missionWeapon.Amount == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00019538 File Offset: 0x00017738
		private SpawnedItemEntity SelectPickableItem(Vec3 bMin, Vec3 bMax)
		{
			Agent targetAgent = this.Agent.GetTargetAgent();
			Vec3 vec = ((targetAgent == null) ? Vec3.Invalid : (targetAgent.Position - this.Agent.Position));
			int num = this.Agent.Mission.Scene.SelectEntitiesInBoxWithScriptComponent<SpawnedItemEntity>(ref bMin, ref bMax, this._tempPickableEntities, this._pickableItemsId, false);
			float num2 = 0f;
			SpawnedItemEntity spawnedItemEntity = null;
			for (int i = 0; i < num; i++)
			{
				WeakGameEntity weakGameEntity = this._tempPickableEntities[i];
				SpawnedItemEntity firstScriptOfType = weakGameEntity.GetFirstScriptOfType<SpawnedItemEntity>();
				bool flag = false;
				if (firstScriptOfType != null)
				{
					MissionWeapon weaponCopy = firstScriptOfType.WeaponCopy;
					flag = !weaponCopy.IsEmpty && (weaponCopy.IsShield() || weaponCopy.IsBanner() || firstScriptOfType.IsStuckMissile() || firstScriptOfType.IsQuiverAndNotEmpty());
				}
				if (flag && !firstScriptOfType.HasUser && (!firstScriptOfType.HasAIMovingTo || firstScriptOfType.IsAIMovingTo(this.Agent)) && firstScriptOfType.GameEntityWithWorldPosition.GetNavMesh() != UIntPtr.Zero)
				{
					WorldFrame worldFrame = firstScriptOfType.GetUserFrameForAgent(this.Agent);
					Vec3 vec2 = worldFrame.Origin.GetGroundVec3() - this.Agent.Position;
					float z = vec2.z;
					vec2.Normalize();
					if (targetAgent == null || vec.Length - Vec3.DotProduct(vec, vec2) > targetAgent.GetMaximumForwardUnlimitedSpeed() * 3f)
					{
						EquipmentIndex equipmentIndex = MissionEquipment.SelectWeaponPickUpSlot(this.Agent, firstScriptOfType.WeaponCopy, firstScriptOfType.IsStuckMissile());
						if (equipmentIndex != EquipmentIndex.None && firstScriptOfType.GameEntityWithWorldPosition.GetNavMesh() != UIntPtr.Zero)
						{
							Agent agent = this.Agent;
							UsableMissionObject usableMissionObject = firstScriptOfType;
							worldFrame = firstScriptOfType.GetUserFrameForAgent(this.Agent);
							if (agent.CanReachObjectFromPosition(usableMissionObject, worldFrame.Origin.GetGroundVec3().DistanceSquared(firstScriptOfType.GameEntityWithWorldPosition.GetNavMeshVec3()), firstScriptOfType.GameEntityWithWorldPosition.GetNavMeshVec3()) && MissionGameModels.Current.ItemPickupModel.IsItemAvailableForAgent(firstScriptOfType, this.Agent, equipmentIndex))
							{
								Agent agent2 = this.Agent;
								Vec2 asVec = firstScriptOfType.GameEntityWithWorldPosition.AsVec2;
								if (agent2.CanMoveDirectlyToPosition(in asVec) && (!this.Agent.Mission.IsPositionInsideAnyBlockerNavMeshFace2D(firstScriptOfType.GameEntityWithWorldPosition.AsVec2) || MathF.Abs(z) >= 1.5f))
								{
									float itemScoreForAgent = MissionGameModels.Current.ItemPickupModel.GetItemScoreForAgent(firstScriptOfType, this.Agent);
									if (itemScoreForAgent > num2)
									{
										spawnedItemEntity = firstScriptOfType;
										num2 = itemScoreForAgent;
									}
								}
							}
						}
					}
				}
			}
			return spawnedItemEntity;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x000197D5 File Offset: 0x000179D5
		internal void ItemPickupDone(SpawnedItemEntity spawnedItemEntity)
		{
			this._itemToPickUp = null;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x000197DE File Offset: 0x000179DE
		private void RequestMoveToItem(SpawnedItemEntity item)
		{
			Agent movingAgent = item.MovingAgent;
			if (movingAgent != null)
			{
				movingAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
			this.MoveToUsableGameObject(item, null, Agent.AIScriptedFrameFlags.NoAttack);
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x000197FC File Offset: 0x000179FC
		public UsableMissionObject GetCurrentlyMovingGameObject()
		{
			return this._objectOfInterest;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00019804 File Offset: 0x00017A04
		private void SetCurrentlyMovingGameObject(UsableMissionObject objectOfInterest)
		{
			this._objectOfInterest = objectOfInterest;
			this._objectInterestKind = ((this._objectOfInterest != null) ? HumanAIComponent.UsableObjectInterestKind.MovingTo : HumanAIComponent.UsableObjectInterestKind.None);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0001981F File Offset: 0x00017A1F
		public UsableMissionObject GetCurrentlyDefendingGameObject()
		{
			return this._objectOfInterest;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00019827 File Offset: 0x00017A27
		private void SetCurrentlyDefendingGameObject(UsableMissionObject objectOfInterest)
		{
			this._objectOfInterest = objectOfInterest;
			this._objectInterestKind = ((this._objectOfInterest != null) ? HumanAIComponent.UsableObjectInterestKind.Defending : HumanAIComponent.UsableObjectInterestKind.None);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00019844 File Offset: 0x00017A44
		public void MoveToUsableGameObject(UsableMissionObject usedObject, IDetachment detachment, Agent.AIScriptedFrameFlags scriptedFrameFlags = Agent.AIScriptedFrameFlags.NoAttack)
		{
			this.Agent.AIStateFlags |= Agent.AIStateFlag.UseObjectMoving;
			this.SetCurrentlyMovingGameObject(usedObject);
			usedObject.OnAIMoveToUse(this.Agent, detachment);
			WorldFrame userFrameForAgent = usedObject.GetUserFrameForAgent(this.Agent);
			this.Agent.SetScriptedPositionAndDirection(ref userFrameForAgent.Origin, userFrameForAgent.Rotation.f.AsVec2.RotationInRadians, false, scriptedFrameFlags);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x000198B3 File Offset: 0x00017AB3
		public void MoveToClear()
		{
			UsableMissionObject currentlyMovingGameObject = this.GetCurrentlyMovingGameObject();
			if (currentlyMovingGameObject != null)
			{
				currentlyMovingGameObject.OnMoveToStopped(this.Agent);
			}
			this.SetCurrentlyMovingGameObject(null);
			this.Agent.AIStateFlags &= ~Agent.AIStateFlag.UseObjectMoving;
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x000198E7 File Offset: 0x00017AE7
		public void StartDefendingGameObject(UsableMissionObject usedObject, IDetachment detachment)
		{
			this.SetCurrentlyDefendingGameObject(usedObject);
			usedObject.OnAIDefendBegin(this.Agent, detachment);
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x000198FD File Offset: 0x00017AFD
		public void StopDefendingGameObject()
		{
			this.GetCurrentlyDefendingGameObject().OnAIDefendEnd(this.Agent);
			this.SetCurrentlyDefendingGameObject(null);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00019917 File Offset: 0x00017B17
		public bool IsInterestedInAnyGameObject()
		{
			return this._objectInterestKind > HumanAIComponent.UsableObjectInterestKind.None;
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00019924 File Offset: 0x00017B24
		public bool IsInterestedInGameObject(UsableMissionObject usableMissionObject)
		{
			bool flag = false;
			switch (this._objectInterestKind)
			{
			case HumanAIComponent.UsableObjectInterestKind.None:
				break;
			case HumanAIComponent.UsableObjectInterestKind.MovingTo:
				flag = usableMissionObject == this.GetCurrentlyMovingGameObject();
				break;
			case HumanAIComponent.UsableObjectInterestKind.Defending:
				flag = usableMissionObject == this.GetCurrentlyDefendingGameObject();
				break;
			default:
				Debug.FailedAssert("Unexpected object interest kind: " + this._objectInterestKind, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\AgentComponents\\HumanAIComponent.cs", "IsInterestedInGameObject", 686);
				break;
			}
			return flag;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00019990 File Offset: 0x00017B90
		public void FollowAgent(Agent agent)
		{
			this.FollowedAgent = agent;
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0001999C File Offset: 0x00017B9C
		public float GetDesiredSpeedInFormation(bool isCharging)
		{
			if (!(this.Agent.Formation.Arrangement is ColumnFormation) && this.ShouldCatchUpWithFormation && (!isCharging || !Mission.Current.IsMissionEnding))
			{
				Agent mountAgent = this.Agent.MountAgent;
				float num = ((mountAgent != null) ? mountAgent.GetMaximumForwardUnlimitedSpeed() : this.Agent.GetMaximumForwardUnlimitedSpeed());
				bool flag = !isCharging;
				if (isCharging)
				{
					FormationQuerySystem cachedClosestEnemyFormation = this.Agent.Formation.CachedClosestEnemyFormation;
					float num2 = float.MaxValue;
					float num3 = 4f * num * num;
					if (cachedClosestEnemyFormation != null)
					{
						WorldPosition cachedMedianPosition = this.Agent.Formation.CachedMedianPosition;
						WorldPosition cachedMedianPosition2 = cachedClosestEnemyFormation.Formation.CachedMedianPosition;
						num2 = cachedMedianPosition.AsVec2.DistanceSquared(cachedMedianPosition2.AsVec2);
						if (num2 <= num3)
						{
							num2 = this.Agent.Formation.CachedMedianPosition.GetNavMeshVec3MT().DistanceSquared(cachedClosestEnemyFormation.Formation.CachedMedianPosition.GetNavMeshVec3MT());
						}
					}
					flag = num2 > num3;
				}
				if (flag)
				{
					Vec2 vec = this.Agent.Formation.GetCurrentGlobalPositionOfUnit(this.Agent, true) - this.Agent.Position.AsVec2;
					float num4 = -this.Agent.AverageVelocity.AsVec2.DotProduct(vec);
					num4 = MathF.Clamp(num4, 0f, 100f);
					float num5 = ((this.Agent.MountAgent != null) ? 4f : 2f);
					float num6 = (isCharging ? this.Agent.Formation.CachedFormationIntegrityData.AverageMaxUnlimitedSpeedExcludeFarAgents : this.Agent.Formation.CachedMovementSpeed) / num;
					return MathF.Clamp((0.7f + 0.4f * ((num - num4 * num5) / (num + num4 * num5))) * num6, 0.2f, 1f);
				}
			}
			return 1f;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00019B94 File Offset: 0x00017D94
		private unsafe bool GetFormationFrame(out WorldPosition formationPosition, out Vec2 formationDirection, out float speedLimit, out bool limitIsMultiplier)
		{
			Formation formation = this.Agent.Formation;
			limitIsMultiplier = false;
			bool flag = false;
			if (HumanAIComponent.FormationSpeedAdjustmentEnabled && this.Agent.IsMount)
			{
				formationPosition = WorldPosition.Invalid;
				formationDirection = Vec2.Invalid;
				if (this.Agent.RiderAgent == null || (this.Agent.RiderAgent != null && (!this.Agent.RiderAgent.IsActive() || this.Agent.RiderAgent.Formation == null)))
				{
					speedLimit = -1f;
				}
				else
				{
					limitIsMultiplier = true;
					HumanAIComponent humanAIComponent = this.Agent.RiderAgent.HumanAIComponent;
					MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
					speedLimit = humanAIComponent.GetDesiredSpeedInFormation(movementOrder.MovementState == MovementOrder.MovementStateEnum.Charge);
				}
			}
			else
			{
				bool baseFormationFrame = this.Agent.GetBaseFormationFrame(out formationPosition, out formationDirection);
				if (formation == null)
				{
					speedLimit = -1f;
				}
				else if (this.Agent.IsDetachedFromFormation)
				{
					speedLimit = -1f;
					flag = baseFormationFrame;
				}
				else
				{
					MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
					switch (movementOrder.MovementState)
					{
					case MovementOrder.MovementStateEnum.Charge:
						limitIsMultiplier = true;
						speedLimit = (HumanAIComponent.FormationSpeedAdjustmentEnabled ? this.GetDesiredSpeedInFormation(true) : (-1f));
						flag = formationPosition.IsValid;
						break;
					case MovementOrder.MovementStateEnum.Hold:
						if (HumanAIComponent.FormationSpeedAdjustmentEnabled && this.ShouldCatchUpWithFormation)
						{
							limitIsMultiplier = true;
							speedLimit = this.GetDesiredSpeedInFormation(false);
						}
						else
						{
							speedLimit = -1f;
						}
						flag = true;
						break;
					case MovementOrder.MovementStateEnum.Retreat:
						speedLimit = -1f;
						break;
					case MovementOrder.MovementStateEnum.StandGround:
						speedLimit = -1f;
						flag = true;
						break;
					default:
						Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\AgentComponents\\HumanAIComponent.cs", "GetFormationFrame", 836);
						speedLimit = -1f;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00019D4B File Offset: 0x00017F4B
		public void AdjustSpeedLimit(Agent agent, float desiredSpeed, bool limitIsMultiplier)
		{
			if (agent.MissionPeer != null)
			{
				desiredSpeed = -1f;
			}
			this.Agent.SetMaximumSpeedLimit(desiredSpeed, limitIsMultiplier);
			Agent mountAgent = agent.MountAgent;
			if (mountAgent == null)
			{
				return;
			}
			mountAgent.SetMaximumSpeedLimit(desiredSpeed, limitIsMultiplier);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00019D7C File Offset: 0x00017F7C
		public void ParallelUpdateFormationMovement()
		{
			WorldPosition worldPosition;
			Vec2 vec;
			float num;
			bool flag;
			bool formationFrame = this.GetFormationFrame(out worldPosition, out vec, out num, out flag);
			this.AdjustSpeedLimit(this.Agent, num, flag);
			if (this.Agent.Controller == AgentControllerType.AI && this.Agent.Formation != null && !(this.Agent.Formation.Arrangement is ColumnFormation) && this.Agent.Formation.GetReadonlyMovementOrderReference().OrderEnum != MovementOrder.MovementOrderEnum.Stop && this.Agent.Formation.GetReadonlyMovementOrderReference().OrderEnum != MovementOrder.MovementOrderEnum.Retreat && !this.Agent.IsRetreating())
			{
				Formation.FormationIntegrityDataGroup cachedFormationIntegrityData = this.Agent.Formation.CachedFormationIntegrityData;
				float num2 = cachedFormationIntegrityData.AverageMaxUnlimitedSpeedExcludeFarAgents * 3f;
				if (cachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents > num2)
				{
					this.ShouldCatchUpWithFormation = false;
					this.Agent.SetFormationIntegrityData(Vec2.Zero, Vec2.Zero, Vec2.Zero, 0f, 0f, false);
				}
				else
				{
					Vec2 currentGlobalPositionOfUnit = this.Agent.Formation.GetCurrentGlobalPositionOfUnit(this.Agent, true);
					float num3 = this.Agent.Position.AsVec2.Distance(currentGlobalPositionOfUnit);
					this.ShouldCatchUpWithFormation = num3 < num2 * 2f;
					bool flag2 = this.ShouldCatchUpWithFormation && this.Agent.GetOffhandWieldedItemIndex() == EquipmentIndex.ExtraWeaponSlot && this.Agent.Formation.QuerySystem.RangedUnitRatioReadOnly + this.Agent.Formation.QuerySystem.RangedCavalryUnitRatioReadOnly >= 0.5f;
					this.Agent.SetFormationIntegrityData(this.ShouldCatchUpWithFormation ? currentGlobalPositionOfUnit : Vec2.Zero, this.Agent.Formation.CurrentDirection, cachedFormationIntegrityData.AverageVelocityExcludeFarAgents, cachedFormationIntegrityData.AverageMaxUnlimitedSpeedExcludeFarAgents, cachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents, flag2);
				}
			}
			else
			{
				this.ShouldCatchUpWithFormation = false;
			}
			if (!formationFrame)
			{
				this.Agent.SetFormationFrameDisabled();
				return;
			}
			if (this.Agent.TrySetFormationFrame(in worldPosition, in vec))
			{
				this.Agent.UpdateDirectionChangeTendency();
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00019F96 File Offset: 0x00018196
		public override void OnRetreating()
		{
			base.OnRetreating();
			this.AdjustSpeedLimit(this.Agent, -1f, false);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00019FB0 File Offset: 0x000181B0
		public override void OnDismount(Agent mount)
		{
			base.OnDismount(mount);
			mount.SetMaximumSpeedLimit(-1f, false);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00019FC8 File Offset: 0x000181C8
		public void SetBehaviorValueSet(HumanAIComponent.BehaviorValueSet behaviorValueSet)
		{
			if (this._lastBehaviorValueSet != behaviorValueSet)
			{
				this._lastBehaviorValueSet = behaviorValueSet;
				switch (behaviorValueSet)
				{
				case HumanAIComponent.BehaviorValueSet.Default:
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.GoToPos, 3f, 7f, 5f, 20f, 6f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Melee, 8f, 7f, 4f, 20f, 1f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Ranged, 2f, 7f, 4f, 20f, 5f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.ChargeHorseback, 2f, 25f, 5f, 30f, 5f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.RangedHorseback, 2f, 15f, 6.5f, 30f, 5.5f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityMelee, 5f, 12f, 7.5f, 30f, 4f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityRanged, 5.5f, 12f, 8f, 30f, 4.5f);
					return;
				case HumanAIComponent.BehaviorValueSet.DefensiveArrangementMove:
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.GoToPos, 3f, 8f, 5f, 20f, 6f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Melee, 4f, 5f, 0f, 20f, 0f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Ranged, 0f, 7f, 0f, 20f, 0f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.ChargeHorseback, 0f, 7f, 0f, 30f, 0f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.RangedHorseback, 0f, 15f, 0f, 30f, 0f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityMelee, 5f, 12f, 7.5f, 30f, 9f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityRanged, 0.55f, 12f, 0.8f, 30f, 0.45f);
					return;
				case HumanAIComponent.BehaviorValueSet.Follow:
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.GoToPos, 3f, 7f, 5f, 20f, 6f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Melee, 6f, 7f, 4f, 20f, 0f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Ranged, 0f, 7f, 0f, 20f, 0f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.ChargeHorseback, 0f, 7f, 0f, 30f, 0f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.RangedHorseback, 0f, 15f, 0f, 30f, 0f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityMelee, 5f, 12f, 7.5f, 30f, 9f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityRanged, 0.55f, 12f, 0.8f, 30f, 0.45f);
					return;
				case HumanAIComponent.BehaviorValueSet.DefaultMove:
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.GoToPos, 3f, 7f, 5f, 20f, 6f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Melee, 8f, 7f, 5f, 20f, 0.01f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Ranged, 0.02f, 7f, 0.04f, 20f, 0.03f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.ChargeHorseback, 100f, 3f, 10f, 15f, 0.1f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.RangedHorseback, 0.02f, 15f, 0.065f, 30f, 0.055f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityMelee, 5f, 12f, 7.5f, 30f, 9f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityRanged, 0.55f, 12f, 0.8f, 30f, 0.45f);
					return;
				case HumanAIComponent.BehaviorValueSet.Charge:
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.GoToPos, 3f, 7f, 5f, 20f, 6f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Melee, 8f, 7f, 4f, 20f, 1f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Ranged, 2f, 7f, 4f, 20f, 5f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.ChargeHorseback, 2f, 25f, 5f, 30f, 5f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.RangedHorseback, 0f, 10f, 3f, 20f, 6f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityMelee, 5f, 12f, 7.5f, 30f, 9f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityRanged, 0.55f, 12f, 0.8f, 30f, 0.45f);
					return;
				case HumanAIComponent.BehaviorValueSet.DefaultDetached:
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.GoToPos, 3f, 7f, 5f, 20f, 6f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Melee, 8f, 7f, 4f, 20f, 1f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.Ranged, 0.02f, 7f, 0.04f, 20f, 0.03f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.ChargeHorseback, 2f, 25f, 5f, 30f, 5f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.RangedHorseback, 0.02f, 15f, 0.065f, 30f, 0.055f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityMelee, 3.5f, 7f, 5.5f, 20f, 6.5f);
					this.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.AttackEntityRanged, 0.25f, 7f, 0.45f, 20f, 0.55f);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0001A54C File Offset: 0x0001874C
		public void RefreshBehaviorValues(MovementOrder.MovementOrderEnum movementOrder, ArrangementOrder.ArrangementOrderEnum arrangementOrder)
		{
			if (this.Agent.IsDetachedFromFormation)
			{
				this.SetBehaviorValueSet(HumanAIComponent.BehaviorValueSet.DefaultDetached);
				return;
			}
			if (movementOrder == MovementOrder.MovementOrderEnum.Charge || movementOrder == MovementOrder.MovementOrderEnum.ChargeToTarget)
			{
				this.SetBehaviorValueSet(HumanAIComponent.BehaviorValueSet.Charge);
				return;
			}
			if (movementOrder == MovementOrder.MovementOrderEnum.Follow || arrangementOrder == ArrangementOrder.ArrangementOrderEnum.Column)
			{
				this.SetBehaviorValueSet(HumanAIComponent.BehaviorValueSet.Follow);
				return;
			}
			if (arrangementOrder != ArrangementOrder.ArrangementOrderEnum.ShieldWall && arrangementOrder != ArrangementOrder.ArrangementOrderEnum.Circle && arrangementOrder != ArrangementOrder.ArrangementOrderEnum.Square)
			{
				this.SetBehaviorValueSet(HumanAIComponent.BehaviorValueSet.DefaultMove);
				return;
			}
			this.SetBehaviorValueSet(HumanAIComponent.BehaviorValueSet.DefensiveArrangementMove);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0001A5A8 File Offset: 0x000187A8
		public void ForceDisablePickUpForAgent()
		{
			this._forceDisableItemPickup = true;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0001A5B4 File Offset: 0x000187B4
		public void SetScriptedPositionAndDirectionTimed(Vec2 position, float directionAsRotationInRadians, float duration)
		{
			this._scriptedFrameTimer = duration;
			WorldPosition worldPosition = this.Agent.GetWorldPosition();
			worldPosition.SetVec2(position);
			this.Agent.SetScriptedPositionAndDirection(ref worldPosition, directionAsRotationInRadians, false, Agent.AIScriptedFrameFlags.None);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0001A5EC File Offset: 0x000187EC
		public void DisableTimedScriptedMovement()
		{
			this._scriptedFrameTimer = -1f;
			this.Agent.DisableScriptedMovement();
		}

		// Token: 0x0400030A RID: 778
		private const float AvoidPickUpIfLookAgentIsCloseDistance = 20f;

		// Token: 0x0400030B RID: 779
		private const float AvoidPickUpIfLookAgentIsCloseDistanceSquared = 400f;

		// Token: 0x0400030C RID: 780
		private const float ClosestMountSearchRangeSq = 6400f;

		// Token: 0x0400030D RID: 781
		public static bool FormationSpeedAdjustmentEnabled = true;

		// Token: 0x0400030E RID: 782
		private readonly HumanAIComponent.BehaviorValues[] _behaviorValues;

		// Token: 0x0400030F RID: 783
		private bool _hasNewBehaviorValues;

		// Token: 0x04000310 RID: 784
		private readonly WeakGameEntity[] _tempPickableEntities = new WeakGameEntity[16];

		// Token: 0x04000311 RID: 785
		private readonly UIntPtr[] _pickableItemsId = new UIntPtr[16];

		// Token: 0x04000312 RID: 786
		private SpawnedItemEntity _itemToPickUp;

		// Token: 0x04000313 RID: 787
		private readonly MissionTimer _itemPickUpTickTimer;

		// Token: 0x04000314 RID: 788
		private bool _disablePickUpForAgent;

		// Token: 0x04000315 RID: 789
		private readonly MissionTimer _mountSearchTimer;

		// Token: 0x04000316 RID: 790
		private UsableMissionObject _objectOfInterest;

		// Token: 0x04000317 RID: 791
		private HumanAIComponent.UsableObjectInterestKind _objectInterestKind;

		// Token: 0x04000318 RID: 792
		private HumanAIComponent.BehaviorValueSet _lastBehaviorValueSet;

		// Token: 0x0400031A RID: 794
		private bool _shouldCatchUpWithFormation;

		// Token: 0x0400031B RID: 795
		private bool _forceDisableItemPickup;

		// Token: 0x0400031C RID: 796
		private float _scriptedFrameTimer = -1f;

		// Token: 0x02000434 RID: 1076
		[EngineStruct("behavior_values_struct", false, null)]
		public struct BehaviorValues
		{
			// Token: 0x060038EB RID: 14571 RVA: 0x000EA2E4 File Offset: 0x000E84E4
			public float GetValueAt(float x)
			{
				if (x <= this.x2)
				{
					return (this.y2 - this.y1) * x / this.x2 + this.y1;
				}
				if (x <= this.x3)
				{
					return (this.y3 - this.y2) * (x - this.x2) / (this.x3 - this.x2) + this.y2;
				}
				return this.y3;
			}

			// Token: 0x04001995 RID: 6549
			public float y1;

			// Token: 0x04001996 RID: 6550
			public float x2;

			// Token: 0x04001997 RID: 6551
			public float y2;

			// Token: 0x04001998 RID: 6552
			public float x3;

			// Token: 0x04001999 RID: 6553
			public float y3;
		}

		// Token: 0x02000435 RID: 1077
		public enum AISimpleBehaviorKind
		{
			// Token: 0x0400199B RID: 6555
			GoToPos,
			// Token: 0x0400199C RID: 6556
			Melee,
			// Token: 0x0400199D RID: 6557
			Ranged,
			// Token: 0x0400199E RID: 6558
			ChargeHorseback,
			// Token: 0x0400199F RID: 6559
			RangedHorseback,
			// Token: 0x040019A0 RID: 6560
			AttackEntityMelee,
			// Token: 0x040019A1 RID: 6561
			AttackEntityRanged,
			// Token: 0x040019A2 RID: 6562
			Count
		}

		// Token: 0x02000436 RID: 1078
		public enum BehaviorValueSet
		{
			// Token: 0x040019A4 RID: 6564
			Default,
			// Token: 0x040019A5 RID: 6565
			DefensiveArrangementMove,
			// Token: 0x040019A6 RID: 6566
			Follow,
			// Token: 0x040019A7 RID: 6567
			DefaultMove,
			// Token: 0x040019A8 RID: 6568
			Charge,
			// Token: 0x040019A9 RID: 6569
			DefaultDetached,
			// Token: 0x040019AA RID: 6570
			Overriden
		}

		// Token: 0x02000437 RID: 1079
		public enum UsableObjectInterestKind
		{
			// Token: 0x040019AC RID: 6572
			None,
			// Token: 0x040019AD RID: 6573
			MovingTo,
			// Token: 0x040019AE RID: 6574
			Defending,
			// Token: 0x040019AF RID: 6575
			Count
		}
	}
}
