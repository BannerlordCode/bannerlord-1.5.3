using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000087 RID: 135
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateBanner : GameNetworkMessage
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00009770 File Offset: 0x00007970
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x00009778 File Offset: 0x00007978
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00009781 File Offset: 0x00007981
		// (set) Token: 0x0600052A RID: 1322 RVA: 0x00009789 File Offset: 0x00007989
		public string BannerCode { get; private set; }

		// Token: 0x0600052B RID: 1323 RVA: 0x00009792 File Offset: 0x00007992
		public CreateBanner(NetworkCommunicator peer, string bannerCode)
		{
			this.Peer = peer;
			this.BannerCode = bannerCode;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000097A8 File Offset: 0x000079A8
		public CreateBanner()
		{
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000097B0 File Offset: 0x000079B0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteStringToPacket(this.BannerCode);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000097C8 File Offset: 0x000079C8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.BannerCode = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000097F3 File Offset: 0x000079F3
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000097FB File Offset: 0x000079FB
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Create banner for peer: ",
				this.Peer.UserName,
				", with index: ",
				this.Peer.Index
			});
		}
	}
}
