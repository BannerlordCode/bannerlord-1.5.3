using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008D RID: 141
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ExistingObjectsBegin : GameNetworkMessage
	{
		// Token: 0x06000586 RID: 1414 RVA: 0x0000A1E4 File Offset: 0x000083E4
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0000A1E7 File Offset: 0x000083E7
		protected override void OnWrite()
		{
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0000A1E9 File Offset: 0x000083E9
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0000A1ED File Offset: 0x000083ED
		protected override string OnGetLogFormat()
		{
			return "Started receiving existing objects";
		}
	}
}
