using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Encyclopedia;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000E5 RID: 229
	public class EncyclopediaListItemComparer : IComparer<EncyclopediaListItemVM>
	{
		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x0600158E RID: 5518 RVA: 0x00055517 File Offset: 0x00053717
		public EncyclopediaSortController SortController { get; }

		// Token: 0x0600158F RID: 5519 RVA: 0x0005551F File Offset: 0x0005371F
		public EncyclopediaListItemComparer(EncyclopediaSortController sortController)
		{
			this.SortController = sortController;
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x00055530 File Offset: 0x00053730
		private int GetBookmarkComparison(EncyclopediaListItemVM x, EncyclopediaListItemVM y)
		{
			return -x.IsBookmarked.CompareTo(y.IsBookmarked);
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x00055554 File Offset: 0x00053754
		public int Compare(EncyclopediaListItemVM x, EncyclopediaListItemVM y)
		{
			int bookmarkComparison = this.GetBookmarkComparison(x, y);
			if (bookmarkComparison != 0)
			{
				return bookmarkComparison;
			}
			return this.SortController.Comparer.Compare(x.ListItem, y.ListItem);
		}
	}
}
