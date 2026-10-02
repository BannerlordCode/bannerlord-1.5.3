using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000B3 RID: 179
	public abstract class MultiplayerAdminPanelOptionBaseVM : ViewModel
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x0600110D RID: 4365 RVA: 0x00035B90 File Offset: 0x00033D90
		// (remove) Token: 0x0600110E RID: 4366 RVA: 0x00035BC4 File Offset: 0x00033DC4
		public static event Action<MultiplayerAdminPanelOptionBaseVM> OnOptionRefreshed;

		// Token: 0x0600110F RID: 4367 RVA: 0x00035BF8 File Offset: 0x00033DF8
		protected MultiplayerAdminPanelOptionBaseVM(IAdminPanelOption option)
		{
			this._option = option;
			IAdminPanelOption option2 = this._option;
			if (option2 != null)
			{
				option2.SetOnRefreshCallback(new Action(this.OnOptionRefreshedAux));
			}
			this.RequiresRestart = option != null && option.RequiresMissionRestart;
			this.RefreshValues();
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00035C48 File Offset: 0x00033E48
		public override void RefreshValues()
		{
			base.RefreshValues();
			IAdminPanelOption option = this._option;
			string text;
			if (option == null)
			{
				text = null;
			}
			else
			{
				string name = option.Name;
				text = ((name != null) ? name.ToString() : null);
			}
			this.OptionTitle = text;
			IAdminPanelOption option2 = this._option;
			string text2;
			if (option2 == null)
			{
				text2 = null;
			}
			else
			{
				string description = option2.Description;
				text2 = ((description != null) ? description.ToString() : null);
			}
			this.OptionDescription = text2;
			IAdminPanelOption option3 = this._option;
			if (!string.IsNullOrEmpty((option3 != null) ? option3.Description : null))
			{
				this.DescriptionHint = new HintViewModel(new TextObject("{=!}" + this._option.Description, null), null);
			}
			else
			{
				this.DescriptionHint = null;
			}
			this.RequiresRestartHint = new HintViewModel(new TextObject("{=MxRJ4CWL}This option won't take effect until next mission.", null), null);
			this.IsDirtyHint = new HintViewModel(new TextObject("{=ftM2TjQ5}Revert changes", null), null);
			this.RestoreToDefaultsHint = new HintViewModel(new TextObject("{=36ll5uSI}Restore to defaults", null), null);
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00035D31 File Offset: 0x00033F31
		public override void OnFinalize()
		{
			base.OnFinalize();
			IAdminPanelOption option = this._option;
			if (option == null)
			{
				return;
			}
			option.SetOnRefreshCallback(null);
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00035D4A File Offset: 0x00033F4A
		private void OnOptionRefreshedAux()
		{
			Action<MultiplayerAdminPanelOptionBaseVM> onOptionRefreshed = MultiplayerAdminPanelOptionBaseVM.OnOptionRefreshed;
			if (onOptionRefreshed == null)
			{
				return;
			}
			onOptionRefreshed(this);
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00035D5C File Offset: 0x00033F5C
		public virtual void UpdateValues()
		{
			IAdminPanelOption option = this._option;
			this.IsFilteredOut = option != null && !option.GetIsAvailable();
			IAdminPanelOption option2 = this._option;
			this.IsDirty = option2 != null && option2.IsDirty;
			IAdminPanelOption option3 = this._option;
			this.CanResetToDefault = option3 != null && option3.CanRevertToDefaultValue;
			string empty = string.Empty;
			IAdminPanelOption option4 = this._option;
			this.IsDisabled = option4 != null && option4.GetIsDisabled(out empty);
			IAdminPanelOption option5 = this._option;
			this.IsRequired = option5 != null && option5.IsRequired;
			if (!string.IsNullOrEmpty(empty))
			{
				this.DisabledHint = new HintViewModel(new TextObject("{=!}" + empty, null), null);
				return;
			}
			this.DisabledHint = null;
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00035E19 File Offset: 0x00034019
		public virtual void ExecuteRevertChanges()
		{
			IAdminPanelOption option = this._option;
			if (option == null)
			{
				return;
			}
			option.RevertChanges();
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00035E2B File Offset: 0x0003402B
		public virtual void ExecuteRestoreDefaults()
		{
			IAdminPanelOption option = this._option;
			if (option == null)
			{
				return;
			}
			option.RestoreDefaults();
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x00035E3D File Offset: 0x0003403D
		// (set) Token: 0x06001117 RID: 4375 RVA: 0x00035E45 File Offset: 0x00034045
		[DataSourceProperty]
		public bool IsRequired
		{
			get
			{
				return this._isRequired;
			}
			set
			{
				if (value != this._isRequired)
				{
					this._isRequired = value;
					base.OnPropertyChangedWithValue(value, "IsRequired");
				}
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001118 RID: 4376 RVA: 0x00035E63 File Offset: 0x00034063
		// (set) Token: 0x06001119 RID: 4377 RVA: 0x00035E6B File Offset: 0x0003406B
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x0600111A RID: 4378 RVA: 0x00035E89 File Offset: 0x00034089
		// (set) Token: 0x0600111B RID: 4379 RVA: 0x00035E91 File Offset: 0x00034091
		[DataSourceProperty]
		public bool IsDirty
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				if (value != this._isDirty)
				{
					this._isDirty = value;
					base.OnPropertyChangedWithValue(value, "IsDirty");
				}
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x0600111C RID: 4380 RVA: 0x00035EAF File Offset: 0x000340AF
		// (set) Token: 0x0600111D RID: 4381 RVA: 0x00035EB7 File Offset: 0x000340B7
		[DataSourceProperty]
		public bool CanResetToDefault
		{
			get
			{
				return this._canResetToDefault;
			}
			set
			{
				if (value != this._canResetToDefault)
				{
					this._canResetToDefault = value;
					base.OnPropertyChangedWithValue(value, "CanResetToDefault");
				}
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x0600111E RID: 4382 RVA: 0x00035ED5 File Offset: 0x000340D5
		// (set) Token: 0x0600111F RID: 4383 RVA: 0x00035EDD File Offset: 0x000340DD
		[DataSourceProperty]
		public bool IsFilteredOut
		{
			get
			{
				return this._isFilteredOut;
			}
			set
			{
				if (value != this._isFilteredOut)
				{
					this._isFilteredOut = value;
					base.OnPropertyChangedWithValue(value, "IsFilteredOut");
				}
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001120 RID: 4384 RVA: 0x00035EFB File Offset: 0x000340FB
		// (set) Token: 0x06001121 RID: 4385 RVA: 0x00035F03 File Offset: 0x00034103
		[DataSourceProperty]
		public bool RequiresRestart
		{
			get
			{
				return this._requiresRestart;
			}
			set
			{
				if (value != this._requiresRestart)
				{
					this._requiresRestart = value;
					base.OnPropertyChangedWithValue(value, "RequiresRestart");
				}
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x00035F21 File Offset: 0x00034121
		// (set) Token: 0x06001123 RID: 4387 RVA: 0x00035F29 File Offset: 0x00034129
		[DataSourceProperty]
		public string OptionTitle
		{
			get
			{
				return this._optionTitle;
			}
			set
			{
				if (value != this._optionTitle)
				{
					this._optionTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionTitle");
				}
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001124 RID: 4388 RVA: 0x00035F4C File Offset: 0x0003414C
		// (set) Token: 0x06001125 RID: 4389 RVA: 0x00035F54 File Offset: 0x00034154
		[DataSourceProperty]
		public string OptionDescription
		{
			get
			{
				return this._optionDescription;
			}
			set
			{
				if (value != this._optionDescription)
				{
					this._optionDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionDescription");
				}
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x00035F77 File Offset: 0x00034177
		// (set) Token: 0x06001127 RID: 4391 RVA: 0x00035F7F File Offset: 0x0003417F
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

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001128 RID: 4392 RVA: 0x00035F9D File Offset: 0x0003419D
		// (set) Token: 0x06001129 RID: 4393 RVA: 0x00035FA5 File Offset: 0x000341A5
		[DataSourceProperty]
		public HintViewModel DescriptionHint
		{
			get
			{
				return this._descriptionHint;
			}
			set
			{
				if (value != this._descriptionHint)
				{
					this._descriptionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DescriptionHint");
				}
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x0600112A RID: 4394 RVA: 0x00035FC3 File Offset: 0x000341C3
		// (set) Token: 0x0600112B RID: 4395 RVA: 0x00035FCB File Offset: 0x000341CB
		[DataSourceProperty]
		public HintViewModel RequiresRestartHint
		{
			get
			{
				return this._requiresRestartHint;
			}
			set
			{
				if (value != this._requiresRestartHint)
				{
					this._requiresRestartHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RequiresRestartHint");
				}
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x00035FE9 File Offset: 0x000341E9
		// (set) Token: 0x0600112D RID: 4397 RVA: 0x00035FF1 File Offset: 0x000341F1
		[DataSourceProperty]
		public HintViewModel IsDirtyHint
		{
			get
			{
				return this._isDirtyHint;
			}
			set
			{
				if (value != this._isDirtyHint)
				{
					this._isDirtyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "IsDirtyHint");
				}
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x0003600F File Offset: 0x0003420F
		// (set) Token: 0x0600112F RID: 4399 RVA: 0x00036017 File Offset: 0x00034217
		[DataSourceProperty]
		public HintViewModel RestoreToDefaultsHint
		{
			get
			{
				return this._restoreToDefaultsHint;
			}
			set
			{
				if (value != this._restoreToDefaultsHint)
				{
					this._restoreToDefaultsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RestoreToDefaultsHint");
				}
			}
		}

		// Token: 0x04000804 RID: 2052
		protected readonly IAdminPanelOption _option;

		// Token: 0x04000805 RID: 2053
		private bool _isRequired;

		// Token: 0x04000806 RID: 2054
		private bool _isDisabled;

		// Token: 0x04000807 RID: 2055
		private bool _isDirty;

		// Token: 0x04000808 RID: 2056
		private bool _canResetToDefault;

		// Token: 0x04000809 RID: 2057
		private bool _isFilteredOut;

		// Token: 0x0400080A RID: 2058
		private bool _requiresRestart;

		// Token: 0x0400080B RID: 2059
		private string _optionTitle;

		// Token: 0x0400080C RID: 2060
		private string _optionDescription;

		// Token: 0x0400080D RID: 2061
		private HintViewModel _disabledHint;

		// Token: 0x0400080E RID: 2062
		private HintViewModel _descriptionHint;

		// Token: 0x0400080F RID: 2063
		private HintViewModel _requiresRestartHint;

		// Token: 0x04000810 RID: 2064
		private HintViewModel _isDirtyHint;

		// Token: 0x04000811 RID: 2065
		private HintViewModel _restoreToDefaultsHint;
	}
}
