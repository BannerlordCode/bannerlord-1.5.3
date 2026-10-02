using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000105 RID: 261
	public class DefaultCampaignShipParametersModel : CampaignShipParametersModel
	{
		// Token: 0x0600173C RID: 5948 RVA: 0x0006C6C8 File Offset: 0x0006A8C8
		public override float GetShipSizeWeatherFactor(ShipHull shipHull)
		{
			return 0f;
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x0006C6CF File Offset: 0x0006A8CF
		public override float GetDefaultCombatFactor(ShipHull shipHull)
		{
			return 0f;
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x0006C6D6 File Offset: 0x0006A8D6
		public override float GetCampaignSpeedBonusFactor(Ship ship)
		{
			return 0f;
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x0006C6DD File Offset: 0x0006A8DD
		public override float GetCrewCapacityBonusFactor(Ship ship)
		{
			return 0f;
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x0006C6E4 File Offset: 0x0006A8E4
		public override float GetShipWeightFactor(Ship ship)
		{
			return 0f;
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x0006C6EB File Offset: 0x0006A8EB
		public override float GetForwardDragFactor(Ship ship)
		{
			return 0f;
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x0006C6F2 File Offset: 0x0006A8F2
		public override float GetCrewShieldHitPointsFactor(Ship ship)
		{
			return 0f;
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x0006C6F9 File Offset: 0x0006A8F9
		public override int GetAdditionalAmmoBonus(Ship ship)
		{
			return 0;
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x0006C6FC File Offset: 0x0006A8FC
		public override float GetMaxOarPowerFactor(Ship ship)
		{
			return 0f;
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x0006C703 File Offset: 0x0006A903
		public override float GetMaxOarForceFactor(Ship ship)
		{
			return 0f;
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x0006C70A File Offset: 0x0006A90A
		public override float GetSailForceFactor(Ship ship)
		{
			return 0f;
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x0006C711 File Offset: 0x0006A911
		public override float GetCrewMeleeDamageFactor(Ship ship)
		{
			return 0f;
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x0006C718 File Offset: 0x0006A918
		public override int GetAdditionalArcherQuivers(Ship ship)
		{
			return 0;
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x0006C71B File Offset: 0x0006A91B
		public override int GetAdditionalThrowingWeaponStack(Ship ship)
		{
			return 0;
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x0006C71E File Offset: 0x0006A91E
		public override float GetSailRotationSpeedFactor(Ship ship)
		{
			return 0f;
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x0006C725 File Offset: 0x0006A925
		public override float GetFurlUnfurlSpeedFactor(Ship ship)
		{
			return 0f;
		}
	}
}
