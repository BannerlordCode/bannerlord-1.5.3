using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;

namespace Helpers
{
	// Token: 0x02000016 RID: 22
	public static class TeleportationHelper
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00009B94 File Offset: 0x00007D94
		public static float GetHoursLeftForTeleportingHeroToReachItsDestination(Hero teleportingHero)
		{
			ITeleportationCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ITeleportationCampaignBehavior>();
			if (campaignBehavior != null)
			{
				return campaignBehavior.GetHeroArrivalTimeToDestination(teleportingHero).RemainingHoursFromNow;
			}
			return 0f;
		}
	}
}
