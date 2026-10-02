using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200001E RID: 30
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SendClanInfo : GameNetworkMessage
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000338F File Offset: 0x0000158F
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00003397 File Offset: 0x00001597
		public string ClanName { get; private set; }

		// Token: 0x060000E4 RID: 228 RVA: 0x000033A0 File Offset: 0x000015A0
		public SendClanInfo(string clanName)
		{
			this.ClanName = clanName ?? string.Empty;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000033B8 File Offset: 0x000015B8
		public SendClanInfo()
		{
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000033C0 File Offset: 0x000015C0
		protected override bool OnRead()
		{
			bool flag = true;
			this.ClanName = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000033DD File Offset: 0x000015DD
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.ClanName);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000033EA File Offset: 0x000015EA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000033EE File Offset: 0x000015EE
		protected override string OnGetLogFormat()
		{
			return "Client sent clan info: " + this.ClanName;
		}
	}
}
