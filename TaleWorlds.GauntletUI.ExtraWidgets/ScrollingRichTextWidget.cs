using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000010 RID: 16
	public class ScrollingRichTextWidget : RichTextWidget
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005B31 File Offset: 0x00003D31
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00005B39 File Offset: 0x00003D39
		public string ActualText { get; private set; } = string.Empty;

		// Token: 0x060000EB RID: 235 RVA: 0x00005B44 File Offset: 0x00003D44
		public ScrollingRichTextWidget(UIContext context)
			: base(context)
		{
			this.ScrollOnHoverWidget = this;
			this.DefaultTextHorizontalAlignment = base.Brush.TextHorizontalAlignment;
			base.ClipContents = true;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005BA0 File Offset: 0x00003DA0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.Size != this._currentSize)
			{
				this._currentSize = base.Size;
				this.UpdateScrollable();
			}
			if (this._shouldScroll)
			{
				this._scrollTimeElapsed += dt;
				if (this._scrollTimeElapsed < this.InbetweenScrollDuration)
				{
					this._currentScrollAmount = 0f;
				}
				else if (this._scrollTimeElapsed >= this.InbetweenScrollDuration && this._currentScrollAmount < this._totalScrollAmount)
				{
					this._currentScrollAmount += dt * this.ScrollPerSecond;
					this._currentScrollAmount += dt * this.ScrollRatioPerSecond * this._totalScrollAmount;
				}
				else if (this._currentScrollAmount >= this._totalScrollAmount)
				{
					if (this._scrollTimeNeeded.ApproximatelyEqualsTo(0f, 1E-05f))
					{
						this._scrollTimeNeeded = this._scrollTimeElapsed;
					}
					if (this._scrollTimeElapsed < this._scrollTimeNeeded + this.InbetweenScrollDuration)
					{
						this._currentScrollAmount = this._totalScrollAmount;
					}
					else
					{
						this._scrollTimeNeeded = 0f;
						this._scrollTimeElapsed = 0f;
					}
				}
			}
			if (base.EventManager.HoveredWidget == this.ScrollOnHoverWidget && !this._isHovering)
			{
				this._isHovering = true;
				if (!this.IsAutoScrolling)
				{
					base.Text = this.ActualText;
					this.UpdateWordWidth();
					this._shouldScroll = this._wordWidth > this.GetMaximumAllowedWidth();
				}
			}
			else if (base.EventManager.HoveredWidget != this.ScrollOnHoverWidget && this._isHovering)
			{
				if (!this.IsAutoScrolling)
				{
					this.ResetScroll();
				}
				this._isHovering = false;
				this.UpdateScrollable();
			}
			this._renderOffset.x = -this._currentScrollAmount;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005D6D File Offset: 0x00003F6D
		public override void OnBrushChanged()
		{
			base.OnBrushChanged();
			this.DefaultTextHorizontalAlignment = base.Brush.TextHorizontalAlignment;
			this.UpdateScrollable();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005D8C File Offset: 0x00003F8C
		protected override void SetText(string value)
		{
			base.SetText(value);
			this._richText.SkipLineOnContainerExceeded = false;
			this.ActualText = this._richText.Value;
			this._currentSize = Vec2.Zero;
			this.ResetScroll();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005DC4 File Offset: 0x00003FC4
		private void UpdateScrollable()
		{
			this.UpdateWordWidth();
			if (this._wordWidth > this.GetMaximumAllowedWidth())
			{
				this._shouldScroll = this.IsAutoScrolling;
				this._totalScrollAmount = this._wordWidth - this.GetMaximumAllowedWidth();
				base.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Left;
				if (!this.IsAutoScrolling && !this._isHovering)
				{
					bool flag = false;
					for (int i = this.ActualText.Length; i > 3; i--)
					{
						if (this.ActualText[i - 1] == '>')
						{
							flag = true;
						}
						else if (this.ActualText[i - 1] == '<')
						{
							flag = false;
						}
						if (!flag && this._richText.GetPreferredSize(base.WidthSizePolicy == SizePolicy.Fixed, base.SuggestedWidth, base.HeightSizePolicy == SizePolicy.Fixed, base.SuggestedHeight, base.Context.SpriteData, base._scaleToUse).X <= this.GetMaximumAllowedWidth())
						{
							this._richText.Value = this.ActualText.Substring(0, i - 3) + "...";
							return;
						}
					}
					return;
				}
			}
			else
			{
				this.ResetScroll();
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005EE7 File Offset: 0x000040E7
		private float GetMaximumAllowedWidth()
		{
			if (base.WidthSizePolicy != SizePolicy.CoverChildren)
			{
				return base.Size.X;
			}
			if (base.ScaledMaxWidth == 0f)
			{
				return 2.1474836E+09f;
			}
			return base.ScaledMaxWidth;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005F18 File Offset: 0x00004118
		private void UpdateWordWidth()
		{
			this._wordWidth = this._richText.GetPreferredSize(base.WidthSizePolicy == SizePolicy.Fixed, base.SuggestedWidth, base.HeightSizePolicy == SizePolicy.Fixed, base.SuggestedHeight, base.Context.SpriteData, base._scaleToUse).X;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005F6A File Offset: 0x0000416A
		private void ResetScroll()
		{
			this._shouldScroll = false;
			this._scrollTimeElapsed = 0f;
			this._currentScrollAmount = 0f;
			base.Brush.TextHorizontalAlignment = this.DefaultTextHorizontalAlignment;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005F9A File Offset: 0x0000419A
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00005FA2 File Offset: 0x000041A2
		[Editor(false)]
		public Widget ScrollOnHoverWidget
		{
			get
			{
				return this._scrollOnHoverWidget;
			}
			set
			{
				if (value != this._scrollOnHoverWidget)
				{
					this._scrollOnHoverWidget = value;
					base.OnPropertyChanged<Widget>(value, "ScrollOnHoverWidget");
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00005FC0 File Offset: 0x000041C0
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00005FC8 File Offset: 0x000041C8
		[Editor(false)]
		public bool IsAutoScrolling
		{
			get
			{
				return this._isAutoScrolling;
			}
			set
			{
				if (value != this._isAutoScrolling)
				{
					this._isAutoScrolling = value;
					base.OnPropertyChanged(value, "IsAutoScrolling");
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00005FE6 File Offset: 0x000041E6
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00005FEE File Offset: 0x000041EE
		[Editor(false)]
		public float ScrollPerSecond
		{
			get
			{
				return this._scrollPerSecond;
			}
			set
			{
				if (value != this._scrollPerSecond)
				{
					this._scrollPerSecond = value;
					base.OnPropertyChanged(value, "ScrollPerSecond");
				}
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x0000600C File Offset: 0x0000420C
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00006014 File Offset: 0x00004214
		[Editor(false)]
		public float ScrollRatioPerSecond
		{
			get
			{
				return this._scrollRatioPerSecond;
			}
			set
			{
				if (value != this._scrollRatioPerSecond)
				{
					this._scrollRatioPerSecond = value;
					base.OnPropertyChanged(value, "ScrollRatioPerSecond");
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00006032 File Offset: 0x00004232
		// (set) Token: 0x060000FC RID: 252 RVA: 0x0000603A File Offset: 0x0000423A
		[Editor(false)]
		public float InbetweenScrollDuration
		{
			get
			{
				return this._inbetweenScrollDuration;
			}
			set
			{
				if (value != this._inbetweenScrollDuration)
				{
					this._inbetweenScrollDuration = value;
					base.OnPropertyChanged(value, "InbetweenScrollDuration");
				}
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00006058 File Offset: 0x00004258
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00006060 File Offset: 0x00004260
		[Editor(false)]
		public TextHorizontalAlignment DefaultTextHorizontalAlignment
		{
			get
			{
				return this._defaultTextHorizontalAlignment;
			}
			set
			{
				if (value != this._defaultTextHorizontalAlignment)
				{
					this._defaultTextHorizontalAlignment = value;
					switch (value)
					{
					case TextHorizontalAlignment.Left:
						base.OnPropertyChanged<string>("Left", "DefaultTextHorizontalAlignment");
						return;
					case TextHorizontalAlignment.Right:
						base.OnPropertyChanged<string>("Right", "DefaultTextHorizontalAlignment");
						return;
					case TextHorizontalAlignment.Center:
						base.OnPropertyChanged<string>("Center", "DefaultTextHorizontalAlignment");
						return;
					case TextHorizontalAlignment.Justify:
						base.OnPropertyChanged<string>("Justify", "DefaultTextHorizontalAlignment");
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x04000066 RID: 102
		private bool _shouldScroll;

		// Token: 0x04000067 RID: 103
		private float _scrollTimeNeeded;

		// Token: 0x04000068 RID: 104
		private float _scrollTimeElapsed;

		// Token: 0x04000069 RID: 105
		private float _totalScrollAmount;

		// Token: 0x0400006A RID: 106
		private float _currentScrollAmount;

		// Token: 0x0400006B RID: 107
		private Vec2 _currentSize;

		// Token: 0x0400006D RID: 109
		private bool _isHovering;

		// Token: 0x0400006E RID: 110
		private float _wordWidth;

		// Token: 0x0400006F RID: 111
		private Widget _scrollOnHoverWidget;

		// Token: 0x04000070 RID: 112
		private bool _isAutoScrolling = true;

		// Token: 0x04000071 RID: 113
		private float _scrollPerSecond = 30f;

		// Token: 0x04000072 RID: 114
		private float _scrollRatioPerSecond;

		// Token: 0x04000073 RID: 115
		private float _inbetweenScrollDuration = 1f;

		// Token: 0x04000074 RID: 116
		private TextHorizontalAlignment _defaultTextHorizontalAlignment;
	}
}
