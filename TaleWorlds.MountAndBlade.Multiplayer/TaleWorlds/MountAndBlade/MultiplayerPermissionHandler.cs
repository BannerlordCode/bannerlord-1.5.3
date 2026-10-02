using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000026 RID: 38
	public class MultiplayerPermissionHandler : UdpNetworkComponent
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060001C7 RID: 455 RVA: 0x000080E0 File Offset: 0x000062E0
		// (remove) Token: 0x060001C8 RID: 456 RVA: 0x00008118 File Offset: 0x00006318
		public event Action<PlayerId, bool> OnPlayerPlatformMuteChanged;

		// Token: 0x060001C9 RID: 457 RVA: 0x0000814D File Offset: 0x0000634D
		public MultiplayerPermissionHandler()
		{
			this._chatBox = Game.Current.GetGameHandler<ChatBox>();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008170 File Offset: 0x00006370
		public override void OnUdpNetworkHandlerClose()
		{
			base.OnUdpNetworkHandlerClose();
			this.HandleClientDisconnect();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008180 File Offset: 0x00006380
		private void HandleClientDisconnect()
		{
			foreach (ValueTuple<PlayerId, Permission> valueTuple in this._registeredEvents.Keys)
			{
				PlatformServices.Instance.UnregisterPermissionChangeEvent(valueTuple.Item1, valueTuple.Item2, new PermissionChanged(this.VoicePermissionChanged));
				bool flag;
				this._registeredEvents.TryRemove(new ValueTuple<PlayerId, Permission>(valueTuple.Item1, valueTuple.Item2), out flag);
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008210 File Offset: 0x00006410
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<InitializeLobbyPeer>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventInitializeLobbyPeer));
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000822C File Offset: 0x0000642C
		private void HandleServerEventInitializeLobbyPeer(GameNetworkMessage baseMessage)
		{
			InitializeLobbyPeer initializeLobbyPeer = (InitializeLobbyPeer)baseMessage;
			if (GameNetwork.MyPeer != null && initializeLobbyPeer.Peer != GameNetwork.MyPeer)
			{
				if (PlatformServices.Instance.RegisterPermissionChangeEvent(initializeLobbyPeer.ProvidedId, Permission.CommunicateUsingText, new PermissionChanged(this.TextPermissionChanged)))
				{
					this._registeredEvents[new ValueTuple<PlayerId, Permission>(initializeLobbyPeer.ProvidedId, Permission.CommunicateUsingText)] = true;
				}
				if (PlatformServices.Instance.RegisterPermissionChangeEvent(initializeLobbyPeer.ProvidedId, Permission.CommunicateUsingVoice, new PermissionChanged(this.VoicePermissionChanged)))
				{
					this._registeredEvents[new ValueTuple<PlayerId, Permission>(initializeLobbyPeer.ProvidedId, Permission.CommunicateUsingVoice)] = true;
				}
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000082C4 File Offset: 0x000064C4
		public override void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
			base.OnPlayerDisconnectedFromServer(networkPeer);
			if (PlatformServices.Instance.UnregisterPermissionChangeEvent(networkPeer.VirtualPlayer.Id, Permission.CommunicateUsingText, new PermissionChanged(this.TextPermissionChanged)))
			{
				bool flag;
				this._registeredEvents.TryRemove(new ValueTuple<PlayerId, Permission>(networkPeer.VirtualPlayer.Id, Permission.CommunicateUsingText), out flag);
			}
			if (PlatformServices.Instance.UnregisterPermissionChangeEvent(networkPeer.VirtualPlayer.Id, Permission.CommunicateUsingVoice, new PermissionChanged(this.VoicePermissionChanged)))
			{
				bool flag;
				this._registeredEvents.TryRemove(new ValueTuple<PlayerId, Permission>(networkPeer.VirtualPlayer.Id, Permission.CommunicateUsingVoice), out flag);
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00008360 File Offset: 0x00006560
		private void TextPermissionChanged(PlayerId targetPlayerId, Permission permission, bool hasPermission)
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (!(targetPlayerId != networkCommunicator.VirtualPlayer.Id))
				{
					networkCommunicator.GetComponent<MissionPeer>();
					bool flag = !hasPermission;
					this._chatBox.SetPlayerMutedFromPlatform(targetPlayerId, flag);
					Action<PlayerId, bool> onPlayerPlatformMuteChanged = this.OnPlayerPlatformMuteChanged;
					if (onPlayerPlatformMuteChanged != null)
					{
						onPlayerPlatformMuteChanged(targetPlayerId, flag);
					}
				}
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000083EC File Offset: 0x000065EC
		private void VoicePermissionChanged(PlayerId targetPlayerId, Permission permission, bool hasPermission)
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (!(targetPlayerId != networkCommunicator.VirtualPlayer.Id))
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					bool flag = !hasPermission;
					component.SetMutedFromPlatform(flag);
					Action<PlayerId, bool> onPlayerPlatformMuteChanged = this.OnPlayerPlatformMuteChanged;
					if (onPlayerPlatformMuteChanged != null)
					{
						onPlayerPlatformMuteChanged(targetPlayerId, flag);
					}
				}
			}
		}

		// Token: 0x0400006B RID: 107
		private ChatBox _chatBox;

		// Token: 0x0400006D RID: 109
		[TupleElementNames(new string[] { "PlayerId", "Permission" })]
		private ConcurrentDictionary<ValueTuple<PlayerId, Permission>, bool> _registeredEvents = new ConcurrentDictionary<ValueTuple<PlayerId, Permission>, bool>();
	}
}
