using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000194 RID: 404
	public abstract class ItemDiscardModel : MBGameModel<ItemDiscardModel>
	{
		// Token: 0x06001CC4 RID: 7364
		public abstract int GetXpBonusForDiscardingItems(ItemRoster itemRoster);

		// Token: 0x06001CC5 RID: 7365
		public abstract int GetXpBonusForDiscardingItem(ItemObject item, int amount = 1);

		// Token: 0x06001CC6 RID: 7366
		public abstract bool PlayerCanDonateItem(ItemObject item);
	}
}
