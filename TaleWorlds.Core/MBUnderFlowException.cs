using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A2 RID: 162
	public class MBUnderFlowException : MBException
	{
		// Token: 0x06000914 RID: 2324 RVA: 0x0001DE4E File Offset: 0x0001C04E
		public MBUnderFlowException()
			: base("The given value is less than the expected value.")
		{
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001DE5B File Offset: 0x0001C05B
		public MBUnderFlowException(string parameterName)
			: base("The given value is less than the expected value : " + parameterName)
		{
		}
	}
}
