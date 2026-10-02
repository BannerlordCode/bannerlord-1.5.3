using System;

namespace System.Management
{
	// Token: 0x0200002F RID: 47
	public class PutOptions : ManagementOptions
	{
		// Token: 0x060001CE RID: 462 RVA: 0x000037FD File Offset: 0x000019FD
		public PutOptions()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000380F File Offset: 0x00001A0F
		public PutOptions(ManagementNamedValueCollection context)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00003821 File Offset: 0x00001A21
		public PutOptions(ManagementNamedValueCollection context, TimeSpan timeout, bool useAmendedQualifiers, PutType putType)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00003833 File Offset: 0x00001A33
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000383F File Offset: 0x00001A3F
		public PutType Type
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000384B File Offset: 0x00001A4B
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00003857 File Offset: 0x00001A57
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

		// Token: 0x060001D5 RID: 469 RVA: 0x00003863 File Offset: 0x00001A63
		public override object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
