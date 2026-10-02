using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000097 RID: 151
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemoveAgentVisualsForPeer : GameNetworkMessage
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x0000ACA5 File Offset: 0x00008EA5
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x0000ACAD File Offset: 0x00008EAD
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x060005FC RID: 1532 RVA: 0x0000ACB6 File Offset: 0x00008EB6
		public RemoveAgentVisualsForPeer(NetworkCommunicator peer)
		{
			this.Peer = peer;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0000ACC5 File Offset: 0x00008EC5
		public RemoveAgentVisualsForPeer()
		{
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0000ACD0 File Offset: 0x00008ED0
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			return flag;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0000ACEE File Offset: 0x00008EEE
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0000ACFB File Offset: 0x00008EFB
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0000AD03 File Offset: 0x00008F03
		protected override string OnGetLogFormat()
		{
			return "Removing all AgentVisuals for peer: " + this.Peer.UserName;
		}
	}
}
