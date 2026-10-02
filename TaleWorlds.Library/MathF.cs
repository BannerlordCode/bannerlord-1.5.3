using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200006A RID: 106
	public static class MathF
	{
		// Token: 0x0600035A RID: 858 RVA: 0x0000C35A File Offset: 0x0000A55A
		public static float Sqrt(float x)
		{
			return (float)Math.Sqrt((double)x);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000C364 File Offset: 0x0000A564
		public static float Sin(float x)
		{
			return (float)Math.Sin((double)x);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000C36E File Offset: 0x0000A56E
		public static float Asin(float x)
		{
			return (float)Math.Asin((double)x);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000C378 File Offset: 0x0000A578
		public static float Cos(float x)
		{
			return (float)Math.Cos((double)x);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000C382 File Offset: 0x0000A582
		public static float Acos(float x)
		{
			return (float)Math.Acos((double)x);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000C38C File Offset: 0x0000A58C
		public static float Tan(float x)
		{
			return (float)Math.Tan((double)x);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000C396 File Offset: 0x0000A596
		public static float Tanh(float x)
		{
			return (float)Math.Tanh((double)x);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000C3A0 File Offset: 0x0000A5A0
		public static float Atan(float x)
		{
			return (float)Math.Atan((double)x);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000C3AA File Offset: 0x0000A5AA
		public static float Atan2(float y, float x)
		{
			return (float)Math.Atan2((double)y, (double)x);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000C3B6 File Offset: 0x0000A5B6
		public static double Pow(double x, double y)
		{
			return Math.Pow(x, y);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000C3BF File Offset: 0x0000A5BF
		[Obsolete("Types must match!", true)]
		public static double Pow(float x, double y)
		{
			return Math.Pow((double)x, y);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000C3C9 File Offset: 0x0000A5C9
		[Obsolete("Types must match!", true)]
		public static double Pow(double x, float y)
		{
			return Math.Pow(x, (double)y);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000C3D3 File Offset: 0x0000A5D3
		public static float Pow(float x, float y)
		{
			return (float)Math.Pow((double)x, (double)y);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000C3DF File Offset: 0x0000A5DF
		public static int PowTwo32(int x)
		{
			return 1 << x;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000C3E7 File Offset: 0x0000A5E7
		public static ulong PowTwo64(int x)
		{
			return 1UL << x;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000C3F0 File Offset: 0x0000A5F0
		public static bool IsValidValue(float f)
		{
			return !float.IsNaN(f) && !float.IsInfinity(f);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000C405 File Offset: 0x0000A605
		public static float Clamp(float value, float minValue, float maxValue)
		{
			if (value < minValue)
			{
				return minValue;
			}
			if (value > maxValue)
			{
				return maxValue;
			}
			return value;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000C414 File Offset: 0x0000A614
		public static float AngleClamp(float angle)
		{
			while (angle < 0f)
			{
				angle += 6.2831855f;
			}
			while (angle > 6.2831855f)
			{
				angle -= 6.2831855f;
			}
			return angle;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000C43D File Offset: 0x0000A63D
		public static float Lerp(float valueFrom, float valueTo, float amount, float minimumDifference = 1E-05f)
		{
			if (Math.Abs(valueFrom - valueTo) <= minimumDifference)
			{
				return valueTo;
			}
			return valueFrom + (valueTo - valueFrom) * amount;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000C453 File Offset: 0x0000A653
		[Obsolete("Blend amount must be float")]
		public static float Lerp(float valueFrom, float valueTo, int amount, float minimumDifference = 1E-05f)
		{
			return 0f;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000C45C File Offset: 0x0000A65C
		public static float AngleLerp(float angleFrom, float angleTo, float amount, float minimumDifference = 1E-05f)
		{
			float num = (angleTo - angleFrom) % 6.2831855f;
			float num2 = 2f * num % 6.2831855f - num;
			return MathF.AngleClamp(angleFrom + num2 * amount);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000C48D File Offset: 0x0000A68D
		public static int Round(double f)
		{
			return (int)Math.Round(f);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000C496 File Offset: 0x0000A696
		public static int Round(float f)
		{
			return (int)Math.Round((double)f);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000C4A0 File Offset: 0x0000A6A0
		public static float Round(float f, int digits)
		{
			return (float)Math.Round((double)f, digits);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000C4AB File Offset: 0x0000A6AB
		[Obsolete("Type is already int!", true)]
		public static int Round(int f)
		{
			return (int)Math.Round((double)((float)f));
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000C4B6 File Offset: 0x0000A6B6
		public static int Floor(double f)
		{
			return (int)Math.Floor(f);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000C4BF File Offset: 0x0000A6BF
		public static int Floor(float f)
		{
			return (int)Math.Floor((double)f);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000C4C9 File Offset: 0x0000A6C9
		[Obsolete("Type is already int!", true)]
		public static int Floor(int f)
		{
			return (int)Math.Floor((double)((float)f));
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000C4D4 File Offset: 0x0000A6D4
		public static int Ceiling(double f)
		{
			return (int)Math.Ceiling(f);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000C4DD File Offset: 0x0000A6DD
		public static int Ceiling(float f)
		{
			return (int)Math.Ceiling((double)f);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000C4E7 File Offset: 0x0000A6E7
		[Obsolete("Type is already int!", true)]
		public static int Ceiling(int f)
		{
			return (int)Math.Ceiling((double)((float)f));
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000C4F2 File Offset: 0x0000A6F2
		public static double Abs(double f)
		{
			if (f < 0.0)
			{
				return -f;
			}
			return f;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000C504 File Offset: 0x0000A704
		public static float Abs(float f)
		{
			if (f < 0f)
			{
				return -f;
			}
			return f;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000C512 File Offset: 0x0000A712
		public static int Abs(int f)
		{
			if ((float)f < 0f)
			{
				return -f;
			}
			return f;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000C521 File Offset: 0x0000A721
		public static double Max(double a, double b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000C52A File Offset: 0x0000A72A
		public static float Max(float a, float b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000C533 File Offset: 0x0000A733
		public static ValueTuple<float, float> MinMax(float a, float b)
		{
			if (a < b)
			{
				return new ValueTuple<float, float>(a, b);
			}
			return new ValueTuple<float, float>(b, a);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000C548 File Offset: 0x0000A748
		[Obsolete("Types must match!", true)]
		public static float Max(float a, int b)
		{
			if (a <= (float)b)
			{
				return (float)b;
			}
			return a;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000C553 File Offset: 0x0000A753
		[Obsolete("Types must match!", true)]
		public static float Max(int a, float b)
		{
			if ((float)a <= b)
			{
				return b;
			}
			return (float)a;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000C55E File Offset: 0x0000A75E
		public static int Max(int a, int b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000C567 File Offset: 0x0000A767
		public static long Max(long a, long b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000C570 File Offset: 0x0000A770
		public static uint Max(uint a, uint b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000C579 File Offset: 0x0000A779
		public static float Max(float a, float b, float c)
		{
			return Math.Max(a, Math.Max(b, c));
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000C588 File Offset: 0x0000A788
		public static double Min(double a, double b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000C591 File Offset: 0x0000A791
		public static float Min(float a, float b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000C59A File Offset: 0x0000A79A
		public static short Min(short a, short b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000C5A3 File Offset: 0x0000A7A3
		public static int Min(int a, int b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000C5AC File Offset: 0x0000A7AC
		public static long Min(long a, long b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000C5B5 File Offset: 0x0000A7B5
		public static uint Min(uint a, uint b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000C5BE File Offset: 0x0000A7BE
		[Obsolete("Types must match!", true)]
		public static int Min(int a, float b)
		{
			if ((float)a >= b)
			{
				return (int)b;
			}
			return a;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000C5C9 File Offset: 0x0000A7C9
		[Obsolete("Types must match!", true)]
		public static int Min(float a, int b)
		{
			if (a >= (float)b)
			{
				return b;
			}
			return (int)a;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000C5D4 File Offset: 0x0000A7D4
		public static float Min(float a, float b, float c)
		{
			return Math.Min(a, Math.Min(b, c));
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
		public static float PingPong(float min, float max, float time)
		{
			int num = (int)(min * 100f);
			int num2 = (int)(max * 100f);
			int num3 = (int)(time * 100f);
			int num4 = num2 - num;
			bool flag = num3 / num4 % 2 == 0;
			int num5 = num3 % num4;
			return (float)(flag ? (num5 + num) : (num2 - num5)) / 100f;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000C630 File Offset: 0x0000A830
		public static int GreatestCommonDivisor(int a, int b)
		{
			while (b != 0)
			{
				int num = a % b;
				a = b;
				b = num;
			}
			return a;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000C640 File Offset: 0x0000A840
		public static float Log(float a)
		{
			return (float)Math.Log((double)a);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000C64A File Offset: 0x0000A84A
		public static float Log(float a, float newBase)
		{
			return (float)Math.Log((double)a, (double)newBase);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000C656 File Offset: 0x0000A856
		public static int Sign(float f)
		{
			return Math.Sign(f);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000C65E File Offset: 0x0000A85E
		public static int Sign(int f)
		{
			return Math.Sign(f);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000C666 File Offset: 0x0000A866
		public static void SinCos(float a, out float sa, out float ca)
		{
			sa = MathF.Sin(a);
			ca = MathF.Cos(a);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000C678 File Offset: 0x0000A878
		public static float Log10(float val)
		{
			return (float)Math.Log10((double)val);
		}

		// Token: 0x04000134 RID: 308
		public const float DegToRad = 0.017453292f;

		// Token: 0x04000135 RID: 309
		public const float RadToDeg = 57.29578f;

		// Token: 0x04000136 RID: 310
		public const float TwoPI = 6.2831855f;

		// Token: 0x04000137 RID: 311
		public const float PI = 3.1415927f;

		// Token: 0x04000138 RID: 312
		public const float HalfPI = 1.5707964f;

		// Token: 0x04000139 RID: 313
		public const float E = 2.7182817f;

		// Token: 0x0400013A RID: 314
		public const float Epsilon = 1E-05f;
	}
}
