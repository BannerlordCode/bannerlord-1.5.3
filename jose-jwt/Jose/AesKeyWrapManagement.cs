using System;
using System.Collections.Generic;

namespace Jose
{
	// Token: 0x0200001B RID: 27
	public class AesKeyWrapManagement : IKeyManagement
	{
		// Token: 0x06000079 RID: 121 RVA: 0x0000440B File Offset: 0x0000260B
		public AesKeyWrapManagement(int kekLengthBits)
		{
			this.kekLengthBits = kekLengthBits;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000441C File Offset: 0x0000261C
		public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
		{
			byte[] array = Ensure.Type<byte[]>(key, "AesKeyWrap management algorithm expectes key to be byte[] array.", Array.Empty<object>());
			Ensure.BitSize(array, (long)this.kekLengthBits, string.Format("AesKeyWrap management algorithm expected key of size {0} bits, but was given {1} bits", this.kekLengthBits, (long)array.Length * 8L), Array.Empty<object>());
			byte[] array2 = Arrays.Random(cekSizeBits);
			byte[] array3 = AesKeyWrap.Wrap(array2, array);
			return new byte[][] { array2, array3 };
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000448C File Offset: 0x0000268C
		public byte[] Unwrap(byte[] encryptedCek, object key, int cekSizeBits, IDictionary<string, object> header)
		{
			byte[] array = Ensure.Type<byte[]>(key, "AesKeyWrap management algorithm expectes key to be byte[] array.", Array.Empty<object>());
			Ensure.BitSize(array, (long)this.kekLengthBits, string.Format("AesKeyWrap management algorithm expected key of size {0} bits, but was given {1} bits", this.kekLengthBits, (long)array.Length * 8L), Array.Empty<object>());
			return AesKeyWrap.Unwrap(encryptedCek, array);
		}

		// Token: 0x0400004F RID: 79
		private readonly int kekLengthBits;
	}
}
