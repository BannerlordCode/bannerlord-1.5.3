using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009E RID: 158
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentHealth : GameNetworkMessage
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x0000B389 File Offset: 0x00009589
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x0000B391 File Offset: 0x00009591
		public int AgentIndex { get; private set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0000B39A File Offset: 0x0000959A
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x0000B3A2 File Offset: 0x000095A2
		public int Health { get; private set; }

		// Token: 0x06000650 RID: 1616 RVA: 0x0000B3AB File Offset: 0x000095AB
		public SetAgentHealth(int agentIndex, int newHealth)
		{
			this.AgentIndex = agentIndex;
			this.Health = newHealth;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0000B3C1 File Offset: 0x000095C1
		public SetAgentHealth()
		{
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0000B3CC File Offset: 0x000095CC
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Health = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentHealthCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0000B3FB File Offset: 0x000095FB
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.Health, CompressionMission.AgentHealthCompressionInfo);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0000B418 File Offset: 0x00009618
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0000B420 File Offset: 0x00009620
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set agent health to: ", this.Health, ", for agent-index: ", this.AgentIndex });
		}
	}
}
