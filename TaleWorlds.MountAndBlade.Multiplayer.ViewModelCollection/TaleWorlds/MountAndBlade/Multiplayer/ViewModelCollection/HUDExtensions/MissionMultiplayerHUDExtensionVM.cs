using System;
using System.Collections.Generic;
using System.ComponentModel;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000095 RID: 149
	public class MissionMultiplayerHUDExtensionVM : ViewModel
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000E84 RID: 3716 RVA: 0x0002CAA4 File Offset: 0x0002ACA4
		// (remove) Token: 0x06000E85 RID: 3717 RVA: 0x0002CADC File Offset: 0x0002ACDC
		public event Action<Agent> OnPlayerFollowRequested;

		// Token: 0x06000E86 RID: 3718 RVA: 0x0002CB14 File Offset: 0x0002AD14
		public MissionMultiplayerHUDExtensionVM(Mission mission)
		{
			this._mission = mission;
			this._missionScoreboardComponent = mission.GetMissionBehavior<MissionScoreboardComponent>();
			this._gameMode = this._mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this.SpectatorControls = new MissionMultiplayerSpectatorHUDVM(this._mission);
			if (this._gameMode.RoundComponent != null)
			{
				this._gameMode.RoundComponent.OnCurrentRoundStateChanged += this.OnCurrentGameModeStateChanged;
			}
			if (this._gameMode.WarmupComponent != null)
			{
				this._gameMode.WarmupComponent.OnWarmupEnded += this.OnCurrentGameModeStateChanged;
			}
			this._missionScoreboardComponent.OnRoundPropertiesChanged += this.SetTeamScoresDirty;
			MissionPeer.OnTeamChanged += this.OnTeamChanged;
			NetworkCommunicator.OnPeerComponentAdded += this.OnPeerComponentAdded;
			Mission.Current.OnMissionReset += this.OnMissionReset;
			MissionLobbyComponent missionBehavior = mission.GetMissionBehavior<MissionLobbyComponent>();
			this._isTeamsEnabled = missionBehavior.MissionType != MultiplayerGameType.Duel;
			this.IsRoundCountdownAvailable = this._gameMode.IsGameModeUsingRoundCountdown;
			this.IsRoundCountdownSuspended = false;
			this._isTeamScoresEnabled = this._isTeamsEnabled;
			this.UpdateShowTeamScores();
			this.Teammates = new MBBindingList<MPPlayerVM>();
			this.Enemies = new MBBindingList<MPPlayerVM>();
			this._teammateDictionary = new Dictionary<MissionPeer, MPPlayerVM>();
			this._enemyDictionary = new Dictionary<MissionPeer, MPPlayerVM>();
			this.OverlayAttackerSide = new MPOverlaySideVM();
			this.OverlayDefenderSide = new MPOverlaySideVM();
			this._overlayPlayerDictionary = new Dictionary<MissionPeer, MPOverlayPlayerVM>();
			this.ShowHud = true;
			this.RefreshValues();
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0002CCE0 File Offset: 0x0002AEE0
		public override void RefreshValues()
		{
			base.RefreshValues();
			string strValue = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			TextObject textObject = new TextObject("{=XJTX8w8M}Warmup Phase - {GAME_MODE}{newline}Waiting for players to join", null);
			textObject.SetTextVariable("GAME_MODE", GameTexts.FindText("str_multiplayer_official_game_type_name", strValue));
			this.WarmupInfoText = textObject.ToString();
			this.SpectatorControls.RefreshValues();
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0002CD36 File Offset: 0x0002AF36
		private void OnMissionReset(object sender, PropertyChangedEventArgs e)
		{
			this.IsGeneralWarningCountdownActive = false;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0002CD40 File Offset: 0x0002AF40
		private void OnPeerComponentAdded(PeerComponent component)
		{
			if (component.IsMine && component is MissionRepresentativeBase)
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				MissionRepresentativeBase missionRepresentativeBase = ((myPeer != null) ? myPeer.VirtualPlayer.GetComponent<MissionRepresentativeBase>() : null);
				this.AllyTeamScore = this._missionScoreboardComponent.GetRoundScore(BattleSideEnum.Attacker);
				this.EnemyTeamScore = this._missionScoreboardComponent.GetRoundScore(BattleSideEnum.Defender);
				this._isTeammateAndEnemiesRelevant = MultiplayerSpectatorHelper.IsStreamerModeActive() || (Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>().IsGameModeTactical && !Mission.Current.HasMissionBehavior<MissionMultiplayerSiegeClient>() && this._gameMode.GameType != MultiplayerGameType.Battle);
				this.CommanderInfo = new CommanderInfoVM(missionRepresentativeBase);
				this.CommanderInfo.OnTeamChanged();
				this.ShowCommanderInfo = true;
				if (this._isTeammateAndEnemiesRelevant)
				{
					this.OnRefreshTeamMembers();
					this.OnRefreshEnemyMembers();
				}
				this.ShowPowerLevels = this._gameMode.GameType == MultiplayerGameType.Battle;
			}
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x0002CE28 File Offset: 0x0002B028
		public override void OnFinalize()
		{
			MissionPeer.OnTeamChanged -= this.OnTeamChanged;
			if (this._gameMode.RoundComponent != null)
			{
				this._gameMode.RoundComponent.OnCurrentRoundStateChanged -= this.OnCurrentGameModeStateChanged;
			}
			if (this._gameMode.WarmupComponent != null)
			{
				this._gameMode.WarmupComponent.OnWarmupEnded -= this.OnCurrentGameModeStateChanged;
			}
			this._missionScoreboardComponent.OnRoundPropertiesChanged -= this.SetTeamScoresDirty;
			NetworkCommunicator.OnPeerComponentAdded -= this.OnPeerComponentAdded;
			CommanderInfoVM commanderInfo = this.CommanderInfo;
			if (commanderInfo != null)
			{
				commanderInfo.OnFinalize();
			}
			this.CommanderInfo = null;
			MissionMultiplayerSpectatorHUDVM spectatorControls = this.SpectatorControls;
			if (spectatorControls != null)
			{
				spectatorControls.OnFinalize();
			}
			this.SpectatorControls = null;
			base.OnFinalize();
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0002CEF8 File Offset: 0x0002B0F8
		public void Tick(float dt)
		{
			this.IsInWarmup = this._gameMode.IsInWarmup;
			this.CheckTimers(false);
			if (this._isTeammateAndEnemiesRelevant || MultiplayerSpectatorHelper.IsStreamerModeActive())
			{
				this.OnRefreshTeamMembers();
				this.OnRefreshEnemyMembers();
			}
			this.ShowAllPlayersOverlay = MultiplayerSpectatorHelper.IsStreamerModeActive();
			if (this.ShowAllPlayersOverlay)
			{
				this.OnRefreshOverlayMembers();
			}
			if (this._isTeamScoresDirty)
			{
				this.UpdateTeamScores();
				this._isTeamScoresDirty = false;
			}
			CommanderInfoVM commanderInfo = this._commanderInfo;
			if (commanderInfo != null)
			{
				commanderInfo.Tick(dt);
			}
			MissionMultiplayerSpectatorHUDVM spectatorControls = this._spectatorControls;
			if (spectatorControls == null)
			{
				return;
			}
			spectatorControls.Tick(dt);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0002CF8C File Offset: 0x0002B18C
		private void CheckTimers(bool forceUpdate = false)
		{
			int num;
			int num2;
			if (this._gameMode.CheckTimer(out num, out num2, forceUpdate))
			{
				this.RemainingRoundTime = TimeSpan.FromSeconds((double)num).ToString("mm':'ss");
				this.WarnRemainingTime = (float)num <= 5f;
				if (this.GeneralWarningCountdown != num2)
				{
					this.IsGeneralWarningCountdownActive = num2 > 0;
					this.GeneralWarningCountdown = num2;
				}
			}
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0002CFF4 File Offset: 0x0002B1F4
		public void OnSpectatedAgentFocusIn(Agent followedAgent)
		{
			MissionMultiplayerSpectatorHUDVM spectatorControls = this._spectatorControls;
			if (spectatorControls != null)
			{
				spectatorControls.OnSpectatedAgentFocusIn(followedAgent);
			}
			MissionPeer missionPeer;
			if ((missionPeer = ((followedAgent != null) ? followedAgent.MissionPeer : null)) == null)
			{
				if (followedAgent == null)
				{
					missionPeer = null;
				}
				else
				{
					Formation formation = followedAgent.Formation;
					if (formation == null)
					{
						missionPeer = null;
					}
					else
					{
						Agent playerOwner = formation.PlayerOwner;
						missionPeer = ((playerOwner != null) ? playerOwner.MissionPeer : null);
					}
				}
			}
			this._followedPeer = missionPeer;
			this.RefreshFollowedOverlayRow();
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0002D053 File Offset: 0x0002B253
		public void OnSpectatedAgentFocusOut(Agent followedPeer)
		{
			MissionMultiplayerSpectatorHUDVM spectatorControls = this._spectatorControls;
			if (spectatorControls != null)
			{
				spectatorControls.OnSpectatedAgentFocusOut(followedPeer);
			}
			this._followedPeer = null;
			this.RefreshFollowedOverlayRow();
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0002D074 File Offset: 0x0002B274
		private void OnOverlayPlayerSelected(MPOverlayPlayerVM overlayPlayer)
		{
			this.OnAvatarPlayerSelected(overlayPlayer);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0002D080 File Offset: 0x0002B280
		private void OnAvatarPlayerSelected(MPPlayerVM player)
		{
			Agent agent;
			if (player == null)
			{
				agent = null;
			}
			else
			{
				MissionPeer peer = player.Peer;
				agent = ((peer != null) ? peer.ControlledAgent : null);
			}
			Agent agent2 = agent;
			if (agent2 == null)
			{
				return;
			}
			Action<Agent> onPlayerFollowRequested = this.OnPlayerFollowRequested;
			if (onPlayerFollowRequested == null)
			{
				return;
			}
			onPlayerFollowRequested(agent2);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0002D0BC File Offset: 0x0002B2BC
		private void RefreshFollowedOverlayRow()
		{
			if (this._focusedOverlayPlayer != null)
			{
				this._focusedOverlayPlayer.IsFocused = false;
				this._focusedOverlayPlayer = null;
			}
			MPOverlayPlayerVM mpoverlayPlayerVM;
			if (this._followedPeer != null && this._overlayPlayerDictionary.TryGetValue(this._followedPeer, out mpoverlayPlayerVM))
			{
				mpoverlayPlayerVM.IsFocused = true;
				this._focusedOverlayPlayer = mpoverlayPlayerVM;
			}
			this.OverlayAttackerSide.SetFollowedPeer(this._followedPeer);
			this.OverlayDefenderSide.SetFollowedPeer(this._followedPeer);
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0002D131 File Offset: 0x0002B331
		private void OnCurrentGameModeStateChanged()
		{
			this.CheckTimers(true);
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0002D13A File Offset: 0x0002B33A
		private void SetTeamScoresDirty()
		{
			this._isTeamScoresDirty = true;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0002D144 File Offset: 0x0002B344
		private void UpdateTeamScores()
		{
			if (this._isTeamScoresEnabled)
			{
				int roundScore = this._missionScoreboardComponent.GetRoundScore(BattleSideEnum.Attacker);
				int roundScore2 = this._missionScoreboardComponent.GetRoundScore(BattleSideEnum.Defender);
				this.AllyTeamScore = (this._isAttackerTeamAlly ? roundScore : roundScore2);
				this.EnemyTeamScore = (this._isAttackerTeamAlly ? roundScore2 : roundScore);
			}
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0002D198 File Offset: 0x0002B398
		private void UpdateTeamBanners()
		{
			Team attackerTeam = Mission.Current.AttackerTeam;
			BannerImageIdentifierVM bannerImageIdentifierVM = new BannerImageIdentifierVM((attackerTeam != null) ? attackerTeam.Banner : null, true);
			Team defenderTeam = Mission.Current.DefenderTeam;
			BannerImageIdentifierVM bannerImageIdentifierVM2 = new BannerImageIdentifierVM((defenderTeam != null) ? defenderTeam.Banner : null, true);
			this.AllyBanner = (this._isAttackerTeamAlly ? bannerImageIdentifierVM : bannerImageIdentifierVM2);
			this.EnemyBanner = (this._isAttackerTeamAlly ? bannerImageIdentifierVM2 : bannerImageIdentifierVM);
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0002D204 File Offset: 0x0002B404
		private void OnTeamChanged(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (peer.IsMine)
			{
				if (this._isTeamScoresEnabled || this._gameMode.GameType == MultiplayerGameType.Battle)
				{
					this._isAttackerTeamAlly = newTeam.Side == BattleSideEnum.Attacker;
					this.SetTeamScoresDirty();
				}
				CommanderInfoVM commanderInfo = this.CommanderInfo;
				if (commanderInfo != null)
				{
					commanderInfo.OnTeamChanged();
				}
			}
			if (this.CommanderInfo == null)
			{
				return;
			}
			MissionPeer missionPeer = ((peer != null) ? peer.GetComponent<MissionPeer>() : null);
			MPPlayerVM mpplayerVM;
			if (missionPeer != null && this._teammateDictionary.TryGetValue(missionPeer, out mpplayerVM))
			{
				mpplayerVM.RefreshTeam();
			}
			string text;
			string text2;
			this.GetTeamColors(Mission.Current.AttackerTeam, out text, out text2);
			if (this._isTeamScoresEnabled || this._gameMode.GameType == MultiplayerGameType.Battle)
			{
				string text3;
				string text4;
				this.GetTeamColors(Mission.Current.DefenderTeam, out text3, out text4);
				if (this._isAttackerTeamAlly)
				{
					this.AllyTeamColor = text;
					this.AllyTeamColor2 = text2;
					this.EnemyTeamColor = text3;
					this.EnemyTeamColor2 = text4;
				}
				else
				{
					this.AllyTeamColor = text3;
					this.AllyTeamColor2 = text4;
					this.EnemyTeamColor = text;
					this.EnemyTeamColor2 = text2;
				}
				this.CommanderInfo.RefreshColors(this.AllyTeamColor, this.AllyTeamColor2, this.EnemyTeamColor, this.EnemyTeamColor2);
			}
			else
			{
				this.AllyTeamColor = text;
				this.AllyTeamColor2 = text2;
				this.CommanderInfo.RefreshColors(this.AllyTeamColor, this.AllyTeamColor2, this.EnemyTeamColor, this.EnemyTeamColor2);
			}
			this.UpdateTeamBanners();
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x0002D364 File Offset: 0x0002B564
		private void GetTeamColors(Team team, out string color, out string color2)
		{
			color = team.Color.ToString("X");
			color = color.Remove(0, 2);
			color = "#" + color + "FF";
			color2 = team.Color2.ToString("X");
			color2 = color2.Remove(0, 2);
			color2 = "#" + color2 + "FF";
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0002D3D8 File Offset: 0x0002B5D8
		private bool IsPeerOnAllySide(MissionPeer lobbyPeer)
		{
			Team team = ((lobbyPeer != null) ? lobbyPeer.Team : null);
			if (team == null || team == Mission.Current.SpectatorTeam)
			{
				return false;
			}
			if (MultiplayerSpectatorHelper.IsLocalPeerSpectator())
			{
				return team.Side == BattleSideEnum.Attacker;
			}
			return this._playerTeam != null && team == this._playerTeam;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0002D428 File Offset: 0x0002B628
		private bool IsPeerOnEnemySide(MissionPeer lobbyPeer)
		{
			Team team = ((lobbyPeer != null) ? lobbyPeer.Team : null);
			if (team == null || team == Mission.Current.SpectatorTeam)
			{
				return false;
			}
			if (MultiplayerSpectatorHelper.IsLocalPeerSpectator())
			{
				return team.Side == BattleSideEnum.Defender;
			}
			return this._playerTeam != null && team != this._playerTeam;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0002D47C File Offset: 0x0002B67C
		private void OnRefreshTeamMembers()
		{
			this._teammatesToRemoveScratch.Clear();
			for (int i = 0; i < this.Teammates.Count; i++)
			{
				this._teammatesToRemoveScratch.Add(this.Teammates[i]);
			}
			List<MPPlayerVM> teammatesToRemoveScratch = this._teammatesToRemoveScratch;
			foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
			{
				if (missionPeer.GetNetworkPeer().GetComponent<MissionPeer>() != null && this.IsPeerOnAllySide(missionPeer))
				{
					MPPlayerVM mpplayerVM;
					if (this._teammateDictionary.TryGetValue(missionPeer, out mpplayerVM))
					{
						teammatesToRemoveScratch.Remove(mpplayerVM);
					}
					else
					{
						MPPlayerVM mpplayerVM2 = new MPPlayerVM(missionPeer);
						mpplayerVM2.SetSelectionHandler(new Action<MPPlayerVM>(this.OnAvatarPlayerSelected));
						this.Teammates.Add(mpplayerVM2);
						this._teammateDictionary.Add(missionPeer, mpplayerVM2);
					}
				}
			}
			foreach (MPPlayerVM mpplayerVM3 in teammatesToRemoveScratch)
			{
				this.Teammates.Remove(mpplayerVM3);
				this._teammateDictionary.Remove(mpplayerVM3.Peer);
			}
			bool flag = MultiplayerSpectatorHelper.IsLocalPeerSpectator();
			foreach (MPPlayerVM mpplayerVM4 in this.Teammates)
			{
				mpplayerVM4.RefreshDivision(false);
				mpplayerVM4.RefreshGold();
				mpplayerVM4.RefreshProperties();
				mpplayerVM4.UpdateDisabled();
				mpplayerVM4.IsSelectable = flag;
			}
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0002D62C File Offset: 0x0002B82C
		private void OnRefreshEnemyMembers()
		{
			this._enemiesToRemoveScratch.Clear();
			for (int i = 0; i < this.Enemies.Count; i++)
			{
				this._enemiesToRemoveScratch.Add(this.Enemies[i]);
			}
			List<MPPlayerVM> enemiesToRemoveScratch = this._enemiesToRemoveScratch;
			foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
			{
				if (missionPeer.GetNetworkPeer().GetComponent<MissionPeer>() != null && this.IsPeerOnEnemySide(missionPeer))
				{
					MPPlayerVM mpplayerVM;
					if (this._enemyDictionary.TryGetValue(missionPeer, out mpplayerVM))
					{
						enemiesToRemoveScratch.Remove(mpplayerVM);
					}
					else
					{
						MPPlayerVM mpplayerVM2 = new MPPlayerVM(missionPeer);
						mpplayerVM2.SetSelectionHandler(new Action<MPPlayerVM>(this.OnAvatarPlayerSelected));
						this.Enemies.Add(mpplayerVM2);
						this._enemyDictionary.Add(missionPeer, mpplayerVM2);
					}
				}
			}
			foreach (MPPlayerVM mpplayerVM3 in enemiesToRemoveScratch)
			{
				this.Enemies.Remove(mpplayerVM3);
				this._enemyDictionary.Remove(mpplayerVM3.Peer);
			}
			bool flag = MultiplayerSpectatorHelper.IsLocalPeerSpectator();
			foreach (MPPlayerVM mpplayerVM4 in this.Enemies)
			{
				mpplayerVM4.RefreshDivision(false);
				mpplayerVM4.UpdateDisabled();
				mpplayerVM4.IsSelectable = flag;
			}
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0002D7D0 File Offset: 0x0002B9D0
		private void OnRefreshOverlayMembers()
		{
			MissionScoreboardComponent missionScoreboardComponent = this._missionScoreboardComponent;
			MissionScoreboardComponent.ScoreboardHeader[] array = ((missionScoreboardComponent != null) ? missionScoreboardComponent.Headers : null);
			this.OverlayAttackerSide.RefreshStatHeaders(array);
			this.OverlayDefenderSide.RefreshStatHeaders(array);
			this._overlayPlayersToRemoveScratch.Clear();
			foreach (MPOverlayPlayerVM mpoverlayPlayerVM in this._overlayPlayerDictionary.Values)
			{
				this._overlayPlayersToRemoveScratch.Add(mpoverlayPlayerVM);
			}
			this._overlayAttackersScratch.Clear();
			this._overlayDefendersScratch.Clear();
			foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
			{
				Team team = ((missionPeer != null) ? missionPeer.Team : null);
				if (team != null && team != Mission.Current.SpectatorTeam && (team.Side == BattleSideEnum.Attacker || team.Side == BattleSideEnum.Defender))
				{
					MPOverlayPlayerVM mpoverlayPlayerVM2;
					if (!this._overlayPlayerDictionary.TryGetValue(missionPeer, out mpoverlayPlayerVM2))
					{
						mpoverlayPlayerVM2 = new MPOverlayPlayerVM(missionPeer, new Action<MPOverlayPlayerVM>(this.OnOverlayPlayerSelected));
						mpoverlayPlayerVM2.RebuildStats(array);
						this._overlayPlayerDictionary.Add(missionPeer, mpoverlayPlayerVM2);
					}
					else
					{
						this._overlayPlayersToRemoveScratch.Remove(mpoverlayPlayerVM2);
						mpoverlayPlayerVM2.RefreshStats(array);
					}
					mpoverlayPlayerVM2.RefreshDivision(false);
					mpoverlayPlayerVM2.RefreshProperties();
					mpoverlayPlayerVM2.UpdateDisabled();
					if (team.Side == BattleSideEnum.Attacker)
					{
						this._overlayAttackersScratch.Add(mpoverlayPlayerVM2);
					}
					else
					{
						this._overlayDefendersScratch.Add(mpoverlayPlayerVM2);
					}
				}
			}
			foreach (MPOverlayPlayerVM mpoverlayPlayerVM3 in this._overlayPlayersToRemoveScratch)
			{
				this._overlayPlayerDictionary.Remove(mpoverlayPlayerVM3.Peer);
				if (mpoverlayPlayerVM3 == this._focusedOverlayPlayer)
				{
					this._focusedOverlayPlayer = null;
				}
			}
			this.OverlayAttackerSide.ApplyPlayers(this._overlayAttackersScratch);
			this.OverlayDefenderSide.ApplyPlayers(this._overlayDefendersScratch);
			this.RefreshFollowedOverlayRow();
			this.UpdateOverlayOverflow();
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0002DA18 File Offset: 0x0002BC18
		private void UpdateOverlayOverflow()
		{
			MissionMultiplayerHUDExtensionVM.UpdateOverlaySideOverflow(this.OverlayAttackerSide);
			MissionMultiplayerHUDExtensionVM.UpdateOverlaySideOverflow(this.OverlayDefenderSide);
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0002DA30 File Offset: 0x0002BC30
		private static void UpdateOverlaySideOverflow(MPOverlaySideVM side)
		{
			int num = MBMath.ClampInt(side.Players.Count - 8, 0, int.MaxValue);
			side.ShowOverflow = num > 0;
			if (side.ShowOverflow)
			{
				TextObject textObject = new TextObject("{=n8jxXmgP}+{COUNT} more", null);
				textObject.SetTextVariable("COUNT", num);
				side.OverflowText = textObject.ToString();
			}
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0002DA8D File Offset: 0x0002BC8D
		private void UpdateShowTeamScores()
		{
			this.ShowTeamScores = !this._gameMode.IsInWarmup && this.ShowCommanderInfo && this._gameMode.GameType != MultiplayerGameType.Siege;
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x0002DAC0 File Offset: 0x0002BCC0
		private Team _playerTeam
		{
			get
			{
				if (!GameNetwork.IsMyPeerReady)
				{
					return null;
				}
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (component == null)
				{
					return null;
				}
				if (component.Team == null || component.Team.Side == BattleSideEnum.None)
				{
					return null;
				}
				return component.Team;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x0002DB04 File Offset: 0x0002BD04
		// (set) Token: 0x06000EA2 RID: 3746 RVA: 0x0002DB0C File Offset: 0x0002BD0C
		[DataSourceProperty]
		public bool IsOrderActive
		{
			get
			{
				return this._isOrderActive;
			}
			set
			{
				if (value != this._isOrderActive)
				{
					this._isOrderActive = value;
					base.OnPropertyChangedWithValue(value, "IsOrderActive");
				}
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x0002DB2A File Offset: 0x0002BD2A
		// (set) Token: 0x06000EA4 RID: 3748 RVA: 0x0002DB32 File Offset: 0x0002BD32
		[DataSourceProperty]
		public CommanderInfoVM CommanderInfo
		{
			get
			{
				return this._commanderInfo;
			}
			set
			{
				if (value != this._commanderInfo)
				{
					this._commanderInfo = value;
					base.OnPropertyChangedWithValue<CommanderInfoVM>(value, "CommanderInfo");
				}
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x0002DB50 File Offset: 0x0002BD50
		// (set) Token: 0x06000EA6 RID: 3750 RVA: 0x0002DB58 File Offset: 0x0002BD58
		[DataSourceProperty]
		public MissionMultiplayerSpectatorHUDVM SpectatorControls
		{
			get
			{
				return this._spectatorControls;
			}
			set
			{
				if (value != this._spectatorControls)
				{
					this._spectatorControls = value;
					base.OnPropertyChangedWithValue<MissionMultiplayerSpectatorHUDVM>(value, "SpectatorControls");
				}
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x0002DB76 File Offset: 0x0002BD76
		// (set) Token: 0x06000EA8 RID: 3752 RVA: 0x0002DB7E File Offset: 0x0002BD7E
		[DataSourceProperty]
		public MBBindingList<MPPlayerVM> Teammates
		{
			get
			{
				return this._teammatesList;
			}
			set
			{
				if (value != this._teammatesList)
				{
					this._teammatesList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPlayerVM>>(value, "Teammates");
				}
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0002DB9C File Offset: 0x0002BD9C
		// (set) Token: 0x06000EAA RID: 3754 RVA: 0x0002DBA4 File Offset: 0x0002BDA4
		[DataSourceProperty]
		public MBBindingList<MPPlayerVM> Enemies
		{
			get
			{
				return this._enemiesList;
			}
			set
			{
				if (value != this._enemiesList)
				{
					this._enemiesList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPlayerVM>>(value, "Enemies");
				}
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x0002DBC2 File Offset: 0x0002BDC2
		// (set) Token: 0x06000EAC RID: 3756 RVA: 0x0002DBCA File Offset: 0x0002BDCA
		[DataSourceProperty]
		public MPOverlaySideVM OverlayAttackerSide
		{
			get
			{
				return this._overlayAttackerSide;
			}
			set
			{
				if (value != this._overlayAttackerSide)
				{
					this._overlayAttackerSide = value;
					base.OnPropertyChangedWithValue<MPOverlaySideVM>(value, "OverlayAttackerSide");
				}
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06000EAD RID: 3757 RVA: 0x0002DBE8 File Offset: 0x0002BDE8
		// (set) Token: 0x06000EAE RID: 3758 RVA: 0x0002DBF0 File Offset: 0x0002BDF0
		[DataSourceProperty]
		public MPOverlaySideVM OverlayDefenderSide
		{
			get
			{
				return this._overlayDefenderSide;
			}
			set
			{
				if (value != this._overlayDefenderSide)
				{
					this._overlayDefenderSide = value;
					base.OnPropertyChangedWithValue<MPOverlaySideVM>(value, "OverlayDefenderSide");
				}
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06000EAF RID: 3759 RVA: 0x0002DC0E File Offset: 0x0002BE0E
		// (set) Token: 0x06000EB0 RID: 3760 RVA: 0x0002DC16 File Offset: 0x0002BE16
		[DataSourceProperty]
		public bool ShowAllPlayersOverlay
		{
			get
			{
				return this._showAllPlayersOverlay;
			}
			set
			{
				if (value != this._showAllPlayersOverlay)
				{
					this._showAllPlayersOverlay = value;
					base.OnPropertyChangedWithValue(value, "ShowAllPlayersOverlay");
				}
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x0002DC34 File Offset: 0x0002BE34
		// (set) Token: 0x06000EB2 RID: 3762 RVA: 0x0002DC3C File Offset: 0x0002BE3C
		[DataSourceProperty]
		public BannerImageIdentifierVM AllyBanner
		{
			get
			{
				return this._defenderBanner;
			}
			set
			{
				if (value != this._defenderBanner)
				{
					this._defenderBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "AllyBanner");
				}
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x0002DC5A File Offset: 0x0002BE5A
		// (set) Token: 0x06000EB4 RID: 3764 RVA: 0x0002DC62 File Offset: 0x0002BE62
		[DataSourceProperty]
		public BannerImageIdentifierVM EnemyBanner
		{
			get
			{
				return this._attackerBanner;
			}
			set
			{
				if (value != this._attackerBanner)
				{
					this._attackerBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "EnemyBanner");
				}
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x0002DC80 File Offset: 0x0002BE80
		// (set) Token: 0x06000EB6 RID: 3766 RVA: 0x0002DC88 File Offset: 0x0002BE88
		[DataSourceProperty]
		public bool IsRoundCountdownAvailable
		{
			get
			{
				return this._isRoundCountdownAvailable;
			}
			set
			{
				if (value != this._isRoundCountdownAvailable)
				{
					this._isRoundCountdownAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsRoundCountdownAvailable");
				}
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x0002DCA6 File Offset: 0x0002BEA6
		// (set) Token: 0x06000EB8 RID: 3768 RVA: 0x0002DCAE File Offset: 0x0002BEAE
		[DataSourceProperty]
		public bool IsRoundCountdownSuspended
		{
			get
			{
				return this._isRoundCountdownSuspended;
			}
			set
			{
				if (value != this._isRoundCountdownSuspended)
				{
					this._isRoundCountdownSuspended = value;
					base.OnPropertyChangedWithValue(value, "IsRoundCountdownSuspended");
				}
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x0002DCCC File Offset: 0x0002BECC
		// (set) Token: 0x06000EBA RID: 3770 RVA: 0x0002DCD4 File Offset: 0x0002BED4
		[DataSourceProperty]
		public bool ShowTeamScores
		{
			get
			{
				return this._showTeamScores;
			}
			set
			{
				if (value != this._showTeamScores)
				{
					this._showTeamScores = value;
					base.OnPropertyChangedWithValue(value, "ShowTeamScores");
				}
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x0002DCF2 File Offset: 0x0002BEF2
		// (set) Token: 0x06000EBC RID: 3772 RVA: 0x0002DCFA File Offset: 0x0002BEFA
		[DataSourceProperty]
		public string RemainingRoundTime
		{
			get
			{
				return this._remainingRoundTime;
			}
			set
			{
				if (value != this._remainingRoundTime)
				{
					this._remainingRoundTime = value;
					base.OnPropertyChangedWithValue<string>(value, "RemainingRoundTime");
				}
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x0002DD1D File Offset: 0x0002BF1D
		// (set) Token: 0x06000EBE RID: 3774 RVA: 0x0002DD25 File Offset: 0x0002BF25
		[DataSourceProperty]
		public bool WarnRemainingTime
		{
			get
			{
				return this._warnRemainingTime;
			}
			set
			{
				if (value != this._warnRemainingTime)
				{
					this._warnRemainingTime = value;
					base.OnPropertyChangedWithValue(value, "WarnRemainingTime");
				}
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x0002DD43 File Offset: 0x0002BF43
		// (set) Token: 0x06000EC0 RID: 3776 RVA: 0x0002DD4B File Offset: 0x0002BF4B
		[DataSourceProperty]
		public int AllyTeamScore
		{
			get
			{
				return this._allyTeamScore;
			}
			set
			{
				if (value != this._allyTeamScore)
				{
					this._allyTeamScore = value;
					base.OnPropertyChangedWithValue(value, "AllyTeamScore");
				}
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x0002DD69 File Offset: 0x0002BF69
		// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x0002DD71 File Offset: 0x0002BF71
		[DataSourceProperty]
		public int EnemyTeamScore
		{
			get
			{
				return this._enemyTeamScore;
			}
			set
			{
				if (value != this._enemyTeamScore)
				{
					this._enemyTeamScore = value;
					base.OnPropertyChangedWithValue(value, "EnemyTeamScore");
				}
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x0002DD8F File Offset: 0x0002BF8F
		// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x0002DD97 File Offset: 0x0002BF97
		[DataSourceProperty]
		public string AllyTeamColor
		{
			get
			{
				return this._allyTeamColor;
			}
			set
			{
				if (value != this._allyTeamColor)
				{
					this._allyTeamColor = value;
					base.OnPropertyChangedWithValue<string>(value, "AllyTeamColor");
				}
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x0002DDBA File Offset: 0x0002BFBA
		// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x0002DDC2 File Offset: 0x0002BFC2
		[DataSourceProperty]
		public string AllyTeamColor2
		{
			get
			{
				return this._allyTeamColor2;
			}
			set
			{
				if (value != this._allyTeamColor2)
				{
					this._allyTeamColor2 = value;
					base.OnPropertyChangedWithValue<string>(value, "AllyTeamColor2");
				}
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x0002DDE5 File Offset: 0x0002BFE5
		// (set) Token: 0x06000EC8 RID: 3784 RVA: 0x0002DDED File Offset: 0x0002BFED
		[DataSourceProperty]
		public string EnemyTeamColor
		{
			get
			{
				return this._enemyTeamColor;
			}
			set
			{
				if (value != this._enemyTeamColor)
				{
					this._enemyTeamColor = value;
					base.OnPropertyChangedWithValue<string>(value, "EnemyTeamColor");
				}
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x0002DE10 File Offset: 0x0002C010
		// (set) Token: 0x06000ECA RID: 3786 RVA: 0x0002DE18 File Offset: 0x0002C018
		[DataSourceProperty]
		public string EnemyTeamColor2
		{
			get
			{
				return this._enemyTeamColor2;
			}
			set
			{
				if (value != this._enemyTeamColor2)
				{
					this._enemyTeamColor2 = value;
					base.OnPropertyChangedWithValue<string>(value, "EnemyTeamColor2");
				}
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x0002DE3B File Offset: 0x0002C03B
		// (set) Token: 0x06000ECC RID: 3788 RVA: 0x0002DE43 File Offset: 0x0002C043
		[DataSourceProperty]
		public bool ShowHud
		{
			get
			{
				return this._showHUD;
			}
			set
			{
				if (value != this._showHUD)
				{
					this._showHUD = value;
					base.OnPropertyChangedWithValue(value, "ShowHud");
				}
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x0002DE61 File Offset: 0x0002C061
		// (set) Token: 0x06000ECE RID: 3790 RVA: 0x0002DE69 File Offset: 0x0002C069
		[DataSourceProperty]
		public bool ShowCommanderInfo
		{
			get
			{
				return this._showCommanderInfo;
			}
			set
			{
				if (value != this._showCommanderInfo)
				{
					this._showCommanderInfo = value;
					base.OnPropertyChangedWithValue(value, "ShowCommanderInfo");
					this.UpdateShowTeamScores();
				}
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0002DE8D File Offset: 0x0002C08D
		// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x0002DE95 File Offset: 0x0002C095
		[DataSourceProperty]
		public bool ShowPowerLevels
		{
			get
			{
				return this._showPowerLevels;
			}
			set
			{
				if (value != this._showPowerLevels)
				{
					this._showPowerLevels = value;
					base.OnPropertyChangedWithValue(value, "ShowPowerLevels");
				}
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x0002DEB3 File Offset: 0x0002C0B3
		// (set) Token: 0x06000ED2 RID: 3794 RVA: 0x0002DEBB File Offset: 0x0002C0BB
		[DataSourceProperty]
		public bool IsInWarmup
		{
			get
			{
				return this._isInWarmup;
			}
			set
			{
				if (value != this._isInWarmup)
				{
					this._isInWarmup = value;
					base.OnPropertyChangedWithValue(value, "IsInWarmup");
					this.UpdateShowTeamScores();
					CommanderInfoVM commanderInfo = this.CommanderInfo;
					if (commanderInfo == null)
					{
						return;
					}
					commanderInfo.UpdateWarmupDependentFlags(this._isInWarmup);
				}
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x0002DEF5 File Offset: 0x0002C0F5
		// (set) Token: 0x06000ED4 RID: 3796 RVA: 0x0002DEFD File Offset: 0x0002C0FD
		[DataSourceProperty]
		public string WarmupInfoText
		{
			get
			{
				return this._warmupInfoText;
			}
			set
			{
				if (value != this._warmupInfoText)
				{
					this._warmupInfoText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarmupInfoText");
				}
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0002DF20 File Offset: 0x0002C120
		// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x0002DF28 File Offset: 0x0002C128
		[DataSourceProperty]
		public int GeneralWarningCountdown
		{
			get
			{
				return this._generalWarningCountdown;
			}
			set
			{
				if (value != this._generalWarningCountdown)
				{
					this._generalWarningCountdown = value;
					base.OnPropertyChangedWithValue(value, "GeneralWarningCountdown");
				}
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x0002DF46 File Offset: 0x0002C146
		// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x0002DF4E File Offset: 0x0002C14E
		[DataSourceProperty]
		public bool IsGeneralWarningCountdownActive
		{
			get
			{
				return this._isGeneralWarningCountdownActive;
			}
			set
			{
				if (value != this._isGeneralWarningCountdownActive)
				{
					this._isGeneralWarningCountdownActive = value;
					base.OnPropertyChangedWithValue(value, "IsGeneralWarningCountdownActive");
				}
			}
		}

		// Token: 0x040006A3 RID: 1699
		private const float RemainingTimeWarningThreshold = 5f;

		// Token: 0x040006A5 RID: 1701
		private readonly Mission _mission;

		// Token: 0x040006A6 RID: 1702
		private readonly Dictionary<MissionPeer, MPPlayerVM> _teammateDictionary;

		// Token: 0x040006A7 RID: 1703
		private readonly Dictionary<MissionPeer, MPPlayerVM> _enemyDictionary;

		// Token: 0x040006A8 RID: 1704
		private readonly List<MPPlayerVM> _teammatesToRemoveScratch = new List<MPPlayerVM>();

		// Token: 0x040006A9 RID: 1705
		private readonly List<MPPlayerVM> _enemiesToRemoveScratch = new List<MPPlayerVM>();

		// Token: 0x040006AA RID: 1706
		private const int MaxVisibleOverlayRows = 8;

		// Token: 0x040006AB RID: 1707
		private readonly Dictionary<MissionPeer, MPOverlayPlayerVM> _overlayPlayerDictionary;

		// Token: 0x040006AC RID: 1708
		private readonly List<MPOverlayPlayerVM> _overlayPlayersToRemoveScratch = new List<MPOverlayPlayerVM>();

		// Token: 0x040006AD RID: 1709
		private readonly List<MPOverlayPlayerVM> _overlayAttackersScratch = new List<MPOverlayPlayerVM>();

		// Token: 0x040006AE RID: 1710
		private readonly List<MPOverlayPlayerVM> _overlayDefendersScratch = new List<MPOverlayPlayerVM>();

		// Token: 0x040006AF RID: 1711
		private MissionPeer _followedPeer;

		// Token: 0x040006B0 RID: 1712
		private MPOverlayPlayerVM _focusedOverlayPlayer;

		// Token: 0x040006B1 RID: 1713
		private readonly MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x040006B2 RID: 1714
		private readonly MissionMultiplayerGameModeBaseClient _gameMode;

		// Token: 0x040006B3 RID: 1715
		private readonly bool _isTeamsEnabled;

		// Token: 0x040006B4 RID: 1716
		private bool _isAttackerTeamAlly;

		// Token: 0x040006B5 RID: 1717
		private bool _isTeammateAndEnemiesRelevant;

		// Token: 0x040006B6 RID: 1718
		private bool _isTeamScoresEnabled;

		// Token: 0x040006B7 RID: 1719
		private bool _isTeamScoresDirty;

		// Token: 0x040006B8 RID: 1720
		private bool _isOrderActive;

		// Token: 0x040006B9 RID: 1721
		private CommanderInfoVM _commanderInfo;

		// Token: 0x040006BA RID: 1722
		private MissionMultiplayerSpectatorHUDVM _spectatorControls;

		// Token: 0x040006BB RID: 1723
		private bool _warnRemainingTime;

		// Token: 0x040006BC RID: 1724
		private bool _isRoundCountdownAvailable;

		// Token: 0x040006BD RID: 1725
		private bool _isRoundCountdownSuspended;

		// Token: 0x040006BE RID: 1726
		private bool _showTeamScores;

		// Token: 0x040006BF RID: 1727
		private string _remainingRoundTime;

		// Token: 0x040006C0 RID: 1728
		private string _allyTeamColor;

		// Token: 0x040006C1 RID: 1729
		private string _allyTeamColor2;

		// Token: 0x040006C2 RID: 1730
		private string _enemyTeamColor;

		// Token: 0x040006C3 RID: 1731
		private string _enemyTeamColor2;

		// Token: 0x040006C4 RID: 1732
		private string _warmupInfoText;

		// Token: 0x040006C5 RID: 1733
		private int _allyTeamScore = -1;

		// Token: 0x040006C6 RID: 1734
		private int _enemyTeamScore = -1;

		// Token: 0x040006C7 RID: 1735
		private MBBindingList<MPPlayerVM> _teammatesList;

		// Token: 0x040006C8 RID: 1736
		private MBBindingList<MPPlayerVM> _enemiesList;

		// Token: 0x040006C9 RID: 1737
		private MPOverlaySideVM _overlayAttackerSide;

		// Token: 0x040006CA RID: 1738
		private MPOverlaySideVM _overlayDefenderSide;

		// Token: 0x040006CB RID: 1739
		private bool _showAllPlayersOverlay;

		// Token: 0x040006CC RID: 1740
		private bool _showHUD;

		// Token: 0x040006CD RID: 1741
		private bool _showCommanderInfo;

		// Token: 0x040006CE RID: 1742
		private bool _showPowerLevels;

		// Token: 0x040006CF RID: 1743
		private bool _isInWarmup;

		// Token: 0x040006D0 RID: 1744
		private int _generalWarningCountdown;

		// Token: 0x040006D1 RID: 1745
		private bool _isGeneralWarningCountdownActive;

		// Token: 0x040006D2 RID: 1746
		private BannerImageIdentifierVM _defenderBanner;

		// Token: 0x040006D3 RID: 1747
		private BannerImageIdentifierVM _attackerBanner;
	}
}
