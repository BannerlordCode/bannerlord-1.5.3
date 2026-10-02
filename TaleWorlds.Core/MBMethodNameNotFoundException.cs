using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A4 RID: 164
	public class MBMethodNameNotFoundException : MBException
	{
		// Token: 0x06000917 RID: 2327 RVA: 0x0001DE81 File Offset: 0x0001C081
		public MBMethodNameNotFoundException(string methodName)
			: base("Unable to find method " + methodName)
		{
		}
	}
}
