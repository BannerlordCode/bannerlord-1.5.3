using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting
{
	// Token: 0x02000103 RID: 259
	public class CraftingSecondaryUsageItemVM : SelectorItemVM
	{
		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x060016F9 RID: 5881 RVA: 0x00059262 File Offset: 0x00057462
		public int UsageIndex { get; }

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x0005926A File Offset: 0x0005746A
		public int SelectorIndex { get; }

		// Token: 0x060016FB RID: 5883 RVA: 0x00059272 File Offset: 0x00057472
		public CraftingSecondaryUsageItemVM(TextObject name, int index, int usageIndex, SelectorVM<CraftingSecondaryUsageItemVM> parentSelector)
			: base(name)
		{
			this._parentSelector = parentSelector;
			this.SelectorIndex = index;
			this.UsageIndex = usageIndex;
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x00059291 File Offset: 0x00057491
		public void ExecuteSelect()
		{
			this._parentSelector.SelectedIndex = this.SelectorIndex;
		}

		// Token: 0x04000A77 RID: 2679
		private SelectorVM<CraftingSecondaryUsageItemVM> _parentSelector;
	}
}
