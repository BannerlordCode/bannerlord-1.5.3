using System;
using System.Collections.Generic;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200000B RID: 11
	[OverrideView(typeof(MissionLobbyEquipmentUIHandler))]
	public class MissionGauntletClassLoadout : MissionView
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000096 RID: 150 RVA: 0x0000496C File Offset: 0x00002B6C
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00004974 File Offset: 0x00002B74
		public bool IsActive { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000098 RID: 152 RVA: 0x0000497D File Offset: 0x00002B7D
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00004985 File Offset: 0x00002B85
		public bool IsForceClosed { get; private set; }

		// Token: 0x0600009A RID: 154 RVA: 0x00004990 File Offset: 0x00002B90
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this.ViewOrderPriority = 20;
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._missionLobbyEquipmentNetworkComponent = base.Mission.GetMissionBehavior<MissionLobbyEquipmentNetworkComponent>();
			this._gameModeClient = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._teamSelectComponent = base.Mission.GetMissionBehavior<MultiplayerTeamSelectComponent>();
			if (this._teamSelectComponent != null)
			{
				this._teamSelectComponent.OnSelectingTeam += this.OnSelectingTeam;
			}
			this._scoreboardGauntletComponent = base.Mission.GetMissionBehavior<MissionGauntletMultiplayerScoreboard>();
			if (this._scoreboardGauntletComponent != null)
			{
				MissionGauntletMultiplayerScoreboard scoreboardGauntletComponent = this._scoreboardGauntletComponent;
				scoreboardGauntletComponent.OnScoreboardToggled = (Action<bool>)Delegate.Combine(scoreboardGauntletComponent.OnScoreboardToggled, new Action<bool>(this.OnScoreboardToggled));
			}
			this._missionNetworkComponent = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
			if (this._missionNetworkComponent != null)
			{
				this._missionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
			}
			MissionPeer.OnTeamChanged += this.OnTeamChanged;
			this._missionLobbyEquipmentNetworkComponent.OnToggleLoadout += this.OnTryToggle;
			this._missionLobbyEquipmentNetworkComponent.OnEquipmentRefreshed += this.OnPeerEquipmentRefreshed;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004AC0 File Offset: 0x00002CC0
		private void OnMyClientSynchronized()
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			this._myRepresentative = ((myPeer != null) ? myPeer.VirtualPlayer.GetComponent<MissionRepresentativeBase>() : null);
			this._myRepresentative.OnGoldUpdated += this.OnGoldUpdated;
			this._missionLobbyComponent.OnClassRestrictionChanged += this.OnGoldUpdated;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004B17 File Offset: 0x00002D17
		private void OnTeamChanged(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (peer.IsMine && newTeam != null && (newTeam.IsAttacker || newTeam.IsDefender))
			{
				if (this.IsActive)
				{
					this.OnTryToggle(false);
				}
				this.OnTryToggle(true);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004B4A File Offset: 0x00002D4A
		private void OnRefreshSelection(MultiplayerClassDivisions.MPHeroClass heroClass)
		{
			this._lastSelectedHeroClass = heroClass;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004B54 File Offset: 0x00002D54
		public override void OnMissionScreenFinalize()
		{
			if (this._gauntletLayer != null)
			{
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
				this._gauntletLayer = null;
			}
			if (this._dataSource != null)
			{
				this._dataSource.OnFinalize();
				this._dataSource = null;
			}
			if (this._teamSelectComponent != null)
			{
				this._teamSelectComponent.OnSelectingTeam -= this.OnSelectingTeam;
			}
			if (this._scoreboardGauntletComponent != null)
			{
				MissionGauntletMultiplayerScoreboard scoreboardGauntletComponent = this._scoreboardGauntletComponent;
				scoreboardGauntletComponent.OnScoreboardToggled = (Action<bool>)Delegate.Remove(scoreboardGauntletComponent.OnScoreboardToggled, new Action<bool>(this.OnScoreboardToggled));
			}
			if (this._missionNetworkComponent != null)
			{
				this._missionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
				if (this._myRepresentative != null)
				{
					this._myRepresentative.OnGoldUpdated -= this.OnGoldUpdated;
					this._missionLobbyComponent.OnClassRestrictionChanged -= this.OnGoldUpdated;
				}
			}
			this._missionLobbyEquipmentNetworkComponent.OnToggleLoadout -= this.OnTryToggle;
			this._missionLobbyEquipmentNetworkComponent.OnEquipmentRefreshed -= this.OnPeerEquipmentRefreshed;
			MissionPeer.OnTeamChanged -= this.OnTeamChanged;
			base.OnMissionScreenFinalize();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004C84 File Offset: 0x00002E84
		private void CreateView()
		{
			if (this._dataSource != null)
			{
				Debug.FailedAssert("MissionGauntletClassLoadout datasource is already created!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.GauntletUI\\Mission\\MissionGauntletClassLoadout.cs", "CreateView", 135);
			}
			MissionMultiplayerGameModeBaseClient missionBehavior = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._dataSource = new MultiplayerClassLoadoutVM(missionBehavior, new Action<MultiplayerClassDivisions.MPHeroClass>(this.OnRefreshSelection), this._lastSelectedHeroClass);
			this._gauntletLayer = new GauntletLayer("MultiplayerClassLoadout", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerClassLoadout", this._dataSource);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004D0A File Offset: 0x00002F0A
		public void OnTryToggle(bool isActive)
		{
			if (isActive)
			{
				this._tryToInitialize = true;
				return;
			}
			this.IsForceClosed = false;
			this.OnToggled(false);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004D28 File Offset: 0x00002F28
		private bool OnToggled(bool isActive)
		{
			if (this.IsActive == isActive)
			{
				return true;
			}
			if (!base.MissionScreen.SetDisplayDialog(isActive))
			{
				return false;
			}
			if (isActive)
			{
				this.CreateView();
				this._dataSource.Tick(1f);
				this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				base.MissionScreen.AddLayer(this._gauntletLayer);
			}
			else
			{
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
				this._dataSource.OnFinalize();
				this._dataSource = null;
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				this._gauntletLayer = null;
			}
			this.IsActive = isActive;
			return true;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004DD0 File Offset: 0x00002FD0
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._tryToInitialize && GameNetwork.IsMyPeerReady && GameNetwork.MyPeer.GetComponent<MissionPeer>().HasSpawnedAgentVisuals && this.OnToggled(true))
			{
				this._tryToInitialize = false;
			}
			if (this.IsActive)
			{
				this._dataSource.Tick(dt);
				MissionMultiplayerGameModeFlagDominationClient missionMultiplayerGameModeFlagDominationClient;
				if (base.Input.IsHotKeyReleased("ForfeitSpawn") && (missionMultiplayerGameModeFlagDominationClient = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>() as MissionMultiplayerGameModeFlagDominationClient) != null)
				{
					missionMultiplayerGameModeFlagDominationClient.OnRequestForfeitSpawn();
				}
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004E54 File Offset: 0x00003054
		private void OnSelectingTeam(List<Team> disableTeams)
		{
			this.IsForceClosed = true;
			this.OnToggled(false);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004E65 File Offset: 0x00003065
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

		// Token: 0x060000A5 RID: 165 RVA: 0x00004E97 File Offset: 0x00003097
		private void OnPeerEquipmentRefreshed(MissionPeer peer)
		{
			if (this._gameModeClient.GameType == MultiplayerGameType.Skirmish || this._gameModeClient.GameType == MultiplayerGameType.Captain)
			{
				MultiplayerClassLoadoutVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.OnPeerEquipmentRefreshed(peer);
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004EC6 File Offset: 0x000030C6
		private void OnGoldUpdated()
		{
			MultiplayerClassLoadoutVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnGoldUpdated();
		}

		// Token: 0x0400002E RID: 46
		private MultiplayerClassLoadoutVM _dataSource;

		// Token: 0x0400002F RID: 47
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000030 RID: 48
		private MissionRepresentativeBase _myRepresentative;

		// Token: 0x04000031 RID: 49
		private MissionNetworkComponent _missionNetworkComponent;

		// Token: 0x04000032 RID: 50
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000033 RID: 51
		private MissionLobbyEquipmentNetworkComponent _missionLobbyEquipmentNetworkComponent;

		// Token: 0x04000034 RID: 52
		private MissionMultiplayerGameModeBaseClient _gameModeClient;

		// Token: 0x04000035 RID: 53
		private MultiplayerTeamSelectComponent _teamSelectComponent;

		// Token: 0x04000036 RID: 54
		private MissionGauntletMultiplayerScoreboard _scoreboardGauntletComponent;

		// Token: 0x04000037 RID: 55
		private MultiplayerClassDivisions.MPHeroClass _lastSelectedHeroClass;

		// Token: 0x0400003A RID: 58
		private bool _tryToInitialize;
	}
}
