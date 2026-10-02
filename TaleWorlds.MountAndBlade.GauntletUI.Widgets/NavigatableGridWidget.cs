using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200002E RID: 46
	public class NavigatableGridWidget : GridWidget
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00008503 File Offset: 0x00006703
		// (set) Token: 0x0600024D RID: 589 RVA: 0x0000850B File Offset: 0x0000670B
		public ScrollablePanel ParentPanel { get; set; }

		// Token: 0x0600024E RID: 590 RVA: 0x00008514 File Offset: 0x00006714
		public NavigatableGridWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00008530 File Offset: 0x00006730
		protected override void OnLateUpdate(float dt)
		{
			if (this._areIndicesDirty)
			{
				for (int i = 0; i < base.ChildCount; i++)
				{
					base.Children[i].GamepadNavigationIndex = -1;
				}
				this.RefreshChildNavigationIndices();
				this._areIndicesDirty = false;
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00008575 File Offset: 0x00006775
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			if (this.ParentPanel == null)
			{
				this.ParentPanel = base.FindParentPanel();
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00008594 File Offset: 0x00006794
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.OnGamepadNavigationFocusGained = new Action<Widget>(this.OnWidgetGainedGamepadFocus);
			child.EventFire += this.OnChildSiblingIndexChanged;
			child.boolPropertyChanged += this.OnChildVisibilityChanged;
			this._areIndicesDirty = true;
			this.UpdateEmptyNavigationWidget();
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000085EC File Offset: 0x000067EC
		protected override void OnAfterChildRemoved(Widget child, int previousIndexOfChild)
		{
			base.OnAfterChildRemoved(child, previousIndexOfChild);
			this._areIndicesDirty = true;
			child.OnGamepadNavigationFocusGained = null;
			child.EventFire -= this.OnChildSiblingIndexChanged;
			child.boolPropertyChanged -= this.OnChildVisibilityChanged;
			child.GamepadNavigationIndex = -1;
			this.UpdateEmptyNavigationWidget();
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00008640 File Offset: 0x00006840
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			for (int i = 0; i < base.Children.Count; i++)
			{
				base.Children[i].OnGamepadNavigationFocusGained = null;
				base.Children[i].EventFire -= this.OnChildSiblingIndexChanged;
				base.Children[i].boolPropertyChanged -= this.OnChildVisibilityChanged;
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000086B5 File Offset: 0x000068B5
		private void OnChildVisibilityChanged(PropertyOwnerObject child, string propertyName, bool value)
		{
			if (propertyName == "IsVisible")
			{
				this._areIndicesDirty = true;
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x000086CC File Offset: 0x000068CC
		private void OnWidgetGainedGamepadFocus(Widget widget)
		{
			if (this.ParentPanel != null)
			{
				ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters((float)this.AutoScrollTopOffset, (float)this.AutoScrollBottomOffset, (float)this.AutoScrollLeftOffset, (float)this.AutoScrollRightOffset, -1f, -1f, 0f);
				this.ParentPanel.ScrollToChild(widget, autoScrollParameters);
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000871F File Offset: 0x0000691F
		private void OnChildSiblingIndexChanged(Widget widget, string eventName, object[] parameters)
		{
			if (eventName == "SiblingIndexChanged")
			{
				this._areIndicesDirty = true;
			}
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00008738 File Offset: 0x00006938
		private void SetNavigationIndexForChild(Widget widget)
		{
			if (!widget.IsVisible)
			{
				widget.GamepadNavigationIndex = -1;
				return;
			}
			int num = this.MinIndex + widget.GetVisibleSiblingIndex() * this.StepSize;
			if (num <= this.MaxIndex)
			{
				widget.GamepadNavigationIndex = num;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000877A File Offset: 0x0000697A
		protected override void OnGamepadNavigationIndexUpdated(int newIndex)
		{
			if (newIndex != -1 && this.UseSelfIndexForMinimum)
			{
				this.SetNavigationIndicesFromSelf();
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000878E File Offset: 0x0000698E
		private void SetNavigationIndicesFromSelf()
		{
			this.MinIndex = base.GamepadNavigationIndex;
			base.GamepadNavigationIndex = -1;
			this._areIndicesDirty = true;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000087AA File Offset: 0x000069AA
		private void UpdateEmptyNavigationWidget()
		{
			if (this._emptyNavigationWidget != null)
			{
				if (base.Children.Count == 0)
				{
					this.EmptyNavigationWidget.GamepadNavigationIndex = this.MinIndex;
					return;
				}
				this.EmptyNavigationWidget.GamepadNavigationIndex = -1;
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000087E0 File Offset: 0x000069E0
		protected void RefreshChildNavigationIndices()
		{
			for (int i = 0; i < base.Children.Count; i++)
			{
				this.SetNavigationIndexForChild(base.Children[i]);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00008815 File Offset: 0x00006A15
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000881D File Offset: 0x00006A1D
		public int AutoScrollTopOffset { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00008826 File Offset: 0x00006A26
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000882E File Offset: 0x00006A2E
		public int AutoScrollBottomOffset { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00008837 File Offset: 0x00006A37
		// (set) Token: 0x06000261 RID: 609 RVA: 0x0000883F File Offset: 0x00006A3F
		public int AutoScrollLeftOffset { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00008848 File Offset: 0x00006A48
		// (set) Token: 0x06000263 RID: 611 RVA: 0x00008850 File Offset: 0x00006A50
		public int AutoScrollRightOffset { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00008859 File Offset: 0x00006A59
		// (set) Token: 0x06000265 RID: 613 RVA: 0x00008861 File Offset: 0x00006A61
		public int MinIndex
		{
			get
			{
				return this._minIndex;
			}
			set
			{
				if (value != this._minIndex)
				{
					this._minIndex = value;
					this.RefreshChildNavigationIndices();
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00008879 File Offset: 0x00006A79
		// (set) Token: 0x06000267 RID: 615 RVA: 0x00008881 File Offset: 0x00006A81
		public int MaxIndex
		{
			get
			{
				return this._maxIndex;
			}
			set
			{
				if (value != this._maxIndex)
				{
					this._maxIndex = value;
					this.RefreshChildNavigationIndices();
				}
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00008899 File Offset: 0x00006A99
		// (set) Token: 0x06000269 RID: 617 RVA: 0x000088A1 File Offset: 0x00006AA1
		public int StepSize
		{
			get
			{
				return this._stepSize;
			}
			set
			{
				if (value != this._stepSize)
				{
					this._stepSize = value;
					this.RefreshChildNavigationIndices();
				}
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600026A RID: 618 RVA: 0x000088B9 File Offset: 0x00006AB9
		// (set) Token: 0x0600026B RID: 619 RVA: 0x000088C1 File Offset: 0x00006AC1
		public bool UseSelfIndexForMinimum
		{
			get
			{
				return this._useSelfIndexForMinimum;
			}
			set
			{
				if (value != this._useSelfIndexForMinimum)
				{
					this._useSelfIndexForMinimum = value;
					if (this._useSelfIndexForMinimum && base.GamepadNavigationIndex != -1)
					{
						this.SetNavigationIndicesFromSelf();
					}
				}
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600026C RID: 620 RVA: 0x000088EA File Offset: 0x00006AEA
		// (set) Token: 0x0600026D RID: 621 RVA: 0x000088F2 File Offset: 0x00006AF2
		public Widget EmptyNavigationWidget
		{
			get
			{
				return this._emptyNavigationWidget;
			}
			set
			{
				if (value != this._emptyNavigationWidget)
				{
					if (this._emptyNavigationWidget != null)
					{
						this._emptyNavigationWidget.GamepadNavigationIndex = -1;
					}
					this._emptyNavigationWidget = value;
					this.UpdateEmptyNavigationWidget();
				}
			}
		}

		// Token: 0x04000112 RID: 274
		private bool _areIndicesDirty;

		// Token: 0x04000117 RID: 279
		private int _minIndex;

		// Token: 0x04000118 RID: 280
		private int _maxIndex = int.MaxValue;

		// Token: 0x04000119 RID: 281
		private int _stepSize = 1;

		// Token: 0x0400011A RID: 282
		private bool _useSelfIndexForMinimum;

		// Token: 0x0400011B RID: 283
		private Widget _emptyNavigationWidget;
	}
}
