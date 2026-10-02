using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001EE RID: 494
	[Flags]
	[EngineStruct("Blow_flags", true, "bf", false)]
	public enum BlowFlags
	{
		// Token: 0x040009C4 RID: 2500
		None = 0,
		// Token: 0x040009C5 RID: 2501
		KnockBack = 16,
		// Token: 0x040009C6 RID: 2502
		KnockDown = 32,
		// Token: 0x040009C7 RID: 2503
		NoSound = 64,
		// Token: 0x040009C8 RID: 2504
		CrushThrough = 128,
		// Token: 0x040009C9 RID: 2505
		ShrugOff = 256,
		// Token: 0x040009CA RID: 2506
		MakesRear = 512,
		// Token: 0x040009CB RID: 2507
		NonTipThrust = 1024,
		// Token: 0x040009CC RID: 2508
		CanDismount = 2048
	}
}
