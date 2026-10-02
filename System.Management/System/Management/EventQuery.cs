using System;

namespace System.Management
{
	// Token: 0x0200000F RID: 15
	public class EventQuery : ManagementQuery
	{
		// Token: 0x0600008F RID: 143 RVA: 0x0000286A File Offset: 0x00000A6A
		public EventQuery()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000287C File Offset: 0x00000A7C
		public EventQuery(string query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000288E File Offset: 0x00000A8E
		public EventQuery(string language, string query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000028A0 File Offset: 0x00000AA0
		public override object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
