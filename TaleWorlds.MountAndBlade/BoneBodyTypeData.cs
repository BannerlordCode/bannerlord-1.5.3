using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E0 RID: 480
	[EngineStruct("Bone_body_type_data", false, null)]
	public struct BoneBodyTypeData
	{
		// Token: 0x040009A4 RID: 2468
		[CustomEngineStructMemberData(true)]
		public readonly BoneBodyPartType BodyPartType;

		// Token: 0x040009A5 RID: 2469
		[CustomEngineStructMemberData(true)]
		public readonly sbyte Priority;

		// Token: 0x040009A6 RID: 2470
		[CustomEngineStructMemberData(true)]
		public readonly SkeletonModelBoundsRecFlags DataFlags;
	}
}
