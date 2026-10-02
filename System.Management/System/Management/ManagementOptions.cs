using System;
using System.ComponentModel;

namespace System.Management
{
	// Token: 0x0200001E RID: 30
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public abstract class ManagementOptions : ICloneable
	{
		// Token: 0x06000158 RID: 344 RVA: 0x000032F3 File Offset: 0x000014F3
		internal ManagementOptions()
		{
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000032FB File Offset: 0x000014FB
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00003307 File Offset: 0x00001507
		public ManagementNamedValueCollection Context
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00003313 File Offset: 0x00001513
		// (set) Token: 0x0600015C RID: 348 RVA: 0x0000331F File Offset: 0x0000151F
		public TimeSpan Timeout
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

		// Token: 0x0600015D RID: 349
		public abstract object Clone();

		// Token: 0x04000031 RID: 49
		public static readonly TimeSpan InfiniteTimeout;
	}
}
