using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000081 RID: 129
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ClearAgentTargetFrame : GameNetworkMessage
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00008901 File Offset: 0x00006B01
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x00008909 File Offset: 0x00006B09
		public int AgentIndex { get; private set; }

		// Token: 0x060004AC RID: 1196 RVA: 0x00008912 File Offset: 0x00006B12
		public ClearAgentTargetFrame(int agentIndex)
		{
			this.AgentIndex = agentIndex;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00008921 File Offset: 0x00006B21
		public ClearAgentTargetFrame()
		{
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000892C File Offset: 0x00006B2C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00008949 File Offset: 0x00006B49
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00008956 File Offset: 0x00006B56
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000895E File Offset: 0x00006B5E
		protected override string OnGetLogFormat()
		{
			return "Clear target frame on agent with agent-index: " + this.AgentIndex;
		}
	}
}
