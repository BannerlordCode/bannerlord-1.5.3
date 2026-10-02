using System;
using System.Collections.Generic;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000012 RID: 18
	public class ContextMenuBrushWidget : BrushWidget
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004590 File Offset: 0x00002790
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00004598 File Offset: 0x00002798
		public float HorizontalPadding { get; set; } = 10f;

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000045A1 File Offset: 0x000027A1
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x000045A9 File Offset: 0x000027A9
		public float VerticalPadding { get; set; } = 10f;

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000045B2 File Offset: 0x000027B2
		private bool _isClickedOnOtherWidget
		{
			get
			{
				return this._isPrimaryClickedOnOtherWidget || this._isAlternateClickedOnOtherWidget;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000045C4 File Offset: 0x000027C4
		private bool _isPrimaryClickedOnOtherWidget
		{
			get
			{
				return this._latestMouseUpWidgetWhenActivated != base.EventManager.LatestMouseDownWidget && !base.CheckIsMyChildRecursive(base.EventManager.LatestMouseDownWidget);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000EC RID: 236 RVA: 0x000045EF File Offset: 0x000027EF
		private bool _isAlternateClickedOnOtherWidget
		{
			get
			{
				return this._latestAltMouseUpWidgetWhenActivated != base.EventManager.LatestMouseAlternateDownWidget && !base.CheckIsMyChildRecursive(base.EventManager.LatestMouseAlternateDownWidget);
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000461C File Offset: 0x0000281C
		public ContextMenuBrushWidget(UIContext context)
			: base(context)
		{
			this._newlyAddedItemList = new List<ContextMenuItemWidget>();
			this._newlyRemovedItemList = new List<ContextMenuItemWidget>();
			base.EventManager.AddLateUpdateAction(this, new Action<float>(this.CustomLateUpdate), 1);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004678 File Offset: 0x00002878
		private void CustomLateUpdate(float dt)
		{
			if (!this._isDestroyed)
			{
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.CustomLateUpdate), 1);
				if (base.IsVisible && !base.IsRecursivelyVisible())
				{
					this.Deactivate();
				}
				if (base.IsVisible && !this._isActivatedThisFrame && this._isClickedOnOtherWidget)
				{
					this.Deactivate();
				}
				if (this._isActivatedThisFrame)
				{
					Vector2 vector = this.DetermineMenuPositionFromMousePosition(base.EventManager.MousePosition);
					this._targetPosition = base.ParentWidget.GetLocalPoint(vector);
					this._isActivatedThisFrame = false;
				}
				base.ScaledPositionXOffset = MathF.Clamp(this._targetPosition.X, 0f, base.EventManager.PageSize.X - base.Size.X);
				base.ScaledPositionYOffset = MathF.Clamp(this._targetPosition.Y, 0f, base.EventManager.PageSize.Y - base.Size.Y);
				this.HandleNewlyAddedRemovedList();
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004784 File Offset: 0x00002984
		private void HandleNewlyAddedRemovedList()
		{
			foreach (ContextMenuItemWidget contextMenuItemWidget in this._newlyAddedItemList)
			{
				contextMenuItemWidget.ActionButtonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnAnyAction));
			}
			this._newlyAddedItemList.Clear();
			foreach (ContextMenuItemWidget contextMenuItemWidget2 in this._newlyRemovedItemList)
			{
				contextMenuItemWidget2.ActionButtonWidget.ClickEventHandlers.Remove(new Action<Widget>(this.OnAnyAction));
			}
			this._newlyRemovedItemList.Clear();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004858 File Offset: 0x00002A58
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			this._isDestroyed = true;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004867 File Offset: 0x00002A67
		private void Activate()
		{
			this._isActivatedThisFrame = true;
			this._latestMouseUpWidgetWhenActivated = base.EventManager.LatestMouseDownWidget;
			this._latestAltMouseUpWidgetWhenActivated = base.EventManager.LatestMouseAlternateDownWidget;
			base.IsVisible = true;
			this.AddGamepadNavigation();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000048A0 File Offset: 0x00002AA0
		private void Deactivate()
		{
			base.ScaledPositionXOffset = base.EventManager.PageSize.X;
			base.ScaledPositionYOffset = base.EventManager.PageSize.Y;
			base.IsVisible = false;
			this.IsActivated = false;
			this.DestroyGamepadNavigation();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000048F0 File Offset: 0x00002AF0
		private void AddGamepadNavigation()
		{
			if (this._navigationScope == null && this._scopeCollection == null)
			{
				this._navigationScope = new GamepadNavigationScope
				{
					ScopeID = "ContextMenuScope",
					ScopeMovements = GamepadNavigationTypes.Vertical,
					ParentWidget = this,
					DoNotAutomaticallyFindChildren = true,
					HasCircularMovement = true
				};
				base.GamepadNavigationContext.AddNavigationScope(this._navigationScope, false);
				for (int i = 0; i < this.ActionListPanel.Children.Count; i++)
				{
					ContextMenuItemWidget contextMenuItemWidget;
					if ((contextMenuItemWidget = this.ActionListPanel.Children[i] as ContextMenuItemWidget) != null)
					{
						this._navigationScope.AddWidgetAtIndex(contextMenuItemWidget, i);
					}
				}
				this._scopeCollection = new GamepadNavigationForcedScopeCollection
				{
					CollectionID = "ContextMenuCollection",
					CollectionOrder = 999,
					ParentWidget = this
				};
				base.GamepadNavigationContext.AddForcedScopeCollection(this._scopeCollection);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000049D4 File Offset: 0x00002BD4
		private void DestroyGamepadNavigation()
		{
			if (this._navigationScope != null && this._scopeCollection != null)
			{
				this._navigationScope.ClearNavigatableWidgets();
				this._scopeCollection.ClearScopes();
				base.GamepadNavigationContext.RemoveNavigationScope(this._navigationScope);
				base.GamepadNavigationContext.RemoveForcedScopeCollection(this._scopeCollection);
				this._navigationScope = null;
				this._scopeCollection = null;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004A37 File Offset: 0x00002C37
		private void OnScrollOfContextItem(float scrollAmount)
		{
			this.Deactivate();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004A3F File Offset: 0x00002C3F
		private void OnAnyAction(Widget obj)
		{
			this.Deactivate();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004A48 File Offset: 0x00002C48
		private void OnNewActionButtonRemoved(Widget obj, Widget child)
		{
			ContextMenuItemWidget contextMenuItemWidget;
			if ((contextMenuItemWidget = child as ContextMenuItemWidget) != null)
			{
				this._newlyRemovedItemList.Add(contextMenuItemWidget);
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004A6C File Offset: 0x00002C6C
		private void OnNewActionButtonAdded(Widget listPanel, Widget child)
		{
			ContextMenuItemWidget contextMenuItemWidget;
			if ((contextMenuItemWidget = child as ContextMenuItemWidget) != null)
			{
				this._newlyAddedItemList.Add(contextMenuItemWidget);
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004A90 File Offset: 0x00002C90
		private Vector2 DetermineMenuPositionFromMousePosition(Vector2 mousePosition)
		{
			bool flag = mousePosition.X > base.EventManager.PageSize.X / 2f;
			bool flag2 = mousePosition.Y > base.EventManager.PageSize.Y / 2f;
			float num = (flag ? (mousePosition.X - base.Size.X) : mousePosition.X);
			float num2 = (flag2 ? (mousePosition.Y - base.Size.Y) : mousePosition.Y);
			float num3 = num + (flag ? (-this.HorizontalPadding) : this.HorizontalPadding);
			num2 += (flag2 ? (-this.VerticalPadding) : this.VerticalPadding);
			return new Vector2(num3, num2);
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00004B44 File Offset: 0x00002D44
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00004B4C File Offset: 0x00002D4C
		[Editor(false)]
		public bool IsActivated
		{
			get
			{
				return this._isActivated;
			}
			set
			{
				if (this._isActivated != value)
				{
					this._isActivated = value;
					base.OnPropertyChanged(value, "IsActivated");
					if (this._isActivated)
					{
						this.Activate();
						return;
					}
					this.Deactivate();
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00004B7F File Offset: 0x00002D7F
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00004B88 File Offset: 0x00002D88
		[Editor(false)]
		public ListPanel ActionListPanel
		{
			get
			{
				return this._actionListPanel;
			}
			set
			{
				if (this._actionListPanel != value)
				{
					this._actionListPanel = value;
					this._actionListPanel.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnNewActionButtonAdded));
					this._actionListPanel.ItemRemoveEventHandlers.Add(new Action<Widget, Widget>(this.OnNewActionButtonRemoved));
					base.OnPropertyChanged<ListPanel>(value, "ActionListPanel");
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00004BE9 File Offset: 0x00002DE9
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00004BF1 File Offset: 0x00002DF1
		[Editor(false)]
		public ScrollablePanel ScrollPanelToWatch
		{
			get
			{
				return this._scrollPanelToWatch;
			}
			set
			{
				if (this._scrollPanelToWatch != value)
				{
					this._scrollPanelToWatch = value;
					this._scrollPanelToWatch.OnScroll += this.OnScrollOfContextItem;
					base.OnPropertyChanged<ScrollablePanel>(value, "ScrollPanelToWatch");
				}
			}
		}

		// Token: 0x0400006F RID: 111
		private Vector2 _targetPosition;

		// Token: 0x04000070 RID: 112
		private Widget _latestMouseUpWidgetWhenActivated;

		// Token: 0x04000071 RID: 113
		private Widget _latestAltMouseUpWidgetWhenActivated;

		// Token: 0x04000072 RID: 114
		private bool _isDestroyed;

		// Token: 0x04000073 RID: 115
		private bool _isActivatedThisFrame;

		// Token: 0x04000074 RID: 116
		private List<ContextMenuItemWidget> _newlyAddedItemList;

		// Token: 0x04000075 RID: 117
		private List<ContextMenuItemWidget> _newlyRemovedItemList;

		// Token: 0x04000076 RID: 118
		private GamepadNavigationScope _navigationScope;

		// Token: 0x04000077 RID: 119
		private GamepadNavigationForcedScopeCollection _scopeCollection;

		// Token: 0x04000078 RID: 120
		private bool _isActivated;

		// Token: 0x04000079 RID: 121
		public ScrollablePanel _scrollPanelToWatch;

		// Token: 0x0400007A RID: 122
		public ListPanel _actionListPanel;
	}
}
