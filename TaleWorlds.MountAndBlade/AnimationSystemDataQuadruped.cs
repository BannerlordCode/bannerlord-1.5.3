using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000196 RID: 406
	[EngineStruct("Animation_system_data_quadruped", false, null)]
	[Serializable]
	public struct AnimationSystemDataQuadruped
	{
		// Token: 0x0400060E RID: 1550
		public Vec3 ReinHandleLeftLocalPosition;

		// Token: 0x0400060F RID: 1551
		public Vec3 ReinHandleRightLocalPosition;

		// Token: 0x04000610 RID: 1552
		public string ReinSkeleton;

		// Token: 0x04000611 RID: 1553
		public string ReinCollisionBody;

		// Token: 0x04000612 RID: 1554
		public sbyte IndexOfBoneToDetectGroundSlopeFront;

		// Token: 0x04000613 RID: 1555
		public sbyte IndexOfBoneToDetectGroundSlopeBack;

		// Token: 0x04000614 RID: 1556
		public AnimationSystemBoneDataQuadruped Bones;
	}
}
