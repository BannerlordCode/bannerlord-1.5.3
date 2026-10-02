using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000DB RID: 219
	public interface IPlayerTradeBehavior
	{
		// Token: 0x06001507 RID: 5383
		int GetProjectedProfit(ItemRosterElement itemRosterElement, int itemCost);
	}
}
