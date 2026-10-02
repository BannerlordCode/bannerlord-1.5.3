using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006F RID: 111
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RoundCountChange : GameNetworkMessage
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003DE RID: 990 RVA: 0x00007666 File Offset: 0x00005866
		// (set) Token: 0x060003DF RID: 991 RVA: 0x0000766E File Offset: 0x0000586E
		public int RoundCount { get; private set; }

		// Token: 0x060003E0 RID: 992 RVA: 0x00007677 File Offset: 0x00005877
		public RoundCountChange(int roundCount)
		{
			this.RoundCount = roundCount;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00007686 File Offset: 0x00005886
		public RoundCountChange()
		{
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00007690 File Offset: 0x00005890
		protected override bool OnRead()
		{
			bool flag = true;
			this.RoundCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissionRoundCountCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000076B2 File Offset: 0x000058B2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.RoundCount, CompressionMission.MissionRoundCountCompressionInfo);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000076C4 File Offset: 0x000058C4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000076CC File Offset: 0x000058CC
		protected override string OnGetLogFormat()
		{
			return "Change round count to: " + this.RoundCount;
		}
	}
}
