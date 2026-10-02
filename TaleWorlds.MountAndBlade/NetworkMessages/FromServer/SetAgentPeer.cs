using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A1 RID: 161
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentPeer : GameNetworkMessage
	{
		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x0000B5D1 File Offset: 0x000097D1
		// (set) Token: 0x0600066B RID: 1643 RVA: 0x0000B5D9 File Offset: 0x000097D9
		public int AgentIndex { get; private set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x0000B5E2 File Offset: 0x000097E2
		// (set) Token: 0x0600066D RID: 1645 RVA: 0x0000B5EA File Offset: 0x000097EA
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x0600066E RID: 1646 RVA: 0x0000B5F3 File Offset: 0x000097F3
		public SetAgentPeer(int agentIndex, NetworkCommunicator peer)
		{
			this.AgentIndex = agentIndex;
			this.Peer = peer;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0000B609 File Offset: 0x00009809
		public SetAgentPeer()
		{
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0000B614 File Offset: 0x00009814
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, true);
			return flag;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0000B63F File Offset: 0x0000983F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0000B657 File Offset: 0x00009857
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers | MultiplayerMessageFilter.Agents;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0000B660 File Offset: 0x00009860
		protected override string OnGetLogFormat()
		{
			if (this.AgentIndex < 0)
			{
				return "Ignoring the message for invalid agent.";
			}
			return string.Concat(new object[]
			{
				"Set NetworkPeer ",
				(this.Peer != null) ? "" : "(to NULL) ",
				"on Agent with agent-index: ",
				this.AgentIndex
			});
		}
	}
}
