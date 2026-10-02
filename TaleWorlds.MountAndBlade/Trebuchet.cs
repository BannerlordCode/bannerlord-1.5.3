using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000368 RID: 872
	public class Trebuchet : RangedSiegeWeapon, ISpawnable
	{
		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600321C RID: 12828 RVA: 0x000CC4FE File Offset: 0x000CA6FE
		public override float DirectionRestriction
		{
			get
			{
				return 1.3962635f;
			}
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x000CC508 File Offset: 0x000CA708
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject;
			if (usableGameObject.GameEntity.HasTag(this.AmmoPickUpTag))
			{
				textObject = new TextObject("{=bNYm3K6b}{KEY} Pick Up", null);
			}
			else if (usableGameObject.GameEntity.HasTag("reload"))
			{
				textObject = new TextObject((base.PilotStandingPoint == usableGameObject) ? "{=fEQAPJ2e}{KEY} Use" : "{=Na81xuXn}{KEY} Rearm", null);
			}
			else if (usableGameObject.GameEntity.HasTag("rotate"))
			{
				textObject = new TextObject("{=5wx4BF5h}{KEY} Rotate", null);
			}
			else if (usableGameObject.GameEntity.HasTag("ammoload"))
			{
				textObject = new TextObject("{=ibC4xPoo}{KEY} Load Ammo", null);
			}
			else
			{
				textObject = TextObject.GetEmpty();
			}
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x000CC5DB File Offset: 0x000CA7DB
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			if (!gameEntity.HasTag(this.AmmoPickUpTag))
			{
				return new TextObject("{=4Skg9QhO}Trebuchet", null);
			}
			return new TextObject("{=pzfbPbWW}Boulder", null);
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x000CC604 File Offset: 0x000CA804
		protected override void RegisterAnimationParameters()
		{
			this.SkeletonOwnerObjects = new SynchedMissionObject[3];
			this.Skeletons = new Skeleton[3];
			this.SkeletonNames = new string[3];
			this.FireAnimations = new string[3];
			this.FireAnimationIndices = new int[3];
			this.SetUpAnimations = new string[3];
			this.SetUpAnimationIndices = new int[3];
			this.SkeletonOwnerObjects[0] = this._body;
			this.Skeletons[0] = this._body.GameEntity.Skeleton;
			this.SkeletonNames[0] = "trebuchet_a_skeleton";
			this.FireAnimations[0] = this.BodyFireAnimation;
			this.FireAnimationIndices[0] = MBAnimation.GetAnimationIndexWithName(this.BodyFireAnimation);
			this.SetUpAnimations[0] = this.BodySetUpAnimation;
			this.SetUpAnimationIndices[0] = MBAnimation.GetAnimationIndexWithName(this.BodySetUpAnimation);
			this.SkeletonOwnerObjects[1] = this._sling;
			this.Skeletons[1] = this._sling.GameEntity.Skeleton;
			this.SkeletonNames[1] = "trebuchet_a_sling_skeleton";
			this.FireAnimations[1] = this.SlingFireAnimation;
			this.FireAnimationIndices[1] = MBAnimation.GetAnimationIndexWithName(this.SlingFireAnimation);
			this.SetUpAnimations[1] = this.SlingSetUpAnimation;
			this.SetUpAnimationIndices[1] = MBAnimation.GetAnimationIndexWithName(this.SlingSetUpAnimation);
			this.SkeletonOwnerObjects[2] = this._rope;
			this.Skeletons[2] = this._rope.GameEntity.Skeleton;
			this.SkeletonNames[2] = "trebuchet_a_rope_skeleton";
			this.FireAnimations[2] = this.RopeFireAnimation;
			this.FireAnimationIndices[2] = MBAnimation.GetAnimationIndexWithName(this.RopeFireAnimation);
			this.SetUpAnimations[2] = this.RopeSetUpAnimation;
			this.SetUpAnimationIndices[2] = MBAnimation.GetAnimationIndexWithName(this.RopeSetUpAnimation);
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x000CC7CD File Offset: 0x000CA9CD
		public override SiegeEngineType GetSiegeEngineType()
		{
			return DefaultSiegeEngineTypes.Trebuchet;
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x000CC7D4 File Offset: 0x000CA9D4
		protected override void GetSoundEventIndices()
		{
			this.MoveSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/trebuchet/move");
			this.ReloadSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/trebuchet/reload");
			this.FireSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/trebuchet/fire");
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06003222 RID: 12834 RVA: 0x000CC806 File Offset: 0x000CAA06
		protected override float ShootingSpeed
		{
			get
			{
				return this.ProjectileSpeed;
			}
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000CC80E File Offset: 0x000CAA0E
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new TrebuchetAI(this);
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x000CC818 File Offset: 0x000CAA18
		protected internal override void OnInit()
		{
			List<SynchedMissionObject> list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("body");
			this._body = list[0];
			list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("sling");
			this._sling = list[0];
			list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("rope");
			this._rope = list[0];
			List<WeakGameEntity> list2 = base.GameEntity.CollectChildrenEntitiesWithTag("vertical_adjuster");
			this._verticalAdjuster = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(list2[0]);
			this._verticalAdjusterSkeleton = this._verticalAdjuster.Skeleton;
			this._verticalAdjusterSkeleton.SetAnimationAtChannel(this.VerticalAdjusterAnimation, 0, 1f, -1f, 0f);
			this._verticalAdjusterStartingLocalFrame = this._verticalAdjuster.GetFrame();
			this._verticalAdjusterStartingLocalFrame = this._body.GameEntity.GetBoneEntitialFrameWithIndex(0).TransformToLocal(in this._verticalAdjusterStartingLocalFrame);
			list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("rotate_entity");
			this.RotationObject = list[0];
			base.OnInit();
			this.TimeGapBetweenShootActionAndProjectileLeaving = 1.6f;
			this.TimeGapBetweenShootingEndAndReloadingStart = 0f;
			this._ammoLoadPoints = new List<StandingPointWithWeaponRequirement>();
			if (base.StandingPoints != null)
			{
				for (int i = 0; i < base.StandingPoints.Count; i++)
				{
					if (base.StandingPoints[i].GameEntity.HasTag("ammoload"))
					{
						this._ammoLoadPoints.Add(base.StandingPoints[i] as StandingPointWithWeaponRequirement);
					}
					else if (base.StandingPoints[i] != base.PilotStandingPoint && !base.StandingPoints[i].GameEntity.HasTag(this.AmmoPickUpTag) && !GameNetwork.IsClientOrReplay)
					{
						base.StandingPoints[i].SetIsDisabledForPlayersSynched(true);
					}
				}
				MatrixFrame globalFrame = this._body.GameEntity.GetGlobalFrame();
				this._standingPointLocalIKFrames = new MatrixFrame[base.StandingPoints.Count];
				for (int j = 0; j < base.StandingPoints.Count; j++)
				{
					this._standingPointLocalIKFrames[j] = base.StandingPoints[j].GameEntity.GetGlobalFrame().TransformToLocal(in globalFrame);
					base.StandingPoints[j].AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
				}
			}
			this.ApplyAimChange();
			if (!GameNetwork.IsClientOrReplay)
			{
				this.SetActivationLoadAmmoPoint(false);
				this.EnemyRangeToStopUsing = 11f;
				this.MachinePositionOffsetToStopUsingLocal = new Vec2(0f, 2.8f);
				this._sling.SetAnimationAtChannelSynched((base.State == RangedSiegeWeapon.WeaponState.Idle) ? this.IdleWithAmmoAnimation : this.IdleEmptyAnimation, 0, 1f);
			}
			this._missileBoneIndex = Skeleton.GetBoneIndexFromName(this._sling.GameEntity.Skeleton.GetName(), "bn_projectile_holder");
			this._shootAnimPlayed = false;
			this.UpdateAmmoMesh();
			base.SetScriptComponentToTick(this.GetTickRequirement());
			this.UpdateProjectilePosition();
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x000CCB44 File Offset: 0x000CAD44
		public override void AfterMissionStart()
		{
			if (base.AmmoPickUpPoints != null)
			{
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					standingPoint.LockUserFrames = true;
				}
			}
			if (this._ammoLoadPoints != null)
			{
				foreach (StandingPointWithWeaponRequirement standingPointWithWeaponRequirement in this._ammoLoadPoints)
				{
					standingPointWithWeaponRequirement.LockUserFrames = true;
				}
			}
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x000CCBE8 File Offset: 0x000CADE8
		protected override void OnRangedSiegeWeaponStateChange()
		{
			base.OnRangedSiegeWeaponStateChange();
			if (base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeIdle)
			{
				this.UpdateProjectilePosition();
			}
			if (GameNetwork.IsClientOrReplay)
			{
				return;
			}
			RangedSiegeWeapon.WeaponState state = base.State;
			if (state <= RangedSiegeWeapon.WeaponState.Shooting)
			{
				if (state == RangedSiegeWeapon.WeaponState.Idle)
				{
					base.Projectile.SetVisibleSynched(true, false);
					return;
				}
				if (state != RangedSiegeWeapon.WeaponState.Shooting)
				{
					return;
				}
				base.Projectile.SetVisibleSynched(false, false);
				return;
			}
			else
			{
				if (state == RangedSiegeWeapon.WeaponState.LoadingAmmo)
				{
					this._sling.SetAnimationAtChannelSynched(this.IdleEmptyAnimation, 0, 1f);
					return;
				}
				if (state != RangedSiegeWeapon.WeaponState.Reloading)
				{
					return;
				}
				this._shootAnimPlayed = false;
				return;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06003227 RID: 12839 RVA: 0x000CCC69 File Offset: 0x000CAE69
		protected override float HorizontalAimSensitivity
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06003228 RID: 12840 RVA: 0x000CCC70 File Offset: 0x000CAE70
		protected override float VerticalAimSensitivity
		{
			get
			{
				return 0.075f;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06003229 RID: 12841 RVA: 0x000CCC78 File Offset: 0x000CAE78
		protected override Vec3 ShootingDirection
		{
			get
			{
				Mat3 rotation = this.RotationObject.GameEntity.GetGlobalFrame().rotation;
				rotation.RotateAboutSide(-this.CurrentReleaseAngle);
				Vec3 vec = new Vec3(0f, -1f, 0f, -1f);
				return rotation.TransformToParent(in vec);
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x0600322A RID: 12842 RVA: 0x000CCCCF File Offset: 0x000CAECF
		// (set) Token: 0x0600322B RID: 12843 RVA: 0x000CCCFB File Offset: 0x000CAEFB
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

		// Token: 0x0600322C RID: 12844 RVA: 0x000CCD04 File Offset: 0x000CAF04
		public override float ProcessTargetValue(float baseValue, TargetFlags flags)
		{
			if (flags.HasAnyFlag(TargetFlags.NotAThreat))
			{
				return -1000f;
			}
			if (flags.HasAllFlags(TargetFlags.IsSiegeEngine | TargetFlags.IsAttacker))
			{
				baseValue *= 1.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeEngine))
			{
				baseValue *= 2.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsStructure))
			{
				baseValue *= 0.1f;
			}
			if (flags.HasAnyFlag(TargetFlags.DebugThreat))
			{
				baseValue *= 10000f;
			}
			return baseValue;
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x000CCD70 File Offset: 0x000CAF70
		public override TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			targetFlags |= TargetFlags.IsFlammable;
			targetFlags |= TargetFlags.IsSiegeEngine;
			targetFlags |= TargetFlags.IsAttacker;
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

		// Token: 0x0600322E RID: 12846 RVA: 0x000CCDD3 File Offset: 0x000CAFD3
		public override float GetTargetValue(List<Vec3> weaponPos)
		{
			return 40f * base.GetUserMultiplierOfWeapon() * this.GetDistanceMultiplierOfWeapon(weaponPos[0]) * base.GetHitPointMultiplierOfWeapon();
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x000CCDF6 File Offset: 0x000CAFF6
		protected override bool CanRotate()
		{
			return base.State == RangedSiegeWeapon.WeaponState.Idle || base.State == RangedSiegeWeapon.WeaponState.LoadingAmmo || base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeIdle;
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x000CCE14 File Offset: 0x000CB014
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x000CCE44 File Offset: 0x000CB044
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
								MissionWeapon missionWeapon = new MissionWeapon(this.OriginalMissileItem, null, null);
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
					bool flag = false;
					foreach (StandingPointWithWeaponRequirement standingPointWithWeaponRequirement in this._ammoLoadPoints)
					{
						if (flag)
						{
							if (standingPointWithWeaponRequirement.IsDeactivated)
							{
								if ((standingPointWithWeaponRequirement.HasUser || standingPointWithWeaponRequirement.HasAIMovingTo) && (standingPointWithWeaponRequirement.UserAgent == this.ReloaderAgent || standingPointWithWeaponRequirement.MovingAgent == this.ReloaderAgent))
								{
									base.SendReloaderAgentToOriginalPoint();
								}
								standingPointWithWeaponRequirement.SetIsDeactivatedSynched(true);
							}
						}
						else if (standingPointWithWeaponRequirement.HasUser)
						{
							flag = true;
							Agent userAgent2 = standingPointWithWeaponRequirement.UserAgent;
							ActionIndexCache currentAction2 = userAgent2.GetCurrentAction(1);
							if (currentAction2 == ActionIndexCache.act_usage_trebuchet_load_ammo && userAgent2.GetCurrentActionProgress(1) > 0.56f)
							{
								EquipmentIndex primaryWieldedItemIndex = userAgent2.GetPrimaryWieldedItemIndex();
								if (primaryWieldedItemIndex != EquipmentIndex.None && userAgent2.Equipment[primaryWieldedItemIndex].CurrentUsageItem.WeaponClass == this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
								{
									base.ChangeProjectileEntityServer(userAgent2, userAgent2.Equipment[primaryWieldedItemIndex].Item.StringId);
									userAgent2.RemoveEquippedWeapon(primaryWieldedItemIndex);
									this._timeElapsedAfterLoading = 0f;
									base.Projectile.SetVisibleSynched(true, false);
									this._sling.SetAnimationAtChannelSynched(this.IdleWithAmmoAnimation, 0, 1f);
									base.State = RangedSiegeWeapon.WeaponState.WaitingBeforeIdle;
								}
								else
								{
									userAgent2.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
									if (!userAgent2.IsPlayerControlled)
									{
										base.SendAgentToAmmoPickup(userAgent2);
									}
								}
							}
							else if (currentAction2 != ActionIndexCache.act_usage_trebuchet_load_ammo && !userAgent2.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_load_ammo, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
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
								}
							}
						}
						else if (standingPointWithWeaponRequirement.HasAIMovingTo)
						{
							Agent movingAgent = standingPointWithWeaponRequirement.MovingAgent;
							EquipmentIndex primaryWieldedItemIndex2 = movingAgent.GetPrimaryWieldedItemIndex();
							if (primaryWieldedItemIndex2 == EquipmentIndex.None || movingAgent.Equipment[primaryWieldedItemIndex2].CurrentUsageItem.WeaponClass != this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
							{
								movingAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
								base.SendAgentToAmmoPickup(movingAgent);
							}
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

		// Token: 0x06003232 RID: 12850 RVA: 0x000CD33C File Offset: 0x000CB53C
		protected internal override void OnTickParallel(float dt)
		{
			base.OnTickParallel(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving)
			{
				this.UpdateProjectilePosition();
			}
			float num = MBMath.ClampFloat((this.CurrentReleaseAngle - this.BottomReleaseAngleRestriction) / (this.TopReleaseAngleRestriction - this.BottomReleaseAngleRestriction), 0f, 1f);
			this._verticalAdjusterSkeleton.SetAnimationParameterAtChannel(0, num);
			MatrixFrame matrixFrame = this._body.GameEntity.GetBoneEntitialFrameWithIndex(0).TransformToParent(in this._verticalAdjusterStartingLocalFrame);
			this._verticalAdjuster.SetFrame(ref matrixFrame, true);
			MatrixFrame globalFrame = this._body.GameEntity.GetGlobalFrame();
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				if (base.StandingPoints[i].HasUser)
				{
					if (base.StandingPoints[i].UserAgent.IsInBeingStruckAction)
					{
						base.StandingPoints[i].UserAgent.ClearHandInverseKinematics();
					}
					else if (base.StandingPoints[i] != base.PilotStandingPoint)
					{
						if (base.StandingPoints[i].UserAgent.GetCurrentAction(1) == ActionIndexCache.act_usage_trebuchet_reload_2)
						{
							base.StandingPoints[i].UserAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._standingPointLocalIKFrames[i], in globalFrame, 0f);
						}
						else
						{
							base.StandingPoints[i].UserAgent.ClearHandInverseKinematics();
						}
					}
					else
					{
						base.StandingPoints[i].UserAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._standingPointLocalIKFrames[i], in globalFrame, 0f);
					}
				}
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				if (base.PilotAgent != null)
				{
					ActionIndexCache currentAction = base.PilotAgent.GetCurrentAction(1);
					if (base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving || base.State == RangedSiegeWeapon.WeaponState.Shooting || base.State == RangedSiegeWeapon.WeaponState.WaitingBeforeReloading)
					{
						if (!this._shootAnimPlayed && currentAction != ActionIndexCache.act_usage_trebuchet_shoot)
						{
							this._shootAnimPlayed = base.PilotAgent.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_shoot, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						}
						else if (currentAction != ActionIndexCache.act_usage_trebuchet_shoot && !base.PilotAgent.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_reload_idle, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
						{
							base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
					else if (currentAction != ActionIndexCache.act_usage_trebuchet_reload && currentAction != ActionIndexCache.act_usage_trebuchet_shoot && !base.PilotAgent.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_idle, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
					{
						base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
				if (base.State != RangedSiegeWeapon.WeaponState.Reloading)
				{
					foreach (StandingPoint standingPoint in this.ReloadStandingPoints)
					{
						if (standingPoint.HasUser && standingPoint != base.PilotStandingPoint)
						{
							Agent userAgent = standingPoint.UserAgent;
							if (!userAgent.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_reload_2_idle, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && userAgent.Controller != AgentControllerType.AI)
							{
								userAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							}
						}
					}
				}
				foreach (StandingPoint standingPoint2 in base.StandingPoints)
				{
					if (standingPoint2.HasUser && this.ReloadStandingPoints.IndexOf(standingPoint2) < 0 && (!(standingPoint2 is StandingPointWithWeaponRequirement) || (this._ammoLoadPoints.IndexOf((StandingPointWithWeaponRequirement)standingPoint2) < 0 && base.AmmoPickUpPoints.IndexOf(standingPoint2) < 0)))
					{
						Agent userAgent2 = standingPoint2.UserAgent;
						if (!userAgent2.SetActionChannel(1, in ActionIndexCache.act_usage_trebuchet_reload_2_idle, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && userAgent2.Controller != AgentControllerType.AI)
						{
							userAgent2.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
				}
			}
			RangedSiegeWeapon.WeaponState state = base.State;
			if (state == RangedSiegeWeapon.WeaponState.Reloading)
			{
				for (int j = 0; j < this.ReloadStandingPoints.Count; j++)
				{
					if (this.ReloadStandingPoints[j].HasUser)
					{
						Agent userAgent3 = this.ReloadStandingPoints[j].UserAgent;
						ActionIndexCache currentAction2 = userAgent3.GetCurrentAction(1);
						if (currentAction2 == ActionIndexCache.act_usage_trebuchet_reload || currentAction2 == ActionIndexCache.act_usage_trebuchet_reload_2)
						{
							userAgent3.SetCurrentActionProgress(1, this.Skeletons[0].GetAnimationParameterAtChannel(0));
						}
						else if (!GameNetwork.IsClientOrReplay)
						{
							ActionIndexCache actionIndexCache = ActionIndexCache.act_usage_trebuchet_reload;
							if (this.ReloadStandingPoints[j].GameEntity.HasTag("right"))
							{
								actionIndexCache = ActionIndexCache.act_usage_trebuchet_reload_2;
							}
							if (!userAgent3.SetActionChannel(1, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, this.Skeletons[0].GetAnimationParameterAtChannel(0), false, -0.2f, 0, true) && userAgent3.Controller != AgentControllerType.AI)
							{
								userAgent3.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x06003233 RID: 12851 RVA: 0x000CD920 File Offset: 0x000CBB20
		protected override void SetActivationLoadAmmoPoint(bool activate)
		{
			foreach (StandingPointWithWeaponRequirement standingPointWithWeaponRequirement in this._ammoLoadPoints)
			{
				standingPointWithWeaponRequirement.SetIsDeactivatedSynched(!activate);
			}
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x000CD974 File Offset: 0x000CBB74
		protected override void UpdateProjectilePosition()
		{
			MatrixFrame boneEntitialFrameWithIndex = this._sling.GameEntity.GetBoneEntitialFrameWithIndex(this._missileBoneIndex);
			base.Projectile.GameEntity.SetFrame(ref boneEntitialFrameWithIndex, true);
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x000CD9B1 File Offset: 0x000CBBB1
		protected internal override bool IsStandingPointNotUsedOnAccountOfBeingAmmoLoad(StandingPoint standingPoint)
		{
			return (this._ammoLoadPoints.Contains(standingPoint) && this.LoadAmmoStandingPoint != standingPoint) || base.IsStandingPointNotUsedOnAccountOfBeingAmmoLoad(standingPoint);
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x000CD9D3 File Offset: 0x000CBBD3
		protected override float GetDetachmentWeightAux(BattleSideEnum side)
		{
			return base.GetDetachmentWeightAuxForExternalAmmoWeapons(side);
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x000CD9DC File Offset: 0x000CBBDC
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x04001522 RID: 5410
		public const float TrebuchetDirectionRestriction = 1.3962635f;

		// Token: 0x04001523 RID: 5411
		private const string BodyTag = "body";

		// Token: 0x04001524 RID: 5412
		private const string SlingTag = "sling";

		// Token: 0x04001525 RID: 5413
		private const string RopeTag = "rope";

		// Token: 0x04001526 RID: 5414
		private const string RotateTag = "rotate";

		// Token: 0x04001527 RID: 5415
		private const string VerticalAdjusterTag = "vertical_adjuster";

		// Token: 0x04001528 RID: 5416
		private const string MissileBoneName = "bn_projectile_holder";

		// Token: 0x04001529 RID: 5417
		private const string RotateObjectTag = "rotate_entity";

		// Token: 0x0400152A RID: 5418
		public float ProjectileSpeed = 45f;

		// Token: 0x0400152B RID: 5419
		private SynchedMissionObject _body;

		// Token: 0x0400152C RID: 5420
		private SynchedMissionObject _sling;

		// Token: 0x0400152D RID: 5421
		private SynchedMissionObject _rope;

		// Token: 0x0400152E RID: 5422
		public string IdleWithAmmoAnimation;

		// Token: 0x0400152F RID: 5423
		public string IdleEmptyAnimation;

		// Token: 0x04001530 RID: 5424
		public string BodyFireAnimation;

		// Token: 0x04001531 RID: 5425
		public string BodySetUpAnimation;

		// Token: 0x04001532 RID: 5426
		public string SlingFireAnimation;

		// Token: 0x04001533 RID: 5427
		public string SlingSetUpAnimation;

		// Token: 0x04001534 RID: 5428
		public string RopeFireAnimation;

		// Token: 0x04001535 RID: 5429
		public string RopeSetUpAnimation;

		// Token: 0x04001536 RID: 5430
		public string VerticalAdjusterAnimation;

		// Token: 0x04001537 RID: 5431
		private GameEntity _verticalAdjuster;

		// Token: 0x04001538 RID: 5432
		private Skeleton _verticalAdjusterSkeleton;

		// Token: 0x04001539 RID: 5433
		private MatrixFrame _verticalAdjusterStartingLocalFrame;

		// Token: 0x0400153A RID: 5434
		private float _timeElapsedAfterLoading;

		// Token: 0x0400153B RID: 5435
		private bool _shootAnimPlayed;

		// Token: 0x0400153C RID: 5436
		private MatrixFrame[] _standingPointLocalIKFrames;

		// Token: 0x0400153D RID: 5437
		private List<StandingPointWithWeaponRequirement> _ammoLoadPoints;

		// Token: 0x0400153E RID: 5438
		private sbyte _missileBoneIndex;
	}
}
