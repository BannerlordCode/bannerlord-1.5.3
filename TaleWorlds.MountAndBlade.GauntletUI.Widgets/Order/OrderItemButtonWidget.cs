using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000071 RID: 113
	public class OrderItemButtonWidget : ButtonWidget
	{
		// Token: 0x0600061C RID: 1564 RVA: 0x0001206C File Offset: 0x0001026C
		public OrderItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00012075 File Offset: 0x00010275
		private void SelectionStateChanged()
		{
			if (!string.IsNullOrEmpty(this.SelectionState))
			{
				ImageWidget selectionVisualWidget = this.SelectionVisualWidget;
				if (selectionVisualWidget != null && selectionVisualWidget.ContainsState(this.SelectionState))
				{
					this.SelectionVisualWidget.SetState(this.SelectionState);
				}
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x000120AF File Offset: 0x000102AF
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x000120B7 File Offset: 0x000102B7
		[Editor(false)]
		public string SelectionState
		{
			get
			{
				return this._selectionState;
			}
			set
			{
				if (this._selectionState != value)
				{
					this._selectionState = value;
					base.OnPropertyChanged<string>(value, "SelectionState");
					this.SelectionStateChanged();
				}
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x000120E0 File Offset: 0x000102E0
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x000120E8 File Offset: 0x000102E8
		[Editor(false)]
		public ImageWidget SelectionVisualWidget
		{
			get
			{
				return this._selectionVisualWidget;
			}
			set
			{
				if (this._selectionVisualWidget != value)
				{
					this._selectionVisualWidget = value;
					base.OnPropertyChanged<ImageWidget>(value, "SelectionVisualWidget");
					if (value != null)
					{
						value.AddState("Disabled");
						value.AddState("PartiallyActive");
						value.AddState("Active");
					}
					this.SelectionStateChanged();
				}
			}
		}

		// Token: 0x0400029C RID: 668
		private string _selectionState;

		// Token: 0x0400029D RID: 669
		private ImageWidget _selectionVisualWidget;
	}
}
