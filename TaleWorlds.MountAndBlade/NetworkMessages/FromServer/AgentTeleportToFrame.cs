using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000079 RID: 121
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AgentTeleportToFrame : GameNetworkMessage
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0000802B File Offset: 0x0000622B
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x00008033 File Offset: 0x00006233
		public int AgentIndex { get; private set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0000803C File Offset: 0x0000623C
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x00008044 File Offset: 0x00006244
		public Vec3 Position { get; private set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0000804D File Offset: 0x0000624D
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x00008055 File Offset: 0x00006255
		public Vec2 Direction { get; private set; }

		// Token: 0x06000456 RID: 1110 RVA: 0x0000805E File Offset: 0x0000625E
		public AgentTeleportToFrame(int agentIndex, Vec3 position, Vec2 direction)
		{
			this.AgentIndex = agentIndex;
			this.Position = position;
			this.Direction = direction.Normalized();
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00008081 File Offset: 0x00006281
		public AgentTeleportToFrame()
		{
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000808C File Offset: 0x0000628C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.Direction = GameNetworkMessage.ReadVec2FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000080CD File Offset: 0x000062CD
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec2ToPacket(this.Direction, CompressionBasic.UnitVectorCompressionInfo);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000080FA File Offset: 0x000062FA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00008104 File Offset: 0x00006304
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Teleporting agent with agent-index: ", this.AgentIndex, " to frame with position: ", this.Position, " and direction: ", this.Direction });
		}
	}
}
