using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200001A RID: 26
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class KickPlayerPollRequested : GameNetworkMessage
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000031AF File Offset: 0x000013AF
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x000031B7 File Offset: 0x000013B7
		public NetworkCommunicator PlayerPeer { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000031C0 File Offset: 0x000013C0
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x000031C8 File Offset: 0x000013C8
		public bool BanPlayer { get; private set; }

		// Token: 0x060000C7 RID: 199 RVA: 0x000031D1 File Offset: 0x000013D1
		public KickPlayerPollRequested(NetworkCommunicator playerPeer, bool banPlayer)
		{
			this.PlayerPeer = playerPeer;
			this.BanPlayer = banPlayer;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000031E7 File Offset: 0x000013E7
		public KickPlayerPollRequested()
		{
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000031F0 File Offset: 0x000013F0
		protected override bool OnRead()
		{
			bool flag = true;
			this.PlayerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, true);
			this.BanPlayer = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000321B File Offset: 0x0000141B
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.PlayerPeer);
			GameNetworkMessage.WriteBoolToPacket(this.BanPlayer);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003233 File Offset: 0x00001433
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000323B File Offset: 0x0000143B
		protected override string OnGetLogFormat()
		{
			string text = "Requested to start poll to kick";
			string text2 = (this.BanPlayer ? " and ban" : "");
			string text3 = " player: ";
			NetworkCommunicator playerPeer = this.PlayerPeer;
			return text + text2 + text3 + ((playerPeer != null) ? playerPeer.UserName : null);
		}
	}
}
