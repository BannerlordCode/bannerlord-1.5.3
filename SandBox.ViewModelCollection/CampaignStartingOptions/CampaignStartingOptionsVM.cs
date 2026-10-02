using System;
using System.Collections.Generic;
using SandBox.AdvancedStartOptions;
using SandBox.ViewModelCollection.Input;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.CampaignStartingOptions
{
	// Token: 0x0200005E RID: 94
	public class CampaignStartingOptionsVM : ViewModel
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x000159A4 File Offset: 0x00013BA4
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x000159AC File Offset: 0x00013BAC
		public StartingOptionVM FocusedOption { get; private set; }

		// Token: 0x060005C8 RID: 1480 RVA: 0x000159B8 File Offset: 0x00013BB8
		public CampaignStartingOptionsVM(AdvancedStartOptions startOptions, Action<AdvancedStartOptions> onConfirm, Action onClose)
		{
			this._isASOEnabled = startOptions.HasAnyChange();
			this._onConfirm = onConfirm;
			this._onClose = onClose;
			this._stagedOptions = startOptions;
			this.Categories = new MBBindingList<StartingOptionCategoryVM>();
			this.BuildCategories();
			this.RelevantOptionTexts = new MBBindingList<StartingOptionTitleDescriptionTupleVM>();
			StartingOptionVM.OnOptionFocusBegin += this.OnOptionFocusBegin;
			StartingOptionVM.OnOptionFocusEnd += this.OnOptionFocusEnd;
			StartingOptionVM.OnOptionChanged += this.OnOptionChanged;
			this.OnOptionChanged();
			this.RefreshValues();
			this._isInitialized = true;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00015A50 File Offset: 0x00013C50
		private void BuildCategories()
		{
			Dictionary<string, StartingOptionCategoryVM> dictionary = new Dictionary<string, StartingOptionCategoryVM>();
			TextObject textObject = Module.CurrentModule.GlobalTextManager.FindText("str_campaign_starting_options_category_name", "general");
			StartingOptionCategoryVM startingOptionCategoryVM = CampaignStartingOptionsVM.CreateCategory("general", textObject);
			startingOptionCategoryVM.Options.Add(new StartingOptionVM("EnableAdvancedStartingOptions", () => this._isASOEnabled, delegate(bool value)
			{
				this.OnASOOptionToggled(value);
			}));
			dictionary.Add("general", startingOptionCategoryVM);
			this.Categories.Add(startingOptionCategoryVM);
			IReadOnlyList<AdvancedStartOption> allOptions = this._stagedOptions.GetAllOptions();
			for (int i = 0; i < allOptions.Count; i++)
			{
				AdvancedStartOption advancedStartOption = allOptions[i];
				string categoryId = advancedStartOption.CategoryId;
				if (string.IsNullOrEmpty(categoryId))
				{
					Debug.FailedAssert("Empty category id", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\CampaignStartingOptions\\CampaignStartingOptionsVM.cs", "BuildCategories", 67);
				}
				else
				{
					StartingOptionCategoryVM startingOptionCategoryVM2;
					if (!dictionary.TryGetValue(categoryId, out startingOptionCategoryVM2))
					{
						TextObject textObject2 = Module.CurrentModule.GlobalTextManager.FindText("str_campaign_starting_options_category_name", categoryId);
						startingOptionCategoryVM2 = CampaignStartingOptionsVM.CreateCategory(categoryId, textObject2);
						dictionary.Add(categoryId, startingOptionCategoryVM2);
						this.Categories.Add(startingOptionCategoryVM2);
					}
					StartingOptionVM startingOptionVM = new StartingOptionVM(advancedStartOption, this._stagedOptions, () => !this._isASOEnabled);
					if (advancedStartOption.StringId == "Seed")
					{
						startingOptionVM.AllowRandomization = true;
						if (!this._stagedOptions.HasAnyChange())
						{
							startingOptionVM.ExecuteRandomize();
						}
					}
					startingOptionCategoryVM2.Options.Add(startingOptionVM);
				}
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00015BCA File Offset: 0x00013DCA
		private static StartingOptionCategoryVM CreateCategory(string categoryId, TextObject categoryName)
		{
			if (categoryId == "general")
			{
				return new GeneralCategoryVM(categoryId, categoryName);
			}
			if (!(categoryId == "globalmodifiers"))
			{
				return new StartingOptionCategoryVM(categoryId, categoryName);
			}
			return new GlobalModifiersCategoryVM(categoryId, categoryName);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00015C00 File Offset: 0x00013E00
		private void OnASOOptionToggled(bool newValue)
		{
			if (!this._isInitialized)
			{
				return;
			}
			this._isASOEnabled = newValue;
			if (newValue)
			{
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=LW8QCm22}Notice", null).ToString(), new TextObject("{=7AZYcnV4}Using the Advanced Starting Options can affect game balance, progression, AI behavior, difficulty etc. Recommended for experienced players only.", null).ToString(), true, false, new TextObject("{=DM6luo3c}Continue", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00015C70 File Offset: 0x00013E70
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleLabel = new TextObject("{=LuaC2Lz2}Advanced Starting Options", null).ToString();
			this.StartGameLabel = new TextObject("{=lBQXP6Wj}Start Game", null).ToString();
			this.Categories.ApplyActionOnAllItems(delegate(StartingOptionCategoryVM x)
			{
				x.RefreshValues();
			});
			this.RefreshRelevantOptionTexts();
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00015CE0 File Offset: 0x00013EE0
		public void SetRandomizeInputKey(HotKey hotkey)
		{
			for (int i = 0; i < this.Categories.Count; i++)
			{
				StartingOptionCategoryVM startingOptionCategoryVM = this.Categories[i];
				for (int j = 0; j < startingOptionCategoryVM.Options.Count; j++)
				{
					StartingOptionVM startingOptionVM = startingOptionCategoryVM.Options[j];
					if (startingOptionVM.AllowRandomization)
					{
						startingOptionVM.SetRandomizeInputKey(hotkey);
					}
				}
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00015D42 File Offset: 0x00013F42
		private void OnOptionFocusBegin(StartingOptionVM optionVM)
		{
			if (optionVM == null)
			{
				return;
			}
			this.FocusedOption = optionVM;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00015D4F File Offset: 0x00013F4F
		private void OnOptionFocusEnd(StartingOptionVM optionVM)
		{
			if (optionVM == null || this.FocusedOption != optionVM)
			{
				return;
			}
			this.FocusedOption = null;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00015D65 File Offset: 0x00013F65
		public void ExecuteConfirm()
		{
			Action<AdvancedStartOptions> onConfirm = this._onConfirm;
			if (onConfirm == null)
			{
				return;
			}
			onConfirm(this._isASOEnabled ? this._stagedOptions : new AdvancedStartOptions());
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00015D8C File Offset: 0x00013F8C
		public void ExecuteCancel()
		{
			Action onClose = this._onClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00015D9E File Offset: 0x00013F9E
		private void OnOptionChanged()
		{
			this.Categories.ApplyActionOnAllItems(delegate(StartingOptionCategoryVM x)
			{
				x.UpdateOptionStates();
			});
			this.RefreshRelevantOptionTexts();
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00015DD0 File Offset: 0x00013FD0
		private void RefreshRelevantOptionTexts()
		{
			this.RelevantOptionTexts.Clear();
			for (int i = 0; i < this.Categories.Count; i++)
			{
				StartingOptionCategoryVM startingOptionCategoryVM = this.Categories[i];
				if (!string.IsNullOrEmpty(startingOptionCategoryVM.DescriptionText))
				{
					this.RelevantOptionTexts.Add(new StartingOptionTitleDescriptionTupleVM(startingOptionCategoryVM.Name, startingOptionCategoryVM.DescriptionText));
				}
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00015E34 File Offset: 0x00014034
		public override void OnFinalize()
		{
			base.OnFinalize();
			StartingOptionVM.OnOptionFocusBegin -= this.OnOptionFocusBegin;
			StartingOptionVM.OnOptionFocusEnd -= this.OnOptionFocusEnd;
			StartingOptionVM.OnOptionChanged -= this.OnOptionChanged;
			this.Categories.ApplyActionOnAllItems(delegate(StartingOptionCategoryVM x)
			{
				x.OnFinalize();
			});
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

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00015EC5 File Offset: 0x000140C5
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x00015ECD File Offset: 0x000140CD
		[DataSourceProperty]
		public MBBindingList<StartingOptionCategoryVM> Categories
		{
			get
			{
				return this._categories;
			}
			set
			{
				if (value != this._categories)
				{
					this._categories = value;
					base.OnPropertyChangedWithValue<MBBindingList<StartingOptionCategoryVM>>(value, "Categories");
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x00015EEB File Offset: 0x000140EB
		// (set) Token: 0x060005D8 RID: 1496 RVA: 0x00015EF3 File Offset: 0x000140F3
		[DataSourceProperty]
		public string TitleLabel
		{
			get
			{
				return this._titleLabel;
			}
			set
			{
				if (value != this._titleLabel)
				{
					this._titleLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleLabel");
				}
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x00015F16 File Offset: 0x00014116
		// (set) Token: 0x060005DA RID: 1498 RVA: 0x00015F1E File Offset: 0x0001411E
		[DataSourceProperty]
		public string StartGameLabel
		{
			get
			{
				return this._startGameLabel;
			}
			set
			{
				if (value != this._startGameLabel)
				{
					this._startGameLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "StartGameLabel");
				}
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x00015F41 File Offset: 0x00014141
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x00015F49 File Offset: 0x00014149
		[DataSourceProperty]
		public MBBindingList<StartingOptionTitleDescriptionTupleVM> RelevantOptionTexts
		{
			get
			{
				return this._relevantOptionTexts;
			}
			set
			{
				if (value != this._relevantOptionTexts)
				{
					this._relevantOptionTexts = value;
					base.OnPropertyChangedWithValue<MBBindingList<StartingOptionTitleDescriptionTupleVM>>(value, "RelevantOptionTexts");
				}
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00015F67 File Offset: 0x00014167
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00015F76 File Offset: 0x00014176
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x00015F85 File Offset: 0x00014185
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x00015F8D File Offset: 0x0001418D
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

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x00015FAB File Offset: 0x000141AB
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x00015FB3 File Offset: 0x000141B3
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

		// Token: 0x040002E8 RID: 744
		private readonly Action<AdvancedStartOptions> _onConfirm;

		// Token: 0x040002E9 RID: 745
		private readonly Action _onClose;

		// Token: 0x040002EA RID: 746
		private readonly AdvancedStartOptions _stagedOptions;

		// Token: 0x040002EB RID: 747
		private bool _isASOEnabled;

		// Token: 0x040002EC RID: 748
		private bool _isInitialized;

		// Token: 0x040002ED RID: 749
		private MBBindingList<StartingOptionCategoryVM> _categories;

		// Token: 0x040002EE RID: 750
		private string _titleLabel;

		// Token: 0x040002EF RID: 751
		private string _startGameLabel;

		// Token: 0x040002F0 RID: 752
		private MBBindingList<StartingOptionTitleDescriptionTupleVM> _relevantOptionTexts;

		// Token: 0x040002F1 RID: 753
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040002F2 RID: 754
		private InputKeyItemVM _cancelInputKey;
	}
}
