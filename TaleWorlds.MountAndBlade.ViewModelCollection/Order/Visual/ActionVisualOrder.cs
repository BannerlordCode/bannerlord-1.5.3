using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x02000024 RID: 36
	public sealed class ActionVisualOrder : VisualOrder
	{
		// Token: 0x06000324 RID: 804 RVA: 0x0000BD1F File Offset: 0x00009F1F
		public ActionVisualOrder(string iconId, ActionVisualOrder.OrderActionDelegate orderAction, TextObject name)
			: base(iconId)
		{
			this._name = name;
			this._orderAction = orderAction;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000BD36 File Offset: 0x00009F36
		public override TextObject GetName(OrderController orderController)
		{
			return this._name;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000BD3E File Offset: 0x00009F3E
		public override bool IsTargeted()
		{
			return false;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000BD41 File Offset: 0x00009F41
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			ActionVisualOrder.OrderActionDelegate orderAction = this._orderAction;
			if (orderAction == null)
			{
				return;
			}
			orderAction(orderController, executionParameters);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000BD55 File Offset: 0x00009F55
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(false);
		}

		// Token: 0x04000166 RID: 358
		private readonly ActionVisualOrder.OrderActionDelegate _orderAction;

		// Token: 0x04000167 RID: 359
		private readonly TextObject _name;

		// Token: 0x020000C2 RID: 194
		// (Invoke) Token: 0x06000C40 RID: 3136
		public delegate void OrderActionDelegate(OrderController orderController, VisualOrderExecutionParameters executionParameters);
	}
}
