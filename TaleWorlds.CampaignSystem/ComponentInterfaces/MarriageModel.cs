using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E5 RID: 485
	public abstract class MarriageModel : MBGameModel<MarriageModel>
	{
		// Token: 0x06001F47 RID: 8007
		public abstract bool IsCoupleSuitableForMarriage(Hero firstHero, Hero secondHero);

		// Token: 0x06001F48 RID: 8008
		public abstract int GetEffectiveRelationIncrease(Hero firstHero, Hero secondHero);

		// Token: 0x06001F49 RID: 8009
		public abstract Clan GetClanAfterMarriage(Hero firstHero, Hero secondHero);

		// Token: 0x06001F4A RID: 8010
		public abstract bool IsSuitableForMarriage(Hero hero);

		// Token: 0x06001F4B RID: 8011
		public abstract bool IsClanSuitableForMarriage(Clan clan);

		// Token: 0x06001F4C RID: 8012
		public abstract float NpcCoupleMarriageChance(Hero firstHero, Hero secondHero);

		// Token: 0x06001F4D RID: 8013
		public abstract bool ShouldNpcMarriageBetweenClansBeAllowed(Clan consideringClan, Clan targetClan);

		// Token: 0x06001F4E RID: 8014
		public abstract List<Hero> GetAdultChildrenSuitableForMarriage(Hero hero);

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06001F4F RID: 8015
		public abstract int MinimumMarriageAgeMale { get; }

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06001F50 RID: 8016
		public abstract int MinimumMarriageAgeFemale { get; }
	}
}
