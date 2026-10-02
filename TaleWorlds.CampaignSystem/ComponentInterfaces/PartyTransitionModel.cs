using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B0 RID: 432
	public abstract class PartyTransitionModel : MBGameModel<PartyTransitionModel>
	{
		// Token: 0x06001D79 RID: 7545
		public abstract CampaignTime GetTransitionTimeForEmbarking(MobileParty mobileParty);

		// Token: 0x06001D7A RID: 7546
		public abstract CampaignTime GetTransitionTimeDisembarking(MobileParty mobileParty);

		// Token: 0x06001D7B RID: 7547
		public abstract CampaignTime GetFleetTravelTimeToSettlement(MobileParty mobileParty, Settlement targetSettlement);
	}
}
