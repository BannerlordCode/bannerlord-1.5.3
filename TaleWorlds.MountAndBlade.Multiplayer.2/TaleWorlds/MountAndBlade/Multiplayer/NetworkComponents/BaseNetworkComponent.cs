using System;
using System.Threading.Tasks;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.NetworkComponents
{
	// Token: 0x02000066 RID: 102
	public class BaseNetworkComponent : UdpNetworkComponent
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000DD1A File Offset: 0x0000BF1A
		// (set) Token: 0x06000308 RID: 776 RVA: 0x0000DD22 File Offset: 0x0000BF22
		public MultiplayerIntermissionState ClientIntermissionState { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000DD2B File Offset: 0x0000BF2B
		// (set) Token: 0x0600030A RID: 778 RVA: 0x0000DD33 File Offset: 0x0000BF33
		public float CurrentIntermissionTimer { get; private set; }

		// Token: 0x0600030B RID: 779 RVA: 0x0000DD3C File Offset: 0x0000BF3C
		private void EnsureBaseNetworkComponentData()
		{
			if (this._baseNetworkComponentData == null)
			{
				this._baseNetworkComponentData = GameNetwork.GetNetworkComponent<BaseNetworkComponentData>();
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000DD54 File Offset: 0x0000BF54
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			base.AddRemoveMessageHandlers(registerer);
			if (GameNetwork.IsClientOrReplay)
			{
				registerer.RegisterBaseHandler<AddPeerComponent>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventAddPeerComponent));
				registerer.RegisterBaseHandler<RemovePeerComponent>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventRemovePeerComponent));
				registerer.RegisterBaseHandler<SynchronizingDone>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSynchronizingDone));
				registerer.RegisterBaseHandler<LoadMission>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventLoadMission));
				registerer.RegisterBaseHandler<UnloadMission>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventUnloadMission));
				registerer.RegisterBaseHandler<InitializeCustomGameMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventInitializeCustomGame));
				registerer.RegisterBaseHandler<MultiplayerOptionsInitial>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventMultiplayerOptionsInitial));
				registerer.RegisterBaseHandler<MultiplayerOptionsImmediate>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventMultiplayerOptionsImmediate));
				registerer.RegisterBaseHandler<MultiplayerIntermissionUpdate>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventMultiplayerIntermissionUpdate));
				registerer.RegisterBaseHandler<MultiplayerIntermissionMapItemAdded>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventIntermissionMapItemAdded));
				registerer.RegisterBaseHandler<MultiplayerIntermissionCultureItemAdded>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventIntermissionCultureItemAdded));
				registerer.RegisterBaseHandler<MultiplayerIntermissionMapItemVoteCountChanged>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventIntermissionMapItemVoteCountChanged));
				registerer.RegisterBaseHandler<MultiplayerIntermissionCultureItemVoteCountChanged>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventIntermissionCultureItemVoteCountChanged));
				registerer.RegisterBaseHandler<MultiplayerIntermissionUsableMapAdded>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventUsableMapAdded));
				registerer.RegisterBaseHandler<UpdateIntermissionVotingManagerValues>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventUpdateIntermissionVotingManagerValues));
				registerer.RegisterBaseHandler<SyncMutedPlayers>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSyncMutedPlayers));
				registerer.RegisterBaseHandler<SyncPlayerMuteState>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSyncPlayerMuteState));
				return;
			}
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<FinishedLoading>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventFinishedLoading));
				registerer.RegisterBaseHandler<SyncRelevantGameOptionsToServer>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleSyncRelevantGameOptionsToServer));
				registerer.RegisterBaseHandler<IntermissionVote>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleIntermissionClientVote));
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000DEE4 File Offset: 0x0000C0E4
		public override void OnUdpNetworkHandlerTick(float dt)
		{
			base.OnUdpNetworkHandlerTick(dt);
			if (GameNetwork.IsClientOrReplay && (this.ClientIntermissionState == MultiplayerIntermissionState.CountingForMission || this.ClientIntermissionState == MultiplayerIntermissionState.CountingForEnd || this.ClientIntermissionState == MultiplayerIntermissionState.CountingForMapVote || this.ClientIntermissionState == MultiplayerIntermissionState.CountingForCultureVote))
			{
				this.CurrentIntermissionTimer -= dt;
				if (this.CurrentIntermissionTimer <= 0f)
				{
					this.CurrentIntermissionTimer = 0f;
				}
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000DF4C File Offset: 0x0000C14C
		public override void HandleNewClientConnect(PlayerConnectionInfo playerConnectionInfo)
		{
			this.EnsureBaseNetworkComponentData();
			NetworkCommunicator networkPeer = playerConnectionInfo.NetworkPeer;
			if (!networkPeer.IsServerPeer)
			{
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new MultiplayerOptionsInitial());
				GameNetwork.EndModuleEventAsServer();
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new MultiplayerOptionsImmediate());
				GameNetwork.EndModuleEventAsServer();
				foreach (IntermissionVoteItem intermissionVoteItem in MultiplayerIntermissionVotingManager.Instance.MapVoteItems)
				{
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new MultiplayerIntermissionMapItemAdded(intermissionVoteItem.Id));
					GameNetwork.EndModuleEventAsServer();
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new MultiplayerIntermissionMapItemVoteCountChanged(intermissionVoteItem.Index, intermissionVoteItem.VoteCount));
					GameNetwork.EndModuleEventAsServer();
				}
				foreach (IntermissionVoteItem intermissionVoteItem2 in MultiplayerIntermissionVotingManager.Instance.CultureVoteItems)
				{
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new MultiplayerIntermissionCultureItemAdded(intermissionVoteItem2.Id));
					GameNetwork.EndModuleEventAsServer();
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new MultiplayerIntermissionCultureItemVoteCountChanged(intermissionVoteItem2.Index, intermissionVoteItem2.VoteCount));
					GameNetwork.EndModuleEventAsServer();
				}
				if (networkPeer.IsAdmin)
				{
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new MultiplayerOptionsDefault());
					GameNetwork.EndModuleEventAsServer();
					foreach (CustomGameUsableMap customGameUsableMap in MultiplayerIntermissionVotingManager.Instance.UsableMaps)
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new MultiplayerIntermissionUsableMapAdded(customGameUsableMap.Map, customGameUsableMap.IsCompatibleWithAllGameTypes, customGameUsableMap.IsCompatibleWithAllGameTypes ? 0 : customGameUsableMap.CompatibleGameTypes.Count, customGameUsableMap.CompatibleGameTypes));
						GameNetwork.EndModuleEventAsServer();
					}
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new UpdateIntermissionVotingManagerValues());
					GameNetwork.EndModuleEventAsServer();
				}
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new SyncMutedPlayers(MultiplayerGlobalMutedPlayersManager.MutedPlayers));
				GameNetwork.EndModuleEventAsServer();
				if (BannerlordNetwork.LobbyMissionType == LobbyMissionType.Custom || BannerlordNetwork.LobbyMissionType == LobbyMissionType.Community)
				{
					bool flag = false;
					string text = "";
					string text2 = "";
					if ((GameNetwork.IsDedicatedServer && Mission.Current != null) || !GameNetwork.IsDedicatedServer)
					{
						flag = true;
						MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.Map, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out text);
						MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.GameType, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out text2);
					}
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new InitializeCustomGameMessage(flag, text2, text, this._baseNetworkComponentData.CurrentBattleIndex));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000E1F0 File Offset: 0x0000C3F0
		public override void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
			MultiplayerIntermissionVotingManager.Instance.HandlePlayerDisconnect(networkPeer.VirtualPlayer.Id);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000E207 File Offset: 0x0000C407
		public void IntermissionCastVote(string itemID, int voteCount)
		{
			GameNetwork.BeginModuleEventAsClient();
			GameNetwork.WriteMessage(new IntermissionVote(itemID, voteCount));
			GameNetwork.EndModuleEventAsClient();
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000E220 File Offset: 0x0000C420
		public override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			Mission mission = Mission.Current;
			MissionNetworkComponent missionNetworkComponent = ((mission != null) ? mission.GetMissionBehavior<MissionNetworkComponent>() : null);
			if (missionNetworkComponent != null)
			{
				missionNetworkComponent.OnClientSynchronized(networkPeer);
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000E249 File Offset: 0x0000C449
		public override void OnUdpNetworkHandlerClose()
		{
			base.OnUdpNetworkHandlerClose();
			MultiplayerGlobalMutedPlayersManager.ClearMutedPlayers();
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000E256 File Offset: 0x0000C456
		// (set) Token: 0x06000314 RID: 788 RVA: 0x0000E25E File Offset: 0x0000C45E
		public bool DisplayingWelcomeMessage { get; private set; }

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000315 RID: 789 RVA: 0x0000E268 File Offset: 0x0000C468
		// (remove) Token: 0x06000316 RID: 790 RVA: 0x0000E2A0 File Offset: 0x0000C4A0
		public event BaseNetworkComponent.WelcomeMessageReceivedDelegate WelcomeMessageReceived = delegate(string messageText)
		{
			InformationManager.DisplayMessage(new InformationMessage(messageText));
		};

		// Token: 0x06000317 RID: 791 RVA: 0x0000E2D5 File Offset: 0x0000C4D5
		public void SetDisplayingWelcomeMessage(bool displaying)
		{
			this.DisplayingWelcomeMessage = displaying;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000E2E0 File Offset: 0x0000C4E0
		private void HandleServerEventMultiplayerOptionsInitial(GameNetworkMessage baseMessage)
		{
			MultiplayerOptionsInitial multiplayerOptionsInitial = (MultiplayerOptionsInitial)baseMessage;
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				if (optionProperty.Replication == MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad)
				{
					switch (optionProperty.OptionValueType)
					{
					case MultiplayerOptions.OptionValueType.Bool:
					{
						bool flag;
						multiplayerOptionsInitial.GetOption(optionType).GetValue(out flag);
						optionType.SetValue(flag, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
						break;
					}
					case MultiplayerOptions.OptionValueType.Integer:
					case MultiplayerOptions.OptionValueType.Enum:
					{
						int num;
						multiplayerOptionsInitial.GetOption(optionType).GetValue(out num);
						optionType.SetValue(num, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
						break;
					}
					case MultiplayerOptions.OptionValueType.String:
					{
						string text;
						multiplayerOptionsInitial.GetOption(optionType).GetValue(out text);
						optionType.SetValue(text, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
						break;
					}
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
			}
			string strValue = MultiplayerOptions.OptionType.WelcomeMessage.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			if (!string.IsNullOrEmpty(strValue))
			{
				BaseNetworkComponent.WelcomeMessageReceivedDelegate welcomeMessageReceived = this.WelcomeMessageReceived;
				if (welcomeMessageReceived == null)
				{
					return;
				}
				welcomeMessageReceived(strValue);
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000E3AC File Offset: 0x0000C5AC
		private void HandleServerEventMultiplayerOptionsImmediate(GameNetworkMessage baseMessage)
		{
			MultiplayerOptionsImmediate multiplayerOptionsImmediate = (MultiplayerOptionsImmediate)baseMessage;
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				if (optionProperty.Replication == MultiplayerOptionsProperty.ReplicationOccurrence.Immediately)
				{
					switch (optionProperty.OptionValueType)
					{
					case MultiplayerOptions.OptionValueType.Bool:
					{
						bool flag;
						multiplayerOptionsImmediate.GetOption(optionType).GetValue(out flag);
						optionType.SetValue(flag, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
						break;
					}
					case MultiplayerOptions.OptionValueType.Integer:
					case MultiplayerOptions.OptionValueType.Enum:
					{
						int num;
						multiplayerOptionsImmediate.GetOption(optionType).GetValue(out num);
						optionType.SetValue(num, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
						break;
					}
					case MultiplayerOptions.OptionValueType.String:
					{
						string text;
						multiplayerOptionsImmediate.GetOption(optionType).GetValue(out text);
						optionType.SetValue(text, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
						break;
					}
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000E454 File Offset: 0x0000C654
		private void HandleServerEventMultiplayerIntermissionUpdate(GameNetworkMessage baseMessage)
		{
			MultiplayerIntermissionUpdate multiplayerIntermissionUpdate = (MultiplayerIntermissionUpdate)baseMessage;
			this.CurrentIntermissionTimer = multiplayerIntermissionUpdate.IntermissionTimer;
			this.ClientIntermissionState = multiplayerIntermissionUpdate.IntermissionState;
			Action onIntermissionStateUpdated = this.OnIntermissionStateUpdated;
			if (onIntermissionStateUpdated == null)
			{
				return;
			}
			onIntermissionStateUpdated();
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000E490 File Offset: 0x0000C690
		private void HandleServerEventIntermissionMapItemAdded(GameNetworkMessage baseMessage)
		{
			MultiplayerIntermissionMapItemAdded multiplayerIntermissionMapItemAdded = (MultiplayerIntermissionMapItemAdded)baseMessage;
			MultiplayerIntermissionVotingManager.Instance.AddMapItem(multiplayerIntermissionMapItemAdded.MapId);
			Action onIntermissionStateUpdated = this.OnIntermissionStateUpdated;
			if (onIntermissionStateUpdated == null)
			{
				return;
			}
			onIntermissionStateUpdated();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000E4C4 File Offset: 0x0000C6C4
		private void HandleServerEventUsableMapAdded(GameNetworkMessage baseMessage)
		{
			MultiplayerIntermissionUsableMapAdded multiplayerIntermissionUsableMapAdded = (MultiplayerIntermissionUsableMapAdded)baseMessage;
			MultiplayerIntermissionVotingManager.Instance.AddUsableMap(new CustomGameUsableMap(multiplayerIntermissionUsableMapAdded.MapId, multiplayerIntermissionUsableMapAdded.IsCompatibleWithAllGameTypes, multiplayerIntermissionUsableMapAdded.CompatibleGameTypes));
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000E4FC File Offset: 0x0000C6FC
		private void HandleServerEventSyncPlayerMuteState(GameNetworkMessage baseMessage)
		{
			SyncPlayerMuteState syncPlayerMuteState = (SyncPlayerMuteState)baseMessage;
			if (syncPlayerMuteState.IsMuted)
			{
				MultiplayerGlobalMutedPlayersManager.MutePlayer(syncPlayerMuteState.PlayerId);
				return;
			}
			MultiplayerGlobalMutedPlayersManager.UnmutePlayer(syncPlayerMuteState.PlayerId);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000E530 File Offset: 0x0000C730
		private void HandleServerEventSyncMutedPlayers(GameNetworkMessage baseMessage)
		{
			SyncMutedPlayers syncMutedPlayers = (SyncMutedPlayers)baseMessage;
			MultiplayerGlobalMutedPlayersManager.ClearMutedPlayers();
			if (syncMutedPlayers.MutedPlayerCount > 0)
			{
				foreach (PlayerId playerId in syncMutedPlayers.MutedPlayerIds)
				{
					MultiplayerGlobalMutedPlayersManager.MutePlayer(playerId);
				}
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000E598 File Offset: 0x0000C798
		private void HandleServerEventUpdateIntermissionVotingManagerValues(GameNetworkMessage baseMessage)
		{
			UpdateIntermissionVotingManagerValues updateIntermissionVotingManagerValues = (UpdateIntermissionVotingManagerValues)baseMessage;
			MultiplayerIntermissionVotingManager.Instance.IsAutomatedBattleSwitchingEnabled = updateIntermissionVotingManagerValues.IsAutomatedBattleSwitchingEnabled;
			MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = updateIntermissionVotingManagerValues.IsMapVoteEnabled;
			MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled = updateIntermissionVotingManagerValues.IsCultureVoteEnabled;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000E5DC File Offset: 0x0000C7DC
		private void HandleServerEventIntermissionCultureItemAdded(GameNetworkMessage baseMessage)
		{
			MultiplayerIntermissionCultureItemAdded multiplayerIntermissionCultureItemAdded = (MultiplayerIntermissionCultureItemAdded)baseMessage;
			MultiplayerIntermissionVotingManager.Instance.AddCultureItem(multiplayerIntermissionCultureItemAdded.CultureId);
			Action onIntermissionStateUpdated = this.OnIntermissionStateUpdated;
			if (onIntermissionStateUpdated == null)
			{
				return;
			}
			onIntermissionStateUpdated();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000E610 File Offset: 0x0000C810
		private void HandleServerEventIntermissionMapItemVoteCountChanged(GameNetworkMessage baseMessage)
		{
			MultiplayerIntermissionMapItemVoteCountChanged multiplayerIntermissionMapItemVoteCountChanged = (MultiplayerIntermissionMapItemVoteCountChanged)baseMessage;
			MultiplayerIntermissionVotingManager.Instance.SetVotesOfMap(multiplayerIntermissionMapItemVoteCountChanged.MapItemIndex, multiplayerIntermissionMapItemVoteCountChanged.VoteCount);
			Action onIntermissionStateUpdated = this.OnIntermissionStateUpdated;
			if (onIntermissionStateUpdated == null)
			{
				return;
			}
			onIntermissionStateUpdated();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000E64C File Offset: 0x0000C84C
		private void HandleServerEventIntermissionCultureItemVoteCountChanged(GameNetworkMessage baseMessage)
		{
			MultiplayerIntermissionCultureItemVoteCountChanged multiplayerIntermissionCultureItemVoteCountChanged = (MultiplayerIntermissionCultureItemVoteCountChanged)baseMessage;
			MultiplayerIntermissionVotingManager.Instance.SetVotesOfCulture(multiplayerIntermissionCultureItemVoteCountChanged.CultureItemIndex, multiplayerIntermissionCultureItemVoteCountChanged.VoteCount);
			Action onIntermissionStateUpdated = this.OnIntermissionStateUpdated;
			if (onIntermissionStateUpdated == null)
			{
				return;
			}
			onIntermissionStateUpdated();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000E688 File Offset: 0x0000C888
		private void HandleServerEventAddPeerComponent(GameNetworkMessage baseMessage)
		{
			AddPeerComponent addPeerComponent = (AddPeerComponent)baseMessage;
			NetworkCommunicator peer = addPeerComponent.Peer;
			uint componentId = addPeerComponent.ComponentId;
			if (peer.GetComponent(componentId) == null)
			{
				peer.AddComponent(componentId);
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000E6B8 File Offset: 0x0000C8B8
		private void HandleServerEventRemovePeerComponent(GameNetworkMessage baseMessage)
		{
			RemovePeerComponent removePeerComponent = (RemovePeerComponent)baseMessage;
			NetworkCommunicator peer = removePeerComponent.Peer;
			uint componentId = removePeerComponent.ComponentId;
			PeerComponent component = peer.GetComponent(componentId);
			peer.RemoveComponent(component);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000E6E8 File Offset: 0x0000C8E8
		private void HandleServerEventSynchronizingDone(GameNetworkMessage baseMessage)
		{
			SynchronizingDone synchronizingDone = (SynchronizingDone)baseMessage;
			NetworkCommunicator peer = synchronizingDone.Peer;
			Mission mission = Mission.Current;
			MissionNetworkComponent missionNetworkComponent = ((mission != null) ? mission.GetMissionBehavior<MissionNetworkComponent>() : null);
			if (missionNetworkComponent != null && !peer.IsMine)
			{
				missionNetworkComponent.OnClientSynchronized(peer);
				return;
			}
			peer.IsSynchronized = synchronizingDone.Synchronized;
			if (missionNetworkComponent != null && synchronizingDone.Synchronized)
			{
				if (peer.GetComponent<MissionPeer>() == null)
				{
					LobbyClient gameClient = NetworkMain.GameClient;
					CommunityClient communityClient = NetworkMain.CommunityClient;
					if (communityClient.IsInGame)
					{
						communityClient.QuitFromGame();
						return;
					}
					if (gameClient.CurrentState == LobbyClient.State.InCustomGame)
					{
						gameClient.QuitFromCustomGame();
						return;
					}
					if (gameClient.CurrentState == LobbyClient.State.HostingCustomGame)
					{
						gameClient.EndCustomGame();
						return;
					}
					gameClient.QuitFromMatchmakerGame();
					return;
				}
				else
				{
					missionNetworkComponent.OnClientSynchronized(peer);
				}
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000E798 File Offset: 0x0000C998
		private async void HandleServerEventLoadMission(GameNetworkMessage baseMessage)
		{
			LoadMission message = (LoadMission)baseMessage;
			this.EnsureBaseNetworkComponentData();
			while (GameStateManager.Current.ActiveState is MissionState)
			{
				await Task.Delay(1);
			}
			if (GameNetwork.MyPeer != null)
			{
				GameNetwork.MyPeer.IsSynchronized = false;
			}
			this.CurrentIntermissionTimer = 0f;
			this.ClientIntermissionState = MultiplayerIntermissionState.Idle;
			this._baseNetworkComponentData.UpdateCurrentBattleIndex(message.BattleIndex);
			if (!Module.CurrentModule.StartMultiplayerGame(message.GameType, message.Map))
			{
				Debug.FailedAssert("[DEBUG]Invalid multiplayer game type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\NetworkComponents\\BaseNetworkComponent.cs", "HandleServerEventLoadMission", 470);
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000E7DC File Offset: 0x0000C9DC
		private void HandleServerEventUnloadMission(GameNetworkMessage baseMessage)
		{
			UnloadMission unloadMission = (UnloadMission)baseMessage;
			this.HandleServerEventUnloadMissionAux(unloadMission);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000E7F8 File Offset: 0x0000C9F8
		private void HandleServerEventInitializeCustomGame(GameNetworkMessage baseMessage)
		{
			InitializeCustomGameMessage initializeCustomGameMessage = (InitializeCustomGameMessage)baseMessage;
			this.InitializeCustomGameAux(initializeCustomGameMessage);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000E814 File Offset: 0x0000CA14
		private async void InitializeCustomGameAux(InitializeCustomGameMessage message)
		{
			this.EnsureBaseNetworkComponentData();
			await Task.Delay(200);
			while (!(GameStateManager.Current.ActiveState is LobbyGameStateCustomGameClient) && !(GameStateManager.Current.ActiveState is LobbyGameStateCommunityClient))
			{
				await Task.Delay(1);
			}
			if (message.InMission)
			{
				MBDebug.Print(string.Concat(new string[] { "Client: I have received InitializeCustomGameMessage with mission ", message.GameType, " ", message.Map, ". Loading it..." }), 0, Debug.DebugColor.White, 17179869184UL);
				this._baseNetworkComponentData.UpdateCurrentBattleIndex(message.BattleIndex);
				if (!Module.CurrentModule.StartMultiplayerGame(message.GameType, message.Map))
				{
					Debug.FailedAssert("[DEBUG]Invalid multiplayer game type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\NetworkComponents\\BaseNetworkComponent.cs", "InitializeCustomGameAux", 507);
				}
			}
			else
			{
				LoadingWindow.DisableGlobalLoadingWindow();
				GameNetwork.SyncRelevantGameOptionsToServer();
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000E858 File Offset: 0x0000CA58
		private async void HandleServerEventUnloadMissionAux(UnloadMission message)
		{
			GameNetwork.MyPeer.IsSynchronized = false;
			this.CurrentIntermissionTimer = 0f;
			this.ClientIntermissionState = MultiplayerIntermissionState.Idle;
			if (Mission.Current != null)
			{
				MissionCustomGameClientComponent missionBehavior = Mission.Current.GetMissionBehavior<MissionCustomGameClientComponent>();
				if (missionBehavior != null)
				{
					missionBehavior.SetServerEndingBeforeClientLoaded(message.UnloadingForBattleIndexMismatch);
				}
				MissionCommunityClientComponent missionBehavior2 = Mission.Current.GetMissionBehavior<MissionCommunityClientComponent>();
				if (missionBehavior2 != null)
				{
					missionBehavior2.SetServerEndingBeforeClientLoaded(message.UnloadingForBattleIndexMismatch);
				}
			}
			BannerlordNetwork.EndMultiplayerLobbyMission();
			Game.Current.GetGameHandler<ChatBox>().ResetMuteList();
			while (Mission.Current != null)
			{
				await Task.Delay(1);
			}
			LoadingWindow.DisableGlobalLoadingWindow();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000E89C File Offset: 0x0000CA9C
		private bool HandleClientEventFinishedLoading(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			FinishedLoading finishedLoading = (FinishedLoading)baseMessage;
			this.HandleClientEventFinishedLoadingAux(networkPeer, finishedLoading);
			return true;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000E8BC File Offset: 0x0000CABC
		private async void HandleClientEventFinishedLoadingAux(NetworkCommunicator networkPeer, FinishedLoading message)
		{
			this.EnsureBaseNetworkComponentData();
			while (Mission.Current != null && Mission.Current.CurrentState != Mission.State.Continuing)
			{
				await Task.Delay(1);
			}
			if (!networkPeer.IsServerPeer)
			{
				MBDebug.Print("Server: " + networkPeer.UserName + " has finished loading. From now on, I will include him in the broadcasted messages", 0, Debug.DebugColor.White, 17179869184UL);
				if (Mission.Current == null || this._baseNetworkComponentData.CurrentBattleIndex != message.BattleIndex)
				{
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new UnloadMission(true));
					GameNetwork.EndModuleEventAsServer();
				}
				else
				{
					GameNetwork.ClientFinishedLoading(networkPeer);
				}
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000E908 File Offset: 0x0000CB08
		private bool HandleSyncRelevantGameOptionsToServer(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			SyncRelevantGameOptionsToServer syncRelevantGameOptionsToServer = (SyncRelevantGameOptionsToServer)baseMessage;
			networkPeer.SetRelevantGameOptions(syncRelevantGameOptionsToServer.SendMeBloodEvents, syncRelevantGameOptionsToServer.SendMeSoundEvents);
			return true;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000E930 File Offset: 0x0000CB30
		private bool HandleIntermissionClientVote(NetworkCommunicator networkPeer, GameNetworkMessage baseMessage)
		{
			IntermissionVote intermissionVote = (IntermissionVote)baseMessage;
			int voteCount = intermissionVote.VoteCount;
			if (voteCount == -1 || voteCount == 1)
			{
				if ((MultiplayerIntermissionVotingManager.Instance.CurrentVoteState == MultiplayerIntermissionState.CountingForMapVote && MultiplayerIntermissionVotingManager.Instance.IsMapItem(intermissionVote.ItemID)) || (MultiplayerIntermissionVotingManager.Instance.CurrentVoteState == MultiplayerIntermissionState.CountingForCultureVote && MultiplayerIntermissionVotingManager.Instance.IsCultureItem(intermissionVote.ItemID)))
				{
					MultiplayerIntermissionVotingManager.Instance.AddVote(networkPeer.VirtualPlayer.Id, intermissionVote.ItemID, intermissionVote.VoteCount);
				}
				return true;
			}
			return false;
		}

		// Token: 0x040000F5 RID: 245
		public Action OnIntermissionStateUpdated;

		// Token: 0x040000F6 RID: 246
		private BaseNetworkComponentData _baseNetworkComponentData;

		// Token: 0x020000C4 RID: 196
		// (Invoke) Token: 0x060004D7 RID: 1239
		public delegate void WelcomeMessageReceivedDelegate(string messageText);
	}
}
