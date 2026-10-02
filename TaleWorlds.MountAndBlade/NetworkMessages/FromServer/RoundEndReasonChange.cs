using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000070 RID: 112
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RoundEndReasonChange : GameNetworkMessage
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x000076E3 File Offset: 0x000058E3
		// (set) Token: 0x060003E7 RID: 999 RVA: 0x000076EB File Offset: 0x000058EB
		public RoundEndReason RoundEndReason { get; private set; }

		// Token: 0x060003E8 RID: 1000 RVA: 0x000076F4 File Offset: 0x000058F4
		public RoundEndReasonChange()
		{
			this.RoundEndReason = RoundEndReason.Invalid;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00007703 File Offset: 0x00005903
		public RoundEndReasonChange(RoundEndReason roundEndReason)
		{
			this.RoundEndReason = roundEndReason;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00007712 File Offset: 0x00005912
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.RoundEndReason, CompressionMission.RoundEndReasonCompressionInfo);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00007724 File Offset: 0x00005924
		protected override bool OnRead()
		{
			bool flag = true;
			this.RoundEndReason = (RoundEndReason)GameNetworkMessage.ReadIntFromPacket(CompressionMission.RoundEndReasonCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00007746 File Offset: 0x00005946
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00007750 File Offset: 0x00005950
		protected override string OnGetLogFormat()
		{
			return "Change round end reason to: " + this.RoundEndReason.ToString();
		}
	}
}
