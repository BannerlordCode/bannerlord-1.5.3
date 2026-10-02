using System;

namespace psai.Editor
{
	// Token: 0x02000005 RID: 5
	[Serializable]
	public enum CompatibilityType
	{
		// Token: 0x0400001E RID: 30
		undefined,
		// Token: 0x0400001F RID: 31
		allowed_implicitly,
		// Token: 0x04000020 RID: 32
		allowed_manually,
		// Token: 0x04000021 RID: 33
		blocked_implicitly,
		// Token: 0x04000022 RID: 34
		blocked_manually,
		// Token: 0x04000023 RID: 35
		logically_impossible
	}
}
