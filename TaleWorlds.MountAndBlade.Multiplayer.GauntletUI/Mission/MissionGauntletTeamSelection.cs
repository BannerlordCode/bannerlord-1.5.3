using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.TeamSelection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200001B RID: 27
	[OverrideView(typeof(MultiplayerTeamSelectUIHandler))]
	public class MissionGauntletTeamSelection : MissionView
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00007BC3 File Offset: 0x00005DC3
		public MissionGauntletTeamSelection()
		{
			this.ViewOrderPriority = 22;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00007BD4 File Offset: 0x00005DD4
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._missionNetworkComponent = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
			this._multiplayerTeamSelectComponent = base.Mission.GetMissionBehavior<MultiplayerTeamSelectComponent>();
			this._classLoadoutGauntletComponent = base.Mission.GetMissionBehavior<MissionGauntletClassLoadout>();
			this._lobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._missionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
			this._lobbyComponent.OnPostMatchEnded += this.OnClose;
			this._multiplayerTeamSelectComponent.OnSelectingTeam += this.MissionLobbyComponentOnSelectingTeam;
			this._multiplayerTeamSelectComponent.OnUpdateTeams += this.MissionLobbyComponentOnUpdateTeams;
			this._multiplayerTeamSelectComponent.OnUpdateFriendsPerTeam += this.MissionLobbyComponentOnFriendsUpdated;
			this._scoreboardGauntletComponent = base.Mission.GetMissionBehavior<MissionGauntletMultiplayerScoreboard>();
			if (this._scoreboardGauntletComponent != null)
			{
				MissionGauntletMultiplayerScoreboard scoreboardGauntletComponent = this._scoreboardGauntletComponent;
				scoreboardGauntletComponent.OnScoreboardToggled = (Action<bool>)Delegate.Combine(scoreboardGauntletComponent.OnScoreboardToggled, new Action<bool>(this.OnScoreboardToggled));
			}
			this._multiplayerTeamSelectComponent.OnMyTeamChange += this.OnMyTeamChanged;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007CF8 File Offset: 0x00005EF8
		public override void OnMissionScreenFinalize()
		{
			this._missionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
			this._lobbyComponent.OnPostMatchEnded -= this.OnClose;
			this._multiplayerTeamSelectComponent.OnSelectingTeam -= this.MissionLobbyComponentOnSelectingTeam;
			this._multiplayerTeamSelectComponent.OnUpdateTeams -= this.MissionLobbyComponentOnUpdateTeams;
			this._multiplayerTeamSelectComponent.OnUpdateFriendsPerTeam -= this.MissionLobbyComponentOnFriendsUpdated;
			this._multiplayerTeamSelectComponent.OnMyTeamChange -= this.OnMyTeamChanged;
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
				this._gauntletLayer = null;
			}
			if (this._dataSource != null)
			{
				this._dataSource.OnFinalize();
				this._dataSource = null;
			}
			if (this._scoreboardGauntletComponent != null)
			{
				MissionGauntletMultiplayerScoreboard scoreboardGauntletComponent = this._scoreboardGauntletComponent;
				scoreboardGauntletComponent.OnScoreboardToggled = (Action<bool>)Delegate.Remove(scoreboardGauntletComponent.OnScoreboardToggled, new Action<bool>(this.OnScoreboardToggled));
			}
			base.OnMissionScreenFinalize();
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00007E0E File Offset: 0x0000600E
		public override bool OnEscape()
		{
			if (this._isActive && !this._dataSource.IsCancelDisabled)
			{
				this.OnClose();
				return true;
			}
			return base.OnEscape();
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00007E34 File Offset: 0x00006034
		private void OnClose()
		{
			if (!this._isActive)
			{
				return;
			}
			this._isActive = false;
			this._disabledTeams = null;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			base.MissionScreen.SetCameraLockState(false);
			base.MissionScreen.SetDisplayDialog(false);
			this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
			if (this._classLoadoutGauntletComponent != null && this._classLoadoutGauntletComponent.IsForceClosed)
			{
				this._classLoadoutGauntletComponent.OnTryToggle(true);
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00007ECC File Offset: 0x000060CC
		private void OnOpen()
		{
			if (this._isActive)
			{
				return;
			}
			this._isActive = true;
			string strValue = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this._dataSource = new MultiplayerTeamSelectVM(base.Mission, new Action<Team>(this.OnChangeTeamTo), new Action(this.OnAutoassign), new Action(this.OnClose), base.Mission.Teams, strValue);
			this._dataSource.RefreshDisabledTeams(this._disabledTeams);
			this._gauntletLayer = new GauntletLayer("MultiplayerTeamSelection", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerTeamSelection", this._dataSource);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.Mouse);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.MissionScreen.SetCameraLockState(true);
			this.MissionLobbyComponentOnUpdateTeams();
			this.MissionLobbyComponentOnFriendsUpdated();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007FAD File Offset: 0x000061AD
		private void OnChangeTeamTo(Team targetTeam)
		{
			this._multiplayerTeamSelectComponent.ChangeTeam(targetTeam);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007FBB File Offset: 0x000061BB
		private void OnMyTeamChanged()
		{
			this.OnClose();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00007FC3 File Offset: 0x000061C3
		private void OnAutoassign()
		{
			this._multiplayerTeamSelectComponent.AutoAssignTeam(GameNetwork.MyPeer);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00007FD8 File Offset: 0x000061D8
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._isSynchronized && this._toOpen && base.MissionScreen.SetDisplayDialog(true))
			{
				this._toOpen = false;
				this.OnOpen();
			}
			MultiplayerTeamSelectVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.Tick(dt);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00008028 File Offset: 0x00006228
		private void MissionLobbyComponentOnSelectingTeam(List<Team> disabledTeams)
		{
			this._disabledTeams = disabledTeams;
			this._toOpen = true;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00008038 File Offset: 0x00006238
		private void MissionLobbyComponentOnFriendsUpdated()
		{
			if (!this._isActive)
			{
				return;
			}
			IEnumerable<MissionPeer> enumerable = from x in this._multiplayerTeamSelectComponent.GetFriendsForTeam(base.Mission.AttackerTeam)
				select x.GetComponent<MissionPeer>();
			IEnumerable<MissionPeer> enumerable2 = from x in this._multiplayerTeamSelectComponent.GetFriendsForTeam(base.Mission.DefenderTeam)
				select x.GetComponent<MissionPeer>();
			this._dataSource.RefreshFriendsPerTeam(enumerable, enumerable2);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000080D4 File Offset: 0x000062D4
		private void MissionLobbyComponentOnUpdateTeams()
		{
			if (!this._isActive)
			{
				return;
			}
			List<Team> disabledTeams = this._multiplayerTeamSelectComponent.GetDisabledTeams();
			this._dataSource.RefreshDisabledTeams(disabledTeams);
			int playerCountForTeam = this._multiplayerTeamSelectComponent.GetPlayerCountForTeam(base.Mission.AttackerTeam);
			int playerCountForTeam2 = this._multiplayerTeamSelectComponent.GetPlayerCountForTeam(base.Mission.DefenderTeam);
			int intValue = MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			int intValue2 = MultiplayerOptions.OptionType.NumberOfBotsTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this._dataSource.RefreshPlayerAndBotCount(playerCountForTeam, playerCountForTeam2, intValue, intValue2);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00008153 File Offset: 0x00006353
		private void OnScoreboardToggled(bool isEnabled)
		{
			if (isEnabled)
			{
				GauntletLayer gauntletLayer = this._gauntletLayer;
				if (gauntletLayer == null)
				{
					return;
				}
				gauntletLayer.InputRestrictions.ResetInputRestrictions();
				return;
			}
			else
			{
				GauntletLayer gauntletLayer2 = this._gauntletLayer;
				if (gauntletLayer2 == null)
				{
					return;
				}
				gauntletLayer2.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				return;
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00008185 File Offset: 0x00006385
		private void OnMyClientSynchronized()
		{
			this._isSynchronized = true;
		}

		// Token: 0x04000088 RID: 136
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000089 RID: 137
		private MultiplayerTeamSelectVM _dataSource;

		// Token: 0x0400008A RID: 138
		private MissionNetworkComponent _missionNetworkComponent;

		// Token: 0x0400008B RID: 139
		private MultiplayerTeamSelectComponent _multiplayerTeamSelectComponent;

		// Token: 0x0400008C RID: 140
		private MissionGauntletMultiplayerScoreboard _scoreboardGauntletComponent;

		// Token: 0x0400008D RID: 141
		private MissionGauntletClassLoadout _classLoadoutGauntletComponent;

		// Token: 0x0400008E RID: 142
		private MissionLobbyComponent _lobbyComponent;

		// Token: 0x0400008F RID: 143
		private List<Team> _disabledTeams;

		// Token: 0x04000090 RID: 144
		private bool _toOpen;

		// Token: 0x04000091 RID: 145
		private bool _isSynchronized;

		// Token: 0x04000092 RID: 146
		private bool _isActive;
	}
}
