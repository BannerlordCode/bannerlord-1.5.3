using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000031 RID: 49
	public class OrderOfBattleFormationFilterSelectorItemComparer : IComparer<OrderOfBattleFormationFilterSelectorItemVM>
	{
		// Token: 0x0600037D RID: 893 RVA: 0x0000C79F File Offset: 0x0000A99F
		public int Compare(OrderOfBattleFormationFilterSelectorItemVM x, OrderOfBattleFormationFilterSelectorItemVM y)
		{
			return x.FilterType.CompareTo(y.FilterType);
		}
	}
}
