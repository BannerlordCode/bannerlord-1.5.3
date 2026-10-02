using System;

namespace System.Management
{
	// Token: 0x02000029 RID: 41
	public class ObjectReadyEventArgs : ManagementEventArgs
	{
		// Token: 0x060001AA RID: 426 RVA: 0x000036BD File Offset: 0x000018BD
		internal ObjectReadyEventArgs()
		{
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001AB RID: 427 RVA: 0x000036C5 File Offset: 0x000018C5
		public ManagementBaseObject NewObject
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}
	}
}
