using System;

namespace TaleWorlds.ObjectSystem
{
	// Token: 0x0200000B RID: 11
	public class MBOutOfRangeException : ObjectSystemException
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00004E73 File Offset: 0x00003073
		internal MBOutOfRangeException(string parameterName)
			: base("The given value is out of range : " + parameterName)
		{
		}
	}
}
