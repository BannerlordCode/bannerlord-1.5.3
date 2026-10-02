using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A9 RID: 425
	public abstract class SmithingModel : MBGameModel<SmithingModel>
	{
		// Token: 0x06001D47 RID: 7495
		public abstract int GetCraftingPartDifficulty(CraftingPiece craftingPiece);

		// Token: 0x06001D48 RID: 7496
		public abstract int CalculateWeaponDesignDifficulty(WeaponDesign weaponDesign);

		// Token: 0x06001D49 RID: 7497
		public abstract ItemModifier GetCraftedWeaponModifier(WeaponDesign weaponDesign, Hero weaponsmith);

		// Token: 0x06001D4A RID: 7498
		public abstract IEnumerable<Crafting.RefiningFormula> GetRefiningFormulas(Hero weaponsmith);

		// Token: 0x06001D4B RID: 7499
		public abstract ItemObject GetCraftingMaterialItem(CraftingMaterials craftingMaterial);

		// Token: 0x06001D4C RID: 7500
		public abstract int[] GetSmeltingOutputForItem(ItemObject item);

		// Token: 0x06001D4D RID: 7501
		public abstract int GetSkillXpForRefining(ref Crafting.RefiningFormula refineFormula);

		// Token: 0x06001D4E RID: 7502
		public abstract int GetSkillXpForSmelting(ItemObject item);

		// Token: 0x06001D4F RID: 7503
		public abstract int GetSkillXpForSmithingInFreeBuildMode(ItemObject item);

		// Token: 0x06001D50 RID: 7504
		public abstract int GetSkillXpForSmithingInCraftingOrderMode(ItemObject item);

		// Token: 0x06001D51 RID: 7505
		public abstract int[] GetSmithingCostsForWeaponDesign(WeaponDesign weaponDesign);

		// Token: 0x06001D52 RID: 7506
		public abstract int GetEnergyCostForRefining(ref Crafting.RefiningFormula refineFormula, Hero hero);

		// Token: 0x06001D53 RID: 7507
		public abstract int GetEnergyCostForSmithing(ItemObject item, Hero hero);

		// Token: 0x06001D54 RID: 7508
		public abstract int GetEnergyCostForSmelting(ItemObject item, Hero hero);

		// Token: 0x06001D55 RID: 7509
		public abstract float ResearchPointsNeedForNewPart(int totalPartCount, int openedPartCount);

		// Token: 0x06001D56 RID: 7510
		public abstract int GetPartResearchGainForSmeltingItem(ItemObject item, Hero hero);

		// Token: 0x06001D57 RID: 7511
		public abstract int GetPartResearchGainForSmithingItem(ItemObject item, Hero hero, bool isFreeBuildMode);
	}
}
