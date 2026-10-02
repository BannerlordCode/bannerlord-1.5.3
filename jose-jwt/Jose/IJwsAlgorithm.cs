using System;

namespace Jose
{
	// Token: 0x02000026 RID: 38
	public interface IJwsAlgorithm
	{
		// Token: 0x060000A2 RID: 162
		byte[] Sign(byte[] securedInput, object key);

		// Token: 0x060000A3 RID: 163
		bool Verify(byte[] signature, byte[] securedInput, object key);
	}
}
