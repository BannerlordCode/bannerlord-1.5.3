using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000097 RID: 151
	public class ItemMenuVM : ViewModel
	{
		// Token: 0x06000CAD RID: 3245 RVA: 0x00033A14 File Offset: 0x00031C14
		public ItemMenuVM(Action<ItemVM, int> resetComparedItems, InventoryLogic inventoryLogic, Func<WeaponComponentData, ItemObject.ItemUsageSetFlags> getItemUsageSetFlags, Func<EquipmentIndex, SPItemVM> getEquipmentAtIndex)
		{
			this._resetComparedItems = resetComparedItems;
			this._inventoryLogic = inventoryLogic;
			this._comparedItemProperties = new MBBindingList<ItemMenuTooltipPropertyVM>();
			this._targetItemProperties = new MBBindingList<ItemMenuTooltipPropertyVM>();
			this._getItemUsageSetFlags = getItemUsageSetFlags;
			this._getEquipmentAtIndex = getEquipmentAtIndex;
			this.TargetItemFlagList = new MBBindingList<ItemFlagVM>();
			this.ComparedItemFlagList = new MBBindingList<ItemFlagVM>();
			this.AlternativeUsages = new MBBindingList<StringItemWithHintVM>();
			this._tradeRumorsBehavior = Campaign.Current.GetCampaignBehavior<ITradeRumorCampaignBehavior>();
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00033D28 File Offset: 0x00031F28
		public void SetItem(SPItemVM item, InventoryLogic.InventorySide currentEquipmentMode, ItemVM comparedItem = null, BasicCharacterObject character = null, int alternativeUsageIndex = 0)
		{
			this.IsInitializationOver = false;
			this._character = character;
			bool flag = item != this._targetItem;
			bool flag2;
			if (comparedItem == this._comparedItem)
			{
				int lastComparedItemVersion = this._lastComparedItemVersion;
				ItemVM comparedItem2 = this._comparedItem;
				int? num = ((comparedItem2 != null) ? new int?(comparedItem2.Version) : null);
				flag2 = !((lastComparedItemVersion == num.GetValueOrDefault()) & (num != null));
			}
			else
			{
				flag2 = true;
			}
			bool flag3 = flag2;
			if (flag)
			{
				this._targetItem = item;
				this.IsPlayerItem = item.InventorySide == InventoryLogic.InventorySide.PlayerInventory;
				this.ImageIdentifier = item.ImageIdentifier;
				this.ItemName = item.ItemDescription;
				this.AlternativeUsages.Clear();
			}
			if (flag3)
			{
				this._comparedItem = comparedItem;
				ItemVM comparedItem3 = this._comparedItem;
				this.IsComparing = ((comparedItem3 != null) ? comparedItem3.ItemRosterElement.EquipmentElement.Item : null) != null;
				this.IsStealthModeActive = currentEquipmentMode == InventoryLogic.InventorySide.StealthEquipment;
				ItemVM comparedItem4 = this._comparedItem;
				this.ComparedImageIdentifier = ((comparedItem4 != null) ? comparedItem4.ImageIdentifier : null);
				ItemVM comparedItem5 = this._comparedItem;
				this.ComparedItemName = ((comparedItem5 != null) ? comparedItem5.ItemRosterElement.EquipmentElement.GetModifiedItemName().ToString() : null);
				ItemVM comparedItem6 = this._comparedItem;
				this._lastComparedItemVersion = ((comparedItem6 != null) ? comparedItem6.Version : 0);
			}
			this.RefreshItemTooltips(item, comparedItem, alternativeUsageIndex);
			this.IsInitializationOver = true;
			Game game = Game.Current;
			if (game == null)
			{
				return;
			}
			game.EventManager.TriggerEvent<InventoryItemInspectedEvent>(new InventoryItemInspectedEvent(item.ItemRosterElement, item.InventorySide));
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00033EA4 File Offset: 0x000320A4
		private void RefreshItemTooltips(ItemVM item, ItemVM comparedItem, int alternativeUsageIndex = 0)
		{
			this.TargetItemProperties.Clear();
			this.TargetItemFlagList.Clear();
			this.ComparedItemProperties.Clear();
			this.ComparedItemFlagList.Clear();
			this.SetGeneralComponentTooltip();
			Town town = this._inventoryLogic.CurrentSettlementComponent as Town;
			EquipmentElement equipmentElement;
			if (town != null && Game.Current.IsDevelopmentMode)
			{
				MBBindingList<ItemMenuTooltipPropertyVM> targetItemProperties = this.TargetItemProperties;
				string text = "Category:";
				equipmentElement = item.ItemRosterElement.EquipmentElement;
				this.CreateProperty(targetItemProperties, text, equipmentElement.Item.ItemCategory.GetName().ToString(), 0, null);
				MBBindingList<ItemMenuTooltipPropertyVM> targetItemProperties2 = this.TargetItemProperties;
				string text2 = "Supply:";
				TownMarketData marketData = town.MarketData;
				equipmentElement = item.ItemRosterElement.EquipmentElement;
				this.CreateProperty(targetItemProperties2, text2, marketData.GetSupply(equipmentElement.Item.ItemCategory).ToString(), 0, null);
				MBBindingList<ItemMenuTooltipPropertyVM> targetItemProperties3 = this.TargetItemProperties;
				string text3 = "Demand:";
				TownMarketData marketData2 = town.MarketData;
				equipmentElement = item.ItemRosterElement.EquipmentElement;
				this.CreateProperty(targetItemProperties3, text3, marketData2.GetDemand(equipmentElement.Item.ItemCategory).ToString(), 0, null);
				MBBindingList<ItemMenuTooltipPropertyVM> targetItemProperties4 = this.TargetItemProperties;
				string text4 = "Price Index:";
				TownMarketData marketData3 = town.MarketData;
				equipmentElement = item.ItemRosterElement.EquipmentElement;
				this.CreateProperty(targetItemProperties4, text4, marketData3.GetPriceFactor(equipmentElement.Item.ItemCategory).ToString(), 0, null);
			}
			equipmentElement = item.ItemRosterElement.EquipmentElement;
			if (equipmentElement.Item.HasArmorComponent)
			{
				this.SetArmorComponentTooltip();
			}
			else
			{
				equipmentElement = item.ItemRosterElement.EquipmentElement;
				if (equipmentElement.Item.WeaponComponent != null)
				{
					equipmentElement = this._targetItem.ItemRosterElement.EquipmentElement;
					this.SetWeaponComponentTooltip(in equipmentElement, alternativeUsageIndex, EquipmentElement.Invalid, -1);
				}
				else
				{
					equipmentElement = item.ItemRosterElement.EquipmentElement;
					if (equipmentElement.Item.HasHorseComponent)
					{
						this.SetHorseComponentTooltip();
					}
					else
					{
						equipmentElement = item.ItemRosterElement.EquipmentElement;
						if (equipmentElement.Item.IsFood)
						{
							this.SetFoodTooltip();
						}
					}
				}
			}
			equipmentElement = item.ItemRosterElement.EquipmentElement;
			if (InventoryScreenHelper.GetInventoryItemTypeOfItem(equipmentElement.Item) == InventoryScreenHelper.InventoryItemType.Goods)
			{
				this.SetMerchandiseComponentTooltip();
			}
			if (this.IsComparing && !Input.IsGamepadActive)
			{
				for (EquipmentIndex equipmentIndex = this._comparedItem.ItemType + 1; equipmentIndex != this._comparedItem.ItemType; equipmentIndex = (equipmentIndex + 1) % EquipmentIndex.NumEquipmentSetSlots)
				{
					SPItemVM spitemVM = this._getEquipmentAtIndex(equipmentIndex);
					if (spitemVM != null)
					{
						equipmentElement = spitemVM.ItemRosterElement.EquipmentElement;
						ItemObject item2 = equipmentElement.Item;
						equipmentElement = comparedItem.ItemRosterElement.EquipmentElement;
						if (ItemHelper.CheckComparability(item2, equipmentElement.Item))
						{
							TextObject textObject = new TextObject("{=8fqFGxD9}Press {KEY} to compare with: {ITEM}", null);
							textObject.SetTextVariable("KEY", GameTexts.FindText("str_game_key_text", "anyalt"));
							textObject.SetTextVariable("ITEM", spitemVM.ItemDescription);
							this.CreateProperty(this.TargetItemProperties, "", textObject.ToString(), 0, null);
							this.CreateProperty(this.ComparedItemProperties, "", "", 0, null);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x000341B8 File Offset: 0x000323B8
		private int CompareValues(float currentValue, float comparedValue)
		{
			int num = (int)(currentValue * 10f);
			int num2 = (int)(comparedValue * 10f);
			if ((num != 0 && (float)MathF.Abs(num) <= MathF.Abs(currentValue)) || (num2 != 0 && (float)MathF.Abs(num2) <= MathF.Abs(comparedValue)))
			{
				return 0;
			}
			return this.CompareValues(num, num2);
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0003420D File Offset: 0x0003240D
		private int CompareValues(int currentValue, int comparedValue)
		{
			if (this._comparedItem == null || currentValue == comparedValue)
			{
				return 0;
			}
			if (currentValue <= comparedValue)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00034224 File Offset: 0x00032424
		private void AlternativeUsageIndexUpdated()
		{
			if (this.AlternativeUsageIndex < 0 || !this.IsInitializationOver)
			{
				return;
			}
			if (this._targetItem.ItemRosterElement.EquipmentElement.Item.WeaponComponent != null)
			{
				WeaponComponentData weaponComponentData = this._targetItem.ItemRosterElement.EquipmentElement.Item.Weapons[this.AlternativeUsageIndex];
				EquipmentElement equipmentElement;
				int num;
				this.GetComparedWeapon(weaponComponentData.WeaponDescriptionId, out equipmentElement, out num);
				if (!equipmentElement.IsEmpty)
				{
					this.RefreshItemTooltips(this._targetItem, this._comparedItem, this.AlternativeUsageIndex);
					return;
				}
				this._resetComparedItems(this._targetItem, this.AlternativeUsageIndex);
			}
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x000342D4 File Offset: 0x000324D4
		private void GetComparedWeapon(string weaponUsageId, out EquipmentElement comparedWeapon, out int comparedUsageIndex)
		{
			comparedWeapon = EquipmentElement.Invalid;
			comparedUsageIndex = -1;
			int num;
			if (this.IsComparing && this._comparedItem != null && ItemHelper.IsWeaponComparableWithUsage(this._comparedItem.ItemRosterElement.EquipmentElement.Item, weaponUsageId, out num))
			{
				comparedWeapon = this._comparedItem.ItemRosterElement.EquipmentElement;
				comparedUsageIndex = num;
			}
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0003433C File Offset: 0x0003253C
		private void SetGeneralComponentTooltip()
		{
			if (this._targetItem.ItemCost >= 0)
			{
				if (this._targetItem.ItemRosterElement.EquipmentElement.Item.Type == ItemObject.ItemTypeEnum.Goods || this._targetItem.ItemRosterElement.EquipmentElement.Item.Type == ItemObject.ItemTypeEnum.Animal || this._targetItem.ItemRosterElement.EquipmentElement.Item.Type == ItemObject.ItemTypeEnum.Horse)
				{
					Town town = this._inventoryLogic.CurrentSettlementComponent as Town;
					Village village;
					if (town == null && (village = this._inventoryLogic.CurrentSettlementComponent as Village) != null && village.TradeBound != null)
					{
						town = village.TradeBound.Town;
					}
					if (town == null)
					{
						town = SettlementHelper.FindNearestTownToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, null);
					}
					float num = ((town != null) ? TownHelpers.CalculatePriceDeviationRatio(town, this._targetItem.ItemRosterElement.EquipmentElement) : 1f);
					GameTexts.SetVariable("PERCENTAGE", MathF.Round(MathF.Abs(num * 100f)));
					if (num > 0.3f)
					{
						this._costProperty = this.CreateColoredProperty(this.TargetItemProperties, "", this._targetItem.ItemCost + this.GoldIcon, UIColors.NegativeIndicator, 1, new HintViewModel(GameTexts.FindText("str_inventory_cost_higher", null), null), TooltipProperty.TooltipPropertyFlags.Cost);
					}
					else if (num < -0.2f)
					{
						this._costProperty = this.CreateColoredProperty(this.TargetItemProperties, "", this._targetItem.ItemCost + this.GoldIcon, UIColors.PositiveIndicator, 1, new HintViewModel(GameTexts.FindText("str_inventory_cost_lower", null), null), TooltipProperty.TooltipPropertyFlags.Cost);
					}
					else
					{
						this._costProperty = this.CreateColoredProperty(this.TargetItemProperties, "", this._targetItem.ItemCost + this.GoldIcon, UIColors.Gold, 1, new HintViewModel(GameTexts.FindText("str_inventory_cost_normal", null), null), TooltipProperty.TooltipPropertyFlags.Cost);
					}
				}
				else
				{
					this._costProperty = this.CreateColoredProperty(this.TargetItemProperties, "", this._targetItem.ItemCost + this.GoldIcon, UIColors.Gold, 1, null, TooltipProperty.TooltipPropertyFlags.Cost);
				}
			}
			if (this.IsComparing)
			{
				this.CreateColoredProperty(this.ComparedItemProperties, "", this._comparedItem.ItemCost + this.GoldIcon, UIColors.Gold, 2, null, TooltipProperty.TooltipPropertyFlags.Cost);
			}
			if (Game.Current.IsDevelopmentMode)
			{
				if (this._targetItem.ItemRosterElement.EquipmentElement.Item.Culture != null)
				{
					this.CreateColoredProperty(this.TargetItemProperties, "Culture: ", this._targetItem.ItemRosterElement.EquipmentElement.Item.Culture.StringId, UIColors.Gold, 0, null, TooltipProperty.TooltipPropertyFlags.None);
				}
				else
				{
					this.CreateColoredProperty(this.TargetItemProperties, "Culture: ", "No Culture", UIColors.Gold, 0, null, TooltipProperty.TooltipPropertyFlags.None);
				}
				this.CreateColoredProperty(this.TargetItemProperties, "ID: ", this._targetItem.ItemRosterElement.EquipmentElement.Item.StringId, UIColors.Gold, 0, null, TooltipProperty.TooltipPropertyFlags.None);
			}
			float equipmentWeightMultiplier = 1f;
			CharacterObject characterObject;
			bool flag = (characterObject = this._character as CharacterObject) != null && characterObject.GetPerkValue(DefaultPerks.Athletics.FormFittingArmor);
			SPItemVM spitemVM = this._getEquipmentAtIndex(this._targetItem.ItemType);
			bool flag2 = spitemVM != null && spitemVM.ItemType != EquipmentIndex.None && spitemVM.ItemType != EquipmentIndex.HorseHarness && this._targetItem.ItemRosterElement.EquipmentElement.Item.HasArmorComponent;
			if (flag && flag2)
			{
				equipmentWeightMultiplier += DefaultPerks.Athletics.FormFittingArmor.PrimaryBonus;
			}
			this.AddFloatProperty(this._weightText, (EquipmentElement x) => x.GetEquipmentElementWeight() * equipmentWeightMultiplier, true);
			ItemObject item = this._targetItem.ItemRosterElement.EquipmentElement.Item;
			if (item.RelevantSkill != null && (item.Difficulty > 0 || (this.IsComparing && this._comparedItem.ItemRosterElement.EquipmentElement.Item.Difficulty > 0)))
			{
				this.AddSkillRequirement(this._targetItem, this.TargetItemProperties, false);
				if (this.IsComparing)
				{
					this.AddSkillRequirement(this._comparedItem, this.ComparedItemProperties, true);
				}
			}
			this.AddGeneralItemFlags(this.TargetItemFlagList, item);
			if (this.IsComparing)
			{
				this.AddGeneralItemFlags(this.ComparedItemFlagList, this._comparedItem.ItemRosterElement.EquipmentElement.Item);
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00034814 File Offset: 0x00032A14
		private void AddSkillRequirement(ItemVM itemVm, MBBindingList<ItemMenuTooltipPropertyVM> itemProperties, bool isComparison)
		{
			ItemObject item = itemVm.ItemRosterElement.EquipmentElement.Item;
			string text = "";
			if (item.Difficulty > 0)
			{
				text = item.RelevantSkill.Name.ToString();
				text += " ";
				text += item.Difficulty.ToString();
			}
			string text2 = "";
			if (!isComparison)
			{
				text2 = this._requiresText.ToString();
			}
			this.CreateColoredProperty(itemProperties, text2, text, this.GetColorFromBool(this._character == null || CharacterHelper.CanUseItem(this._character, itemVm.ItemRosterElement.EquipmentElement)), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000348C0 File Offset: 0x00032AC0
		private void AddGeneralItemFlags(MBBindingList<ItemFlagVM> list, ItemObject item)
		{
			if (item.IsUniqueItem)
			{
				list.Add(new ItemFlagVM("GeneralFlagIcons\\unique", GameTexts.FindText("str_inventory_flag_unique", null)));
			}
			if (item.IsCivilian)
			{
				list.Add(new ItemFlagVM("GeneralFlagIcons\\civillian", GameTexts.FindText("str_inventory_flag_civillian", null)));
			}
			if (item.IsStealthItem)
			{
				list.Add(new ItemFlagVM("GeneralFlagIcons\\stealth", GameTexts.FindText("str_inventory_flag_stealth", null)));
			}
			if (item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByFemale))
			{
				list.Add(new ItemFlagVM("GeneralFlagIcons\\male_only", GameTexts.FindText("str_inventory_flag_male_only", null)));
			}
			if (item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByMale))
			{
				list.Add(new ItemFlagVM("GeneralFlagIcons\\female_only", GameTexts.FindText("str_inventory_flag_female_only", null)));
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00034990 File Offset: 0x00032B90
		private void AddFoodItemFlags(MBBindingList<ItemFlagVM> list, ItemObject item)
		{
			list.Add(new ItemFlagVM("GoodsFlagIcons\\consumable", GameTexts.FindText("str_inventory_flag_consumable", null)));
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x000349B0 File Offset: 0x00032BB0
		private void AddWeaponItemFlags(MBBindingList<ItemFlagVM> list, WeaponComponentData weapon)
		{
			if (weapon == null)
			{
				Debug.FailedAssert("Trying to add flags for a null weapon", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Inventory\\ItemMenuVM.cs", "AddWeaponItemFlags", 430);
				return;
			}
			ItemObject.ItemUsageSetFlags itemUsageSetFlags = this._getItemUsageSetFlags(weapon);
			foreach (ValueTuple<string, TextObject> valueTuple in CampaignUIHelper.GetFlagDetailsForWeapon(weapon, itemUsageSetFlags, this._character as CharacterObject))
			{
				list.Add(new ItemFlagVM(valueTuple.Item1, valueTuple.Item2));
			}
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x00034A4C File Offset: 0x00032C4C
		private Color GetColorFromBool(bool booleanValue)
		{
			if (!booleanValue)
			{
				return UIColors.NegativeIndicator;
			}
			return UIColors.PositiveIndicator;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00034A5C File Offset: 0x00032C5C
		private void SetFoodTooltip()
		{
			this.CreateColoredProperty(this.TargetItemProperties, "", this._foodText.ToString(), this.ConsumableColor, 1, null, TooltipProperty.TooltipPropertyFlags.None);
			this.AddFoodItemFlags(this.TargetItemFlagList, this._targetItem.ItemRosterElement.EquipmentElement.Item);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00034AB4 File Offset: 0x00032CB4
		private void SetHorseComponentTooltip()
		{
			HorseComponent horseComponent = this._targetItem.ItemRosterElement.EquipmentElement.Item.HorseComponent;
			HorseComponent horseComponent2 = (this.IsComparing ? this._comparedItem.ItemRosterElement.EquipmentElement.Item.HorseComponent : null);
			this.CreateProperty(this.TargetItemProperties, this._typeText.ToString(), GameTexts.FindText("str_inventory_type_" + (int)this._targetItem.ItemRosterElement.EquipmentElement.Item.Type, null).ToString(), 0, null);
			this.AddHorseItemFlags(this.TargetItemFlagList, this._targetItem.ItemRosterElement.EquipmentElement.Item, horseComponent);
			if (this.IsComparing)
			{
				this.CreateProperty(this.ComparedItemProperties, " ", GameTexts.FindText("str_inventory_type_" + (int)this._comparedItem.ItemRosterElement.EquipmentElement.Item.Type, null).ToString(), 0, null);
				this.AddHorseItemFlags(this.ComparedItemFlagList, this._comparedItem.ItemRosterElement.EquipmentElement.Item, horseComponent2);
			}
			if (this._targetItem.ItemRosterElement.EquipmentElement.Item.IsMountable)
			{
				this.AddIntProperty(this._horseTierText, (int)(this._targetItem.ItemRosterElement.EquipmentElement.Item.Tier + 1), (this.IsComparing && this._comparedItem != null) ? new int?((int)(this._comparedItem.ItemRosterElement.EquipmentElement.Item.Tier + 1)) : null);
				this.AddIntProperty(this._chargeDamageText, this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedMountCharge(in EquipmentElement.Invalid), (this.IsComparing && this._comparedItem != null) ? new int?(this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedMountCharge(in EquipmentElement.Invalid)) : null);
				this.AddIntProperty(this._speedText, this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedMountSpeed(in EquipmentElement.Invalid), (this.IsComparing && this._comparedItem != null) ? new int?(this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedMountSpeed(in EquipmentElement.Invalid)) : null);
				this.AddIntProperty(this._maneuverText, this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedMountManeuver(in EquipmentElement.Invalid), (this.IsComparing && this._comparedItem != null) ? new int?(this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedMountManeuver(in EquipmentElement.Invalid)) : null);
				this.AddIntProperty(this._hitPointsText, this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedMountHitPoints(), (this.IsComparing && this._comparedItem != null) ? new int?(this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedMountHitPoints()) : null);
				if (this._targetItem.ItemRosterElement.EquipmentElement.Item.HasHorseComponent && this._targetItem.ItemRosterElement.EquipmentElement.Item.HorseComponent.IsMount)
				{
					this.AddComparableStringProperty(this._horseTypeText, (EquipmentElement x) => x.Item.ItemCategory.GetName().ToString(), (EquipmentElement x) => this.GetHorseCategoryValue(x.Item.ItemCategory));
				}
			}
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00034E8C File Offset: 0x0003308C
		private void AddHorseItemFlags(MBBindingList<ItemFlagVM> list, ItemObject item, HorseComponent horse)
		{
			if (!horse.IsLiveStock)
			{
				if (item.ItemCategory == DefaultItemCategories.PackAnimal)
				{
					list.Add(new ItemFlagVM("MountFlagIcons\\weight_carrying_mount", GameTexts.FindText("str_inventory_flag_carrying_mount", null)));
				}
				else
				{
					list.Add(new ItemFlagVM("MountFlagIcons\\speed_mount", GameTexts.FindText("str_inventory_flag_speed_mount", null)));
				}
			}
			if (this._inventoryLogic.IsSlaughterable(item))
			{
				list.Add(new ItemFlagVM("MountFlagIcons\\slaughterable", GameTexts.FindText("str_inventory_flag_slaughterable", null)));
			}
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00034F10 File Offset: 0x00033110
		private void SetMerchandiseComponentTooltip()
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				if (this._tradeRumorsBehavior == null)
				{
					return;
				}
				IEnumerable<TradeRumor> tradeRumors = this._tradeRumorsBehavior.TradeRumors;
				bool flag = true;
				IMarketData marketData = this._inventoryLogic.MarketData;
				foreach (TradeRumor tradeRumor in from x in tradeRumors
					orderby x.SellPrice descending, x.BuyPrice descending
					select x)
				{
					bool flag2 = false;
					bool flag3 = false;
					if (this._targetItem.ItemRosterElement.EquipmentElement.Item == tradeRumor.ItemCategory)
					{
						if ((float)tradeRumor.BuyPrice < 0.9f * (float)marketData.GetPrice(tradeRumor.ItemCategory, MobileParty.MainParty, true, this._inventoryLogic.OtherParty))
						{
							flag3 = true;
						}
						if ((float)tradeRumor.SellPrice > 1.1f * (float)marketData.GetPrice(tradeRumor.ItemCategory, MobileParty.MainParty, false, this._inventoryLogic.OtherParty))
						{
							flag2 = true;
						}
						if ((Settlement.CurrentSettlement == null || Settlement.CurrentSettlement != tradeRumor.Settlement) && this._targetItem.ItemRosterElement.EquipmentElement.Item == tradeRumor.ItemCategory && (flag3 || flag2))
						{
							if (flag)
							{
								this.CreateColoredProperty(this.TargetItemProperties, "", this._tradeRumorsText.ToString(), this.TitleColor, 1, null, TooltipProperty.TooltipPropertyFlags.None);
								if (this.IsComparing)
								{
									this.CreateProperty(this.ComparedItemProperties, "", "", 0, null);
									this.CreateProperty(this.ComparedItemProperties, "", "", 0, null);
								}
								flag = false;
							}
							MBTextManager.SetTextVariable("SETTLEMENT_NAME", tradeRumor.Settlement.Name, false);
							MBTextManager.SetTextVariable("SELL_PRICE", tradeRumor.SellPrice);
							MBTextManager.SetTextVariable("BUY_PRICE", tradeRumor.BuyPrice);
							float num = this.CalculateTradeRumorOldnessFactor(tradeRumor);
							Color color = new Color(this.TitleColor.Red, this.TitleColor.Green, this.TitleColor.Blue, num);
							TextObject textObject = (flag3 ? GameTexts.FindText("str_trade_rumors_text_buy", null) : GameTexts.FindText("str_trade_rumors_text_sell", null));
							this.CreateColoredProperty(this.TargetItemProperties, "", textObject.ToString(), color, 0, null, TooltipProperty.TooltipPropertyFlags.None);
							if (this.IsComparing)
							{
								this.CreateProperty(this.ComparedItemProperties, "", "", 0, null);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x000351D8 File Offset: 0x000333D8
		private float CalculateTradeRumorOldnessFactor(TradeRumor rumor)
		{
			return MathF.Clamp((float)((int)rumor.RumorEndTime.RemainingDaysFromNow) / 5f, 0.5f, 1f);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0003520C File Offset: 0x0003340C
		private void UpdateAlternativeUsages(EquipmentElement targetWeapon)
		{
			List<StringItemWithHintVM> list = new List<StringItemWithHintVM>();
			foreach (WeaponComponentData weaponComponentData in targetWeapon.Item.Weapons)
			{
				if (CampaignUIHelper.IsItemUsageApplicable(weaponComponentData))
				{
					list.Add(new StringItemWithHintVM(GameTexts.FindText("str_weapon_usage", weaponComponentData.WeaponDescriptionId).ToString(), GameTexts.FindText("str_inventory_alternative_usage_hint", null)));
				}
			}
			for (int i = this.AlternativeUsages.Count - 1; i >= 0; i--)
			{
				StringItemWithHintVM oldUsage = this.AlternativeUsages[i];
				if (!list.Any<StringItemWithHintVM>((StringItemWithHintVM x) => x.Text == oldUsage.Text))
				{
					this.AlternativeUsages.RemoveAt(i);
				}
			}
			using (List<StringItemWithHintVM>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					StringItemWithHintVM newUsage = enumerator2.Current;
					if (!this.AlternativeUsages.Any<StringItemWithHintVM>((StringItemWithHintVM x) => x.Text == newUsage.Text))
					{
						this.AlternativeUsages.Add(newUsage);
					}
				}
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0003535C File Offset: 0x0003355C
		private void SetWeaponComponentTooltip(in EquipmentElement targetWeapon, int targetWeaponUsageIndex, EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			WeaponComponentData weaponWithUsageIndex = equipmentElement.Item.GetWeaponWithUsageIndex(targetWeaponUsageIndex);
			if (this.IsComparing && this._comparedItem != null && comparedWeapon.IsEmpty)
			{
				this.GetComparedWeapon(weaponWithUsageIndex.WeaponDescriptionId, out comparedWeapon, out comparedWeaponUsageIndex);
			}
			WeaponComponentData weaponComponentData = (comparedWeapon.IsEmpty ? null : comparedWeapon.Item.GetWeaponWithUsageIndex(comparedWeaponUsageIndex));
			this.AddWeaponItemFlags(this.TargetItemFlagList, weaponWithUsageIndex);
			if (this.IsComparing)
			{
				this.AddWeaponItemFlags(this.ComparedItemFlagList, weaponComponentData);
			}
			this.UpdateAlternativeUsages(targetWeapon);
			this.AlternativeUsageIndex = targetWeaponUsageIndex;
			this.CreateProperty(this.TargetItemProperties, this._classText.ToString(), GameTexts.FindText("str_inventory_weapon", weaponWithUsageIndex.WeaponClass.ToString()).ToString(), 0, null);
			if (!comparedWeapon.IsEmpty)
			{
				this.CreateProperty(this.ComparedItemProperties, " ", GameTexts.FindText("str_inventory_weapon", weaponComponentData.WeaponClass.ToString()).ToString(), 0, null);
			}
			else if (this.IsComparing)
			{
				this.CreateProperty(this.ComparedItemProperties, "", "", 0, null);
			}
			equipmentElement = targetWeapon;
			if (equipmentElement.Item.BannerComponent == null)
			{
				int num = 0;
				if (!comparedWeapon.IsEmpty)
				{
					num = (int)(comparedWeapon.Item.Tier + 1);
				}
				TextObject weaponTierText = this._weaponTierText;
				equipmentElement = targetWeapon;
				this.AddIntProperty(weaponTierText, (int)(equipmentElement.Item.Tier + 1), new int?(num));
			}
			ItemObject.ItemTypeEnum itemTypeFromWeaponClass = WeaponComponentData.GetItemTypeFromWeaponClass(weaponWithUsageIndex.WeaponClass);
			ItemObject.ItemTypeEnum itemTypeEnum = ((!comparedWeapon.IsEmpty) ? WeaponComponentData.GetItemTypeFromWeaponClass(weaponWithUsageIndex.WeaponClass) : ItemObject.ItemTypeEnum.Invalid);
			if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.OneHandedWeapon || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.TwoHandedWeapon || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Polearm || itemTypeEnum == ItemObject.ItemTypeEnum.OneHandedWeapon || itemTypeEnum == ItemObject.ItemTypeEnum.TwoHandedWeapon || itemTypeEnum == ItemObject.ItemTypeEnum.Polearm)
			{
				if (weaponWithUsageIndex.SwingDamageType != DamageTypes.Invalid)
				{
					this.AddSwingSpeedProperty(this._swingSpeedText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
					this.AddSwingDamageProperty(this._swingDamageText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
				}
				if (weaponWithUsageIndex.ThrustDamageType != DamageTypes.Invalid)
				{
					this.AddThrustSpeedProperty(this._thrustSpeedText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
					this.AddThrustDamageProperty(this._thrustDamageText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
				}
				this.AddIntProperty(this._lengthText, weaponWithUsageIndex.WeaponLength, (weaponComponentData != null) ? new int?(weaponComponentData.WeaponLength) : null);
				TextObject handlingText = this._handlingText;
				equipmentElement = targetWeapon;
				this.AddIntProperty(handlingText, equipmentElement.GetModifiedHandlingForUsage(targetWeaponUsageIndex), comparedWeapon.IsEmpty ? null : new int?(comparedWeapon.GetModifiedHandlingForUsage(comparedWeaponUsageIndex)));
			}
			if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Thrown || itemTypeEnum == ItemObject.ItemTypeEnum.Thrown)
			{
				this.AddIntProperty(this._weaponLengthText, weaponWithUsageIndex.WeaponLength, (weaponComponentData != null) ? new int?(weaponComponentData.WeaponLength) : null);
				this.AddMissileDamageProperty(this._damageText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
				this.AddMissileSpeedProperty(this._missileSpeedText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
				this.AddIntProperty(this._accuracyText, weaponWithUsageIndex.Accuracy, (weaponComponentData != null) ? new int?(weaponComponentData.Accuracy) : null);
				this.AddStackAmountProperty(this._stackAmountText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
			}
			if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Shield || itemTypeEnum == ItemObject.ItemTypeEnum.Shield)
			{
				this.AddSwingSpeedProperty(this._swingSpeedText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
				this.AddHitPointsProperty(this._hitPointsText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
			}
			if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Bow || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Crossbow || itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Sling || itemTypeEnum == ItemObject.ItemTypeEnum.Bow || itemTypeEnum == ItemObject.ItemTypeEnum.Crossbow || itemTypeEnum == ItemObject.ItemTypeEnum.Sling)
			{
				this.AddSwingSpeedProperty(this._swingSpeedText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
				this.AddThrustDamageProperty(this._damageText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
				this.AddIntProperty(this._accuracyText, weaponWithUsageIndex.Accuracy, (weaponComponentData != null) ? new int?(weaponComponentData.Accuracy) : null);
				this.AddMissileSpeedProperty(this._missileSpeedText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
				if (itemTypeFromWeaponClass == ItemObject.ItemTypeEnum.Crossbow || itemTypeEnum == ItemObject.ItemTypeEnum.Crossbow)
				{
					TextObject ammoLimitText = this._ammoLimitText;
					int maxDataValue = (int)weaponWithUsageIndex.MaxDataValue;
					short? num2 = ((weaponComponentData != null) ? new short?(weaponComponentData.MaxDataValue) : null);
					this.AddIntProperty(ammoLimitText, maxDataValue, (num2 != null) ? new int?((int)num2.GetValueOrDefault()) : null);
				}
			}
			if (weaponWithUsageIndex.IsAmmo || (weaponComponentData != null && weaponComponentData.IsAmmo))
			{
				if ((itemTypeFromWeaponClass != ItemObject.ItemTypeEnum.Arrows && itemTypeFromWeaponClass != ItemObject.ItemTypeEnum.Bolts && itemTypeFromWeaponClass != ItemObject.ItemTypeEnum.SlingStones) || (weaponComponentData != null && itemTypeEnum != ItemObject.ItemTypeEnum.Arrows && itemTypeEnum != ItemObject.ItemTypeEnum.Bolts && itemTypeEnum != ItemObject.ItemTypeEnum.SlingStones))
				{
					this.AddIntProperty(this._accuracyText, weaponWithUsageIndex.Accuracy, (weaponComponentData != null) ? new int?(weaponComponentData.Accuracy) : null);
				}
				this.AddThrustDamageProperty(this._bonusDamageText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
				this.AddStackAmountProperty(this._stackAmountText, in targetWeapon, targetWeaponUsageIndex, in comparedWeapon, comparedWeaponUsageIndex);
			}
			equipmentElement = targetWeapon;
			ItemObject item = equipmentElement.Item;
			if (item == null || !item.HasBannerComponent)
			{
				ItemObject item2 = comparedWeapon.Item;
				if (item2 == null || !item2.HasBannerComponent)
				{
					goto IL_051E;
				}
			}
			Func<EquipmentElement, string> func = delegate(EquipmentElement x)
			{
				ItemObject item3 = x.Item;
				bool flag;
				if (item3 == null)
				{
					flag = null != null;
				}
				else
				{
					BannerComponent bannerComponent = item3.BannerComponent;
					flag = ((bannerComponent != null) ? bannerComponent.BannerEffect : null) != null;
				}
				if (flag)
				{
					GameTexts.SetVariable("RANK", x.Item.BannerComponent.BannerEffect.Name);
					string text = string.Empty;
					if (x.Item.BannerComponent.BannerEffect.IncrementType == EffectIncrementType.AddFactor)
					{
						GameTexts.FindText("str_NUMBER_percent", null).SetTextVariable("NUMBER", ((int)Math.Abs(x.Item.BannerComponent.GetBannerEffectBonus() * 100f)).ToString());
						object obj;
						text = obj.ToString();
					}
					else if (x.Item.BannerComponent.BannerEffect.IncrementType == EffectIncrementType.Add)
					{
						text = x.Item.BannerComponent.GetBannerEffectBonus().ToString();
					}
					GameTexts.SetVariable("NUMBER", text);
					return GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
				}
				return this._noneText.ToString();
			};
			this.AddComparableStringProperty(this._bannerEffectText, func, (EquipmentElement x) => 0);
			IL_051E:
			this.AddDonationXpTooltip();
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0003588D File Offset: 0x00033A8D
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x00035895 File Offset: 0x00033A95
		[DataSourceProperty]
		public bool IsComparing
		{
			get
			{
				return this._isComparing;
			}
			set
			{
				if (value != this._isComparing)
				{
					this._isComparing = value;
					base.OnPropertyChangedWithValue(value, "IsComparing");
				}
			}
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x000358B4 File Offset: 0x00033AB4
		private void AddIntProperty(TextObject description, int targetValue, int? comparedValue)
		{
			string text = targetValue.ToString();
			if (this.IsComparing && comparedValue != null)
			{
				string text2 = comparedValue.Value.ToString();
				int num = this.CompareValues(targetValue, comparedValue.Value);
				this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), text, this.GetColorFromComparison(num, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				this.CreateColoredProperty(this.ComparedItemProperties, " ", text2, this.GetColorFromComparison(num, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				return;
			}
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), text, this.GetColorFromComparison(0, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x00035956 File Offset: 0x00033B56
		// (set) Token: 0x06000CC5 RID: 3269 RVA: 0x0003595E File Offset: 0x00033B5E
		[DataSourceProperty]
		public bool IsPlayerItem
		{
			get
			{
				return this._isPlayerItem;
			}
			set
			{
				if (value != this._isPlayerItem)
				{
					this._isPlayerItem = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerItem");
				}
			}
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0003597C File Offset: 0x00033B7C
		private void AddFloatProperty(TextObject description, Func<EquipmentElement, float> func, bool reversedCompare = false)
		{
			float num = func(this._targetItem.ItemRosterElement.EquipmentElement);
			float? num2 = null;
			if (this.IsComparing && this._comparedItem != null)
			{
				num2 = new float?(func(this._comparedItem.ItemRosterElement.EquipmentElement));
			}
			this.AddFloatProperty(description, num, num2, reversedCompare);
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x000359DF File Offset: 0x00033BDF
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x000359E7 File Offset: 0x00033BE7
		[DataSourceProperty]
		public ItemImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00035A08 File Offset: 0x00033C08
		private void AddFloatProperty(TextObject description, float targetValue, float? comparedValue, bool reversedCompare = false)
		{
			string formattedItemPropertyText = CampaignUIHelper.GetFormattedItemPropertyText(targetValue, false);
			if (this.IsComparing && comparedValue != null)
			{
				string formattedItemPropertyText2 = CampaignUIHelper.GetFormattedItemPropertyText(comparedValue.Value, false);
				int num = this.CompareValues(targetValue, comparedValue.Value);
				if (reversedCompare)
				{
					num *= -1;
				}
				this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), formattedItemPropertyText, this.GetColorFromComparison(num, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				this.CreateColoredProperty(this.ComparedItemProperties, " ", formattedItemPropertyText2, this.GetColorFromComparison(num, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				return;
			}
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), formattedItemPropertyText, this.GetColorFromComparison(0, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x00035AB0 File Offset: 0x00033CB0
		// (set) Token: 0x06000CCB RID: 3275 RVA: 0x00035AB8 File Offset: 0x00033CB8
		[DataSourceProperty]
		public ItemImageIdentifierVM ComparedImageIdentifier
		{
			get
			{
				return this._comparedImageIdentifier;
			}
			set
			{
				if (value != this._comparedImageIdentifier)
				{
					this._comparedImageIdentifier = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "ComparedImageIdentifier");
				}
			}
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00035AD8 File Offset: 0x00033CD8
		private void AddComparableStringProperty(TextObject description, Func<EquipmentElement, string> valueAsStringFunc, Func<EquipmentElement, int> valueAsIntFunc)
		{
			string text = valueAsStringFunc(this._targetItem.ItemRosterElement.EquipmentElement);
			int num = valueAsIntFunc(this._targetItem.ItemRosterElement.EquipmentElement);
			if (this.IsComparing && this._comparedItem != null)
			{
				int num2 = valueAsIntFunc(this._comparedItem.ItemRosterElement.EquipmentElement);
				int num3 = this.CompareValues(num, num2);
				this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), text, this.GetColorFromComparison(num3, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				this.CreateColoredProperty(this.ComparedItemProperties, " ", valueAsStringFunc(this._comparedItem.ItemRosterElement.EquipmentElement), this.GetColorFromComparison(num3, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				return;
			}
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), text, this.GetColorFromComparison(0, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00035BB5 File Offset: 0x00033DB5
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x00035BBD File Offset: 0x00033DBD
		[DataSourceProperty]
		public int TransactionTotalCost
		{
			get
			{
				return this._transactionTotalCost;
			}
			set
			{
				if (value != this._transactionTotalCost)
				{
					this._transactionTotalCost = value;
					base.OnPropertyChangedWithValue(value, "TransactionTotalCost");
				}
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00035BDC File Offset: 0x00033DDC
		private void AddSwingDamageProperty(TextObject description, in EquipmentElement targetWeapon, int targetWeaponUsageIndex, in EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			int modifiedSwingDamageForUsage = equipmentElement.GetModifiedSwingDamageForUsage(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			WeaponComponentData weaponWithUsageIndex = equipmentElement.Item.GetWeaponWithUsageIndex(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			string text = ItemHelper.GetSwingDamageText(weaponWithUsageIndex, equipmentElement.ItemModifier).ToString();
			if (this.IsComparing)
			{
				equipmentElement = comparedWeapon;
				if (!equipmentElement.IsEmpty)
				{
					equipmentElement = comparedWeapon;
					int modifiedSwingDamageForUsage2 = equipmentElement.GetModifiedSwingDamageForUsage(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					WeaponComponentData weaponWithUsageIndex2 = equipmentElement.Item.GetWeaponWithUsageIndex(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					string text2 = ItemHelper.GetSwingDamageText(weaponWithUsageIndex2, equipmentElement.ItemModifier).ToString();
					int num = this.CompareValues(modifiedSwingDamageForUsage, modifiedSwingDamageForUsage2);
					this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), text.ToString(), this.GetColorFromComparison(num, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					this.CreateColoredProperty(this.ComparedItemProperties, " ", text2, this.GetColorFromComparison(num, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					return;
				}
			}
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), text.ToString(), this.GetColorFromComparison(0, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00035CFC File Offset: 0x00033EFC
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x00035D04 File Offset: 0x00033F04
		[DataSourceProperty]
		public bool IsInitializationOver
		{
			get
			{
				return this._isInitializationOver;
			}
			set
			{
				if (value != this._isInitializationOver)
				{
					this._isInitializationOver = value;
					base.OnPropertyChangedWithValue(value, "IsInitializationOver");
				}
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00035D24 File Offset: 0x00033F24
		private void AddHitPointsProperty(TextObject description, in EquipmentElement targetWeapon, int targetWeaponUsageIndex, in EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			short modifiedMaximumHitPointsForUsage = equipmentElement.GetModifiedMaximumHitPointsForUsage(targetWeaponUsageIndex);
			if (this.IsComparing)
			{
				equipmentElement = comparedWeapon;
				if (!equipmentElement.IsEmpty)
				{
					equipmentElement = comparedWeapon;
					short modifiedMaximumHitPointsForUsage2 = equipmentElement.GetModifiedMaximumHitPointsForUsage(comparedWeaponUsageIndex);
					int num = this.CompareValues((int)modifiedMaximumHitPointsForUsage, (int)modifiedMaximumHitPointsForUsage2);
					this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), modifiedMaximumHitPointsForUsage.ToString(), this.GetColorFromComparison(num, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					this.CreateColoredProperty(this.ComparedItemProperties, " ", modifiedMaximumHitPointsForUsage2.ToString(), this.GetColorFromComparison(num, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					return;
				}
			}
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), modifiedMaximumHitPointsForUsage.ToString(), this.GetColorFromComparison(0, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00035DE4 File Offset: 0x00033FE4
		private void AddStackAmountProperty(TextObject description, in EquipmentElement targetWeapon, int targetWeaponUsageIndex, in EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			short modifiedStackCountForUsage = equipmentElement.GetModifiedStackCountForUsage(targetWeaponUsageIndex);
			if (this.IsComparing)
			{
				equipmentElement = comparedWeapon;
				if (!equipmentElement.IsEmpty)
				{
					equipmentElement = comparedWeapon;
					short modifiedStackCountForUsage2 = equipmentElement.GetModifiedStackCountForUsage(comparedWeaponUsageIndex);
					int num = this.CompareValues((int)modifiedStackCountForUsage, (int)modifiedStackCountForUsage2);
					this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), modifiedStackCountForUsage.ToString(), this.GetColorFromComparison(num, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					this.CreateColoredProperty(this.ComparedItemProperties, " ", modifiedStackCountForUsage2.ToString(), this.GetColorFromComparison(num, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					return;
				}
			}
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), modifiedStackCountForUsage.ToString(), this.GetColorFromComparison(0, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00035EA4 File Offset: 0x000340A4
		private void AddThrustSpeedProperty(TextObject description, in EquipmentElement targetWeapon, int targetWeaponUsageIndex, in EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			int modifiedThrustSpeedForUsage = equipmentElement.GetModifiedThrustSpeedForUsage(targetWeaponUsageIndex);
			if (this.IsComparing)
			{
				equipmentElement = comparedWeapon;
				if (!equipmentElement.IsEmpty)
				{
					equipmentElement = comparedWeapon;
					int modifiedThrustSpeedForUsage2 = equipmentElement.GetModifiedThrustSpeedForUsage(comparedWeaponUsageIndex);
					int num = this.CompareValues(modifiedThrustSpeedForUsage, modifiedThrustSpeedForUsage2);
					this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), modifiedThrustSpeedForUsage.ToString(), this.GetColorFromComparison(num, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					this.CreateColoredProperty(this.ComparedItemProperties, " ", modifiedThrustSpeedForUsage2.ToString(), this.GetColorFromComparison(num, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					return;
				}
			}
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), modifiedThrustSpeedForUsage.ToString(), this.GetColorFromComparison(0, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00035F64 File Offset: 0x00034164
		private void AddSwingSpeedProperty(TextObject description, in EquipmentElement targetWeapon, int targetWeaponUsageIndex, in EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			int modifiedSwingSpeedForUsage = equipmentElement.GetModifiedSwingSpeedForUsage(targetWeaponUsageIndex);
			if (this.IsComparing)
			{
				equipmentElement = comparedWeapon;
				if (!equipmentElement.IsEmpty)
				{
					equipmentElement = comparedWeapon;
					int modifiedSwingSpeedForUsage2 = equipmentElement.GetModifiedSwingSpeedForUsage(comparedWeaponUsageIndex);
					int num = this.CompareValues(modifiedSwingSpeedForUsage, modifiedSwingSpeedForUsage2);
					this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), modifiedSwingSpeedForUsage.ToString(), this.GetColorFromComparison(num, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					this.CreateColoredProperty(this.ComparedItemProperties, " ", modifiedSwingSpeedForUsage2.ToString(), this.GetColorFromComparison(num, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					return;
				}
			}
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), modifiedSwingSpeedForUsage.ToString(), this.GetColorFromComparison(0, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00036024 File Offset: 0x00034224
		private void AddMissileSpeedProperty(TextObject description, in EquipmentElement targetWeapon, int targetWeaponUsageIndex, in EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			int modifiedMissileSpeedForUsage = equipmentElement.GetModifiedMissileSpeedForUsage(targetWeaponUsageIndex);
			if (this.IsComparing)
			{
				equipmentElement = comparedWeapon;
				if (!equipmentElement.IsEmpty)
				{
					equipmentElement = comparedWeapon;
					int modifiedMissileSpeedForUsage2 = equipmentElement.GetModifiedMissileSpeedForUsage(comparedWeaponUsageIndex);
					int num = this.CompareValues(modifiedMissileSpeedForUsage, modifiedMissileSpeedForUsage2);
					this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), modifiedMissileSpeedForUsage.ToString(), this.GetColorFromComparison(num, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					this.CreateColoredProperty(this.ComparedItemProperties, " ", modifiedMissileSpeedForUsage2.ToString(), this.GetColorFromComparison(num, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					return;
				}
			}
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), modifiedMissileSpeedForUsage.ToString(), this.GetColorFromComparison(0, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x000360E4 File Offset: 0x000342E4
		private void AddMissileDamageProperty(TextObject description, in EquipmentElement targetWeapon, int targetWeaponUsageIndex, in EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			int modifiedMissileDamageForUsage = equipmentElement.GetModifiedMissileDamageForUsage(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			WeaponComponentData weaponWithUsageIndex = equipmentElement.Item.GetWeaponWithUsageIndex(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			string text = ItemHelper.GetMissileDamageText(weaponWithUsageIndex, equipmentElement.ItemModifier).ToString();
			if (this.IsComparing)
			{
				equipmentElement = comparedWeapon;
				if (!equipmentElement.IsEmpty)
				{
					equipmentElement = comparedWeapon;
					int modifiedMissileDamageForUsage2 = equipmentElement.GetModifiedMissileDamageForUsage(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					WeaponComponentData weaponWithUsageIndex2 = equipmentElement.Item.GetWeaponWithUsageIndex(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					string text2 = ItemHelper.GetMissileDamageText(weaponWithUsageIndex2, equipmentElement.ItemModifier).ToString();
					int num = this.CompareValues(modifiedMissileDamageForUsage, modifiedMissileDamageForUsage2);
					this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), text.ToString(), this.GetColorFromComparison(num, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					this.CreateColoredProperty(this.ComparedItemProperties, " ", text2, this.GetColorFromComparison(num, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					return;
				}
			}
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), text.ToString(), this.GetColorFromComparison(0, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00036204 File Offset: 0x00034404
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x0003620C File Offset: 0x0003440C
		[DataSourceProperty]
		public string ItemName
		{
			get
			{
				return this._itemName;
			}
			set
			{
				if (value != this._itemName)
				{
					this._itemName = value;
					base.OnPropertyChangedWithValue<string>(value, "ItemName");
				}
			}
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00036230 File Offset: 0x00034430
		private void AddThrustDamageProperty(TextObject description, in EquipmentElement targetWeapon, int targetWeaponUsageIndex, in EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			int modifiedThrustDamageForUsage = equipmentElement.GetModifiedThrustDamageForUsage(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			WeaponComponentData weaponWithUsageIndex = equipmentElement.Item.GetWeaponWithUsageIndex(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			TextObject thrustDamageText = ItemHelper.GetThrustDamageText(weaponWithUsageIndex, equipmentElement.ItemModifier);
			if (this.IsComparing)
			{
				equipmentElement = comparedWeapon;
				if (!equipmentElement.IsEmpty)
				{
					equipmentElement = comparedWeapon;
					int modifiedThrustDamageForUsage2 = equipmentElement.GetModifiedThrustDamageForUsage(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					WeaponComponentData weaponWithUsageIndex2 = equipmentElement.Item.GetWeaponWithUsageIndex(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					string text = ItemHelper.GetThrustDamageText(weaponWithUsageIndex2, equipmentElement.ItemModifier).ToString();
					int num = this.CompareValues(modifiedThrustDamageForUsage, modifiedThrustDamageForUsage2);
					this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), thrustDamageText.ToString(), this.GetColorFromComparison(num, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					this.CreateColoredProperty(this.ComparedItemProperties, " ", text, this.GetColorFromComparison(num, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					return;
				}
			}
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), thrustDamageText.ToString(), this.GetColorFromComparison(0, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x0003634B File Offset: 0x0003454B
		// (set) Token: 0x06000CDC RID: 3292 RVA: 0x00036353 File Offset: 0x00034553
		[DataSourceProperty]
		public string ComparedItemName
		{
			get
			{
				return this._comparedItemName;
			}
			set
			{
				if (value != this._comparedItemName)
				{
					this._comparedItemName = value;
					base.OnPropertyChangedWithValue<string>(value, "ComparedItemName");
				}
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00036378 File Offset: 0x00034578
		private void SetArmorComponentTooltip()
		{
			int num = 0;
			if (this._comparedItem != null && this._comparedItem.ItemRosterElement.EquipmentElement.Item != null)
			{
				num = (int)(this._comparedItem.ItemRosterElement.EquipmentElement.Item.Tier + 1);
			}
			this.AddIntProperty(this._armorTierText, (int)(this._targetItem.ItemRosterElement.EquipmentElement.Item.Tier + 1), new int?(num));
			this.CreateProperty(this.TargetItemProperties, this._typeText.ToString(), GameTexts.FindText("str_inventory_type_" + (int)this._targetItem.ItemRosterElement.EquipmentElement.Item.Type, null).ToString(), 0, null);
			if (this.IsComparing)
			{
				this.CreateProperty(this.ComparedItemProperties, " ", GameTexts.FindText("str_inventory_type_" + (int)this._targetItem.ItemRosterElement.EquipmentElement.Item.Type, null).ToString(), 0, null);
			}
			ArmorComponent armorComponent = this._targetItem.ItemRosterElement.EquipmentElement.Item.ArmorComponent;
			ArmorComponent armorComponent2 = (this.IsComparing ? this._comparedItem.ItemRosterElement.EquipmentElement.Item.ArmorComponent : null);
			if (armorComponent.HeadArmor != 0 || (this.IsComparing && armorComponent2.HeadArmor != 0))
			{
				int num2 = (this.IsComparing ? this.CompareValues(this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedHeadArmor(), this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedHeadArmor()) : 0);
				this.CreateColoredProperty(this.TargetItemProperties, this._headArmorText.ToString(), this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedHeadArmor().ToString(), this.GetColorFromComparison(num2, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				if (this.IsComparing)
				{
					this.CreateColoredProperty(this.ComparedItemProperties, " ", this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedHeadArmor().ToString(), this.GetColorFromComparison(num2, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (armorComponent.BodyArmor != 0 || (this.IsComparing && this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedBodyArmor() != 0))
			{
				if (this._targetItem.ItemType == EquipmentIndex.HorseHarness)
				{
					int num2 = (this.IsComparing ? this.CompareValues(this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedMountBodyArmor(), this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedMountBodyArmor()) : 0);
					this.CreateColoredProperty(this.TargetItemProperties, this._horseArmorText.ToString(), this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedMountBodyArmor().ToString(), this.GetColorFromComparison(num2, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					if (this.IsComparing)
					{
						this.CreateColoredProperty(this.ComparedItemProperties, " ", this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedMountBodyArmor().ToString(), this.GetColorFromComparison(num2, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				else
				{
					int num2 = (this.IsComparing ? this.CompareValues(this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedBodyArmor(), this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedBodyArmor()) : 0);
					this.CreateColoredProperty(this.TargetItemProperties, this._bodyArmorText.ToString(), this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedBodyArmor().ToString(), this.GetColorFromComparison(num2, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					if (this.IsComparing)
					{
						this.CreateColoredProperty(this.ComparedItemProperties, " ", this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedBodyArmor().ToString(), this.GetColorFromComparison(num2, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
			}
			if (this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor() != 0 || (this.IsComparing && this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor() != 0))
			{
				int num2 = (this.IsComparing ? this.CompareValues(this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor(), this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor()) : 0);
				this.CreateColoredProperty(this.TargetItemProperties, this._legArmorText.ToString(), this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor().ToString(), this.GetColorFromComparison(num2, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				if (this.IsComparing)
				{
					this.CreateColoredProperty(this.ComparedItemProperties, " ", this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor().ToString(), this.GetColorFromComparison(num2, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor() != 0 || (this.IsComparing && this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor() != 0))
			{
				int num2 = (this.IsComparing ? this.CompareValues(this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor(), this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor()) : 0);
				this.CreateColoredProperty(this.TargetItemProperties, this._armArmorText.ToString(), this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor().ToString(), this.GetColorFromComparison(num2, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				if (this.IsComparing)
				{
					this.CreateColoredProperty(this.ComparedItemProperties, " ", this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor().ToString(), this.GetColorFromComparison(num2, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (this.IsStealthModeActive)
			{
				int num2 = (this.IsComparing ? this.CompareValues(this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedStealthFactor(), this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedStealthFactor()) : 0);
				this.CreateColoredProperty(this.TargetItemProperties, this._stealthBonusText.ToString(), this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedStealthFactor().ToString(), this.GetColorFromComparison(num2, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				if (this.IsComparing)
				{
					this.CreateColoredProperty(this.ComparedItemProperties, " ", this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedStealthFactor().ToString(), this.GetColorFromComparison(num2, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			this.AddDonationXpTooltip();
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x00036AAA File Offset: 0x00034CAA
		// (set) Token: 0x06000CDF RID: 3295 RVA: 0x00036AB2 File Offset: 0x00034CB2
		[DataSourceProperty]
		public bool IsStealthModeActive
		{
			get
			{
				return this._isStealthModeActive;
			}
			set
			{
				if (value != this._isStealthModeActive)
				{
					this._isStealthModeActive = value;
					base.OnPropertyChangedWithValue(value, "IsStealthModeActive");
				}
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00036AD0 File Offset: 0x00034CD0
		// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x00036AD8 File Offset: 0x00034CD8
		[DataSourceProperty]
		public MBBindingList<ItemMenuTooltipPropertyVM> TargetItemProperties
		{
			get
			{
				return this._targetItemProperties;
			}
			set
			{
				if (value != this._targetItemProperties)
				{
					this._targetItemProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<ItemMenuTooltipPropertyVM>>(value, "TargetItemProperties");
				}
			}
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00036AF8 File Offset: 0x00034CF8
		private void AddDonationXpTooltip()
		{
			ItemDiscardModel itemDiscardModel = Campaign.Current.Models.ItemDiscardModel;
			int xpBonusForDiscardingItem = itemDiscardModel.GetXpBonusForDiscardingItem(this._targetItem.ItemRosterElement.EquipmentElement.Item, 1);
			int num = (this.IsComparing ? itemDiscardModel.GetXpBonusForDiscardingItem(this._comparedItem.ItemRosterElement.EquipmentElement.Item, 1) : 0);
			if (xpBonusForDiscardingItem > 0 || (this.IsComparing && num > 0))
			{
				InventoryLogic inventoryLogic = this._inventoryLogic;
				if (inventoryLogic != null && inventoryLogic.CanGainXpFromDiscarding)
				{
					MBTextManager.SetTextVariable("LEFT", GameTexts.FindText("str_inventory_donation_item_hint", null).ToString(), false);
					int num2 = (this.IsComparing ? this.CompareValues(xpBonusForDiscardingItem, num) : 0);
					this.CreateColoredProperty(this.TargetItemProperties, GameTexts.FindText("str_LEFT_colon", null).ToString(), xpBonusForDiscardingItem.ToString(), this.GetColorFromComparison(num2, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					if (this.IsComparing)
					{
						this.CreateColoredProperty(this.ComparedItemProperties, " ", num.ToString(), this.GetColorFromComparison(num2, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x00036C1A File Offset: 0x00034E1A
		// (set) Token: 0x06000CE4 RID: 3300 RVA: 0x00036C22 File Offset: 0x00034E22
		[DataSourceProperty]
		public MBBindingList<ItemMenuTooltipPropertyVM> ComparedItemProperties
		{
			get
			{
				return this._comparedItemProperties;
			}
			set
			{
				if (value != this._comparedItemProperties)
				{
					this._comparedItemProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<ItemMenuTooltipPropertyVM>>(value, "ComparedItemProperties");
				}
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00036C40 File Offset: 0x00034E40
		private Color GetColorFromComparison(int result, bool isCompared)
		{
			if (result != -1)
			{
				if (result != 1)
				{
					return Colors.Black;
				}
				if (!isCompared)
				{
					return UIColors.PositiveIndicator;
				}
				return UIColors.NegativeIndicator;
			}
			else
			{
				if (!isCompared)
				{
					return UIColors.NegativeIndicator;
				}
				return UIColors.PositiveIndicator;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x00036C6F File Offset: 0x00034E6F
		// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x00036C77 File Offset: 0x00034E77
		[DataSourceProperty]
		public MBBindingList<ItemFlagVM> TargetItemFlagList
		{
			get
			{
				return this._targetItemFlagList;
			}
			set
			{
				if (value != this._targetItemFlagList)
				{
					this._targetItemFlagList = value;
					base.OnPropertyChangedWithValue<MBBindingList<ItemFlagVM>>(value, "TargetItemFlagList");
				}
			}
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00036C98 File Offset: 0x00034E98
		private int GetHorseCategoryValue(ItemCategory itemCategory)
		{
			if (itemCategory.IsAnimal)
			{
				if (itemCategory == DefaultItemCategories.PackAnimal)
				{
					return 1;
				}
				if (itemCategory == DefaultItemCategories.Horse)
				{
					return 2;
				}
				if (itemCategory == DefaultItemCategories.WarHorse)
				{
					return 3;
				}
				if (itemCategory == DefaultItemCategories.NobleHorse)
				{
					return 4;
				}
			}
			Debug.FailedAssert("This horse item category is not defined", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Inventory\\ItemMenuVM.cs", "GetHorseCategoryValue", 1536);
			return -1;
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00036CEF File Offset: 0x00034EEF
		// (set) Token: 0x06000CEA RID: 3306 RVA: 0x00036CF7 File Offset: 0x00034EF7
		[DataSourceProperty]
		public MBBindingList<ItemFlagVM> ComparedItemFlagList
		{
			get
			{
				return this._comparedItemFlagList;
			}
			set
			{
				if (value != this._comparedItemFlagList)
				{
					this._comparedItemFlagList = value;
					base.OnPropertyChangedWithValue<MBBindingList<ItemFlagVM>>(value, "ComparedItemFlagList");
				}
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00036D18 File Offset: 0x00034F18
		private ItemMenuTooltipPropertyVM CreateProperty(MBBindingList<ItemMenuTooltipPropertyVM> targetList, string definition, string value, int textHeight = 0, HintViewModel hint = null)
		{
			ItemMenuTooltipPropertyVM itemMenuTooltipPropertyVM = new ItemMenuTooltipPropertyVM(definition, value, textHeight, false, hint, null, false);
			targetList.Add(itemMenuTooltipPropertyVM);
			return itemMenuTooltipPropertyVM;
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00036D3C File Offset: 0x00034F3C
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x00036D44 File Offset: 0x00034F44
		[DataSourceProperty]
		public int AlternativeUsageIndex
		{
			get
			{
				return this._alternativeUsageIndex;
			}
			set
			{
				if (value != this._alternativeUsageIndex)
				{
					this._alternativeUsageIndex = value;
					base.OnPropertyChangedWithValue(value, "AlternativeUsageIndex");
					this.AlternativeUsageIndexUpdated();
				}
			}
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00036D68 File Offset: 0x00034F68
		private ItemMenuTooltipPropertyVM CreateColoredProperty(MBBindingList<ItemMenuTooltipPropertyVM> targetList, string definition, string value, Color color, int textHeight = 0, HintViewModel hint = null, TooltipProperty.TooltipPropertyFlags propertyFlags = TooltipProperty.TooltipPropertyFlags.None)
		{
			if (color == Colors.Black)
			{
				this.CreateProperty(targetList, definition, value, textHeight, hint);
				return null;
			}
			ItemMenuTooltipPropertyVM itemMenuTooltipPropertyVM = new ItemMenuTooltipPropertyVM(definition, value, textHeight, color, false, hint, propertyFlags, null, false);
			targetList.Add(itemMenuTooltipPropertyVM);
			return itemMenuTooltipPropertyVM;
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00036DAE File Offset: 0x00034FAE
		// (set) Token: 0x06000CF0 RID: 3312 RVA: 0x00036DB6 File Offset: 0x00034FB6
		[DataSourceProperty]
		public MBBindingList<StringItemWithHintVM> AlternativeUsages
		{
			get
			{
				return this._alternativeUsages;
			}
			set
			{
				if (value != this._alternativeUsages)
				{
					this._alternativeUsages = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithHintVM>>(value, "AlternativeUsages");
				}
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00036DD4 File Offset: 0x00034FD4
		public void SetTransactionCost(int getItemTotalPrice, int maxIndividualPrice)
		{
			this.TransactionTotalCost = getItemTotalPrice;
			if (this._targetItem.ItemCost == maxIndividualPrice)
			{
				this._costProperty.ValueLabel = this._targetItem.ItemCost + this.GoldIcon;
				return;
			}
			if (this._targetItem.ItemCost < maxIndividualPrice)
			{
				this._costProperty.ValueLabel = string.Concat(new object[]
				{
					this._targetItem.ItemCost,
					" - ",
					maxIndividualPrice,
					this.GoldIcon
				});
				return;
			}
			this._costProperty.ValueLabel = string.Concat(new object[]
			{
				maxIndividualPrice,
				" - ",
				this._targetItem.ItemCost,
				this.GoldIcon
			});
		}

		// Token: 0x0400059F RID: 1439
		private readonly TextObject _swingDamageText = GameTexts.FindText("str_swing_damage", null);

		// Token: 0x040005A0 RID: 1440
		private readonly TextObject _swingSpeedText = new TextObject("{=345a87fcc69f626ae3916939ef2fc135}Swing Speed: ", null);

		// Token: 0x040005A1 RID: 1441
		private readonly TextObject _weaponTierText = new TextObject("{=weaponTier}Weapon Tier: ", null);

		// Token: 0x040005A2 RID: 1442
		private readonly TextObject _armorTierText = new TextObject("{=armorTier}Armor Tier: ", null);

		// Token: 0x040005A3 RID: 1443
		private readonly TextObject _horseTierText = new TextObject("{=mountTier}Mount Tier: ", null);

		// Token: 0x040005A4 RID: 1444
		private readonly TextObject _horseTypeText = new TextObject("{=9sxECG6e}Mount Type: ", null);

		// Token: 0x040005A5 RID: 1445
		private readonly TextObject _chargeDamageText = new TextObject("{=c7638a0869219ae845de0f660fd57a9d}Charge Damage: ", null);

		// Token: 0x040005A6 RID: 1446
		private readonly TextObject _hitPointsText = GameTexts.FindText("str_hit_points", null);

		// Token: 0x040005A7 RID: 1447
		private readonly TextObject _speedText = new TextObject("{=74dc1908cb0b990e80fb977b5a0ef10d}Speed: ", null);

		// Token: 0x040005A8 RID: 1448
		private readonly TextObject _maneuverText = new TextObject("{=3025020b83b218707499f0de3135ed0a}Maneuver: ", null);

		// Token: 0x040005A9 RID: 1449
		private readonly TextObject _thrustSpeedText = GameTexts.FindText("str_thrust_speed", null);

		// Token: 0x040005AA RID: 1450
		private readonly TextObject _thrustDamageText = GameTexts.FindText("str_thrust_damage", null);

		// Token: 0x040005AB RID: 1451
		private readonly TextObject _lengthText = GameTexts.FindText("str_crafting_stat", "WeaponReach");

		// Token: 0x040005AC RID: 1452
		private readonly TextObject _weightText = GameTexts.FindText("str_weight_text", null);

		// Token: 0x040005AD RID: 1453
		private readonly TextObject _handlingText = new TextObject("{=ca8b1e8956057b831dfc665f54bae4b0}Handling: ", null);

		// Token: 0x040005AE RID: 1454
		private readonly TextObject _weaponLengthText = new TextObject("{=5fa36d2798479803b4518a64beb4d732}Weapon Length: ", null);

		// Token: 0x040005AF RID: 1455
		private readonly TextObject _damageText = new TextObject("{=c9c5dfed2ca6bcb7a73d905004c97b23}Damage: ", null);

		// Token: 0x040005B0 RID: 1456
		private readonly TextObject _bonusDamageText = new TextObject("{=LIYoQWOB}Bonus Damage: ", null);

		// Token: 0x040005B1 RID: 1457
		private readonly TextObject _missileSpeedText = GameTexts.FindText("str_missile_speed", null);

		// Token: 0x040005B2 RID: 1458
		private readonly TextObject _accuracyText = new TextObject("{=5dec16fa0be433ade3c4cb0074ef366d}Accuracy: ", null);

		// Token: 0x040005B3 RID: 1459
		private readonly TextObject _stackAmountText = new TextObject("{=05fdfc6e238429753ef282f2ce97c1f8}Stack Amount: ", null);

		// Token: 0x040005B4 RID: 1460
		private readonly TextObject _ammoLimitText = new TextObject("{=6adabc1f82216992571c3e22abc164d7}Ammo Limit: ", null);

		// Token: 0x040005B5 RID: 1461
		private readonly TextObject _requiresText = new TextObject("{=154a34f8caccfc833238cc89d38861e8}Requires: ", null);

		// Token: 0x040005B6 RID: 1462
		private readonly TextObject _foodText = new TextObject("{=qSi4DlT4}Food", null);

		// Token: 0x040005B7 RID: 1463
		private readonly TextObject _partyMoraleText = new TextObject("{=a241aacb1780599430c79fd9f667b67f}Party Morale: ", null);

		// Token: 0x040005B8 RID: 1464
		private readonly TextObject _typeText = new TextObject("{=08abd5af7774d311cadc3ed900b47754}Type: ", null);

		// Token: 0x040005B9 RID: 1465
		private readonly TextObject _tradeRumorsText = new TextObject("{=f2971dc587a9777223ad2d7be236fb05}Trade Rumors", null);

		// Token: 0x040005BA RID: 1466
		private readonly TextObject _classText = new TextObject("{=8cad4a279770f269c4bb0dc7a357ee1e}Class: ", null);

		// Token: 0x040005BB RID: 1467
		private readonly TextObject _headArmorText = GameTexts.FindText("str_head_armor", null);

		// Token: 0x040005BC RID: 1468
		private readonly TextObject _horseArmorText = new TextObject("{=305cf7f98458b22e9af72b60a131714f}Horse Armor: ", null);

		// Token: 0x040005BD RID: 1469
		private readonly TextObject _bodyArmorText = GameTexts.FindText("str_body_armor", null);

		// Token: 0x040005BE RID: 1470
		private readonly TextObject _legArmorText = GameTexts.FindText("str_leg_armor", null);

		// Token: 0x040005BF RID: 1471
		private readonly TextObject _armArmorText = new TextObject("{=cf61cce254c7dca65be9bebac7fb9bf5}Arm Armor: ", null);

		// Token: 0x040005C0 RID: 1472
		private readonly TextObject _stealthBonusText = new TextObject("{=YJkAqExw}Stealth Bonus: ", null);

		// Token: 0x040005C1 RID: 1473
		private readonly TextObject _bannerEffectText = new TextObject("{=DbXZjPdf}Banner Effect: ", null);

		// Token: 0x040005C2 RID: 1474
		private readonly TextObject _noneText = new TextObject("{=koX9okuG}None", null);

		// Token: 0x040005C3 RID: 1475
		private readonly string GoldIcon = "<img src=\"General\\Icons\\Coin@2x\" extend=\"6\"/>";

		// Token: 0x040005C4 RID: 1476
		private readonly Color ConsumableColor = Color.FromUint(4290873921U);

		// Token: 0x040005C5 RID: 1477
		private readonly Color TitleColor = Color.FromUint(4293446041U);

		// Token: 0x040005C6 RID: 1478
		private TooltipProperty _costProperty;

		// Token: 0x040005C7 RID: 1479
		private InventoryLogic _inventoryLogic;

		// Token: 0x040005C8 RID: 1480
		private Action<ItemVM, int> _resetComparedItems;

		// Token: 0x040005C9 RID: 1481
		private readonly Func<WeaponComponentData, ItemObject.ItemUsageSetFlags> _getItemUsageSetFlags;

		// Token: 0x040005CA RID: 1482
		private readonly Func<EquipmentIndex, SPItemVM> _getEquipmentAtIndex;

		// Token: 0x040005CB RID: 1483
		private int _lastComparedItemVersion;

		// Token: 0x040005CC RID: 1484
		private ItemVM _targetItem;

		// Token: 0x040005CD RID: 1485
		private bool _isComparing;

		// Token: 0x040005CE RID: 1486
		private bool _isStealthModeActive;

		// Token: 0x040005CF RID: 1487
		private ItemVM _comparedItem;

		// Token: 0x040005D0 RID: 1488
		private bool _isPlayerItem;

		// Token: 0x040005D1 RID: 1489
		private BasicCharacterObject _character;

		// Token: 0x040005D2 RID: 1490
		private ItemImageIdentifierVM _imageIdentifier;

		// Token: 0x040005D3 RID: 1491
		private ItemImageIdentifierVM _comparedImageIdentifier;

		// Token: 0x040005D4 RID: 1492
		private string _itemName;

		// Token: 0x040005D5 RID: 1493
		private string _comparedItemName;

		// Token: 0x040005D6 RID: 1494
		private MBBindingList<ItemMenuTooltipPropertyVM> _comparedItemProperties;

		// Token: 0x040005D7 RID: 1495
		private MBBindingList<ItemMenuTooltipPropertyVM> _targetItemProperties;

		// Token: 0x040005D8 RID: 1496
		private bool _isInitializationOver;

		// Token: 0x040005D9 RID: 1497
		private int _transactionTotalCost = -1;

		// Token: 0x040005DA RID: 1498
		private MBBindingList<ItemFlagVM> _targetItemFlagList;

		// Token: 0x040005DB RID: 1499
		private MBBindingList<ItemFlagVM> _comparedItemFlagList;

		// Token: 0x040005DC RID: 1500
		private int _alternativeUsageIndex;

		// Token: 0x040005DD RID: 1501
		private MBBindingList<StringItemWithHintVM> _alternativeUsages;

		// Token: 0x040005DE RID: 1502
		private ITradeRumorCampaignBehavior _tradeRumorsBehavior;
	}
}
