using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.MovementOrders
{
	// Token: 0x02000008 RID: 8
	public class FollowMeVisualOrder : VisualOrder
	{
		// Token: 0x0600001B RID: 27 RVA: 0x000022B7 File Offset: 0x000004B7
		public FollowMeVisualOrder(string iconId)
			: base(iconId)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000022C0 File Offset: 0x000004C0
		public override TextObject GetName(OrderController orderController)
		{
			return new TextObject("{=5LpufKs7}Follow Me", null);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000022CD File Offset: 0x000004CD
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			orderController.SetOrderWithAgent(OrderType.FollowMe, executionParameters.Agent);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000022DC File Offset: 0x000004DC
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(OrderController.GetActiveMovementOrderOf(formation) == OrderType.FollowMe);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000022EC File Offset: 0x000004EC
		public override bool IsTargeted()
		{
			return false;
		}
	}
}
