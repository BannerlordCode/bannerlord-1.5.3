using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000305 RID: 773
	public class LobbyNetworkComponent : UdpNetworkComponent
	{
		// Token: 0x06002C62 RID: 11362 RVA: 0x000AAD8E File Offset: 0x000A8F8E
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClientOrReplay)
			{
				registerer.RegisterBaseHandler<InitializeLobbyPeer>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventInitializeLobbyPeer));
			}
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x000AADAC File Offset: 0x000A8FAC
		private void HandleServerEventInitializeLobbyPeer(GameNetworkMessage baseMessage)
		{
			InitializeLobbyPeer initializeLobbyPeer = (InitializeLobbyPeer)baseMessage;
			NetworkCommunicator peer = initializeLobbyPeer.Peer;
			VirtualPlayer virtualPlayer = peer.VirtualPlayer;
			virtualPlayer.Id = initializeLobbyPeer.ProvidedId;
			virtualPlayer.IsFemale = initializeLobbyPeer.IsFemale;
			virtualPlayer.BannerCode = initializeLobbyPeer.BannerCode;
			virtualPlayer.BodyProperties = initializeLobbyPeer.BodyProperties;
			virtualPlayer.ChosenBadgeIndex = initializeLobbyPeer.ChosenBadgeIndex;
			peer.ForcedAvatarIndex = initializeLobbyPeer.ForcedAvatarIndex;
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x000AAE14 File Offset: 0x000A9014
		public override void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			PlayerData parameter = networkPeer.PlayerConnectionInfo.GetParameter<PlayerData>("PlayerData");
			Dictionary<int, List<int>> parameter2 = networkPeer.PlayerConnectionInfo.GetParameter<Dictionary<int, List<int>>>("UsedCosmetics");
			VirtualPlayer virtualPlayer = networkPeer.VirtualPlayer;
			virtualPlayer.Id = parameter.PlayerId;
			virtualPlayer.BannerCode = parameter.Sigil;
			virtualPlayer.BodyProperties = parameter.BodyProperties;
			virtualPlayer.IsFemale = parameter.IsFemale;
			virtualPlayer.ChosenBadgeIndex = parameter.ShownBadgeIndex;
			virtualPlayer.UsedCosmetics = parameter2;
			networkPeer.IsMuted = parameter.IsMuted;
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x000AAE98 File Offset: 0x000A9098
		public override void HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			VirtualPlayer virtualPlayer = networkPeer.VirtualPlayer;
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new InitializeLobbyPeer(networkPeer, virtualPlayer, -1));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord | GameNetwork.EventBroadcastFlags.DontSendToPeers, null);
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeersIncludingDisconnectedPeers)
			{
				if (networkCommunicator.IsSynchronized || networkCommunicator == networkPeer)
				{
					bool flag = GameNetwork.VirtualPlayers[networkCommunicator.VirtualPlayer.Index] != networkCommunicator.VirtualPlayer;
					if (networkCommunicator != networkPeer && !flag && networkCommunicator != GameNetwork.MyPeer)
					{
						GameNetwork.BeginModuleEventAsServer(networkCommunicator);
						GameNetwork.WriteMessage(new InitializeLobbyPeer(networkPeer, virtualPlayer, -1));
						GameNetwork.EndModuleEventAsServer();
					}
					if (!networkPeer.IsServerPeer)
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new InitializeLobbyPeer(networkCommunicator, networkCommunicator.VirtualPlayer, -1));
						GameNetwork.EndModuleEventAsServer();
					}
				}
			}
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x000AAF7C File Offset: 0x000A917C
		public override void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x000AAF7E File Offset: 0x000A917E
		public override void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x000AAF80 File Offset: 0x000A9180
		public override void OnUdpNetworkHandlerTick(float dt)
		{
		}

		// Token: 0x0400112D RID: 4397
		public const int MaxForcedAvatarIndex = 100;
	}
}
