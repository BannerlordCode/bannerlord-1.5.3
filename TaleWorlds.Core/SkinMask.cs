using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000D1 RID: 209
	[Flags]
	public enum SkinMask
	{
		// Token: 0x04000634 RID: 1588
		NoneVisible = 0,
		// Token: 0x04000635 RID: 1589
		HeadVisible = 1,
		// Token: 0x04000636 RID: 1590
		BodyVisible = 32,
		// Token: 0x04000637 RID: 1591
		UnderwearVisible = 64,
		// Token: 0x04000638 RID: 1592
		HandsVisible = 128,
		// Token: 0x04000639 RID: 1593
		LegsVisible = 256,
		// Token: 0x0400063A RID: 1594
		AllVisible = 481
	}
}
