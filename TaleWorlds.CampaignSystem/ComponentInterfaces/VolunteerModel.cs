using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B9 RID: 441
	public abstract class VolunteerModel : MBGameModel<VolunteerModel>
	{
		// Token: 0x06001DF1 RID: 7665
		public abstract int MaximumIndexHeroCanRecruitFromHero(Hero buyerHero, Hero sellerHero, int useValueAsRelation = -101);

		// Token: 0x06001DF2 RID: 7666
		public abstract int MaximumIndexGarrisonCanRecruitFromHero(Settlement settlement, Hero sellerHero);

		// Token: 0x06001DF3 RID: 7667
		public abstract float GetDailyVolunteerProductionProbability(Hero hero, int index, Settlement settlement);

		// Token: 0x06001DF4 RID: 7668
		public abstract CharacterObject GetBasicVolunteer(Hero hero);

		// Token: 0x06001DF5 RID: 7669
		public abstract bool CanHaveRecruits(Hero hero);

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06001DF6 RID: 7670
		public abstract int MaxVolunteerTier { get; }
	}
}
