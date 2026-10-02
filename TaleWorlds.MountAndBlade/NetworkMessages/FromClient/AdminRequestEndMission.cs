using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000007 RID: 7
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class AdminRequestEndMission : GameNetworkMessage
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002330 File Offset: 0x00000530
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002333 File Offset: 0x00000533
		protected override void OnWrite()
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002335 File Offset: 0x00000535
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000233D File Offset: 0x0000053D
		protected override string OnGetLogFormat()
		{
			return "AdminRequestEndMission called";
		}
	}
}
