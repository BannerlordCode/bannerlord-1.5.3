using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000105 RID: 261
	[EngineStruct("Agent_movement_locked_state", true, "amls", false)]
	public enum AgentMovementLockedState
	{
		// Token: 0x040002D2 RID: 722
		None,
		// Token: 0x040002D3 RID: 723
		PositionLocked,
		// Token: 0x040002D4 RID: 724
		FrameLocked
	}
}
