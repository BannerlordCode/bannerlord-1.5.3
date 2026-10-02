using System;
using System.Collections.Generic;
using System.Linq;

namespace Jose
{
	// Token: 0x0200002F RID: 47
	public class Ensure
	{
		// Token: 0x060000CD RID: 205 RVA: 0x00005C08 File Offset: 0x00003E08
		public static void IsEmpty(byte[] arr, string msg, params object[] args)
		{
			if (arr.Length != 0)
			{
				throw new ArgumentException(msg);
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005C15 File Offset: 0x00003E15
		public static T Type<T>(object obj, string msg, params object[] args)
		{
			if (!(obj is T))
			{
				throw new ArgumentException(msg);
			}
			return (T)((object)obj);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005C2C File Offset: 0x00003E2C
		public static void IsNull(object key, string msg, params object[] args)
		{
			if (key != null)
			{
				throw new ArgumentException(msg);
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005C38 File Offset: 0x00003E38
		public static void BitSize(byte[] array, long expectedSize, string msg, params object[] args)
		{
			if (expectedSize != (long)array.Length * 8L)
			{
				throw new ArgumentException(string.Format(msg, args));
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005C51 File Offset: 0x00003E51
		public static void BitSize(int actualSize, int expectedSize, string msg)
		{
			if (expectedSize != actualSize)
			{
				throw new ArgumentException(msg);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005C5E File Offset: 0x00003E5E
		public static void IsNotEmpty(string arg, string msg, params object[] args)
		{
			if (string.IsNullOrWhiteSpace(arg))
			{
				throw new ArgumentException(msg);
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005C6F File Offset: 0x00003E6F
		public static void Divisible(int arg, int divisor, string msg, params object[] args)
		{
			if (arg % divisor != 0)
			{
				throw new ArgumentException(string.Format(msg, args));
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005C83 File Offset: 0x00003E83
		public static void MinValue(long arg, long min, string msg, params object[] args)
		{
			if (arg < min)
			{
				throw new ArgumentException(string.Format(msg, args));
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005C96 File Offset: 0x00003E96
		public static void MaxValue(int arg, long max, string msg, params object[] args)
		{
			if ((long)arg > max)
			{
				throw new ArgumentException(string.Format(msg, args));
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005CAA File Offset: 0x00003EAA
		public static void MinBitSize(byte[] arr, long minBitSize, string msg, params object[] args)
		{
			Ensure.MinValue((long)arr.Length * 8L, minBitSize, msg, args);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005CBC File Offset: 0x00003EBC
		public static void Contains(IDictionary<string, object> dict, string[] keys, string msg, params object[] args)
		{
			if (keys.Any<string>((string key) => !dict.ContainsKey(key)))
			{
				throw new ArgumentException(string.Format(msg, args));
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005CF7 File Offset: 0x00003EF7
		public static void SameSize(byte[] left, byte[] right, string msg, params object[] args)
		{
			if (left.Length != right.Length)
			{
				throw new ArgumentException(string.Format(msg, args));
			}
		}
	}
}
