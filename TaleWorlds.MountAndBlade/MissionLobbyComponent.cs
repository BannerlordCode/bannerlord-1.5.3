using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B0 RID: 688
	public abstract class MissionLobbyComponent : MissionNetwork
	{
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x060025FA RID: 9722 RVA: 0x00089A78 File Offset: 0x00087C78
		// (remove) Token: 0x060025FB RID: 9723 RVA: 0x00089AB0 File Offset: 0x00087CB0
		public event Action OnPostMatchEnded;

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x060025FC RID: 9724 RVA: 0x00089AE8 File Offset: 0x00087CE8
		// (remove) Token: 0x060025FD RID: 9725 RVA: 0x00089B20 File Offset: 0x00087D20
		public event Action OnCultureSelectionRequested;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x060025FE RID: 9726 RVA: 0x00089B58 File Offset: 0x00087D58
		// (remove) Token: 0x060025FF RID: 9727 RVA: 0x00089B90 File Offset: 0x00087D90
		public event Action<string, bool> OnAdminMessageRequested;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06002600 RID: 9728 RVA: 0x00089BC8 File Offset: 0x00087DC8
		// (remove) Token: 0x06002601 RID: 9729 RVA: 0x00089C00 File Offset: 0x00087E00
		public event Action OnClassRestrictionChanged;

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002602 RID: 9730 RVA: 0x00089C35 File Offset: 0x00087E35
		public bool IsInWarmup
		{
			get
			{
				return this._warmupComponent != null && this._warmupComponent.IsInWarmup;
			}
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x00089C4C File Offset: 0x00087E4C
		static MissionLobbyComponent()
		{
			MissionLobbyComponent.AddLobbyComponentType(typeof(MissionBattleSchedulerClientComponent), LobbyMissionType.Matchmaker, false);
			MissionLobbyComponent.AddLobbyComponentType(typeof(MissionCustomGameClientComponent), LobbyMissionType.Custom, false);
			MissionLobbyComponent.AddLobbyComponentType(typeof(MissionCommunityClientComponent), LobbyMissionType.Community, false);
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x00089CAA File Offset: 0x00087EAA
		public static void AddLobbyComponentType(Type type, LobbyMissionType missionType, bool isSeverComponent)
		{
			MissionLobbyComponent._lobbyComponentTypes.Add(new Tuple<LobbyMissionType, bool>(missionType, isSeverComponent), type);
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x00089CC0 File Offset: 0x00087EC0
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this.CurrentMultiplayerState = MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers;
			if (GameNetwork.IsServerOrRecorder)
			{
				MissionMultiplayerGameModeBase missionBehavior = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBase>();
				if (missionBehavior != null && !missionBehavior.AllowCustomPlayerBanners())
				{
					this._usingFixedBanners = true;
					return;
				}
			}
			else
			{
				this._inactivityTimer = new Timer(base.Mission.CurrentTime, MissionLobbyComponent.InactivityThreshold, true);
			}
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x00089D1C File Offset: 0x00087F1C
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<KillDeathCountChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventKillDeathCountChangeEvent));
				registerer.RegisterBaseHandler<MissionStateChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventMissionStateChange));
				registerer.RegisterBaseHandler<NetworkMessages.FromServer.CreateBanner>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventCreateBannerForPeer));
				registerer.RegisterBaseHandler<ChangeCulture>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventChangeCulture));
				registerer.RegisterBaseHandler<ChangeClassRestrictions>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventChangeClassRestrictions));
				registerer.RegisterBaseHandler<PeerClanInfoChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPeerClanInfoChange));
				registerer.RegisterBaseHandler<PeerLastKillChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPeerLastKillChange));
				registerer.RegisterBaseHandler<PeerMostUsedWeaponChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPeerMostUsedWeaponChange));
				return;
			}
			if (GameNetwork.IsClientOrReplay)
			{
				registerer.RegisterBaseHandler<ChangeCulture>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventChangeCulture));
				registerer.RegisterBaseHandler<PeerClanInfoChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPeerClanInfoChange));
				registerer.RegisterBaseHandler<PeerLastKillChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPeerLastKillChange));
				registerer.RegisterBaseHandler<PeerMostUsedWeaponChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPeerMostUsedWeaponChange));
				return;
			}
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<NetworkMessages.FromClient.CreateBanner>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventCreateBannerForPeer));
				registerer.RegisterBaseHandler<RequestCultureChange>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventRequestCultureChange));
				registerer.RegisterBaseHandler<RequestChangeCharacterMessage>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventRequestChangeCharacterMessage));
				registerer.RegisterBaseHandler<SendClanInfo>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventSendClanInfo));
			}
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x00089E63 File Offset: 0x00088063
		protected override void OnUdpNetworkHandlerClose()
		{
			if (GameNetwork.IsServerOrRecorder || this._usingFixedBanners)
			{
				this._usingFixedBanners = false;
			}
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x00089E7B File Offset: 0x0008807B
		public static MissionLobbyComponent CreateBehavior()
		{
			return (MissionLobbyComponent)Activator.CreateInstance(MissionLobbyComponent._lobbyComponentTypes[new Tuple<LobbyMissionType, bool>(BannerlordNetwork.LobbyMissionType, GameNetwork.IsDedicatedServer)]);
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x00089EA0 File Offset: 0x000880A0
		public virtual void QuitMission()
		{
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x00089EA4 File Offset: 0x000880A4
		public override void AfterStart()
		{
			base.Mission.DeploymentPlan.MakeDefaultDeploymentPlans();
			this._missionScoreboardComponent = base.Mission.GetMissionBehavior<MissionScoreboardComponent>();
			this._gameMode = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			this._timerComponent = base.Mission.GetMissionBehavior<MultiplayerTimerComponent>();
			this._roundComponent = base.Mission.GetMissionBehavior<IRoundComponent>();
			this._warmupComponent = base.Mission.GetMissionBehavior<MultiplayerWarmupComponent>();
			if (GameNetwork.IsClient)
			{
				base.Mission.GetMissionBehavior<MissionNetworkComponent>().OnMyClientSynchronized += this.OnMyClientSynchronized;
			}
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x00089F3C File Offset: 0x0008813C
		private void OnMyClientSynchronized()
		{
			base.Mission.GetMissionBehavior<MissionNetworkComponent>().OnMyClientSynchronized -= this.OnMyClientSynchronized;
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			if (component != null && component.Culture == null && !MissionLobbyComponent.IsLocalPeerSpectator())
			{
				this.RequestCultureSelection();
			}
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x00089F88 File Offset: 0x00088188
		private static bool IsLocalPeerSpectator()
		{
			return SpectatorHelper.IsLocalPeerSpectator();
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x00089F90 File Offset: 0x00088190
		public override void EarlyStart()
		{
			if (GameNetwork.IsServer)
			{
				base.Mission.SpectatorTeam = base.Mission.Teams.Add(BattleSideEnum.None, uint.MaxValue, uint.MaxValue, null, true, false, true);
				List<MissionPeer> list = VirtualPlayer.Peers<MissionPeer>();
				for (int i = 0; i < list.Count; i++)
				{
					list[i].HasSentClanInfo = false;
				}
			}
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x00089FEC File Offset: 0x000881EC
		public override void OnMissionTick(float dt)
		{
			if (GameNetwork.IsClient && this._inactivityTimer.Check(base.Mission.CurrentTime))
			{
				NetworkMain.GameClient.IsInCriticalState = MBAPI.IMBNetwork.ElapsedTimeSinceLastUdpPacketArrived() > (double)MissionLobbyComponent.InactivityThreshold;
			}
			if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				if (GameNetwork.IsServer && (this._warmupComponent == null || (!this._warmupComponent.IsInWarmup && this._timerComponent.CheckIfTimerPassed())))
				{
					int num = GameNetwork.NetworkPeers.Count<NetworkCommunicator>((NetworkCommunicator x) => x.IsSynchronized && !this.IsSpectatorPeer(x));
					int num2 = MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) + MultiplayerOptions.OptionType.NumberOfBotsTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					int intValue = MultiplayerOptions.OptionType.MinNumberOfPlayersForMatchStart.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					if (num + num2 >= intValue || MBCommon.CurrentGameType == MBCommon.GameType.MultiClientServer)
					{
						this.SetStatePlayingAsServer();
						return;
					}
				}
			}
			else if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing)
			{
				bool flag = this._timerComponent.CheckIfTimerPassed();
				if (GameNetwork.IsServerOrRecorder && this._gameMode.RoundController == null && (flag || this._gameMode.CheckForMatchEnd()))
				{
					this._gameMode.GetWinnerTeam();
					this._gameMode.SpawnComponent.SpawningBehavior.RequestStopSpawnSession();
					this._gameMode.SpawnComponent.SpawningBehavior.SetRemainingAgentsInvulnerable();
					this.SetStateEndingAsServer();
				}
			}
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x0008A128 File Offset: 0x00088328
		protected override void OnUdpNetworkHandlerTick()
		{
			if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Ending && this._timerComponent.CheckIfTimerPassed() && GameNetwork.IsServer)
			{
				this.EndGameAsServer();
			}
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x0008A14D File Offset: 0x0008834D
		public override void OnRemoveBehavior()
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			this.QuitMission();
			base.OnRemoveBehavior();
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x0008A161 File Offset: 0x00088361
		public bool IsClassAvailable(FormationClass formationClass)
		{
			return !this._classRestrictions[(int)formationClass];
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x0008A16E File Offset: 0x0008836E
		public void ChangeClassRestriction(FormationClass classToChangeRestriction, bool value)
		{
			this._classRestrictions[(int)classToChangeRestriction] = value;
			Action onClassRestrictionChanged = this.OnClassRestrictionChanged;
			if (onClassRestrictionChanged == null)
			{
				return;
			}
			onClassRestrictionChanged();
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x0008A18C File Offset: 0x0008838C
		private void HandleServerEventMissionStateChange(GameNetworkMessage baseMessage)
		{
			MissionStateChange missionStateChange = (MissionStateChange)baseMessage;
			this.CurrentMultiplayerState = missionStateChange.CurrentState;
			if (this.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing && this._warmupComponent != null)
				{
					base.Mission.RemoveMissionBehavior(this._warmupComponent);
					this._warmupComponent = null;
				}
				float num = ((this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing) ? ((float)(MultiplayerOptions.OptionType.MapTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) * 60)) : MissionLobbyComponent.PostMatchWaitDuration);
				this._timerComponent.StartTimerAsClient(missionStateChange.StateStartTimeInSeconds, num);
			}
			if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Ending)
			{
				this.SetStateEndingAsClient();
			}
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x0008A21C File Offset: 0x0008841C
		private void HandleServerEventKillDeathCountChangeEvent(GameNetworkMessage baseMessage)
		{
			KillDeathCountChange killDeathCountChange = (KillDeathCountChange)baseMessage;
			if (killDeathCountChange.VictimPeer != null)
			{
				MissionPeer component = killDeathCountChange.VictimPeer.GetComponent<MissionPeer>();
				NetworkCommunicator attackerPeer = killDeathCountChange.AttackerPeer;
				MissionPeer missionPeer = ((attackerPeer != null) ? attackerPeer.GetComponent<MissionPeer>() : null);
				if (component != null)
				{
					component.KillCount = killDeathCountChange.KillCount;
					component.AssistCount = killDeathCountChange.AssistCount;
					component.DeathCount = killDeathCountChange.DeathCount;
					component.Score = killDeathCountChange.Score;
					if (missionPeer != null)
					{
						missionPeer.OnKillAnotherPeer(component);
					}
					if (killDeathCountChange.KillCount == 0 && killDeathCountChange.AssistCount == 0 && killDeathCountChange.DeathCount == 0 && killDeathCountChange.Score == 0)
					{
						component.ResetKillRegistry();
					}
				}
				if (this._missionScoreboardComponent != null)
				{
					this._missionScoreboardComponent.PlayerPropertiesChanged(killDeathCountChange.VictimPeer);
				}
			}
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x0008A2D8 File Offset: 0x000884D8
		private void HandleServerEventPeerClanInfoChange(GameNetworkMessage baseMessage)
		{
			PeerClanInfoChange peerClanInfoChange = (PeerClanInfoChange)baseMessage;
			NetworkCommunicator peer = peerClanInfoChange.Peer;
			MissionPeer missionPeer = ((peer != null) ? peer.GetComponent<MissionPeer>() : null);
			if (missionPeer != null)
			{
				missionPeer.ClanName = peerClanInfoChange.ClanName;
			}
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x0008A310 File Offset: 0x00088510
		private void HandleServerEventPeerLastKillChange(GameNetworkMessage baseMessage)
		{
			PeerLastKillChange peerLastKillChange = (PeerLastKillChange)baseMessage;
			NetworkCommunicator killerPeer = peerLastKillChange.KillerPeer;
			MissionPeer missionPeer = ((killerPeer != null) ? killerPeer.GetComponent<MissionPeer>() : null);
			if (missionPeer != null)
			{
				missionPeer.LastKillVictimName = peerLastKillChange.VictimName;
			}
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x0008A348 File Offset: 0x00088548
		private void HandleServerEventPeerMostUsedWeaponChange(GameNetworkMessage baseMessage)
		{
			PeerMostUsedWeaponChange peerMostUsedWeaponChange = (PeerMostUsedWeaponChange)baseMessage;
			NetworkCommunicator peer = peerMostUsedWeaponChange.Peer;
			MissionPeer missionPeer = ((peer != null) ? peer.GetComponent<MissionPeer>() : null);
			if (missionPeer != null)
			{
				missionPeer.MostUsedWeaponName = GameTexts.FindText("str_inventory_weapon", peerMostUsedWeaponChange.WeaponClass.ToString()).ToString();
			}
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x0008A39C File Offset: 0x0008859C
		private static string SanitizeReceivedClanName(string rawClanName)
		{
			if (string.IsNullOrEmpty(rawClanName))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(rawClanName.Length);
			foreach (char c in rawClanName)
			{
				if (!char.IsControl(c))
				{
					stringBuilder.Append(c);
				}
			}
			string text = stringBuilder.ToString().Trim();
			if (text.Length > 64)
			{
				text = text.Substring(0, 64);
			}
			Game game = Game.Current;
			ChatBox chatBox = ((game != null) ? game.GetGameHandler<ChatBox>() : null);
			if (chatBox != null)
			{
				text = chatBox.CensorClientText(text);
			}
			return text;
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x0008A434 File Offset: 0x00088634
		private bool HandleClientEventSendClanInfo(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			SendClanInfo sendClanInfo = (SendClanInfo)baseMessage;
			MissionPeer component = peer.GetComponent<MissionPeer>();
			if (component == null)
			{
				return false;
			}
			if (component.HasSentClanInfo)
			{
				return true;
			}
			component.HasSentClanInfo = true;
			string text = MissionLobbyComponent.SanitizeReceivedClanName(sendClanInfo.ClanName);
			component.ClanName = text;
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new PeerClanInfoChange(peer, text));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeTargetPlayer, peer);
			return true;
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x0008A494 File Offset: 0x00088694
		private void HandleServerEventCreateBannerForPeer(GameNetworkMessage baseMessage)
		{
			NetworkMessages.FromServer.CreateBanner createBanner = (NetworkMessages.FromServer.CreateBanner)baseMessage;
			MissionPeer component = createBanner.Peer.GetComponent<MissionPeer>();
			if (component != null)
			{
				component.Peer.BannerCode = createBanner.BannerCode;
			}
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x0008A4C8 File Offset: 0x000886C8
		private void HandleServerEventChangeCulture(GameNetworkMessage baseMessage)
		{
			ChangeCulture changeCulture = (ChangeCulture)baseMessage;
			MissionPeer component = changeCulture.Peer.GetComponent<MissionPeer>();
			if (component != null)
			{
				component.Culture = changeCulture.Culture;
			}
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x0008A4F8 File Offset: 0x000886F8
		private void HandleServerEventChangeClassRestrictions(GameNetworkMessage baseMessage)
		{
			ChangeClassRestrictions changeClassRestrictions = (ChangeClassRestrictions)baseMessage;
			this.ChangeClassRestriction(changeClassRestrictions.ClassToChangeRestriction, changeClassRestrictions.NewValue);
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x0008A520 File Offset: 0x00088720
		private bool HandleClientEventRequestCultureChange(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			RequestCultureChange requestCultureChange = (RequestCultureChange)baseMessage;
			MissionPeer component = peer.GetComponent<MissionPeer>();
			if (component != null && this._gameMode.CheckIfPlayerCanDespawn(component))
			{
				component.Culture = requestCultureChange.Culture;
				this.DespawnPlayer(component);
			}
			return true;
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x0008A560 File Offset: 0x00088760
		private bool HandleClientEventCreateBannerForPeer(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			NetworkMessages.FromClient.CreateBanner createBanner = (NetworkMessages.FromClient.CreateBanner)baseMessage;
			MissionMultiplayerGameModeBase missionBehavior = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			if (missionBehavior == null || !missionBehavior.AllowCustomPlayerBanners())
			{
				return false;
			}
			MissionPeer component = peer.GetComponent<MissionPeer>();
			if (component == null)
			{
				return false;
			}
			component.Peer.BannerCode = createBanner.BannerCode;
			MissionLobbyComponent.SyncBannersToAllClients(createBanner.BannerCode, component.GetNetworkPeer());
			return true;
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x0008A5BC File Offset: 0x000887BC
		private bool HandleClientEventRequestChangeCharacterMessage(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			MissionPeer component = ((RequestChangeCharacterMessage)baseMessage).NetworkPeer.GetComponent<MissionPeer>();
			if (component != null && this._gameMode.CheckIfPlayerCanDespawn(component))
			{
				this.DespawnPlayer(component);
			}
			return true;
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x0008A5F3 File Offset: 0x000887F3
		private static void SyncBannersToAllClients(string bannerCode, NetworkCommunicator ownerPeer)
		{
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new NetworkMessages.FromServer.CreateBanner(ownerPeer, bannerCode));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeTargetPlayer, ownerPeer);
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x0008A60D File Offset: 0x0008880D
		protected override void HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
			base.HandleNewClientConnect(clientConnectionInfo);
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x0008A616 File Offset: 0x00088816
		protected override void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			if (!networkPeer.IsServerPeer)
			{
				this.SendExistingObjectsToPeer(networkPeer);
			}
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x0008A628 File Offset: 0x00088828
		private void SendExistingObjectsToPeer(NetworkCommunicator peer)
		{
			long num = 0L;
			if (this.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				num = this._timerComponent.GetCurrentTimerStartTime().NumberOfTicks;
			}
			GameNetwork.BeginModuleEventAsServer(peer);
			GameNetwork.WriteMessage(new MissionStateChange(this.CurrentMultiplayerState, num));
			GameNetwork.EndModuleEventAsServer();
			this.SendPeerInformationsToPeer(peer);
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x0008A678 File Offset: 0x00088878
		private void SendLastKillToSpectators(MissionPeer killerPeer)
		{
			if (killerPeer == null || string.IsNullOrEmpty(killerPeer.LastKillVictimName))
			{
				return;
			}
			NetworkCommunicator networkPeer = killerPeer.GetNetworkPeer();
			if (networkPeer == null)
			{
				return;
			}
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator != null && networkCommunicator.IsSynchronized && this.IsSpectatorPeer(networkCommunicator))
				{
					GameNetwork.BeginModuleEventAsServer(networkCommunicator);
					GameNetwork.WriteMessage(new PeerLastKillChange(networkPeer, killerPeer.LastKillVictimName));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x0008A710 File Offset: 0x00088910
		private bool IsSpectatorPeer(NetworkCommunicator networkPeer)
		{
			return SpectatorHelper.IsPeerSpectator(networkPeer);
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x0008A718 File Offset: 0x00088918
		private void SendPeerInformationsToPeer(NetworkCommunicator peer)
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeersIncludingDisconnectedPeers)
			{
				bool flag = networkCommunicator.VirtualPlayer != GameNetwork.VirtualPlayers[networkCommunicator.VirtualPlayer.Index];
				if (flag || networkCommunicator.IsSynchronized || networkCommunicator.JustReconnecting)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null)
					{
						GameNetwork.BeginModuleEventAsServer(peer);
						GameNetwork.WriteMessage(new KillDeathCountChange(component.GetNetworkPeer(), null, component.KillCount, component.AssistCount, component.DeathCount, component.Score));
						GameNetwork.EndModuleEventAsServer();
						if (!string.IsNullOrEmpty(component.ClanName))
						{
							GameNetwork.BeginModuleEventAsServer(peer);
							GameNetwork.WriteMessage(new PeerClanInfoChange(component.GetNetworkPeer(), component.ClanName));
							GameNetwork.EndModuleEventAsServer();
						}
						if (component.MostUsedWeaponClass != WeaponClass.Undefined)
						{
							GameNetwork.BeginModuleEventAsServer(peer);
							GameNetwork.WriteMessage(new PeerMostUsedWeaponChange(component.GetNetworkPeer(), component.MostUsedWeaponClass));
							GameNetwork.EndModuleEventAsServer();
						}
						if (!string.IsNullOrEmpty(component.LastKillVictimName))
						{
							GameNetwork.BeginModuleEventAsServer(peer);
							GameNetwork.WriteMessage(new PeerLastKillChange(component.GetNetworkPeer(), component.LastKillVictimName));
							GameNetwork.EndModuleEventAsServer();
						}
						if (component.BotsUnderControlAlive != 0 || component.BotsUnderControlTotal != 0)
						{
							GameNetwork.BeginModuleEventAsServer(peer);
							GameNetwork.WriteMessage(new BotsControlledChange(component.GetNetworkPeer(), component.BotsUnderControlAlive, component.BotsUnderControlTotal));
							GameNetwork.EndModuleEventAsServer();
						}
					}
					else
					{
						Debug.Print(">#< SendPeerInformationsToPeer MissionPeer is null.", 0, Debug.DebugColor.BrightWhite, 17179869184UL);
					}
				}
				else
				{
					Debug.Print(string.Concat(new string[] { ">#< Can't send the info of ", networkCommunicator.UserName, " to ", peer.UserName, "." }), 0, Debug.DebugColor.BrightWhite, 17179869184UL);
					Debug.Print(string.Format("isDisconnectedPeer: {0}", flag), 0, Debug.DebugColor.BrightWhite, 17179869184UL);
					Debug.Print(string.Format("networkPeer.IsSynchronized: {0}", networkCommunicator.IsSynchronized), 0, Debug.DebugColor.BrightWhite, 17179869184UL);
					Debug.Print(string.Format("peer == networkPeer: {0}", peer == networkCommunicator), 0, Debug.DebugColor.BrightWhite, 17179869184UL);
					Debug.Print(string.Format("networkPeer.JustReconnecting: {0}", networkCommunicator.JustReconnecting), 0, Debug.DebugColor.BrightWhite, 17179869184UL);
				}
			}
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x0008A99C File Offset: 0x00088B9C
		public void DespawnPlayer(MissionPeer missionPeer)
		{
			if (missionPeer.ControlledAgent != null && missionPeer.ControlledAgent.IsActive())
			{
				Agent controlledAgent = missionPeer.ControlledAgent;
				if (controlledAgent == null)
				{
					return;
				}
				controlledAgent.FadeOut(true, true);
			}
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x0008A9C5 File Offset: 0x00088BC5
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent != null && GameNetwork.IsServer && !isBlocked && affectorAgent != affectedAgent && affectorAgent.MissionPeer != null && damagedHp > 0f)
			{
				affectedAgent.AddHitter(affectorAgent.MissionPeer, damagedHp, affectorAgent.IsFriendOf(affectedAgent));
			}
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x0008AA00 File Offset: 0x00088C00
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			if (GameNetwork.IsServer)
			{
				if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Ending)
				{
					return;
				}
				if ((agentState == AgentState.Killed || agentState == AgentState.Unconscious || agentState == AgentState.Routed) && affectedAgent != null && affectedAgent.IsHuman)
				{
					MissionPeer missionPeer = ((affectorAgent != null) ? affectorAgent.MissionPeer : null) ?? ((affectorAgent != null) ? affectorAgent.OwningAgentMissionPeer : null);
					MissionPeer missionPeer2 = this.RemoveHittersAndGetAssistorPeer((affectorAgent != null) ? affectorAgent.MissionPeer : null, affectedAgent);
					if (affectedAgent.MissionPeer != null)
					{
						this.OnPlayerDies(affectedAgent.MissionPeer, missionPeer, missionPeer2);
					}
					else
					{
						this.OnBotDies(affectedAgent, missionPeer, missionPeer2);
					}
					if (affectorAgent != null && affectorAgent.IsHuman)
					{
						if (affectorAgent != affectedAgent)
						{
							if (affectorAgent.MissionPeer != null)
							{
								this.OnPlayerKills(affectorAgent.MissionPeer, affectedAgent, missionPeer2);
								return;
							}
							this.OnBotKills(affectorAgent, affectedAgent);
							return;
						}
						else if (affectorAgent.MissionPeer != null)
						{
							affectorAgent.MissionPeer.Score -= (int)((float)this._gameMode.GetScoreForKill(affectedAgent) * 1.5f);
							this._missionScoreboardComponent.PlayerPropertiesChanged(affectorAgent.MissionPeer.GetNetworkPeer());
							GameNetwork.BeginBroadcastModuleEvent();
							GameNetwork.WriteMessage(new KillDeathCountChange(affectorAgent.MissionPeer.GetNetworkPeer(), affectedAgent.MissionPeer.GetNetworkPeer(), affectorAgent.MissionPeer.KillCount, affectorAgent.MissionPeer.AssistCount, affectorAgent.MissionPeer.DeathCount, affectorAgent.MissionPeer.Score));
							GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						}
					}
				}
			}
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x0008AB74 File Offset: 0x00088D74
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (GameNetwork.IsServer)
			{
				if (agent.IsMount)
				{
					return;
				}
				if (agent.MissionPeer == null)
				{
					if (agent.OwningAgentMissionPeer != null)
					{
						MissionPeer owningAgentMissionPeer = agent.OwningAgentMissionPeer;
						int num = owningAgentMissionPeer.BotsUnderControlAlive;
						owningAgentMissionPeer.BotsUnderControlAlive = num + 1;
						MissionPeer owningAgentMissionPeer2 = agent.OwningAgentMissionPeer;
						num = owningAgentMissionPeer2.BotsUnderControlTotal;
						owningAgentMissionPeer2.BotsUnderControlTotal = num + 1;
						return;
					}
					this._missionScoreboardComponent.Sides[(int)agent.Team.Side].BotScores.AliveCount++;
				}
			}
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x0008ABF8 File Offset: 0x00088DF8
		protected virtual void OnPlayerKills(MissionPeer killerPeer, Agent killedAgent, MissionPeer assistorPeer)
		{
			if (killedAgent.MissionPeer == null)
			{
				NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.SingleOrDefault<NetworkCommunicator>((NetworkCommunicator x) => x.GetComponent<MissionPeer>() != null && x.GetComponent<MissionPeer>().ControlledFormation != null && x.GetComponent<MissionPeer>().ControlledFormation == killedAgent.Formation);
				if (networkCommunicator != null)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					killerPeer.OnKillAnotherPeer(component);
				}
				else
				{
					killerPeer.OnKillBot(killedAgent.Name.ToString());
				}
			}
			else
			{
				killerPeer.OnKillAnotherPeer(killedAgent.MissionPeer);
			}
			this.SendLastKillToSpectators(killerPeer);
			if (killerPeer.Team == null)
			{
				return;
			}
			if (killerPeer.Team.IsEnemyOf(killedAgent.Team))
			{
				killerPeer.Score += this._gameMode.GetScoreForKill(killedAgent);
				int num = killerPeer.KillCount;
				killerPeer.KillCount = num + 1;
			}
			else
			{
				killerPeer.Score -= (int)((float)this._gameMode.GetScoreForKill(killedAgent) * 1.5f);
				int num = killerPeer.KillCount;
				killerPeer.KillCount = num - 1;
			}
			this._missionScoreboardComponent.PlayerPropertiesChanged(killerPeer.GetNetworkPeer());
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new KillDeathCountChange(killerPeer.GetNetworkPeer(), null, killerPeer.KillCount, killerPeer.AssistCount, killerPeer.DeathCount, killerPeer.Score));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x0008AD48 File Offset: 0x00088F48
		protected virtual void OnPlayerDies(MissionPeer peer, MissionPeer affectorPeer, MissionPeer assistorPeer)
		{
			if (assistorPeer != null)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new KillDeathCountChange(assistorPeer.GetNetworkPeer(), null, assistorPeer.KillCount, assistorPeer.AssistCount, assistorPeer.DeathCount, assistorPeer.Score));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			int deathCount = peer.DeathCount;
			peer.DeathCount = deathCount + 1;
			peer.SpawnTimer.Reset(Mission.Current.CurrentTime, (float)MissionLobbyComponent.GetSpawnPeriodDurationForPeer(peer));
			peer.WantsToSpawnAsBot = false;
			peer.HasSpawnTimerExpired = false;
			this._missionScoreboardComponent.PlayerPropertiesChanged(peer.GetNetworkPeer());
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new KillDeathCountChange(peer.GetNetworkPeer(), (affectorPeer != null) ? affectorPeer.GetNetworkPeer() : null, peer.KillCount, peer.AssistCount, peer.DeathCount, peer.Score));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x0008AE18 File Offset: 0x00089018
		protected virtual void OnBotKills(Agent botAgent, Agent killedAgent)
		{
			Agent botAgent2 = botAgent;
			if (((botAgent2 != null) ? botAgent2.Team : null) != null)
			{
				Formation formation = botAgent.Formation;
				if (((formation != null) ? formation.PlayerOwner : null) != null)
				{
					NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.SingleOrDefault<NetworkCommunicator>((NetworkCommunicator x) => x.GetComponent<MissionPeer>() != null && x.GetComponent<MissionPeer>().ControlledFormation == botAgent.Formation);
					if (networkCommunicator != null)
					{
						MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
						MissionPeer missionPeer = killedAgent.MissionPeer;
						NetworkCommunicator networkCommunicator2 = ((missionPeer != null) ? missionPeer.GetNetworkPeer() : null);
						if (killedAgent.MissionPeer == null)
						{
							NetworkCommunicator networkCommunicator3 = GameNetwork.NetworkPeers.SingleOrDefault<NetworkCommunicator>((NetworkCommunicator x) => x.GetComponent<MissionPeer>() != null && x.GetComponent<MissionPeer>().ControlledFormation == killedAgent.Formation);
							if (networkCommunicator3 != null)
							{
								NetworkCommunicator networkCommunicator4 = networkCommunicator3;
								component.OnKillAnotherPeer(networkCommunicator4.GetComponent<MissionPeer>());
							}
						}
						else
						{
							component.OnKillAnotherPeer(killedAgent.MissionPeer);
						}
						if (botAgent.Team.IsEnemyOf(killedAgent.Team))
						{
							MissionPeer missionPeer2 = component;
							int num = missionPeer2.KillCount;
							missionPeer2.KillCount = num + 1;
							component.Score += this._gameMode.GetScoreForKill(killedAgent);
						}
						else
						{
							MissionPeer missionPeer3 = component;
							int num = missionPeer3.KillCount;
							missionPeer3.KillCount = num - 1;
							component.Score -= (int)((float)this._gameMode.GetScoreForKill(killedAgent) * 1.5f);
						}
						this._missionScoreboardComponent.PlayerPropertiesChanged(networkCommunicator);
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new KillDeathCountChange(networkCommunicator, null, component.KillCount, component.AssistCount, component.DeathCount, component.Score));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
				else
				{
					MissionScoreboardComponent.MissionScoreboardSide sideSafe = this._missionScoreboardComponent.GetSideSafe(botAgent.Team.Side);
					BotData botScores = sideSafe.BotScores;
					if (botAgent.Team.IsEnemyOf(killedAgent.Team))
					{
						botScores.KillCount++;
					}
					else
					{
						botScores.KillCount--;
					}
					this._missionScoreboardComponent.BotPropertiesChanged(sideSafe.Side);
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new BotData(sideSafe.Side, botScores.KillCount, botScores.AssistCount, botScores.DeathCount, botScores.AliveCount));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				this._missionScoreboardComponent.BotPropertiesChanged(botAgent.Team.Side);
			}
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x0008B084 File Offset: 0x00089284
		protected virtual void OnBotDies(Agent botAgent, MissionPeer affectorPeer, MissionPeer assistorPeer)
		{
			if (assistorPeer != null)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new KillDeathCountChange(assistorPeer.GetNetworkPeer(), (affectorPeer != null) ? affectorPeer.GetNetworkPeer() : null, assistorPeer.KillCount, assistorPeer.AssistCount, assistorPeer.DeathCount, assistorPeer.Score));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			if (botAgent != null)
			{
				Formation formation = botAgent.Formation;
				if (((formation != null) ? formation.PlayerOwner : null) != null)
				{
					NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.SingleOrDefault<NetworkCommunicator>((NetworkCommunicator x) => x.GetComponent<MissionPeer>() != null && x.GetComponent<MissionPeer>().ControlledFormation == botAgent.Formation);
					if (networkCommunicator != null)
					{
						MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
						MissionPeer missionPeer = component;
						int num = missionPeer.DeathCount;
						missionPeer.DeathCount = num + 1;
						MissionPeer missionPeer2 = component;
						num = missionPeer2.BotsUnderControlAlive;
						missionPeer2.BotsUnderControlAlive = num - 1;
						this._missionScoreboardComponent.PlayerPropertiesChanged(networkCommunicator);
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new KillDeathCountChange(networkCommunicator, (affectorPeer != null) ? affectorPeer.GetNetworkPeer() : null, component.KillCount, component.AssistCount, component.DeathCount, component.Score));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new BotsControlledChange(networkCommunicator, component.BotsUnderControlAlive, component.BotsUnderControlTotal));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
				else
				{
					MissionScoreboardComponent.MissionScoreboardSide sideSafe = this._missionScoreboardComponent.GetSideSafe(botAgent.Team.Side);
					BotData botScores = sideSafe.BotScores;
					botScores.DeathCount++;
					botScores.AliveCount--;
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new BotData(sideSafe.Side, botScores.KillCount, botScores.AssistCount, botScores.DeathCount, botScores.AliveCount));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				this._missionScoreboardComponent.BotPropertiesChanged(botAgent.Team.Side);
			}
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x0008B24C File Offset: 0x0008944C
		public override void OnClearScene()
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeersIncludingDisconnectedPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null)
				{
					component.BotsUnderControlAlive = 0;
					component.BotsUnderControlTotal = 0;
					component.ControlledFormation = null;
				}
			}
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x0008B2B0 File Offset: 0x000894B0
		public static int GetSpawnPeriodDurationForPeer(MissionPeer peer)
		{
			return Mission.Current.GetMissionBehavior<SpawnComponent>().GetMaximumReSpawnPeriodForPeer(peer);
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x0008B2C4 File Offset: 0x000894C4
		public virtual void SetStateEndingAsServer()
		{
			this.CurrentMultiplayerState = MissionLobbyComponent.MultiplayerGameState.Ending;
			MBDebug.Print("Multiplayer game mission ending", 0, Debug.DebugColor.White, 17592186044416UL);
			this._timerComponent.StartTimerAsServer(MissionLobbyComponent.PostMatchWaitDuration);
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new MissionStateChange(this.CurrentMultiplayerState, this._timerComponent.GetCurrentTimerStartTime().NumberOfTicks));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			Debug.Print(string.Format("Current multiplayer state sent to clients: {0}", this.CurrentMultiplayerState), 0, Debug.DebugColor.White, 17592186044416UL);
			Action onPostMatchEnded = this.OnPostMatchEnded;
			if (onPostMatchEnded == null)
			{
				return;
			}
			onPostMatchEnded();
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x0008B364 File Offset: 0x00089564
		private void SetStatePlayingAsServer()
		{
			this._warmupComponent = null;
			this.CurrentMultiplayerState = MissionLobbyComponent.MultiplayerGameState.Playing;
			this._timerComponent.StartTimerAsServer((float)(MultiplayerOptions.OptionType.MapTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) * 60));
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new MissionStateChange(this.CurrentMultiplayerState, this._timerComponent.GetCurrentTimerStartTime().NumberOfTicks));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x0008B3C5 File Offset: 0x000895C5
		protected virtual void EndGameAsServer()
		{
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x0008B3C8 File Offset: 0x000895C8
		private MissionPeer RemoveHittersAndGetAssistorPeer(MissionPeer killerPeer, Agent killedAgent)
		{
			Agent.Hitter assistingHitter = killedAgent.GetAssistingHitter(killerPeer);
			if (((assistingHitter != null) ? assistingHitter.HitterPeer : null) != null)
			{
				if (!assistingHitter.IsFriendlyHit)
				{
					MissionPeer hitterPeer = assistingHitter.HitterPeer;
					int num = hitterPeer.AssistCount;
					hitterPeer.AssistCount = num + 1;
				}
				else
				{
					MissionPeer hitterPeer2 = assistingHitter.HitterPeer;
					int num = hitterPeer2.AssistCount;
					hitterPeer2.AssistCount = num - 1;
				}
			}
			if (assistingHitter == null)
			{
				return null;
			}
			return assistingHitter.HitterPeer;
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x0008B42A File Offset: 0x0008962A
		private void SetStateEndingAsClient()
		{
			Action onPostMatchEnded = this.OnPostMatchEnded;
			if (onPostMatchEnded == null)
			{
				return;
			}
			onPostMatchEnded();
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x0008B43C File Offset: 0x0008963C
		public void RequestCultureSelection()
		{
			Action onCultureSelectionRequested = this.OnCultureSelectionRequested;
			if (onCultureSelectionRequested == null)
			{
				return;
			}
			onCultureSelectionRequested();
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x0008B44E File Offset: 0x0008964E
		public void RequestAdminMessage(string message, bool isBroadcast)
		{
			Action<string, bool> onAdminMessageRequested = this.OnAdminMessageRequested;
			if (onAdminMessageRequested == null)
			{
				return;
			}
			onAdminMessageRequested(message, isBroadcast);
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x0008B464 File Offset: 0x00089664
		public void RequestTroopSelection()
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new RequestChangeCharacterMessage(GameNetwork.MyPeer));
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			if (GameNetwork.IsServer)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (component != null && this._gameMode.CheckIfPlayerCanDespawn(component))
				{
					this.DespawnPlayer(component);
				}
			}
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x0008B4BC File Offset: 0x000896BC
		public void OnCultureSelected(BasicCultureObject culture)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new RequestCultureChange(culture));
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			if (GameNetwork.IsServer)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (component != null && this._gameMode.CheckIfPlayerCanDespawn(component))
				{
					component.Culture = culture;
					this.DespawnPlayer(component);
				}
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x0600263A RID: 9786 RVA: 0x0008B517 File Offset: 0x00089717
		// (set) Token: 0x0600263B RID: 9787 RVA: 0x0008B51F File Offset: 0x0008971F
		public MultiplayerGameType MissionType { get; set; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x0600263C RID: 9788 RVA: 0x0008B528 File Offset: 0x00089728
		// (set) Token: 0x0600263D RID: 9789 RVA: 0x0008B530 File Offset: 0x00089730
		public MissionLobbyComponent.MultiplayerGameState CurrentMultiplayerState
		{
			get
			{
				return this._currentMultiplayerState;
			}
			private set
			{
				if (this._currentMultiplayerState != value)
				{
					this._currentMultiplayerState = value;
					Action<MissionLobbyComponent.MultiplayerGameState> currentMultiplayerStateChanged = this.CurrentMultiplayerStateChanged;
					if (currentMultiplayerStateChanged == null)
					{
						return;
					}
					currentMultiplayerStateChanged(value);
				}
			}
		}

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x0600263E RID: 9790 RVA: 0x0008B554 File Offset: 0x00089754
		// (remove) Token: 0x0600263F RID: 9791 RVA: 0x0008B58C File Offset: 0x0008978C
		public event Action<MissionLobbyComponent.MultiplayerGameState> CurrentMultiplayerStateChanged;

		// Token: 0x06002640 RID: 9792 RVA: 0x0008B5C1 File Offset: 0x000897C1
		public int GetRandomFaceSeedForCharacter(BasicCharacterObject character, int addition = 0)
		{
			IRoundComponent roundComponent = this._roundComponent;
			return character.GetDefaultFaceSeed(addition + ((roundComponent != null) ? roundComponent.RoundCount : 0)) % 2000;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x0008B5E4 File Offset: 0x000897E4
		[CommandLineFunctionality.CommandLineArgumentFunction("kill_player", "mp_host")]
		public static string MPHostChangeParam(List<string> strings)
		{
			if (Mission.Current == null)
			{
				return "kill_player can only be called within a mission.";
			}
			if (!GameNetwork.IsServer)
			{
				return "kill_player can only be called by the server.";
			}
			if (strings == null || strings.Count == 0)
			{
				return "usage: kill_player {UserName}.";
			}
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.UserName == strings[0] && networkCommunicator.ControlledAgent != null)
				{
					Mission.Current.KillAgentCheat(networkCommunicator.ControlledAgent);
					return "Success.";
				}
			}
			return "Could not find the player " + strings[0] + " or the agent.";
		}

		// Token: 0x04000EA3 RID: 3747
		private static readonly float InactivityThreshold = 2f;

		// Token: 0x04000EA4 RID: 3748
		public static readonly float PostMatchWaitDuration = 15f;

		// Token: 0x04000EA5 RID: 3749
		private const int MaxReceivedClanNameLength = 64;

		// Token: 0x04000EA8 RID: 3752
		private bool[] _classRestrictions = new bool[8];

		// Token: 0x04000EAB RID: 3755
		private MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x04000EAC RID: 3756
		private MissionMultiplayerGameModeBase _gameMode;

		// Token: 0x04000EAD RID: 3757
		private MultiplayerTimerComponent _timerComponent;

		// Token: 0x04000EAE RID: 3758
		private IRoundComponent _roundComponent;

		// Token: 0x04000EAF RID: 3759
		private Timer _inactivityTimer;

		// Token: 0x04000EB0 RID: 3760
		private MultiplayerWarmupComponent _warmupComponent;

		// Token: 0x04000EB1 RID: 3761
		private static readonly Dictionary<Tuple<LobbyMissionType, bool>, Type> _lobbyComponentTypes = new Dictionary<Tuple<LobbyMissionType, bool>, Type>();

		// Token: 0x04000EB2 RID: 3762
		private bool _usingFixedBanners;

		// Token: 0x04000EB4 RID: 3764
		private MissionLobbyComponent.MultiplayerGameState _currentMultiplayerState;

		// Token: 0x02000581 RID: 1409
		public enum MultiplayerGameState
		{
			// Token: 0x04001ED0 RID: 7888
			WaitingFirstPlayers,
			// Token: 0x04001ED1 RID: 7889
			Playing,
			// Token: 0x04001ED2 RID: 7890
			Ending
		}
	}
}
