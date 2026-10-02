using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200041C RID: 1052
	public interface ICraftingCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06004322 RID: 17186
		IReadOnlyDictionary<Town, CraftingCampaignBehavior.CraftingOrderSlots> CraftingOrders { get; }

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x06004323 RID: 17187
		IReadOnlyCollection<WeaponDesign> CraftingHistory { get; }

		// Token: 0x06004324 RID: 17188
		void CompleteOrder(Town town, CraftingOrder craftingOrder, ItemObject craftedItem, Hero completerHero);

		// Token: 0x06004325 RID: 17189
		ItemModifier GetCurrentItemModifier();

		// Token: 0x06004326 RID: 17190
		void SetCurrentItemModifier(ItemModifier modifier);

		// Token: 0x06004327 RID: 17191
		void SetCraftedWeaponName(ItemObject craftedWeaponItem, TextObject name);

		// Token: 0x06004328 RID: 17192
		void GetOrderResult(CraftingOrder craftingOrder, ItemObject craftedItem, out bool isSucceed, out TextObject orderRemark, out TextObject orderResult, out int finalPrice);

		// Token: 0x06004329 RID: 17193
		int GetCraftingDifficulty(WeaponDesign weaponDesign);

		// Token: 0x0600432A RID: 17194
		int GetHeroCraftingStamina(Hero hero);

		// Token: 0x0600432B RID: 17195
		void SetHeroCraftingStamina(Hero hero, int value);

		// Token: 0x0600432C RID: 17196
		int GetMaxHeroCraftingStamina(Hero hero);

		// Token: 0x0600432D RID: 17197
		void DoRefinement(Hero hero, Crafting.RefiningFormula refineFormula);

		// Token: 0x0600432E RID: 17198
		void DoSmelting(Hero currentCraftingHero, EquipmentElement equipmentElement);

		// Token: 0x0600432F RID: 17199
		ItemObject CreateCraftedWeaponInFreeBuildMode(Hero hero, WeaponDesign weaponDesign, ItemModifier weaponModifier = null);

		// Token: 0x06004330 RID: 17200
		ItemObject CreateCraftedWeaponInCraftingOrderMode(Hero crafterHero, CraftingOrder craftingOrder, WeaponDesign weaponDesign);

		// Token: 0x06004331 RID: 17201
		bool IsOpened(CraftingPiece craftingPiece, CraftingTemplate craftingTemplate);

		// Token: 0x06004332 RID: 17202
		CraftingOrder CreateCustomOrderForHero(Hero orderOwner, float orderDifficulty = -1f, WeaponDesign weaponDesign = null, CraftingTemplate craftingTemplate = null);

		// Token: 0x06004333 RID: 17203
		void CancelCustomOrder(Town town, CraftingOrder craftingOrder);

		// Token: 0x06004334 RID: 17204
		Hero GetActiveCraftingHero();

		// Token: 0x06004335 RID: 17205
		void SetActiveCraftingHero(Hero hero);
	}
}
