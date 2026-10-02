using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000194 RID: 404
	[EngineStruct("Animation_system_bone_data_biped", false, null)]
	[Serializable]
	public struct AnimationSystemBoneDataBiped
	{
		// Token: 0x040005EB RID: 1515
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public sbyte[] RagdollStationaryCheckBoneIndices;

		// Token: 0x040005EC RID: 1516
		public sbyte RagdollStationaryCheckBoneCount;

		// Token: 0x040005ED RID: 1517
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
		public sbyte[] MoveAdderBoneIndices;

		// Token: 0x040005EE RID: 1518
		public sbyte MoveAdderBoneCount;

		// Token: 0x040005EF RID: 1519
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public sbyte[] SplashDecalBoneIndices;

		// Token: 0x040005F0 RID: 1520
		public sbyte SplashDecalBoneCount;

		// Token: 0x040005F1 RID: 1521
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public sbyte[] BloodBurstBoneIndices;

		// Token: 0x040005F2 RID: 1522
		public sbyte BloodBurstBoneCount;

		// Token: 0x040005F3 RID: 1523
		public sbyte MainHandBoneIndex;

		// Token: 0x040005F4 RID: 1524
		public sbyte OffHandBoneIndex;

		// Token: 0x040005F5 RID: 1525
		public sbyte MainHandItemBoneIndex;

		// Token: 0x040005F6 RID: 1526
		public sbyte OffHandItemBoneIndex;

		// Token: 0x040005F7 RID: 1527
		public sbyte MainHandItemSecondaryBoneIndex;

		// Token: 0x040005F8 RID: 1528
		public sbyte OffHandItemSecondaryBoneIndex;

		// Token: 0x040005F9 RID: 1529
		public sbyte OffHandShoulderBoneIndex;

		// Token: 0x040005FA RID: 1530
		public sbyte HandNumBonesForIk;

		// Token: 0x040005FB RID: 1531
		public sbyte PrimaryFootBoneIndex;

		// Token: 0x040005FC RID: 1532
		public sbyte SecondaryFootBoneIndex;

		// Token: 0x040005FD RID: 1533
		public sbyte RightFootIkEndEffectorBoneIndex;

		// Token: 0x040005FE RID: 1534
		public sbyte LeftFootIkEndEffectorBoneIndex;

		// Token: 0x040005FF RID: 1535
		public sbyte RightFootIkTipBoneIndex;

		// Token: 0x04000600 RID: 1536
		public sbyte LeftFootIkTipBoneIndex;

		// Token: 0x04000601 RID: 1537
		public sbyte FootNumBonesForIk;
	}
}
