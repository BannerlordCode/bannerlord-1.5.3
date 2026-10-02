using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000193 RID: 403
	[EngineStruct("Animation_system_bone_data", false, null)]
	[Serializable]
	public struct AnimationSystemBoneData
	{
		// Token: 0x040005DC RID: 1500
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
		public sbyte[] IndicesOfRagdollBonesToCheckForCorpses;

		// Token: 0x040005DD RID: 1501
		public sbyte CountOfRagdollBonesToCheckForCorpses;

		// Token: 0x040005DE RID: 1502
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public sbyte[] RagdollFallSoundBoneIndices;

		// Token: 0x040005DF RID: 1503
		public sbyte RagdollFallSoundBoneIndexCount;

		// Token: 0x040005E0 RID: 1504
		public sbyte HeadLookDirectionBoneIndex;

		// Token: 0x040005E1 RID: 1505
		public sbyte SpineLowerBoneIndex;

		// Token: 0x040005E2 RID: 1506
		public sbyte SpineUpperBoneIndex;

		// Token: 0x040005E3 RID: 1507
		public sbyte ThoraxLookDirectionBoneIndex;

		// Token: 0x040005E4 RID: 1508
		public sbyte NeckRootBoneIndex;

		// Token: 0x040005E5 RID: 1509
		public sbyte PelvisBoneIndex;

		// Token: 0x040005E6 RID: 1510
		public sbyte RightUpperArmBoneIndex;

		// Token: 0x040005E7 RID: 1511
		public sbyte LeftUpperArmBoneIndex;

		// Token: 0x040005E8 RID: 1512
		public sbyte FallBlowDamageBoneIndex;

		// Token: 0x040005E9 RID: 1513
		[CustomEngineStructMemberData("terrain_decal_bone_0_index")]
		public sbyte TerrainDecalBone0Index;

		// Token: 0x040005EA RID: 1514
		[CustomEngineStructMemberData("terrain_decal_bone_1_index")]
		public sbyte TerrainDecalBone1Index;
	}
}
