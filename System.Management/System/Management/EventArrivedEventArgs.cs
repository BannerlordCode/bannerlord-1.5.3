using System;

namespace System.Management
{
	// Token: 0x0200000D RID: 13
	public class EventArrivedEventArgs : ManagementEventArgs
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00002856 File Offset: 0x00000A56
		internal EventArrivedEventArgs()
		{
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000285E File Offset: 0x00000A5E
		public ManagementBaseObject NewEvent
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}
	}
}
