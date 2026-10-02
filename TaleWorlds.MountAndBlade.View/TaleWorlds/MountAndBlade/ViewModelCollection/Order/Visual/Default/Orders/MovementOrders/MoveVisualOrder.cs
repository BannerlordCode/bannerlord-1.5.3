using System;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.MovementOrders
{
	// Token: 0x02000009 RID: 9
	public class MoveVisualOrder : VisualOrder
	{
		// Token: 0x06000020 RID: 32 RVA: 0x000022EF File Offset: 0x000004EF
		public MoveVisualOrder(string iconId)
			: base(iconId)
		{
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000022F8 File Offset: 0x000004F8
		public override TextObject GetName(OrderController orderController)
		{
			return new TextObject("{=vbAZwibd}Move to Position", null);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002308 File Offset: 0x00000508
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			if (executionParameters.HasWorldPosition)
			{
				WorldPosition worldPosition = executionParameters.WorldPosition;
				orderController.SetOrderWithTwoPositions(OrderType.MoveToLineSegment, worldPosition, worldPosition);
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002330 File Offset: 0x00000530
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			OrderType activeMovementOrderOf = OrderController.GetActiveMovementOrderOf(formation);
			return new bool?(activeMovementOrderOf == OrderType.Move || activeMovementOrderOf == OrderType.MoveToLineSegment || activeMovementOrderOf == OrderType.MoveToLineSegmentWithHorizontalLayout);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002358 File Offset: 0x00000558
		public override bool IsTargeted()
		{
			return false;
		}
	}
}
