using System;
using System.IO;
using System.Security.Cryptography;
using Jose.jwe;

namespace Jose
{
	// Token: 0x02000023 RID: 35
	public class AesCbcHmacEncryption : IJweAlgorithm
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00004C23 File Offset: 0x00002E23
		public AesCbcHmacEncryption(IJwsAlgorithm hashAlgorithm, int keyLength)
		{
			this.hashAlgorithm = hashAlgorithm;
			this.keyLength = keyLength;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004C3C File Offset: 0x00002E3C
		public byte[][] Encrypt(byte[] aad, byte[] plainText, byte[] cek)
		{
			Ensure.BitSize(cek, (long)this.keyLength, string.Format("AES-CBC with HMAC algorithm expected key of size {0} bits, but was given {1} bits", this.keyLength, (long)cek.Length * 8L), Array.Empty<object>());
			byte[] array = Arrays.FirstHalf(cek);
			byte[] array2 = Arrays.SecondHalf(cek);
			byte[] array3 = Arrays.Random(128);
			byte[] array4;
			try
			{
				using (Aes aes = Aes.Create())
				{
					aes.Key = array2;
					aes.IV = array3;
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV))
						{
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
							{
								cryptoStream.Write(plainText, 0, plainText.Length);
								cryptoStream.FlushFinalBlock();
								array4 = memoryStream.ToArray();
							}
						}
					}
				}
			}
			catch (CryptographicException ex)
			{
				throw new EncryptionException("Unable to encrypt content.", ex);
			}
			byte[] array5 = this.ComputeAuthTag(aad, array3, array4, array);
			return new byte[][] { array3, array4, array5 };
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004D9C File Offset: 0x00002F9C
		public byte[] Decrypt(byte[] aad, byte[] cek, byte[] iv, byte[] cipherText, byte[] authTag)
		{
			Ensure.BitSize(cek, (long)this.keyLength, string.Format("AES-CBC with HMAC algorithm expected key of size {0} bits, but was given {1} bits", this.keyLength, (long)cek.Length * 8L), Array.Empty<object>());
			byte[] array = Arrays.FirstHalf(cek);
			byte[] array2 = Arrays.SecondHalf(cek);
			if (!Arrays.ConstantTimeEquals(this.ComputeAuthTag(aad, iv, cipherText, array), authTag))
			{
				throw new IntegrityException("Authentication tag do not match.");
			}
			byte[] array3;
			try
			{
				using (Aes aes = Aes.Create())
				{
					aes.Key = array2;
					aes.IV = iv;
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV))
						{
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
							{
								cryptoStream.Write(cipherText, 0, cipherText.Length);
								cryptoStream.FlushFinalBlock();
								array3 = memoryStream.ToArray();
							}
						}
					}
				}
			}
			catch (CryptographicException ex)
			{
				throw new EncryptionException("Unable to decrypt content", ex);
			}
			return array3;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004EE0 File Offset: 0x000030E0
		public int KeySize
		{
			get
			{
				return this.keyLength;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004EE8 File Offset: 0x000030E8
		private byte[] ComputeAuthTag(byte[] aad, byte[] iv, byte[] cipherText, byte[] hmacKey)
		{
			byte[] array = Arrays.LongToBytes((long)aad.Length * 8L);
			byte[] array2 = Arrays.Concat(new byte[][] { aad, iv, cipherText, array });
			return Arrays.FirstHalf(this.hashAlgorithm.Sign(array2, hmacKey));
		}

		// Token: 0x04000056 RID: 86
		private IJwsAlgorithm hashAlgorithm;

		// Token: 0x04000057 RID: 87
		private readonly int keyLength;
	}
}
