using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000066 RID: 102
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PollCancelled : GameNetworkMessage
	{
		// Token: 0x0600038D RID: 909 RVA: 0x00006EA0 File Offset: 0x000050A0
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00006EA3 File Offset: 0x000050A3
		protected override void OnWrite()
		{
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00006EA5 File Offset: 0x000050A5
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00006EAD File Offset: 0x000050AD
		protected override string OnGetLogFormat()
		{
			return "Poll cancelled.";
		}
	}
}
