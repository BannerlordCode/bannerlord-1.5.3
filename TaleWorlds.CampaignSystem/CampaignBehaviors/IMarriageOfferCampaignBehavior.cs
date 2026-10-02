using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000423 RID: 1059
	public interface IMarriageOfferCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x06004346 RID: 17222
		void OnMarriageOfferedToPlayer(Hero suitor, Hero maiden);

		// Token: 0x06004347 RID: 17223
		void OnMarriageOfferCanceled(Hero suitor, Hero maiden);

		// Token: 0x06004348 RID: 17224
		MBBindingList<TextObject> GetMarriageAcceptedConsequences();

		// Token: 0x06004349 RID: 17225
		void OnMarriageOfferAcceptedOnPopUp();

		// Token: 0x0600434A RID: 17226
		void OnMarriageOfferDeclinedOnPopUp();

		// Token: 0x0600434B RID: 17227
		bool IsHeroEngaged(Hero hero);
	}
}
