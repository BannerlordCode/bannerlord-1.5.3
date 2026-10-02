using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000173 RID: 371
	public static class Items
	{
		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06001BD8 RID: 7128 RVA: 0x000904C7 File Offset: 0x0008E6C7
		public static MBReadOnlyList<ItemObject> All
		{
			get
			{
				return Campaign.Current.AllItems;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x000904D3 File Offset: 0x0008E6D3
		public static IEnumerable<ItemObject> AllTradeGoods
		{
			get
			{
				MBReadOnlyList<ItemObject> all = Items.All;
				foreach (ItemObject itemObject in all)
				{
					if (itemObject.IsTradeGood)
					{
						yield return itemObject;
					}
				}
				List<ItemObject>.Enumerator enumerator = default(List<ItemObject>.Enumerator);
				yield break;
				yield break;
			}
		}
	}
}
