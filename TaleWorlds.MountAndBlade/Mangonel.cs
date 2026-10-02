using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000354 RID: 852
	public class Mangonel : RangedSiegeWeapon, ISpawnable
	{
		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06003046 RID: 12358 RVA: 0x000BE61D File Offset: 0x000BC81D
		protected override float MaximumBallisticError
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06003047 RID: 12359 RVA: 0x000BE624 File Offset: 0x000BC824
		protected override float ShootingSpeed
		{
			get
			{
				return this.ProjectileSpeed;
			}
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x000BE62C File Offset: 0x000BC82C
		protected override void RegisterAnimationParameters()
		{
			this.SkeletonOwnerObjects = new SynchedMissionObject[2];
			this.Skeletons = new Skeleton[2];
			this.SkeletonNames = new string[1];
			this.FireAnimations = new string[2];
			this.FireAnimationIndices = new int[2];
			this.SetUpAnimations = new string[2];
			this.SetUpAnimationIndices = new int[2];
			this.SkeletonOwnerObjects[0] = this._body;
			this.Skeletons[0] = this._body.GameEntity.Skeleton;
			this.SkeletonNames[0] = this.MangonelBodySkeleton;
			this.FireAnimations[0] = this.MangonelBodyFire;
			this.FireAnimationIndices[0] = MBAnimation.GetAnimationIndexWithName(this.MangonelBodyFire);
			this.SetUpAnimations[0] = this.MangonelBodyReload;
			this.SetUpAnimationIndices[0] = MBAnimation.GetAnimationIndexWithName(this.MangonelBodyReload);
			this.SkeletonOwnerObjects[1] = this._rope;
			this.Skeletons[1] = this._rope.GameEntity.Skeleton;
			this.FireAnimations[1] = this.MangonelRopeFire;
			this.FireAnimationIndices[1] = MBAnimation.GetAnimationIndexWithName(this.MangonelRopeFire);
			this.SetUpAnimations[1] = this.MangonelRopeReload;
			this.SetUpAnimationIndices[1] = MBAnimation.GetAnimationIndexWithName(this.MangonelRopeReload);
			this._missileBoneName = this.ProjectileBoneName;
			this._idleAnimationActionIndex = ActionIndexCache.Create(this.IdleActionName);
			this._shootAnimationActionIndex = ActionIndexCache.Create(this.ShootActionName);
			this._reload1AnimationActionIndex = ActionIndexCache.Create(this.Reload1ActionName);
			this._reload2AnimationActionIndex = ActionIndexCache.Create(this.Reload2ActionName);
			this._rotateLeftAnimationActionIndex = ActionIndexCache.Create(this.RotateLeftActionName);
			this._rotateRightAnimationActionIndex = ActionIndexCache.Create(this.RotateRightActionName);
			this._loadAmmoBeginAnimationActionIndex = ActionIndexCache.Create(this.LoadAmmoBeginActionName);
			this._loadAmmoEndAnimationActionIndex = ActionIndexCache.Create(this.LoadAmmoEndActionName);
			this._reload2IdleActionIndex = ActionIndexCache.Create(this.Reload2IdleActionName);
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x000BE816 File Offset: 0x000BCA16
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new MangonelAI(this);
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x000BE81E File Offset: 0x000BCA1E
		public override SiegeEngineType GetSiegeEngineType()
		{
			if (this.DefaultSide != BattleSideEnum.Attacker)
			{
				return DefaultSiegeEngineTypes.Catapult;
			}
			return DefaultSiegeEngineTypes.Onager;
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x000BE834 File Offset: 0x000BCA34
		protected internal override void OnInit()
		{
			List<SynchedMissionObject> list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("rope");
			if (list.Count > 0)
			{
				this._rope = list[0];
			}
			list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("body");
			this._body = list[0];
			this._bodySkeleton = this._body.GameEntity.Skeleton;
			this.RotationObject = this._body;
			List<WeakGameEntity> list2 = base.GameEntity.CollectChildrenEntitiesWithTag("vertical_adjuster");
			this._verticalAdjuster = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(list2[0]);
			this._verticalAdjusterSkeleton = this._verticalAdjuster.Skeleton;
			if (this._verticalAdjusterSkeleton != null)
			{
				this._verticalAdjusterSkeleton.SetAnimationAtChannel(this.MangonelAimAnimation, 0, 1f, -1f, 0f);
			}
			this._verticalAdjusterStartingLocalFrame = this._verticalAdjuster.GetFrame();
			this._verticalAdjusterStartingLocalFrame = this._body.GameEntity.GetBoneEntitialFrameWithIndex(0).TransformToLocal(in this._verticalAdjusterStartingLocalFrame);
			base.OnInit();
			this.TimeGapBetweenShootActionAndProjectileLeaving = 0.23f;
			this.TimeGapBetweenShootingEndAndReloadingStart = 0f;
			this._rotateStandingPoints = new List<StandingPoint>();
			if (base.StandingPoints != null)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					if (standingPoint.GameEntity.HasTag("rotate"))
					{
						if (standingPoint.GameEntity.HasTag("left") && this._rotateStandingPoints.Count > 0)
						{
							this._rotateStandingPoints.Insert(0, standingPoint);
						}
						else
						{
							this._rotateStandingPoints.Add(standingPoint);
						}
					}
				}
				MatrixFrame globalFrame = this._body.GameEntity.GetGlobalFrame();
				this._standingPointLocalIKFrames = new MatrixFrame[base.StandingPoints.Count];
				for (int i = 0; i < base.StandingPoints.Count; i++)
				{
					this._standingPointLocalIKFrames[i] = base.StandingPoints[i].GameEntity.GetGlobalFrame().TransformToLocalNonOrthogonal(in globalFrame);
					base.StandingPoints[i].AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
				}
			}
			this._missileBoneIndex = Skeleton.GetBoneIndexFromName(this.Skeletons[0].GetName(), this._missileBoneName);
			this.ApplyAimChange();
			foreach (StandingPoint standingPoint2 in this.ReloadStandingPoints)
			{
				if (standingPoint2 != base.PilotStandingPoint)
				{
					this._reloadWithoutPilot = standingPoint2;
				}
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				this.SetActivationLoadAmmoPoint(false);
			}
			this.EnemyRangeToStopUsing = 9f;
			base.SetScriptComponentToTick(this.GetTickRequirement());
			if (base.AmmoPickUpPoints != null)
			{
				foreach (StandingPoint standingPoint3 in base.AmmoPickUpPoints)
				{
					standingPoint3.LockUserFrames = true;
				}
			}
			this._isInitialProjectilePositionUpdated = false;
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x000BEB8C File Offset: 0x000BCD8C
		protected internal override void OnEditorInit()
		{
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x000BEB90 File Offset: 0x000BCD90
		public override void OnPilotAssignedDuringSpawn()
		{
			base.PilotAgent.SetActionChannel(1, in this._idleAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			MatrixFrame globalFrame = base.PilotStandingPoint.GameEntity.GetGlobalFrame();
			base.PilotAgent.TeleportToPosition(globalFrame.origin);
			base.PilotAgent.DisableScriptedMovement();
			Agent pilotAgent = base.PilotAgent;
			Vec2 vec = globalFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			pilotAgent.SetMovementDirection(in vec);
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000BEC2B File Offset: 0x000BCE2B
		protected override bool CanRotate()
		{
			return base.State == RangedSiegeWeapon.WeaponState.Idle || base.State == RangedSiegeWeapon.WeaponState.LoadingAmmo || base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeIdle;
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x000BEC4C File Offset: 0x000BCE4C
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x000BEC7C File Offset: 0x000BCE7C
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					if (standingPoint.HasUser)
					{
						Agent userAgent = standingPoint.UserAgent;
						ActionIndexCache currentAction = userAgent.GetCurrentAction(1);
						if (!(currentAction == ActionIndexCache.act_pickup_boulder_begin))
						{
							if (currentAction == ActionIndexCache.act_pickup_boulder_end)
							{
								MissionWeapon missionWeapon = new MissionWeapon(this.OriginalMissileItem, null, null, 1);
								userAgent.EquipWeaponToExtraSlotAndWield(ref missionWeapon);
								userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
								this.ConsumeAmmo();
								if (userAgent.IsAIControlled)
								{
									if (!this.LoadAmmoStandingPoint.HasUser && !this.LoadAmmoStandingPoint.IsDeactivated)
									{
										userAgent.AIMoveToGameObjectEnable(this.LoadAmmoStandingPoint, this, base.Ai.GetScriptedFrameFlags(userAgent));
									}
									else if (this.ReloaderAgentOriginalPoint != null && !this.ReloaderAgentOriginalPoint.HasUser && !this.ReloaderAgentOriginalPoint.HasAIMovingTo)
									{
										userAgent.AIMoveToGameObjectEnable(this.ReloaderAgentOriginalPoint, this, base.Ai.GetScriptedFrameFlags(userAgent));
									}
									else
									{
										Agent reloaderAgent = this.ReloaderAgent;
										if (reloaderAgent != null)
										{
											Formation formation = reloaderAgent.Formation;
											if (formation != null)
											{
												formation.AttachUnit(this.ReloaderAgent);
											}
										}
										this.ReloaderAgent = null;
									}
								}
							}
							else if (!userAgent.SetActionChannel(1, in ActionIndexCache.act_pickup_boulder_begin, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && userAgent.Controller != AgentControllerType.AI)
							{
								userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							}
						}
					}
				}
			}
			switch (base.State)
			{
			case RangedSiegeWeapon.WeaponState.LoadingAmmo:
				if (!GameNetwork.IsClientOrReplay)
				{
					if (this.LoadAmmoStandingPoint.HasUser)
					{
						Agent userAgent2 = this.LoadAmmoStandingPoint.UserAgent;
						if (userAgent2.GetCurrentAction(1) == this._loadAmmoEndAnimationActionIndex)
						{
							EquipmentIndex primaryWieldedItemIndex = userAgent2.GetPrimaryWieldedItemIndex();
							if (primaryWieldedItemIndex != EquipmentIndex.None && userAgent2.Equipment[primaryWieldedItemIndex].CurrentUsageItem.WeaponClass == this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
							{
								base.ChangeProjectileEntityServer(userAgent2, userAgent2.Equipment[primaryWieldedItemIndex].Item.StringId);
								userAgent2.RemoveEquippedWeapon(primaryWieldedItemIndex);
								this._timeElapsedAfterLoading = 0f;
								base.Projectile.SetVisibleSynched(true, false);
								base.State = RangedSiegeWeapon.WeaponState.WaitingBeforeIdle;
								return;
							}
							userAgent2.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
							if (!userAgent2.IsPlayerControlled)
							{
								base.SendAgentToAmmoPickup(userAgent2);
								return;
							}
						}
						else if (userAgent2.GetCurrentAction(1) != this._loadAmmoBeginAnimationActionIndex && !userAgent2.SetActionChannel(1, in this._loadAmmoBeginAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
						{
							for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
							{
								if (!userAgent2.Equipment[equipmentIndex].IsEmpty && userAgent2.Equipment[equipmentIndex].CurrentUsageItem.WeaponClass == this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
								{
									userAgent2.RemoveEquippedWeapon(equipmentIndex);
								}
							}
							userAgent2.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
							if (!userAgent2.IsPlayerControlled)
							{
								base.SendAgentToAmmoPickup(userAgent2);
								return;
							}
						}
					}
					else if (this.LoadAmmoStandingPoint.HasAIMovingTo)
					{
						Agent movingAgent = this.LoadAmmoStandingPoint.MovingAgent;
						EquipmentIndex primaryWieldedItemIndex2 = movingAgent.GetPrimaryWieldedItemIndex();
						if (primaryWieldedItemIndex2 == EquipmentIndex.None || movingAgent.Equipment[primaryWieldedItemIndex2].CurrentUsageItem.WeaponClass != this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
						{
							movingAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
							base.SendAgentToAmmoPickup(movingAgent);
						}
					}
				}
				break;
			case RangedSiegeWeapon.WeaponState.WaitingBeforeIdle:
				this._timeElapsedAfterLoading += dt;
				if (this._timeElapsedAfterLoading > 1f)
				{
					base.State = RangedSiegeWeapon.WeaponState.Idle;
					return;
				}
				break;
			case RangedSiegeWeapon.WeaponState.Reloading:
			case RangedSiegeWeapon.WeaponState.ReloadingPaused:
				break;
			default:
				return;
			}
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x000BF0AC File Offset: 0x000BD2AC
		protected internal override void OnTickParallel(float dt)
		{
			base.OnTickParallel(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (!this._isInitialProjectilePositionUpdated)
			{
				this._isInitialProjectilePositionUpdated = true;
				this.UpdateProjectilePosition();
			}
			if (base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving)
			{
				this.UpdateProjectilePosition();
			}
			if (this._verticalAdjusterSkeleton != null)
			{
				float num = MBMath.ClampFloat((this.CurrentReleaseAngle - this.BottomReleaseAngleRestriction) / (this.TopReleaseAngleRestriction - this.BottomReleaseAngleRestriction), 0f, 1f);
				this._verticalAdjusterSkeleton.SetAnimationParameterAtChannel(0, num);
			}
			MatrixFrame matrixFrame = this.Skeletons[0].GetBoneEntitialFrameWithIndex(0).TransformToParent(in this._verticalAdjusterStartingLocalFrame);
			this._verticalAdjuster.SetFrame(ref matrixFrame, true);
			MatrixFrame globalFrame = this._body.GameEntity.GetGlobalFrame();
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				if (base.StandingPoints[i].HasUser)
				{
					if (base.StandingPoints[i].UserAgent.IsInBeingStruckAction || base.AmmoPickUpPoints.IndexOf(base.StandingPoints[i]) >= 0)
					{
						base.StandingPoints[i].UserAgent.ClearHandInverseKinematics();
					}
					else
					{
						ActionIndexCache currentAction = base.StandingPoints[i].UserAgent.GetCurrentAction(1);
						float currentActionProgress = base.StandingPoints[i].UserAgent.GetCurrentActionProgress(1);
						if (currentAction != this._reload2IdleActionIndex && (currentAction != this._reload2AnimationActionIndex || currentActionProgress > 0.1f) && (currentAction != this._shootAnimationActionIndex || currentActionProgress < 0.15f))
						{
							base.StandingPoints[i].UserAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._standingPointLocalIKFrames[i], in globalFrame, 0f);
						}
						else
						{
							base.StandingPoints[i].UserAgent.ClearHandInverseKinematics();
						}
					}
				}
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				for (int j = 0; j < this._rotateStandingPoints.Count; j++)
				{
					StandingPoint standingPoint = this._rotateStandingPoints[j];
					if (standingPoint.HasUser)
					{
						Agent userAgent = standingPoint.UserAgent;
						int num2 = 1;
						ActionIndexCache actionIndexCache = ((j == 0) ? this._rotateLeftAnimationActionIndex : this._rotateRightAnimationActionIndex);
						if (!userAgent.SetActionChannel(num2, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && standingPoint.UserAgent.Controller != AgentControllerType.AI)
						{
							standingPoint.UserAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
				}
				if (base.PilotAgent != null)
				{
					ActionIndexCache currentAction2 = base.PilotAgent.GetCurrentAction(1);
					if (base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving)
					{
						if (base.PilotAgent.IsInBeingStruckAction)
						{
							if (currentAction2 != ActionIndexCache.act_none && currentAction2 != ActionIndexCache.act_strike_bent_over)
							{
								base.PilotAgent.SetActionChannel(1, in ActionIndexCache.act_strike_bent_over, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
							}
						}
						else if (!base.PilotAgent.SetActionChannel(1, in this._shootAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
						{
							base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
					else if (!base.PilotAgent.SetActionChannel(1, in this._idleAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && currentAction2 != this._reload1AnimationActionIndex && currentAction2 != this._shootAnimationActionIndex && base.PilotAgent.Controller != AgentControllerType.AI)
					{
						base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
				if (this._reloadWithoutPilot.HasUser)
				{
					Agent userAgent2 = this._reloadWithoutPilot.UserAgent;
					if (!userAgent2.SetActionChannel(1, in this._reload2IdleActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && userAgent2.GetCurrentAction(1) != this._reload2AnimationActionIndex && userAgent2.Controller != AgentControllerType.AI)
					{
						userAgent2.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
			}
			RangedSiegeWeapon.WeaponState state = base.State;
			if (state == RangedSiegeWeapon.WeaponState.Reloading)
			{
				foreach (StandingPoint standingPoint2 in this.ReloadStandingPoints)
				{
					if (standingPoint2.HasUser)
					{
						ActionIndexCache currentAction3 = standingPoint2.UserAgent.GetCurrentAction(1);
						if (currentAction3 == this._reload1AnimationActionIndex || currentAction3 == this._reload2AnimationActionIndex)
						{
							standingPoint2.UserAgent.SetCurrentActionProgress(1, this._bodySkeleton.GetAnimationParameterAtChannel(0));
						}
						else if (!GameNetwork.IsClientOrReplay)
						{
							ActionIndexCache actionIndexCache2 = ((standingPoint2 == base.PilotStandingPoint) ? this._reload1AnimationActionIndex : this._reload2AnimationActionIndex);
							if (!standingPoint2.UserAgent.SetActionChannel(1, in actionIndexCache2, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, this._bodySkeleton.GetAnimationParameterAtChannel(0), false, -0.2f, 0, true) && standingPoint2.UserAgent.Controller != AgentControllerType.AI)
							{
								standingPoint2.UserAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x000BF658 File Offset: 0x000BD858
		protected override void SetActivationLoadAmmoPoint(bool activate)
		{
			this.LoadAmmoStandingPoint.SetIsDeactivatedSynched(!activate);
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000BF66C File Offset: 0x000BD86C
		protected override void UpdateProjectilePosition()
		{
			MatrixFrame boneEntitialFrameWithIndex = this.Skeletons[0].GetBoneEntitialFrameWithIndex(this._missileBoneIndex);
			base.Projectile.GameEntity.SetFrame(ref boneEntitialFrameWithIndex, true);
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x000BF6A4 File Offset: 0x000BD8A4
		protected override void OnRangedSiegeWeaponStateChange()
		{
			base.OnRangedSiegeWeaponStateChange();
			RangedSiegeWeapon.WeaponState state = base.State;
			if (state != RangedSiegeWeapon.WeaponState.Idle)
			{
				if (state != RangedSiegeWeapon.WeaponState.Shooting)
				{
					if (state == RangedSiegeWeapon.WeaponState.WaitingBeforeIdle)
					{
						this.UpdateProjectilePosition();
						return;
					}
				}
				else
				{
					if (!GameNetwork.IsClientOrReplay)
					{
						base.Projectile.SetVisibleSynched(false, false);
						return;
					}
					base.Projectile.GameEntity.SetVisibilityExcludeParents(false);
					return;
				}
			}
			else
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					base.Projectile.SetVisibleSynched(true, false);
					return;
				}
				base.Projectile.GameEntity.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x000BF723 File Offset: 0x000BD923
		protected override void GetSoundEventIndices()
		{
			this.MoveSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/mangonel/move");
			this.ReloadSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/mangonel/reload");
			this.FireSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/mangonel/fire");
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06003056 RID: 12374 RVA: 0x000BF758 File Offset: 0x000BD958
		protected override float HorizontalAimSensitivity
		{
			get
			{
				if (this.DefaultSide == BattleSideEnum.Defender)
				{
					return 0.25f;
				}
				float num = 0.05f;
				foreach (StandingPoint standingPoint in this._rotateStandingPoints)
				{
					if (standingPoint.HasUser && !standingPoint.UserAgent.IsInBeingStruckAction)
					{
						num += 0.1f;
					}
				}
				return num;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06003057 RID: 12375 RVA: 0x000BF7D8 File Offset: 0x000BD9D8
		protected override float VerticalAimSensitivity
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06003058 RID: 12376 RVA: 0x000BF7E0 File Offset: 0x000BD9E0
		protected override Vec3 ShootingDirection
		{
			get
			{
				Mat3 rotation = this._body.GameEntity.GetGlobalFrame().rotation;
				rotation.RotateAboutSide(-this.CurrentReleaseAngle);
				Vec3 vec = new Vec3(0f, -1f, 0f, -1f);
				return rotation.TransformToParent(in vec);
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003059 RID: 12377 RVA: 0x000BF837 File Offset: 0x000BDA37
		// (set) Token: 0x0600305A RID: 12378 RVA: 0x000BF863 File Offset: 0x000BDA63
		protected override bool HasAmmo
		{
			get
			{
				return base.HasAmmo || base.CurrentlyUsedAmmoPickUpPoint != null || this.LoadAmmoStandingPoint.HasUser || this.LoadAmmoStandingPoint.HasAIMovingTo;
			}
			set
			{
				base.HasAmmo = value;
			}
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x000BF86C File Offset: 0x000BDA6C
		protected override void ApplyAimChange()
		{
			base.ApplyAimChange();
			this.ShootingDirection.Normalize();
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x000BF88E File Offset: 0x000BDA8E
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			if (!gameEntity.HasTag(this.AmmoPickUpTag))
			{
				return new TextObject("{=NbpcDXtJ}Mangonel", null);
			}
			return new TextObject("{=pzfbPbWW}Boulder", null);
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x000BF8B8 File Offset: 0x000BDAB8
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject;
			if (usableGameObject.GameEntity.HasTag("reload"))
			{
				textObject = new TextObject((base.PilotStandingPoint == usableGameObject) ? "{=fEQAPJ2e}{KEY} Use" : "{=Na81xuXn}{KEY} Rearm", null);
			}
			else if (usableGameObject.GameEntity.HasTag("rotate"))
			{
				textObject = new TextObject("{=5wx4BF5h}{KEY} Rotate", null);
			}
			else if (usableGameObject.GameEntity.HasTag(this.AmmoPickUpTag))
			{
				textObject = new TextObject("{=bNYm3K6b}{KEY} Pick Up", null);
			}
			else if (usableGameObject.GameEntity.HasTag("ammoload"))
			{
				textObject = new TextObject("{=ibC4xPoo}{KEY} Load Ammo", null);
			}
			else
			{
				textObject = new TextObject("{=fEQAPJ2e}{KEY} Use", null);
			}
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x000BF994 File Offset: 0x000BDB94
		public override TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			targetFlags |= TargetFlags.IsFlammable;
			targetFlags |= TargetFlags.IsSiegeEngine;
			if (this.Side == BattleSideEnum.Attacker)
			{
				targetFlags |= TargetFlags.IsAttacker;
			}
			if (base.IsDestroyed || this.IsDeactivated)
			{
				targetFlags |= TargetFlags.NotAThreat;
			}
			if (this.Side == BattleSideEnum.Attacker && DebugSiegeBehavior.DebugDefendState == DebugSiegeBehavior.DebugStateDefender.DebugDefendersToMangonels)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			if (this.Side == BattleSideEnum.Defender && DebugSiegeBehavior.DebugAttackState == DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToMangonels)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			return targetFlags;
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x000BFA00 File Offset: 0x000BDC00
		public override float GetTargetValue(List<Vec3> weaponPos)
		{
			return 40f * base.GetUserMultiplierOfWeapon() * this.GetDistanceMultiplierOfWeapon(weaponPos[0]) * base.GetHitPointMultiplierOfWeapon();
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000BFA24 File Offset: 0x000BDC24
		public override float ProcessTargetValue(float baseValue, TargetFlags flags)
		{
			if (flags.HasAnyFlag(TargetFlags.NotAThreat))
			{
				return -1000f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeEngine))
			{
				baseValue *= 10000f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsStructure))
			{
				baseValue *= 2.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSmall))
			{
				baseValue *= 8f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsMoving))
			{
				baseValue *= 8f;
			}
			if (flags.HasAnyFlag(TargetFlags.DebugThreat))
			{
				baseValue *= 10000f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeTower))
			{
				baseValue *= 8f;
			}
			return baseValue;
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000BFAB7 File Offset: 0x000BDCB7
		protected override float GetDetachmentWeightAux(BattleSideEnum side)
		{
			return base.GetDetachmentWeightAuxForExternalAmmoWeapons(side);
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x000BFAC0 File Offset: 0x000BDCC0
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x040013BC RID: 5052
		private const string BodyTag = "body";

		// Token: 0x040013BD RID: 5053
		private const string RopeTag = "rope";

		// Token: 0x040013BE RID: 5054
		private const string RotateTag = "rotate";

		// Token: 0x040013BF RID: 5055
		private const string LeftTag = "left";

		// Token: 0x040013C0 RID: 5056
		private const string VerticalAdjusterTag = "vertical_adjuster";

		// Token: 0x040013C1 RID: 5057
		private string _missileBoneName = "end_throwarm";

		// Token: 0x040013C2 RID: 5058
		private List<StandingPoint> _rotateStandingPoints;

		// Token: 0x040013C3 RID: 5059
		private SynchedMissionObject _body;

		// Token: 0x040013C4 RID: 5060
		private SynchedMissionObject _rope;

		// Token: 0x040013C5 RID: 5061
		private GameEntity _verticalAdjuster;

		// Token: 0x040013C6 RID: 5062
		private MatrixFrame _verticalAdjusterStartingLocalFrame;

		// Token: 0x040013C7 RID: 5063
		private Skeleton _verticalAdjusterSkeleton;

		// Token: 0x040013C8 RID: 5064
		private Skeleton _bodySkeleton;

		// Token: 0x040013C9 RID: 5065
		private float _timeElapsedAfterLoading;

		// Token: 0x040013CA RID: 5066
		private bool _isInitialProjectilePositionUpdated;

		// Token: 0x040013CB RID: 5067
		private MatrixFrame[] _standingPointLocalIKFrames;

		// Token: 0x040013CC RID: 5068
		private StandingPoint _reloadWithoutPilot;

		// Token: 0x040013CD RID: 5069
		public string MangonelBodySkeleton = "mangonel_skeleton";

		// Token: 0x040013CE RID: 5070
		public string MangonelBodyFire = "mangonel_fire";

		// Token: 0x040013CF RID: 5071
		public string MangonelBodyReload = "mangonel_set_up";

		// Token: 0x040013D0 RID: 5072
		public string MangonelRopeFire = "mangonel_holder_fire";

		// Token: 0x040013D1 RID: 5073
		public string MangonelRopeReload = "mangonel_holder_set_up";

		// Token: 0x040013D2 RID: 5074
		public string MangonelAimAnimation = "mangonel_a_anglearm_state";

		// Token: 0x040013D3 RID: 5075
		public string ProjectileBoneName = "end_throwarm";

		// Token: 0x040013D4 RID: 5076
		public string IdleActionName;

		// Token: 0x040013D5 RID: 5077
		public string ShootActionName;

		// Token: 0x040013D6 RID: 5078
		public string Reload1ActionName;

		// Token: 0x040013D7 RID: 5079
		public string Reload2ActionName;

		// Token: 0x040013D8 RID: 5080
		public string RotateLeftActionName;

		// Token: 0x040013D9 RID: 5081
		public string RotateRightActionName;

		// Token: 0x040013DA RID: 5082
		public string LoadAmmoBeginActionName;

		// Token: 0x040013DB RID: 5083
		public string LoadAmmoEndActionName;

		// Token: 0x040013DC RID: 5084
		public string Reload2IdleActionName;

		// Token: 0x040013DD RID: 5085
		public float ProjectileSpeed = 40f;

		// Token: 0x040013DE RID: 5086
		private ActionIndexCache _idleAnimationActionIndex;

		// Token: 0x040013DF RID: 5087
		private ActionIndexCache _shootAnimationActionIndex;

		// Token: 0x040013E0 RID: 5088
		private ActionIndexCache _reload1AnimationActionIndex;

		// Token: 0x040013E1 RID: 5089
		private ActionIndexCache _reload2AnimationActionIndex;

		// Token: 0x040013E2 RID: 5090
		private ActionIndexCache _rotateLeftAnimationActionIndex;

		// Token: 0x040013E3 RID: 5091
		private ActionIndexCache _rotateRightAnimationActionIndex;

		// Token: 0x040013E4 RID: 5092
		private ActionIndexCache _loadAmmoBeginAnimationActionIndex;

		// Token: 0x040013E5 RID: 5093
		private ActionIndexCache _loadAmmoEndAnimationActionIndex;

		// Token: 0x040013E6 RID: 5094
		private ActionIndexCache _reload2IdleActionIndex;

		// Token: 0x040013E7 RID: 5095
		private sbyte _missileBoneIndex;
	}
}
