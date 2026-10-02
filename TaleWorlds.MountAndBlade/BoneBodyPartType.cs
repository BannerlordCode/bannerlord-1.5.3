using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001DE RID: 478
	[EngineStruct("Bone_body_part_type", false, null)]
	public enum BoneBodyPartType : sbyte
	{
		// Token: 0x04000992 RID: 2450
		None = -1,
		// Token: 0x04000993 RID: 2451
		Head,
		// Token: 0x04000994 RID: 2452
		Neck,
		// Token: 0x04000995 RID: 2453
		Chest,
		// Token: 0x04000996 RID: 2454
		Abdomen,
		// Token: 0x04000997 RID: 2455
		ShoulderLeft,
		// Token: 0x04000998 RID: 2456
		ShoulderRight,
		// Token: 0x04000999 RID: 2457
		ArmLeft,
		// Token: 0x0400099A RID: 2458
		ArmRight,
		// Token: 0x0400099B RID: 2459
		Legs,
		// Token: 0x0400099C RID: 2460
		NumOfBodyPartTypes,
		// Token: 0x0400099D RID: 2461
		CriticalBodyPartsBegin = 0,
		// Token: 0x0400099E RID: 2462
		CriticalBodyPartsEnd = 6
	}
}
