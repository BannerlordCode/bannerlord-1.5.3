using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;

namespace TaleWorlds.MountAndBlade.View.VisualOrders.OrderSets
{
	// Token: 0x0200002D RID: 45
	public class GenericVisualOrderSet : VisualOrderSet
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000090B8 File Offset: 0x000072B8
		public override bool IsSoloOrder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000090BB File Offset: 0x000072BB
		public override string StringId
		{
			get
			{
				return this._stringId;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000090C4 File Offset: 0x000072C4
		public override string IconId
		{
			get
			{
				if (this._useActiveOrderForIconId)
				{
					for (int i = 0; i < base.Orders.Count; i++)
					{
						if (base.Orders[i].GetActiveState(Mission.Current.PlayerTeam.PlayerOrderController) == OrderState.Active)
						{
							return base.Orders[i].IconId;
						}
					}
				}
				return this._stringId;
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000912C File Offset: 0x0000732C
		public override TextObject GetName(OrderController orderController)
		{
			if (this._useActiveOrderForName)
			{
				for (int i = 0; i < base.Orders.Count; i++)
				{
					if (base.Orders[i].GetActiveState(orderController) == OrderState.Active)
					{
						return base.Orders[i].GetName(orderController);
					}
				}
			}
			return this._name;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00009185 File Offset: 0x00007385
		public GenericVisualOrderSet(string stringId, TextObject name, bool useActiveOrderForIconId, bool useActiveOrderForName)
		{
			this._stringId = stringId;
			this._name = name;
			this._useActiveOrderForIconId = useActiveOrderForIconId;
			this._useActiveOrderForName = useActiveOrderForName;
		}

		// Token: 0x04000058 RID: 88
		private readonly TextObject _name;

		// Token: 0x04000059 RID: 89
		private readonly string _stringId;

		// Token: 0x0400005A RID: 90
		private readonly bool _useActiveOrderForIconId;

		// Token: 0x0400005B RID: 91
		private readonly bool _useActiveOrderForName;
	}
}
