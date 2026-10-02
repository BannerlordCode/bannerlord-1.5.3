using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions;

namespace TaleWorlds.MountAndBlade.AI
{
	// Token: 0x0200040E RID: 1038
	public class SiegeWeaponAutoDeployer
	{
		// Token: 0x060038A6 RID: 14502 RVA: 0x000E9A62 File Offset: 0x000E7C62
		public SiegeWeaponAutoDeployer(List<DeploymentPoint> deploymentPoints, IMissionSiegeWeaponsController weaponsController)
		{
			this.deploymentPoints = deploymentPoints;
			this.siegeWeaponsController = weaponsController;
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x000E9A78 File Offset: 0x000E7C78
		public void DeployAll(BattleSideEnum side)
		{
			if (side == BattleSideEnum.Attacker)
			{
				this.DeployAllForAttackers();
				return;
			}
			if (side == BattleSideEnum.Defender)
			{
				this.DeployAllForDefenders();
				return;
			}
			Debug.FailedAssert("Invalid side", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponAutoDeployer.cs", "DeployAll", 32);
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x000E9AA8 File Offset: 0x000E7CA8
		private bool DeployWeaponFrom(DeploymentPoint dp)
		{
			IEnumerable<Type> enumerable = dp.DeployableWeaponTypes.Where<Type>((Type t) => this.deploymentPoints.Count<DeploymentPoint>((DeploymentPoint dep) => dep.IsDeployed && MissionSiegeWeaponsController.GetWeaponType(dep.DeployedWeapon) == t) < this.siegeWeaponsController.GetMaxDeployableWeaponCount(t));
			if (!enumerable.IsEmpty<Type>())
			{
				Type type = enumerable.MaxBy<Type, float>((Type t) => this.GetWeaponValue(t));
				dp.Deploy(type);
				return true;
			}
			return false;
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x000E9AF4 File Offset: 0x000E7CF4
		private void DeployAllForAttackers()
		{
			List<DeploymentPoint> list = this.deploymentPoints.Where<DeploymentPoint>((DeploymentPoint dp) => !dp.IsDisabled && !dp.IsDeployed).ToList<DeploymentPoint>();
			list.Shuffle<DeploymentPoint>();
			int num = this.deploymentPoints.Count<DeploymentPoint>((DeploymentPoint dp) => dp.GetDeploymentPointType() == DeploymentPoint.DeploymentPointType.Breach);
			bool flag = Mission.Current.AttackerTeam != Mission.Current.PlayerTeam && num >= 2;
			foreach (DeploymentPoint deploymentPoint in list)
			{
				if (!flag || deploymentPoint.GetDeploymentPointType() == DeploymentPoint.DeploymentPointType.Ranged)
				{
					this.DeployWeaponFrom(deploymentPoint);
				}
			}
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x000E9BD0 File Offset: 0x000E7DD0
		private void DeployAllForDefenders()
		{
			Mission mission = Mission.Current;
			Scene scene = mission.Scene;
			List<ICastleKeyPosition> list = (from amo in mission.ActiveMissionObjects
				select amo.GameEntity into e
				select e.GetFirstScriptOfType<UsableMachine>() into um
				where um is ICastleKeyPosition
				select um).Cast<ICastleKeyPosition>().Where<ICastleKeyPosition>(delegate(ICastleKeyPosition x)
			{
				IPrimarySiegeWeapon attackerSiegeWeapon = x.AttackerSiegeWeapon;
				return attackerSiegeWeapon == null || attackerSiegeWeapon.WeaponSide != FormationAI.BehaviorSide.BehaviorSideNotSet;
			}).ToList<ICastleKeyPosition>();
			List<DeploymentPoint> list2 = this.deploymentPoints.Where<DeploymentPoint>((DeploymentPoint dp) => !dp.IsDeployed).ToList<DeploymentPoint>();
			while (!list2.IsEmpty<DeploymentPoint>())
			{
				Threat maxThreat = RangedSiegeWeaponAi.ThreatSeeker.GetMaxThreat(list);
				Vec3 mostDangerousThreatPosition = maxThreat.TargetingPosition;
				DeploymentPoint deploymentPoint = list2.MinBy<DeploymentPoint, float>((DeploymentPoint dp) => dp.GameEntity.GlobalPosition.DistanceSquared(mostDangerousThreatPosition));
				if (this.DeployWeaponFrom(deploymentPoint))
				{
					maxThreat.ThreatValue *= 0.5f;
				}
				list2.Remove(deploymentPoint);
			}
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x000E9D18 File Offset: 0x000E7F18
		protected virtual float GetWeaponValue(Type weaponType)
		{
			if (weaponType == typeof(BatteringRam) || weaponType == typeof(SiegeTower) || weaponType == typeof(SiegeLadder))
			{
				return 0.9f + MBRandom.RandomFloat * 0.2f;
			}
			if (typeof(RangedSiegeWeapon).IsAssignableFrom(weaponType))
			{
				return 0.7f + MBRandom.RandomFloat * 0.2f;
			}
			return 1f;
		}

		// Token: 0x04001860 RID: 6240
		private IMissionSiegeWeaponsController siegeWeaponsController;

		// Token: 0x04001861 RID: 6241
		private List<DeploymentPoint> deploymentPoints;
	}
}
