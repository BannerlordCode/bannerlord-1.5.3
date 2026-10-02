using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000037 RID: 55
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SetFollowedAgent : GameNetworkMessage
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000040C1 File Offset: 0x000022C1
		// (set) Token: 0x060001AE RID: 430 RVA: 0x000040C9 File Offset: 0x000022C9
		public int AgentIndex { get; private set; }

		// Token: 0x060001AF RID: 431 RVA: 0x000040D2 File Offset: 0x000022D2
		public SetFollowedAgent(int agentIndex)
		{
			this.AgentIndex = agentIndex;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000040E1 File Offset: 0x000022E1
		public SetFollowedAgent()
		{
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000040EC File Offset: 0x000022EC
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00004109 File Offset: 0x00002309
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00004116 File Offset: 0x00002316
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000411E File Offset: 0x0000231E
		protected override string OnGetLogFormat()
		{
			return "Peer switched spectating an agent";
		}
	}
}
