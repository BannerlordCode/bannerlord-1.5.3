using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000003 RID: 3
	internal static class MathF
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Abs(float x)
		{
			return Math.Abs(x);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Acos(float x)
		{
			return (float)Math.Acos((double)x);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002062 File Offset: 0x00000262
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Cos(float x)
		{
			return (float)Math.Cos((double)x);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000206C File Offset: 0x0000026C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float IEEERemainder(float x, float y)
		{
			return (float)Math.IEEERemainder((double)x, (double)y);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002078 File Offset: 0x00000278
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sin(float x)
		{
			return (float)Math.Sin((double)x);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002082 File Offset: 0x00000282
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sqrt(float x)
		{
			return (float)Math.Sqrt((double)x);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000208C File Offset: 0x0000028C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Tan(float x)
		{
			return (float)Math.Tan((double)x);
		}

		// Token: 0x04000001 RID: 1
		public const float PI = 3.1415927f;
	}
}
