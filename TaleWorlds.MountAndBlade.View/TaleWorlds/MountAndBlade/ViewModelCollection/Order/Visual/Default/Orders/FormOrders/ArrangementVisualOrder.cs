using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.FormOrders
{
	// Token: 0x0200000C RID: 12
	public class ArrangementVisualOrder : VisualOrder
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000023C1 File Offset: 0x000005C1
		public ArrangementOrder.ArrangementOrderEnum ArrangementOrder { get; }

		// Token: 0x06000030 RID: 48 RVA: 0x000023C9 File Offset: 0x000005C9
		public ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum arrangementOrder, string iconId)
			: base(iconId)
		{
			this.ArrangementOrder = arrangementOrder;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000023DC File Offset: 0x000005DC
		public override TextObject GetName(OrderController orderController)
		{
			switch (this.ArrangementOrder)
			{
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.Circle:
				return new TextObject("{=9TGLirQf}Circle", null);
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.Column:
				return new TextObject("{=WsmZzaOq}Column", null);
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.Line:
				return new TextObject("{=9aboazgu}Line", null);
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.Loose:
				return new TextObject("{=iJXH3841}Loose", null);
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.Scatter:
				return new TextObject("{=eEf7hE4r}Scatter", null);
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.ShieldWall:
				return new TextObject("{=rTPnyeJ3}Shield Wall", null);
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.Skein:
				return new TextObject("{=uCyQNvq1}Skein", null);
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.Square:
				return new TextObject("{=squareOrder}Square", null);
			default:
				return TextObject.GetEmpty();
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002480 File Offset: 0x00000680
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			ArrangementOrder arrangementOrder = new ArrangementOrder(this.ArrangementOrder);
			orderController.SetOrder(arrangementOrder.OrderType);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000024A7 File Offset: 0x000006A7
		public override bool IsTargeted()
		{
			return false;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000024AA File Offset: 0x000006AA
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(formation.ArrangementOrder.OrderEnum == this.ArrangementOrder);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000024C4 File Offset: 0x000006C4
		private static OrderType GetArrangementOrderType(ArrangementOrder.ArrangementOrderEnum arrangementOrderEnum)
		{
			switch (arrangementOrderEnum)
			{
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.Circle:
				return OrderType.ArrangementCircular;
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.Column:
				return OrderType.ArrangementColumn;
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.Line:
				return OrderType.ArrangementLine;
			case TaleWorlds.MountAndBlade.ArrangementOrder.ArrangementOrderEnum.Loose:
				return OrderType.ArrangementLine;
			}
			Debug.FailedAssert("Failed to find arrangement order type: " + arrangementOrderEnum, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\VisualOrders\\Orders\\FormOrders\\ArrangementVisualOrder.cs", "GetArrangementOrderType", 78);
			return OrderType.None;
		}
	}
}
