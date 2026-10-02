using System;
using System.Security;

namespace System.Management
{
	// Token: 0x0200000A RID: 10
	public class ConnectionOptions : ManagementOptions
	{
		// Token: 0x06000061 RID: 97 RVA: 0x0000264C File Offset: 0x0000084C
		public ConnectionOptions()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000265E File Offset: 0x0000085E
		public ConnectionOptions(string locale, string username, SecureString password, string authority, ImpersonationLevel impersonation, AuthenticationLevel authentication, bool enablePrivileges, ManagementNamedValueCollection context, TimeSpan timeout)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002670 File Offset: 0x00000870
		public ConnectionOptions(string locale, string username, string password, string authority, ImpersonationLevel impersonation, AuthenticationLevel authentication, bool enablePrivileges, ManagementNamedValueCollection context, TimeSpan timeout)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002682 File Offset: 0x00000882
		// (set) Token: 0x06000065 RID: 101 RVA: 0x0000268E File Offset: 0x0000088E
		public AuthenticationLevel Authentication
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

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000269A File Offset: 0x0000089A
		// (set) Token: 0x06000067 RID: 103 RVA: 0x000026A6 File Offset: 0x000008A6
		public string Authority
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

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000026B2 File Offset: 0x000008B2
		// (set) Token: 0x06000069 RID: 105 RVA: 0x000026BE File Offset: 0x000008BE
		public bool EnablePrivileges
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

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000026CA File Offset: 0x000008CA
		// (set) Token: 0x0600006B RID: 107 RVA: 0x000026D6 File Offset: 0x000008D6
		public ImpersonationLevel Impersonation
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

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000026E2 File Offset: 0x000008E2
		// (set) Token: 0x0600006D RID: 109 RVA: 0x000026EE File Offset: 0x000008EE
		public string Locale
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

		// Token: 0x17000057 RID: 87
		// (set) Token: 0x0600006E RID: 110 RVA: 0x000026FA File Offset: 0x000008FA
		public string Password
		{
			set
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000058 RID: 88
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00002706 File Offset: 0x00000906
		public SecureString SecurePassword
		{
			set
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002712 File Offset: 0x00000912
		// (set) Token: 0x06000071 RID: 113 RVA: 0x0000271E File Offset: 0x0000091E
		public string Username
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

		// Token: 0x06000072 RID: 114 RVA: 0x0000272A File Offset: 0x0000092A
		public override object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
