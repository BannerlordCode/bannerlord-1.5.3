using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000021 RID: 33
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrder : GameNetworkMessage
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000034D7 File Offset: 0x000016D7
		// (set) Token: 0x060000FA RID: 250 RVA: 0x000034DF File Offset: 0x000016DF
		public OrderType OrderType { get; private set; }

		// Token: 0x060000FB RID: 251 RVA: 0x000034E8 File Offset: 0x000016E8
		public ApplyOrder(OrderType orderType)
		{
			this.OrderType = orderType;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000034F7 File Offset: 0x000016F7
		public ApplyOrder()
		{
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00003500 File Offset: 0x00001700
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00003522 File Offset: 0x00001722
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003534 File Offset: 0x00001734
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Orders;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000353C File Offset: 0x0000173C
		protected override string OnGetLogFormat()
		{
			return "Apply order: " + this.OrderType;
		}
	}
}
