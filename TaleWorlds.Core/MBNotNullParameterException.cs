using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A7 RID: 167
	public class MBNotNullParameterException : MBException
	{
		// Token: 0x0600091A RID: 2330 RVA: 0x0001DEBA File Offset: 0x0001C0BA
		public MBNotNullParameterException(string parameterName)
			: base("The parameter must be null : " + parameterName)
		{
		}
	}
}
