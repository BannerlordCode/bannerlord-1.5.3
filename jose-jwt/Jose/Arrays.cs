using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Jose
{
	// Token: 0x0200002A RID: 42
	public class Arrays
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00005308 File Offset: 0x00003508
		public static byte[] FirstHalf(byte[] arr)
		{
			Ensure.Divisible(arr.Length, 2, "Arrays.FirstHalf() expects even number of element in array.", Array.Empty<object>());
			int num = arr.Length / 2;
			byte[] array = new byte[num];
			Buffer.BlockCopy(arr, 0, array, 0, num);
			return array;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005340 File Offset: 0x00003540
		public static byte[] SecondHalf(byte[] arr)
		{
			Ensure.Divisible(arr.Length, 2, "Arrays.SecondHalf() expects even number of element in array.", Array.Empty<object>());
			int num = arr.Length / 2;
			byte[] array = new byte[num];
			Buffer.BlockCopy(arr, num, array, 0, num);
			return array;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005378 File Offset: 0x00003578
		public static byte[] Concat(params byte[][] arrays)
		{
			byte[] array = new byte[arrays.Sum<byte[]>(delegate(byte[] a)
			{
				if (a != null)
				{
					return a.Length;
				}
				return 0;
			})];
			int num = 0;
			foreach (byte[] array2 in arrays)
			{
				if (array2 != null)
				{
					Buffer.BlockCopy(array2, 0, array, num, array2.Length);
					num += array2.Length;
				}
			}
			return array;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000053E4 File Offset: 0x000035E4
		public static byte[][] Slice(byte[] array, int count)
		{
			Ensure.MinValue((long)count, 1L, "Arrays.Slice() expects count to be above zero, but was {0}", new object[] { count });
			Ensure.Divisible(array.Length, count, "Arrays.Slice() expects array length to be divisible by {0}", new object[] { count });
			int num = array.Length / count;
			byte[][] array2 = new byte[num][];
			for (int i = 0; i < num; i++)
			{
				byte[] array3 = new byte[count];
				Buffer.BlockCopy(array, i * count, array3, 0, count);
				array2[i] = array3;
			}
			return array2;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000545C File Offset: 0x0000365C
		public static byte[] Xor(byte[] left, long right)
		{
			Ensure.BitSize(left, 64L, "Arrays.Xor(byte[], long) expects array size to be 8 bytes, but was {0}", new object[] { left.Length });
			return Arrays.LongToBytes(Arrays.BytesToLong(left) ^ right);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000548C File Offset: 0x0000368C
		public static byte[] Xor(byte[] left, byte[] right)
		{
			Ensure.SameSize(left, right, "Arrays.Xor(byte[], byte[]) expects both arrays to be same legnth, but was given {0} and {1}", new object[] { left.Length, right.Length });
			byte[] array = new byte[left.Length];
			for (int i = 0; i < left.Length; i++)
			{
				array[i] = left[i] ^ right[i];
			}
			return array;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000054E4 File Offset: 0x000036E4
		public static bool ConstantTimeEquals(byte[] expected, byte[] actual)
		{
			if (expected == actual)
			{
				return true;
			}
			if (expected == null || actual == null)
			{
				return false;
			}
			if (expected.Length != actual.Length)
			{
				return false;
			}
			byte b = 0;
			for (int i = 0; i < expected.Length; i++)
			{
				b |= expected[i] ^ actual[i];
			}
			return b == 0;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000552C File Offset: 0x0000372C
		public static string Dump(byte[] arr, string label = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("{0}({1} bytes): [", label + " ", arr.Length).Trim());
			foreach (byte b in arr)
			{
				stringBuilder.Append(b);
				stringBuilder.Append(",");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("] Hex:[").Append(BitConverter.ToString(arr).Replace("-", " "));
			stringBuilder.Append("] Base64Url:").Append(Base64Url.Encode(arr)).Append("\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000055F0 File Offset: 0x000037F0
		public static byte[] Random(int sizeBits = 128)
		{
			byte[] array = new byte[sizeBits / 8];
			Arrays.RNG.GetBytes(array);
			return array;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00005612 File Offset: 0x00003812
		internal static RandomNumberGenerator RNG
		{
			get
			{
				RandomNumberGenerator randomNumberGenerator;
				if ((randomNumberGenerator = Arrays.rng) == null)
				{
					randomNumberGenerator = (Arrays.rng = RandomNumberGenerator.Create());
				}
				return randomNumberGenerator;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005628 File Offset: 0x00003828
		public static byte[] IntToBytes(int value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				return new byte[]
				{
					(byte)(value & 255),
					(byte)(((uint)value >> 8) & 255U),
					(byte)(((uint)value >> 16) & 255U),
					(byte)(((uint)value >> 24) & 255U)
				};
			}
			return new byte[]
			{
				(byte)(((uint)value >> 24) & 255U),
				(byte)(((uint)value >> 16) & 255U),
				(byte)(((uint)value >> 8) & 255U),
				(byte)(value & 255)
			};
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000056B4 File Offset: 0x000038B4
		public static byte[] LongToBytes(long value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				return new byte[]
				{
					(byte)(value & 255L),
					(byte)(((ulong)value >> 8) & 255UL),
					(byte)(((ulong)value >> 16) & 255UL),
					(byte)(((ulong)value >> 24) & 255UL),
					(byte)(((ulong)value >> 32) & 255UL),
					(byte)(((ulong)value >> 40) & 255UL),
					(byte)(((ulong)value >> 48) & 255UL),
					(byte)(((ulong)value >> 56) & 255UL)
				};
			}
			return new byte[]
			{
				(byte)(((ulong)value >> 56) & 255UL),
				(byte)(((ulong)value >> 48) & 255UL),
				(byte)(((ulong)value >> 40) & 255UL),
				(byte)(((ulong)value >> 32) & 255UL),
				(byte)(((ulong)value >> 24) & 255UL),
				(byte)(((ulong)value >> 16) & 255UL),
				(byte)(((ulong)value >> 8) & 255UL),
				(byte)(value & 255L)
			};
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000057C0 File Offset: 0x000039C0
		public static long BytesToLong(byte[] array)
		{
			long num = (BitConverter.IsLittleEndian ? ((long)(((int)array[0] << 24) | ((int)array[1] << 16) | ((int)array[2] << 8) | (int)array[3]) << 32) : ((long)(((int)array[7] << 24) | ((int)array[6] << 16) | ((int)array[5] << 8) | (int)array[4]) << 32));
			long num2 = (BitConverter.IsLittleEndian ? ((long)(((int)array[4] << 24) | ((int)array[5] << 16) | ((int)array[6] << 8) | (int)array[7]) & (long)((ulong)(-1))) : ((long)(((int)array[3] << 24) | ((int)array[2] << 16) | ((int)array[1] << 8) | (int)array[0]) & (long)((ulong)(-1))));
			return num | num2;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005850 File Offset: 0x00003A50
		public static byte[] LeftmostBits(byte[] data, int lengthBits)
		{
			Ensure.Divisible(lengthBits, 8, "LeftmostBits() expects length in bits divisible by 8, but was given {0}", new object[] { lengthBits });
			int num = lengthBits / 8;
			byte[] array = new byte[num];
			Buffer.BlockCopy(data, 0, array, 0, num);
			return array;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005890 File Offset: 0x00003A90
		public static byte[] RightmostBits(byte[] data, int lengthBits)
		{
			Ensure.Divisible(lengthBits, 8, "RightmostBits() expects length in bits divisible by 8, but was given {0}", new object[] { lengthBits });
			int num = lengthBits / 8;
			byte[] array = new byte[num];
			Buffer.BlockCopy(data, data.Length - num, array, 0, num);
			return array;
		}

		// Token: 0x0400005C RID: 92
		public static readonly byte[] Empty = new byte[0];

		// Token: 0x0400005D RID: 93
		public static readonly byte[] Zero = new byte[1];

		// Token: 0x0400005E RID: 94
		private static RandomNumberGenerator rng;
	}
}
