using System;

namespace TaleWorlds.MountAndBlade.Network.Messages
{
	// Token: 0x020003C7 RID: 967
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreatePlayer : GameNetworkMessage
	{
		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06003668 RID: 13928 RVA: 0x000E155D File Offset: 0x000DF75D
		// (set) Token: 0x06003669 RID: 13929 RVA: 0x000E1565 File Offset: 0x000DF765
		public int PlayerIndex { get; private set; }

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x0600366A RID: 13930 RVA: 0x000E156E File Offset: 0x000DF76E
		// (set) Token: 0x0600366B RID: 13931 RVA: 0x000E1576 File Offset: 0x000DF776
		public string PlayerName { get; private set; }

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x0600366C RID: 13932 RVA: 0x000E157F File Offset: 0x000DF77F
		// (set) Token: 0x0600366D RID: 13933 RVA: 0x000E1587 File Offset: 0x000DF787
		public int DisconnectedPeerIndex { get; private set; }

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x0600366E RID: 13934 RVA: 0x000E1590 File Offset: 0x000DF790
		// (set) Token: 0x0600366F RID: 13935 RVA: 0x000E1598 File Offset: 0x000DF798
		public bool IsNonExistingDisconnectedPeer { get; private set; }

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x06003670 RID: 13936 RVA: 0x000E15A1 File Offset: 0x000DF7A1
		// (set) Token: 0x06003671 RID: 13937 RVA: 0x000E15A9 File Offset: 0x000DF7A9
		public bool IsReceiverPeer { get; private set; }

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06003672 RID: 13938 RVA: 0x000E15B2 File Offset: 0x000DF7B2
		// (set) Token: 0x06003673 RID: 13939 RVA: 0x000E15BA File Offset: 0x000DF7BA
		public bool IsSpectator { get; private set; }

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x06003674 RID: 13940 RVA: 0x000E15C3 File Offset: 0x000DF7C3
		// (set) Token: 0x06003675 RID: 13941 RVA: 0x000E15CB File Offset: 0x000DF7CB
		public bool IsAdmin { get; private set; }

		// Token: 0x06003676 RID: 13942 RVA: 0x000E15D4 File Offset: 0x000DF7D4
		public CreatePlayer(int playerIndex, string playerName, int disconnectedPeerIndex, bool isNonExistingDisconnectedPeer = false, bool isReceiverPeer = false, bool isSpectator = false, bool isAdmin = false)
		{
			this.PlayerIndex = playerIndex;
			this.PlayerName = playerName;
			this.DisconnectedPeerIndex = disconnectedPeerIndex;
			this.IsNonExistingDisconnectedPeer = isNonExistingDisconnectedPeer;
			this.IsReceiverPeer = isReceiverPeer;
			this.IsSpectator = isSpectator;
			this.IsAdmin = isAdmin;
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x000E1611 File Offset: 0x000DF811
		public CreatePlayer()
		{
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x000E161C File Offset: 0x000DF81C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.PlayerIndex, CompressionBasic.PlayerCompressionInfo);
			GameNetworkMessage.WriteStringToPacket(this.PlayerName);
			GameNetworkMessage.WriteIntToPacket(this.DisconnectedPeerIndex, CompressionBasic.PlayerCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.IsNonExistingDisconnectedPeer);
			GameNetworkMessage.WriteBoolToPacket(this.IsReceiverPeer);
			GameNetworkMessage.WriteBoolToPacket(this.IsSpectator);
			GameNetworkMessage.WriteBoolToPacket(this.IsAdmin);
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x000E1680 File Offset: 0x000DF880
		protected override bool OnRead()
		{
			bool flag = true;
			this.PlayerIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			this.PlayerName = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.DisconnectedPeerIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			this.IsNonExistingDisconnectedPeer = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsReceiverPeer = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsSpectator = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsAdmin = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x000E16F5 File Offset: 0x000DF8F5
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x000E16FC File Offset: 0x000DF8FC
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Create a new player with name: ",
				this.PlayerName,
				" and index: ",
				this.PlayerIndex,
				" and dcedIndex: ",
				this.DisconnectedPeerIndex,
				" which is ",
				(!this.IsNonExistingDisconnectedPeer) ? "not" : "",
				" a NonExistingDisconnectedPeer, isSpectator: ",
				this.IsSpectator.ToString(),
				", isAdmin: ",
				this.IsAdmin.ToString()
			});
		}
	}
}
