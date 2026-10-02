using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CC RID: 204
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class StopUsingObject : GameNetworkMessage
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0000E35A File Offset: 0x0000C55A
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x0000E362 File Offset: 0x0000C562
		public int AgentIndex { get; private set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x0000E36B File Offset: 0x0000C56B
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x0000E373 File Offset: 0x0000C573
		public bool IsSuccessful { get; private set; }

		// Token: 0x0600085D RID: 2141 RVA: 0x0000E37C File Offset: 0x0000C57C
		public StopUsingObject(int agentIndex, bool isSuccessful)
		{
			this.AgentIndex = agentIndex;
			this.IsSuccessful = isSuccessful;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0000E392 File Offset: 0x0000C592
		public StopUsingObject()
		{
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0000E39C File Offset: 0x0000C59C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.IsSuccessful = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0000E3C6 File Offset: 0x0000C5C6
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteBoolToPacket(this.IsSuccessful);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0000E3DE File Offset: 0x0000C5DE
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed | MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0000E3E6 File Offset: 0x0000C5E6
		protected override string OnGetLogFormat()
		{
			return "Stop using Object on Agent with agent-index: " + this.AgentIndex;
		}
	}
}
