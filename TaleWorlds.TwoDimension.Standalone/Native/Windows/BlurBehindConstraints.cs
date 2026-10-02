using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000037 RID: 55
	[Flags]
	public enum BlurBehindConstraints : uint
	{
		// Token: 0x04000154 RID: 340
		Enable = 1U,
		// Token: 0x04000155 RID: 341
		BlurRegion = 2U,
		// Token: 0x04000156 RID: 342
		TransitionOnMaximized = 4U
	}
}
