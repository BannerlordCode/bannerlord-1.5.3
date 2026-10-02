using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.Layout;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200002F RID: 47
	public class NavigatableListPanel : ListPanel
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000891E File Offset: 0x00006B1E
		// (set) Token: 0x0600026F RID: 623 RVA: 0x00008926 File Offset: 0x00006B26
		public ScrollablePanel ParentPanel { get; set; }

		// Token: 0x06000270 RID: 624 RVA: 0x0000892F File Offset: 0x00006B2F
		public NavigatableListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000894A File Offset: 0x00006B4A
		protected override void OnLateUpdate(float dt)
		{
			if (this._areIndicesDirty)
			{
				this.RefreshChildNavigationIndices();
				this._areIndicesDirty = false;
			}
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00008961 File Offset: 0x00006B61
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			if (this.ParentPanel == null)
			{
				this.ParentPanel = base.FindParentPanel();
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00008980 File Offset: 0x00006B80
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.OnGamepadNavigationFocusGained = new Action<Widget>(this.OnWidgetGainedGamepadFocus);
			child.EventFire += this.OnChildSiblingIndexChanged;
			child.boolPropertyChanged += this.OnChildVisibilityChanged;
			this._areIndicesDirty = true;
			this.UpdateEmptyNavigationWidget();
		}

		// Token: 0x06000274 RID: 628 RVA: 0x000089D8 File Offset: 0x00006BD8
		protected override void OnAfterChildRemoved(Widget child, int previousIndexOfChild)
		{
			base.OnAfterChildRemoved(child, previousIndexOfChild);
			child.OnGamepadNavigationFocusGained = null;
			child.EventFire -= this.OnChildSiblingIndexChanged;
			child.boolPropertyChanged -= this.OnChildVisibilityChanged;
			child.GamepadNavigationIndex = -1;
			this.UpdateEmptyNavigationWidget();
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00008A28 File Offset: 0x00006C28
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			for (int i = 0; i < base.Children.Count; i++)
			{
				base.Children[i].OnGamepadNavigationFocusGained = null;
				base.Children[i].EventFire -= this.OnChildSiblingIndexChanged;
				base.Children[i].boolPropertyChanged -= this.OnChildVisibilityChanged;
				base.Children[i].GamepadNavigationIndex = -1;
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00008AB0 File Offset: 0x00006CB0
		private void OnChildVisibilityChanged(PropertyOwnerObject child, string propertyName, bool value)
		{
			if (propertyName == "IsVisible")
			{
				Widget widget = (Widget)child;
				if (!value)
				{
					widget.GamepadNavigationIndex = -1;
					return;
				}
				this.SetNavigationIndexForChild(widget);
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00008AE4 File Offset: 0x00006CE4
		private void OnWidgetGainedGamepadFocus(Widget widget)
		{
			if (this.ParentPanel != null)
			{
				ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters((float)this.AutoScrollTopOffset, (float)this.AutoScrollBottomOffset, (float)this.AutoScrollLeftOffset, (float)this.AutoScrollRightOffset, -1f, -1f, 0f);
				this.ParentPanel.ScrollToChild(widget, autoScrollParameters);
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00008B37 File Offset: 0x00006D37
		private void OnChildSiblingIndexChanged(Widget widget, string eventName, object[] parameters)
		{
			if (eventName == "SiblingIndexChanged")
			{
				this._areIndicesDirty = true;
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00008B50 File Offset: 0x00006D50
		private void SetNavigationIndexForChild(Widget widget)
		{
			int num;
			if (base.StackLayout.LayoutMethod == LayoutMethod.VerticalBottomToTop || base.StackLayout.LayoutMethod == LayoutMethod.HorizontalRightToLeft)
			{
				num = this.MaxIndex - widget.GetSiblingIndex() * this.StepSize;
			}
			else
			{
				num = this.MinIndex + widget.GetSiblingIndex() * this.StepSize;
			}
			if (num <= this.MaxIndex)
			{
				widget.GamepadNavigationIndex = num;
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00008BB5 File Offset: 0x00006DB5
		protected override void OnGamepadNavigationIndexUpdated(int newIndex)
		{
			if (newIndex != -1 && this.UseSelfIndexForMinimum)
			{
				this.SetNavigationIndicesFromSelf();
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00008BC9 File Offset: 0x00006DC9
		private void SetNavigationIndicesFromSelf()
		{
			this.MinIndex = base.GamepadNavigationIndex;
			base.GamepadNavigationIndex = -1;
			this._areIndicesDirty = true;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00008BE8 File Offset: 0x00006DE8
		protected void RefreshChildNavigationIndices()
		{
			for (int i = 0; i < base.Children.Count; i++)
			{
				this.SetNavigationIndexForChild(base.Children[i]);
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00008C1D File Offset: 0x00006E1D
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

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600027E RID: 638 RVA: 0x00008C52 File Offset: 0x00006E52
		// (set) Token: 0x0600027F RID: 639 RVA: 0x00008C5A File Offset: 0x00006E5A
		public int AutoScrollTopOffset { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000280 RID: 640 RVA: 0x00008C63 File Offset: 0x00006E63
		// (set) Token: 0x06000281 RID: 641 RVA: 0x00008C6B File Offset: 0x00006E6B
		public int AutoScrollBottomOffset { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000282 RID: 642 RVA: 0x00008C74 File Offset: 0x00006E74
		// (set) Token: 0x06000283 RID: 643 RVA: 0x00008C7C File Offset: 0x00006E7C
		public int AutoScrollLeftOffset { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000284 RID: 644 RVA: 0x00008C85 File Offset: 0x00006E85
		// (set) Token: 0x06000285 RID: 645 RVA: 0x00008C8D File Offset: 0x00006E8D
		public int AutoScrollRightOffset { get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000286 RID: 646 RVA: 0x00008C96 File Offset: 0x00006E96
		// (set) Token: 0x06000287 RID: 647 RVA: 0x00008C9E File Offset: 0x00006E9E
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

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000288 RID: 648 RVA: 0x00008CB6 File Offset: 0x00006EB6
		// (set) Token: 0x06000289 RID: 649 RVA: 0x00008CBE File Offset: 0x00006EBE
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

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00008CD6 File Offset: 0x00006ED6
		// (set) Token: 0x0600028B RID: 651 RVA: 0x00008CDE File Offset: 0x00006EDE
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

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00008CF6 File Offset: 0x00006EF6
		// (set) Token: 0x0600028D RID: 653 RVA: 0x00008CFE File Offset: 0x00006EFE
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

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00008D27 File Offset: 0x00006F27
		// (set) Token: 0x0600028F RID: 655 RVA: 0x00008D2F File Offset: 0x00006F2F
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

		// Token: 0x0400011C RID: 284
		private bool _areIndicesDirty;

		// Token: 0x0400011E RID: 286
		private int _minIndex;

		// Token: 0x0400011F RID: 287
		private int _maxIndex = int.MaxValue;

		// Token: 0x04000120 RID: 288
		private int _stepSize = 1;

		// Token: 0x04000121 RID: 289
		private bool _useSelfIndexForMinimum;

		// Token: 0x04000122 RID: 290
		private Widget _emptyNavigationWidget;
	}
}
