using System;
using System.Security.Cryptography;
using Jose.native;

namespace Jose
{
	// Token: 0x02000015 RID: 21
	public static class RsaOaep
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00003FB8 File Offset: 0x000021B8
		public static byte[] Decrypt(byte[] cipherText, CngKey key, CngAlgorithm hash)
		{
			BCrypt.BCRYPT_OAEP_PADDING_INFO bcrypt_OAEP_PADDING_INFO = new BCrypt.BCRYPT_OAEP_PADDING_INFO(hash.Algorithm);
			uint num2;
			uint num = NCrypt.NCryptDecrypt(key.Handle, cipherText, cipherText.Length, ref bcrypt_OAEP_PADDING_INFO, null, 0U, out num2, 4U);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("NCrypt.Decrypt() (plaintext buffer size) failed with status code:{0}", num));
			}
			byte[] array = new byte[num2];
			num = NCrypt.NCryptDecrypt(key.Handle, cipherText, cipherText.Length, ref bcrypt_OAEP_PADDING_INFO, array, num2, out num2, 4U);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("NCrypt.Decrypt() failed with status code:{0}", num));
			}
			return array;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000403C File Offset: 0x0000223C
		public static byte[] Encrypt(byte[] plainText, CngKey key, CngAlgorithm hash)
		{
			BCrypt.BCRYPT_OAEP_PADDING_INFO bcrypt_OAEP_PADDING_INFO = new BCrypt.BCRYPT_OAEP_PADDING_INFO(hash.Algorithm);
			uint num2;
			uint num = NCrypt.NCryptEncrypt(key.Handle, plainText, plainText.Length, ref bcrypt_OAEP_PADDING_INFO, null, 0U, out num2, 4U);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("NCrypt.Encrypt() (ciphertext buffer size) failed with status code:{0}", num));
			}
			byte[] array = new byte[num2];
			num = NCrypt.NCryptEncrypt(key.Handle, plainText, plainText.Length, ref bcrypt_OAEP_PADDING_INFO, array, num2, out num2, 4U);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("NCrypt.Encrypt() failed with status code:{0}", num));
			}
			return array;
		}
	}
}
