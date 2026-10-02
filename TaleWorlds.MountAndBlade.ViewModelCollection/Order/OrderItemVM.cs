using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x0200001E RID: 30
	public class OrderItemVM : OrderItemBaseVM
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060002B3 RID: 691 RVA: 0x0000AB9C File Offset: 0x00008D9C
		// (remove) Token: 0x060002B4 RID: 692 RVA: 0x0000ABD0 File Offset: 0x00008DD0
		public static event Action<OrderItemVM> OnExecuteOrder;

		// Token: 0x060002B5 RID: 693 RVA: 0x0000AC03 File Offset: 0x00008E03
		public OrderItemVM(OrderController orderController, VisualOrder order)
			: base(orderController)
		{
			this.Order = order;
			base.OrderIconId = order.IconId;
			base.IsActive = true;
			this.RefreshValues();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000AC2C File Offset: 0x00008E2C
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject name = this.Order.GetName(this._orderController);
			base.Name = ((name != null) ? name.ToString() : null);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000AC58 File Offset: 0x00008E58
		protected override void OnRefreshState()
		{
			OrderState activeState = this.Order.GetActiveState(this._orderController);
			base.IsActive = activeState == OrderState.Active;
			base.SelectionState = activeState.ToString();
			base.Name = this.Order.GetName(this._orderController).ToString();
			base.OrderIconId = this.Order.IconId;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000ACC1 File Offset: 0x00008EC1
		protected override void OnExecuteAction(VisualOrderExecutionParameters executionParameters)
		{
			this.Order.BeforeExecuteOrder(this._orderController, executionParameters);
			this.Order.ExecuteOrder(this._orderController, executionParameters);
			Action<OrderItemVM> onExecuteOrder = OrderItemVM.OnExecuteOrder;
			if (onExecuteOrder == null)
			{
				return;
			}
			onExecuteOrder(this);
		}

		// Token: 0x04000136 RID: 310
		public readonly VisualOrder Order;
	}
}
