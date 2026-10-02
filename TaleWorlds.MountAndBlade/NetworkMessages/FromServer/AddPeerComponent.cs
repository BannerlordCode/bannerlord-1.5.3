using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200003E RID: 62
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AddPeerComponent : GameNetworkMessage
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000045AE File Offset: 0x000027AE
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x000045B6 File Offset: 0x000027B6
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x000045BF File Offset: 0x000027BF
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x000045C7 File Offset: 0x000027C7
		public uint ComponentId { get; private set; }

		// Token: 0x060001F6 RID: 502 RVA: 0x000045D0 File Offset: 0x000027D0
		public AddPeerComponent(NetworkCommunicator peer, uint componentId)
		{
			this.Peer = peer;
			this.ComponentId = componentId;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000045E6 File Offset: 0x000027E6
		public AddPeerComponent()
		{
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000045EE File Offset: 0x000027EE
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteUintToPacket(this.ComponentId, CompressionBasic.PeerComponentCompressionInfo);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000460C File Offset: 0x0000280C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.ComponentId = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.PeerComponentCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000463C File Offset: 0x0000283C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00004640 File Offset: 0x00002840
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Add component with ID: ",
				this.ComponentId,
				" to peer:",
				this.Peer.UserName,
				" with peer-index:",
				this.Peer.Index
			});
		}
	}
}
