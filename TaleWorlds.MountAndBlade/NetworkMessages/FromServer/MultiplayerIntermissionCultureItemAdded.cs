using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005A RID: 90
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionCultureItemAdded : GameNetworkMessage
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00006265 File Offset: 0x00004465
		// (set) Token: 0x06000328 RID: 808 RVA: 0x0000626D File Offset: 0x0000446D
		public string CultureId { get; private set; }

		// Token: 0x06000329 RID: 809 RVA: 0x00006276 File Offset: 0x00004476
		public MultiplayerIntermissionCultureItemAdded()
		{
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000627E File Offset: 0x0000447E
		public MultiplayerIntermissionCultureItemAdded(string cultureId)
		{
			this.CultureId = cultureId;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00006290 File Offset: 0x00004490
		protected override bool OnRead()
		{
			bool flag = true;
			this.CultureId = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x000062AD File Offset: 0x000044AD
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.CultureId);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000062BA File Offset: 0x000044BA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000062C2 File Offset: 0x000044C2
		protected override string OnGetLogFormat()
		{
			return "Adding culture for voting with id: " + this.CultureId + ".";
		}
	}
}
