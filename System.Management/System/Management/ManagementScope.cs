using System;

namespace System.Management
{
	// Token: 0x02000021 RID: 33
	public class ManagementScope : ICloneable
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00003483 File Offset: 0x00001683
		public ManagementScope()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00003495 File Offset: 0x00001695
		public ManagementScope(ManagementPath path)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000034A7 File Offset: 0x000016A7
		public ManagementScope(ManagementPath path, ConnectionOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000034B9 File Offset: 0x000016B9
		public ManagementScope(string path)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000034CB File Offset: 0x000016CB
		public ManagementScope(string path, ConnectionOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000034DD File Offset: 0x000016DD
		public bool IsConnected
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000181 RID: 385 RVA: 0x000034E9 File Offset: 0x000016E9
		// (set) Token: 0x06000182 RID: 386 RVA: 0x000034F5 File Offset: 0x000016F5
		public ConnectionOptions Options
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

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00003501 File Offset: 0x00001701
		// (set) Token: 0x06000184 RID: 388 RVA: 0x0000350D File Offset: 0x0000170D
		public ManagementPath Path
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

		// Token: 0x06000185 RID: 389 RVA: 0x00003519 File Offset: 0x00001719
		public ManagementScope Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00003525 File Offset: 0x00001725
		public void Connect()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00003531 File Offset: 0x00001731
		object ICloneable.Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
