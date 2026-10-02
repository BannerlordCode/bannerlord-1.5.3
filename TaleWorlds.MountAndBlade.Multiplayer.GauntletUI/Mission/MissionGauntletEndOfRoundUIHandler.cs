using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.EndOfRound;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000010 RID: 16
	[OverrideView(typeof(MultiplayerEndOfRoundUIHandler))]
	public class MissionGauntletEndOfRoundUIHandler : MissionView
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x000055FA File Offset: 0x000037FA
		private IRoundComponent RoundComponent
		{
			get
			{
				return this._mpGameModeBase.RoundComponent;
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005608 File Offset: 0x00003808
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._scoreboardComponent = base.Mission.GetMissionBehavior<MissionScoreboardComponent>();
			this._mpGameModeBase = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this.ViewOrderPriority = 23;
			this._dataSource = new MultiplayerEndOfRoundVM(this._scoreboardComponent, this._missionLobbyComponent, this.RoundComponent);
			this._gauntletLayer = new GauntletLayer("MultiplayerAdminPanel", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerEndOfRound", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			if (this.RoundComponent != null)
			{
				this.RoundComponent.OnRoundStarted += this.RoundStarted;
				this._scoreboardComponent.OnRoundPropertiesChanged += this.OnRoundPropertiesChanged;
				this.RoundComponent.OnPostRoundEnded += this.ShowEndOfRoundUI;
				this._scoreboardComponent.OnMVPSelected += this.OnMVPSelected;
			}
			this._missionLobbyComponent.OnPostMatchEnded += this.OnPostMatchEnded;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000573C File Offset: 0x0000393C
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			if (this.RoundComponent != null)
			{
				this.RoundComponent.OnRoundStarted -= this.RoundStarted;
				this._scoreboardComponent.OnRoundPropertiesChanged -= this.OnRoundPropertiesChanged;
				this.RoundComponent.OnPostRoundEnded -= this.ShowEndOfRoundUI;
				this._scoreboardComponent.OnMVPSelected -= this.OnMVPSelected;
			}
			this._missionLobbyComponent.OnPostMatchEnded -= this.OnPostMatchEnded;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000057F4 File Offset: 0x000039F4
		private void RoundStarted()
		{
			ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
			this._dataSource.IsShown = false;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000581E File Offset: 0x00003A1E
		private void OnRoundPropertiesChanged()
		{
			if (this.RoundComponent.RoundCount != 0 && this._missionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending)
			{
				this._dataSource.Refresh();
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005846 File Offset: 0x00003A46
		private void ShowEndOfRoundUI()
		{
			this.ShowEndOfRoundUI(false);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005850 File Offset: 0x00003A50
		private void ShowEndOfRoundUI(bool isForced)
		{
			if (isForced || (this.RoundComponent.RoundCount != 0 && this._missionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending))
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
				this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Mouse);
				this._dataSource.IsShown = true;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000058A5 File Offset: 0x00003AA5
		private void OnPostMatchEnded()
		{
			ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			this._dataSource.IsShown = false;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000058BF File Offset: 0x00003ABF
		private void OnMVPSelected(MissionPeer mvpPeer, int mvpCount)
		{
			this._dataSource.OnMVPSelected(mvpPeer);
		}

		// Token: 0x0400004C RID: 76
		private MultiplayerEndOfRoundVM _dataSource;

		// Token: 0x0400004D RID: 77
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400004E RID: 78
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x0400004F RID: 79
		private MissionScoreboardComponent _scoreboardComponent;

		// Token: 0x04000050 RID: 80
		private MissionMultiplayerGameModeBaseClient _mpGameModeBase;
	}
}
