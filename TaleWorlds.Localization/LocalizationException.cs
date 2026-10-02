using System;

namespace TaleWorlds.Localization
{
	// Token: 0x02000005 RID: 5
	public class LocalizationException : Exception
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00002D97 File Offset: 0x00000F97
		public LocalizationException()
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002D9F File Offset: 0x00000F9F
		public LocalizationException(string message)
			: base(message)
		{
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002DA8 File Offset: 0x00000FA8
		public LocalizationException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
