using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200000A RID: 10
	[OverrideView(typeof(MultiplayerAdminPanelUIHandler))]
	public class MissionGauntletAdminPanel : MissionView
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00004574 File Offset: 0x00002774
		public MissionGauntletAdminPanel()
		{
			this.ViewOrderPriority = 45;
			this._optionProviderCreators = new MBList<MissionGauntletAdminPanel.CreateOptionProviderDelegeate>();
			this._optionViewModelCreators = new MBList<MissionGauntletAdminPanel.CreateOptionViewModelDelegate>();
			this._actionViewModelCreators = new MBList<MissionGauntletAdminPanel.CreateActionViewModelDelegate>();
			this.AddOptionViewModelCreator(new MissionGauntletAdminPanel.CreateOptionViewModelDelegate(this.CreateDefaultOptionViewModels));
			this.AddActionViewModelCreator(new MissionGauntletAdminPanel.CreateActionViewModelDelegate(this.CreateDefaultActionViewModels));
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000045D4 File Offset: 0x000027D4
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._multiplayerAdminComponent = base.Mission.GetMissionBehavior<MultiplayerAdminComponent>();
			this._multiplayerAdminComponent.OnSetAdminMenuActiveState += this.OnShowAdminPanel;
			this.AddOptionProviderCreator(new MissionGauntletAdminPanel.CreateOptionProviderDelegeate(this.CreateDefaultAdminPanelOptionProvider));
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004632 File Offset: 0x00002832
		private IAdminPanelOptionProvider CreateDefaultAdminPanelOptionProvider()
		{
			return new DefaultAdminPanelOptionProvider(this._multiplayerAdminComponent, this._missionLobbyComponent);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004645 File Offset: 0x00002845
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._isActive && base.Mission.CurrentState != Mission.State.Continuing)
			{
				this.OnExitAdminPanel();
			}
			if (this._isActive)
			{
				this._dataSource.OnTick(dt);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004680 File Offset: 0x00002880
		public override void OnMissionScreenFinalize()
		{
			this._multiplayerAdminComponent.OnSetAdminMenuActiveState -= this.OnShowAdminPanel;
			this.OnEscapeMenuToggled(false);
			MultiplayerAdminPanelVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
			this._optionProviderCreators.Clear();
			base.OnMissionScreenFinalize();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000046D4 File Offset: 0x000028D4
		public override bool OnEscape()
		{
			if (this._isActive)
			{
				this.OnExitAdminPanel();
				return true;
			}
			return base.OnEscape();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000046EC File Offset: 0x000028EC
		public void AddOptionProviderCreator(MissionGauntletAdminPanel.CreateOptionProviderDelegeate creator)
		{
			if (creator != null)
			{
				this._optionProviderCreators.Add(creator);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000046FD File Offset: 0x000028FD
		private void OnExitAdminPanel()
		{
			this.OnEscapeMenuToggled(false);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004706 File Offset: 0x00002906
		private void OnShowAdminPanel(bool show)
		{
			this.OnEscapeMenuToggled(show);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000470F File Offset: 0x0000290F
		public void AddOptionViewModelCreator(MissionGauntletAdminPanel.CreateOptionViewModelDelegate creator)
		{
			this._optionViewModelCreators.Add(creator);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000471D File Offset: 0x0000291D
		public void AddActionViewModelCreator(MissionGauntletAdminPanel.CreateActionViewModelDelegate creator)
		{
			this._actionViewModelCreators.Add(creator);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000472C File Offset: 0x0000292C
		private MultiplayerAdminPanelOptionBaseVM CreateDefaultOptionViewModels(IAdminPanelOption option)
		{
			IAdminPanelMultiSelectionOption adminPanelMultiSelectionOption;
			if ((adminPanelMultiSelectionOption = option as IAdminPanelMultiSelectionOption) != null)
			{
				return new MultiplayerAdminPanelMultiSelectionOptionVM(adminPanelMultiSelectionOption);
			}
			IAdminPanelAction adminPanelAction;
			if ((adminPanelAction = option as IAdminPanelAction) != null)
			{
				return new MultiplayerAdminPanelActionOptionVM(adminPanelAction);
			}
			IAdminPanelOption<string> adminPanelOption;
			if ((adminPanelOption = option as IAdminPanelOption<string>) != null)
			{
				return new MultiplayerAdminPanelStringOptionVM(adminPanelOption);
			}
			IAdminPanelNumericOption adminPanelNumericOption;
			if ((adminPanelNumericOption = option as IAdminPanelNumericOption) != null)
			{
				return new MultiplayerAdminPanelNumericOptionVM(adminPanelNumericOption);
			}
			IAdminPanelOption<bool> adminPanelOption2;
			if ((adminPanelOption2 = option as IAdminPanelOption<bool>) != null)
			{
				return new MultiplayerAdminPanelToggleOptionVM(adminPanelOption2);
			}
			return null;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004791 File Offset: 0x00002991
		private MultiplayerAdminPanelOptionBaseVM CreateDefaultActionViewModels(IAdminPanelAction action)
		{
			return new MultiplayerAdminPanelActionOptionVM(action);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000479C File Offset: 0x0000299C
		private MultiplayerAdminPanelOptionBaseVM OnCreateOptionViewModel(IAdminPanelOption option)
		{
			for (int i = this._optionViewModelCreators.Count - 1; i >= 0; i--)
			{
				MissionGauntletAdminPanel.CreateOptionViewModelDelegate createOptionViewModelDelegate = this._optionViewModelCreators[i];
				MultiplayerAdminPanelOptionBaseVM multiplayerAdminPanelOptionBaseVM = ((createOptionViewModelDelegate != null) ? createOptionViewModelDelegate(option) : null);
				if (multiplayerAdminPanelOptionBaseVM != null)
				{
					return multiplayerAdminPanelOptionBaseVM;
				}
			}
			return null;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000047E4 File Offset: 0x000029E4
		private MultiplayerAdminPanelOptionBaseVM OnCreateActionViewModel(IAdminPanelAction action)
		{
			for (int i = this._actionViewModelCreators.Count - 1; i >= 0; i--)
			{
				MissionGauntletAdminPanel.CreateActionViewModelDelegate createActionViewModelDelegate = this._actionViewModelCreators[i];
				MultiplayerAdminPanelOptionBaseVM multiplayerAdminPanelOptionBaseVM = ((createActionViewModelDelegate != null) ? createActionViewModelDelegate(action) : null);
				if (multiplayerAdminPanelOptionBaseVM != null)
				{
					return multiplayerAdminPanelOptionBaseVM;
				}
			}
			return null;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000482C File Offset: 0x00002A2C
		private void OnEscapeMenuToggled(bool isOpened)
		{
			if (isOpened == this._isActive || !base.MissionScreen.SetDisplayDialog(isOpened))
			{
				return;
			}
			this._isActive = isOpened;
			if (isOpened)
			{
				if (this._dataSource == null)
				{
					MBList<IAdminPanelOptionProvider> mblist = new MBList<IAdminPanelOptionProvider>();
					for (int i = 0; i < this._optionProviderCreators.Count; i++)
					{
						MissionGauntletAdminPanel.CreateOptionProviderDelegeate createOptionProviderDelegeate = this._optionProviderCreators[i];
						IAdminPanelOptionProvider adminPanelOptionProvider = ((createOptionProviderDelegeate != null) ? createOptionProviderDelegeate() : null);
						if (adminPanelOptionProvider != null)
						{
							mblist.Add(adminPanelOptionProvider);
						}
					}
					this._dataSource = new MultiplayerAdminPanelVM(new Action<bool>(this.OnEscapeMenuToggled), mblist, new Func<IAdminPanelOption, MultiplayerAdminPanelOptionBaseVM>(this.OnCreateOptionViewModel), new Func<IAdminPanelAction, MultiplayerAdminPanelOptionBaseVM>(this.OnCreateActionViewModel));
				}
				this._gauntletLayer = new GauntletLayer("MultiplayerAdminPanel", this.ViewOrderPriority, false);
				this._movie = this._gauntletLayer.LoadMovie("MultiplayerAdminPanel", this._dataSource);
				this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				base.MissionScreen.AddLayer(this._gauntletLayer);
				return;
			}
			MultiplayerAdminPanelVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
			this._gauntletLayer = null;
		}

		// Token: 0x04000025 RID: 37
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000026 RID: 38
		private MultiplayerAdminPanelVM _dataSource;

		// Token: 0x04000027 RID: 39
		private GauntletMovieIdentifier _movie;

		// Token: 0x04000028 RID: 40
		private bool _isActive;

		// Token: 0x04000029 RID: 41
		private MultiplayerAdminComponent _multiplayerAdminComponent;

		// Token: 0x0400002A RID: 42
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x0400002B RID: 43
		private readonly MBList<MissionGauntletAdminPanel.CreateOptionProviderDelegeate> _optionProviderCreators;

		// Token: 0x0400002C RID: 44
		private readonly MBList<MissionGauntletAdminPanel.CreateOptionViewModelDelegate> _optionViewModelCreators;

		// Token: 0x0400002D RID: 45
		private readonly MBList<MissionGauntletAdminPanel.CreateActionViewModelDelegate> _actionViewModelCreators;

		// Token: 0x02000024 RID: 36
		// (Invoke) Token: 0x06000154 RID: 340
		public delegate IAdminPanelOptionProvider CreateOptionProviderDelegeate();

		// Token: 0x02000025 RID: 37
		// (Invoke) Token: 0x06000158 RID: 344
		public delegate MultiplayerAdminPanelOptionBaseVM CreateOptionViewModelDelegate(IAdminPanelOption option);

		// Token: 0x02000026 RID: 38
		// (Invoke) Token: 0x0600015C RID: 348
		public delegate MultiplayerAdminPanelOptionBaseVM CreateActionViewModelDelegate(IAdminPanelAction action);
	}
}
