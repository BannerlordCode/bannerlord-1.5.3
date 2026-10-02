using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004C RID: 76
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelSessionStarted : GameNetworkMessage
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000512E File Offset: 0x0000332E
		// (set) Token: 0x06000282 RID: 642 RVA: 0x00005136 File Offset: 0x00003336
		public NetworkCommunicator RequesterPeer { get; private set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000513F File Offset: 0x0000333F
		// (set) Token: 0x06000284 RID: 644 RVA: 0x00005147 File Offset: 0x00003347
		public NetworkCommunicator RequestedPeer { get; private set; }

		// Token: 0x06000285 RID: 645 RVA: 0x00005150 File Offset: 0x00003350
		public DuelSessionStarted(NetworkCommunicator requesterPeer, NetworkCommunicator requestedPeer)
		{
			this.RequesterPeer = requesterPeer;
			this.RequestedPeer = requestedPeer;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00005166 File Offset: 0x00003366
		public DuelSessionStarted()
		{
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00005170 File Offset: 0x00003370
		protected override bool OnRead()
		{
			bool flag = true;
			this.RequesterPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.RequestedPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			return flag;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000519C File Offset: 0x0000339C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.RequesterPeer);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.RequestedPeer);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x000051B4 File Offset: 0x000033B4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x000051BC File Offset: 0x000033BC
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Duel session started between agent with name: ",
				this.RequestedPeer.UserName,
				" and index: ",
				this.RequestedPeer.Index,
				" and agent with name: ",
				this.RequesterPeer.UserName,
				" and index: ",
				this.RequesterPeer.Index
			});
		}
	}
}
