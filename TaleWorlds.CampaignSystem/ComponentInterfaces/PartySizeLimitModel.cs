using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C7 RID: 455
	public abstract class PartySizeLimitModel : MBGameModel<PartySizeLimitModel>
	{
		// Token: 0x06001E79 RID: 7801
		public abstract ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false);

		// Token: 0x06001E7A RID: 7802
		public abstract ExplainedNumber GetPartyPrisonerSizeLimit(PartyBase party, bool includeDescriptions = false);

		// Token: 0x06001E7B RID: 7803
		public abstract ExplainedNumber CalculateGarrisonPartySizeLimit(Settlement settlement, bool includeDescriptions = false);

		// Token: 0x06001E7C RID: 7804
		public abstract int GetClanTierPartySizeEffectForHero(Hero hero);

		// Token: 0x06001E7D RID: 7805
		public abstract int GetNextClanTierPartySizeEffectChangeForHero(Hero hero);

		// Token: 0x06001E7E RID: 7806
		public abstract int GetAssumedPartySizeForLordParty(Hero leaderHero, IFaction partyMapFaction, Clan actualClan);

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001E7F RID: 7807
		public abstract int MinimumNumberOfVillagersAtVillagerParty { get; }

		// Token: 0x06001E80 RID: 7808
		public abstract int GetIdealVillagerPartySize(Village village);

		// Token: 0x06001E81 RID: 7809
		public abstract TroopRoster FindAppropriateInitialRosterForMobileParty(MobileParty party, PartyTemplateObject partyTemplate);

		// Token: 0x06001E82 RID: 7810
		public abstract List<Ship> FindAppropriateInitialShipsForMobileParty(MobileParty party, PartyTemplateObject partyTemplate);
	}
}
