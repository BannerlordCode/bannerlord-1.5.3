using System;

namespace Jose
{
	// Token: 0x02000009 RID: 9
	public class JoseException : Exception
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002D2F File Offset: 0x00000F2F
		public JoseException(string message)
			: base(message)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002D38 File Offset: 0x00000F38
		public JoseException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
