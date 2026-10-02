using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A3 RID: 163
	public class MBOutOfRangeException : MBException
	{
		// Token: 0x06000916 RID: 2326 RVA: 0x0001DE6E File Offset: 0x0001C06E
		public MBOutOfRangeException(string parameterName)
			: base("The given value is out of range : " + parameterName)
		{
		}
	}
}
