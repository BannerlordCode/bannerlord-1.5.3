using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000DD RID: 221
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemovePeerComponent : GameNetworkMessage
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x0000F24C File Offset: 0x0000D44C
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x0000F254 File Offset: 0x0000D454
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x0000F25D File Offset: 0x0000D45D
		// (set) Token: 0x06000908 RID: 2312 RVA: 0x0000F265 File Offset: 0x0000D465
		public uint ComponentId { get; private set; }

		// Token: 0x06000909 RID: 2313 RVA: 0x0000F26E File Offset: 0x0000D46E
		public RemovePeerComponent(NetworkCommunicator peer, uint componentId)
		{
			this.Peer = peer;
			this.ComponentId = componentId;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0000F284 File Offset: 0x0000D484
		public RemovePeerComponent()
		{
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0000F28C File Offset: 0x0000D48C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteUintToPacket(this.ComponentId, CompressionBasic.PeerComponentCompressionInfo);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0000F2AC File Offset: 0x0000D4AC
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.ComponentId = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.PeerComponentCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0000F2DC File Offset: 0x0000D4DC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0000F2E0 File Offset: 0x0000D4E0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Remove component with ID: ",
				this.ComponentId,
				" from peer: ",
				this.Peer.UserName,
				" with peer-index: ",
				this.Peer.Index
			});
		}
	}
}
