using System;
using System.Collections.Generic;

namespace Jose
{
	// Token: 0x0200001A RID: 26
	public class AesGcmKeyWrapManagement : IKeyManagement
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00004293 File Offset: 0x00002493
		public AesGcmKeyWrapManagement(int keyLengthBits)
		{
			this.keyLengthBits = keyLengthBits;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000042A4 File Offset: 0x000024A4
		public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
		{
			byte[] array = Ensure.Type<byte[]>(key, "AesGcmKeyWrapManagement alg expectes key to be byte[] array.", Array.Empty<object>());
			Ensure.BitSize(array, (long)this.keyLengthBits, string.Format("AesGcmKeyWrapManagement management algorithm expected key of size {0} bits, but was given {1} bits", this.keyLengthBits, (long)array.Length * 8L), Array.Empty<object>());
			byte[] array2 = Arrays.Random(96);
			byte[] array3 = Arrays.Random(cekSizeBits);
			byte[][] array4 = AesGcm.Encrypt(array, array2, null, array3);
			header["iv"] = Base64Url.Encode(array2);
			header["tag"] = Base64Url.Encode(array4[1]);
			return new byte[][]
			{
				array3,
				array4[0]
			};
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004344 File Offset: 0x00002544
		public byte[] Unwrap(byte[] encryptedCek, object key, int cekSizeBits, IDictionary<string, object> header)
		{
			byte[] array = Ensure.Type<byte[]>(key, "AesGcmKeyWrapManagement alg expectes key to be byte[] array.", Array.Empty<object>());
			Ensure.BitSize(array, (long)this.keyLengthBits, string.Format("AesGcmKeyWrapManagement management algorithm expected key of size {0} bits, but was given {1} bits", this.keyLengthBits, (long)array.Length * 8L), Array.Empty<object>());
			Ensure.Contains(header, new string[] { "iv" }, "AesGcmKeyWrapManagement algorithm expects 'iv' param in JWT header, but was not found", Array.Empty<object>());
			Ensure.Contains(header, new string[] { "tag" }, "AesGcmKeyWrapManagement algorithm expects 'tag' param in JWT header, but was not found", Array.Empty<object>());
			byte[] array2 = Base64Url.Decode((string)header["iv"]);
			byte[] array3 = Base64Url.Decode((string)header["tag"]);
			return AesGcm.Decrypt(array, array2, null, encryptedCek, array3);
		}

		// Token: 0x0400004E RID: 78
		private int keyLengthBits;
	}
}
