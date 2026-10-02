using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000198 RID: 408
	[EngineStruct("Melee_collision_reaction", true, "mcr", false)]
	public enum MeleeCollisionReaction
	{
		// Token: 0x04000629 RID: 1577
		Invalid = -1,
		// Token: 0x0400062A RID: 1578
		SlicedThrough,
		// Token: 0x0400062B RID: 1579
		ContinueChecking,
		// Token: 0x0400062C RID: 1580
		Stuck,
		// Token: 0x0400062D RID: 1581
		Bounced,
		// Token: 0x0400062E RID: 1582
		Staggered
	}
}
