using System;
using System.Reflection;

namespace TaleWorlds.Library
{
	// Token: 0x0200002E RID: 46
	internal static class EnumHelper<T1>
	{
		// Token: 0x0600018E RID: 398 RVA: 0x00006AFD File Offset: 0x00004CFD
		public static bool Overlaps(sbyte p1, sbyte p2)
		{
			return (p1 & p2) != 0;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006B05 File Offset: 0x00004D05
		public static bool Overlaps(byte p1, byte p2)
		{
			return (p1 & p2) > 0;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006B0D File Offset: 0x00004D0D
		public static bool Overlaps(short p1, short p2)
		{
			return (p1 & p2) != 0;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006B15 File Offset: 0x00004D15
		public static bool Overlaps(ushort p1, ushort p2)
		{
			return (p1 & p2) > 0;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006B1D File Offset: 0x00004D1D
		public static bool Overlaps(int p1, int p2)
		{
			return (p1 & p2) != 0;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006B25 File Offset: 0x00004D25
		public static bool Overlaps(uint p1, uint p2)
		{
			return (p1 & p2) > 0U;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006B2D File Offset: 0x00004D2D
		public static bool Overlaps(long p1, long p2)
		{
			return (p1 & p2) != 0L;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006B36 File Offset: 0x00004D36
		public static bool Overlaps(ulong p1, ulong p2)
		{
			return (p1 & p2) > 0UL;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006B3F File Offset: 0x00004D3F
		public static bool ContainsAll(sbyte p1, sbyte p2)
		{
			return (p1 & p2) == p2;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006B47 File Offset: 0x00004D47
		public static bool ContainsAll(byte p1, byte p2)
		{
			return (p1 & p2) == p2;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00006B4F File Offset: 0x00004D4F
		public static bool ContainsAll(short p1, short p2)
		{
			return (p1 & p2) == p2;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006B57 File Offset: 0x00004D57
		public static bool ContainsAll(ushort p1, ushort p2)
		{
			return (p1 & p2) == p2;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00006B5F File Offset: 0x00004D5F
		public static bool ContainsAll(int p1, int p2)
		{
			return (p1 & p2) == p2;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006B67 File Offset: 0x00004D67
		public static bool ContainsAll(uint p1, uint p2)
		{
			return (p1 & p2) == p2;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00006B6F File Offset: 0x00004D6F
		public static bool ContainsAll(long p1, long p2)
		{
			return (p1 & p2) == p2;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006B77 File Offset: 0x00004D77
		public static bool ContainsAll(ulong p1, ulong p2)
		{
			return (p1 & p2) == p2;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00006B80 File Offset: 0x00004D80
		public static bool initProc(T1 p1, T1 p2)
		{
			Type type = typeof(T1);
			if (type.IsEnum)
			{
				type = Enum.GetUnderlyingType(type);
			}
			Type[] array = new Type[] { type, type };
			MethodInfo methodInfo = typeof(EnumHelper<T1>).GetMethod("Overlaps", array);
			if (methodInfo == null)
			{
				methodInfo = typeof(T1).GetMethod("Overlaps", array);
			}
			if (methodInfo == null)
			{
				throw new MissingMethodException("Unknown type of enum");
			}
			EnumHelper<T1>.HasAnyFlag = (Func<T1, T1, bool>)Delegate.CreateDelegate(typeof(Func<T1, T1, bool>), methodInfo);
			return EnumHelper<T1>.HasAnyFlag(p1, p2);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006C28 File Offset: 0x00004E28
		public static bool initAllProc(T1 p1, T1 p2)
		{
			Type type = typeof(T1);
			if (type.IsEnum)
			{
				type = Enum.GetUnderlyingType(type);
			}
			Type[] array = new Type[] { type, type };
			MethodInfo methodInfo = typeof(EnumHelper<T1>).GetMethod("ContainsAll", array);
			if (methodInfo == null)
			{
				methodInfo = typeof(T1).GetMethod("ContainsAll", array);
			}
			if (methodInfo == null)
			{
				throw new MissingMethodException("Unknown type of enum");
			}
			EnumHelper<T1>.HasAllFlags = (Func<T1, T1, bool>)Delegate.CreateDelegate(typeof(Func<T1, T1, bool>), methodInfo);
			return EnumHelper<T1>.HasAllFlags(p1, p2);
		}

		// Token: 0x040000AA RID: 170
		public static Func<T1, T1, bool> HasAnyFlag = new Func<T1, T1, bool>(EnumHelper<T1>.initProc);

		// Token: 0x040000AB RID: 171
		public static Func<T1, T1, bool> HasAllFlags = new Func<T1, T1, bool>(EnumHelper<T1>.initAllProc);
	}
}
