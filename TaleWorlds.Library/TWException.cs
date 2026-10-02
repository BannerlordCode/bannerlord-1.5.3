using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Library
{
	// Token: 0x02000097 RID: 151
	public class TWException : ApplicationException
	{
		// Token: 0x06000572 RID: 1394 RVA: 0x00013796 File Offset: 0x00011996
		public TWException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x000137A0 File Offset: 0x000119A0
		public TWException(string message)
			: base(message)
		{
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x000137A9 File Offset: 0x000119A9
		public TWException()
		{
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000137B1 File Offset: 0x000119B1
		public TWException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
