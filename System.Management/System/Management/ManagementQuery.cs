using System;

namespace System.Management
{
	// Token: 0x02000020 RID: 32
	public abstract class ManagementQuery : ICloneable
	{
		// Token: 0x06000174 RID: 372 RVA: 0x0000343F File Offset: 0x0000163F
		internal ManagementQuery()
		{
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00003447 File Offset: 0x00001647
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00003453 File Offset: 0x00001653
		public virtual string QueryLanguage
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

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000345F File Offset: 0x0000165F
		// (set) Token: 0x06000178 RID: 376 RVA: 0x0000346B File Offset: 0x0000166B
		public virtual string QueryString
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

		// Token: 0x06000179 RID: 377
		public abstract object Clone();

		// Token: 0x0600017A RID: 378 RVA: 0x00003477 File Offset: 0x00001677
		protected internal virtual void ParseQuery(string query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
