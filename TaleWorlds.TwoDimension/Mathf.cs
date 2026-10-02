using System;
using System.Numerics;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000028 RID: 40
	public static class Mathf
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x00007701 File Offset: 0x00005901
		public static float Sqrt(float f)
		{
			return (float)Math.Sqrt((double)f);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000770B File Offset: 0x0000590B
		public static float Abs(float f)
		{
			return Math.Abs(f);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00007714 File Offset: 0x00005914
		public static float Floor(float f)
		{
			return (float)Math.Floor((double)f);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000771E File Offset: 0x0000591E
		public static float Cos(float radian)
		{
			return (float)Math.Cos((double)radian);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00007728 File Offset: 0x00005928
		public static float Sin(float radian)
		{
			return (float)Math.Sin((double)radian);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00007732 File Offset: 0x00005932
		public static float Acos(float f)
		{
			return (float)Math.Acos((double)f);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000773C File Offset: 0x0000593C
		public static float Atan2(float y, float x)
		{
			return (float)Math.Atan2((double)y, (double)x);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00007748 File Offset: 0x00005948
		public static float Clamp(float value, float min, float max)
		{
			if (value > max)
			{
				return max;
			}
			if (value >= min)
			{
				return value;
			}
			return min;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00007757 File Offset: 0x00005957
		public static int Clamp(int value, int min, int max)
		{
			if (value > max)
			{
				return max;
			}
			if (value >= min)
			{
				return value;
			}
			return min;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00007766 File Offset: 0x00005966
		public static float Min(float a, float b)
		{
			if (a <= b)
			{
				return a;
			}
			return b;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000776F File Offset: 0x0000596F
		public static float Max(float a, float b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00007778 File Offset: 0x00005978
		public static bool IsZero(float f)
		{
			return f < 1E-05f && f > -1E-05f;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000778C File Offset: 0x0000598C
		public static bool IsZero(Vector2 vector2)
		{
			return Mathf.IsZero(vector2.X) && Mathf.IsZero(vector2.Y);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000077A8 File Offset: 0x000059A8
		public static float Sign(float f)
		{
			return (float)Math.Sign(f);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000077B1 File Offset: 0x000059B1
		public static float Ceil(float f)
		{
			return (float)Math.Ceiling((double)f);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000077BB File Offset: 0x000059BB
		public static float Round(float f)
		{
			return (float)Math.Round((double)f);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000077C5 File Offset: 0x000059C5
		public static float Lerp(float start, float end, float amount)
		{
			return (end - start) * amount + start;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000077D0 File Offset: 0x000059D0
		private static float PingPong(float min, float max, float time)
		{
			int num = (int)(min * 100f);
			int num2 = (int)(max * 100f);
			int num3 = (int)(time * 100f);
			int num4 = num2 - num;
			bool flag = num3 / num4 % 2 == 0;
			int num5 = num3 % num4;
			return (float)(flag ? (num5 + num) : (num2 - num5)) / 100f;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000781C File Offset: 0x00005A1C
		public static Vec3 GetClosestPointInLineSegmentToLine(Vec3 linePosition, Vec3 lineDirection, Vec3 lineSegmentBegin, Vec3 lineSegmentEnd)
		{
			Vec3 vec = lineSegmentEnd - lineSegmentBegin;
			Vec3 vec2 = linePosition - lineSegmentBegin;
			if (!vec.IsNonZero)
			{
				return lineSegmentBegin;
			}
			float num = Vec3.DotProduct(lineDirection, lineDirection);
			float num2 = Vec3.DotProduct(lineDirection, vec);
			float num3 = Vec3.DotProduct(vec, vec);
			float num4 = Vec3.DotProduct(lineDirection, vec2);
			float num5 = Vec3.DotProduct(vec, vec2);
			float num6 = num * num3 - num2 * num2;
			float num7;
			if (num6.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				num7 = Vec3.DotProduct(vec, linePosition - lineSegmentBegin) / Vec3.DotProduct(vec, vec);
			}
			else
			{
				num7 = (num * num5 - num2 * num4) / num6;
			}
			num7 = MathF.Clamp(num7, 0f, 1f);
			return lineSegmentBegin + num7 * vec;
		}

		// Token: 0x040000E0 RID: 224
		public const float PI = 3.1415927f;

		// Token: 0x040000E1 RID: 225
		public const float Deg2Rad = 0.017453292f;

		// Token: 0x040000E2 RID: 226
		public const float Rad2Deg = 57.295776f;

		// Token: 0x040000E3 RID: 227
		public const float Epsilon = 1E-05f;
	}
}
