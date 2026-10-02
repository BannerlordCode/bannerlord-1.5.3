using System;
using System.Collections.Generic;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000020 RID: 32
	public class EventManager
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000C268 File Offset: 0x0000A468
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000C270 File Offset: 0x0000A470
		public float Time { get; private set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000C279 File Offset: 0x0000A479
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000C281 File Offset: 0x0000A481
		public Vec2 UsableArea { get; set; } = new Vec2(1f, 1f);

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000C28A File Offset: 0x0000A48A
		// (set) Token: 0x06000261 RID: 609 RVA: 0x0000C292 File Offset: 0x0000A492
		public float LeftUsableAreaStart { get; private set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000C29B File Offset: 0x0000A49B
		// (set) Token: 0x06000263 RID: 611 RVA: 0x0000C2A3 File Offset: 0x0000A4A3
		public float TopUsableAreaStart { get; private set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000C2AC File Offset: 0x0000A4AC
		// (set) Token: 0x06000265 RID: 613 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
		public Vector2 PageSize { get; private set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000C2BD File Offset: 0x0000A4BD
		// (set) Token: 0x06000267 RID: 615 RVA: 0x0000C2C4 File Offset: 0x0000A4C4
		public static EventManager UIEventManager { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000C2CC File Offset: 0x0000A4CC
		public Vector2 MousePositionInReferenceResolution
		{
			get
			{
				return this.MousePosition * this.Context.CustomInverseScale;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000C2E4 File Offset: 0x0000A4E4
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000C2EC File Offset: 0x0000A4EC
		public bool IsControllerActive { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000C2F5 File Offset: 0x0000A4F5
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000C2FD File Offset: 0x0000A4FD
		public UIContext Context { get; private set; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600026D RID: 621 RVA: 0x0000C308 File Offset: 0x0000A508
		// (remove) Token: 0x0600026E RID: 622 RVA: 0x0000C340 File Offset: 0x0000A540
		public event Action OnDragStarted;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600026F RID: 623 RVA: 0x0000C378 File Offset: 0x0000A578
		// (remove) Token: 0x06000270 RID: 624 RVA: 0x0000C3B0 File Offset: 0x0000A5B0
		public event Action OnDragEnded;

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000C3E5 File Offset: 0x0000A5E5
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000C3ED File Offset: 0x0000A5ED
		public Widget Root { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000C3F6 File Offset: 0x0000A5F6
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000C400 File Offset: 0x0000A600
		public Widget FocusedWidget
		{
			get
			{
				return this._focusedWidget;
			}
			set
			{
				if (this._isOnScreenKeyboardRequested || (this._focusedWidget is EditableTextWidget && Input.IsOnScreenKeyboardActive))
				{
					return;
				}
				if (this._focusedWidget != value)
				{
					Widget focusedWidget = this._focusedWidget;
					if (focusedWidget != null)
					{
						focusedWidget.OnLoseFocus();
					}
					if (value != null && (!value.ConnectedToRoot || !value.IsFocusable))
					{
						this._focusedWidget = null;
					}
					else
					{
						this._focusedWidget = value;
						Widget focusedWidget2 = this._focusedWidget;
						if (focusedWidget2 != null)
						{
							focusedWidget2.OnGainFocus();
						}
						if (this._focusedWidget is EditableTextWidget && this.IsControllerActive)
						{
							this._isOnScreenKeyboardRequested = true;
						}
					}
					Action onFocusedWidgetChanged = this.OnFocusedWidgetChanged;
					if (onFocusedWidgetChanged == null)
					{
						return;
					}
					onFocusedWidgetChanged();
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000C4A4 File Offset: 0x0000A6A4
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000C4AC File Offset: 0x0000A6AC
		public Widget HoveredWidget
		{
			get
			{
				return this._hoveredWidget;
			}
			set
			{
				if (this._hoveredWidget != value)
				{
					Widget hoveredWidget = this._hoveredWidget;
					if (hoveredWidget != null)
					{
						hoveredWidget.OnHoverEnd();
					}
					if (value != null && !value.ConnectedToRoot)
					{
						this._hoveredWidget = null;
						return;
					}
					this._hoveredWidget = value;
					Widget hoveredWidget2 = this._hoveredWidget;
					if (hoveredWidget2 == null)
					{
						return;
					}
					hoveredWidget2.OnHoverBegin();
				}
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000C4FD File Offset: 0x0000A6FD
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000C505 File Offset: 0x0000A705
		public List<Widget> MouseOveredWidgets
		{
			get
			{
				return this._mouseOveredWidgets;
			}
			private set
			{
				if (value != null)
				{
					this._mouseOveredWidgets = value;
					return;
				}
				this._mouseOveredWidgets = null;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000C519 File Offset: 0x0000A719
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000C524 File Offset: 0x0000A724
		public Widget DragHoveredWidget
		{
			get
			{
				return this._dragHoveredWidget;
			}
			private set
			{
				if (this._dragHoveredWidget != value)
				{
					Widget dragHoveredWidget = this._dragHoveredWidget;
					if (dragHoveredWidget != null)
					{
						dragHoveredWidget.OnDragHoverEnd();
					}
					if (value != null && (!value.ConnectedToRoot || !value.AcceptDrop))
					{
						this._dragHoveredWidget = null;
						return;
					}
					this._dragHoveredWidget = value;
					Widget dragHoveredWidget2 = this._dragHoveredWidget;
					if (dragHoveredWidget2 == null)
					{
						return;
					}
					dragHoveredWidget2.OnDragHoverBegin();
				}
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000C57D File Offset: 0x0000A77D
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000C585 File Offset: 0x0000A785
		public Widget DraggedWidget { get; private set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000C590 File Offset: 0x0000A790
		public Vector2 DraggedWidgetPosition
		{
			get
			{
				if (this.DraggedWidget != null)
				{
					return this._dragCarrier.AreaRect.TopLeft * this.Context.CustomScale - new Vector2(this.LeftUsableAreaStart, this.TopUsableAreaStart);
				}
				return this.MousePositionInReferenceResolution;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000C5E2 File Offset: 0x0000A7E2
		// (set) Token: 0x0600027F RID: 639 RVA: 0x0000C5EA File Offset: 0x0000A7EA
		public Widget LatestMouseDownWidget
		{
			get
			{
				return this._latestMouseDownWidget;
			}
			private set
			{
				if (value != null && value.ConnectedToRoot)
				{
					this._latestMouseDownWidget = value;
					return;
				}
				this._latestMouseDownWidget = null;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000C606 File Offset: 0x0000A806
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000C60E File Offset: 0x0000A80E
		public Widget LatestMouseUpWidget
		{
			get
			{
				return this._latestMouseUpWidget;
			}
			private set
			{
				if (value != null && value.ConnectedToRoot)
				{
					this._latestMouseUpWidget = value;
					return;
				}
				this._latestMouseUpWidget = null;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000C62A File Offset: 0x0000A82A
		// (set) Token: 0x06000283 RID: 643 RVA: 0x0000C632 File Offset: 0x0000A832
		public Widget LatestMouseAlternateDownWidget
		{
			get
			{
				return this._latestMouseAlternateDownWidget;
			}
			private set
			{
				if (value != null && value.ConnectedToRoot)
				{
					this._latestMouseAlternateDownWidget = value;
					return;
				}
				this._latestMouseAlternateDownWidget = null;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000C64E File Offset: 0x0000A84E
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000C656 File Offset: 0x0000A856
		public Widget LatestMouseAlternateUpWidget
		{
			get
			{
				return this._latestMouseAlternateUpWidget;
			}
			private set
			{
				if (value != null && value.ConnectedToRoot)
				{
					this._latestMouseAlternateUpWidget = value;
					return;
				}
				this._latestMouseAlternateUpWidget = null;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000C672 File Offset: 0x0000A872
		public Vector2 MousePosition
		{
			get
			{
				return this.Context.InputContext.GetMousePosition();
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000C684 File Offset: 0x0000A884
		public ulong LocalFrameNumber
		{
			get
			{
				return this.Context.LocalFrameNumber;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000C691 File Offset: 0x0000A891
		private bool IsDragging
		{
			get
			{
				return this.DraggedWidget != null;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000C69C File Offset: 0x0000A89C
		public float DeltaMouseScroll
		{
			get
			{
				return this.Context.InputContext.GetMouseScrollDelta();
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000C6B0 File Offset: 0x0000A8B0
		public float RightStickVerticalScrollAmount
		{
			get
			{
				float y = Input.GetKeyState(InputKey.ControllerRStick).Y;
				return 3000f * y * 0.4f * this.CachedDt;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000C6E4 File Offset: 0x0000A8E4
		public float RightStickHorizontalScrollAmount
		{
			get
			{
				float x = Input.GetKeyState(InputKey.ControllerRStick).X;
				return 3000f * x * 0.4f * this.CachedDt;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000C718 File Offset: 0x0000A918
		// (set) Token: 0x0600028D RID: 653 RVA: 0x0000C720 File Offset: 0x0000A920
		internal float CachedDt { get; private set; }

		// Token: 0x0600028E RID: 654 RVA: 0x0000C72C File Offset: 0x0000A92C
		internal EventManager(UIContext context)
		{
			this.Context = context;
			this.Root = new Widget(context)
			{
				Id = "Root"
			};
			if (EventManager.UIEventManager == null)
			{
				EventManager.UIEventManager = new EventManager();
			}
			this.AreaRectangle = Rectangle2D.Create();
			this._widgetContainers = new Dictionary<WidgetContainer.ContainerType, WidgetContainer>
			{
				{
					WidgetContainer.ContainerType.Update,
					new WidgetContainer(32)
				},
				{
					WidgetContainer.ContainerType.ParallelUpdate,
					new WidgetContainer(16)
				},
				{
					WidgetContainer.ContainerType.LateUpdate,
					new WidgetContainer(32)
				},
				{
					WidgetContainer.ContainerType.VisualDefinition,
					new WidgetContainer(16)
				},
				{
					WidgetContainer.ContainerType.UpdateBrushes,
					new WidgetContainer(64)
				}
			};
			this._lateUpdateActionLocker = new object();
			this._lateUpdateActions = new Dictionary<int, List<UpdateAction>>();
			this._lateUpdateActionsRunning = new Dictionary<int, List<UpdateAction>>();
			this._onAfterFinalizedCallbacks = new List<Action>();
			for (int i = 1; i <= 5; i++)
			{
				this._lateUpdateActions.Add(i, new List<UpdateAction>(32));
				this._lateUpdateActionsRunning.Add(i, new List<UpdateAction>(32));
			}
			this._drawContext = new TwoDimensionDrawContext();
			this.MouseOveredWidgets = new List<Widget>();
			this.ParallelUpdateWidgetPredicate = new TWParallel.ParallelForWithDtAuxPredicate(this.ParallelUpdateWidget);
			this.UpdateBrushesWidgetPredicate = new TWParallel.ParallelForWithDtAuxPredicate(this.UpdateBrushesWidget);
			this.IsControllerActive = Input.IsGamepadActive;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000C8A4 File Offset: 0x0000AAA4
		internal void OnFinalize()
		{
			if (!this._lastSetFrictionValue.ApproximatelyEqualsTo(1f, 1E-05f))
			{
				this._lastSetFrictionValue = 1f;
				Input.SetCursorFriction(this._lastSetFrictionValue);
			}
			foreach (KeyValuePair<WidgetContainer.ContainerType, WidgetContainer> keyValuePair in this._widgetContainers)
			{
				keyValuePair.Value.Clear();
			}
			for (int i = 0; i < this._onAfterFinalizedCallbacks.Count; i++)
			{
				Action action = this._onAfterFinalizedCallbacks[i];
				if (action != null)
				{
					action();
				}
			}
			this._onAfterFinalizedCallbacks.Clear();
			this._onAfterFinalizedCallbacks = null;
			this._widgetContainers = null;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000C970 File Offset: 0x0000AB70
		public void AddAfterFinalizedCallback(Action callback)
		{
			this._onAfterFinalizedCallbacks.Add(callback);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000C980 File Offset: 0x0000AB80
		internal void OnContextActivated()
		{
			List<Widget> allChildrenAndThisRecursive = this.Root.GetAllChildrenAndThisRecursive();
			for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
			{
				allChildrenAndThisRecursive[i].OnContextActivated();
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000C9B8 File Offset: 0x0000ABB8
		internal void OnContextDeactivated()
		{
			List<Widget> allChildrenAndThisRecursive = this.Root.GetAllChildrenAndThisRecursive();
			for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
			{
				allChildrenAndThisRecursive[i].OnContextDeactivated();
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000C9F0 File Offset: 0x0000ABF0
		internal void OnWidgetConnectedToRoot(Widget widget)
		{
			widget.HandleOnConnectedToRoot();
			List<Widget> allChildrenAndThisRecursive = widget.GetAllChildrenAndThisRecursive();
			for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
			{
				Widget widget2 = allChildrenAndThisRecursive[i];
				widget2.HandleOnConnectedToRoot();
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.Update, widget2);
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.LateUpdate, widget2);
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.UpdateBrushes, widget2);
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.ParallelUpdate, widget2);
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.VisualDefinition, widget2);
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000CA54 File Offset: 0x0000AC54
		internal void OnWidgetDisconnectedFromRoot(Widget widget)
		{
			widget.HandleOnDisconnectedFromRoot();
			if (widget == this.DraggedWidget && this.DraggedWidget.DragWidget != null)
			{
				this.ReleaseDraggedWidget();
				this.ClearDragObject();
			}
			GauntletGamepadNavigationManager.Instance.OnWidgetDisconnectedFromRoot(widget);
			List<Widget> allChildrenAndThisRecursive = widget.GetAllChildrenAndThisRecursive();
			for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
			{
				Widget widget2 = allChildrenAndThisRecursive[i];
				widget2.HandleOnDisconnectedFromRoot();
				this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.Update, widget2);
				this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.LateUpdate, widget2);
				this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.UpdateBrushes, widget2);
				this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.ParallelUpdate, widget2);
				this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.VisualDefinition, widget2);
				GauntletGamepadNavigationManager instance = GauntletGamepadNavigationManager.Instance;
				if (instance != null)
				{
					instance.OnWidgetDisconnectedFromRoot(widget2);
				}
				widget2.GamepadNavigationIndex = -1;
				widget2.UsedNavigationMovements = GamepadNavigationTypes.None;
				widget2.IsUsingNavigation = false;
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000CB0C File Offset: 0x0000AD0C
		internal void RegisterWidgetForEvent(WidgetContainer.ContainerType type, Widget widget)
		{
			if ((type == WidgetContainer.ContainerType.Update && widget.WidgetInfo.GotCustomUpdate) || (type == WidgetContainer.ContainerType.ParallelUpdate && widget.WidgetInfo.GotCustomParallelUpdate) || (type == WidgetContainer.ContainerType.LateUpdate && widget.WidgetInfo.GotCustomLateUpdate) || (type == WidgetContainer.ContainerType.VisualDefinition && widget.VisualDefinition != null) || (type == WidgetContainer.ContainerType.UpdateBrushes && widget.WidgetInfo.GotUpdateBrushes))
			{
				this._widgetContainers[type].Add(widget);
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000CB7C File Offset: 0x0000AD7C
		internal void UnRegisterWidgetForEvent(WidgetContainer.ContainerType type, Widget widget)
		{
			if ((type == WidgetContainer.ContainerType.Update && widget.WidgetInfo.GotCustomUpdate) || (type == WidgetContainer.ContainerType.ParallelUpdate && widget.WidgetInfo.GotCustomParallelUpdate) || (type == WidgetContainer.ContainerType.LateUpdate && widget.WidgetInfo.GotCustomLateUpdate) || (type == WidgetContainer.ContainerType.VisualDefinition && widget.VisualDefinition == null) || (type == WidgetContainer.ContainerType.UpdateBrushes && widget.WidgetInfo.GotUpdateBrushes))
			{
				this._widgetContainers[type].Remove(widget);
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000CBEA File Offset: 0x0000ADEA
		internal void OnWidgetVisualDefinitionChanged(Widget widget)
		{
			if (widget.VisualDefinition != null)
			{
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.VisualDefinition, widget);
				return;
			}
			this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.VisualDefinition, widget);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000CC05 File Offset: 0x0000AE05
		private void MeasureAll()
		{
			this.Root.Measure(this.PageSize);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000CC18 File Offset: 0x0000AE18
		private void LayoutAll(float left, float bottom, float right, float top)
		{
			this.Root.Layout(left, bottom, right, top);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000CC2C File Offset: 0x0000AE2C
		private void UpdatePositions()
		{
			this.AreaRectangle.LocalPosition = new Vector2(this.LeftUsableAreaStart, this.TopUsableAreaStart);
			this.AreaRectangle.LocalScale = new Vector2(this.PageSize.X, this.PageSize.Y);
			this.AreaRectangle.LocalPivot = new Vector2(0.5f, 0.5f);
			Rectangle2D invalid = Rectangle2D.Invalid;
			this.AreaRectangle.CalculateMatrixFrame(in invalid);
			this.Root.UpdatePosition();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000CCB4 File Offset: 0x0000AEB4
		internal void CalculateCanvas(Vector2 pageSize, float dt)
		{
			if (this._measureDirty > 0 || this._layoutDirty > 0)
			{
				this.PageSize = pageSize;
				Vec2 vec = new Vec2(pageSize.X / this.UsableArea.X, pageSize.Y / this.UsableArea.Y);
				this.LeftUsableAreaStart = (vec.X - vec.X * this.UsableArea.X) * 0.5f;
				this.TopUsableAreaStart = (vec.Y - vec.Y * this.UsableArea.Y) * 0.5f;
				this.AreaRectangle.LocalPosition = new Vector2(this.LeftUsableAreaStart, this.TopUsableAreaStart);
				this.AreaRectangle.LocalScale = new Vector2(this.PageSize.X, this.PageSize.Y);
				if (this._measureDirty > 0)
				{
					this.MeasureAll();
				}
				this.LayoutAll(0f, this.PageSize.Y, this.PageSize.X, 0f);
				this.UpdatePositions();
				if (this._measureDirty > 0)
				{
					this._measureDirty--;
				}
				if (this._layoutDirty > 0)
				{
					this._layoutDirty--;
				}
				this._positionsDirty = false;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000CE14 File Offset: 0x0000B014
		internal void RecalculateCanvas()
		{
			if (this._measureDirty == 2 || this._layoutDirty == 2)
			{
				if (this._measureDirty == 2)
				{
					this.MeasureAll();
				}
				this.LayoutAll(0f, this.PageSize.Y, this.PageSize.X, 0f);
				if (this._positionsDirty)
				{
					this.UpdatePositions();
					this._positionsDirty = false;
				}
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000CE80 File Offset: 0x0000B080
		internal void MouseDown()
		{
			this._mouseIsDown = true;
			this._lastClickPosition = this.MousePosition;
			Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MousePressed);
			if (widgetAtMousePositionForEvent != null)
			{
				this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.MousePressed, true);
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000CEB4 File Offset: 0x0000B0B4
		internal void MouseUp(bool isFromInput = true)
		{
			this._mouseIsDown = false;
			if (this.IsDragging)
			{
				if (this.DraggedWidget.PreviewEvent(GauntletEvent.DragEnd))
				{
					this.DispatchEvent(this.DraggedWidget, GauntletEvent.DragEnd, true);
				}
				Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.Drop);
				if (widgetAtMousePositionForEvent != null && isFromInput)
				{
					this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.Drop, true);
				}
				else
				{
					this.CancelAndReturnDrag();
				}
				if (this.DraggedWidget != null)
				{
					this.ClearDragObject();
					return;
				}
			}
			else
			{
				Widget widgetAtMousePositionForEvent2 = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MouseReleased);
				this.DispatchEvent(widgetAtMousePositionForEvent2, GauntletEvent.MouseReleased, isFromInput);
				this.LatestMouseUpWidget = widgetAtMousePositionForEvent2;
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000CF34 File Offset: 0x0000B134
		internal void MouseAlternateDown()
		{
			this._mouseAlternateIsDown = true;
			Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MouseAlternatePressed);
			if (widgetAtMousePositionForEvent != null)
			{
				this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.MouseAlternatePressed, true);
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000CF5C File Offset: 0x0000B15C
		internal void MouseAlternateUp(bool isFromInput = true)
		{
			this._mouseAlternateIsDown = false;
			Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MouseAlternateReleased);
			this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.MouseAlternateReleased, isFromInput);
			this.LatestMouseAlternateUpWidget = widgetAtMousePositionForEvent;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000CF88 File Offset: 0x0000B188
		internal void MouseScroll()
		{
			if (MathF.Abs(this.DeltaMouseScroll) > 0.001f)
			{
				Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MouseScroll);
				if (widgetAtMousePositionForEvent != null)
				{
					this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.MouseScroll, true);
				}
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
		internal void RightStickMovement()
		{
			if (Input.GetKeyState(InputKey.ControllerRStick).X != 0f || Input.GetKeyState(InputKey.ControllerRStick).Y != 0f)
			{
				Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.RightStickMovement);
				if (widgetAtMousePositionForEvent != null)
				{
					this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.RightStickMovement, true);
				}
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000D015 File Offset: 0x0000B215
		public void ClearFocus()
		{
			this.FocusedWidget = null;
			this.HoveredWidget = null;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000D028 File Offset: 0x0000B228
		private void CancelAndReturnDrag()
		{
			if (this._draggedWidgetPreviousParent != null)
			{
				this.DraggedWidget.ParentWidget = this._draggedWidgetPreviousParent;
				this.DraggedWidget.SetSiblingIndex(this._draggedWidgetIndex, false);
				this.DraggedWidget.PosOffset = new Vector2(0f, 0f);
				if (this.DraggedWidget.DragWidget != null)
				{
					this.DraggedWidget.DragWidget.ParentWidget = this.DraggedWidget;
					this.DraggedWidget.DragWidget.IsVisible = false;
				}
			}
			else
			{
				this.ReleaseDraggedWidget();
			}
			this._draggedWidgetPreviousParent = null;
			this._draggedWidgetIndex = -1;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000D0C8 File Offset: 0x0000B2C8
		private void ClearDragObject()
		{
			this.DraggedWidget = null;
			Action onDragEnded = this.OnDragEnded;
			if (onDragEnded != null)
			{
				onDragEnded();
			}
			this._dragOffset = new Vector2(0f, 0f);
			this._dragCarrier.ParentWidget = null;
			this._dragCarrier = null;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000D118 File Offset: 0x0000B318
		internal void MouseMove()
		{
			if (this._mouseIsDown)
			{
				if (this.IsDragging)
				{
					Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.DragHover);
					if (widgetAtMousePositionForEvent != null)
					{
						this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.DragHover, true);
					}
					else
					{
						this.DragHoveredWidget = null;
					}
				}
				else if (this.LatestMouseDownWidget != null)
				{
					if (this.LatestMouseDownWidget.PreviewEvent(GauntletEvent.MouseMove))
					{
						this.DispatchEvent(this.LatestMouseDownWidget, GauntletEvent.MouseMove, true);
					}
					if (!this.IsDragging && this.LatestMouseDownWidget.PreviewEvent(GauntletEvent.DragBegin))
					{
						Vector2 vector = this._lastClickPosition - this.MousePosition;
						Vector2 vector2 = new Vector2(vector.X, vector.Y);
						if (vector2.LengthSquared() > 100f * this.Context.Scale)
						{
							this.DispatchEvent(this.LatestMouseDownWidget, GauntletEvent.DragBegin, true);
						}
					}
				}
			}
			else if (!this._mouseAlternateIsDown)
			{
				Widget widgetAtMousePositionForEvent2 = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MouseMove);
				if (widgetAtMousePositionForEvent2 != null)
				{
					this.DispatchEvent(widgetAtMousePositionForEvent2, GauntletEvent.MouseMove, true);
				}
			}
			List<Widget> list = new List<Widget>();
			List<Widget> list2 = new List<Widget>();
			EventManager.CollectEnableWidgetsAt(this.Root, this.MousePosition, list2);
			for (int i = 0; i < list2.Count; i++)
			{
				Widget widget = list2[i];
				if (!this.MouseOveredWidgets.Contains(widget))
				{
					widget.OnMouseOverBegin();
					GauntletGamepadNavigationManager instance = GauntletGamepadNavigationManager.Instance;
					if (instance != null)
					{
						instance.OnWidgetHoverBegin(widget);
					}
				}
				list.Add(widget);
			}
			for (int j = 0; j < this.MouseOveredWidgets.Count; j++)
			{
				Widget widget2 = this.MouseOveredWidgets[j];
				if (!list.Contains(widget2))
				{
					widget2.OnMouseOverEnd();
					if (widget2.GamepadNavigationIndex != -1)
					{
						GauntletGamepadNavigationManager instance2 = GauntletGamepadNavigationManager.Instance;
						if (instance2 != null)
						{
							instance2.OnWidgetHoverEnd(widget2);
						}
					}
				}
			}
			this.MouseOveredWidgets = list;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000D2D2 File Offset: 0x0000B4D2
		private static bool IsPointInsideMeasuredArea(Widget w, Vector2 p)
		{
			return w.AreaRect.IsPointInside(in p);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000D2E1 File Offset: 0x0000B4E1
		public bool IsPointInsideUsableArea(Vector2 p)
		{
			return this.AreaRectangle.IsPointInside(in p);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000D2F0 File Offset: 0x0000B4F0
		private Widget GetWidgetAtMousePositionForEvent(GauntletEvent gauntletEvent)
		{
			if (!this.GetIsHitThisFrame())
			{
				return null;
			}
			return this.GetWidgetAtPositionForEvent(gauntletEvent, this.MousePosition);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000D30C File Offset: 0x0000B50C
		private Widget GetWidgetAtPositionForEvent(GauntletEvent gauntletEvent, Vector2 pointerPosition)
		{
			Widget widget = null;
			List<Widget> list = new List<Widget>();
			EventManager.CollectEnableWidgetsAt(this.Root, pointerPosition, list);
			for (int i = 0; i < list.Count; i++)
			{
				Widget widget2 = list[i];
				if (widget2.PreviewEvent(gauntletEvent))
				{
					widget = widget2;
					break;
				}
			}
			return widget;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002AB RID: 683 RVA: 0x0000D358 File Offset: 0x0000B558
		// (remove) Token: 0x060002AC RID: 684 RVA: 0x0000D390 File Offset: 0x0000B590
		public event Action OnFocusedWidgetChanged;

		// Token: 0x060002AD RID: 685 RVA: 0x0000D3C8 File Offset: 0x0000B5C8
		private void DispatchEvent(Widget selectedWidget, GauntletEvent gauntletEvent, bool isFromInput = true)
		{
			if (gauntletEvent != GauntletEvent.MouseReleased)
			{
			}
			switch (gauntletEvent)
			{
			case GauntletEvent.MouseMove:
				selectedWidget.OnMouseMove();
				this.HoveredWidget = selectedWidget;
				return;
			case GauntletEvent.MousePressed:
				this.LatestMouseDownWidget = selectedWidget;
				selectedWidget.OnMousePressed();
				this.FocusedWidget = selectedWidget;
				return;
			case GauntletEvent.MouseReleased:
				if (this.LatestMouseDownWidget != selectedWidget)
				{
					Widget latestMouseDownWidget = this.LatestMouseDownWidget;
					if (latestMouseDownWidget != null)
					{
						latestMouseDownWidget.OnMouseReleased(isFromInput);
					}
				}
				if (selectedWidget != null)
				{
					selectedWidget.OnMouseReleased(isFromInput);
					return;
				}
				break;
			case GauntletEvent.MouseAlternatePressed:
				this.LatestMouseAlternateDownWidget = selectedWidget;
				selectedWidget.OnMouseAlternatePressed();
				this.FocusedWidget = selectedWidget;
				return;
			case GauntletEvent.MouseAlternateReleased:
				if (this.LatestMouseAlternateDownWidget != selectedWidget)
				{
					Widget latestMouseAlternateDownWidget = this.LatestMouseAlternateDownWidget;
					if (latestMouseAlternateDownWidget != null)
					{
						latestMouseAlternateDownWidget.OnMouseAlternateReleased(isFromInput);
					}
				}
				if (selectedWidget != null)
				{
					selectedWidget.OnMouseAlternateReleased(isFromInput);
					return;
				}
				break;
			case GauntletEvent.DragHover:
				this.DragHoveredWidget = selectedWidget;
				return;
			case GauntletEvent.DragBegin:
				selectedWidget.OnDragBegin();
				return;
			case GauntletEvent.DragEnd:
				selectedWidget.OnDragEnd();
				return;
			case GauntletEvent.Drop:
				selectedWidget.OnDrop();
				return;
			case GauntletEvent.MouseScroll:
				selectedWidget.OnMouseScroll();
				return;
			case GauntletEvent.RightStickMovement:
				selectedWidget.OnRightStickMovement();
				break;
			default:
				return;
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000D4BF File Offset: 0x0000B6BF
		public static bool HitTest(Widget widget, Vector2 position)
		{
			if (widget == null)
			{
				Debug.FailedAssert("Calling HitTest using null widget!", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\EventManager.cs", "HitTest", 977);
				return false;
			}
			return EventManager.AnyWidgetsAt(widget, position);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000D4E8 File Offset: 0x0000B6E8
		public bool FocusTest(Widget root)
		{
			for (Widget widget = this.FocusedWidget; widget != null; widget = widget.ParentWidget)
			{
				if (root == widget)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000D510 File Offset: 0x0000B710
		private static bool AnyWidgetsAt(Widget widget, Vector2 position)
		{
			if (widget.IsEnabled && widget.IsVisible)
			{
				if (!widget.DoNotAcceptEvents && EventManager.IsPointInsideMeasuredArea(widget, position))
				{
					return true;
				}
				if (!widget.DoNotPassEventsToChildren)
				{
					for (int i = widget.ChildCount - 1; i >= 0; i--)
					{
						Widget child = widget.GetChild(i);
						if (!child.IsHidden && !child.IsDisabled && EventManager.AnyWidgetsAt(child, position))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000D580 File Offset: 0x0000B780
		private static void CollectEnableWidgetsAt(Widget widget, Vector2 position, List<Widget> widgets)
		{
			if (widget.IsEnabled && widget.IsVisible)
			{
				if (!widget.DoNotPassEventsToChildren)
				{
					for (int i = widget.ChildCount - 1; i >= 0; i--)
					{
						Widget child = widget.GetChild(i);
						if (!child.IsHidden && !child.IsDisabled && EventManager.IsPointInsideMeasuredArea(child, position))
						{
							EventManager.CollectEnableWidgetsAt(child, position, widgets);
						}
					}
				}
				if (!widget.DoNotAcceptEvents && EventManager.IsPointInsideMeasuredArea(widget, position))
				{
					widgets.Add(widget);
				}
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000D5FC File Offset: 0x0000B7FC
		private static void CollectVisibleWidgetsAt(Widget widget, Vector2 position, List<Widget> widgets)
		{
			if (widget.IsVisible)
			{
				for (int i = widget.ChildCount - 1; i >= 0; i--)
				{
					Widget child = widget.GetChild(i);
					if (child.IsVisible && EventManager.IsPointInsideMeasuredArea(child, position))
					{
						EventManager.CollectVisibleWidgetsAt(child, position, widgets);
					}
				}
				if (EventManager.IsPointInsideMeasuredArea(widget, position))
				{
					widgets.Add(widget);
				}
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000D658 File Offset: 0x0000B858
		internal void ManualAddRange(List<Widget> list, LinkedList<Widget> linked_list)
		{
			if (list.Capacity < linked_list.Count)
			{
				list.Capacity = linked_list.Count;
			}
			for (LinkedListNode<Widget> linkedListNode = linked_list.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				list.Add(linkedListNode.Value);
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000D6A0 File Offset: 0x0000B8A0
		private void ParallelUpdateWidget(int startInclusive, int endExclusive, float dt)
		{
			MBReadOnlyList<Widget> activeList = this._widgetContainers[WidgetContainer.ContainerType.ParallelUpdate].GetActiveList();
			for (int i = startInclusive; i < endExclusive; i++)
			{
				activeList[i].ParallelUpdate(dt);
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000D6D8 File Offset: 0x0000B8D8
		internal void ParallelUpdateWidgets(float dt)
		{
			WidgetContainer widgetContainer = this._widgetContainers[WidgetContainer.ContainerType.ParallelUpdate];
			TWParallel.ForWithoutRenderThreadDt(0, widgetContainer.Count, dt, this.ParallelUpdateWidgetPredicate, 16);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000D708 File Offset: 0x0000B908
		internal void Update(float dt)
		{
			this.Time += dt;
			this.CachedDt = dt;
			this.IsControllerActive = Input.IsGamepadActive;
			this.DefragContainers();
			MBReadOnlyList<Widget> activeList = this._widgetContainers[WidgetContainer.ContainerType.VisualDefinition].GetActiveList();
			for (int i = 0; i < activeList.Count; i++)
			{
				activeList[i].UpdateVisualDefinitions(dt);
			}
			this.UpdateDragCarrier();
			Widget hoveredWidget = this.HoveredWidget;
			UIContext.MouseCursors mouseCursors = ((((hoveredWidget != null) ? hoveredWidget.HoveredCursorState : null) != null) ? ((UIContext.MouseCursors)Enum.Parse(typeof(UIContext.MouseCursors), this.HoveredWidget.HoveredCursorState)) : UIContext.MouseCursors.Default);
			this.Context.ActiveCursorOfContext = mouseCursors;
			MBReadOnlyList<Widget> activeList2 = this._widgetContainers[WidgetContainer.ContainerType.Update].GetActiveList();
			for (int j = 0; j < activeList2.Count; j++)
			{
				activeList2[j].Update(dt);
			}
			this._doingParallelTask = true;
			this.DefragContainers();
			WidgetContainer widgetContainer = this._widgetContainers[WidgetContainer.ContainerType.ParallelUpdate];
			if (widgetContainer.Count > 64)
			{
				this.ParallelUpdateWidgets(dt);
			}
			else
			{
				MBReadOnlyList<Widget> activeList3 = widgetContainer.GetActiveList();
				for (int k = 0; k < activeList3.Count; k++)
				{
					activeList3[k].ParallelUpdate(dt);
				}
			}
			this._doingParallelTask = false;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000D854 File Offset: 0x0000BA54
		internal void DefragContainers()
		{
			foreach (KeyValuePair<WidgetContainer.ContainerType, WidgetContainer> keyValuePair in this._widgetContainers)
			{
				keyValuePair.Value.Defrag();
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000D8AC File Offset: 0x0000BAAC
		internal void ParallelUpdateBrushes(float dt)
		{
			WidgetContainer widgetContainer = this._widgetContainers[WidgetContainer.ContainerType.UpdateBrushes];
			TWParallel.ForWithoutRenderThreadDt(0, widgetContainer.Count, dt, this.UpdateBrushesWidgetPredicate, 16);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000D8DC File Offset: 0x0000BADC
		internal void UpdateBrushes(float dt)
		{
			WidgetContainer widgetContainer = this._widgetContainers[WidgetContainer.ContainerType.UpdateBrushes];
			if (widgetContainer.Count > 64)
			{
				this.ParallelUpdateBrushes(dt);
				return;
			}
			MBReadOnlyList<Widget> activeList = widgetContainer.GetActiveList();
			for (int i = 0; i < activeList.Count; i++)
			{
				activeList[i].UpdateBrushes(dt);
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000D930 File Offset: 0x0000BB30
		private void UpdateBrushesWidget(int startInclusive, int endExclusive, float dt)
		{
			MBReadOnlyList<Widget> activeList = this._widgetContainers[WidgetContainer.ContainerType.UpdateBrushes].GetActiveList();
			for (int i = startInclusive; i < endExclusive; i++)
			{
				activeList[i].UpdateBrushes(dt);
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000D968 File Offset: 0x0000BB68
		public void AddLateUpdateAction(Widget owner, Action<float> action, int order)
		{
			UpdateAction updateAction = default(UpdateAction);
			updateAction.Target = owner;
			updateAction.Action = action;
			updateAction.Order = order;
			if (this._doingParallelTask)
			{
				object lateUpdateActionLocker = this._lateUpdateActionLocker;
				lock (lateUpdateActionLocker)
				{
					this._lateUpdateActions[order].Add(updateAction);
					return;
				}
			}
			this._lateUpdateActions[order].Add(updateAction);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000D9F0 File Offset: 0x0000BBF0
		internal void LateUpdate(float dt)
		{
			this.DefragContainers();
			MBReadOnlyList<Widget> activeList = this._widgetContainers[WidgetContainer.ContainerType.LateUpdate].GetActiveList();
			for (int i = 0; i < activeList.Count; i++)
			{
				activeList[i].LateUpdate(dt);
			}
			Dictionary<int, List<UpdateAction>> lateUpdateActions = this._lateUpdateActions;
			this._lateUpdateActions = this._lateUpdateActionsRunning;
			this._lateUpdateActionsRunning = lateUpdateActions;
			for (int j = 1; j <= 5; j++)
			{
				List<UpdateAction> list = this._lateUpdateActionsRunning[j];
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k].Target.ConnectedToRoot)
					{
						list[k].Action(dt);
					}
				}
				list.Clear();
			}
			if (this.IsControllerActive)
			{
				if (this.HoveredWidget != null && this.HoveredWidget.IsRecursivelyVisible())
				{
					if (this.HoveredWidget.FrictionEnabled && this.DraggedWidget == null)
					{
						this._lastSetFrictionValue = 0.45f;
					}
					else
					{
						this._lastSetFrictionValue = 1f;
					}
					Input.SetCursorFriction(this._lastSetFrictionValue);
				}
				if (!this._lastSetFrictionValue.ApproximatelyEqualsTo(1f, 1E-05f) && this.HoveredWidget == null)
				{
					this._lastSetFrictionValue = 1f;
					Input.SetCursorFriction(this._lastSetFrictionValue);
				}
			}
			if (this._isOnScreenKeyboardRequested)
			{
				EditableTextWidget editableTextWidget;
				if (this.IsControllerActive && (editableTextWidget = this.FocusedWidget as EditableTextWidget) != null)
				{
					string text = editableTextWidget.Text ?? string.Empty;
					string text2 = editableTextWidget.KeyboardInfoText ?? string.Empty;
					int maxLength = editableTextWidget.MaxLength;
					int num = (editableTextWidget.IsObfuscationEnabled ? 2 : 0);
					if (this.FocusedWidget is IntegerInputTextWidget || this.FocusedWidget is FloatInputTextWidget)
					{
						num = 1;
					}
					this.Context.TwoDimensionContext.Platform.OpenOnScreenKeyboard(text, text2, maxLength, num);
				}
				this._isOnScreenKeyboardRequested = false;
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000DBE0 File Offset: 0x0000BDE0
		private void UpdateDragCarrier()
		{
			if (this._dragCarrier != null)
			{
				this._dragCarrier.PosOffset = this.MousePositionInReferenceResolution + this._dragOffset - new Vector2(this.LeftUsableAreaStart, this.TopUsableAreaStart) * this.Context.InverseScale;
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000DC38 File Offset: 0x0000BE38
		internal void BeginDragging(Widget draggedObject)
		{
			if (this.DraggedWidget != null)
			{
				Debug.FailedAssert("Trying to BeginDragging while there is already a dragged object.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\EventManager.cs", "BeginDragging", 1382);
				this.ClearDragObject();
			}
			if (!draggedObject.ConnectedToRoot)
			{
				Debug.FailedAssert("Trying to drag a widget with no parent, possibly a widget which is already being dragged", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\EventManager.cs", "BeginDragging", 1388);
				return;
			}
			draggedObject.IsPressed = false;
			this._draggedWidgetPreviousParent = null;
			this._draggedWidgetIndex = -1;
			Widget parentWidget = draggedObject.ParentWidget;
			this.DraggedWidget = draggedObject;
			Vector2 globalPosition = this.DraggedWidget.GlobalPosition;
			this._dragCarrier = new DragCarrierWidget(this.Context);
			this._dragCarrier.ParentWidget = this.Root;
			if (draggedObject.DragWidget != null)
			{
				Widget dragWidget = draggedObject.DragWidget;
				this._dragCarrier.WidthSizePolicy = SizePolicy.CoverChildren;
				this._dragCarrier.HeightSizePolicy = SizePolicy.CoverChildren;
				this._dragOffset = Vector2.Zero;
				dragWidget.IsVisible = true;
				dragWidget.ParentWidget = this._dragCarrier;
				if (this.DraggedWidget.HideOnDrag)
				{
					this.DraggedWidget.IsVisible = false;
				}
				this._draggedWidgetPreviousParent = null;
			}
			else
			{
				this._dragOffset = (globalPosition - this.MousePosition) * this.Context.InverseScale;
				this._dragCarrier.WidthSizePolicy = SizePolicy.Fixed;
				this._dragCarrier.HeightSizePolicy = SizePolicy.Fixed;
				if (this.DraggedWidget.WidthSizePolicy == SizePolicy.StretchToParent)
				{
					this._dragCarrier.ScaledSuggestedWidth = this.DraggedWidget.Size.X + (this.DraggedWidget.MarginRight + this.DraggedWidget.MarginLeft) * this.Context.Scale;
					this._dragOffset += new Vector2(-this.DraggedWidget.MarginLeft, 0f);
				}
				else
				{
					this._dragCarrier.ScaledSuggestedWidth = this.DraggedWidget.Size.X;
				}
				if (this.DraggedWidget.HeightSizePolicy == SizePolicy.StretchToParent)
				{
					this._dragCarrier.ScaledSuggestedHeight = this.DraggedWidget.Size.Y + (this.DraggedWidget.MarginTop + this.DraggedWidget.MarginBottom) * this.Context.Scale;
					this._dragOffset += new Vector2(0f, -this.DraggedWidget.MarginTop);
				}
				else
				{
					this._dragCarrier.ScaledSuggestedHeight = this.DraggedWidget.Size.Y;
				}
				if (parentWidget != null)
				{
					this._draggedWidgetPreviousParent = parentWidget;
					this._draggedWidgetIndex = draggedObject.GetSiblingIndex();
				}
				this.DraggedWidget.ParentWidget = this._dragCarrier;
			}
			this._dragCarrier.PosOffset = this.MousePositionInReferenceResolution + this._dragOffset - new Vector2(this.LeftUsableAreaStart, this.TopUsableAreaStart) * this.Context.InverseScale;
			Action onDragStarted = this.OnDragStarted;
			if (onDragStarted == null)
			{
				return;
			}
			onDragStarted();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000DF1C File Offset: 0x0000C11C
		internal Widget ReleaseDraggedWidget()
		{
			Widget draggedWidget = this.DraggedWidget;
			if (this._draggedWidgetPreviousParent != null)
			{
				this.DraggedWidget.ParentWidget = this._draggedWidgetPreviousParent;
				this._draggedWidgetIndex = MathF.Max(0, MathF.Min(MathF.Max(0, this.DraggedWidget.ParentWidget.ChildCount - 1), this._draggedWidgetIndex));
				this.DraggedWidget.SetSiblingIndex(this._draggedWidgetIndex, false);
			}
			else
			{
				this.DraggedWidget.IsVisible = true;
			}
			this.DragHoveredWidget = null;
			return draggedWidget;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000DFA0 File Offset: 0x0000C1A0
		internal void Render(TwoDimensionContext twoDimensionContext)
		{
			twoDimensionContext.ResetScissor();
			SimpleRectangle boundingBox = this.AreaRectangle.GetBoundingBox();
			twoDimensionContext.SetScissor(new ScissorTestInfo(boundingBox.X, boundingBox.Y, boundingBox.X2, boundingBox.Y2));
			this._drawContext.Reset();
			this.Root.Render(twoDimensionContext, this._drawContext);
			this._drawContext.DrawTo(twoDimensionContext);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000E00B File Offset: 0x0000C20B
		public void UpdateLayout()
		{
			this.SetMeasureDirty();
			this.SetLayoutDirty();
			this.Root.LayoutUpdated();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000E024 File Offset: 0x0000C224
		internal void SetMeasureDirty()
		{
			this._measureDirty = 2;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000E02D File Offset: 0x0000C22D
		internal void SetLayoutDirty()
		{
			this._layoutDirty = 2;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000E036 File Offset: 0x0000C236
		internal void SetPositionsDirty()
		{
			this._positionsDirty = true;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000E03F File Offset: 0x0000C23F
		public bool GetIsHitThisFrame()
		{
			return this.OnGetIsHitThisFrame != null && this.OnGetIsHitThisFrame();
		}

		// Token: 0x0400013A RID: 314
		public const int MinParallelUpdateCount = 64;

		// Token: 0x0400013B RID: 315
		private const int DirtyCount = 2;

		// Token: 0x0400013C RID: 316
		private const float DragStartThreshold = 100f;

		// Token: 0x0400013D RID: 317
		private const float ScrollScale = 0.4f;

		// Token: 0x04000141 RID: 321
		public Rectangle2D AreaRectangle;

		// Token: 0x04000147 RID: 327
		private List<Action> _onAfterFinalizedCallbacks;

		// Token: 0x04000149 RID: 329
		private Widget _focusedWidget;

		// Token: 0x0400014A RID: 330
		private Widget _hoveredWidget;

		// Token: 0x0400014B RID: 331
		private List<Widget> _mouseOveredWidgets;

		// Token: 0x0400014C RID: 332
		private Widget _dragHoveredWidget;

		// Token: 0x0400014E RID: 334
		private Widget _latestMouseDownWidget;

		// Token: 0x0400014F RID: 335
		private Widget _latestMouseUpWidget;

		// Token: 0x04000150 RID: 336
		private Widget _latestMouseAlternateDownWidget;

		// Token: 0x04000151 RID: 337
		private Widget _latestMouseAlternateUpWidget;

		// Token: 0x04000152 RID: 338
		private int _measureDirty;

		// Token: 0x04000153 RID: 339
		private int _layoutDirty;

		// Token: 0x04000154 RID: 340
		private bool _positionsDirty;

		// Token: 0x04000155 RID: 341
		private const int _stickMovementScaleAmount = 3000;

		// Token: 0x04000157 RID: 343
		private Vector2 _lastClickPosition;

		// Token: 0x04000158 RID: 344
		private bool _mouseIsDown;

		// Token: 0x04000159 RID: 345
		private bool _mouseAlternateIsDown;

		// Token: 0x0400015A RID: 346
		private Vector2 _dragOffset = new Vector2(0f, 0f);

		// Token: 0x0400015B RID: 347
		private Widget _draggedWidgetPreviousParent;

		// Token: 0x0400015C RID: 348
		private int _draggedWidgetIndex;

		// Token: 0x0400015D RID: 349
		private DragCarrierWidget _dragCarrier;

		// Token: 0x0400015E RID: 350
		private object _lateUpdateActionLocker;

		// Token: 0x0400015F RID: 351
		private Dictionary<int, List<UpdateAction>> _lateUpdateActions;

		// Token: 0x04000160 RID: 352
		private Dictionary<int, List<UpdateAction>> _lateUpdateActionsRunning;

		// Token: 0x04000161 RID: 353
		private Dictionary<WidgetContainer.ContainerType, WidgetContainer> _widgetContainers;

		// Token: 0x04000162 RID: 354
		private const int UpdateActionOrderCount = 5;

		// Token: 0x04000163 RID: 355
		private volatile bool _doingParallelTask;

		// Token: 0x04000164 RID: 356
		private TwoDimensionDrawContext _drawContext;

		// Token: 0x04000165 RID: 357
		private readonly TWParallel.ParallelForWithDtAuxPredicate ParallelUpdateWidgetPredicate;

		// Token: 0x04000166 RID: 358
		private readonly TWParallel.ParallelForWithDtAuxPredicate UpdateBrushesWidgetPredicate;

		// Token: 0x04000167 RID: 359
		private float _lastSetFrictionValue = 1f;

		// Token: 0x04000168 RID: 360
		private bool _isOnScreenKeyboardRequested;

		// Token: 0x0400016A RID: 362
		public Func<bool> OnGetIsHitThisFrame;
	}
}
