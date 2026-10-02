using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000082 RID: 130
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ClearMission : GameNetworkMessage
	{
		// Token: 0x060004B3 RID: 1203 RVA: 0x0000897D File Offset: 0x00006B7D
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00008980 File Offset: 0x00006B80
		protected override void OnWrite()
		{
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00008982 File Offset: 0x00006B82
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000898A File Offset: 0x00006B8A
		protected override string OnGetLogFormat()
		{
			return "Clear Mission";
		}
	}
}
