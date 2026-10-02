using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A6 RID: 166
	public class MBNullParameterException : MBException
	{
		// Token: 0x06000919 RID: 2329 RVA: 0x0001DEA7 File Offset: 0x0001C0A7
		public MBNullParameterException(string parameterName)
			: base("The parameter cannot be null : " + parameterName)
		{
		}
	}
}
