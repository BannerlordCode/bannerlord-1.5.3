using System;

namespace Jose
{
	// Token: 0x0200000A RID: 10
	public class IntegrityException : JoseException
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002D42 File Offset: 0x00000F42
		public IntegrityException(string message)
			: base(message)
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002D4B File Offset: 0x00000F4B
		public IntegrityException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
