using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000090 RID: 144
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class FormationWipedMessage : GameNetworkMessage
	{
		// Token: 0x0600059A RID: 1434 RVA: 0x0000A2B2 File Offset: 0x000084B2
		protected override void OnWrite()
		{
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0000A2B4 File Offset: 0x000084B4
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0000A2B7 File Offset: 0x000084B7
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0000A2BF File Offset: 0x000084BF
		protected override string OnGetLogFormat()
		{
			return "FormationWipedMessage";
		}
	}
}
