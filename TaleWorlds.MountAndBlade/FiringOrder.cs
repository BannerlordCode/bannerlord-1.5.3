using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000158 RID: 344
	public struct FiringOrder
	{
		// Token: 0x0600120B RID: 4619 RVA: 0x000373D0 File Offset: 0x000355D0
		private FiringOrder(FiringOrder.RangedWeaponUsageOrderEnum orderEnum)
		{
			this.OrderEnum = orderEnum;
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x000373D9 File Offset: 0x000355D9
		public OrderType OrderType
		{
			get
			{
				if (this.OrderEnum != FiringOrder.RangedWeaponUsageOrderEnum.FireAtWill)
				{
					return OrderType.HoldFire;
				}
				return OrderType.FireAtWill;
			}
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x000373E8 File Offset: 0x000355E8
		public override bool Equals(object obj)
		{
			if (obj is FiringOrder)
			{
				FiringOrder firingOrder = (FiringOrder)obj;
				return firingOrder == this;
			}
			return false;
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00037414 File Offset: 0x00035614
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0003741C File Offset: 0x0003561C
		public static bool operator !=(FiringOrder f1, FiringOrder f2)
		{
			return f1.OrderEnum != f2.OrderEnum;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0003742F File Offset: 0x0003562F
		public static bool operator ==(FiringOrder f1, FiringOrder f2)
		{
			return f1.OrderEnum == f2.OrderEnum;
		}

		// Token: 0x0400045C RID: 1116
		public readonly FiringOrder.RangedWeaponUsageOrderEnum OrderEnum;

		// Token: 0x0400045D RID: 1117
		public static readonly FiringOrder FiringOrderFireAtWill = new FiringOrder(FiringOrder.RangedWeaponUsageOrderEnum.FireAtWill);

		// Token: 0x0400045E RID: 1118
		public static readonly FiringOrder FiringOrderHoldYourFire = new FiringOrder(FiringOrder.RangedWeaponUsageOrderEnum.HoldYourFire);

		// Token: 0x0200047F RID: 1151
		public enum RangedWeaponUsageOrderEnum
		{
			// Token: 0x04001AEE RID: 6894
			FireAtWill,
			// Token: 0x04001AEF RID: 6895
			HoldYourFire
		}
	}
}
