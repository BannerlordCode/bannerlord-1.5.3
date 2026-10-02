using System;
using System.Security.Cryptography;
using Jose.native;

namespace Jose
{
	// Token: 0x02000016 RID: 22
	public static class RsaPss
	{
		// Token: 0x06000068 RID: 104 RVA: 0x000040C0 File Offset: 0x000022C0
		public static byte[] Sign(byte[] input, CngKey key, CngAlgorithm hash, int saltSize)
		{
			byte[] array;
			using (HashAlgorithm hashAlgorithm = RsaPss.HashAlgorithm(hash))
			{
				array = RsaPss.SignHash(hashAlgorithm.ComputeHash(input), key, hash.Algorithm, saltSize);
			}
			return array;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004108 File Offset: 0x00002308
		public static bool Verify(byte[] securedInput, byte[] signature, CngKey key, CngAlgorithm hash, int saltSize)
		{
			bool flag;
			using (HashAlgorithm hashAlgorithm = RsaPss.HashAlgorithm(hash))
			{
				flag = RsaPss.VerifyHash(hashAlgorithm.ComputeHash(securedInput), signature, key, hash.Algorithm, saltSize);
			}
			return flag;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004150 File Offset: 0x00002350
		private static bool VerifyHash(byte[] hash, byte[] signature, CngKey key, string algorithm, int saltSize)
		{
			BCrypt.BCRYPT_PSS_PADDING_INFO bcrypt_PSS_PADDING_INFO = new BCrypt.BCRYPT_PSS_PADDING_INFO(algorithm, saltSize);
			uint num = NCrypt.NCryptVerifySignature(key.Handle, ref bcrypt_PSS_PADDING_INFO, hash, hash.Length, signature, signature.Length, 8U);
			if (num == 2148073478U)
			{
				return false;
			}
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("NCrypt.NCryptSignHash() (signature size) failed with status code:{0}", num));
			}
			return true;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000041A4 File Offset: 0x000023A4
		private static byte[] SignHash(byte[] hash, CngKey key, string algorithm, int saltSize)
		{
			BCrypt.BCRYPT_PSS_PADDING_INFO bcrypt_PSS_PADDING_INFO = new BCrypt.BCRYPT_PSS_PADDING_INFO(algorithm, saltSize);
			uint num2;
			uint num = NCrypt.NCryptSignHash(key.Handle, ref bcrypt_PSS_PADDING_INFO, hash, hash.Length, null, 0, out num2, 8U);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("NCrypt.NCryptSignHash() (signature size) failed with status code:{0}", num));
			}
			byte[] array = new byte[num2];
			num = NCrypt.NCryptSignHash(key.Handle, ref bcrypt_PSS_PADDING_INFO, hash, hash.Length, array, array.Length, out num2, 8U);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("NCrypt.NCryptSignHash() failed with status code:{0}", num));
			}
			return array;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003ECC File Offset: 0x000020CC
		private static HashAlgorithm HashAlgorithm(CngAlgorithm hash)
		{
			throw new NotImplementedException("not yet");
		}
	}
}
