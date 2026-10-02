using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000014 RID: 20
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class AdminRequestEndWarmup : GameNetworkMessage
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00002C86 File Offset: 0x00000E86
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002C89 File Offset: 0x00000E89
		protected override void OnWrite()
		{
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002C8B File Offset: 0x00000E8B
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002C93 File Offset: 0x00000E93
		protected override string OnGetLogFormat()
		{
			return "Requested to end warmup";
		}
	}
}
