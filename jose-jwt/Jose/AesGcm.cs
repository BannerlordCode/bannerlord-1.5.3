using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Jose.native;

namespace Jose
{
	// Token: 0x02000011 RID: 17
	public static class AesGcm
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00003738 File Offset: 0x00001938
		public static byte[][] Encrypt(byte[] key, byte[] iv, byte[] aad, byte[] plainText)
		{
			IntPtr intPtr = AesGcm.OpenAlgorithmProvider(BCrypt.BCRYPT_AES_ALGORITHM, BCrypt.MS_PRIMITIVE_PROVIDER, BCrypt.BCRYPT_CHAIN_MODE_GCM);
			IntPtr intPtr3;
			IntPtr intPtr2 = AesGcm.ImportKey(intPtr, key, out intPtr3);
			byte[] array = new byte[AesGcm.MaxAuthTagSize(intPtr)];
			BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO bcrypt_AUTHENTICATED_CIPHER_MODE_INFO = new BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(iv, aad, array);
			byte[] array3;
			using (bcrypt_AUTHENTICATED_CIPHER_MODE_INFO)
			{
				byte[] array2 = new byte[array.Length];
				int num = 0;
				uint num2 = BCrypt.BCryptEncrypt(intPtr3, plainText, plainText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array2, array2.Length, null, 0, ref num, 0U);
				if (num2 != 0U)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptEncrypt() (get size) failed with status code:{0}", num2));
				}
				array3 = new byte[num];
				num2 = BCrypt.BCryptEncrypt(intPtr3, plainText, plainText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array2, array2.Length, array3, array3.Length, ref num, 0U);
				if (num2 != 0U)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptEncrypt() failed with status code:{0}", num2));
				}
				Marshal.Copy(bcrypt_AUTHENTICATED_CIPHER_MODE_INFO.pbTag, array, 0, bcrypt_AUTHENTICATED_CIPHER_MODE_INFO.cbTag);
			}
			BCrypt.BCryptDestroyKey(intPtr3);
			Marshal.FreeHGlobal(intPtr2);
			BCrypt.BCryptCloseAlgorithmProvider(intPtr, 0U);
			return new byte[][] { array3, array };
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000385C File Offset: 0x00001A5C
		public static byte[] Decrypt(byte[] key, byte[] iv, byte[] aad, byte[] cipherText, byte[] authTag)
		{
			IntPtr intPtr = AesGcm.OpenAlgorithmProvider(BCrypt.BCRYPT_AES_ALGORITHM, BCrypt.MS_PRIMITIVE_PROVIDER, BCrypt.BCRYPT_CHAIN_MODE_GCM);
			IntPtr intPtr3;
			IntPtr intPtr2 = AesGcm.ImportKey(intPtr, key, out intPtr3);
			BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO bcrypt_AUTHENTICATED_CIPHER_MODE_INFO = new BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(iv, aad, authTag);
			byte[] array2;
			using (bcrypt_AUTHENTICATED_CIPHER_MODE_INFO)
			{
				byte[] array = new byte[AesGcm.MaxAuthTagSize(intPtr)];
				int num = 0;
				uint num2 = BCrypt.BCryptDecrypt(intPtr3, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, null, 0, ref num, 0);
				if (num2 != 0U)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptDecrypt() (get size) failed with status code: {0}", num2));
				}
				array2 = new byte[num];
				num2 = BCrypt.BCryptDecrypt(intPtr3, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, array2, array2.Length, ref num, 0);
				if (num2 == BCrypt.STATUS_AUTH_TAG_MISMATCH)
				{
					throw new CryptographicException("BCrypt.BCryptDecrypt(): authentication tag mismatch");
				}
				if (num2 != 0U)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptDecrypt() failed with status code:{0}", num2));
				}
			}
			BCrypt.BCryptDestroyKey(intPtr3);
			Marshal.FreeHGlobal(intPtr2);
			BCrypt.BCryptCloseAlgorithmProvider(intPtr, 0U);
			return array2;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003964 File Offset: 0x00001B64
		private static int MaxAuthTagSize(IntPtr hAlg)
		{
			byte[] property = AesGcm.GetProperty(hAlg, BCrypt.BCRYPT_AUTH_TAG_LENGTH);
			return BitConverter.ToInt32(new byte[]
			{
				property[4],
				property[5],
				property[6],
				property[7]
			}, 0);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000039A4 File Offset: 0x00001BA4
		private static IntPtr OpenAlgorithmProvider(string alg, string provider, string chainingMode)
		{
			IntPtr zero = IntPtr.Zero;
			uint num = BCrypt.BCryptOpenAlgorithmProvider(out zero, alg, provider, 0U);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptOpenAlgorithmProvider() failed with status code:{0}", num));
			}
			byte[] bytes = Encoding.Unicode.GetBytes(chainingMode);
			num = BCrypt.BCryptSetAlgorithmProperty(zero, BCrypt.BCRYPT_CHAINING_MODE, bytes, bytes.Length, 0);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptSetAlgorithmProperty(BCrypt.BCRYPT_CHAINING_MODE, BCrypt.BCRYPT_CHAIN_MODE_GCM) failed with status code:{0}", num));
			}
			return zero;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003A14 File Offset: 0x00001C14
		private static IntPtr ImportKey(IntPtr hAlg, byte[] key, out IntPtr hKey)
		{
			int num = BitConverter.ToInt32(AesGcm.GetProperty(hAlg, BCrypt.BCRYPT_OBJECT_LENGTH), 0);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			byte[] array = Arrays.Concat(new byte[][]
			{
				BCrypt.BCRYPT_KEY_DATA_BLOB_MAGIC,
				BitConverter.GetBytes(1),
				BitConverter.GetBytes(key.Length),
				key
			});
			uint num2 = BCrypt.BCryptImportKey(hAlg, IntPtr.Zero, BCrypt.BCRYPT_KEY_DATA_BLOB, out hKey, intPtr, num, array, array.Length, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptImportKey() failed with status code:{0}", num2));
			}
			return intPtr;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003A9C File Offset: 0x00001C9C
		private static byte[] GetProperty(IntPtr hAlg, string name)
		{
			int num = 0;
			uint num2 = BCrypt.BCryptGetProperty(hAlg, name, null, 0, ref num, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptGetProperty() (get size) failed with status code:{0}", num2));
			}
			byte[] array = new byte[num];
			num2 = BCrypt.BCryptGetProperty(hAlg, name, array, array.Length, ref num, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptGetProperty() failed with status code:{0}", num2));
			}
			return array;
		}
	}
}
