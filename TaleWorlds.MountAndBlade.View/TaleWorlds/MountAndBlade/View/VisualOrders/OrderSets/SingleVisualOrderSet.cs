using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;

namespace TaleWorlds.MountAndBlade.View.VisualOrders.OrderSets
{
	// Token: 0x0200002E RID: 46
	public class SingleVisualOrderSet : VisualOrderSet
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000091AA File Offset: 0x000073AA
		public override bool IsSoloOrder
		{
			get
			{
				return !this._isInitializing;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000091B5 File Offset: 0x000073B5
		public override TextObject GetName(OrderController orderController)
		{
			return this.Order.GetName(orderController);
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000091C3 File Offset: 0x000073C3
		public override string StringId
		{
			get
			{
				return this.Order.StringId;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000091D0 File Offset: 0x000073D0
		public override string IconId
		{
			get
			{
				return this.Order.IconId;
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000091DD File Offset: 0x000073DD
		public SingleVisualOrderSet(VisualOrder order)
		{
			this._isInitializing = true;
			this.Order = order;
			base.AddOrder(this.Order);
			this._isInitializing = false;
		}

		// Token: 0x0400005C RID: 92
		public readonly VisualOrder Order;

		// Token: 0x0400005D RID: 93
		private bool _isInitializing;
	}
}
