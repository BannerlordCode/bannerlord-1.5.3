using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x0200009F RID: 159
	public class SPItemVM : ItemVM
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x0003E272 File Offset: 0x0003C472
		// (set) Token: 0x06000ED5 RID: 3797 RVA: 0x0003E27A File Offset: 0x0003C47A
		public InventoryLogic.InventorySide InventorySide { get; private set; }

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0003E283 File Offset: 0x0003C483
		public SPItemVM()
		{
			base.StringId = "";
			base.ImageIdentifier = new ItemImageIdentifierVM(null, "");
			this._itemType = EquipmentIndex.None;
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0003E2B8 File Offset: 0x0003C4B8
		public SPItemVM(InventoryLogic inventoryLogic, bool isHeroFemale, bool canCharacterUseItem, InventoryScreenHelper.InventoryMode usageType, ItemRosterElement newItem, InventoryLogic.InventorySide inventorySide, int itemCost = 0, EquipmentIndex? itemType = -1)
		{
			if (newItem.EquipmentElement.Item == null)
			{
				return;
			}
			this._usageType = usageType;
			this._tradeGoodConceptObj = Concept.All.SingleOrDefault<Concept>((Concept c) => c.StringId == "str_game_objects_trade_goods");
			this._itemConceptObj = Concept.All.SingleOrDefault<Concept>((Concept c) => c.StringId == "str_game_objects_item");
			this._inventoryLogic = inventoryLogic;
			this.ItemRosterElement = new ItemRosterElement(newItem.EquipmentElement, newItem.Amount);
			base.ItemCost = itemCost;
			this.ItemCount = newItem.Amount;
			this.TransactionCount = 1;
			this.ItemLevel = newItem.EquipmentElement.Item.Difficulty;
			this.InventorySide = inventorySide;
			if (itemType != null)
			{
				EquipmentIndex? equipmentIndex = itemType;
				EquipmentIndex equipmentIndex2 = EquipmentIndex.None;
				if (!((equipmentIndex.GetValueOrDefault() == equipmentIndex2) & (equipmentIndex != null)))
				{
					this._itemType = itemType.Value;
				}
			}
			base.OnItemTypeUpdated();
			base.ItemDescription = newItem.EquipmentElement.GetModifiedItemName().ToString();
			base.StringId = CampaignUIHelper.GetItemLockStringID(newItem.EquipmentElement);
			ItemObject item = newItem.EquipmentElement.Item;
			Clan playerClan = Clan.PlayerClan;
			base.ImageIdentifier = new ItemImageIdentifierVM(item, (playerClan != null) ? playerClan.Banner.Serialize() : null);
			this.IsCivilianItem = newItem.EquipmentElement.Item.ItemFlags.HasAnyFlag(ItemFlags.Civilian);
			this.IsStealthItem = newItem.EquipmentElement.Item.ItemFlags.HasAnyFlag(ItemFlags.Stealth);
			this.IsGenderDifferent = (isHeroFemale && this.ItemRosterElement.EquipmentElement.Item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByFemale)) || (!isHeroFemale && this.ItemRosterElement.EquipmentElement.Item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByMale));
			this.CanCharacterUseItem = canCharacterUseItem;
			this.IsArtifact = newItem.EquipmentElement.Item.IsUniqueItem;
			InventoryLogic inventoryLogic2 = this._inventoryLogic;
			this.CanBeDonated = inventoryLogic2 != null && inventoryLogic2.CanDonateItem(this.ItemRosterElement, this.InventorySide);
			this.TradeData = new InventoryTradeVM(this._inventoryLogic, this.ItemRosterElement, inventorySide, new Action<int, bool>(this.OnTradeApplyTransaction));
			InventoryState activeInventoryState = InventoryScreenHelper.GetActiveInventoryState();
			InventoryScreenHelper.InventoryMode inventoryMode = ((activeInventoryState != null) ? activeInventoryState.InventoryMode : InventoryScreenHelper.InventoryMode.Default);
			this.IsTransferable = !this.ItemRosterElement.EquipmentElement.IsQuestItem && this.ItemRosterElement.EquipmentElement.Item.IsTransferable && (inventoryMode != InventoryScreenHelper.InventoryMode.Warehouse || this.ItemRosterElement.EquipmentElement.Item.IsTradeGood);
			this.TradeData.IsTradeable = this.IsTransferable;
			this.IsEquipableItem = (InventoryScreenHelper.GetInventoryItemTypeOfItem(newItem.EquipmentElement.Item) & InventoryScreenHelper.InventoryItemType.Equipable) > InventoryScreenHelper.InventoryItemType.None;
			this.UpdateCanBeSlaughtered();
			this.UpdateHintTexts();
			this.UpdateProfitType();
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0003E5F4 File Offset: 0x0003C7F4
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.ItemRosterElement.EquipmentElement.Item != null)
			{
				TextObject modifiedItemName = this.ItemRosterElement.EquipmentElement.GetModifiedItemName();
				base.ItemDescription = ((modifiedItemName != null) ? modifiedItemName.ToString() : null) ?? "";
				return;
			}
			base.ItemDescription = "";
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0003E658 File Offset: 0x0003C858
		public void RefreshWith(SPItemVM itemVM, InventoryLogic.InventorySide inventorySide)
		{
			this.InventorySide = inventorySide;
			if (itemVM == null)
			{
				this.Reset();
				return;
			}
			base.ItemDescription = itemVM.ItemDescription;
			base.ItemCost = itemVM.ItemCost;
			base.TypeName = itemVM.TypeName;
			this._itemType = itemVM.ItemType;
			this.ItemCount = itemVM.ItemCount;
			this.TransactionCount = itemVM.TransactionCount;
			this.ItemLevel = itemVM.ItemLevel;
			base.StringId = itemVM.StringId;
			base.ImageIdentifier = itemVM.ImageIdentifier.Clone();
			this.ItemRosterElement = itemVM.ItemRosterElement;
			this.IsCivilianItem = itemVM.IsCivilianItem;
			this.IsStealthItem = itemVM.IsStealthItem;
			this.IsGenderDifferent = itemVM.IsGenderDifferent;
			this.IsEquipableItem = itemVM.IsEquipableItem;
			this.CanCharacterUseItem = this.CanCharacterUseItem;
			this.IsArtifact = itemVM.IsArtifact;
			this.IsSelected = itemVM.IsSelected;
			this._inventoryLogic = itemVM._inventoryLogic;
			this.IsTransferable = itemVM.IsTransferable;
			this.UpdateCanBeSlaughtered();
			this.UpdateHintTexts();
			InventoryLogic inventoryLogic = this._inventoryLogic;
			this.CanBeDonated = inventoryLogic != null && inventoryLogic.CanDonateItem(this.ItemRosterElement, this.InventorySide);
			this.TradeData = new InventoryTradeVM(this._inventoryLogic, itemVM.ItemRosterElement, inventorySide, new Action<int, bool>(this.OnTradeApplyTransaction));
			this.UpdateProfitType();
			int version = base.Version;
			base.Version = version + 1;
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0003E7CC File Offset: 0x0003C9CC
		private void Reset()
		{
			base.ItemDescription = "";
			base.ItemCost = 0;
			base.TypeName = "";
			this._itemType = EquipmentIndex.None;
			this.ItemCount = 0;
			this.TransactionCount = 0;
			this.ItemLevel = 0;
			base.StringId = "";
			base.ImageIdentifier = new ItemImageIdentifierVM(null, "");
			this.ItemRosterElement = default(ItemRosterElement);
			this.ProfitType = 0;
			this.IsCivilianItem = true;
			this.IsStealthItem = false;
			this.IsGenderDifferent = false;
			this.IsEquipableItem = true;
			this.IsArtifact = false;
			this.IsSelected = false;
			this.TradeData = new InventoryTradeVM(this._inventoryLogic, this.ItemRosterElement, InventoryLogic.InventorySide.None, new Action<int, bool>(this.OnTradeApplyTransaction));
			int version = base.Version;
			base.Version = version + 1;
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0003E8A0 File Offset: 0x0003CAA0
		private void UpdateProfitType()
		{
			this.ProfitType = 0;
			if (Campaign.Current != null)
			{
				InventoryState activeInventoryState = InventoryScreenHelper.GetActiveInventoryState();
				if (((activeInventoryState != null) ? activeInventoryState.InventoryMode : InventoryScreenHelper.InventoryMode.Default) == InventoryScreenHelper.InventoryMode.Trade)
				{
					if (this.InventorySide == InventoryLogic.InventorySide.PlayerInventory)
					{
						Hero mainHero = Hero.MainHero;
						if (mainHero == null || !mainHero.GetPerkValue(DefaultPerks.Trade.Appraiser))
						{
							Hero mainHero2 = Hero.MainHero;
							if (mainHero2 == null || !mainHero2.GetPerkValue(DefaultPerks.Trade.WholeSeller))
							{
								return;
							}
						}
						IPlayerTradeBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IPlayerTradeBehavior>();
						if (campaignBehavior != null)
						{
							int num = -campaignBehavior.GetProjectedProfit(this.ItemRosterElement, base.ItemCost) + base.ItemCost;
							this.ProfitType = (int)SPItemVM.GetProfitTypeFromDiff((float)num, (float)base.ItemCost);
							return;
						}
					}
					else if (this.InventorySide == InventoryLogic.InventorySide.OtherInventory && Settlement.CurrentSettlement != null && (Settlement.CurrentSettlement.IsFortification || Settlement.CurrentSettlement.IsVillage))
					{
						Hero mainHero3 = Hero.MainHero;
						if (mainHero3 == null || !mainHero3.GetPerkValue(DefaultPerks.Trade.CaravanMaster))
						{
							Hero mainHero4 = Hero.MainHero;
							if (mainHero4 == null || !mainHero4.GetPerkValue(DefaultPerks.Trade.MarketDealer))
							{
								return;
							}
						}
						float averagePriceFactorItemCategory = this._inventoryLogic.GetAveragePriceFactorItemCategory(this.ItemRosterElement.EquipmentElement.Item.ItemCategory);
						Town town = (Settlement.CurrentSettlement.IsVillage ? Settlement.CurrentSettlement.Village.Bound.Town : Settlement.CurrentSettlement.Town);
						if (averagePriceFactorItemCategory != -99f)
						{
							this.ProfitType = (int)SPItemVM.GetProfitTypeFromDiff(town.MarketData.GetPriceFactor(this.ItemRosterElement.EquipmentElement.Item.ItemCategory), averagePriceFactorItemCategory);
						}
					}
				}
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0003EA3F File Offset: 0x0003CC3F
		public void ExecuteBuySingle()
		{
			this.ExecuteBuy(1);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0003EA48 File Offset: 0x0003CC48
		public void ExecuteBuy(int amount)
		{
			this.TransactionCount = amount;
			ItemVM.ProcessBuyItem(this, false);
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0003EA5D File Offset: 0x0003CC5D
		public void ExecuteSellSingle()
		{
			this.ExecuteSell(1);
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0003EA66 File Offset: 0x0003CC66
		public void ExecuteSell(int amount)
		{
			this.TransactionCount = amount;
			SPItemVM.ProcessSellItem(this, false);
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0003EA7B File Offset: 0x0003CC7B
		private void OnTradeApplyTransaction(int amount, bool isBuying)
		{
			this.TransactionCount = amount;
			if (isBuying)
			{
				ItemVM.ProcessBuyItem(this, true);
				return;
			}
			SPItemVM.ProcessSellItem(this, true);
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0003EAA0 File Offset: 0x0003CCA0
		public void ExecuteSellItem()
		{
			SPItemVM.ProcessSellItem(this, false);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0003EAB0 File Offset: 0x0003CCB0
		public void ExecuteConcept()
		{
			if (this._tradeGoodConceptObj != null)
			{
				ItemObject item = this.ItemRosterElement.EquipmentElement.Item;
				if (item != null && item.Type == ItemObject.ItemTypeEnum.Goods)
				{
					Campaign.Current.EncyclopediaManager.GoToLink(this._tradeGoodConceptObj.EncyclopediaLink);
					return;
				}
			}
			if (this._itemConceptObj != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._itemConceptObj.EncyclopediaLink);
			}
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0003EB27 File Offset: 0x0003CD27
		public void ExecuteResetTrade()
		{
			this.TradeData.ExecuteReset();
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0003EB34 File Offset: 0x0003CD34
		private void UpdateTotalCost()
		{
			if (this.TransactionCount <= 0 || this._inventoryLogic == null || InventoryLogic.IsEquipmentSide(this.InventorySide))
			{
				return;
			}
			int num;
			this.TotalCost = this._inventoryLogic.GetItemTotalPrice(this.ItemRosterElement, this.TransactionCount, out num, this.InventorySide == InventoryLogic.InventorySide.OtherInventory);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0003EB88 File Offset: 0x0003CD88
		public void UpdateTradeData(bool forceUpdateAmounts)
		{
			InventoryTradeVM tradeData = this.TradeData;
			if (tradeData != null)
			{
				tradeData.UpdateItemData(this.ItemRosterElement, this.InventorySide, forceUpdateAmounts);
			}
			this.UpdateProfitType();
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0003EBAE File Offset: 0x0003CDAE
		public void ExecuteSlaughterItem()
		{
			if (this.CanBeSlaughtered)
			{
				SPItemVM.ProcessItemSlaughter(this);
			}
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0003EBC3 File Offset: 0x0003CDC3
		public void ExecuteDonateItem()
		{
			if (this.CanBeDonated)
			{
				SPItemVM.ProcessItemDonate(this);
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0003EBD8 File Offset: 0x0003CDD8
		public void ExecuteSetFocused()
		{
			this.IsFocused = true;
			Action<SPItemVM> onFocus = SPItemVM.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(this);
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0003EBF1 File Offset: 0x0003CDF1
		public void ExecuteSetUnfocused()
		{
			this.IsFocused = false;
			Action<SPItemVM> onFocus = SPItemVM.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(null);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0003EC0C File Offset: 0x0003CE0C
		public void UpdateCanBeSlaughtered()
		{
			InventoryLogic inventoryLogic = this._inventoryLogic;
			this.CanBeSlaughtered = inventoryLogic != null && inventoryLogic.CanSlaughterItem(this.ItemRosterElement, this.InventorySide) && !this.ItemRosterElement.EquipmentElement.IsQuestItem;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0003EC58 File Offset: 0x0003CE58
		private string GetStackModifierString()
		{
			if (InventoryLogic.IsEquipmentSide(this.InventorySide))
			{
				return string.Empty;
			}
			InventoryLogic inventoryLogic = this._inventoryLogic;
			TextObject textObject = ((((inventoryLogic != null) ? inventoryLogic.OtherParty : null) == null) ? GameTexts.FindText("str_entire_stack_shortcut_discard", null) : GameTexts.FindText("str_entire_stack_shortcut_transfer", null));
			InventoryLogic inventoryLogic2 = this._inventoryLogic;
			TextObject textObject2 = ((((inventoryLogic2 != null) ? inventoryLogic2.OtherParty : null) == null) ? GameTexts.FindText("str_five_stack_shortcut_discard", null) : GameTexts.FindText("str_five_stack_shortcut_transfer", null));
			return CampaignUIHelper.GetStackModifierString(textObject, textObject2, this.ItemCount >= 5);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0003ECE4 File Offset: 0x0003CEE4
		private string GetTextWithStackModifierText(string mainText)
		{
			string stackModifierString = this.GetStackModifierString();
			if (string.IsNullOrEmpty(stackModifierString))
			{
				return mainText;
			}
			return GameTexts.FindText("str_string_newline_string", null).SetTextVariable("STR1", mainText).SetTextVariable("STR2", stackModifierString)
				.ToString();
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0003ED28 File Offset: 0x0003CF28
		public void UpdateHintTexts()
		{
			base.SlaughterHint = new BasicTooltipViewModel(() => this.GetTextWithStackModifierText(GameTexts.FindText("str_inventory_slaughter", null).ToString()));
			base.DonateHint = new BasicTooltipViewModel(() => this.GetTextWithStackModifierText(GameTexts.FindText("str_inventory_donate", null).ToString()));
			base.PreviewHint = new HintViewModel(GameTexts.FindText("str_inventory_preview", null), null);
			base.EquipHint = new HintViewModel(GameTexts.FindText("str_inventory_equip", null), null);
			base.UnequipHint = new HintViewModel(GameTexts.FindText("str_inventory_unequip", null), null);
			base.LockHint = new HintViewModel(GameTexts.FindText("str_lock_in_inventory", null).SetTextVariable("TRANSFERABLE", GameTexts.FindText("str_items", null).ToString()), null);
			if (this._usageType == InventoryScreenHelper.InventoryMode.Loot || this._usageType == InventoryScreenHelper.InventoryMode.Stash)
			{
				base.BuyAndEquipHint = new BasicTooltipViewModel(() => GameTexts.FindText("str_inventory_take_and_equip", null).ToString());
				base.BuyHint = new BasicTooltipViewModel(() => this.GetTextWithStackModifierText(GameTexts.FindText("str_take", null).ToString()));
			}
			else if (this._usageType == InventoryScreenHelper.InventoryMode.Default)
			{
				base.BuyAndEquipHint = new BasicTooltipViewModel(() => GameTexts.FindText("str_inventory_take_and_equip", null).ToString());
				base.BuyHint = new BasicTooltipViewModel(() => this.GetTextWithStackModifierText(GameTexts.FindText("str_take", null).ToString()));
			}
			else
			{
				base.BuyAndEquipHint = new BasicTooltipViewModel(() => GameTexts.FindText("str_inventory_buy_and_equip", null).ToString());
				base.BuyHint = new BasicTooltipViewModel(() => this.GetTextWithStackModifierText(GameTexts.FindText("str_inventory_buy", null).ToString()));
			}
			if (!this.IsTransferable)
			{
				base.SellHint = new BasicTooltipViewModel(() => new TextObject("{=8xKky9ja}This item cannot be traded or discarded", null).ToString());
				return;
			}
			if (this._usageType == InventoryScreenHelper.InventoryMode.Loot || this._usageType == InventoryScreenHelper.InventoryMode.Stash)
			{
				base.SellHint = new BasicTooltipViewModel(() => this.GetTextWithStackModifierText(GameTexts.FindText("str_give", null).ToString()));
				return;
			}
			if (this._usageType == InventoryScreenHelper.InventoryMode.Default)
			{
				base.SellHint = new BasicTooltipViewModel(() => this.GetTextWithStackModifierText(GameTexts.FindText("str_inventory_discard", null).ToString()));
				return;
			}
			base.SellHint = new BasicTooltipViewModel(() => this.GetTextWithStackModifierText(GameTexts.FindText("str_inventory_sell", null).ToString()));
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0003EF51 File Offset: 0x0003D151
		public static SPItemVM.ProfitTypes GetProfitTypeFromDiff(float averageValue, float currentValue)
		{
			if (averageValue == 0f)
			{
				return SPItemVM.ProfitTypes.Default;
			}
			if (averageValue < currentValue * 0.8f)
			{
				return SPItemVM.ProfitTypes.HighProfit;
			}
			if (averageValue < currentValue * 0.95f)
			{
				return SPItemVM.ProfitTypes.Profit;
			}
			if (averageValue > currentValue * 1.05f)
			{
				return SPItemVM.ProfitTypes.Loss;
			}
			if (averageValue > currentValue * 1.2f)
			{
				return SPItemVM.ProfitTypes.HighLoss;
			}
			return SPItemVM.ProfitTypes.Default;
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06000EEF RID: 3823 RVA: 0x0003EF8F File Offset: 0x0003D18F
		// (set) Token: 0x06000EF0 RID: 3824 RVA: 0x0003EF97 File Offset: 0x0003D197
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0003EFB5 File Offset: 0x0003D1B5
		// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x0003EFBD File Offset: 0x0003D1BD
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0003EFDB File Offset: 0x0003D1DB
		// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x0003EFE3 File Offset: 0x0003D1E3
		[DataSourceProperty]
		public bool IsArtifact
		{
			get
			{
				return this._isArtifact;
			}
			set
			{
				if (value != this._isArtifact)
				{
					this._isArtifact = value;
					base.OnPropertyChangedWithValue(value, "IsArtifact");
				}
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0003F001 File Offset: 0x0003D201
		// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x0003F009 File Offset: 0x0003D209
		[DataSourceProperty]
		public bool IsTransferable
		{
			get
			{
				return this._isTransferable;
			}
			set
			{
				if (value != this._isTransferable)
				{
					this._isTransferable = value;
					base.OnPropertyChangedWithValue(value, "IsTransferable");
				}
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0003F027 File Offset: 0x0003D227
		// (set) Token: 0x06000EF8 RID: 3832 RVA: 0x0003F02F File Offset: 0x0003D22F
		[DataSourceProperty]
		public bool IsTransferButtonHighlighted
		{
			get
			{
				return this._isTransferButtonHighlighted;
			}
			set
			{
				if (value != this._isTransferButtonHighlighted)
				{
					this._isTransferButtonHighlighted = value;
					base.OnPropertyChangedWithValue(value, "IsTransferButtonHighlighted");
				}
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x0003F04D File Offset: 0x0003D24D
		// (set) Token: 0x06000EFA RID: 3834 RVA: 0x0003F055 File Offset: 0x0003D255
		[DataSourceProperty]
		public bool IsItemHighlightEnabled
		{
			get
			{
				return this._isItemHighlightEnabled;
			}
			set
			{
				if (value != this._isItemHighlightEnabled)
				{
					this._isItemHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsItemHighlightEnabled");
				}
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0003F073 File Offset: 0x0003D273
		// (set) Token: 0x06000EFC RID: 3836 RVA: 0x0003F07B File Offset: 0x0003D27B
		[DataSourceProperty]
		public bool IsCivilianItem
		{
			get
			{
				return this._isCivilianItem;
			}
			set
			{
				if (value != this._isCivilianItem)
				{
					this._isCivilianItem = value;
					base.OnPropertyChangedWithValue(value, "IsCivilianItem");
				}
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x0003F099 File Offset: 0x0003D299
		// (set) Token: 0x06000EFE RID: 3838 RVA: 0x0003F0A1 File Offset: 0x0003D2A1
		[DataSourceProperty]
		public bool IsStealthItem
		{
			get
			{
				return this._isStealthItem;
			}
			set
			{
				if (value != this._isStealthItem)
				{
					this._isStealthItem = value;
					base.OnPropertyChangedWithValue(value, "IsStealthItem");
				}
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0003F0BF File Offset: 0x0003D2BF
		// (set) Token: 0x06000F00 RID: 3840 RVA: 0x0003F0C7 File Offset: 0x0003D2C7
		[DataSourceProperty]
		public bool IsNew
		{
			get
			{
				return this._isNew;
			}
			set
			{
				if (value != this._isNew)
				{
					this._isNew = value;
					base.OnPropertyChangedWithValue(value, "IsNew");
				}
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x0003F0E5 File Offset: 0x0003D2E5
		// (set) Token: 0x06000F02 RID: 3842 RVA: 0x0003F0ED File Offset: 0x0003D2ED
		[DataSourceProperty]
		public bool IsGenderDifferent
		{
			get
			{
				return this._isGenderDifferent;
			}
			set
			{
				if (value != this._isGenderDifferent)
				{
					this._isGenderDifferent = value;
					base.OnPropertyChangedWithValue(value, "IsGenderDifferent");
				}
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x0003F10B File Offset: 0x0003D30B
		// (set) Token: 0x06000F04 RID: 3844 RVA: 0x0003F113 File Offset: 0x0003D313
		[DataSourceProperty]
		public bool CanBeSlaughtered
		{
			get
			{
				return this._canBeSlaughtered;
			}
			set
			{
				if (value != this._canBeSlaughtered)
				{
					this._canBeSlaughtered = value;
					base.OnPropertyChangedWithValue(value, "CanBeSlaughtered");
				}
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x0003F131 File Offset: 0x0003D331
		// (set) Token: 0x06000F06 RID: 3846 RVA: 0x0003F139 File Offset: 0x0003D339
		[DataSourceProperty]
		public bool CanBeDonated
		{
			get
			{
				return this._canBeDonated;
			}
			set
			{
				if (value != this._canBeDonated)
				{
					this._canBeDonated = value;
					base.OnPropertyChangedWithValue(value, "CanBeDonated");
				}
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x0003F157 File Offset: 0x0003D357
		// (set) Token: 0x06000F08 RID: 3848 RVA: 0x0003F15F File Offset: 0x0003D35F
		[DataSourceProperty]
		public bool IsEquipableItem
		{
			get
			{
				return this._isEquipableItem;
			}
			set
			{
				if (value != this._isEquipableItem)
				{
					this._isEquipableItem = value;
					base.OnPropertyChangedWithValue(value, "IsEquipableItem");
				}
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x0003F17D File Offset: 0x0003D37D
		// (set) Token: 0x06000F0A RID: 3850 RVA: 0x0003F185 File Offset: 0x0003D385
		[DataSourceProperty]
		public bool CanCharacterUseItem
		{
			get
			{
				return this._canCharacterUseItem;
			}
			set
			{
				if (value != this._canCharacterUseItem)
				{
					this._canCharacterUseItem = value;
					base.OnPropertyChangedWithValue(value, "CanCharacterUseItem");
				}
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x0003F1A3 File Offset: 0x0003D3A3
		// (set) Token: 0x06000F0C RID: 3852 RVA: 0x0003F1AB File Offset: 0x0003D3AB
		[DataSourceProperty]
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				if (value != this._isLocked)
				{
					this._isLocked = value;
					base.OnPropertyChangedWithValue(value, "IsLocked");
					SPItemVM.ProcessLockItem(this, value);
				}
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0003F1D5 File Offset: 0x0003D3D5
		// (set) Token: 0x06000F0E RID: 3854 RVA: 0x0003F1DD File Offset: 0x0003D3DD
		[DataSourceProperty]
		public int ItemCount
		{
			get
			{
				return this._count;
			}
			set
			{
				if (value != this._count)
				{
					this._count = value;
					base.OnPropertyChangedWithValue(value, "ItemCount");
					this.UpdateTotalCost();
					this.UpdateTradeData(false);
				}
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x0003F208 File Offset: 0x0003D408
		// (set) Token: 0x06000F10 RID: 3856 RVA: 0x0003F210 File Offset: 0x0003D410
		[DataSourceProperty]
		public int ItemLevel
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue(value, "ItemLevel");
				}
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x0003F22E File Offset: 0x0003D42E
		// (set) Token: 0x06000F12 RID: 3858 RVA: 0x0003F236 File Offset: 0x0003D436
		[DataSourceProperty]
		public int ProfitType
		{
			get
			{
				return this._profitType;
			}
			set
			{
				if (value != this._profitType)
				{
					this._profitType = value;
					base.OnPropertyChangedWithValue(value, "ProfitType");
				}
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0003F254 File Offset: 0x0003D454
		// (set) Token: 0x06000F14 RID: 3860 RVA: 0x0003F25C File Offset: 0x0003D45C
		[DataSourceProperty]
		public int TransactionCount
		{
			get
			{
				return this._transactionCount;
			}
			set
			{
				if (value != this._transactionCount)
				{
					this._transactionCount = value;
					base.OnPropertyChangedWithValue(value, "TransactionCount");
					this.UpdateTotalCost();
				}
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x0003F280 File Offset: 0x0003D480
		// (set) Token: 0x06000F16 RID: 3862 RVA: 0x0003F288 File Offset: 0x0003D488
		[DataSourceProperty]
		public int TotalCost
		{
			get
			{
				return this._totalCost;
			}
			set
			{
				if (value != this._totalCost)
				{
					this._totalCost = value;
					base.OnPropertyChangedWithValue(value, "TotalCost");
				}
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0003F2A6 File Offset: 0x0003D4A6
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x0003F2AE File Offset: 0x0003D4AE
		[DataSourceProperty]
		public InventoryTradeVM TradeData
		{
			get
			{
				return this._tradeData;
			}
			set
			{
				if (value != this._tradeData)
				{
					this._tradeData = value;
					base.OnPropertyChangedWithValue<InventoryTradeVM>(value, "TradeData");
				}
			}
		}

		// Token: 0x040006AD RID: 1709
		public static Action<SPItemVM> OnFocus;

		// Token: 0x040006AE RID: 1710
		public static Action<SPItemVM, bool> ProcessSellItem;

		// Token: 0x040006AF RID: 1711
		public static Action<SPItemVM> ProcessItemSlaughter;

		// Token: 0x040006B0 RID: 1712
		public static Action<SPItemVM> ProcessItemDonate;

		// Token: 0x040006B1 RID: 1713
		public static Action<SPItemVM, bool> ProcessLockItem;

		// Token: 0x040006B2 RID: 1714
		private readonly InventoryScreenHelper.InventoryMode _usageType;

		// Token: 0x040006B3 RID: 1715
		private Concept _tradeGoodConceptObj;

		// Token: 0x040006B4 RID: 1716
		private Concept _itemConceptObj;

		// Token: 0x040006B6 RID: 1718
		private InventoryLogic _inventoryLogic;

		// Token: 0x040006B7 RID: 1719
		private bool _isFocused;

		// Token: 0x040006B8 RID: 1720
		private bool _isSelected;

		// Token: 0x040006B9 RID: 1721
		private int _level;

		// Token: 0x040006BA RID: 1722
		private bool _isTransferable;

		// Token: 0x040006BB RID: 1723
		private bool _isCivilianItem;

		// Token: 0x040006BC RID: 1724
		private bool _isStealthItem;

		// Token: 0x040006BD RID: 1725
		private bool _isGenderDifferent;

		// Token: 0x040006BE RID: 1726
		private bool _isEquipableItem;

		// Token: 0x040006BF RID: 1727
		private bool _canCharacterUseItem;

		// Token: 0x040006C0 RID: 1728
		private bool _isLocked;

		// Token: 0x040006C1 RID: 1729
		private bool _isArtifact;

		// Token: 0x040006C2 RID: 1730
		private bool _canBeSlaughtered;

		// Token: 0x040006C3 RID: 1731
		private bool _canBeDonated;

		// Token: 0x040006C4 RID: 1732
		private int _count;

		// Token: 0x040006C5 RID: 1733
		private int _profitType = -5;

		// Token: 0x040006C6 RID: 1734
		private int _transactionCount;

		// Token: 0x040006C7 RID: 1735
		private int _totalCost;

		// Token: 0x040006C8 RID: 1736
		private bool _isTransferButtonHighlighted;

		// Token: 0x040006C9 RID: 1737
		private bool _isItemHighlightEnabled;

		// Token: 0x040006CA RID: 1738
		private bool _isNew;

		// Token: 0x040006CB RID: 1739
		private InventoryTradeVM _tradeData;

		// Token: 0x02000213 RID: 531
		public enum ProfitTypes
		{
			// Token: 0x040011EB RID: 4587
			HighLoss = -2,
			// Token: 0x040011EC RID: 4588
			Loss,
			// Token: 0x040011ED RID: 4589
			Default,
			// Token: 0x040011EE RID: 4590
			Profit,
			// Token: 0x040011EF RID: 4591
			HighProfit
		}
	}
}
