using System;

namespace System.Management
{
	// Token: 0x02000016 RID: 22
	public abstract class ManagementEventArgs : EventArgs
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00002C58 File Offset: 0x00000E58
		internal ManagementEventArgs()
		{
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00002C60 File Offset: 0x00000E60
		public object Context
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}
	}
}
