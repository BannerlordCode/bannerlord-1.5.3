using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000195 RID: 405
	public abstract class MapVisibilityModel : MBGameModel<MapVisibilityModel>
	{
		// Token: 0x06001CC8 RID: 7368
		public abstract float MaximumSeeingRange();

		// Token: 0x06001CC9 RID: 7369
		public abstract float GetPartySeeingRangeBase(MobileParty party);

		// Token: 0x06001CCA RID: 7370
		public abstract ExplainedNumber GetPartySpottingRange(MobileParty party, bool includeDescriptions = false);

		// Token: 0x06001CCB RID: 7371
		public abstract float GetPartySpottingRatioForMainPartySeeingRange(MobileParty party);

		// Token: 0x06001CCC RID: 7372
		public abstract float GetHideoutSpottingDistance();

		// Token: 0x06001CCD RID: 7373
		public abstract void GetMobilePartyVisibilityAndInspectedState(MobileParty mobileParty, Vec2[] points, float seeingRange, out bool isVisible, out bool isInspected, out bool isDistanceDependent);

		// Token: 0x06001CCE RID: 7374
		public abstract void GetSettlementInspectedState(Settlement settlement, Vec2[] points, float seeingRange, out bool isInspected, out bool isDistanceDependent);
	}
}
