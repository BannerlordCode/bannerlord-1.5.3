using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003FD RID: 1021
	public class DefaultSiegeEngineCalculationModel : MissionSiegeEngineCalculationModel
	{
		// Token: 0x06003826 RID: 14374 RVA: 0x000E9313 File Offset: 0x000E7513
		public override float CalculateReloadSpeed(Agent userAgent, float baseSpeed)
		{
			return baseSpeed;
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x000E9316 File Offset: 0x000E7516
		public override int CalculateShipSiegeWeaponAmmoCount(IShipOrigin shipOrigin, Agent captain, RangedSiegeWeapon weapon)
		{
			return weapon.AmmoCount;
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x000E931E File Offset: 0x000E751E
		public override int CalculateDamage(Agent attackerAgent, float baseDamage)
		{
			return (int)baseDamage;
		}
	}
}
