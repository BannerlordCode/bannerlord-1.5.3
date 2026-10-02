using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000B4 RID: 180
	public class MultiplayerAdminPanelOptionGroupVM : ViewModel
	{
		// Token: 0x06001130 RID: 4400 RVA: 0x00036038 File Offset: 0x00034238
		public MultiplayerAdminPanelOptionGroupVM(IAdminPanelOptionGroup optionGroup, Func<IAdminPanelOption, MultiplayerAdminPanelOptionBaseVM> onCreateOptionVm, Func<IAdminPanelAction, MultiplayerAdminPanelOptionBaseVM> onCreateActionVm)
		{
			this._optionGroup = optionGroup;
			this._onCreateOptionVM = onCreateOptionVm;
			this._onCreateActionVM = onCreateActionVm;
			this.Options = new MBBindingList<MultiplayerAdminPanelOptionBaseVM>();
			for (int i = 0; i < this._optionGroup.Options.Count; i++)
			{
				Func<IAdminPanelOption, MultiplayerAdminPanelOptionBaseVM> onCreateOptionVM = this._onCreateOptionVM;
				MultiplayerAdminPanelOptionBaseVM multiplayerAdminPanelOptionBaseVM = ((onCreateOptionVM != null) ? onCreateOptionVM(optionGroup.Options[i]) : null);
				if (multiplayerAdminPanelOptionBaseVM != null)
				{
					this.Options.Add(multiplayerAdminPanelOptionBaseVM);
				}
				else
				{
					Debug.FailedAssert("Failed to create view model for option type: " + optionGroup.Options[i].GetType().Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\AdminPanel\\MultiplayerAdminPanelOptionGroupVM.cs", ".ctor", 34);
				}
			}
			for (int j = 0; j < this._optionGroup.Actions.Count; j++)
			{
				Func<IAdminPanelAction, MultiplayerAdminPanelOptionBaseVM> onCreateActionVM = this._onCreateActionVM;
				MultiplayerAdminPanelOptionBaseVM multiplayerAdminPanelOptionBaseVM2 = ((onCreateActionVM != null) ? onCreateActionVM(optionGroup.Actions[j]) : null);
				if (multiplayerAdminPanelOptionBaseVM2 != null)
				{
					this.Options.Add(multiplayerAdminPanelOptionBaseVM2);
				}
				else
				{
					Debug.FailedAssert("Failed to create view model for option type: " + optionGroup.Options[j].GetType().Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\AdminPanel\\MultiplayerAdminPanelOptionGroupVM.cs", ".ctor", 48);
				}
			}
			this.RequiresRestart = this._optionGroup.RequiresRestart;
			this.RequiresRestartHint = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00036188 File Offset: 0x00034388
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RequiresRestartHint.HintText = (this.RequiresRestart ? new TextObject("{=sTVcpXkf}All options under this category requires restart.", null) : TextObject.GetEmpty());
			this.GroupName = this._optionGroup.Name.ToString();
			this.Options.ApplyActionOnAllItems(delegate(MultiplayerAdminPanelOptionBaseVM o)
			{
				o.RefreshValues();
			});
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00036200 File Offset: 0x00034400
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Options.ApplyActionOnAllItems(delegate(MultiplayerAdminPanelOptionBaseVM o)
			{
				o.OnFinalize();
			});
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x00036232 File Offset: 0x00034432
		// (set) Token: 0x06001134 RID: 4404 RVA: 0x0003623A File Offset: 0x0003443A
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

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x00036258 File Offset: 0x00034458
		// (set) Token: 0x06001136 RID: 4406 RVA: 0x00036260 File Offset: 0x00034460
		[DataSourceProperty]
		public string GroupName
		{
			get
			{
				return this._groupName;
			}
			set
			{
				if (value != this._groupName)
				{
					this._groupName = value;
					base.OnPropertyChangedWithValue<string>(value, "GroupName");
				}
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x00036283 File Offset: 0x00034483
		// (set) Token: 0x06001138 RID: 4408 RVA: 0x0003628B File Offset: 0x0003448B
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

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001139 RID: 4409 RVA: 0x000362A9 File Offset: 0x000344A9
		// (set) Token: 0x0600113A RID: 4410 RVA: 0x000362B1 File Offset: 0x000344B1
		[DataSourceProperty]
		public MBBindingList<MultiplayerAdminPanelOptionBaseVM> Options
		{
			get
			{
				return this._options;
			}
			set
			{
				if (value != this._options)
				{
					this._options = value;
					base.OnPropertyChangedWithValue<MBBindingList<MultiplayerAdminPanelOptionBaseVM>>(value, "Options");
				}
			}
		}

		// Token: 0x04000812 RID: 2066
		private readonly IAdminPanelOptionGroup _optionGroup;

		// Token: 0x04000813 RID: 2067
		private readonly Func<IAdminPanelOption, MultiplayerAdminPanelOptionBaseVM> _onCreateOptionVM;

		// Token: 0x04000814 RID: 2068
		private readonly Func<IAdminPanelAction, MultiplayerAdminPanelOptionBaseVM> _onCreateActionVM;

		// Token: 0x04000815 RID: 2069
		private bool _requiresRestart;

		// Token: 0x04000816 RID: 2070
		private string _groupName;

		// Token: 0x04000817 RID: 2071
		private HintViewModel _requiresRestartHint;

		// Token: 0x04000818 RID: 2072
		private MBBindingList<MultiplayerAdminPanelOptionBaseVM> _options;
	}
}
