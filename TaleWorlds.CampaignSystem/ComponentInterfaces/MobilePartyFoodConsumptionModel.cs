using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AC RID: 428
	public abstract class MobilePartyFoodConsumptionModel : MBGameModel<MobilePartyFoodConsumptionModel>
	{
		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06001D60 RID: 7520
		public abstract int NumberOfMenOnMapToEatOneFood { get; }

		// Token: 0x06001D61 RID: 7521
		public abstract ExplainedNumber CalculateDailyBaseFoodConsumptionf(MobileParty party, bool includeDescription = false);

		// Token: 0x06001D62 RID: 7522
		public abstract ExplainedNumber CalculateDailyFoodConsumptionf(MobileParty party, ExplainedNumber baseConsumption);

		// Token: 0x06001D63 RID: 7523
		public abstract bool DoesPartyConsumeFood(MobileParty mobileParty);
	}
}
