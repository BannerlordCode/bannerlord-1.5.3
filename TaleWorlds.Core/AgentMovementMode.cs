using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000009 RID: 9
	[Flags]
	public enum AgentMovementMode : byte
	{
		// Token: 0x040000CE RID: 206
		None = 0,
		// Token: 0x040000CF RID: 207
		Land = 1,
		// Token: 0x040000D0 RID: 208
		WaterSurface = 2,
		// Token: 0x040000D1 RID: 209
		WaterDiving = 3,
		// Token: 0x040000D2 RID: 210
		PhysicsCheck = 4,
		// Token: 0x040000D3 RID: 211
		NoPhysics = 8,
		// Token: 0x040000D4 RID: 212
		MovementModeMask = 3
	}
}
