using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004F RID: 79
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class BotData : GameNetworkMessage
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000546B File Offset: 0x0000366B
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x00005473 File Offset: 0x00003673
		public BattleSideEnum Side { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000547C File Offset: 0x0000367C
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x00005484 File Offset: 0x00003684
		public int KillCount { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000548D File Offset: 0x0000368D
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x00005495 File Offset: 0x00003695
		public int AssistCount { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000549E File Offset: 0x0000369E
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x000054A6 File Offset: 0x000036A6
		public int DeathCount { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x000054AF File Offset: 0x000036AF
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x000054B7 File Offset: 0x000036B7
		public int AliveBotCount { get; private set; }

		// Token: 0x060002A9 RID: 681 RVA: 0x000054C0 File Offset: 0x000036C0
		public BotData(BattleSideEnum side, int kill, int assist, int death, int alive)
		{
			this.Side = side;
			this.KillCount = kill;
			this.AssistCount = assist;
			this.DeathCount = death;
			this.AliveBotCount = alive;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000054ED File Offset: 0x000036ED
		public BotData()
		{
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000054F8 File Offset: 0x000036F8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Side = (BattleSideEnum)GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamSideCompressionInfo, ref flag);
			this.KillCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.AssistCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.DeathCount = GameNetworkMessage.ReadIntFromPacket(CompressionMatchmaker.KillDeathAssistCountCompressionInfo, ref flag);
			this.AliveBotCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00005564 File Offset: 0x00003764
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.Side, CompressionMission.TeamSideCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.KillCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.AssistCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.DeathCount, CompressionMatchmaker.KillDeathAssistCountCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.AliveBotCount, CompressionMission.AgentCompressionInfo);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000055C1 File Offset: 0x000037C1
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000055C8 File Offset: 0x000037C8
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "BOTS for side: ", this.Side, ", Kill: ", this.KillCount, " Death: ", this.DeathCount, " Assist: ", this.AssistCount, ", Alive: ", this.AliveBotCount });
		}
	}
}
