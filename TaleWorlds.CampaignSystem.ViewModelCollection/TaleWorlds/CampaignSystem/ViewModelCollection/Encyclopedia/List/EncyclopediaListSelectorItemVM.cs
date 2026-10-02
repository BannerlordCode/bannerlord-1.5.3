using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000E7 RID: 231
	public class EncyclopediaListSelectorItemVM : SelectorItemVM
	{
		// Token: 0x06001594 RID: 5524 RVA: 0x000555AE File Offset: 0x000537AE
		public EncyclopediaListSelectorItemVM(EncyclopediaListItemComparer comparer)
			: base(comparer.SortController.Name.ToString())
		{
			this.Comparer = comparer;
		}

		// Token: 0x040009C8 RID: 2504
		public EncyclopediaListItemComparer Comparer;
	}
}
