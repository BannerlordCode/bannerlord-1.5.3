using System;
using System.Collections.Generic;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions
{
	// Token: 0x0200004B RID: 75
	public class MultipleSelectionHostGameOptionDataVM : GenericHostGameOptionDataVM
	{
		// Token: 0x0600069E RID: 1694 RVA: 0x0001566C File Offset: 0x0001386C
		public MultipleSelectionHostGameOptionDataVM(MultiplayerOptions.OptionType optionType, int preferredIndex)
			: base(OptionsVM.OptionsDataType.MultipleSelectionOption, optionType, preferredIndex)
		{
			List<string> multiplayerOptionsList = MultiplayerOptions.Instance.GetMultiplayerOptionsList(base.OptionType);
			List<string> multiplayerOptionsTextList = MultiplayerOptions.Instance.GetMultiplayerOptionsTextList(base.OptionType);
			List<string> list = new List<string>();
			foreach (string text in multiplayerOptionsTextList)
			{
				list.Add(text);
			}
			this.Selector = new SelectorVM<SelectorItemVM>(list, multiplayerOptionsList.IndexOf(MultiplayerOptions.Instance.GetValueTextForOptionWithMultipleSelection(base.OptionType)), null);
			this.Selector.SetOnChangeAction(new Action<SelectorVM<SelectorItemVM>>(this.OnChangeSelected));
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00015724 File Offset: 0x00013924
		public override void RefreshData()
		{
			this.Selector.SetOnChangeAction(null);
			List<string> multiplayerOptionsList = MultiplayerOptions.Instance.GetMultiplayerOptionsList(base.OptionType);
			List<string> multiplayerOptionsTextList = MultiplayerOptions.Instance.GetMultiplayerOptionsTextList(base.OptionType);
			List<string> list = new List<string>();
			foreach (string text in multiplayerOptionsTextList)
			{
				list.Add(text);
			}
			int num = multiplayerOptionsList.IndexOf(MultiplayerOptions.Instance.GetValueTextForOptionWithMultipleSelection(base.OptionType));
			if (num != this.Selector.SelectedIndex)
			{
				this.Selector.SelectedIndex = num;
			}
			this.Selector.SetOnChangeAction(new Action<SelectorVM<SelectorItemVM>>(this.OnChangeSelected));
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x000157F0 File Offset: 0x000139F0
		public void RefreshList()
		{
			List<string> multiplayerOptionsList = MultiplayerOptions.Instance.GetMultiplayerOptionsList(base.OptionType);
			List<string> multiplayerOptionsTextList = MultiplayerOptions.Instance.GetMultiplayerOptionsTextList(base.OptionType);
			List<string> list = new List<string>();
			foreach (string text in multiplayerOptionsTextList)
			{
				list.Add(text);
			}
			this.Selector.Refresh(list, multiplayerOptionsList.IndexOf(MultiplayerOptions.Instance.GetValueTextForOptionWithMultipleSelection(base.OptionType)), new Action<SelectorVM<SelectorItemVM>>(this.OnChangeSelected));
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00015894 File Offset: 0x00013A94
		private void OnChangeSelected(SelectorVM<SelectorItemVM> selector)
		{
			if (selector.SelectedIndex < 0 || selector.SelectedIndex >= selector.ItemList.Count)
			{
				return;
			}
			string text = MultiplayerOptions.Instance.GetMultiplayerOptionsList(base.OptionType)[selector.SelectedIndex];
			MultiplayerOptions.Instance.SetValueForOptionWithMultipleSelectionFromText(base.OptionType, text);
			Action<MultipleSelectionHostGameOptionDataVM> onChangedSelection = this.OnChangedSelection;
			if (onChangedSelection == null)
			{
				return;
			}
			onChangedSelection(this);
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x000158FC File Offset: 0x00013AFC
		// (set) Token: 0x060006A3 RID: 1699 RVA: 0x00015904 File Offset: 0x00013B04
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> Selector
		{
			get
			{
				return this._selector;
			}
			set
			{
				if (value != this._selector)
				{
					this._selector = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "Selector");
				}
			}
		}

		// Token: 0x0400031F RID: 799
		public Action<MultipleSelectionHostGameOptionDataVM> OnChangedSelection;

		// Token: 0x04000320 RID: 800
		private SelectorVM<SelectorItemVM> _selector;
	}
}
