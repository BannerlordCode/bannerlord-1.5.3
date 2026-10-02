using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A0 RID: 160
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentOwningMissionPeer : GameNetworkMessage
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0000B511 File Offset: 0x00009711
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x0000B519 File Offset: 0x00009719
		public int AgentIndex { get; private set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0000B522 File Offset: 0x00009722
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x0000B52A File Offset: 0x0000972A
		public VirtualPlayer Peer { get; private set; }

		// Token: 0x06000664 RID: 1636 RVA: 0x0000B533 File Offset: 0x00009733
		public SetAgentOwningMissionPeer(int agentIndex, VirtualPlayer peer)
		{
			this.AgentIndex = agentIndex;
			this.Peer = peer;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0000B549 File Offset: 0x00009749
		public SetAgentOwningMissionPeer()
		{
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0000B554 File Offset: 0x00009754
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Peer = GameNetworkMessage.ReadVirtualPlayerReferenceToPacket(ref flag, true);
			return flag;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0000B57F File Offset: 0x0000977F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteVirtualPlayerReferenceToPacket(this.Peer);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0000B597 File Offset: 0x00009797
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers | MultiplayerMessageFilter.Agents;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0000B59F File Offset: 0x0000979F
		protected override string OnGetLogFormat()
		{
			string text = "SetAgentOwningMissionPeer for agent-index: {0} to {1}";
			object obj = this.AgentIndex;
			VirtualPlayer peer = this.Peer;
			return string.Format(text, obj, ((peer != null) ? peer.UserName : null) ?? "null");
		}
	}
}
