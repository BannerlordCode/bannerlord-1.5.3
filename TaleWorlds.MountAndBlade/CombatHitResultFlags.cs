using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000199 RID: 409
	[Flags]
	[EngineStruct("Combat_hit_result_flags", true, "chrf", false)]
	public enum CombatHitResultFlags : byte
	{
		// Token: 0x04000630 RID: 1584
		NormalHit = 0,
		// Token: 0x04000631 RID: 1585
		HitWithStartOfTheAnimation = 1,
		// Token: 0x04000632 RID: 1586
		HitWithArm = 2,
		// Token: 0x04000633 RID: 1587
		HitWithBackOfTheWeapon = 4
	}
}
