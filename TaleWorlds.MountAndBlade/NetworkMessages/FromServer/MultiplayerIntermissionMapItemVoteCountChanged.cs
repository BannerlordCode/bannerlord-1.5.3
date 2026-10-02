using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005D RID: 93
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionMapItemVoteCountChanged : GameNetworkMessage
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000640D File Offset: 0x0000460D
		// (set) Token: 0x06000342 RID: 834 RVA: 0x00006415 File Offset: 0x00004615
		public int MapItemIndex { get; private set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000641E File Offset: 0x0000461E
		// (set) Token: 0x06000344 RID: 836 RVA: 0x00006426 File Offset: 0x00004626
		public int VoteCount { get; private set; }

		// Token: 0x06000345 RID: 837 RVA: 0x0000642F File Offset: 0x0000462F
		public MultiplayerIntermissionMapItemVoteCountChanged()
		{
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00006437 File Offset: 0x00004637
		public MultiplayerIntermissionMapItemVoteCountChanged(int mapItemIndex, int voteCount)
		{
			this.MapItemIndex = mapItemIndex;
			this.VoteCount = voteCount;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00006450 File Offset: 0x00004650
		protected override bool OnRead()
		{
			bool flag = true;
			this.MapItemIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionMapVoteItemCountCompressionInfo, ref flag);
			this.VoteCount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionVoterCountCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00006484 File Offset: 0x00004684
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.MapItemIndex, CompressionBasic.IntermissionMapVoteItemCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.VoteCount, CompressionBasic.IntermissionVoterCountCompressionInfo);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x000064A6 File Offset: 0x000046A6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000064AE File Offset: 0x000046AE
		protected override string OnGetLogFormat()
		{
			return string.Format("Vote count changed for map with index: {0}, vote count: {1}.", this.MapItemIndex, this.VoteCount);
		}
	}
}
