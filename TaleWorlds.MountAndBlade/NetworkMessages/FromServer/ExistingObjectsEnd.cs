using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008E RID: 142
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ExistingObjectsEnd : GameNetworkMessage
	{
		// Token: 0x0600058B RID: 1419 RVA: 0x0000A1FC File Offset: 0x000083FC
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0000A1FF File Offset: 0x000083FF
		protected override void OnWrite()
		{
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0000A201 File Offset: 0x00008401
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0000A205 File Offset: 0x00008405
		protected override string OnGetLogFormat()
		{
			return "Finished receiving existing objects";
		}
	}
}
