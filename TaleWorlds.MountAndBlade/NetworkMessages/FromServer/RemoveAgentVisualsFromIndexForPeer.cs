using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000098 RID: 152
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemoveAgentVisualsFromIndexForPeer : GameNetworkMessage
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x0000AD1A File Offset: 0x00008F1A
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x0000AD22 File Offset: 0x00008F22
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x0000AD2B File Offset: 0x00008F2B
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x0000AD33 File Offset: 0x00008F33
		public int VisualsIndex { get; private set; }

		// Token: 0x06000606 RID: 1542 RVA: 0x0000AD3C File Offset: 0x00008F3C
		public RemoveAgentVisualsFromIndexForPeer(NetworkCommunicator peer, int index)
		{
			this.Peer = peer;
			this.VisualsIndex = index;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0000AD52 File Offset: 0x00008F52
		public RemoveAgentVisualsFromIndexForPeer()
		{
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0000AD5C File Offset: 0x00008F5C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.VisualsIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentOffsetCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0000AD8C File Offset: 0x00008F8C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket(this.VisualsIndex, CompressionMission.AgentOffsetCompressionInfo);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0000ADA9 File Offset: 0x00008FA9
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0000ADB1 File Offset: 0x00008FB1
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Removing AgentVisuals with Index: ",
				this.VisualsIndex,
				", for peer: ",
				this.Peer.UserName
			});
		}
	}
}
