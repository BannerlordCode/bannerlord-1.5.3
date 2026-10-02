using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Missions
{
	// Token: 0x020003EB RID: 1003
	public interface IMissionSiegeWeaponsController
	{
		// Token: 0x06003795 RID: 14229
		int GetMaxDeployableWeaponCount(Type t);

		// Token: 0x06003796 RID: 14230
		IEnumerable<IMissionSiegeWeapon> GetSiegeWeapons();

		// Token: 0x06003797 RID: 14231
		void OnWeaponDeployed(SiegeWeapon missionWeapon);

		// Token: 0x06003798 RID: 14232
		void OnWeaponUndeployed(SiegeWeapon missionWeapon);
	}
}
