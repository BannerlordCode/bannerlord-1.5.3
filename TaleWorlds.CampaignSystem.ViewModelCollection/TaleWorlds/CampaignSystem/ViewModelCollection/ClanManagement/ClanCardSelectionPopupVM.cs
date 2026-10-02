using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000127 RID: 295
	public class ClanCardSelectionPopupVM : ViewModel
	{
		// Token: 0x06001A8E RID: 6798 RVA: 0x0006458C File Offset: 0x0006278C
		public ClanCardSelectionPopupVM()
		{
			this._titleText = TextObject.GetEmpty();
			this.Items = new MBBindingList<ClanCardSelectionPopupItemVM>();
			this.DisabledHint = new HintViewModel();
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x000645B8 File Offset: 0x000627B8
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (!this._isMultiSelection)
			{
				ClanCardSelectionPopupItemVM lastSelectedItem = this._lastSelectedItem;
				string text;
				if (lastSelectedItem == null)
				{
					text = null;
				}
				else
				{
					TextObject actionResultText = lastSelectedItem.ActionResultText;
					text = ((actionResultText != null) ? actionResultText.ToString() : null);
				}
				this.ActionResult = text ?? string.Empty;
			}
			this.DoneLbl = GameTexts.FindText("str_done", null).ToString();
			TextObject titleText = this._titleText;
			this.Title = ((titleText != null) ? titleText.ToString() : null) ?? string.Empty;
			this.Items.ApplyActionOnAllItems(delegate(ClanCardSelectionPopupItemVM x)
			{
				x.RefreshValues();
			});
			this.RefreshHintText();
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x00064668 File Offset: 0x00062868
		private void RefreshHintText()
		{
			TextObject textObject = TextObject.GetEmpty();
			if (this._isMultiSelection)
			{
				if (this._maximumSelection > 0 && this._selectedItemCount > this._maximumSelection)
				{
					textObject = new TextObject("{=lIGdkJGm}You must choose less than {NUMBER} {?NUMBER>1}items{?}item{\\?}", null);
					textObject.SetTextVariable("NUMBER", this._maximumSelection);
				}
				else if (this._selectedItemCount < this._minimumSelection)
				{
					textObject = new TextObject("{=woD234nb}You must choose more than {NUMBER} {?NUMBER>1}items{?}item{\\?}", null);
					textObject.SetTextVariable("NUMBER", this._minimumSelection);
				}
			}
			else if (this._selectedItemCount != 1)
			{
				textObject = new TextObject("{=aYm5Ehv1}You must choose an item", null);
			}
			this.DisabledHint.HintText = textObject;
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x00064709 File Offset: 0x00062909
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey == null)
			{
				return;
			}
			cancelInputKey.OnFinalize();
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x00064732 File Offset: 0x00062932
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x00064741 File Offset: 0x00062941
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x00064750 File Offset: 0x00062950
		public void Open(ClanCardSelectionInfo info)
		{
			this._isMultiSelection = info.IsMultiSelection;
			this._minimumSelection = info.MinimumSelection;
			this._maximumSelection = info.MaximumSelection;
			this._titleText = info.Title;
			this._onClosed = info.OnClosedAction;
			this._selectedItemCount = 0;
			foreach (ClanCardSelectionItemInfo clanCardSelectionItemInfo in info.Items)
			{
				this.Items.Add(new ClanCardSelectionPopupItemVM(in clanCardSelectionItemInfo, new Action<ClanCardSelectionPopupItemVM>(this.OnItemSelected)));
			}
			this.RefreshValues();
			this.IsVisible = true;
			this.UpdateIsDoneEnabled();
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x0006480C File Offset: 0x00062A0C
		public void ExecuteCancel()
		{
			Action<List<object>, Action> onClosed = this._onClosed;
			if (onClosed != null)
			{
				onClosed(new List<object>(), null);
			}
			this.Close();
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x0006482C File Offset: 0x00062A2C
		public void ExecuteDone()
		{
			List<object> selectedItems = new List<object>();
			this.Items.ApplyActionOnAllItems(delegate(ClanCardSelectionPopupItemVM x)
			{
				if (x.IsSelected)
				{
					selectedItems.Add(x.Identifier);
				}
			});
			Action<List<object>, Action> onClosed = this._onClosed;
			if (onClosed == null)
			{
				return;
			}
			onClosed(selectedItems, new Action(this.Close));
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x00064884 File Offset: 0x00062A84
		private void Close()
		{
			this.IsVisible = false;
			this._lastSelectedItem = null;
			this._titleText = TextObject.GetEmpty();
			this.ActionResult = string.Empty;
			this.Title = string.Empty;
			this._onClosed = null;
			this.Items.Clear();
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x000648D4 File Offset: 0x00062AD4
		private void OnItemSelected(ClanCardSelectionPopupItemVM item)
		{
			if (this._isMultiSelection)
			{
				item.IsSelected = !item.IsSelected;
				if (item.IsSelected)
				{
					this._selectedItemCount++;
				}
				else
				{
					this._selectedItemCount--;
				}
			}
			else if (item != this._lastSelectedItem)
			{
				if (this._lastSelectedItem != null)
				{
					this._lastSelectedItem.IsSelected = false;
				}
				item.IsSelected = true;
				TextObject actionResultText = item.ActionResultText;
				this.ActionResult = ((actionResultText != null) ? actionResultText.ToString() : null) ?? string.Empty;
				this._selectedItemCount = 1;
			}
			this._lastSelectedItem = item;
			this.UpdateIsDoneEnabled();
			this.RefreshHintText();
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00064980 File Offset: 0x00062B80
		private void UpdateIsDoneEnabled()
		{
			if (this._isMultiSelection)
			{
				this.IsDoneEnabled = this._selectedItemCount >= this._minimumSelection && (this._maximumSelection <= 0 || this._selectedItemCount <= this._maximumSelection);
				return;
			}
			this.IsDoneEnabled = this._selectedItemCount == 1;
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06001A9A RID: 6810 RVA: 0x000649D9 File Offset: 0x00062BD9
		// (set) Token: 0x06001A9B RID: 6811 RVA: 0x000649E1 File Offset: 0x00062BE1
		[DataSourceProperty]
		public MBBindingList<ClanCardSelectionPopupItemVM> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				if (value != this._items)
				{
					this._items = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanCardSelectionPopupItemVM>>(value, "Items");
				}
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06001A9C RID: 6812 RVA: 0x000649FF File Offset: 0x00062BFF
		// (set) Token: 0x06001A9D RID: 6813 RVA: 0x00064A07 File Offset: 0x00062C07
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06001A9E RID: 6814 RVA: 0x00064A25 File Offset: 0x00062C25
		// (set) Token: 0x06001A9F RID: 6815 RVA: 0x00064A2D File Offset: 0x00062C2D
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x00064A4B File Offset: 0x00062C4B
		// (set) Token: 0x06001AA1 RID: 6817 RVA: 0x00064A53 File Offset: 0x00062C53
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06001AA2 RID: 6818 RVA: 0x00064A76 File Offset: 0x00062C76
		// (set) Token: 0x06001AA3 RID: 6819 RVA: 0x00064A7E File Offset: 0x00062C7E
		[DataSourceProperty]
		public string ActionResult
		{
			get
			{
				return this._actionResult;
			}
			set
			{
				if (value != this._actionResult)
				{
					this._actionResult = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionResult");
				}
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06001AA4 RID: 6820 RVA: 0x00064AA1 File Offset: 0x00062CA1
		// (set) Token: 0x06001AA5 RID: 6821 RVA: 0x00064AA9 File Offset: 0x00062CA9
		[DataSourceProperty]
		public string DoneLbl
		{
			get
			{
				return this._doneLbl;
			}
			set
			{
				if (value != this._doneLbl)
				{
					this._doneLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneLbl");
				}
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x00064ACC File Offset: 0x00062CCC
		// (set) Token: 0x06001AA7 RID: 6823 RVA: 0x00064AD4 File Offset: 0x00062CD4
		[DataSourceProperty]
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value != this._isVisible)
				{
					this._isVisible = value;
					base.OnPropertyChangedWithValue(value, "IsVisible");
				}
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06001AA8 RID: 6824 RVA: 0x00064AF2 File Offset: 0x00062CF2
		// (set) Token: 0x06001AA9 RID: 6825 RVA: 0x00064AFA File Offset: 0x00062CFA
		[DataSourceProperty]
		public bool IsDoneEnabled
		{
			get
			{
				return this._isDoneEnabled;
			}
			set
			{
				if (value != this._isDoneEnabled)
				{
					this._isDoneEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsDoneEnabled");
				}
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06001AAA RID: 6826 RVA: 0x00064B18 File Offset: 0x00062D18
		// (set) Token: 0x06001AAB RID: 6827 RVA: 0x00064B20 File Offset: 0x00062D20
		[DataSourceProperty]
		public HintViewModel DisabledHint
		{
			get
			{
				return this._disabledHint;
			}
			set
			{
				if (value != this._disabledHint)
				{
					this._disabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisabledHint");
				}
			}
		}

		// Token: 0x04000C45 RID: 3141
		private TextObject _titleText;

		// Token: 0x04000C46 RID: 3142
		private bool _isMultiSelection;

		// Token: 0x04000C47 RID: 3143
		private int _minimumSelection;

		// Token: 0x04000C48 RID: 3144
		private int _maximumSelection;

		// Token: 0x04000C49 RID: 3145
		private ClanCardSelectionPopupItemVM _lastSelectedItem;

		// Token: 0x04000C4A RID: 3146
		private int _selectedItemCount;

		// Token: 0x04000C4B RID: 3147
		private Action<List<object>, Action> _onClosed;

		// Token: 0x04000C4C RID: 3148
		private MBBindingList<ClanCardSelectionPopupItemVM> _items;

		// Token: 0x04000C4D RID: 3149
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000C4E RID: 3150
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000C4F RID: 3151
		private string _title;

		// Token: 0x04000C50 RID: 3152
		private string _actionResult;

		// Token: 0x04000C51 RID: 3153
		private string _doneLbl;

		// Token: 0x04000C52 RID: 3154
		private bool _isVisible;

		// Token: 0x04000C53 RID: 3155
		private bool _isDoneEnabled;

		// Token: 0x04000C54 RID: 3156
		private HintViewModel _disabledHint;
	}
}
