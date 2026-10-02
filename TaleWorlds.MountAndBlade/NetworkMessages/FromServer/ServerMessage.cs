using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000042 RID: 66
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ServerMessage : GameNetworkMessage
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600021A RID: 538 RVA: 0x000048B8 File Offset: 0x00002AB8
		// (set) Token: 0x0600021B RID: 539 RVA: 0x000048C0 File Offset: 0x00002AC0
		public string Message { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600021C RID: 540 RVA: 0x000048C9 File Offset: 0x00002AC9
		// (set) Token: 0x0600021D RID: 541 RVA: 0x000048D1 File Offset: 0x00002AD1
		public bool IsMessageTextId { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600021E RID: 542 RVA: 0x000048DA File Offset: 0x00002ADA
		// (set) Token: 0x0600021F RID: 543 RVA: 0x000048E2 File Offset: 0x00002AE2
		public bool IsAdminAnnouncement { get; private set; }

		// Token: 0x06000220 RID: 544 RVA: 0x000048EB File Offset: 0x00002AEB
		public ServerMessage(string message, bool isMessageTextId = false, bool isAdminAnnouncement = false)
		{
			this.Message = message;
			this.IsMessageTextId = isMessageTextId;
			this.IsAdminAnnouncement = isAdminAnnouncement;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00004908 File Offset: 0x00002B08
		public ServerMessage()
		{
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00004910 File Offset: 0x00002B10
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.Message);
			GameNetworkMessage.WriteBoolToPacket(this.IsMessageTextId);
			GameNetworkMessage.WriteBoolToPacket(this.IsAdminAnnouncement);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00004934 File Offset: 0x00002B34
		protected override bool OnRead()
		{
			bool flag = true;
			this.Message = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.IsMessageTextId = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsAdminAnnouncement = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000496B File Offset: 0x00002B6B
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000496F File Offset: 0x00002B6F
		protected override string OnGetLogFormat()
		{
			return "Message from server: " + this.Message;
		}
	}
}
