using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000056 RID: 86
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class KickPlayerPollClosed : GameNetworkMessage
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00005D56 File Offset: 0x00003F56
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x00005D5E File Offset: 0x00003F5E
		public NetworkCommunicator PlayerPeer { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x00005D67 File Offset: 0x00003F67
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x00005D6F File Offset: 0x00003F6F
		public bool Accepted { get; private set; }

		// Token: 0x060002F9 RID: 761 RVA: 0x00005D78 File Offset: 0x00003F78
		public KickPlayerPollClosed(NetworkCommunicator playerPeer, bool accepted)
		{
			this.PlayerPeer = playerPeer;
			this.Accepted = accepted;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00005D8E File Offset: 0x00003F8E
		public KickPlayerPollClosed()
		{
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00005D98 File Offset: 0x00003F98
		protected override bool OnRead()
		{
			bool flag = true;
			this.PlayerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Accepted = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00005DC3 File Offset: 0x00003FC3
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.PlayerPeer);
			GameNetworkMessage.WriteBoolToPacket(this.Accepted);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00005DDB File Offset: 0x00003FDB
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00005DE4 File Offset: 0x00003FE4
		protected override string OnGetLogFormat()
		{
			string[] array = new string[5];
			array[0] = "Poll is closed. ";
			int num = 1;
			NetworkCommunicator playerPeer = this.PlayerPeer;
			array[num] = ((playerPeer != null) ? playerPeer.UserName : null);
			array[2] = " is ";
			array[3] = (this.Accepted ? "" : "not ");
			array[4] = "kicked.";
			return string.Concat(array);
		}
	}
}
