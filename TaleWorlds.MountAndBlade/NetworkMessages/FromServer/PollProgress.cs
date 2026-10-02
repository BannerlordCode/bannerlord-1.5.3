using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000067 RID: 103
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PollProgress : GameNetworkMessage
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00006EB4 File Offset: 0x000050B4
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00006EBC File Offset: 0x000050BC
		public int VotesAccepted { get; private set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00006EC5 File Offset: 0x000050C5
		// (set) Token: 0x06000394 RID: 916 RVA: 0x00006ECD File Offset: 0x000050CD
		public int VotesRejected { get; private set; }

		// Token: 0x06000395 RID: 917 RVA: 0x00006ED6 File Offset: 0x000050D6
		public PollProgress(int votesAccepted, int votesRejected)
		{
			this.VotesAccepted = votesAccepted;
			this.VotesRejected = votesRejected;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00006EEC File Offset: 0x000050EC
		public PollProgress()
		{
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00006EF4 File Offset: 0x000050F4
		protected override bool OnRead()
		{
			bool flag = true;
			this.VotesAccepted = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			this.VotesRejected = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00006F28 File Offset: 0x00005128
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.VotesAccepted, CompressionBasic.PlayerCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.VotesRejected, CompressionBasic.PlayerCompressionInfo);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00006F4A File Offset: 0x0000514A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00006F52 File Offset: 0x00005152
		protected override string OnGetLogFormat()
		{
			return "Update on the voting progress.";
		}
	}
}
