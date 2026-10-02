using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000041 RID: 65
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ServerAdminMessage : GameNetworkMessage
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000481E File Offset: 0x00002A1E
		// (set) Token: 0x06000211 RID: 529 RVA: 0x00004826 File Offset: 0x00002A26
		public string Message { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000482F File Offset: 0x00002A2F
		// (set) Token: 0x06000213 RID: 531 RVA: 0x00004837 File Offset: 0x00002A37
		public bool IsAdminBroadcast { get; private set; }

		// Token: 0x06000214 RID: 532 RVA: 0x00004840 File Offset: 0x00002A40
		public ServerAdminMessage(string message, bool isAdminBroadcast)
		{
			this.Message = message;
			this.IsAdminBroadcast = isAdminBroadcast;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00004856 File Offset: 0x00002A56
		public ServerAdminMessage()
		{
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000485E File Offset: 0x00002A5E
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.Message);
			GameNetworkMessage.WriteBoolToPacket(this.IsAdminBroadcast);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00004878 File Offset: 0x00002A78
		protected override bool OnRead()
		{
			bool flag = true;
			this.Message = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.IsAdminBroadcast = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000048A2 File Offset: 0x00002AA2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000048A6 File Offset: 0x00002AA6
		protected override string OnGetLogFormat()
		{
			return "Admin message from server: " + this.Message;
		}
	}
}
