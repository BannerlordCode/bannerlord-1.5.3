using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000122 RID: 290
	[Flags]
	internal enum InventoryItemType
	{
		// Token: 0x040002CA RID: 714
		None = 0,
		// Token: 0x040002CB RID: 715
		Weapon = 1,
		// Token: 0x040002CC RID: 716
		Shield = 2,
		// Token: 0x040002CD RID: 717
		HeadArmor = 4,
		// Token: 0x040002CE RID: 718
		BodyArmor = 8,
		// Token: 0x040002CF RID: 719
		LegArmor = 16,
		// Token: 0x040002D0 RID: 720
		HandArmor = 32,
		// Token: 0x040002D1 RID: 721
		Horse = 64,
		// Token: 0x040002D2 RID: 722
		HorseHarness = 128,
		// Token: 0x040002D3 RID: 723
		Goods = 256,
		// Token: 0x040002D4 RID: 724
		Book = 512,
		// Token: 0x040002D5 RID: 725
		Animal = 1024,
		// Token: 0x040002D6 RID: 726
		Cape = 2048,
		// Token: 0x040002D7 RID: 727
		HorseCategory = 192,
		// Token: 0x040002D8 RID: 728
		Armors = 2108,
		// Token: 0x040002D9 RID: 729
		Equipable = 2303,
		// Token: 0x040002DA RID: 730
		All = 4095
	}
}
