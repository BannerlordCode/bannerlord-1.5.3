using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.ToggleOrders
{
	// Token: 0x02000004 RID: 4
	public class GenericToggleVisualOrder : VisualOrder
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
		public OrderType PositiveOrder { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002060 File Offset: 0x00000260
		public OrderType NegativeOrder { get; }

		// Token: 0x06000005 RID: 5 RVA: 0x00002068 File Offset: 0x00000268
		public GenericToggleVisualOrder(string stringId, OrderType positiveOrder, OrderType negativeOrder)
			: base(stringId)
		{
			this.PositiveOrder = positiveOrder;
			this.NegativeOrder = negativeOrder;
			this._positiveOrderName = GenericToggleVisualOrder.GetOrderName(positiveOrder);
			this._negativeOrderName = GenericToggleVisualOrder.GetOrderName(negativeOrder);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002098 File Offset: 0x00000298
		public override TextObject GetName(OrderController orderController)
		{
			OrderState activeState = base.GetActiveState(orderController);
			if (activeState == OrderState.Active || activeState == OrderState.PartiallyActive)
			{
				return this._positiveOrderName;
			}
			return this._negativeOrderName;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020C2 File Offset: 0x000002C2
		public override bool IsTargeted()
		{
			return false;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020C8 File Offset: 0x000002C8
		private static TextObject GetOrderName(OrderType orderType)
		{
			if (orderType == OrderType.LookAtEnemy)
			{
				return new TextObject("{=u8j8nN5U}Face Enemy", null);
			}
			if (orderType != OrderType.LookAtDirection)
			{
				switch (orderType)
				{
				case OrderType.HoldFire:
					return new TextObject("{=VyI0rimN}Holding Fire", null);
				case OrderType.FireAtWill:
					return new TextObject("{=itoYrj8d}Firing at will", null);
				case OrderType.Mount:
					return new TextObject("{=ubTGIdcv}Mounted", null);
				case OrderType.Dismount:
					return new TextObject("{=Ema5Vd6o}Dismounted", null);
				case OrderType.AIControlOn:
					return new TextObject("{=zatDiaEI}Delegate Command On", null);
				case OrderType.AIControlOff:
					return new TextObject("{=JceqNdWx}Delegate Command Off", null);
				}
				return TextObject.GetEmpty();
			}
			return new TextObject("{=1gC25EMb}Face this Direction", null);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000216E File Offset: 0x0000036E
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			if (base.GetActiveState(orderController) == OrderState.Active)
			{
				orderController.SetOrder(this.NegativeOrder);
				return;
			}
			orderController.SetOrder(this.PositiveOrder);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002193 File Offset: 0x00000393
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			if (VisualOrderHelper.DoesFormationHaveOrderType(formation, this.PositiveOrder))
			{
				return new bool?(true);
			}
			return new bool?(false);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021B0 File Offset: 0x000003B0
		protected override string GetIconId()
		{
			string iconId = base.GetIconId();
			if (this._lastActiveState == OrderState.Active)
			{
				return iconId + "_active";
			}
			return iconId;
		}

		// Token: 0x04000003 RID: 3
		private readonly TextObject _positiveOrderName;

		// Token: 0x04000004 RID: 4
		private readonly TextObject _negativeOrderName;
	}
}
