using System;
using System.Collections.Generic;

namespace Jose
{
	// Token: 0x0200001E RID: 30
	public class EcdhKeyManagementWithAesKeyWrap : EcdhKeyManagement
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00004846 File Offset: 0x00002A46
		public EcdhKeyManagementWithAesKeyWrap(int keyLengthBits, AesKeyWrapManagement aesKw)
			: base(false)
		{
			this.aesKW = aesKw;
			this.keyLengthBits = keyLengthBits;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004860 File Offset: 0x00002A60
		public override byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
		{
			byte[] array = base.WrapNewKey(this.keyLengthBits, key, header)[0];
			return this.aesKW.WrapNewKey(cekSizeBits, array, header);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000488C File Offset: 0x00002A8C
		public override byte[] Unwrap(byte[] encryptedCek, object key, int cekSizeBits, IDictionary<string, object> header)
		{
			byte[] array = base.Unwrap(Arrays.Empty, key, this.keyLengthBits, header);
			return this.aesKW.Unwrap(encryptedCek, array, cekSizeBits, header);
		}

		// Token: 0x04000051 RID: 81
		private AesKeyWrapManagement aesKW;

		// Token: 0x04000052 RID: 82
		private int keyLengthBits;
	}
}
