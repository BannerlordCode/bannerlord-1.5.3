using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Chat
{
	// Token: 0x0200017E RID: 382
	public class ChatLogWidget : Widget
	{
		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x000365BE File Offset: 0x000347BE
		private float _resizeTransitionTime
		{
			get
			{
				return 0.14f;
			}
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x000365C5 File Offset: 0x000347C5
		public ChatLogWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x000365DC File Offset: 0x000347DC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this.IsChatDisabled && this.TextInputWidget != null && this.FullyShowChatWithTyping && this._focusOnNextUpdate)
			{
				base.EventManager.FocusedWidget = this.TextInputWidget;
				this._focusOnNextUpdate = false;
			}
			if (!this.FullyShowChat)
			{
				this.ScrollablePanel.ResetTweenSpeed();
				this.Scrollbar.ValueFloat = this.Scrollbar.MaxValue;
			}
			base.ParentWidget.DoNotPassEventsToChildren = !this.FullyShowChat;
			if (this.ResizerWidget != null && this.ResizeFrameWidget != null)
			{
				this.UpdateResize(dt);
			}
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0003667C File Offset: 0x0003487C
		private void UpdateResize(float dt)
		{
			if (Input.IsKeyPressed(InputKey.LeftMouseButton) && base.EventManager.HoveredWidget == this.ResizerWidget)
			{
				this._isResizing = true;
				this._resizeStartMousePosition = Input.MousePositionPixel;
				this._resizeOriginalSize = new Vec2(this.SizeX, this.SizeY);
				this.ResizeFrameWidget.IsVisible = true;
				this.ResizeFrameWidget.WidthSizePolicy = SizePolicy.Fixed;
				this.ResizeFrameWidget.HeightSizePolicy = SizePolicy.Fixed;
				this.ResizeFrameWidget.SuggestedHeight = this.SizeY;
				this.ResizeFrameWidget.SuggestedWidth = this.SizeX;
				this._innerPanelDefaultSizePolicies = new ValueTuple<SizePolicy, SizePolicy>(this.ScrollablePanel.InnerPanel.WidthSizePolicy, this.ScrollablePanel.InnerPanel.HeightSizePolicy);
				this.ScrollablePanel.InnerPanel.WidthSizePolicy = SizePolicy.Fixed;
				this.ScrollablePanel.InnerPanel.HeightSizePolicy = SizePolicy.Fixed;
				this.ScrollablePanel.InnerPanel.SuggestedWidth = this.ScrollablePanel.InnerPanel.Size.X;
				this.ScrollablePanel.InnerPanel.SuggestedHeight = this.ScrollablePanel.InnerPanel.Size.Y;
			}
			else if (Input.IsKeyReleased(InputKey.LeftMouseButton))
			{
				if (this._isResizing)
				{
					this.ResizeFrameWidget.IsVisible = false;
					this._resizeActualPanel = true;
					this._lerpRatio = 0f;
				}
				this._isResizing = false;
			}
			if (this._isResizing)
			{
				Vec2 vec = this._resizeOriginalSize + new Vec2((Input.MousePositionPixel - this._resizeStartMousePosition).X, -(Input.MousePositionPixel - this._resizeStartMousePosition).Y);
				this.ResizeFrameWidget.SuggestedWidth = Mathf.Clamp(vec.X, base.MinWidth, base.MaxWidth);
				this.ResizeFrameWidget.SuggestedHeight = Mathf.Clamp(vec.Y, base.MinHeight, base.MaxHeight) - this.ResizeFrameWidget.MarginBottom;
				return;
			}
			if (this._resizeActualPanel)
			{
				this._lerpRatio = Mathf.Clamp(this._lerpRatio + dt / this._resizeTransitionTime, 0f, 1f);
				this.SizeX = Mathf.Lerp(this._resizeOriginalSize.x, this.ResizeFrameWidget.SuggestedWidth, this._lerpRatio);
				this.SizeY = Mathf.Lerp(this._resizeOriginalSize.y, this.ResizeFrameWidget.SuggestedHeight + this.ResizeFrameWidget.MarginBottom, this._lerpRatio);
				if (this.SizeX.ApproximatelyEqualsTo(this.ResizeFrameWidget.SuggestedWidth, 0.01f) && this.SizeY.ApproximatelyEqualsTo(this.ResizeFrameWidget.SuggestedHeight + this.ResizeFrameWidget.MarginBottom, 0.01f))
				{
					this.SizeX = this.ResizeFrameWidget.SuggestedWidth;
					this.SizeY = this.ResizeFrameWidget.SuggestedHeight + this.ResizeFrameWidget.MarginBottom;
					this.ResizeFrameWidget.WidthSizePolicy = SizePolicy.StretchToParent;
					this.ResizeFrameWidget.HeightSizePolicy = SizePolicy.StretchToParent;
					this.ScrollablePanel.InnerPanel.WidthSizePolicy = this._innerPanelDefaultSizePolicies.Item1;
					this.ScrollablePanel.InnerPanel.HeightSizePolicy = this._innerPanelDefaultSizePolicies.Item2;
					this._resizeActualPanel = false;
					base.EventFired("FinishResize", Array.Empty<object>());
					return;
				}
			}
			else if (!this._isInitialized)
			{
				this.SizeX = base.SuggestedWidth;
				this.SizeY = base.SuggestedHeight;
				this._isInitialized = true;
			}
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00036A1F File Offset: 0x00034C1F
		public void RegisterMultiLineElement(ChatCollapsableListPanel element)
		{
			if (!this._registeredMultilineWidgets.Contains(element))
			{
				this._registeredMultilineWidgets.Add(element);
			}
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00036A3B File Offset: 0x00034C3B
		public void RemoveMultiLineElement(ChatCollapsableListPanel element)
		{
			if (this._registeredMultilineWidgets.Contains(element))
			{
				this._registeredMultilineWidgets.Remove(element);
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00036A58 File Offset: 0x00034C58
		// (set) Token: 0x060013F9 RID: 5113 RVA: 0x00036A60 File Offset: 0x00034C60
		[DataSourceProperty]
		public bool IsChatDisabled
		{
			get
			{
				return this._isChatDisabled;
			}
			set
			{
				if (value != this._isChatDisabled)
				{
					this._isChatDisabled = value;
					base.OnPropertyChanged(value, "IsChatDisabled");
				}
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x00036A7E File Offset: 0x00034C7E
		// (set) Token: 0x060013FB RID: 5115 RVA: 0x00036A86 File Offset: 0x00034C86
		[DataSourceProperty]
		public bool FinishedResizing
		{
			get
			{
				return this._finishedResizing;
			}
			set
			{
				if (value != this._finishedResizing)
				{
					this._finishedResizing = value;
					base.OnPropertyChanged(value, "FinishedResizing");
				}
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x00036AA4 File Offset: 0x00034CA4
		// (set) Token: 0x060013FD RID: 5117 RVA: 0x00036AAC File Offset: 0x00034CAC
		[DataSourceProperty]
		public bool FullyShowChat
		{
			get
			{
				return this._fullyShowChat;
			}
			set
			{
				if (value != this._fullyShowChat)
				{
					this._fullyShowChat = value;
				}
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x00036ABE File Offset: 0x00034CBE
		// (set) Token: 0x060013FF RID: 5119 RVA: 0x00036AC8 File Offset: 0x00034CC8
		[DataSourceProperty]
		public bool FullyShowChatWithTyping
		{
			get
			{
				return this._fullyShowChatWithTyping;
			}
			set
			{
				if (value != this._fullyShowChatWithTyping)
				{
					this._fullyShowChatWithTyping = value;
					if (!this.IsChatDisabled && this.TextInputWidget != null && this._fullyShowChatWithTyping)
					{
						this._focusOnNextUpdate = true;
					}
					base.EventManager.FocusedWidget = null;
					base.OnPropertyChanged(value, "FullyShowChatWithTyping");
				}
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x00036B1C File Offset: 0x00034D1C
		// (set) Token: 0x06001401 RID: 5121 RVA: 0x00036B24 File Offset: 0x00034D24
		[DataSourceProperty]
		public EditableTextWidget TextInputWidget
		{
			get
			{
				return this._textInputWidget;
			}
			set
			{
				if (value != this._textInputWidget)
				{
					this._textInputWidget = value;
					base.OnPropertyChanged<EditableTextWidget>(value, "TextInputWidget");
				}
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06001402 RID: 5122 RVA: 0x00036B42 File Offset: 0x00034D42
		// (set) Token: 0x06001403 RID: 5123 RVA: 0x00036B4A File Offset: 0x00034D4A
		[DataSourceProperty]
		public ScrollbarWidget Scrollbar
		{
			get
			{
				return this._scrollbar;
			}
			set
			{
				if (value != this._scrollbar)
				{
					this._scrollbar = value;
					base.OnPropertyChanged<ScrollbarWidget>(value, "Scrollbar");
				}
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001404 RID: 5124 RVA: 0x00036B68 File Offset: 0x00034D68
		// (set) Token: 0x06001405 RID: 5125 RVA: 0x00036B70 File Offset: 0x00034D70
		[DataSourceProperty]
		public ScrollablePanel ScrollablePanel
		{
			get
			{
				return this._scrollablePanel;
			}
			set
			{
				if (value != this._scrollablePanel)
				{
					this._scrollablePanel = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "ScrollablePanel");
				}
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x00036B8E File Offset: 0x00034D8E
		// (set) Token: 0x06001407 RID: 5127 RVA: 0x00036B96 File Offset: 0x00034D96
		[DataSourceProperty]
		public Widget ResizerWidget
		{
			get
			{
				return this._resizerWidget;
			}
			set
			{
				if (value != this._resizerWidget)
				{
					this._resizerWidget = value;
					base.OnPropertyChanged<Widget>(value, "ResizerWidget");
				}
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06001408 RID: 5128 RVA: 0x00036BB4 File Offset: 0x00034DB4
		// (set) Token: 0x06001409 RID: 5129 RVA: 0x00036BBC File Offset: 0x00034DBC
		[DataSourceProperty]
		public Widget ResizeFrameWidget
		{
			get
			{
				return this._resizeFrameWidget;
			}
			set
			{
				if (value != this._resizeFrameWidget)
				{
					this._resizeFrameWidget = value;
					base.OnPropertyChanged<Widget>(value, "ResizeFrameWidget");
				}
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x00036BDA File Offset: 0x00034DDA
		// (set) Token: 0x0600140B RID: 5131 RVA: 0x00036BE2 File Offset: 0x00034DE2
		[DataSourceProperty]
		public float SizeX
		{
			get
			{
				return this._sizeX;
			}
			set
			{
				if (value != this._sizeX)
				{
					this._sizeX = value;
					base.SuggestedWidth = value;
					base.OnPropertyChanged(value, "SizeX");
				}
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x00036C07 File Offset: 0x00034E07
		// (set) Token: 0x0600140D RID: 5133 RVA: 0x00036C0F File Offset: 0x00034E0F
		[DataSourceProperty]
		public float SizeY
		{
			get
			{
				return this._sizeY;
			}
			set
			{
				if (value != this._sizeY)
				{
					this._sizeY = value;
					base.SuggestedHeight = value;
					base.OnPropertyChanged(value, "SizeY");
				}
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x00036C34 File Offset: 0x00034E34
		// (set) Token: 0x0600140F RID: 5135 RVA: 0x00036C3C File Offset: 0x00034E3C
		[DataSourceProperty]
		public ListPanel MessageHistoryList
		{
			get
			{
				return this._messageHistoryList;
			}
			set
			{
				if (value != this._messageHistoryList)
				{
					this._messageHistoryList = value;
					base.OnPropertyChanged<ListPanel>(value, "MessageHistoryList");
				}
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x00036C5A File Offset: 0x00034E5A
		// (set) Token: 0x06001411 RID: 5137 RVA: 0x00036C62 File Offset: 0x00034E62
		[DataSourceProperty]
		public bool IsMPChatLog
		{
			get
			{
				return this._isMPChatLog;
			}
			set
			{
				if (value != this._isMPChatLog)
				{
					this._isMPChatLog = value;
					base.OnPropertyChanged(value, "IsMPChatLog");
				}
			}
		}

		// Token: 0x0400090F RID: 2319
		private List<ChatCollapsableListPanel> _registeredMultilineWidgets = new List<ChatCollapsableListPanel>();

		// Token: 0x04000910 RID: 2320
		private bool _isInitialized;

		// Token: 0x04000911 RID: 2321
		private float _lerpRatio;

		// Token: 0x04000912 RID: 2322
		private bool _isResizing;

		// Token: 0x04000913 RID: 2323
		private bool _resizeActualPanel;

		// Token: 0x04000914 RID: 2324
		private Vec2 _resizeStartMousePosition;

		// Token: 0x04000915 RID: 2325
		private Vec2 _resizeOriginalSize;

		// Token: 0x04000916 RID: 2326
		private ValueTuple<SizePolicy, SizePolicy> _innerPanelDefaultSizePolicies;

		// Token: 0x04000917 RID: 2327
		private bool _focusOnNextUpdate;

		// Token: 0x04000918 RID: 2328
		private bool _isChatDisabled;

		// Token: 0x04000919 RID: 2329
		private bool _isMPChatLog;

		// Token: 0x0400091A RID: 2330
		private bool _finishedResizing;

		// Token: 0x0400091B RID: 2331
		private bool _fullyShowChat;

		// Token: 0x0400091C RID: 2332
		private bool _fullyShowChatWithTyping;

		// Token: 0x0400091D RID: 2333
		private EditableTextWidget _textInputWidget;

		// Token: 0x0400091E RID: 2334
		private ScrollbarWidget _scrollbar;

		// Token: 0x0400091F RID: 2335
		private ScrollablePanel _scrollablePanel;

		// Token: 0x04000920 RID: 2336
		private Widget _resizerWidget;

		// Token: 0x04000921 RID: 2337
		private Widget _resizeFrameWidget;

		// Token: 0x04000922 RID: 2338
		private float _sizeX;

		// Token: 0x04000923 RID: 2339
		private float _sizeY;

		// Token: 0x04000924 RID: 2340
		private ListPanel _messageHistoryList;
	}
}
