using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Settlements
{
	// Token: 0x020003D9 RID: 985
	public interface IMarketData
	{
		// Token: 0x06003B26 RID: 15142
		int GetPrice(ItemObject item, MobileParty tradingParty, bool isSelling, PartyBase merchantParty);

		// Token: 0x06003B27 RID: 15143
		int GetPrice(EquipmentElement itemRosterElement, MobileParty tradingParty, bool isSelling, PartyBase merchantParty);
	}
}
