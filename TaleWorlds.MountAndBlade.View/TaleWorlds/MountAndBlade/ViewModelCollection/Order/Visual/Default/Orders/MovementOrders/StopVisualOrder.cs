using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.MovementOrders
{
	// Token: 0x0200000B RID: 11
	public class StopVisualOrder : VisualOrder
	{
		// Token: 0x0600002A RID: 42 RVA: 0x0000238F File Offset: 0x0000058F
		public StopVisualOrder(string iconId)
			: base(iconId)
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002398 File Offset: 0x00000598
		public override TextObject GetName(OrderController orderController)
		{
			return new TextObject("{=QTr6UDAa}Stop", null);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000023A5 File Offset: 0x000005A5
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			orderController.SetOrder(OrderType.StandYourGround);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000023AE File Offset: 0x000005AE
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(OrderController.GetActiveMovementOrderOf(formation) == OrderType.StandYourGround);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000023BE File Offset: 0x000005BE
		public override bool IsTargeted()
		{
			return false;
		}
	}
}
