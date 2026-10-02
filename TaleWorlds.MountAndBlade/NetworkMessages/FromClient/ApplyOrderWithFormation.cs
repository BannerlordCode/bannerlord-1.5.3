using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000023 RID: 35
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithFormation : GameNetworkMessage
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00003621 File Offset: 0x00001821
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00003629 File Offset: 0x00001829
		public OrderType OrderType { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00003632 File Offset: 0x00001832
		// (set) Token: 0x0600010E RID: 270 RVA: 0x0000363A File Offset: 0x0000183A
		public int FormationIndex { get; private set; }

		// Token: 0x0600010F RID: 271 RVA: 0x00003643 File Offset: 0x00001843
		public ApplyOrderWithFormation(OrderType orderType, int formationIndex)
		{
			this.OrderType = orderType;
			this.FormationIndex = formationIndex;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003659 File Offset: 0x00001859
		public ApplyOrderWithFormation()
		{
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00003664 File Offset: 0x00001864
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00003698 File Offset: 0x00001898
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000036BA File Offset: 0x000018BA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000036C2 File Offset: 0x000018C2
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to formation with index: ", this.FormationIndex });
		}
	}
}
