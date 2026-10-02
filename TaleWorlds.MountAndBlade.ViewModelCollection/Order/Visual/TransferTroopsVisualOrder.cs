using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x02000026 RID: 38
	public class TransferTroopsVisualOrder : VisualOrder
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600032E RID: 814 RVA: 0x0000BD84 File Offset: 0x00009F84
		// (remove) Token: 0x0600032F RID: 815 RVA: 0x0000BDB8 File Offset: 0x00009FB8
		public static event Action OnTransferStarted;

		// Token: 0x06000330 RID: 816 RVA: 0x0000BDEB File Offset: 0x00009FEB
		public TransferTroopsVisualOrder()
			: base("order_toggle_transfer")
		{
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000BDF8 File Offset: 0x00009FF8
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			Action onTransferStarted = TransferTroopsVisualOrder.OnTransferStarted;
			if (onTransferStarted == null)
			{
				return;
			}
			onTransferStarted();
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000BE09 File Offset: 0x0000A009
		public override TextObject GetName(OrderController orderController)
		{
			return new TextObject("{=AmbKQ7LT}Transfer", null);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000BE16 File Offset: 0x0000A016
		public override bool IsTargeted()
		{
			return false;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000BE19 File Offset: 0x0000A019
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(false);
		}
	}
}
