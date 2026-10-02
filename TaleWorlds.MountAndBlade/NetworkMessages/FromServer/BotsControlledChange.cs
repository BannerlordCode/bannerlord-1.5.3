using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000050 RID: 80
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class BotsControlledChange : GameNetworkMessage
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00005650 File Offset: 0x00003850
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x00005658 File Offset: 0x00003858
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00005661 File Offset: 0x00003861
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x00005669 File Offset: 0x00003869
		public int AliveCount { get; private set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00005672 File Offset: 0x00003872
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000567A File Offset: 0x0000387A
		public int TotalCount { get; private set; }

		// Token: 0x060002B5 RID: 693 RVA: 0x00005683 File Offset: 0x00003883
		public BotsControlledChange(NetworkCommunicator peer, int aliveCount, int totalCount)
		{
			this.Peer = peer;
			this.AliveCount = aliveCount;
			this.TotalCount = totalCount;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x000056A0 File Offset: 0x000038A0
		public BotsControlledChange()
		{
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000056A8 File Offset: 0x000038A8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.AliveCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentOffsetCompressionInfo, ref flag);
			this.TotalCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentOffsetCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000056EA File Offset: 0x000038EA
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket(this.AliveCount, CompressionMission.AgentOffsetCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.TotalCount, CompressionMission.AgentOffsetCompressionInfo);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00005717 File Offset: 0x00003917
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00005720 File Offset: 0x00003920
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Bot Controlled Count Changed. Peer: ",
				this.Peer.UserName,
				" now has ",
				this.AliveCount,
				" alive bots, out of: ",
				this.TotalCount,
				" total bots."
			});
		}
	}
}
