using System;

namespace System.Management
{
	// Token: 0x0200003A RID: 58
	public class WqlObjectQuery : ObjectQuery
	{
		// Token: 0x0600024F RID: 591 RVA: 0x00003E69 File Offset: 0x00002069
		public WqlObjectQuery()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00003E7B File Offset: 0x0000207B
		public WqlObjectQuery(string query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00003E8D File Offset: 0x0000208D
		public override string QueryLanguage
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00003E99 File Offset: 0x00002099
		public override object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
