using System;

namespace Jose
{
	// Token: 0x02000010 RID: 16
	public interface ICompression
	{
		// Token: 0x06000055 RID: 85
		byte[] Compress(byte[] plainText);

		// Token: 0x06000056 RID: 86
		byte[] Decompress(byte[] compressedText);
	}
}
