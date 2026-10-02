using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001FC RID: 508
	public abstract class BannerItemModel : MBGameModel<BannerItemModel>
	{
		// Token: 0x06001FF5 RID: 8181
		public abstract IEnumerable<ItemObject> GetPossibleRewardBannerItems();

		// Token: 0x06001FF6 RID: 8182
		public abstract IEnumerable<ItemObject> GetPossibleRewardBannerItemsForHero(Hero hero);

		// Token: 0x06001FF7 RID: 8183
		public abstract int GetBannerItemLevelForHero(Hero hero);

		// Token: 0x06001FF8 RID: 8184
		public abstract bool CanBannerBeUpdated(ItemObject item);
	}
}
