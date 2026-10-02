using System;
using System.Globalization;

namespace System.Numerics
{
	// Token: 0x0200000E RID: 14
	public struct Quaternion : IEquatable<Quaternion>
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0001ADF6 File Offset: 0x00018FF6
		public static Quaternion Identity
		{
			get
			{
				return new Quaternion(0f, 0f, 0f, 1f);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0001AE11 File Offset: 0x00019011
		public bool IsIdentity
		{
			get
			{
				return this.X == 0f && this.Y == 0f && this.Z == 0f && this.W == 1f;
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0001AE49 File Offset: 0x00019049
		public Quaternion(float x, float y, float z, float w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0001AE68 File Offset: 0x00019068
		public Quaternion(Vector3 vectorPart, float scalarPart)
		{
			this.X = vectorPart.X;
			this.Y = vectorPart.Y;
			this.Z = vectorPart.Z;
			this.W = scalarPart;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0001AE98 File Offset: 0x00019098
		public float Length()
		{
			float num = this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
			return MathF.Sqrt(num);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0001AEE3 File Offset: 0x000190E3
		public float LengthSquared()
		{
			return this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0001AF1C File Offset: 0x0001911C
		public static Quaternion Normalize(Quaternion value)
		{
			float num = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;
			float num2 = 1f / MathF.Sqrt(num);
			Quaternion quaternion;
			quaternion.X = value.X * num2;
			quaternion.Y = value.Y * num2;
			quaternion.Z = value.Z * num2;
			quaternion.W = value.W * num2;
			return quaternion;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0001AFAC File Offset: 0x000191AC
		public static Quaternion Conjugate(Quaternion value)
		{
			Quaternion quaternion;
			quaternion.X = -value.X;
			quaternion.Y = -value.Y;
			quaternion.Z = -value.Z;
			quaternion.W = value.W;
			return quaternion;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0001AFF4 File Offset: 0x000191F4
		public static Quaternion Inverse(Quaternion value)
		{
			float num = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;
			float num2 = 1f / num;
			Quaternion quaternion;
			quaternion.X = -value.X * num2;
			quaternion.Y = -value.Y * num2;
			quaternion.Z = -value.Z * num2;
			quaternion.W = value.W * num2;
			return quaternion;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0001B084 File Offset: 0x00019284
		public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
		{
			float num = angle * 0.5f;
			float num2 = MathF.Sin(num);
			float num3 = MathF.Cos(num);
			Quaternion quaternion;
			quaternion.X = axis.X * num2;
			quaternion.Y = axis.Y * num2;
			quaternion.Z = axis.Z * num2;
			quaternion.W = num3;
			return quaternion;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0001B0E0 File Offset: 0x000192E0
		public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
		{
			float num = roll * 0.5f;
			float num2 = MathF.Sin(num);
			float num3 = MathF.Cos(num);
			float num4 = pitch * 0.5f;
			float num5 = MathF.Sin(num4);
			float num6 = MathF.Cos(num4);
			float num7 = yaw * 0.5f;
			float num8 = MathF.Sin(num7);
			float num9 = MathF.Cos(num7);
			Quaternion quaternion;
			quaternion.X = num9 * num5 * num3 + num8 * num6 * num2;
			quaternion.Y = num8 * num6 * num3 - num9 * num5 * num2;
			quaternion.Z = num9 * num6 * num2 - num8 * num5 * num3;
			quaternion.W = num9 * num6 * num3 + num8 * num5 * num2;
			return quaternion;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0001B18C File Offset: 0x0001938C
		public static Quaternion CreateFromRotationMatrix(Matrix4x4 matrix)
		{
			float num = matrix.M11 + matrix.M22 + matrix.M33;
			Quaternion quaternion = default(Quaternion);
			if (num > 0f)
			{
				float num2 = MathF.Sqrt(num + 1f);
				quaternion.W = num2 * 0.5f;
				num2 = 0.5f / num2;
				quaternion.X = (matrix.M23 - matrix.M32) * num2;
				quaternion.Y = (matrix.M31 - matrix.M13) * num2;
				quaternion.Z = (matrix.M12 - matrix.M21) * num2;
			}
			else if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
			{
				float num3 = MathF.Sqrt(1f + matrix.M11 - matrix.M22 - matrix.M33);
				float num4 = 0.5f / num3;
				quaternion.X = 0.5f * num3;
				quaternion.Y = (matrix.M12 + matrix.M21) * num4;
				quaternion.Z = (matrix.M13 + matrix.M31) * num4;
				quaternion.W = (matrix.M23 - matrix.M32) * num4;
			}
			else if (matrix.M22 > matrix.M33)
			{
				float num5 = MathF.Sqrt(1f + matrix.M22 - matrix.M11 - matrix.M33);
				float num6 = 0.5f / num5;
				quaternion.X = (matrix.M21 + matrix.M12) * num6;
				quaternion.Y = 0.5f * num5;
				quaternion.Z = (matrix.M32 + matrix.M23) * num6;
				quaternion.W = (matrix.M31 - matrix.M13) * num6;
			}
			else
			{
				float num7 = MathF.Sqrt(1f + matrix.M33 - matrix.M11 - matrix.M22);
				float num8 = 0.5f / num7;
				quaternion.X = (matrix.M31 + matrix.M13) * num8;
				quaternion.Y = (matrix.M32 + matrix.M23) * num8;
				quaternion.Z = 0.5f * num7;
				quaternion.W = (matrix.M12 - matrix.M21) * num8;
			}
			return quaternion;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0001B3DD File Offset: 0x000195DD
		public static float Dot(Quaternion quaternion1, Quaternion quaternion2)
		{
			return quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0001B418 File Offset: 0x00019618
		public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
		{
			float num = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
			bool flag = false;
			if (num < 0f)
			{
				flag = true;
				num = -num;
			}
			float num2;
			float num3;
			if (num > 0.999999f)
			{
				num2 = 1f - amount;
				num3 = (flag ? (-amount) : amount);
			}
			else
			{
				float num4 = MathF.Acos(num);
				float num5 = 1f / MathF.Sin(num4);
				num2 = MathF.Sin((1f - amount) * num4) * num5;
				num3 = (flag ? (-MathF.Sin(amount * num4) * num5) : (MathF.Sin(amount * num4) * num5));
			}
			Quaternion quaternion3;
			quaternion3.X = num2 * quaternion1.X + num3 * quaternion2.X;
			quaternion3.Y = num2 * quaternion1.Y + num3 * quaternion2.Y;
			quaternion3.Z = num2 * quaternion1.Z + num3 * quaternion2.Z;
			quaternion3.W = num2 * quaternion1.W + num3 * quaternion2.W;
			return quaternion3;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0001B53C File Offset: 0x0001973C
		public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
		{
			float num = 1f - amount;
			Quaternion quaternion3 = default(Quaternion);
			float num2 = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
			if (num2 >= 0f)
			{
				quaternion3.X = num * quaternion1.X + amount * quaternion2.X;
				quaternion3.Y = num * quaternion1.Y + amount * quaternion2.Y;
				quaternion3.Z = num * quaternion1.Z + amount * quaternion2.Z;
				quaternion3.W = num * quaternion1.W + amount * quaternion2.W;
			}
			else
			{
				quaternion3.X = num * quaternion1.X - amount * quaternion2.X;
				quaternion3.Y = num * quaternion1.Y - amount * quaternion2.Y;
				quaternion3.Z = num * quaternion1.Z - amount * quaternion2.Z;
				quaternion3.W = num * quaternion1.W - amount * quaternion2.W;
			}
			float num3 = quaternion3.X * quaternion3.X + quaternion3.Y * quaternion3.Y + quaternion3.Z * quaternion3.Z + quaternion3.W * quaternion3.W;
			float num4 = 1f / MathF.Sqrt(num3);
			quaternion3.X *= num4;
			quaternion3.Y *= num4;
			quaternion3.Z *= num4;
			quaternion3.W *= num4;
			return quaternion3;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0001B6DC File Offset: 0x000198DC
		public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
		{
			float x = value2.X;
			float y = value2.Y;
			float z = value2.Z;
			float w = value2.W;
			float x2 = value1.X;
			float y2 = value1.Y;
			float z2 = value1.Z;
			float w2 = value1.W;
			float num = y * z2 - z * y2;
			float num2 = z * x2 - x * z2;
			float num3 = x * y2 - y * x2;
			float num4 = x * x2 + y * y2 + z * z2;
			Quaternion quaternion;
			quaternion.X = x * w2 + x2 * w + num;
			quaternion.Y = y * w2 + y2 * w + num2;
			quaternion.Z = z * w2 + z2 * w + num3;
			quaternion.W = w * w2 - num4;
			return quaternion;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0001B7A4 File Offset: 0x000199A4
		public static Quaternion Negate(Quaternion value)
		{
			Quaternion quaternion;
			quaternion.X = -value.X;
			quaternion.Y = -value.Y;
			quaternion.Z = -value.Z;
			quaternion.W = -value.W;
			return quaternion;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0001B7EC File Offset: 0x000199EC
		public static Quaternion Add(Quaternion value1, Quaternion value2)
		{
			Quaternion quaternion;
			quaternion.X = value1.X + value2.X;
			quaternion.Y = value1.Y + value2.Y;
			quaternion.Z = value1.Z + value2.Z;
			quaternion.W = value1.W + value2.W;
			return quaternion;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0001B84C File Offset: 0x00019A4C
		public static Quaternion Subtract(Quaternion value1, Quaternion value2)
		{
			Quaternion quaternion;
			quaternion.X = value1.X - value2.X;
			quaternion.Y = value1.Y - value2.Y;
			quaternion.Z = value1.Z - value2.Z;
			quaternion.W = value1.W - value2.W;
			return quaternion;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0001B8AC File Offset: 0x00019AAC
		public static Quaternion Multiply(Quaternion value1, Quaternion value2)
		{
			float x = value1.X;
			float y = value1.Y;
			float z = value1.Z;
			float w = value1.W;
			float x2 = value2.X;
			float y2 = value2.Y;
			float z2 = value2.Z;
			float w2 = value2.W;
			float num = y * z2 - z * y2;
			float num2 = z * x2 - x * z2;
			float num3 = x * y2 - y * x2;
			float num4 = x * x2 + y * y2 + z * z2;
			Quaternion quaternion;
			quaternion.X = x * w2 + x2 * w + num;
			quaternion.Y = y * w2 + y2 * w + num2;
			quaternion.Z = z * w2 + z2 * w + num3;
			quaternion.W = w * w2 - num4;
			return quaternion;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0001B974 File Offset: 0x00019B74
		public static Quaternion Multiply(Quaternion value1, float value2)
		{
			Quaternion quaternion;
			quaternion.X = value1.X * value2;
			quaternion.Y = value1.Y * value2;
			quaternion.Z = value1.Z * value2;
			quaternion.W = value1.W * value2;
			return quaternion;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0001B9C0 File Offset: 0x00019BC0
		public static Quaternion Divide(Quaternion value1, Quaternion value2)
		{
			float x = value1.X;
			float y = value1.Y;
			float z = value1.Z;
			float w = value1.W;
			float num = value2.X * value2.X + value2.Y * value2.Y + value2.Z * value2.Z + value2.W * value2.W;
			float num2 = 1f / num;
			float num3 = -value2.X * num2;
			float num4 = -value2.Y * num2;
			float num5 = -value2.Z * num2;
			float num6 = value2.W * num2;
			float num7 = y * num5 - z * num4;
			float num8 = z * num3 - x * num5;
			float num9 = x * num4 - y * num3;
			float num10 = x * num3 + y * num4 + z * num5;
			Quaternion quaternion;
			quaternion.X = x * num6 + num3 * w + num7;
			quaternion.Y = y * num6 + num4 * w + num8;
			quaternion.Z = z * num6 + num5 * w + num9;
			quaternion.W = w * num6 - num10;
			return quaternion;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0001BADC File Offset: 0x00019CDC
		public static Quaternion operator -(Quaternion value)
		{
			Quaternion quaternion;
			quaternion.X = -value.X;
			quaternion.Y = -value.Y;
			quaternion.Z = -value.Z;
			quaternion.W = -value.W;
			return quaternion;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0001BB24 File Offset: 0x00019D24
		public static Quaternion operator +(Quaternion value1, Quaternion value2)
		{
			Quaternion quaternion;
			quaternion.X = value1.X + value2.X;
			quaternion.Y = value1.Y + value2.Y;
			quaternion.Z = value1.Z + value2.Z;
			quaternion.W = value1.W + value2.W;
			return quaternion;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0001BB84 File Offset: 0x00019D84
		public static Quaternion operator -(Quaternion value1, Quaternion value2)
		{
			Quaternion quaternion;
			quaternion.X = value1.X - value2.X;
			quaternion.Y = value1.Y - value2.Y;
			quaternion.Z = value1.Z - value2.Z;
			quaternion.W = value1.W - value2.W;
			return quaternion;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0001BBE4 File Offset: 0x00019DE4
		public static Quaternion operator *(Quaternion value1, Quaternion value2)
		{
			float x = value1.X;
			float y = value1.Y;
			float z = value1.Z;
			float w = value1.W;
			float x2 = value2.X;
			float y2 = value2.Y;
			float z2 = value2.Z;
			float w2 = value2.W;
			float num = y * z2 - z * y2;
			float num2 = z * x2 - x * z2;
			float num3 = x * y2 - y * x2;
			float num4 = x * x2 + y * y2 + z * z2;
			Quaternion quaternion;
			quaternion.X = x * w2 + x2 * w + num;
			quaternion.Y = y * w2 + y2 * w + num2;
			quaternion.Z = z * w2 + z2 * w + num3;
			quaternion.W = w * w2 - num4;
			return quaternion;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0001BCAC File Offset: 0x00019EAC
		public static Quaternion operator *(Quaternion value1, float value2)
		{
			Quaternion quaternion;
			quaternion.X = value1.X * value2;
			quaternion.Y = value1.Y * value2;
			quaternion.Z = value1.Z * value2;
			quaternion.W = value1.W * value2;
			return quaternion;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0001BCF8 File Offset: 0x00019EF8
		public static Quaternion operator /(Quaternion value1, Quaternion value2)
		{
			float x = value1.X;
			float y = value1.Y;
			float z = value1.Z;
			float w = value1.W;
			float num = value2.X * value2.X + value2.Y * value2.Y + value2.Z * value2.Z + value2.W * value2.W;
			float num2 = 1f / num;
			float num3 = -value2.X * num2;
			float num4 = -value2.Y * num2;
			float num5 = -value2.Z * num2;
			float num6 = value2.W * num2;
			float num7 = y * num5 - z * num4;
			float num8 = z * num3 - x * num5;
			float num9 = x * num4 - y * num3;
			float num10 = x * num3 + y * num4 + z * num5;
			Quaternion quaternion;
			quaternion.X = x * num6 + num3 * w + num7;
			quaternion.Y = y * num6 + num4 * w + num8;
			quaternion.Z = z * num6 + num5 * w + num9;
			quaternion.W = w * num6 - num10;
			return quaternion;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0001BE11 File Offset: 0x0001A011
		public static bool operator ==(Quaternion value1, Quaternion value2)
		{
			return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z && value1.W == value2.W;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0001BE4D File Offset: 0x0001A04D
		public static bool operator !=(Quaternion value1, Quaternion value2)
		{
			return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z || value1.W != value2.W;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0001BE11 File Offset: 0x0001A011
		public bool Equals(Quaternion other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0001BE8C File Offset: 0x0001A08C
		public override bool Equals(object obj)
		{
			return obj is Quaternion && this.Equals((Quaternion)obj);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0001BEA4 File Offset: 0x0001A0A4
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1} Z:{2} W:{3}}}", new object[]
			{
				this.X.ToString(currentCulture),
				this.Y.ToString(currentCulture),
				this.Z.ToString(currentCulture),
				this.W.ToString(currentCulture)
			});
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0001BF04 File Offset: 0x0001A104
		public override int GetHashCode()
		{
			return this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode() + this.W.GetHashCode();
		}

		// Token: 0x04000065 RID: 101
		public float X;

		// Token: 0x04000066 RID: 102
		public float Y;

		// Token: 0x04000067 RID: 103
		public float Z;

		// Token: 0x04000068 RID: 104
		public float W;
	}
}
