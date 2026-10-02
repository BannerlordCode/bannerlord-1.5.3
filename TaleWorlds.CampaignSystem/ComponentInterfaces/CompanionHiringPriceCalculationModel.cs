using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F2 RID: 498
	public abstract class CompanionHiringPriceCalculationModel : MBGameModel<CompanionHiringPriceCalculationModel>
	{
		// Token: 0x06001FB1 RID: 8113
		public abstract int GetCompanionHiringPrice(Hero companion);
	}
}
