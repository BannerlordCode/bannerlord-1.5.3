using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A3 RID: 163
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentTargetPosition : GameNetworkMessage
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0000B7E3 File Offset: 0x000099E3
		// (set) Token: 0x06000681 RID: 1665 RVA: 0x0000B7EB File Offset: 0x000099EB
		public int AgentIndex { get; private set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0000B7F4 File Offset: 0x000099F4
		// (set) Token: 0x06000683 RID: 1667 RVA: 0x0000B7FC File Offset: 0x000099FC
		public Vec2 Position { get; private set; }

		// Token: 0x06000684 RID: 1668 RVA: 0x0000B805 File Offset: 0x00009A05
		public SetAgentTargetPosition(int agentIndex, ref Vec2 position)
		{
			this.AgentIndex = agentIndex;
			this.Position = position;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0000B820 File Offset: 0x00009A20
		public SetAgentTargetPosition()
		{
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0000B828 File Offset: 0x00009A28
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Position = GameNetworkMessage.ReadVec2FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0000B857 File Offset: 0x00009A57
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteVec2ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0000B874 File Offset: 0x00009A74
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0000B87C File Offset: 0x00009A7C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Target Position: ", this.Position, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
