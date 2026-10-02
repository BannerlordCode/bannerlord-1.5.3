using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200001D RID: 29
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestForfeitSpawn : GameNetworkMessage
	{
		// Token: 0x060000DE RID: 222 RVA: 0x0000337B File Offset: 0x0000157B
		protected override void OnWrite()
		{
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000337D File Offset: 0x0000157D
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00003380 File Offset: 0x00001580
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00003388 File Offset: 0x00001588
		protected override string OnGetLogFormat()
		{
			return "Someone has requested to forfeit spawning.";
		}
	}
}
