using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Missions;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000299 RID: 665
	public class MissionSiegeEnginesLogic : MissionLogic
	{
		// Token: 0x0600250F RID: 9487 RVA: 0x00086F93 File Offset: 0x00085193
		public MissionSiegeEnginesLogic(List<MissionSiegeWeapon> defenderSiegeWeapons, List<MissionSiegeWeapon> attackerSiegeWeapons)
		{
			this._defenderSiegeWeaponsController = new MissionSiegeWeaponsController(BattleSideEnum.Defender, defenderSiegeWeapons);
			this._attackerSiegeWeaponsController = new MissionSiegeWeaponsController(BattleSideEnum.Attacker, attackerSiegeWeapons);
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x00086FB5 File Offset: 0x000851B5
		public IMissionSiegeWeaponsController GetSiegeWeaponsController(BattleSideEnum side)
		{
			if (side == BattleSideEnum.Defender)
			{
				return this._defenderSiegeWeaponsController;
			}
			if (side == BattleSideEnum.Attacker)
			{
				return this._attackerSiegeWeaponsController;
			}
			return null;
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x00086FCD File Offset: 0x000851CD
		public void GetMissionSiegeWeapons(out IEnumerable<IMissionSiegeWeapon> defenderSiegeWeapons, out IEnumerable<IMissionSiegeWeapon> attackerSiegeWeapons)
		{
			defenderSiegeWeapons = this._defenderSiegeWeaponsController.GetSiegeWeapons();
			attackerSiegeWeapons = this._attackerSiegeWeaponsController.GetSiegeWeapons();
		}

		// Token: 0x04000E43 RID: 3651
		private readonly MissionSiegeWeaponsController _defenderSiegeWeaponsController;

		// Token: 0x04000E44 RID: 3652
		private readonly MissionSiegeWeaponsController _attackerSiegeWeaponsController;
	}
}
