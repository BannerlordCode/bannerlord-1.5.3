using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.MovementOrders
{
	// Token: 0x0200000A RID: 10
	public class RetreatVisualOrder : VisualOrder
	{
		// Token: 0x06000025 RID: 37 RVA: 0x0000235B File Offset: 0x0000055B
		public RetreatVisualOrder(string iconId)
			: base(iconId)
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002364 File Offset: 0x00000564
		public override TextObject GetName(OrderController orderController)
		{
			return new TextObject("{=VbeHEAsa}Retreat", null);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002371 File Offset: 0x00000571
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			orderController.SetOrder(OrderType.Retreat);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000237B File Offset: 0x0000057B
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(OrderController.GetActiveMovementOrderOf(formation) == OrderType.Retreat);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000238C File Offset: 0x0000058C
		public override bool IsTargeted()
		{
			return false;
		}
	}
}
