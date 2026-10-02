using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Missions
{
	// Token: 0x020003EC RID: 1004
	public class MissionSiegeWeaponsController : IMissionSiegeWeaponsController
	{
		// Token: 0x06003799 RID: 14233 RVA: 0x000E71F0 File Offset: 0x000E53F0
		public MissionSiegeWeaponsController(BattleSideEnum side, List<MissionSiegeWeapon> weapons)
		{
			this._side = side;
			this._weapons = weapons;
			this._undeployedWeapons = new List<MissionSiegeWeapon>(this._weapons);
			this._deployedWeapons = new Dictionary<DestructableComponent, MissionSiegeWeapon>();
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x000E7224 File Offset: 0x000E5424
		public int GetMaxDeployableWeaponCount(Type t)
		{
			int num = 0;
			using (List<MissionSiegeWeapon>.Enumerator enumerator = this._weapons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (MissionSiegeWeaponsController.GetSiegeWeaponBaseType(enumerator.Current.Type) == t)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x000E7288 File Offset: 0x000E5488
		public IEnumerable<IMissionSiegeWeapon> GetSiegeWeapons()
		{
			return this._weapons.Cast<IMissionSiegeWeapon>();
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x000E7298 File Offset: 0x000E5498
		public void OnWeaponDeployed(SiegeWeapon missionWeapon)
		{
			SiegeEngineType missionWeaponType = missionWeapon.GetSiegeEngineType();
			int num = this._undeployedWeapons.FindIndex((MissionSiegeWeapon uw) => uw.Type == missionWeaponType);
			MissionSiegeWeapon missionSiegeWeapon = this._undeployedWeapons[num];
			DestructableComponent destructionComponent = missionWeapon.DestructionComponent;
			destructionComponent.MaxHitPoint = missionSiegeWeapon.MaxHealth;
			destructionComponent.HitPoint = missionSiegeWeapon.InitialHealth;
			destructionComponent.OnHitTaken += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponHit);
			destructionComponent.OnDestroyed += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponDestroyed);
			this._undeployedWeapons.RemoveAt(num);
			this._deployedWeapons.Add(destructionComponent, missionSiegeWeapon);
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x000E7338 File Offset: 0x000E5538
		public void OnWeaponUndeployed(SiegeWeapon missionWeapon)
		{
			DestructableComponent destructionComponent = missionWeapon.DestructionComponent;
			MissionSiegeWeapon missionSiegeWeapon;
			this._deployedWeapons.TryGetValue(destructionComponent, out missionSiegeWeapon);
			SiegeEngineType siegeEngineType = missionWeapon.GetSiegeEngineType();
			destructionComponent.MaxHitPoint = (float)siegeEngineType.BaseHitPoints;
			destructionComponent.HitPoint = destructionComponent.MaxHitPoint;
			destructionComponent.OnHitTaken -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponHit);
			destructionComponent.OnDestroyed -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponDestroyed);
			this._deployedWeapons.Remove(destructionComponent);
			this._undeployedWeapons.Add(missionSiegeWeapon);
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x000E73B8 File Offset: 0x000E55B8
		private void OnWeaponHit(DestructableComponent target, Agent attackerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			MissionSiegeWeapon missionSiegeWeapon;
			if (target.BattleSide == this._side && this._deployedWeapons.TryGetValue(target, out missionSiegeWeapon))
			{
				float num = Math.Max(0f, missionSiegeWeapon.Health - (float)inflictedDamage);
				missionSiegeWeapon.SetHealth(num);
			}
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x000E7400 File Offset: 0x000E5600
		private void OnWeaponDestroyed(DestructableComponent target, Agent attackerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			MissionSiegeWeapon missionSiegeWeapon;
			if (target.BattleSide == this._side && this._deployedWeapons.TryGetValue(target, out missionSiegeWeapon))
			{
				missionSiegeWeapon.SetHealth(0f);
				target.OnHitTaken -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponHit);
				target.OnDestroyed -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnWeaponDestroyed);
				this._deployedWeapons.Remove(target);
			}
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x000E7468 File Offset: 0x000E5668
		public static Type GetWeaponType(ScriptComponentBehavior weapon)
		{
			if (weapon is UsableGameObjectGroup)
			{
				return weapon.GameEntity.GetChildren().SelectMany<WeakGameEntity, ScriptComponentBehavior>((WeakGameEntity c) => c.GetScriptComponents()).First<ScriptComponentBehavior>((ScriptComponentBehavior s) => s is IFocusable)
					.GetType();
			}
			return weapon.GetType();
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x000E74E0 File Offset: 0x000E56E0
		private static Type GetSiegeWeaponBaseType(SiegeEngineType siegeWeaponType)
		{
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ladder)
			{
				return typeof(SiegeLadder);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ballista)
			{
				return typeof(Ballista);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.FireBallista)
			{
				return typeof(FireBallista);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Ram)
			{
				return typeof(BatteringRam);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.SiegeTower)
			{
				return typeof(SiegeTower);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Onager || siegeWeaponType == DefaultSiegeEngineTypes.Catapult)
			{
				return typeof(Mangonel);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.FireOnager || siegeWeaponType == DefaultSiegeEngineTypes.FireCatapult)
			{
				return typeof(FireMangonel);
			}
			if (siegeWeaponType == DefaultSiegeEngineTypes.Trebuchet)
			{
				return typeof(Trebuchet);
			}
			return null;
		}

		// Token: 0x04001809 RID: 6153
		private readonly List<MissionSiegeWeapon> _weapons;

		// Token: 0x0400180A RID: 6154
		private readonly List<MissionSiegeWeapon> _undeployedWeapons;

		// Token: 0x0400180B RID: 6155
		private readonly Dictionary<DestructableComponent, MissionSiegeWeapon> _deployedWeapons;

		// Token: 0x0400180C RID: 6156
		private BattleSideEnum _side;
	}
}
