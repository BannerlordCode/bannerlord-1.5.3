using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000E6 RID: 230
	public class EncyclopediaListSelectorVM : SelectorVM<EncyclopediaListSelectorItemVM>
	{
		// Token: 0x06001592 RID: 5522 RVA: 0x0005558B File Offset: 0x0005378B
		public EncyclopediaListSelectorVM(int selectedIndex, Action<SelectorVM<EncyclopediaListSelectorItemVM>> onChange, Action onActivate)
			: base(selectedIndex, onChange)
		{
			this._onActivate = onActivate;
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0005559C File Offset: 0x0005379C
		public void ExecuteOnDropdownActivated()
		{
			Action onActivate = this._onActivate;
			if (onActivate == null)
			{
				return;
			}
			onActivate();
		}

		// Token: 0x040009C7 RID: 2503
		private Action _onActivate;
	}
}
