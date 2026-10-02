using System;
using System.Security.Cryptography;
using Jose.jwe;

namespace Jose
{
	// Token: 0x02000024 RID: 36
	public class AesGcmEncryption : IJweAlgorithm
	{
		// Token: 0x0600009A RID: 154 RVA: 0x00004F31 File Offset: 0x00003131
		public AesGcmEncryption(int keyLength)
		{
			this.keyLength = keyLength;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004F40 File Offset: 0x00003140
		public byte[][] Encrypt(byte[] aad, byte[] plainText, byte[] cek)
		{
			Ensure.BitSize(cek, (long)this.keyLength, string.Format("AES-GCM algorithm expected key of size {0} bits, but was given {1} bits", this.keyLength, (long)cek.Length * 8L), Array.Empty<object>());
			byte[] array = Arrays.Random(96);
			byte[][] array3;
			try
			{
				byte[][] array2 = AesGcm.Encrypt(cek, array, aad, plainText);
				array3 = new byte[][]
				{
					array,
					array2[0],
					array2[1]
				};
			}
			catch (CryptographicException ex)
			{
				throw new EncryptionException("Unable to encrypt content.", ex);
			}
			return array3;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004FCC File Offset: 0x000031CC
		public byte[] Decrypt(byte[] aad, byte[] cek, byte[] iv, byte[] cipherText, byte[] authTag)
		{
			Ensure.BitSize(cek, (long)this.keyLength, string.Format("AES-GCM algorithm expected key of size {0} bits, but was given {1} bits", this.keyLength, (long)cek.Length * 8L), Array.Empty<object>());
			byte[] array;
			try
			{
				array = AesGcm.Decrypt(cek, iv, aad, cipherText, authTag);
			}
			catch (CryptographicException ex)
			{
				throw new EncryptionException("Unable to decrypt content or authentication tag do not match.", ex);
			}
			return array;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000503C File Offset: 0x0000323C
		public int KeySize
		{
			get
			{
				return this.keyLength;
			}
		}

		// Token: 0x04000058 RID: 88
		private int keyLength;
	}
}
