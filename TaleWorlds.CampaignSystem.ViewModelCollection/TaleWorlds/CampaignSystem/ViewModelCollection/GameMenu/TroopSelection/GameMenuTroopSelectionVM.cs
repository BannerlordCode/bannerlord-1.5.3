using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TroopSelection
{
	// Token: 0x020000A5 RID: 165
	public class GameMenuTroopSelectionVM : ViewModel
	{
		// Token: 0x06000F9A RID: 3994 RVA: 0x00041138 File Offset: 0x0003F338
		public GameMenuTroopSelectionVM(TroopRoster fullRoster, TroopRoster initialSelections, Func<CharacterObject, bool> canChangeChangeStatusOfTroop, Action<TroopRoster> onDone, int maxSelectableTroopCount, int minSelectableTroopCount)
		{
			this._canChangeChangeStatusOfTroop = canChangeChangeStatusOfTroop;
			this._onDone = onDone;
			this._fullRoster = fullRoster;
			this._initialSelections = initialSelections;
			this._maxSelectableTroopCount = maxSelectableTroopCount;
			this._minSelectableTroopCount = minSelectableTroopCount;
			this.DoneHint = new HintViewModel();
			this.InitList();
			this.RefreshValues();
			this.OnCurrentSelectedAmountChange();
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x000411B8 File Offset: 0x0003F3B8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = this._titleTextObject.ToString();
			this.CurrentSelectedAmountTitle = this._chosenTitleTextObject.ToString();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.ClearSelectionText = new TextObject("{=QMNWbmao}Clear Selection", null).ToString();
			this.RefreshDoneHint();
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00041238 File Offset: 0x0003F438
		protected virtual void RefreshDoneHint()
		{
			if (this.IsDoneEnabled)
			{
				this.DoneHint.HintText = TextObject.GetEmpty();
				return;
			}
			if (this._currentTotalSelectedTroopCount < this._minSelectableTroopCount)
			{
				this.DoneHint.HintText = new TextObject("{=LlV29O9B}You must select at least {TROOP_COUNT} troops", null).SetTextVariable("TROOP_COUNT", this._minSelectableTroopCount);
				return;
			}
			this.DoneHint.HintText = new TextObject("{=TdWQM7QZ}You must select less than {TROOP_COUNT} troops", null).SetTextVariable("TROOP_COUNT", this._maxSelectableTroopCount);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x000412BC File Offset: 0x0003F4BC
		protected void InitList()
		{
			this.Troops = new MBBindingList<TroopSelectionItemVM>();
			this._currentTotalSelectedTroopCount = 0;
			foreach (TroopRosterElement troopRosterElement in this._fullRoster.GetTroopRoster())
			{
				TroopSelectionItemVM troopSelectionItemVM = new TroopSelectionItemVM(troopRosterElement, new Action<TroopSelectionItemVM>(this.OnAddTroop), new Action<TroopSelectionItemVM>(this.OnRemoveTroop));
				troopSelectionItemVM.IsLocked = !this._canChangeChangeStatusOfTroop(troopRosterElement.Character) || troopRosterElement.Number - troopRosterElement.WoundedNumber <= 0;
				this.Troops.Add(troopSelectionItemVM);
				int troopCount = this._initialSelections.GetTroopCount(troopRosterElement.Character);
				if (troopCount > 0)
				{
					troopSelectionItemVM.CurrentAmount = troopCount;
					this._currentTotalSelectedTroopCount += troopCount;
				}
			}
			this.Troops.Sort(new TroopItemComparer());
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x000413BC File Offset: 0x0003F5BC
		private void OnRemoveTroop(TroopSelectionItemVM troopItem)
		{
			if (troopItem.CurrentAmount > 0)
			{
				int num = 1;
				if (this.IsEntireStackModifierActive)
				{
					num = troopItem.CurrentAmount;
				}
				else if (this.IsFiveStackModifierActive)
				{
					num = MathF.Min(troopItem.CurrentAmount, 5);
				}
				troopItem.CurrentAmount -= num;
				this._currentTotalSelectedTroopCount -= num;
			}
			this.OnCurrentSelectedAmountChange();
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0004141C File Offset: 0x0003F61C
		private void OnAddTroop(TroopSelectionItemVM troopItem)
		{
			if (troopItem.CurrentAmount < troopItem.MaxAmount && this._currentTotalSelectedTroopCount < this._maxSelectableTroopCount)
			{
				int num = 1;
				if (this.IsEntireStackModifierActive)
				{
					num = MathF.Min(troopItem.MaxAmount - troopItem.CurrentAmount, this._maxSelectableTroopCount - this._currentTotalSelectedTroopCount);
				}
				else if (this.IsFiveStackModifierActive)
				{
					num = MathF.Min(MathF.Min(troopItem.MaxAmount - troopItem.CurrentAmount, this._maxSelectableTroopCount - this._currentTotalSelectedTroopCount), 5);
				}
				troopItem.CurrentAmount += num;
				this._currentTotalSelectedTroopCount += num;
			}
			this.OnCurrentSelectedAmountChange();
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x000414C4 File Offset: 0x0003F6C4
		protected virtual void OnCurrentSelectedAmountChange()
		{
			foreach (TroopSelectionItemVM troopSelectionItemVM in this.Troops)
			{
				troopSelectionItemVM.IsRosterFull = this._currentTotalSelectedTroopCount >= this._maxSelectableTroopCount;
			}
			GameTexts.SetVariable("LEFT", this._currentTotalSelectedTroopCount);
			GameTexts.SetVariable("RIGHT", this._maxSelectableTroopCount);
			this.CurrentSelectedAmountText = GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null).ToString();
			this.IsDoneEnabled = this._currentTotalSelectedTroopCount <= this._maxSelectableTroopCount && this._currentTotalSelectedTroopCount >= this._minSelectableTroopCount;
			this.RefreshDoneHint();
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00041584 File Offset: 0x0003F784
		protected TroopRoster BuildSelectedTroopRoster()
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (TroopSelectionItemVM troopSelectionItemVM in this.Troops)
			{
				if (troopSelectionItemVM.CurrentAmount > 0)
				{
					troopRoster.AddToCounts(troopSelectionItemVM.Troop.Character, troopSelectionItemVM.CurrentAmount, false, 0, 0, true, -1);
				}
			}
			return troopRoster;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x000415F8 File Offset: 0x0003F7F8
		protected virtual void OnDone()
		{
			TroopRoster troopRoster = this.BuildSelectedTroopRoster();
			this.IsEnabled = false;
			this._onDone.DynamicInvokeWithLog(new object[] { troopRoster });
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00041629 File Offset: 0x0003F829
		protected void UpdateMaxSelectableTroopCount(int maxValue)
		{
			this._maxSelectableTroopCount = maxValue;
			this.OnCurrentSelectedAmountChange();
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00041638 File Offset: 0x0003F838
		public void ExecuteDone()
		{
			TextObject warningMessageOnDone = this.GetWarningMessageOnDone();
			if (!TextObject.IsNullOrEmpty(warningMessageOnDone))
			{
				string text = warningMessageOnDone.ToString();
				InformationManager.ShowInquiry(new InquiryData(this.TitleText, text, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.OnDone), null, "", 0f, null, null, null), false, false);
				return;
			}
			this.OnDone();
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x000416B2 File Offset: 0x0003F8B2
		protected virtual TextObject GetWarningMessageOnDone()
		{
			if (this.GetAvailableSelectableTroopCount() > 0)
			{
				return new TextObject("{=z2Slmx4N}There are still some room for more soldiers. Do you want to proceed?", null);
			}
			return null;
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x000416CC File Offset: 0x0003F8CC
		protected int GetAvailableSelectableTroopCount()
		{
			int num = 0;
			foreach (TroopSelectionItemVM troopSelectionItemVM in this.Troops)
			{
				if (!troopSelectionItemVM.IsLocked && troopSelectionItemVM.CurrentAmount < troopSelectionItemVM.MaxAmount)
				{
					num += troopSelectionItemVM.MaxAmount - troopSelectionItemVM.CurrentAmount;
				}
			}
			if (this._currentTotalSelectedTroopCount + num > this._maxSelectableTroopCount)
			{
				num = this._maxSelectableTroopCount - this._currentTotalSelectedTroopCount;
			}
			return num;
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0004175C File Offset: 0x0003F95C
		public void ExecuteCancel()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00041765 File Offset: 0x0003F965
		public virtual void ExecuteReset()
		{
			this.InitList();
			this.OnCurrentSelectedAmountChange();
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00041773 File Offset: 0x0003F973
		public virtual void ExecuteClearSelection()
		{
			this.Troops.ApplyActionOnAllItems(delegate(TroopSelectionItemVM troopItem)
			{
				if (this._canChangeChangeStatusOfTroop(troopItem.Troop.Character))
				{
					int currentAmount = troopItem.CurrentAmount;
					for (int i = 0; i < currentAmount; i++)
					{
						troopItem.ExecuteRemove();
					}
				}
			});
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0004178C File Offset: 0x0003F98C
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM resetInputKey = this.ResetInputKey;
			if (resetInputKey == null)
			{
				return;
			}
			resetInputKey.OnFinalize();
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x000417C6 File Offset: 0x0003F9C6
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x000417D5 File Offset: 0x0003F9D5
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x000417E4 File Offset: 0x0003F9E4
		public void SetResetInputKey(HotKey hotkey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06000FAE RID: 4014 RVA: 0x000417F3 File Offset: 0x0003F9F3
		// (set) Token: 0x06000FAF RID: 4015 RVA: 0x000417FB File Offset: 0x0003F9FB
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

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x00041819 File Offset: 0x0003FA19
		// (set) Token: 0x06000FB1 RID: 4017 RVA: 0x00041821 File Offset: 0x0003FA21
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

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0004183F File Offset: 0x0003FA3F
		// (set) Token: 0x06000FB3 RID: 4019 RVA: 0x00041847 File Offset: 0x0003FA47
		[DataSourceProperty]
		public InputKeyItemVM ResetInputKey
		{
			get
			{
				return this._resetInputKey;
			}
			set
			{
				if (value != this._resetInputKey)
				{
					this._resetInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ResetInputKey");
				}
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x00041865 File Offset: 0x0003FA65
		// (set) Token: 0x06000FB5 RID: 4021 RVA: 0x0004186D File Offset: 0x0003FA6D
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

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x0004188B File Offset: 0x0003FA8B
		// (set) Token: 0x06000FB7 RID: 4023 RVA: 0x00041893 File Offset: 0x0003FA93
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

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x000418B1 File Offset: 0x0003FAB1
		// (set) Token: 0x06000FB9 RID: 4025 RVA: 0x000418B9 File Offset: 0x0003FAB9
		[DataSourceProperty]
		public HintViewModel DoneHint
		{
			get
			{
				return this._doneHint;
			}
			set
			{
				if (value != this._doneHint)
				{
					this._doneHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DoneHint");
				}
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x000418D7 File Offset: 0x0003FAD7
		// (set) Token: 0x06000FBB RID: 4027 RVA: 0x000418DF File Offset: 0x0003FADF
		[DataSourceProperty]
		public MBBindingList<TroopSelectionItemVM> Troops
		{
			get
			{
				return this._troops;
			}
			set
			{
				if (value != this._troops)
				{
					this._troops = value;
					base.OnPropertyChangedWithValue<MBBindingList<TroopSelectionItemVM>>(value, "Troops");
				}
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x000418FD File Offset: 0x0003FAFD
		// (set) Token: 0x06000FBD RID: 4029 RVA: 0x00041905 File Offset: 0x0003FB05
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

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x00041928 File Offset: 0x0003FB28
		// (set) Token: 0x06000FBF RID: 4031 RVA: 0x00041930 File Offset: 0x0003FB30
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

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x00041953 File Offset: 0x0003FB53
		// (set) Token: 0x06000FC1 RID: 4033 RVA: 0x0004195B File Offset: 0x0003FB5B
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

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0004197E File Offset: 0x0003FB7E
		// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x00041986 File Offset: 0x0003FB86
		[DataSourceProperty]
		public string ClearSelectionText
		{
			get
			{
				return this._clearSelectionText;
			}
			set
			{
				if (value != this._clearSelectionText)
				{
					this._clearSelectionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClearSelectionText");
				}
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x000419A9 File Offset: 0x0003FBA9
		// (set) Token: 0x06000FC5 RID: 4037 RVA: 0x000419B1 File Offset: 0x0003FBB1
		[DataSourceProperty]
		public string CurrentSelectedAmountText
		{
			get
			{
				return this._currentSelectedAmountText;
			}
			set
			{
				if (value != this._currentSelectedAmountText)
				{
					this._currentSelectedAmountText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentSelectedAmountText");
				}
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x000419D4 File Offset: 0x0003FBD4
		// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x000419DC File Offset: 0x0003FBDC
		[DataSourceProperty]
		public string CurrentSelectedAmountTitle
		{
			get
			{
				return this._currentSelectedAmountTitle;
			}
			set
			{
				if (value != this._currentSelectedAmountTitle)
				{
					this._currentSelectedAmountTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentSelectedAmountTitle");
				}
			}
		}

		// Token: 0x04000717 RID: 1815
		private readonly Action<TroopRoster> _onDone;

		// Token: 0x04000718 RID: 1816
		private readonly TroopRoster _fullRoster;

		// Token: 0x04000719 RID: 1817
		private readonly TroopRoster _initialSelections;

		// Token: 0x0400071A RID: 1818
		private readonly Func<CharacterObject, bool> _canChangeChangeStatusOfTroop;

		// Token: 0x0400071B RID: 1819
		private int _maxSelectableTroopCount;

		// Token: 0x0400071C RID: 1820
		private readonly int _minSelectableTroopCount;

		// Token: 0x0400071D RID: 1821
		private readonly TextObject _titleTextObject = new TextObject("{=uQgNPJnc}Manage Troops", null);

		// Token: 0x0400071E RID: 1822
		private readonly TextObject _chosenTitleTextObject = new TextObject("{=InqmgBiF}Chosen Crew", null);

		// Token: 0x0400071F RID: 1823
		private int _currentTotalSelectedTroopCount;

		// Token: 0x04000720 RID: 1824
		public bool IsFiveStackModifierActive;

		// Token: 0x04000721 RID: 1825
		public bool IsEntireStackModifierActive;

		// Token: 0x04000722 RID: 1826
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000723 RID: 1827
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000724 RID: 1828
		private InputKeyItemVM _resetInputKey;

		// Token: 0x04000725 RID: 1829
		private bool _isEnabled;

		// Token: 0x04000726 RID: 1830
		private bool _isDoneEnabled;

		// Token: 0x04000727 RID: 1831
		private HintViewModel _doneHint;

		// Token: 0x04000728 RID: 1832
		private string _doneText;

		// Token: 0x04000729 RID: 1833
		private string _cancelText;

		// Token: 0x0400072A RID: 1834
		private string _titleText;

		// Token: 0x0400072B RID: 1835
		private string _clearSelectionText;

		// Token: 0x0400072C RID: 1836
		private string _currentSelectedAmountText;

		// Token: 0x0400072D RID: 1837
		private string _currentSelectedAmountTitle;

		// Token: 0x0400072E RID: 1838
		private MBBindingList<TroopSelectionItemVM> _troops;
	}
}
