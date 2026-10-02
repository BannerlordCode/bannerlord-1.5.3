using System;
using System.Globalization;
using System.Numerics.Hashing;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Numerics
{
	// Token: 0x0200000F RID: 15
	public struct Vector2 : IEquatable<Vector2>, IFormattable
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0001BF38 File Offset: 0x0001A138
		public static Vector2 Zero
		{
			get
			{
				return default(Vector2);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0001BF4E File Offset: 0x0001A14E
		public static Vector2 One
		{
			get
			{
				return new Vector2(1f, 1f);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0001BF5F File Offset: 0x0001A15F
		public static Vector2 UnitX
		{
			get
			{
				return new Vector2(1f, 0f);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0001BF70 File Offset: 0x0001A170
		public static Vector2 UnitY
		{
			get
			{
				return new Vector2(0f, 1f);
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0001BF84 File Offset: 0x0001A184
		public override int GetHashCode()
		{
			int hashCode = this.X.GetHashCode();
			return HashHelpers.Combine(hashCode, this.Y.GetHashCode());
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0001BFB0 File Offset: 0x0001A1B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			return obj is Vector2 && this.Equals((Vector2)obj);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0001BFC8 File Offset: 0x0001A1C8
		public override string ToString()
		{
			return this.ToString("G", CultureInfo.CurrentCulture);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0001BFDA File Offset: 0x0001A1DA
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0001BFE8 File Offset: 0x0001A1E8
		public string ToString(string format, IFormatProvider formatProvider)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			stringBuilder.Append('<');
			stringBuilder.Append(this.X.ToString(format, formatProvider));
			stringBuilder.Append(numberGroupSeparator);
			stringBuilder.Append(' ');
			stringBuilder.Append(this.Y.ToString(format, formatProvider));
			stringBuilder.Append('>');
			return stringBuilder.ToString();
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0001C058 File Offset: 0x0001A258
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Length()
		{
			if (Vector.IsHardwareAccelerated)
			{
				float num = Vector2.Dot(this, this);
				return MathF.Sqrt(num);
			}
			float num2 = this.X * this.X + this.Y * this.Y;
			return MathF.Sqrt(num2);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0001C0A7 File Offset: 0x0001A2A7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float LengthSquared()
		{
			if (Vector.IsHardwareAccelerated)
			{
				return Vector2.Dot(this, this);
			}
			return this.X * this.X + this.Y * this.Y;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0001C0E0 File Offset: 0x0001A2E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(Vector2 value1, Vector2 value2)
		{
			if (Vector.IsHardwareAccelerated)
			{
				Vector2 vector = value1 - value2;
				float num = Vector2.Dot(vector, vector);
				return MathF.Sqrt(num);
			}
			float num2 = value1.X - value2.X;
			float num3 = value1.Y - value2.Y;
			float num4 = num2 * num2 + num3 * num3;
			return MathF.Sqrt(num4);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0001C138 File Offset: 0x0001A338
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistanceSquared(Vector2 value1, Vector2 value2)
		{
			if (Vector.IsHardwareAccelerated)
			{
				Vector2 vector = value1 - value2;
				return Vector2.Dot(vector, vector);
			}
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			return num * num + num2 * num2;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0001C180 File Offset: 0x0001A380
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Normalize(Vector2 value)
		{
			if (Vector.IsHardwareAccelerated)
			{
				float num = value.Length();
				return value / num;
			}
			float num2 = value.X * value.X + value.Y * value.Y;
			float num3 = 1f / MathF.Sqrt(num2);
			return new Vector2(value.X * num3, value.Y * num3);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0001C1E4 File Offset: 0x0001A3E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Reflect(Vector2 vector, Vector2 normal)
		{
			if (Vector.IsHardwareAccelerated)
			{
				float num = Vector2.Dot(vector, normal);
				return vector - 2f * num * normal;
			}
			float num2 = vector.X * normal.X + vector.Y * normal.Y;
			return new Vector2(vector.X - 2f * num2 * normal.X, vector.Y - 2f * num2 * normal.Y);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0001C260 File Offset: 0x0001A460
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
		{
			float num = value1.X;
			num = ((num > max.X) ? max.X : num);
			num = ((num < min.X) ? min.X : num);
			float num2 = value1.Y;
			num2 = ((num2 > max.Y) ? max.Y : num2);
			num2 = ((num2 < min.Y) ? min.Y : num2);
			return new Vector2(num, num2);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0001C2CE File Offset: 0x0001A4CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
		{
			return new Vector2(value1.X + (value2.X - value1.X) * amount, value1.Y + (value2.Y - value1.Y) * amount);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0001C304 File Offset: 0x0001A504
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Transform(Vector2 position, Matrix3x2 matrix)
		{
			return new Vector2(position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M31, position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M32);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0001C35C File Offset: 0x0001A55C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Transform(Vector2 position, Matrix4x4 matrix)
		{
			return new Vector2(position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41, position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0001C3B2 File Offset: 0x0001A5B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 TransformNormal(Vector2 normal, Matrix3x2 matrix)
		{
			return new Vector2(normal.X * matrix.M11 + normal.Y * matrix.M21, normal.X * matrix.M12 + normal.Y * matrix.M22);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0001C3EF File Offset: 0x0001A5EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 TransformNormal(Vector2 normal, Matrix4x4 matrix)
		{
			return new Vector2(normal.X * matrix.M11 + normal.Y * matrix.M21, normal.X * matrix.M12 + normal.Y * matrix.M22);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0001C42C File Offset: 0x0001A62C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Transform(Vector2 value, Quaternion rotation)
		{
			float num = rotation.X + rotation.X;
			float num2 = rotation.Y + rotation.Y;
			float num3 = rotation.Z + rotation.Z;
			float num4 = rotation.W * num3;
			float num5 = rotation.X * num;
			float num6 = rotation.X * num2;
			float num7 = rotation.Y * num2;
			float num8 = rotation.Z * num3;
			return new Vector2(value.X * (1f - num7 - num8) + value.Y * (num6 - num4), value.X * (num6 + num4) + value.Y * (1f - num5 - num8));
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0001C4D5 File Offset: 0x0001A6D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Add(Vector2 left, Vector2 right)
		{
			return left + right;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0001C4DE File Offset: 0x0001A6DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Subtract(Vector2 left, Vector2 right)
		{
			return left - right;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0001C4E7 File Offset: 0x0001A6E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Multiply(Vector2 left, Vector2 right)
		{
			return left * right;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0001C4F0 File Offset: 0x0001A6F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Multiply(Vector2 left, float right)
		{
			return left * right;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0001C4F9 File Offset: 0x0001A6F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Multiply(float left, Vector2 right)
		{
			return left * right;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0001C502 File Offset: 0x0001A702
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Divide(Vector2 left, Vector2 right)
		{
			return left / right;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0001C50B File Offset: 0x0001A70B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Divide(Vector2 left, float divisor)
		{
			return left / divisor;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0001C514 File Offset: 0x0001A714
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Negate(Vector2 value)
		{
			return -value;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0001C51C File Offset: 0x0001A71C
		[JitIntrinsic]
		public Vector2(float value)
		{
			this = new Vector2(value, value);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0001C526 File Offset: 0x0001A726
		[JitIntrinsic]
		public Vector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0001C536 File Offset: 0x0001A736
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTo(float[] array)
		{
			this.CopyTo(array, 0);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0001C540 File Offset: 0x0001A740
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
			if (array.Length - index < 2)
			{
				throw new ArgumentException(SR.Format(SR.Arg_ElementsInSourceIsGreaterThanDestination, index));
			}
			array[index] = this.X;
			array[index + 1] = this.Y;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0001C5B2 File Offset: 0x0001A7B2
		[JitIntrinsic]
		public bool Equals(Vector2 other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0001C5D2 File Offset: 0x0001A7D2
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(Vector2 value1, Vector2 value2)
		{
			return value1.X * value2.X + value1.Y * value2.Y;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0001C5EF File Offset: 0x0001A7EF
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Min(Vector2 value1, Vector2 value2)
		{
			return new Vector2((value1.X < value2.X) ? value1.X : value2.X, (value1.Y < value2.Y) ? value1.Y : value2.Y);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0001C62E File Offset: 0x0001A82E
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Max(Vector2 value1, Vector2 value2)
		{
			return new Vector2((value1.X > value2.X) ? value1.X : value2.X, (value1.Y > value2.Y) ? value1.Y : value2.Y);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0001C66D File Offset: 0x0001A86D
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Abs(Vector2 value)
		{
			return new Vector2(MathF.Abs(value.X), MathF.Abs(value.Y));
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0001C68A File Offset: 0x0001A88A
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 SquareRoot(Vector2 value)
		{
			return new Vector2(MathF.Sqrt(value.X), MathF.Sqrt(value.Y));
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0001C6A7 File Offset: 0x0001A8A7
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator +(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X + right.X, left.Y + right.Y);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0001C6C8 File Offset: 0x0001A8C8
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X - right.X, left.Y - right.Y);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0001C6E9 File Offset: 0x0001A8E9
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X * right.X, left.Y * right.Y);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0001C70A File Offset: 0x0001A90A
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(float left, Vector2 right)
		{
			return new Vector2(left, left) * right;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0001C719 File Offset: 0x0001A919
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 left, float right)
		{
			return left * new Vector2(right, right);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0001C728 File Offset: 0x0001A928
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X / right.X, left.Y / right.Y);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0001C74C File Offset: 0x0001A94C
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 value1, float value2)
		{
			float num = 1f / value2;
			return new Vector2(value1.X * num, value1.Y * num);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0001C776 File Offset: 0x0001A976
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 value)
		{
			return Vector2.Zero - value;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0001C783 File Offset: 0x0001A983
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2 left, Vector2 right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0001C78D File Offset: 0x0001A98D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2 left, Vector2 right)
		{
			return !(left == right);
		}

		// Token: 0x04000069 RID: 105
		public float X;

		// Token: 0x0400006A RID: 106
		public float Y;
	}
}
