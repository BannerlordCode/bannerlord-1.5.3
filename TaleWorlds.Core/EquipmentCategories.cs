using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200009E RID: 158
	[Flags]
	public enum EquipmentCategories : uint
	{
		// Token: 0x0400050D RID: 1293
		None = 0U,
		// Token: 0x0400050E RID: 1294
		IsFemaleTemplate = 1U,
		// Token: 0x0400050F RID: 1295
		IsLordTemplate = 2U,
		// Token: 0x04000510 RID: 1296
		IsChildEquipmentTemplate = 4U,
		// Token: 0x04000511 RID: 1297
		IsTeenagerEquipmentTemplate = 8U,
		// Token: 0x04000512 RID: 1298
		IsKingdomRulerTemplate = 16U
	}
}
