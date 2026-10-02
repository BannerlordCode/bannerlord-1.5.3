using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DD RID: 477
	public abstract class ClanPoliticsModel : MBGameModel<ClanPoliticsModel>
	{
		// Token: 0x06001F20 RID: 7968
		public abstract ExplainedNumber CalculateInfluenceChange(Clan clan, bool includeDescriptions = false);

		// Token: 0x06001F21 RID: 7969
		public abstract float CalculateSupportForPolicyInClan(Clan clan, PolicyObject policy);

		// Token: 0x06001F22 RID: 7970
		public abstract float CalculateRelationshipChangeWithSponsor(Clan clan, Clan sponsorClan);

		// Token: 0x06001F23 RID: 7971
		public abstract int GetInfluenceRequiredToOverrideKingdomDecision(DecisionOutcome popularOption, DecisionOutcome overridingOption, KingdomDecision decision);

		// Token: 0x06001F24 RID: 7972
		public abstract bool CanHeroBeGovernor(Hero hero);
	}
}
