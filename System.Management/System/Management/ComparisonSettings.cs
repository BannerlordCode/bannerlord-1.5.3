using System;

namespace System.Management
{
	// Token: 0x02000007 RID: 7
	[Flags]
	public enum ComparisonSettings
	{
		// Token: 0x04000024 RID: 36
		IncludeAll = 0,
		// Token: 0x04000025 RID: 37
		IgnoreQualifiers = 1,
		// Token: 0x04000026 RID: 38
		IgnoreObjectSource = 2,
		// Token: 0x04000027 RID: 39
		IgnoreDefaultValues = 4,
		// Token: 0x04000028 RID: 40
		IgnoreClass = 8,
		// Token: 0x04000029 RID: 41
		IgnoreCase = 16,
		// Token: 0x0400002A RID: 42
		IgnoreFlavor = 32
	}
}
