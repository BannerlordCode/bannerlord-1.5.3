using System;

namespace Jose
{
	// Token: 0x02000027 RID: 39
	public class Plaintext : IJwsAlgorithm
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x0000460C File Offset: 0x0000280C
		public byte[] Sign(byte[] securedInput, object key)
		{
			return Arrays.Empty;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005128 File Offset: 0x00003328
		public bool Verify(byte[] signature, byte[] securedInput, object key)
		{
			Ensure.IsNull(key, "Plaintext alg expectes key to be null.", Array.Empty<object>());
			return signature.Length == 0;
		}
	}
}
