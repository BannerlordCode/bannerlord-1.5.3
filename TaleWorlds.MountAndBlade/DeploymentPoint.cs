using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions;
using TaleWorlds.MountAndBlade.Objects.Siege;
using TaleWorlds.MountAndBlade.Objects.Usables;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200034C RID: 844
	public class DeploymentPoint : SynchedMissionObject
	{
		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06002FB8 RID: 12216 RVA: 0x000BAD8C File Offset: 0x000B8F8C
		// (remove) Token: 0x06002FB9 RID: 12217 RVA: 0x000BADC4 File Offset: 0x000B8FC4
		public event Action<DeploymentPoint, SynchedMissionObject> OnDeploymentStateChanged;

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06002FBA RID: 12218 RVA: 0x000BADFC File Offset: 0x000B8FFC
		// (remove) Token: 0x06002FBB RID: 12219 RVA: 0x000BAE34 File Offset: 0x000B9034
		public event Action<DeploymentPoint> OnDeploymentPointTypeDetermined;

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06002FBC RID: 12220 RVA: 0x000BAE6C File Offset: 0x000B906C
		// (remove) Token: 0x06002FBD RID: 12221 RVA: 0x000BAEA4 File Offset: 0x000B90A4
		public event Action<DeploymentPoint> OnDeployOrDisband;

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06002FBE RID: 12222 RVA: 0x000BAED9 File Offset: 0x000B90D9
		// (set) Token: 0x06002FBF RID: 12223 RVA: 0x000BAEE1 File Offset: 0x000B90E1
		public Vec3 DeploymentTargetPosition { get; private set; }

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06002FC0 RID: 12224 RVA: 0x000BAEEA File Offset: 0x000B90EA
		// (set) Token: 0x06002FC1 RID: 12225 RVA: 0x000BAEF2 File Offset: 0x000B90F2
		public WallSegment AssociatedWallSegment { get; private set; }

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06002FC2 RID: 12226 RVA: 0x000BAEFB File Offset: 0x000B90FB
		public IEnumerable<SynchedMissionObject> DeployableWeapons
		{
			get
			{
				return this._weapons.Where<SynchedMissionObject>((SynchedMissionObject w) => !w.IsDisabled);
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06002FC3 RID: 12227 RVA: 0x000BAF27 File Offset: 0x000B9127
		public bool IsDeployed
		{
			get
			{
				return this.DeployedWeapon != null;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06002FC4 RID: 12228 RVA: 0x000BAF32 File Offset: 0x000B9132
		// (set) Token: 0x06002FC5 RID: 12229 RVA: 0x000BAF3A File Offset: 0x000B913A
		public SynchedMissionObject DeployedWeapon { get; private set; }

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x000BAF43 File Offset: 0x000B9143
		// (set) Token: 0x06002FC7 RID: 12231 RVA: 0x000BAF4B File Offset: 0x000B914B
		public SynchedMissionObject DisbandedWeapon { get; private set; }

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000BAF54 File Offset: 0x000B9154
		protected internal override void OnInit()
		{
			this._weapons = new MBList<SynchedMissionObject>();
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000BAF64 File Offset: 0x000B9164
		public override void AfterMissionStart()
		{
			base.OnInit();
			if (!GameNetwork.IsClientOrReplay)
			{
				this._weapons = this.GetWeaponsUnder();
				this._associatedSiegeLadders = new List<SiegeLadder>();
				if (this.DeployableWeapons.IsEmpty<SynchedMissionObject>())
				{
					this.SetVisibleSynched(false, false);
					this.SetBreachSideDeploymentPoint();
				}
				base.AfterMissionStart();
				if (!GameNetwork.IsClientOrReplay)
				{
					this.DetermineDeploymentPointType();
				}
				this.HideAllWeapons();
			}
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x000BAFCC File Offset: 0x000B91CC
		private void SetBreachSideDeploymentPoint()
		{
			Debug.Print("Deployment point " + (base.GameEntity.IsValid ? ("upgrade level mask " + base.GameEntity.GetUpgradeLevelMask().ToString()) : "no game entity.") + "\n", 0, Debug.DebugColor.White, 17592186044416UL);
			this._isBreachSideDeploymentPoint = true;
			this._deploymentPointType = DeploymentPoint.DeploymentPointType.Breach;
			FormationAI.BehaviorSide deploymentPointSide = (this._weapons.FirstOrDefault<SynchedMissionObject>((SynchedMissionObject w) => w is SiegeTower) as IPrimarySiegeWeapon).WeaponSide;
			this.AssociatedWallSegment = Mission.Current.ActiveMissionObjects.FindAllWithType<WallSegment>().FirstOrDefault<WallSegment>((WallSegment ws) => ws.DefenseSide == deploymentPointSide);
			this.DeploymentTargetPosition = this.AssociatedWallSegment.GameEntity.GlobalPosition;
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x000BB0C4 File Offset: 0x000B92C4
		public Vec3 GetDeploymentOrigin()
		{
			return base.GameEntity.GlobalPosition;
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x000BB0E0 File Offset: 0x000B92E0
		public DeploymentPoint.DeploymentPointState GetDeploymentPointState()
		{
			switch (this._deploymentPointType)
			{
			case DeploymentPoint.DeploymentPointType.BatteringRam:
				if (!this.IsDeployed)
				{
					return DeploymentPoint.DeploymentPointState.NotDeployed;
				}
				return DeploymentPoint.DeploymentPointState.BatteringRam;
			case DeploymentPoint.DeploymentPointType.TowerLadder:
				if (!this.IsDeployed)
				{
					return DeploymentPoint.DeploymentPointState.SiegeLadder;
				}
				return DeploymentPoint.DeploymentPointState.SiegeTower;
			case DeploymentPoint.DeploymentPointType.Breach:
				return DeploymentPoint.DeploymentPointState.Breach;
			case DeploymentPoint.DeploymentPointType.Ranged:
				if (!this.IsDeployed)
				{
					return DeploymentPoint.DeploymentPointState.NotDeployed;
				}
				return DeploymentPoint.DeploymentPointState.Ranged;
			default:
				MBDebug.ShowWarning("Undefined deployment point type fetched.");
				return DeploymentPoint.DeploymentPointState.NotDeployed;
			}
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000BB13D File Offset: 0x000B933D
		public DeploymentPoint.DeploymentPointType GetDeploymentPointType()
		{
			return this._deploymentPointType;
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x000BB145 File Offset: 0x000B9345
		public List<SiegeLadder> GetAssociatedSiegeLadders()
		{
			return this._associatedSiegeLadders;
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x000BB150 File Offset: 0x000B9350
		private void DetermineDeploymentPointType()
		{
			if (this._isBreachSideDeploymentPoint)
			{
				this._deploymentPointType = DeploymentPoint.DeploymentPointType.Breach;
			}
			else if (this._weapons.Any<SynchedMissionObject>((SynchedMissionObject w) => w is BatteringRam))
			{
				this._deploymentPointType = DeploymentPoint.DeploymentPointType.BatteringRam;
				this.DeploymentTargetPosition = (this._weapons.First<SynchedMissionObject>((SynchedMissionObject w) => w is BatteringRam) as IPrimarySiegeWeapon).TargetCastlePosition.GameEntity.GlobalPosition;
			}
			else if (this._weapons.Any<SynchedMissionObject>((SynchedMissionObject w) => w is SiegeTower))
			{
				SiegeTower tower = this._weapons.FirstOrDefault<SynchedMissionObject>((SynchedMissionObject w) => w is SiegeTower) as SiegeTower;
				this._deploymentPointType = DeploymentPoint.DeploymentPointType.TowerLadder;
				this.DeploymentTargetPosition = tower.TargetCastlePosition.GameEntity.GlobalPosition;
				this._associatedSiegeLadders = (from sl in Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeLadder>()
					where sl.WeaponSide == tower.WeaponSide
					select sl).ToList<SiegeLadder>();
			}
			else
			{
				this._deploymentPointType = DeploymentPoint.DeploymentPointType.Ranged;
				this.DeploymentTargetPosition = Vec3.Invalid;
			}
			Action<DeploymentPoint> onDeploymentPointTypeDetermined = this.OnDeploymentPointTypeDetermined;
			if (onDeploymentPointTypeDetermined == null)
			{
				return;
			}
			onDeploymentPointTypeDetermined(this);
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000BB2D0 File Offset: 0x000B94D0
		public MBList<SynchedMissionObject> GetWeaponsUnder()
		{
			TeamAISiegeComponent teamAISiegeComponent;
			List<SiegeWeapon> list;
			if ((teamAISiegeComponent = Mission.Current.Teams[0].TeamAI as TeamAISiegeComponent) != null)
			{
				list = teamAISiegeComponent.SceneSiegeWeapons;
			}
			else
			{
				List<GameEntity> list2 = new List<GameEntity>();
				base.GameEntity.Scene.GetEntities(ref list2);
				list = (from se in list2
					where se.HasScriptOfType<SiegeWeapon>()
					select se.GetScriptComponents<SiegeWeapon>().FirstOrDefault<SiegeWeapon>()).ToList<SiegeWeapon>();
			}
			MBList<SynchedMissionObject> mblist = new MBList<SynchedMissionObject>();
			float num = this.Radius * this.Radius;
			foreach (SiegeWeapon siegeWeapon in list)
			{
				if (siegeWeapon.GameEntity.HasTag(this.SiegeWeaponTag) || (siegeWeapon.GameEntity.Parent.IsValid && siegeWeapon.GameEntity.Parent.HasTag(this.SiegeWeaponTag)) || (siegeWeapon.GameEntity != base.GameEntity && siegeWeapon.GameEntity.GlobalPosition.DistanceSquared(base.GameEntity.GlobalPosition) < num))
				{
					mblist.Add(siegeWeapon);
				}
			}
			return mblist;
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000BB464 File Offset: 0x000B9664
		public IEnumerable<SpawnerBase> GetSpawnersForEditor()
		{
			List<GameEntity> list = new List<GameEntity>();
			base.GameEntity.Scene.GetEntities(ref list);
			IEnumerable<SpawnerBase> enumerable = from se in list
				where se.HasScriptOfType<SpawnerBase>()
				select se.GetScriptComponents<SpawnerBase>().FirstOrDefault<SpawnerBase>();
			IEnumerable<SpawnerBase> enumerable2 = from ssw in enumerable
				where ssw.GameEntity.HasTag(this.SiegeWeaponTag)
				select (ssw);
			Vec3 globalPosition = base.GameEntity.GlobalPosition;
			float radiusSquared = this.Radius * this.Radius;
			IEnumerable<SpawnerBase> enumerable3 = from ssw in enumerable
				where ssw.GameEntity != this.GameEntity && ssw.GameEntity.GlobalPosition.DistanceSquared(globalPosition) < radiusSquared
				select (ssw);
			return enumerable2.Concat<SpawnerBase>(enumerable3).Distinct<SpawnerBase>();
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000BB584 File Offset: 0x000B9784
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._weapons = null;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000BB594 File Offset: 0x000B9794
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			foreach (GameEntity gameEntity in this._highlightedEntites)
			{
				gameEntity.SetContourColor(null, true);
			}
			this._highlightedEntites.Clear();
			if (MBEditor.IsEntitySelected(base.GameEntity))
			{
				uint num = 4294901760U;
				if (this.Radius > 0f)
				{
					DebugExtensions.RenderDebugCircleOnTerrain(base.Scene, base.GameEntity.GetGlobalFrame(), this.Radius, num, true, false);
				}
				foreach (SpawnerBase spawnerBase in this.GetSpawnersForEditor())
				{
					spawnerBase.GameEntity.SetContourColor(new uint?(num), true);
					this._highlightedEntites.Add(TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(spawnerBase.GameEntity));
				}
			}
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x000BB6B0 File Offset: 0x000B98B0
		private void OnDeploymentStateChangedAux(SynchedMissionObject targetObject)
		{
			if (this.IsDeployed)
			{
				targetObject.SetVisibleSynched(true, false);
				targetObject.SetPhysicsStateSynched(true, true);
			}
			else
			{
				targetObject.SetVisibleSynched(false, false);
				targetObject.SetPhysicsStateSynched(false, true);
			}
			Action<DeploymentPoint, SynchedMissionObject> onDeploymentStateChanged = this.OnDeploymentStateChanged;
			if (onDeploymentStateChanged != null)
			{
				onDeploymentStateChanged(this, targetObject);
			}
			SiegeWeapon siegeWeapon;
			if ((siegeWeapon = targetObject as SiegeWeapon) != null)
			{
				siegeWeapon.OnDeploymentStateChanged(this.IsDeployed);
			}
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x000BB710 File Offset: 0x000B9910
		public void Deploy(Type t)
		{
			this.DeployedWeapon = this._weapons.First<SynchedMissionObject>((SynchedMissionObject w) => MissionSiegeWeaponsController.GetWeaponType(w) == t);
			this.OnDeploymentStateChangedAux(this.DeployedWeapon);
			this.ToggleDeploymentPointVisibility(false);
			this.ToggleDeployedWeaponVisibility(true);
			Action<DeploymentPoint> onDeployOrDisband = this.OnDeployOrDisband;
			if (onDeployOrDisband == null)
			{
				return;
			}
			onDeployOrDisband(this);
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000BB772 File Offset: 0x000B9972
		public void Deploy(SiegeWeapon s)
		{
			this.DeployedWeapon = s;
			this.DisbandedWeapon = null;
			this.OnDeploymentStateChangedAux(s);
			this.ToggleDeploymentPointVisibility(false);
			this.ToggleDeployedWeaponVisibility(true);
			Action<DeploymentPoint> onDeployOrDisband = this.OnDeployOrDisband;
			if (onDeployOrDisband == null)
			{
				return;
			}
			onDeployOrDisband(this);
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x000BB7A8 File Offset: 0x000B99A8
		public ScriptComponentBehavior Disband()
		{
			this.ToggleDeploymentPointVisibility(true);
			this.ToggleDeployedWeaponVisibility(false);
			this.DisbandedWeapon = this.DeployedWeapon;
			this.DeployedWeapon = null;
			this.OnDeploymentStateChangedAux(this.DisbandedWeapon);
			Action<DeploymentPoint> onDeployOrDisband = this.OnDeployOrDisband;
			if (onDeployOrDisband != null)
			{
				onDeployOrDisband(this);
			}
			return this.DisbandedWeapon;
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06002FD8 RID: 12248 RVA: 0x000BB7FA File Offset: 0x000B99FA
		public IEnumerable<Type> DeployableWeaponTypes
		{
			get
			{
				return this.DeployableWeapons.Select<SynchedMissionObject, Type>(new Func<SynchedMissionObject, Type>(MissionSiegeWeaponsController.GetWeaponType));
			}
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x000BB814 File Offset: 0x000B9A14
		public void Hide()
		{
			this.ToggleDeploymentPointVisibility(false);
			foreach (SynchedMissionObject synchedMissionObject in this.GetWeaponsUnder())
			{
				if (synchedMissionObject != null)
				{
					synchedMissionObject.SetVisibleSynched(false, false);
					synchedMissionObject.SetPhysicsStateSynched(false, true);
				}
			}
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x000BB87C File Offset: 0x000B9A7C
		public void Show()
		{
			this.ToggleDeploymentPointVisibility(!this.IsDeployed);
			if (this.IsDeployed)
			{
				this.ToggleDeployedWeaponVisibility(true);
			}
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x000BB89C File Offset: 0x000B9A9C
		private void ToggleDeploymentPointVisibility(bool visible)
		{
			this.SetVisibleSynched(visible, false);
			this.SetPhysicsStateSynched(visible, true);
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x000BB8AE File Offset: 0x000B9AAE
		private void ToggleDeployedWeaponVisibility(bool visible)
		{
			this.ToggleWeaponVisibility(visible, this.DeployedWeapon);
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x000BB8C0 File Offset: 0x000B9AC0
		public void ToggleWeaponVisibility(bool visible, SynchedMissionObject weapon)
		{
			WeakGameEntity weakGameEntity = ((weapon != null) ? weapon.GameEntity.Parent : WeakGameEntity.Invalid);
			SynchedMissionObject synchedMissionObject = (weakGameEntity.IsValid ? weakGameEntity.GetFirstScriptOfType<SynchedMissionObject>() : null);
			if (synchedMissionObject != null)
			{
				synchedMissionObject.SetVisibleSynched(visible, false);
				synchedMissionObject.SetPhysicsStateSynched(visible, true);
			}
			else
			{
				if (weapon != null)
				{
					weapon.SetVisibleSynched(visible, false);
				}
				if (weapon != null)
				{
					weapon.SetPhysicsStateSynched(visible, true);
				}
			}
			if (weapon is SiegeWeapon && weapon.GameEntity.Parent.IsValid)
			{
				foreach (WeakGameEntity weakGameEntity2 in weapon.GameEntity.Parent.GetChildren())
				{
					SiegeMachineStonePile firstScriptOfType = weakGameEntity2.GetFirstScriptOfType<SiegeMachineStonePile>();
					if (firstScriptOfType != null)
					{
						firstScriptOfType.SetPhysicsStateSynched(visible, true);
						break;
					}
				}
			}
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x000BB9A8 File Offset: 0x000B9BA8
		public void HideAllWeapons()
		{
			foreach (SynchedMissionObject synchedMissionObject in this.DeployableWeapons)
			{
				this.ToggleWeaponVisibility(false, synchedMissionObject);
			}
		}

		// Token: 0x0400136F RID: 4975
		public BattleSideEnum Side = BattleSideEnum.Attacker;

		// Token: 0x04001370 RID: 4976
		public float Radius = 3f;

		// Token: 0x04001371 RID: 4977
		public string SiegeWeaponTag = "dpWeapon";

		// Token: 0x04001372 RID: 4978
		private readonly List<GameEntity> _highlightedEntites = new List<GameEntity>();

		// Token: 0x04001373 RID: 4979
		private DeploymentPoint.DeploymentPointType _deploymentPointType;

		// Token: 0x04001374 RID: 4980
		private List<SiegeLadder> _associatedSiegeLadders;

		// Token: 0x04001375 RID: 4981
		private bool _isBreachSideDeploymentPoint;

		// Token: 0x04001376 RID: 4982
		private MBList<SynchedMissionObject> _weapons;

		// Token: 0x02000620 RID: 1568
		public enum DeploymentPointType
		{
			// Token: 0x040020F0 RID: 8432
			BatteringRam,
			// Token: 0x040020F1 RID: 8433
			TowerLadder,
			// Token: 0x040020F2 RID: 8434
			Breach,
			// Token: 0x040020F3 RID: 8435
			Ranged
		}

		// Token: 0x02000621 RID: 1569
		public enum DeploymentPointState
		{
			// Token: 0x040020F5 RID: 8437
			NotDeployed,
			// Token: 0x040020F6 RID: 8438
			BatteringRam,
			// Token: 0x040020F7 RID: 8439
			SiegeLadder,
			// Token: 0x040020F8 RID: 8440
			SiegeTower,
			// Token: 0x040020F9 RID: 8441
			Breach,
			// Token: 0x040020FA RID: 8442
			Ranged
		}
	}
}
