using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.MovementOrders
{
	// Token: 0x02000005 RID: 5
	public class AdvanceVisualOrder : VisualOrder
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000021DA File Offset: 0x000003DA
		public AdvanceVisualOrder(string iconId)
			: base(iconId)
		{
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021E3 File Offset: 0x000003E3
		public override TextObject GetName(OrderController orderController)
		{
			return new TextObject("{=A38xbjqm}Engage", null);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021F0 File Offset: 0x000003F0
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			if (executionParameters.HasFormation)
			{
				orderController.SetOrderWithFormation(OrderType.Advance, executionParameters.Formation);
				return;
			}
			orderController.SetOrder(OrderType.Advance);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002211 File Offset: 0x00000411
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(OrderController.GetActiveMovementOrderOf(formation) == OrderType.Advance);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002222 File Offset: 0x00000422
		public override bool IsTargeted()
		{
			return true;
		}
	}
}
