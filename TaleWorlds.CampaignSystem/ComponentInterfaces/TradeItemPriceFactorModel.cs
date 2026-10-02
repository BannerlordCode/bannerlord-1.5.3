using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CC RID: 460
	public abstract class TradeItemPriceFactorModel : MBGameModel<TradeItemPriceFactorModel>
	{
		// Token: 0x06001E99 RID: 7833
		public abstract float GetTradePenalty(ItemObject item, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStore, float supply, float demand);

		// Token: 0x06001E9A RID: 7834
		public abstract float GetBasePriceFactor(ItemCategory itemCategory, float inStoreValue, float supply, float demand, bool isSelling, int transferValue);

		// Token: 0x06001E9B RID: 7835
		public abstract int GetPrice(EquipmentElement itemRosterElement, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStoreValue, float supply, float demand);

		// Token: 0x06001E9C RID: 7836
		public abstract int GetTheoreticalMaxItemMarketValue(ItemObject item);
	}
}
