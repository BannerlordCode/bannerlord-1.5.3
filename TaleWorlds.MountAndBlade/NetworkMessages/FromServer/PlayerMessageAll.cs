using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200003F RID: 63
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PlayerMessageAll : GameNetworkMessage
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000469F File Offset: 0x0000289F
		// (set) Token: 0x060001FD RID: 509 RVA: 0x000046A7 File Offset: 0x000028A7
		public NetworkCommunicator Player { get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001FE RID: 510 RVA: 0x000046B0 File Offset: 0x000028B0
		// (set) Token: 0x060001FF RID: 511 RVA: 0x000046B8 File Offset: 0x000028B8
		public string Message { get; private set; }

		// Token: 0x06000200 RID: 512 RVA: 0x000046C1 File Offset: 0x000028C1
		public PlayerMessageAll(NetworkCommunicator player, string message)
		{
			this.Player = player;
			this.Message = message;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000046D7 File Offset: 0x000028D7
		public PlayerMessageAll()
		{
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000046DF File Offset: 0x000028DF
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Player);
			GameNetworkMessage.WriteStringToPacket(this.Message);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000046F8 File Offset: 0x000028F8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Player = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Message = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00004723 File Offset: 0x00002923
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00004727 File Offset: 0x00002927
		protected override string OnGetLogFormat()
		{
			return "Receiving Player message to all: " + this.Message;
		}
	}
}
