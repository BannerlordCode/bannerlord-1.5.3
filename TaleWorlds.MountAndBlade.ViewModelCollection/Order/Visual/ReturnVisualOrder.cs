using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x02000025 RID: 37
	public sealed class ReturnVisualOrder : VisualOrder
	{
		// Token: 0x06000329 RID: 809 RVA: 0x0000BD5D File Offset: 0x00009F5D
		public ReturnVisualOrder()
			: base("order_return")
		{
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000BD6A File Offset: 0x00009F6A
		public override TextObject GetName(OrderController orderController)
		{
			return new TextObject("{=EmVbbIUc}Return", null);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000BD77 File Offset: 0x00009F77
		public override bool IsTargeted()
		{
			return false;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000BD7A File Offset: 0x00009F7A
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000BD7C File Offset: 0x00009F7C
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(false);
		}
	}
}
