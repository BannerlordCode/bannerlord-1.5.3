using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000064 RID: 100
	public class PartyHeaderToggleWidget : ToggleButtonWidget
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x000103FC File Offset: 0x0000E5FC
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x00010404 File Offset: 0x0000E604
		public bool AutoToggleTransferButtonState { get; set; } = true;

		// Token: 0x06000562 RID: 1378 RVA: 0x0001040D File Offset: 0x0000E60D
		public PartyHeaderToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00010424 File Offset: 0x0000E624
		protected override void OnClick(Widget widget)
		{
			if (!this.BlockInputsWhenDisabled || this._listPanel == null || this._listPanel.ChildCount > 0)
			{
				base.OnClick(widget);
				this.UpdateCollapseIndicator();
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00010451 File Offset: 0x0000E651
		private void OnListSizeChange(Widget widget)
		{
			this.UpdateSize();
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00010459 File Offset: 0x0000E659
		private void OnListSizeChange(Widget parentWidget, Widget addedWidget)
		{
			this.UpdateSize();
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00010461 File Offset: 0x0000E661
		public override void SetState(string stateName)
		{
			if (!this.BlockInputsWhenDisabled || this._listPanel == null || this._listPanel.ChildCount > 0)
			{
				base.SetState(stateName);
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00010488 File Offset: 0x0000E688
		private void UpdateSize()
		{
			if (this.TransferButtonWidget != null && this.AutoToggleTransferButtonState)
			{
				this.TransferButtonWidget.IsEnabled = this._listPanel.ChildCount > 0;
			}
			if (this.IsRelevant)
			{
				base.IsVisible = true;
				if (this._listPanel.ChildCount > 0)
				{
					this._listPanel.IsVisible = true;
				}
				if (this._listPanel.ChildCount > this._latestChildCount && !base.WidgetToClose.IsVisible)
				{
					this.HandleClick();
				}
			}
			else
			{
				this._listPanel.IsVisible = false;
			}
			this._latestChildCount = this._listPanel.ChildCount;
			this.UpdateCollapseIndicator();
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00010534 File Offset: 0x0000E734
		private void ListPanelUpdated()
		{
			if (this.TransferButtonWidget != null)
			{
				this.TransferButtonWidget.IsEnabled = false;
			}
			this._listPanel.ItemAfterRemoveEventHandlers.Add(new Action<Widget>(this.OnListSizeChange));
			this._listPanel.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnListSizeChange));
			this.UpdateSize();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00010593 File Offset: 0x0000E793
		private void TransferButtonUpdated()
		{
			this.TransferButtonWidget.IsEnabled = false;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x000105A1 File Offset: 0x0000E7A1
		private void CollapseIndicatorUpdated()
		{
			this.CollapseIndicator.AddState("Collapsed");
			this.CollapseIndicator.AddState("Expanded");
			this.UpdateCollapseIndicator();
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x000105C9 File Offset: 0x0000E7C9
		private void UpdateCollapseIndicator()
		{
			if (base.WidgetToClose != null && this.CollapseIndicator != null)
			{
				if (base.WidgetToClose.IsVisible)
				{
					this.CollapseIndicator.SetState("Expanded");
					return;
				}
				this.CollapseIndicator.SetState("Collapsed");
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00010609 File Offset: 0x0000E809
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x00010611 File Offset: 0x0000E811
		[Editor(false)]
		public ListPanel ListPanel
		{
			get
			{
				return this._listPanel;
			}
			set
			{
				if (this._listPanel != value)
				{
					this._listPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "ListPanel");
					this.ListPanelUpdated();
				}
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x00010635 File Offset: 0x0000E835
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x0001063D File Offset: 0x0000E83D
		[Editor(false)]
		public ButtonWidget TransferButtonWidget
		{
			get
			{
				return this._transferButtonWidget;
			}
			set
			{
				if (this._transferButtonWidget != value)
				{
					this._transferButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "TransferButtonWidget");
					this.TransferButtonUpdated();
				}
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00010661 File Offset: 0x0000E861
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x00010669 File Offset: 0x0000E869
		[Editor(false)]
		public BrushWidget CollapseIndicator
		{
			get
			{
				return this._collapseIndicator;
			}
			set
			{
				if (this._collapseIndicator != value)
				{
					this._collapseIndicator = value;
					base.OnPropertyChanged<BrushWidget>(value, "CollapseIndicator");
					this.CollapseIndicatorUpdated();
				}
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x0001068D File Offset: 0x0000E88D
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x00010695 File Offset: 0x0000E895
		[Editor(false)]
		public bool IsRelevant
		{
			get
			{
				return this._isRelevant;
			}
			set
			{
				if (this._isRelevant != value)
				{
					this._isRelevant = value;
					if (!this._isRelevant)
					{
						base.IsVisible = false;
					}
					this.UpdateSize();
					base.OnPropertyChanged(value, "IsRelevant");
				}
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x000106C8 File Offset: 0x0000E8C8
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x000106D0 File Offset: 0x0000E8D0
		[Editor(false)]
		public bool BlockInputsWhenDisabled
		{
			get
			{
				return this._blockInputsWhenDisabled;
			}
			set
			{
				if (this._blockInputsWhenDisabled != value)
				{
					this._blockInputsWhenDisabled = value;
					base.OnPropertyChanged(value, "BlockInputsWhenDisabled");
				}
			}
		}

		// Token: 0x0400024A RID: 586
		private int _latestChildCount;

		// Token: 0x0400024C RID: 588
		private ListPanel _listPanel;

		// Token: 0x0400024D RID: 589
		private ButtonWidget _transferButtonWidget;

		// Token: 0x0400024E RID: 590
		private BrushWidget _collapseIndicator;

		// Token: 0x0400024F RID: 591
		private bool _isRelevant = true;

		// Token: 0x04000250 RID: 592
		private bool _blockInputsWhenDisabled;
	}
}
