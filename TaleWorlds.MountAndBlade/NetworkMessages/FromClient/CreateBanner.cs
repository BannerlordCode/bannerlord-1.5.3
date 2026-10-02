using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002D RID: 45
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class CreateBanner : GameNetworkMessage
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00003D0C File Offset: 0x00001F0C
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00003D14 File Offset: 0x00001F14
		public string BannerCode { get; private set; }

		// Token: 0x06000167 RID: 359 RVA: 0x00003D1D File Offset: 0x00001F1D
		public CreateBanner(string bannerCode)
		{
			this.BannerCode = bannerCode;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00003D2C File Offset: 0x00001F2C
		public CreateBanner()
		{
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00003D34 File Offset: 0x00001F34
		protected override bool OnRead()
		{
			bool flag = true;
			this.BannerCode = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00003D51 File Offset: 0x00001F51
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.BannerCode);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00003D5E File Offset: 0x00001F5E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00003D66 File Offset: 0x00001F66
		protected override string OnGetLogFormat()
		{
			return "Clients has updated his banner";
		}
	}
}
