using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000035 RID: 53
	public class PaginatedScrollablePanel : ScrollablePanel
	{
		// Token: 0x0600031D RID: 797 RVA: 0x00009CE2 File Offset: 0x00007EE2
		public PaginatedScrollablePanel(UIContext context)
			: base(context)
		{
			this.ItemsPerPage = 4;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00009CF4 File Offset: 0x00007EF4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			int num = -1;
			for (int i = 0; i < this.ListWidget.ChildCount; i++)
			{
				Widget child = this.ListWidget.GetChild(i);
				object obj;
				if (child == null)
				{
					obj = null;
				}
				else
				{
					obj = child.GetFirstInChildrenAndThisRecursive((Widget x) => x is ButtonWidget);
				}
				ButtonWidget buttonWidget;
				if ((buttonWidget = obj as ButtonWidget) != null && buttonWidget.IsSelected)
				{
					num = i;
					break;
				}
			}
			if (this._selectedIndex != num)
			{
				this._selectedIndex = num;
				this.OnSelectedWidgetUpdated();
			}
			bool flag = base.IsRecursivelyVisible();
			if (this._isRecursivelyVisible != flag)
			{
				this._isRecursivelyVisible = flag;
				this.OnVisibilityUpdated();
			}
			ScrollablePanel.ScrollbarInterpolationController horizontalScrollbarInterpolationController = this._horizontalScrollbarInterpolationController;
			bool flag2;
			if (horizontalScrollbarInterpolationController == null || !horizontalScrollbarInterpolationController.IsInterpolating)
			{
				ScrollablePanel.ScrollbarInterpolationController verticalScrollbarInterpolationController = this._verticalScrollbarInterpolationController;
				flag2 = verticalScrollbarInterpolationController != null && verticalScrollbarInterpolationController.IsInterpolating;
			}
			else
			{
				flag2 = true;
			}
			bool flag3 = flag2;
			if (this._isInterpolating != flag3)
			{
				this._isInterpolating = flag3;
				if (this.NavigationScope != null)
				{
					this.NavigationScope.DoNotAutoNavigateAfterSort = this._isInterpolating;
				}
				this.UpdateChildrenNavigationStates();
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00009DFA File Offset: 0x00007FFA
		private void ListWidget_EventFire(Widget widget, string eventName, object[] eventArgs)
		{
			if (eventName == "ItemAdd" || eventName == "ItemRemove" || eventName == "AfterItemRemove")
			{
				this.UpdatePageInfo();
				this.UpdateChildrenNavigationStates();
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00009E2F File Offset: 0x0000802F
		private void OnPreviousButtonPressed(Widget widget)
		{
			this.UpdatePageInfo();
			this._pageIndex = Mathf.Clamp(this._pageIndex - 1, 0, this._maxPages - 1);
			this.UpdateButtonEnabledStates();
			this.UpdateChildrenNavigationStates();
			this.UpdateViewport();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00009E65 File Offset: 0x00008065
		private void OnNextButtonPressed(Widget widget)
		{
			this.UpdatePageInfo();
			this._pageIndex = Mathf.Clamp(this._pageIndex + 1, 0, this._maxPages - 1);
			this.UpdateButtonEnabledStates();
			this.UpdateChildrenNavigationStates();
			this.UpdateViewport();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00009E9C File Offset: 0x0000809C
		private void UpdatePageInfo()
		{
			if (this.ListWidget == null || base.ClipRect == null)
			{
				this._pageIndex = 0;
				this._maxPages = 0;
				return;
			}
			int childCount = this.ListWidget.ChildCount;
			this._maxPages = (int)Mathf.Ceil((float)childCount / (float)this.ItemsPerPage);
			this._pageIndex = Mathf.Clamp(this._pageIndex, 0, this._maxPages - 1);
			this.UpdateButtonEnabledStates();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00009F0C File Offset: 0x0000810C
		private void UpdateButtonEnabledStates()
		{
			if (this._maxPages == 1)
			{
				if (this.PreviousButtonWidget != null)
				{
					this.PreviousButtonWidget.IsDisabled = true;
					this.PreviousButtonWidget.DoNotAcceptNavigation = this.PreviousButtonWidget.IsDisabled;
				}
				if (this.NextButtonWidget != null)
				{
					this.NextButtonWidget.IsDisabled = true;
					this.NextButtonWidget.DoNotAcceptNavigation = this.NextButtonWidget.IsDisabled;
					return;
				}
			}
			else
			{
				if (this.PreviousButtonWidget != null)
				{
					this.PreviousButtonWidget.IsDisabled = this._pageIndex == 0;
					this.PreviousButtonWidget.DoNotAcceptNavigation = this.PreviousButtonWidget.IsDisabled;
				}
				if (this.NextButtonWidget != null)
				{
					this.NextButtonWidget.IsDisabled = this._pageIndex == this._maxPages - 1;
					this.NextButtonWidget.DoNotAcceptNavigation = this.NextButtonWidget.IsDisabled;
				}
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00009FE8 File Offset: 0x000081E8
		private void UpdateViewport()
		{
			if (base.ClipRect == null || this.ListWidget == null || this.ListWidget.ChildCount == 0)
			{
				return;
			}
			Vector2 vector = Vector2.Zero;
			for (int i = 0; i < this.ListWidget.ChildCount; i++)
			{
				Widget child = this.ListWidget.GetChild(i);
				vector += child.Size + new Vector2(child.ScaledMarginLeft + child.ScaledMarginRight, child.ScaledMarginBottom + child.ScaledMarginTop);
			}
			vector /= (float)this.ListWidget.ChildCount;
			if (this.ContainerDirection == PaginatedScrollablePanel.ContainerDirections.Horizontal && base.HorizontalScrollbar != null)
			{
				float value = this._horizontalScrollbarInterpolationController.GetValue();
				float num = vector.X * (float)this.ItemsPerPage;
				float num2 = Mathf.Abs(value - vector.X * (float)this.ItemsPerPage * (float)this._pageIndex) / num * this.ScrollTime;
				this._horizontalScrollbarInterpolationController.StartInterpolation(vector.X * (float)this.ItemsPerPage * (float)this._pageIndex, num2);
				return;
			}
			if (this.ContainerDirection == PaginatedScrollablePanel.ContainerDirections.Vertical && base.VerticalScrollbar != null)
			{
				float value2 = this._horizontalScrollbarInterpolationController.GetValue();
				float num3 = vector.Y * (float)this.ItemsPerPage;
				float num4 = Mathf.Abs(value2 - vector.Y * (float)this.ItemsPerPage * (float)this._pageIndex) / num3 * this.ScrollTime;
				this._verticalScrollbarInterpolationController.StartInterpolation(vector.Y * (float)this.ItemsPerPage * (float)this._pageIndex, num4);
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000A16C File Offset: 0x0000836C
		private void UpdateChildrenNavigationStates()
		{
			if (base.ClipRect == null || this.ListWidget == null || this.ListWidget.ChildCount == 0)
			{
				return;
			}
			if (this._isInterpolating)
			{
				for (int i = 0; i < this.ListWidget.ChildCount; i++)
				{
					this.ListWidget.GetChild(i).DoNotAcceptNavigation = true;
				}
				return;
			}
			if (this._pageIndex == this._maxPages - 1)
			{
				for (int j = 0; j < this.ListWidget.ChildCount; j++)
				{
					Widget child = this.ListWidget.GetChild(j);
					if (j >= this.ListWidget.ChildCount - this.ItemsPerPage)
					{
						child.GamepadNavigationIndex = j - this.ListWidget.ChildCount + this.ItemsPerPage + 1;
						child.DoNotAcceptNavigation = false;
					}
					else
					{
						child.DoNotAcceptNavigation = true;
					}
				}
				return;
			}
			for (int k = 0; k < this.ListWidget.ChildCount; k++)
			{
				Widget child2 = this.ListWidget.GetChild(k);
				if (k >= this._pageIndex * this.ItemsPerPage && k < (this._pageIndex + 1) * this.ItemsPerPage)
				{
					child2.GamepadNavigationIndex = k - this._pageIndex * this.ItemsPerPage + 1;
					child2.DoNotAcceptNavigation = false;
				}
				else
				{
					child2.DoNotAcceptNavigation = true;
				}
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000A2B0 File Offset: 0x000084B0
		private void ScrollToSelectedElement()
		{
			this.UpdatePageInfo();
			if (this._pageIndex != this._maxPages - 1 || this._selectedIndex < this.ListWidget.ChildCount - this.ItemsPerPage)
			{
				this._pageIndex = Mathf.Clamp(this._selectedIndex / this.ItemsPerPage, 0, this._maxPages - 1);
			}
			this.UpdateButtonEnabledStates();
			this.UpdateChildrenNavigationStates();
			this.UpdateViewport();
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000A320 File Offset: 0x00008520
		private void OnSelectedWidgetUpdated()
		{
			this.ScrollToSelectedElement();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000A328 File Offset: 0x00008528
		private void OnVisibilityUpdated()
		{
			if (this._isRecursivelyVisible && this.ScrollToSelectedOnVisibilityChanged)
			{
				this.ScrollToSelectedElement();
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000A340 File Offset: 0x00008540
		// (set) Token: 0x0600032A RID: 810 RVA: 0x0000A348 File Offset: 0x00008548
		[Editor(false)]
		public bool ScrollToSelectedOnVisibilityChanged
		{
			get
			{
				return this._scrollToSelectedOnVisibilityChanged;
			}
			set
			{
				if (value != this._scrollToSelectedOnVisibilityChanged)
				{
					this._scrollToSelectedOnVisibilityChanged = value;
					base.OnPropertyChanged(value, "ScrollToSelectedOnVisibilityChanged");
				}
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000A366 File Offset: 0x00008566
		// (set) Token: 0x0600032C RID: 812 RVA: 0x0000A36E File Offset: 0x0000856E
		[Editor(false)]
		public int ItemsPerPage
		{
			get
			{
				return this._itemsPerPage;
			}
			set
			{
				if (value != this._itemsPerPage)
				{
					this._itemsPerPage = value;
					base.OnPropertyChanged(value, "ItemsPerPage");
				}
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000A38C File Offset: 0x0000858C
		// (set) Token: 0x0600032E RID: 814 RVA: 0x0000A394 File Offset: 0x00008594
		[Editor(false)]
		public float ScrollTime
		{
			get
			{
				return this._scrollTime;
			}
			set
			{
				if (value != this._scrollTime)
				{
					this._scrollTime = value;
					base.OnPropertyChanged(value, "ScrollTime");
				}
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000A3B2 File Offset: 0x000085B2
		// (set) Token: 0x06000330 RID: 816 RVA: 0x0000A3BA File Offset: 0x000085BA
		[Editor(false)]
		public PaginatedScrollablePanel.ContainerDirections ContainerDirection
		{
			get
			{
				return this._containerDirection;
			}
			set
			{
				if (value != this._containerDirection)
				{
					this._containerDirection = value;
					base.OnPropertyChanged((int)value, "ContainerDirection");
					this.UpdatePageInfo();
					this.UpdateChildrenNavigationStates();
					this.UpdateViewport();
				}
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000A3EA File Offset: 0x000085EA
		// (set) Token: 0x06000332 RID: 818 RVA: 0x0000A3F4 File Offset: 0x000085F4
		[Editor(false)]
		public ListPanel ListWidget
		{
			get
			{
				return this._listWidget;
			}
			set
			{
				if (value != this._listWidget)
				{
					if (this._listWidget != null)
					{
						this._listWidget.EventFire -= this.ListWidget_EventFire;
					}
					this._listWidget = value;
					if (this._listWidget != null)
					{
						this._listWidget.EventFire += this.ListWidget_EventFire;
					}
					this.UpdatePageInfo();
					this.UpdateChildrenNavigationStates();
					this.UpdateViewport();
					base.OnPropertyChanged<ListPanel>(value, "ListWidget");
				}
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000A46D File Offset: 0x0000866D
		// (set) Token: 0x06000334 RID: 820 RVA: 0x0000A478 File Offset: 0x00008678
		[Editor(false)]
		public ButtonWidget PreviousButtonWidget
		{
			get
			{
				return this._previousButtonWidget;
			}
			set
			{
				if (value != this._previousButtonWidget)
				{
					ButtonWidget previousButtonWidget = this._previousButtonWidget;
					if (previousButtonWidget != null)
					{
						previousButtonWidget.ClickEventHandlers.Remove(new Action<Widget>(this.OnPreviousButtonPressed));
					}
					this._previousButtonWidget = value;
					ButtonWidget previousButtonWidget2 = this._previousButtonWidget;
					if (previousButtonWidget2 != null)
					{
						previousButtonWidget2.ClickEventHandlers.Add(new Action<Widget>(this.OnPreviousButtonPressed));
					}
					base.OnPropertyChanged<ButtonWidget>(value, "PreviousButtonWidget");
				}
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000A4E6 File Offset: 0x000086E6
		// (set) Token: 0x06000336 RID: 822 RVA: 0x0000A4F0 File Offset: 0x000086F0
		[Editor(false)]
		public ButtonWidget NextButtonWidget
		{
			get
			{
				return this._nextButtonWidget;
			}
			set
			{
				if (value != this._nextButtonWidget)
				{
					ButtonWidget nextButtonWidget = this._nextButtonWidget;
					if (nextButtonWidget != null)
					{
						nextButtonWidget.ClickEventHandlers.Remove(new Action<Widget>(this.OnNextButtonPressed));
					}
					this._nextButtonWidget = value;
					ButtonWidget nextButtonWidget2 = this._nextButtonWidget;
					if (nextButtonWidget2 != null)
					{
						nextButtonWidget2.ClickEventHandlers.Add(new Action<Widget>(this.OnNextButtonPressed));
					}
					base.OnPropertyChanged<ButtonWidget>(value, "NextButtonWidget");
				}
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000A55E File Offset: 0x0000875E
		// (set) Token: 0x06000338 RID: 824 RVA: 0x0000A566 File Offset: 0x00008766
		[Editor(false)]
		public NavigationScopeTargeter NavigationScope
		{
			get
			{
				return this._navigationScope;
			}
			set
			{
				if (value != this._navigationScope)
				{
					this._navigationScope = value;
					base.OnPropertyChanged<NavigationScopeTargeter>(value, "NavigationScope");
				}
			}
		}

		// Token: 0x04000141 RID: 321
		private int _pageIndex;

		// Token: 0x04000142 RID: 322
		private int _maxPages;

		// Token: 0x04000143 RID: 323
		private int _selectedIndex;

		// Token: 0x04000144 RID: 324
		private bool _isRecursivelyVisible;

		// Token: 0x04000145 RID: 325
		private bool _isInterpolating;

		// Token: 0x04000146 RID: 326
		private bool _scrollToSelectedOnVisibilityChanged;

		// Token: 0x04000147 RID: 327
		private int _itemsPerPage;

		// Token: 0x04000148 RID: 328
		private float _scrollTime;

		// Token: 0x04000149 RID: 329
		private PaginatedScrollablePanel.ContainerDirections _containerDirection;

		// Token: 0x0400014A RID: 330
		private ListPanel _listWidget;

		// Token: 0x0400014B RID: 331
		private ButtonWidget _previousButtonWidget;

		// Token: 0x0400014C RID: 332
		private ButtonWidget _nextButtonWidget;

		// Token: 0x0400014D RID: 333
		private NavigationScopeTargeter _navigationScope;

		// Token: 0x020001A2 RID: 418
		public enum ContainerDirections
		{
			// Token: 0x040009D4 RID: 2516
			Horizontal,
			// Token: 0x040009D5 RID: 2517
			Vertical
		}
	}
}
