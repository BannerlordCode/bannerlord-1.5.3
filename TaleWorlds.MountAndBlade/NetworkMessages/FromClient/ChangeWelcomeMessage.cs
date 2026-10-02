using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000017 RID: 23
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ChangeWelcomeMessage : GameNetworkMessage
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00002FCF File Offset: 0x000011CF
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00002FD7 File Offset: 0x000011D7
		public string NewWelcomeMessage { get; private set; }

		// Token: 0x060000A9 RID: 169 RVA: 0x00002FE0 File Offset: 0x000011E0
		public ChangeWelcomeMessage(string newWelcomeMessage)
		{
			this.NewWelcomeMessage = newWelcomeMessage;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00002FEF File Offset: 0x000011EF
		public ChangeWelcomeMessage()
		{
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00002FF8 File Offset: 0x000011F8
		protected override bool OnRead()
		{
			bool flag = true;
			this.NewWelcomeMessage = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003015 File Offset: 0x00001215
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.NewWelcomeMessage);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003022 File Offset: 0x00001222
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000302A File Offset: 0x0000122A
		protected override string OnGetLogFormat()
		{
			return "Requested to change the welcome message to: " + this.NewWelcomeMessage;
		}
	}
}
