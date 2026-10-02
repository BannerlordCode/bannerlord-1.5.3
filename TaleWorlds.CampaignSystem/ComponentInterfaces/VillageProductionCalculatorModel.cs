using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B7 RID: 439
	public abstract class VillageProductionCalculatorModel : MBGameModel<VillageProductionCalculatorModel>
	{
		// Token: 0x06001DE1 RID: 7649
		public abstract float CalculateProductionSpeedOfItemCategory(ItemCategory item);

		// Token: 0x06001DE2 RID: 7650
		public abstract ExplainedNumber CalculateDailyProductionAmount(Village village, ItemObject item);

		// Token: 0x06001DE3 RID: 7651
		public abstract float CalculateDailyFoodProductionAmount(Village village);
	}
}
