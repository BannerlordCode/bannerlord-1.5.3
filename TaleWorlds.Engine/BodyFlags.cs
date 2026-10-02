using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200004A RID: 74
	[Flags]
	[EngineStruct("rglBody_flags", true, "rgl_bf", false)]
	public enum BodyFlags : uint
	{
		// Token: 0x0400007D RID: 125
		None = 0U,
		// Token: 0x0400007E RID: 126
		Disabled = 1U,
		// Token: 0x0400007F RID: 127
		NotDestructible = 2U,
		// Token: 0x04000080 RID: 128
		TwoSided = 4U,
		// Token: 0x04000081 RID: 129
		Dynamic = 8U,
		// Token: 0x04000082 RID: 130
		Moveable = 16U,
		// Token: 0x04000083 RID: 131
		DynamicConvexHull = 32U,
		// Token: 0x04000084 RID: 132
		Ladder = 64U,
		// Token: 0x04000085 RID: 133
		OnlyCollideWithRaycast = 128U,
		// Token: 0x04000086 RID: 134
		[CustomEngineStructMemberData("ai_limiter")]
		AILimiter = 256U,
		// Token: 0x04000087 RID: 135
		Barrier = 512U,
		// Token: 0x04000088 RID: 136
		Barrier3D = 1024U,
		// Token: 0x04000089 RID: 137
		HasSteps = 2048U,
		// Token: 0x0400008A RID: 138
		Ragdoll = 4096U,
		// Token: 0x0400008B RID: 139
		RagdollLimiter = 8192U,
		// Token: 0x0400008C RID: 140
		DestructibleDoor = 16384U,
		// Token: 0x0400008D RID: 141
		DroppedItem = 32768U,
		// Token: 0x0400008E RID: 142
		DoNotCollideWithRaycast = 65536U,
		// Token: 0x0400008F RID: 143
		DontTransferToPhysicsEngine = 131072U,
		// Token: 0x04000090 RID: 144
		DontCollideWithCamera = 262144U,
		// Token: 0x04000091 RID: 145
		ExcludePathSnap = 524288U,
		// Token: 0x04000092 RID: 146
		WaterBody = 1048576U,
		// Token: 0x04000093 RID: 147
		AfterAddFlags = 0U,
		// Token: 0x04000094 RID: 148
		AgentOnly = 2097152U,
		// Token: 0x04000095 RID: 149
		MissileOnly = 4194304U,
		// Token: 0x04000096 RID: 150
		HasMaterial = 8388608U,
		// Token: 0x04000097 RID: 151
		IgnoreSoundOcclusion = 268435456U,
		// Token: 0x04000098 RID: 152
		StealthBox = 536870912U,
		// Token: 0x04000099 RID: 153
		Sinking = 1073741824U,
		// Token: 0x0400009A RID: 154
		FloatingDebris = 2147483648U,
		// Token: 0x0400009B RID: 155
		BodyFlagFilter = 4043309055U,
		// Token: 0x0400009C RID: 156
		BodyOwnerNone = 0U,
		// Token: 0x0400009D RID: 157
		BodyOwnerEntity = 16777216U,
		// Token: 0x0400009E RID: 158
		BodyOwnerTerrain = 33554432U,
		// Token: 0x0400009F RID: 159
		BodyOwnerFlora = 67108864U,
		// Token: 0x040000A0 RID: 160
		BodyOwnerFilter = 251658240U,
		// Token: 0x040000A1 RID: 161
		CommonCollisionExcludeFlags = 544321929U,
		// Token: 0x040000A2 RID: 162
		CameraCollisionRayCastExludeFlags = 544323529U,
		// Token: 0x040000A3 RID: 163
		CommonCollisionExcludeFlagsForAgent = 542224777U,
		// Token: 0x040000A4 RID: 164
		CommonCollisionExcludeFlagsForMissile = 540129161U,
		// Token: 0x040000A5 RID: 165
		CommonCollisionExcludeFlagsForCombat = 540127625U,
		// Token: 0x040000A6 RID: 166
		CommonCollisionExcludeFlagsForEditor = 540127625U,
		// Token: 0x040000A7 RID: 167
		CommonFlagsThatDoNotBlockRay = 4043259711U,
		// Token: 0x040000A8 RID: 168
		CommonFocusRayCastExcludeFlags = 79617U
	}
}
