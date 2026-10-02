using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x0200017C RID: 380
	public class EncyclopediaFilterGroup : ViewModel
	{
		// Token: 0x06001BF8 RID: 7160 RVA: 0x00090919 File Offset: 0x0008EB19
		public EncyclopediaFilterGroup(List<EncyclopediaFilterItem> filters, TextObject name)
		{
			this.Filters = filters;
			this.Name = name;
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x0009092F File Offset: 0x0008EB2F
		public Predicate<object> Predicate
		{
			get
			{
				return delegate(object item)
				{
					if (!this.Filters.Any<EncyclopediaFilterItem>((EncyclopediaFilterItem f) => f.IsActive))
					{
						return true;
					}
					foreach (EncyclopediaFilterItem encyclopediaFilterItem in this.Filters)
					{
						if (encyclopediaFilterItem.IsActive && encyclopediaFilterItem.Predicate(item))
						{
							return true;
						}
					}
					return false;
				};
			}
		}

		// Token: 0x0400094C RID: 2380
		public readonly List<EncyclopediaFilterItem> Filters;

		// Token: 0x0400094D RID: 2381
		public readonly TextObject Name;
	}
}
