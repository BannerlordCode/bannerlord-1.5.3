using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000047 RID: 71
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelEnded : GameNetworkMessage
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00004D02 File Offset: 0x00002F02
		// (set) Token: 0x0600024C RID: 588 RVA: 0x00004D0A File Offset: 0x00002F0A
		public NetworkCommunicator WinnerPeer { get; private set; }

		// Token: 0x0600024D RID: 589 RVA: 0x00004D13 File Offset: 0x00002F13
		public DuelEnded(NetworkCommunicator winnerPeer)
		{
			this.WinnerPeer = winnerPeer;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00004D22 File Offset: 0x00002F22
		public DuelEnded()
		{
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00004D2C File Offset: 0x00002F2C
		protected override bool OnRead()
		{
			bool flag = true;
			this.WinnerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			return flag;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00004D4A File Offset: 0x00002F4A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.WinnerPeer);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00004D57 File Offset: 0x00002F57
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00004D5F File Offset: 0x00002F5F
		protected override string OnGetLogFormat()
		{
			return this.WinnerPeer.UserName + "has won the duel";
		}
	}
}
