using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x020000A1 RID: 161
	[EngineStruct("rglWorld_position::z_validity_state", true, "zvs", false)]
	public enum ZValidityState
	{
		// Token: 0x0400020E RID: 526
		Invalid,
		// Token: 0x0400020F RID: 527
		BatchFormationUnitPosition,
		// Token: 0x04000210 RID: 528
		ValidAccordingToNavMesh,
		// Token: 0x04000211 RID: 529
		Valid
	}
}
