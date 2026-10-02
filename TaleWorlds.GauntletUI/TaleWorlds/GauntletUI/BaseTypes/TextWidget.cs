using System;
using TaleWorlds.GauntletUI.Layout;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200006C RID: 108
	public class TextWidget : ImageWidget
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x0001FC30 File Offset: 0x0001DE30
		// (set) Token: 0x06000766 RID: 1894 RVA: 0x0001FC38 File Offset: 0x0001DE38
		public bool AutoHideIfEmpty
		{
			get
			{
				return this._autoHideIfEmpty;
			}
			set
			{
				if (value != this._autoHideIfEmpty)
				{
					this._autoHideIfEmpty = value;
					if (this._autoHideIfEmpty)
					{
						base.IsVisible = !string.IsNullOrEmpty(this.Text);
					}
				}
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x0001FC66 File Offset: 0x0001DE66
		// (set) Token: 0x06000768 RID: 1896 RVA: 0x0001FC73 File Offset: 0x0001DE73
		[Editor(false)]
		public string Text
		{
			get
			{
				return this._text.Value;
			}
			set
			{
				if (this._text.Value != value)
				{
					this.SetText(value);
				}
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0001FC90 File Offset: 0x0001DE90
		// (set) Token: 0x0600076A RID: 1898 RVA: 0x0001FCB4 File Offset: 0x0001DEB4
		[Editor(false)]
		public int IntText
		{
			get
			{
				int num;
				if (int.TryParse(this._text.Value, out num))
				{
					return num;
				}
				return -1;
			}
			set
			{
				if (this._text.Value != value.ToString())
				{
					this.SetText(value.ToString());
				}
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x0001FCDC File Offset: 0x0001DEDC
		// (set) Token: 0x0600076C RID: 1900 RVA: 0x0001FD04 File Offset: 0x0001DF04
		[Editor(false)]
		public float FloatText
		{
			get
			{
				float num;
				if (float.TryParse(this._text.Value, out num))
				{
					return num;
				}
				return -1f;
			}
			set
			{
				if (this._text.Value != value.ToString())
				{
					this.SetText(value.ToString());
				}
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001FD2C File Offset: 0x0001DF2C
		public TextWidget(UIContext context)
			: base(context)
		{
			FontFactory fontFactory = context.FontFactory;
			this._text = new Text((int)base.Size.X, (int)base.Size.Y, fontFactory.DefaultFont, new Func<int, Font>(fontFactory.GetUsableFontForCharacter));
			base.LayoutImp = new TextLayout(this._text);
			this._renderOffset = Vec2.Zero;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0001FDA0 File Offset: 0x0001DFA0
		protected virtual void SetText(string value)
		{
			base.SetMeasureAndLayoutDirty();
			this._text.CurrentLanguage = base.Context.FontFactory.CurrentLanguage;
			this._text.Value = value;
			base.OnPropertyChanged(this.FloatText, "FloatText");
			base.OnPropertyChanged(this.IntText, "IntText");
			base.OnPropertyChanged<string>(this.Text, "Text");
			this.RefreshTextParameters();
			if (this.AutoHideIfEmpty)
			{
				base.IsVisible = !string.IsNullOrEmpty(this.Text);
			}
			this._renderOffset = Vec2.Zero;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001FE3C File Offset: 0x0001E03C
		protected void RefreshTextParameters()
		{
			float num = (float)base.ReadOnlyBrush.FontSize * base._scaleToUse;
			this._text.HorizontalAlignment = base.ReadOnlyBrush.TextHorizontalAlignment;
			this._text.VerticalAlignment = base.ReadOnlyBrush.TextVerticalAlignment;
			this._text.FontSize = num;
			this._text.CurrentLanguage = base.Context.FontFactory.CurrentLanguage;
			Font font;
			if (base.ReadOnlyBrush.Font != null)
			{
				font = base.ReadOnlyBrush.Font;
			}
			else
			{
				font = base.Context.FontFactory.DefaultFont;
			}
			this._text.Font = base.Context.FontFactory.GetMappedFontForLocalization(font.Name);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0001FF00 File Offset: 0x0001E100
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			base.OnRender(twoDimensionContext, drawContext);
			this.RefreshTextParameters();
			TextMaterial textMaterial = base.BrushRenderer.CreateTextMaterial(drawContext);
			textMaterial.AlphaFactor *= base.Context.ContextAlpha;
			Rectangle2D areaRect = this.AreaRect;
			Brush brush = base.Brush;
			Style style = ((brush != null) ? brush.GetStyleOrDefault(base.CurrentState) : null);
			areaRect.AddVisualOffset(style.XOffset, style.YOffset);
			areaRect.AddVisualOffset(this._renderOffset.X, this._renderOffset.Y);
			drawContext.Draw(this._text, textMaterial, in base.ParentWidget.AreaRect, in areaRect);
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0001FFAA File Offset: 0x0001E1AA
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x0001FFB2 File Offset: 0x0001E1B2
		public bool CanBreakWords
		{
			get
			{
				return this._canBreakWords;
			}
			set
			{
				if (value != this._canBreakWords)
				{
					this._canBreakWords = value;
					this._text.CanBreakWords = value;
					base.OnPropertyChanged(value, "CanBreakWords");
				}
			}
		}

		// Token: 0x04000372 RID: 882
		protected readonly Text _text;

		// Token: 0x04000373 RID: 883
		private bool _autoHideIfEmpty;

		// Token: 0x04000374 RID: 884
		protected Vec2 _renderOffset;

		// Token: 0x04000375 RID: 885
		private bool _canBreakWords = true;
	}
}
