using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000028 RID: 40
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithTwoPositions : GameNetworkMessage
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00003AB3 File Offset: 0x00001CB3
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00003ABB File Offset: 0x00001CBB
		public OrderType OrderType { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00003AC4 File Offset: 0x00001CC4
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00003ACC File Offset: 0x00001CCC
		public Vec3 Position1 { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00003AD5 File Offset: 0x00001CD5
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00003ADD File Offset: 0x00001CDD
		public Vec3 Position2 { get; private set; }

		// Token: 0x06000145 RID: 325 RVA: 0x00003AE6 File Offset: 0x00001CE6
		public ApplyOrderWithTwoPositions(OrderType orderType, Vec3 position1, Vec3 position2)
		{
			this.OrderType = orderType;
			this.Position1 = position1;
			this.Position2 = position2;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00003B03 File Offset: 0x00001D03
		public ApplyOrderWithTwoPositions()
		{
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00003B0C File Offset: 0x00001D0C
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (OrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			this.Position1 = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.OrderPositionCompressionInfo, ref flag);
			this.Position2 = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.OrderPositionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00003B52 File Offset: 0x00001D52
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Position1, CompressionMission.OrderPositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Position2, CompressionMission.OrderPositionCompressionInfo);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00003B84 File Offset: 0x00001D84
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Orders;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00003B8C File Offset: 0x00001D8C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Apply order: ", this.OrderType, ", to position 1: ", this.Position1, " and position 2: ", this.Position2 });
		}
	}
}
