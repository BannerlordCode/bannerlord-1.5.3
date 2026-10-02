using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200037E RID: 894
	[Flags]
	public enum TargetFlags
	{
		// Token: 0x040015BF RID: 5567
		None = 0,
		// Token: 0x040015C0 RID: 5568
		IsMoving = 1,
		// Token: 0x040015C1 RID: 5569
		IsFlammable = 2,
		// Token: 0x040015C2 RID: 5570
		IsStructure = 4,
		// Token: 0x040015C3 RID: 5571
		IsSiegeEngine = 8,
		// Token: 0x040015C4 RID: 5572
		IsAttacker = 16,
		// Token: 0x040015C5 RID: 5573
		IsSmall = 32,
		// Token: 0x040015C6 RID: 5574
		NotAThreat = 64,
		// Token: 0x040015C7 RID: 5575
		DebugThreat = 128,
		// Token: 0x040015C8 RID: 5576
		IsSiegeTower = 256,
		// Token: 0x040015C9 RID: 5577
		IsShip = 512
	}
}
