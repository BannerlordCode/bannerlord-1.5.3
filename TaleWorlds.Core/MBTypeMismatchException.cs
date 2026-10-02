using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A1 RID: 161
	public class MBTypeMismatchException : MBException
	{
		// Token: 0x06000913 RID: 2323 RVA: 0x0001DE3B File Offset: 0x0001C03B
		public MBTypeMismatchException(string exceptionString)
			: base("Type Does not match with the expected one. " + exceptionString)
		{
		}
	}
}
