using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200034D RID: 845
	public class DestructableComponent : SynchedMissionObject, IFocusable
	{
		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06002FE0 RID: 12256 RVA: 0x000BBA28 File Offset: 0x000B9C28
		// (remove) Token: 0x06002FE1 RID: 12257 RVA: 0x000BBA60 File Offset: 0x000B9C60
		public event Action OnNextDestructionState;

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06002FE2 RID: 12258 RVA: 0x000BBA98 File Offset: 0x000B9C98
		// (remove) Token: 0x06002FE3 RID: 12259 RVA: 0x000BBAD0 File Offset: 0x000B9CD0
		public event DestructableComponent.OnHitTakenAndDestroyedDelegate OnDestroyed;

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06002FE4 RID: 12260 RVA: 0x000BBB08 File Offset: 0x000B9D08
		// (remove) Token: 0x06002FE5 RID: 12261 RVA: 0x000BBB40 File Offset: 0x000B9D40
		public event DestructableComponent.OnHitTakenAndDestroyedDelegate OnHitTaken;

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06002FE6 RID: 12262 RVA: 0x000BBB78 File Offset: 0x000B9D78
		// (remove) Token: 0x06002FE7 RID: 12263 RVA: 0x000BBBB0 File Offset: 0x000B9DB0
		public event DestructableComponent.OnHitTakenWithImpactDelegate OnHitTakenWithImpact;

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06002FE8 RID: 12264 RVA: 0x000BBBE5 File Offset: 0x000B9DE5
		// (set) Token: 0x06002FE9 RID: 12265 RVA: 0x000BBBF0 File Offset: 0x000B9DF0
		public float HitPoint
		{
			get
			{
				return this._hitPoint;
			}
			set
			{
				if (!this._hitPoint.Equals(value))
				{
					this._hitPoint = MathF.Max(value, 0f);
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SyncObjectHitpoints(base.Id, value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					}
				}
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06002FEA RID: 12266 RVA: 0x000BBC41 File Offset: 0x000B9E41
		public FocusableObjectType FocusableObjectType
		{
			get
			{
				return FocusableObjectType.None;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06002FEB RID: 12267 RVA: 0x000BBC44 File Offset: 0x000B9E44
		public virtual bool IsFocusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06002FEC RID: 12268 RVA: 0x000BBC47 File Offset: 0x000B9E47
		public bool IsDestroyed
		{
			get
			{
				return this.HitPoint <= 0f;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002FED RID: 12269 RVA: 0x000BBC59 File Offset: 0x000B9E59
		// (set) Token: 0x06002FEE RID: 12270 RVA: 0x000BBC61 File Offset: 0x000B9E61
		public GameEntity CurrentState { get; private set; }

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002FEF RID: 12271 RVA: 0x000BBC6A File Offset: 0x000B9E6A
		private bool HasDestructionState
		{
			get
			{
				return this._destructionStates != null && !this._destructionStates.IsEmpty<string>();
			}
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x000BBC84 File Offset: 0x000B9E84
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			this._referenceEntity = null;
			this._previousState = null;
			this._originalState = null;
			this.CurrentState = null;
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000BBCAC File Offset: 0x000B9EAC
		protected DestructableComponent()
		{
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000BBD04 File Offset: 0x000B9F04
		protected internal override void OnInit()
		{
			base.OnInit();
			this._hitPoint = this.MaxHitPoint;
			this._referenceEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(string.IsNullOrEmpty(this.ReferenceEntityTag) ? base.GameEntity : base.GameEntity.GetFirstChildEntityWithTag(this.ReferenceEntityTag));
			if (!string.IsNullOrEmpty(this.DestructionStates))
			{
				this._destructionStates = this.DestructionStates.Replace(" ", string.Empty).Split(new char[] { ',' });
				bool flag = false;
				string[] destructionStates = this._destructionStates;
				for (int i = 0; i < destructionStates.Length; i++)
				{
					string item = destructionStates[i];
					if (!string.IsNullOrEmpty(item))
					{
						WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == item);
						if (weakGameEntity.IsValid)
						{
							weakGameEntity.AddBodyFlags(BodyFlags.Moveable, true);
							PhysicsShape bodyShape = weakGameEntity.GetBodyShape();
							if (bodyShape != null)
							{
								PhysicsShape.AddPreloadQueueWithName(bodyShape.GetName(), weakGameEntity.GetGlobalScale());
								flag = true;
							}
						}
						else
						{
							GameEntity gameEntity = TaleWorlds.Engine.GameEntity.Instantiate(null, item, false, true, "");
							List<GameEntity> list = new List<GameEntity>();
							gameEntity.GetChildrenRecursive(ref list);
							list.Add(gameEntity);
							foreach (GameEntity gameEntity2 in list)
							{
								PhysicsShape bodyShape2 = gameEntity2.GetBodyShape();
								if (bodyShape2 != null)
								{
									Vec3 globalScale = gameEntity2.GetGlobalScale();
									Vec3 globalScale2 = this._referenceEntity.GetGlobalScale();
									Vec3 vec = new Vec3(globalScale.x * globalScale2.x, globalScale.y * globalScale2.y, globalScale.z * globalScale2.z, -1f);
									PhysicsShape.AddPreloadQueueWithName(bodyShape2.GetName(), vec);
									flag = true;
								}
							}
						}
					}
				}
				if (flag)
				{
					PhysicsShape.ProcessPreloadQueue();
				}
			}
			WeakGameEntity originalState = this.GetOriginalState(base.GameEntity);
			this._originalState = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(originalState.IsValid ? originalState : base.GameEntity);
			this.CurrentState = this._originalState;
			this._originalState.AddBodyFlags(BodyFlags.Moveable, true);
			List<WeakGameEntity> list2 = new List<WeakGameEntity>();
			base.GameEntity.GetChildrenRecursive(ref list2);
			foreach (WeakGameEntity weakGameEntity2 in list2.Where<WeakGameEntity>((WeakGameEntity child) => child.BodyFlag.HasAnyFlag(BodyFlags.Dynamic)))
			{
				weakGameEntity2.SetPhysicsState(false, true);
				weakGameEntity2.SetFrameChanged();
			}
			this._heavyHitParticles = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity).CollectChildrenEntitiesWithTag(this.HeavyHitParticlesTag);
			base.GameEntity.SetAnimationSoundActivation(true);
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000BC01C File Offset: 0x000BA21C
		public WeakGameEntity GetOriginalState(WeakGameEntity parent)
		{
			int childCount = parent.ChildCount;
			for (int i = 0; i < childCount; i++)
			{
				WeakGameEntity child = parent.GetChild(i);
				if (!child.HasScriptOfType<DestructableComponent>())
				{
					if (child.HasTag(this.OriginalStateTag))
					{
						return child;
					}
					WeakGameEntity originalState = this.GetOriginalState(child);
					if (originalState.IsValid)
					{
						return originalState;
					}
				}
			}
			return WeakGameEntity.Invalid;
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000BC078 File Offset: 0x000BA278
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._referenceEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(string.IsNullOrEmpty(this.ReferenceEntityTag) ? base.GameEntity : base.GameEntity.GetFirstChildEntityWithTag(this.ReferenceEntityTag));
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x000BC0C0 File Offset: 0x000BA2C0
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName.Equals(this.ReferenceEntityTag))
			{
				this._referenceEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(string.IsNullOrEmpty(this.ReferenceEntityTag) ? base.GameEntity : base.GameEntity.GetFirstChildEntityWithTag(this.ReferenceEntityTag));
			}
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x000BC116 File Offset: 0x000BA316
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this.Reset();
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x000BC124 File Offset: 0x000BA324
		public void Reset()
		{
			this.RestoreEntity();
			this._hitPoint = this.MaxHitPoint;
			this._currentStateIndex = 0;
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x000BC140 File Offset: 0x000BA340
		private void RestoreEntity()
		{
			if (this._destructionStates != null)
			{
				int j;
				int i;
				for (i = 0; i < this._destructionStates.Length; i = j + 1)
				{
					WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == this._destructionStates[i].ToString());
					if (weakGameEntity.IsValid)
					{
						Skeleton skeleton = weakGameEntity.Skeleton;
						if (skeleton != null)
						{
							skeleton.SetAnimationAtChannel(-1, 0, 1f, -1f, 0f);
						}
					}
					j = i;
				}
			}
			if (this.CurrentState != this._originalState)
			{
				this.CurrentState.SetVisibilityExcludeParents(false);
				this.CurrentState.SetPhysicsState(false, true);
				this.CurrentState = this._originalState;
			}
			this.CurrentState.SetVisibilityExcludeParents(true);
			this.CurrentState.SetPhysicsState(true, true);
			this.CurrentState.SetFrameChanged();
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x000BC238 File Offset: 0x000BA438
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (this._referenceEntity != null && this._referenceEntity != base.GameEntity && MBEditor.IsEntitySelected(this._referenceEntity))
			{
				new Vec3(-2f, -0.5f, -1f, -1f);
				new Vec3(2f, 0.5f, 1f, -1f);
				MatrixFrame identity = MatrixFrame.Identity;
				this._referenceEntity.Root.GetMeshBendedFrame(this._referenceEntity.GetGlobalFrame(), ref identity);
			}
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x000BC2D4 File Offset: 0x000BA4D4
		public void TriggerOnHit(Agent attackerAgent, int inflictedDamage, Vec3 impactPosition, Vec3 impactDirection, in MissionWeapon weapon, int affectorWeaponSlotOrMissileIndex, ScriptComponentBehavior attackerScriptComponentBehavior)
		{
			bool flag;
			float num;
			float num2;
			float num3;
			this.OnHit(attackerAgent, inflictedDamage, impactPosition, impactDirection, in weapon, affectorWeaponSlotOrMissileIndex, attackerScriptComponentBehavior, out flag, out num, out num2, out num3);
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000BC2FC File Offset: 0x000BA4FC
		protected internal override bool OnHit(Agent attackerAgent, int inflictedDamage, Vec3 impactPosition, Vec3 impactDirection, in MissionWeapon weapon, int affectorWeaponSlotOrMissileIndex, ScriptComponentBehavior attackerScriptComponentBehavior, out bool reportDamage, out float modifiedDamage, out float fireDamage, out float modifiedFireDamage)
		{
			reportDamage = false;
			modifiedDamage = (float)inflictedDamage;
			fireDamage = -1f;
			modifiedFireDamage = -1f;
			if (base.IsDisabled)
			{
				return true;
			}
			MissionWeapon missionWeapon = weapon;
			if (missionWeapon.IsEmpty && !(attackerScriptComponentBehavior is BatteringRam))
			{
				inflictedDamage = 0;
			}
			else if (this.DestroyedByStoneOnly)
			{
				missionWeapon = weapon;
				WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
				if ((currentUsageItem.WeaponClass != WeaponClass.Sling && currentUsageItem.WeaponClass != WeaponClass.Stone && currentUsageItem.WeaponClass != WeaponClass.Boulder && currentUsageItem.WeaponClass != WeaponClass.BallistaBoulder && currentUsageItem.WeaponClass != WeaponClass.BallistaStone) || !currentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.NotUsableWithOneHand))
				{
					inflictedDamage = 0;
				}
			}
			bool isDestroyed = this.IsDestroyed;
			if (this.DestroyOnAnyHit)
			{
				inflictedDamage = (int)(this.MaxHitPoint + 1f);
			}
			if (inflictedDamage > 0 && !isDestroyed)
			{
				this.HitPoint -= (float)inflictedDamage;
				if ((float)inflictedDamage > this.HeavyHitParticlesThreshold)
				{
					this.BurstHeavyHitParticles();
				}
				int num = this.CalculateNextDestructionLevel(inflictedDamage);
				if (!this.IsDestroyed)
				{
					DestructableComponent.OnHitTakenAndDestroyedDelegate onHitTaken = this.OnHitTaken;
					if (onHitTaken != null)
					{
						onHitTaken(this, attackerAgent, in weapon, attackerScriptComponentBehavior, inflictedDamage);
					}
					DestructableComponent.OnHitTakenWithImpactDelegate onHitTakenWithImpact = this.OnHitTakenWithImpact;
					if (onHitTakenWithImpact != null)
					{
						onHitTakenWithImpact(this, attackerAgent, impactPosition, impactDirection, inflictedDamage);
					}
				}
				else if (this.IsDestroyed && !isDestroyed)
				{
					Mission.Current.OnObjectDisabled(this);
					DestructableComponent.OnHitTakenAndDestroyedDelegate onHitTaken2 = this.OnHitTaken;
					if (onHitTaken2 != null)
					{
						onHitTaken2(this, attackerAgent, in weapon, attackerScriptComponentBehavior, inflictedDamage);
					}
					DestructableComponent.OnHitTakenWithImpactDelegate onHitTakenWithImpact2 = this.OnHitTakenWithImpact;
					if (onHitTakenWithImpact2 != null)
					{
						onHitTakenWithImpact2(this, attackerAgent, impactPosition, impactDirection, inflictedDamage);
					}
					DestructableComponent.OnHitTakenAndDestroyedDelegate onDestroyed = this.OnDestroyed;
					if (onDestroyed != null)
					{
						onDestroyed(this, attackerAgent, in weapon, attackerScriptComponentBehavior, inflictedDamage);
					}
					MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
					globalFrame.origin += globalFrame.rotation.u * this.SoundAndParticleEffectHeightOffset + globalFrame.rotation.f * this.SoundAndParticleEffectForwardOffset;
					globalFrame.rotation.Orthonormalize();
					if (this.ParticleEffectOnDestroy != "")
					{
						Mission.Current.Scene.CreateBurstParticle(ParticleSystemManager.GetRuntimeIdByName(this.ParticleEffectOnDestroy), globalFrame);
					}
					if (this.SoundEffectOnDestroy != "")
					{
						Mission.Current.MakeSound(SoundEvent.GetEventIdFromString(this.SoundEffectOnDestroy), globalFrame.origin, false, true, (attackerAgent != null) ? attackerAgent.Index : (-1), -1);
					}
				}
				this.SetDestructionLevel(num, -1, (float)inflictedDamage, impactPosition, impactDirection, false);
				reportDamage = true;
			}
			return !this.PassHitOnToParent;
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x000BC58C File Offset: 0x000BA78C
		public void BurstHeavyHitParticles()
		{
			foreach (GameEntity gameEntity in this._heavyHitParticles)
			{
				gameEntity.BurstEntityParticle(false);
			}
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new BurstAllHeavyHitParticles(base.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000BC604 File Offset: 0x000BA804
		private int CalculateNextDestructionLevel(int inflictedDamage)
		{
			if (this.HasDestructionState)
			{
				int num = this._destructionStates.Length;
				float num2 = this.MaxHitPoint / (float)num;
				float num3 = this.MaxHitPoint;
				int num4 = 0;
				while (num3 - num2 >= this.HitPoint)
				{
					num3 -= num2;
					num4++;
				}
				Func<int, int, int, int> onCalculateDestructionStateIndex = this.OnCalculateDestructionStateIndex;
				return (onCalculateDestructionStateIndex != null) ? onCalculateDestructionStateIndex(num4, inflictedDamage, this.DestructionStates.Length) : num4;
			}
			if (this.IsDestroyed)
			{
				return this._currentStateIndex + 1;
			}
			return this._currentStateIndex;
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x000BC684 File Offset: 0x000BA884
		public void SetDestructionLevel(int state, int forcedId, float blowMagnitude, Vec3 blowPosition, Vec3 blowDirection, bool noEffects = false)
		{
			if (this._currentStateIndex != state)
			{
				float num = MBMath.ClampFloat(blowMagnitude, 1f, DestructableComponent.MaxBlowMagnitude);
				this._currentStateIndex = state;
				this.ReplaceEntityWithBrokenEntity(forcedId);
				if (this.CurrentState != null)
				{
					List<GameEntity> list = new List<GameEntity>();
					if (this.CurrentState.Parent != null)
					{
						list.Add(this.CurrentState);
					}
					this.CurrentState.GetChildrenRecursive(ref list);
					foreach (GameEntity gameEntity in list)
					{
						if (gameEntity.BodyFlag.HasAnyFlag(BodyFlags.Dynamic))
						{
							gameEntity.Parent.RemoveChild(gameEntity, true, true, false, 178);
							gameEntity.SetPhysicsState(true, true);
							gameEntity.SetFrameChanged();
						}
					}
					if (!GameNetwork.IsDedicatedServer && !noEffects)
					{
						this.CurrentState.BurstEntityParticle(true);
						this.ApplyPhysics(num, blowPosition, blowDirection);
					}
					Action onNextDestructionState = this.OnNextDestructionState;
					if (onNextDestructionState != null)
					{
						onNextDestructionState();
					}
				}
				if (GameNetwork.IsServerOrRecorder)
				{
					if (this.CurrentState != null)
					{
						MissionObject firstScriptOfType = this.CurrentState.GetFirstScriptOfType<MissionObject>();
						if (firstScriptOfType != null)
						{
							forcedId = firstScriptOfType.Id.Id;
						}
					}
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SyncObjectDestructionLevel(base.Id, state, forcedId, num, blowPosition, blowDirection));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x000BC7F4 File Offset: 0x000BA9F4
		private void ApplyPhysics(float blowMagnitude, Vec3 blowPosition, Vec3 blowDirection)
		{
			if (this.CurrentState != null)
			{
				IEnumerable<GameEntity> enumerable = from child in this.CurrentState.GetChildren()
					where child.HasBody() && child.BodyFlag.HasAnyFlag(BodyFlags.Dynamic) && !child.HasScriptOfType<SpawnedItemEntity>()
					select child;
				int num = enumerable.Count<GameEntity>();
				float num2 = ((num > 1) ? (blowMagnitude / (float)num) : blowMagnitude);
				foreach (GameEntity gameEntity in enumerable)
				{
					gameEntity.ApplyLocalImpulseToDynamicBody(Vec3.Zero, blowDirection * num2);
					Mission.Current.AddTimerToDynamicEntity(gameEntity, 10f + MBRandom.RandomFloat * 2f);
				}
			}
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x000BC8B8 File Offset: 0x000BAAB8
		private void ReplaceEntityWithBrokenEntity(int forcedId)
		{
			this._previousState = this.CurrentState;
			this._previousState.SetVisibilityExcludeParents(false);
			this._previousState.SetPhysicsState(false, true);
			if (this.HasDestructionState)
			{
				bool flag;
				this.CurrentState = this.AddBrokenEntity(this._destructionStates[this._currentStateIndex - 1], out flag);
				if (flag)
				{
					if (this._originalState != base.GameEntity)
					{
						base.GameEntity.AddChild(this.CurrentState.WeakEntity, true);
					}
					if (forcedId != -1)
					{
						MissionObject firstScriptOfType = this.CurrentState.GetFirstScriptOfType<MissionObject>();
						if (firstScriptOfType != null)
						{
							firstScriptOfType.Id = new MissionObjectId(forcedId, true);
							using (IEnumerator<GameEntity> enumerator = this.CurrentState.GetChildren().GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									GameEntity gameEntity = enumerator.Current;
									MissionObject firstScriptOfType2 = gameEntity.GetFirstScriptOfType<MissionObject>();
									if (firstScriptOfType2 != null && firstScriptOfType2.Id.CreatedAtRuntime)
									{
										firstScriptOfType2.Id = new MissionObjectId(++forcedId, true);
									}
								}
								return;
							}
						}
						MBDebug.ShowWarning("Current destruction state doesn't have mission object script component.");
					}
				}
			}
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x000BC9D8 File Offset: 0x000BABD8
		protected internal override bool MovesEntity()
		{
			return true;
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x000BC9DB File Offset: 0x000BABDB
		public void PreDestroy()
		{
			DestructableComponent.OnHitTakenAndDestroyedDelegate onDestroyed = this.OnDestroyed;
			if (onDestroyed != null)
			{
				onDestroyed(this, null, in MissionWeapon.Invalid, null, 0);
			}
			this.SetVisibleSynched(false, true);
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x000BCA00 File Offset: 0x000BAC00
		private GameEntity AddBrokenEntity(string prefab, out bool newCreated)
		{
			if (!string.IsNullOrEmpty(prefab))
			{
				int childCount = base.GameEntity.ChildCount;
				int num = 0;
				WeakGameEntity weakGameEntity = WeakGameEntity.Invalid;
				for (int i = 0; i < childCount; i++)
				{
					WeakGameEntity child = base.GameEntity.GetChild(i);
					if (child.Name == prefab)
					{
						num++;
						if (MBRandom.RandomInt(num) == 0)
						{
							weakGameEntity = child;
						}
					}
				}
				GameEntity gameEntity;
				if (weakGameEntity.IsValid)
				{
					gameEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity);
					weakGameEntity.SetVisibilityExcludeParents(true);
					weakGameEntity.SetPhysicsState(true, true);
					if (!GameNetwork.IsClientOrReplay)
					{
						MissionObject firstScriptOfType = weakGameEntity.GetFirstScriptOfType<MissionObject>();
						if (firstScriptOfType != null)
						{
							firstScriptOfType.SetAbilityOfFaces(true);
						}
					}
					newCreated = false;
				}
				else
				{
					gameEntity = TaleWorlds.Engine.GameEntity.Instantiate(Mission.Current.Scene, prefab, this._referenceEntity.GetGlobalFrame(), true);
					if (gameEntity != null)
					{
						gameEntity.SetMobility(TaleWorlds.Engine.GameEntity.Mobility.Stationary);
					}
					if (base.GameEntity.Parent.IsValid)
					{
						base.GameEntity.Parent.AddChild(gameEntity.WeakEntity, true);
					}
					newCreated = true;
				}
				if (this._referenceEntity.Skeleton != null && gameEntity.Skeleton != null)
				{
					Skeleton skeleton = ((this.CurrentState != this._originalState) ? this.CurrentState : this._referenceEntity).Skeleton;
					int animationIndexAtChannel = skeleton.GetAnimationIndexAtChannel(0);
					float animationParameterAtChannel = skeleton.GetAnimationParameterAtChannel(0);
					if (animationIndexAtChannel != -1)
					{
						gameEntity.Skeleton.SetAnimationAtChannel(animationIndexAtChannel, 0, 1f, -1f, animationParameterAtChannel);
						gameEntity.ResumeSkeletonAnimation();
					}
				}
				WeakGameEntity weakGameEntity2 = base.GameEntity;
				while (weakGameEntity2 != null)
				{
					ColorAssigner firstScriptOfType2 = weakGameEntity2.GetFirstScriptOfType<ColorAssigner>();
					if (firstScriptOfType2 != null)
					{
						firstScriptOfType2.SetColor(gameEntity.WeakEntity);
						break;
					}
					weakGameEntity2 = weakGameEntity2.Parent;
				}
				return gameEntity;
			}
			newCreated = false;
			return null;
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x000BCBD8 File Offset: 0x000BADD8
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteFloatToPacket(MathF.Max(this.HitPoint, 0f), CompressionMission.UsableGameObjectHealthCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this._currentStateIndex, CompressionMission.UsableGameObjectDestructionStateCompressionInfo);
			if (this._currentStateIndex != 0)
			{
				MissionObject firstScriptOfType = this.CurrentState.GetFirstScriptOfType<MissionObject>();
				GameNetworkMessage.WriteBoolToPacket(firstScriptOfType != null);
				if (firstScriptOfType != null)
				{
					GameNetworkMessage.WriteMissionObjectIdToPacket(firstScriptOfType.Id);
				}
			}
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x000BCC40 File Offset: 0x000BAE40
		public override void AddStuckMissile(GameEntity missileEntity)
		{
			if (this.CurrentState != null)
			{
				this.CurrentState.AddChild(missileEntity, false);
				return;
			}
			base.GameEntity.AddChild(missileEntity.WeakEntity, false);
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x000BCC80 File Offset: 0x000BAE80
		protected internal override bool OnCheckForProblems()
		{
			bool flag = base.OnCheckForProblems();
			if (!(string.IsNullOrEmpty(this.ReferenceEntityTag) ? base.GameEntity : base.GameEntity.GetFirstChildEntityWithTag(this.ReferenceEntityTag)).IsValid)
			{
				MBEditor.AddEntityWarning(base.GameEntity, "Reference entity must be assigned. Root entity is " + base.GameEntity.Root.Name + ", child is " + base.GameEntity.Name);
				flag = true;
			}
			string[] array = this.DestructionStates.Replace(" ", string.Empty).Split(new char[] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				string destructionState = array[i];
				if (!string.IsNullOrEmpty(destructionState) && !base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == destructionState).IsValid && TaleWorlds.Engine.GameEntity.Instantiate(null, destructionState, false, true, "") == null)
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Destruction state '" + destructionState + "' is not valid.");
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x000BCDCF File Offset: 0x000BAFCF
		public void OnFocusGain(Agent userAgent)
		{
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x000BCDD1 File Offset: 0x000BAFD1
		public void OnFocusLose(Agent userAgent)
		{
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x000BCDD3 File Offset: 0x000BAFD3
		public TextObject GetInfoTextForBeingNotInteractable(Agent userAgent)
		{
			return null;
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x000BCDD8 File Offset: 0x000BAFD8
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			DestructableComponent.DestructableComponentRecord destructableComponentRecord = (DestructableComponent.DestructableComponentRecord)synchedMissionObjectReadableRecord.Item2;
			this.HitPoint = destructableComponentRecord.HitPoint;
			if (destructableComponentRecord.DestructionState != 0)
			{
				if (this.IsDestroyed)
				{
					DestructableComponent.OnHitTakenAndDestroyedDelegate onDestroyed = this.OnDestroyed;
					if (onDestroyed != null)
					{
						onDestroyed(this, null, in MissionWeapon.Invalid, null, 0);
					}
				}
				this.SetDestructionLevel(destructableComponentRecord.DestructionState, destructableComponentRecord.ForceIndex, 0f, Vec3.Zero, Vec3.Zero, true);
			}
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x000BCE58 File Offset: 0x000BB058
		public TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			int num;
			TextObject textObject;
			if (int.TryParse(gameEntity.Name.Split(new char[] { '_' }).Last<string>(), out num))
			{
				string text = gameEntity.Name;
				text = text.Remove(text.Length - num.ToString().Length);
				text += "x";
				if (GameTexts.TryGetText("str_destructible_component", out textObject, text))
				{
					return textObject;
				}
			}
			if (GameTexts.TryGetText("str_destructible_component", out textObject, gameEntity.Name))
			{
				return textObject;
			}
			return null;
		}

		// Token: 0x0400137B RID: 4987
		public const string CleanStateTag = "operational";

		// Token: 0x0400137C RID: 4988
		public static float MaxBlowMagnitude = 20f;

		// Token: 0x0400137D RID: 4989
		public string DestructionStates;

		// Token: 0x0400137E RID: 4990
		public bool DestroyedByStoneOnly;

		// Token: 0x0400137F RID: 4991
		public bool CanBeDestroyedInitially = true;

		// Token: 0x04001380 RID: 4992
		public float MaxHitPoint = 100f;

		// Token: 0x04001381 RID: 4993
		public bool DestroyOnAnyHit;

		// Token: 0x04001382 RID: 4994
		public bool PassHitOnToParent;

		// Token: 0x04001383 RID: 4995
		public string ReferenceEntityTag;

		// Token: 0x04001384 RID: 4996
		public string HeavyHitParticlesTag;

		// Token: 0x04001385 RID: 4997
		public float HeavyHitParticlesThreshold = 5f;

		// Token: 0x04001386 RID: 4998
		public string ParticleEffectOnDestroy = "";

		// Token: 0x04001387 RID: 4999
		public string SoundEffectOnDestroy = "";

		// Token: 0x04001388 RID: 5000
		public float SoundAndParticleEffectHeightOffset;

		// Token: 0x04001389 RID: 5001
		public float SoundAndParticleEffectForwardOffset;

		// Token: 0x0400138E RID: 5006
		public BattleSideEnum BattleSide = BattleSideEnum.None;

		// Token: 0x0400138F RID: 5007
		[EditableScriptComponentVariable(false, "")]
		public Func<int, int, int, int> OnCalculateDestructionStateIndex;

		// Token: 0x04001390 RID: 5008
		private float _hitPoint;

		// Token: 0x04001391 RID: 5009
		private string OriginalStateTag = "operational";

		// Token: 0x04001392 RID: 5010
		private GameEntity _referenceEntity;

		// Token: 0x04001393 RID: 5011
		private GameEntity _previousState;

		// Token: 0x04001394 RID: 5012
		private GameEntity _originalState;

		// Token: 0x04001396 RID: 5014
		private string[] _destructionStates;

		// Token: 0x04001397 RID: 5015
		private int _currentStateIndex;

		// Token: 0x04001398 RID: 5016
		private List<GameEntity> _heavyHitParticles;

		// Token: 0x02000627 RID: 1575
		[DefineSynchedMissionObjectType(typeof(DestructableComponent))]
		public struct DestructableComponentRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AD1 RID: 2769
			// (get) Token: 0x0600409D RID: 16541 RVA: 0x000FC2B2 File Offset: 0x000FA4B2
			// (set) Token: 0x0600409E RID: 16542 RVA: 0x000FC2BA File Offset: 0x000FA4BA
			public float HitPoint { get; private set; }

			// Token: 0x17000AD2 RID: 2770
			// (get) Token: 0x0600409F RID: 16543 RVA: 0x000FC2C3 File Offset: 0x000FA4C3
			// (set) Token: 0x060040A0 RID: 16544 RVA: 0x000FC2CB File Offset: 0x000FA4CB
			public int DestructionState { get; private set; }

			// Token: 0x17000AD3 RID: 2771
			// (get) Token: 0x060040A1 RID: 16545 RVA: 0x000FC2D4 File Offset: 0x000FA4D4
			// (set) Token: 0x060040A2 RID: 16546 RVA: 0x000FC2DC File Offset: 0x000FA4DC
			public int ForceIndex { get; private set; }

			// Token: 0x17000AD4 RID: 2772
			// (get) Token: 0x060040A3 RID: 16547 RVA: 0x000FC2E5 File Offset: 0x000FA4E5
			// (set) Token: 0x060040A4 RID: 16548 RVA: 0x000FC2ED File Offset: 0x000FA4ED
			public bool IsMissionObject { get; private set; }

			// Token: 0x060040A5 RID: 16549 RVA: 0x000FC2F6 File Offset: 0x000FA4F6
			public DestructableComponentRecord(float hitPoint, int destructionState, int forceIndex, bool isMissionObject)
			{
				this.HitPoint = hitPoint;
				this.DestructionState = destructionState;
				this.ForceIndex = forceIndex;
				this.IsMissionObject = isMissionObject;
			}

			// Token: 0x060040A6 RID: 16550 RVA: 0x000FC318 File Offset: 0x000FA518
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.HitPoint = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.UsableGameObjectHealthCompressionInfo, ref bufferReadValid);
				this.DestructionState = GameNetworkMessage.ReadIntFromPacket(CompressionMission.UsableGameObjectDestructionStateCompressionInfo, ref bufferReadValid);
				this.ForceIndex = -1;
				if (this.DestructionState != 0)
				{
					this.IsMissionObject = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
					if (this.IsMissionObject)
					{
						this.ForceIndex = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref bufferReadValid).Id;
					}
				}
				return bufferReadValid;
			}
		}

		// Token: 0x02000628 RID: 1576
		// (Invoke) Token: 0x060040A8 RID: 16552
		public delegate void OnHitTakenAndDestroyedDelegate(DestructableComponent target, Agent attackerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage);

		// Token: 0x02000629 RID: 1577
		// (Invoke) Token: 0x060040AC RID: 16556
		public delegate void OnHitTakenWithImpactDelegate(DestructableComponent target, Agent attackerAgent, Vec3 impactPosition, Vec3 impactDirection, int inflictedDamage);
	}
}
