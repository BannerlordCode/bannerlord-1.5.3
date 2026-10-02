using System;
using System.Numerics;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000015 RID: 21
	internal class TextMeshGenerator
	{
		// Token: 0x060000DD RID: 221 RVA: 0x0000627F File Offset: 0x0000447F
		internal TextMeshGenerator()
		{
			this._scaleValue = 1f;
			this._drawRectangle = Rectangle2D.Create();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000062A0 File Offset: 0x000044A0
		internal void Refresh(Font font, int possibleMaxCharacterLength, float scaleValue)
		{
			this._font = font;
			this._textMeshCharacterCount = 0;
			int num = possibleMaxCharacterLength * 8 * 2;
			int num2 = possibleMaxCharacterLength * 8 * 2;
			if (this._vertices == null || this._vertices.Length < num)
			{
				this._vertices = new float[num];
			}
			if (this._uvs == null || this._uvs.Length < num2)
			{
				this._uvs = new float[num2];
			}
			this._scaleValue = scaleValue;
			this._areVerticesNormalized = false;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006314 File Offset: 0x00004514
		internal TextDrawObject GenerateMesh()
		{
			if (!this._areVerticesNormalized)
			{
				this._indices = new uint[this._textMeshCharacterCount * 6];
				uint num = 0U;
				while ((ulong)num < (ulong)((long)this._textMeshCharacterCount))
				{
					int num2 = (int)(6U * num);
					uint num3 = 4U * num;
					this._indices[num2] = num3;
					this._indices[num2 + 1] = 1U + num3;
					this._indices[num2 + 2] = 2U + num3;
					this._indices[num2 + 3] = num3;
					this._indices[num2 + 4] = 2U + num3;
					this._indices[num2 + 5] = 3U + num3;
					num += 1U;
				}
				this._meshNormalizationInfo = this.GetNormalizedVertices();
				this._drawRectangle.LocalScale = new Vector2(this._meshNormalizationInfo.MeshWidth, this._meshNormalizationInfo.MeshHeight);
				this._areVerticesNormalized = true;
			}
			TextDrawObject textDrawObject = TextDrawObject.Create(this._meshNormalizationInfo.NormalizedVertices, this._uvs, this._indices, this._meshNormalizationInfo.MeshWidth, this._meshNormalizationInfo.MeshHeight, in this._drawRectangle);
			textDrawObject.ConvertToHashInPlace();
			return textDrawObject;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000641C File Offset: 0x0000461C
		private TextMeshGenerator.MeshNormalizationInfo GetNormalizedVertices()
		{
			if (this._textMeshCharacterCount == 0)
			{
				return new TextMeshGenerator.MeshNormalizationInfo(0f, 0f, new float[0]);
			}
			int num = this._textMeshCharacterCount * 8;
			float[] array = new float[num];
			float num2 = float.MaxValue;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			float num5 = float.MinValue;
			for (int i = 0; i < num; i++)
			{
				if (i % 2 == 0)
				{
					if (this._vertices[i] < num2)
					{
						num2 = this._vertices[i];
					}
					else if (this._vertices[i] > num4)
					{
						num4 = this._vertices[i];
					}
				}
				else if (i % 2 == 1)
				{
					if (this._vertices[i] < num3)
					{
						num3 = this._vertices[i];
					}
					else if (this._vertices[i] > num5)
					{
						num5 = this._vertices[i];
					}
				}
			}
			float num6 = num4 - num2;
			float num7 = num5 - num3;
			for (int j = 0; j < num; j++)
			{
				if (j % 2 == 0)
				{
					array[j] = this._vertices[j] / num6;
				}
				else if (j % 2 == 1)
				{
					array[j] = this._vertices[j] / num7;
				}
			}
			return new TextMeshGenerator.MeshNormalizationInfo(num6, num7, array);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006548 File Offset: 0x00004748
		internal void AddCharacterToMesh(float x, float y, BitmapFontCharacter fontCharacter)
		{
			float minU = this._font.FontSprite.MinU;
			float minV = this._font.FontSprite.MinV;
			Texture texture = this._font.FontSprite.Texture;
			int? num = ((texture != null) ? new int?(texture.Width) : null);
			float num2 = ((num != null) ? ((float)num.GetValueOrDefault()) : 1f);
			Texture texture2 = this._font.FontSprite.Texture;
			num = ((texture2 != null) ? new int?(texture2.Height) : null);
			float num3 = ((num != null) ? ((float)num.GetValueOrDefault()) : 1f);
			float num4 = 1f / num2;
			float num5 = 1f / num3;
			float num6 = minU + (float)fontCharacter.X * num4;
			float num7 = minV + (float)fontCharacter.Y * num5;
			float num8 = num6 + (float)fontCharacter.Width * num4;
			float num9 = num7 + (float)fontCharacter.Height * num5;
			float num10 = (float)fontCharacter.Width * this._scaleValue;
			float num11 = (float)fontCharacter.Height * this._scaleValue;
			this._uvs[8 * this._textMeshCharacterCount] = num6;
			this._uvs[8 * this._textMeshCharacterCount + 1] = num7;
			this._uvs[8 * this._textMeshCharacterCount + 2] = num8;
			this._uvs[8 * this._textMeshCharacterCount + 3] = num7;
			this._uvs[8 * this._textMeshCharacterCount + 4] = num8;
			this._uvs[8 * this._textMeshCharacterCount + 5] = num9;
			this._uvs[8 * this._textMeshCharacterCount + 6] = num6;
			this._uvs[8 * this._textMeshCharacterCount + 7] = num9;
			this._vertices[8 * this._textMeshCharacterCount] = x;
			this._vertices[8 * this._textMeshCharacterCount + 1] = y;
			this._vertices[8 * this._textMeshCharacterCount + 2] = x + num10;
			this._vertices[8 * this._textMeshCharacterCount + 3] = y;
			this._vertices[8 * this._textMeshCharacterCount + 4] = x + num10;
			this._vertices[8 * this._textMeshCharacterCount + 5] = y + num11;
			this._vertices[8 * this._textMeshCharacterCount + 6] = x;
			this._vertices[8 * this._textMeshCharacterCount + 7] = y + num11;
			this._textMeshCharacterCount++;
			this._areVerticesNormalized = false;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000067B0 File Offset: 0x000049B0
		internal void AddValueToX(float value)
		{
			for (int i = 0; i < this._vertices.Length; i += 2)
			{
				this._vertices[i] += value;
			}
			this._areVerticesNormalized = false;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000067E8 File Offset: 0x000049E8
		internal void AddValueToY(float value)
		{
			for (int i = 0; i < this._vertices.Length; i += 2)
			{
				this._vertices[i + 1] += value;
			}
			this._areVerticesNormalized = false;
		}

		// Token: 0x04000086 RID: 134
		private Font _font;

		// Token: 0x04000087 RID: 135
		private float[] _vertices;

		// Token: 0x04000088 RID: 136
		private TextMeshGenerator.MeshNormalizationInfo _meshNormalizationInfo;

		// Token: 0x04000089 RID: 137
		private float[] _uvs;

		// Token: 0x0400008A RID: 138
		private uint[] _indices;

		// Token: 0x0400008B RID: 139
		private int _textMeshCharacterCount;

		// Token: 0x0400008C RID: 140
		private float _scaleValue;

		// Token: 0x0400008D RID: 141
		private bool _areVerticesNormalized;

		// Token: 0x0400008E RID: 142
		private Rectangle2D _drawRectangle;

		// Token: 0x0200003E RID: 62
		private struct MeshNormalizationInfo
		{
			// Token: 0x060002AD RID: 685 RVA: 0x0000A05E File Offset: 0x0000825E
			public MeshNormalizationInfo(float meshWidth, float meshHeight, float[] normalizedVertices)
			{
				this.MeshWidth = meshWidth;
				this.MeshHeight = meshHeight;
				this.NormalizedVertices = normalizedVertices;
			}

			// Token: 0x04000145 RID: 325
			public readonly float MeshWidth;

			// Token: 0x04000146 RID: 326
			public readonly float MeshHeight;

			// Token: 0x04000147 RID: 327
			public readonly float[] NormalizedVertices;
		}
	}
}
