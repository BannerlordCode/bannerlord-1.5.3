using System;

namespace System.Management
{
	// Token: 0x02000036 RID: 54
	public class StoppedEventArgs : ManagementEventArgs
	{
		// Token: 0x0600022F RID: 559 RVA: 0x00003CED File Offset: 0x00001EED
		internal StoppedEventArgs()
		{
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00003CF5 File Offset: 0x00001EF5
		public ManagementStatus Status
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}
	}
}
