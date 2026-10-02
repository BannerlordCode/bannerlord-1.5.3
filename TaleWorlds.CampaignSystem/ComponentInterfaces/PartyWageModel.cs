using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CA RID: 458
	public abstract class PartyWageModel : MBGameModel<PartyWageModel>
	{
		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06001E8D RID: 7821
		public abstract int MaxWagePaymentLimit { get; }

		// Token: 0x06001E8E RID: 7822
		public abstract int GetCharacterWage(CharacterObject character);

		// Token: 0x06001E8F RID: 7823
		public abstract ExplainedNumber GetTotalWage(MobileParty mobileParty, TroopRoster troopRoster, bool includeDescriptions = false);

		// Token: 0x06001E90 RID: 7824
		public abstract ExplainedNumber GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false);
	}
}
