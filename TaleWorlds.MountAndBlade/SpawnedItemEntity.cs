using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000383 RID: 899
	public class SpawnedItemEntity : UsableMissionObject
	{
		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06003336 RID: 13110 RVA: 0x000D16DA File Offset: 0x000CF8DA
		public MissionWeapon WeaponCopy
		{
			get
			{
				return this._weapon;
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06003337 RID: 13111 RVA: 0x000D16E2 File Offset: 0x000CF8E2
		// (set) Token: 0x06003338 RID: 13112 RVA: 0x000D16EA File Offset: 0x000CF8EA
		public bool HasLifeTime
		{
			get
			{
				return this._hasLifeTime;
			}
			set
			{
				if (this._hasLifeTime != value)
				{
					this._hasLifeTime = value;
					base.SetScriptComponentToTickMT(this.GetTickRequirement());
				}
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06003339 RID: 13113 RVA: 0x000D1708 File Offset: 0x000CF908
		// (set) Token: 0x0600333A RID: 13114 RVA: 0x000D1710 File Offset: 0x000CF910
		private bool PhysicsStopped
		{
			get
			{
				return this._physicsStopped;
			}
			set
			{
				if (this._physicsStopped != value)
				{
					this._physicsStopped = value;
					base.SetScriptComponentToTickMT(this.GetTickRequirement());
				}
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x0600333B RID: 13115 RVA: 0x000D172E File Offset: 0x000CF92E
		public bool IsRemoved
		{
			get
			{
				return this._ownerGameEntity == null;
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x0600333D RID: 13117 RVA: 0x000D1745 File Offset: 0x000CF945
		// (set) Token: 0x0600333C RID: 13116 RVA: 0x000D173C File Offset: 0x000CF93C
		public bool SpawnedOnACorpse { get; private set; }

		// Token: 0x0600333E RID: 13118 RVA: 0x000D174D File Offset: 0x000CF94D
		public TextObject GetActionMessage(ItemObject weaponToReplaceWith, bool fillUp)
		{
			if (weaponToReplaceWith != null)
			{
				MBTextManager.SetTextVariable("ITEM_NAME", weaponToReplaceWith.Name, false);
				return GameTexts.FindText("str_ui_swap", null);
			}
			if (!fillUp)
			{
				return GameTexts.FindText("str_ui_equip", null);
			}
			return GameTexts.FindText("str_ui_fill", null);
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x000D178C File Offset: 0x000CF98C
		public TextObject GetDescriptionMessage(bool fillUp)
		{
			if (!fillUp)
			{
				return this._weapon.GetModifiedItemName();
			}
			return GameTexts.FindText("str_inventory_weapon", this._weapon.CurrentUsageItem.WeaponClass.ToString());
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06003340 RID: 13120 RVA: 0x000D17D0 File Offset: 0x000CF9D0
		public override bool LockUserFrames
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x000D17D3 File Offset: 0x000CF9D3
		// (set) Token: 0x06003342 RID: 13122 RVA: 0x000D17DB File Offset: 0x000CF9DB
		public Mission.WeaponSpawnFlags SpawnFlags { get; private set; }

		// Token: 0x06003343 RID: 13123 RVA: 0x000D17E4 File Offset: 0x000CF9E4
		public void Initialize(MissionWeapon weapon, bool hasLifeTime, Mission.WeaponSpawnFlags spawnFlags, in Vec3 fakeSimulationVelocity, bool spawnedOnACorpse = false)
		{
			this._weapon = weapon;
			this.HasLifeTime = hasLifeTime;
			this.SpawnFlags = spawnFlags;
			this._fakeSimulationVelocity = fakeSimulationVelocity;
			this.SpawnedOnACorpse = spawnedOnACorpse;
			if (this.SpawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithPhysics))
			{
				this._disablePhysicsTimer = new Timer(0f, 10f, true);
			}
			if (Mission.Current.IsLoadingFinished)
			{
				this.InitializeTimers();
			}
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x000D1851 File Offset: 0x000CFA51
		public override void AfterMissionStart()
		{
			this.InitializeTimers();
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x000D185C File Offset: 0x000CFA5C
		private void InitializeTimers()
		{
			if (this.HasLifeTime)
			{
				float num = 0f;
				if (!this._weapon.IsEmpty)
				{
					num = (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.QuickFadeOut) ? 5f : 180f);
					base.IsDeactivated = this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.CannotBePickedUp);
					if (this._weapon.CurrentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.RangedWeapon | WeaponFlags.NotUsableWithOneHand | WeaponFlags.Consumable))
					{
						this._lastSoundPlayTime = 0.333f;
					}
					else
					{
						this._lastSoundPlayTime = -0.333f;
					}
				}
				else
				{
					base.IsDeactivated = true;
				}
				this._deletionTimer = new Timer(Mission.Current.CurrentTime, num, true);
			}
			else
			{
				this._deletionTimer = new Timer(Mission.Current.CurrentTime, float.MaxValue, true);
			}
			if (this._disablePhysicsTimer != null)
			{
				this._disablePhysicsTimer.Reset(Mission.Current.CurrentTime, 10f);
			}
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x000D1968 File Offset: 0x000CFB68
		protected internal override void OnInit()
		{
			base.OnInit();
			this._ownerGameEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity);
			if (!string.IsNullOrEmpty(this.WeaponName))
			{
				ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(this.WeaponName);
				this._weapon = new MissionWeapon(@object, null, null);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x000D19CC File Offset: 0x000CFBCC
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (GameNetwork.IsClientOrReplay || base.HasUser || !this.PhysicsStopped)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel2;
			}
			if (this.HasLifeTime)
			{
				ScriptComponentBehavior.TickRequirement tickRequirement = base.GetTickRequirement();
				if (tickRequirement.HasAnyFlag(ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel2))
				{
					tickRequirement |= ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel2;
				}
				else
				{
					tickRequirement |= ScriptComponentBehavior.TickRequirement.TickOccasionally;
				}
				return tickRequirement;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x000D1A28 File Offset: 0x000CFC28
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._disableDynamicPhysicsNextFrame)
			{
				this.DisableDynamicBody();
				this._disableDynamicPhysicsNextFrame = false;
				return;
			}
			if (GameNetwork.IsClientOrReplay && this._clientSyncData != null)
			{
				if (this._clientSyncData.Timer.Check(Mission.Current.CurrentTime))
				{
					this._ownerGameEntity.SetAlpha(1f);
					this._clientSyncData = null;
					return;
				}
				float duration = this._clientSyncData.Timer.Duration;
				float num = MBMath.ClampFloat(this._clientSyncData.Timer.ElapsedTime() / duration, 0f, 1f);
				if (num < (1f - 0.1f / duration) * 0.5f)
				{
					this._ownerGameEntity.SetAlpha(1f - num * 2f);
					return;
				}
				if (num < (1f + 0.1f / duration) * 0.5f)
				{
					this._ownerGameEntity.SetAlpha(0f);
					this._ownerGameEntity.SetGlobalFrame(in this._clientSyncData.Frame, true);
					GameEntity parent = this._clientSyncData.Parent;
					if (parent != null)
					{
						parent.AddChild(this._ownerGameEntity, true);
					}
					this._clientSyncData.Timer.Reset(Mission.Current.CurrentTime - duration * (1f + 0.1f / duration) * 0.5f);
					return;
				}
				this._ownerGameEntity.SetAlpha(num * 2f - 1f);
			}
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x000D1BA0 File Offset: 0x000CFDA0
		protected internal override void OnTickParallel2(float dt)
		{
			base.OnTickParallel2(dt);
			if (!GameNetwork.IsClientOrReplay)
			{
				if (base.HasUser)
				{
					ActionIndexCache currentAction = base.UserAgent.GetCurrentAction(this._usedChannelIndex);
					if (currentAction == this._successActionIndex)
					{
						base.UserAgent.StopUsingGameObjectMT(base.UserAgent.CanUseObject(this) && !base.UserAgent.IsInWater(), Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
					else if (currentAction != this._progressActionIndex)
					{
						base.UserAgent.StopUsingGameObjectMT(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
				else if (this.HasLifeTime && this._deletionTimer.Check(Mission.Current.CurrentTime))
				{
					this._readyToBeDeleted = true;
				}
				if (!this.PhysicsStopped)
				{
					if (this._ownerGameEntity != null)
					{
						if (this._weapon.IsBanner())
						{
							MatrixFrame globalFrame = this._ownerGameEntity.GetGlobalFrame();
							this._fakeSimulationVelocity.z = this._fakeSimulationVelocity.z - dt * 9.8f;
							globalFrame.origin += this._fakeSimulationVelocity * dt;
							this._ownerGameEntity.SetGlobalFrame(in globalFrame, true);
							if (this._ownerGameEntity.Scene.GetGroundHeightAtPosition(globalFrame.origin, BodyFlags.CommonCollisionExcludeFlags) > globalFrame.origin.z + 0.3f)
							{
								this.PhysicsStopped = true;
								return;
							}
						}
						else
						{
							Vec3 globalPosition = this._ownerGameEntity.GlobalPosition;
							if (globalPosition.z <= CompressionBasic.PositionCompressionInfo.GetMinimumValue() + 5f)
							{
								this._readyToBeDeleted = true;
							}
							if (!this._ownerGameEntity.BodyFlag.HasAnyFlag(BodyFlags.Dynamic))
							{
								this.PhysicsStopped = true;
								return;
							}
							MatrixFrame globalFrame2 = this._ownerGameEntity.GetGlobalFrame();
							if (!globalFrame2.rotation.IsUnit())
							{
								globalFrame2.rotation.Orthonormalize();
								this._ownerGameEntity.SetGlobalFrame(in globalFrame2, true);
							}
							bool flag = this._disablePhysicsTimer.Check(Mission.Current.CurrentTime);
							if ((flag || this._disablePhysicsTimer.ElapsedTime() > 1f) && (flag || this._ownerGameEntity.IsDynamicBodyStationaryMT()))
							{
								this._groundEntityWhenDisabled = this.TryFindProperGroundEntityForSpawnedEntity();
								this._disableDynamicPhysicsNextFrame = true;
							}
							if (!this.PhysicsStopped && this._disablePhysicsTimer.ElapsedTime() > 0.2f)
							{
								Vec3 vec;
								Vec3 vec2;
								this._ownerGameEntity.GetPhysicsMinMax(true, out vec, out vec2, true);
								MatrixFrame globalFrame3 = this._ownerGameEntity.GetGlobalFrame();
								MatrixFrame previousGlobalFrame = this._ownerGameEntity.GetPreviousGlobalFrame();
								Vec3 vec3 = globalFrame3.TransformToParent(in vec);
								Vec3 vec4 = previousGlobalFrame.TransformToParent(in vec);
								Vec3 vec5 = globalFrame3.TransformToParent(in vec2);
								Vec3 vec6 = previousGlobalFrame.TransformToParent(in vec2);
								Vec3 vec7 = Vec3.Vec3Min(vec3, vec5);
								Vec3 vec8 = Vec3.Vec3Min(vec4, vec6);
								Vec3 vec9 = Vec3.Vec3Max(vec3, vec5);
								float waterLevelAtPositionMT = Mission.Current.GetWaterLevelAtPositionMT(vec7.AsVec2, !GameNetwork.IsMultiplayer);
								bool flag2 = vec7.z < waterLevelAtPositionMT;
								bool flag3 = vec8.z < waterLevelAtPositionMT;
								if (flag2)
								{
									this._disablePhysicsTimer.AdjustStartTime(dt * 0.8f);
									float num = waterLevelAtPositionMT - 3.5f;
									if (vec9.z < num)
									{
										this._readyToBeDeleted = true;
									}
									if (!flag3)
									{
										BodyFlags bodyFlags;
										base.GameEntity.Scene.GetGroundHeightAndBodyFlagsAtPosition(globalFrame3.origin, out bodyFlags, BodyFlags.CommonCollisionExcludeFlagsForCombat);
										if (!bodyFlags.HasAnyFlag(BodyFlags.Moveable))
										{
											Vec3 linearVelocityMT = this._ownerGameEntity.GetLinearVelocityMT();
											float num2 = this._ownerGameEntity.Mass * linearVelocityMT.Length;
											if (!this._alreadyMadeWaterDropSound && num2 > 0f)
											{
												num2 *= 0.0625f;
												num2 = MathF.Min(num2, 1f);
												Vec3 vec10 = globalPosition;
												vec10.z = waterLevelAtPositionMT;
												SoundEventParameter soundEventParameter = new SoundEventParameter("Size", num2);
												Mission.Current.MakeSound(ItemPhysicsSoundContainer.SoundCodePhysicsWater, vec10, false, true, -1, -1, ref soundEventParameter);
												this._alreadyMadeWaterDropSound = true;
											}
										}
									}
								}
								if (flag2 != flag3)
								{
									float num3 = (flag2 ? 100f : 1f);
									PhysicsMaterial physicsMaterial = base.GameEntity.GetPhysicsMaterial();
									float num4 = physicsMaterial.GetLinearDamping() * num3;
									float num5 = physicsMaterial.GetAngularDamping() * num3;
									if (num4 > 15f)
									{
										num4 = 15f;
									}
									if (num5 > 15f)
									{
										num5 = 15f;
									}
									base.GameEntity.SetDampingMT(num4, num5);
									return;
								}
							}
						}
					}
					else
					{
						this.PhysicsStopped = true;
					}
				}
			}
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x000D2010 File Offset: 0x000D0210
		private void DisableDynamicBody()
		{
			using (new TWSharedMutexWriteLock(Scene.PhysicsAndRayCastLock))
			{
				if (this._groundEntityWhenDisabled != null)
				{
					this._groundEntityWhenDisabled.AddChild(TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity), true);
				}
				if (!this._weapon.IsEmpty && !this._ownerGameEntity.BodyFlag.HasAnyFlag(BodyFlags.Disabled))
				{
					this._ownerGameEntity.SetPhysicsMoveToBatched(true);
					this._ownerGameEntity.ConvertDynamicBodyToRayCast();
				}
				else
				{
					this._ownerGameEntity.RemovePhysics(false);
				}
				this.ClampEntityPositionForStoppingIfNeeded();
				this.PhysicsStopped = true;
				if ((!base.IsDeactivated || this._groundEntityWhenDisabled != null) && !this._weapon.IsEmpty && GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					MissionObjectId id = base.Id;
					GameEntity groundEntityWhenDisabled = this._groundEntityWhenDisabled;
					GameNetwork.WriteMessage(new StopPhysicsAndSetFrameOfMissionObject(id, (groundEntityWhenDisabled != null) ? groundEntityWhenDisabled.GetFirstScriptOfType<MissionObject>().Id : MissionObjectId.Invalid, this._ownerGameEntity.GetLocalFrame()));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x000D2130 File Offset: 0x000D0330
		private GameEntity TryFindProperGroundEntityForSpawnedEntity()
		{
			Vec3 vec;
			Vec3 vec2;
			this._ownerGameEntity.GetPhysicsMinMax(true, out vec, out vec2, false);
			float num = vec2.z - vec.z;
			vec.z = vec2.z - 0.001f;
			Vec3 vec3 = (vec2 + vec) * 0.5f;
			float num2;
			Vec3 vec4;
			WeakGameEntity weakGameEntity;
			this._ownerGameEntity.Scene.RayCastForClosestEntityOrTerrain(vec3, vec3 - new Vec3(0f, 0f, num + 0.5f, -1f), out num2, out vec4, out weakGameEntity, 0.01f, BodyFlags.CommonCollisionExcludeFlagsForCombat);
			GameEntity gameEntity;
			if (!weakGameEntity.IsValid)
			{
				gameEntity = null;
			}
			else
			{
				MissionObject firstScriptOfTypeInFamily = weakGameEntity.GetFirstScriptOfTypeInFamily<MissionObject>();
				gameEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity((firstScriptOfTypeInFamily != null) ? firstScriptOfTypeInFamily.GameEntity : WeakGameEntity.Invalid);
			}
			this._groundEntityWhenDisabled = gameEntity;
			if (MathF.Abs(vec4.z - vec3.z) <= num + 0.5f)
			{
				return this._groundEntityWhenDisabled;
			}
			vec2.z = vec3.z;
			vec.z = vec3.z - 0.001f;
			this._ownerGameEntity.Scene.BoxCast(vec, vec2, false, Vec3.Zero, -Vec3.Up, num + 0.5f, out num2, out vec4, out weakGameEntity, BodyFlags.CommonCollisionExcludeFlagsForCombat);
			if (!MathF.IsValidValue(num2))
			{
				this._readyToBeDeleted = true;
				return null;
			}
			GameEntity gameEntity2;
			if (!weakGameEntity.IsValid)
			{
				gameEntity2 = null;
			}
			else
			{
				MissionObject firstScriptOfTypeInFamily2 = weakGameEntity.GetFirstScriptOfTypeInFamily<MissionObject>();
				gameEntity2 = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity((firstScriptOfTypeInFamily2 != null) ? firstScriptOfTypeInFamily2.GameEntity : WeakGameEntity.Invalid);
			}
			this._groundEntityWhenDisabled = gameEntity2;
			if (this._groundEntityWhenDisabled != null && MathF.Abs(vec4.z - vec3.z) <= num + 0.5f)
			{
				return this._groundEntityWhenDisabled;
			}
			return null;
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x000D22DC File Offset: 0x000D04DC
		protected internal override void OnTickOccasionally(float currentFrameDeltaTime)
		{
			this.OnTickParallel2(currentFrameDeltaTime);
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x000D22E8 File Offset: 0x000D04E8
		private void ClampEntityPositionForStoppingIfNeeded()
		{
			float minimumValue = CompressionBasic.PositionCompressionInfo.GetMinimumValue();
			float maximumValue = CompressionBasic.PositionCompressionInfo.GetMaximumValue();
			Vec3 vec = base.GameEntity.GetFrame().origin;
			bool flag;
			vec = vec.ClampedCopy(minimumValue, maximumValue, out flag);
			if (flag)
			{
				base.GameEntity.SetLocalPosition(vec);
			}
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x000D233F File Offset: 0x000D053F
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			if (base.CreatedAtRuntime)
			{
				Mission.Current.AddSpawnedItemEntityCreatedAtRuntime(this);
			}
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x000D235C File Offset: 0x000D055C
		protected override void OnRemoved(int removeReason)
		{
			if (base.HasUser && !GameNetwork.IsClientOrReplay)
			{
				base.UserAgent.StopUsingGameObjectMT(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
			base.OnRemoved(removeReason);
			base.InvalidateWeakPointersIfValid();
			this._ownerGameEntity = null;
			Agent userAgent = base.UserAgent;
			if (userAgent != null)
			{
				userAgent.OnItemRemovedFromScene();
			}
			Agent movingAgent = this.MovingAgent;
			if (movingAgent == null)
			{
				return;
			}
			movingAgent.OnItemRemovedFromScene();
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x000D23BA File Offset: 0x000D05BA
		public void AttachWeaponToWeapon(MissionWeapon attachedWeapon, ref MatrixFrame attachLocalFrame)
		{
			this._weapon.AttachWeapon(attachedWeapon, ref attachLocalFrame);
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x000D23CC File Offset: 0x000D05CC
		public bool IsReadyToBeDeleted()
		{
			return (!base.HasUser && this._readyToBeDeleted) || (this._groundEntityWhenDisabled != null && !this._groundEntityWhenDisabled.HasScene()) || (this._groundEntityWhenDisabled != null && !this._groundEntityWhenDisabled.IsVisibleIncludeParents() && (!this._groundEntityWhenDisabled.HasBody() || this._groundEntityWhenDisabled.BodyFlag.HasAnyFlag(BodyFlags.Disabled)));
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x000D2444 File Offset: 0x000D0644
		public override void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			base.GameEntity.SetPhysicsMoveToBatched(false);
			base.OnUseStopped(userAgent, isSuccessful, preferenceIndex);
			if (isSuccessful)
			{
				if (this._clientSyncData != null)
				{
					this._clientSyncData = null;
					base.GameEntity.SetAlpha(1f);
				}
				bool flag;
				userAgent.OnItemPickup(this, (EquipmentIndex)preferenceIndex, out flag);
				if (flag)
				{
					this._readyToBeDeleted = true;
					this.PhysicsStopped = true;
					base.IsDeactivated = true;
				}
			}
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x000D24B0 File Offset: 0x000D06B0
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			base.GameEntity.SetPhysicsMoveToBatched(false);
			base.OnUse(userAgent, agentBoneIndex);
			if (!GameNetwork.IsClientOrReplay)
			{
				MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
				float num = globalFrame.origin.z;
				num = Math.Max(num, globalFrame.origin.z + globalFrame.rotation.u.z * (float)this._weapon.CurrentUsageItem.WeaponLength * 0.0075f);
				float eyeGlobalHeight = userAgent.GetEyeGlobalHeight();
				bool isLeftStance = userAgent.GetIsLeftStance();
				ItemObject.ItemTypeEnum itemType = this._weapon.Item.ItemType;
				if (userAgent.HasMount)
				{
					this._usedChannelIndex = 1;
					MatrixFrame frame = userAgent.Frame;
					bool flag = Vec2.DotProduct(frame.rotation.f.AsVec2.LeftVec(), (base.GameEntity.GetGlobalFrame().origin - frame.origin).AsVec2) > 0f;
					if (num < eyeGlobalHeight * 0.7f + userAgent.Position.z)
					{
						if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
						{
							this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_down_horseback_left_begin : ActionIndexCache.act_pickup_from_right_down_horseback_left_begin);
							this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_down_horseback_left_end : ActionIndexCache.act_pickup_from_right_down_horseback_left_end);
						}
						else
						{
							this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_down_horseback_begin : ActionIndexCache.act_pickup_from_right_down_horseback_begin);
							this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_down_horseback_end : ActionIndexCache.act_pickup_from_right_down_horseback_end);
						}
					}
					else if (num < eyeGlobalHeight * 1.1f + userAgent.Position.z)
					{
						if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
						{
							this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_middle_horseback_left_begin : ActionIndexCache.act_pickup_from_right_middle_horseback_left_begin);
							this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_middle_horseback_left_end : ActionIndexCache.act_pickup_from_right_middle_horseback_left_end);
						}
						else
						{
							this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_middle_horseback_begin : ActionIndexCache.act_pickup_from_right_middle_horseback_begin);
							this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_middle_horseback_end : ActionIndexCache.act_pickup_from_right_middle_horseback_end);
						}
					}
					else if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
					{
						this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_up_horseback_left_begin : ActionIndexCache.act_pickup_from_right_up_horseback_left_begin);
						this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_up_horseback_left_end : ActionIndexCache.act_pickup_from_right_up_horseback_left_end);
					}
					else
					{
						this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_up_horseback_begin : ActionIndexCache.act_pickup_from_right_up_horseback_begin);
						this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_up_horseback_end : ActionIndexCache.act_pickup_from_right_up_horseback_end);
					}
				}
				else if (this._weapon.CurrentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.RangedWeapon | WeaponFlags.NotUsableWithOneHand | WeaponFlags.Consumable))
				{
					this._usedChannelIndex = 0;
					this._progressActionIndex = ActionIndexCache.act_pickup_boulder_begin;
					this._successActionIndex = ActionIndexCache.act_pickup_boulder_end;
				}
				else if (num < eyeGlobalHeight * 0.4f + userAgent.Position.z)
				{
					this._usedChannelIndex = 0;
					if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_down_left_begin_left_stance : ActionIndexCache.act_pickup_down_left_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_down_left_end_left_stance : ActionIndexCache.act_pickup_down_left_end);
					}
					else
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_down_begin_left_stance : ActionIndexCache.act_pickup_down_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_down_end_left_stance : ActionIndexCache.act_pickup_down_end);
					}
				}
				else if (num < eyeGlobalHeight * 1.1f + userAgent.Position.z)
				{
					this._usedChannelIndex = 1;
					if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_middle_left_begin_left_stance : ActionIndexCache.act_pickup_middle_left_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_middle_left_end_left_stance : ActionIndexCache.act_pickup_middle_left_end);
					}
					else
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_middle_begin_left_stance : ActionIndexCache.act_pickup_middle_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_middle_end_left_stance : ActionIndexCache.act_pickup_middle_end);
					}
				}
				else
				{
					this._usedChannelIndex = 1;
					if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_up_left_begin_left_stance : ActionIndexCache.act_pickup_up_left_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_up_left_end_left_stance : ActionIndexCache.act_pickup_up_left_end);
					}
					else
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_up_begin_left_stance : ActionIndexCache.act_pickup_up_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_up_end_left_stance : ActionIndexCache.act_pickup_up_end);
					}
				}
				this.SetVisibleSynched(true, false);
				userAgent.SetActionChannel(this._usedChannelIndex, in this._progressActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			}
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x000D29D8 File Offset: 0x000D0BD8
		public override bool IsDisabledForAgent(Agent agent)
		{
			return (this._weapon.IsAnyConsumable() && this._weapon.Amount == 0) || (this._weapon.IsBanner() && !MissionGameModels.Current.BattleBannerBearersModel.IsInteractableFormationBanner(this, agent));
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x000D2A24 File Offset: 0x000D0C24
		protected internal override void OnPhysicsCollision(ref PhysicsContact contact, WeakGameEntity entity0, WeakGameEntity entity1)
		{
			if (!GameNetwork.IsDedicatedServer && contact.NumberOfContactPairs > 0)
			{
				PhysicsContactInfo physicsContactInfo = default(PhysicsContactInfo);
				bool flag = false;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < contact.NumberOfContactPairs; i++)
				{
					for (int j = 0; j < contact[i].NumberOfContacts; j++)
					{
						if (!flag || contact[i][j].Impulse.LengthSquared > physicsContactInfo.Impulse.LengthSquared)
						{
							physicsContactInfo = contact[i][j];
							flag = true;
						}
					}
					switch (contact[i].ContactEventType)
					{
					case PhysicsEventType.CollisionStart:
						num++;
						break;
					case PhysicsEventType.CollisionStay:
						num2++;
						break;
					case PhysicsEventType.CollisionEnd:
						num3++;
						break;
					default:
						Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Usables\\SpawnedItemEntity.cs", "OnPhysicsCollision", 823);
						break;
					}
				}
				if (num2 > 0)
				{
					this.PlayPhysicsRollSound(physicsContactInfo.Impulse, physicsContactInfo.Position, physicsContactInfo.PhysicsMaterial1);
					return;
				}
				if (num > 0)
				{
					this.PlayPhysicsCollisionSound(physicsContactInfo.Impulse, physicsContactInfo.PhysicsMaterial1, physicsContactInfo.Position);
				}
			}
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x000D2B64 File Offset: 0x000D0D64
		private void PlayPhysicsCollisionSound(Vec3 impulse, PhysicsMaterial collidedMat, Vec3 collisionPoint)
		{
			float num = this._deletionTimer.ElapsedTime();
			if (impulse.LengthSquared > 0.0025000002f && this._lastSoundPlayTime + 0.333f < num)
			{
				this._lastSoundPlayTime = num;
				WeaponClass weaponClass = this._weapon.CurrentUsageItem.WeaponClass;
				float num2 = impulse.Length;
				bool flag = false;
				int num3;
				int num4;
				int num5;
				switch (weaponClass)
				{
				case WeaponClass.Dagger:
				case WeaponClass.ThrowingAxe:
				case WeaponClass.ThrowingKnife:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeStone;
					goto IL_01AA;
				case WeaponClass.OneHandedSword:
				case WeaponClass.OneHandedAxe:
				case WeaponClass.Mace:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeStone;
					goto IL_01AA;
				case WeaponClass.TwoHandedSword:
				case WeaponClass.TwoHandedAxe:
				case WeaponClass.TwoHandedMace:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeStone;
					goto IL_01AA;
				case WeaponClass.OneHandedPolearm:
				case WeaponClass.TwoHandedPolearm:
				case WeaponClass.LowGripPolearm:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeStone;
					goto IL_01AA;
				case WeaponClass.Arrow:
				case WeaponClass.Bolt:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeStone;
					goto IL_01AA;
				case WeaponClass.SlingStone:
				case WeaponClass.Sling:
				case WeaponClass.Stone:
				case WeaponClass.BallistaStone:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeStone;
					goto IL_01AA;
				case WeaponClass.Bow:
				case WeaponClass.Crossbow:
				case WeaponClass.Javelin:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeStone;
					goto IL_01AA;
				case WeaponClass.Boulder:
				case WeaponClass.BallistaBoulder:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderStone;
					flag = true;
					goto IL_01AA;
				case WeaponClass.SmallShield:
				case WeaponClass.LargeShield:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeStone;
					goto IL_01AA;
				}
				num3 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeDefault;
				num4 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeWood;
				num5 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeStone;
				IL_01AA:
				if (!flag)
				{
					num2 *= 0.16666667f;
					num2 = MBMath.ClampFloat(num2, 0f, 1f);
				}
				else
				{
					num2 = (num2 - 7f) * 0.030303031f * 0.1f + 0.9f;
					num2 = MBMath.ClampFloat(num2, 0.9f, 1f);
				}
				int num6 = num3;
				if (collidedMat.IsValid)
				{
					string name = collidedMat.Name;
					if (name.Contains("wood"))
					{
						num6 = num4;
					}
					else if (name.Contains("stone"))
					{
						num6 = num5;
					}
				}
				SoundEventParameter soundEventParameter = new SoundEventParameter("Force", num2);
				Mission.Current.MakeSound(num6, collisionPoint, true, false, -1, -1, ref soundEventParameter);
			}
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x000D2DC8 File Offset: 0x000D0FC8
		private void PlayPhysicsRollSound(Vec3 impulse, Vec3 collisionPoint, PhysicsMaterial collidedMat)
		{
			WeaponComponentData currentUsageItem = this._weapon.CurrentUsageItem;
			if (currentUsageItem.WeaponClass == WeaponClass.Boulder && currentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.RangedWeapon | WeaponFlags.NotUsableWithOneHand | WeaponFlags.Consumable))
			{
				float num = this._deletionTimer.ElapsedTime();
				if (impulse.LengthSquared > 0.0001f && this._lastSoundPlayTime + 0.333f < num)
				{
					if (this._rollingSoundEvent == null || !this._rollingSoundEvent.IsValid)
					{
						this._lastSoundPlayTime = num;
						int num2 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderDefault;
						string name = collidedMat.Name;
						if (name.Contains("stone"))
						{
							num2 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderStone;
						}
						else if (name.Contains("wood"))
						{
							num2 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderWood;
						}
						this._rollingSoundEvent = SoundEvent.CreateEvent(num2, Mission.Current.Scene);
						this._rollingSoundEvent.PlayInPosition(collisionPoint);
					}
					float num3 = impulse.Length * 0.033333335f;
					num3 = MBMath.ClampFloat(num3, 0f, 1f);
					this._rollingSoundEvent.SetParameter("Force", num3);
					this._rollingSoundEvent.SetPosition(collisionPoint);
				}
			}
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x000D2EE9 File Offset: 0x000D10E9
		public bool IsStuckMissile()
		{
			return this.SpawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.AsMissile);
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x000D2EF7 File Offset: 0x000D10F7
		public bool IsQuiverAndNotEmpty()
		{
			return this._weapon.Item.PrimaryWeapon.IsConsumable && this._weapon.Amount > 0;
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x000D2F20 File Offset: 0x000D1120
		public bool IsBanner()
		{
			return this._weapon.IsBanner();
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x000D2F2D File Offset: 0x000D112D
		public override TextObject GetInfoTextForBeingNotInteractable(Agent userAgent)
		{
			if (!base.IsDeactivated && this._weapon.IsAnyConsumable() && this._weapon.Amount == 0)
			{
				return GameTexts.FindText("str_ui_empty_quiver", null);
			}
			return base.GetInfoTextForBeingNotInteractable(userAgent);
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x000D2F64 File Offset: 0x000D1164
		public void StopPhysicsAndSetFrameForClient(MatrixFrame frame, GameEntity parent)
		{
			if (parent != null)
			{
				frame = parent.GetGlobalFrame().TransformToParent(in frame);
			}
			frame.rotation.Orthonormalize();
			this._clientSyncData = new SpawnedItemEntity.ClientSyncData();
			this._clientSyncData.Frame = frame;
			this._clientSyncData.Timer = new Timer(Mission.Current.CurrentTime, 0.5f, false);
			this._clientSyncData.Parent = parent;
			if (!this.PhysicsStopped)
			{
				this.PhysicsStopped = true;
				if (!this._weapon.IsEmpty && !base.GameEntity.BodyFlag.HasAnyFlag(BodyFlags.Disabled))
				{
					base.GameEntity.DisableDynamicBodySimulation();
					return;
				}
				base.GameEntity.RemovePhysics(false);
			}
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x000D3025 File Offset: 0x000D1225
		public void ConsumeWeaponAmount(short consumedAmount)
		{
			this._weapon.Consume(consumedAmount);
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x000D3034 File Offset: 0x000D1234
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return null;
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x000D3037 File Offset: 0x000D1237
		public void RequestDeletionOnNextTick()
		{
			this._deletionTimer = new Timer(Mission.Current.CurrentTime, -1f, true);
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x000D3054 File Offset: 0x000D1254
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, false);
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x000D305E File Offset: 0x000D125E
		public SpawnedItemEntity()
			: base(false)
		{
		}

		// Token: 0x040015CF RID: 5583
		private MissionWeapon _weapon;

		// Token: 0x040015D0 RID: 5584
		private bool _hasLifeTime;

		// Token: 0x040015D1 RID: 5585
		public string WeaponName = "";

		// Token: 0x040015D2 RID: 5586
		private const float LongLifeTime = 180f;

		// Token: 0x040015D3 RID: 5587
		private const float DisablePhysicsTime = 10f;

		// Token: 0x040015D4 RID: 5588
		private const float QuickFadeoutLifeTime = 5f;

		// Token: 0x040015D5 RID: 5589
		private const float TotalFadeOutInDuration = 0.5f;

		// Token: 0x040015D6 RID: 5590
		private const float PreventStationaryCheckTime = 1f;

		// Token: 0x040015D7 RID: 5591
		private Timer _disablePhysicsTimer;

		// Token: 0x040015D8 RID: 5592
		private bool _physicsStopped;

		// Token: 0x040015D9 RID: 5593
		private bool _readyToBeDeleted;

		// Token: 0x040015DA RID: 5594
		private Timer _deletionTimer;

		// Token: 0x040015DB RID: 5595
		private int _usedChannelIndex;

		// Token: 0x040015DC RID: 5596
		private ActionIndexCache _progressActionIndex;

		// Token: 0x040015DD RID: 5597
		private ActionIndexCache _successActionIndex;

		// Token: 0x040015DE RID: 5598
		private float _lastSoundPlayTime;

		// Token: 0x040015DF RID: 5599
		private const float MinSoundDelay = 0.333f;

		// Token: 0x040015E0 RID: 5600
		private SoundEvent _rollingSoundEvent;

		// Token: 0x040015E1 RID: 5601
		private SpawnedItemEntity.ClientSyncData _clientSyncData;

		// Token: 0x040015E2 RID: 5602
		private GameEntity _ownerGameEntity;

		// Token: 0x040015E3 RID: 5603
		private Vec3 _fakeSimulationVelocity;

		// Token: 0x040015E4 RID: 5604
		private bool _alreadyMadeWaterDropSound;

		// Token: 0x040015E6 RID: 5606
		private bool _disableDynamicPhysicsNextFrame;

		// Token: 0x040015E7 RID: 5607
		private GameEntity _groundEntityWhenDisabled;

		// Token: 0x02000658 RID: 1624
		private class ClientSyncData
		{
			// Token: 0x040021D1 RID: 8657
			public MatrixFrame Frame;

			// Token: 0x040021D2 RID: 8658
			public GameEntity Parent;

			// Token: 0x040021D3 RID: 8659
			public Timer Timer;
		}
	}
}
