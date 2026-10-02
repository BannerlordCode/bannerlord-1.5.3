using System;
using System.Globalization;

namespace System.Numerics
{
	// Token: 0x0200000B RID: 11
	public struct Matrix3x2 : IEquatable<Matrix3x2>
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000152E3 File Offset: 0x000134E3
		public static Matrix3x2 Identity
		{
			get
			{
				return Matrix3x2._identity;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000152EC File Offset: 0x000134EC
		public bool IsIdentity
		{
			get
			{
				return this.M11 == 1f && this.M22 == 1f && this.M12 == 0f && this.M21 == 0f && this.M31 == 0f && this.M32 == 0f;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00015349 File Offset: 0x00013549
		// (set) Token: 0x060000BD RID: 189 RVA: 0x0001535C File Offset: 0x0001355C
		public Vector2 Translation
		{
			get
			{
				return new Vector2(this.M31, this.M32);
			}
			set
			{
				this.M31 = value.X;
				this.M32 = value.Y;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00015376 File Offset: 0x00013576
		public Matrix3x2(float m11, float m12, float m21, float m22, float m31, float m32)
		{
			this.M11 = m11;
			this.M12 = m12;
			this.M21 = m21;
			this.M22 = m22;
			this.M31 = m31;
			this.M32 = m32;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000153A8 File Offset: 0x000135A8
		public static Matrix3x2 CreateTranslation(Vector2 position)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = 1f;
			matrix3x.M12 = 0f;
			matrix3x.M21 = 0f;
			matrix3x.M22 = 1f;
			matrix3x.M31 = position.X;
			matrix3x.M32 = position.Y;
			return matrix3x;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00015400 File Offset: 0x00013600
		public static Matrix3x2 CreateTranslation(float xPosition, float yPosition)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = 1f;
			matrix3x.M12 = 0f;
			matrix3x.M21 = 0f;
			matrix3x.M22 = 1f;
			matrix3x.M31 = xPosition;
			matrix3x.M32 = yPosition;
			return matrix3x;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00015450 File Offset: 0x00013650
		public static Matrix3x2 CreateScale(float xScale, float yScale)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = xScale;
			matrix3x.M12 = 0f;
			matrix3x.M21 = 0f;
			matrix3x.M22 = yScale;
			matrix3x.M31 = 0f;
			matrix3x.M32 = 0f;
			return matrix3x;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000154A0 File Offset: 0x000136A0
		public static Matrix3x2 CreateScale(float xScale, float yScale, Vector2 centerPoint)
		{
			float num = centerPoint.X * (1f - xScale);
			float num2 = centerPoint.Y * (1f - yScale);
			Matrix3x2 matrix3x;
			matrix3x.M11 = xScale;
			matrix3x.M12 = 0f;
			matrix3x.M21 = 0f;
			matrix3x.M22 = yScale;
			matrix3x.M31 = num;
			matrix3x.M32 = num2;
			return matrix3x;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00015504 File Offset: 0x00013704
		public static Matrix3x2 CreateScale(Vector2 scales)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = scales.X;
			matrix3x.M12 = 0f;
			matrix3x.M21 = 0f;
			matrix3x.M22 = scales.Y;
			matrix3x.M31 = 0f;
			matrix3x.M32 = 0f;
			return matrix3x;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0001555C File Offset: 0x0001375C
		public static Matrix3x2 CreateScale(Vector2 scales, Vector2 centerPoint)
		{
			float num = centerPoint.X * (1f - scales.X);
			float num2 = centerPoint.Y * (1f - scales.Y);
			Matrix3x2 matrix3x;
			matrix3x.M11 = scales.X;
			matrix3x.M12 = 0f;
			matrix3x.M21 = 0f;
			matrix3x.M22 = scales.Y;
			matrix3x.M31 = num;
			matrix3x.M32 = num2;
			return matrix3x;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000155D4 File Offset: 0x000137D4
		public static Matrix3x2 CreateScale(float scale)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = scale;
			matrix3x.M12 = 0f;
			matrix3x.M21 = 0f;
			matrix3x.M22 = scale;
			matrix3x.M31 = 0f;
			matrix3x.M32 = 0f;
			return matrix3x;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00015624 File Offset: 0x00013824
		public static Matrix3x2 CreateScale(float scale, Vector2 centerPoint)
		{
			float num = centerPoint.X * (1f - scale);
			float num2 = centerPoint.Y * (1f - scale);
			Matrix3x2 matrix3x;
			matrix3x.M11 = scale;
			matrix3x.M12 = 0f;
			matrix3x.M21 = 0f;
			matrix3x.M22 = scale;
			matrix3x.M31 = num;
			matrix3x.M32 = num2;
			return matrix3x;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00015688 File Offset: 0x00013888
		public static Matrix3x2 CreateSkew(float radiansX, float radiansY)
		{
			float num = MathF.Tan(radiansX);
			float num2 = MathF.Tan(radiansY);
			Matrix3x2 matrix3x;
			matrix3x.M11 = 1f;
			matrix3x.M12 = num2;
			matrix3x.M21 = num;
			matrix3x.M22 = 1f;
			matrix3x.M31 = 0f;
			matrix3x.M32 = 0f;
			return matrix3x;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000156E4 File Offset: 0x000138E4
		public static Matrix3x2 CreateSkew(float radiansX, float radiansY, Vector2 centerPoint)
		{
			float num = MathF.Tan(radiansX);
			float num2 = MathF.Tan(radiansY);
			float num3 = -centerPoint.Y * num;
			float num4 = -centerPoint.X * num2;
			Matrix3x2 matrix3x;
			matrix3x.M11 = 1f;
			matrix3x.M12 = num2;
			matrix3x.M21 = num;
			matrix3x.M22 = 1f;
			matrix3x.M31 = num3;
			matrix3x.M32 = num4;
			return matrix3x;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00015750 File Offset: 0x00013950
		public static Matrix3x2 CreateRotation(float radians)
		{
			radians = MathF.IEEERemainder(radians, 6.2831855f);
			float num;
			float num2;
			if (radians > -1.7453294E-05f && radians < 1.7453294E-05f)
			{
				num = 1f;
				num2 = 0f;
			}
			else if (radians > 1.570779f && radians < 1.5708138f)
			{
				num = 0f;
				num2 = 1f;
			}
			else if (radians < -3.1415753f || radians > 3.1415753f)
			{
				num = -1f;
				num2 = 0f;
			}
			else if (radians > -1.5708138f && radians < -1.570779f)
			{
				num = 0f;
				num2 = -1f;
			}
			else
			{
				num = MathF.Cos(radians);
				num2 = MathF.Sin(radians);
			}
			Matrix3x2 matrix3x;
			matrix3x.M11 = num;
			matrix3x.M12 = num2;
			matrix3x.M21 = -num2;
			matrix3x.M22 = num;
			matrix3x.M31 = 0f;
			matrix3x.M32 = 0f;
			return matrix3x;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0001582C File Offset: 0x00013A2C
		public static Matrix3x2 CreateRotation(float radians, Vector2 centerPoint)
		{
			radians = MathF.IEEERemainder(radians, 6.2831855f);
			float num;
			float num2;
			if (radians > -1.7453294E-05f && radians < 1.7453294E-05f)
			{
				num = 1f;
				num2 = 0f;
			}
			else if (radians > 1.570779f && radians < 1.5708138f)
			{
				num = 0f;
				num2 = 1f;
			}
			else if (radians < -3.1415753f || radians > 3.1415753f)
			{
				num = -1f;
				num2 = 0f;
			}
			else if (radians > -1.5708138f && radians < -1.570779f)
			{
				num = 0f;
				num2 = -1f;
			}
			else
			{
				num = MathF.Cos(radians);
				num2 = MathF.Sin(radians);
			}
			float num3 = centerPoint.X * (1f - num) + centerPoint.Y * num2;
			float num4 = centerPoint.Y * (1f - num) - centerPoint.X * num2;
			Matrix3x2 matrix3x;
			matrix3x.M11 = num;
			matrix3x.M12 = num2;
			matrix3x.M21 = -num2;
			matrix3x.M22 = num;
			matrix3x.M31 = num3;
			matrix3x.M32 = num4;
			return matrix3x;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00015930 File Offset: 0x00013B30
		public float GetDeterminant()
		{
			return this.M11 * this.M22 - this.M21 * this.M12;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00015950 File Offset: 0x00013B50
		public static bool Invert(Matrix3x2 matrix, out Matrix3x2 result)
		{
			float num = matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12;
			if (MathF.Abs(num) < 1E-45f)
			{
				result = new Matrix3x2(float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN);
				return false;
			}
			float num2 = 1f / num;
			result.M11 = matrix.M22 * num2;
			result.M12 = -matrix.M12 * num2;
			result.M21 = -matrix.M21 * num2;
			result.M22 = matrix.M11 * num2;
			result.M31 = (matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22) * num2;
			result.M32 = (matrix.M31 * matrix.M12 - matrix.M11 * matrix.M32) * num2;
			return true;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00015A3C File Offset: 0x00013C3C
		public static Matrix3x2 Lerp(Matrix3x2 matrix1, Matrix3x2 matrix2, float amount)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
			matrix3x.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
			matrix3x.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
			matrix3x.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
			matrix3x.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
			matrix3x.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
			return matrix3x;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00015AF8 File Offset: 0x00013CF8
		public static Matrix3x2 Negate(Matrix3x2 value)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = -value.M11;
			matrix3x.M12 = -value.M12;
			matrix3x.M21 = -value.M21;
			matrix3x.M22 = -value.M22;
			matrix3x.M31 = -value.M31;
			matrix3x.M32 = -value.M32;
			return matrix3x;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00015B5C File Offset: 0x00013D5C
		public static Matrix3x2 Add(Matrix3x2 value1, Matrix3x2 value2)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = value1.M11 + value2.M11;
			matrix3x.M12 = value1.M12 + value2.M12;
			matrix3x.M21 = value1.M21 + value2.M21;
			matrix3x.M22 = value1.M22 + value2.M22;
			matrix3x.M31 = value1.M31 + value2.M31;
			matrix3x.M32 = value1.M32 + value2.M32;
			return matrix3x;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00015BE4 File Offset: 0x00013DE4
		public static Matrix3x2 Subtract(Matrix3x2 value1, Matrix3x2 value2)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = value1.M11 - value2.M11;
			matrix3x.M12 = value1.M12 - value2.M12;
			matrix3x.M21 = value1.M21 - value2.M21;
			matrix3x.M22 = value1.M22 - value2.M22;
			matrix3x.M31 = value1.M31 - value2.M31;
			matrix3x.M32 = value1.M32 - value2.M32;
			return matrix3x;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00015C6C File Offset: 0x00013E6C
		public static Matrix3x2 Multiply(Matrix3x2 value1, Matrix3x2 value2)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21;
			matrix3x.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22;
			matrix3x.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21;
			matrix3x.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22;
			matrix3x.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value2.M31;
			matrix3x.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value2.M32;
			return matrix3x;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00015D54 File Offset: 0x00013F54
		public static Matrix3x2 Multiply(Matrix3x2 value1, float value2)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = value1.M11 * value2;
			matrix3x.M12 = value1.M12 * value2;
			matrix3x.M21 = value1.M21 * value2;
			matrix3x.M22 = value1.M22 * value2;
			matrix3x.M31 = value1.M31 * value2;
			matrix3x.M32 = value1.M32 * value2;
			return matrix3x;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00015DBC File Offset: 0x00013FBC
		public static Matrix3x2 operator -(Matrix3x2 value)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = -value.M11;
			matrix3x.M12 = -value.M12;
			matrix3x.M21 = -value.M21;
			matrix3x.M22 = -value.M22;
			matrix3x.M31 = -value.M31;
			matrix3x.M32 = -value.M32;
			return matrix3x;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00015E20 File Offset: 0x00014020
		public static Matrix3x2 operator +(Matrix3x2 value1, Matrix3x2 value2)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = value1.M11 + value2.M11;
			matrix3x.M12 = value1.M12 + value2.M12;
			matrix3x.M21 = value1.M21 + value2.M21;
			matrix3x.M22 = value1.M22 + value2.M22;
			matrix3x.M31 = value1.M31 + value2.M31;
			matrix3x.M32 = value1.M32 + value2.M32;
			return matrix3x;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00015EA8 File Offset: 0x000140A8
		public static Matrix3x2 operator -(Matrix3x2 value1, Matrix3x2 value2)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = value1.M11 - value2.M11;
			matrix3x.M12 = value1.M12 - value2.M12;
			matrix3x.M21 = value1.M21 - value2.M21;
			matrix3x.M22 = value1.M22 - value2.M22;
			matrix3x.M31 = value1.M31 - value2.M31;
			matrix3x.M32 = value1.M32 - value2.M32;
			return matrix3x;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00015F30 File Offset: 0x00014130
		public static Matrix3x2 operator *(Matrix3x2 value1, Matrix3x2 value2)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21;
			matrix3x.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22;
			matrix3x.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21;
			matrix3x.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22;
			matrix3x.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value2.M31;
			matrix3x.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value2.M32;
			return matrix3x;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00016018 File Offset: 0x00014218
		public static Matrix3x2 operator *(Matrix3x2 value1, float value2)
		{
			Matrix3x2 matrix3x;
			matrix3x.M11 = value1.M11 * value2;
			matrix3x.M12 = value1.M12 * value2;
			matrix3x.M21 = value1.M21 * value2;
			matrix3x.M22 = value1.M22 * value2;
			matrix3x.M31 = value1.M31 * value2;
			matrix3x.M32 = value1.M32 * value2;
			return matrix3x;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00016080 File Offset: 0x00014280
		public static bool operator ==(Matrix3x2 value1, Matrix3x2 value2)
		{
			return value1.M11 == value2.M11 && value1.M22 == value2.M22 && value1.M12 == value2.M12 && value1.M21 == value2.M21 && value1.M31 == value2.M31 && value1.M32 == value2.M32;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000160E4 File Offset: 0x000142E4
		public static bool operator !=(Matrix3x2 value1, Matrix3x2 value2)
		{
			return value1.M11 != value2.M11 || value1.M12 != value2.M12 || value1.M21 != value2.M21 || value1.M22 != value2.M22 || value1.M31 != value2.M31 || value1.M32 != value2.M32;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0001614C File Offset: 0x0001434C
		public bool Equals(Matrix3x2 other)
		{
			return this.M11 == other.M11 && this.M22 == other.M22 && this.M12 == other.M12 && this.M21 == other.M21 && this.M31 == other.M31 && this.M32 == other.M32;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000161AF File Offset: 0x000143AF
		public override bool Equals(object obj)
		{
			return obj is Matrix3x2 && this.Equals((Matrix3x2)obj);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000161C8 File Offset: 0x000143C8
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{ {{M11:{0} M12:{1}}} {{M21:{2} M22:{3}}} {{M31:{4} M32:{5}}} }}", new object[]
			{
				this.M11.ToString(currentCulture),
				this.M12.ToString(currentCulture),
				this.M21.ToString(currentCulture),
				this.M22.ToString(currentCulture),
				this.M31.ToString(currentCulture),
				this.M32.ToString(currentCulture)
			});
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00016248 File Offset: 0x00014448
		public override int GetHashCode()
		{
			return this.M11.GetHashCode() + this.M12.GetHashCode() + this.M21.GetHashCode() + this.M22.GetHashCode() + this.M31.GetHashCode() + this.M32.GetHashCode();
		}

		// Token: 0x0400004B RID: 75
		public float M11;

		// Token: 0x0400004C RID: 76
		public float M12;

		// Token: 0x0400004D RID: 77
		public float M21;

		// Token: 0x0400004E RID: 78
		public float M22;

		// Token: 0x0400004F RID: 79
		public float M31;

		// Token: 0x04000050 RID: 80
		public float M32;

		// Token: 0x04000051 RID: 81
		private static readonly Matrix3x2 _identity = new Matrix3x2(1f, 0f, 0f, 1f, 0f, 0f);
	}
}
