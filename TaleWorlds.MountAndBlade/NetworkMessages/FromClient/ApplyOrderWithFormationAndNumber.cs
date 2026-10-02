using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000024 RID: 36
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithFormationAndNumber : GameNetworkMessage
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000036FB File Offset: 0x000018FB
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00003703 File Offset: 0x00001903
		public OrderType OrderType { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000117 RID: 279 RVA: 0x0000370C File Offset: 0x0000190C
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00003714 File Offset: 0x00001914
		public int FormationIndex { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000371D File Offset: 0x0000191D
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00003725 File Offset: 0x00001925
		public int Number { get; private set; }

		// Token: 0x0600011B RID: 283 RVA: 0x0000372E File Offset: 0x0000192E
		public ApplyOrderWithFormationAndNumber(OrderType orderType, int formationIndex, int number)
		{
			this.OrderType = orderType;
			this.FormationIndex = formationIndex;
			this.Number = number;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000374B File Offset: 0x0000194B
		public ApplyOrderWithFormationAndNumber()
		{
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00003754 File Offset: 0x00001954
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.Number = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.DebugIntNonCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000379A File Offset: 0x0000199A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.Number, CompressionBasic.DebugIntNonCompressionInfo);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000037CC File Offset: 0x000019CC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000037D4 File Offset: 0x000019D4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to formation with index: ", this.FormationIndex, " and number: ", this.Number });
		}
	}
}
