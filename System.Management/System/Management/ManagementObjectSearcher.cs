using System;
using System.ComponentModel;

namespace System.Management
{
	// Token: 0x0200001C RID: 28
	[ToolboxItem(false)]
	public class ManagementObjectSearcher : Component
	{
		// Token: 0x0600013F RID: 319 RVA: 0x000031E7 File Offset: 0x000013E7
		public ManagementObjectSearcher()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000031F9 File Offset: 0x000013F9
		public ManagementObjectSearcher(ManagementScope scope, ObjectQuery query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000320B File Offset: 0x0000140B
		public ManagementObjectSearcher(ManagementScope scope, ObjectQuery query, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000321D File Offset: 0x0000141D
		public ManagementObjectSearcher(ObjectQuery query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000322F File Offset: 0x0000142F
		public ManagementObjectSearcher(string queryString)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00003241 File Offset: 0x00001441
		public ManagementObjectSearcher(string scope, string queryString)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00003253 File Offset: 0x00001453
		public ManagementObjectSearcher(string scope, string queryString, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00003265 File Offset: 0x00001465
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00003271 File Offset: 0x00001471
		public EnumerationOptions Options
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

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000327D File Offset: 0x0000147D
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00003289 File Offset: 0x00001489
		public ObjectQuery Query
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00003295 File Offset: 0x00001495
		// (set) Token: 0x0600014B RID: 331 RVA: 0x000032A1 File Offset: 0x000014A1
		public ManagementScope Scope
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

		// Token: 0x0600014C RID: 332 RVA: 0x000032AD File Offset: 0x000014AD
		public ManagementObjectCollection Get()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000032B9 File Offset: 0x000014B9
		public void Get(ManagementOperationObserver watcher)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
