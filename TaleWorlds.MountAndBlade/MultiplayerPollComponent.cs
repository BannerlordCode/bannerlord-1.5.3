using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C3 RID: 707
	public class MultiplayerPollComponent : MissionNetwork
	{
		// Token: 0x0600289B RID: 10395 RVA: 0x0009A41E File Offset: 0x0009861E
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._notificationsComponent = base.Mission.GetMissionBehavior<MultiplayerGameNotificationsComponent>();
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x0009A448 File Offset: 0x00098648
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			MultiplayerPollComponent.MultiplayerPoll ongoingPoll = this._ongoingPoll;
			if (ongoingPoll == null)
			{
				return;
			}
			ongoingPoll.Tick();
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x0009A464 File Offset: 0x00098664
		public void Vote(bool accepted)
		{
			if (GameNetwork.IsServer)
			{
				if (GameNetwork.MyPeer != null)
				{
					this.ApplyVote(GameNetwork.MyPeer, accepted);
					return;
				}
			}
			else if (this._ongoingPoll != null && this._ongoingPoll.IsOpen)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new PollResponse(accepted));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x0009A4B8 File Offset: 0x000986B8
		private void ApplyVote(NetworkCommunicator peer, bool accepted)
		{
			if (this._ongoingPoll != null && this._ongoingPoll.ApplyVote(peer, accepted))
			{
				List<NetworkCommunicator> pollProgressReceivers = this._ongoingPoll.GetPollProgressReceivers();
				int count = pollProgressReceivers.Count;
				for (int i = 0; i < count; i++)
				{
					GameNetwork.BeginModuleEventAsServer(pollProgressReceivers[i]);
					GameNetwork.WriteMessage(new PollProgress(this._ongoingPoll.AcceptedCount, this._ongoingPoll.RejectedCount));
					GameNetwork.EndModuleEventAsServer();
				}
				this.UpdatePollProgress(this._ongoingPoll.AcceptedCount, this._ongoingPoll.RejectedCount);
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x0009A548 File Offset: 0x00098748
		private void RejectPollOnServer(NetworkCommunicator pollCreatorPeer, MultiplayerPollRejectReason rejectReason)
		{
			if (pollCreatorPeer.IsMine)
			{
				this.RejectPoll(rejectReason);
				return;
			}
			GameNetwork.BeginModuleEventAsServer(pollCreatorPeer);
			GameNetwork.WriteMessage(new PollRequestRejected((int)rejectReason));
			GameNetwork.EndModuleEventAsServer();
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x0009A570 File Offset: 0x00098770
		private void RejectPoll(MultiplayerPollRejectReason rejectReason)
		{
			if (!GameNetwork.IsDedicatedServer)
			{
				this._notificationsComponent.PollRejected(rejectReason);
			}
			Action<MultiplayerPollRejectReason> onPollRejected = this.OnPollRejected;
			if (onPollRejected == null)
			{
				return;
			}
			onPollRejected(rejectReason);
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x0009A596 File Offset: 0x00098796
		private void UpdatePollProgress(int votesAccepted, int votesRejected)
		{
			Action<int, int> onPollUpdated = this.OnPollUpdated;
			if (onPollUpdated == null)
			{
				return;
			}
			onPollUpdated(votesAccepted, votesRejected);
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x0009A5AA File Offset: 0x000987AA
		private void CancelPoll()
		{
			if (this._ongoingPoll != null)
			{
				this._ongoingPoll.Cancel();
				this._ongoingPoll = null;
			}
			Action onPollCancelled = this.OnPollCancelled;
			if (onPollCancelled == null)
			{
				return;
			}
			onPollCancelled();
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x0009A5D8 File Offset: 0x000987D8
		private void OnPollCancelledOnServer(MultiplayerPollComponent.MultiplayerPoll multiplayerPoll)
		{
			List<NetworkCommunicator> pollProgressReceivers = multiplayerPoll.GetPollProgressReceivers();
			int count = pollProgressReceivers.Count;
			for (int i = 0; i < count; i++)
			{
				GameNetwork.BeginModuleEventAsServer(pollProgressReceivers[i]);
				GameNetwork.WriteMessage(new PollCancelled());
				GameNetwork.EndModuleEventAsServer();
			}
			this.CancelPoll();
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x0009A620 File Offset: 0x00098820
		public void RequestKickPlayerPoll(NetworkCommunicator peer, bool banPlayer)
		{
			if (GameNetwork.IsServer)
			{
				if (GameNetwork.MyPeer != null)
				{
					this.OpenKickPlayerPollOnServer(GameNetwork.MyPeer, peer, banPlayer);
					return;
				}
			}
			else
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new KickPlayerPollRequested(peer, banPlayer));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x0009A654 File Offset: 0x00098854
		private void OpenKickPlayerPollOnServer(NetworkCommunicator pollCreatorPeer, NetworkCommunicator targetPeer, bool banPlayer)
		{
			if (this._ongoingPoll == null)
			{
				bool flag = pollCreatorPeer != null && pollCreatorPeer.IsConnectionActive;
				bool flag2 = targetPeer != null && targetPeer.IsConnectionActive;
				if (flag && flag2)
				{
					if (!targetPeer.IsSynchronized)
					{
						this.RejectPollOnServer(pollCreatorPeer, MultiplayerPollRejectReason.KickPollTargetNotSynced);
						return;
					}
					MissionPeer component = pollCreatorPeer.GetComponent<MissionPeer>();
					if (component != null)
					{
						if (component.RequestedKickPollCount >= 2)
						{
							this.RejectPollOnServer(pollCreatorPeer, MultiplayerPollRejectReason.TooManyPollRequests);
							return;
						}
						List<NetworkCommunicator> list = new List<NetworkCommunicator>();
						foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
						{
							if (networkCommunicator != targetPeer && networkCommunicator.IsSynchronized)
							{
								MissionPeer component2 = networkCommunicator.GetComponent<MissionPeer>();
								if (component2 != null && component2.Team == component.Team)
								{
									list.Add(networkCommunicator);
								}
							}
						}
						int count = list.Count;
						if (count + 1 >= 3)
						{
							this.OpenKickPlayerPoll(targetPeer, pollCreatorPeer, false, list);
							for (int i = 0; i < count; i++)
							{
								GameNetwork.BeginModuleEventAsServer(this._ongoingPoll.ParticipantsToVote[i]);
								GameNetwork.WriteMessage(new KickPlayerPollOpened(pollCreatorPeer, targetPeer, banPlayer));
								GameNetwork.EndModuleEventAsServer();
							}
							GameNetwork.BeginModuleEventAsServer(targetPeer);
							GameNetwork.WriteMessage(new KickPlayerPollOpened(pollCreatorPeer, targetPeer, banPlayer));
							GameNetwork.EndModuleEventAsServer();
							component.IncrementRequestedKickPollCount();
							return;
						}
						this.RejectPollOnServer(pollCreatorPeer, MultiplayerPollRejectReason.NotEnoughPlayersToOpenPoll);
						return;
					}
				}
			}
			else
			{
				this.RejectPollOnServer(pollCreatorPeer, MultiplayerPollRejectReason.HasOngoingPoll);
			}
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x0009A7BC File Offset: 0x000989BC
		private void OpenKickPlayerPoll(NetworkCommunicator targetPeer, NetworkCommunicator pollCreatorPeer, bool banPlayer, List<NetworkCommunicator> participantsToVote)
		{
			MissionPeer component = pollCreatorPeer.GetComponent<MissionPeer>();
			MissionPeer component2 = targetPeer.GetComponent<MissionPeer>();
			this._ongoingPoll = new MultiplayerPollComponent.KickPlayerPoll(this._missionLobbyComponent.MissionType, participantsToVote, targetPeer, component.Team);
			if (GameNetwork.IsServer)
			{
				MultiplayerPollComponent.MultiplayerPoll ongoingPoll = this._ongoingPoll;
				ongoingPoll.OnClosedOnServer = (Action<MultiplayerPollComponent.MultiplayerPoll>)Delegate.Combine(ongoingPoll.OnClosedOnServer, new Action<MultiplayerPollComponent.MultiplayerPoll>(this.OnKickPlayerPollClosedOnServer));
				MultiplayerPollComponent.MultiplayerPoll ongoingPoll2 = this._ongoingPoll;
				ongoingPoll2.OnCancelledOnServer = (Action<MultiplayerPollComponent.MultiplayerPoll>)Delegate.Combine(ongoingPoll2.OnCancelledOnServer, new Action<MultiplayerPollComponent.MultiplayerPoll>(this.OnPollCancelledOnServer));
			}
			Action<MissionPeer, MissionPeer, bool> onKickPollOpened = this.OnKickPollOpened;
			if (onKickPollOpened != null)
			{
				onKickPollOpened(component, component2, banPlayer);
			}
			if (GameNetwork.MyPeer == pollCreatorPeer)
			{
				this.Vote(true);
			}
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x0009A870 File Offset: 0x00098A70
		private void OnKickPlayerPollClosedOnServer(MultiplayerPollComponent.MultiplayerPoll multiplayerPoll)
		{
			MultiplayerPollComponent.KickPlayerPoll kickPlayerPoll = multiplayerPoll as MultiplayerPollComponent.KickPlayerPoll;
			bool flag = kickPlayerPoll.GotEnoughAcceptVotesToEnd();
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new KickPlayerPollClosed(kickPlayerPoll.TargetPeer, flag));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			this.CloseKickPlayerPoll(flag, kickPlayerPoll.TargetPeer);
			if (flag)
			{
				DisconnectInfo disconnectInfo = kickPlayerPoll.TargetPeer.PlayerConnectionInfo.GetParameter<DisconnectInfo>("DisconnectInfo") ?? new DisconnectInfo();
				disconnectInfo.Type = DisconnectType.KickedByPoll;
				kickPlayerPoll.TargetPeer.PlayerConnectionInfo.AddParameter("DisconnectInfo", disconnectInfo);
				GameNetwork.AddNetworkPeerToDisconnectAsServer(kickPlayerPoll.TargetPeer);
			}
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x0009A900 File Offset: 0x00098B00
		private void CloseKickPlayerPoll(bool accepted, NetworkCommunicator targetPeer)
		{
			if (this._ongoingPoll != null)
			{
				this._ongoingPoll.Close();
				this._ongoingPoll = null;
			}
			Action onPollClosed = this.OnPollClosed;
			if (onPollClosed != null)
			{
				onPollClosed();
			}
			if (!GameNetwork.IsDedicatedServer && accepted && !targetPeer.IsMine)
			{
				this._notificationsComponent.PlayerKicked(targetPeer);
			}
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x0009A958 File Offset: 0x00098B58
		private void OnBanPlayerPollClosedOnServer(MultiplayerPollComponent.MultiplayerPoll multiplayerPoll)
		{
			MissionPeer component = (multiplayerPoll as MultiplayerPollComponent.BanPlayerPoll).TargetPeer.GetComponent<MissionPeer>();
			if (component != null)
			{
				NetworkCommunicator networkPeer = component.GetNetworkPeer();
				DisconnectInfo disconnectInfo = networkPeer.PlayerConnectionInfo.GetParameter<DisconnectInfo>("DisconnectInfo") ?? new DisconnectInfo();
				disconnectInfo.Type = DisconnectType.BannedByPoll;
				networkPeer.PlayerConnectionInfo.AddParameter("DisconnectInfo", disconnectInfo);
				GameNetwork.AddNetworkPeerToDisconnectAsServer(networkPeer);
				if (GameNetwork.IsServer)
				{
					CustomGameBannedPlayerManager.AddBannedPlayer(component.Peer.Id, Environment.TickCount + 600000);
				}
				if (GameNetwork.IsDedicatedServer)
				{
					throw new NotImplementedException();
				}
				NetworkMain.GameClient.KickPlayer(component.Peer.Id, true);
			}
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x0009AA00 File Offset: 0x00098C00
		private void StartChangeGamePollOnServer(NetworkCommunicator pollCreatorPeer, string gameType, string scene)
		{
			if (this._ongoingPoll == null)
			{
				List<NetworkCommunicator> list = GameNetwork.NetworkPeers.ToList<NetworkCommunicator>();
				this._ongoingPoll = new MultiplayerPollComponent.ChangeGamePoll(this._missionLobbyComponent.MissionType, list, gameType, scene);
				if (GameNetwork.IsServer)
				{
					MultiplayerPollComponent.MultiplayerPoll ongoingPoll = this._ongoingPoll;
					ongoingPoll.OnClosedOnServer = (Action<MultiplayerPollComponent.MultiplayerPoll>)Delegate.Combine(ongoingPoll.OnClosedOnServer, new Action<MultiplayerPollComponent.MultiplayerPoll>(this.OnChangeGamePollClosedOnServer));
				}
				if (!GameNetwork.IsDedicatedServer)
				{
					this.ShowChangeGamePoll(gameType, scene);
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new NetworkMessages.FromServer.ChangeGamePoll(gameType, scene));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				return;
			}
			this.RejectPollOnServer(pollCreatorPeer, MultiplayerPollRejectReason.HasOngoingPoll);
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x0009AA97 File Offset: 0x00098C97
		private void StartChangeGamePoll(string gameType, string map)
		{
			if (GameNetwork.IsServer)
			{
				if (GameNetwork.MyPeer != null)
				{
					this.StartChangeGamePollOnServer(GameNetwork.MyPeer, gameType, map);
					return;
				}
			}
			else
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new NetworkMessages.FromClient.ChangeGamePoll(gameType, map));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x0009AACB File Offset: 0x00098CCB
		private void ShowChangeGamePoll(string gameType, string scene)
		{
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x0009AAD0 File Offset: 0x00098CD0
		private void OnChangeGamePollClosedOnServer(MultiplayerPollComponent.MultiplayerPoll multiplayerPoll)
		{
			MultiplayerPollComponent.ChangeGamePoll changeGamePoll = multiplayerPoll as MultiplayerPollComponent.ChangeGamePoll;
			MultiplayerOptions.OptionType.GameType.SetValue(changeGamePoll.GameType, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			MultiplayerOptions.Instance.OnGameTypeChanged(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			MultiplayerOptions.OptionType.Map.SetValue(changeGamePoll.MapName, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this._missionLobbyComponent.SetStateEndingAsServer();
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x0009AB18 File Offset: 0x00098D18
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<PollRequestRejected>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPollRequestRejected));
				registerer.RegisterBaseHandler<PollProgress>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventUpdatePollProgress));
				registerer.RegisterBaseHandler<PollCancelled>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPollCancelled));
				registerer.RegisterBaseHandler<KickPlayerPollOpened>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventKickPlayerPollOpened));
				registerer.RegisterBaseHandler<KickPlayerPollClosed>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventKickPlayerPollClosed));
				registerer.RegisterBaseHandler<NetworkMessages.FromServer.ChangeGamePoll>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventChangeGamePoll));
				return;
			}
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<PollResponse>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventPollResponse));
				registerer.RegisterBaseHandler<KickPlayerPollRequested>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventKickPlayerPollRequested));
				registerer.RegisterBaseHandler<NetworkMessages.FromClient.ChangeGamePoll>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventChangeGamePoll));
			}
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x0009ABD8 File Offset: 0x00098DD8
		private bool HandleClientEventChangeGamePoll(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			NetworkMessages.FromClient.ChangeGamePoll changeGamePoll = (NetworkMessages.FromClient.ChangeGamePoll)baseMessage;
			this.StartChangeGamePollOnServer(peer, changeGamePoll.GameType, changeGamePoll.Map);
			return true;
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x0009AC00 File Offset: 0x00098E00
		private bool HandleClientEventKickPlayerPollRequested(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			KickPlayerPollRequested kickPlayerPollRequested = (KickPlayerPollRequested)baseMessage;
			this.OpenKickPlayerPollOnServer(peer, kickPlayerPollRequested.PlayerPeer, kickPlayerPollRequested.BanPlayer);
			return true;
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x0009AC28 File Offset: 0x00098E28
		private bool HandleClientEventPollResponse(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			PollResponse pollResponse = (PollResponse)baseMessage;
			this.ApplyVote(peer, pollResponse.Accepted);
			return true;
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x0009AC4C File Offset: 0x00098E4C
		private void HandleServerEventChangeGamePoll(GameNetworkMessage baseMessage)
		{
			NetworkMessages.FromServer.ChangeGamePoll changeGamePoll = (NetworkMessages.FromServer.ChangeGamePoll)baseMessage;
			this.ShowChangeGamePoll(changeGamePoll.GameType, changeGamePoll.Map);
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x0009AC74 File Offset: 0x00098E74
		private void HandleServerEventKickPlayerPollOpened(GameNetworkMessage baseMessage)
		{
			KickPlayerPollOpened kickPlayerPollOpened = (KickPlayerPollOpened)baseMessage;
			this.OpenKickPlayerPoll(kickPlayerPollOpened.PlayerPeer, kickPlayerPollOpened.InitiatorPeer, kickPlayerPollOpened.BanPlayer, null);
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x0009ACA4 File Offset: 0x00098EA4
		private void HandleServerEventUpdatePollProgress(GameNetworkMessage baseMessage)
		{
			PollProgress pollProgress = (PollProgress)baseMessage;
			this.UpdatePollProgress(pollProgress.VotesAccepted, pollProgress.VotesRejected);
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x0009ACCA File Offset: 0x00098ECA
		private void HandleServerEventPollCancelled(GameNetworkMessage baseMessage)
		{
			this.CancelPoll();
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x0009ACD4 File Offset: 0x00098ED4
		private void HandleServerEventKickPlayerPollClosed(GameNetworkMessage baseMessage)
		{
			KickPlayerPollClosed kickPlayerPollClosed = (KickPlayerPollClosed)baseMessage;
			this.CloseKickPlayerPoll(kickPlayerPollClosed.Accepted, kickPlayerPollClosed.PlayerPeer);
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x0009ACFC File Offset: 0x00098EFC
		private void HandleServerEventPollRequestRejected(GameNetworkMessage baseMessage)
		{
			PollRequestRejected pollRequestRejected = (PollRequestRejected)baseMessage;
			this.RejectPoll((MultiplayerPollRejectReason)pollRequestRejected.Reason);
		}

		// Token: 0x04000F88 RID: 3976
		public const int MinimumParticipantCountRequired = 3;

		// Token: 0x04000F89 RID: 3977
		public Action<MissionPeer, MissionPeer, bool> OnKickPollOpened;

		// Token: 0x04000F8A RID: 3978
		public Action<MultiplayerPollRejectReason> OnPollRejected;

		// Token: 0x04000F8B RID: 3979
		public Action<int, int> OnPollUpdated;

		// Token: 0x04000F8C RID: 3980
		public Action OnPollClosed;

		// Token: 0x04000F8D RID: 3981
		public Action OnPollCancelled;

		// Token: 0x04000F8E RID: 3982
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000F8F RID: 3983
		private MultiplayerGameNotificationsComponent _notificationsComponent;

		// Token: 0x04000F90 RID: 3984
		private MultiplayerPollComponent.MultiplayerPoll _ongoingPoll;

		// Token: 0x020005AE RID: 1454
		private abstract class MultiplayerPoll
		{
			// Token: 0x17000A9B RID: 2715
			// (get) Token: 0x06003EB8 RID: 16056 RVA: 0x000F8A99 File Offset: 0x000F6C99
			public MultiplayerPollComponent.MultiplayerPoll.Type PollType { get; }

			// Token: 0x17000A9C RID: 2716
			// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x000F8AA1 File Offset: 0x000F6CA1
			// (set) Token: 0x06003EBA RID: 16058 RVA: 0x000F8AA9 File Offset: 0x000F6CA9
			public bool IsOpen { get; private set; }

			// Token: 0x17000A9D RID: 2717
			// (get) Token: 0x06003EBB RID: 16059 RVA: 0x000F8AB2 File Offset: 0x000F6CB2
			private int OpenTime { get; }

			// Token: 0x17000A9E RID: 2718
			// (get) Token: 0x06003EBC RID: 16060 RVA: 0x000F8ABA File Offset: 0x000F6CBA
			// (set) Token: 0x06003EBD RID: 16061 RVA: 0x000F8AC2 File Offset: 0x000F6CC2
			private int CloseTime { get; set; }

			// Token: 0x17000A9F RID: 2719
			// (get) Token: 0x06003EBE RID: 16062 RVA: 0x000F8ACB File Offset: 0x000F6CCB
			public List<NetworkCommunicator> ParticipantsToVote
			{
				get
				{
					return this._participantsToVote;
				}
			}

			// Token: 0x06003EBF RID: 16063 RVA: 0x000F8AD4 File Offset: 0x000F6CD4
			protected MultiplayerPoll(MultiplayerGameType gameType, MultiplayerPollComponent.MultiplayerPoll.Type pollType, List<NetworkCommunicator> participantsToVote)
			{
				this._gameType = gameType;
				this.PollType = pollType;
				if (participantsToVote != null)
				{
					this._participantsToVote = participantsToVote;
				}
				this.OpenTime = Environment.TickCount;
				this.CloseTime = 0;
				this.AcceptedCount = 0;
				this.RejectedCount = 0;
				this.IsOpen = true;
			}

			// Token: 0x06003EC0 RID: 16064 RVA: 0x000F8B26 File Offset: 0x000F6D26
			public virtual bool IsCancelled()
			{
				return false;
			}

			// Token: 0x06003EC1 RID: 16065 RVA: 0x000F8B29 File Offset: 0x000F6D29
			public virtual List<NetworkCommunicator> GetPollProgressReceivers()
			{
				return GameNetwork.NetworkPeers.ToList<NetworkCommunicator>();
			}

			// Token: 0x06003EC2 RID: 16066 RVA: 0x000F8B38 File Offset: 0x000F6D38
			public void Tick()
			{
				if (GameNetwork.IsServer)
				{
					for (int i = this._participantsToVote.Count - 1; i >= 0; i--)
					{
						if (!this._participantsToVote[i].IsConnectionActive)
						{
							this._participantsToVote.RemoveAt(i);
						}
					}
					if (this.IsCancelled())
					{
						Action<MultiplayerPollComponent.MultiplayerPoll> onCancelledOnServer = this.OnCancelledOnServer;
						if (onCancelledOnServer == null)
						{
							return;
						}
						onCancelledOnServer(this);
						return;
					}
					else if (this.OpenTime < Environment.TickCount - 30000 || this.ResultsFinalized())
					{
						Action<MultiplayerPollComponent.MultiplayerPoll> onClosedOnServer = this.OnClosedOnServer;
						if (onClosedOnServer == null)
						{
							return;
						}
						onClosedOnServer(this);
					}
				}
			}

			// Token: 0x06003EC3 RID: 16067 RVA: 0x000F8BCB File Offset: 0x000F6DCB
			public void Close()
			{
				this.CloseTime = Environment.TickCount;
				this.IsOpen = false;
			}

			// Token: 0x06003EC4 RID: 16068 RVA: 0x000F8BDF File Offset: 0x000F6DDF
			public void Cancel()
			{
				this.Close();
			}

			// Token: 0x06003EC5 RID: 16069 RVA: 0x000F8BE8 File Offset: 0x000F6DE8
			public bool ApplyVote(NetworkCommunicator peer, bool accepted)
			{
				bool flag = false;
				if (this._participantsToVote.Contains(peer))
				{
					if (accepted)
					{
						this.AcceptedCount++;
					}
					else
					{
						this.RejectedCount++;
					}
					this._participantsToVote.Remove(peer);
					flag = true;
				}
				return flag;
			}

			// Token: 0x06003EC6 RID: 16070 RVA: 0x000F8C38 File Offset: 0x000F6E38
			public bool GotEnoughAcceptVotesToEnd()
			{
				bool flag;
				if (this._gameType == MultiplayerGameType.Skirmish || this._gameType == MultiplayerGameType.Captain)
				{
					flag = this.AcceptedByAllParticipants();
				}
				else
				{
					flag = this.AcceptedByMajority();
				}
				return flag;
			}

			// Token: 0x06003EC7 RID: 16071 RVA: 0x000F8C70 File Offset: 0x000F6E70
			private bool GotEnoughRejectVotesToEnd()
			{
				bool flag;
				if (this._gameType == MultiplayerGameType.Skirmish || this._gameType == MultiplayerGameType.Captain)
				{
					flag = this.RejectedByAtLeastOneParticipant();
				}
				else
				{
					flag = this.RejectedByMajority();
				}
				return flag;
			}

			// Token: 0x06003EC8 RID: 16072 RVA: 0x000F8CA7 File Offset: 0x000F6EA7
			private bool AcceptedByAllParticipants()
			{
				return this.AcceptedCount == this.GetPollParticipantCount();
			}

			// Token: 0x06003EC9 RID: 16073 RVA: 0x000F8CB7 File Offset: 0x000F6EB7
			private bool AcceptedByMajority()
			{
				return (float)this.AcceptedCount / (float)this.GetPollParticipantCount() > 0.50001f;
			}

			// Token: 0x06003ECA RID: 16074 RVA: 0x000F8CCF File Offset: 0x000F6ECF
			private bool RejectedByAtLeastOneParticipant()
			{
				return this.RejectedCount > 0;
			}

			// Token: 0x06003ECB RID: 16075 RVA: 0x000F8CDA File Offset: 0x000F6EDA
			private bool RejectedByMajority()
			{
				return (float)this.RejectedCount / (float)this.GetPollParticipantCount() > 0.50001f;
			}

			// Token: 0x06003ECC RID: 16076 RVA: 0x000F8CF2 File Offset: 0x000F6EF2
			private int GetPollParticipantCount()
			{
				return this._participantsToVote.Count + this.AcceptedCount + this.RejectedCount;
			}

			// Token: 0x06003ECD RID: 16077 RVA: 0x000F8D0D File Offset: 0x000F6F0D
			private bool ResultsFinalized()
			{
				return this.GotEnoughAcceptVotesToEnd() || this.GotEnoughRejectVotesToEnd() || this._participantsToVote.Count == 0;
			}

			// Token: 0x04001F42 RID: 8002
			private const int TimeoutInSeconds = 30;

			// Token: 0x04001F43 RID: 8003
			public Action<MultiplayerPollComponent.MultiplayerPoll> OnClosedOnServer;

			// Token: 0x04001F44 RID: 8004
			public Action<MultiplayerPollComponent.MultiplayerPoll> OnCancelledOnServer;

			// Token: 0x04001F45 RID: 8005
			public int AcceptedCount;

			// Token: 0x04001F46 RID: 8006
			public int RejectedCount;

			// Token: 0x04001F47 RID: 8007
			private readonly List<NetworkCommunicator> _participantsToVote;

			// Token: 0x04001F48 RID: 8008
			private readonly MultiplayerGameType _gameType;

			// Token: 0x020006D0 RID: 1744
			public enum Type
			{
				// Token: 0x040023D7 RID: 9175
				KickPlayer,
				// Token: 0x040023D8 RID: 9176
				BanPlayer,
				// Token: 0x040023D9 RID: 9177
				ChangeGame
			}
		}

		// Token: 0x020005AF RID: 1455
		private class KickPlayerPoll : MultiplayerPollComponent.MultiplayerPoll
		{
			// Token: 0x17000AA0 RID: 2720
			// (get) Token: 0x06003ECE RID: 16078 RVA: 0x000F8D2F File Offset: 0x000F6F2F
			public NetworkCommunicator TargetPeer { get; }

			// Token: 0x06003ECF RID: 16079 RVA: 0x000F8D37 File Offset: 0x000F6F37
			public KickPlayerPoll(MultiplayerGameType gameType, List<NetworkCommunicator> participantsToVote, NetworkCommunicator targetPeer, Team team)
				: base(gameType, MultiplayerPollComponent.MultiplayerPoll.Type.KickPlayer, participantsToVote)
			{
				this.TargetPeer = targetPeer;
				this._team = team;
			}

			// Token: 0x06003ED0 RID: 16080 RVA: 0x000F8D51 File Offset: 0x000F6F51
			public override bool IsCancelled()
			{
				return !this.TargetPeer.IsConnectionActive || this.TargetPeer.QuitFromMission;
			}

			// Token: 0x06003ED1 RID: 16081 RVA: 0x000F8D70 File Offset: 0x000F6F70
			public override List<NetworkCommunicator> GetPollProgressReceivers()
			{
				List<NetworkCommunicator> list = new List<NetworkCommunicator>();
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && component.Team == this._team)
					{
						list.Add(networkCommunicator);
					}
				}
				return list;
			}

			// Token: 0x04001F4D RID: 8013
			public const int RequestLimitPerPeer = 2;

			// Token: 0x04001F4E RID: 8014
			private readonly Team _team;
		}

		// Token: 0x020005B0 RID: 1456
		private class BanPlayerPoll : MultiplayerPollComponent.MultiplayerPoll
		{
			// Token: 0x17000AA1 RID: 2721
			// (get) Token: 0x06003ED2 RID: 16082 RVA: 0x000F8DE4 File Offset: 0x000F6FE4
			public NetworkCommunicator TargetPeer { get; }

			// Token: 0x06003ED3 RID: 16083 RVA: 0x000F8DEC File Offset: 0x000F6FEC
			public BanPlayerPoll(MultiplayerGameType gameType, List<NetworkCommunicator> participantsToVote, NetworkCommunicator targetPeer)
				: base(gameType, MultiplayerPollComponent.MultiplayerPoll.Type.BanPlayer, participantsToVote)
			{
				this.TargetPeer = targetPeer;
			}
		}

		// Token: 0x020005B1 RID: 1457
		private class ChangeGamePoll : MultiplayerPollComponent.MultiplayerPoll
		{
			// Token: 0x17000AA2 RID: 2722
			// (get) Token: 0x06003ED4 RID: 16084 RVA: 0x000F8DFE File Offset: 0x000F6FFE
			public string GameType { get; }

			// Token: 0x17000AA3 RID: 2723
			// (get) Token: 0x06003ED5 RID: 16085 RVA: 0x000F8E06 File Offset: 0x000F7006
			public string MapName { get; }

			// Token: 0x06003ED6 RID: 16086 RVA: 0x000F8E0E File Offset: 0x000F700E
			public ChangeGamePoll(MultiplayerGameType currentGameType, List<NetworkCommunicator> participantsToVote, string gameType, string scene)
				: base(currentGameType, MultiplayerPollComponent.MultiplayerPoll.Type.ChangeGame, participantsToVote)
			{
				this.GameType = gameType;
				this.MapName = scene;
			}
		}
	}
}
