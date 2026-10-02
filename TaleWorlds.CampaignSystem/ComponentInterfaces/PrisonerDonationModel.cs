using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001EB RID: 491
	public abstract class PrisonerDonationModel : MBGameModel<PrisonerDonationModel>
	{
		// Token: 0x06001F74 RID: 8052
		public abstract float CalculateRelationGainAfterHeroPrisonerDonate(PartyBase donatingParty, Hero donatedHero, Settlement donatedSettlement);

		// Token: 0x06001F75 RID: 8053
		public abstract float CalculateInfluenceGainAfterPrisonerDonation(PartyBase donatingParty, CharacterObject donatedPrisoner, Settlement donatedSettlement);

		// Token: 0x06001F76 RID: 8054
		public abstract float CalculateInfluenceGainAfterTroopDonation(PartyBase donatingParty, CharacterObject donatedTroop, Settlement donatedSettlement);
	}
}
