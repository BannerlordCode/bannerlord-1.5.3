using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000008 RID: 8
	[Flags]
	public enum AgentFlag : uint
	{
		// Token: 0x040000B1 RID: 177
		None = 0U,
		// Token: 0x040000B2 RID: 178
		Mountable = 1U,
		// Token: 0x040000B3 RID: 179
		CanJump = 2U,
		// Token: 0x040000B4 RID: 180
		CanRear = 4U,
		// Token: 0x040000B5 RID: 181
		CanAttack = 8U,
		// Token: 0x040000B6 RID: 182
		CanDefend = 16U,
		// Token: 0x040000B7 RID: 183
		RunsAwayWhenHit = 32U,
		// Token: 0x040000B8 RID: 184
		CanCharge = 64U,
		// Token: 0x040000B9 RID: 185
		CanBeCharged = 128U,
		// Token: 0x040000BA RID: 186
		CanClimbLadders = 256U,
		// Token: 0x040000BB RID: 187
		CanBeInGroup = 512U,
		// Token: 0x040000BC RID: 188
		CanSprint = 1024U,
		// Token: 0x040000BD RID: 189
		IsHumanoid = 2048U,
		// Token: 0x040000BE RID: 190
		CanGetScared = 4096U,
		// Token: 0x040000BF RID: 191
		CanRide = 8192U,
		// Token: 0x040000C0 RID: 192
		CanWieldWeapon = 16384U,
		// Token: 0x040000C1 RID: 193
		CanCrouch = 32768U,
		// Token: 0x040000C2 RID: 194
		CanGetAlarmed = 65536U,
		// Token: 0x040000C3 RID: 195
		CanWander = 131072U,
		// Token: 0x040000C4 RID: 196
		CanKick = 524288U,
		// Token: 0x040000C5 RID: 197
		CanRetreat = 1048576U,
		// Token: 0x040000C6 RID: 198
		MoveAsHerd = 2097152U,
		// Token: 0x040000C7 RID: 199
		MoveForwardOnly = 4194304U,
		// Token: 0x040000C8 RID: 200
		IsUnique = 8388608U,
		// Token: 0x040000C9 RID: 201
		CanUseAllBowsMounted = 16777216U,
		// Token: 0x040000CA RID: 202
		CanReloadAllXBowsMounted = 33554432U,
		// Token: 0x040000CB RID: 203
		CanDeflectArrowsWith2HSword = 67108864U,
		// Token: 0x040000CC RID: 204
		UnreachableViaNavMesh = 134217728U
	}
}
