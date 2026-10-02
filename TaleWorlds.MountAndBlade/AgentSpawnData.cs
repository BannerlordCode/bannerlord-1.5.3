using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000109 RID: 265
	[EngineStruct("Agent_spawn_data", false, null)]
	public struct AgentSpawnData
	{
		// Token: 0x040002E5 RID: 741
		public int HitPoints;

		// Token: 0x040002E6 RID: 742
		public int MonsterUsageIndex;

		// Token: 0x040002E7 RID: 743
		public int Weight;

		// Token: 0x040002E8 RID: 744
		public float StandingChestHeight;

		// Token: 0x040002E9 RID: 745
		public float StandingPelvisHeight;

		// Token: 0x040002EA RID: 746
		public float StandingEyeHeight;

		// Token: 0x040002EB RID: 747
		public float CrouchEyeHeight;

		// Token: 0x040002EC RID: 748
		public float MountedEyeHeight;

		// Token: 0x040002ED RID: 749
		public float RiderEyeHeightAdder;

		// Token: 0x040002EE RID: 750
		public float JumpAcceleration;

		// Token: 0x040002EF RID: 751
		public Vec3 EyeOffsetWrtHead;

		// Token: 0x040002F0 RID: 752
		public Vec3 FirstPersonCameraOffsetWrtHead;

		// Token: 0x040002F1 RID: 753
		public float RiderCameraHeightAdder;

		// Token: 0x040002F2 RID: 754
		public float RiderBodyCapsuleHeightAdder;

		// Token: 0x040002F3 RID: 755
		public float RiderBodyCapsuleForwardAdder;

		// Token: 0x040002F4 RID: 756
		public float ArmLength;

		// Token: 0x040002F5 RID: 757
		public float ArmWeight;

		// Token: 0x040002F6 RID: 758
		public float JumpSpeedLimit;

		// Token: 0x040002F7 RID: 759
		public float RelativeSpeedLimitForCharge;
	}
}
