using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005B RID: 91
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionCultureItemVoteCountChanged : GameNetworkMessage
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600032F RID: 815 RVA: 0x000062D9 File Offset: 0x000044D9
		// (set) Token: 0x06000330 RID: 816 RVA: 0x000062E1 File Offset: 0x000044E1
		public int CultureItemIndex { get; private set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000331 RID: 817 RVA: 0x000062EA File Offset: 0x000044EA
		// (set) Token: 0x06000332 RID: 818 RVA: 0x000062F2 File Offset: 0x000044F2
		public int VoteCount { get; private set; }

		// Token: 0x06000333 RID: 819 RVA: 0x000062FB File Offset: 0x000044FB
		public MultiplayerIntermissionCultureItemVoteCountChanged()
		{
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00006303 File Offset: 0x00004503
		public MultiplayerIntermissionCultureItemVoteCountChanged(int cultureItemIndex, int voteCount)
		{
			this.CultureItemIndex = cultureItemIndex;
			this.VoteCount = voteCount;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000631C File Offset: 0x0000451C
		protected override bool OnRead()
		{
			bool flag = true;
			this.CultureItemIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.CultureIndexCompressionInfo, ref flag);
			this.VoteCount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionVoterCountCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00006350 File Offset: 0x00004550
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.CultureItemIndex, CompressionBasic.CultureIndexCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.VoteCount, CompressionBasic.IntermissionVoterCountCompressionInfo);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00006372 File Offset: 0x00004572
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000637A File Offset: 0x0000457A
		protected override string OnGetLogFormat()
		{
			return string.Format("Vote count changed for culture with index: {0}, vote count: {1}.", this.CultureItemIndex, this.VoteCount);
		}
	}
}
