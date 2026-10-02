using System;

namespace System.Management
{
	// Token: 0x02000010 RID: 16
	public class EventWatcherOptions : ManagementOptions
	{
		// Token: 0x06000093 RID: 147 RVA: 0x000028AC File Offset: 0x00000AAC
		public EventWatcherOptions()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000028BE File Offset: 0x00000ABE
		public EventWatcherOptions(ManagementNamedValueCollection context, TimeSpan timeout, int blockSize)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000028D0 File Offset: 0x00000AD0
		// (set) Token: 0x06000096 RID: 150 RVA: 0x000028DC File Offset: 0x00000ADC
		public int BlockSize
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
			set
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000028E8 File Offset: 0x00000AE8
		public override object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
