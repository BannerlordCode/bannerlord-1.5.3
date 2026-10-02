using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000022 RID: 34
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithAgent : GameNetworkMessage
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00003553 File Offset: 0x00001753
		// (set) Token: 0x06000102 RID: 258 RVA: 0x0000355B File Offset: 0x0000175B
		public OrderType OrderType { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00003564 File Offset: 0x00001764
		// (set) Token: 0x06000104 RID: 260 RVA: 0x0000356C File Offset: 0x0000176C
		public int AgentIndex { get; private set; }

		// Token: 0x06000105 RID: 261 RVA: 0x00003575 File Offset: 0x00001775
		public ApplyOrderWithAgent(OrderType orderType, int agentIndex)
		{
			this.OrderType = orderType;
			this.AgentIndex = agentIndex;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000358B File Offset: 0x0000178B
		public ApplyOrderWithAgent()
		{
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00003594 File Offset: 0x00001794
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000035C3 File Offset: 0x000017C3
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000035E0 File Offset: 0x000017E0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000035E8 File Offset: 0x000017E8
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to agent with index: ", this.AgentIndex });
		}
	}
}
