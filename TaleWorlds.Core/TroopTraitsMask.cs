using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000D8 RID: 216
	[Flags]
	public enum TroopTraitsMask : ushort
	{
		// Token: 0x04000662 RID: 1634
		None = 0,
		// Token: 0x04000663 RID: 1635
		Melee = 1,
		// Token: 0x04000664 RID: 1636
		Ranged = 2,
		// Token: 0x04000665 RID: 1637
		Mount = 4,
		// Token: 0x04000666 RID: 1638
		Armor = 8,
		// Token: 0x04000667 RID: 1639
		Thrown = 16,
		// Token: 0x04000668 RID: 1640
		Spear = 32,
		// Token: 0x04000669 RID: 1641
		Shield = 64,
		// Token: 0x0400066A RID: 1642
		LowTier = 128,
		// Token: 0x0400066B RID: 1643
		HighTier = 256,
		// Token: 0x0400066C RID: 1644
		All = 511
	}
}
