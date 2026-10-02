using System;

namespace TaleWorlds.MountAndBlade.Diamond.Cosmetics
{
	// Token: 0x0200017C RID: 380
	public class CosmeticElement
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x00011AEB File Offset: 0x0000FCEB
		public bool IsFree
		{
			get
			{
				return this.Cost <= 0;
			}
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00011AF9 File Offset: 0x0000FCF9
		public CosmeticElement(string id, CosmeticsManager.CosmeticRarity rarity, int cost, CosmeticsManager.CosmeticType type)
		{
			this.UsageIndex = -1;
			this.Id = id;
			this.Rarity = rarity;
			this.Cost = cost;
			this.Type = type;
		}

		// Token: 0x04000554 RID: 1364
		public int UsageIndex;

		// Token: 0x04000555 RID: 1365
		public string Id;

		// Token: 0x04000556 RID: 1366
		public CosmeticsManager.CosmeticRarity Rarity;

		// Token: 0x04000557 RID: 1367
		public int Cost;

		// Token: 0x04000558 RID: 1368
		public CosmeticsManager.CosmeticType Type;
	}
}
