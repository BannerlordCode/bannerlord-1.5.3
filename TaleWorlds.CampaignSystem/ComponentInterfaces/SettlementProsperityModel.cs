using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D6 RID: 470
	public abstract class SettlementProsperityModel : MBGameModel<SettlementProsperityModel>
	{
		// Token: 0x06001EF4 RID: 7924
		public abstract ExplainedNumber CalculateProsperityChange(Town fortification, bool includeDescriptions = false);

		// Token: 0x06001EF5 RID: 7925
		public abstract ExplainedNumber CalculateHearthChange(Village village, bool includeDescriptions = false);
	}
}
