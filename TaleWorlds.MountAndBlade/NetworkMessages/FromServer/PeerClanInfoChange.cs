using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000063 RID: 99
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PeerClanInfoChange : GameNetworkMessage
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00006C1F File Offset: 0x00004E1F
		// (set) Token: 0x0600036F RID: 879 RVA: 0x00006C27 File Offset: 0x00004E27
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00006C30 File Offset: 0x00004E30
		// (set) Token: 0x06000371 RID: 881 RVA: 0x00006C38 File Offset: 0x00004E38
		public string ClanName { get; private set; }

		// Token: 0x06000372 RID: 882 RVA: 0x00006C41 File Offset: 0x00004E41
		public PeerClanInfoChange(NetworkCommunicator peer, string clanName)
		{
			this.Peer = peer;
			this.ClanName = clanName ?? string.Empty;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00006C60 File Offset: 0x00004E60
		public PeerClanInfoChange()
		{
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00006C68 File Offset: 0x00004E68
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.ClanName = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00006C93 File Offset: 0x00004E93
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteStringToPacket(this.ClanName);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00006CAB File Offset: 0x00004EAB
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00006CAF File Offset: 0x00004EAF
		protected override string OnGetLogFormat()
		{
			string text = "Peer clan info change for peer: ";
			NetworkCommunicator peer = this.Peer;
			return text + (((peer != null) ? peer.UserName : null) ?? "NULL") + " clan: " + this.ClanName;
		}
	}
}
