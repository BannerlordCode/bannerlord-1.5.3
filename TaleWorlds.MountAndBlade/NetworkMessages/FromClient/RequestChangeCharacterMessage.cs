using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200000D RID: 13
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestChangeCharacterMessage : GameNetworkMessage
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002833 File Offset: 0x00000A33
		// (set) Token: 0x0600004D RID: 77 RVA: 0x0000283B File Offset: 0x00000A3B
		public NetworkCommunicator NetworkPeer { get; private set; }

		// Token: 0x0600004E RID: 78 RVA: 0x00002844 File Offset: 0x00000A44
		public RequestChangeCharacterMessage(NetworkCommunicator networkPeer)
		{
			this.NetworkPeer = networkPeer;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002853 File Offset: 0x00000A53
		public RequestChangeCharacterMessage()
		{
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000285B File Offset: 0x00000A5B
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.NetworkPeer);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002868 File Offset: 0x00000A68
		protected override bool OnRead()
		{
			bool flag = true;
			this.NetworkPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			return flag;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002886 File Offset: 0x00000A86
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000288E File Offset: 0x00000A8E
		protected override string OnGetLogFormat()
		{
			return this.NetworkPeer.UserName + " has requested to change character.";
		}
	}
}
