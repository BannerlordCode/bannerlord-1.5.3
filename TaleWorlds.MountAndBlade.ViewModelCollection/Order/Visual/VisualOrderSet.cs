using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x0200002D RID: 45
	public abstract class VisualOrderSet
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000C217 File Offset: 0x0000A417
		public MBReadOnlyList<VisualOrder> Orders
		{
			get
			{
				return this._orders;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000C21F File Offset: 0x0000A41F
		public VisualOrder SoloOrder
		{
			get
			{
				if (!this.IsSoloOrder)
				{
					return null;
				}
				return this._orders[0];
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600034D RID: 845
		public abstract bool IsSoloOrder { get; }

		// Token: 0x0600034E RID: 846
		public abstract TextObject GetName(OrderController orderController);

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600034F RID: 847
		public abstract string StringId { get; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000350 RID: 848
		public abstract string IconId { get; }

		// Token: 0x06000351 RID: 849 RVA: 0x0000C237 File Offset: 0x0000A437
		public VisualOrderSet()
		{
			this._orders = new MBList<VisualOrder>();
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000C24C File Offset: 0x0000A44C
		public void AddOrder(VisualOrder order)
		{
			if (this.IsSoloOrder)
			{
				Debug.FailedAssert("Can't add additional orders to solo orders", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderSet.cs", "AddOrder", 32);
				return;
			}
			if (this._orders.Contains(order))
			{
				Debug.FailedAssert("Order:" + order.StringId + " is already in collection: " + this.StringId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderSet.cs", "AddOrder", 38);
				return;
			}
			this._orders.Add(order);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000C2C0 File Offset: 0x0000A4C0
		public void RemoveOrder(VisualOrder order)
		{
			if (this.IsSoloOrder)
			{
				Debug.FailedAssert("Can't remove orders from solo orders", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderSet.cs", "RemoveOrder", 49);
				return;
			}
			if (!this._orders.Contains(order))
			{
				Debug.FailedAssert("Order:" + order.StringId + " is not in collection: " + this.StringId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderSet.cs", "RemoveOrder", 55);
				return;
			}
			this._orders.Remove(order);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000C334 File Offset: 0x0000A534
		public void ClearOrders()
		{
			if (this.IsSoloOrder)
			{
				Debug.FailedAssert("Can't remove orders from solo orders", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderSet.cs", "ClearOrders", 66);
				return;
			}
			this._orders.Clear();
		}

		// Token: 0x04000177 RID: 375
		private MBList<VisualOrder> _orders;
	}
}
