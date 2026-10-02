using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D8 RID: 472
	public abstract class SettlementTaxModel : MBGameModel<SettlementTaxModel>
	{
		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06001EFC RID: 7932
		public abstract float SettlementCommissionRateTown { get; }

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06001EFD RID: 7933
		public abstract float SettlementCommissionRateVillage { get; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06001EFE RID: 7934
		public abstract int SettlementCommissionDecreaseSecurityThreshold { get; }

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06001EFF RID: 7935
		public abstract int MaximumDecreaseBasedOnSecuritySecurity { get; }

		// Token: 0x06001F00 RID: 7936
		public abstract float GetTownTaxRatio(Town town);

		// Token: 0x06001F01 RID: 7937
		public abstract float GetVillageTaxRatio(Village village);

		// Token: 0x06001F02 RID: 7938
		public abstract float GetTownCommissionChangeBasedOnSecurity(Town town, float commission);

		// Token: 0x06001F03 RID: 7939
		public abstract ExplainedNumber CalculateTownTax(Town town, bool includeDescriptions = false);

		// Token: 0x06001F04 RID: 7940
		public abstract int CalculateVillageTaxFromIncome(Village village, int marketIncome);
	}
}
