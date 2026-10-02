using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Core
{
	// Token: 0x020000A0 RID: 160
	public class MBException : ApplicationException
	{
		// Token: 0x0600090F RID: 2319 RVA: 0x0001DE16 File Offset: 0x0001C016
		public MBException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0001DE20 File Offset: 0x0001C020
		public MBException(string message)
			: base(message)
		{
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001DE29 File Offset: 0x0001C029
		public MBException()
		{
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0001DE31 File Offset: 0x0001C031
		public MBException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
