using System;

namespace System.Management
{
	// Token: 0x02000008 RID: 8
	public class CompletedEventArgs : ManagementEventArgs
	{
		// Token: 0x0600005A RID: 90 RVA: 0x0000262C File Offset: 0x0000082C
		internal CompletedEventArgs()
		{
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002634 File Offset: 0x00000834
		public ManagementStatus Status
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002640 File Offset: 0x00000840
		public ManagementBaseObject StatusObject
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}
	}
}
