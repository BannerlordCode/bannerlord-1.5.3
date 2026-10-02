using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000057 RID: 87
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class KickPlayerPollOpened : GameNetworkMessage
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00005E40 File Offset: 0x00004040
		// (set) Token: 0x06000300 RID: 768 RVA: 0x00005E48 File Offset: 0x00004048
		public NetworkCommunicator InitiatorPeer { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00005E51 File Offset: 0x00004051
		// (set) Token: 0x06000302 RID: 770 RVA: 0x00005E59 File Offset: 0x00004059
		public NetworkCommunicator PlayerPeer { get; private set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00005E62 File Offset: 0x00004062
		// (set) Token: 0x06000304 RID: 772 RVA: 0x00005E6A File Offset: 0x0000406A
		public bool BanPlayer { get; private set; }

		// Token: 0x06000305 RID: 773 RVA: 0x00005E73 File Offset: 0x00004073
		public KickPlayerPollOpened(NetworkCommunicator initiatorPeer, NetworkCommunicator playerPeer, bool banPlayer)
		{
			this.InitiatorPeer = initiatorPeer;
			this.PlayerPeer = playerPeer;
			this.BanPlayer = banPlayer;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00005E90 File Offset: 0x00004090
		public KickPlayerPollOpened()
		{
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00005E98 File Offset: 0x00004098
		protected override bool OnRead()
		{
			bool flag = true;
			this.InitiatorPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.PlayerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.BanPlayer = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00005ED1 File Offset: 0x000040D1
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.InitiatorPeer);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.PlayerPeer);
			GameNetworkMessage.WriteBoolToPacket(this.BanPlayer);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00005EF4 File Offset: 0x000040F4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00005EFC File Offset: 0x000040FC
		protected override string OnGetLogFormat()
		{
			string[] array = new string[5];
			int num = 0;
			NetworkCommunicator initiatorPeer = this.InitiatorPeer;
			array[num] = ((initiatorPeer != null) ? initiatorPeer.UserName : null);
			array[1] = " wants to start poll to kick";
			array[2] = (this.BanPlayer ? " and ban" : "");
			array[3] = " player: ";
			int num2 = 4;
			NetworkCommunicator playerPeer = this.PlayerPeer;
			array[num2] = ((playerPeer != null) ? playerPeer.UserName : null);
			return string.Concat(array);
		}
	}
}
