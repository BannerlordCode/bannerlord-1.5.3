using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Jose
{
	// Token: 0x02000022 RID: 34
	public class RsaOaep256KeyManagement : IKeyManagement
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00004BC8 File Offset: 0x00002DC8
		public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
		{
			byte[] array = Arrays.Random(cekSizeBits);
			RSA rsa = Ensure.Type<RSA>(key, "RsaKeyManagement algorithm expects key to be of RSA type.", Array.Empty<object>());
			return new byte[][]
			{
				array,
				rsa.Encrypt(array, RSAEncryptionPadding.OaepSHA256)
			};
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004C06 File Offset: 0x00002E06
		public byte[] Unwrap(byte[] encryptedCek, object key, int cekSizeBits, IDictionary<string, object> header)
		{
			return Ensure.Type<RSA>(key, "RsaKeyManagement algorithm expects key to be of RSA type.", Array.Empty<object>()).Decrypt(encryptedCek, RSAEncryptionPadding.OaepSHA256);
		}
	}
}
