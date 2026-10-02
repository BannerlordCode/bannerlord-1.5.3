using System;

namespace Jose
{
	// Token: 0x0200000C RID: 12
	public class InvalidAlgorithmException : JoseException
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00002D42 File Offset: 0x00000F42
		public InvalidAlgorithmException(string message)
			: base(message)
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002D4B File Offset: 0x00000F4B
		public InvalidAlgorithmException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
