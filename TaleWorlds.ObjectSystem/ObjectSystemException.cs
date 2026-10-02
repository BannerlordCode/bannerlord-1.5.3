using System;

namespace TaleWorlds.ObjectSystem
{
	// Token: 0x02000010 RID: 16
	public class ObjectSystemException : Exception
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00004F4B File Offset: 0x0000314B
		internal ObjectSystemException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004F55 File Offset: 0x00003155
		internal ObjectSystemException(string message)
			: base(message)
		{
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004F5E File Offset: 0x0000315E
		internal ObjectSystemException()
		{
		}
	}
}
