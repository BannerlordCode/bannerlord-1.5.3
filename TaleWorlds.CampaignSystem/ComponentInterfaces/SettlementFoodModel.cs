using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CE RID: 462
	public abstract class SettlementFoodModel : MBGameModel<SettlementFoodModel>
	{
		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06001EA3 RID: 7843
		public abstract int FoodStocksUpperLimit { get; }

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06001EA4 RID: 7844
		public abstract int NumberOfProsperityToEatOneFood { get; }

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001EA5 RID: 7845
		public abstract int NumberOfMenOnGarrisonToEatOneFood { get; }

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001EA6 RID: 7846
		public abstract int CastleFoodStockUpperLimitBonus { get; }

		// Token: 0x06001EA7 RID: 7847
		public abstract ExplainedNumber CalculateTownFoodStocksChange(Town town, bool includeMarketStocks = true, bool includeDescriptions = false);
	}
}
