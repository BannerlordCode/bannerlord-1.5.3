using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x0200001F RID: 31
	public class OrderSetVM : OrderItemBaseVM
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060002B9 RID: 697 RVA: 0x0000ACF8 File Offset: 0x00008EF8
		// (remove) Token: 0x060002BA RID: 698 RVA: 0x0000AD2C File Offset: 0x00008F2C
		public static event OrderSetVM.OnOrderSetSelectionStateChangedDelegate OnSelectionStateChanged;

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000AD5F File Offset: 0x00008F5F
		public bool HasSingleOrder
		{
			get
			{
				return this.SoloOrder != null;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000AD6A File Offset: 0x00008F6A
		public VisualOrderSet OrderSet { get; }

		// Token: 0x060002BD RID: 701 RVA: 0x0000AD72 File Offset: 0x00008F72
		public OrderSetVM(OrderController orderController, VisualOrderSet collection)
			: base(orderController)
		{
			this.OrderSet = collection;
			this.Orders = new MBBindingList<OrderItemVM>();
			this.RefreshOrders();
			this.RefreshValues();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000AD99 File Offset: 0x00008F99
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.Name = this.OrderSet.GetName(this._orderController).ToString();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000ADC0 File Offset: 0x00008FC0
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this.SoloOrder != null)
			{
				this.SoloOrder = null;
			}
			for (int i = 0; i < this.Orders.Count; i++)
			{
				this.Orders[i].OnFinalize();
			}
			InputKeyItemVM shortcutKey = base.ShortcutKey;
			if (shortcutKey == null)
			{
				return;
			}
			shortcutKey.OnFinalize();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000AE19 File Offset: 0x00009019
		protected override void OnExecuteAction(VisualOrderExecutionParameters executionParameters)
		{
			if (this.OrderSet.IsSoloOrder)
			{
				this.SoloOrder.ExecuteAction(executionParameters);
			}
			else
			{
				OrderSetVM.OnOrderSetSelectionStateChangedDelegate onSelectionStateChanged = OrderSetVM.OnSelectionStateChanged;
				if (onSelectionStateChanged != null)
				{
					onSelectionStateChanged(this, true);
				}
			}
			this.RefreshOrderStates();
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000AE50 File Offset: 0x00009050
		protected override void OnRefreshState()
		{
			base.Name = this.OrderSet.GetName(this._orderController).ToString();
			if (this.OrderSet.IsSoloOrder)
			{
				OrderState activeState = this.OrderSet.SoloOrder.GetActiveState(this._orderController);
				base.SelectionState = activeState.ToString();
				base.IsActive = activeState == OrderState.Active;
				return;
			}
			base.IsActive = false;
			base.SelectionState = OrderState.Default.ToString();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000AED7 File Offset: 0x000090D7
		public void ExecuteSelect()
		{
			OrderSetVM.OnOrderSetSelectionStateChangedDelegate onSelectionStateChanged = OrderSetVM.OnSelectionStateChanged;
			if (onSelectionStateChanged == null)
			{
				return;
			}
			onSelectionStateChanged(this, true);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000AEEA File Offset: 0x000090EA
		public void ExecuteDeSelect()
		{
			OrderSetVM.OnOrderSetSelectionStateChangedDelegate onSelectionStateChanged = OrderSetVM.OnSelectionStateChanged;
			if (onSelectionStateChanged == null)
			{
				return;
			}
			onSelectionStateChanged(this, false);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000AEFD File Offset: 0x000090FD
		public void OnOrderExecuted(OrderItemVM order)
		{
			this.RefreshOrderStates();
			this.RefreshValues();
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000AF0C File Offset: 0x0000910C
		public void RefreshOrders()
		{
			this.Orders.Clear();
			if (this.SoloOrder != null)
			{
				this.SoloOrder = null;
			}
			if (this.OrderSet != null)
			{
				MBReadOnlyList<VisualOrder> orders = this.OrderSet.Orders;
				for (int i = 0; i < orders.Count; i++)
				{
					this.Orders.Add(new OrderItemVM(this._orderController, orders[i]));
				}
				if (this.OrderSet.IsSoloOrder)
				{
					this.SoloOrder = this.Orders[0];
				}
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000AF94 File Offset: 0x00009194
		protected override void OnSelectedStateChanged(bool isSelected)
		{
			base.OnSelectedStateChanged(isSelected);
			OrderSetVM.OnOrderSetSelectionStateChangedDelegate onSelectionStateChanged = OrderSetVM.OnSelectionStateChanged;
			if (onSelectionStateChanged != null)
			{
				onSelectionStateChanged(this, isSelected);
			}
			if (this.SoloOrder != null)
			{
				this.SoloOrder.IsSelected = isSelected;
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000AFC4 File Offset: 0x000091C4
		public void RefreshOrderStates()
		{
			base.OrderIconId = this.OrderSet.IconId;
			base.RefreshState();
			for (int i = 0; i < this.Orders.Count; i++)
			{
				this.Orders[i].RefreshState();
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000B010 File Offset: 0x00009210
		public void UpdateCanUseShortcuts(bool value)
		{
			base.CanUseShortcuts = value;
			for (int i = 0; i < this.Orders.Count; i++)
			{
				this.Orders[i].CanUseShortcuts = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000B04C File Offset: 0x0000924C
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000B054 File Offset: 0x00009254
		[DataSourceProperty]
		public string SelectedOrderText
		{
			get
			{
				return this._selectedOrderText;
			}
			set
			{
				if (value != this._selectedOrderText)
				{
					this._selectedOrderText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedOrderText");
				}
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000B077 File Offset: 0x00009277
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000B07F File Offset: 0x0000927F
		[DataSourceProperty]
		public OrderItemVM SoloOrder
		{
			get
			{
				return this._soloOrder;
			}
			set
			{
				if (value != this._soloOrder)
				{
					this._soloOrder = value;
					base.OnPropertyChangedWithValue<OrderItemVM>(value, "SoloOrder");
				}
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000B09D File Offset: 0x0000929D
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000B0A5 File Offset: 0x000092A5
		[DataSourceProperty]
		public MBBindingList<OrderItemVM> Orders
		{
			get
			{
				return this._orders;
			}
			set
			{
				if (value != this._orders)
				{
					this._orders = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderItemVM>>(value, "Orders");
				}
			}
		}

		// Token: 0x04000139 RID: 313
		private string _selectedOrderText;

		// Token: 0x0400013A RID: 314
		private OrderItemVM _soloOrder;

		// Token: 0x0400013B RID: 315
		private MBBindingList<OrderItemVM> _orders;

		// Token: 0x020000C1 RID: 193
		// (Invoke) Token: 0x06000C3C RID: 3132
		public delegate void OnOrderSetSelectionStateChangedDelegate(OrderSetVM orderSet, bool isSelected);
	}
}
