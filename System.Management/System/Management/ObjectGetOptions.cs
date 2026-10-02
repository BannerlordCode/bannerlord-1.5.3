using System;

namespace System.Management
{
	// Token: 0x02000025 RID: 37
	public class ObjectGetOptions : ManagementOptions
	{
		// Token: 0x0600019A RID: 410 RVA: 0x0000360D File Offset: 0x0000180D
		public ObjectGetOptions()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000361F File Offset: 0x0000181F
		public ObjectGetOptions(ManagementNamedValueCollection context)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00003631 File Offset: 0x00001831
		public ObjectGetOptions(ManagementNamedValueCollection context, TimeSpan timeout, bool useAmendedQualifiers)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00003643 File Offset: 0x00001843
		// (set) Token: 0x0600019E RID: 414 RVA: 0x0000364F File Offset: 0x0000184F
		public bool UseAmendedQualifiers
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

		// Token: 0x0600019F RID: 415 RVA: 0x0000365B File Offset: 0x0000185B
		public override object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
