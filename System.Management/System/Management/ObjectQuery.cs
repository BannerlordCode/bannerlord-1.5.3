using System;

namespace System.Management
{
	// Token: 0x02000028 RID: 40
	public class ObjectQuery : ManagementQuery
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x0000367B File Offset: 0x0000187B
		public ObjectQuery()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000368D File Offset: 0x0000188D
		public ObjectQuery(string query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000369F File Offset: 0x0000189F
		public ObjectQuery(string language, string query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000036B1 File Offset: 0x000018B1
		public override object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
