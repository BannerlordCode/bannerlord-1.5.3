using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000178 RID: 376
	public static class ItemObjectExtensions
	{
		// Token: 0x06001BDF RID: 7135 RVA: 0x00090519 File Offset: 0x0008E719
		public static ItemCategory GetItemCategory(this ItemObject item)
		{
			return item.ItemCategory;
		}
	}
}
