using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000029 RID: 41
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplySiegeWeaponOrder : GameNetworkMessage
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00003BE6 File Offset: 0x00001DE6
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00003BEE File Offset: 0x00001DEE
		public SiegeWeaponOrderType OrderType { get; private set; }

		// Token: 0x0600014D RID: 333 RVA: 0x00003BF7 File Offset: 0x00001DF7
		public ApplySiegeWeaponOrder(SiegeWeaponOrderType orderType)
		{
			this.OrderType = orderType;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00003C06 File Offset: 0x00001E06
		public ApplySiegeWeaponOrder()
		{
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00003C10 File Offset: 0x00001E10
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (SiegeWeaponOrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00003C32 File Offset: 0x00001E32
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00003C44 File Offset: 0x00001E44
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00003C4C File Offset: 0x00001E4C
		protected override string OnGetLogFormat()
		{
			return "Apply siege order: " + this.OrderType;
		}
	}
}
