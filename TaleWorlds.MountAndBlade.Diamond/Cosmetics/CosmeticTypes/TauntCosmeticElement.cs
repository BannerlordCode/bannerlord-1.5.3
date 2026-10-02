using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes
{
	// Token: 0x02000180 RID: 384
	public class TauntCosmeticElement : CosmeticElement
	{
		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x000120F1 File Offset: 0x000102F1
		public static int MaxNumberOfTaunts
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x000120F4 File Offset: 0x000102F4
		public TextObject Name { get; }

		// Token: 0x06000AD4 RID: 2772 RVA: 0x000120FC File Offset: 0x000102FC
		public TauntCosmeticElement(int index, string id, CosmeticsManager.CosmeticRarity rarity, int cost, string name)
			: base(id, rarity, cost, CosmeticsManager.CosmeticType.Taunt)
		{
			this.UsageIndex = index;
			this.Name = new TextObject(name, null);
		}
	}
}
