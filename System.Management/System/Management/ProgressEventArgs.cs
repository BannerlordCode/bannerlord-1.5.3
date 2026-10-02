using System;

namespace System.Management
{
	// Token: 0x0200002B RID: 43
	public class ProgressEventArgs : ManagementEventArgs
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x000036D1 File Offset: 0x000018D1
		internal ProgressEventArgs()
		{
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x000036D9 File Offset: 0x000018D9
		public int Current
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x000036E5 File Offset: 0x000018E5
		public string Message
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x000036F1 File Offset: 0x000018F1
		public int UpperBound
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}
	}
}
