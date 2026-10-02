using System;
using System.Security.Cryptography;

namespace Jose
{
	// Token: 0x02000014 RID: 20
	public static class PBKDF2
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00003ED8 File Offset: 0x000020D8
		public static byte[] DeriveKey(byte[] password, byte[] salt, int iterationCount, int keyBitLength, HMAC prf)
		{
			prf.Key = password;
			Ensure.MaxValue(keyBitLength, (long)((ulong)(-1)), "PBKDF2 expect derived key size to be not more that (2^32-1) bits, but was requested {0} bits.", new object[] { keyBitLength });
			int num = prf.HashSize / 8;
			int num2 = keyBitLength / 8;
			int num3 = (int)Math.Ceiling((double)num2 / (double)num);
			int num4 = num2 - (num3 - 1) * num;
			byte[][] array = new byte[num3][];
			for (int i = 0; i < num3; i++)
			{
				array[i] = PBKDF2.F(salt, iterationCount, i + 1, prf);
			}
			array[num3 - 1] = Arrays.LeftmostBits(array[num3 - 1], num4 * 8);
			return Arrays.Concat(array);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003F6C File Offset: 0x0000216C
		private static byte[] F(byte[] salt, int iterationCount, int blockIndex, HMAC prf)
		{
			byte[] array = prf.ComputeHash(Arrays.Concat(new byte[][]
			{
				salt,
				Arrays.IntToBytes(blockIndex)
			}));
			byte[] array2 = array;
			for (int i = 2; i <= iterationCount; i++)
			{
				array = prf.ComputeHash(array);
				array2 = Arrays.Xor(array2, array);
			}
			return array2;
		}
	}
}
