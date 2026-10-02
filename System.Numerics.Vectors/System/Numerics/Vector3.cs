using System;
using System.Globalization;
using System.Numerics.Hashing;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Numerics
{
	// Token: 0x02000010 RID: 16
	public struct Vector3 : IEquatable<Vector3>, IFormattable
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0001C79C File Offset: 0x0001A99C
		public static Vector3 Zero
		{
			get
			{
				return default(Vector3);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0001C7B2 File Offset: 0x0001A9B2
		public static Vector3 One
		{
			get
			{
				return new Vector3(1f, 1f, 1f);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0001C7C8 File Offset: 0x0001A9C8
		public static Vector3 UnitX
		{
			get
			{
				return new Vector3(1f, 0f, 0f);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0001C7DE File Offset: 0x0001A9DE
		public static Vector3 UnitY
		{
			get
			{
				return new Vector3(0f, 1f, 0f);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0001C7F4 File Offset: 0x0001A9F4
		public static Vector3 UnitZ
		{
			get
			{
				return new Vector3(0f, 0f, 1f);
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0001C80C File Offset: 0x0001AA0C
		public override int GetHashCode()
		{
			int num = this.X.GetHashCode();
			num = HashHelpers.Combine(num, this.Y.GetHashCode());
			return HashHelpers.Combine(num, this.Z.GetHashCode());
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0001C84A File Offset: 0x0001AA4A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			return obj is Vector3 && this.Equals((Vector3)obj);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0001C862 File Offset: 0x0001AA62
		public override string ToString()
		{
			return this.ToString("G", CultureInfo.CurrentCulture);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0001C874 File Offset: 0x0001AA74
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0001C884 File Offset: 0x0001AA84
		public string ToString(string format, IFormatProvider formatProvider)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			stringBuilder.Append('<');
			stringBuilder.Append(((IFormattable)this.X).ToString(format, formatProvider));
			stringBuilder.Append(numberGroupSeparator);
			stringBuilder.Append(' ');
			stringBuilder.Append(((IFormattable)this.Y).ToString(format, formatProvider));
			stringBuilder.Append(numberGroupSeparator);
			stringBuilder.Append(' ');
			stringBuilder.Append(((IFormattable)this.Z).ToString(format, formatProvider));
			stringBuilder.Append('>');
			return stringBuilder.ToString();
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0001C928 File Offset: 0x0001AB28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Length()
		{
			if (Vector.IsHardwareAccelerated)
			{
				float num = Vector3.Dot(this, this);
				return MathF.Sqrt(num);
			}
			float num2 = this.X * this.X + this.Y * this.Y + this.Z * this.Z;
			return MathF.Sqrt(num2);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0001C988 File Offset: 0x0001AB88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float LengthSquared()
		{
			if (Vector.IsHardwareAccelerated)
			{
				return Vector3.Dot(this, this);
			}
			return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0001C9D8 File Offset: 0x0001ABD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(Vector3 value1, Vector3 value2)
		{
			if (Vector.IsHardwareAccelerated)
			{
				Vector3 vector = value1 - value2;
				float num = Vector3.Dot(vector, vector);
				return MathF.Sqrt(num);
			}
			float num2 = value1.X - value2.X;
			float num3 = value1.Y - value2.Y;
			float num4 = value1.Z - value2.Z;
			float num5 = num2 * num2 + num3 * num3 + num4 * num4;
			return MathF.Sqrt(num5);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0001CA44 File Offset: 0x0001AC44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistanceSquared(Vector3 value1, Vector3 value2)
		{
			if (Vector.IsHardwareAccelerated)
			{
				Vector3 vector = value1 - value2;
				return Vector3.Dot(vector, vector);
			}
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			return num * num + num2 * num2 + num3 * num3;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0001CAA0 File Offset: 0x0001ACA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Normalize(Vector3 value)
		{
			if (Vector.IsHardwareAccelerated)
			{
				float num = value.Length();
				return value / num;
			}
			float num2 = value.X * value.X + value.Y * value.Y + value.Z * value.Z;
			float num3 = MathF.Sqrt(num2);
			return new Vector3(value.X / num3, value.Y / num3, value.Z / num3);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0001CB14 File Offset: 0x0001AD14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
		{
			return new Vector3(vector1.Y * vector2.Z - vector1.Z * vector2.Y, vector1.Z * vector2.X - vector1.X * vector2.Z, vector1.X * vector2.Y - vector1.Y * vector2.X);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0001CB78 File Offset: 0x0001AD78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Reflect(Vector3 vector, Vector3 normal)
		{
			if (Vector.IsHardwareAccelerated)
			{
				float num = Vector3.Dot(vector, normal);
				Vector3 vector2 = normal * num * 2f;
				return vector - vector2;
			}
			float num2 = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
			float num3 = normal.X * num2 * 2f;
			float num4 = normal.Y * num2 * 2f;
			float num5 = normal.Z * num2 * 2f;
			return new Vector3(vector.X - num3, vector.Y - num4, vector.Z - num5);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0001CC28 File Offset: 0x0001AE28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
		{
			float num = value1.X;
			num = ((num > max.X) ? max.X : num);
			num = ((num < min.X) ? min.X : num);
			float num2 = value1.Y;
			num2 = ((num2 > max.Y) ? max.Y : num2);
			num2 = ((num2 < min.Y) ? min.Y : num2);
			float num3 = value1.Z;
			num3 = ((num3 > max.Z) ? max.Z : num3);
			num3 = ((num3 < min.Z) ? min.Z : num3);
			return new Vector3(num, num2, num3);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0001CCC4 File Offset: 0x0001AEC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
		{
			if (Vector.IsHardwareAccelerated)
			{
				Vector3 vector = value1 * (1f - amount);
				Vector3 vector2 = value2 * amount;
				return vector + vector2;
			}
			return new Vector3(value1.X + (value2.X - value1.X) * amount, value1.Y + (value2.Y - value1.Y) * amount, value1.Z + (value2.Z - value1.Z) * amount);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0001CD40 File Offset: 0x0001AF40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Transform(Vector3 position, Matrix4x4 matrix)
		{
			return new Vector3(position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41, position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42, position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0001CDE4 File Offset: 0x0001AFE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 TransformNormal(Vector3 normal, Matrix4x4 matrix)
		{
			return new Vector3(normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31, normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32, normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0001CE74 File Offset: 0x0001B074
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Transform(Vector3 value, Quaternion rotation)
		{
			float num = rotation.X + rotation.X;
			float num2 = rotation.Y + rotation.Y;
			float num3 = rotation.Z + rotation.Z;
			float num4 = rotation.W * num;
			float num5 = rotation.W * num2;
			float num6 = rotation.W * num3;
			float num7 = rotation.X * num;
			float num8 = rotation.X * num2;
			float num9 = rotation.X * num3;
			float num10 = rotation.Y * num2;
			float num11 = rotation.Y * num3;
			float num12 = rotation.Z * num3;
			return new Vector3(value.X * (1f - num10 - num12) + value.Y * (num8 - num6) + value.Z * (num9 + num5), value.X * (num8 + num6) + value.Y * (1f - num7 - num12) + value.Z * (num11 - num4), value.X * (num9 - num5) + value.Y * (num11 + num4) + value.Z * (1f - num7 - num10));
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0001CF8B File Offset: 0x0001B18B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Add(Vector3 left, Vector3 right)
		{
			return left + right;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0001CF94 File Offset: 0x0001B194
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Subtract(Vector3 left, Vector3 right)
		{
			return left - right;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0001CF9D File Offset: 0x0001B19D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Multiply(Vector3 left, Vector3 right)
		{
			return left * right;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0001CFA6 File Offset: 0x0001B1A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Multiply(Vector3 left, float right)
		{
			return left * right;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0001CFAF File Offset: 0x0001B1AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Multiply(float left, Vector3 right)
		{
			return left * right;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0001CFB8 File Offset: 0x0001B1B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Divide(Vector3 left, Vector3 right)
		{
			return left / right;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0001CFC1 File Offset: 0x0001B1C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Divide(Vector3 left, float divisor)
		{
			return left / divisor;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0001CFCA File Offset: 0x0001B1CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Negate(Vector3 value)
		{
			return -value;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0001CFD2 File Offset: 0x0001B1D2
		[JitIntrinsic]
		public Vector3(float value)
		{
			this = new Vector3(value, value, value);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0001CFDD File Offset: 0x0001B1DD
		public Vector3(Vector2 value, float z)
		{
			this = new Vector3(value.X, value.Y, z);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0001CFF2 File Offset: 0x0001B1F2
		[JitIntrinsic]
		public Vector3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0001D009 File Offset: 0x0001B209
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTo(float[] array)
		{
			this.CopyTo(array, 0);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0001D014 File Offset: 0x0001B214
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTo(float[] array, int index)
		{
			if (array == null)
			{
				throw new NullReferenceException(SR.Arg_NullArgumentNullRef);
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SR.Format(SR.Arg_ArgumentOutOfRangeException, index));
			}
			if (array.Length - index < 3)
			{
				throw new ArgumentException(SR.Format(SR.Arg_ElementsInSourceIsGreaterThanDestination, index));
			}
			array[index] = this.X;
			array[index + 1] = this.Y;
			array[index + 2] = this.Z;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0001D091 File Offset: 0x0001B291
		[JitIntrinsic]
		public bool Equals(Vector3 other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0001D0BF File Offset: 0x0001B2BF
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(Vector3 vector1, Vector3 vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0001D0EC File Offset: 0x0001B2EC
		[JitIntrinsic]
		public static Vector3 Min(Vector3 value1, Vector3 value2)
		{
			return new Vector3((value1.X < value2.X) ? value1.X : value2.X, (value1.Y < value2.Y) ? value1.Y : value2.Y, (value1.Z < value2.Z) ? value1.Z : value2.Z);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0001D154 File Offset: 0x0001B354
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Max(Vector3 value1, Vector3 value2)
		{
			return new Vector3((value1.X > value2.X) ? value1.X : value2.X, (value1.Y > value2.Y) ? value1.Y : value2.Y, (value1.Z > value2.Z) ? value1.Z : value2.Z);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0001D1BA File Offset: 0x0001B3BA
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Abs(Vector3 value)
		{
			return new Vector3(MathF.Abs(value.X), MathF.Abs(value.Y), MathF.Abs(value.Z));
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0001D1E2 File Offset: 0x0001B3E2
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 SquareRoot(Vector3 value)
		{
			return new Vector3(MathF.Sqrt(value.X), MathF.Sqrt(value.Y), MathF.Sqrt(value.Z));
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0001D20A File Offset: 0x0001B40A
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator +(Vector3 left, Vector3 right)
		{
			return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0001D238 File Offset: 0x0001B438
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator -(Vector3 left, Vector3 right)
		{
			return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0001D266 File Offset: 0x0001B466
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator *(Vector3 left, Vector3 right)
		{
			return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0001D294 File Offset: 0x0001B494
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator *(Vector3 left, float right)
		{
			return left * new Vector3(right);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0001D2A2 File Offset: 0x0001B4A2
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator *(float left, Vector3 right)
		{
			return new Vector3(left) * right;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0001D2B0 File Offset: 0x0001B4B0
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator /(Vector3 left, Vector3 right)
		{
			return new Vector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0001D2E0 File Offset: 0x0001B4E0
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator /(Vector3 value1, float value2)
		{
			float num = 1f / value2;
			return new Vector3(value1.X * num, value1.Y * num, value1.Z * num);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0001D312 File Offset: 0x0001B512
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator -(Vector3 value)
		{
			return Vector3.Zero - value;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0001D091 File Offset: 0x0001B291
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3 left, Vector3 right)
		{
			return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0001D31F File Offset: 0x0001B51F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3 left, Vector3 right)
		{
			return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
		}

		// Token: 0x0400006B RID: 107
		public float X;

		// Token: 0x0400006C RID: 108
		public float Y;

		// Token: 0x0400006D RID: 109
		public float Z;
	}
}
