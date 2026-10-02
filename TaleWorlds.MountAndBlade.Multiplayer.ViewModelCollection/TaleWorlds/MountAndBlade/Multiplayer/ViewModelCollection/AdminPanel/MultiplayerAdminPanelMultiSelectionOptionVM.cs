using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000B1 RID: 177
	public class MultiplayerAdminPanelMultiSelectionOptionVM : MultiplayerAdminPanelOptionBaseVM
	{
		// Token: 0x060010F7 RID: 4343 RVA: 0x0003573E File Offset: 0x0003393E
		public MultiplayerAdminPanelMultiSelectionOptionVM(IAdminPanelMultiSelectionOption option)
			: base(option)
		{
			this._option = option;
			this.MultiSelectionOptions = new MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorVM(-1, null);
			this.IsMultiSelectionOption = true;
			this.RefreshValues();
			this._initialValue = this.MultiSelectionOptions.SelectedItem;
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00035779 File Offset: 0x00033979
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RefreshOptions();
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00035787 File Offset: 0x00033987
		private void OnSelectorChange(SelectorVM<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM> selector)
		{
			IAdminPanelOption<IAdminPanelMultiSelectionItem> option = this._option;
			MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM selectedItem = selector.SelectedItem;
			option.SetValue((selectedItem != null) ? selectedItem.SelectionItem : null);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x000357A8 File Offset: 0x000339A8
		public override void UpdateValues()
		{
			base.UpdateValues();
			if (!this._option.GetAvailableOptions().SequenceEqual<IAdminPanelMultiSelectionItem>(this.MultiSelectionOptions.ItemList.Select<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM, IAdminPanelMultiSelectionItem>((MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM i) => i.SelectionItem)))
			{
				this.RefreshOptions();
			}
			for (int j = 0; j < this.MultiSelectionOptions.ItemList.Count; j++)
			{
				if (this.MultiSelectionOptions.ItemList[j].SelectionItem == this._option.GetValue())
				{
					this.MultiSelectionOptions.SelectedIndex = j;
					return;
				}
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0003584D File Offset: 0x00033A4D
		public override void ExecuteRestoreDefaults()
		{
			base.ExecuteRestoreDefaults();
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00035855 File Offset: 0x00033A55
		public override void ExecuteRevertChanges()
		{
			base.ExecuteRevertChanges();
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00035860 File Offset: 0x00033A60
		private void RefreshOptions()
		{
			if (this.MultiSelectionOptions == null)
			{
				return;
			}
			List<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM> list = new List<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM>();
			if (this._option == null)
			{
				this.MultiSelectionOptions.Refresh(list, 0, new Action<SelectorVM<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM>>(this.OnSelectorChange));
				return;
			}
			IAdminPanelMultiSelectionItem value = this._option.GetValue();
			MBReadOnlyList<IAdminPanelMultiSelectionItem> mbreadOnlyList = this._option.GetAvailableOptions() ?? new MBReadOnlyList<IAdminPanelMultiSelectionItem>();
			if (mbreadOnlyList != null)
			{
				for (int i = 0; i < mbreadOnlyList.Count; i++)
				{
					list.Add(new MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM(mbreadOnlyList[i]));
				}
			}
			this.MultiSelectionOptions.IsEnabled = !base.IsDisabled;
			this.MultiSelectionOptions.Refresh(list, 0, new Action<SelectorVM<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM>>(this.OnSelectorChange));
			if (this.MultiSelectionOptions != null)
			{
				for (int j = 0; j < this.MultiSelectionOptions.ItemList.Count; j++)
				{
					if (this.MultiSelectionOptions.ItemList[j].SelectionItem == value)
					{
						this.MultiSelectionOptions.SelectedIndex = j;
						return;
					}
				}
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060010FE RID: 4350 RVA: 0x00035961 File Offset: 0x00033B61
		// (set) Token: 0x060010FF RID: 4351 RVA: 0x00035969 File Offset: 0x00033B69
		[DataSourceProperty]
		public bool IsMultiSelectionOption
		{
			get
			{
				return this._isMultiSelectionOption;
			}
			set
			{
				if (value != this._isMultiSelectionOption)
				{
					this._isMultiSelectionOption = value;
					base.OnPropertyChangedWithValue(value, "IsMultiSelectionOption");
				}
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x00035987 File Offset: 0x00033B87
		// (set) Token: 0x06001101 RID: 4353 RVA: 0x0003598F File Offset: 0x00033B8F
		[DataSourceProperty]
		public MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorVM MultiSelectionOptions
		{
			get
			{
				return this._multiSelectionOptions;
			}
			set
			{
				if (value != this._multiSelectionOptions)
				{
					this._multiSelectionOptions = value;
					base.OnPropertyChangedWithValue<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorVM>(value, "MultiSelectionOptions");
				}
			}
		}

		// Token: 0x040007FA RID: 2042
		private new readonly IAdminPanelMultiSelectionOption _option;

		// Token: 0x040007FB RID: 2043
		private readonly SelectorItemVM _initialValue;

		// Token: 0x040007FC RID: 2044
		private bool _isMultiSelectionOption;

		// Token: 0x040007FD RID: 2045
		private MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorVM _multiSelectionOptions;

		// Token: 0x0200019E RID: 414
		public class AdminPanelOptionSelectorVM : SelectorVM<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM>
		{
			// Token: 0x060013AC RID: 5036 RVA: 0x0003E7AD File Offset: 0x0003C9AD
			public AdminPanelOptionSelectorVM(int selectedIndex, Action<SelectorVM<MultiplayerAdminPanelMultiSelectionOptionVM.AdminPanelOptionSelectorItemVM>> onChange)
				: base(selectedIndex, onChange)
			{
			}

			// Token: 0x170005D4 RID: 1492
			// (get) Token: 0x060013AD RID: 5037 RVA: 0x0003E7B7 File Offset: 0x0003C9B7
			// (set) Token: 0x060013AE RID: 5038 RVA: 0x0003E7BF File Offset: 0x0003C9BF
			[DataSourceProperty]
			public bool IsEnabled
			{
				get
				{
					return this._isEnabled;
				}
				set
				{
					if (value != this._isEnabled)
					{
						this._isEnabled = value;
						base.OnPropertyChangedWithValue(value, "IsEnabled");
					}
				}
			}

			// Token: 0x04000ADA RID: 2778
			private bool _isEnabled;
		}

		// Token: 0x0200019F RID: 415
		public class AdminPanelOptionSelectorItemVM : SelectorItemVM
		{
			// Token: 0x060013AF RID: 5039 RVA: 0x0003E7DD File Offset: 0x0003C9DD
			public AdminPanelOptionSelectorItemVM(IAdminPanelMultiSelectionItem selectionItem)
				: base(((selectionItem != null) ? selectionItem.DisplayName : null) ?? ((selectionItem != null) ? selectionItem.Value : null))
			{
				this.SelectionItem = selectionItem;
			}

			// Token: 0x04000ADB RID: 2779
			public readonly IAdminPanelMultiSelectionItem SelectionItem;
		}
	}
}
