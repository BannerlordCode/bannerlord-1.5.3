using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A5 RID: 165
	public class MBInvalidParameterException : MBException
	{
		// Token: 0x06000918 RID: 2328 RVA: 0x0001DE94 File Offset: 0x0001C094
		public MBInvalidParameterException(string parameterName)
			: base("The parameter must be valid : " + parameterName)
		{
		}
	}
}
