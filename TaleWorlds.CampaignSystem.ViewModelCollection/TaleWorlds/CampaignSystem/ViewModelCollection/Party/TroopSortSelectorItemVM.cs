using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x0200002A RID: 42
	public class TroopSortSelectorItemVM : SelectorItemVM
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00016B9C File Offset: 0x00014D9C
		// (set) Token: 0x06000350 RID: 848 RVA: 0x00016BA4 File Offset: 0x00014DA4
		public PartyScreenLogic.TroopSortType SortType { get; private set; }

		// Token: 0x06000351 RID: 849 RVA: 0x00016BAD File Offset: 0x00014DAD
		public TroopSortSelectorItemVM(TextObject s, PartyScreenLogic.TroopSortType sortType)
			: base(s)
		{
			this.SortType = sortType;
		}
	}
}
