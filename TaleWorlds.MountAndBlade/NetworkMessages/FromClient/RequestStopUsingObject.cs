using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000030 RID: 48
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestStopUsingObject : GameNetworkMessage
	{
		// Token: 0x06000180 RID: 384 RVA: 0x00003EC7 File Offset: 0x000020C7
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00003ECA File Offset: 0x000020CA
		protected override void OnWrite()
		{
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00003ECC File Offset: 0x000020CC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00003ED4 File Offset: 0x000020D4
		protected override string OnGetLogFormat()
		{
			return "Request to stop using UsableMissionObject";
		}
	}
}
