using System;
using System.Linq;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000147 RID: 327
	public class InventoryScreenWidget : Widget
	{
		// Token: 0x06001132 RID: 4402 RVA: 0x0002F5E7 File Offset: 0x0002D7E7
		public InventoryScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0002F60C File Offset: 0x0002D80C
		private T IsWidgetChildOfType<T>(Widget currentWidget) where T : Widget
		{
			while (currentWidget != null)
			{
				if (currentWidget is T)
				{
					return (T)((object)currentWidget);
				}
				currentWidget = currentWidget.ParentWidget;
			}
			return default(T);
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x0002F63E File Offset: 0x0002D83E
		private bool IsWidgetChildOf(Widget parentWidget, Widget currentWidget)
		{
			while (currentWidget != null)
			{
				if (currentWidget == parentWidget)
				{
					return true;
				}
				currentWidget = currentWidget.ParentWidget;
			}
			return false;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x0002F654 File Offset: 0x0002D854
		private bool IsWidgetChildOfId(string parentId, Widget currentWidget)
		{
			while (currentWidget != null)
			{
				if (currentWidget.Id == parentId)
				{
					return true;
				}
				currentWidget = currentWidget.ParentWidget;
			}
			return false;
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x0002F674 File Offset: 0x0002D874
		private InventoryListPanel GetCurrentHoveredListPanel()
		{
			for (int i = 0; i < base.EventManager.MouseOveredWidgets.Count; i++)
			{
				InventoryListPanel inventoryListPanel;
				if ((inventoryListPanel = base.EventManager.MouseOveredWidgets[i] as InventoryListPanel) != null)
				{
					return inventoryListPanel;
				}
			}
			return null;
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0002F6B9 File Offset: 0x0002D8B9
		private Widget GetFirstBannerItem()
		{
			ListPanel listPanel = this.OtherInventoryListWidget.InnerPanel as ListPanel;
			ListPanel listPanel2 = ((listPanel != null) ? listPanel.GetChild(0) : null) as ListPanel;
			if (listPanel2 == null)
			{
				return null;
			}
			return listPanel2.FindChild((Widget x) => (x as InventoryItemTupleWidget).ItemType == this.BannerTypeName);
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0002F6F4 File Offset: 0x0002D8F4
		private Widget GetItemWithId(ScrollablePanel listWidget, string id)
		{
			ListPanel listPanel = listWidget.InnerPanel as ListPanel;
			ListPanel listPanel2 = ((listPanel != null) ? listPanel.GetChild(0) : null) as ListPanel;
			if (listPanel2 == null)
			{
				return null;
			}
			return listPanel2.FindChild((Widget x) => (x as InventoryItemTupleWidget).ItemID == id);
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0002F744 File Offset: 0x0002D944
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.EventManager.DraggedWidget == null)
			{
				this.TargetEquipmentIndex = -1;
				this._currentDraggedItemWidget = null;
			}
			if (this._latestMouseDownWidget != base.EventManager.LatestMouseDownWidget)
			{
				this._latestMouseDownWidget = base.EventManager.LatestMouseDownWidget;
				bool flag;
				if (this._latestMouseDownWidget != null)
				{
					if (!(this._latestMouseDownWidget is InventoryItemButtonWidget) && !(this._latestMouseDownWidget is InventoryEquippedItemControlsBrushWidget))
					{
						flag = this._latestMouseDownWidget.GetAllParents().Any<Widget>((Widget x) => x is InventoryItemButtonWidget || x is InventoryEquippedItemControlsBrushWidget);
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				bool flag3 = this.IsWidgetChildOf(this.InventoryTooltip, this._latestMouseDownWidget);
				if (this._latestMouseDownWidget == null || (!flag2 && !flag3 && !this.ItemPreviewWidget.IsVisible))
				{
					base.EventFired("OnEmptyClick", Array.Empty<object>());
				}
			}
			Widget hoveredWidget = base.EventManager.HoveredWidget;
			if (hoveredWidget != null)
			{
				InventoryItemButtonWidget inventoryItemButtonWidget = this.IsWidgetChildOfType<InventoryItemButtonWidget>(hoveredWidget);
				bool flag4 = this.IsWidgetChildOfId("InventoryTooltip", hoveredWidget);
				if (inventoryItemButtonWidget != null)
				{
					this.ItemWidgetHoverBegin(inventoryItemButtonWidget);
				}
				else if (flag4 && GauntletGamepadNavigationManager.Instance.IsCursorMovingForNavigation)
				{
					this.ItemWidgetHoverEnd(null);
				}
				else if (!flag4 && hoveredWidget.ParentWidget != null)
				{
					this.ItemWidgetHoverEnd(null);
				}
			}
			else
			{
				this.ItemWidgetHoverEnd(null);
			}
			this.UpdateControllerTransferKeyVisuals();
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0002F89C File Offset: 0x0002DA9C
		private void UpdateControllerTransferKeyVisuals()
		{
			InventoryListPanel currentHoveredListPanel = this.GetCurrentHoveredListPanel();
			this.IsFocusedOnItemList = currentHoveredListPanel != null;
			if (!base.EventManager.IsControllerActive || !this.IsFocusedOnItemList)
			{
				this.PreviousCharacterInputVisualParent.IsVisible = true;
				this.NextCharacterInputVisualParent.IsVisible = true;
				this.TransferInputKeyVisualWidget.IsVisible = false;
				return;
			}
			this.PreviousCharacterInputVisualParent.IsVisible = false;
			this.NextCharacterInputVisualParent.IsVisible = false;
			InventoryItemTupleWidget inventoryItemTupleWidget;
			if ((inventoryItemTupleWidget = this._currentHoveredItemWidget as InventoryItemTupleWidget) != null && inventoryItemTupleWidget.IsHovered && inventoryItemTupleWidget.IsTransferable)
			{
				this.TransferInputKeyVisualWidget.IsVisible = true;
				Vector2 vector;
				if (inventoryItemTupleWidget.IsRightSide)
				{
					InputKeyVisualWidget transferInputKeyVisualWidget = this.TransferInputKeyVisualWidget;
					InputKeyVisualWidget nextCharacterInputKeyVisual = this._nextCharacterInputKeyVisual;
					transferInputKeyVisualWidget.KeyID = ((nextCharacterInputKeyVisual != null) ? nextCharacterInputKeyVisual.KeyID : null) ?? "";
					vector = this._currentHoveredItemWidget.GlobalPosition - new Vector2(0f, 20f * base._scaleToUse);
				}
				else
				{
					InputKeyVisualWidget transferInputKeyVisualWidget2 = this.TransferInputKeyVisualWidget;
					InputKeyVisualWidget previousCharacterInputKeyVisual = this._previousCharacterInputKeyVisual;
					transferInputKeyVisualWidget2.KeyID = ((previousCharacterInputKeyVisual != null) ? previousCharacterInputKeyVisual.KeyID : null) ?? "";
					vector = this._currentHoveredItemWidget.GlobalPosition - new Vector2(60f * base._scaleToUse - this._currentHoveredItemWidget.Size.X, 20f * base._scaleToUse);
				}
				this.TransferInputKeyVisualWidget.ScaledPositionXOffset = vector.X;
				this.TransferInputKeyVisualWidget.ScaledPositionYOffset = vector.Y;
				return;
			}
			this.TransferInputKeyVisualWidget.IsVisible = false;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0002FA34 File Offset: 0x0002DC34
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._scrollToBannersInFrames > -1)
			{
				if (this._scrollToBannersInFrames == 0)
				{
					ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters(0f, 0f, 0f, 0f, -1f, 0.2f, 0.35f);
					this.OtherInventoryListWidget.ScrollToChild(this.GetFirstBannerItem(), autoScrollParameters);
				}
				this._scrollToBannersInFrames--;
			}
			if (this.ScrollToItem)
			{
				this._scrollToItemInSeconds = 0.2f;
				this.ScrollToItem = false;
			}
			if (this._scrollToItemInSeconds >= 0f)
			{
				this._scrollToItemInSeconds -= dt;
				if (this._scrollToItemInSeconds <= 0f)
				{
					ScrollablePanel.AutoScrollParameters autoScrollParameters2 = new ScrollablePanel.AutoScrollParameters(100f, 100f, 0f, 0f, -1f, -1f, 0.35f);
					this.OtherInventoryListWidget.ScrollToChild(this.GetItemWithId(this.OtherInventoryListWidget, this.ScrollItemId), autoScrollParameters2);
					this.PlayerInventoryListWidget.ScrollToChild(this.GetItemWithId(this.PlayerInventoryListWidget, this.ScrollItemId), autoScrollParameters2);
				}
			}
			if (this._focusLostThisFrame)
			{
				base.EventFired("OnFocusLose", Array.Empty<object>());
				this._focusLostThisFrame = false;
			}
			this.UpdateTooltipPosition();
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0002FB74 File Offset: 0x0002DD74
		private void UpdateTooltipPosition()
		{
			if (base.EventManager.DraggedWidget != null)
			{
				this.InventoryTooltip.IsHidden = true;
			}
			InventoryItemButtonWidget currentHoveredItemWidget = this._currentHoveredItemWidget;
			if (((currentHoveredItemWidget != null) ? currentHoveredItemWidget.ParentWidget : null) == null)
			{
				this._lastDisplayedTooltipItem = null;
				return;
			}
			if (this._tooltipHiddenFrameCount < this.TooltipHideFrameLength)
			{
				this._tooltipHiddenFrameCount++;
				this.InventoryTooltip.PositionXOffset = 5000f;
				this.InventoryTooltip.PositionYOffset = 5000f;
				return;
			}
			if (this._currentHoveredItemWidget.IsRightSide)
			{
				this.InventoryTooltip.ScaledPositionXOffset = this._currentHoveredItemWidget.ParentWidget.GlobalPosition.X - this.InventoryTooltip.Size.X + 10f * base._scaleToUse;
			}
			else
			{
				this.InventoryTooltip.ScaledPositionXOffset = this._currentHoveredItemWidget.ParentWidget.GlobalPosition.X + this._currentHoveredItemWidget.ParentWidget.Size.X - 10f * base._scaleToUse;
			}
			float num = base.EventManager.PageSize.Y - this.InventoryTooltip.MeasuredSize.Y;
			this.InventoryTooltip.ScaledPositionYOffset = Mathf.Clamp(this._currentHoveredItemWidget.GlobalPosition.Y, 0f, num);
			this._lastDisplayedTooltipItem = this._currentHoveredItemWidget;
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x0002FCDA File Offset: 0x0002DEDA
		private void TradeLabelOnPropertyChanged(PropertyOwnerObject owner, string propertyName, object value)
		{
			if (propertyName == "Text")
			{
				this.TradeLabel.IsDisabled = string.IsNullOrEmpty(this.TradeLabel.Text);
			}
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x0002FD04 File Offset: 0x0002DF04
		private void ItemWidgetHoverBegin(InventoryItemButtonWidget itemWidget)
		{
			if (this._currentHoveredItemWidget != itemWidget)
			{
				this._currentHoveredItemWidget = itemWidget;
				this._tooltipHiddenFrameCount = 0;
				Widget widget = this.InventoryTooltip.FindChild("TargetItemTooltip");
				if (this._currentHoveredItemWidget.IsRightSide)
				{
					widget.SetSiblingIndex(1, false);
				}
				else
				{
					widget.SetSiblingIndex(0, false);
				}
				this.InventoryTooltip.IsHidden = false;
				base.EventFired("ItemHoverBegin", new object[] { itemWidget });
			}
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0002FD79 File Offset: 0x0002DF79
		private void ItemWidgetHoverEnd(InventoryItemButtonWidget itemWidget)
		{
			if (this._currentHoveredItemWidget != null && itemWidget == null)
			{
				this._currentHoveredItemWidget = null;
				this.InventoryTooltip.IsHidden = true;
				base.EventFired("ItemHoverEnd", Array.Empty<object>());
			}
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x0002FDAC File Offset: 0x0002DFAC
		public void ItemWidgetDragBegin(InventoryItemButtonWidget itemWidget)
		{
			base.EventFired("OnEmptyClick", Array.Empty<object>());
			this._currentDraggedItemWidget = itemWidget;
			InventoryEquippedItemSlotWidget inventoryEquippedItemSlotWidget = itemWidget as InventoryEquippedItemSlotWidget;
			if (inventoryEquippedItemSlotWidget != null)
			{
				this.TargetEquipmentIndex = inventoryEquippedItemSlotWidget.TargetEquipmentIndex;
				return;
			}
			this.TargetEquipmentIndex = itemWidget.EquipmentIndex;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0002FDF3 File Offset: 0x0002DFF3
		public void ItemWidgetDrop(InventoryItemButtonWidget itemWidget)
		{
			if (this._currentDraggedItemWidget == itemWidget)
			{
				this._currentDraggedItemWidget = null;
				this.TargetEquipmentIndex = -1;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x0002FE0C File Offset: 0x0002E00C
		// (set) Token: 0x06001143 RID: 4419 RVA: 0x0002FE14 File Offset: 0x0002E014
		[Editor(false)]
		public InputKeyVisualWidget TransferInputKeyVisualWidget
		{
			get
			{
				return this._transferInputKeyVisualWidget;
			}
			set
			{
				if (this._transferInputKeyVisualWidget != value)
				{
					this._transferInputKeyVisualWidget = value;
					base.OnPropertyChanged<InputKeyVisualWidget>(value, "TransferInputKeyVisualWidget");
				}
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x0002FE32 File Offset: 0x0002E032
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x0002FE3C File Offset: 0x0002E03C
		public Widget PreviousCharacterInputVisualParent
		{
			get
			{
				return this._previousCharacterInputVisualParent;
			}
			set
			{
				if (value != this._previousCharacterInputVisualParent)
				{
					this._previousCharacterInputVisualParent = value;
					if (this._previousCharacterInputVisualParent != null)
					{
						this._previousCharacterInputKeyVisual = this._previousCharacterInputVisualParent.Children.FirstOrDefault<Widget>((Widget x) => x is InputKeyVisualWidget) as InputKeyVisualWidget;
					}
				}
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x0002FE9B File Offset: 0x0002E09B
		// (set) Token: 0x06001147 RID: 4423 RVA: 0x0002FEA4 File Offset: 0x0002E0A4
		public Widget NextCharacterInputVisualParent
		{
			get
			{
				return this._nextCharacterInputVisualParent;
			}
			set
			{
				if (value != this._nextCharacterInputVisualParent)
				{
					this._nextCharacterInputVisualParent = value;
					if (this._nextCharacterInputVisualParent != null)
					{
						this._nextCharacterInputKeyVisual = this._nextCharacterInputVisualParent.Children.FirstOrDefault<Widget>((Widget x) => x is InputKeyVisualWidget) as InputKeyVisualWidget;
					}
				}
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x0002FF03 File Offset: 0x0002E103
		// (set) Token: 0x06001149 RID: 4425 RVA: 0x0002FF0C File Offset: 0x0002E10C
		[Editor(false)]
		public RichTextWidget TradeLabel
		{
			get
			{
				return this._tradeLabel;
			}
			set
			{
				if (this._tradeLabel != value)
				{
					if (this._tradeLabel != null)
					{
						this._tradeLabel.PropertyChanged -= this.TradeLabelOnPropertyChanged;
					}
					this._tradeLabel = value;
					if (this._tradeLabel != null)
					{
						this._tradeLabel.PropertyChanged += this.TradeLabelOnPropertyChanged;
					}
					base.OnPropertyChanged<RichTextWidget>(value, "TradeLabel");
				}
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x0002FF73 File Offset: 0x0002E173
		// (set) Token: 0x0600114B RID: 4427 RVA: 0x0002FF7B File Offset: 0x0002E17B
		[Editor(false)]
		public Widget InventoryTooltip
		{
			get
			{
				return this._inventoryTooltip;
			}
			set
			{
				if (this._inventoryTooltip != value)
				{
					this._inventoryTooltip = value;
					base.OnPropertyChanged<Widget>(value, "InventoryTooltip");
				}
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x0002FF99 File Offset: 0x0002E199
		// (set) Token: 0x0600114D RID: 4429 RVA: 0x0002FFA1 File Offset: 0x0002E1A1
		[Editor(false)]
		public InventoryItemPreviewWidget ItemPreviewWidget
		{
			get
			{
				return this._itemPreviewWidget;
			}
			set
			{
				if (this._itemPreviewWidget != value)
				{
					this._itemPreviewWidget = value;
					base.OnPropertyChanged<InventoryItemPreviewWidget>(value, "ItemPreviewWidget");
				}
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x0002FFBF File Offset: 0x0002E1BF
		// (set) Token: 0x0600114F RID: 4431 RVA: 0x0002FFC7 File Offset: 0x0002E1C7
		[Editor(false)]
		public int TransactionCount
		{
			get
			{
				return this._transactionCount;
			}
			set
			{
				if (this._transactionCount != value)
				{
					this._transactionCount = value;
					base.OnPropertyChanged(value, "TransactionCount");
				}
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x0002FFE5 File Offset: 0x0002E1E5
		// (set) Token: 0x06001151 RID: 4433 RVA: 0x0002FFED File Offset: 0x0002E1ED
		[Editor(false)]
		public int EquipmentMode
		{
			get
			{
				return this._equipmentMode;
			}
			set
			{
				if (this._equipmentMode != value)
				{
					this._equipmentMode = value;
					base.OnPropertyChanged(value, "EquipmentMode");
				}
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x0003000B File Offset: 0x0002E20B
		// (set) Token: 0x06001153 RID: 4435 RVA: 0x00030013 File Offset: 0x0002E213
		[Editor(false)]
		public int TargetEquipmentIndex
		{
			get
			{
				return this._targetEquipmentIndex;
			}
			set
			{
				if (this._targetEquipmentIndex != value)
				{
					this._targetEquipmentIndex = value;
					base.OnPropertyChanged(value, "TargetEquipmentIndex");
				}
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x00030031 File Offset: 0x0002E231
		// (set) Token: 0x06001155 RID: 4437 RVA: 0x00030039 File Offset: 0x0002E239
		[Editor(false)]
		public ScrollablePanel OtherInventoryListWidget
		{
			get
			{
				return this._otherInventoryListWidget;
			}
			set
			{
				if (value != this._otherInventoryListWidget)
				{
					this._otherInventoryListWidget = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "OtherInventoryListWidget");
				}
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x00030057 File Offset: 0x0002E257
		// (set) Token: 0x06001157 RID: 4439 RVA: 0x0003005F File Offset: 0x0002E25F
		[Editor(false)]
		public ScrollablePanel PlayerInventoryListWidget
		{
			get
			{
				return this._playerInventoryListWidget;
			}
			set
			{
				if (value != this._playerInventoryListWidget)
				{
					this._playerInventoryListWidget = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "PlayerInventoryListWidget");
				}
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x0003007D File Offset: 0x0002E27D
		// (set) Token: 0x06001159 RID: 4441 RVA: 0x00030085 File Offset: 0x0002E285
		[Editor(false)]
		public bool IsFocusedOnItemList
		{
			get
			{
				return this._isFocusedOnItemList;
			}
			set
			{
				if (value != this._isFocusedOnItemList)
				{
					this._isFocusedOnItemList = value;
					base.OnPropertyChanged(value, "IsFocusedOnItemList");
				}
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x000300A3 File Offset: 0x0002E2A3
		// (set) Token: 0x0600115B RID: 4443 RVA: 0x000300AB File Offset: 0x0002E2AB
		[Editor(false)]
		public bool IsBannerTutorialActive
		{
			get
			{
				return this._isBannerTutorialActive;
			}
			set
			{
				if (value != this._isBannerTutorialActive)
				{
					this._isBannerTutorialActive = value;
					base.OnPropertyChanged(value, "IsBannerTutorialActive");
					if (value)
					{
						this._scrollToBannersInFrames = 1;
					}
				}
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x000300D3 File Offset: 0x0002E2D3
		// (set) Token: 0x0600115D RID: 4445 RVA: 0x000300DB File Offset: 0x0002E2DB
		[Editor(false)]
		public string BannerTypeName
		{
			get
			{
				return this._bannerTypeName;
			}
			set
			{
				if (value != this._bannerTypeName)
				{
					this._bannerTypeName = value;
					base.OnPropertyChanged<string>(value, "BannerTypeName");
				}
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x000300FE File Offset: 0x0002E2FE
		// (set) Token: 0x0600115F RID: 4447 RVA: 0x00030106 File Offset: 0x0002E306
		[Editor(false)]
		public bool ScrollToItem
		{
			get
			{
				return this._scrollToItem;
			}
			set
			{
				if (value != this._scrollToItem)
				{
					this._scrollToItem = value;
					base.OnPropertyChanged(value, "ScrollToItem");
				}
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x00030124 File Offset: 0x0002E324
		// (set) Token: 0x06001161 RID: 4449 RVA: 0x0003012C File Offset: 0x0002E32C
		[Editor(false)]
		public string ScrollItemId
		{
			get
			{
				return this._scrollItemId;
			}
			set
			{
				if (value != this._scrollItemId)
				{
					this._scrollItemId = value;
					base.OnPropertyChanged<string>(value, "ScrollItemId");
				}
			}
		}

		// Token: 0x040007CC RID: 1996
		private readonly int TooltipHideFrameLength = 2;

		// Token: 0x040007CD RID: 1997
		private Widget _latestMouseDownWidget;

		// Token: 0x040007CE RID: 1998
		private InventoryItemButtonWidget _currentHoveredItemWidget;

		// Token: 0x040007CF RID: 1999
		private InventoryItemButtonWidget _currentDraggedItemWidget;

		// Token: 0x040007D0 RID: 2000
		private InventoryItemButtonWidget _lastDisplayedTooltipItem;

		// Token: 0x040007D1 RID: 2001
		private int _tooltipHiddenFrameCount;

		// Token: 0x040007D2 RID: 2002
		private int _scrollToBannersInFrames = -1;

		// Token: 0x040007D3 RID: 2003
		private float _scrollToItemInSeconds = -1f;

		// Token: 0x040007D4 RID: 2004
		private InputKeyVisualWidget _previousCharacterInputKeyVisual;

		// Token: 0x040007D5 RID: 2005
		private InputKeyVisualWidget _nextCharacterInputKeyVisual;

		// Token: 0x040007D6 RID: 2006
		private Widget _previousCharacterInputVisualParent;

		// Token: 0x040007D7 RID: 2007
		private Widget _nextCharacterInputVisualParent;

		// Token: 0x040007D8 RID: 2008
		private InputKeyVisualWidget _transferInputKeyVisualWidget;

		// Token: 0x040007D9 RID: 2009
		private RichTextWidget _tradeLabel;

		// Token: 0x040007DA RID: 2010
		private Widget _inventoryTooltip;

		// Token: 0x040007DB RID: 2011
		private InventoryItemPreviewWidget _itemPreviewWidget;

		// Token: 0x040007DC RID: 2012
		private int _transactionCount;

		// Token: 0x040007DD RID: 2013
		private int _equipmentMode;

		// Token: 0x040007DE RID: 2014
		private int _targetEquipmentIndex;

		// Token: 0x040007DF RID: 2015
		private ScrollablePanel _otherInventoryListWidget;

		// Token: 0x040007E0 RID: 2016
		private ScrollablePanel _playerInventoryListWidget;

		// Token: 0x040007E1 RID: 2017
		private bool _focusLostThisFrame;

		// Token: 0x040007E2 RID: 2018
		private bool _isFocusedOnItemList;

		// Token: 0x040007E3 RID: 2019
		private bool _isBannerTutorialActive;

		// Token: 0x040007E4 RID: 2020
		private bool _scrollToItem;

		// Token: 0x040007E5 RID: 2021
		private string _bannerTypeName;

		// Token: 0x040007E6 RID: 2022
		private string _scrollItemId;
	}
}
