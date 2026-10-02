using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000027 RID: 39
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithPosition : GameNetworkMessage
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000039DA File Offset: 0x00001BDA
		// (set) Token: 0x06000136 RID: 310 RVA: 0x000039E2 File Offset: 0x00001BE2
		public OrderType OrderType { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000137 RID: 311 RVA: 0x000039EB File Offset: 0x00001BEB
		// (set) Token: 0x06000138 RID: 312 RVA: 0x000039F3 File Offset: 0x00001BF3
		public Vec3 Position { get; private set; }

		// Token: 0x06000139 RID: 313 RVA: 0x000039FC File Offset: 0x00001BFC
		public ApplyOrderWithPosition(OrderType orderType, Vec3 position)
		{
			this.OrderType = orderType;
			this.Position = position;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00003A12 File Offset: 0x00001C12
		public ApplyOrderWithPosition()
		{
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00003A1C File Offset: 0x00001C1C
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.OrderPositionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00003A50 File Offset: 0x00001C50
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionMission.OrderPositionCompressionInfo);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00003A72 File Offset: 0x00001C72
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Orders;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00003A7A File Offset: 0x00001C7A
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to position: ", this.Position });
		}
	}
}
