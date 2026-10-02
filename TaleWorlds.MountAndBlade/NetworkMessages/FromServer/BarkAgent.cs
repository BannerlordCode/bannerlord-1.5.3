using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007E RID: 126
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class BarkAgent : GameNetworkMessage
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x0000871D File Offset: 0x0000691D
		// (set) Token: 0x0600048F RID: 1167 RVA: 0x00008725 File Offset: 0x00006925
		public int AgentIndex { get; private set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0000872E File Offset: 0x0000692E
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x00008736 File Offset: 0x00006936
		public int IndexOfBark { get; private set; }

		// Token: 0x06000492 RID: 1170 RVA: 0x0000873F File Offset: 0x0000693F
		public BarkAgent(int agent, int indexOfBark)
		{
			this.AgentIndex = agent;
			this.IndexOfBark = indexOfBark;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00008755 File Offset: 0x00006955
		public BarkAgent()
		{
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00008760 File Offset: 0x00006960
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.IndexOfBark = GameNetworkMessage.ReadIntFromPacket(CompressionMission.BarkIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000878F File Offset: 0x0000698F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.IndexOfBark, CompressionMission.BarkIndexCompressionInfo);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x000087AC File Offset: 0x000069AC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x000087B0 File Offset: 0x000069B0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "FromServer.BarkAgent agent-index: ", this.AgentIndex, ", IndexOfBark", this.IndexOfBark });
		}
	}
}
