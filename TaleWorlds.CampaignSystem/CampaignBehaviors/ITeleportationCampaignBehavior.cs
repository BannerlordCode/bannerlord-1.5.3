using System;
using TaleWorlds.CampaignSystem.Map;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200042D RID: 1069
	public interface ITeleportationCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x060043EA RID: 17386
		bool GetTargetOfTeleportingHero(Hero teleportingHero, out bool isGovernor, out bool isPartyLeader, out IMapPoint target);

		// Token: 0x060043EB RID: 17387
		CampaignTime GetHeroArrivalTimeToDestination(Hero teleportingHero);
	}
}
