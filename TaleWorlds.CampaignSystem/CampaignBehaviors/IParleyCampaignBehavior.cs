using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000428 RID: 1064
	public interface IParleyCampaignBehavior
	{
		// Token: 0x06004377 RID: 17271
		PartyBase GetParleyedParty();

		// Token: 0x06004378 RID: 17272
		void StartParley(PartyBase partyBase);
	}
}
