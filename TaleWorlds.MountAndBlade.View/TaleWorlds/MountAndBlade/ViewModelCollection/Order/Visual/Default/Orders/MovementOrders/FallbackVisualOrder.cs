using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.MovementOrders
{
	// Token: 0x02000007 RID: 7
	public class FallbackVisualOrder : VisualOrder
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002283 File Offset: 0x00000483
		public FallbackVisualOrder(string iconId)
			: base(iconId)
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000228C File Offset: 0x0000048C
		public override TextObject GetName(OrderController orderController)
		{
			return new TextObject("{=WhUoF9Mw}Fallback", null);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002299 File Offset: 0x00000499
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			orderController.SetOrder(OrderType.FallBack);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000022A3 File Offset: 0x000004A3
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(OrderController.GetActiveMovementOrderOf(formation) == OrderType.FallBack);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000022B4 File Offset: 0x000004B4
		public override bool IsTargeted()
		{
			return false;
		}
	}
}
