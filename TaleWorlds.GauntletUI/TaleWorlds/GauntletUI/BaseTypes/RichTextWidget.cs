using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using TaleWorlds.GauntletUI.Layout;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000062 RID: 98
	public class RichTextWidget : ImageWidget
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x0001C030 File Offset: 0x0001A230
		// (set) Token: 0x06000685 RID: 1669 RVA: 0x0001C038 File Offset: 0x0001A238
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

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x0001C068 File Offset: 0x0001A268
		private Vector2 LocalMousePosition
		{
			get
			{
				Vector2 mousePosition = base.EventManager.MousePosition;
				return this.AreaRect.TransformScreenPositionToLocal(in mousePosition);
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0001C08E File Offset: 0x0001A28E
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x0001C096 File Offset: 0x0001A296
		[Editor(false)]
		public string LinkHoverCursorState
		{
			get
			{
				return this._linkHoverCursorState;
			}
			set
			{
				if (this._linkHoverCursorState != value)
				{
					this._linkHoverCursorState = value;
				}
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001C0AD File Offset: 0x0001A2AD
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x0001C0BC File Offset: 0x0001A2BC
		[Editor(false)]
		public string Text
		{
			get
			{
				return this._richText.Value;
			}
			set
			{
				if (this._richText.Value != value)
				{
					this._richText.CurrentLanguage = base.Context.FontFactory.CurrentLanguage;
					this._richText.Value = value;
					base.OnPropertyChanged<string>(value, "Text");
					base.SetMeasureAndLayoutDirty();
					this.SetText(this._richText.Value);
				}
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001C128 File Offset: 0x0001A328
		public RichTextWidget(UIContext context)
			: base(context)
		{
			this._fontFactory = context.FontFactory;
			this._textHeight = -1;
			Font defaultFont = base.Context.FontFactory.DefaultFont;
			this._richText = new RichText((int)base.Size.X, (int)base.Size.Y, defaultFont, new Func<int, Font>(this._fontFactory.GetUsableFontForCharacter));
			this._textureMaterialDict = new Dictionary<Texture, SimpleMaterial>();
			this._lastFontBrush = null;
			base.LayoutImp = new TextLayout(this._richText);
			this.CanBreakWords = true;
			base.AddState("Pressed");
			base.AddState("Hovered");
			base.AddState("Disabled");
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001C1E7 File Offset: 0x0001A3E7
		public override void OnBrushChanged()
		{
			base.OnBrushChanged();
			this.UpdateFontData();
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001C1F5 File Offset: 0x0001A3F5
		protected virtual void SetText(string value)
		{
			if (this.AutoHideIfEmpty)
			{
				base.IsVisible = !string.IsNullOrEmpty(this.Text);
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001C214 File Offset: 0x0001A414
		private void SetRichTextParameters()
		{
			bool flag = false;
			this._richText.CurrentLanguage = base.Context.FontFactory.CurrentLanguage;
			this.UpdateFontData();
			if (this._richText.HorizontalAlignment != base.ReadOnlyBrush.TextHorizontalAlignment)
			{
				this._richText.HorizontalAlignment = base.ReadOnlyBrush.TextHorizontalAlignment;
				flag = true;
			}
			if (this._richText.VerticalAlignment != base.ReadOnlyBrush.TextVerticalAlignment)
			{
				this._richText.VerticalAlignment = base.ReadOnlyBrush.TextVerticalAlignment;
				flag = true;
			}
			if (this._richText.TextHeight != this._textHeight)
			{
				this._textHeight = this._richText.TextHeight;
				flag = true;
			}
			if (this._richText.CurrentStyle != base.CurrentState && !string.IsNullOrEmpty(base.CurrentState))
			{
				this._richText.CurrentStyle = base.CurrentState;
				flag = true;
			}
			if (flag)
			{
				base.SetMeasureAndLayoutDirty();
				this._richText.SetAllDirty();
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001C318 File Offset: 0x0001A518
		private void UpdateFontData()
		{
			if (this._lastFontBrush == base.ReadOnlyBrush && this._lastContextScale == base._scaleToUse && this._lastLanguageCode == base.Context.FontFactory.CurrentLanguage.LanguageID)
			{
				return;
			}
			this._richText.StyleFontContainer.ClearFonts();
			foreach (Style style in base.ReadOnlyBrush.Styles)
			{
				Font font;
				if (style.Font != null)
				{
					font = style.Font;
				}
				else if (base.ReadOnlyBrush.Font != null)
				{
					font = base.ReadOnlyBrush.Font;
				}
				else
				{
					font = base.Context.FontFactory.DefaultFont;
				}
				Font mappedFontForLocalization = base.Context.FontFactory.GetMappedFontForLocalization(font.Name);
				this._richText.StyleFontContainer.Add(style.Name, mappedFontForLocalization, (float)style.FontSize * base._scaleToUse);
			}
			this._lastFontBrush = base.ReadOnlyBrush;
			this._lastLanguageCode = base.Context.FontFactory.CurrentLanguage.LanguageID;
			this._lastContextScale = base._scaleToUse;
			this._richText.CurrentLanguage = base.Context.FontFactory.CurrentLanguage;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001C488 File Offset: 0x0001A688
		private Font GetFont(Style style = null)
		{
			if (((style != null) ? style.Font : null) != null)
			{
				return base.Context.FontFactory.GetMappedFontForLocalization(style.Font.Name);
			}
			if (base.ReadOnlyBrush.Font != null)
			{
				return base.Context.FontFactory.GetMappedFontForLocalization(base.ReadOnlyBrush.Font.Name);
			}
			return base.Context.FontFactory.DefaultFont;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001C500 File Offset: 0x0001A700
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.SetRichTextParameters();
			if (base.Size.X > 0f && base.Size.Y > 0f)
			{
				Vector2 vector = this.LocalMousePosition;
				bool flag = this._mouseState == RichTextWidget.MouseState.Down || this._mouseState == RichTextWidget.MouseState.AlternateDown;
				bool flag2 = this._mouseState == RichTextWidget.MouseState.Up || this._mouseState == RichTextWidget.MouseState.AlternateUp;
				if (flag)
				{
					vector = this._mouseDownPosition;
				}
				RichTextLinkGroup focusedLinkGroup = this._richText.FocusedLinkGroup;
				this._richText.UpdateSize((int)base.Size.X, (int)base.Size.Y);
				if (focusedLinkGroup != null && this.LinkHoverCursorState != null)
				{
					base.Context.ActiveCursorOfContext = (UIContext.MouseCursors)Enum.Parse(typeof(UIContext.MouseCursors), this.LinkHoverCursorState);
				}
				bool flag3 = base.WidthSizePolicy != SizePolicy.CoverChildren || base.MaxWidth != 0f;
				bool flag4 = base.HeightSizePolicy != SizePolicy.CoverChildren || base.MaxHeight != 0f;
				this._richText.Update(dt, base.Context.SpriteData, vector, flag, flag3, flag4, base._scaleToUse);
				if (flag2)
				{
					RichTextLinkGroup focusedLinkGroup2 = this._richText.FocusedLinkGroup;
					if (focusedLinkGroup != null && focusedLinkGroup == focusedLinkGroup2)
					{
						string text = focusedLinkGroup.Href;
						string[] array = text.Split(new char[] { ':' });
						if (array.Length == 2)
						{
							text = array[1];
						}
						if (this._mouseState == RichTextWidget.MouseState.Up)
						{
							base.EventFired("LinkClick", new object[] { text });
						}
						else if (this._mouseState == RichTextWidget.MouseState.AlternateUp)
						{
							base.EventFired("LinkAlternateClick", new object[] { text });
						}
					}
					this._mouseState = RichTextWidget.MouseState.None;
				}
				this._renderOffset = Vec2.Zero;
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001C6D4 File Offset: 0x0001A8D4
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			base.OnRender(twoDimensionContext, drawContext);
			if (string.IsNullOrEmpty(this._richText.Value))
			{
				return;
			}
			List<RichTextPart> parts = this._richText.GetParts();
			for (int i = 0; i < parts.Count; i++)
			{
				RichTextPart richTextPart = parts[i];
				if (richTextPart.Type == RichTextPartType.Text)
				{
					this.RenderText(richTextPart, drawContext);
				}
				else if (richTextPart.Type == RichTextPartType.Sprite)
				{
					this.RenderImage(richTextPart, drawContext);
				}
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001C744 File Offset: 0x0001A944
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RenderText(RichTextPart richTextPart, TwoDimensionDrawContext drawContext)
		{
			if (!richTextPart.TextDrawObject.IsValid)
			{
				return;
			}
			TextDrawObject textDrawObject = richTextPart.TextDrawObject;
			Rectangle2D rectangle = textDrawObject.Rectangle;
			rectangle.LocalPosition = new Vector2(base.LocalPosition.X + this._renderOffset.X, base.LocalPosition.Y + this._renderOffset.Y);
			rectangle.LocalScale = new Vector2(textDrawObject.Text_MeshWidth, textDrawObject.Text_MeshHeight);
			Style styleOrDefault = base.ReadOnlyBrush.GetStyleOrDefault(richTextPart.Style);
			Font defaultFont = richTextPart.DefaultFont;
			float num = (float)styleOrDefault.FontSize * base._scaleToUse;
			TextMaterial textMaterial = styleOrDefault.CreateTextMaterial(drawContext);
			textMaterial.ColorFactor *= base.ReadOnlyBrush.GlobalColorFactor;
			textMaterial.AlphaFactor *= base.ReadOnlyBrush.GlobalAlphaFactor * base.Context.ContextAlpha;
			textMaterial.Color *= base.ReadOnlyBrush.GlobalColor;
			textMaterial.Texture = defaultFont.FontSprite.Texture;
			textMaterial.ScaleFactor = num;
			textMaterial.SmoothingConstant = defaultFont.SmoothingConstant;
			textMaterial.Smooth = defaultFont.Smooth;
			rectangle.SetVisualOffset(styleOrDefault.XOffset, styleOrDefault.YOffset);
			rectangle.CalculateMatrixFrame(in base.ParentWidget.AreaRect);
			textDrawObject.Rectangle = rectangle;
			richTextPart.TextDrawObject = textDrawObject;
			if (textMaterial.GlowRadius > 0f || textMaterial.Blur > 0f || textMaterial.OutlineAmount > 0f)
			{
				TextMaterial textMaterial2 = styleOrDefault.CreateTextMaterial(drawContext);
				textMaterial2.CopyFrom(textMaterial);
				drawContext.Draw(textMaterial2, in textDrawObject);
			}
			textMaterial.GlowRadius = 0f;
			textMaterial.Blur = 0f;
			textMaterial.OutlineAmount = 0f;
			drawContext.Draw(textMaterial, in textDrawObject);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001C92C File Offset: 0x0001AB2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RenderImage(RichTextPart richTextPart, TwoDimensionDrawContext drawContext)
		{
			Sprite sprite = richTextPart.Sprite;
			if (((sprite != null) ? sprite.Texture : null) == null || !richTextPart.ImageDrawObject.IsValid)
			{
				return;
			}
			ImageDrawObject imageDrawObject = richTextPart.ImageDrawObject;
			Rectangle2D rectangle = imageDrawObject.Rectangle;
			rectangle.LocalPosition = new Vector2(base.LocalPosition.X + richTextPart.SpritePosition.X + this._renderOffset.X, base.LocalPosition.Y + richTextPart.SpritePosition.Y + this._renderOffset.Y);
			if (!this._textureMaterialDict.ContainsKey(sprite.Texture))
			{
				this._textureMaterialDict[sprite.Texture] = new SimpleMaterial(sprite.Texture);
			}
			SimpleMaterial simpleMaterial = this._textureMaterialDict[sprite.Texture];
			if (simpleMaterial.ColorFactor != base.ReadOnlyBrush.GlobalColorFactor)
			{
				simpleMaterial.ColorFactor = base.ReadOnlyBrush.GlobalColorFactor;
			}
			if (simpleMaterial.AlphaFactor != base.ReadOnlyBrush.GlobalAlphaFactor * base.Context.ContextAlpha)
			{
				simpleMaterial.AlphaFactor = base.ReadOnlyBrush.GlobalAlphaFactor * base.Context.ContextAlpha;
			}
			if (simpleMaterial.Color != base.ReadOnlyBrush.GlobalColor)
			{
				simpleMaterial.Color = base.ReadOnlyBrush.GlobalColor;
			}
			rectangle.CalculateMatrixFrame(in base.ParentWidget.AreaRect);
			imageDrawObject.Rectangle = rectangle;
			richTextPart.ImageDrawObject = imageDrawObject;
			drawContext.Draw(simpleMaterial, in imageDrawObject);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001CAB0 File Offset: 0x0001ACB0
		protected internal override void OnMousePressed()
		{
			if (this._mouseState == RichTextWidget.MouseState.None)
			{
				this._mouseDownPosition = this.LocalMousePosition;
				this._mouseState = RichTextWidget.MouseState.Down;
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001CACD File Offset: 0x0001ACCD
		protected internal override void OnMouseReleased(bool isFromInput)
		{
			if (this._mouseState == RichTextWidget.MouseState.Down)
			{
				this._mouseState = (isFromInput ? RichTextWidget.MouseState.Up : RichTextWidget.MouseState.None);
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001CAE5 File Offset: 0x0001ACE5
		protected internal override void OnMouseAlternatePressed()
		{
			if (this._mouseState == RichTextWidget.MouseState.None)
			{
				this._mouseDownPosition = this.LocalMousePosition;
				this._mouseState = RichTextWidget.MouseState.AlternateDown;
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001CB02 File Offset: 0x0001AD02
		protected internal override void OnMouseAlternateReleased(bool isFromInput)
		{
			if (this._mouseState == RichTextWidget.MouseState.AlternateDown)
			{
				this._mouseState = (isFromInput ? RichTextWidget.MouseState.AlternateUp : RichTextWidget.MouseState.None);
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001CB1A File Offset: 0x0001AD1A
		// (set) Token: 0x0600069A RID: 1690 RVA: 0x0001CB22 File Offset: 0x0001AD22
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
					this._richText.CanBreakWords = value;
					base.OnPropertyChanged(value, "CanBreakWords");
				}
			}
		}

		// Token: 0x0400030C RID: 780
		protected readonly RichText _richText;

		// Token: 0x0400030D RID: 781
		private bool _autoHideIfEmpty;

		// Token: 0x0400030E RID: 782
		private Brush _lastFontBrush;

		// Token: 0x0400030F RID: 783
		private string _lastLanguageCode;

		// Token: 0x04000310 RID: 784
		private float _lastContextScale;

		// Token: 0x04000311 RID: 785
		private FontFactory _fontFactory;

		// Token: 0x04000312 RID: 786
		private RichTextWidget.MouseState _mouseState;

		// Token: 0x04000313 RID: 787
		private Dictionary<Texture, SimpleMaterial> _textureMaterialDict;

		// Token: 0x04000314 RID: 788
		private Vector2 _mouseDownPosition;

		// Token: 0x04000315 RID: 789
		private int _textHeight;

		// Token: 0x04000316 RID: 790
		protected Vec2 _renderOffset;

		// Token: 0x04000317 RID: 791
		private string _linkHoverCursorState;

		// Token: 0x04000318 RID: 792
		private bool _canBreakWords = true;

		// Token: 0x02000097 RID: 151
		private enum MouseState
		{
			// Token: 0x0400049C RID: 1180
			None,
			// Token: 0x0400049D RID: 1181
			Down,
			// Token: 0x0400049E RID: 1182
			Up,
			// Token: 0x0400049F RID: 1183
			AlternateDown,
			// Token: 0x040004A0 RID: 1184
			AlternateUp
		}
	}
}
