using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000058 RID: 88
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class KillDeathCountChange : GameNetworkMessage
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00005F65 File Offset: 0x00004165
		// (set) Token: 0x0600030C RID: 780 RVA: 0x00005F6D File Offset: 0x0000416D
		public NetworkCommunicator VictimPeer { get; private set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00005F76 File Offset: 0x00004176
		// (set) Token: 0x0600030E RID: 782 RVA: 0x00005F7E File Offset: 0x0000417E
		public NetworkCommunicator AttackerPeer { get; private set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00005F87 File Offset: 0x00004187
		// (set) Token: 0x06000310 RID: 784 RVA: 0x00005F8F File Offset: 0x0000418F
		public int KillCount { get; private set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00005F98 File Offset: 0x00004198
		// (set) Token: 0x06000312 RID: 786 RVA: 0x00005FA0 File Offset: 0x000041A0
		public int AssistCount { get; private set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00005FA9 File Offset: 0x000041A9
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00005FB1 File Offset: 0x000041B1
		public int DeathCount { get; private set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00005FBA File Offset: 0x000041BA
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00005FC2 File Offset: 0x000041C2
		public int Score { get; private set; }

		// Token: 0x06000317 RID: 791 RVA: 0x00005FCB File Offset: 0x000041CB
		public KillDeathCountChange(NetworkCommunicator peer, NetworkCommunicator attackerPeer, int killCount, int assistCount, int deathCount, int score)
		{
			this.VictimPeer = peer;
			this.AttackerPeer = attackerPeer;
			this.KillCount = killCount;
			this.AssistCount = assistCount;
			this.DeathCount = deathCount;
			this.Score = score;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00006000 File Offset: 0x00004200
		public KillDeathCountChange()
		{
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00006008 File Offset: 0x00004208
		protected override bool OnRead()
		{
			bool flag = true;
			this.VictimPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.AttackerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, true);
			this.KillCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.AssistCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.DeathCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.Score = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.ScoreCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000607C File Offset: 0x0000427C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.VictimPeer);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.AttackerPeer);
			GameNetworkMessage.WriteIntToPacket(this.KillCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.AssistCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.DeathCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.Score, CompressionMatchmaker.ScoreCompressionInfo);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000060DF File Offset: 0x000042DF
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000060E8 File Offset: 0x000042E8
		protected override string OnGetLogFormat()
		{
			object[] array = new object[11];
			array[0] = "Kill-Death Count Changed. Peer: ";
			int num = 1;
			NetworkCommunicator victimPeer = this.VictimPeer;
			array[num] = ((victimPeer != null) ? victimPeer.UserName : null) ?? "NULL";
			array[2] = " killed peer: ";
			int num2 = 3;
			NetworkCommunicator attackerPeer = this.AttackerPeer;
			array[num2] = ((attackerPeer != null) ? attackerPeer.UserName : null) ?? "NULL";
			array[4] = " and now has ";
			array[5] = this.KillCount;
			array[6] = " kills, ";
			array[7] = this.AssistCount;
			array[8] = " assists, and ";
			array[9] = this.DeathCount;
			array[10] = " deaths.";
			return string.Concat(array);
		}
	}
}
