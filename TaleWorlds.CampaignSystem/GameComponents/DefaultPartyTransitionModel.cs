using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000143 RID: 323
	public class DefaultPartyTransitionModel : PartyTransitionModel
	{
		// Token: 0x06001A14 RID: 6676 RVA: 0x00082EB5 File Offset: 0x000810B5
		public override CampaignTime GetFleetTravelTimeToSettlement(MobileParty mobileParty, Settlement targetSettlement)
		{
			return CampaignTime.Never;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x00082EBC File Offset: 0x000810BC
		public override CampaignTime GetTransitionTimeDisembarking(MobileParty mobileParty)
		{
			return CampaignTime.Never;
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x00082EC3 File Offset: 0x000810C3
		public override CampaignTime GetTransitionTimeForEmbarking(MobileParty mobileParty)
		{
			return CampaignTime.Never;
		}
	}
}
