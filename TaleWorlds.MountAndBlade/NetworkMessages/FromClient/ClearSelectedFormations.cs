using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002C RID: 44
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ClearSelectedFormations : GameNetworkMessage
	{
		// Token: 0x06000161 RID: 353 RVA: 0x00003CFB File Offset: 0x00001EFB
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00003CFE File Offset: 0x00001EFE
		protected override void OnWrite()
		{
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00003D00 File Offset: 0x00001F00
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00003D05 File Offset: 0x00001F05
		protected override string OnGetLogFormat()
		{
			return "Clear Selected Formations";
		}
	}
}
