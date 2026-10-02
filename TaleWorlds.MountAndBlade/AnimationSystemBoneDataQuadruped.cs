using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000195 RID: 405
	[EngineStruct("Animation_system_bone_data_quadruped", false, null)]
	[Serializable]
	public struct AnimationSystemBoneDataQuadruped
	{
		// Token: 0x04000602 RID: 1538
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
		public sbyte[] BoneIndicesToModifyOnSlopingGround;

		// Token: 0x04000603 RID: 1539
		public sbyte BoneIndicesToModifyOnSlopingGroundCount;

		// Token: 0x04000604 RID: 1540
		public sbyte BodyRotationReferenceBoneIndex;

		// Token: 0x04000605 RID: 1541
		public sbyte RiderSitBoneIndex;

		// Token: 0x04000606 RID: 1542
		public sbyte ReinHandleBoneIndex;

		// Token: 0x04000607 RID: 1543
		[CustomEngineStructMemberData("rein_collision_1_bone_index")]
		public sbyte ReinCollision1BoneIndex;

		// Token: 0x04000608 RID: 1544
		[CustomEngineStructMemberData("rein_collision_2_bone_index")]
		public sbyte ReinCollision2BoneIndex;

		// Token: 0x04000609 RID: 1545
		public sbyte ReinHeadBoneIndex;

		// Token: 0x0400060A RID: 1546
		public sbyte ReinHeadRightAttachmentBoneIndex;

		// Token: 0x0400060B RID: 1547
		public sbyte ReinHeadLeftAttachmentBoneIndex;

		// Token: 0x0400060C RID: 1548
		public sbyte ReinRightHandBoneIndex;

		// Token: 0x0400060D RID: 1549
		public sbyte ReinLeftHandBoneIndex;
	}
}
