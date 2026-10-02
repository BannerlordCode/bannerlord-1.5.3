using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000012 RID: 18
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class AdminMuteUnmutePlayer : GameNetworkMessage
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002B0B File Offset: 0x00000D0B
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00002B13 File Offset: 0x00000D13
		public NetworkCommunicator PlayerPeer { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002B1C File Offset: 0x00000D1C
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00002B24 File Offset: 0x00000D24
		public bool Unmute { get; private set; }

		// Token: 0x0600007A RID: 122 RVA: 0x00002B2D File Offset: 0x00000D2D
		public AdminMuteUnmutePlayer(NetworkCommunicator playerPeer, bool unmute)
		{
			this.PlayerPeer = playerPeer;
			this.Unmute = unmute;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002B43 File Offset: 0x00000D43
		public AdminMuteUnmutePlayer()
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002B4C File Offset: 0x00000D4C
		protected override bool OnRead()
		{
			bool flag = true;
			this.PlayerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Unmute = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002B77 File Offset: 0x00000D77
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.PlayerPeer);
			GameNetworkMessage.WriteBoolToPacket(this.Unmute);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002B8F File Offset: 0x00000D8F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002B97 File Offset: 0x00000D97
		protected override string OnGetLogFormat()
		{
			return "Requested to " + (this.Unmute ? " unmute" : "mute") + " player: " + this.PlayerPeer.UserName;
		}
	}
}
