using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000176 RID: 374
	public static class ItemCategories
	{
		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x00090501 File Offset: 0x0008E701
		public static MBReadOnlyList<ItemCategory> All
		{
			get
			{
				return Campaign.Current.AllItemCategories;
			}
		}
	}
}
