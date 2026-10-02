using System;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A2 RID: 418
	public abstract class BarterModel : MBGameModel<BarterModel>
	{
		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06001D1A RID: 7450
		public abstract int BarterCooldownWithHeroInDays { get; }

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06001D1B RID: 7451
		public abstract float MaximumPercentageOfNpcGoldToSpendAtBarter { get; }

		// Token: 0x06001D1C RID: 7452
		public abstract int CalculateOverpayRelationIncreaseCosts(Hero hero, float overpayAmount);

		// Token: 0x06001D1D RID: 7453
		public abstract ExplainedNumber GetBarterPenalty(IFaction faction, ItemBarterable itemBarterable, Hero otherHero, PartyBase otherParty);
	}
}
