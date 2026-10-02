using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000182 RID: 386
	public abstract class EncyclopediaListItemComparerBase : IComparer<EncyclopediaListItem>
	{
		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06001C20 RID: 7200 RVA: 0x000910DE File Offset: 0x0008F2DE
		// (set) Token: 0x06001C21 RID: 7201 RVA: 0x000910E6 File Offset: 0x0008F2E6
		public bool IsAscending { get; private set; }

		// Token: 0x06001C22 RID: 7202 RVA: 0x000910EF File Offset: 0x0008F2EF
		public void SetSortOrder(bool isAscending)
		{
			this.IsAscending = isAscending;
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x000910F8 File Offset: 0x0008F2F8
		public void SwitchSortOrder()
		{
			this.IsAscending = !this.IsAscending;
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x00091109 File Offset: 0x0008F309
		public void SetDefaultSortOrder()
		{
			this.IsAscending = false;
		}

		// Token: 0x06001C25 RID: 7205
		public abstract int Compare(EncyclopediaListItem x, EncyclopediaListItem y);

		// Token: 0x06001C26 RID: 7206
		public abstract string GetComparedValueText(EncyclopediaListItem item);

		// Token: 0x06001C27 RID: 7207 RVA: 0x00091112 File Offset: 0x0008F312
		protected int ResolveEquality(EncyclopediaListItem x, EncyclopediaListItem y)
		{
			return x.Name.CompareTo(y.Name);
		}

		// Token: 0x04000966 RID: 2406
		protected readonly TextObject _emptyValue = new TextObject("{=4NaOKslb}-", null);

		// Token: 0x04000967 RID: 2407
		protected readonly TextObject _missingValue = new TextObject("{=keqS2dGa}???", null);
	}
}
