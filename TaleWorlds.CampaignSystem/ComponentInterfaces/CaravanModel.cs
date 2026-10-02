using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A0 RID: 416
	public abstract class CaravanModel : MBGameModel<CaravanModel>
	{
		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06001D0A RID: 7434
		public abstract int MaxNumberOfItemsToBuyFromSingleCategory { get; }

		// Token: 0x06001D0B RID: 7435
		public abstract int GetMaxGoldToSpendOnOneItemCategory(MobileParty caravan, ItemCategory itemCategory);

		// Token: 0x06001D0C RID: 7436
		public abstract int GetInitialTradeGold(Hero owner, bool isNavalCaravan, bool eliteCaravan);

		// Token: 0x06001D0D RID: 7437
		public abstract int GetCaravanFormingCost(bool eliteCaravan, bool navalCaravan);

		// Token: 0x06001D0E RID: 7438
		public abstract int GetPowerChangeAfterCaravanCreation(Hero hero, MobileParty caravanParty);

		// Token: 0x06001D0F RID: 7439
		public abstract bool CanHeroCreateCaravan(Hero hero);

		// Token: 0x06001D10 RID: 7440
		public abstract float GetEliteCaravanSpawnChance(Hero hero);
	}
}
