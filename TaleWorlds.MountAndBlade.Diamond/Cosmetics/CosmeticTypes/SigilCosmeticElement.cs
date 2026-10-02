using System;

namespace TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes
{
	// Token: 0x0200017F RID: 383
	public class SigilCosmeticElement : CosmeticElement
	{
		// Token: 0x06000AD1 RID: 2769 RVA: 0x000120DD File Offset: 0x000102DD
		public SigilCosmeticElement(string id, CosmeticsManager.CosmeticRarity rarity, int cost, string bannerCode)
			: base(id, rarity, cost, CosmeticsManager.CosmeticType.Sigil)
		{
			this.BannerCode = bannerCode;
		}

		// Token: 0x0400055D RID: 1373
		public string BannerCode;
	}
}
