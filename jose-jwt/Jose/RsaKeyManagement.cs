using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Jose
{
	// Token: 0x02000021 RID: 33
	public class RsaKeyManagement : IKeyManagement
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00004B2C File Offset: 0x00002D2C
		public RsaKeyManagement(bool useRsaOaepPadding)
		{
			this.useRsaOaepPadding = useRsaOaepPadding;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004B3C File Offset: 0x00002D3C
		public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
		{
			byte[] array = Arrays.Random(cekSizeBits);
			RSA rsa = Ensure.Type<RSA>(key, "RsaKeyManagement alg expects key to be of RSA type.", Array.Empty<object>());
			RSAEncryptionPadding rsaencryptionPadding = (this.useRsaOaepPadding ? RSAEncryptionPadding.OaepSHA1 : RSAEncryptionPadding.Pkcs1);
			return new byte[][]
			{
				array,
				rsa.Encrypt(array, rsaencryptionPadding)
			};
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004B8C File Offset: 0x00002D8C
		public byte[] Unwrap(byte[] encryptedCek, object key, int cekSizeBits, IDictionary<string, object> header)
		{
			RSA rsa = Ensure.Type<RSA>(key, "RsaKeyManagement algorithm expects key to be of RSA type.", Array.Empty<object>());
			RSAEncryptionPadding rsaencryptionPadding = (this.useRsaOaepPadding ? RSAEncryptionPadding.OaepSHA1 : RSAEncryptionPadding.Pkcs1);
			return rsa.Decrypt(encryptedCek, rsaencryptionPadding);
		}

		// Token: 0x04000055 RID: 85
		private bool useRsaOaepPadding;
	}
}
