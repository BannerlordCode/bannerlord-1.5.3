using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A4 RID: 164
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentTargetPositionAndDirection : GameNetworkMessage
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0000B8B5 File Offset: 0x00009AB5
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x0000B8BD File Offset: 0x00009ABD
		public int AgentIndex { get; private set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0000B8C6 File Offset: 0x00009AC6
		// (set) Token: 0x0600068D RID: 1677 RVA: 0x0000B8CE File Offset: 0x00009ACE
		public Vec2 Position { get; private set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x0000B8D7 File Offset: 0x00009AD7
		// (set) Token: 0x0600068F RID: 1679 RVA: 0x0000B8DF File Offset: 0x00009ADF
		public Vec3 Direction { get; private set; }

		// Token: 0x06000690 RID: 1680 RVA: 0x0000B8E8 File Offset: 0x00009AE8
		public SetAgentTargetPositionAndDirection(int agentIndex, ref Vec2 position, ref Vec3 direction)
		{
			this.AgentIndex = agentIndex;
			this.Position = position;
			this.Direction = direction;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0000B90F File Offset: 0x00009B0F
		public SetAgentTargetPositionAndDirection()
		{
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0000B918 File Offset: 0x00009B18
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Position = GameNetworkMessage.ReadVec2FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.Direction = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0000B959 File Offset: 0x00009B59
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteVec2ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Direction, CompressionBasic.UnitVectorCompressionInfo);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0000B986 File Offset: 0x00009B86
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0000B990 File Offset: 0x00009B90
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set TargetPositionAndDirection: ", this.Position, " ", this.Direction, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
