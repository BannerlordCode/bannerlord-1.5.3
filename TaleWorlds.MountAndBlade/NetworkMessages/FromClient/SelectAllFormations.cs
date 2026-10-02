using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000033 RID: 51
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SelectAllFormations : GameNetworkMessage
	{
		// Token: 0x06000194 RID: 404 RVA: 0x00003FAB File Offset: 0x000021AB
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00003FAE File Offset: 0x000021AE
		protected override void OnWrite()
		{
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00003FB0 File Offset: 0x000021B0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00003FB5 File Offset: 0x000021B5
		protected override string OnGetLogFormat()
		{
			return "Select all formations";
		}
	}
}
