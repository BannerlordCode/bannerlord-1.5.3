using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000031 RID: 49
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestToSpawnAsBot : GameNetworkMessage
	{
		// Token: 0x06000185 RID: 389 RVA: 0x00003EE3 File Offset: 0x000020E3
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00003EE6 File Offset: 0x000020E6
		protected override void OnWrite()
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00003EE8 File Offset: 0x000020E8
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00003EF0 File Offset: 0x000020F0
		protected override string OnGetLogFormat()
		{
			return "Request to spawn as a bot";
		}
	}
}
