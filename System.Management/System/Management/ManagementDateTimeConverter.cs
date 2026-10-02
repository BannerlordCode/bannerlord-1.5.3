using System;

namespace System.Management
{
	// Token: 0x02000015 RID: 21
	public sealed class ManagementDateTimeConverter
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x00002C20 File Offset: 0x00000E20
		internal ManagementDateTimeConverter()
		{
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00002C28 File Offset: 0x00000E28
		public static DateTime ToDateTime(string dmtfDate)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00002C34 File Offset: 0x00000E34
		public static string ToDmtfDateTime(DateTime date)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00002C40 File Offset: 0x00000E40
		public static string ToDmtfTimeInterval(TimeSpan timespan)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00002C4C File Offset: 0x00000E4C
		public static TimeSpan ToTimeSpan(string dmtfTimespan)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
