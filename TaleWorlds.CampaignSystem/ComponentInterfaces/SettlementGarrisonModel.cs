using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D7 RID: 471
	public abstract class SettlementGarrisonModel : MBGameModel<SettlementGarrisonModel>
	{
		// Token: 0x06001EF7 RID: 7927
		public abstract ExplainedNumber GetMaximumDailyAutoRecruitmentCount(Town town, bool includeDescriptions = false);

		// Token: 0x06001EF8 RID: 7928
		public abstract ExplainedNumber CalculateBaseGarrisonChange(Settlement settlement, bool includeDescriptions = false);

		// Token: 0x06001EF9 RID: 7929
		public abstract int FindNumberOfTroopsToTakeFromGarrison(MobileParty mobileParty, Settlement settlement, float idealGarrisonStrengthPerWalledCenter = 0f);

		// Token: 0x06001EFA RID: 7930
		public abstract float GetMaximumDailyRepairAmount(Settlement settlement);
	}
}
