using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000064 RID: 100
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PeerLastKillChange : GameNetworkMessage
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00006CE1 File Offset: 0x00004EE1
		// (set) Token: 0x06000379 RID: 889 RVA: 0x00006CE9 File Offset: 0x00004EE9
		public NetworkCommunicator KillerPeer { get; private set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00006CF2 File Offset: 0x00004EF2
		// (set) Token: 0x0600037B RID: 891 RVA: 0x00006CFA File Offset: 0x00004EFA
		public string VictimName { get; private set; }

		// Token: 0x0600037C RID: 892 RVA: 0x00006D03 File Offset: 0x00004F03
		public PeerLastKillChange(NetworkCommunicator killerPeer, string victimName)
		{
			this.KillerPeer = killerPeer;
			this.VictimName = victimName ?? string.Empty;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00006D22 File Offset: 0x00004F22
		public PeerLastKillChange()
		{
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00006D2C File Offset: 0x00004F2C
		protected override bool OnRead()
		{
			bool flag = true;
			this.KillerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.VictimName = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00006D57 File Offset: 0x00004F57
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.KillerPeer);
			GameNetworkMessage.WriteStringToPacket(this.VictimName);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00006D6F File Offset: 0x00004F6F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00006D77 File Offset: 0x00004F77
		protected override string OnGetLogFormat()
		{
			string text = "Peer last kill change for killer: ";
			NetworkCommunicator killerPeer = this.KillerPeer;
			return text + (((killerPeer != null) ? killerPeer.UserName : null) ?? "NULL") + " victim: " + this.VictimName;
		}
	}
}
