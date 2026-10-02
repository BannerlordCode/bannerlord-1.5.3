using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000065 RID: 101
	[EngineStruct("rglCull_mode", true, "rgl_cull", false)]
	public enum MBMeshCullingMode : byte
	{
		// Token: 0x04000117 RID: 279
		None,
		// Token: 0x04000118 RID: 280
		Backfaces,
		// Token: 0x04000119 RID: 281
		Frontfaces,
		// Token: 0x0400011A RID: 282
		Count,
		// Token: 0x0400011B RID: 283
		Invalid
	}
}
