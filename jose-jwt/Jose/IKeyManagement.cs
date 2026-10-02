using System;
using System.Collections.Generic;

namespace Jose
{
	// Token: 0x0200001F RID: 31
	public interface IKeyManagement
	{
		// Token: 0x06000089 RID: 137
		byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header);

		// Token: 0x0600008A RID: 138
		byte[] Unwrap(byte[] encryptedCek, object key, int cekSizeBits, IDictionary<string, object> header);
	}
}
