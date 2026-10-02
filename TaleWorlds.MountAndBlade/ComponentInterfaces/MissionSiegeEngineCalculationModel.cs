using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x02000400 RID: 1024
	public abstract class MissionSiegeEngineCalculationModel : MBGameModel<MissionSiegeEngineCalculationModel>
	{
		// Token: 0x06003838 RID: 14392
		public abstract float CalculateReloadSpeed(Agent userAgent, float baseSpeed);

		// Token: 0x06003839 RID: 14393
		public abstract int CalculateShipSiegeWeaponAmmoCount(IShipOrigin shipOrigin, Agent captain, RangedSiegeWeapon weapon);

		// Token: 0x0600383A RID: 14394
		public abstract int CalculateDamage(Agent attackerAgent, float baseDamage);
	}
}
