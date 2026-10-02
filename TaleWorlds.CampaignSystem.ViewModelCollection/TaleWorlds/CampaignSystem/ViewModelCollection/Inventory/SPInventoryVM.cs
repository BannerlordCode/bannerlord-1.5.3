using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x0200009A RID: 154
	public class SPInventoryVM : ViewModel
	{
		// Token: 0x06000D1C RID: 3356 RVA: 0x000375E1 File Offset: 0x000357E1
		private InventoryLogic.InventorySide GetEquipmentToInventorySide(SPInventoryVM.EquipmentModes equipmentMode)
		{
			switch (equipmentMode)
			{
			case SPInventoryVM.EquipmentModes.Civilian:
				return InventoryLogic.InventorySide.CivilianEquipment;
			case SPInventoryVM.EquipmentModes.Battle:
				return InventoryLogic.InventorySide.BattleEquipment;
			case SPInventoryVM.EquipmentModes.Stealth:
				return InventoryLogic.InventorySide.StealthEquipment;
			default:
				return InventoryLogic.InventorySide.None;
			}
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00037600 File Offset: 0x00035800
		public SPInventoryVM(InventoryLogic inventoryLogic, bool isInCivilianModeByDefault, Func<WeaponComponentData, ItemObject.ItemUsageSetFlags> getItemUsageSetFlags)
		{
			this.IsSearchAvailable = true;
			InventoryState activeInventoryState = InventoryScreenHelper.GetActiveInventoryState();
			this._usageType = ((activeInventoryState != null) ? activeInventoryState.InventoryMode : InventoryScreenHelper.InventoryMode.Default);
			this._inventoryLogic = inventoryLogic;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this._getItemUsageSetFlags = getItemUsageSetFlags;
			this._filters = new Dictionary<SPInventoryVM.Filters, List<int>>();
			this._filters.Add(SPInventoryVM.Filters.All, this._everyItemType);
			this._filters.Add(SPInventoryVM.Filters.Weapons, this._weaponItemTypes);
			this._filters.Add(SPInventoryVM.Filters.Armors, this._armorItemTypes);
			this._filters.Add(SPInventoryVM.Filters.Mounts, this._mountItemTypes);
			this._filters.Add(SPInventoryVM.Filters.ShieldsAndRanged, this._shieldAndRangedItemTypes);
			this._filters.Add(SPInventoryVM.Filters.Miscellaneous, this._miscellaneousItemTypes);
			this._equipAfterTransferStack = new Stack<SPItemVM>();
			this._comparedItemList = new List<ItemVM>();
			this._donationMaxShareableXp = MobilePartyHelper.GetMaximumXpAmountPartyCanGet(MobileParty.MainParty);
			MBTextManager.SetTextVariable("XP_DONATION_LIMIT", this._donationMaxShareableXp);
			if (this._inventoryLogic != null)
			{
				this._currentCharacter = this._inventoryLogic.InitialEquipmentCharacter;
				this._isTrading = inventoryLogic.IsTrading;
				this._inventoryLogic.AfterReset += this.AfterReset;
				InventoryLogic inventoryLogic2 = this._inventoryLogic;
				inventoryLogic2.TotalAmountChange = (Action<int>)Delegate.Combine(inventoryLogic2.TotalAmountChange, new Action<int>(this.OnTotalAmountChange));
				InventoryLogic inventoryLogic3 = this._inventoryLogic;
				inventoryLogic3.DonationXpChange = (Action)Delegate.Combine(inventoryLogic3.DonationXpChange, new Action(this.OnDonationXpChange));
				this._inventoryLogic.AfterTransfer += this.AfterTransfer;
				this._rightTroopRoster = inventoryLogic.RightMemberRoster;
				this._leftTroopRoster = inventoryLogic.LeftMemberRoster;
				this._currentInventoryCharacterIndex = this._rightTroopRoster.FindIndexOfTroop(this._currentCharacter);
				this.OnDonationXpChange();
				this.CompanionExists = this.DoesCompanionExist();
			}
			this.MainCharacter = new HeroViewModel(CharacterViewModel.StanceTypes.None);
			this.MainCharacter.FillFrom(this._currentCharacter.HeroObject, -1, false, false);
			this.ItemMenu = new ItemMenuVM(new Action<ItemVM, int>(this.ResetComparedItems), this._inventoryLogic, this._getItemUsageSetFlags, new Func<EquipmentIndex, SPItemVM>(this.GetItemFromIndex));
			this.IsRefreshed = false;
			this.RightItemListVM = new MBBindingList<SPItemVM>();
			this.LeftItemListVM = new MBBindingList<SPItemVM>();
			this.CharacterHelmSlot = new SPItemVM();
			this.CharacterCloakSlot = new SPItemVM();
			this.CharacterTorsoSlot = new SPItemVM();
			this.CharacterGloveSlot = new SPItemVM();
			this.CharacterBootSlot = new SPItemVM();
			this.CharacterMountSlot = new SPItemVM();
			this.CharacterMountArmorSlot = new SPItemVM();
			this.CharacterWeapon1Slot = new SPItemVM();
			this.CharacterWeapon2Slot = new SPItemVM();
			this.CharacterWeapon3Slot = new SPItemVM();
			this.CharacterWeapon4Slot = new SPItemVM();
			this.CharacterBannerSlot = new SPItemVM();
			this.ProductionTooltip = new BasicTooltipViewModel();
			this.CurrentCharacterSkillsTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetInventoryCharacterTooltip(this._currentCharacter.HeroObject));
			this.RefreshCallbacks();
			this._selectedEquipmentIndex = 0;
			if (isInCivilianModeByDefault)
			{
				this.EquipmentMode = 0;
			}
			if (this._inventoryLogic != null)
			{
				this.UpdateRightCharacter();
				this.UpdateLeftCharacter();
				this.InitializeInventory();
			}
			this.RightInventoryOwnerGold = Hero.MainHero.Gold;
			if (this._inventoryLogic.OtherSideCapacityData != null)
			{
				this.OtherSideHasCapacity = this._inventoryLogic.OtherSideCapacityData.GetCapacity() != -1;
			}
			this.IsOtherInventoryGoldRelevant = this._usageType != InventoryScreenHelper.InventoryMode.Loot;
			this.PlayerInventorySortController = new SPInventorySortControllerVM(ref this._rightItemListVM);
			this.OtherInventorySortController = new SPInventorySortControllerVM(ref this._leftItemListVM);
			this.PlayerInventorySortController.SortByDefaultState();
			if (this._usageType == InventoryScreenHelper.InventoryMode.Loot)
			{
				this.OtherInventorySortController.CostState = 1;
				this.OtherInventorySortController.ExecuteSortByCost();
			}
			else
			{
				this.OtherInventorySortController.SortByDefaultState();
			}
			Tuple<int, int> tuple = this._viewDataTracker.InventoryGetSortPreference((int)this._usageType);
			if (tuple != null)
			{
				this.PlayerInventorySortController.SortByOption((SPInventorySortControllerVM.InventoryItemSortOption)tuple.Item1, (SPInventorySortControllerVM.InventoryItemSortState)tuple.Item2);
			}
			this.ItemPreview = new ItemPreviewVM(new Action(this.OnPreviewClosed));
			this._characterList = new SelectorVM<InventoryCharacterSelectorItemVM>(0, new Action<SelectorVM<InventoryCharacterSelectorItemVM>>(this.OnCharacterSelected));
			this.AddApplicableCharactersToListFromRoster(this._rightTroopRoster.GetTroopRoster());
			if (this._inventoryLogic.IsOtherPartyFromPlayerClan && this._leftTroopRoster != null)
			{
				this.AddApplicableCharactersToListFromRoster(this._leftTroopRoster.GetTroopRoster());
			}
			if (this._characterList.SelectedIndex == -1 && this._characterList.ItemList.Count > 0)
			{
				this._characterList.SelectedIndex = 0;
			}
			this.BannerTypeName = ItemObject.ItemTypeEnum.Banner.ToString();
			InventoryTradeVM.RemoveZeroCounts += this.ExecuteRemoveZeroCounts;
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this.RefreshValues();
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00037D0C File Offset: 0x00035F0C
		private void AddApplicableCharactersToListFromRoster(MBList<TroopRosterElement> roster)
		{
			for (int i = 0; i < roster.Count; i++)
			{
				CharacterObject character = roster[i].Character;
				if (character.IsHero && this.CanSelectHero(character.HeroObject))
				{
					this._characterList.AddItem(new InventoryCharacterSelectorItemVM(character.StringId, character.HeroObject, character.HeroObject.Name));
					if (character == this._currentCharacter)
					{
						this._characterList.SelectedIndex = this._characterList.ItemList.Count - 1;
					}
				}
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00037D9C File Offset: 0x00035F9C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RightInventoryOwnerName = PartyBase.MainParty.Name.ToString();
			this.SeparatorText = new TextObject("{=dB6cFDmz}/", null).ToString();
			this.DoneLbl = GameTexts.FindText("str_done", null).ToString();
			this.CancelLbl = GameTexts.FindText("str_cancel", null).ToString();
			this.ResetLbl = GameTexts.FindText("str_reset", null).ToString();
			this.TypeText = GameTexts.FindText("str_sort_by_type_label", null).ToString();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.QuantityText = GameTexts.FindText("str_quantity_sign", null).ToString();
			this.CostText = GameTexts.FindText("str_value", null).ToString();
			this.SearchPlaceholderText = new TextObject("{=tQOPRBFg}Search...", null).ToString();
			this.FilterAllHint = new HintViewModel(GameTexts.FindText("str_inventory_filter_all", null), null);
			this.FilterWeaponHint = new HintViewModel(GameTexts.FindText("str_inventory_filter_weapons", null), null);
			this.FilterArmorHint = new HintViewModel(GameTexts.FindText("str_inventory_filter_armors", null), null);
			this.FilterShieldAndRangedHint = new HintViewModel(GameTexts.FindText("str_inventory_filter_shields_ranged", null), null);
			this.FilterMountAndHarnessHint = new HintViewModel(GameTexts.FindText("str_inventory_filter_mounts", null), null);
			this.FilterMiscHint = new HintViewModel(GameTexts.FindText("str_inventory_filter_other", null), null);
			this.CivilianOutfitHint = new HintViewModel(GameTexts.FindText("str_inventory_civilian_outfit", null), null);
			this.BattleOutfitHint = new HintViewModel(GameTexts.FindText("str_inventory_battle_outfit", null), null);
			this.StealthOutfitHint = new HintViewModel(GameTexts.FindText("str_inventory_stealth_outfit", null), null);
			this.EquipmentHelmSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_helm_slot", null), null);
			this.EquipmentArmorSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_armor_slot", null), null);
			this.EquipmentBootSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_boot_slot", null), null);
			this.EquipmentCloakSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_cloak_slot", null), null);
			this.EquipmentGloveSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_glove_slot", null), null);
			this.EquipmentHarnessSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_mount_armor_slot", null), null);
			this.EquipmentMountSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_mount_slot", null), null);
			this.EquipmentWeaponSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_filter_weapons", null), null);
			this.EquipmentBannerSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_banner_slot", null), null);
			this.WeightHint = new HintViewModel(GameTexts.FindText("str_inventory_weight_desc", null), null);
			this.ArmArmorHint = new HintViewModel(GameTexts.FindText("str_inventory_arm_armor", null), null);
			this.BodyArmorHint = new HintViewModel(GameTexts.FindText("str_inventory_body_armor", null), null);
			this.HeadArmorHint = new HintViewModel(GameTexts.FindText("str_inventory_head_armor", null), null);
			this.LegArmorHint = new HintViewModel(GameTexts.FindText("str_inventory_leg_armor", null), null);
			this.HorseArmorHint = new HintViewModel(GameTexts.FindText("str_inventory_horse_armor", null), null);
			this.DonationLblHint = new HintViewModel(GameTexts.FindText("str_inventory_donation_label_hint", null), null);
			this.SetPreviousCharacterHint();
			this.SetNextCharacterHint();
			this.PreviewHint = new HintViewModel(GameTexts.FindText("str_inventory_preview", null), null);
			this.EquipHint = new HintViewModel(GameTexts.FindText("str_inventory_equip", null), null);
			this.UnequipHint = new HintViewModel(GameTexts.FindText("str_inventory_unequip", null), null);
			this.ResetHint = new HintViewModel(GameTexts.FindText("str_reset", null), null);
			this.PlayerSideCapacityExceededText = GameTexts.FindText("str_capacity_exceeded", null).ToString();
			this.PlayerSideCapacityExceededHint = new HintViewModel(GameTexts.FindText("str_capacity_exceeded_hint", null), null);
			this.MainPartyLandCapacityExceededText = new TextObject("{=fgyvzyB5}Land Capacity Exceeded", null).ToString();
			this.MainPartySeaCapacityExceededText = new TextObject("{=7dXs9c2b}Sea Capacity Exceeded", null).ToString();
			this.MainPartyLandCapacityExceededHint = new HintViewModel(new TextObject("{=knayk28P}You will slow down on land. Be careful.", null), null);
			this.MainPartySeaCapacityExceededHint = new HintViewModel(new TextObject("{=zoX9akov}You will slow down at sea. Be careful.", null), null);
			if (this._inventoryLogic.OtherSideCapacityData != null)
			{
				TextObject capacityExceededWarningText = this._inventoryLogic.OtherSideCapacityData.GetCapacityExceededWarningText();
				this.OtherSideCapacityExceededText = ((capacityExceededWarningText != null) ? capacityExceededWarningText.ToString() : null);
				this.OtherSideCapacityExceededHint = new HintViewModel(this._inventoryLogic.OtherSideCapacityData.GetCapacityExceededHintText(), null);
			}
			this.SetBuyAllHint();
			this.SetSellAllHint();
			if (this._usageType == InventoryScreenHelper.InventoryMode.Loot || this._usageType == InventoryScreenHelper.InventoryMode.Stash)
			{
				this.SellHint = new HintViewModel(GameTexts.FindText("str_give", null), null);
			}
			else if (this._usageType == InventoryScreenHelper.InventoryMode.Default)
			{
				this.SellHint = new HintViewModel(GameTexts.FindText("str_inventory_discard", null), null);
			}
			else
			{
				this.SellHint = new HintViewModel(GameTexts.FindText("str_inventory_sell", null), null);
			}
			this.CharacterHelmSlot.RefreshValues();
			this.CharacterCloakSlot.RefreshValues();
			this.CharacterTorsoSlot.RefreshValues();
			this.CharacterGloveSlot.RefreshValues();
			this.CharacterBootSlot.RefreshValues();
			this.CharacterMountSlot.RefreshValues();
			this.CharacterMountArmorSlot.RefreshValues();
			this.CharacterWeapon1Slot.RefreshValues();
			this.CharacterWeapon2Slot.RefreshValues();
			this.CharacterWeapon3Slot.RefreshValues();
			this.CharacterWeapon4Slot.RefreshValues();
			this.CharacterBannerSlot.RefreshValues();
			SPInventorySortControllerVM playerInventorySortController = this.PlayerInventorySortController;
			if (playerInventorySortController != null)
			{
				playerInventorySortController.RefreshValues();
			}
			SPInventorySortControllerVM otherInventorySortController = this.OtherInventorySortController;
			if (otherInventorySortController == null)
			{
				return;
			}
			otherInventorySortController.RefreshValues();
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00038318 File Offset: 0x00036518
		public override void OnFinalize()
		{
			ItemVM.ProcessEquipItem = null;
			ItemVM.ProcessUnequipItem = null;
			ItemVM.ProcessPreviewItem = null;
			ItemVM.ProcessBuyItem = null;
			SPItemVM.ProcessSellItem = null;
			ItemVM.ProcessItemSelect = null;
			ItemVM.ProcessItemTooltip = null;
			SPItemVM.ProcessItemSlaughter = null;
			SPItemVM.ProcessItemDonate = null;
			SPItemVM.OnFocus = null;
			InventoryTradeVM.RemoveZeroCounts -= this.ExecuteRemoveZeroCounts;
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this.ItemPreview.OnFinalize();
			this.ItemPreview = null;
			this.CancelInputKey.OnFinalize();
			this.DoneInputKey.OnFinalize();
			this.ResetInputKey.OnFinalize();
			this.PreviousCharacterInputKey.OnFinalize();
			this.NextCharacterInputKey.OnFinalize();
			this.BuyAllInputKey.OnFinalize();
			this.SellAllInputKey.OnFinalize();
			ItemVM.ProcessEquipItem = null;
			ItemVM.ProcessUnequipItem = null;
			ItemVM.ProcessPreviewItem = null;
			ItemVM.ProcessBuyItem = null;
			SPItemVM.ProcessLockItem = null;
			SPItemVM.ProcessSellItem = null;
			ItemVM.ProcessItemSelect = null;
			ItemVM.ProcessItemTooltip = null;
			SPItemVM.ProcessItemSlaughter = null;
			SPItemVM.ProcessItemDonate = null;
			SPItemVM.OnFocus = null;
			this.MainCharacter.OnFinalize();
			this._inventoryLogic = null;
			base.OnFinalize();
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00038448 File Offset: 0x00036648
		public void RefreshCallbacks()
		{
			ItemVM.ProcessEquipItem = new Action<ItemVM>(this.ProcessEquipItem);
			ItemVM.ProcessUnequipItem = new Action<ItemVM>(this.ProcessUnequipItem);
			ItemVM.ProcessPreviewItem = new Action<ItemVM>(this.ProcessPreviewItem);
			ItemVM.ProcessBuyItem = new Action<ItemVM, bool>(this.ProcessBuyItem);
			SPItemVM.ProcessLockItem = new Action<SPItemVM, bool>(this.ProcessLockItem);
			SPItemVM.ProcessSellItem = new Action<SPItemVM, bool>(this.ProcessSellItem);
			ItemVM.ProcessItemSelect = new Action<ItemVM>(this.ProcessItemSelect);
			ItemVM.ProcessItemTooltip = new Action<ItemVM>(this.ProcessItemTooltip);
			SPItemVM.ProcessItemSlaughter = new Action<SPItemVM>(this.ProcessItemSlaughter);
			SPItemVM.ProcessItemDonate = new Action<SPItemVM>(this.ProcessItemDonate);
			SPItemVM.OnFocus = new Action<SPItemVM>(this.OnItemFocus);
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00038510 File Offset: 0x00036710
		private bool CanSelectHero(Hero hero)
		{
			return hero.IsAlive && hero.CanHeroEquipmentBeChanged() && hero.Clan == Clan.PlayerClan && hero.HeroState != Hero.CharacterStates.Disabled && !hero.IsChild;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00038543 File Offset: 0x00036743
		private void OnEquipmentModeChanged()
		{
			this.IsCivilianMode = this.EquipmentMode == 0;
			this.IsBattleMode = this.EquipmentMode == 1;
			this.IsStealthMode = this.EquipmentMode == 2;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00038572 File Offset: 0x00036772
		private void SetPreviousCharacterHint()
		{
			this.PreviousCharacterHint = new BasicTooltipViewModel(delegate
			{
				GameTexts.SetVariable("HOTKEY", this.GetPreviousCharacterKeyText());
				GameTexts.SetVariable("TEXT", GameTexts.FindText("str_inventory_prev_char", null));
				return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
			});
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0003858B File Offset: 0x0003678B
		private void SetNextCharacterHint()
		{
			this.NextCharacterHint = new BasicTooltipViewModel(delegate
			{
				GameTexts.SetVariable("HOTKEY", this.GetNextCharacterKeyText());
				GameTexts.SetVariable("TEXT", GameTexts.FindText("str_inventory_next_char", null));
				return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
			});
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x000385A4 File Offset: 0x000367A4
		private void SetBuyAllHint()
		{
			TextObject buyAllHintText;
			if (this._usageType == InventoryScreenHelper.InventoryMode.Trade)
			{
				buyAllHintText = GameTexts.FindText("str_inventory_buy_all", null);
			}
			else
			{
				buyAllHintText = GameTexts.FindText("str_inventory_take_all", null);
			}
			this.BuyAllHint = new BasicTooltipViewModel(delegate
			{
				GameTexts.SetVariable("HOTKEY", this.GetBuyAllKeyText());
				GameTexts.SetVariable("TEXT", buyAllHintText);
				return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
			});
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00038604 File Offset: 0x00036804
		private void SetSellAllHint()
		{
			TextObject sellAllHintText;
			if (this._usageType == InventoryScreenHelper.InventoryMode.Loot || this._usageType == InventoryScreenHelper.InventoryMode.Stash)
			{
				sellAllHintText = GameTexts.FindText("str_inventory_give_all", null);
			}
			else if (this._usageType == InventoryScreenHelper.InventoryMode.Default)
			{
				sellAllHintText = GameTexts.FindText("str_inventory_discard_all", null);
			}
			else
			{
				sellAllHintText = GameTexts.FindText("str_inventory_sell_all", null);
			}
			this.SellAllHint = new BasicTooltipViewModel(delegate
			{
				GameTexts.SetVariable("HOTKEY", this.GetSellAllKeyText());
				GameTexts.SetVariable("TEXT", sellAllHintText);
				return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
			});
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00038688 File Offset: 0x00036888
		private void OnCharacterSelected(SelectorVM<InventoryCharacterSelectorItemVM> selector)
		{
			if (this._inventoryLogic == null || selector.SelectedItem == null)
			{
				return;
			}
			for (int i = 0; i < this._rightTroopRoster.Count; i++)
			{
				if (this._rightTroopRoster.GetCharacterAtIndex(i).StringId == selector.SelectedItem.CharacterID)
				{
					this.UpdateCurrentCharacterIfPossible(i, true);
					return;
				}
			}
			if (this._leftTroopRoster != null)
			{
				for (int j = 0; j < this._leftTroopRoster.Count; j++)
				{
					if (this._leftTroopRoster.GetCharacterAtIndex(j).StringId == selector.SelectedItem.CharacterID)
					{
						this.UpdateCurrentCharacterIfPossible(j, false);
						return;
					}
				}
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00038734 File Offset: 0x00036934
		private Equipment ActiveEquipment
		{
			get
			{
				switch (this.EquipmentMode)
				{
				case 0:
					return this._currentCharacter.FirstCivilianEquipment;
				case 1:
					return this._currentCharacter.FirstBattleEquipment;
				case 2:
					return this._currentCharacter.FirstStealthEquipment;
				default:
					Debug.FailedAssert("Invalid active equipment type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Inventory\\SPInventoryVM.cs", "ActiveEquipment", 517);
					return null;
				}
			}
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0003879A File Offset: 0x0003699A
		public void ExecuteShowRecap()
		{
			InformationManager.ShowTooltip(typeof(InventoryLogic), new object[] { this._inventoryLogic });
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x000387BA File Offset: 0x000369BA
		public void ExecuteCancelRecap()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x000387C4 File Offset: 0x000369C4
		public void ExecuteRemoveZeroCounts()
		{
			List<SPItemVM> list = this.LeftItemListVM.ToList<SPItemVM>();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].ItemCount == 0 && i >= 0 && i < this.LeftItemListVM.Count)
				{
					list[i].IsSelected = false;
					this.LeftItemListVM.RemoveAt(i);
				}
			}
			List<SPItemVM> list2 = this.RightItemListVM.ToList<SPItemVM>();
			for (int j = list2.Count - 1; j >= 0; j--)
			{
				if (list2[j].ItemCount == 0 && j >= 0 && j < this.RightItemListVM.Count)
				{
					list2[j].IsSelected = false;
					this.RightItemListVM.RemoveAt(j);
				}
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00038881 File Offset: 0x00036A81
		private void ProcessPreviewItem(ItemVM item)
		{
			this._inventoryLogic.IsPreviewingItem = true;
			this.ItemPreview.Open(item.ItemRosterElement.EquipmentElement);
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x000388A5 File Offset: 0x00036AA5
		public void ClosePreview()
		{
			this.ItemPreview.Close();
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x000388B2 File Offset: 0x00036AB2
		private void OnPreviewClosed()
		{
			this._inventoryLogic.IsPreviewingItem = false;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x000388C0 File Offset: 0x00036AC0
		private void ProcessEquipItem(ItemVM draggedItem)
		{
			SPItemVM spitemVM = draggedItem as SPItemVM;
			if (!spitemVM.IsTransferable && !this._currentCharacter.IsPlayerCharacter)
			{
				return;
			}
			this.IsRefreshed = false;
			this.EquipEquipment(spitemVM);
			this.RefreshInformationValues();
			this.ExecuteRemoveZeroCounts();
			this.IsRefreshed = true;
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0003890B File Offset: 0x00036B0B
		private void ProcessUnequipItem(ItemVM draggedItem)
		{
			if (!(draggedItem as SPItemVM).IsTransferable && !this._currentCharacter.IsPlayerCharacter)
			{
				return;
			}
			this.IsRefreshed = false;
			this.UnequipEquipment(draggedItem as SPItemVM);
			this.RefreshInformationValues();
			this.IsRefreshed = true;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00038948 File Offset: 0x00036B48
		private void ProcessBuyItem(ItemVM itemBase, bool cameFromTradeData)
		{
			SPItemVM spitemVM = itemBase as SPItemVM;
			if (spitemVM == null || !spitemVM.IsTransferable)
			{
				return;
			}
			if (this.IsEntireStackModifierActive && !cameFromTradeData)
			{
				ItemRosterElement? itemRosterElement;
				this.TransactionCount = ((this._inventoryLogic.FindItemFromSide(InventoryLogic.InventorySide.OtherInventory, (spitemVM != null) ? spitemVM.ItemRosterElement.EquipmentElement : EquipmentElement.Invalid) != null) ? itemRosterElement.GetValueOrDefault().Amount : 0);
			}
			else if (this.IsFiveStackModifierActive && !cameFromTradeData)
			{
				this.TransactionCount = 5;
			}
			else
			{
				this.TransactionCount = ((spitemVM != null) ? spitemVM.TransactionCount : 0);
			}
			if (this.TransactionCount == 0)
			{
				Debug.FailedAssert("Transaction count should not be zero", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Inventory\\SPInventoryVM.cs", "ProcessBuyItem", 634);
				return;
			}
			this.IsRefreshed = false;
			MBTextManager.SetTextVariable("ITEM_DESCRIPTION", itemBase.ItemDescription, false);
			MBTextManager.SetTextVariable("ITEM_COST", itemBase.ItemCost);
			this.BuyItem(spitemVM);
			if (!cameFromTradeData)
			{
				this.ExecuteRemoveZeroCounts();
			}
			this.RefreshInformationValues();
			this.IsRefreshed = true;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00038A4C File Offset: 0x00036C4C
		private void ProcessSellItem(SPItemVM item, bool cameFromTradeData)
		{
			if (!item.IsTransferable)
			{
				return;
			}
			if (InventoryLogic.IsEquipmentSide(item.InventorySide))
			{
				this.TransactionCount = 1;
			}
			else if (this.IsEntireStackModifierActive && !cameFromTradeData)
			{
				ItemRosterElement? itemRosterElement = this._inventoryLogic.FindItemFromSide(InventoryLogic.InventorySide.PlayerInventory, item.ItemRosterElement.EquipmentElement);
				this.TransactionCount = ((itemRosterElement != null) ? itemRosterElement.GetValueOrDefault().Amount : 0);
			}
			else if (this.IsFiveStackModifierActive && !cameFromTradeData)
			{
				this.TransactionCount = 5;
			}
			else
			{
				this.TransactionCount = item.TransactionCount;
			}
			if (this.TransactionCount == 0)
			{
				Debug.FailedAssert("Transaction count should not be zero", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Inventory\\SPInventoryVM.cs", "ProcessSellItem", 684);
				return;
			}
			this.IsRefreshed = false;
			MBTextManager.SetTextVariable("ITEM_DESCRIPTION", item.ItemDescription, false);
			MBTextManager.SetTextVariable("ITEM_COST", item.ItemCost);
			this.SellItem(item);
			if (!cameFromTradeData)
			{
				this.ExecuteRemoveZeroCounts();
			}
			this.RefreshInformationValues();
			this.IsRefreshed = true;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00038B48 File Offset: 0x00036D48
		private void ProcessLockItem(SPItemVM item, bool isLocked)
		{
			if (isLocked && item.InventorySide == InventoryLogic.InventorySide.PlayerInventory && !this._lockedItemIDs.Contains(item.StringId))
			{
				this._lockedItemIDs.Add(item.StringId);
				return;
			}
			if (!isLocked && item.InventorySide == InventoryLogic.InventorySide.PlayerInventory && this._lockedItemIDs.Contains(item.StringId))
			{
				this._lockedItemIDs.Remove(item.StringId);
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00038BB8 File Offset: 0x00036DB8
		private ItemVM ProcessCompareItem(ItemVM item, int alternativeUsageIndex = 0)
		{
			this._selectedEquipmentIndex = 0;
			this._comparedItemList.Clear();
			ItemVM itemVM = null;
			bool flag = false;
			EquipmentIndex equipmentIndex = EquipmentIndex.None;
			SPItemVM spitemVM = null;
			bool flag2 = item.ItemType >= EquipmentIndex.WeaponItemBeginSlot && item.ItemType < EquipmentIndex.ExtraWeaponSlot;
			if (!InventoryLogic.IsEquipmentSide(((SPItemVM)item).InventorySide))
			{
				if (flag2)
				{
					for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex2++)
					{
						EquipmentIndex equipmentIndex3 = equipmentIndex2;
						SPItemVM itemFromIndex = this.GetItemFromIndex(equipmentIndex3);
						if (itemFromIndex != null && itemFromIndex.ItemRosterElement.EquipmentElement.Item != null && ItemHelper.CheckComparability(item.ItemRosterElement.EquipmentElement.Item, itemFromIndex.ItemRosterElement.EquipmentElement.Item, alternativeUsageIndex))
						{
							this._comparedItemList.Add(itemFromIndex);
						}
					}
					if (!this._comparedItemList.IsEmpty<ItemVM>())
					{
						this.SortComparedItems(item);
						itemVM = this._comparedItemList[0];
						this._lastComparedItemIndex = 0;
					}
					if (itemVM != null)
					{
						equipmentIndex = itemVM.ItemType;
					}
				}
				else
				{
					equipmentIndex = item.ItemType;
				}
			}
			if (item.ItemType >= EquipmentIndex.WeaponItemBeginSlot && item.ItemType < EquipmentIndex.NumEquipmentSetSlots)
			{
				spitemVM = ((equipmentIndex != EquipmentIndex.None) ? this.GetItemFromIndex(equipmentIndex) : null);
				flag = spitemVM != null && !string.IsNullOrEmpty(spitemVM.StringId) && item.StringId != spitemVM.StringId;
			}
			if (!this._selectedTooltipItemStringID.Equals(item.StringId) || (flag && !this._comparedTooltipItemStringID.Equals(spitemVM.StringId)))
			{
				this._selectedTooltipItemStringID = item.StringId;
				if (flag)
				{
					this._comparedTooltipItemStringID = spitemVM.StringId;
				}
			}
			this._selectedEquipmentIndex = (int)equipmentIndex;
			if (spitemVM == null || spitemVM.ItemRosterElement.IsEmpty)
			{
				return null;
			}
			return spitemVM;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00038D6C File Offset: 0x00036F6C
		private void ResetComparedItems(ItemVM item, int alternativeUsageIndex)
		{
			ItemVM itemVM = this.ProcessCompareItem(item, alternativeUsageIndex);
			this.ItemMenu.SetItem(this._selectedItem, this.GetEquipmentToInventorySide((SPInventoryVM.EquipmentModes)this.EquipmentMode), itemVM, this._currentCharacter, alternativeUsageIndex);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00038DA8 File Offset: 0x00036FA8
		private void SortComparedItems(ItemVM selectedItem)
		{
			List<ItemVM> list = new List<ItemVM>();
			for (int i = 0; i < this._comparedItemList.Count; i++)
			{
				if (selectedItem.StringId == this._comparedItemList[i].StringId && !list.Contains(this._comparedItemList[i]))
				{
					list.Add(this._comparedItemList[i]);
				}
			}
			for (int j = 0; j < this._comparedItemList.Count; j++)
			{
				if (this._comparedItemList[j].ItemRosterElement.EquipmentElement.Item.Type == selectedItem.ItemRosterElement.EquipmentElement.Item.Type && !list.Contains(this._comparedItemList[j]))
				{
					list.Add(this._comparedItemList[j]);
				}
			}
			for (int k = 0; k < this._comparedItemList.Count; k++)
			{
				WeaponComponent weaponComponent = this._comparedItemList[k].ItemRosterElement.EquipmentElement.Item.WeaponComponent;
				WeaponComponent weaponComponent2 = selectedItem.ItemRosterElement.EquipmentElement.Item.WeaponComponent;
				if (((weaponComponent2.Weapons.Count > 1 && weaponComponent2.Weapons[1].WeaponClass == weaponComponent.Weapons[0].WeaponClass) || (weaponComponent.Weapons.Count > 1 && weaponComponent.Weapons[1].WeaponClass == weaponComponent2.Weapons[0].WeaponClass) || (weaponComponent2.Weapons.Count > 1 && weaponComponent.Weapons.Count > 1 && weaponComponent2.Weapons[1].WeaponClass == weaponComponent.Weapons[1].WeaponClass)) && !list.Contains(this._comparedItemList[k]))
				{
					list.Add(this._comparedItemList[k]);
				}
			}
			if (this._comparedItemList.Count != list.Count)
			{
				foreach (ItemVM itemVM in this._comparedItemList)
				{
					if (!list.Contains(itemVM))
					{
						list.Add(itemVM);
					}
				}
			}
			this._comparedItemList = list;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00039034 File Offset: 0x00037234
		public void ProcessItemTooltip(ItemVM item)
		{
			if (item == null || string.IsNullOrEmpty(item.StringId))
			{
				return;
			}
			this._selectedItem = item as SPItemVM;
			ItemVM itemVM = this.ProcessCompareItem(item, 0);
			this.ItemMenu.SetItem(this._selectedItem, this.GetEquipmentToInventorySide((SPInventoryVM.EquipmentModes)this.EquipmentMode), itemVM, this._currentCharacter, 0);
			this.RefreshTransactionCost(1);
			this._selectedItem.UpdateCanBeSlaughtered();
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0003909E File Offset: 0x0003729E
		public void ResetSelectedItem()
		{
			this._selectedItem = null;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x000390A8 File Offset: 0x000372A8
		private void ProcessItemSlaughter(SPItemVM item)
		{
			this.IsRefreshed = false;
			if (string.IsNullOrEmpty(item.StringId) || !item.CanBeSlaughtered)
			{
				return;
			}
			this.SlaughterItem(item);
			this.RefreshInformationValues();
			if (item.ItemCount == 0)
			{
				this.ExecuteRemoveZeroCounts();
			}
			this.IsRefreshed = true;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x000390F4 File Offset: 0x000372F4
		private void ProcessItemDonate(SPItemVM item)
		{
			this.IsRefreshed = false;
			if (string.IsNullOrEmpty(item.StringId) || !item.CanBeDonated)
			{
				return;
			}
			this.DonateItem(item);
			this.RefreshInformationValues();
			if (item.ItemCount == 0)
			{
				this.ExecuteRemoveZeroCounts();
			}
			this.IsRefreshed = true;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00039140 File Offset: 0x00037340
		private void OnItemFocus(SPItemVM item)
		{
			this.CurrentFocusedItem = item;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00039149 File Offset: 0x00037349
		private void ProcessItemSelect(ItemVM item)
		{
			this.ExecuteRemoveZeroCounts();
			this.ExecuteSelectItem(item);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00039158 File Offset: 0x00037358
		private void RefreshTransactionCost(int transactionCount = 1)
		{
			if (this._selectedItem != null && this.IsTrading)
			{
				int num;
				int itemTotalPrice = this._inventoryLogic.GetItemTotalPrice(this._selectedItem.ItemRosterElement, transactionCount, out num, this._selectedItem.InventorySide == InventoryLogic.InventorySide.OtherInventory);
				this.ItemMenu.SetTransactionCost(itemTotalPrice, num);
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x000391AC File Offset: 0x000373AC
		public void RefreshComparedItem()
		{
			this._lastComparedItemIndex++;
			if (this._lastComparedItemIndex > this._comparedItemList.Count - 1)
			{
				this._lastComparedItemIndex = 0;
			}
			if (!this._comparedItemList.IsEmpty<ItemVM>() && this._selectedItem != null && this._comparedItemList[this._lastComparedItemIndex] != null)
			{
				this.ItemMenu.SetItem(this._selectedItem, this.GetEquipmentToInventorySide((SPInventoryVM.EquipmentModes)this.EquipmentMode), this._comparedItemList[this._lastComparedItemIndex], this._currentCharacter, 0);
			}
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00039240 File Offset: 0x00037440
		private void AfterReset(InventoryLogic itemRoster, bool fromCancel)
		{
			this._inventoryLogic = itemRoster;
			if (!fromCancel)
			{
				switch (this.ActiveFilterIndex)
				{
				case 1:
					this._inventoryLogic.MerchantItemType = InventoryScreenHelper.InventoryCategoryType.Weapon;
					break;
				case 2:
					this._inventoryLogic.MerchantItemType = InventoryScreenHelper.InventoryCategoryType.Shield;
					break;
				case 3:
					this._inventoryLogic.MerchantItemType = InventoryScreenHelper.InventoryCategoryType.Armors;
					break;
				case 4:
					this._inventoryLogic.MerchantItemType = InventoryScreenHelper.InventoryCategoryType.HorseCategory;
					break;
				case 5:
					this._inventoryLogic.MerchantItemType = InventoryScreenHelper.InventoryCategoryType.Goods;
					break;
				default:
					this._inventoryLogic.MerchantItemType = InventoryScreenHelper.InventoryCategoryType.All;
					break;
				}
				this.InitializeInventory();
				this.PlayerInventorySortController = new SPInventorySortControllerVM(ref this._rightItemListVM);
				this.OtherInventorySortController = new SPInventorySortControllerVM(ref this._leftItemListVM);
				this.PlayerInventorySortController.SortByDefaultState();
				this.OtherInventorySortController.SortByDefaultState();
				Tuple<int, int> tuple = this._viewDataTracker.InventoryGetSortPreference((int)this._usageType);
				if (tuple != null)
				{
					this.PlayerInventorySortController.SortByOption((SPInventorySortControllerVM.InventoryItemSortOption)tuple.Item1, (SPInventorySortControllerVM.InventoryItemSortState)tuple.Item2);
				}
				this.UpdateRightCharacter();
				this.UpdateLeftCharacter();
				this.RightInventoryOwnerName = PartyBase.MainParty.Name.ToString();
				this.RightInventoryOwnerGold = Hero.MainHero.Gold;
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0003936C File Offset: 0x0003756C
		private void OnTotalAmountChange(int newTotalAmount)
		{
			MBTextManager.SetTextVariable("PAY_OR_GET", (this._inventoryLogic.TotalAmount < 0) ? 1 : 0);
			int num = MathF.Min(-this._inventoryLogic.TotalAmount, this._inventoryLogic.InventoryListener.GetGold());
			MBTextManager.SetTextVariable("TRADE_AMOUNT", MathF.Abs(num));
			this.TradeLbl = ((this._inventoryLogic.TotalAmount == 0) ? "" : GameTexts.FindText("str_inventory_trade_label", null).ToString());
			this.RightInventoryOwnerGold = Hero.MainHero.Gold - this._inventoryLogic.TotalAmount;
			InventoryListener inventoryListener = this._inventoryLogic.InventoryListener;
			this.LeftInventoryOwnerGold = (((inventoryListener != null) ? new int?(inventoryListener.GetGold()) : null) + this._inventoryLogic.TotalAmount) ?? 0;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0003947C File Offset: 0x0003767C
		private void OnDonationXpChange()
		{
			int num = (int)this._inventoryLogic.XpGainFromDonations;
			bool flag = false;
			if (num > this._donationMaxShareableXp)
			{
				num = this._donationMaxShareableXp;
				flag = true;
			}
			this.IsDonationXpGainExceedsMax = flag;
			this.HasGainedExperience = num > 0;
			MBTextManager.SetTextVariable("XP_AMOUNT", num);
			this.ExperienceLbl = ((num == 0) ? "" : GameTexts.FindText("str_inventory_donation_label", null).ToString());
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x000394E8 File Offset: 0x000376E8
		private void AfterTransfer(InventoryLogic inventoryLogic, List<TransferCommandResult> results)
		{
			this._isCharacterEquipmentDirty = false;
			List<SPItemVM> list = new List<SPItemVM>();
			HashSet<ItemCategory> hashSet = new HashSet<ItemCategory>();
			for (int num = 0; num != results.Count; num++)
			{
				TransferCommandResult transferCommandResult = results[num];
				if (transferCommandResult.ResultSide == InventoryLogic.InventorySide.OtherInventory || transferCommandResult.ResultSide == InventoryLogic.InventorySide.PlayerInventory)
				{
					bool flag = false;
					MBBindingList<SPItemVM> mbbindingList = ((transferCommandResult.ResultSide == InventoryLogic.InventorySide.OtherInventory) ? this.LeftItemListVM : this.RightItemListVM);
					for (int i = 0; i < mbbindingList.Count; i++)
					{
						SPItemVM spitemVM = mbbindingList[i];
						if (spitemVM != null && spitemVM.ItemRosterElement.EquipmentElement.IsEqualTo(transferCommandResult.EffectedItemRosterElement.EquipmentElement))
						{
							spitemVM.ItemRosterElement.Amount = transferCommandResult.FinalNumber;
							spitemVM.ItemCount = transferCommandResult.FinalNumber;
							spitemVM.ItemCost = this._inventoryLogic.GetItemPrice(spitemVM.ItemRosterElement.EquipmentElement, transferCommandResult.ResultSide == InventoryLogic.InventorySide.OtherInventory);
							list.Add(spitemVM);
							if (!hashSet.Contains(spitemVM.ItemRosterElement.EquipmentElement.Item.GetItemCategory()))
							{
								hashSet.Add(spitemVM.ItemRosterElement.EquipmentElement.Item.GetItemCategory());
							}
							if (spitemVM.IsSelected)
							{
								this.ScrollItemId = spitemVM.ItemRosterElement.EquipmentElement.Item.StringId;
								this.ScrollToItem = true;
							}
							flag = true;
							break;
						}
					}
					if (!flag && transferCommandResult.EffectedNumber > 0 && this._inventoryLogic != null)
					{
						SPItemVM newItem = null;
						SPItemVM spitemVM2;
						if (transferCommandResult.ResultSide == InventoryLogic.InventorySide.OtherInventory)
						{
							newItem = new SPItemVM(this._inventoryLogic, this.MainCharacter.IsFemale, this.CanCharacterUseItem(transferCommandResult.EffectedItemRosterElement), this._usageType, transferCommandResult.EffectedItemRosterElement, InventoryLogic.InventorySide.OtherInventory, this._inventoryLogic.GetCostOfItemRosterElement(transferCommandResult.EffectedItemRosterElement, transferCommandResult.ResultSide), null);
							spitemVM2 = this.RightItemListVM.FirstOrDefault<SPItemVM>((SPItemVM x) => x.ItemRosterElement.EquipmentElement.IsEqualTo(newItem.ItemRosterElement.EquipmentElement));
						}
						else
						{
							newItem = new SPItemVM(this._inventoryLogic, this.MainCharacter.IsFemale, this.CanCharacterUseItem(transferCommandResult.EffectedItemRosterElement), this._usageType, transferCommandResult.EffectedItemRosterElement, InventoryLogic.InventorySide.PlayerInventory, this._inventoryLogic.GetCostOfItemRosterElement(transferCommandResult.EffectedItemRosterElement, transferCommandResult.ResultSide), null);
							spitemVM2 = this.LeftItemListVM.FirstOrDefault<SPItemVM>((SPItemVM x) => x.ItemRosterElement.EquipmentElement.IsEqualTo(newItem.ItemRosterElement.EquipmentElement));
						}
						this.UpdateFilteredStatusOfItem(newItem);
						newItem.ItemCount = transferCommandResult.FinalNumber;
						newItem.IsLocked = newItem.InventorySide == InventoryLogic.InventorySide.PlayerInventory && this._lockedItemIDs.Contains(newItem.StringId);
						newItem.IsNew = true;
						newItem.IsSelected = spitemVM2 != null && spitemVM2.IsSelected;
						mbbindingList.Add(newItem);
						if (newItem.IsSelected)
						{
							this.ScrollItemId = transferCommandResult.EffectedItemRosterElement.EquipmentElement.Item.StringId;
							this.ScrollToItem = true;
						}
					}
				}
				else if (InventoryLogic.IsEquipmentSide(transferCommandResult.ResultSide))
				{
					SPItemVM spitemVM3 = null;
					if (transferCommandResult.FinalNumber > 0)
					{
						spitemVM3 = new SPItemVM(this._inventoryLogic, this.MainCharacter.IsFemale, this.CanCharacterUseItem(transferCommandResult.EffectedItemRosterElement), this._usageType, transferCommandResult.EffectedItemRosterElement, transferCommandResult.ResultSide, this._inventoryLogic.GetCostOfItemRosterElement(transferCommandResult.EffectedItemRosterElement, transferCommandResult.ResultSide), new EquipmentIndex?(transferCommandResult.EffectedEquipmentIndex));
						spitemVM3.IsNew = true;
					}
					this.UpdateEquipment(transferCommandResult.ResultSideEquipment, spitemVM3, transferCommandResult.EffectedEquipmentIndex);
					this._isCharacterEquipmentDirty = true;
				}
			}
			SPItemVM selectedItem = this._selectedItem;
			if (selectedItem != null && selectedItem.ItemCount > 1)
			{
				this.ProcessItemTooltip(this._selectedItem);
				this._selectedItem.UpdateCanBeSlaughtered();
			}
			this.CheckEquipAfterTransferStack();
			if (!this.ActiveEquipment[EquipmentIndex.HorseHarness].IsEmpty && this.ActiveEquipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty)
			{
				this.UnequipEquipment(this.CharacterMountArmorSlot);
			}
			if (!this.ActiveEquipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty && !this.ActiveEquipment[EquipmentIndex.HorseHarness].IsEmpty && this.ActiveEquipment[EquipmentIndex.ArmorItemEndSlot].Item.HorseComponent.Monster.FamilyType != this.ActiveEquipment[EquipmentIndex.HorseHarness].Item.ArmorComponent.FamilyType)
			{
				this.UnequipEquipment(this.CharacterMountArmorSlot);
			}
			foreach (SPItemVM spitemVM4 in list)
			{
				spitemVM4.UpdateTradeData(true);
				spitemVM4.UpdateCanBeSlaughtered();
			}
			this.UpdateCostOfItemsInCategory(hashSet);
			if (PartyBase.MainParty.IsMobile)
			{
				PartyBase.MainParty.MobileParty.MemberRoster.UpdateVersion();
				PartyBase.MainParty.MobileParty.PrisonRoster.UpdateVersion();
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00039A64 File Offset: 0x00037C64
		private void UpdateCostOfItemsInCategory(HashSet<ItemCategory> categories)
		{
			foreach (SPItemVM spitemVM in this.LeftItemListVM)
			{
				if (categories.Contains(spitemVM.ItemRosterElement.EquipmentElement.Item.GetItemCategory()))
				{
					spitemVM.ItemCost = this._inventoryLogic.GetCostOfItemRosterElement(spitemVM.ItemRosterElement, InventoryLogic.InventorySide.OtherInventory);
				}
			}
			foreach (SPItemVM spitemVM2 in this.RightItemListVM)
			{
				if (categories.Contains(spitemVM2.ItemRosterElement.EquipmentElement.Item.GetItemCategory()))
				{
					spitemVM2.ItemCost = this._inventoryLogic.GetCostOfItemRosterElement(spitemVM2.ItemRosterElement, InventoryLogic.InventorySide.PlayerInventory);
				}
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00039B50 File Offset: 0x00037D50
		private void CheckEquipAfterTransferStack()
		{
			while (this._equipAfterTransferStack.Count > 0)
			{
				SPItemVM spitemVM = new SPItemVM();
				spitemVM.RefreshWith(this._equipAfterTransferStack.Pop(), InventoryLogic.InventorySide.PlayerInventory);
				this.EquipEquipment(spitemVM);
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00039B8C File Offset: 0x00037D8C
		private void RefreshInformationValues()
		{
			TextObject textObject = GameTexts.FindText("str_LEFT_over_RIGHT", null);
			int inventoryCapacity = MobileParty.MainParty.InventoryCapacity;
			float totalWeightCarried = MobileParty.MainParty.TotalWeightCarried;
			this.MainPartyTotalWeightCarriedText = totalWeightCarried.ToString("#0");
			this.MainPartyInventoryCapacityText = inventoryCapacity.ToString();
			this.PlayerEquipmentCountWarned = totalWeightCarried > (float)inventoryCapacity;
			this.ShowMainPartyLandCapacityTexts = MobileParty.MainParty.IsCurrentlyAtSea;
			this.ShowMainPartySeaCapacityTexts = !this.ShowMainPartyLandCapacityTexts && MobileParty.MainParty.HasNavalNavigationCapability;
			if (this.ShowMainPartyLandCapacityTexts)
			{
				int num = (int)Campaign.Current.Models.InventoryCapacityModel.CalculateTotalWeightCarried(MobileParty.MainParty, false, false).ResultNumber;
				int num2 = (int)Campaign.Current.Models.InventoryCapacityModel.CalculateInventoryCapacity(MobileParty.MainParty, false, false, 0, 0, 0, false).ResultNumber;
				this.MainPartyLandWeightText = num.ToString();
				this.MainPartyLandCapacityText = num2.ToString();
				this.IsMainPartyLandCapacityWarned = num > num2;
			}
			else if (this.ShowMainPartySeaCapacityTexts)
			{
				int num3 = (int)Campaign.Current.Models.InventoryCapacityModel.CalculateTotalWeightCarried(MobileParty.MainParty, true, false).ResultNumber;
				int num4 = (int)Campaign.Current.Models.InventoryCapacityModel.CalculateInventoryCapacity(MobileParty.MainParty, true, false, 0, 0, 0, false).ResultNumber;
				this.MainPartySeaWeightText = num3.ToString();
				this.MainPartySeaCapacityText = num4.ToString();
				this.IsMainPartySeaCapacityWarned = num3 > num4;
			}
			this.ShowMainPartyLandCapacityWarning = this.ShowMainPartyLandCapacityTexts && this.IsMainPartyLandCapacityWarned && !this.PlayerEquipmentCountWarned;
			this.ShowMainPartySeaCapacityWarning = this.ShowMainPartySeaCapacityTexts && this.IsMainPartySeaCapacityWarned && !this.PlayerEquipmentCountWarned;
			if (this.OtherSideHasCapacity)
			{
				int otherSideCurrentWeight = this._inventoryLogic.OtherSideCurrentWeight;
				int capacity = this._inventoryLogic.OtherSideCapacityData.GetCapacity();
				textObject.SetTextVariable("LEFT", otherSideCurrentWeight);
				textObject.SetTextVariable("RIGHT", capacity);
				this.OtherEquipmentCountText = textObject.ToString();
				this.OtherEquipmentCountWarned = otherSideCurrentWeight > capacity;
				this.OtherEquipmentCapacityExceededWarning = this.OtherEquipmentCountWarned && !this.PlayerEquipmentCountWarned && !this.IsMainPartyLandCapacityWarned && !this.IsMainPartySeaCapacityWarned;
			}
			this.NoSaddleText = new TextObject("{=QSPrSsHv}No Saddle!", null).ToString();
			this.NoSaddleHint = new HintViewModel(new TextObject("{=VzCoqt8D}No sadle equipped. -10% penalty to mounted speed and maneuver.", null), null);
			SPItemVM characterMountSlot = this.CharacterMountSlot;
			bool flag;
			if (characterMountSlot != null && !characterMountSlot.ItemRosterElement.IsEmpty)
			{
				SPItemVM characterMountArmorSlot = this.CharacterMountArmorSlot;
				flag = characterMountArmorSlot != null && characterMountArmorSlot.ItemRosterElement.IsEmpty;
			}
			else
			{
				flag = false;
			}
			this.NoSaddleWarned = flag;
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				this.InventoryCapacityHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetPartyInventoryCapacityTooltip(MobileParty.MainParty, false, false));
				this.LandCapacityHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetPartyInventoryCapacityTooltip(MobileParty.MainParty, true, false));
				this.SeaCapacityHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetPartyInventoryCapacityTooltip(MobileParty.MainParty, false, true));
				this.TotalWeightCarriedHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetPartyInventoryWeightTooltip(MobileParty.MainParty, false, false));
				this.LandWeightHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetPartyInventoryWeightTooltip(MobileParty.MainParty, true, false));
				this.SeaWeightHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetPartyInventoryWeightTooltip(MobileParty.MainParty, false, true));
			}
			if (this._isCharacterEquipmentDirty)
			{
				this.MainCharacter.SetEquipment(this.ActiveEquipment);
				this.UpdateCharacterArmorValues();
				this.RefreshCharacterTotalWeight();
			}
			this._isCharacterEquipmentDirty = false;
			this.UpdateIsDoneDisabled();
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00039F84 File Offset: 0x00038184
		public bool IsItemEquipmentPossible(SPItemVM itemVM)
		{
			if (itemVM == null)
			{
				return false;
			}
			if (!itemVM.IsStealthItem && this._equipmentMode == SPInventoryVM.EquipmentModes.Stealth)
			{
				TextObject textObject = new TextObject("{=VEss9aG5}{ITEM_NAME} cannot be equipped with stealth equipment.", null);
				textObject.SetTextVariable("ITEM_NAME", itemVM.ItemRosterElement.EquipmentElement.GetModifiedItemName());
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
				return false;
			}
			if (!itemVM.IsCivilianItem && this._equipmentMode == SPInventoryVM.EquipmentModes.Civilian)
			{
				TextObject textObject2 = new TextObject("{=PsMI3Bn5}{ITEM_NAME} cannot be equipped with civilian equipment.", null);
				textObject2.SetTextVariable("ITEM_NAME", itemVM.ItemRosterElement.EquipmentElement.GetModifiedItemName());
				MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "");
				return false;
			}
			if (!this._currentCharacter.IsPlayerCharacter && (itemVM == null || !itemVM.IsTransferable))
			{
				return false;
			}
			if (this.TargetEquipmentType == EquipmentIndex.None)
			{
				this.TargetEquipmentType = itemVM.GetItemTypeWithItemObject();
				if (this.TargetEquipmentType == EquipmentIndex.None)
				{
					return false;
				}
				if (this.TargetEquipmentType == EquipmentIndex.WeaponItemBeginSlot)
				{
					EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot;
					bool flag = false;
					bool flag2 = false;
					SPItemVM[] array = new SPItemVM[] { this.CharacterWeapon1Slot, this.CharacterWeapon2Slot, this.CharacterWeapon3Slot, this.CharacterWeapon4Slot };
					for (int i = 0; i < array.Length; i++)
					{
						if (string.IsNullOrEmpty(array[i].StringId))
						{
							flag = true;
							equipmentIndex = EquipmentIndex.WeaponItemBeginSlot + i;
							break;
						}
						if (!flag2 && array[i].ItemRosterElement.EquipmentElement.Item.Type == itemVM.ItemRosterElement.EquipmentElement.Item.Type)
						{
							flag2 = true;
							equipmentIndex = EquipmentIndex.WeaponItemBeginSlot + i;
						}
					}
					if (flag || flag2)
					{
						this.TargetEquipmentType = equipmentIndex;
					}
					else
					{
						this.TargetEquipmentType = EquipmentIndex.WeaponItemBeginSlot;
					}
				}
			}
			else if (itemVM.ItemType != this.TargetEquipmentType && (this.TargetEquipmentType < EquipmentIndex.WeaponItemBeginSlot || this.TargetEquipmentType > EquipmentIndex.Weapon3 || itemVM.ItemType < EquipmentIndex.WeaponItemBeginSlot || itemVM.ItemType > EquipmentIndex.Weapon3))
			{
				return false;
			}
			TextObject textObject3;
			if (!this.CanCharacterUseItem(itemVM.ItemRosterElement, out textObject3))
			{
				MBInformationManager.AddQuickInformation(textObject3, 0, null, null, "");
				return false;
			}
			if (!Equipment.IsItemFitsToSlot((EquipmentIndex)this.TargetEquipmentIndex, itemVM.ItemRosterElement.EquipmentElement.Item))
			{
				TextObject textObject4 = new TextObject("{=Omjlnsk3}{ITEM_NAME} cannot be equipped on this slot.", null);
				textObject4.SetTextVariable("ITEM_NAME", itemVM.ItemRosterElement.EquipmentElement.GetModifiedItemName());
				MBInformationManager.AddQuickInformation(textObject4, 0, null, null, "");
				return false;
			}
			if (this.TargetEquipmentType == EquipmentIndex.HorseHarness)
			{
				if (string.IsNullOrEmpty(this.CharacterMountSlot.StringId))
				{
					return false;
				}
				if (!this.ActiveEquipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty && this.ActiveEquipment[EquipmentIndex.ArmorItemEndSlot].Item.HorseComponent.Monster.FamilyType != itemVM.ItemRosterElement.EquipmentElement.Item.ArmorComponent.FamilyType)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0003A259 File Offset: 0x00038459
		private bool CanCharacterUseItem(ItemRosterElement itemRosterElement, out TextObject reason)
		{
			return CharacterHelper.CanUseItem(this._currentCharacter, itemRosterElement.EquipmentElement, out reason);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0003A26E File Offset: 0x0003846E
		private bool CanCharacterUseItem(ItemRosterElement itemRosterElement)
		{
			return CharacterHelper.CanUseItem(this._currentCharacter, itemRosterElement.EquipmentElement);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0003A284 File Offset: 0x00038484
		private void EquipEquipment(SPItemVM itemVM)
		{
			if (itemVM == null || string.IsNullOrEmpty(itemVM.StringId))
			{
				return;
			}
			SPItemVM spitemVM = new SPItemVM();
			spitemVM.RefreshWith(itemVM, this.GetEquipmentToInventorySide(this._equipmentMode));
			if (!this.IsItemEquipmentPossible(spitemVM))
			{
				return;
			}
			SPItemVM itemFromIndex = this.GetItemFromIndex(this.TargetEquipmentType);
			if (itemFromIndex != null && itemFromIndex.ItemRosterElement.EquipmentElement.IsEqualTo(spitemVM.ItemRosterElement.EquipmentElement))
			{
				return;
			}
			bool flag = itemFromIndex != null && itemFromIndex.ItemType != EquipmentIndex.None && InventoryLogic.IsEquipmentSide(itemVM.InventorySide);
			if (!flag)
			{
				EquipmentIndex equipmentIndex = EquipmentIndex.None;
				if (itemVM.ItemRosterElement.EquipmentElement.Item.Type == ItemObject.ItemTypeEnum.Shield && !InventoryLogic.IsEquipmentSide(itemVM.InventorySide))
				{
					for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 <= EquipmentIndex.NumAllWeaponSlots; equipmentIndex2++)
					{
						SPItemVM itemFromIndex2 = this.GetItemFromIndex(equipmentIndex2);
						bool flag2;
						if (itemFromIndex2 == null)
						{
							flag2 = false;
						}
						else
						{
							ItemObject item = itemFromIndex2.ItemRosterElement.EquipmentElement.Item;
							ItemObject.ItemTypeEnum? itemTypeEnum = ((item != null) ? new ItemObject.ItemTypeEnum?(item.Type) : null);
							ItemObject.ItemTypeEnum itemTypeEnum2 = ItemObject.ItemTypeEnum.Shield;
							flag2 = (itemTypeEnum.GetValueOrDefault() == itemTypeEnum2) & (itemTypeEnum != null);
						}
						if (flag2)
						{
							equipmentIndex = equipmentIndex2;
							break;
						}
					}
				}
				if (itemVM != null)
				{
					ItemObject item2 = itemVM.ItemRosterElement.EquipmentElement.Item;
					ItemObject.ItemTypeEnum? itemTypeEnum = ((item2 != null) ? new ItemObject.ItemTypeEnum?(item2.Type) : null);
					ItemObject.ItemTypeEnum itemTypeEnum2 = ItemObject.ItemTypeEnum.Shield;
					if (((itemTypeEnum.GetValueOrDefault() == itemTypeEnum2) & (itemTypeEnum != null)) && equipmentIndex != EquipmentIndex.None)
					{
						this.TargetEquipmentType = equipmentIndex;
					}
				}
			}
			List<TransferCommand> list = new List<TransferCommand>();
			TransferCommand transferCommand = TransferCommand.Transfer(1, itemVM.InventorySide, this.GetEquipmentToInventorySide(this._equipmentMode), spitemVM.ItemRosterElement, spitemVM.ItemType, this.TargetEquipmentType, this._currentCharacter);
			list.Add(transferCommand);
			if (flag)
			{
				TransferCommand transferCommand2 = TransferCommand.Transfer(1, InventoryLogic.InventorySide.PlayerInventory, this.GetEquipmentToInventorySide(this._equipmentMode), itemFromIndex.ItemRosterElement, EquipmentIndex.None, spitemVM.ItemType, this._currentCharacter);
				list.Add(transferCommand2);
			}
			this._inventoryLogic.AddTransferCommands(list);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0003A488 File Offset: 0x00038688
		private void UnequipEquipment(SPItemVM itemVM)
		{
			if (itemVM == null || string.IsNullOrEmpty(itemVM.StringId))
			{
				return;
			}
			TransferCommand transferCommand = TransferCommand.Transfer(1, this.GetEquipmentToInventorySide(this._equipmentMode), InventoryLogic.InventorySide.PlayerInventory, itemVM.ItemRosterElement, itemVM.ItemType, itemVM.ItemType, this._currentCharacter);
			this._inventoryLogic.AddTransferCommand(transferCommand);
			itemVM.IsSelected = false;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0003A4E8 File Offset: 0x000386E8
		private void UpdateEquipment(Equipment equipment, SPItemVM itemVM, EquipmentIndex itemType)
		{
			if (this.ActiveEquipment == equipment)
			{
				this.RefreshEquipment(itemVM, itemType);
			}
			equipment[itemType] = ((itemVM == null) ? default(EquipmentElement) : itemVM.ItemRosterElement.EquipmentElement);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0003A528 File Offset: 0x00038728
		private void UnequipEquipmentWithEquipmentIndex(EquipmentIndex slotType)
		{
			switch (slotType)
			{
			case EquipmentIndex.None:
				return;
			case EquipmentIndex.WeaponItemBeginSlot:
				this.UnequipEquipment(this.CharacterWeapon1Slot);
				return;
			case EquipmentIndex.Weapon1:
				this.UnequipEquipment(this.CharacterWeapon2Slot);
				return;
			case EquipmentIndex.Weapon2:
				this.UnequipEquipment(this.CharacterWeapon3Slot);
				return;
			case EquipmentIndex.Weapon3:
				this.UnequipEquipment(this.CharacterWeapon4Slot);
				return;
			case EquipmentIndex.ExtraWeaponSlot:
				this.UnequipEquipment(this.CharacterBannerSlot);
				return;
			case EquipmentIndex.NumAllWeaponSlots:
				this.UnequipEquipment(this.CharacterHelmSlot);
				return;
			case EquipmentIndex.Body:
				this.UnequipEquipment(this.CharacterTorsoSlot);
				return;
			case EquipmentIndex.Leg:
				this.UnequipEquipment(this.CharacterBootSlot);
				return;
			case EquipmentIndex.Gloves:
				this.UnequipEquipment(this.CharacterGloveSlot);
				return;
			case EquipmentIndex.Cape:
				this.UnequipEquipment(this.CharacterCloakSlot);
				return;
			case EquipmentIndex.ArmorItemEndSlot:
				this.UnequipEquipment(this.CharacterMountSlot);
				if (!string.IsNullOrEmpty(this.CharacterMountArmorSlot.StringId))
				{
					this.UnequipEquipment(this.CharacterMountArmorSlot);
				}
				return;
			case EquipmentIndex.HorseHarness:
				this.UnequipEquipment(this.CharacterMountArmorSlot);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0003A62C File Offset: 0x0003882C
		protected void RefreshEquipment(SPItemVM itemVM, EquipmentIndex itemType)
		{
			InventoryLogic.InventorySide equipmentToInventorySide = this.GetEquipmentToInventorySide(this._equipmentMode);
			switch (itemType)
			{
			case EquipmentIndex.None:
				return;
			case EquipmentIndex.WeaponItemBeginSlot:
				this.CharacterWeapon1Slot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			case EquipmentIndex.Weapon1:
				this.CharacterWeapon2Slot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			case EquipmentIndex.Weapon2:
				this.CharacterWeapon3Slot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			case EquipmentIndex.Weapon3:
				this.CharacterWeapon4Slot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			case EquipmentIndex.ExtraWeaponSlot:
				this.CharacterBannerSlot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			case EquipmentIndex.NumAllWeaponSlots:
				this.CharacterHelmSlot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			case EquipmentIndex.Body:
				this.CharacterTorsoSlot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			case EquipmentIndex.Leg:
				this.CharacterBootSlot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			case EquipmentIndex.Gloves:
				this.CharacterGloveSlot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			case EquipmentIndex.Cape:
				this.CharacterCloakSlot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			case EquipmentIndex.ArmorItemEndSlot:
				this.CharacterMountSlot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			case EquipmentIndex.HorseHarness:
				this.CharacterMountArmorSlot.RefreshWith(itemVM, equipmentToInventorySide);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0003A72C File Offset: 0x0003892C
		private bool UpdateCurrentCharacterIfPossible(int characterIndex, bool isFromRightSide)
		{
			CharacterObject character = (isFromRightSide ? this._rightTroopRoster : this._leftTroopRoster).GetElementCopyAtIndex(characterIndex).Character;
			if (character.IsHero)
			{
				if (!character.HeroObject.CanHeroEquipmentBeChanged())
				{
					Hero mainHero = Hero.MainHero;
					bool flag;
					if (mainHero == null)
					{
						flag = false;
					}
					else
					{
						Clan clan = mainHero.Clan;
						bool? flag2 = ((clan != null) ? new bool?(clan.AliveLords.Contains(character.HeroObject)) : null);
						bool flag3 = true;
						flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
					}
					if (!flag)
					{
						return false;
					}
				}
				this._currentInventoryCharacterIndex = characterIndex;
				this._currentCharacter = character;
				this.MainCharacter.FillFrom(this._currentCharacter.HeroObject, -1, false, false);
				if (this._currentCharacter.IsHero)
				{
					CharacterViewModel mainCharacter = this.MainCharacter;
					IFaction mapFaction = this._currentCharacter.HeroObject.MapFaction;
					mainCharacter.ArmorColor1 = ((mapFaction != null) ? mapFaction.Color : 0U);
					CharacterViewModel mainCharacter2 = this.MainCharacter;
					IFaction mapFaction2 = this._currentCharacter.HeroObject.MapFaction;
					mainCharacter2.ArmorColor2 = ((mapFaction2 != null) ? mapFaction2.Color2 : 0U);
				}
				this.UpdateRightCharacter();
				this.RefreshInformationValues();
				return true;
			}
			return false;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0003A850 File Offset: 0x00038A50
		private bool DoesCompanionExist()
		{
			for (int i = 1; i < this._rightTroopRoster.Count; i++)
			{
				CharacterObject character = this._rightTroopRoster.GetElementCopyAtIndex(i).Character;
				if (character.IsHero && !character.HeroObject.CanHeroEquipmentBeChanged() && character.HeroObject != Hero.MainHero)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0003A8AC File Offset: 0x00038AAC
		private void UpdateLeftCharacter()
		{
			this.IsTradingWithSettlement = false;
			if (this._inventoryLogic.LeftRosterName != null)
			{
				this.LeftInventoryOwnerName = this._inventoryLogic.LeftRosterName.ToString();
				Settlement settlement2 = this._currentCharacter.HeroObject.CurrentSettlement;
				InventoryState activeInventoryState = InventoryScreenHelper.GetActiveInventoryState();
				InventoryScreenHelper.InventoryMode inventoryMode = ((activeInventoryState != null) ? activeInventoryState.InventoryMode : InventoryScreenHelper.InventoryMode.Default);
				if (settlement2 != null && inventoryMode == InventoryScreenHelper.InventoryMode.Warehouse)
				{
					this.IsTradingWithSettlement = true;
					this.ProductionTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetSettlementProductionTooltip(settlement2));
					return;
				}
			}
			else
			{
				Settlement settlement = this._currentCharacter.HeroObject.CurrentSettlement;
				if (settlement != null)
				{
					this.LeftInventoryOwnerName = settlement.Name.ToString();
					this.ProductionTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetSettlementProductionTooltip(settlement));
					this.IsTradingWithSettlement = !settlement.IsHideout;
					if (this._inventoryLogic.InventoryListener != null)
					{
						this.LeftInventoryOwnerGold = this._inventoryLogic.InventoryListener.GetGold();
						return;
					}
				}
				else
				{
					PartyBase oppositePartyFromListener = this._inventoryLogic.OppositePartyFromListener;
					MobileParty mobileParty = ((oppositePartyFromListener != null) ? oppositePartyFromListener.MobileParty : null);
					if (mobileParty != null && (mobileParty.IsCaravan || mobileParty.IsVillager))
					{
						this.LeftInventoryOwnerName = mobileParty.Name.ToString();
						InventoryListener inventoryListener = this._inventoryLogic.InventoryListener;
						this.LeftInventoryOwnerGold = ((inventoryListener != null) ? inventoryListener.GetGold() : 0);
						return;
					}
					this.LeftInventoryOwnerName = GameTexts.FindText("str_loot", null).ToString();
				}
			}
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0003AA44 File Offset: 0x00038C44
		private void UpdateRightCharacter()
		{
			this.UpdateCharacterEquipment();
			this.UpdateCharacterArmorValues();
			this.UpdateCharacterArmorColor();
			this.RefreshCharacterTotalWeight();
			this.RefreshCharacterCanUseItem();
			this.CurrentCharacterName = this._currentCharacter.Name.ToString();
			this.RightInventoryOwnerGold = Hero.MainHero.Gold - this._inventoryLogic.TotalAmount;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0003AAA4 File Offset: 0x00038CA4
		private SPItemVM InitializeCharacterEquipmentSlot(ItemRosterElement itemRosterElement, EquipmentIndex equipmentIndex)
		{
			InventoryLogic.InventorySide equipmentToInventorySide = this.GetEquipmentToInventorySide(this._equipmentMode);
			SPItemVM spitemVM;
			if (!itemRosterElement.IsEmpty)
			{
				spitemVM = new SPItemVM(this._inventoryLogic, this.MainCharacter.IsFemale, this.CanCharacterUseItem(itemRosterElement), this._usageType, itemRosterElement, equipmentToInventorySide, this._inventoryLogic.GetCostOfItemRosterElement(itemRosterElement, equipmentToInventorySide), new EquipmentIndex?(equipmentIndex));
			}
			else
			{
				spitemVM = new SPItemVM();
				spitemVM.RefreshWith(null, equipmentToInventorySide);
			}
			return spitemVM;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0003AB14 File Offset: 0x00038D14
		private void UpdateCharacterEquipment()
		{
			this.CharacterHelmSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.NumAllWeaponSlots), 1), EquipmentIndex.NumAllWeaponSlots);
			this.CharacterCloakSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Cape), 1), EquipmentIndex.Cape);
			this.CharacterTorsoSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Body), 1), EquipmentIndex.Body);
			this.CharacterGloveSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Gloves), 1), EquipmentIndex.Gloves);
			this.CharacterBootSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Leg), 1), EquipmentIndex.Leg);
			this.CharacterMountSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.ArmorItemEndSlot), 1), EquipmentIndex.ArmorItemEndSlot);
			this.CharacterMountArmorSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.HorseHarness), 1), EquipmentIndex.HorseHarness);
			this.CharacterWeapon1Slot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.WeaponItemBeginSlot), 1), EquipmentIndex.WeaponItemBeginSlot);
			this.CharacterWeapon2Slot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon1), 1), EquipmentIndex.Weapon1);
			this.CharacterWeapon3Slot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon2), 1), EquipmentIndex.Weapon2);
			this.CharacterWeapon4Slot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon3), 1), EquipmentIndex.Weapon3);
			this.CharacterBannerSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.ExtraWeaponSlot), 1), EquipmentIndex.ExtraWeaponSlot);
			this.MainCharacter.SetEquipment(this.ActiveEquipment);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0003ACAC File Offset: 0x00038EAC
		private void UpdateCharacterArmorColor()
		{
			if (this.ActiveEquipment.IsStealth)
			{
				this.MainCharacter.ArmorColor1 = 4279111698U;
				this.MainCharacter.ArmorColor2 = 4279111698U;
				return;
			}
			CharacterViewModel mainCharacter = this.MainCharacter;
			IFaction mapFaction = this._currentCharacter.HeroObject.MapFaction;
			mainCharacter.ArmorColor1 = ((mapFaction != null) ? mapFaction.Color : 0U);
			CharacterViewModel mainCharacter2 = this.MainCharacter;
			IFaction mapFaction2 = this._currentCharacter.HeroObject.MapFaction;
			mainCharacter2.ArmorColor2 = ((mapFaction2 != null) ? mapFaction2.Color2 : 0U);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0003AD38 File Offset: 0x00038F38
		private void UpdateCharacterArmorValues()
		{
			Equipment.EquipmentType equipmentType = this.ChangeIntoEquipmentType(this.GetEquipmentToInventorySide(this._equipmentMode));
			this.CurrentCharacterArmArmor = this._currentCharacter.GetArmArmorSum(equipmentType);
			this.CurrentCharacterBodyArmor = this._currentCharacter.GetBodyArmorSum(equipmentType);
			this.CurrentCharacterHeadArmor = this._currentCharacter.GetHeadArmorSum(equipmentType);
			this.CurrentCharacterLegArmor = this._currentCharacter.GetLegArmorSum(equipmentType);
			this.CurrentCharacterHorseArmor = this._currentCharacter.GetHorseArmorSum(equipmentType);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0003ADB2 File Offset: 0x00038FB2
		private Equipment.EquipmentType ChangeIntoEquipmentType(InventoryLogic.InventorySide equipmentMode)
		{
			switch (equipmentMode)
			{
			case InventoryLogic.InventorySide.CivilianEquipment:
				return Equipment.EquipmentType.Civilian;
			case InventoryLogic.InventorySide.BattleEquipment:
				return Equipment.EquipmentType.Battle;
			case InventoryLogic.InventorySide.StealthEquipment:
				return Equipment.EquipmentType.Stealth;
			default:
				Debug.FailedAssert("Cannot change InventoryLogic EquipmentMode to EquiptmentType", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Inventory\\SPInventoryVM.cs", "ChangeIntoEquipmentType", 1906);
				return Equipment.EquipmentType.Invalid;
			}
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0003ADEC File Offset: 0x00038FEC
		private void RefreshCharacterTotalWeight()
		{
			CharacterObject currentCharacter = this._currentCharacter;
			float num = ((currentCharacter != null && currentCharacter.GetPerkValue(DefaultPerks.Athletics.FormFittingArmor)) ? (1f + DefaultPerks.Athletics.FormFittingArmor.PrimaryBonus) : 1f);
			this.CurrentCharacterTotalEncumbrance = MathF.Round(this.ActiveEquipment.GetTotalWeightOfWeapons() + this.ActiveEquipment.GetTotalWeightOfArmor(true) * num, 1).ToString("0.0");
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0003AE60 File Offset: 0x00039060
		private void RefreshCharacterCanUseItem()
		{
			for (int i = 0; i < this.RightItemListVM.Count; i++)
			{
				this.RightItemListVM[i].CanCharacterUseItem = this.CanCharacterUseItem(this.RightItemListVM[i].ItemRosterElement);
			}
			for (int j = 0; j < this.LeftItemListVM.Count; j++)
			{
				this.LeftItemListVM[j].CanCharacterUseItem = this.CanCharacterUseItem(this.LeftItemListVM[j].ItemRosterElement);
			}
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0003AEEC File Offset: 0x000390EC
		private void InitializeInventory()
		{
			this.IsRefreshed = false;
			switch (this._inventoryLogic.MerchantItemType)
			{
			case InventoryScreenHelper.InventoryCategoryType.Armors:
				this.ActiveFilterIndex = 3;
				break;
			case InventoryScreenHelper.InventoryCategoryType.Weapon:
				this.ActiveFilterIndex = 1;
				break;
			case InventoryScreenHelper.InventoryCategoryType.Shield:
				this.ActiveFilterIndex = 2;
				break;
			case InventoryScreenHelper.InventoryCategoryType.HorseCategory:
				this.ActiveFilterIndex = 4;
				break;
			case InventoryScreenHelper.InventoryCategoryType.Goods:
				this.ActiveFilterIndex = 5;
				break;
			default:
				this.ActiveFilterIndex = 0;
				break;
			}
			this.RightItemListVM.Clear();
			this.LeftItemListVM.Clear();
			int num = MathF.Max(this._inventoryLogic.GetElementCountOnSide(InventoryLogic.InventorySide.PlayerInventory), this._inventoryLogic.GetElementCountOnSide(InventoryLogic.InventorySide.OtherInventory));
			ItemRosterElement[] array = (from i in this._inventoryLogic.GetElementsInRoster(InventoryLogic.InventorySide.PlayerInventory)
				orderby i.EquipmentElement.GetModifiedItemName().ToString()
				select i).ToArray<ItemRosterElement>();
			ItemRosterElement[] array2 = (from i in this._inventoryLogic.GetElementsInRoster(InventoryLogic.InventorySide.OtherInventory)
				orderby i.EquipmentElement.GetModifiedItemName().ToString()
				select i).ToArray<ItemRosterElement>();
			this._lockedItemIDs = this._viewDataTracker.GetInventoryLocks().ToList<string>();
			for (int j = 0; j < num; j++)
			{
				if (j < array.Length)
				{
					ItemRosterElement itemRosterElement = array[j];
					SPItemVM spitemVM = new SPItemVM(this._inventoryLogic, this.MainCharacter.IsFemale, this.CanCharacterUseItem(itemRosterElement), this._usageType, itemRosterElement, InventoryLogic.InventorySide.PlayerInventory, this._inventoryLogic.GetCostOfItemRosterElement(itemRosterElement, InventoryLogic.InventorySide.PlayerInventory), null);
					this.UpdateFilteredStatusOfItem(spitemVM);
					spitemVM.IsLocked = spitemVM.InventorySide == InventoryLogic.InventorySide.PlayerInventory && this.IsItemLocked(itemRosterElement);
					this.RightItemListVM.Add(spitemVM);
				}
				if (j < array2.Length)
				{
					ItemRosterElement itemRosterElement2 = array2[j];
					SPItemVM spitemVM2 = new SPItemVM(this._inventoryLogic, this.MainCharacter.IsFemale, this.CanCharacterUseItem(itemRosterElement2), this._usageType, itemRosterElement2, InventoryLogic.InventorySide.OtherInventory, this._inventoryLogic.GetCostOfItemRosterElement(itemRosterElement2, InventoryLogic.InventorySide.OtherInventory), null);
					this.UpdateFilteredStatusOfItem(spitemVM2);
					spitemVM2.IsLocked = spitemVM2.InventorySide == InventoryLogic.InventorySide.PlayerInventory && this.IsItemLocked(itemRosterElement2);
					this.LeftItemListVM.Add(spitemVM2);
				}
			}
			this.RefreshInformationValues();
			this.IsRefreshed = true;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0003B140 File Offset: 0x00039340
		private bool IsItemLocked(ItemRosterElement item)
		{
			string text = item.EquipmentElement.Item.StringId;
			if (item.EquipmentElement.ItemModifier != null)
			{
				text += item.EquipmentElement.ItemModifier.StringId;
			}
			return this._lockedItemIDs.Contains(text);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0003B19A File Offset: 0x0003939A
		public void CompareNextItem()
		{
			this.CycleBetweenWeaponSlots();
			this.RefreshComparedItem();
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0003B1A8 File Offset: 0x000393A8
		public void ExecuteSelectItem(ItemVM item)
		{
			if (item != null)
			{
				SPItemVM spitemVM = item as SPItemVM;
				if (spitemVM == null || !spitemVM.IsSelected)
				{
					goto IL_0049;
				}
			}
			using (IEnumerator<SPItemVM> enumerator = this.GetAllItems(true).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SPItemVM spitemVM2 = enumerator.Current;
					spitemVM2.IsSelected = false;
				}
				return;
			}
			IL_0049:
			if (this.GetEquippedItems().Contains(item))
			{
				foreach (SPItemVM spitemVM3 in this.GetAllItems(false))
				{
					spitemVM3.IsSelected = false;
				}
				using (IEnumerator<SPItemVM> enumerator = this.GetEquippedItems().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SPItemVM spitemVM4 = enumerator.Current;
						spitemVM4.IsSelected = spitemVM4 == item;
					}
					return;
				}
			}
			foreach (SPItemVM spitemVM5 in this.GetEquippedItems())
			{
				spitemVM5.IsSelected = false;
			}
			foreach (SPItemVM spitemVM6 in this.GetAllItems(false))
			{
				spitemVM6.IsSelected = spitemVM6.ItemRosterElement.EquipmentElement.IsEqualTo(item.ItemRosterElement.EquipmentElement);
				if (spitemVM6.IsSelected)
				{
					this.ScrollItemId = spitemVM6.ItemRosterElement.EquipmentElement.Item.StringId;
					this.ScrollToItem = true;
				}
			}
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0003B358 File Offset: 0x00039558
		public void ExecuteClearSelectedItem()
		{
			this.ExecuteSelectItem(null);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0003B361 File Offset: 0x00039561
		private IEnumerable<SPItemVM> GetAllItems(bool includeEquipped)
		{
			foreach (SPItemVM spitemVM in this.LeftItemListVM)
			{
				yield return spitemVM;
			}
			IEnumerator<SPItemVM> enumerator = null;
			foreach (SPItemVM spitemVM2 in this.RightItemListVM)
			{
				yield return spitemVM2;
			}
			enumerator = null;
			if (includeEquipped)
			{
				foreach (SPItemVM spitemVM3 in this.GetEquippedItems())
				{
					yield return spitemVM3;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0003B378 File Offset: 0x00039578
		private IEnumerable<SPItemVM> GetEquippedItems()
		{
			yield return this.CharacterHelmSlot;
			yield return this.CharacterCloakSlot;
			yield return this.CharacterTorsoSlot;
			yield return this.CharacterGloveSlot;
			yield return this.CharacterBootSlot;
			yield return this.CharacterMountSlot;
			yield return this.CharacterMountArmorSlot;
			yield return this.CharacterWeapon1Slot;
			yield return this.CharacterWeapon2Slot;
			yield return this.CharacterWeapon3Slot;
			yield return this.CharacterWeapon4Slot;
			yield return this.CharacterBannerSlot;
			yield break;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0003B388 File Offset: 0x00039588
		public bool IsAnyEquippedItemSelected()
		{
			using (IEnumerator<SPItemVM> enumerator = this.GetEquippedItems().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsSelected)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0003B3DC File Offset: 0x000395DC
		private void BuyItem(SPItemVM item)
		{
			if (this.TargetEquipmentType != EquipmentIndex.None && item.ItemType != this.TargetEquipmentType && (this.TargetEquipmentType < EquipmentIndex.WeaponItemBeginSlot || this.TargetEquipmentType > EquipmentIndex.ExtraWeaponSlot || item.ItemType < EquipmentIndex.WeaponItemBeginSlot || item.ItemType > EquipmentIndex.ExtraWeaponSlot))
			{
				return;
			}
			if (this.TargetEquipmentType == EquipmentIndex.None)
			{
				this.TargetEquipmentType = item.ItemType;
				if (item.ItemType >= EquipmentIndex.WeaponItemBeginSlot && item.ItemType <= EquipmentIndex.ExtraWeaponSlot)
				{
					this.TargetEquipmentType = this.ActiveEquipment.GetWeaponPickUpSlotIndex(item.ItemRosterElement.EquipmentElement, false);
				}
			}
			int num = item.ItemCount;
			if (item.InventorySide == InventoryLogic.InventorySide.PlayerInventory)
			{
				ItemRosterElement? itemRosterElement = this._inventoryLogic.FindItemFromSide(InventoryLogic.InventorySide.OtherInventory, item.ItemRosterElement.EquipmentElement);
				if (itemRosterElement != null)
				{
					num = itemRosterElement.Value.Amount;
				}
			}
			TransferCommand transferCommand = TransferCommand.Transfer(MathF.Min(this.TransactionCount, num), InventoryLogic.InventorySide.OtherInventory, InventoryLogic.InventorySide.PlayerInventory, item.ItemRosterElement, item.ItemType, this.TargetEquipmentType, this._currentCharacter);
			this._inventoryLogic.AddTransferCommand(transferCommand);
			if (this.EquipAfterBuy)
			{
				this._equipAfterTransferStack.Push(item);
			}
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0003B4F8 File Offset: 0x000396F8
		private void SellItem(SPItemVM item)
		{
			InventoryLogic.InventorySide inventorySide = item.InventorySide;
			int num = item.ItemCount;
			if (inventorySide == InventoryLogic.InventorySide.OtherInventory)
			{
				inventorySide = InventoryLogic.InventorySide.PlayerInventory;
				ItemRosterElement? itemRosterElement = this._inventoryLogic.FindItemFromSide(InventoryLogic.InventorySide.PlayerInventory, item.ItemRosterElement.EquipmentElement);
				if (itemRosterElement != null)
				{
					num = itemRosterElement.Value.Amount;
				}
			}
			TransferCommand transferCommand = TransferCommand.Transfer(MathF.Min(this.TransactionCount, num), inventorySide, InventoryLogic.InventorySide.OtherInventory, item.ItemRosterElement, item.ItemType, this.TargetEquipmentType, this._currentCharacter);
			this._inventoryLogic.AddTransferCommand(transferCommand);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0003B584 File Offset: 0x00039784
		private void SlaughterItem(SPItemVM item)
		{
			int num = 1;
			if (this.IsFiveStackModifierActive)
			{
				num = MathF.Min(5, item.ItemCount);
			}
			else if (this.IsEntireStackModifierActive)
			{
				num = item.ItemCount;
			}
			for (int i = 0; i < num; i++)
			{
				this._inventoryLogic.SlaughterItem(item.ItemRosterElement);
			}
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0003B5D8 File Offset: 0x000397D8
		private void DonateItem(SPItemVM item)
		{
			if (this.IsFiveStackModifierActive)
			{
				int itemCount = item.ItemCount;
				for (int i = 0; i < MathF.Min(5, itemCount); i++)
				{
					this._inventoryLogic.DonateItem(item.ItemRosterElement);
				}
				return;
			}
			this._inventoryLogic.DonateItem(item.ItemRosterElement);
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0003B62C File Offset: 0x0003982C
		private float GetCapacityBudget(MobileParty party, bool isBuy)
		{
			if (isBuy)
			{
				if (party != null)
				{
					return (float)party.InventoryCapacity - party.TotalWeightCarried;
				}
				return 0f;
			}
			else
			{
				if (this._inventoryLogic.OtherSideCapacityData != null)
				{
					return (float)(this._inventoryLogic.OtherSideCapacityData.GetCapacity() - this._inventoryLogic.OtherSideCurrentWeight);
				}
				return -1f;
			}
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0003B684 File Offset: 0x00039884
		private void TransferAll(bool isBuy)
		{
			this.IsRefreshed = false;
			List<TransferCommand> list = new List<TransferCommand>(this.LeftItemListVM.Count);
			MBBindingList<SPItemVM> mbbindingList = new MBBindingList<SPItemVM>();
			foreach (SPItemVM spitemVM in (isBuy ? this.LeftItemListVM : this.RightItemListVM))
			{
				if (spitemVM != null && !spitemVM.IsFiltered && spitemVM != null && !spitemVM.IsLocked && spitemVM != null && spitemVM.IsTransferable)
				{
					mbbindingList.Add(spitemVM);
				}
			}
			MobileParty mobileParty;
			if (!isBuy)
			{
				PartyBase otherParty = this._inventoryLogic.OtherParty;
				mobileParty = ((otherParty != null) ? otherParty.MobileParty : null);
			}
			else
			{
				mobileParty = MobileParty.MainParty;
			}
			MobileParty mobileParty2 = mobileParty;
			PartyBase otherParty2 = this._inventoryLogic.OtherParty;
			bool flag = otherParty2 != null && otherParty2.IsSettlement;
			InventoryCapacityModel inventoryCapacityModel = Campaign.Current.Models.InventoryCapacityModel;
			mbbindingList.Sort(new SPInventoryVM.RosterElementComparer(inventoryCapacityModel, mobileParty2, flag));
			InventoryLogic.InventorySide inventorySide = (isBuy ? InventoryLogic.InventorySide.OtherInventory : InventoryLogic.InventorySide.PlayerInventory);
			InventoryLogic.InventorySide inventorySide2 = (isBuy ? InventoryLogic.InventorySide.PlayerInventory : InventoryLogic.InventorySide.OtherInventory);
			if (flag && !isBuy)
			{
				this.TransferAllForSettlement(mbbindingList, inventorySide, inventorySide2, list);
			}
			else
			{
				InventoryState activeInventoryState = InventoryScreenHelper.GetActiveInventoryState();
				bool flag2 = ((activeInventoryState != null) ? activeInventoryState.InventoryMode : InventoryScreenHelper.InventoryMode.Default) == InventoryScreenHelper.InventoryMode.Warehouse;
				float num = 0f;
				float num2 = 0f;
				if (mbbindingList.Count > 0)
				{
					if (mobileParty2 != null)
					{
						TextObject textObject;
						num2 = inventoryCapacityModel.GetItemEffectiveWeight(mbbindingList[0].ItemRosterElement.EquipmentElement, mobileParty2, mobileParty2.IsCurrentlyAtSea, out textObject);
					}
					else if (flag2)
					{
						num2 = mbbindingList[0].ItemRosterElement.EquipmentElement.GetEquipmentElementWeight();
					}
				}
				float num3 = this.GetCapacityBudget(mobileParty2, isBuy);
				bool flag3 = num3 < num2;
				bool flag4 = this._inventoryLogic.CanInventoryCapacityIncrease(inventorySide2);
				if (!flag3 && flag4)
				{
					List<TransferCommand> list2 = new List<TransferCommand>(0);
					for (int i = 0; i < mbbindingList.Count; i++)
					{
						SPItemVM spitemVM2 = mbbindingList[i];
						if (!this._inventoryLogic.GetCanItemIncreaseInventoryCapacity(spitemVM2.ItemRosterElement.EquipmentElement.Item))
						{
							break;
						}
						TransferCommand transferCommand = TransferCommand.Transfer(spitemVM2.ItemRosterElement.Amount, inventorySide, inventorySide2, spitemVM2.ItemRosterElement, EquipmentIndex.None, EquipmentIndex.None, this._currentCharacter);
						list2.Add(transferCommand);
						mbbindingList.Remove(spitemVM2);
						i--;
					}
					if (list2.Count > 0)
					{
						this._inventoryLogic.AddTransferCommands(list2);
						list2.Clear();
						num3 = this.GetCapacityBudget(mobileParty2, isBuy);
					}
				}
				int num4 = mbbindingList.Count - 1;
				while (0 <= num4)
				{
					SPItemVM spitemVM3 = mbbindingList[num4];
					int num5 = spitemVM3.ItemRosterElement.Amount;
					if (!flag3)
					{
						TextObject textObject2;
						float num6 = (flag2 ? spitemVM3.ItemRosterElement.EquipmentElement.GetEquipmentElementWeight() : inventoryCapacityModel.GetItemEffectiveWeight(spitemVM3.ItemRosterElement.EquipmentElement, mobileParty2, mobileParty2.IsCurrentlyAtSea, out textObject2));
						float num7 = num + num6 * (float)num5;
						if (num5 > 0 && num7 > num3)
						{
							num5 = MBMath.ClampInt(num5, 0, MathF.Floor((num3 - num) / num6));
						}
						num += (float)num5 * num6;
					}
					if (num5 > 0)
					{
						TransferCommand transferCommand2 = TransferCommand.Transfer(num5, inventorySide, inventorySide2, spitemVM3.ItemRosterElement, EquipmentIndex.None, EquipmentIndex.None, this._currentCharacter);
						list.Add(transferCommand2);
					}
					num4--;
				}
			}
			this._inventoryLogic.AddTransferCommands(list);
			this.RefreshInformationValues();
			this.ExecuteRemoveZeroCounts();
			this.IsRefreshed = true;
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0003B9F0 File Offset: 0x00039BF0
		private void TransferAllForSettlement(MBBindingList<SPItemVM> list, InventoryLogic.InventorySide fromSide, InventoryLogic.InventorySide toSide, List<TransferCommand> commands)
		{
			float num = (float)this.LeftInventoryOwnerGold;
			float num2 = float.MaxValue;
			float num3 = 0f;
			foreach (SPItemVM spitemVM in list)
			{
				int itemCost = spitemVM.ItemCost;
				if ((float)itemCost < num2)
				{
					num2 = (float)itemCost;
				}
			}
			bool flag = num < num2;
			int num4 = list.Count - 1;
			while (0 <= num4)
			{
				SPItemVM spitemVM2 = list[num4];
				int amount = spitemVM2.ItemRosterElement.Amount;
				if (!flag)
				{
					for (int i = 0; i < amount; i++)
					{
						float num5 = (float)spitemVM2.ItemCost;
						num3 += num5;
						if (num3 >= num)
						{
							num3 -= num5;
							break;
						}
						this._inventoryLogic.AddTransferCommands(new List<TransferCommand> { TransferCommand.Transfer(1, fromSide, toSide, spitemVM2.ItemRosterElement, EquipmentIndex.None, EquipmentIndex.None, this._currentCharacter) });
					}
				}
				else
				{
					TransferCommand transferCommand = TransferCommand.Transfer(amount, fromSide, toSide, spitemVM2.ItemRosterElement, EquipmentIndex.None, EquipmentIndex.None, this._currentCharacter);
					commands.Add(transferCommand);
				}
				num4--;
			}
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0003BB18 File Offset: 0x00039D18
		public void ExecuteSelectStealthOutfit()
		{
			this.EquipmentMode = 2;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0003BB21 File Offset: 0x00039D21
		public void ExecuteSelectBattleOutfit()
		{
			this.EquipmentMode = 1;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0003BB2A File Offset: 0x00039D2A
		public void ExecuteSelectCivilianOutfit()
		{
			this.EquipmentMode = 0;
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0003BB33 File Offset: 0x00039D33
		public void ExecuteBuyAllItems()
		{
			this.TransferAll(true);
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0003BB3C File Offset: 0x00039D3C
		public void ExecuteSellAllItems()
		{
			this.TransferAll(false);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0003BB48 File Offset: 0x00039D48
		public void ExecuteBuyItemTest()
		{
			this.TransactionCount = 1;
			this.EquipAfterBuy = false;
			int totalGold = Hero.MainHero.Gold;
			foreach (SPItemVM spitemVM in this.LeftItemListVM.Where<SPItemVM>(delegate(SPItemVM i)
			{
				ItemObject item = i.ItemRosterElement.EquipmentElement.Item;
				return item != null && item.IsFood && i.ItemCost <= totalGold;
			}))
			{
				if (spitemVM.ItemCost <= totalGold)
				{
					this.ProcessBuyItem(spitemVM, false);
					totalGold -= spitemVM.ItemCost;
				}
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0003BBEC File Offset: 0x00039DEC
		public void ExecuteResetTranstactions()
		{
			this._inventoryLogic.Reset(false);
			InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_inventory_reset_message", null).ToString()));
			this.CurrentFocusedItem = null;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0003BC1B File Offset: 0x00039E1B
		public void ExecuteResetAndCompleteTranstactionsWithoutInquiry()
		{
			this.ExecuteResetAndCompleteTranstactions(false);
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0003BC24 File Offset: 0x00039E24
		public void ExecuteResetAndCompleteTranstactions(bool showCancelInquiry = false)
		{
			this.ExecuteRemoveZeroCounts();
			InventoryState activeInventoryState = InventoryScreenHelper.GetActiveInventoryState();
			if (((activeInventoryState != null) ? activeInventoryState.InventoryMode : InventoryScreenHelper.InventoryMode.Default) == InventoryScreenHelper.InventoryMode.Loot)
			{
				if (this._inventoryLogic.IsThereAnyChanges())
				{
					InformationManager.ShowInquiry(new InquiryData("", GameTexts.FindText("str_cancelling_changes", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
					{
						InventoryScreenHelper.CloseScreen(true);
					}, null, "", 0f, null, null, null), false, false);
					return;
				}
				if (this._inventoryLogic.GetElementsInInitialRoster(InventoryLogic.InventorySide.OtherInventory).Any<ItemRosterElement>())
				{
					InformationManager.ShowInquiry(new InquiryData("", GameTexts.FindText("str_leaving_loot_behind", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
					{
						InventoryScreenHelper.CloseScreen(true);
					}, null, "", 0f, null, null, null), false, false);
					return;
				}
				InventoryScreenHelper.CloseScreen(true);
				return;
			}
			else
			{
				if (showCancelInquiry && this._inventoryLogic.IsThereAnyChanges())
				{
					InformationManager.ShowInquiry(new InquiryData("", GameTexts.FindText("str_cancelling_changes", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
					{
						InventoryScreenHelper.CloseScreen(true);
					}, null, "", 0f, null, null, null), false, false);
					return;
				}
				InventoryScreenHelper.CloseScreen(true);
				return;
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0003BDE0 File Offset: 0x00039FE0
		public void ExecuteCompleteTranstactions()
		{
			this.ExecuteRemoveZeroCounts();
			InventoryState activeInventoryState = InventoryScreenHelper.GetActiveInventoryState();
			if (((activeInventoryState != null) ? activeInventoryState.InventoryMode : InventoryScreenHelper.InventoryMode.Default) == InventoryScreenHelper.InventoryMode.Loot && !this.PlayerEquipmentCountWarned && this._inventoryLogic.GetElementCountOnSide(InventoryLogic.InventorySide.OtherInventory) > 0)
			{
				InformationManager.ShowInquiry(new InquiryData("", GameTexts.FindText("str_leaving_loot_behind", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.HandleDone), null, "", 0f, null, null, null), false, false);
				return;
			}
			if (this._inventoryLogic.IsThereAnyChanges() && (this.PlayerEquipmentCountWarned || this.OtherEquipmentCountWarned))
			{
				GameTexts.SetVariable("newline", "\n");
				string text = string.Empty;
				if (this.PlayerEquipmentCountWarned)
				{
					text = GameTexts.FindText("str_inventory_over_limit", null).ToString();
				}
				else if (this.OtherEquipmentCountWarned)
				{
					text = GameTexts.FindText("str_inventory_other_party_over_limit", null).ToString();
				}
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=uJro3Bua}Over Limit", null).ToString(), text, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.HandleDone), null, "", 0f, null, null, null), false, false);
				return;
			}
			this.HandleDone();
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0003BF48 File Offset: 0x0003A148
		private void HandleDone()
		{
			MBInformationManager.HideInformations();
			bool flag = this._inventoryLogic.TotalAmount < 0;
			InventoryListener inventoryListener = this._inventoryLogic.InventoryListener;
			bool flag2 = ((inventoryListener != null) ? inventoryListener.GetGold() : 0) >= MathF.Abs(this._inventoryLogic.TotalAmount);
			int num = (int)this._inventoryLogic.XpGainFromDonations;
			int num2 = ((this._usageType == InventoryScreenHelper.InventoryMode.Default && num == 0 && !Game.Current.CheatMode) ? this._inventoryLogic.GetElementCountOnSide(InventoryLogic.InventorySide.OtherInventory) : 0);
			if (flag && !flag2)
			{
				InformationManager.ShowInquiry(new InquiryData("", GameTexts.FindText("str_trader_doesnt_have_enough_money", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					InventoryScreenHelper.CloseScreen(false);
				}, null, "", 0f, null, null, null), false, false);
			}
			else if (num2 > 0)
			{
				InformationManager.ShowInquiry(new InquiryData("", GameTexts.FindText("str_discarding_items", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					InventoryScreenHelper.CloseScreen(false);
				}, null, "", 0f, null, null, null), false, false);
			}
			else
			{
				InventoryScreenHelper.CloseScreen(false);
			}
			this.SaveItemLockStates();
			this.SaveItemSortStates();
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0003C0CE File Offset: 0x0003A2CE
		private void SaveItemLockStates()
		{
			this._viewDataTracker.SetInventoryLocks(this._lockedItemIDs);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0003C0E4 File Offset: 0x0003A2E4
		private void SaveItemSortStates()
		{
			this._viewDataTracker.InventorySetSortPreference((int)this._usageType, (int)this.PlayerInventorySortController.CurrentSortOption.Value, (int)this.PlayerInventorySortController.CurrentSortState.Value);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0003C128 File Offset: 0x0003A328
		public void ExecuteTransferWithParameters(SPItemVM item, int index, string targetTag)
		{
			bool isTransferable = item.IsTransferable;
			bool flag = targetTag == "PlayerInventory" || targetTag.StartsWith("Equipment");
			bool isPlayerCharacter = this._currentCharacter.IsPlayerCharacter;
			if (!isTransferable && (!flag || !isPlayerCharacter))
			{
				return;
			}
			if (targetTag == "OverCharacter")
			{
				this.TargetEquipmentIndex = -1;
				if (item.InventorySide == InventoryLogic.InventorySide.OtherInventory)
				{
					item.TransactionCount = 1;
					this.TransactionCount = 1;
					this.ProcessEquipItem(item);
					return;
				}
				if (item.InventorySide == InventoryLogic.InventorySide.PlayerInventory)
				{
					this.ProcessEquipItem(item);
					return;
				}
			}
			else if (targetTag == "PlayerInventory")
			{
				this.TargetEquipmentIndex = -1;
				if (item.InventorySide == this.GetEquipmentToInventorySide(this._equipmentMode))
				{
					this.ProcessUnequipItem(item);
					return;
				}
				if (item.InventorySide == InventoryLogic.InventorySide.OtherInventory)
				{
					item.TransactionCount = item.ItemCount;
					this.TransactionCount = item.ItemCount;
					this.ProcessBuyItem(item, false);
					return;
				}
			}
			else if (targetTag == "OtherInventory")
			{
				if (item.InventorySide != InventoryLogic.InventorySide.OtherInventory)
				{
					item.TransactionCount = item.ItemCount;
					this.TransactionCount = item.ItemCount;
					this.ProcessSellItem(item, false);
					return;
				}
			}
			else if (targetTag.StartsWith("Equipment"))
			{
				this.TargetEquipmentIndex = int.Parse(targetTag.Substring("Equipment".Length + 1));
				if (item.InventorySide == InventoryLogic.InventorySide.OtherInventory)
				{
					item.TransactionCount = 1;
					this.TransactionCount = 1;
					this.ProcessEquipItem(item);
					return;
				}
				if (item.InventorySide == InventoryLogic.InventorySide.PlayerInventory || item.InventorySide == this.GetEquipmentToInventorySide(this._equipmentMode))
				{
					this.ProcessEquipItem(item);
				}
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0003C2B3 File Offset: 0x0003A4B3
		private void UpdateIsDoneDisabled()
		{
			this.IsDoneDisabled = !this._inventoryLogic.CanPlayerCompleteTransaction();
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0003C2CC File Offset: 0x0003A4CC
		private void ProcessFilter(SPInventoryVM.Filters filterIndex)
		{
			this.ActiveFilterIndex = (int)filterIndex;
			this.IsRefreshed = false;
			foreach (SPItemVM spitemVM in this.LeftItemListVM)
			{
				if (spitemVM != null)
				{
					this.UpdateFilteredStatusOfItem(spitemVM);
				}
			}
			foreach (SPItemVM spitemVM2 in this.RightItemListVM)
			{
				if (spitemVM2 != null)
				{
					this.UpdateFilteredStatusOfItem(spitemVM2);
				}
			}
			this.IsRefreshed = true;
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0003C370 File Offset: 0x0003A570
		private void UpdateFilteredStatusOfItem(SPItemVM item)
		{
			bool flag = !this._filters[this._activeFilterIndex].Contains(item.TypeId);
			bool flag2 = false;
			if (this.IsSearchAvailable && (item.InventorySide == InventoryLogic.InventorySide.OtherInventory || item.InventorySide == InventoryLogic.InventorySide.PlayerInventory))
			{
				string text = ((item.InventorySide == InventoryLogic.InventorySide.OtherInventory) ? this.LeftSearchText : this.RightSearchText);
				if (text.Length > 1)
				{
					flag2 = !item.ItemDescription.ToLower().Contains(text);
				}
			}
			item.IsFiltered = flag || flag2;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0003C3FB File Offset: 0x0003A5FB
		private void OnSearchTextChanged(bool isLeft)
		{
			if (this.IsSearchAvailable)
			{
				(isLeft ? this.LeftItemListVM : this.RightItemListVM).ApplyActionOnAllItems(delegate(SPItemVM x)
				{
					this.UpdateFilteredStatusOfItem(x);
				});
			}
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0003C427 File Offset: 0x0003A627
		public void ExecuteFilterNone()
		{
			this.ProcessFilter(SPInventoryVM.Filters.All);
			Game.Current.EventManager.TriggerEvent<InventoryFilterChangedEvent>(new InventoryFilterChangedEvent(SPInventoryVM.Filters.All));
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0003C445 File Offset: 0x0003A645
		public void ExecuteFilterWeapons()
		{
			this.ProcessFilter(SPInventoryVM.Filters.Weapons);
			Game.Current.EventManager.TriggerEvent<InventoryFilterChangedEvent>(new InventoryFilterChangedEvent(SPInventoryVM.Filters.Weapons));
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0003C463 File Offset: 0x0003A663
		public void ExecuteFilterArmors()
		{
			this.ProcessFilter(SPInventoryVM.Filters.Armors);
			Game.Current.EventManager.TriggerEvent<InventoryFilterChangedEvent>(new InventoryFilterChangedEvent(SPInventoryVM.Filters.Armors));
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0003C481 File Offset: 0x0003A681
		public void ExecuteFilterShieldsAndRanged()
		{
			this.ProcessFilter(SPInventoryVM.Filters.ShieldsAndRanged);
			Game.Current.EventManager.TriggerEvent<InventoryFilterChangedEvent>(new InventoryFilterChangedEvent(SPInventoryVM.Filters.ShieldsAndRanged));
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0003C49F File Offset: 0x0003A69F
		public void ExecuteFilterMounts()
		{
			this.ProcessFilter(SPInventoryVM.Filters.Mounts);
			Game.Current.EventManager.TriggerEvent<InventoryFilterChangedEvent>(new InventoryFilterChangedEvent(SPInventoryVM.Filters.Mounts));
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0003C4BD File Offset: 0x0003A6BD
		public void ExecuteFilterMisc()
		{
			this.ProcessFilter(SPInventoryVM.Filters.Miscellaneous);
			Game.Current.EventManager.TriggerEvent<InventoryFilterChangedEvent>(new InventoryFilterChangedEvent(SPInventoryVM.Filters.Miscellaneous));
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0003C4DC File Offset: 0x0003A6DC
		public void CycleBetweenWeaponSlots()
		{
			EquipmentIndex selectedEquipmentIndex = (EquipmentIndex)this._selectedEquipmentIndex;
			if (selectedEquipmentIndex >= EquipmentIndex.WeaponItemBeginSlot && selectedEquipmentIndex < EquipmentIndex.NumAllWeaponSlots)
			{
				int selectedEquipmentIndex2 = this._selectedEquipmentIndex;
				do
				{
					if (this._selectedEquipmentIndex < 3)
					{
						this._selectedEquipmentIndex++;
					}
					else
					{
						this._selectedEquipmentIndex = 0;
					}
				}
				while (this._selectedEquipmentIndex != selectedEquipmentIndex2 && this.GetItemFromIndex((EquipmentIndex)this._selectedEquipmentIndex).ItemRosterElement.EquipmentElement.Item == null);
			}
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0003C548 File Offset: 0x0003A748
		private SPItemVM GetItemFromIndex(EquipmentIndex itemType)
		{
			switch (itemType)
			{
			case EquipmentIndex.WeaponItemBeginSlot:
				return this.CharacterWeapon1Slot;
			case EquipmentIndex.Weapon1:
				return this.CharacterWeapon2Slot;
			case EquipmentIndex.Weapon2:
				return this.CharacterWeapon3Slot;
			case EquipmentIndex.Weapon3:
				return this.CharacterWeapon4Slot;
			case EquipmentIndex.ExtraWeaponSlot:
				return this.CharacterBannerSlot;
			case EquipmentIndex.NumAllWeaponSlots:
				return this.CharacterHelmSlot;
			case EquipmentIndex.Body:
				return this.CharacterTorsoSlot;
			case EquipmentIndex.Leg:
				return this.CharacterBootSlot;
			case EquipmentIndex.Gloves:
				return this.CharacterGloveSlot;
			case EquipmentIndex.Cape:
				return this.CharacterCloakSlot;
			case EquipmentIndex.ArmorItemEndSlot:
				return this.CharacterMountSlot;
			case EquipmentIndex.HorseHarness:
				return this.CharacterMountArmorSlot;
			default:
				return null;
			}
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0003C5E4 File Offset: 0x0003A7E4
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (obj.NewNotificationElementID != this._latestTutorialElementID)
			{
				if (this._latestTutorialElementID != null)
				{
					if (obj.NewNotificationElementID != "TransferButtonOnlyFood" && this._isFoodTransferButtonHighlightApplied)
					{
						this.SetFoodTransferButtonHighlightState(false);
						this._isFoodTransferButtonHighlightApplied = false;
					}
					if (obj.NewNotificationElementID != "InventoryMicsFilter" && this.IsMicsFilterHighlightEnabled)
					{
						this.IsMicsFilterHighlightEnabled = false;
					}
					if (obj.NewNotificationElementID != "EquipmentSetFilters" && this.IsEquipmentSetFiltersHighlighted)
					{
						this.IsEquipmentSetFiltersHighlighted = false;
					}
					if (obj.NewNotificationElementID != "InventoryOtherBannerItems" && this.IsBannerItemsHighlightApplied)
					{
						this.SetBannerItemsHighlightState(false);
						this.IsEquipmentSetFiltersHighlighted = false;
					}
				}
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (!string.IsNullOrEmpty(this._latestTutorialElementID))
				{
					if (!this._isFoodTransferButtonHighlightApplied && this._latestTutorialElementID == "TransferButtonOnlyFood")
					{
						this.SetFoodTransferButtonHighlightState(true);
						this._isFoodTransferButtonHighlightApplied = true;
					}
					if (!this.IsMicsFilterHighlightEnabled && this._latestTutorialElementID == "InventoryMicsFilter")
					{
						this.IsMicsFilterHighlightEnabled = true;
					}
					if (!this.IsEquipmentSetFiltersHighlighted && this._latestTutorialElementID == "EquipmentSetFilters")
					{
						this.IsEquipmentSetFiltersHighlighted = true;
					}
					if (!this.IsBannerItemsHighlightApplied && this._latestTutorialElementID == "InventoryOtherBannerItems")
					{
						this.IsBannerItemsHighlightApplied = true;
						this.ExecuteFilterMisc();
						this.SetBannerItemsHighlightState(true);
						return;
					}
				}
				else
				{
					if (this._isFoodTransferButtonHighlightApplied)
					{
						this.SetFoodTransferButtonHighlightState(false);
						this._isFoodTransferButtonHighlightApplied = false;
					}
					if (this.IsMicsFilterHighlightEnabled)
					{
						this.IsMicsFilterHighlightEnabled = false;
					}
					if (this.IsEquipmentSetFiltersHighlighted)
					{
						this.IsEquipmentSetFiltersHighlighted = false;
					}
					if (this.IsBannerItemsHighlightApplied)
					{
						this.SetBannerItemsHighlightState(false);
						this.IsBannerItemsHighlightApplied = false;
					}
				}
			}
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0003C7A4 File Offset: 0x0003A9A4
		private void SetFoodTransferButtonHighlightState(bool state)
		{
			for (int i = 0; i < this.LeftItemListVM.Count; i++)
			{
				SPItemVM spitemVM = this.LeftItemListVM[i];
				if (spitemVM.ItemRosterElement.EquipmentElement.Item.IsFood)
				{
					spitemVM.IsTransferButtonHighlighted = state;
				}
			}
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0003C7F8 File Offset: 0x0003A9F8
		private void SetBannerItemsHighlightState(bool state)
		{
			for (int i = 0; i < this.LeftItemListVM.Count; i++)
			{
				SPItemVM spitemVM = this.LeftItemListVM[i];
				if (spitemVM.ItemRosterElement.EquipmentElement.Item.IsBannerItem)
				{
					spitemVM.IsItemHighlightEnabled = state;
				}
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x0003C849 File Offset: 0x0003AA49
		// (set) Token: 0x06000D87 RID: 3463 RVA: 0x0003C851 File Offset: 0x0003AA51
		[DataSourceProperty]
		public HintViewModel ResetHint
		{
			get
			{
				return this._resetHint;
			}
			set
			{
				if (value != this._resetHint)
				{
					this._resetHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResetHint");
				}
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x0003C86F File Offset: 0x0003AA6F
		// (set) Token: 0x06000D89 RID: 3465 RVA: 0x0003C877 File Offset: 0x0003AA77
		[DataSourceProperty]
		public string LeftInventoryLabel
		{
			get
			{
				return this._leftInventoryLabel;
			}
			set
			{
				if (value != this._leftInventoryLabel)
				{
					this._leftInventoryLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "LeftInventoryLabel");
				}
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0003C89A File Offset: 0x0003AA9A
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x0003C8A2 File Offset: 0x0003AAA2
		[DataSourceProperty]
		public string RightInventoryLabel
		{
			get
			{
				return this._rightInventoryLabel;
			}
			set
			{
				if (value != this._rightInventoryLabel)
				{
					this._rightInventoryLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "RightInventoryLabel");
				}
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x0003C8C5 File Offset: 0x0003AAC5
		// (set) Token: 0x06000D8D RID: 3469 RVA: 0x0003C8CD File Offset: 0x0003AACD
		[DataSourceProperty]
		public string DoneLbl
		{
			get
			{
				return this._doneLbl;
			}
			set
			{
				if (value != this._doneLbl)
				{
					this._doneLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneLbl");
				}
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0003C8F0 File Offset: 0x0003AAF0
		// (set) Token: 0x06000D8F RID: 3471 RVA: 0x0003C8F8 File Offset: 0x0003AAF8
		[DataSourceProperty]
		public bool IsDoneDisabled
		{
			get
			{
				return this._isDoneDisabled;
			}
			set
			{
				if (value != this._isDoneDisabled)
				{
					this._isDoneDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDoneDisabled");
				}
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x0003C916 File Offset: 0x0003AB16
		// (set) Token: 0x06000D91 RID: 3473 RVA: 0x0003C91E File Offset: 0x0003AB1E
		[DataSourceProperty]
		public bool OtherSideHasCapacity
		{
			get
			{
				return this._otherSideHasCapacity;
			}
			set
			{
				if (value != this._otherSideHasCapacity)
				{
					this._otherSideHasCapacity = value;
					base.OnPropertyChangedWithValue(value, "OtherSideHasCapacity");
				}
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x0003C93C File Offset: 0x0003AB3C
		// (set) Token: 0x06000D93 RID: 3475 RVA: 0x0003C944 File Offset: 0x0003AB44
		[DataSourceProperty]
		public bool IsSearchAvailable
		{
			get
			{
				return this._isSearchAvailable;
			}
			set
			{
				if (value != this._isSearchAvailable)
				{
					if (!value)
					{
						this.LeftSearchText = string.Empty;
						this.RightSearchText = string.Empty;
					}
					this._isSearchAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsSearchAvailable");
				}
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0003C97B File Offset: 0x0003AB7B
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x0003C983 File Offset: 0x0003AB83
		[DataSourceProperty]
		public bool IsOtherInventoryGoldRelevant
		{
			get
			{
				return this._isOtherInventoryGoldRelevant;
			}
			set
			{
				if (value != this._isOtherInventoryGoldRelevant)
				{
					this._isOtherInventoryGoldRelevant = value;
					base.OnPropertyChangedWithValue(value, "IsOtherInventoryGoldRelevant");
				}
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0003C9A1 File Offset: 0x0003ABA1
		// (set) Token: 0x06000D97 RID: 3479 RVA: 0x0003C9A9 File Offset: 0x0003ABA9
		[DataSourceProperty]
		public string CancelLbl
		{
			get
			{
				return this._cancelLbl;
			}
			set
			{
				if (value != this._cancelLbl)
				{
					this._cancelLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelLbl");
				}
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x0003C9CC File Offset: 0x0003ABCC
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x0003C9D4 File Offset: 0x0003ABD4
		[DataSourceProperty]
		public string ResetLbl
		{
			get
			{
				return this._resetLbl;
			}
			set
			{
				if (value != this._resetLbl)
				{
					this._resetLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ResetLbl");
				}
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x0003C9F7 File Offset: 0x0003ABF7
		// (set) Token: 0x06000D9B RID: 3483 RVA: 0x0003C9FF File Offset: 0x0003ABFF
		[DataSourceProperty]
		public string TypeText
		{
			get
			{
				return this._typeText;
			}
			set
			{
				if (value != this._typeText)
				{
					this._typeText = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeText");
				}
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x0003CA22 File Offset: 0x0003AC22
		// (set) Token: 0x06000D9D RID: 3485 RVA: 0x0003CA2A File Offset: 0x0003AC2A
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x0003CA4D File Offset: 0x0003AC4D
		// (set) Token: 0x06000D9F RID: 3487 RVA: 0x0003CA55 File Offset: 0x0003AC55
		[DataSourceProperty]
		public string QuantityText
		{
			get
			{
				return this._quantityText;
			}
			set
			{
				if (value != this._quantityText)
				{
					this._quantityText = value;
					base.OnPropertyChangedWithValue<string>(value, "QuantityText");
				}
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x0003CA78 File Offset: 0x0003AC78
		// (set) Token: 0x06000DA1 RID: 3489 RVA: 0x0003CA80 File Offset: 0x0003AC80
		[DataSourceProperty]
		public string CostText
		{
			get
			{
				return this._costText;
			}
			set
			{
				if (value != this._costText)
				{
					this._costText = value;
					base.OnPropertyChangedWithValue<string>(value, "CostText");
				}
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x0003CAA3 File Offset: 0x0003ACA3
		// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x0003CAAB File Offset: 0x0003ACAB
		[DataSourceProperty]
		public string SearchPlaceholderText
		{
			get
			{
				return this._searchPlaceholderText;
			}
			set
			{
				if (value != this._searchPlaceholderText)
				{
					this._searchPlaceholderText = value;
					base.OnPropertyChangedWithValue<string>(value, "SearchPlaceholderText");
				}
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0003CACE File Offset: 0x0003ACCE
		// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x0003CAD6 File Offset: 0x0003ACD6
		[DataSourceProperty]
		public BasicTooltipViewModel ProductionTooltip
		{
			get
			{
				return this._productionTooltip;
			}
			set
			{
				if (value != this._productionTooltip)
				{
					this._productionTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ProductionTooltip");
				}
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x0003CAF4 File Offset: 0x0003ACF4
		// (set) Token: 0x06000DA7 RID: 3495 RVA: 0x0003CAFC File Offset: 0x0003ACFC
		[DataSourceProperty]
		public BasicTooltipViewModel InventoryCapacityHint
		{
			get
			{
				return this._inventoryCapacityHint;
			}
			set
			{
				if (value != this._inventoryCapacityHint)
				{
					this._inventoryCapacityHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "InventoryCapacityHint");
				}
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x0003CB1A File Offset: 0x0003AD1A
		// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x0003CB22 File Offset: 0x0003AD22
		[DataSourceProperty]
		public BasicTooltipViewModel LandCapacityHint
		{
			get
			{
				return this._landCapacityHint;
			}
			set
			{
				if (value != this._landCapacityHint)
				{
					this._landCapacityHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "LandCapacityHint");
				}
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0003CB40 File Offset: 0x0003AD40
		// (set) Token: 0x06000DAB RID: 3499 RVA: 0x0003CB48 File Offset: 0x0003AD48
		[DataSourceProperty]
		public BasicTooltipViewModel SeaCapacityHint
		{
			get
			{
				return this._seaCapacityHint;
			}
			set
			{
				if (value != this._seaCapacityHint)
				{
					this._seaCapacityHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "SeaCapacityHint");
				}
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0003CB66 File Offset: 0x0003AD66
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x0003CB6E File Offset: 0x0003AD6E
		[DataSourceProperty]
		public BasicTooltipViewModel TotalWeightCarriedHint
		{
			get
			{
				return this._totalWeightCarriedHint;
			}
			set
			{
				if (value != this._totalWeightCarriedHint)
				{
					this._totalWeightCarriedHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "TotalWeightCarriedHint");
				}
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x0003CB8C File Offset: 0x0003AD8C
		// (set) Token: 0x06000DAF RID: 3503 RVA: 0x0003CB94 File Offset: 0x0003AD94
		[DataSourceProperty]
		public BasicTooltipViewModel LandWeightHint
		{
			get
			{
				return this._landWeightHint;
			}
			set
			{
				if (value != this._landWeightHint)
				{
					this._landWeightHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "LandWeightHint");
				}
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x0003CBB2 File Offset: 0x0003ADB2
		// (set) Token: 0x06000DB1 RID: 3505 RVA: 0x0003CBBA File Offset: 0x0003ADBA
		[DataSourceProperty]
		public BasicTooltipViewModel SeaWeightHint
		{
			get
			{
				return this._seaWeightHint;
			}
			set
			{
				if (value != this._seaWeightHint)
				{
					this._seaWeightHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "SeaWeightHint");
				}
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0003CBD8 File Offset: 0x0003ADD8
		// (set) Token: 0x06000DB3 RID: 3507 RVA: 0x0003CBE0 File Offset: 0x0003ADE0
		[DataSourceProperty]
		public BasicTooltipViewModel CurrentCharacterSkillsTooltip
		{
			get
			{
				return this._currentCharacterSkillsTooltip;
			}
			set
			{
				if (value != this._currentCharacterSkillsTooltip)
				{
					this._currentCharacterSkillsTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CurrentCharacterSkillsTooltip");
				}
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x0003CBFE File Offset: 0x0003ADFE
		// (set) Token: 0x06000DB5 RID: 3509 RVA: 0x0003CC06 File Offset: 0x0003AE06
		[DataSourceProperty]
		public HintViewModel NoSaddleHint
		{
			get
			{
				return this._noSaddleHint;
			}
			set
			{
				if (value != this._noSaddleHint)
				{
					this._noSaddleHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "NoSaddleHint");
				}
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x0003CC24 File Offset: 0x0003AE24
		// (set) Token: 0x06000DB7 RID: 3511 RVA: 0x0003CC2C File Offset: 0x0003AE2C
		[DataSourceProperty]
		public HintViewModel DonationLblHint
		{
			get
			{
				return this._donationLblHint;
			}
			set
			{
				if (value != this._donationLblHint)
				{
					this._donationLblHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DonationLblHint");
				}
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x0003CC4A File Offset: 0x0003AE4A
		// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x0003CC52 File Offset: 0x0003AE52
		[DataSourceProperty]
		public HintViewModel ArmArmorHint
		{
			get
			{
				return this._armArmorHint;
			}
			set
			{
				if (value != this._armArmorHint)
				{
					this._armArmorHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ArmArmorHint");
				}
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0003CC70 File Offset: 0x0003AE70
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x0003CC78 File Offset: 0x0003AE78
		[DataSourceProperty]
		public HintViewModel BodyArmorHint
		{
			get
			{
				return this._bodyArmorHint;
			}
			set
			{
				if (value != this._bodyArmorHint)
				{
					this._bodyArmorHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BodyArmorHint");
				}
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x0003CC96 File Offset: 0x0003AE96
		// (set) Token: 0x06000DBD RID: 3517 RVA: 0x0003CC9E File Offset: 0x0003AE9E
		[DataSourceProperty]
		public HintViewModel HeadArmorHint
		{
			get
			{
				return this._headArmorHint;
			}
			set
			{
				if (value != this._headArmorHint)
				{
					this._headArmorHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HeadArmorHint");
				}
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x0003CCBC File Offset: 0x0003AEBC
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x0003CCC4 File Offset: 0x0003AEC4
		[DataSourceProperty]
		public HintViewModel LegArmorHint
		{
			get
			{
				return this._legArmorHint;
			}
			set
			{
				if (value != this._legArmorHint)
				{
					this._legArmorHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LegArmorHint");
				}
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0003CCE2 File Offset: 0x0003AEE2
		// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x0003CCEA File Offset: 0x0003AEEA
		[DataSourceProperty]
		public HintViewModel HorseArmorHint
		{
			get
			{
				return this._horseArmorHint;
			}
			set
			{
				if (value != this._horseArmorHint)
				{
					this._horseArmorHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HorseArmorHint");
				}
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0003CD08 File Offset: 0x0003AF08
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x0003CD10 File Offset: 0x0003AF10
		[DataSourceProperty]
		public HintViewModel FilterAllHint
		{
			get
			{
				return this._filterAllHint;
			}
			set
			{
				if (value != this._filterAllHint)
				{
					this._filterAllHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FilterAllHint");
				}
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x0003CD2E File Offset: 0x0003AF2E
		// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x0003CD36 File Offset: 0x0003AF36
		[DataSourceProperty]
		public HintViewModel FilterWeaponHint
		{
			get
			{
				return this._filterWeaponHint;
			}
			set
			{
				if (value != this._filterWeaponHint)
				{
					this._filterWeaponHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FilterWeaponHint");
				}
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x0003CD54 File Offset: 0x0003AF54
		// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x0003CD5C File Offset: 0x0003AF5C
		[DataSourceProperty]
		public HintViewModel FilterArmorHint
		{
			get
			{
				return this._filterArmorHint;
			}
			set
			{
				if (value != this._filterArmorHint)
				{
					this._filterArmorHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FilterArmorHint");
				}
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x0003CD7A File Offset: 0x0003AF7A
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x0003CD82 File Offset: 0x0003AF82
		[DataSourceProperty]
		public HintViewModel FilterShieldAndRangedHint
		{
			get
			{
				return this._filterShieldAndRangedHint;
			}
			set
			{
				if (value != this._filterShieldAndRangedHint)
				{
					this._filterShieldAndRangedHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FilterShieldAndRangedHint");
				}
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x0003CDA0 File Offset: 0x0003AFA0
		// (set) Token: 0x06000DCB RID: 3531 RVA: 0x0003CDA8 File Offset: 0x0003AFA8
		[DataSourceProperty]
		public HintViewModel FilterMountAndHarnessHint
		{
			get
			{
				return this._filterMountAndHarnessHint;
			}
			set
			{
				if (value != this._filterMountAndHarnessHint)
				{
					this._filterMountAndHarnessHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FilterMountAndHarnessHint");
				}
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06000DCC RID: 3532 RVA: 0x0003CDC6 File Offset: 0x0003AFC6
		// (set) Token: 0x06000DCD RID: 3533 RVA: 0x0003CDCE File Offset: 0x0003AFCE
		[DataSourceProperty]
		public HintViewModel FilterMiscHint
		{
			get
			{
				return this._filterMiscHint;
			}
			set
			{
				if (value != this._filterMiscHint)
				{
					this._filterMiscHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FilterMiscHint");
				}
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x0003CDEC File Offset: 0x0003AFEC
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x0003CDF4 File Offset: 0x0003AFF4
		[DataSourceProperty]
		public HintViewModel StealthOutfitHint
		{
			get
			{
				return this._stealthOutfitHint;
			}
			set
			{
				if (value != this._stealthOutfitHint)
				{
					this._stealthOutfitHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "StealthOutfitHint");
				}
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x0003CE12 File Offset: 0x0003B012
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x0003CE1A File Offset: 0x0003B01A
		[DataSourceProperty]
		public HintViewModel CivilianOutfitHint
		{
			get
			{
				return this._civilianOutfitHint;
			}
			set
			{
				if (value != this._civilianOutfitHint)
				{
					this._civilianOutfitHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CivilianOutfitHint");
				}
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x0003CE38 File Offset: 0x0003B038
		// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x0003CE40 File Offset: 0x0003B040
		[DataSourceProperty]
		public HintViewModel BattleOutfitHint
		{
			get
			{
				return this._battleOutfitHint;
			}
			set
			{
				if (value != this._battleOutfitHint)
				{
					this._battleOutfitHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BattleOutfitHint");
				}
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x0003CE5E File Offset: 0x0003B05E
		// (set) Token: 0x06000DD5 RID: 3541 RVA: 0x0003CE66 File Offset: 0x0003B066
		[DataSourceProperty]
		public HintViewModel EquipmentHelmSlotHint
		{
			get
			{
				return this._equipmentHelmSlotHint;
			}
			set
			{
				if (value != this._equipmentHelmSlotHint)
				{
					this._equipmentHelmSlotHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EquipmentHelmSlotHint");
				}
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x0003CE84 File Offset: 0x0003B084
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x0003CE8C File Offset: 0x0003B08C
		[DataSourceProperty]
		public HintViewModel EquipmentArmorSlotHint
		{
			get
			{
				return this._equipmentArmorSlotHint;
			}
			set
			{
				if (value != this._equipmentArmorSlotHint)
				{
					this._equipmentArmorSlotHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EquipmentArmorSlotHint");
				}
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x0003CEAA File Offset: 0x0003B0AA
		// (set) Token: 0x06000DD9 RID: 3545 RVA: 0x0003CEB2 File Offset: 0x0003B0B2
		[DataSourceProperty]
		public HintViewModel EquipmentBootSlotHint
		{
			get
			{
				return this._equipmentBootSlotHint;
			}
			set
			{
				if (value != this._equipmentBootSlotHint)
				{
					this._equipmentBootSlotHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EquipmentBootSlotHint");
				}
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x0003CED0 File Offset: 0x0003B0D0
		// (set) Token: 0x06000DDB RID: 3547 RVA: 0x0003CED8 File Offset: 0x0003B0D8
		[DataSourceProperty]
		public HintViewModel EquipmentCloakSlotHint
		{
			get
			{
				return this._equipmentCloakSlotHint;
			}
			set
			{
				if (value != this._equipmentCloakSlotHint)
				{
					this._equipmentCloakSlotHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EquipmentCloakSlotHint");
				}
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x0003CEF6 File Offset: 0x0003B0F6
		// (set) Token: 0x06000DDD RID: 3549 RVA: 0x0003CEFE File Offset: 0x0003B0FE
		[DataSourceProperty]
		public HintViewModel EquipmentGloveSlotHint
		{
			get
			{
				return this._equipmentGloveSlotHint;
			}
			set
			{
				if (value != this._equipmentGloveSlotHint)
				{
					this._equipmentGloveSlotHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EquipmentGloveSlotHint");
				}
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06000DDE RID: 3550 RVA: 0x0003CF1C File Offset: 0x0003B11C
		// (set) Token: 0x06000DDF RID: 3551 RVA: 0x0003CF24 File Offset: 0x0003B124
		[DataSourceProperty]
		public HintViewModel EquipmentHarnessSlotHint
		{
			get
			{
				return this._equipmentHarnessSlotHint;
			}
			set
			{
				if (value != this._equipmentHarnessSlotHint)
				{
					this._equipmentHarnessSlotHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EquipmentHarnessSlotHint");
				}
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x0003CF42 File Offset: 0x0003B142
		// (set) Token: 0x06000DE1 RID: 3553 RVA: 0x0003CF4A File Offset: 0x0003B14A
		[DataSourceProperty]
		public HintViewModel EquipmentMountSlotHint
		{
			get
			{
				return this._equipmentMountSlotHint;
			}
			set
			{
				if (value != this._equipmentMountSlotHint)
				{
					this._equipmentMountSlotHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EquipmentMountSlotHint");
				}
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x0003CF68 File Offset: 0x0003B168
		// (set) Token: 0x06000DE3 RID: 3555 RVA: 0x0003CF70 File Offset: 0x0003B170
		[DataSourceProperty]
		public HintViewModel EquipmentWeaponSlotHint
		{
			get
			{
				return this._equipmentWeaponSlotHint;
			}
			set
			{
				if (value != this._equipmentWeaponSlotHint)
				{
					this._equipmentWeaponSlotHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EquipmentWeaponSlotHint");
				}
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x0003CF8E File Offset: 0x0003B18E
		// (set) Token: 0x06000DE5 RID: 3557 RVA: 0x0003CF96 File Offset: 0x0003B196
		[DataSourceProperty]
		public HintViewModel EquipmentBannerSlotHint
		{
			get
			{
				return this._equipmentBannerSlotHint;
			}
			set
			{
				if (value != this._equipmentBannerSlotHint)
				{
					this._equipmentBannerSlotHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EquipmentBannerSlotHint");
				}
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x0003CFB4 File Offset: 0x0003B1B4
		// (set) Token: 0x06000DE7 RID: 3559 RVA: 0x0003CFBC File Offset: 0x0003B1BC
		[DataSourceProperty]
		public BasicTooltipViewModel BuyAllHint
		{
			get
			{
				return this._buyAllHint;
			}
			set
			{
				if (value != this._buyAllHint)
				{
					this._buyAllHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "BuyAllHint");
				}
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x0003CFDA File Offset: 0x0003B1DA
		// (set) Token: 0x06000DE9 RID: 3561 RVA: 0x0003CFE2 File Offset: 0x0003B1E2
		[DataSourceProperty]
		public BasicTooltipViewModel SellAllHint
		{
			get
			{
				return this._sellAllHint;
			}
			set
			{
				if (value != this._sellAllHint)
				{
					this._sellAllHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "SellAllHint");
				}
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x0003D000 File Offset: 0x0003B200
		// (set) Token: 0x06000DEB RID: 3563 RVA: 0x0003D008 File Offset: 0x0003B208
		[DataSourceProperty]
		public BasicTooltipViewModel PreviousCharacterHint
		{
			get
			{
				return this._previousCharacterHint;
			}
			set
			{
				if (value != this._previousCharacterHint)
				{
					this._previousCharacterHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "PreviousCharacterHint");
				}
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x0003D026 File Offset: 0x0003B226
		// (set) Token: 0x06000DED RID: 3565 RVA: 0x0003D02E File Offset: 0x0003B22E
		[DataSourceProperty]
		public BasicTooltipViewModel NextCharacterHint
		{
			get
			{
				return this._nextCharacterHint;
			}
			set
			{
				if (value != this._nextCharacterHint)
				{
					this._nextCharacterHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "NextCharacterHint");
				}
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x0003D04C File Offset: 0x0003B24C
		// (set) Token: 0x06000DEF RID: 3567 RVA: 0x0003D054 File Offset: 0x0003B254
		[DataSourceProperty]
		public HintViewModel WeightHint
		{
			get
			{
				return this._weightHint;
			}
			set
			{
				if (value != this._weightHint)
				{
					this._weightHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "WeightHint");
				}
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06000DF0 RID: 3568 RVA: 0x0003D072 File Offset: 0x0003B272
		// (set) Token: 0x06000DF1 RID: 3569 RVA: 0x0003D07A File Offset: 0x0003B27A
		[DataSourceProperty]
		public HintViewModel PreviewHint
		{
			get
			{
				return this._previewHint;
			}
			set
			{
				if (value != this._previewHint)
				{
					this._previewHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PreviewHint");
				}
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x0003D098 File Offset: 0x0003B298
		// (set) Token: 0x06000DF3 RID: 3571 RVA: 0x0003D0A0 File Offset: 0x0003B2A0
		[DataSourceProperty]
		public HintViewModel EquipHint
		{
			get
			{
				return this._equipHint;
			}
			set
			{
				if (value != this._equipHint)
				{
					this._equipHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EquipHint");
				}
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x0003D0BE File Offset: 0x0003B2BE
		// (set) Token: 0x06000DF5 RID: 3573 RVA: 0x0003D0C6 File Offset: 0x0003B2C6
		[DataSourceProperty]
		public HintViewModel UnequipHint
		{
			get
			{
				return this._unequipHint;
			}
			set
			{
				if (value != this._unequipHint)
				{
					this._unequipHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "UnequipHint");
				}
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0003D0E4 File Offset: 0x0003B2E4
		// (set) Token: 0x06000DF7 RID: 3575 RVA: 0x0003D0EC File Offset: 0x0003B2EC
		[DataSourceProperty]
		public HintViewModel SellHint
		{
			get
			{
				return this._sellHint;
			}
			set
			{
				if (value != this._sellHint)
				{
					this._sellHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SellHint");
				}
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x0003D10A File Offset: 0x0003B30A
		// (set) Token: 0x06000DF9 RID: 3577 RVA: 0x0003D112 File Offset: 0x0003B312
		[DataSourceProperty]
		public HintViewModel PlayerSideCapacityExceededHint
		{
			get
			{
				return this._playerSideCapacityExceededHint;
			}
			set
			{
				if (value != this._playerSideCapacityExceededHint)
				{
					this._playerSideCapacityExceededHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PlayerSideCapacityExceededHint");
				}
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x0003D130 File Offset: 0x0003B330
		// (set) Token: 0x06000DFB RID: 3579 RVA: 0x0003D138 File Offset: 0x0003B338
		[DataSourceProperty]
		public HintViewModel MainPartyLandCapacityExceededHint
		{
			get
			{
				return this._mainPartyLandCapacityExceededHint;
			}
			set
			{
				if (value != this._mainPartyLandCapacityExceededHint)
				{
					this._mainPartyLandCapacityExceededHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "MainPartyLandCapacityExceededHint");
				}
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0003D156 File Offset: 0x0003B356
		// (set) Token: 0x06000DFD RID: 3581 RVA: 0x0003D15E File Offset: 0x0003B35E
		[DataSourceProperty]
		public HintViewModel MainPartySeaCapacityExceededHint
		{
			get
			{
				return this._mainPartySeaCapacityExceededHint;
			}
			set
			{
				if (value != this._mainPartySeaCapacityExceededHint)
				{
					this._mainPartySeaCapacityExceededHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "MainPartySeaCapacityExceededHint");
				}
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x0003D17C File Offset: 0x0003B37C
		// (set) Token: 0x06000DFF RID: 3583 RVA: 0x0003D184 File Offset: 0x0003B384
		[DataSourceProperty]
		public HintViewModel OtherSideCapacityExceededHint
		{
			get
			{
				return this._otherSideCapacityExceededHint;
			}
			set
			{
				if (value != this._otherSideCapacityExceededHint)
				{
					this._otherSideCapacityExceededHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "OtherSideCapacityExceededHint");
				}
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06000E00 RID: 3584 RVA: 0x0003D1A2 File Offset: 0x0003B3A2
		// (set) Token: 0x06000E01 RID: 3585 RVA: 0x0003D1AA File Offset: 0x0003B3AA
		[DataSourceProperty]
		public SelectorVM<InventoryCharacterSelectorItemVM> CharacterList
		{
			get
			{
				return this._characterList;
			}
			set
			{
				if (value != this._characterList)
				{
					this._characterList = value;
					base.OnPropertyChangedWithValue<SelectorVM<InventoryCharacterSelectorItemVM>>(value, "CharacterList");
				}
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x0003D1C8 File Offset: 0x0003B3C8
		// (set) Token: 0x06000E03 RID: 3587 RVA: 0x0003D1D0 File Offset: 0x0003B3D0
		[DataSourceProperty]
		public SPInventorySortControllerVM PlayerInventorySortController
		{
			get
			{
				return this._playerInventorySortController;
			}
			set
			{
				if (value != this._playerInventorySortController)
				{
					this._playerInventorySortController = value;
					base.OnPropertyChangedWithValue<SPInventorySortControllerVM>(value, "PlayerInventorySortController");
				}
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x0003D1EE File Offset: 0x0003B3EE
		// (set) Token: 0x06000E05 RID: 3589 RVA: 0x0003D1F6 File Offset: 0x0003B3F6
		[DataSourceProperty]
		public SPInventorySortControllerVM OtherInventorySortController
		{
			get
			{
				return this._otherInventorySortController;
			}
			set
			{
				if (value != this._otherInventorySortController)
				{
					this._otherInventorySortController = value;
					base.OnPropertyChangedWithValue<SPInventorySortControllerVM>(value, "OtherInventorySortController");
				}
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06000E06 RID: 3590 RVA: 0x0003D214 File Offset: 0x0003B414
		// (set) Token: 0x06000E07 RID: 3591 RVA: 0x0003D21C File Offset: 0x0003B41C
		[DataSourceProperty]
		public ItemPreviewVM ItemPreview
		{
			get
			{
				return this._itemPreview;
			}
			set
			{
				if (value != this._itemPreview)
				{
					this._itemPreview = value;
					base.OnPropertyChangedWithValue<ItemPreviewVM>(value, "ItemPreview");
				}
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06000E08 RID: 3592 RVA: 0x0003D23A File Offset: 0x0003B43A
		// (set) Token: 0x06000E09 RID: 3593 RVA: 0x0003D242 File Offset: 0x0003B442
		[DataSourceProperty]
		public int ActiveFilterIndex
		{
			get
			{
				return (int)this._activeFilterIndex;
			}
			set
			{
				if (value != (int)this._activeFilterIndex)
				{
					this._activeFilterIndex = (SPInventoryVM.Filters)value;
					base.OnPropertyChangedWithValue(value, "ActiveFilterIndex");
				}
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x0003D260 File Offset: 0x0003B460
		// (set) Token: 0x06000E0B RID: 3595 RVA: 0x0003D268 File Offset: 0x0003B468
		[DataSourceProperty]
		public bool CompanionExists
		{
			get
			{
				return this._companionExists;
			}
			set
			{
				if (value != this._companionExists)
				{
					this._companionExists = value;
					base.OnPropertyChangedWithValue(value, "CompanionExists");
				}
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0003D286 File Offset: 0x0003B486
		// (set) Token: 0x06000E0D RID: 3597 RVA: 0x0003D28E File Offset: 0x0003B48E
		[DataSourceProperty]
		public bool IsTradingWithSettlement
		{
			get
			{
				return this._isTradingWithSettlement;
			}
			set
			{
				if (value != this._isTradingWithSettlement)
				{
					this._isTradingWithSettlement = value;
					base.OnPropertyChangedWithValue(value, "IsTradingWithSettlement");
				}
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x0003D2AC File Offset: 0x0003B4AC
		// (set) Token: 0x06000E0F RID: 3599 RVA: 0x0003D2B4 File Offset: 0x0003B4B4
		[DataSourceProperty]
		public int EquipmentMode
		{
			get
			{
				return (int)this._equipmentMode;
			}
			set
			{
				if (value != (int)this._equipmentMode)
				{
					this._equipmentMode = (SPInventoryVM.EquipmentModes)value;
					base.OnPropertyChangedWithValue(value, "EquipmentMode");
					this.UpdateRightCharacter();
					this.OnEquipmentModeChanged();
					this.RefreshInformationValues();
					Game.Current.EventManager.TriggerEvent<InventoryEquipmentTypeChangedEvent>(new InventoryEquipmentTypeChangedEvent(this._equipmentMode == SPInventoryVM.EquipmentModes.Battle));
				}
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0003D30C File Offset: 0x0003B50C
		// (set) Token: 0x06000E11 RID: 3601 RVA: 0x0003D314 File Offset: 0x0003B514
		[DataSourceProperty]
		public bool IsMicsFilterHighlightEnabled
		{
			get
			{
				return this._isMicsFilterHighlightEnabled;
			}
			set
			{
				if (value != this._isMicsFilterHighlightEnabled)
				{
					this._isMicsFilterHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsMicsFilterHighlightEnabled");
				}
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x0003D332 File Offset: 0x0003B532
		// (set) Token: 0x06000E13 RID: 3603 RVA: 0x0003D33A File Offset: 0x0003B53A
		[DataSourceProperty]
		public bool IsEquipmentSetFiltersHighlighted
		{
			get
			{
				return this._isCivilianFilterHighlightEnabled;
			}
			set
			{
				if (value != this._isCivilianFilterHighlightEnabled)
				{
					this._isCivilianFilterHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEquipmentSetFiltersHighlighted");
				}
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06000E14 RID: 3604 RVA: 0x0003D358 File Offset: 0x0003B558
		// (set) Token: 0x06000E15 RID: 3605 RVA: 0x0003D360 File Offset: 0x0003B560
		[DataSourceProperty]
		public ItemMenuVM ItemMenu
		{
			get
			{
				return this._itemMenu;
			}
			set
			{
				if (value != this._itemMenu)
				{
					this._itemMenu = value;
					base.OnPropertyChangedWithValue<ItemMenuVM>(value, "ItemMenu");
				}
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x0003D37E File Offset: 0x0003B57E
		// (set) Token: 0x06000E17 RID: 3607 RVA: 0x0003D386 File Offset: 0x0003B586
		[DataSourceProperty]
		public string PlayerSideCapacityExceededText
		{
			get
			{
				return this._playerSideCapacityExceededText;
			}
			set
			{
				if (value != this._playerSideCapacityExceededText)
				{
					this._playerSideCapacityExceededText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayerSideCapacityExceededText");
				}
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0003D3A9 File Offset: 0x0003B5A9
		// (set) Token: 0x06000E19 RID: 3609 RVA: 0x0003D3B1 File Offset: 0x0003B5B1
		[DataSourceProperty]
		public string MainPartyLandCapacityExceededText
		{
			get
			{
				return this._mainPartyLandCapacityExceededText;
			}
			set
			{
				if (value != this._mainPartyLandCapacityExceededText)
				{
					this._mainPartyLandCapacityExceededText = value;
					base.OnPropertyChangedWithValue<string>(value, "MainPartyLandCapacityExceededText");
				}
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x0003D3D4 File Offset: 0x0003B5D4
		// (set) Token: 0x06000E1B RID: 3611 RVA: 0x0003D3DC File Offset: 0x0003B5DC
		[DataSourceProperty]
		public string MainPartySeaCapacityExceededText
		{
			get
			{
				return this._mainPartySeaCapacityExceededText;
			}
			set
			{
				if (value != this._mainPartySeaCapacityExceededText)
				{
					this._mainPartySeaCapacityExceededText = value;
					base.OnPropertyChangedWithValue<string>(value, "MainPartySeaCapacityExceededText");
				}
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x0003D3FF File Offset: 0x0003B5FF
		// (set) Token: 0x06000E1D RID: 3613 RVA: 0x0003D407 File Offset: 0x0003B607
		[DataSourceProperty]
		public string SeparatorText
		{
			get
			{
				return this._separatorText;
			}
			set
			{
				if (value != this._separatorText)
				{
					this._separatorText = value;
					base.OnPropertyChangedWithValue<string>(value, "SeparatorText");
				}
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x0003D42A File Offset: 0x0003B62A
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x0003D432 File Offset: 0x0003B632
		[DataSourceProperty]
		public string OtherSideCapacityExceededText
		{
			get
			{
				return this._otherSideCapacityExceededText;
			}
			set
			{
				if (value != this._otherSideCapacityExceededText)
				{
					this._otherSideCapacityExceededText = value;
					base.OnPropertyChangedWithValue<string>(value, "OtherSideCapacityExceededText");
				}
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x0003D455 File Offset: 0x0003B655
		// (set) Token: 0x06000E21 RID: 3617 RVA: 0x0003D45D File Offset: 0x0003B65D
		[DataSourceProperty]
		public string LeftSearchText
		{
			get
			{
				return this._leftSearchText;
			}
			set
			{
				if (value != this._leftSearchText)
				{
					this._leftSearchText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeftSearchText");
					this.OnSearchTextChanged(true);
				}
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x0003D487 File Offset: 0x0003B687
		// (set) Token: 0x06000E23 RID: 3619 RVA: 0x0003D48F File Offset: 0x0003B68F
		[DataSourceProperty]
		public string RightSearchText
		{
			get
			{
				return this._rightSearchText;
			}
			set
			{
				if (value != this._rightSearchText)
				{
					this._rightSearchText = value;
					base.OnPropertyChangedWithValue<string>(value, "RightSearchText");
					this.OnSearchTextChanged(false);
				}
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x0003D4B9 File Offset: 0x0003B6B9
		// (set) Token: 0x06000E25 RID: 3621 RVA: 0x0003D4C1 File Offset: 0x0003B6C1
		[DataSourceProperty]
		public bool HasGainedExperience
		{
			get
			{
				return this._hasGainedExperience;
			}
			set
			{
				if (value != this._hasGainedExperience)
				{
					this._hasGainedExperience = value;
					base.OnPropertyChangedWithValue(value, "HasGainedExperience");
				}
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0003D4DF File Offset: 0x0003B6DF
		// (set) Token: 0x06000E27 RID: 3623 RVA: 0x0003D4E7 File Offset: 0x0003B6E7
		[DataSourceProperty]
		public bool IsDonationXpGainExceedsMax
		{
			get
			{
				return this._isDonationXpGainExceedsMax;
			}
			set
			{
				if (value != this._isDonationXpGainExceedsMax)
				{
					this._isDonationXpGainExceedsMax = value;
					base.OnPropertyChangedWithValue(value, "IsDonationXpGainExceedsMax");
				}
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x0003D505 File Offset: 0x0003B705
		// (set) Token: 0x06000E29 RID: 3625 RVA: 0x0003D50D File Offset: 0x0003B70D
		[DataSourceProperty]
		public bool NoSaddleWarned
		{
			get
			{
				return this._noSaddleWarned;
			}
			set
			{
				if (value != this._noSaddleWarned)
				{
					this._noSaddleWarned = value;
					base.OnPropertyChangedWithValue(value, "NoSaddleWarned");
				}
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x0003D52B File Offset: 0x0003B72B
		// (set) Token: 0x06000E2B RID: 3627 RVA: 0x0003D533 File Offset: 0x0003B733
		[DataSourceProperty]
		public bool ShowMainPartyLandCapacityTexts
		{
			get
			{
				return this._showMainPartyLandCapacityTexts;
			}
			set
			{
				if (value != this._showMainPartyLandCapacityTexts)
				{
					this._showMainPartyLandCapacityTexts = value;
					base.OnPropertyChangedWithValue(value, "ShowMainPartyLandCapacityTexts");
				}
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x0003D551 File Offset: 0x0003B751
		// (set) Token: 0x06000E2D RID: 3629 RVA: 0x0003D559 File Offset: 0x0003B759
		[DataSourceProperty]
		public bool ShowMainPartySeaCapacityTexts
		{
			get
			{
				return this._showMainPartySeaCapacityTexts;
			}
			set
			{
				if (value != this._showMainPartySeaCapacityTexts)
				{
					this._showMainPartySeaCapacityTexts = value;
					base.OnPropertyChangedWithValue(value, "ShowMainPartySeaCapacityTexts");
				}
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0003D577 File Offset: 0x0003B777
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x0003D57F File Offset: 0x0003B77F
		[DataSourceProperty]
		public bool PlayerEquipmentCountWarned
		{
			get
			{
				return this._playerEquipmentCountWarned;
			}
			set
			{
				if (value != this._playerEquipmentCountWarned)
				{
					this._playerEquipmentCountWarned = value;
					base.OnPropertyChangedWithValue(value, "PlayerEquipmentCountWarned");
				}
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0003D59D File Offset: 0x0003B79D
		// (set) Token: 0x06000E31 RID: 3633 RVA: 0x0003D5A5 File Offset: 0x0003B7A5
		[DataSourceProperty]
		public bool IsMainPartyLandCapacityWarned
		{
			get
			{
				return this._isMainPartyLandCapacityWarned;
			}
			set
			{
				if (value != this._isMainPartyLandCapacityWarned)
				{
					this._isMainPartyLandCapacityWarned = value;
					base.OnPropertyChangedWithValue(value, "IsMainPartyLandCapacityWarned");
				}
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x0003D5C3 File Offset: 0x0003B7C3
		// (set) Token: 0x06000E33 RID: 3635 RVA: 0x0003D5CB File Offset: 0x0003B7CB
		[DataSourceProperty]
		public bool IsMainPartySeaCapacityWarned
		{
			get
			{
				return this._isMainPartySeaCapacityWarned;
			}
			set
			{
				if (value != this._isMainPartySeaCapacityWarned)
				{
					this._isMainPartySeaCapacityWarned = value;
					base.OnPropertyChangedWithValue(value, "IsMainPartySeaCapacityWarned");
				}
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x0003D5E9 File Offset: 0x0003B7E9
		// (set) Token: 0x06000E35 RID: 3637 RVA: 0x0003D5F1 File Offset: 0x0003B7F1
		[DataSourceProperty]
		public bool ShowMainPartyLandCapacityWarning
		{
			get
			{
				return this._showMainPartyLandCapacityWarning;
			}
			set
			{
				if (value != this._showMainPartyLandCapacityWarning)
				{
					this._showMainPartyLandCapacityWarning = value;
					base.OnPropertyChangedWithValue(value, "ShowMainPartyLandCapacityWarning");
				}
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x0003D60F File Offset: 0x0003B80F
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x0003D617 File Offset: 0x0003B817
		[DataSourceProperty]
		public bool ShowMainPartySeaCapacityWarning
		{
			get
			{
				return this._showMainPartySeaCapacityWarning;
			}
			set
			{
				if (value != this._showMainPartySeaCapacityWarning)
				{
					this._showMainPartySeaCapacityWarning = value;
					base.OnPropertyChangedWithValue(value, "ShowMainPartySeaCapacityWarning");
				}
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x0003D635 File Offset: 0x0003B835
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x0003D63D File Offset: 0x0003B83D
		[DataSourceProperty]
		public bool OtherEquipmentCountWarned
		{
			get
			{
				return this._otherEquipmentCountWarned;
			}
			set
			{
				if (value != this._otherEquipmentCountWarned)
				{
					this._otherEquipmentCountWarned = value;
					base.OnPropertyChangedWithValue(value, "OtherEquipmentCountWarned");
				}
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x0003D65B File Offset: 0x0003B85B
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x0003D663 File Offset: 0x0003B863
		[DataSourceProperty]
		public bool OtherEquipmentCapacityExceededWarning
		{
			get
			{
				return this._otherEquipmentCapacityExceededWarning;
			}
			set
			{
				if (value != this._otherEquipmentCapacityExceededWarning)
				{
					this._otherEquipmentCapacityExceededWarning = value;
					base.OnPropertyChangedWithValue(value, "OtherEquipmentCapacityExceededWarning");
				}
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x0003D681 File Offset: 0x0003B881
		// (set) Token: 0x06000E3D RID: 3645 RVA: 0x0003D689 File Offset: 0x0003B889
		[DataSourceProperty]
		public string OtherEquipmentCountText
		{
			get
			{
				return this._otherEquipmentCountText;
			}
			set
			{
				if (value != this._otherEquipmentCountText)
				{
					this._otherEquipmentCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "OtherEquipmentCountText");
				}
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x0003D6AC File Offset: 0x0003B8AC
		// (set) Token: 0x06000E3F RID: 3647 RVA: 0x0003D6B4 File Offset: 0x0003B8B4
		[DataSourceProperty]
		public string MainPartyTotalWeightCarriedText
		{
			get
			{
				return this._mainPartyTotalWeightCarriedText;
			}
			set
			{
				if (value != this._mainPartyTotalWeightCarriedText)
				{
					this._mainPartyTotalWeightCarriedText = value;
					base.OnPropertyChangedWithValue<string>(value, "MainPartyTotalWeightCarriedText");
				}
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x0003D6D7 File Offset: 0x0003B8D7
		// (set) Token: 0x06000E41 RID: 3649 RVA: 0x0003D6DF File Offset: 0x0003B8DF
		[DataSourceProperty]
		public string MainPartyLandWeightText
		{
			get
			{
				return this._mainPartyLandWeightText;
			}
			set
			{
				if (value != this._mainPartyLandWeightText)
				{
					this._mainPartyLandWeightText = value;
					base.OnPropertyChangedWithValue<string>(value, "MainPartyLandWeightText");
				}
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x0003D702 File Offset: 0x0003B902
		// (set) Token: 0x06000E43 RID: 3651 RVA: 0x0003D70A File Offset: 0x0003B90A
		[DataSourceProperty]
		public string MainPartySeaWeightText
		{
			get
			{
				return this._mainPartySeaWeightText;
			}
			set
			{
				if (value != this._mainPartySeaWeightText)
				{
					this._mainPartySeaWeightText = value;
					base.OnPropertyChangedWithValue<string>(value, "MainPartySeaWeightText");
				}
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x0003D72D File Offset: 0x0003B92D
		// (set) Token: 0x06000E45 RID: 3653 RVA: 0x0003D735 File Offset: 0x0003B935
		[DataSourceProperty]
		public string MainPartyInventoryCapacityText
		{
			get
			{
				return this._mainPartyInventoryCapacityText;
			}
			set
			{
				if (value != this._mainPartyInventoryCapacityText)
				{
					this._mainPartyInventoryCapacityText = value;
					base.OnPropertyChangedWithValue<string>(value, "MainPartyInventoryCapacityText");
				}
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0003D758 File Offset: 0x0003B958
		// (set) Token: 0x06000E47 RID: 3655 RVA: 0x0003D760 File Offset: 0x0003B960
		[DataSourceProperty]
		public string MainPartyLandCapacityText
		{
			get
			{
				return this._mainPartyLandCapacityText;
			}
			set
			{
				if (value != this._mainPartyLandCapacityText)
				{
					this._mainPartyLandCapacityText = value;
					base.OnPropertyChangedWithValue<string>(value, "MainPartyLandCapacityText");
				}
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0003D783 File Offset: 0x0003B983
		// (set) Token: 0x06000E49 RID: 3657 RVA: 0x0003D78B File Offset: 0x0003B98B
		[DataSourceProperty]
		public string MainPartySeaCapacityText
		{
			get
			{
				return this._mainPartySeaCapacityText;
			}
			set
			{
				if (value != this._mainPartySeaCapacityText)
				{
					this._mainPartySeaCapacityText = value;
					base.OnPropertyChangedWithValue<string>(value, "MainPartySeaCapacityText");
				}
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x0003D7AE File Offset: 0x0003B9AE
		// (set) Token: 0x06000E4B RID: 3659 RVA: 0x0003D7B6 File Offset: 0x0003B9B6
		[DataSourceProperty]
		public string NoSaddleText
		{
			get
			{
				return this._noSaddleText;
			}
			set
			{
				if (value != this._noSaddleText)
				{
					this._noSaddleText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoSaddleText");
				}
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x0003D7D9 File Offset: 0x0003B9D9
		// (set) Token: 0x06000E4D RID: 3661 RVA: 0x0003D7E1 File Offset: 0x0003B9E1
		[DataSourceProperty]
		public int TargetEquipmentIndex
		{
			get
			{
				return (int)this._targetEquipmentIndex;
			}
			set
			{
				if (value != (int)this._targetEquipmentIndex)
				{
					this._targetEquipmentIndex = (EquipmentIndex)value;
					base.OnPropertyChangedWithValue(value, "TargetEquipmentIndex");
				}
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x0003D7FF File Offset: 0x0003B9FF
		// (set) Token: 0x06000E4F RID: 3663 RVA: 0x0003D807 File Offset: 0x0003BA07
		public EquipmentIndex TargetEquipmentType
		{
			get
			{
				return this._targetEquipmentIndex;
			}
			set
			{
				if (value != this._targetEquipmentIndex)
				{
					this._targetEquipmentIndex = value;
					base.OnPropertyChanged("TargetEquipmentIndex");
				}
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x0003D824 File Offset: 0x0003BA24
		// (set) Token: 0x06000E51 RID: 3665 RVA: 0x0003D82C File Offset: 0x0003BA2C
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
				}
				this.RefreshTransactionCost(value);
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x0003D851 File Offset: 0x0003BA51
		// (set) Token: 0x06000E53 RID: 3667 RVA: 0x0003D859 File Offset: 0x0003BA59
		[DataSourceProperty]
		public bool IsTrading
		{
			get
			{
				return this._isTrading;
			}
			set
			{
				if (value != this._isTrading)
				{
					this._isTrading = value;
					base.OnPropertyChangedWithValue(value, "IsTrading");
				}
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x0003D877 File Offset: 0x0003BA77
		// (set) Token: 0x06000E55 RID: 3669 RVA: 0x0003D87F File Offset: 0x0003BA7F
		[DataSourceProperty]
		public bool EquipAfterBuy
		{
			get
			{
				return this._equipAfterBuy;
			}
			set
			{
				if (value != this._equipAfterBuy)
				{
					this._equipAfterBuy = value;
					base.OnPropertyChangedWithValue(value, "EquipAfterBuy");
				}
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0003D89D File Offset: 0x0003BA9D
		// (set) Token: 0x06000E57 RID: 3671 RVA: 0x0003D8A5 File Offset: 0x0003BAA5
		[DataSourceProperty]
		public string TradeLbl
		{
			get
			{
				return this._tradeLbl;
			}
			set
			{
				if (value != this._tradeLbl)
				{
					this._tradeLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "TradeLbl");
				}
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06000E58 RID: 3672 RVA: 0x0003D8C8 File Offset: 0x0003BAC8
		// (set) Token: 0x06000E59 RID: 3673 RVA: 0x0003D8D0 File Offset: 0x0003BAD0
		[DataSourceProperty]
		public string ExperienceLbl
		{
			get
			{
				return this._experienceLbl;
			}
			set
			{
				if (value != this._experienceLbl)
				{
					this._experienceLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ExperienceLbl");
				}
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0003D8F3 File Offset: 0x0003BAF3
		// (set) Token: 0x06000E5B RID: 3675 RVA: 0x0003D8FB File Offset: 0x0003BAFB
		[DataSourceProperty]
		public string CurrentCharacterName
		{
			get
			{
				return this._currentCharacterName;
			}
			set
			{
				if (value != this._currentCharacterName)
				{
					this._currentCharacterName = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentCharacterName");
				}
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x0003D91E File Offset: 0x0003BB1E
		// (set) Token: 0x06000E5D RID: 3677 RVA: 0x0003D926 File Offset: 0x0003BB26
		[DataSourceProperty]
		public string RightInventoryOwnerName
		{
			get
			{
				return this._rightInventoryOwnerName;
			}
			set
			{
				if (value != this._rightInventoryOwnerName)
				{
					this._rightInventoryOwnerName = value;
					base.OnPropertyChangedWithValue<string>(value, "RightInventoryOwnerName");
				}
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0003D949 File Offset: 0x0003BB49
		// (set) Token: 0x06000E5F RID: 3679 RVA: 0x0003D951 File Offset: 0x0003BB51
		[DataSourceProperty]
		public string LeftInventoryOwnerName
		{
			get
			{
				return this._leftInventoryOwnerName;
			}
			set
			{
				if (value != this._leftInventoryOwnerName)
				{
					this._leftInventoryOwnerName = value;
					base.OnPropertyChangedWithValue<string>(value, "LeftInventoryOwnerName");
				}
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x0003D974 File Offset: 0x0003BB74
		// (set) Token: 0x06000E61 RID: 3681 RVA: 0x0003D97C File Offset: 0x0003BB7C
		[DataSourceProperty]
		public int RightInventoryOwnerGold
		{
			get
			{
				return this._rightInventoryOwnerGold;
			}
			set
			{
				if (value != this._rightInventoryOwnerGold)
				{
					this._rightInventoryOwnerGold = value;
					base.OnPropertyChangedWithValue(value, "RightInventoryOwnerGold");
				}
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x0003D99A File Offset: 0x0003BB9A
		// (set) Token: 0x06000E63 RID: 3683 RVA: 0x0003D9A2 File Offset: 0x0003BBA2
		[DataSourceProperty]
		public int LeftInventoryOwnerGold
		{
			get
			{
				return this._leftInventoryOwnerGold;
			}
			set
			{
				if (value != this._leftInventoryOwnerGold)
				{
					this._leftInventoryOwnerGold = value;
					base.OnPropertyChangedWithValue(value, "LeftInventoryOwnerGold");
				}
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x0003D9C0 File Offset: 0x0003BBC0
		// (set) Token: 0x06000E65 RID: 3685 RVA: 0x0003D9C8 File Offset: 0x0003BBC8
		[DataSourceProperty]
		public int ItemCountToBuy
		{
			get
			{
				return this._itemCountToBuy;
			}
			set
			{
				if (value != this._itemCountToBuy)
				{
					this._itemCountToBuy = value;
					base.OnPropertyChangedWithValue(value, "ItemCountToBuy");
				}
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x0003D9E6 File Offset: 0x0003BBE6
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x0003D9EE File Offset: 0x0003BBEE
		[DataSourceProperty]
		public string CurrentCharacterTotalEncumbrance
		{
			get
			{
				return this._currentCharacterTotalEncumbrance;
			}
			set
			{
				if (value != this._currentCharacterTotalEncumbrance)
				{
					this._currentCharacterTotalEncumbrance = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentCharacterTotalEncumbrance");
				}
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0003DA11 File Offset: 0x0003BC11
		// (set) Token: 0x06000E69 RID: 3689 RVA: 0x0003DA19 File Offset: 0x0003BC19
		[DataSourceProperty]
		public float CurrentCharacterLegArmor
		{
			get
			{
				return this._currentCharacterLegArmor;
			}
			set
			{
				if (MathF.Abs(value - this._currentCharacterLegArmor) > 0.01f)
				{
					this._currentCharacterLegArmor = value;
					base.OnPropertyChangedWithValue(value, "CurrentCharacterLegArmor");
				}
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x0003DA42 File Offset: 0x0003BC42
		// (set) Token: 0x06000E6B RID: 3691 RVA: 0x0003DA4A File Offset: 0x0003BC4A
		[DataSourceProperty]
		public float CurrentCharacterHeadArmor
		{
			get
			{
				return this._currentCharacterHeadArmor;
			}
			set
			{
				if (MathF.Abs(value - this._currentCharacterHeadArmor) > 0.01f)
				{
					this._currentCharacterHeadArmor = value;
					base.OnPropertyChangedWithValue(value, "CurrentCharacterHeadArmor");
				}
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0003DA73 File Offset: 0x0003BC73
		// (set) Token: 0x06000E6D RID: 3693 RVA: 0x0003DA7B File Offset: 0x0003BC7B
		[DataSourceProperty]
		public float CurrentCharacterBodyArmor
		{
			get
			{
				return this._currentCharacterBodyArmor;
			}
			set
			{
				if (MathF.Abs(value - this._currentCharacterBodyArmor) > 0.01f)
				{
					this._currentCharacterBodyArmor = value;
					base.OnPropertyChangedWithValue(value, "CurrentCharacterBodyArmor");
				}
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0003DAA4 File Offset: 0x0003BCA4
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x0003DAAC File Offset: 0x0003BCAC
		[DataSourceProperty]
		public float CurrentCharacterArmArmor
		{
			get
			{
				return this._currentCharacterArmArmor;
			}
			set
			{
				if (MathF.Abs(value - this._currentCharacterArmArmor) > 0.01f)
				{
					this._currentCharacterArmArmor = value;
					base.OnPropertyChangedWithValue(value, "CurrentCharacterArmArmor");
				}
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0003DAD5 File Offset: 0x0003BCD5
		// (set) Token: 0x06000E71 RID: 3697 RVA: 0x0003DADD File Offset: 0x0003BCDD
		[DataSourceProperty]
		public float CurrentCharacterHorseArmor
		{
			get
			{
				return this._currentCharacterHorseArmor;
			}
			set
			{
				if (MathF.Abs(value - this._currentCharacterHorseArmor) > 0.01f)
				{
					this._currentCharacterHorseArmor = value;
					base.OnPropertyChangedWithValue(value, "CurrentCharacterHorseArmor");
				}
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0003DB06 File Offset: 0x0003BD06
		// (set) Token: 0x06000E73 RID: 3699 RVA: 0x0003DB0E File Offset: 0x0003BD0E
		[DataSourceProperty]
		public bool IsRefreshed
		{
			get
			{
				return this._isRefreshed;
			}
			set
			{
				if (this._isRefreshed != value)
				{
					this._isRefreshed = value;
					base.OnPropertyChangedWithValue(value, "IsRefreshed");
				}
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0003DB2C File Offset: 0x0003BD2C
		// (set) Token: 0x06000E75 RID: 3701 RVA: 0x0003DB34 File Offset: 0x0003BD34
		[DataSourceProperty]
		public bool IsExtendedEquipmentControlsEnabled
		{
			get
			{
				return this._isExtendedEquipmentControlsEnabled;
			}
			set
			{
				if (value != this._isExtendedEquipmentControlsEnabled)
				{
					this._isExtendedEquipmentControlsEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsExtendedEquipmentControlsEnabled");
				}
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0003DB52 File Offset: 0x0003BD52
		// (set) Token: 0x06000E77 RID: 3703 RVA: 0x0003DB5A File Offset: 0x0003BD5A
		[DataSourceProperty]
		public bool IsFocusedOnItemList
		{
			get
			{
				return this._isFocusedOnItemList;
			}
			set
			{
				if (value != this._isFocusedOnItemList)
				{
					this._isFocusedOnItemList = value;
					base.OnPropertyChangedWithValue(value, "IsFocusedOnItemList");
				}
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0003DB78 File Offset: 0x0003BD78
		// (set) Token: 0x06000E79 RID: 3705 RVA: 0x0003DB80 File Offset: 0x0003BD80
		[DataSourceProperty]
		public SPItemVM CurrentFocusedItem
		{
			get
			{
				return this._currentFocusedItem;
			}
			set
			{
				if (value != this._currentFocusedItem)
				{
					this._currentFocusedItem = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CurrentFocusedItem");
				}
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0003DB9E File Offset: 0x0003BD9E
		// (set) Token: 0x06000E7B RID: 3707 RVA: 0x0003DBA6 File Offset: 0x0003BDA6
		[DataSourceProperty]
		public SPItemVM CharacterHelmSlot
		{
			get
			{
				return this._characterHelmSlot;
			}
			set
			{
				if (value != this._characterHelmSlot)
				{
					this._characterHelmSlot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterHelmSlot");
				}
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0003DBC4 File Offset: 0x0003BDC4
		// (set) Token: 0x06000E7D RID: 3709 RVA: 0x0003DBCC File Offset: 0x0003BDCC
		[DataSourceProperty]
		public SPItemVM CharacterCloakSlot
		{
			get
			{
				return this._characterCloakSlot;
			}
			set
			{
				if (value != this._characterCloakSlot)
				{
					this._characterCloakSlot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterCloakSlot");
				}
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x0003DBEA File Offset: 0x0003BDEA
		// (set) Token: 0x06000E7F RID: 3711 RVA: 0x0003DBF2 File Offset: 0x0003BDF2
		[DataSourceProperty]
		public SPItemVM CharacterTorsoSlot
		{
			get
			{
				return this._characterTorsoSlot;
			}
			set
			{
				if (value != this._characterTorsoSlot)
				{
					this._characterTorsoSlot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterTorsoSlot");
				}
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x0003DC10 File Offset: 0x0003BE10
		// (set) Token: 0x06000E81 RID: 3713 RVA: 0x0003DC18 File Offset: 0x0003BE18
		[DataSourceProperty]
		public SPItemVM CharacterGloveSlot
		{
			get
			{
				return this._characterGloveSlot;
			}
			set
			{
				if (value != this._characterGloveSlot)
				{
					this._characterGloveSlot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterGloveSlot");
				}
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0003DC36 File Offset: 0x0003BE36
		// (set) Token: 0x06000E83 RID: 3715 RVA: 0x0003DC3E File Offset: 0x0003BE3E
		[DataSourceProperty]
		public SPItemVM CharacterBootSlot
		{
			get
			{
				return this._characterBootSlot;
			}
			set
			{
				if (value != this._characterBootSlot)
				{
					this._characterBootSlot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterBootSlot");
				}
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0003DC5C File Offset: 0x0003BE5C
		// (set) Token: 0x06000E85 RID: 3717 RVA: 0x0003DC64 File Offset: 0x0003BE64
		[DataSourceProperty]
		public SPItemVM CharacterMountSlot
		{
			get
			{
				return this._characterMountSlot;
			}
			set
			{
				if (value != this._characterMountSlot)
				{
					this._characterMountSlot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterMountSlot");
				}
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0003DC82 File Offset: 0x0003BE82
		// (set) Token: 0x06000E87 RID: 3719 RVA: 0x0003DC8A File Offset: 0x0003BE8A
		[DataSourceProperty]
		public SPItemVM CharacterMountArmorSlot
		{
			get
			{
				return this._characterMountArmorSlot;
			}
			set
			{
				if (value != this._characterMountArmorSlot)
				{
					this._characterMountArmorSlot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterMountArmorSlot");
				}
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0003DCA8 File Offset: 0x0003BEA8
		// (set) Token: 0x06000E89 RID: 3721 RVA: 0x0003DCB0 File Offset: 0x0003BEB0
		[DataSourceProperty]
		public SPItemVM CharacterWeapon1Slot
		{
			get
			{
				return this._characterWeapon1Slot;
			}
			set
			{
				if (value != this._characterWeapon1Slot)
				{
					this._characterWeapon1Slot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterWeapon1Slot");
				}
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0003DCCE File Offset: 0x0003BECE
		// (set) Token: 0x06000E8B RID: 3723 RVA: 0x0003DCD6 File Offset: 0x0003BED6
		[DataSourceProperty]
		public SPItemVM CharacterWeapon2Slot
		{
			get
			{
				return this._characterWeapon2Slot;
			}
			set
			{
				if (value != this._characterWeapon2Slot)
				{
					this._characterWeapon2Slot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterWeapon2Slot");
				}
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x0003DCF4 File Offset: 0x0003BEF4
		// (set) Token: 0x06000E8D RID: 3725 RVA: 0x0003DCFC File Offset: 0x0003BEFC
		[DataSourceProperty]
		public SPItemVM CharacterWeapon3Slot
		{
			get
			{
				return this._characterWeapon3Slot;
			}
			set
			{
				if (value != this._characterWeapon3Slot)
				{
					this._characterWeapon3Slot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterWeapon3Slot");
				}
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0003DD1A File Offset: 0x0003BF1A
		// (set) Token: 0x06000E8F RID: 3727 RVA: 0x0003DD22 File Offset: 0x0003BF22
		[DataSourceProperty]
		public SPItemVM CharacterWeapon4Slot
		{
			get
			{
				return this._characterWeapon4Slot;
			}
			set
			{
				if (value != this._characterWeapon4Slot)
				{
					this._characterWeapon4Slot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterWeapon4Slot");
				}
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0003DD40 File Offset: 0x0003BF40
		// (set) Token: 0x06000E91 RID: 3729 RVA: 0x0003DD48 File Offset: 0x0003BF48
		[DataSourceProperty]
		public SPItemVM CharacterBannerSlot
		{
			get
			{
				return this._characterBannerSlot;
			}
			set
			{
				if (value != this._characterBannerSlot)
				{
					this._characterBannerSlot = value;
					base.OnPropertyChangedWithValue<SPItemVM>(value, "CharacterBannerSlot");
				}
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0003DD66 File Offset: 0x0003BF66
		// (set) Token: 0x06000E93 RID: 3731 RVA: 0x0003DD6E File Offset: 0x0003BF6E
		[DataSourceProperty]
		public HeroViewModel MainCharacter
		{
			get
			{
				return this._mainCharacter;
			}
			set
			{
				if (value != this._mainCharacter)
				{
					this._mainCharacter = value;
					base.OnPropertyChangedWithValue<HeroViewModel>(value, "MainCharacter");
				}
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06000E94 RID: 3732 RVA: 0x0003DD8C File Offset: 0x0003BF8C
		// (set) Token: 0x06000E95 RID: 3733 RVA: 0x0003DD94 File Offset: 0x0003BF94
		[DataSourceProperty]
		public MBBindingList<SPItemVM> RightItemListVM
		{
			get
			{
				return this._rightItemListVM;
			}
			set
			{
				if (value != this._rightItemListVM)
				{
					this._rightItemListVM = value;
					base.OnPropertyChangedWithValue<MBBindingList<SPItemVM>>(value, "RightItemListVM");
				}
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x0003DDB2 File Offset: 0x0003BFB2
		// (set) Token: 0x06000E97 RID: 3735 RVA: 0x0003DDBA File Offset: 0x0003BFBA
		[DataSourceProperty]
		public MBBindingList<SPItemVM> LeftItemListVM
		{
			get
			{
				return this._leftItemListVM;
			}
			set
			{
				if (value != this._leftItemListVM)
				{
					this._leftItemListVM = value;
					base.OnPropertyChangedWithValue<MBBindingList<SPItemVM>>(value, "LeftItemListVM");
				}
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x0003DDD8 File Offset: 0x0003BFD8
		// (set) Token: 0x06000E99 RID: 3737 RVA: 0x0003DDE0 File Offset: 0x0003BFE0
		[DataSourceProperty]
		public bool IsBannerItemsHighlightApplied
		{
			get
			{
				return this._isBannerItemsHighlightApplied;
			}
			set
			{
				if (value != this._isBannerItemsHighlightApplied)
				{
					this._isBannerItemsHighlightApplied = value;
					base.OnPropertyChangedWithValue(value, "IsBannerItemsHighlightApplied");
				}
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x0003DDFE File Offset: 0x0003BFFE
		// (set) Token: 0x06000E9B RID: 3739 RVA: 0x0003DE06 File Offset: 0x0003C006
		[DataSourceProperty]
		public string BannerTypeName
		{
			get
			{
				return this._bannerTypeName;
			}
			set
			{
				if (value != this._bannerTypeName)
				{
					this._bannerTypeName = value;
					base.OnPropertyChangedWithValue<string>(value, "BannerTypeName");
				}
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x0003DE29 File Offset: 0x0003C029
		// (set) Token: 0x06000E9D RID: 3741 RVA: 0x0003DE31 File Offset: 0x0003C031
		[DataSourceProperty]
		public bool ScrollToItem
		{
			get
			{
				return this._scrollToItem;
			}
			set
			{
				if (value != this._scrollToItem)
				{
					this._scrollToItem = value;
					base.OnPropertyChangedWithValue(value, "ScrollToItem");
				}
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x0003DE4F File Offset: 0x0003C04F
		// (set) Token: 0x06000E9F RID: 3743 RVA: 0x0003DE57 File Offset: 0x0003C057
		[DataSourceProperty]
		public string ScrollItemId
		{
			get
			{
				return this._scrollItemId;
			}
			set
			{
				if (value != this._scrollItemId)
				{
					this._scrollItemId = value;
					base.OnPropertyChangedWithValue<string>(value, "ScrollItemId");
				}
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x0003DE7A File Offset: 0x0003C07A
		// (set) Token: 0x06000EA1 RID: 3745 RVA: 0x0003DE82 File Offset: 0x0003C082
		[DataSourceProperty]
		public bool IsCivilianMode
		{
			get
			{
				return this._isCivilianMode;
			}
			set
			{
				if (value != this._isCivilianMode)
				{
					this._isCivilianMode = value;
					base.OnPropertyChangedWithValue(value, "IsCivilianMode");
				}
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x0003DEA0 File Offset: 0x0003C0A0
		// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x0003DEA8 File Offset: 0x0003C0A8
		[DataSourceProperty]
		public bool IsBattleMode
		{
			get
			{
				return this._isBattleMode;
			}
			set
			{
				if (value != this._isBattleMode)
				{
					this._isBattleMode = value;
					base.OnPropertyChangedWithValue(value, "IsBattleMode");
				}
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x0003DEC6 File Offset: 0x0003C0C6
		// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x0003DECE File Offset: 0x0003C0CE
		[DataSourceProperty]
		public bool IsStealthMode
		{
			get
			{
				return this._isStealthMode;
			}
			set
			{
				if (value != this._isStealthMode)
				{
					this._isStealthMode = value;
					base.OnPropertyChangedWithValue(value, "IsStealthMode");
				}
			}
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0003DEEC File Offset: 0x0003C0EC
		private TextObject GetPreviousCharacterKeyText()
		{
			if (this.PreviousCharacterInputKey == null || this._getKeyTextFromKeyId == null)
			{
				return TextObject.GetEmpty();
			}
			return this._getKeyTextFromKeyId(this.PreviousCharacterInputKey.KeyID);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0003DF1A File Offset: 0x0003C11A
		private TextObject GetNextCharacterKeyText()
		{
			if (this.NextCharacterInputKey == null || this._getKeyTextFromKeyId == null)
			{
				return TextObject.GetEmpty();
			}
			return this._getKeyTextFromKeyId(this.NextCharacterInputKey.KeyID);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0003DF48 File Offset: 0x0003C148
		private TextObject GetBuyAllKeyText()
		{
			if (this.BuyAllInputKey == null || this._getKeyTextFromKeyId == null)
			{
				return TextObject.GetEmpty();
			}
			return this._getKeyTextFromKeyId(this.BuyAllInputKey.KeyID);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0003DF76 File Offset: 0x0003C176
		private TextObject GetSellAllKeyText()
		{
			if (this.SellAllInputKey == null || this._getKeyTextFromKeyId == null)
			{
				return TextObject.GetEmpty();
			}
			return this._getKeyTextFromKeyId(this.SellAllInputKey.KeyID);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0003DFA4 File Offset: 0x0003C1A4
		public void SetResetInputKey(HotKey hotkey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0003DFB3 File Offset: 0x0003C1B3
		public void SetCancelInputKey(HotKey gameKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(gameKey, true);
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0003DFC2 File Offset: 0x0003C1C2
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0003DFD1 File Offset: 0x0003C1D1
		public void SetPreviousCharacterInputKey(HotKey hotKey)
		{
			this.PreviousCharacterInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.SetPreviousCharacterHint();
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0003DFE6 File Offset: 0x0003C1E6
		public void SetNextCharacterInputKey(HotKey hotKey)
		{
			this.NextCharacterInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.SetNextCharacterHint();
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0003DFFB File Offset: 0x0003C1FB
		public void SetBuyAllInputKey(HotKey hotKey)
		{
			this.BuyAllInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.SetBuyAllHint();
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0003E010 File Offset: 0x0003C210
		public void SetSellAllInputKey(HotKey hotKey)
		{
			this.SellAllInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.SetSellAllHint();
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0003E025 File Offset: 0x0003C225
		public void SetGetKeyTextFromKeyIDFunc(Func<string, TextObject> getKeyTextFromKeyId)
		{
			this._getKeyTextFromKeyId = getKeyTextFromKeyId;
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x0003E02E File Offset: 0x0003C22E
		// (set) Token: 0x06000EB3 RID: 3763 RVA: 0x0003E036 File Offset: 0x0003C236
		[DataSourceProperty]
		public InputKeyItemVM ResetInputKey
		{
			get
			{
				return this._resetInputKey;
			}
			set
			{
				if (value != this._resetInputKey)
				{
					this._resetInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ResetInputKey");
				}
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x0003E054 File Offset: 0x0003C254
		// (set) Token: 0x06000EB5 RID: 3765 RVA: 0x0003E05C File Offset: 0x0003C25C
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x0003E07A File Offset: 0x0003C27A
		// (set) Token: 0x06000EB7 RID: 3767 RVA: 0x0003E082 File Offset: 0x0003C282
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x0003E0A0 File Offset: 0x0003C2A0
		// (set) Token: 0x06000EB9 RID: 3769 RVA: 0x0003E0A8 File Offset: 0x0003C2A8
		[DataSourceProperty]
		public InputKeyItemVM PreviousCharacterInputKey
		{
			get
			{
				return this._previousCharacterInputKey;
			}
			set
			{
				if (value != this._previousCharacterInputKey)
				{
					this._previousCharacterInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviousCharacterInputKey");
				}
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x0003E0C6 File Offset: 0x0003C2C6
		// (set) Token: 0x06000EBB RID: 3771 RVA: 0x0003E0CE File Offset: 0x0003C2CE
		[DataSourceProperty]
		public InputKeyItemVM NextCharacterInputKey
		{
			get
			{
				return this._nextCharacterInputKey;
			}
			set
			{
				if (value != this._nextCharacterInputKey)
				{
					this._nextCharacterInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "NextCharacterInputKey");
				}
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0003E0EC File Offset: 0x0003C2EC
		// (set) Token: 0x06000EBD RID: 3773 RVA: 0x0003E0F4 File Offset: 0x0003C2F4
		[DataSourceProperty]
		public InputKeyItemVM BuyAllInputKey
		{
			get
			{
				return this._buyAllInputKey;
			}
			set
			{
				if (value != this._buyAllInputKey)
				{
					this._buyAllInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "BuyAllInputKey");
				}
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06000EBE RID: 3774 RVA: 0x0003E112 File Offset: 0x0003C312
		// (set) Token: 0x06000EBF RID: 3775 RVA: 0x0003E11A File Offset: 0x0003C31A
		[DataSourceProperty]
		public InputKeyItemVM SellAllInputKey
		{
			get
			{
				return this._sellAllInputKey;
			}
			set
			{
				if (value != this._sellAllInputKey)
				{
					this._sellAllInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "SellAllInputKey");
				}
			}
		}

		// Token: 0x040005F2 RID: 1522
		public bool DoNotSync;

		// Token: 0x040005F3 RID: 1523
		private readonly Func<WeaponComponentData, ItemObject.ItemUsageSetFlags> _getItemUsageSetFlags;

		// Token: 0x040005F4 RID: 1524
		public bool IsFiveStackModifierActive;

		// Token: 0x040005F5 RID: 1525
		private readonly IViewDataTracker _viewDataTracker;

		// Token: 0x040005F6 RID: 1526
		public bool IsEntireStackModifierActive;

		// Token: 0x040005F7 RID: 1527
		private readonly int _donationMaxShareableXp;

		// Token: 0x040005F8 RID: 1528
		private readonly Stack<SPItemVM> _equipAfterTransferStack;

		// Token: 0x040005F9 RID: 1529
		private readonly TroopRoster _rightTroopRoster;

		// Token: 0x040005FA RID: 1530
		private InventoryScreenHelper.InventoryMode _usageType = InventoryScreenHelper.InventoryMode.Trade;

		// Token: 0x040005FB RID: 1531
		private bool _isTrading;

		// Token: 0x040005FC RID: 1532
		private readonly TroopRoster _leftTroopRoster;

		// Token: 0x040005FD RID: 1533
		private int _lastComparedItemIndex;

		// Token: 0x040005FE RID: 1534
		private bool _isCharacterEquipmentDirty;

		// Token: 0x040005FF RID: 1535
		private int _currentInventoryCharacterIndex;

		// Token: 0x04000600 RID: 1536
		private string _selectedTooltipItemStringID = "";

		// Token: 0x04000601 RID: 1537
		private string _comparedTooltipItemStringID = "";

		// Token: 0x04000602 RID: 1538
		private InventoryLogic _inventoryLogic;

		// Token: 0x04000603 RID: 1539
		private CharacterObject _currentCharacter;

		// Token: 0x04000604 RID: 1540
		private SPItemVM _selectedItem;

		// Token: 0x04000605 RID: 1541
		private List<ItemVM> _comparedItemList;

		// Token: 0x04000606 RID: 1542
		private Func<string, TextObject> _getKeyTextFromKeyId;

		// Token: 0x04000607 RID: 1543
		private List<string> _lockedItemIDs;

		// Token: 0x04000608 RID: 1544
		private readonly List<int> _everyItemType = new List<int>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
			21, 22, 23, 24, 25, 26
		};

		// Token: 0x04000609 RID: 1545
		private readonly List<int> _weaponItemTypes = new List<int> { 2, 3, 4 };

		// Token: 0x0400060A RID: 1546
		private readonly List<int> _armorItemTypes = new List<int> { 14, 15, 16, 17, 23, 24 };

		// Token: 0x0400060B RID: 1547
		private readonly List<int> _mountItemTypes = new List<int> { 1, 25 };

		// Token: 0x0400060C RID: 1548
		private readonly List<int> _shieldAndRangedItemTypes = new List<int>
		{
			8, 5, 6, 7, 9, 10, 11, 12, 18, 19,
			20
		};

		// Token: 0x0400060D RID: 1549
		private readonly List<int> _miscellaneousItemTypes = new List<int> { 13, 21, 22, 26 };

		// Token: 0x0400060E RID: 1550
		private readonly Dictionary<SPInventoryVM.Filters, List<int>> _filters;

		// Token: 0x0400060F RID: 1551
		private int _selectedEquipmentIndex;

		// Token: 0x04000610 RID: 1552
		private bool _isFoodTransferButtonHighlightApplied;

		// Token: 0x04000611 RID: 1553
		private bool _isBannerItemsHighlightApplied;

		// Token: 0x04000612 RID: 1554
		private string _latestTutorialElementID;

		// Token: 0x04000613 RID: 1555
		private string _leftInventoryLabel;

		// Token: 0x04000614 RID: 1556
		private string _rightInventoryLabel;

		// Token: 0x04000615 RID: 1557
		private bool _otherSideHasCapacity;

		// Token: 0x04000616 RID: 1558
		private bool _isDoneDisabled;

		// Token: 0x04000617 RID: 1559
		private bool _isSearchAvailable;

		// Token: 0x04000618 RID: 1560
		private bool _isOtherInventoryGoldRelevant;

		// Token: 0x04000619 RID: 1561
		private string _doneLbl;

		// Token: 0x0400061A RID: 1562
		private string _cancelLbl;

		// Token: 0x0400061B RID: 1563
		private string _resetLbl;

		// Token: 0x0400061C RID: 1564
		private string _typeText;

		// Token: 0x0400061D RID: 1565
		private string _nameText;

		// Token: 0x0400061E RID: 1566
		private string _quantityText;

		// Token: 0x0400061F RID: 1567
		private string _costText;

		// Token: 0x04000620 RID: 1568
		private string _searchPlaceholderText;

		// Token: 0x04000621 RID: 1569
		private HintViewModel _resetHint;

		// Token: 0x04000622 RID: 1570
		private HintViewModel _filterAllHint;

		// Token: 0x04000623 RID: 1571
		private HintViewModel _filterWeaponHint;

		// Token: 0x04000624 RID: 1572
		private HintViewModel _filterArmorHint;

		// Token: 0x04000625 RID: 1573
		private HintViewModel _filterShieldAndRangedHint;

		// Token: 0x04000626 RID: 1574
		private HintViewModel _filterMountAndHarnessHint;

		// Token: 0x04000627 RID: 1575
		private HintViewModel _filterMiscHint;

		// Token: 0x04000628 RID: 1576
		private HintViewModel _stealthOutfitHint;

		// Token: 0x04000629 RID: 1577
		private HintViewModel _civilianOutfitHint;

		// Token: 0x0400062A RID: 1578
		private HintViewModel _battleOutfitHint;

		// Token: 0x0400062B RID: 1579
		private HintViewModel _equipmentHelmSlotHint;

		// Token: 0x0400062C RID: 1580
		private HintViewModel _equipmentArmorSlotHint;

		// Token: 0x0400062D RID: 1581
		private HintViewModel _equipmentBootSlotHint;

		// Token: 0x0400062E RID: 1582
		private HintViewModel _equipmentCloakSlotHint;

		// Token: 0x0400062F RID: 1583
		private HintViewModel _equipmentGloveSlotHint;

		// Token: 0x04000630 RID: 1584
		private HintViewModel _equipmentHarnessSlotHint;

		// Token: 0x04000631 RID: 1585
		private HintViewModel _equipmentMountSlotHint;

		// Token: 0x04000632 RID: 1586
		private HintViewModel _equipmentWeaponSlotHint;

		// Token: 0x04000633 RID: 1587
		private HintViewModel _equipmentBannerSlotHint;

		// Token: 0x04000634 RID: 1588
		private BasicTooltipViewModel _buyAllHint;

		// Token: 0x04000635 RID: 1589
		private BasicTooltipViewModel _sellAllHint;

		// Token: 0x04000636 RID: 1590
		private BasicTooltipViewModel _previousCharacterHint;

		// Token: 0x04000637 RID: 1591
		private BasicTooltipViewModel _nextCharacterHint;

		// Token: 0x04000638 RID: 1592
		private HintViewModel _weightHint;

		// Token: 0x04000639 RID: 1593
		private HintViewModel _armArmorHint;

		// Token: 0x0400063A RID: 1594
		private HintViewModel _bodyArmorHint;

		// Token: 0x0400063B RID: 1595
		private HintViewModel _headArmorHint;

		// Token: 0x0400063C RID: 1596
		private HintViewModel _legArmorHint;

		// Token: 0x0400063D RID: 1597
		private HintViewModel _horseArmorHint;

		// Token: 0x0400063E RID: 1598
		private HintViewModel _previewHint;

		// Token: 0x0400063F RID: 1599
		private HintViewModel _equipHint;

		// Token: 0x04000640 RID: 1600
		private HintViewModel _unequipHint;

		// Token: 0x04000641 RID: 1601
		private HintViewModel _sellHint;

		// Token: 0x04000642 RID: 1602
		private HintViewModel _playerSideCapacityExceededHint;

		// Token: 0x04000643 RID: 1603
		private HintViewModel _mainPartyLandCapacityExceededHint;

		// Token: 0x04000644 RID: 1604
		private HintViewModel _mainPartySeaCapacityExceededHint;

		// Token: 0x04000645 RID: 1605
		private HintViewModel _noSaddleHint;

		// Token: 0x04000646 RID: 1606
		private HintViewModel _donationLblHint;

		// Token: 0x04000647 RID: 1607
		private HintViewModel _otherSideCapacityExceededHint;

		// Token: 0x04000648 RID: 1608
		private BasicTooltipViewModel _totalWeightCarriedHint;

		// Token: 0x04000649 RID: 1609
		private BasicTooltipViewModel _landWeightHint;

		// Token: 0x0400064A RID: 1610
		private BasicTooltipViewModel _seaWeightHint;

		// Token: 0x0400064B RID: 1611
		private BasicTooltipViewModel _inventoryCapacityHint;

		// Token: 0x0400064C RID: 1612
		private BasicTooltipViewModel _landCapacityHint;

		// Token: 0x0400064D RID: 1613
		private BasicTooltipViewModel _seaCapacityHint;

		// Token: 0x0400064E RID: 1614
		private BasicTooltipViewModel _currentCharacterSkillsTooltip;

		// Token: 0x0400064F RID: 1615
		private BasicTooltipViewModel _productionTooltip;

		// Token: 0x04000650 RID: 1616
		private HeroViewModel _mainCharacter;

		// Token: 0x04000651 RID: 1617
		private bool _isExtendedEquipmentControlsEnabled;

		// Token: 0x04000652 RID: 1618
		private bool _isFocusedOnItemList;

		// Token: 0x04000653 RID: 1619
		private SPItemVM _currentFocusedItem;

		// Token: 0x04000654 RID: 1620
		private bool _equipAfterBuy;

		// Token: 0x04000655 RID: 1621
		private MBBindingList<SPItemVM> _leftItemListVM;

		// Token: 0x04000656 RID: 1622
		private MBBindingList<SPItemVM> _rightItemListVM;

		// Token: 0x04000657 RID: 1623
		private ItemMenuVM _itemMenu;

		// Token: 0x04000658 RID: 1624
		private SPItemVM _characterHelmSlot;

		// Token: 0x04000659 RID: 1625
		private SPItemVM _characterCloakSlot;

		// Token: 0x0400065A RID: 1626
		private SPItemVM _characterTorsoSlot;

		// Token: 0x0400065B RID: 1627
		private SPItemVM _characterGloveSlot;

		// Token: 0x0400065C RID: 1628
		private SPItemVM _characterBootSlot;

		// Token: 0x0400065D RID: 1629
		private SPItemVM _characterMountSlot;

		// Token: 0x0400065E RID: 1630
		private SPItemVM _characterMountArmorSlot;

		// Token: 0x0400065F RID: 1631
		private SPItemVM _characterWeapon1Slot;

		// Token: 0x04000660 RID: 1632
		private SPItemVM _characterWeapon2Slot;

		// Token: 0x04000661 RID: 1633
		private SPItemVM _characterWeapon3Slot;

		// Token: 0x04000662 RID: 1634
		private SPItemVM _characterWeapon4Slot;

		// Token: 0x04000663 RID: 1635
		private SPItemVM _characterBannerSlot;

		// Token: 0x04000664 RID: 1636
		private EquipmentIndex _targetEquipmentIndex = EquipmentIndex.None;

		// Token: 0x04000665 RID: 1637
		private int _transactionCount = -1;

		// Token: 0x04000666 RID: 1638
		private bool _isRefreshed;

		// Token: 0x04000667 RID: 1639
		private string _tradeLbl = "";

		// Token: 0x04000668 RID: 1640
		private string _experienceLbl = "";

		// Token: 0x04000669 RID: 1641
		private bool _hasGainedExperience;

		// Token: 0x0400066A RID: 1642
		private bool _isDonationXpGainExceedsMax;

		// Token: 0x0400066B RID: 1643
		private bool _noSaddleWarned;

		// Token: 0x0400066C RID: 1644
		private bool _isTradingWithSettlement;

		// Token: 0x0400066D RID: 1645
		private bool _showMainPartyLandCapacityTexts;

		// Token: 0x0400066E RID: 1646
		private bool _showMainPartySeaCapacityTexts;

		// Token: 0x0400066F RID: 1647
		private string _otherEquipmentCountText;

		// Token: 0x04000670 RID: 1648
		private string _mainPartyTotalWeightCarriedText;

		// Token: 0x04000671 RID: 1649
		private string _mainPartyLandWeightText;

		// Token: 0x04000672 RID: 1650
		private string _mainPartySeaWeightText;

		// Token: 0x04000673 RID: 1651
		private string _mainPartyInventoryCapacityText;

		// Token: 0x04000674 RID: 1652
		private bool _otherEquipmentCapacityExceededWarning;

		// Token: 0x04000675 RID: 1653
		private bool _otherEquipmentCountWarned;

		// Token: 0x04000676 RID: 1654
		private bool _playerEquipmentCountWarned;

		// Token: 0x04000677 RID: 1655
		private string _noSaddleText;

		// Token: 0x04000678 RID: 1656
		private string _mainPartyLandCapacityText;

		// Token: 0x04000679 RID: 1657
		private string _mainPartySeaCapacityText;

		// Token: 0x0400067A RID: 1658
		private bool _isMainPartyLandCapacityWarned;

		// Token: 0x0400067B RID: 1659
		private string _leftSearchText = "";

		// Token: 0x0400067C RID: 1660
		private bool _isMainPartySeaCapacityWarned;

		// Token: 0x0400067D RID: 1661
		private bool _showMainPartyLandCapacityWarning;

		// Token: 0x0400067E RID: 1662
		private bool _showMainPartySeaCapacityWarning;

		// Token: 0x0400067F RID: 1663
		private string _playerSideCapacityExceededText;

		// Token: 0x04000680 RID: 1664
		private string _mainPartyLandCapacityExceededText;

		// Token: 0x04000681 RID: 1665
		private string _mainPartySeaCapacityExceededText;

		// Token: 0x04000682 RID: 1666
		private string _separatorText;

		// Token: 0x04000683 RID: 1667
		private string _otherSideCapacityExceededText;

		// Token: 0x04000684 RID: 1668
		private string _rightSearchText = "";

		// Token: 0x04000685 RID: 1669
		private string _bannerTypeName;

		// Token: 0x04000686 RID: 1670
		private SPInventoryVM.EquipmentModes _equipmentMode = SPInventoryVM.EquipmentModes.Battle;

		// Token: 0x04000687 RID: 1671
		private bool _companionExists;

		// Token: 0x04000688 RID: 1672
		private SPInventoryVM.Filters _activeFilterIndex;

		// Token: 0x04000689 RID: 1673
		private bool _isMicsFilterHighlightEnabled;

		// Token: 0x0400068A RID: 1674
		private bool _isCivilianFilterHighlightEnabled;

		// Token: 0x0400068B RID: 1675
		private ItemPreviewVM _itemPreview;

		// Token: 0x0400068C RID: 1676
		private SelectorVM<InventoryCharacterSelectorItemVM> _characterList;

		// Token: 0x0400068D RID: 1677
		private SPInventorySortControllerVM _otherInventorySortController;

		// Token: 0x0400068E RID: 1678
		private SPInventorySortControllerVM _playerInventorySortController;

		// Token: 0x0400068F RID: 1679
		private bool _scrollToItem;

		// Token: 0x04000690 RID: 1680
		private string _scrollItemId;

		// Token: 0x04000691 RID: 1681
		private bool _isBattleMode = true;

		// Token: 0x04000692 RID: 1682
		private bool _isStealthMode;

		// Token: 0x04000693 RID: 1683
		private bool _isCivilianMode;

		// Token: 0x04000694 RID: 1684
		private string _leftInventoryOwnerName;

		// Token: 0x04000695 RID: 1685
		private int _leftInventoryOwnerGold;

		// Token: 0x04000696 RID: 1686
		private string _rightInventoryOwnerName;

		// Token: 0x04000697 RID: 1687
		private string _currentCharacterName;

		// Token: 0x04000698 RID: 1688
		private int _rightInventoryOwnerGold;

		// Token: 0x04000699 RID: 1689
		private int _itemCountToBuy;

		// Token: 0x0400069A RID: 1690
		private float _currentCharacterArmArmor;

		// Token: 0x0400069B RID: 1691
		private float _currentCharacterBodyArmor;

		// Token: 0x0400069C RID: 1692
		private float _currentCharacterHeadArmor;

		// Token: 0x0400069D RID: 1693
		private float _currentCharacterLegArmor;

		// Token: 0x0400069E RID: 1694
		private float _currentCharacterHorseArmor;

		// Token: 0x0400069F RID: 1695
		private string _currentCharacterTotalEncumbrance;

		// Token: 0x040006A0 RID: 1696
		private InputKeyItemVM _resetInputKey;

		// Token: 0x040006A1 RID: 1697
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040006A2 RID: 1698
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040006A3 RID: 1699
		private InputKeyItemVM _previousCharacterInputKey;

		// Token: 0x040006A4 RID: 1700
		private InputKeyItemVM _nextCharacterInputKey;

		// Token: 0x040006A5 RID: 1701
		private InputKeyItemVM _buyAllInputKey;

		// Token: 0x040006A6 RID: 1702
		private InputKeyItemVM _sellAllInputKey;

		// Token: 0x02000207 RID: 519
		public enum EquipmentModes
		{
			// Token: 0x040011BC RID: 4540
			Civilian,
			// Token: 0x040011BD RID: 4541
			Battle,
			// Token: 0x040011BE RID: 4542
			Stealth
		}

		// Token: 0x02000208 RID: 520
		public enum Filters
		{
			// Token: 0x040011C0 RID: 4544
			All,
			// Token: 0x040011C1 RID: 4545
			Weapons,
			// Token: 0x040011C2 RID: 4546
			ShieldsAndRanged,
			// Token: 0x040011C3 RID: 4547
			Armors,
			// Token: 0x040011C4 RID: 4548
			Mounts,
			// Token: 0x040011C5 RID: 4549
			Miscellaneous
		}

		// Token: 0x02000209 RID: 521
		private class RosterElementComparer : IComparer<SPItemVM>
		{
			// Token: 0x06002547 RID: 9543 RVA: 0x00081730 File Offset: 0x0007F930
			public RosterElementComparer(InventoryCapacityModel inventoryCapacityModel, MobileParty currentParty, bool basedOnGoldAmount)
			{
				this._inventoryCapacityModel = inventoryCapacityModel;
				this._currentParty = currentParty;
				this._basedOnGoldAmount = basedOnGoldAmount;
			}

			// Token: 0x06002548 RID: 9544 RVA: 0x00081750 File Offset: 0x0007F950
			public int Compare(SPItemVM x, SPItemVM y)
			{
				EquipmentElement equipmentElement = x.ItemRosterElement.EquipmentElement;
				EquipmentElement equipmentElement2 = y.ItemRosterElement.EquipmentElement;
				if (this._currentParty != null)
				{
					TextObject textObject;
					TextObject textObject2;
					int num = this._inventoryCapacityModel.GetItemEffectiveWeight(equipmentElement, this._currentParty, this._currentParty.IsCurrentlyAtSea, out textObject).CompareTo(this._inventoryCapacityModel.GetItemEffectiveWeight(equipmentElement2, this._currentParty, this._currentParty.IsCurrentlyAtSea, out textObject2));
					if (num != 0)
					{
						return num;
					}
					return x.ItemCost.CompareTo(y.ItemCost);
				}
				else
				{
					if (this._basedOnGoldAmount)
					{
						return x.ItemCost.CompareTo(y.ItemCost);
					}
					return 0;
				}
			}

			// Token: 0x040011C6 RID: 4550
			private readonly MobileParty _currentParty;

			// Token: 0x040011C7 RID: 4551
			private readonly InventoryCapacityModel _inventoryCapacityModel;

			// Token: 0x040011C8 RID: 4552
			private readonly bool _basedOnGoldAmount;
		}
	}
}
