using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000349 RID: 841
	public class Ballista : RangedSiegeWeapon, ISpawnable
	{
		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002F2B RID: 12075 RVA: 0x000B71C5 File Offset: 0x000B53C5
		// (set) Token: 0x06002F2C RID: 12076 RVA: 0x000B71CD File Offset: 0x000B53CD
		private protected SynchedMissionObject ballistaBody { protected get; private set; }

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002F2D RID: 12077 RVA: 0x000B71D6 File Offset: 0x000B53D6
		// (set) Token: 0x06002F2E RID: 12078 RVA: 0x000B71DE File Offset: 0x000B53DE
		private protected SynchedMissionObject ballistaNavel { protected get; private set; }

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002F2F RID: 12079 RVA: 0x000B71E7 File Offset: 0x000B53E7
		public override float DirectionRestriction
		{
			get
			{
				return this.HorizontalDirectionRestriction;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002F30 RID: 12080 RVA: 0x000B71EF File Offset: 0x000B53EF
		protected override float ShootingSpeed
		{
			get
			{
				return this.BallistaShootingSpeed;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002F31 RID: 12081 RVA: 0x000B71F7 File Offset: 0x000B53F7
		public override Vec3 CanShootAtPointCheckingOffset
		{
			get
			{
				return new Vec3(0f, 0f, 0.5f, -1f);
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002F32 RID: 12082 RVA: 0x000B7212 File Offset: 0x000B5412
		protected override bool WeaponMovesDownToReload
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002F33 RID: 12083 RVA: 0x000B7215 File Offset: 0x000B5415
		public override string MultipleProjectileId
		{
			get
			{
				return "ballista_c_projectile_grape";
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06002F34 RID: 12084 RVA: 0x000B721C File Offset: 0x000B541C
		public override string MultipleProjectileFlyingId
		{
			get
			{
				return "ballista_c_projectile_grape_projectile";
			}
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x000B7224 File Offset: 0x000B5424
		protected override void RegisterAnimationParameters()
		{
			this.SkeletonOwnerObjects = new SynchedMissionObject[1];
			this.Skeletons = new Skeleton[1];
			List<SynchedMissionObject> list = this.ballistaBody.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.SkeletonTag);
			if (list.Count == 0)
			{
				this.SkeletonOwnerObjects[0] = this.ballistaBody;
			}
			else
			{
				this.SkeletonOwnerObjects[0] = list[0];
			}
			this.Skeletons[0] = this.SkeletonOwnerObjects[0].GameEntity.Skeleton;
			base.SkeletonName = "ballista_skeleton";
			base.FireAnimation = "ballista_fire";
			base.FireAnimationIndex = MBAnimation.GetAnimationIndexWithName("ballista_fire");
			base.SetUpAnimation = "ballista_set_up";
			base.SetUpAnimationIndex = MBAnimation.GetAnimationIndexWithName("ballista_set_up");
			this._idleAnimationActionIndex = ActionIndexCache.Create(this.IdleActionName);
			this._reloadAnimationActionIndex = ActionIndexCache.Create(this.ReloadActionName);
			this._placeAmmoStartAnimationActionIndex = ActionIndexCache.Create(this.PlaceAmmoStartActionName);
			this._placeAmmoEndAnimationActionIndex = ActionIndexCache.Create(this.PlaceAmmoEndActionName);
			this._pickUpAmmoStartAnimationActionIndex = ActionIndexCache.Create(this.PickUpAmmoStartActionName);
			this._pickUpAmmoEndAnimationActionIndex = ActionIndexCache.Create(this.PickUpAmmoEndActionName);
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x000B734B File Offset: 0x000B554B
		public override SiegeEngineType GetSiegeEngineType()
		{
			return DefaultSiegeEngineTypes.Ballista;
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x000B7354 File Offset: 0x000B5554
		protected internal override void OnInit()
		{
			this.ballistaBody = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.BodyTag)[0];
			this.ballistaNavel = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.NavelTag)[0];
			this.RotationObject = this;
			base.OnInit();
			this.UsesMouseForAiming = true;
			this.GetSoundEventIndices();
			this._ballistaNavelInitialFrame = this.ballistaNavel.GameEntity.GetFrame();
			MatrixFrame globalFrame = this.ballistaBody.GameEntity.GetGlobalFrame();
			this._ballistaBodyInitialLocalFrame = this.ballistaBody.GameEntity.GetFrame();
			MatrixFrame globalFrame2 = base.PilotStandingPoint.GameEntity.GetGlobalFrame();
			this._pilotInitialLocalFrame = base.PilotStandingPoint.GameEntity.GetFrame();
			this._pilotInitialLocalIKFrame = globalFrame2.TransformToLocal(in globalFrame);
			this._missileInitialLocalFrame = base.Projectile.GameEntity.GetFrame();
			base.PilotStandingPoint.AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
			this.MissileStartingPositionEntityForSimulation = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.Projectile.GameEntity.Parent.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == "projectile_leaving_position"));
			this.EnemyRangeToStopUsing = 7f;
			this.AttackClickWillReload = true;
			this.WeaponNeedsClickToReload = true;
			base.SetScriptComponentToTick(this.GetTickRequirement());
			this.ApplyAimChange();
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x000B74D8 File Offset: 0x000B56D8
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

		// Token: 0x06002F39 RID: 12089 RVA: 0x000B7573 File Offset: 0x000B5773
		protected override bool CanRotate()
		{
			return base.State != RangedSiegeWeapon.WeaponState.Shooting;
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x000B7581 File Offset: 0x000B5781
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new BallistaAI(this);
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x000B758C File Offset: 0x000B578C
		protected override void OnRangedSiegeWeaponStateChange()
		{
			base.OnRangedSiegeWeaponStateChange();
			RangedSiegeWeapon.WeaponState state = base.State;
			if (state != RangedSiegeWeapon.WeaponState.Idle)
			{
				if (state == RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving)
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
			else if (base.AmmoCount > 0)
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this.ConsumeAmmo();
					return;
				}
				this.SetAmmo(base.AmmoCount - 1);
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002F3C RID: 12092 RVA: 0x000B75FC File Offset: 0x000B57FC
		protected override float MaximumBallisticError
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002F3D RID: 12093 RVA: 0x000B7603 File Offset: 0x000B5803
		protected override float HorizontalAimSensitivity
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06002F3E RID: 12094 RVA: 0x000B760A File Offset: 0x000B580A
		protected override float VerticalAimSensitivity
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x000B7611 File Offset: 0x000B5811
		protected override void HandleUserAiming(float dt)
		{
			if (base.PilotAgent == null)
			{
				this.TargetReleaseAngle = 0f;
			}
			base.HandleUserAiming(dt);
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x000B7630 File Offset: 0x000B5830
		protected override void ApplyAimChange()
		{
			MatrixFrame ballistaNavelInitialFrame = this._ballistaNavelInitialFrame;
			ballistaNavelInitialFrame.rotation.RotateAboutAnArbitraryVector(in this._ballistaNavelInitialFrame.rotation.u, this.CurrentDirection);
			this.ballistaNavel.GameEntity.SetLocalFrame(ref ballistaNavelInitialFrame, false);
			MatrixFrame matrixFrame = this._ballistaNavelInitialFrame.TransformToLocal(in this._pilotInitialLocalFrame);
			MatrixFrame matrixFrame2 = ballistaNavelInitialFrame.TransformToParent(in matrixFrame);
			base.PilotStandingPoint.GameEntity.SetLocalFrame(ref matrixFrame2, false);
			MatrixFrame ballistaBodyInitialLocalFrame = this._ballistaBodyInitialLocalFrame;
			ballistaBodyInitialLocalFrame.rotation.RotateAboutAnArbitraryVector(in ballistaBodyInitialLocalFrame.rotation.s, -this.CurrentReleaseAngle);
			this.ballistaBody.GameEntity.SetLocalFrame(ref ballistaBodyInitialLocalFrame, false);
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x000B76ED File Offset: 0x000B58ED
		protected override void ApplyCurrentDirectionToEntity()
		{
			this.ApplyAimChange();
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x000B76F5 File Offset: 0x000B58F5
		protected override void GetSoundEventIndices()
		{
			this.MoveSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/ballista/move");
			this.ReloadSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/ballista/reload");
			this.FireSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/ballista/fire");
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x000B7727 File Offset: 0x000B5927
		protected internal override bool IsTargetValid(ITargetable target)
		{
			return !(target is ICastleKeyPosition);
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x000B7738 File Offset: 0x000B5938
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x000B7766 File Offset: 0x000B5966
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._changeToState != RangedSiegeWeapon.WeaponState.Invalid)
			{
				base.State = this._changeToState;
				this._changeToState = RangedSiegeWeapon.WeaponState.Invalid;
			}
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x000B778C File Offset: 0x000B598C
		protected internal override void OnTickParallel(float dt)
		{
			base.OnTickParallel(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (base.PilotAgent != null)
			{
				Agent pilotAgent = base.PilotAgent;
				MatrixFrame globalFrame = this.ballistaBody.GameEntity.GetGlobalFrame();
				pilotAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._pilotInitialLocalIKFrame, in globalFrame, this.AnimationHeightDifference);
				ActionIndexCache currentAction = base.PilotAgent.GetCurrentAction(1);
				if (currentAction == this._pickUpAmmoEndAnimationActionIndex || currentAction == this._placeAmmoStartAnimationActionIndex)
				{
					MatrixFrame frame = base.PilotAgent.Frame;
					MatrixFrame matrixFrame = base.PilotAgent.GetBoneEntitialFrame(base.PilotAgent.Monster.MainHandItemBoneIndex, false);
					matrixFrame = frame.TransformToParent(in matrixFrame);
					base.Projectile.GameEntity.SetGlobalFrame(in matrixFrame, true);
				}
				else
				{
					base.Projectile.GameEntity.SetFrame(ref this._missileInitialLocalFrame, true);
				}
			}
			if (GameNetwork.IsClientOrReplay)
			{
				return;
			}
			switch (base.State)
			{
			case RangedSiegeWeapon.WeaponState.LoadingAmmo:
			{
				bool flag = false;
				if (base.PilotAgent != null)
				{
					if (!this.HasAmmo)
					{
						if (base.PilotAgent.Controller == AgentControllerType.AI)
						{
							base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							return;
						}
						break;
					}
					else
					{
						ActionIndexCache currentAction2 = base.PilotAgent.GetCurrentAction(1);
						this.FinalReloadSpeed = MissionGameModels.Current.MissionSiegeEngineCalculationModel.CalculateReloadSpeed(base.PilotAgent, this.BaseReloadSpeed);
						base.PilotAgent.SetCurrentActionSpeed(1, this.FinalReloadSpeed);
						if (currentAction2 != this._pickUpAmmoStartAnimationActionIndex && currentAction2 != this._pickUpAmmoEndAnimationActionIndex && currentAction2 != this._placeAmmoStartAnimationActionIndex && currentAction2 != this._placeAmmoEndAnimationActionIndex && !base.PilotAgent.SetActionChannel(1, in this._pickUpAmmoStartAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
						{
							base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
						else if (currentAction2 == this._pickUpAmmoEndAnimationActionIndex || currentAction2 == this._placeAmmoStartAnimationActionIndex)
						{
							flag = true;
						}
						else if (currentAction2 == this._placeAmmoEndAnimationActionIndex)
						{
							flag = true;
							this._changeToState = RangedSiegeWeapon.WeaponState.WaitingBeforeIdle;
						}
					}
				}
				base.Projectile.SetVisibleSynched(flag, false);
				return;
			}
			case RangedSiegeWeapon.WeaponState.WaitingBeforeIdle:
				if (base.PilotAgent == null)
				{
					this._changeToState = RangedSiegeWeapon.WeaponState.Idle;
					return;
				}
				if (base.PilotAgent.GetCurrentAction(1) != this._placeAmmoEndAnimationActionIndex)
				{
					if (base.PilotAgent.Controller != AgentControllerType.AI)
					{
						base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
					this._changeToState = RangedSiegeWeapon.WeaponState.Idle;
					return;
				}
				if (base.PilotAgent.GetCurrentActionProgress(1) > 0.9999f)
				{
					this._changeToState = RangedSiegeWeapon.WeaponState.Idle;
					if (base.PilotAgent != null && !base.PilotAgent.SetActionChannel(1, in this._idleAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
					{
						base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						return;
					}
				}
				break;
			case RangedSiegeWeapon.WeaponState.Reloading:
				this.FinalReloadSpeed = MissionGameModels.Current.MissionSiegeEngineCalculationModel.CalculateReloadSpeed(base.PilotAgent, this.BaseReloadSpeed);
				if (base.PilotAgent != null && !base.PilotAgent.SetActionChannel(1, in this._reloadAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
				{
					base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					return;
				}
				break;
			default:
				if (base.PilotAgent != null)
				{
					if (base.PilotAgent.IsInBeingStruckAction)
					{
						if (base.PilotAgent.GetCurrentAction(1) != ActionIndexCache.act_strike_bent_over)
						{
							base.PilotAgent.SetActionChannel(1, in ActionIndexCache.act_strike_bent_over, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
							return;
						}
					}
					else if (!base.PilotAgent.SetActionChannel(1, in this._idleAnimationActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true) && base.PilotAgent.Controller != AgentControllerType.AI)
					{
						base.PilotAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
				break;
			}
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x000B7C09 File Offset: 0x000B5E09
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = new TextObject("{=fEQAPJ2e}{KEY} Use", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000B7C38 File Offset: 0x000B5E38
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=abbALYlp}Ballista", null);
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000B7C48 File Offset: 0x000B5E48
		protected override void UpdateAmmoMesh()
		{
			int num = 8 - base.AmmoCount;
			base.GameEntity.SetVectorArgument(0f, (float)num, 0f, 0f);
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x000B7C80 File Offset: 0x000B5E80
		public override float ProcessTargetValue(float baseValue, TargetFlags flags)
		{
			if (flags.HasAnyFlag(TargetFlags.NotAThreat))
			{
				return -1000f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeEngine))
			{
				baseValue *= 0.2f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsStructure))
			{
				baseValue *= 0.05f;
			}
			if (flags.HasAnyFlag(TargetFlags.DebugThreat))
			{
				baseValue *= 10000f;
			}
			return baseValue;
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x000B7CD8 File Offset: 0x000B5ED8
		public override TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			targetFlags |= TargetFlags.IsFlammable;
			targetFlags |= TargetFlags.IsSiegeEngine;
			if (this.Side == BattleSideEnum.Attacker)
			{
				targetFlags |= TargetFlags.IsAttacker;
			}
			targetFlags |= TargetFlags.IsSmall;
			if (base.IsDestroyed || this.IsDeactivated)
			{
				targetFlags |= TargetFlags.NotAThreat;
			}
			if (this.Side == BattleSideEnum.Attacker && DebugSiegeBehavior.DebugDefendState == DebugSiegeBehavior.DebugStateDefender.DebugDefendersToBallistae)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			if (this.Side == BattleSideEnum.Defender && DebugSiegeBehavior.DebugAttackState == DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToBallistae)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			return targetFlags;
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x000B7D49 File Offset: 0x000B5F49
		public override float GetTargetValue(List<Vec3> weaponPos)
		{
			return 30f * base.GetUserMultiplierOfWeapon() * this.GetDistanceMultiplierOfWeapon(weaponPos[0]) * base.GetHitPointMultiplierOfWeapon();
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x000B7D6C File Offset: 0x000B5F6C
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x040012EF RID: 4847
		public string NavelTag = "BallistaNavel";

		// Token: 0x040012F0 RID: 4848
		public string BodyTag = "BallistaBody";

		// Token: 0x040012F1 RID: 4849
		public string SkeletonTag = "SkeletonEntity";

		// Token: 0x040012F2 RID: 4850
		public float AnimationHeightDifference;

		// Token: 0x040012F5 RID: 4853
		private MatrixFrame _ballistaBodyInitialLocalFrame;

		// Token: 0x040012F6 RID: 4854
		private MatrixFrame _ballistaNavelInitialFrame;

		// Token: 0x040012F7 RID: 4855
		private MatrixFrame _pilotInitialLocalFrame;

		// Token: 0x040012F8 RID: 4856
		private MatrixFrame _pilotInitialLocalIKFrame;

		// Token: 0x040012F9 RID: 4857
		private MatrixFrame _missileInitialLocalFrame;

		// Token: 0x040012FA RID: 4858
		[EditableScriptComponentVariable(true, "")]
		protected string IdleActionName = "act_usage_ballista_idle_attacker";

		// Token: 0x040012FB RID: 4859
		[EditableScriptComponentVariable(true, "")]
		protected string ReloadActionName = "act_usage_ballista_reload_attacker";

		// Token: 0x040012FC RID: 4860
		[EditableScriptComponentVariable(true, "")]
		protected string PlaceAmmoStartActionName = "act_usage_ballista_ammo_place_start_attacker";

		// Token: 0x040012FD RID: 4861
		[EditableScriptComponentVariable(true, "")]
		protected string PlaceAmmoEndActionName = "act_usage_ballista_ammo_place_end_attacker";

		// Token: 0x040012FE RID: 4862
		[EditableScriptComponentVariable(true, "")]
		protected string PickUpAmmoStartActionName = "act_usage_ballista_ammo_pick_up_start_attacker";

		// Token: 0x040012FF RID: 4863
		[EditableScriptComponentVariable(true, "")]
		protected string PickUpAmmoEndActionName = "act_usage_ballista_ammo_pick_up_end_attacker";

		// Token: 0x04001300 RID: 4864
		private ActionIndexCache _idleAnimationActionIndex;

		// Token: 0x04001301 RID: 4865
		private ActionIndexCache _reloadAnimationActionIndex;

		// Token: 0x04001302 RID: 4866
		private ActionIndexCache _placeAmmoStartAnimationActionIndex;

		// Token: 0x04001303 RID: 4867
		private ActionIndexCache _placeAmmoEndAnimationActionIndex;

		// Token: 0x04001304 RID: 4868
		private ActionIndexCache _pickUpAmmoStartAnimationActionIndex;

		// Token: 0x04001305 RID: 4869
		private ActionIndexCache _pickUpAmmoEndAnimationActionIndex;

		// Token: 0x04001306 RID: 4870
		[EditableScriptComponentVariable(true, "")]
		public float HorizontalDirectionRestriction = 1.5707964f;

		// Token: 0x04001307 RID: 4871
		public float BallistaShootingSpeed = 120f;

		// Token: 0x04001308 RID: 4872
		private RangedSiegeWeapon.WeaponState _changeToState = RangedSiegeWeapon.WeaponState.Invalid;
	}
}
