using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000025 RID: 37
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithFormationAndPercentage : GameNetworkMessage
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000121 RID: 289 RVA: 0x0000382E File Offset: 0x00001A2E
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00003836 File Offset: 0x00001A36
		public OrderType OrderType { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000383F File Offset: 0x00001A3F
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00003847 File Offset: 0x00001A47
		public int FormationIndex { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00003850 File Offset: 0x00001A50
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00003858 File Offset: 0x00001A58
		public int Percentage { get; private set; }

		// Token: 0x06000127 RID: 295 RVA: 0x00003861 File Offset: 0x00001A61
		public ApplyOrderWithFormationAndPercentage(OrderType orderType, int formationIndex, int percentage)
		{
			this.OrderType = orderType;
			this.FormationIndex = formationIndex;
			this.Percentage = percentage;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000387E File Offset: 0x00001A7E
		public ApplyOrderWithFormationAndPercentage()
		{
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00003888 File Offset: 0x00001A88
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.Percentage = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PercentageCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000038CE File Offset: 0x00001ACE
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.Percentage, CompressionBasic.PercentageCompressionInfo);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00003900 File Offset: 0x00001B00
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00003908 File Offset: 0x00001B08
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to formation with index: ", this.FormationIndex, " and percentage: ", this.Percentage });
		}
	}
}
