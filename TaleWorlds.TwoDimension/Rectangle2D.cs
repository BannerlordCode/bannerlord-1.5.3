using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200002B RID: 43
	public struct Rectangle2D
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x000078D4 File Offset: 0x00005AD4
		public static Rectangle2D Invalid
		{
			get
			{
				return Rectangle2D.CreateInvalid();
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000078DC File Offset: 0x00005ADC
		private static Rectangle2D CreateInvalid()
		{
			return new Rectangle2D
			{
				IsValid = false
			};
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000078FC File Offset: 0x00005AFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rectangle2D Create()
		{
			return new Rectangle2D
			{
				IsValid = true,
				LocalScale = new Vector2(10f, 10f),
				_renderProperties = Rectangle2D.RectangleRenderProperties.CreateEmpty(),
				_cachedOrthonormalMatrix = MatrixFrame.Identity,
				_cachedMatrixFrame = MatrixFrame.Identity,
				_cachedVisualMatrixFrame = MatrixFrame.Identity
			};
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00007960 File Offset: 0x00005B60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rectangle2D FillLocalValuesFrom(in Rectangle2D other)
		{
			this.LocalPosition = other.LocalPosition;
			this.LocalScale = other.LocalScale;
			this.LocalRotation = other.LocalRotation;
			this.LocalPivot = other.LocalPivot;
			this._renderProperties.FillValuesFrom(other._renderProperties);
			return this;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000079B4 File Offset: 0x00005BB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2 GetVisualScale()
		{
			return new Vector2(this.LocalScale.X * this._renderProperties.ScaleMultiplier.X, this.LocalScale.Y * this._renderProperties.ScaleMultiplier.Y);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000079F3 File Offset: 0x00005BF3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddVisualOffset(float offsetX, float offsetY)
		{
			this._renderProperties.PositionOffsetPixel = this._renderProperties.PositionOffsetPixel + new Vector2(offsetX, offsetY);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00007A17 File Offset: 0x00005C17
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetVisualOffset(float offsetX, float offsetY)
		{
			this._renderProperties.PositionOffsetPixel = new Vector2(offsetX, offsetY);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00007A2B File Offset: 0x00005C2B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddVisualScale(float scaleX, float scaleY)
		{
			this._renderProperties.ScaleMultiplier = this._renderProperties.ScaleMultiplier + new Vector2(scaleX, scaleY);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00007A4F File Offset: 0x00005C4F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetVisualScale(float scaleX, float scaleY)
		{
			this._renderProperties.ScaleMultiplier = new Vector2(scaleX, scaleY);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00007A63 File Offset: 0x00005C63
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddVisualRotationOffset(float rotationOffset)
		{
			this._renderProperties.RotationOffset = this._renderProperties.RotationOffset + rotationOffset;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00007A75 File Offset: 0x00005C75
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetVisualRotationOffset(float rotationOffset)
		{
			this._renderProperties.RotationOffset = rotationOffset;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00007A84 File Offset: 0x00005C84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ValidateVisuals()
		{
			this._hasDifferentVisuals = this._renderProperties.PositionOffsetPixel.X != 0f || this._renderProperties.PositionOffsetPixel.Y != 0f || this._renderProperties.ScaleMultiplier.X != 1f || this._renderProperties.ScaleMultiplier.Y != 1f || this._renderProperties.RotationOffset != 0f;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00007B0B File Offset: 0x00005D0B
		public void DrawBoundingBox()
		{
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00007B0D File Offset: 0x00005D0D
		public void DrawCorners()
		{
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00007B10 File Offset: 0x00005D10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CalculateMatrixFrame(in Rectangle2D parentRectangle)
		{
			if (this.LocalScale == Vector2.Zero)
			{
				this.LocalScale = Vector2.One;
			}
			this._hasRotation = this.LocalRotation != 0f || this._renderProperties.RotationOffset != 0f;
			this.ValidateVisuals();
			this._cachedMatrixFrame = Rectangle2D.RectangleHelper.CreateMatrixFrame(this.LocalPosition.X, this.LocalPosition.Y, this.LocalPivot.X, this.LocalPivot.Y, this.LocalScale.X, this.LocalScale.Y, this.LocalRotation);
			if (parentRectangle.IsValid)
			{
				this._hasRotation = this._hasRotation || parentRectangle._hasRotation;
				if (this._hasRotation)
				{
					MatrixFrame cachedOrthonormalMatrix = parentRectangle._cachedOrthonormalMatrix;
					this._cachedMatrixFrame = cachedOrthonormalMatrix.TransformToParent(in this._cachedMatrixFrame);
				}
				else
				{
					this._cachedMatrixFrame.origin = this._cachedMatrixFrame.origin + parentRectangle._cachedOrthonormalMatrix.origin;
				}
			}
			this._cachedMatrixFrame.Fill();
			this._cachedOrigin = new Vector2(this._cachedMatrixFrame.origin.x, this._cachedMatrixFrame.origin.y);
			if (this._hasRotation)
			{
				this._cachedOrthonormalMatrix = this._cachedMatrixFrame;
				this._cachedOrthonormalMatrix.rotation.f.Normalize();
				this._cachedOrthonormalMatrix.rotation.s = Vec3.CrossProduct(this._cachedOrthonormalMatrix.rotation.f, this._cachedOrthonormalMatrix.rotation.u);
				this._cachedOrthonormalMatrix.rotation.s.Normalize();
				this._cachedOrthonormalMatrix.rotation.u = Vec3.CrossProduct(this._cachedOrthonormalMatrix.rotation.s, this._cachedOrthonormalMatrix.rotation.f);
			}
			else
			{
				this._cachedOrthonormalMatrix = MatrixFrame.Identity;
				this._cachedOrthonormalMatrix.origin = this._cachedMatrixFrame.origin;
			}
			this.TopLeft = this._cachedOrigin;
			this.TopRight = this._cachedOrigin + new Vector2(this._cachedMatrixFrame.rotation.s.x, this._cachedMatrixFrame.rotation.s.y);
			this.BottomRight = this._cachedOrigin + new Vector2(this._cachedMatrixFrame.rotation.s.x, this._cachedMatrixFrame.rotation.s.y) + new Vector2(this._cachedMatrixFrame.rotation.f.x, this._cachedMatrixFrame.rotation.f.y);
			this.BottomLeft = this._cachedOrigin + new Vector2(this._cachedMatrixFrame.rotation.f.x, this._cachedMatrixFrame.rotation.f.y);
			this._boundingBox = Rectangle2D.RectangleHelper.GetBoundingBox(in this);
			this._visualsNeedCalculation = true;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00007E40 File Offset: 0x00006040
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CalculateVisualMatrixFrame()
		{
			if (!this._visualsNeedCalculation)
			{
				return;
			}
			this._cachedVisualMatrixFrame = this._cachedMatrixFrame;
			if (!this._hasDifferentVisuals)
			{
				this._visualsNeedCalculation = false;
				return;
			}
			Vec3 vec = this._cachedVisualMatrixFrame.origin;
			Mat3 rotation = this._cachedOrthonormalMatrix.rotation;
			Vector2 localScale = this.LocalScale;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			Vec3 vec2 = new Vec3(localScale.X * -vector.X, localScale.Y * -vector.Y, 0f, -1f);
			vec -= rotation.TransformToParent(in vec2);
			rotation.RotateAboutUp(this._renderProperties.RotationOffset * 0.017453292f);
			vec += rotation.TransformToParent(in vec2);
			Vec3 vec3 = new Vec3(this.LocalScale.X, this.LocalScale.Y, 1f, -1f);
			rotation.ApplyScaleLocal(in vec3);
			this._cachedVisualMatrixFrame.origin = vec;
			this._cachedVisualMatrixFrame.rotation = rotation;
			vec3 = new Vec3(this._renderProperties.ScaleMultiplier.X, this._renderProperties.ScaleMultiplier.Y, 1f, -1f);
			this._cachedVisualMatrixFrame.rotation.ApplyScaleLocal(in vec3);
			if (this._renderProperties.ScaleMultiplier.X < 0f)
			{
				this._renderProperties.PositionOffsetPixel.X = this._renderProperties.PositionOffsetPixel.X - this._renderProperties.ScaleMultiplier.X * this.LocalScale.X;
			}
			if (this._renderProperties.ScaleMultiplier.Y < 0f)
			{
				this._renderProperties.PositionOffsetPixel.Y = this._renderProperties.PositionOffsetPixel.Y - this._renderProperties.ScaleMultiplier.Y * this.LocalScale.Y;
			}
			if (this._renderProperties.PositionOffsetPixel.X != 0f || this._renderProperties.PositionOffsetPixel.Y != 0f)
			{
				Vec3 origin = this._cachedVisualMatrixFrame.origin;
				vec3 = new Vec3(this._renderProperties.PositionOffsetPixel.X, this._renderProperties.PositionOffsetPixel.Y, 0f, -1f);
				this._cachedVisualMatrixFrame.origin = origin + this._cachedOrthonormalMatrix.rotation.TransformToParent(in vec3);
			}
			this._visualsNeedCalculation = false;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000080B3 File Offset: 0x000062B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2 GetCachedOrigin()
		{
			return this._cachedOrigin;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000080BB File Offset: 0x000062BB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MatrixFrame GetCachedMatrixFrame()
		{
			return this._cachedMatrixFrame;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000080C3 File Offset: 0x000062C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MatrixFrame GetCachedVisualMatrixFrame()
		{
			return this._cachedVisualMatrixFrame;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000080CB File Offset: 0x000062CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2 GetCenter()
		{
			return this._boundingBox.GetCenter();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000080D8 File Offset: 0x000062D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SimpleRectangle GetBoundingBox()
		{
			return this._boundingBox;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000080E0 File Offset: 0x000062E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsIdentical(in Rectangle2D other)
		{
			return (in this._cachedMatrixFrame) == (in other._cachedMatrixFrame) && (in this._cachedVisualMatrixFrame) == (in other._cachedVisualMatrixFrame);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00008108 File Offset: 0x00006308
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsCollide(in Rectangle2D other)
		{
			return Rectangle2D.RectangleHelper.DoRectanglesIntersect(in this, in other);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00008111 File Offset: 0x00006311
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSubRectOf(in Rectangle2D other)
		{
			return Rectangle2D.RectangleHelper.IsSubRectOf(in this, in other);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000811A File Offset: 0x0000631A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsPointInside(in Vector2 point)
		{
			return Rectangle2D.RectangleHelper.IsPointInside(in point, in this);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008124 File Offset: 0x00006324
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2 TransformScreenPositionToLocal(in Vector2 screenPosition)
		{
			Vec3 vec = new Vec3(screenPosition.X, screenPosition.Y, 0f, -1f);
			Vec3 vec2 = this._cachedOrthonormalMatrix.TransformToLocal(in vec);
			return new Vector2(vec2.x, vec2.y);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000816C File Offset: 0x0000636C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2 TransformLocalPositionToScreen(in Vector2 localPosition)
		{
			Vec3 vec = new Vec3(localPosition.X, localPosition.Y, 0f, -1f);
			Vec3 vec2 = this._cachedOrthonormalMatrix.TransformToParent(in vec);
			return new Vector2(vec2.x, vec2.y);
		}

		// Token: 0x040000EB RID: 235
		public bool IsValid;

		// Token: 0x040000EC RID: 236
		public Vector2 TopLeft;

		// Token: 0x040000ED RID: 237
		public Vector2 TopRight;

		// Token: 0x040000EE RID: 238
		public Vector2 BottomRight;

		// Token: 0x040000EF RID: 239
		public Vector2 BottomLeft;

		// Token: 0x040000F0 RID: 240
		public Vector2 LocalPosition;

		// Token: 0x040000F1 RID: 241
		public Vector2 LocalScale;

		// Token: 0x040000F2 RID: 242
		public Vector2 LocalPivot;

		// Token: 0x040000F3 RID: 243
		public float LocalRotation;

		// Token: 0x040000F4 RID: 244
		private Rectangle2D.RectangleRenderProperties _renderProperties;

		// Token: 0x040000F5 RID: 245
		private bool _hasDifferentVisuals;

		// Token: 0x040000F6 RID: 246
		private bool _visualsNeedCalculation;

		// Token: 0x040000F7 RID: 247
		private Vector2 _cachedOrigin;

		// Token: 0x040000F8 RID: 248
		private MatrixFrame _cachedOrthonormalMatrix;

		// Token: 0x040000F9 RID: 249
		private MatrixFrame _cachedMatrixFrame;

		// Token: 0x040000FA RID: 250
		private MatrixFrame _cachedVisualMatrixFrame;

		// Token: 0x040000FB RID: 251
		private SimpleRectangle _boundingBox;

		// Token: 0x040000FC RID: 252
		private bool _hasRotation;

		// Token: 0x02000042 RID: 66
		private struct RectangleRenderProperties
		{
			// Token: 0x060002BE RID: 702 RVA: 0x0000A36C File Offset: 0x0000856C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle2D.RectangleRenderProperties CreateEmpty()
			{
				return new Rectangle2D.RectangleRenderProperties
				{
					ScaleMultiplier = Vector2.One,
					PositionOffsetPixel = Vector2.Zero,
					RotationOffset = 0f
				};
			}

			// Token: 0x060002BF RID: 703 RVA: 0x0000A3A6 File Offset: 0x000085A6
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal void FillValuesFrom(Rectangle2D.RectangleRenderProperties other)
			{
				this.ScaleMultiplier = other.ScaleMultiplier;
				this.PositionOffsetPixel = other.PositionOffsetPixel;
				this.RotationOffset = other.RotationOffset;
			}

			// Token: 0x0400015F RID: 351
			public Vector2 ScaleMultiplier;

			// Token: 0x04000160 RID: 352
			public Vector2 PositionOffsetPixel;

			// Token: 0x04000161 RID: 353
			public float RotationOffset;
		}

		// Token: 0x02000043 RID: 67
		private static class RectangleHelper
		{
			// Token: 0x060002C0 RID: 704 RVA: 0x0000A3CC File Offset: 0x000085CC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static MatrixFrame CreateMatrixFrame(float posX, float posY, float pivotX, float pivotY, float scaleX, float scaleY, float rotation)
			{
				if (rotation == 0f)
				{
					return new MatrixFrame(scaleX, 0f, 0f, 0f, 0f, scaleY, 0f, 0f, 0f, 0f, 1f, 0f, posX, posY, 0f, 1f);
				}
				Mat3 mat = new Mat3(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f);
				Vec3 vec = new Vec3(posX, posY, 1f, -1f);
				Vec3 vec2 = new Vec3(scaleX * -pivotX, scaleY * -pivotY, 0f, -1f);
				vec -= mat.TransformToParent(in vec2);
				mat.RotateAboutUp(rotation * 0.017453292f);
				vec += mat.TransformToParent(in vec2);
				Vec3 vec3 = new Vec3(scaleX, scaleY, 1f, -1f);
				mat.ApplyScaleLocal(in vec3);
				MatrixFrame matrixFrame = new MatrixFrame(in mat, in vec);
				matrixFrame.Fill();
				return matrixFrame;
			}

			// Token: 0x060002C1 RID: 705 RVA: 0x0000A4EC File Offset: 0x000086EC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static SimpleRectangle GetBoundingBox(in Rectangle2D rectangle)
			{
				if (!rectangle._hasRotation)
				{
					return new SimpleRectangle(rectangle.TopLeft.X, rectangle.TopLeft.Y, rectangle.BottomRight.X - rectangle.TopLeft.X, rectangle.BottomRight.Y - rectangle.TopLeft.Y);
				}
				Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
				Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
				Vector2[] array = new Vector2[] { rectangle.TopLeft, rectangle.TopRight, rectangle.BottomRight, rectangle.BottomLeft };
				for (int i = 0; i < array.Length; i++)
				{
					vector.X = Mathf.Min(array[i].X, vector.X);
					vector.Y = Mathf.Min(array[i].Y, vector.Y);
					vector2.X = Mathf.Max(array[i].X, vector2.X);
					vector2.Y = Mathf.Max(array[i].Y, vector2.Y);
				}
				return new SimpleRectangle(vector.X, vector.Y, vector2.X - vector.X, vector2.Y - vector.Y);
			}

			// Token: 0x060002C2 RID: 706 RVA: 0x0000A65C File Offset: 0x0000885C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static float GetTwoDimensionalCrossProduct(in Vector2 p1, in Vector2 p2)
			{
				return p1.X * p2.Y - p1.Y * p2.X;
			}

			// Token: 0x060002C3 RID: 707 RVA: 0x0000A67C File Offset: 0x0000887C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static bool AreLinesIntersecting(in Vector2 line1Start, in Vector2 line1End, in Vector2 line2Start, in Vector2 line2End)
			{
				Vector2 vector = line1Start;
				Vector2 vector2 = line2Start;
				Vector2 vector3 = line1End - line1Start;
				Vector2 vector4 = line2End - line2Start;
				float twoDimensionalCrossProduct = Rectangle2D.RectangleHelper.GetTwoDimensionalCrossProduct(in vector3, in vector4);
				Vector2 vector5 = vector2 - vector;
				float num = Rectangle2D.RectangleHelper.GetTwoDimensionalCrossProduct(in vector5, in vector4) / twoDimensionalCrossProduct;
				vector5 = vector2 - vector;
				float num2 = Rectangle2D.RectangleHelper.GetTwoDimensionalCrossProduct(in vector5, in vector3) / twoDimensionalCrossProduct;
				return num >= 0f && num <= 1f && num2 >= 0f && num2 <= 1f;
			}

			// Token: 0x060002C4 RID: 708 RVA: 0x0000A718 File Offset: 0x00008918
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool DoRectanglesIntersect(in Rectangle2D rect1, in Rectangle2D rect2)
			{
				SimpleRectangle boundingBox = rect1._boundingBox;
				if (!boundingBox.IsCollide(rect2._boundingBox))
				{
					return false;
				}
				Rectangle2D rectangle2D = rect1;
				if (!rectangle2D.IsSubRectOf(in rect2))
				{
					rectangle2D = rect2;
					if (!rectangle2D.IsSubRectOf(in rect1))
					{
						if (!rect1._hasRotation && !rect2._hasRotation)
						{
							return true;
						}
						Vector2[] array = new Vector2[] { rect1.TopLeft, rect1.TopRight, rect1.BottomRight, rect1.BottomLeft };
						Vector2[] array2 = new Vector2[] { rect2.TopLeft, rect2.TopRight, rect2.BottomRight, rect2.BottomLeft };
						for (int i = 0; i < array.Length; i++)
						{
							for (int j = i + 1; j < array.Length; j++)
							{
								Vector2 vector = array[i];
								Vector2 vector2 = array[j];
								for (int k = 0; k < array.Length; k++)
								{
									for (int l = k + 1; l < array.Length; l++)
									{
										Vector2 vector3 = array[k];
										Vector2 vector4 = array[l];
										if (Rectangle2D.RectangleHelper.AreLinesIntersecting(in vector, in vector2, in vector3, in vector4))
										{
											return true;
										}
									}
								}
							}
						}
						return false;
					}
				}
				return true;
			}

			// Token: 0x060002C5 RID: 709 RVA: 0x0000A86C File Offset: 0x00008A6C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool IsPointInside(in Vector2 point, in Rectangle2D rect)
			{
				SimpleRectangle boundingBox = rect._boundingBox;
				if (!boundingBox.IsPointInside(point))
				{
					return false;
				}
				if (!rect._hasRotation)
				{
					return true;
				}
				Rectangle2D rectangle2D = rect;
				Vector2 vector = rectangle2D.TransformScreenPositionToLocal(in point);
				return vector.X >= 0f && vector.X < rect.LocalScale.X && vector.Y >= 0f && vector.Y < rect.LocalScale.Y;
			}

			// Token: 0x060002C6 RID: 710 RVA: 0x0000A8F0 File Offset: 0x00008AF0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool IsSubRectOf(in Rectangle2D rect1, in Rectangle2D rect2)
			{
				SimpleRectangle boundingBox = rect1._boundingBox;
				return boundingBox.IsSubRectOf(rect2._boundingBox) && ((!rect1._hasRotation && !rect2._hasRotation) || (Rectangle2D.RectangleHelper.IsPointInside(in rect1.TopLeft, in rect2) && Rectangle2D.RectangleHelper.IsPointInside(in rect1.TopRight, in rect2) && Rectangle2D.RectangleHelper.IsPointInside(in rect1.BottomRight, in rect2) && Rectangle2D.RectangleHelper.IsPointInside(in rect1.BottomLeft, in rect2)));
			}
		}
	}
}
