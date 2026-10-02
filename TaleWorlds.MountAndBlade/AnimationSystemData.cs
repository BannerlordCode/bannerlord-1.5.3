using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000197 RID: 407
	[EngineStruct("Animation_system_data", false, null)]
	[Serializable]
	public struct AnimationSystemData
	{
		// Token: 0x06001576 RID: 5494 RVA: 0x00050038 File Offset: 0x0004E238
		public static AnimationSystemData GetHardcodedAnimationSystemDataForHumanSkeleton()
		{
			MBActionSet actionSetWithIndex = MBActionSet.GetActionSetWithIndex(0);
			return new AnimationSystemData
			{
				ActionSet = actionSetWithIndex,
				MonsterUsageSetIndex = -1,
				WalkingSpeedLimit = 1f,
				CrouchWalkingSpeedLimit = 1f,
				StepSize = 1f,
				HasClippingPlane = false,
				Bones = new AnimationSystemBoneData
				{
					IndicesOfRagdollBonesToCheckForCorpses = new sbyte[]
					{
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "head"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "neck"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_foretwist"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_foretwist"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_upperarm_twist"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_upperarm_twist"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_clavicle"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_clavicle"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine"),
						-1,
						-1
					},
					CountOfRagdollBonesToCheckForCorpses = 9,
					RagdollFallSoundBoneIndices = new sbyte[]
					{
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine2"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_upperarm_twist"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_upperarm_twist"),
						-1
					},
					RagdollFallSoundBoneIndexCount = 3,
					HeadLookDirectionBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "head"),
					SpineLowerBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine"),
					SpineUpperBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine1"),
					ThoraxLookDirectionBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine2"),
					NeckRootBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "neck"),
					PelvisBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "pelvis"),
					RightUpperArmBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_upperarm_twist"),
					LeftUpperArmBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_upperarm_twist"),
					FallBlowDamageBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_calf"),
					TerrainDecalBone0Index = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_foot"),
					TerrainDecalBone1Index = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_foot")
				},
				Biped = new AnimationSystemBoneDataBiped
				{
					RagdollStationaryCheckBoneIndices = new sbyte[]
					{
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_upperarm_twist"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_upperarm_twist"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_thigh"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_thigh"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_calf"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_calf"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "pelvis"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "head")
					},
					RagdollStationaryCheckBoneCount = 8,
					MoveAdderBoneIndices = new sbyte[]
					{
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine1"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine2"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_clavicle"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_clavicle"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "neck"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "head")
					},
					MoveAdderBoneCount = 7,
					SplashDecalBoneIndices = new sbyte[]
					{
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_calf"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_foot"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_toe0"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_calf"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_foot"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_toe0")
					},
					SplashDecalBoneCount = 6,
					BloodBurstBoneIndices = new sbyte[]
					{
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_clavicle"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine1"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_clavicle"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_foretwist"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine1"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_foretwist"),
						Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "spine")
					},
					BloodBurstBoneCount = 8,
					MainHandBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_hand"),
					OffHandBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_hand"),
					MainHandItemBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_finger0"),
					OffHandItemBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_finger0"),
					MainHandItemSecondaryBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_foretwist1"),
					OffHandItemSecondaryBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_foretwist1"),
					OffHandShoulderBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_clavicle"),
					HandNumBonesForIk = 6,
					PrimaryFootBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_foot"),
					SecondaryFootBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_foot"),
					RightFootIkEndEffectorBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_foot"),
					LeftFootIkEndEffectorBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_foot"),
					RightFootIkTipBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "r_toe0"),
					LeftFootIkTipBoneIndex = Skeleton.GetBoneIndexFromName(actionSetWithIndex.GetSkeletonName(), "l_toe0"),
					FootNumBonesForIk = 3
				},
				Quadruped = default(AnimationSystemDataQuadruped)
			};
		}

		// Token: 0x04000615 RID: 1557
		public const sbyte InvalidBoneIndex = -1;

		// Token: 0x04000616 RID: 1558
		public const sbyte NumBonesForIkMaxCount = 8;

		// Token: 0x04000617 RID: 1559
		public const sbyte MaxCountOfRagdollBonesToCheckForCorpses = 11;

		// Token: 0x04000618 RID: 1560
		public const sbyte RagdollFallSoundBoneIndexMaxCount = 4;

		// Token: 0x04000619 RID: 1561
		public const sbyte RagdollStationaryCheckBoneMaxCount = 8;

		// Token: 0x0400061A RID: 1562
		public const sbyte MoveAdderBoneMaxCount = 7;

		// Token: 0x0400061B RID: 1563
		public const sbyte SplashDecalBoneMaxCount = 6;

		// Token: 0x0400061C RID: 1564
		public const sbyte BloodBurstBoneMaxCount = 8;

		// Token: 0x0400061D RID: 1565
		public const sbyte BoneIndicesToModifyOnSlopingGroundMaxCount = 7;

		// Token: 0x0400061E RID: 1566
		[CustomEngineStructMemberData("action_set_no")]
		public MBActionSet ActionSet;

		// Token: 0x0400061F RID: 1567
		public int NumPaces;

		// Token: 0x04000620 RID: 1568
		public int MonsterUsageSetIndex;

		// Token: 0x04000621 RID: 1569
		public float WalkingSpeedLimit;

		// Token: 0x04000622 RID: 1570
		public float CrouchWalkingSpeedLimit;

		// Token: 0x04000623 RID: 1571
		public float StepSize;

		// Token: 0x04000624 RID: 1572
		[MarshalAs(UnmanagedType.U1)]
		public bool HasClippingPlane;

		// Token: 0x04000625 RID: 1573
		public AnimationSystemBoneData Bones;

		// Token: 0x04000626 RID: 1574
		[CustomEngineStructMemberData(true)]
		public AnimationSystemBoneDataBiped Biped;

		// Token: 0x04000627 RID: 1575
		[CustomEngineStructMemberData(true)]
		public AnimationSystemDataQuadruped Quadruped;
	}
}
