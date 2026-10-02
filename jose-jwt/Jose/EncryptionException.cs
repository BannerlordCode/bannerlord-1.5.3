using System;

namespace Jose
{
	// Token: 0x0200000B RID: 11
	public class EncryptionException : JoseException
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002D42 File Offset: 0x00000F42
		public EncryptionException(string message)
			: base(message)
		{
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002D4B File Offset: 0x00000F4B
		public EncryptionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
