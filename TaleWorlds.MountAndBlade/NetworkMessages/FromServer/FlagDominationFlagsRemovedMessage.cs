using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000043 RID: 67
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class FlagDominationFlagsRemovedMessage : GameNetworkMessage
	{
		// Token: 0x06000227 RID: 551 RVA: 0x00004989 File Offset: 0x00002B89
		protected override void OnWrite()
		{
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000498B File Offset: 0x00002B8B
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000498E File Offset: 0x00002B8E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00004996 File Offset: 0x00002B96
		protected override string OnGetLogFormat()
		{
			return "Flags got removed.";
		}
	}
}
