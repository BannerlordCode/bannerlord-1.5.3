using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000148 RID: 328
	public class InventoryTwoWaySliderWidget : TwoWaySliderWidget
	{
		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001163 RID: 4451 RVA: 0x00030167 File Offset: 0x0002E367
		// (set) Token: 0x06001164 RID: 4452 RVA: 0x0003016F File Offset: 0x0002E36F
		public bool IsExtended
		{
			get
			{
				return this._isExtended;
			}
			set
			{
				if (this._isExtended != value)
				{
					this.CheckFillerState();
					this._isExtended = value;
				}
			}
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00030187 File Offset: 0x0002E387
		public InventoryTwoWaySliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00030190 File Offset: 0x0002E390
		protected override void OnParallelUpdate(float dt)
		{
			if (this._initFiller == null && base.Filler != null)
			{
				this._initFiller = base.Filler;
			}
			if (this.IsExtended)
			{
				base.OnParallelUpdate(dt);
				this.CheckFillerState();
			}
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x000301C4 File Offset: 0x0002E3C4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._isBeingDragged && !base.IsPressed)
			{
				Widget handle = base.Handle;
				if (handle != null && !handle.IsPressed)
				{
					this._shouldRemoveZeroCounts = true;
				}
			}
			bool flag;
			if (!base.IsPressed)
			{
				Widget handle2 = base.Handle;
				flag = handle2 != null && handle2.IsPressed;
			}
			else
			{
				flag = true;
			}
			this._isBeingDragged = flag;
			if (this._shouldRemoveZeroCounts)
			{
				base.EventFired("RemoveZeroCounts", Array.Empty<object>());
				this._shouldRemoveZeroCounts = false;
			}
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00030248 File Offset: 0x0002E448
		private void CheckFillerState()
		{
			if (this._initFiller != null)
			{
				if (this.IsExtended && base.Filler == null)
				{
					base.Filler = this._initFiller;
					return;
				}
				if (!this.IsExtended && base.Filler != null)
				{
					base.Filler = null;
				}
			}
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00030286 File Offset: 0x0002E486
		private void OnStockChangeClick(Widget obj)
		{
			this._manuallyIncreased = true;
			this._shouldRemoveZeroCounts = true;
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x0600116A RID: 4458 RVA: 0x00030296 File Offset: 0x0002E496
		// (set) Token: 0x0600116B RID: 4459 RVA: 0x0003029E File Offset: 0x0002E49E
		[Editor(false)]
		public ButtonWidget IncreaseStockButtonWidget
		{
			get
			{
				return this._increaseStockButtonWidget;
			}
			set
			{
				if (this._increaseStockButtonWidget != value)
				{
					this._increaseStockButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "IncreaseStockButtonWidget");
					value.ClickEventHandlers.Add(new Action<Widget>(this.OnStockChangeClick));
				}
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x000302D3 File Offset: 0x0002E4D3
		// (set) Token: 0x0600116D RID: 4461 RVA: 0x000302DB File Offset: 0x0002E4DB
		[Editor(false)]
		public ButtonWidget DecreaseStockButtonWidget
		{
			get
			{
				return this._decreaseStockButtonWidget;
			}
			set
			{
				if (this._decreaseStockButtonWidget != value)
				{
					this._decreaseStockButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "DecreaseStockButtonWidget");
					value.ClickEventHandlers.Add(new Action<Widget>(this.OnStockChangeClick));
				}
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x00030310 File Offset: 0x0002E510
		// (set) Token: 0x0600116F RID: 4463 RVA: 0x00030318 File Offset: 0x0002E518
		[Editor(false)]
		public bool IsRightSide
		{
			get
			{
				return this._isRightSide;
			}
			set
			{
				if (this._isRightSide != value)
				{
					this._isRightSide = value;
					base.OnPropertyChanged(value, "IsRightSide");
				}
			}
		}

		// Token: 0x040007E7 RID: 2023
		private bool _isExtended;

		// Token: 0x040007E8 RID: 2024
		private Widget _initFiller;

		// Token: 0x040007E9 RID: 2025
		private bool _isBeingDragged;

		// Token: 0x040007EA RID: 2026
		private bool _shouldRemoveZeroCounts;

		// Token: 0x040007EB RID: 2027
		private ButtonWidget _increaseStockButtonWidget;

		// Token: 0x040007EC RID: 2028
		private ButtonWidget _decreaseStockButtonWidget;

		// Token: 0x040007ED RID: 2029
		private bool _isRightSide;
	}
}
