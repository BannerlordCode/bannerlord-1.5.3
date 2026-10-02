using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CF RID: 463
	public abstract class SettlementEconomyModel : MBGameModel<SettlementEconomyModel>
	{
		// Token: 0x06001EA9 RID: 7849
		public abstract float GetEstimatedDemandForCategory(Town town, ItemData itemData, ItemCategory category);

		// Token: 0x06001EAA RID: 7850
		public abstract float GetDailyDemandForCategory(Town town, ItemCategory category, int extraProsperity = 0);

		// Token: 0x06001EAB RID: 7851
		public abstract float GetDemandChangeFromValue(float purchaseValue);

		// Token: 0x06001EAC RID: 7852
		public abstract ValueTuple<float, float> GetSupplyDemandForCategory(Town town, ItemCategory category, float dailySupply, float dailyDemand, float oldSupply, float oldDemand);

		// Token: 0x06001EAD RID: 7853
		public abstract int GetTownGoldChange(Town town);

		// Token: 0x06001EAE RID: 7854
		public abstract float CalculateDailySettlementBudgetForItemCategory(Town town, float demand, ItemCategory category);
	}
}
