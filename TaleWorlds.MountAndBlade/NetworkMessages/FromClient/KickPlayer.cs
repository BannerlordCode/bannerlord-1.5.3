using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000019 RID: 25
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class KickPlayer : GameNetworkMessage
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000030F3 File Offset: 0x000012F3
		// (set) Token: 0x060000BA RID: 186 RVA: 0x000030FB File Offset: 0x000012FB
		public NetworkCommunicator PlayerPeer { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00003104 File Offset: 0x00001304
		// (set) Token: 0x060000BC RID: 188 RVA: 0x0000310C File Offset: 0x0000130C
		public bool BanPlayer { get; private set; }

		// Token: 0x060000BD RID: 189 RVA: 0x00003115 File Offset: 0x00001315
		public KickPlayer(NetworkCommunicator playerPeer, bool banPlayer)
		{
			this.PlayerPeer = playerPeer;
			this.BanPlayer = banPlayer;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000312B File Offset: 0x0000132B
		public KickPlayer()
		{
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003134 File Offset: 0x00001334
		protected override bool OnRead()
		{
			bool flag = true;
			this.PlayerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.BanPlayer = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000315F File Offset: 0x0000135F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.PlayerPeer);
			GameNetworkMessage.WriteBoolToPacket(this.BanPlayer);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003177 File Offset: 0x00001377
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000317F File Offset: 0x0000137F
		protected override string OnGetLogFormat()
		{
			return "Requested to kick" + (this.BanPlayer ? " and ban" : "") + " player: " + this.PlayerPeer.UserName;
		}
	}
}
