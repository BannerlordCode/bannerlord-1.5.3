using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.MovementOrders
{
	// Token: 0x02000006 RID: 6
	public class ChargeVisualOrder : VisualOrder
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002225 File Offset: 0x00000425
		public ChargeVisualOrder(string iconId)
			: base(iconId)
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000222E File Offset: 0x0000042E
		public override TextObject GetName(OrderController orderController)
		{
			return new TextObject("{=Dxmq32qW}Charge", null);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000223B File Offset: 0x0000043B
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			if (executionParameters.HasFormation)
			{
				orderController.SetOrderWithFormation(OrderType.Charge, executionParameters.Formation);
				return;
			}
			orderController.SetOrder(OrderType.Charge);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000225C File Offset: 0x0000045C
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			OrderType activeMovementOrderOf = OrderController.GetActiveMovementOrderOf(formation);
			return new bool?(activeMovementOrderOf == OrderType.Charge || activeMovementOrderOf == OrderType.ChargeWithTarget);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002280 File Offset: 0x00000480
		public override bool IsTargeted()
		{
			return true;
		}
	}
}
