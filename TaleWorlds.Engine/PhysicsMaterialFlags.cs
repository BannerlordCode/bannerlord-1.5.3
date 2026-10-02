using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000078 RID: 120
	[Flags]
	[EngineStruct("rglPhysics_material::rglPhymat_flags", true, "rgl_phymat", false)]
	public enum PhysicsMaterialFlags : byte
	{
		// Token: 0x04000167 RID: 359
		None = 0,
		// Token: 0x04000168 RID: 360
		DontStickMissiles = 1,
		// Token: 0x04000169 RID: 361
		Flammable = 2,
		// Token: 0x0400016A RID: 362
		RainSplashesEnabled = 4,
		// Token: 0x0400016B RID: 363
		AttacksCanPassThrough = 8
	}
}
