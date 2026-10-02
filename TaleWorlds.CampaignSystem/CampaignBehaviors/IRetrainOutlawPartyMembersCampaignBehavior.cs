using System;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200042A RID: 1066
	public interface IRetrainOutlawPartyMembersCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x0600437A RID: 17274
		int GetRetrainedNumber(CharacterObject character);

		// Token: 0x0600437B RID: 17275
		void SetRetrainedNumber(CharacterObject character, int number);
	}
}
