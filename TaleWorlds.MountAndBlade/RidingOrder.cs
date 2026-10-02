using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200015D RID: 349
	public struct RidingOrder
	{
		// Token: 0x06001269 RID: 4713 RVA: 0x00039AE6 File Offset: 0x00037CE6
		private RidingOrder(RidingOrder.RidingOrderEnum orderEnum)
		{
			this.OrderEnum = orderEnum;
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x0600126A RID: 4714 RVA: 0x00039AEF File Offset: 0x00037CEF
		public OrderType OrderType
		{
			get
			{
				if (this.OrderEnum == RidingOrder.RidingOrderEnum.Free)
				{
					return OrderType.RideFree;
				}
				if (this.OrderEnum != RidingOrder.RidingOrderEnum.Mount)
				{
					return OrderType.Dismount;
				}
				return OrderType.Mount;
			}
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00039B0C File Offset: 0x00037D0C
		public override bool Equals(object obj)
		{
			if (obj is RidingOrder)
			{
				RidingOrder ridingOrder = (RidingOrder)obj;
				return ridingOrder == this;
			}
			return false;
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00039B38 File Offset: 0x00037D38
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00039B40 File Offset: 0x00037D40
		public static bool operator !=(RidingOrder r1, RidingOrder r2)
		{
			return r1.OrderEnum != r2.OrderEnum;
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00039B53 File Offset: 0x00037D53
		public static bool operator ==(RidingOrder r1, RidingOrder r2)
		{
			return r1.OrderEnum == r2.OrderEnum;
		}

		// Token: 0x0400047E RID: 1150
		public readonly RidingOrder.RidingOrderEnum OrderEnum;

		// Token: 0x0400047F RID: 1151
		public static readonly RidingOrder RidingOrderFree = new RidingOrder(RidingOrder.RidingOrderEnum.Free);

		// Token: 0x04000480 RID: 1152
		public static readonly RidingOrder RidingOrderMount = new RidingOrder(RidingOrder.RidingOrderEnum.Mount);

		// Token: 0x04000481 RID: 1153
		public static readonly RidingOrder RidingOrderDismount = new RidingOrder(RidingOrder.RidingOrderEnum.Dismount);

		// Token: 0x0200048F RID: 1167
		public enum RidingOrderEnum
		{
			// Token: 0x04001B22 RID: 6946
			Free,
			// Token: 0x04001B23 RID: 6947
			Mount,
			// Token: 0x04001B24 RID: 6948
			Dismount
		}
	}
}
