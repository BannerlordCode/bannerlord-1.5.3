using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000011 RID: 17
	public class ScrollingTextWidget : TextWidget
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000060D7 File Offset: 0x000042D7
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000060DF File Offset: 0x000042DF
		public string ActualText { get; private set; } = string.Empty;

		// Token: 0x06000101 RID: 257 RVA: 0x000060E8 File Offset: 0x000042E8
		public ScrollingTextWidget(UIContext context)
			: base(context)
		{
			this.ScrollOnHoverWidget = this;
			this.DefaultTextHorizontalAlignment = base.Brush.TextHorizontalAlignment;
			base.ClipHorizontalContent = true;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00006144 File Offset: 0x00004344
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
				if (!this.IsAutoScrolling)
				{
					this._text.Value = this.ActualText;
					this.UpdateWordWidth();
					this._shouldScroll = this._wordWidth > this.GetMaximumAllowedWidth();
				}
				this._isHovering = true;
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

		// Token: 0x06000103 RID: 259 RVA: 0x00006316 File Offset: 0x00004516
		public override void OnBrushChanged()
		{
			this.DefaultTextHorizontalAlignment = base.Brush.TextHorizontalAlignment;
			this.UpdateScrollable();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00006330 File Offset: 0x00004530
		protected override void SetText(string value)
		{
			base.SetText(value);
			this._text.SkipLineOnContainerExceeded = false;
			this._text.ResizeTextOnOverflow = false;
			this.ActualText = this._text.Value;
			this._currentSize = Vec2.Zero;
			this.ResetScroll();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006380 File Offset: 0x00004580
		private void UpdateScrollable()
		{
			this.UpdateWordWidth();
			if (this._wordWidth > this.GetMaximumAllowedWidth())
			{
				this._shouldScroll = this.IsAutoScrolling;
				this._totalScrollAmount = this._wordWidth - this.GetMaximumAllowedWidth();
				base.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Left;
				if (!this.IsAutoScrolling)
				{
					for (int i = this.ActualText.Length; i > 3; i--)
					{
						if (this.GetWordWidth(this.ActualText.Substring(0, i - 3) + "...", 0.25f) * base._scaleToUse <= this.GetMaximumAllowedWidth())
						{
							this._text.Value = this.ActualText.Substring(0, i - 3) + "...";
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

		// Token: 0x06000106 RID: 262 RVA: 0x0000644C File Offset: 0x0000464C
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

		// Token: 0x06000107 RID: 263 RVA: 0x0000647C File Offset: 0x0000467C
		private void UpdateWordWidth()
		{
			float num = 0.5f;
			if (base.WidthSizePolicy == SizePolicy.CoverChildren)
			{
				num = 0f;
			}
			this._wordWidth = this.GetWordWidth(this._text.Value, num) * base._scaleToUse;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000064C0 File Offset: 0x000046C0
		private float GetWordWidth(string word, float padding)
		{
			float num = padding * 2f;
			for (int i = 0; i < word.Length; i++)
			{
				num += this.GetCharacterWidth(word[i]);
			}
			return num;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000064F8 File Offset: 0x000046F8
		private float GetCharacterWidth(char character)
		{
			FontFactory fontFactory = base.Context.FontFactory;
			Brush brush = base.Brush;
			string text;
			if (brush == null)
			{
				text = null;
			}
			else
			{
				Font font = brush.Font;
				text = ((font != null) ? font.Name : null);
			}
			Font mappedFontForLocalization = fontFactory.GetMappedFontForLocalization(text);
			float num2;
			if (!mappedFontForLocalization.Characters.ContainsKey((int)character))
			{
				Font font2 = base.Context.FontFactory.GetUsableFontForCharacter((int)character) ?? mappedFontForLocalization;
				float num = (float)base.Brush.FontSize / (float)font2.Size;
				num2 = font2.GetCharacterWidth(character, 0.5f) * num;
			}
			else
			{
				float num = (float)base.Brush.FontSize / (float)mappedFontForLocalization.Size;
				num2 = mappedFontForLocalization.GetCharacterWidth(character, 0.5f) * num;
			}
			return num2;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000065A4 File Offset: 0x000047A4
		private void ResetScroll()
		{
			this._shouldScroll = false;
			this._scrollTimeElapsed = 0f;
			this._currentScrollAmount = 0f;
			this._renderOffset.x = 0f;
			base.Brush.TextHorizontalAlignment = this.DefaultTextHorizontalAlignment;
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600010B RID: 267 RVA: 0x000065E4 File Offset: 0x000047E4
		// (set) Token: 0x0600010C RID: 268 RVA: 0x000065EC File Offset: 0x000047EC
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

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0000660A File Offset: 0x0000480A
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00006612 File Offset: 0x00004812
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

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00006630 File Offset: 0x00004830
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00006638 File Offset: 0x00004838
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

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00006656 File Offset: 0x00004856
		// (set) Token: 0x06000112 RID: 274 RVA: 0x0000665E File Offset: 0x0000485E
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

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000113 RID: 275 RVA: 0x0000667C File Offset: 0x0000487C
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00006684 File Offset: 0x00004884
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

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000066A2 File Offset: 0x000048A2
		// (set) Token: 0x06000116 RID: 278 RVA: 0x000066AC File Offset: 0x000048AC
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

		// Token: 0x04000075 RID: 117
		private bool _shouldScroll;

		// Token: 0x04000076 RID: 118
		private float _scrollTimeNeeded;

		// Token: 0x04000077 RID: 119
		private float _scrollTimeElapsed;

		// Token: 0x04000078 RID: 120
		private float _totalScrollAmount;

		// Token: 0x04000079 RID: 121
		private float _currentScrollAmount;

		// Token: 0x0400007A RID: 122
		private Vec2 _currentSize;

		// Token: 0x0400007C RID: 124
		private bool _isHovering;

		// Token: 0x0400007D RID: 125
		private float _wordWidth;

		// Token: 0x0400007E RID: 126
		private Widget _scrollOnHoverWidget;

		// Token: 0x0400007F RID: 127
		private bool _isAutoScrolling = true;

		// Token: 0x04000080 RID: 128
		private float _scrollPerSecond = 30f;

		// Token: 0x04000081 RID: 129
		private float _scrollRatioPerSecond;

		// Token: 0x04000082 RID: 130
		private float _inbetweenScrollDuration = 1f;

		// Token: 0x04000083 RID: 131
		private TextHorizontalAlignment _defaultTextHorizontalAlignment;
	}
}
