using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000006 RID: 6
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class AdminRequestAnnouncement : GameNetworkMessage
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002270 File Offset: 0x00000470
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002278 File Offset: 0x00000478
		public string Message { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002281 File Offset: 0x00000481
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002289 File Offset: 0x00000489
		public bool IsAdminBroadcast { get; private set; }

		// Token: 0x0600000B RID: 11 RVA: 0x00002292 File Offset: 0x00000492
		public AdminRequestAnnouncement(string message, bool isAdminBroadcast)
		{
			this.Message = message;
			this.IsAdminBroadcast = isAdminBroadcast;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022A8 File Offset: 0x000004A8
		public AdminRequestAnnouncement()
		{
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022B0 File Offset: 0x000004B0
		protected override bool OnRead()
		{
			bool flag = true;
			this.Message = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.IsAdminBroadcast = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022DA File Offset: 0x000004DA
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.Message);
			GameNetworkMessage.WriteBoolToPacket(this.IsAdminBroadcast);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022F2 File Offset: 0x000004F2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022F8 File Offset: 0x000004F8
		protected override string OnGetLogFormat()
		{
			return "AdminRequestAnnouncement: " + this.Message + " " + this.IsAdminBroadcast.ToString();
		}
	}
}
