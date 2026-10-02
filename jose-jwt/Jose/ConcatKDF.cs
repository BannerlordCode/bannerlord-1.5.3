using System;
using System.Security.Cryptography;

namespace Jose
{
	// Token: 0x02000013 RID: 19
	public class ConcatKDF
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00003ECC File Offset: 0x000020CC
		public static byte[] DeriveKey(CngKey externalPubKey, CngKey privateKey, int keyBitLength, byte[] algorithmId, byte[] partyVInfo, byte[] partyUInfo, byte[] suppPubInfo)
		{
			throw new NotImplementedException("not yet");
		}
	}
}
