using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200008C RID: 140
	[EngineStruct("rglRagdoll::Ragdoll_state", true, "rds", false)]
	public enum RagdollState : ushort
	{
		// Token: 0x040001C1 RID: 449
		Disabled,
		// Token: 0x040001C2 RID: 450
		NeedsActivation,
		// Token: 0x040001C3 RID: 451
		ActiveFirstTick,
		// Token: 0x040001C4 RID: 452
		Active,
		// Token: 0x040001C5 RID: 453
		NeedsDeactivation
	}
}
