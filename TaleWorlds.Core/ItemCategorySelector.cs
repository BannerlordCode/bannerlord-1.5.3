using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000073 RID: 115
	public abstract class ItemCategorySelector : MBGameModel<ItemCategorySelector>
	{
		// Token: 0x060007EE RID: 2030
		public abstract ItemCategory GetItemCategoryForItem(ItemObject itemObject);
	}
}
