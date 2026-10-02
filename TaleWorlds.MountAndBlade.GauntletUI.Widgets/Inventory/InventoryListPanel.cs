using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000146 RID: 326
	public class InventoryListPanel : NavigatableListPanel
	{
		// Token: 0x06001125 RID: 4389 RVA: 0x0002F3A4 File Offset: 0x0002D5A4
		public InventoryListPanel(UIContext context)
			: base(context)
		{
			this._sortByTypeClickHandler = new Action<Widget>(this.OnSortByType);
			this._sortByNameClickHandler = new Action<Widget>(this.OnSortByName);
			this._sortByQuantityClickHandler = new Action<Widget>(this.OnSortByQuantity);
			this._sortByCostClickHandler = new Action<Widget>(this.OnSortByCost);
			base.ClearSelectedOnRemoval = true;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0002F407 File Offset: 0x0002D607
		private void OnSortByType(Widget widget)
		{
			base.RefreshChildNavigationIndices();
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0002F40F File Offset: 0x0002D60F
		private void OnSortByName(Widget widget)
		{
			base.RefreshChildNavigationIndices();
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0002F417 File Offset: 0x0002D617
		private void OnSortByQuantity(Widget widget)
		{
			base.RefreshChildNavigationIndices();
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0002F41F File Offset: 0x0002D61F
		private void OnSortByCost(Widget widget)
		{
			base.RefreshChildNavigationIndices();
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x0600112A RID: 4394 RVA: 0x0002F427 File Offset: 0x0002D627
		// (set) Token: 0x0600112B RID: 4395 RVA: 0x0002F430 File Offset: 0x0002D630
		[Editor(false)]
		public ButtonWidget SortByTypeBtn
		{
			get
			{
				return this._sortByTypeBtn;
			}
			set
			{
				if (this._sortByTypeBtn != value)
				{
					if (this._sortByTypeBtn != null)
					{
						this._sortByTypeBtn.ClickEventHandlers.Remove(this._sortByTypeClickHandler);
					}
					this._sortByTypeBtn = value;
					if (this._sortByTypeBtn != null)
					{
						this._sortByTypeBtn.ClickEventHandlers.Add(this._sortByTypeClickHandler);
					}
					base.OnPropertyChanged<ButtonWidget>(value, "SortByTypeBtn");
				}
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x0002F496 File Offset: 0x0002D696
		// (set) Token: 0x0600112D RID: 4397 RVA: 0x0002F4A0 File Offset: 0x0002D6A0
		[Editor(false)]
		public ButtonWidget SortByNameBtn
		{
			get
			{
				return this._sortByNameBtn;
			}
			set
			{
				if (this._sortByNameBtn != value)
				{
					if (this._sortByNameBtn != null)
					{
						this._sortByNameBtn.ClickEventHandlers.Remove(this._sortByNameClickHandler);
					}
					this._sortByNameBtn = value;
					if (this._sortByNameBtn != null)
					{
						this._sortByNameBtn.ClickEventHandlers.Add(this._sortByNameClickHandler);
					}
					base.OnPropertyChanged<ButtonWidget>(value, "SortByNameBtn");
				}
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x0002F506 File Offset: 0x0002D706
		// (set) Token: 0x0600112F RID: 4399 RVA: 0x0002F510 File Offset: 0x0002D710
		[Editor(false)]
		public ButtonWidget SortByQuantityBtn
		{
			get
			{
				return this._sortByQuantityBtn;
			}
			set
			{
				if (this._sortByQuantityBtn != value)
				{
					if (this._sortByQuantityBtn != null)
					{
						this._sortByQuantityBtn.ClickEventHandlers.Remove(this._sortByQuantityClickHandler);
					}
					this._sortByQuantityBtn = value;
					if (this._sortByQuantityBtn != null)
					{
						this._sortByQuantityBtn.ClickEventHandlers.Add(this._sortByQuantityClickHandler);
					}
					base.OnPropertyChanged<ButtonWidget>(value, "SortByQuantityBtn");
				}
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001130 RID: 4400 RVA: 0x0002F576 File Offset: 0x0002D776
		// (set) Token: 0x06001131 RID: 4401 RVA: 0x0002F580 File Offset: 0x0002D780
		[Editor(false)]
		public ButtonWidget SortByCostBtn
		{
			get
			{
				return this._sortByCostBtn;
			}
			set
			{
				if (this._sortByCostBtn != value)
				{
					if (this._sortByCostBtn != null)
					{
						this._sortByCostBtn.ClickEventHandlers.Remove(this._sortByCostClickHandler);
					}
					this._sortByCostBtn = value;
					if (this._sortByCostBtn != null)
					{
						this._sortByCostBtn.ClickEventHandlers.Remove(this._sortByCostClickHandler);
					}
					base.OnPropertyChanged<ButtonWidget>(value, "SortByCostBtn");
				}
			}
		}

		// Token: 0x040007C4 RID: 1988
		private Action<Widget> _sortByTypeClickHandler;

		// Token: 0x040007C5 RID: 1989
		private Action<Widget> _sortByNameClickHandler;

		// Token: 0x040007C6 RID: 1990
		private Action<Widget> _sortByQuantityClickHandler;

		// Token: 0x040007C7 RID: 1991
		private Action<Widget> _sortByCostClickHandler;

		// Token: 0x040007C8 RID: 1992
		private ButtonWidget _sortByTypeBtn;

		// Token: 0x040007C9 RID: 1993
		private ButtonWidget _sortByNameBtn;

		// Token: 0x040007CA RID: 1994
		private ButtonWidget _sortByQuantityBtn;

		// Token: 0x040007CB RID: 1995
		private ButtonWidget _sortByCostBtn;
	}
}
