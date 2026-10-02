using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200024E RID: 590
	[Flags]
	public enum GoldGainFlags : ushort
	{
		// Token: 0x04000D0F RID: 3343
		FirstRangedKill = 1,
		// Token: 0x04000D10 RID: 3344
		FirstMeleeKill = 2,
		// Token: 0x04000D11 RID: 3345
		FirstAssist = 4,
		// Token: 0x04000D12 RID: 3346
		SecondAssist = 8,
		// Token: 0x04000D13 RID: 3347
		ThirdAssist = 16,
		// Token: 0x04000D14 RID: 3348
		FifthKill = 32,
		// Token: 0x04000D15 RID: 3349
		TenthKill = 64,
		// Token: 0x04000D16 RID: 3350
		DefaultKill = 128,
		// Token: 0x04000D17 RID: 3351
		DefaultAssist = 256,
		// Token: 0x04000D18 RID: 3352
		ObjectiveCompleted = 512,
		// Token: 0x04000D19 RID: 3353
		ObjectiveDestroyed = 1024,
		// Token: 0x04000D1A RID: 3354
		PerkBonus = 2048
	}
}
