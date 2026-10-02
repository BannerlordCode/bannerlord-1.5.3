using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000DB RID: 219
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ServerPerformanceStateReplicationMessage : GameNetworkMessage
	{
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x0000F020 File Offset: 0x0000D220
		// (set) Token: 0x060008F0 RID: 2288 RVA: 0x0000F028 File Offset: 0x0000D228
		internal ServerPerformanceState ServerPerformanceProblemState { get; private set; }

		// Token: 0x060008F1 RID: 2289 RVA: 0x0000F031 File Offset: 0x0000D231
		public ServerPerformanceStateReplicationMessage()
		{
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0000F039 File Offset: 0x0000D239
		internal ServerPerformanceStateReplicationMessage(ServerPerformanceState serverPerformanceProblemState)
		{
			this.ServerPerformanceProblemState = serverPerformanceProblemState;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0000F048 File Offset: 0x0000D248
		protected override bool OnRead()
		{
			bool flag = true;
			this.ServerPerformanceProblemState = (ServerPerformanceState)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ServerPerformanceStateCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0000F06A File Offset: 0x0000D26A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.ServerPerformanceProblemState, CompressionBasic.ServerPerformanceStateCompressionInfo);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0000F07C File Offset: 0x0000D27C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0000F084 File Offset: 0x0000D284
		protected override string OnGetLogFormat()
		{
			return "ServerPerformanceStateReplicationMessage";
		}
	}
}
