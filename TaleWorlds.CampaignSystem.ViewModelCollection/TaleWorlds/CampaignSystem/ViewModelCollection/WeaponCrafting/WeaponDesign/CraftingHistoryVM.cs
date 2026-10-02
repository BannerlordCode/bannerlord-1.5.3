using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000105 RID: 261
	public class CraftingHistoryVM : ViewModel
	{
		// Token: 0x06001769 RID: 5993 RVA: 0x0005AC48 File Offset: 0x00058E48
		public CraftingHistoryVM(Crafting crafting, ICraftingCampaignBehavior craftingBehavior, Func<CraftingOrder> getActiveOrder, Action<WeaponDesignSelectorVM> onDone)
		{
			this._crafting = crafting;
			this._craftingBehavior = craftingBehavior;
			this._getActiveOrder = getActiveOrder;
			this._onDone = onDone;
			this.CraftingHistory = new MBBindingList<WeaponDesignSelectorVM>();
			this.HistoryHint = new HintViewModel(CraftingHistoryVM._craftingHistoryText, null);
			this.HistoryDisabledHint = new HintViewModel(CraftingHistoryVM._noItemsHint, null);
			this.RefreshValues();
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x0005ACAC File Offset: 0x00058EAC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = CraftingHistoryVM._craftingHistoryText.ToString();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.RefreshAvailability();
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x0005AD04 File Offset: 0x00058F04
		private void RefreshCraftingHistory()
		{
			this.FinalizeHistory();
			CraftingOrder craftingOrder = this._getActiveOrder();
			foreach (WeaponDesign weaponDesign in this._craftingBehavior.CraftingHistory)
			{
				if (craftingOrder == null || weaponDesign.Template.TemplateName.ToString() == craftingOrder.PreCraftedWeaponDesignItem.WeaponDesign.Template.TemplateName.ToString())
				{
					this.CraftingHistory.Add(new WeaponDesignSelectorVM(weaponDesign, new Action<WeaponDesignSelectorVM>(this.ExecuteSelect)));
				}
			}
			this.HasItemsInHistory = this.CraftingHistory.Count > 0;
			this.ExecuteSelect(null);
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0005ADD0 File Offset: 0x00058FD0
		private void FinalizeHistory()
		{
			if (this.CraftingHistory.Count > 0)
			{
				foreach (WeaponDesignSelectorVM weaponDesignSelectorVM in this.CraftingHistory)
				{
					weaponDesignSelectorVM.OnFinalize();
				}
			}
			this.CraftingHistory.Clear();
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x0005AE34 File Offset: 0x00059034
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.FinalizeHistory();
			this.DoneKey.OnFinalize();
			this.CancelKey.OnFinalize();
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x0005AE58 File Offset: 0x00059058
		public void RefreshAvailability()
		{
			CraftingOrder activeOrder = this._getActiveOrder();
			this.HasItemsInHistory = ((activeOrder == null) ? (this._craftingBehavior.CraftingHistory.Count > 0) : this._craftingBehavior.CraftingHistory.Any<WeaponDesign>((WeaponDesign x) => x.Template.StringId == activeOrder.PreCraftedWeaponDesignItem.WeaponDesign.Template.StringId));
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x0005AEBB File Offset: 0x000590BB
		public void ExecuteOpen()
		{
			this.RefreshCraftingHistory();
			this.IsVisible = true;
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x0005AECA File Offset: 0x000590CA
		public void ExecuteCancel()
		{
			this.IsVisible = false;
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x0005AED3 File Offset: 0x000590D3
		public void ExecuteDone()
		{
			Action<WeaponDesignSelectorVM> onDone = this._onDone;
			if (onDone != null)
			{
				onDone(this.SelectedDesign);
			}
			this.ExecuteCancel();
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x0005AEF2 File Offset: 0x000590F2
		private void ExecuteSelect(WeaponDesignSelectorVM selector)
		{
			this.IsDoneAvailable = selector != null;
			if (this.SelectedDesign != null)
			{
				this.SelectedDesign.IsSelected = false;
			}
			this.SelectedDesign = selector;
			if (this.SelectedDesign != null)
			{
				this.SelectedDesign.IsSelected = true;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06001773 RID: 6003 RVA: 0x0005AF2D File Offset: 0x0005912D
		// (set) Token: 0x06001774 RID: 6004 RVA: 0x0005AF35 File Offset: 0x00059135
		[DataSourceProperty]
		public bool IsDoneAvailable
		{
			get
			{
				return this._isDoneAvailable;
			}
			set
			{
				if (value != this._isDoneAvailable)
				{
					this._isDoneAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsDoneAvailable");
				}
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06001775 RID: 6005 RVA: 0x0005AF53 File Offset: 0x00059153
		// (set) Token: 0x06001776 RID: 6006 RVA: 0x0005AF5B File Offset: 0x0005915B
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

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06001777 RID: 6007 RVA: 0x0005AF79 File Offset: 0x00059179
		// (set) Token: 0x06001778 RID: 6008 RVA: 0x0005AF81 File Offset: 0x00059181
		[DataSourceProperty]
		public bool HasItemsInHistory
		{
			get
			{
				return this._hasItemsInHistory;
			}
			set
			{
				if (value != this._hasItemsInHistory)
				{
					this._hasItemsInHistory = value;
					base.OnPropertyChangedWithValue(value, "HasItemsInHistory");
				}
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x0005AF9F File Offset: 0x0005919F
		// (set) Token: 0x0600177A RID: 6010 RVA: 0x0005AFA7 File Offset: 0x000591A7
		[DataSourceProperty]
		public HintViewModel HistoryHint
		{
			get
			{
				return this._historyHint;
			}
			set
			{
				if (value != this._historyHint)
				{
					this._historyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HistoryHint");
				}
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x0600177B RID: 6011 RVA: 0x0005AFC5 File Offset: 0x000591C5
		// (set) Token: 0x0600177C RID: 6012 RVA: 0x0005AFCD File Offset: 0x000591CD
		[DataSourceProperty]
		public HintViewModel HistoryDisabledHint
		{
			get
			{
				return this._historyDisabledHint;
			}
			set
			{
				if (value != this._historyDisabledHint)
				{
					this._historyDisabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HistoryDisabledHint");
				}
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x0600177D RID: 6013 RVA: 0x0005AFEB File Offset: 0x000591EB
		// (set) Token: 0x0600177E RID: 6014 RVA: 0x0005AFF3 File Offset: 0x000591F3
		[DataSourceProperty]
		public MBBindingList<WeaponDesignSelectorVM> CraftingHistory
		{
			get
			{
				return this._craftingHistory;
			}
			set
			{
				if (value != this._craftingHistory)
				{
					this._craftingHistory = value;
					base.OnPropertyChangedWithValue<MBBindingList<WeaponDesignSelectorVM>>(value, "CraftingHistory");
				}
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x0600177F RID: 6015 RVA: 0x0005B011 File Offset: 0x00059211
		// (set) Token: 0x06001780 RID: 6016 RVA: 0x0005B019 File Offset: 0x00059219
		[DataSourceProperty]
		public WeaponDesignSelectorVM SelectedDesign
		{
			get
			{
				return this._selectedDesign;
			}
			set
			{
				if (value != this._selectedDesign)
				{
					this._selectedDesign = value;
					base.OnPropertyChangedWithValue<WeaponDesignSelectorVM>(value, "SelectedDesign");
				}
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x0005B037 File Offset: 0x00059237
		// (set) Token: 0x06001782 RID: 6018 RVA: 0x0005B03F File Offset: 0x0005923F
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06001783 RID: 6019 RVA: 0x0005B062 File Offset: 0x00059262
		// (set) Token: 0x06001784 RID: 6020 RVA: 0x0005B06A File Offset: 0x0005926A
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06001785 RID: 6021 RVA: 0x0005B08D File Offset: 0x0005928D
		// (set) Token: 0x06001786 RID: 6022 RVA: 0x0005B095 File Offset: 0x00059295
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x0005B0B8 File Offset: 0x000592B8
		public void SetDoneKey(HotKey hotkey)
		{
			this.DoneKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x0005B0C7 File Offset: 0x000592C7
		public void SetCancelKey(HotKey hotkey)
		{
			this.CancelKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x0005B0D6 File Offset: 0x000592D6
		// (set) Token: 0x0600178A RID: 6026 RVA: 0x0005B0DE File Offset: 0x000592DE
		public InputKeyItemVM CancelKey
		{
			get
			{
				return this._cancelKey;
			}
			set
			{
				if (value != this._cancelKey)
				{
					this._cancelKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelKey");
				}
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x0005B0FC File Offset: 0x000592FC
		// (set) Token: 0x0600178C RID: 6028 RVA: 0x0005B104 File Offset: 0x00059304
		public InputKeyItemVM DoneKey
		{
			get
			{
				return this._doneKey;
			}
			set
			{
				if (value != this._doneKey)
				{
					this._doneKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneKey");
				}
			}
		}

		// Token: 0x04000AA5 RID: 2725
		private static TextObject _noItemsHint = new TextObject("{=saHYZKLt}There are no available items in history", null);

		// Token: 0x04000AA6 RID: 2726
		private static TextObject _craftingHistoryText = new TextObject("{=xW4BPVLX}Crafting History", null);

		// Token: 0x04000AA7 RID: 2727
		private ICraftingCampaignBehavior _craftingBehavior;

		// Token: 0x04000AA8 RID: 2728
		private Func<CraftingOrder> _getActiveOrder;

		// Token: 0x04000AA9 RID: 2729
		private Action<WeaponDesignSelectorVM> _onDone;

		// Token: 0x04000AAA RID: 2730
		private Crafting _crafting;

		// Token: 0x04000AAB RID: 2731
		private bool _isDoneAvailable;

		// Token: 0x04000AAC RID: 2732
		private bool _isVisible;

		// Token: 0x04000AAD RID: 2733
		private bool _hasItemsInHistory;

		// Token: 0x04000AAE RID: 2734
		private HintViewModel _historyHint;

		// Token: 0x04000AAF RID: 2735
		private HintViewModel _historyDisabledHint;

		// Token: 0x04000AB0 RID: 2736
		private MBBindingList<WeaponDesignSelectorVM> _craftingHistory;

		// Token: 0x04000AB1 RID: 2737
		private WeaponDesignSelectorVM _selectedDesign;

		// Token: 0x04000AB2 RID: 2738
		private string _titleText;

		// Token: 0x04000AB3 RID: 2739
		private string _doneText;

		// Token: 0x04000AB4 RID: 2740
		private string _cancelText;

		// Token: 0x04000AB5 RID: 2741
		private InputKeyItemVM _cancelKey;

		// Token: 0x04000AB6 RID: 2742
		private InputKeyItemVM _doneKey;
	}
}
