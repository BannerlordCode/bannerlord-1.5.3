using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000358 RID: 856
	public abstract class RangedSiegeWeapon : SiegeWeapon
	{
		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x000BFDDD File Offset: 0x000BDFDD
		public virtual string MultipleFireProjectileId
		{
			get
			{
				return "grapeshot_fire_stack";
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06003076 RID: 12406 RVA: 0x000BFDE4 File Offset: 0x000BDFE4
		public virtual string MultipleFireProjectileFlyingId
		{
			get
			{
				return "grapeshot_fire_projectile";
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003077 RID: 12407 RVA: 0x000BFDEB File Offset: 0x000BDFEB
		public virtual string MultipleProjectileId
		{
			get
			{
				return "grapeshot_stack";
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06003078 RID: 12408 RVA: 0x000BFDF2 File Offset: 0x000BDFF2
		public virtual string MultipleProjectileFlyingId
		{
			get
			{
				return "grapeshot_projectile";
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06003079 RID: 12409 RVA: 0x000BFDF9 File Offset: 0x000BDFF9
		public virtual string SingleFireProjectileId
		{
			get
			{
				return "pot";
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x0600307A RID: 12410 RVA: 0x000BFE00 File Offset: 0x000BE000
		public virtual string SingleFireProjectileFlyingId
		{
			get
			{
				return "pot_projectile";
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x0600307B RID: 12411 RVA: 0x000BFE07 File Offset: 0x000BE007
		public virtual string SingleProjectileId
		{
			get
			{
				return "boulder";
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x0600307C RID: 12412 RVA: 0x000BFE0E File Offset: 0x000BE00E
		public virtual string SingleProjectileFlyingId
		{
			get
			{
				return "boulder_projectile";
			}
		}

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x0600307D RID: 12413 RVA: 0x000BFE18 File Offset: 0x000BE018
		// (remove) Token: 0x0600307E RID: 12414 RVA: 0x000BFE50 File Offset: 0x000BE050
		public event Action<RangedSiegeWeapon, Agent> OnAgentLoadsMachine;

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x0600307F RID: 12415 RVA: 0x000BFE85 File Offset: 0x000BE085
		// (set) Token: 0x06003080 RID: 12416 RVA: 0x000BFE8D File Offset: 0x000BE08D
		public RangedSiegeWeapon.WeaponState State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state != value)
				{
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetRangedSiegeWeaponState(base.Id, value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					}
					this._state = value;
					this.OnRangedSiegeWeaponStateChange();
				}
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003081 RID: 12417 RVA: 0x000BFECA File Offset: 0x000BE0CA
		protected virtual float MaximumBallisticError
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003082 RID: 12418
		protected abstract float ShootingSpeed { get; }

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003083 RID: 12419 RVA: 0x000BFED1 File Offset: 0x000BE0D1
		public virtual Vec3 CanShootAtPointCheckingOffset
		{
			get
			{
				return Vec3.Zero;
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x000BFED8 File Offset: 0x000BE0D8
		// (set) Token: 0x06003085 RID: 12421 RVA: 0x000BFEE0 File Offset: 0x000BE0E0
		public GameEntity CameraHolder { get; private set; }

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x000BFEE9 File Offset: 0x000BE0E9
		// (set) Token: 0x06003087 RID: 12423 RVA: 0x000BFEF1 File Offset: 0x000BE0F1
		private protected SynchedMissionObject Projectile { protected get; private set; }

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x000BFEFC File Offset: 0x000BE0FC
		protected Vec3 MissileStartingGlobalPositionForSimulation
		{
			get
			{
				if (this.MissileStartingPositionEntityForSimulation != null)
				{
					return this.MissileStartingPositionEntityForSimulation.GlobalPosition;
				}
				SynchedMissionObject projectile = this.Projectile;
				if (projectile == null)
				{
					return Vec3.Zero;
				}
				return projectile.GameEntity.GlobalPosition;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (set) Token: 0x06003089 RID: 12425 RVA: 0x000BFF40 File Offset: 0x000BE140
		protected string SkeletonName
		{
			set
			{
				this.SkeletonNames = new string[] { value };
			}
		}

		// Token: 0x17000910 RID: 2320
		// (set) Token: 0x0600308A RID: 12426 RVA: 0x000BFF52 File Offset: 0x000BE152
		protected string FireAnimation
		{
			set
			{
				this.FireAnimations = new string[] { value };
			}
		}

		// Token: 0x17000911 RID: 2321
		// (set) Token: 0x0600308B RID: 12427 RVA: 0x000BFF64 File Offset: 0x000BE164
		protected string SetUpAnimation
		{
			set
			{
				this.SetUpAnimations = new string[] { value };
			}
		}

		// Token: 0x17000912 RID: 2322
		// (set) Token: 0x0600308C RID: 12428 RVA: 0x000BFF76 File Offset: 0x000BE176
		protected int FireAnimationIndex
		{
			set
			{
				this.FireAnimationIndices = new int[] { value };
			}
		}

		// Token: 0x17000913 RID: 2323
		// (set) Token: 0x0600308D RID: 12429 RVA: 0x000BFF88 File Offset: 0x000BE188
		protected int SetUpAnimationIndex
		{
			set
			{
				this.SetUpAnimationIndices = new int[] { value };
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x000BFF9A File Offset: 0x000BE19A
		// (set) Token: 0x0600308F RID: 12431 RVA: 0x000BFFA2 File Offset: 0x000BE1A2
		protected ItemObject LoadedMissileItem
		{
			get
			{
				return this._loadedMissileItem;
			}
			set
			{
				this._loadedMissileItem = value;
				this.OnLoadedMissileItemChanged();
			}
		}

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06003090 RID: 12432 RVA: 0x000BFFB4 File Offset: 0x000BE1B4
		// (remove) Token: 0x06003091 RID: 12433 RVA: 0x000BFFEC File Offset: 0x000BE1EC
		public event RangedSiegeWeapon.OnSiegeWeaponReloadDone OnReloadDone;

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06003092 RID: 12434 RVA: 0x000C0021 File Offset: 0x000BE221
		protected virtual bool WeaponMovesDownToReload
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06003093 RID: 12435 RVA: 0x000C0024 File Offset: 0x000BE224
		// (set) Token: 0x06003094 RID: 12436 RVA: 0x000C002C File Offset: 0x000BE22C
		public int AmmoCount
		{
			get
			{
				return this.CurrentAmmo;
			}
			protected set
			{
				this.CurrentAmmo = value;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06003095 RID: 12437 RVA: 0x000C0035 File Offset: 0x000BE235
		// (set) Token: 0x06003096 RID: 12438 RVA: 0x000C003D File Offset: 0x000BE23D
		protected virtual bool HasAmmo { get; set; } = true;

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003097 RID: 12439 RVA: 0x000C0046 File Offset: 0x000BE246
		public virtual float DirectionRestriction
		{
			get
			{
				return 2.0943952f;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06003098 RID: 12440 RVA: 0x000C004D File Offset: 0x000BE24D
		protected virtual float HorizontalAimSensitivity
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06003099 RID: 12441 RVA: 0x000C0054 File Offset: 0x000BE254
		protected virtual float VerticalAimSensitivity
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x0600309A RID: 12442 RVA: 0x000C005B File Offset: 0x000BE25B
		protected virtual float ReloadSpeedMultiplier
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x0600309B RID: 12443 RVA: 0x000C0062 File Offset: 0x000BE262
		// (set) Token: 0x0600309C RID: 12444 RVA: 0x000C006A File Offset: 0x000BE26A
		public bool PlayerForceUse { get; private set; }

		// Token: 0x0600309D RID: 12445
		protected abstract void RegisterAnimationParameters();

		// Token: 0x0600309E RID: 12446
		protected abstract void GetSoundEventIndices();

		// Token: 0x0600309F RID: 12447 RVA: 0x000C0074 File Offset: 0x000BE274
		protected virtual void ConsumeAmmo()
		{
			int ammoCount = this.AmmoCount;
			this.AmmoCount = ammoCount - 1;
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetRangedSiegeWeaponAmmo(base.Id, this.AmmoCount));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			this.UpdateAmmoMesh();
			this.CheckAmmo();
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x000C00C7 File Offset: 0x000BE2C7
		public virtual void SetAmmo(int ammoLeft)
		{
			if (this.AmmoCount != ammoLeft)
			{
				this.AmmoCount = ammoLeft;
				this.UpdateAmmoMesh();
				this.CheckAmmo();
			}
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000C00E5 File Offset: 0x000BE2E5
		public virtual void SetStartAmmo(int ammoLeft)
		{
			if (this.AmmoCount != ammoLeft)
			{
				this.AmmoCount = ammoLeft;
				this.UpdateAmmoMesh();
				this.CheckAmmo();
			}
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x000C0104 File Offset: 0x000BE304
		protected virtual void CheckAmmo()
		{
			if (this.AmmoCount <= 0 && this.StartingAmmoCount > 0)
			{
				this.HasAmmo = false;
				base.SetForcedUse(false);
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					standingPoint.IsDeactivated = true;
				}
			}
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x000C0178 File Offset: 0x000BE378
		protected void ChangeProjectileEntityServer(Agent loadingAgent, string missileItemID)
		{
			List<SynchedMissionObject> list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("projectile");
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].GameEntity.HasTag(missileItemID))
				{
					this.Projectile = list[i];
					this._projectileIndex = i;
					break;
				}
			}
			this.LoadedMissileItem = Game.Current.ObjectManager.GetObject<ItemObject>(missileItemID);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new RangedSiegeWeaponChangeProjectile(base.Id, this._projectileIndex));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			Action<RangedSiegeWeapon, Agent> onAgentLoadsMachine = this.OnAgentLoadsMachine;
			if (onAgentLoadsMachine == null)
			{
				return;
			}
			onAgentLoadsMachine(this, loadingAgent);
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x000C0228 File Offset: 0x000BE428
		public void ChangeProjectileEntityClient(int index)
		{
			List<SynchedMissionObject> list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("projectile");
			this.Projectile = list[index];
			this._projectileIndex = index;
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x000C025C File Offset: 0x000BE45C
		protected internal override void OnInit()
		{
			base.OnInit();
			this.DetermineDefaultBattleSide();
			this.ReleaseAngleRestrictionCenter = (this.TopReleaseAngleRestriction + this.BottomReleaseAngleRestriction) * 0.5f;
			this.ReleaseAngleRestrictionAngle = this.TopReleaseAngleRestriction - this.BottomReleaseAngleRestriction;
			this.CurrentReleaseAngle = (this._lastSyncedReleaseAngle = this.ReleaseAngleRestrictionCenter);
			this.OriginalMissileItem = Game.Current.ObjectManager.GetObject<ItemObject>(this.MissileItemID);
			this._projectileRadiusCached = -1f;
			this.LoadedMissileItem = this.OriginalMissileItem;
			this.OriginalMissileWeaponStatsDataForTargeting = new MissionWeapon(this.OriginalMissileItem, null, null).GetWeaponStatsDataForUsage(0);
			if (this.RotationObject == null)
			{
				this.RotationObject = this;
			}
			this._rotationObjectInitialFrame = this.RotationObject.GameEntity.GetFrame();
			this.CurrentDirection = (this._lastSyncedDirection = 0f);
			this._syncTimer = 0f;
			List<WeakGameEntity> list = base.GameEntity.CollectChildrenEntitiesWithTag("cameraHolder");
			if (list.Count > 0)
			{
				this.CameraHolder = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(list[0]);
				this._cameraHolderInitialFrame = this.CameraHolder.GetFrame();
				if (GameNetwork.IsClientOrReplay)
				{
					this.MakeVisibilityCheck = false;
				}
			}
			List<SynchedMissionObject> list2 = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("projectile");
			foreach (SynchedMissionObject synchedMissionObject in list2)
			{
				synchedMissionObject.GameEntity.SetVisibilityExcludeParents(false);
			}
			this.Projectile = list2.FirstOrDefault<SynchedMissionObject>((SynchedMissionObject x) => x.GameEntity.HasTag(this.MissileItemID));
			this._projectileIndex = list2.IndexOf(this.Projectile);
			this.Projectile.GameEntity.SetVisibilityExcludeParents(true);
			WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == "clean");
			if (weakGameEntity.IsValid)
			{
				weakGameEntity = weakGameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == "projectile_leaving_position");
			}
			this.MissileStartingPositionEntityForSimulation = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity);
			this.TargetDirection = this.CurrentDirection;
			this.TargetReleaseAngle = this.CurrentReleaseAngle;
			this.CanPickUpAmmoStandingPoints = new List<StandingPoint>();
			this.ReloadStandingPoints = new List<StandingPoint>();
			if (base.StandingPoints != null)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					standingPoint.AddComponent(new ResetAnimationOnStopUsageComponent(ActionIndexCache.act_none, false));
					if (standingPoint.GameEntity.HasTag("reload"))
					{
						this.ReloadStandingPoints.Add(standingPoint);
					}
					if (standingPoint.GameEntity.HasTag("can_pick_up_ammo"))
					{
						this.CanPickUpAmmoStandingPoints.Add(standingPoint);
					}
				}
			}
			List<StandingPointWithWeaponRequirement> list3 = base.StandingPoints.OfType<StandingPointWithWeaponRequirement>().ToList<StandingPointWithWeaponRequirement>();
			List<StandingPointWithWeaponRequirement> list4 = new List<StandingPointWithWeaponRequirement>();
			foreach (StandingPointWithWeaponRequirement standingPointWithWeaponRequirement in list3)
			{
				if (standingPointWithWeaponRequirement.GameEntity.HasTag(this.AmmoPickUpTag))
				{
					standingPointWithWeaponRequirement.InitGivenWeapon(this.OriginalMissileItem);
					standingPointWithWeaponRequirement.SetupOnUsingStoppedBehavior(false, new Action<Agent, bool>(this.OnAmmoPickupUsingCancelled));
				}
				else
				{
					list4.Add(standingPointWithWeaponRequirement);
					standingPointWithWeaponRequirement.SetupOnUsingStoppedBehavior(false, new Action<Agent, bool>(this.OnLoadingAmmoPointUsingCancelled));
					standingPointWithWeaponRequirement.InitRequiredWeaponClasses(new WeaponClass[] { this.OriginalMissileItem.PrimaryWeapon.WeaponClass });
				}
			}
			if (base.AmmoPickUpPoints.Count > 1)
			{
				this._ammoPickupCenter = default(Vec3);
				foreach (StandingPoint standingPoint2 in base.AmmoPickUpPoints)
				{
					((StandingPointWithWeaponRequirement)standingPoint2).SetHasAlternative(true);
					this._ammoPickupCenter += standingPoint2.GameEntity.GlobalPosition;
				}
				this._ammoPickupCenter /= (float)base.AmmoPickUpPoints.Count;
			}
			else
			{
				this._ammoPickupCenter = base.GameEntity.GlobalPosition;
			}
			list4.Sort(delegate(StandingPointWithWeaponRequirement element1, StandingPointWithWeaponRequirement element2)
			{
				if (element1.GameEntity.GlobalPosition.DistanceSquared(this._ammoPickupCenter) > element2.GameEntity.GlobalPosition.DistanceSquared(this._ammoPickupCenter))
				{
					return 1;
				}
				if (element1.GameEntity.GlobalPosition.DistanceSquared(this._ammoPickupCenter) < element2.GameEntity.GlobalPosition.DistanceSquared(this._ammoPickupCenter))
				{
					return -1;
				}
				return 0;
			});
			this.LoadAmmoStandingPoint = list4.FirstOrDefault<StandingPointWithWeaponRequirement>();
			this.SortCanPickUpAmmoStandingPoints();
			Vec3 vec = base.PilotStandingPoint.GameEntity.GlobalPosition - base.GameEntity.GlobalPosition;
			foreach (StandingPoint standingPoint3 in this.CanPickUpAmmoStandingPoints)
			{
				if (standingPoint3 != base.PilotStandingPoint)
				{
					float length = (standingPoint3.GameEntity.GlobalPosition - base.GameEntity.GlobalPosition + vec).Length;
					this.PilotReservePriorityValues.Add(standingPoint3, length);
				}
			}
			this.AmmoCount = MathF.Max(0, this.StartingAmmoCount - 1);
			this.UpdateAmmoMesh();
			this.RegisterAnimationParameters();
			this.GetSoundEventIndices();
			this.InitAnimations();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x000C0818 File Offset: 0x000BEA18
		protected virtual void DetermineDefaultBattleSide()
		{
			DestructableComponent firstScriptOfType = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			this.DefaultSide = firstScriptOfType.BattleSide;
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x000C0840 File Offset: 0x000BEA40
		private void SortCanPickUpAmmoStandingPoints()
		{
			if (MBMath.GetSmallestDifferenceBetweenTwoAngles(this._lastCanPickUpAmmoStandingPointsSortedAngle, this.CurrentDirection) > 0.18849556f)
			{
				this._lastCanPickUpAmmoStandingPointsSortedAngle = this.CurrentDirection;
				int signOfAmmoPile = Math.Sign(Vec3.DotProduct(base.GameEntity.GetGlobalFrame().rotation.s, this._ammoPickupCenter - base.GameEntity.GlobalPosition));
				this.CanPickUpAmmoStandingPoints.Sort(delegate(StandingPoint element1, StandingPoint element2)
				{
					Vec3 vec = this._ammoPickupCenter - element1.GameEntity.GlobalPosition;
					Vec3 vec2 = this._ammoPickupCenter - element2.GameEntity.GlobalPosition;
					float num = vec.LengthSquared;
					float num2 = vec2.LengthSquared;
					float num3 = Vec3.DotProduct(this.GameEntity.GetGlobalFrame().rotation.s, element1.GameEntity.GlobalPosition - this.GameEntity.GlobalPosition);
					float num4 = Vec3.DotProduct(this.GameEntity.GetGlobalFrame().rotation.s, element2.GameEntity.GlobalPosition - this.GameEntity.GlobalPosition);
					if (!element1.GameEntity.HasTag("no_ammo_pick_up_penalty") && signOfAmmoPile != Math.Sign(num3))
					{
						num += num3 * num3 * 64f;
					}
					if (!element2.GameEntity.HasTag("no_ammo_pick_up_penalty") && signOfAmmoPile != Math.Sign(num4))
					{
						num2 += num4 * num4 * 64f;
					}
					if (element1.GameEntity.HasTag(this.PilotStandingPointTag))
					{
						num += 25f;
					}
					else if (element2.GameEntity.HasTag(this.PilotStandingPointTag))
					{
						num2 += 25f;
					}
					if (num > num2)
					{
						return 1;
					}
					if (num < num2)
					{
						return -1;
					}
					return 0;
				});
			}
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x000C08D8 File Offset: 0x000BEAD8
		protected internal override void OnEditorInit()
		{
			List<SynchedMissionObject> list = base.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>("projectile");
			if (list.Count > 0)
			{
				this.Projectile = list[0];
			}
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x000C090C File Offset: 0x000BEB0C
		private void InitAnimations()
		{
			for (int i = 0; i < this.Skeletons.Length; i++)
			{
				this.Skeletons[i].SetAnimationAtChannel(this.SetUpAnimations[i], 0, 1f, 0f, 0f);
				this.Skeletons[i].SetAnimationParameterAtChannel(0, 1f);
				this.Skeletons[i].TickAnimations(0.0001f, MatrixFrame.Identity, true);
			}
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x000C097C File Offset: 0x000BEB7C
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this.Projectile.GameEntity.SetVisibilityExcludeParents(true);
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				Agent userAgent = standingPoint.UserAgent;
				if (userAgent != null)
				{
					userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				standingPoint.IsDeactivated = false;
			}
			this._state = RangedSiegeWeapon.WeaponState.Idle;
			this.CurrentDirection = (this._lastSyncedDirection = 0f);
			this._syncTimer = 0f;
			this.CurrentReleaseAngle = (this._lastSyncedReleaseAngle = this.ReleaseAngleRestrictionCenter);
			this.TargetDirection = this.CurrentDirection;
			this.TargetReleaseAngle = this.CurrentReleaseAngle;
			this.ApplyCurrentDirectionToEntity();
			this.AmmoCount = MathF.Max(0, this.StartingAmmoCount - 1);
			this.UpdateAmmoMesh();
			if (this.MoveSound != null)
			{
				this.MoveSound.Stop();
				this.MoveSound = null;
			}
			this._hasFrameChangedInPreviousFrame = false;
			Skeleton[] skeletons = this.Skeletons;
			for (int i = 0; i < skeletons.Length; i++)
			{
				skeletons[i].Freeze(false);
			}
			foreach (StandingPoint standingPoint2 in base.AmmoPickUpPoints)
			{
				standingPoint2.IsDeactivated = false;
			}
			this.InitAnimations();
			this.UpdateProjectilePosition();
			if (!GameNetwork.IsClientOrReplay)
			{
				this.SetActivationLoadAmmoPoint(false);
			}
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x000C0B0C File Offset: 0x000BED0C
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.RangedSiegeWeaponStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.TargetDirection, CompressionBasic.RadianCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.TargetReleaseAngle, CompressionBasic.RadianCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.AmmoCount, CompressionMission.RangedSiegeWeaponAmmoCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this._projectileIndex, CompressionMission.RangedSiegeWeaponAmmoIndexCompressionInfo);
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x000C0B6F File Offset: 0x000BED6F
		protected virtual void UpdateProjectilePosition()
		{
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x000C0B74 File Offset: 0x000BED74
		public override bool IsInRangeToCheckAlternativePoints(Agent agent)
		{
			float num = ((base.AmmoPickUpPoints.Count > 0) ? (agent.GetInteractionDistanceToUsable(base.AmmoPickUpPoints[0]) + 2f) : 2f);
			return this._ammoPickupCenter.DistanceSquared(agent.Position) < num * num;
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x000C0BC8 File Offset: 0x000BEDC8
		public override StandingPoint GetBestPointAlternativeTo(StandingPoint standingPoint, Agent agent)
		{
			if (base.AmmoPickUpPoints.Contains(standingPoint))
			{
				IEnumerable<StandingPoint> enumerable = base.AmmoPickUpPoints.Where<StandingPoint>((StandingPoint sp) => !sp.IsDeactivated && (sp.IsInstantUse || (!sp.HasUser && !sp.HasAIMovingTo)) && !sp.IsDisabledForAgent(agent));
				float num = standingPoint.GameEntity.GlobalPosition.DistanceSquared(agent.Position);
				StandingPoint standingPoint2 = standingPoint;
				foreach (StandingPoint standingPoint3 in enumerable)
				{
					float num2 = standingPoint3.GameEntity.GlobalPosition.DistanceSquared(agent.Position);
					if (num2 < num)
					{
						num = num2;
						standingPoint2 = standingPoint3;
					}
				}
				return standingPoint2;
			}
			return standingPoint;
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x000C0C9C File Offset: 0x000BEE9C
		protected virtual void OnRangedSiegeWeaponStateChange()
		{
			switch (this.State)
			{
			case RangedSiegeWeapon.WeaponState.Idle:
			case RangedSiegeWeapon.WeaponState.WaitingBeforeIdle:
				this._cameraState = ((this._cameraState == RangedSiegeWeapon.CameraState.FreeMove) ? RangedSiegeWeapon.CameraState.ApproachToCamera : RangedSiegeWeapon.CameraState.StickToWeapon);
				break;
			case RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving:
				this.AttackClickWillReload = this.WeaponNeedsClickToReload;
				if (!GameNetwork.IsDedicatedServer)
				{
					SoundManager.StartOneShotEventWithIndex(this.FireSoundIndex, in base.GameEntity.GetGlobalFrame().origin);
				}
				break;
			case RangedSiegeWeapon.WeaponState.Shooting:
				if (this.CameraHolder != null)
				{
					this._cameraState = RangedSiegeWeapon.CameraState.DoNotMove;
					this.DontMoveTimer = 0.35f;
				}
				break;
			case RangedSiegeWeapon.WeaponState.WaitingAfterShooting:
				this.AttackClickWillReload = this.WeaponNeedsClickToReload;
				this.CheckAmmo();
				break;
			case RangedSiegeWeapon.WeaponState.WaitingBeforeReloading:
				this.AttackClickWillReload = false;
				if (this.CameraHolder != null && this.WeaponMovesDownToReload)
				{
					this._cameraState = RangedSiegeWeapon.CameraState.MoveDownToReload;
				}
				this.CheckAmmo();
				break;
			case RangedSiegeWeapon.WeaponState.LoadingAmmo:
				if (this.ReloadSound != null && this.ReloadSound.IsValid)
				{
					this.ReloadSound.Stop();
				}
				this.ReloadSound = null;
				break;
			case RangedSiegeWeapon.WeaponState.Reloading:
				if (this.ReloadSound != null && this.ReloadSound.IsValid)
				{
					if (this.ReloadSound.IsPaused())
					{
						this.ReloadSound.Resume();
					}
					else
					{
						this.ReloadSound.PlayInPosition(base.GameEntity.GetGlobalFrame().origin);
					}
				}
				else
				{
					this.ReloadSound = SoundEvent.CreateEvent(this.ReloadSoundIndex, base.Scene);
					this.ReloadSound.PlayInPosition(base.GameEntity.GetGlobalFrame().origin);
				}
				break;
			case RangedSiegeWeapon.WeaponState.ReloadingPaused:
				if (this.ReloadSound != null && this.ReloadSound.IsValid)
				{
					this.ReloadSound.Pause();
				}
				break;
			default:
				Debug.FailedAssert("Invalid WeaponState.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\RangedSiegeWeapon.cs", "OnRangedSiegeWeaponStateChange", 894);
				break;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				switch (this.State)
				{
				case RangedSiegeWeapon.WeaponState.Idle:
				case RangedSiegeWeapon.WeaponState.WaitingAfterShooting:
				case RangedSiegeWeapon.WeaponState.WaitingBeforeReloading:
					break;
				case RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving:
				{
					for (int i = 0; i < this.SkeletonOwnerObjects.Length; i++)
					{
						this.SkeletonOwnerObjects[i].SetAnimationAtChannelSynched(this.FireAnimations[i], 0, 1f);
					}
					return;
				}
				case RangedSiegeWeapon.WeaponState.Shooting:
					this.ShootProjectile();
					return;
				case RangedSiegeWeapon.WeaponState.LoadingAmmo:
					this.SetActivationLoadAmmoPoint(true);
					this.ReloaderAgent = null;
					return;
				case RangedSiegeWeapon.WeaponState.WaitingBeforeIdle:
					this.SendReloaderAgentToOriginalPoint();
					this.SetActivationLoadAmmoPoint(false);
					return;
				case RangedSiegeWeapon.WeaponState.Reloading:
				{
					for (int j = 0; j < this.SkeletonOwnerObjects.Length; j++)
					{
						if (this.SkeletonOwnerObjects[j].GameEntity.IsSkeletonAnimationPaused())
						{
							this.SkeletonOwnerObjects[j].ResumeSkeletonAnimationSynched();
						}
						else
						{
							this.SkeletonOwnerObjects[j].SetAnimationAtChannelSynched(this.SetUpAnimations[j], 0, 1f);
						}
					}
					this._currentReloaderCount = 1;
					return;
				}
				case RangedSiegeWeapon.WeaponState.ReloadingPaused:
				{
					SynchedMissionObject[] skeletonOwnerObjects = this.SkeletonOwnerObjects;
					for (int k = 0; k < skeletonOwnerObjects.Length; k++)
					{
						skeletonOwnerObjects[k].PauseSkeletonAnimationSynched();
					}
					return;
				}
				default:
					Debug.FailedAssert("Invalid WeaponState.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\RangedSiegeWeapon.cs", "OnRangedSiegeWeaponStateChange", 970);
					break;
				}
			}
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x000C0FCB File Offset: 0x000BF1CB
		protected virtual void SetActivationLoadAmmoPoint(bool activate)
		{
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x000C0FCD File Offset: 0x000BF1CD
		protected override float GetDetachmentWeightAux(BattleSideEnum side)
		{
			if (this.HasAmmo)
			{
				return base.GetDetachmentWeightAux(side);
			}
			return float.MinValue;
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x000C0FE4 File Offset: 0x000BF1E4
		protected float GetDetachmentWeightAuxForExternalAmmoWeapons(BattleSideEnum side)
		{
			if (this.IsDisabledForBattleSideAI(side))
			{
				return float.MinValue;
			}
			this.UsableStandingPoints.Clear();
			bool flag = false;
			bool flag2 = false;
			bool flag3 = !base.PilotStandingPoint.HasUser && !base.PilotStandingPoint.HasAIMovingTo && (this.ReloaderAgent == null || this.ReloaderAgentOriginalPoint != base.PilotStandingPoint);
			int num = -1;
			StandingPoint standingPoint = null;
			bool flag4 = false;
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				StandingPoint standingPoint2 = base.StandingPoints[i];
				if (standingPoint2.GameEntity.HasTag("can_pick_up_ammo"))
				{
					if (this.ReloaderAgent == null || standingPoint2 != this.ReloaderAgentOriginalPoint)
					{
						if (standingPoint2.IsUsableBySide(side))
						{
							if (!standingPoint2.HasAIMovingTo)
							{
								if (!flag2)
								{
									this.UsableStandingPoints.Clear();
									num = -1;
								}
								flag2 = true;
							}
							else if (flag2 || standingPoint2.MovingAgent.Formation.Team.Side != side)
							{
								goto IL_016A;
							}
							flag = true;
							this.UsableStandingPoints.Add(new ValueTuple<int, StandingPoint>(i, standingPoint2));
							if (flag3 && base.PilotStandingPoint == standingPoint2)
							{
								num = this.UsableStandingPoints.Count - 1;
							}
						}
						else if (flag3 && standingPoint2.HasAIUser && (standingPoint == null || this.PilotReservePriorityValues[standingPoint2] > this.PilotReservePriorityValues[standingPoint] || flag4))
						{
							standingPoint = standingPoint2;
							flag4 = false;
						}
					}
					else if (flag3 && standingPoint == null)
					{
						standingPoint = standingPoint2;
						flag4 = true;
					}
				}
				IL_016A:;
			}
			if (standingPoint != null)
			{
				if (flag4)
				{
					this.ReloaderAgentOriginalPoint = base.PilotStandingPoint;
				}
				else
				{
					Agent userAgent = standingPoint.UserAgent;
					userAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.DoNotWieldWeaponAfterStoppingUsingGameObject);
					userAgent.AIMoveToGameObjectEnable(base.PilotStandingPoint, this, base.Ai.GetScriptedFrameFlags(userAgent));
				}
				if (num != -1)
				{
					this.UsableStandingPoints.RemoveAt(num);
				}
			}
			this.AreUsableStandingPointsVacant = flag2;
			if (!flag)
			{
				return float.MinValue;
			}
			if (flag2)
			{
				return 1f;
			}
			if (base.IsDetachmentRecentlyEvaluated)
			{
				return 0.01f;
			}
			return 0.1f;
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x000C11F4 File Offset: 0x000BF3F4
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x000C1220 File Offset: 0x000BF420
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				this.UpdateState(dt);
				if (base.PilotAgent != null && !base.PilotAgent.IsInBeingStruckAction)
				{
					if (base.PilotAgent.MovementFlags.HasAnyFlag(Agent.MovementControlFlag.AttackMask))
					{
						if (this.State == RangedSiegeWeapon.WeaponState.Idle)
						{
							this._aiRequestsShoot = false;
							this.Shoot();
						}
						else if (this.State == RangedSiegeWeapon.WeaponState.WaitingAfterShooting && this.AttackClickWillReload)
						{
							this._aiRequestsManualReload = false;
							this.ManualReload();
						}
					}
					if (this._aiRequestsManualReload)
					{
						this.ManualReload();
					}
					if (this._aiRequestsShoot)
					{
						this.Shoot();
					}
				}
				this._aiRequestsShoot = false;
				this._aiRequestsManualReload = false;
			}
			this.HandleUserAiming(dt);
		}

		// Token: 0x060030B5 RID: 12469 RVA: 0x000C12EC File Offset: 0x000BF4EC
		protected static bool ApproachToAngle(ref float angle, float angleToApproach, bool isMouse, float speed_limit, float dt, float sensitivity)
		{
			speed_limit = MathF.Abs(speed_limit);
			if (angle != angleToApproach)
			{
				float num = sensitivity * dt;
				float num2 = MathF.Abs(angle - angleToApproach);
				if (isMouse)
				{
					num *= MathF.Max(num2 * 8f, 0.15f);
				}
				if (speed_limit > 0f)
				{
					num = MathF.Min(num, speed_limit * dt);
				}
				if (num2 <= num)
				{
					angle = angleToApproach;
				}
				else
				{
					angle += num * (float)MathF.Sign(angleToApproach - angle);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x000C1360 File Offset: 0x000BF560
		protected virtual void HandleUserAiming(float dt)
		{
			bool flag = false;
			float horizontalAimSensitivity = this.HorizontalAimSensitivity;
			float verticalAimSensitivity = this.VerticalAimSensitivity;
			bool flag2 = false;
			if (this._cameraState != RangedSiegeWeapon.CameraState.DoNotMove)
			{
				if (this._inputGiven)
				{
					flag2 = true;
					if (this.CanRotate())
					{
						if (this._inputX != 0f)
						{
							this.TargetDirection += horizontalAimSensitivity * dt * this._inputX;
							this.TargetDirection = MBMath.WrapAngle(this.TargetDirection);
							this.TargetDirection = MBMath.ClampAngle(this.TargetDirection, this.CurrentDirection, 0.7f);
							this.TargetDirection = MBMath.ClampAngle(this.TargetDirection, 0f, this.DirectionRestriction);
						}
						if (this._inputY != 0f)
						{
							this.TargetReleaseAngle += verticalAimSensitivity * dt * this._inputY;
							this.TargetReleaseAngle = MBMath.ClampAngle(this.TargetReleaseAngle, this.CurrentReleaseAngle + 0.049999997f, 0.6f);
							this.TargetReleaseAngle = MBMath.ClampAngle(this.TargetReleaseAngle, this.ReleaseAngleRestrictionCenter, this.ReleaseAngleRestrictionAngle);
						}
					}
					this._inputGiven = false;
					this._inputX = 0f;
					this._inputY = 0f;
				}
				else if (this._exactInputGiven)
				{
					bool flag3 = false;
					if (this.CanRotate())
					{
						if (this.TargetDirection != this._inputTargetX)
						{
							float num = horizontalAimSensitivity * dt;
							if (MathF.Abs(this.TargetDirection - this._inputTargetX) < num)
							{
								this.TargetDirection = this._inputTargetX;
							}
							else if (this.TargetDirection < this._inputTargetX)
							{
								this.TargetDirection += num;
								flag3 = true;
							}
							else
							{
								this.TargetDirection -= num;
								flag3 = true;
							}
							this.TargetDirection = MBMath.WrapAngle(this.TargetDirection);
							this.TargetDirection = MBMath.ClampAngle(this.TargetDirection, this.CurrentDirection, 0.7f);
							this.TargetDirection = MBMath.ClampAngle(this.TargetDirection, 0f, this.DirectionRestriction);
						}
						if (this.TargetReleaseAngle != this._inputTargetY)
						{
							float num2 = verticalAimSensitivity * dt;
							if (MathF.Abs(this.TargetReleaseAngle - this._inputTargetY) < num2)
							{
								this.TargetReleaseAngle = this._inputTargetY;
							}
							else if (this.TargetReleaseAngle < this._inputTargetY)
							{
								this.TargetReleaseAngle += num2;
								flag3 = true;
							}
							else
							{
								this.TargetReleaseAngle -= num2;
								flag3 = true;
							}
							this.TargetReleaseAngle = MBMath.ClampAngle(this.TargetReleaseAngle, this.CurrentReleaseAngle + 0.049999997f, 0.6f);
							this.TargetReleaseAngle = MBMath.ClampAngle(this.TargetReleaseAngle, this.ReleaseAngleRestrictionCenter, this.ReleaseAngleRestrictionAngle);
						}
					}
					else
					{
						flag3 = true;
					}
					if (!flag3)
					{
						this._exactInputGiven = false;
					}
				}
			}
			switch (this._cameraState)
			{
			case RangedSiegeWeapon.CameraState.StickToWeapon:
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CurrentDirection, this.TargetDirection, this.UsesMouseForAiming, -1f, dt, horizontalAimSensitivity);
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CurrentReleaseAngle, this.TargetReleaseAngle, this.UsesMouseForAiming, -1f, dt, verticalAimSensitivity) || flag;
				this.CameraDirection = this.CurrentDirection;
				this.CameraReleaseAngle = this.CurrentReleaseAngle;
				break;
			case RangedSiegeWeapon.CameraState.DoNotMove:
				this.DontMoveTimer -= dt;
				if (this.DontMoveTimer < 0f)
				{
					if (!this.AttackClickWillReload && this.WeaponMovesDownToReload)
					{
						this._cameraState = RangedSiegeWeapon.CameraState.MoveDownToReload;
						this.MaxRotateSpeed = 0f;
						this.ReloadTargetReleaseAngle = MBMath.ClampAngle((MathF.Abs(this.CurrentReleaseAngle) > 0.17453292f) ? 0f : this.CurrentReleaseAngle, this.CurrentReleaseAngle - 0.049999997f, 0.6f);
						this.TargetDirection = this.CameraDirection;
						this.CameraReleaseAngle = this.TargetReleaseAngle;
					}
					else
					{
						this._cameraState = RangedSiegeWeapon.CameraState.StickToWeapon;
					}
				}
				break;
			case RangedSiegeWeapon.CameraState.MoveDownToReload:
				this.MaxRotateSpeed += dt * 1.2f;
				this.MaxRotateSpeed = MathF.Min(this.MaxRotateSpeed, 1f);
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CurrentReleaseAngle, this.ReloadTargetReleaseAngle, this.UsesMouseForAiming, 0.4f + this.MaxRotateSpeed, dt, verticalAimSensitivity);
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraDirection, this.TargetDirection, this.UsesMouseForAiming, -1f, dt, horizontalAimSensitivity) || flag;
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraReleaseAngle, this.ReloadTargetReleaseAngle, this.UsesMouseForAiming, 0.5f + this.MaxRotateSpeed, dt, verticalAimSensitivity) || flag;
				if (!flag)
				{
					this._cameraState = RangedSiegeWeapon.CameraState.RememberLastShotDirection;
				}
				break;
			case RangedSiegeWeapon.CameraState.RememberLastShotDirection:
				if (this.State == RangedSiegeWeapon.WeaponState.Idle || flag2)
				{
					this._cameraState = RangedSiegeWeapon.CameraState.FreeMove;
					RangedSiegeWeapon.OnSiegeWeaponReloadDone onReloadDone = this.OnReloadDone;
					if (onReloadDone != null)
					{
						onReloadDone();
					}
				}
				break;
			case RangedSiegeWeapon.CameraState.FreeMove:
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraDirection, this.TargetDirection, this.UsesMouseForAiming, -1f, dt, horizontalAimSensitivity);
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraReleaseAngle, this.TargetReleaseAngle, this.UsesMouseForAiming, -1f, dt, verticalAimSensitivity) || flag;
				this.MaxRotateSpeed = 0f;
				break;
			case RangedSiegeWeapon.CameraState.ApproachToCamera:
				this.MaxRotateSpeed += 0.9f * dt + this.MaxRotateSpeed * 2f * dt;
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraDirection, this.TargetDirection, this.UsesMouseForAiming, -1f, dt, horizontalAimSensitivity);
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CameraReleaseAngle, this.TargetReleaseAngle, this.UsesMouseForAiming, -1f, dt, verticalAimSensitivity) || flag;
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CurrentDirection, this.TargetDirection, this.UsesMouseForAiming, this.MaxRotateSpeed, dt, horizontalAimSensitivity) || flag;
				flag = RangedSiegeWeapon.ApproachToAngle(ref this.CurrentReleaseAngle, this.TargetReleaseAngle, this.UsesMouseForAiming, this.MaxRotateSpeed, dt, verticalAimSensitivity) || flag;
				if (!flag)
				{
					this._cameraState = RangedSiegeWeapon.CameraState.StickToWeapon;
				}
				break;
			}
			if (this.CameraHolder != null)
			{
				MatrixFrame matrixFrame = this._cameraHolderInitialFrame;
				matrixFrame.rotation.RotateAboutForward(this.CameraDirection - this.CurrentDirection);
				matrixFrame.rotation.RotateAboutSide(this.CameraReleaseAngle - this.CurrentReleaseAngle);
				this.CameraHolder.SetFrame(ref matrixFrame, true);
				matrixFrame = this.CameraHolder.GetGlobalFrame();
				matrixFrame.rotation.s.z = 0f;
				matrixFrame.rotation.s.Normalize();
				matrixFrame.rotation.u = Vec3.CrossProduct(matrixFrame.rotation.s, matrixFrame.rotation.f);
				matrixFrame.rotation.u.Normalize();
				matrixFrame.rotation.f = Vec3.CrossProduct(matrixFrame.rotation.u, matrixFrame.rotation.s);
				matrixFrame.rotation.f.Normalize();
				if (base.PilotAgent == null)
				{
					this._cameraMoveBackFactor = ((1f - this._cameraMoveBackFactor > 1E-05f) ? MBMath.LerpFPSIndependent(this._cameraMoveBackFactor, 1f, dt * 8f) : 1f);
				}
				else
				{
					this._cameraMoveBackFactor = ((this._cameraMoveBackFactor > 1E-05f) ? MBMath.LerpFPSIndependent(this._cameraMoveBackFactor, 0f, dt * 8f) : 0f);
				}
				if (this._cameraMoveBackFactor > 0f)
				{
					matrixFrame.origin += matrixFrame.rotation.u * this._cameraMoveBackFactor * 3f + matrixFrame.rotation.f * this._cameraMoveBackFactor * 0.3f;
				}
				this.CameraHolder.SetGlobalFrame(in matrixFrame, true);
			}
			else
			{
				this._cameraMoveBackFactor = ((base.PilotAgent == null) ? 1f : 0f);
			}
			if (flag && !this._hasFrameChangedInPreviousFrame)
			{
				this.OnRotationStarted();
			}
			else if (!flag && this._hasFrameChangedInPreviousFrame)
			{
				this.OnRotationStopped();
			}
			this._hasFrameChangedInPreviousFrame = flag;
			if ((flag && GameNetwork.IsClient && base.PilotAgent == Agent.Main) || GameNetwork.IsServerOrRecorder)
			{
				float num3 = ((GameNetwork.IsClient && base.PilotAgent == Agent.Main) ? 0.0001f : 0.02f);
				if (this._syncTimer > 0.2f && (MathF.Abs(this.CurrentDirection - this._lastSyncedDirection) > num3 || MathF.Abs(this.CurrentReleaseAngle - this._lastSyncedReleaseAngle) > num3))
				{
					this._lastSyncedDirection = this.CurrentDirection;
					this._lastSyncedReleaseAngle = this.CurrentReleaseAngle;
					MissionLobbyComponent missionBehavior = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
					if ((missionBehavior == null || missionBehavior.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending) && GameNetwork.IsClient && base.PilotAgent == Agent.Main)
					{
						GameNetwork.BeginModuleEventAsClient();
						GameNetwork.WriteMessage(new SetMachineRotation(base.Id, this.CurrentDirection, this.CurrentReleaseAngle));
						GameNetwork.EndModuleEventAsClient();
					}
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetMachineTargetRotation(base.Id, this.CurrentDirection, this.CurrentReleaseAngle));
						GameNetwork.EventBroadcastFlags eventBroadcastFlags = GameNetwork.EventBroadcastFlags.ExcludeTargetPlayer | GameNetwork.EventBroadcastFlags.AddToMissionRecord;
						Agent pilotAgent = base.PilotAgent;
						NetworkCommunicator networkCommunicator;
						if (pilotAgent == null)
						{
							networkCommunicator = null;
						}
						else
						{
							MissionPeer missionPeer = pilotAgent.MissionPeer;
							networkCommunicator = ((missionPeer != null) ? missionPeer.GetNetworkPeer() : null);
						}
						GameNetwork.EndBroadcastModuleEvent(eventBroadcastFlags, networkCommunicator);
					}
				}
			}
			this._syncTimer += dt;
			if (this._syncTimer >= 1f)
			{
				this._syncTimer -= 1f;
			}
			if (flag)
			{
				this.ApplyAimChange();
			}
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x000C1CD8 File Offset: 0x000BFED8
		public void GiveInput(float inputX, float inputY)
		{
			this._exactInputGiven = false;
			this._inputGiven = true;
			this._inputX = inputX;
			this._inputY = inputY;
			this._inputX = MBMath.ClampFloat(this._inputX, -1f, 1f);
			this._inputY = MBMath.ClampFloat(this._inputY, -1f, 1f);
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x000C1D37 File Offset: 0x000BFF37
		public void GiveExactInput(float targetX, float targetY)
		{
			this._exactInputGiven = true;
			this._inputGiven = false;
			this._inputTargetX = MBMath.ClampAngle(targetX, 0f, this.DirectionRestriction);
			this._inputTargetY = MBMath.ClampAngle(targetY, this.ReleaseAngleRestrictionCenter, this.ReleaseAngleRestrictionAngle);
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x000C1D76 File Offset: 0x000BFF76
		protected virtual bool CanRotate()
		{
			return this.State == RangedSiegeWeapon.WeaponState.Idle;
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x000C1D81 File Offset: 0x000BFF81
		protected virtual void ApplyAimChange()
		{
			if (this.CanRotate())
			{
				this.ApplyCurrentDirectionToEntity();
				return;
			}
			this.TargetDirection = this.CurrentDirection;
			this.TargetReleaseAngle = this.CurrentReleaseAngle;
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x000C1DAC File Offset: 0x000BFFAC
		protected virtual void ApplyCurrentDirectionToEntity()
		{
			MatrixFrame rotationObjectInitialFrame = this._rotationObjectInitialFrame;
			rotationObjectInitialFrame.rotation.RotateAboutUp(this.CurrentDirection);
			this.RotationObject.GameEntity.SetFrame(ref rotationObjectInitialFrame, true);
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x000C1DE8 File Offset: 0x000BFFE8
		public virtual float GetTargetReleaseAngle(Vec3 target)
		{
			return Mission.GetMissileVerticalAimCorrection(target - this.MissileStartingGlobalPositionForSimulation, this.ShootingSpeed, ref this.OriginalMissileWeaponStatsDataForTargeting, ItemObject.GetAirFrictionConstant(this.OriginalMissileItem.PrimaryWeapon.WeaponClass, this.OriginalMissileItem.PrimaryWeapon.WeaponFlags));
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x000C1E38 File Offset: 0x000C0038
		private void CalculateLocalAnglesFromGlobalDirection(Vec3 globalDirection, out float localTargetDirection, out float localTargetAngle)
		{
			globalDirection.Normalize();
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			if (!globalFrame.rotation.IsUnit())
			{
				globalFrame.rotation.Orthonormalize();
			}
			globalFrame.rotation.RotateAboutAnArbitraryVector(in globalFrame.rotation.u, 3.1415927f);
			Vec3 vec = globalFrame.rotation.TransformToLocal(in globalDirection);
			localTargetDirection = vec.AsVec2.RotationInRadians;
			localTargetAngle = MathF.Atan2(vec.z, MathF.Sqrt(vec.x * vec.x + vec.y * vec.y));
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x000C1EE0 File Offset: 0x000C00E0
		private void CalculateLocalDirectionAndLocalAngleToShootTarget(Vec3 target, out float localTargetDirection, out float localTargetAngle)
		{
			float targetReleaseAngle = this.GetTargetReleaseAngle(target);
			if (targetReleaseAngle > 1.5707964f)
			{
				localTargetDirection = 3.1415927f;
				localTargetAngle = 3.1415927f;
				return;
			}
			Vec3 vec = new Vec3((target - this.MissileStartingGlobalPositionForSimulation).AsVec2, 0f, -1f).NormalizedCopy();
			vec *= MathF.Cos(targetReleaseAngle);
			vec += new Vec3(0f, 0f, MathF.Sin(targetReleaseAngle), -1f);
			vec.Normalize();
			Vec3 globalVelocity = this.GetGlobalVelocity();
			vec *= this.ShootingSpeed;
			vec -= new Vec3(globalVelocity.AsVec2, 0f, -1f);
			vec.Normalize();
			this.CalculateLocalAnglesFromGlobalDirection(vec, out localTargetDirection, out localTargetAngle);
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x000C1FB0 File Offset: 0x000C01B0
		public virtual bool AimAtThreat(Threat threat)
		{
			Vec3 estimatedTargetGlobalPoint = this.GetEstimatedTargetGlobalPoint(threat);
			return this.AimAtTarget(estimatedTargetGlobalPoint);
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x000C1FCC File Offset: 0x000C01CC
		public bool AimAtTarget(Vec3 target)
		{
			float num;
			float num2;
			this.CalculateLocalDirectionAndLocalAngleToShootTarget(target, out num, out num2);
			if (num >= 3.1415927f)
			{
				return false;
			}
			if (!this._exactInputGiven || num != this._inputTargetX || num2 != this._inputTargetY)
			{
				this.GiveExactInput(num, num2);
			}
			return this.CheckIsTargetReached(target);
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x000C2017 File Offset: 0x000C0217
		public virtual bool CheckIsTargetReached(Vec3 target)
		{
			return MathF.Abs(this.CurrentDirection - this._inputTargetX) < 0.001f && MathF.Abs(this.CurrentReleaseAngle - this._inputTargetY) < 0.001f;
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x000C2050 File Offset: 0x000C0250
		public Vec3 GetEstimatedTargetGlobalPoint(Threat threat)
		{
			Vec3 targetingPosition = threat.TargetingPosition;
			return targetingPosition + this.GetEstimatedTargetMovementVector(targetingPosition, threat.GetGlobalVelocity());
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x000C2077 File Offset: 0x000C0277
		public Vec3 GetEstimatedTargetGlobalPointForAgent(Agent agent)
		{
			return agent.CollisionCapsuleCenter + this.GetEstimatedTargetMovementVector(agent.CollisionCapsuleCenter, agent.GetAverageRealGlobalVelocity());
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x000C2098 File Offset: 0x000C0298
		public virtual void AimAtRotation(float horizontalRotation, float verticalRotation)
		{
			horizontalRotation = MBMath.ClampFloat(horizontalRotation, -3.1415927f, 3.1415927f);
			verticalRotation = MBMath.ClampFloat(verticalRotation, -3.1415927f, 3.1415927f);
			horizontalRotation = MBMath.ClampAngle(horizontalRotation, 0f, this.DirectionRestriction);
			verticalRotation = MBMath.ClampAngle(verticalRotation, this.ReleaseAngleRestrictionCenter, this.ReleaseAngleRestrictionAngle);
			if (!this._exactInputGiven || horizontalRotation != this._inputTargetX || verticalRotation != this._inputTargetY)
			{
				this.GiveExactInput(horizontalRotation, verticalRotation);
			}
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x000C2112 File Offset: 0x000C0312
		protected void OnLoadingAmmoPointUsingCancelled(Agent agent, bool isCanceledBecauseOfAnimation)
		{
			if (agent.IsAIControlled)
			{
				if (isCanceledBecauseOfAnimation)
				{
					this.SendAgentToAmmoPickup(agent);
					return;
				}
				this.SendReloaderAgentToOriginalPoint();
			}
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x000C212D File Offset: 0x000C032D
		protected void OnAmmoPickupUsingCancelled(Agent agent, bool isCanceledBecauseOfAnimation)
		{
			if (agent.IsAIControlled)
			{
				this.SendAgentToAmmoPickup(agent);
			}
		}

		// Token: 0x060030C7 RID: 12487 RVA: 0x000C2140 File Offset: 0x000C0340
		protected void SendAgentToAmmoPickup(Agent agent)
		{
			this.ReloaderAgent = agent;
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			if (primaryWieldedItemIndex != EquipmentIndex.None && agent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.WeaponClass == this.OriginalMissileItem.PrimaryWeapon.WeaponClass)
			{
				agent.AIMoveToGameObjectEnable(this.LoadAmmoStandingPoint, this, base.Ai.GetScriptedFrameFlags(agent));
				return;
			}
			StandingPoint standingPoint = base.AmmoPickUpPoints.FirstOrDefault<StandingPoint>((StandingPoint x) => !x.HasUser);
			if (standingPoint != null)
			{
				agent.AIMoveToGameObjectEnable(standingPoint, this, base.Ai.GetScriptedFrameFlags(agent));
				return;
			}
			this.SendReloaderAgentToOriginalPoint();
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x000C21F0 File Offset: 0x000C03F0
		protected void SendReloaderAgentToOriginalPoint()
		{
			if (this.ReloaderAgent != null)
			{
				if (this.ReloaderAgentOriginalPoint != null && !this.ReloaderAgentOriginalPoint.HasAIMovingTo && !this.ReloaderAgentOriginalPoint.HasUser)
				{
					if (this.ReloaderAgent.InteractingWithAnyGameObject())
					{
						this.ReloaderAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
					}
					this.ReloaderAgent.AIMoveToGameObjectEnable(this.ReloaderAgentOriginalPoint, this, base.Ai.GetScriptedFrameFlags(this.ReloaderAgent));
					return;
				}
				if (this.ReloaderAgentOriginalPoint == null || (this.ReloaderAgentOriginalPoint.MovingAgent != this.ReloaderAgent && this.ReloaderAgentOriginalPoint.UserAgent != this.ReloaderAgent))
				{
					if (this.ReloaderAgent.IsUsingGameObject)
					{
						this.ReloaderAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
					this.ReloaderAgent = null;
				}
			}
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x000C22B8 File Offset: 0x000C04B8
		private void UpdateState(float dt)
		{
			if (this.LoadAmmoStandingPoint != null)
			{
				if (this.ReloaderAgent != null)
				{
					if (!this.ReloaderAgent.IsActive() || this.ReloaderAgent.Detachment != this)
					{
						this.ReloaderAgent = null;
					}
					else if (this.ReloaderAgentOriginalPoint.UserAgent == this.ReloaderAgent)
					{
						this.ReloaderAgent = null;
					}
				}
				if (this.State == RangedSiegeWeapon.WeaponState.LoadingAmmo && this.ReloaderAgent == null && !this.LoadAmmoStandingPoint.HasUser)
				{
					this.SortCanPickUpAmmoStandingPoints();
					StandingPoint standingPoint = null;
					StandingPoint standingPoint2 = null;
					foreach (StandingPoint standingPoint3 in this.CanPickUpAmmoStandingPoints)
					{
						if (standingPoint3.HasUser && standingPoint3.UserAgent.IsAIControlled)
						{
							if (standingPoint3 != base.PilotStandingPoint)
							{
								standingPoint = standingPoint3;
								break;
							}
							standingPoint2 = standingPoint3;
						}
					}
					if (standingPoint == null && standingPoint2 != null)
					{
						standingPoint = standingPoint2;
					}
					if (standingPoint != null)
					{
						if (this.HasAmmo)
						{
							Agent userAgent = standingPoint.UserAgent;
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.DoNotWieldWeaponAfterStoppingUsingGameObject);
							this.ReloaderAgentOriginalPoint = standingPoint;
							this.SendAgentToAmmoPickup(userAgent);
						}
						else
						{
							base.IsDisabledForAI = true;
						}
					}
				}
			}
			switch (this.State)
			{
			case RangedSiegeWeapon.WeaponState.Idle:
			case RangedSiegeWeapon.WeaponState.WaitingAfterShooting:
			case RangedSiegeWeapon.WeaponState.LoadingAmmo:
			case RangedSiegeWeapon.WeaponState.WaitingBeforeIdle:
				return;
			case RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving:
				goto IL_03D6;
			case RangedSiegeWeapon.WeaponState.Shooting:
			{
				for (int i = 0; i < this.Skeletons.Length; i++)
				{
					int animationIndexAtChannel = this.Skeletons[i].GetAnimationIndexAtChannel(0);
					float animationParameterAtChannel = this.Skeletons[i].GetAnimationParameterAtChannel(0);
					if (animationIndexAtChannel == this.FireAnimationIndices[i] && animationParameterAtChannel >= 0.9999f)
					{
						this.State = ((!this.AttackClickWillReload) ? RangedSiegeWeapon.WeaponState.WaitingBeforeReloading : RangedSiegeWeapon.WeaponState.WaitingAfterShooting);
						this._animationTimeElapsed = 0f;
					}
				}
				return;
			}
			case RangedSiegeWeapon.WeaponState.WaitingBeforeReloading:
				break;
			case RangedSiegeWeapon.WeaponState.Reloading:
			{
				int num = 0;
				if (this.ReloadStandingPoints.Count == 0)
				{
					if (base.PilotAgent != null && !base.PilotAgent.IsInBeingStruckAction)
					{
						num = 1;
					}
				}
				else
				{
					foreach (StandingPoint standingPoint4 in this.ReloadStandingPoints)
					{
						if (standingPoint4.HasUser && !standingPoint4.UserAgent.IsInBeingStruckAction)
						{
							num++;
						}
					}
				}
				if (num == 0)
				{
					this.State = RangedSiegeWeapon.WeaponState.ReloadingPaused;
					return;
				}
				if (this._currentReloaderCount != num)
				{
					this._currentReloaderCount = num;
				}
				float num2 = MathF.Sqrt((float)this._currentReloaderCount);
				for (int j = 0; j < this.SkeletonOwnerObjects.Length; j++)
				{
					this.SkeletonOwnerObjects[j].SetAnimationChannelSpeedSynched(0, this.FinalReloadSpeed * this.ReloadSpeedMultiplier * num2);
				}
				for (int k = 0; k < this.Skeletons.Length; k++)
				{
					int animationIndexAtChannel2 = this.Skeletons[k].GetAnimationIndexAtChannel(0);
					float animationParameterAtChannel2 = this.Skeletons[k].GetAnimationParameterAtChannel(0);
					if (animationIndexAtChannel2 == this.SetUpAnimationIndices[k] && animationParameterAtChannel2 >= 0.9999f)
					{
						this.State = RangedSiegeWeapon.WeaponState.LoadingAmmo;
						this._animationTimeElapsed = 0f;
					}
				}
				return;
			}
			case RangedSiegeWeapon.WeaponState.ReloadingPaused:
				if (this.ReloadStandingPoints.Count == 0)
				{
					if (base.PilotAgent != null && !base.PilotAgent.IsInBeingStruckAction)
					{
						this.State = RangedSiegeWeapon.WeaponState.Reloading;
						return;
					}
					return;
				}
				else
				{
					using (List<StandingPoint>.Enumerator enumerator = this.ReloadStandingPoints.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							StandingPoint standingPoint5 = enumerator.Current;
							if (standingPoint5.HasUser && !standingPoint5.UserAgent.IsInBeingStruckAction)
							{
								this.State = RangedSiegeWeapon.WeaponState.Reloading;
								break;
							}
						}
						return;
					}
				}
				break;
			default:
				Debug.FailedAssert("Invalid WeaponState.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\RangedSiegeWeapon.cs", "UpdateState", 1996);
				return;
			}
			this._animationTimeElapsed += dt;
			if (this._animationTimeElapsed < this.TimeGapBetweenShootingEndAndReloadingStart || (this._cameraState != RangedSiegeWeapon.CameraState.RememberLastShotDirection && this._cameraState != RangedSiegeWeapon.CameraState.FreeMove && this._cameraState != RangedSiegeWeapon.CameraState.StickToWeapon && !(this.CameraHolder == null)))
			{
				return;
			}
			if (this.ReloadStandingPoints.Count == 0)
			{
				if (base.PilotAgent != null && !base.PilotAgent.IsInBeingStruckAction)
				{
					this.State = RangedSiegeWeapon.WeaponState.Reloading;
					return;
				}
				return;
			}
			else
			{
				using (List<StandingPoint>.Enumerator enumerator = this.ReloadStandingPoints.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						StandingPoint standingPoint6 = enumerator.Current;
						if (standingPoint6.HasUser && !standingPoint6.UserAgent.IsInBeingStruckAction)
						{
							this.State = RangedSiegeWeapon.WeaponState.Reloading;
							break;
						}
					}
					return;
				}
			}
			IL_03D6:
			this._animationTimeElapsed += dt;
			if (this._animationTimeElapsed >= this.TimeGapBetweenShootActionAndProjectileLeaving)
			{
				this.State = RangedSiegeWeapon.WeaponState.Shooting;
				return;
			}
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x000C2778 File Offset: 0x000C0978
		public bool Shoot()
		{
			this.LastShooterAgent = base.PilotAgent;
			if (this.State == RangedSiegeWeapon.WeaponState.Idle)
			{
				this.State = RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving;
				if (!GameNetwork.IsClientOrReplay)
				{
					this._animationTimeElapsed = 0f;
				}
				return true;
			}
			return false;
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x000C27AA File Offset: 0x000C09AA
		public void ManualReload()
		{
			if (this.AttackClickWillReload)
			{
				this.State = RangedSiegeWeapon.WeaponState.WaitingBeforeReloading;
			}
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x000C27BB File Offset: 0x000C09BB
		public void AiRequestsShoot()
		{
			this._aiRequestsShoot = true;
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x000C27C4 File Offset: 0x000C09C4
		public void AiRequestsManualReload()
		{
			this._aiRequestsManualReload = true;
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x000C27D0 File Offset: 0x000C09D0
		private Vec3 GetBallisticErrorAppliedDirection(float BallisticErrorAmount)
		{
			Mat3 mat = new Mat3
			{
				f = this.ShootingDirection,
				u = Vec3.Up
			};
			mat.Orthonormalize();
			float num = MBRandom.RandomFloat * 6.2831855f;
			mat.RotateAboutForward(num);
			float num2 = BallisticErrorAmount * MBRandom.RandomFloat;
			mat.RotateAboutSide(num2.ToRadians());
			return mat.f;
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000C2838 File Offset: 0x000C0A38
		protected void ShootProjectile()
		{
			if (this.LoadedMissileItem.StringId == this.MultipleProjectileId)
			{
				ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(this.MultipleProjectileFlyingId);
				for (int i = 0; i < this.MultipleProjectileCount; i++)
				{
					this.ShootProjectileAux(@object, true);
				}
			}
			else if (this.LoadedMissileItem.StringId == this.MultipleFireProjectileId)
			{
				ItemObject object2 = Game.Current.ObjectManager.GetObject<ItemObject>(this.MultipleFireProjectileFlyingId);
				for (int j = 0; j < this.MultipleProjectileCount; j++)
				{
					this.ShootProjectileAux(object2, true);
				}
			}
			else if (this.LoadedMissileItem.StringId == this.SingleProjectileId)
			{
				this.ShootProjectileAux(Game.Current.ObjectManager.GetObject<ItemObject>(this.SingleProjectileFlyingId), false);
			}
			else if (this.LoadedMissileItem.StringId == this.SingleFireProjectileId)
			{
				this.ShootProjectileAux(Game.Current.ObjectManager.GetObject<ItemObject>(this.SingleFireProjectileFlyingId), false);
			}
			else
			{
				this.ShootProjectileAux(this.LoadedMissileItem, false);
			}
			this.LastShooterAgent = null;
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x000C2960 File Offset: 0x000C0B60
		protected virtual Mission.Missile ShootProjectileAux(ItemObject missileItem, bool randomizeMissileSpeed)
		{
			Vec3 vec;
			Mat3 mat;
			float num;
			float num2;
			this.SetupProjectileToShoot(randomizeMissileSpeed, out vec, out mat, out num, out num2);
			MissionObject missionObject = base.GameEntity.Root.GetFirstScriptOfType<MissionObject>() ?? this;
			Mission mission = Mission.Current;
			Agent lastShooterAgent = this.LastShooterAgent;
			ItemModifier itemModifier = null;
			IAgentOriginBase origin = this.LastShooterAgent.Origin;
			return mission.AddCustomMissile(lastShooterAgent, new MissionWeapon(missileItem, itemModifier, (origin != null) ? origin.Banner : null, 1), this.ProjectileEntityCurrentGlobalPosition, vec, mat, num2, num, false, missionObject, -1);
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x000C29D8 File Offset: 0x000C0BD8
		protected void SetupProjectileToShoot(bool randomizeMissileSpeed, out Vec3 direction, out Mat3 orientation, out float missileBaseSpeed, out float missileShootingSpeed)
		{
			orientation = Mat3.Identity;
			Vec3 globalVelocity = this.GetGlobalVelocity();
			if (randomizeMissileSpeed)
			{
				float num = this.ShootingSpeed * MBRandom.RandomFloatRanged(0.9f, 1.1f);
				orientation.f = this.GetBallisticErrorAppliedDirection(2.5f);
				orientation.Orthonormalize();
				direction = num * orientation.f + globalVelocity;
				missileShootingSpeed = direction.Normalize();
				missileBaseSpeed = num;
				return;
			}
			orientation.f = this.GetBallisticErrorAppliedDirection(this.MaximumBallisticError);
			orientation.Orthonormalize();
			direction = this.ShootingSpeed * orientation.f + globalVelocity;
			missileShootingSpeed = direction.Normalize();
			missileBaseSpeed = this.ShootingSpeed;
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x060030D2 RID: 12498 RVA: 0x000C2A98 File Offset: 0x000C0C98
		protected virtual Vec3 ShootingDirection
		{
			get
			{
				return this.Projectile.GameEntity.GetGlobalFrame().rotation.u.NormalizedCopy();
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x060030D3 RID: 12499 RVA: 0x000C2ACC File Offset: 0x000C0CCC
		public virtual Vec3 ProjectileEntityCurrentGlobalPosition
		{
			get
			{
				return this.Projectile.GameEntity.GetGlobalFrame().origin;
			}
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x000C2AF4 File Offset: 0x000C0CF4
		protected void OnRotationStarted()
		{
			if (this.MoveSound == null || !this.MoveSound.IsValid)
			{
				this.MoveSound = SoundEvent.CreateEvent(this.MoveSoundIndex, base.Scene);
				this.MoveSound.PlayInPosition(this.RotationObject.GameEntity.GlobalPosition);
			}
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000C2B4C File Offset: 0x000C0D4C
		protected void OnRotationStopped()
		{
			this.MoveSound.Stop();
			this.MoveSound = null;
		}

		// Token: 0x060030D6 RID: 12502
		public abstract override SiegeEngineType GetSiegeEngineType();

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x060030D7 RID: 12503 RVA: 0x000C2B60 File Offset: 0x000C0D60
		public override BattleSideEnum Side
		{
			get
			{
				if (base.PilotAgent != null)
				{
					return base.PilotAgent.Team.Side;
				}
				return this.DefaultSide;
			}
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000C2B84 File Offset: 0x000C0D84
		public bool CanShootAtThreat(Threat threat, int attemptCount = 5)
		{
			WeakGameEntity weakGameEntity = WeakGameEntity.Invalid;
			if (threat.TargetableObject != null)
			{
				weakGameEntity = threat.TargetableObject.GetTargetEntity();
			}
			ValueTuple<Vec3, Vec3> valueTuple = threat.ComputeGlobalTargetingBoundingBoxMinMax();
			Vec3 item = valueTuple.Item1;
			Vec3 item2 = valueTuple.Item2;
			Vec3 vec = (item2 + item) / 2f;
			Vec3 vec2 = new Vec3(vec.AsVec2, item.z, -1f);
			Vec3 vec3 = new Vec3(vec.AsVec2, item2.z, -1f);
			for (int i = 0; i < attemptCount; i++)
			{
				Vec3 vec4 = Vec3.Lerp(vec2, vec3, (float)i / (float)(attemptCount - 1));
				Scene scene = base.Scene;
				Vec3 vec5 = this.MissileStartingGlobalPositionForSimulation;
				float num;
				GameEntity gameEntity;
				if (!scene.RayCastForClosestEntityOrTerrainIgnoreEntity(in vec5, in vec4, base.GameEntity.Root, out num, out gameEntity, this._projectileRadiusCached, BodyFlags.CommonCollisionExcludeFlagsForMissile) || (!(gameEntity == null) && !(gameEntity.Root != weakGameEntity.Root)))
				{
					Vec3 estimatedTargetMovementVector = this.GetEstimatedTargetMovementVector(vec4, threat.GetGlobalVelocity());
					vec4 += estimatedTargetMovementVector;
					Scene scene2 = base.Scene;
					vec5 = this.MissileStartingGlobalPositionForSimulation;
					if ((!scene2.RayCastForClosestEntityOrTerrainIgnoreEntity(in vec5, in vec4, base.GameEntity.Root, out num, out gameEntity, this._projectileRadiusCached, BodyFlags.CommonCollisionExcludeFlagsForMissile) || (!(gameEntity == null) && !(gameEntity.Root != weakGameEntity.Root))) && this.CanShootAtPoint(vec4))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x000C2D04 File Offset: 0x000C0F04
		public bool CanShootAtAgent(Agent agent, int attemptCount = 5)
		{
			WeakGameEntity root = base.GameEntity.Root;
			ValueTuple<Vec3, Vec3> boxMinMax = agent.CollisionCapsule.GetBoxMinMax();
			Vec3 item = boxMinMax.Item1;
			Vec3 item2 = boxMinMax.Item2;
			Vec3 vec = (item2 + item) / 2f;
			Vec3 vec2 = new Vec3(vec.AsVec2, item.z, -1f);
			Vec3 vec3 = new Vec3(vec.AsVec2, item2.z, -1f);
			for (int i = 0; i < attemptCount; i++)
			{
				Vec3 vec4 = Vec3.Lerp(vec2, vec3, (float)i / (float)(attemptCount - 1));
				Scene scene = base.Scene;
				Vec3 vec5 = this.MissileStartingGlobalPositionForSimulation;
				float num;
				GameEntity gameEntity;
				if (!scene.RayCastForClosestEntityOrTerrainIgnoreEntity(in vec5, in vec4, root, out num, out gameEntity, this._projectileRadiusCached, BodyFlags.CommonCollisionExcludeFlagsForMissile))
				{
					vec5 = agent.GetAverageRealGlobalVelocity();
					Vec3 vec6 = new Vec3(vec5.AsVec2, 0f, -1f);
					Vec3 estimatedTargetMovementVector = this.GetEstimatedTargetMovementVector(vec4, vec6);
					vec4 += estimatedTargetMovementVector;
					Scene scene2 = base.Scene;
					vec5 = this.MissileStartingGlobalPositionForSimulation;
					if (!scene2.RayCastForClosestEntityOrTerrainIgnoreEntity(in vec5, in vec4, root, out num, out gameEntity, this._projectileRadiusCached, BodyFlags.CommonCollisionExcludeFlagsForMissile) && this.CanShootAtPoint(vec4))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x000C2E48 File Offset: 0x000C1048
		public virtual Vec3 GetEstimatedTargetMovementVector(Vec3 targetCurrentPosition, Vec3 targetVelocity)
		{
			if (targetVelocity != Vec3.Zero)
			{
				return targetVelocity * ((base.GameEntity.GlobalPosition - targetCurrentPosition).Length / this.ShootingSpeed + this.TimeGapBetweenShootActionAndProjectileLeaving);
			}
			return Vec3.Zero;
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000C2E98 File Offset: 0x000C1098
		public bool CanShootAtPoint(Vec3 target)
		{
			float num;
			float num2;
			this.CalculateLocalDirectionAndLocalAngleToShootTarget(target, out num, out num2);
			if (num2 < this.BottomReleaseAngleRestriction || num2 > this.TopReleaseAngleRestriction)
			{
				return false;
			}
			if (this.DirectionRestriction / 2f - MathF.Abs(num) < 0f)
			{
				return false;
			}
			if (this.CheckFriendlyFireForObjects(target))
			{
				return false;
			}
			Vec3 missileStartingGlobalPositionForSimulation = this.MissileStartingGlobalPositionForSimulation;
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Vec3 u = globalFrame.rotation.u;
			if (!u.IsUnit)
			{
				u.Normalize();
			}
			globalFrame.rotation.RotateAboutAnArbitraryVector(in u, 3.1415927f + num);
			Vec3 s = globalFrame.rotation.s;
			if (!s.IsUnit)
			{
				s.Normalize();
			}
			globalFrame.rotation.RotateAboutAnArbitraryVector(in s, num2);
			float x = globalFrame.rotation.GetEulerAngles().x;
			Vec3 vec = ((this.MissileStartingPositionEntityForSimulation == null) ? this.CanShootAtPointCheckingOffset : Vec3.Zero);
			return this.CanShootPointBallistic(missileStartingGlobalPositionForSimulation + vec, x, this.ShootingSpeed, target);
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000C2FAC File Offset: 0x000C11AC
		private bool CanShootPointBallistic(Vec3 startGlobalPos, float verticalAngle, float shootingSpeed, Vec3 targetGlobalPos)
		{
			float num = shootingSpeed * MathF.Sin(verticalAngle) / 9.806f;
			float num2 = 4.903f * num * num;
			Vec3 vec = (startGlobalPos + targetGlobalPos) / 2f + new Vec3(0f, 0f, num2, -1f);
			float projectileRadiusCached = this._projectileRadiusCached;
			if (verticalAngle <= 0f)
			{
				float num3;
				Agent agent = Mission.Current.RayCastForClosestAgent(startGlobalPos, targetGlobalPos, -1, projectileRadiusCached, out num3);
				if (agent != null && !agent.IsEnemyOf(base.PilotAgent))
				{
					return false;
				}
			}
			else
			{
				float num3;
				GameEntity gameEntity;
				if (base.Scene.RayCastForClosestEntityOrTerrainIgnoreEntity(in startGlobalPos, in vec, base.GameEntity.Root, out num3, out gameEntity, projectileRadiusCached, BodyFlags.CommonCollisionExcludeFlagsForMissile))
				{
					return false;
				}
				Agent agent2 = Mission.Current.RayCastForClosestAgent(startGlobalPos, vec, -1, projectileRadiusCached, out num3);
				if (agent2 != null && !agent2.IsEnemyOf(base.PilotAgent))
				{
					return false;
				}
				agent2 = Mission.Current.RayCastForClosestAgent(vec, targetGlobalPos, -1, projectileRadiusCached * 2f, out num3);
				if (agent2 != null && !agent2.IsEnemyOf(base.PilotAgent))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x000C30C0 File Offset: 0x000C12C0
		protected unsafe virtual bool CheckFriendlyFireForObjects(Vec3 target)
		{
			if (this.Side == BattleSideEnum.Attacker)
			{
				foreach (SiegeWeapon siegeWeapon in *Mission.Current.GetAttackerWeaponsForFriendlyFirePreventing())
				{
					if (siegeWeapon.GameEntity != null && siegeWeapon.GameEntity.IsVisibleIncludeParents())
					{
						Vec3 vec = siegeWeapon.GameEntity.ComputeGlobalPhysicsBoundingBoxCenter();
						Vec3 missileStartingGlobalPositionForSimulation = this.MissileStartingGlobalPositionForSimulation;
						if ((MBMath.GetClosestPointOnLineSegmentToPoint(in missileStartingGlobalPositionForSimulation, in target, in vec) - vec).LengthSquared < 100f)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x000C317C File Offset: 0x000C137C
		protected internal virtual bool IsTargetValid(ITargetable target)
		{
			return true;
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x000C317F File Offset: 0x000C137F
		public override OrderType GetOrder(BattleSideEnum side)
		{
			if (base.IsDestroyed)
			{
				return OrderType.None;
			}
			if (this.Side != side)
			{
				return OrderType.AttackEntity;
			}
			return OrderType.Use;
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x000C3199 File Offset: 0x000C1399
		protected override WeakGameEntity GetEntityToAttachNavMeshFaces()
		{
			return this.RotationObject.GameEntity;
		}

		// Token: 0x060030E1 RID: 12513
		public abstract float ProcessTargetValue(float baseValue, TargetFlags flags);

		// Token: 0x060030E2 RID: 12514 RVA: 0x000C31A8 File Offset: 0x000C13A8
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			RangedSiegeWeapon.RangedSiegeWeaponRecord rangedSiegeWeaponRecord = (RangedSiegeWeapon.RangedSiegeWeaponRecord)synchedMissionObjectReadableRecord.Item2;
			this._state = (RangedSiegeWeapon.WeaponState)rangedSiegeWeaponRecord.State;
			this.TargetDirection = rangedSiegeWeaponRecord.TargetDirection;
			this.TargetReleaseAngle = MBMath.ClampFloat(rangedSiegeWeaponRecord.TargetReleaseAngle, this.BottomReleaseAngleRestriction, this.TopReleaseAngleRestriction);
			this.AmmoCount = rangedSiegeWeaponRecord.AmmoCount;
			this.CurrentDirection = this.TargetDirection;
			this.CurrentReleaseAngle = this.TargetReleaseAngle;
			this.CurrentDirection = this.TargetDirection;
			this.CurrentReleaseAngle = this.TargetReleaseAngle;
			this.ApplyCurrentDirectionToEntity();
			this.CheckAmmo();
			this.UpdateAmmoMesh();
			this.ChangeProjectileEntityClient(rangedSiegeWeaponRecord.ProjectileIndex);
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x000C3260 File Offset: 0x000C1460
		protected virtual void UpdateAmmoMesh()
		{
			WeakGameEntity weakGameEntity = base.AmmoPickUpPoints[0].GameEntity;
			int num = this.StartingAmmoCount - this.AmmoCount;
			while (weakGameEntity.Parent.IsValid)
			{
				for (int i = 0; i < weakGameEntity.MultiMeshComponentCount; i++)
				{
					MetaMesh metaMesh = weakGameEntity.GetMetaMesh(i);
					for (int j = 0; j < metaMesh.MeshCount; j++)
					{
						metaMesh.GetMeshAtIndex(j).SetVectorArgument(0f, (float)num, 0f, 0f);
					}
				}
				weakGameEntity = weakGameEntity.Parent;
			}
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x000C32F7 File Offset: 0x000C14F7
		protected override bool IsAnyUserBelongsToFormation(Formation formation)
		{
			bool flag = base.IsAnyUserBelongsToFormation(formation);
			Agent reloaderAgent = this.ReloaderAgent;
			return flag | (((reloaderAgent != null) ? reloaderAgent.Formation : null) == formation);
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x000C3316 File Offset: 0x000C1516
		public virtual Vec3 GetGlobalVelocity()
		{
			return Vec3.Zero;
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x000C3320 File Offset: 0x000C1520
		private float ComputeProjectileCapsuleRadius()
		{
			float num = 0.01f;
			if (this.LoadedMissileItem.BodyName != null)
			{
				PhysicsShape fromResource = PhysicsShape.GetFromResource(this.LoadedMissileItem.BodyName, false);
				BoundingBox boundingBox = new BoundingBox(in Vec3.Zero);
				fromResource.GetBoundingBox(out boundingBox);
				num = (boundingBox.max.AsVec2 - boundingBox.min.AsVec2).Length / 2f;
			}
			return num;
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x000C3390 File Offset: 0x000C1590
		private void OnLoadedMissileItemChanged()
		{
			if (!this.LoadedMissileItem.StringId.Equals(this._lastLoadedMissileItemId))
			{
				this._projectileRadiusCached = this.ComputeProjectileCapsuleRadius();
				this._lastLoadedMissileItemId = this.LoadedMissileItem.StringId;
			}
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x000C33C7 File Offset: 0x000C15C7
		public void SetPlayerForceUse(bool value)
		{
			this.PlayerForceUse = value;
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x000C33D0 File Offset: 0x000C15D0
		protected override bool ShouldDisableTickIfMachineDisabled()
		{
			return base.AmmoPickUpPoints.Count == 0;
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x000C33E0 File Offset: 0x000C15E0
		public override void OnShipCaptured(BattleSideEnum newDefaultSide)
		{
			base.OnShipCaptured(newDefaultSide);
			this.DefaultSide = newDefaultSide;
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x000C33F0 File Offset: 0x000C15F0
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			(base.Ai as RangedSiegeWeaponAi).ResetThreatSeeker();
		}

		// Token: 0x040013F2 RID: 5106
		private const float DefaultMissileRadius = 0.01f;

		// Token: 0x040013F3 RID: 5107
		public const float DefaultDirectionRestriction = 2.0943952f;

		// Token: 0x040013F4 RID: 5108
		public const string CanGoAmmoPickupTag = "can_pick_up_ammo";

		// Token: 0x040013F5 RID: 5109
		public const string DontApplySidePenaltyTag = "no_ammo_pick_up_penalty";

		// Token: 0x040013F6 RID: 5110
		public const string ReloadTag = "reload";

		// Token: 0x040013F7 RID: 5111
		public const string AmmoLoadTag = "ammoload";

		// Token: 0x040013F8 RID: 5112
		public const string CameraHolderTag = "cameraHolder";

		// Token: 0x040013F9 RID: 5113
		public const string ProjectileTag = "projectile";

		// Token: 0x040013FB RID: 5115
		public string MissileItemID;

		// Token: 0x040013FC RID: 5116
		protected bool UsesMouseForAiming;

		// Token: 0x040013FD RID: 5117
		[EditableScriptComponentVariable(true, "")]
		protected int MultipleProjectileCount = 5;

		// Token: 0x040013FE RID: 5118
		private RangedSiegeWeapon.WeaponState _state;

		// Token: 0x040013FF RID: 5119
		public RangedSiegeWeapon.FiringFocus Focus;

		// Token: 0x04001402 RID: 5122
		private int _projectileIndex;

		// Token: 0x04001403 RID: 5123
		protected GameEntity MissileStartingPositionEntityForSimulation;

		// Token: 0x04001404 RID: 5124
		protected Skeleton[] Skeletons;

		// Token: 0x04001405 RID: 5125
		protected SynchedMissionObject[] SkeletonOwnerObjects;

		// Token: 0x04001406 RID: 5126
		protected string[] SkeletonNames;

		// Token: 0x04001407 RID: 5127
		protected string[] FireAnimations;

		// Token: 0x04001408 RID: 5128
		protected string[] SetUpAnimations;

		// Token: 0x04001409 RID: 5129
		protected int[] FireAnimationIndices;

		// Token: 0x0400140A RID: 5130
		protected int[] SetUpAnimationIndices;

		// Token: 0x0400140B RID: 5131
		protected SynchedMissionObject RotationObject;

		// Token: 0x0400140C RID: 5132
		private MatrixFrame _rotationObjectInitialFrame;

		// Token: 0x0400140D RID: 5133
		protected SoundEvent MoveSound;

		// Token: 0x0400140E RID: 5134
		protected SoundEvent ReloadSound;

		// Token: 0x0400140F RID: 5135
		protected int MoveSoundIndex = -1;

		// Token: 0x04001410 RID: 5136
		protected int ReloadSoundIndex = -1;

		// Token: 0x04001411 RID: 5137
		protected int FireSoundIndex = -1;

		// Token: 0x04001412 RID: 5138
		protected ItemObject OriginalMissileItem;

		// Token: 0x04001413 RID: 5139
		protected WeaponStatsData OriginalMissileWeaponStatsDataForTargeting;

		// Token: 0x04001414 RID: 5140
		private ItemObject _loadedMissileItem;

		// Token: 0x04001415 RID: 5141
		protected List<StandingPoint> CanPickUpAmmoStandingPoints;

		// Token: 0x04001416 RID: 5142
		protected List<StandingPoint> ReloadStandingPoints;

		// Token: 0x04001417 RID: 5143
		protected StandingPointWithWeaponRequirement LoadAmmoStandingPoint;

		// Token: 0x04001418 RID: 5144
		protected Dictionary<StandingPoint, float> PilotReservePriorityValues = new Dictionary<StandingPoint, float>();

		// Token: 0x04001419 RID: 5145
		protected Agent ReloaderAgent;

		// Token: 0x0400141A RID: 5146
		protected StandingPoint ReloaderAgentOriginalPoint;

		// Token: 0x0400141C RID: 5148
		protected bool AttackClickWillReload;

		// Token: 0x0400141D RID: 5149
		protected bool WeaponNeedsClickToReload;

		// Token: 0x0400141E RID: 5150
		protected float FinalReloadSpeed = 1f;

		// Token: 0x0400141F RID: 5151
		protected float BaseReloadSpeed = 1f;

		// Token: 0x04001420 RID: 5152
		public int StartingAmmoCount;

		// Token: 0x04001421 RID: 5153
		protected int CurrentAmmo = 1;

		// Token: 0x04001423 RID: 5155
		protected float TargetDirection;

		// Token: 0x04001424 RID: 5156
		protected float TargetReleaseAngle;

		// Token: 0x04001425 RID: 5157
		protected float CameraDirection;

		// Token: 0x04001426 RID: 5158
		protected float CameraReleaseAngle;

		// Token: 0x04001427 RID: 5159
		protected float ReloadTargetReleaseAngle;

		// Token: 0x04001428 RID: 5160
		private MatrixFrame _cameraHolderInitialFrame;

		// Token: 0x04001429 RID: 5161
		protected float MaxRotateSpeed;

		// Token: 0x0400142A RID: 5162
		private RangedSiegeWeapon.CameraState _cameraState;

		// Token: 0x0400142B RID: 5163
		private bool _inputGiven;

		// Token: 0x0400142C RID: 5164
		protected float DontMoveTimer;

		// Token: 0x0400142D RID: 5165
		private float _inputX;

		// Token: 0x0400142E RID: 5166
		private float _inputY;

		// Token: 0x0400142F RID: 5167
		private bool _exactInputGiven;

		// Token: 0x04001430 RID: 5168
		private float _inputTargetX;

		// Token: 0x04001431 RID: 5169
		private float _inputTargetY;

		// Token: 0x04001432 RID: 5170
		private Vec3 _ammoPickupCenter;

		// Token: 0x04001433 RID: 5171
		private float _lastSyncedDirection;

		// Token: 0x04001434 RID: 5172
		private float _lastSyncedReleaseAngle;

		// Token: 0x04001435 RID: 5173
		private float _syncTimer;

		// Token: 0x04001436 RID: 5174
		private float _cameraMoveBackFactor;

		// Token: 0x04001437 RID: 5175
		public float TopReleaseAngleRestriction = 1.5707964f;

		// Token: 0x04001438 RID: 5176
		public float BottomReleaseAngleRestriction = -1.5707964f;

		// Token: 0x04001439 RID: 5177
		protected float CurrentDirection;

		// Token: 0x0400143A RID: 5178
		protected float CurrentReleaseAngle;

		// Token: 0x0400143B RID: 5179
		protected float ReleaseAngleRestrictionCenter;

		// Token: 0x0400143C RID: 5180
		protected float ReleaseAngleRestrictionAngle;

		// Token: 0x0400143D RID: 5181
		private float _animationTimeElapsed;

		// Token: 0x0400143E RID: 5182
		protected float TimeGapBetweenShootingEndAndReloadingStart = 0.6f;

		// Token: 0x0400143F RID: 5183
		protected float TimeGapBetweenShootActionAndProjectileLeaving;

		// Token: 0x04001440 RID: 5184
		private int _currentReloaderCount;

		// Token: 0x04001441 RID: 5185
		protected Agent LastShooterAgent;

		// Token: 0x04001442 RID: 5186
		private float _lastCanPickUpAmmoStandingPointsSortedAngle = -3.1415927f;

		// Token: 0x04001443 RID: 5187
		protected BattleSideEnum DefaultSide;

		// Token: 0x04001444 RID: 5188
		private bool _aiRequestsShoot;

		// Token: 0x04001445 RID: 5189
		private bool _aiRequestsManualReload;

		// Token: 0x04001446 RID: 5190
		private bool _hasFrameChangedInPreviousFrame;

		// Token: 0x04001447 RID: 5191
		private string _lastLoadedMissileItemId;

		// Token: 0x04001448 RID: 5192
		private float _projectileRadiusCached;

		// Token: 0x0200062E RID: 1582
		[DefineSynchedMissionObjectType(typeof(RangedSiegeWeapon))]
		public struct RangedSiegeWeaponRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AD5 RID: 2773
			// (get) Token: 0x060040B9 RID: 16569 RVA: 0x000FC428 File Offset: 0x000FA628
			// (set) Token: 0x060040BA RID: 16570 RVA: 0x000FC430 File Offset: 0x000FA630
			public int State { get; private set; }

			// Token: 0x17000AD6 RID: 2774
			// (get) Token: 0x060040BB RID: 16571 RVA: 0x000FC439 File Offset: 0x000FA639
			// (set) Token: 0x060040BC RID: 16572 RVA: 0x000FC441 File Offset: 0x000FA641
			public float TargetDirection { get; private set; }

			// Token: 0x17000AD7 RID: 2775
			// (get) Token: 0x060040BD RID: 16573 RVA: 0x000FC44A File Offset: 0x000FA64A
			// (set) Token: 0x060040BE RID: 16574 RVA: 0x000FC452 File Offset: 0x000FA652
			public float TargetReleaseAngle { get; private set; }

			// Token: 0x17000AD8 RID: 2776
			// (get) Token: 0x060040BF RID: 16575 RVA: 0x000FC45B File Offset: 0x000FA65B
			// (set) Token: 0x060040C0 RID: 16576 RVA: 0x000FC463 File Offset: 0x000FA663
			public int AmmoCount { get; private set; }

			// Token: 0x17000AD9 RID: 2777
			// (get) Token: 0x060040C1 RID: 16577 RVA: 0x000FC46C File Offset: 0x000FA66C
			// (set) Token: 0x060040C2 RID: 16578 RVA: 0x000FC474 File Offset: 0x000FA674
			public int ProjectileIndex { get; private set; }

			// Token: 0x060040C3 RID: 16579 RVA: 0x000FC480 File Offset: 0x000FA680
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.State = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponStateCompressionInfo, ref bufferReadValid);
				this.TargetDirection = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.RadianCompressionInfo, ref bufferReadValid);
				this.TargetReleaseAngle = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.RadianCompressionInfo, ref bufferReadValid);
				this.AmmoCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoCompressionInfo, ref bufferReadValid);
				this.ProjectileIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoIndexCompressionInfo, ref bufferReadValid);
				return bufferReadValid;
			}
		}

		// Token: 0x0200062F RID: 1583
		public enum WeaponState
		{
			// Token: 0x0400211F RID: 8479
			Invalid = -1,
			// Token: 0x04002120 RID: 8480
			Idle,
			// Token: 0x04002121 RID: 8481
			WaitingBeforeProjectileLeaving,
			// Token: 0x04002122 RID: 8482
			Shooting,
			// Token: 0x04002123 RID: 8483
			WaitingAfterShooting,
			// Token: 0x04002124 RID: 8484
			WaitingBeforeReloading,
			// Token: 0x04002125 RID: 8485
			LoadingAmmo,
			// Token: 0x04002126 RID: 8486
			WaitingBeforeIdle,
			// Token: 0x04002127 RID: 8487
			Reloading,
			// Token: 0x04002128 RID: 8488
			ReloadingPaused,
			// Token: 0x04002129 RID: 8489
			NumberOfStates
		}

		// Token: 0x02000630 RID: 1584
		public enum FiringFocus
		{
			// Token: 0x0400212B RID: 8491
			Troops,
			// Token: 0x0400212C RID: 8492
			Walls,
			// Token: 0x0400212D RID: 8493
			RangedSiegeWeapons,
			// Token: 0x0400212E RID: 8494
			PrimarySiegeWeapons
		}

		// Token: 0x02000631 RID: 1585
		public enum CameraState
		{
			// Token: 0x04002130 RID: 8496
			StickToWeapon,
			// Token: 0x04002131 RID: 8497
			DoNotMove,
			// Token: 0x04002132 RID: 8498
			MoveDownToReload,
			// Token: 0x04002133 RID: 8499
			RememberLastShotDirection,
			// Token: 0x04002134 RID: 8500
			FreeMove,
			// Token: 0x04002135 RID: 8501
			ApproachToCamera
		}

		// Token: 0x02000632 RID: 1586
		public enum ForceUseState
		{
			// Token: 0x04002137 RID: 8503
			NotForced,
			// Token: 0x04002138 RID: 8504
			ForcefullyUsed
		}

		// Token: 0x02000633 RID: 1587
		// (Invoke) Token: 0x060040C5 RID: 16581
		public delegate void OnSiegeWeaponReloadDone();
	}
}
