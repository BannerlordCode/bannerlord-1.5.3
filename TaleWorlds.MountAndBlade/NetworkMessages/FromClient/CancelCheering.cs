using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002B RID: 43
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class CancelCheering : GameNetworkMessage
	{
		// Token: 0x0600015C RID: 348 RVA: 0x00003CE3 File Offset: 0x00001EE3
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00003CE6 File Offset: 0x00001EE6
		protected override void OnWrite()
		{
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00003CE8 File Offset: 0x00001EE8
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00003CEC File Offset: 0x00001EEC
		protected override string OnGetLogFormat()
		{
			return "FromClient.CancelCheering";
		}
	}
}
