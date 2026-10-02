using System;
using System.Collections.Generic;
using System.Numerics;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000038 RID: 56
	public class TwoDimensionDrawContext
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600028A RID: 650 RVA: 0x000097EC File Offset: 0x000079EC
		public bool ScissorTestEnabled
		{
			get
			{
				return this._scissorTestEnabled;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600028B RID: 651 RVA: 0x000097F4 File Offset: 0x000079F4
		public bool CircularMaskEnabled
		{
			get
			{
				return this._circularMaskEnabled;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600028C RID: 652 RVA: 0x000097FC File Offset: 0x000079FC
		public Vector2 CircularMaskCenter
		{
			get
			{
				return this._circularMaskCenter;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00009804 File Offset: 0x00007A04
		public float CircularMaskRadius
		{
			get
			{
				return this._circularMaskRadius;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000980C File Offset: 0x00007A0C
		public float CircularMaskSmoothingRadius
		{
			get
			{
				return this._circularMaskSmoothingRadius;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00009814 File Offset: 0x00007A14
		public ScissorTestInfo CurrentScissor
		{
			get
			{
				return this._scissorStack[this._scissorStack.Count - 1];
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000982E File Offset: 0x00007A2E
		public TwoDimensionDrawContext()
		{
			this._scissorStack = new List<ScissorTestInfo>();
			this._scissorTestEnabled = false;
			this._drawData = new List<TwoDimensionDrawData>();
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000986B File Offset: 0x00007A6B
		public void Reset()
		{
			this._scissorStack.Clear();
			this._scissorTestEnabled = false;
			this._drawData.Clear();
			this._simpleMaterialPool.ResetAll();
			this._textMaterialPool.ResetAll();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000098A0 File Offset: 0x00007AA0
		public SimpleMaterial CreateSimpleMaterial()
		{
			SimpleMaterial simpleMaterial = this._simpleMaterialPool.New();
			simpleMaterial.Texture = null;
			return simpleMaterial;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x000098B4 File Offset: 0x00007AB4
		public TextMaterial CreateTextMaterial()
		{
			TextMaterial textMaterial = this._textMaterialPool.New();
			textMaterial.Texture = null;
			return textMaterial;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000098C8 File Offset: 0x00007AC8
		public void PushScissor(in Rectangle2D newScissorRectangle)
		{
			Rectangle2D rectangle2D = newScissorRectangle;
			SimpleRectangle boundingBox = rectangle2D.GetBoundingBox();
			ScissorTestInfo scissorTestInfo = new ScissorTestInfo(boundingBox.X, boundingBox.Y, boundingBox.X2, boundingBox.Y2);
			if (this._scissorStack.Count > 0)
			{
				scissorTestInfo.ReduceToIntersection(this._scissorStack[this._scissorStack.Count - 1]);
			}
			this._scissorStack.Add(scissorTestInfo);
			this._scissorTestEnabled = true;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00009943 File Offset: 0x00007B43
		public void PopScissor()
		{
			this._scissorStack.RemoveAt(this._scissorStack.Count - 1);
			if (this._scissorTestEnabled && this._scissorStack.Count == 0)
			{
				this._scissorTestEnabled = false;
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000997C File Offset: 0x00007B7C
		public bool IsDiscardedByAnyScissor(in Rectangle2D rect)
		{
			for (int i = 0; i < this._scissorStack.Count; i++)
			{
				if (!this._scissorStack[i].IsCollide(in rect))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000099B9 File Offset: 0x00007BB9
		public void SetCircualMask(Vector2 position, float radius, float smoothingRadius)
		{
			this._circularMaskEnabled = true;
			this._circularMaskCenter = position;
			this._circularMaskRadius = radius;
			this._circularMaskSmoothingRadius = smoothingRadius;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x000099D7 File Offset: 0x00007BD7
		public void ClearCircualMask()
		{
			this._circularMaskEnabled = false;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000099E0 File Offset: 0x00007BE0
		private void UpdateVisualMatricesAux(int startIndexInclusive, int endIndexInclusive)
		{
			for (int i = startIndexInclusive; i < endIndexInclusive; i++)
			{
				TwoDimensionDrawData twoDimensionDrawData = this._drawData[i];
				twoDimensionDrawData.UpdateVisualRect();
				this._drawData[i] = twoDimensionDrawData;
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00009A1C File Offset: 0x00007C1C
		public void DrawTo(TwoDimensionContext twoDimensionContext)
		{
			if (this._drawData.Count > 32)
			{
				TWParallel.ForWithoutRenderThread(0, this._drawData.Count, new TWParallel.ParallelForAuxPredicate(this.UpdateVisualMatricesAux), 16);
			}
			else
			{
				this.UpdateVisualMatricesAux(0, this._drawData.Count);
			}
			for (int i = 0; i < this._drawData.Count; i++)
			{
				this._drawData[i].DrawTo(twoDimensionContext, i);
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00009A98 File Offset: 0x00007C98
		public void DrawSprite(Sprite sprite, SimpleMaterial material, in Rectangle2D rectangle, float scale)
		{
			Vec2 minUvs = sprite.GetMinUvs();
			Vec2 maxUvs = sprite.GetMaxUvs();
			ImageDrawObject imageDrawObject = ImageDrawObject.Create(in rectangle, in minUvs, in maxUvs);
			imageDrawObject.Scale = scale;
			material.Texture = sprite.Texture;
			if (this._circularMaskEnabled)
			{
				material.CircularMaskingEnabled = true;
				material.CircularMaskingCenter = this._circularMaskCenter;
				material.CircularMaskingRadius = this._circularMaskRadius;
				material.CircularMaskingSmoothingRadius = this._circularMaskSmoothingRadius;
			}
			this.Draw(material, in imageDrawObject);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00009B10 File Offset: 0x00007D10
		public void Draw(SimpleMaterial material, in ImageDrawObject drawObject)
		{
			ScissorTestInfo scissorTestInfo = ((this._scissorStack.Count > 0) ? this._scissorStack[this._scissorStack.Count - 1] : default(ScissorTestInfo));
			TwoDimensionDrawData twoDimensionDrawData = new TwoDimensionDrawData(this._scissorTestEnabled, in scissorTestInfo, material, in drawObject);
			this._drawData.Add(twoDimensionDrawData);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00009B6C File Offset: 0x00007D6C
		public void Draw(TextMaterial material, in TextDrawObject drawObject)
		{
			ScissorTestInfo scissorTestInfo = ((this._scissorStack.Count > 0) ? this._scissorStack[this._scissorStack.Count - 1] : default(ScissorTestInfo));
			TwoDimensionDrawData twoDimensionDrawData = new TwoDimensionDrawData(this._scissorTestEnabled, in scissorTestInfo, material, in drawObject);
			this._drawData.Add(twoDimensionDrawData);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00009BC8 File Offset: 0x00007DC8
		public void Draw(Text text, TextMaterial materialOriginal, in Rectangle2D parentRectangle, in Rectangle2D rectangle)
		{
			text.UpdateSize((int)rectangle.LocalScale.X, (int)rectangle.LocalScale.Y);
			foreach (TextPart textPart in text.GetParts())
			{
				TextDrawObject drawObject2D = textPart.DrawObject2D;
				TextMaterial textMaterial = this.CreateTextMaterial();
				textMaterial.CopyFrom(materialOriginal);
				if (drawObject2D.IsValid)
				{
					drawObject2D.Rectangle.FillLocalValuesFrom(in rectangle);
					drawObject2D.Rectangle.LocalScale = new Vector2(drawObject2D.Text_MeshWidth, drawObject2D.Text_MeshHeight);
					drawObject2D.Rectangle.CalculateMatrixFrame(in parentRectangle);
					textMaterial.Texture = textPart.DefaultFont.FontSprite.Texture;
					textMaterial.ScaleFactor = (float)textPart.DefaultFont.Size;
					textMaterial.SmoothingConstant = textPart.DefaultFont.SmoothingConstant;
					textMaterial.Smooth = textPart.DefaultFont.Smooth;
					if (textMaterial.GlowRadius > 0f || textMaterial.Blur > 0f || textMaterial.OutlineAmount > 0f)
					{
						TextMaterial textMaterial2 = this.CreateTextMaterial();
						textMaterial2.CopyFrom(textMaterial);
						this.Draw(textMaterial2, in drawObject2D);
					}
					textMaterial.GlowRadius = 0f;
					textMaterial.Blur = 0f;
					textMaterial.OutlineAmount = 0f;
					this.Draw(textMaterial, in drawObject2D);
				}
			}
		}

		// Token: 0x04000131 RID: 305
		private List<ScissorTestInfo> _scissorStack;

		// Token: 0x04000132 RID: 306
		private bool _scissorTestEnabled;

		// Token: 0x04000133 RID: 307
		private bool _circularMaskEnabled;

		// Token: 0x04000134 RID: 308
		private float _circularMaskRadius;

		// Token: 0x04000135 RID: 309
		private float _circularMaskSmoothingRadius;

		// Token: 0x04000136 RID: 310
		private Vector2 _circularMaskCenter;

		// Token: 0x04000137 RID: 311
		private List<TwoDimensionDrawData> _drawData;

		// Token: 0x04000138 RID: 312
		private MaterialPool<SimpleMaterial> _simpleMaterialPool = new MaterialPool<SimpleMaterial>(8);

		// Token: 0x04000139 RID: 313
		private MaterialPool<TextMaterial> _textMaterialPool = new MaterialPool<TextMaterial>(8);
	}
}
