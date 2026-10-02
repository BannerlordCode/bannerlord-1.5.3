using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000048 RID: 72
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelPointsUpdateMessage : GameNetworkMessage
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00004D76 File Offset: 0x00002F76
		// (set) Token: 0x06000254 RID: 596 RVA: 0x00004D7E File Offset: 0x00002F7E
		public int Bounty { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000255 RID: 597 RVA: 0x00004D87 File Offset: 0x00002F87
		// (set) Token: 0x06000256 RID: 598 RVA: 0x00004D8F File Offset: 0x00002F8F
		public int Score { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00004D98 File Offset: 0x00002F98
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00004DA0 File Offset: 0x00002FA0
		public int NumberOfWins { get; private set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000259 RID: 601 RVA: 0x00004DA9 File Offset: 0x00002FA9
		// (set) Token: 0x0600025A RID: 602 RVA: 0x00004DB1 File Offset: 0x00002FB1
		public NetworkCommunicator NetworkCommunicator { get; private set; }

		// Token: 0x0600025B RID: 603 RVA: 0x00004DBA File Offset: 0x00002FBA
		public DuelPointsUpdateMessage()
		{
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00004DC2 File Offset: 0x00002FC2
		public DuelPointsUpdateMessage(DuelMissionRepresentative representative)
		{
			this.Bounty = representative.Bounty;
			this.Score = representative.Score;
			this.NumberOfWins = representative.NumberOfWins;
			this.NetworkCommunicator = representative.GetNetworkPeer();
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00004DFA File Offset: 0x00002FFA
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.Bounty, CompressionMatchmaker.ScoreCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.Score, CompressionMatchmaker.ScoreCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.NumberOfWins, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.NetworkCommunicator);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00004E38 File Offset: 0x00003038
		protected override bool OnRead()
		{
			bool flag = true;
			this.Bounty = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.ScoreCompressionInfo, ref flag);
			this.Score = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.ScoreCompressionInfo, ref flag);
			this.NumberOfWins = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.NetworkCommunicator = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			return flag;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00004E8C File Offset: 0x0000308C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00004E94 File Offset: 0x00003094
		protected override string OnGetLogFormat()
		{
			return "PointUpdateMessage";
		}
	}
}
