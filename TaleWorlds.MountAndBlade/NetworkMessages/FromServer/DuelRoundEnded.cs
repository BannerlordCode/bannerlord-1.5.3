using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004B RID: 75
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelRoundEnded : GameNetworkMessage
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000279 RID: 633 RVA: 0x000050BB File Offset: 0x000032BB
		// (set) Token: 0x0600027A RID: 634 RVA: 0x000050C3 File Offset: 0x000032C3
		public NetworkCommunicator WinnerPeer { get; private set; }

		// Token: 0x0600027B RID: 635 RVA: 0x000050CC File Offset: 0x000032CC
		public DuelRoundEnded(NetworkCommunicator winnerPeer)
		{
			this.WinnerPeer = winnerPeer;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000050DB File Offset: 0x000032DB
		public DuelRoundEnded()
		{
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000050E4 File Offset: 0x000032E4
		protected override bool OnRead()
		{
			bool flag = true;
			this.WinnerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			return flag;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00005102 File Offset: 0x00003302
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.WinnerPeer);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000510F File Offset: 0x0000330F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00005117 File Offset: 0x00003317
		protected override string OnGetLogFormat()
		{
			return this.WinnerPeer.UserName + "has won the duel against round.";
		}
	}
}
