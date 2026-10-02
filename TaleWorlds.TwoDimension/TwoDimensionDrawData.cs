using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000039 RID: 57
	internal struct TwoDimensionDrawData
	{
		// Token: 0x0600029F RID: 671 RVA: 0x00009D54 File Offset: 0x00007F54
		public TwoDimensionDrawData(bool scissorTestEnabled, in ScissorTestInfo scissorTestInfo, SimpleMaterial imageMaterial, in ImageDrawObject imageDrawObject)
		{
			this._scissorTestEnabled = scissorTestEnabled;
			this._scissorTestInfo = scissorTestInfo;
			this._imageMaterial = imageMaterial;
			this._imageDrawObject = imageDrawObject;
			this._textMaterial = null;
			this._textDrawObject = TextDrawObject.Invalid;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00009D8F File Offset: 0x00007F8F
		public TwoDimensionDrawData(bool scissorTestEnabled, in ScissorTestInfo scissorTestInfo, TextMaterial textMaterial, in TextDrawObject textDrawObject)
		{
			this._scissorTestEnabled = scissorTestEnabled;
			this._scissorTestInfo = scissorTestInfo;
			this._imageMaterial = null;
			this._imageDrawObject = ImageDrawObject.Invalid;
			this._textMaterial = textMaterial;
			this._textDrawObject = textDrawObject;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00009DCC File Offset: 0x00007FCC
		public void DrawTo(TwoDimensionContext twoDimensionContext, int layer)
		{
			if (this._scissorTestEnabled)
			{
				twoDimensionContext.SetScissor(this._scissorTestInfo);
			}
			if (this._imageDrawObject.IsValid && this._imageMaterial != null)
			{
				twoDimensionContext.DrawImage(this._imageMaterial, in this._imageDrawObject, layer);
			}
			else if (this._textDrawObject.IsValid && this._textMaterial != null)
			{
				twoDimensionContext.DrawText(this._textMaterial, in this._textDrawObject, layer);
			}
			if (this._scissorTestEnabled)
			{
				twoDimensionContext.ResetScissor();
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00009E4D File Offset: 0x0000804D
		public void UpdateVisualRect()
		{
			if (this._imageDrawObject.IsValid)
			{
				this._imageDrawObject.Rectangle.CalculateVisualMatrixFrame();
				return;
			}
			if (this._textDrawObject.IsValid)
			{
				this._textDrawObject.Rectangle.CalculateVisualMatrixFrame();
			}
		}

		// Token: 0x0400013A RID: 314
		private bool _scissorTestEnabled;

		// Token: 0x0400013B RID: 315
		private ScissorTestInfo _scissorTestInfo;

		// Token: 0x0400013C RID: 316
		private SimpleMaterial _imageMaterial;

		// Token: 0x0400013D RID: 317
		private ImageDrawObject _imageDrawObject;

		// Token: 0x0400013E RID: 318
		private TextMaterial _textMaterial;

		// Token: 0x0400013F RID: 319
		private TextDrawObject _textDrawObject;
	}
}
