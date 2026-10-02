using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200020A RID: 522
	public abstract class CampaignShipParametersModel : MBGameModel<CampaignShipParametersModel>
	{
		// Token: 0x0600204B RID: 8267
		public abstract float GetShipSizeWeatherFactor(ShipHull shipHull);

		// Token: 0x0600204C RID: 8268
		public abstract float GetDefaultCombatFactor(ShipHull shipHull);

		// Token: 0x0600204D RID: 8269
		public abstract float GetCampaignSpeedBonusFactor(Ship ship);

		// Token: 0x0600204E RID: 8270
		public abstract float GetCrewCapacityBonusFactor(Ship ship);

		// Token: 0x0600204F RID: 8271
		public abstract float GetShipWeightFactor(Ship ship);

		// Token: 0x06002050 RID: 8272
		public abstract float GetForwardDragFactor(Ship ship);

		// Token: 0x06002051 RID: 8273
		public abstract float GetCrewShieldHitPointsFactor(Ship ship);

		// Token: 0x06002052 RID: 8274
		public abstract int GetAdditionalAmmoBonus(Ship ship);

		// Token: 0x06002053 RID: 8275
		public abstract float GetMaxOarPowerFactor(Ship ship);

		// Token: 0x06002054 RID: 8276
		public abstract float GetMaxOarForceFactor(Ship ship);

		// Token: 0x06002055 RID: 8277
		public abstract float GetSailForceFactor(Ship ship);

		// Token: 0x06002056 RID: 8278
		public abstract float GetCrewMeleeDamageFactor(Ship ship);

		// Token: 0x06002057 RID: 8279
		public abstract int GetAdditionalArcherQuivers(Ship ship);

		// Token: 0x06002058 RID: 8280
		public abstract int GetAdditionalThrowingWeaponStack(Ship ship);

		// Token: 0x06002059 RID: 8281
		public abstract float GetSailRotationSpeedFactor(Ship ship);

		// Token: 0x0600205A RID: 8282
		public abstract float GetFurlUnfurlSpeedFactor(Ship ship);
	}
}
