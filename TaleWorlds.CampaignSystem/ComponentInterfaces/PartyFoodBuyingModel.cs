using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B5 RID: 437
	public abstract class PartyFoodBuyingModel : MBGameModel<PartyFoodBuyingModel>
	{
		// Token: 0x06001DD7 RID: 7639
		public abstract void FindItemToBuy(MobileParty mobileParty, Settlement settlement, out ItemRosterElement itemRosterElement, out float itemElementsPrice);

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06001DD8 RID: 7640
		public abstract float MinimumDaysFoodToLastWhileBuyingFoodFromTown { get; }

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06001DD9 RID: 7641
		public abstract float MinimumDaysFoodToLastWhileBuyingFoodFromVillage { get; }

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06001DDA RID: 7642
		public abstract float LowCostFoodPriceAverage { get; }
	}
}
