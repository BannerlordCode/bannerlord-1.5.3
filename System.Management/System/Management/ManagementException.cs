using System;
using System.Runtime.Serialization;

namespace System.Management
{
	// Token: 0x02000018 RID: 24
	public class ManagementException : SystemException
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00002D5E File Offset: 0x00000F5E
		public ManagementException()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00002D70 File Offset: 0x00000F70
		protected ManagementException(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00002D82 File Offset: 0x00000F82
		public ManagementException(string message)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00002D94 File Offset: 0x00000F94
		public ManagementException(string message, Exception innerException)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00002DA6 File Offset: 0x00000FA6
		public ManagementStatus ErrorCode
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00002DB2 File Offset: 0x00000FB2
		public ManagementBaseObject ErrorInformation
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00002DBE File Offset: 0x00000FBE
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
