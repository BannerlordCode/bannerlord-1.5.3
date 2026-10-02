using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000068 RID: 104
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PollRequestRejected : GameNetworkMessage
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00006F59 File Offset: 0x00005159
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00006F61 File Offset: 0x00005161
		public int Reason { get; private set; }

		// Token: 0x0600039D RID: 925 RVA: 0x00006F6A File Offset: 0x0000516A
		public PollRequestRejected(int reason)
		{
			this.Reason = reason;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00006F79 File Offset: 0x00005179
		public PollRequestRejected()
		{
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00006F84 File Offset: 0x00005184
		protected override bool OnRead()
		{
			bool flag = true;
			this.Reason = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MultiplayerPollRejectReasonCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00006FA6 File Offset: 0x000051A6
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.Reason, CompressionMission.MultiplayerPollRejectReasonCompressionInfo);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00006FB8 File Offset: 0x000051B8
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00006FC0 File Offset: 0x000051C0
		protected override string OnGetLogFormat()
		{
			return "Poll request rejected (" + (MultiplayerPollRejectReason)this.Reason + ")";
		}
	}
}
