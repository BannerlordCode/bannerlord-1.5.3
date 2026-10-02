using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D7 RID: 215
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.DebugFromServer)]
	internal sealed class DebugAgentScaleOnNetworkTest : GameNetworkMessage
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0000EE62 File Offset: 0x0000D062
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0000EE6A File Offset: 0x0000D06A
		internal int AgentToTestIndex { get; private set; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0000EE73 File Offset: 0x0000D073
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0000EE7B File Offset: 0x0000D07B
		internal float ScaleToTest { get; private set; }

		// Token: 0x060008D7 RID: 2263 RVA: 0x0000EE84 File Offset: 0x0000D084
		public DebugAgentScaleOnNetworkTest()
		{
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0000EE8C File Offset: 0x0000D08C
		internal DebugAgentScaleOnNetworkTest(int agentToTestIndex, float scale)
		{
			this.AgentToTestIndex = agentToTestIndex;
			this.ScaleToTest = scale;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0000EEA4 File Offset: 0x0000D0A4
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentToTestIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.ScaleToTest = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.DebugScaleValueCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0000EED3 File Offset: 0x0000D0D3
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentToTestIndex);
			GameNetworkMessage.WriteFloatToPacket(this.ScaleToTest, CompressionMission.DebugScaleValueCompressionInfo);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0000EEF0 File Offset: 0x0000D0F0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0000EEF8 File Offset: 0x0000D0F8
		protected override string OnGetLogFormat()
		{
			return "DebugAgentScaleOnNetworkTest";
		}
	}
}
