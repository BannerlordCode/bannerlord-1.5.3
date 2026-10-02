using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000030 RID: 48
	public class NavigationAutoScrollWidget : Widget
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00008D5B File Offset: 0x00006F5B
		// (set) Token: 0x06000291 RID: 657 RVA: 0x00008D63 File Offset: 0x00006F63
		public ScrollablePanel ParentPanel { get; set; }

		// Token: 0x06000292 RID: 658 RVA: 0x00008D6C File Offset: 0x00006F6C
		public NavigationAutoScrollWidget(UIContext context)
			: base(context)
		{
			base.WidthSizePolicy = SizePolicy.Fixed;
			base.HeightSizePolicy = SizePolicy.Fixed;
			base.SuggestedHeight = 0f;
			base.SuggestedWidth = 0f;
			base.IsVisible = false;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00008DA0 File Offset: 0x00006FA0
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			if (this.ParentPanel == null && base.ParentWidget != null)
			{
				for (Widget widget = base.ParentWidget; widget != null; widget = widget.ParentWidget)
				{
					ScrollablePanel scrollablePanel;
					if ((scrollablePanel = widget as ScrollablePanel) != null)
					{
						this.ParentPanel = scrollablePanel;
						return;
					}
				}
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00008DE8 File Offset: 0x00006FE8
		private void OnWidgetGainedGamepadFocus(Widget widget)
		{
			if (this.ParentPanel != null)
			{
				ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters((float)this.AutoScrollTopOffset, (float)this.AutoScrollBottomOffset, (float)this.AutoScrollLeftOffset, (float)this.AutoScrollRightOffset, -1f, -1f, 0f);
				this.ParentPanel.ScrollToChild(this.ScrollTarget ?? widget, autoScrollParameters);
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00008E48 File Offset: 0x00007048
		private void UpdateTargetAutoScrollAndChildren()
		{
			if (this._trackedWidget != null)
			{
				Widget trackedWidget = this._trackedWidget;
				trackedWidget.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Combine(trackedWidget.OnGamepadNavigationFocusGained, new Action<Widget>(this.OnWidgetGainedGamepadFocus));
				foreach (Widget widget in this._trackedWidget.Children)
				{
					if (this.IncludeChildren)
					{
						Widget widget2 = widget;
						widget2.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Combine(widget2.OnGamepadNavigationFocusGained, new Action<Widget>(this.OnWidgetGainedGamepadFocus));
					}
					else
					{
						Widget widget3 = widget;
						widget3.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Remove(widget3.OnGamepadNavigationFocusGained, new Action<Widget>(this.OnWidgetGainedGamepadFocus));
					}
				}
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000296 RID: 662 RVA: 0x00008F1C File Offset: 0x0000711C
		// (set) Token: 0x06000297 RID: 663 RVA: 0x00008F24 File Offset: 0x00007124
		public int AutoScrollTopOffset { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000298 RID: 664 RVA: 0x00008F2D File Offset: 0x0000712D
		// (set) Token: 0x06000299 RID: 665 RVA: 0x00008F35 File Offset: 0x00007135
		public int AutoScrollBottomOffset { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00008F3E File Offset: 0x0000713E
		// (set) Token: 0x0600029B RID: 667 RVA: 0x00008F46 File Offset: 0x00007146
		public int AutoScrollLeftOffset { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00008F4F File Offset: 0x0000714F
		// (set) Token: 0x0600029D RID: 669 RVA: 0x00008F57 File Offset: 0x00007157
		public int AutoScrollRightOffset { get; set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00008F60 File Offset: 0x00007160
		// (set) Token: 0x0600029F RID: 671 RVA: 0x00008F68 File Offset: 0x00007168
		public bool IncludeChildren
		{
			get
			{
				return this._includeChildren;
			}
			set
			{
				if (value != this._includeChildren)
				{
					this._includeChildren = value;
					this.UpdateTargetAutoScrollAndChildren();
				}
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00008F80 File Offset: 0x00007180
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x00008F88 File Offset: 0x00007188
		public Widget TrackedWidget
		{
			get
			{
				return this._trackedWidget;
			}
			set
			{
				if (value != this._trackedWidget)
				{
					if (this._trackedWidget != null)
					{
						this._trackedWidget.OnGamepadNavigationFocusGained = null;
					}
					this._trackedWidget = value;
					this.UpdateTargetAutoScrollAndChildren();
				}
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x00008FB4 File Offset: 0x000071B4
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x00008FBC File Offset: 0x000071BC
		public Widget ScrollTarget
		{
			get
			{
				return this._scrollTarget;
			}
			set
			{
				if (value != this._scrollTarget)
				{
					this._scrollTarget = value;
				}
			}
		}

		// Token: 0x0400012C RID: 300
		private bool _includeChildren;

		// Token: 0x0400012D RID: 301
		private Widget _trackedWidget;

		// Token: 0x0400012E RID: 302
		private Widget _scrollTarget;
	}
}
