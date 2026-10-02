using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200041D RID: 1053
	public interface IDisbandPartyCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x06004336 RID: 17206
		bool IsPartyWaitingForDisband(MobileParty party);
	}
}
