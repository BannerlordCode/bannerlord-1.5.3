using System;
using System.Collections.Generic;

namespace Jose
{
	// Token: 0x0200001C RID: 28
	public class DirectKeyManagement : IKeyManagement
	{
		// Token: 0x0600007C RID: 124 RVA: 0x000044E4 File Offset: 0x000026E4
		public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
		{
			return new byte[][]
			{
				Ensure.Type<byte[]>(key, "DirectKeyManagement alg expectes key to be byte[] array.", Array.Empty<object>()),
				Arrays.Empty
			};
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004507 File Offset: 0x00002707
		public byte[] Unwrap(byte[] encryptedCek, object key, int cekSizeBits, IDictionary<string, object> header)
		{
			Ensure.IsEmpty(encryptedCek, "DirectKeyManagement expects empty content encryption key.", Array.Empty<object>());
			return Ensure.Type<byte[]>(key, "DirectKeyManagement alg expectes key to be byte[] array.", Array.Empty<object>());
		}
	}
}
