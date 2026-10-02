using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000034 RID: 52
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SelectAllSiegeWeapons : GameNetworkMessage
	{
		// Token: 0x06000199 RID: 409 RVA: 0x00003FC4 File Offset: 0x000021C4
		protected override bool OnRead()
		{
			return true;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00003FC7 File Offset: 0x000021C7
		protected override void OnWrite()
		{
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00003FC9 File Offset: 0x000021C9
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00003FD1 File Offset: 0x000021D1
		protected override string OnGetLogFormat()
		{
			return "Select all siege weapons.";
		}
	}
}
