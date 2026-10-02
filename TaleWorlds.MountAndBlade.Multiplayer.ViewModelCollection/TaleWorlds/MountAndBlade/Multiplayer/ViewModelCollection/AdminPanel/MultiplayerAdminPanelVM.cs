using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000B7 RID: 183
	public class MultiplayerAdminPanelVM : ViewModel
	{
		// Token: 0x06001148 RID: 4424 RVA: 0x0003641C File Offset: 0x0003461C
		public MultiplayerAdminPanelVM(Action<bool> onEscapeMenuToggled, MBReadOnlyList<IAdminPanelOptionProvider> optionProviders, Func<IAdminPanelOption, MultiplayerAdminPanelOptionBaseVM> onGetOptionViewModel, Func<IAdminPanelAction, MultiplayerAdminPanelOptionBaseVM> onGetActionViewModel)
		{
			this._onEscapeMenuToggled = onEscapeMenuToggled;
			this._optionProviders = optionProviders;
			this._onCreateOptionViewModel = onGetOptionViewModel;
			this._onCreateActionViewModel = onGetActionViewModel;
			this.OptionGroups = new MBBindingList<MultiplayerAdminPanelOptionGroupVM>();
			this.InitializeOptions();
			this.InitializeCallbacks();
			this.RefreshValues();
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0003646C File Offset: 0x0003466C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=xILeUbY3}Admin Panel", null).ToString();
			this.CancelText = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
			this.ApplyText = new TextObject("{=WZQnNSwV}Apply Changes", null).ToString();
			this.StartMissionText = new TextObject("{=kwo09aDm}Apply and Start Mission", null).ToString();
			this.ApplyDisabledHint = new HintViewModel(new TextObject("{=TrY4VS1R}Please select valid values for options.", null), null);
			this.OptionGroups.ApplyActionOnAllItems(delegate(MultiplayerAdminPanelOptionGroupVM o)
			{
				o.RefreshValues();
			});
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x00036518 File Offset: 0x00034718
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._optionProviders != null)
			{
				for (int i = 0; i < this._optionProviders.Count; i++)
				{
					this._optionProviders[i].OnFinalize();
				}
			}
			this.FinalizeCallbacks();
			this.OptionGroups.ApplyActionOnAllItems(delegate(MultiplayerAdminPanelOptionGroupVM o)
			{
				o.OnFinalize();
			});
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0003658C File Offset: 0x0003478C
		public void OnTick(float dt)
		{
			if (this._optionProviders != null)
			{
				for (int i = 0; i < this._optionProviders.Count; i++)
				{
					this._optionProviders[i].OnTick(dt);
				}
			}
			if (this._areOptionValuesDirty)
			{
				this.UpdateOptionValues();
				this._areOptionValuesDirty = false;
			}
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x000365DE File Offset: 0x000347DE
		private void InitializeCallbacks()
		{
			MultiplayerAdminPanelOptionBaseVM.OnOptionRefreshed += this.OnOptionChanged;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x000365F1 File Offset: 0x000347F1
		private void FinalizeCallbacks()
		{
			MultiplayerAdminPanelOptionBaseVM.OnOptionRefreshed -= this.OnOptionChanged;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00036604 File Offset: 0x00034804
		private void InitializeOptions()
		{
			this.OptionGroups.Clear();
			if (this._optionProviders != null)
			{
				foreach (IAdminPanelOptionProvider adminPanelOptionProvider in this._optionProviders)
				{
					foreach (IAdminPanelOptionGroup adminPanelOptionGroup in adminPanelOptionProvider.GetOptionGroups())
					{
						MultiplayerAdminPanelOptionGroupVM multiplayerAdminPanelOptionGroupVM = new MultiplayerAdminPanelOptionGroupVM(adminPanelOptionGroup, this._onCreateOptionViewModel, this._onCreateActionViewModel);
						this.OptionGroups.Add(multiplayerAdminPanelOptionGroupVM);
					}
				}
			}
			this.UpdateOptionValues();
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x000366C0 File Offset: 0x000348C0
		private void OnOptionChanged(MultiplayerAdminPanelOptionBaseVM option)
		{
			this._areOptionValuesDirty = true;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x000366CC File Offset: 0x000348CC
		private void UpdateOptionValues()
		{
			bool flag = false;
			foreach (MultiplayerAdminPanelOptionGroupVM multiplayerAdminPanelOptionGroupVM in this.OptionGroups)
			{
				foreach (MultiplayerAdminPanelOptionBaseVM multiplayerAdminPanelOptionBaseVM in multiplayerAdminPanelOptionGroupVM.Options)
				{
					multiplayerAdminPanelOptionBaseVM.UpdateValues();
					if (multiplayerAdminPanelOptionBaseVM.IsRequired && multiplayerAdminPanelOptionBaseVM.IsDisabled)
					{
						flag = true;
					}
				}
			}
			this.IsApplyDisabled = flag;
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00036768 File Offset: 0x00034968
		public void ExecuteApplyChanges()
		{
			if (this._optionProviders == null)
			{
				return;
			}
			foreach (IAdminPanelOptionProvider adminPanelOptionProvider in this._optionProviders)
			{
				adminPanelOptionProvider.ApplyOptions();
			}
			this._areOptionValuesDirty = true;
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x000367C8 File Offset: 0x000349C8
		public void ExecuteCancel()
		{
			this._onEscapeMenuToggled(false);
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x000367D6 File Offset: 0x000349D6
		// (set) Token: 0x06001154 RID: 4436 RVA: 0x000367DE File Offset: 0x000349DE
		[DataSourceProperty]
		public bool IsApplyDisabled
		{
			get
			{
				return this._isApplyDisabled;
			}
			set
			{
				if (value != this._isApplyDisabled)
				{
					this._isApplyDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsApplyDisabled");
				}
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x000367FC File Offset: 0x000349FC
		// (set) Token: 0x06001156 RID: 4438 RVA: 0x00036804 File Offset: 0x00034A04
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

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x00036827 File Offset: 0x00034A27
		// (set) Token: 0x06001158 RID: 4440 RVA: 0x0003682F File Offset: 0x00034A2F
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

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001159 RID: 4441 RVA: 0x00036852 File Offset: 0x00034A52
		// (set) Token: 0x0600115A RID: 4442 RVA: 0x0003685A File Offset: 0x00034A5A
		[DataSourceProperty]
		public string ApplyText
		{
			get
			{
				return this._applyText;
			}
			set
			{
				if (value != this._applyText)
				{
					this._applyText = value;
					base.OnPropertyChangedWithValue<string>(value, "ApplyText");
				}
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x0003687D File Offset: 0x00034A7D
		// (set) Token: 0x0600115C RID: 4444 RVA: 0x00036885 File Offset: 0x00034A85
		[DataSourceProperty]
		public string StartMissionText
		{
			get
			{
				return this._startMissionText;
			}
			set
			{
				if (value != this._startMissionText)
				{
					this._startMissionText = value;
					base.OnPropertyChangedWithValue<string>(value, "StartMissionText");
				}
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x000368A8 File Offset: 0x00034AA8
		// (set) Token: 0x0600115E RID: 4446 RVA: 0x000368B0 File Offset: 0x00034AB0
		[DataSourceProperty]
		public HintViewModel ApplyDisabledHint
		{
			get
			{
				return this._applyDisabledHint;
			}
			set
			{
				if (value != this._applyDisabledHint)
				{
					this._applyDisabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ApplyDisabledHint");
				}
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x0600115F RID: 4447 RVA: 0x000368CE File Offset: 0x00034ACE
		// (set) Token: 0x06001160 RID: 4448 RVA: 0x000368D6 File Offset: 0x00034AD6
		[DataSourceProperty]
		public MBBindingList<MultiplayerAdminPanelOptionGroupVM> OptionGroups
		{
			get
			{
				return this._optionGroups;
			}
			set
			{
				if (value != this._optionGroups)
				{
					this._optionGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<MultiplayerAdminPanelOptionGroupVM>>(value, "OptionGroups");
				}
			}
		}

		// Token: 0x0400081F RID: 2079
		private readonly Action<bool> _onEscapeMenuToggled;

		// Token: 0x04000820 RID: 2080
		private readonly MBReadOnlyList<IAdminPanelOptionProvider> _optionProviders;

		// Token: 0x04000821 RID: 2081
		private readonly Func<IAdminPanelOption, MultiplayerAdminPanelOptionBaseVM> _onCreateOptionViewModel;

		// Token: 0x04000822 RID: 2082
		private readonly Func<IAdminPanelAction, MultiplayerAdminPanelOptionBaseVM> _onCreateActionViewModel;

		// Token: 0x04000823 RID: 2083
		private bool _areOptionValuesDirty;

		// Token: 0x04000824 RID: 2084
		private bool _isApplyDisabled;

		// Token: 0x04000825 RID: 2085
		private string _titleText;

		// Token: 0x04000826 RID: 2086
		private string _cancelText;

		// Token: 0x04000827 RID: 2087
		private string _applyText;

		// Token: 0x04000828 RID: 2088
		private string _startMissionText;

		// Token: 0x04000829 RID: 2089
		private HintViewModel _applyDisabledHint;

		// Token: 0x0400082A RID: 2090
		private MBBindingList<MultiplayerAdminPanelOptionGroupVM> _optionGroups;
	}
}
