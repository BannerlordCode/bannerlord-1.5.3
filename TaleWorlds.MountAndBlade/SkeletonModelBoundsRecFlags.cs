using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001DF RID: 479
	[EngineStruct("Skeleton_model_bounds_rec_flags", true, "smbrf", false)]
	public enum SkeletonModelBoundsRecFlags : sbyte
	{
		// Token: 0x040009A0 RID: 2464
		None,
		// Token: 0x040009A1 RID: 2465
		UseSmallerRadiusMultWhileHoldingShield,
		// Token: 0x040009A2 RID: 2466
		Sweep,
		// Token: 0x040009A3 RID: 2467
		DoNotScaleAccordingToAgentScale = 4
	}
}
