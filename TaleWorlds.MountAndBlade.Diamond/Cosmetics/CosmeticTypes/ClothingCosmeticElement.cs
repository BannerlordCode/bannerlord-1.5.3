using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes
{
	// Token: 0x0200017E RID: 382
	public class ClothingCosmeticElement : CosmeticElement
	{
		// Token: 0x06000AD0 RID: 2768 RVA: 0x000120C1 File Offset: 0x000102C1
		public ClothingCosmeticElement(string id, CosmeticsManager.CosmeticRarity rarity, int cost, List<string> replaceItemsId, List<Tuple<string, string>> replaceItemless)
			: base(id, rarity, cost, CosmeticsManager.CosmeticType.Clothing)
		{
			this.ReplaceItemsId = replaceItemsId;
			this.ReplaceItemless = replaceItemless;
		}

		// Token: 0x0400055B RID: 1371
		public readonly List<string> ReplaceItemsId;

		// Token: 0x0400055C RID: 1372
		public readonly List<Tuple<string, string>> ReplaceItemless;
	}
}
