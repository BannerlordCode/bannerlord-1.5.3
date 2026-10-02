using System;

namespace Jose.jwe
{
	// Token: 0x02000033 RID: 51
	public interface IJweAlgorithm
	{
		// Token: 0x060000ED RID: 237
		byte[][] Encrypt(byte[] aad, byte[] plainText, byte[] cek);

		// Token: 0x060000EE RID: 238
		byte[] Decrypt(byte[] aad, byte[] cek, byte[] iv, byte[] cipherText, byte[] authTag);

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000EF RID: 239
		int KeySize { get; }
	}
}
