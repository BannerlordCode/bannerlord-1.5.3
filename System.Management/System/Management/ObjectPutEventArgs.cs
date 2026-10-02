using System;

namespace System.Management
{
	// Token: 0x02000026 RID: 38
	public class ObjectPutEventArgs : ManagementEventArgs
	{
		// Token: 0x060001A0 RID: 416 RVA: 0x00003667 File Offset: 0x00001867
		internal ObjectPutEventArgs()
		{
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000366F File Offset: 0x0000186F
		public ManagementPath Path
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}
	}
}
