using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000DE RID: 222
	public class InventoryLogic
	{
		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x000625F8 File Offset: 0x000607F8
		// (set) Token: 0x06001515 RID: 5397 RVA: 0x00062600 File Offset: 0x00060800
		public bool DisableNetwork { get; set; }

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x00062609 File Offset: 0x00060809
		// (set) Token: 0x06001517 RID: 5399 RVA: 0x00062611 File Offset: 0x00060811
		public Action<int> TotalAmountChange { get; set; }

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x0006261A File Offset: 0x0006081A
		// (set) Token: 0x06001519 RID: 5401 RVA: 0x00062622 File Offset: 0x00060822
		public Action DonationXpChange { get; set; }

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600151A RID: 5402 RVA: 0x0006262C File Offset: 0x0006082C
		// (remove) Token: 0x0600151B RID: 5403 RVA: 0x00062664 File Offset: 0x00060864
		public event InventoryLogic.AfterResetDelegate AfterReset;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600151C RID: 5404 RVA: 0x0006269C File Offset: 0x0006089C
		// (remove) Token: 0x0600151D RID: 5405 RVA: 0x000626D4 File Offset: 0x000608D4
		public event InventoryLogic.ProcessResultListDelegate AfterTransfer;

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x0600151E RID: 5406 RVA: 0x00062709 File Offset: 0x00060909
		// (set) Token: 0x0600151F RID: 5407 RVA: 0x00062711 File Offset: 0x00060911
		public TroopRoster RightMemberRoster { get; private set; }

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x0006271A File Offset: 0x0006091A
		// (set) Token: 0x06001521 RID: 5409 RVA: 0x00062722 File Offset: 0x00060922
		public TroopRoster LeftMemberRoster { get; private set; }

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001522 RID: 5410 RVA: 0x0006272B File Offset: 0x0006092B
		// (set) Token: 0x06001523 RID: 5411 RVA: 0x00062733 File Offset: 0x00060933
		public CharacterObject InitialEquipmentCharacter { get; private set; }

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001524 RID: 5412 RVA: 0x0006273C File Offset: 0x0006093C
		// (set) Token: 0x06001525 RID: 5413 RVA: 0x00062744 File Offset: 0x00060944
		public bool IsTrading { get; private set; }

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x0006274D File Offset: 0x0006094D
		// (set) Token: 0x06001527 RID: 5415 RVA: 0x00062755 File Offset: 0x00060955
		public bool IsSpecialActionsPermitted { get; private set; }

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001528 RID: 5416 RVA: 0x0006275E File Offset: 0x0006095E
		// (set) Token: 0x06001529 RID: 5417 RVA: 0x00062766 File Offset: 0x00060966
		public CharacterObject OwnerCharacter { get; private set; }

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x0006276F File Offset: 0x0006096F
		// (set) Token: 0x0600152B RID: 5419 RVA: 0x00062777 File Offset: 0x00060977
		public MobileParty OwnerParty { get; private set; }

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x00062780 File Offset: 0x00060980
		// (set) Token: 0x0600152D RID: 5421 RVA: 0x00062788 File Offset: 0x00060988
		public PartyBase OtherParty { get; private set; }

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x00062791 File Offset: 0x00060991
		// (set) Token: 0x0600152F RID: 5423 RVA: 0x00062799 File Offset: 0x00060999
		public IMarketData MarketData { get; private set; }

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x000627A2 File Offset: 0x000609A2
		// (set) Token: 0x06001531 RID: 5425 RVA: 0x000627AA File Offset: 0x000609AA
		public InventoryLogic.CapacityData OtherSideCapacityData { get; private set; }

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x000627B4 File Offset: 0x000609B4
		public int OtherSideCurrentWeight
		{
			get
			{
				float num = 0f;
				PartyBase otherParty = this.OtherParty;
				MobileParty mobileParty = ((otherParty != null) ? otherParty.MobileParty : null);
				if (mobileParty != null)
				{
					ItemRoster itemRoster = this._rosters[0];
					InventoryCapacityModel inventoryCapacityModel = Campaign.Current.Models.InventoryCapacityModel;
					for (int i = 0; i < itemRoster.Count; i++)
					{
						TextObject textObject;
						num += inventoryCapacityModel.GetItemEffectiveWeight(itemRoster[i].EquipmentElement, mobileParty, mobileParty.IsCurrentlyAtSea, out textObject) * (float)itemRoster[i].Amount;
					}
				}
				else if (this._inventoryMode == InventoryScreenHelper.InventoryMode.Warehouse && this._workshopWarehouseBehavior != null)
				{
					num = this._workshopWarehouseBehavior.GetWarehouseItemRosterWeight(MobileParty.MainParty.CurrentSettlement);
				}
				return MathF.Ceiling(num);
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x00062870 File Offset: 0x00060A70
		// (set) Token: 0x06001534 RID: 5428 RVA: 0x00062878 File Offset: 0x00060A78
		public TextObject LeftRosterName { get; private set; }

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x00062881 File Offset: 0x00060A81
		// (set) Token: 0x06001536 RID: 5430 RVA: 0x00062889 File Offset: 0x00060A89
		public bool CanGainXpFromDiscarding { get; private set; }

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001537 RID: 5431 RVA: 0x00062892 File Offset: 0x00060A92
		// (set) Token: 0x06001538 RID: 5432 RVA: 0x0006289A File Offset: 0x00060A9A
		public bool IsOtherPartyFromPlayerClan { get; private set; }

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001539 RID: 5433 RVA: 0x000628A3 File Offset: 0x00060AA3
		// (set) Token: 0x0600153A RID: 5434 RVA: 0x000628AB File Offset: 0x00060AAB
		public InventoryListener InventoryListener { get; private set; }

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x0600153B RID: 5435 RVA: 0x000628B4 File Offset: 0x00060AB4
		public int TotalAmount
		{
			get
			{
				return this.TransactionDebt;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x000628BC File Offset: 0x00060ABC
		public PartyBase OppositePartyFromListener
		{
			get
			{
				return this.InventoryListener.GetOppositeParty();
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x0600153D RID: 5437 RVA: 0x000628C9 File Offset: 0x00060AC9
		public SettlementComponent CurrentSettlementComponent
		{
			get
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				if (currentSettlement == null)
				{
					return null;
				}
				return currentSettlement.SettlementComponent;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x000628DC File Offset: 0x00060ADC
		public MobileParty CurrentMobileParty
		{
			get
			{
				if (PlayerEncounter.Current != null)
				{
					return PlayerEncounter.EncounteredParty.MobileParty;
				}
				MapEvent mapEvent = PartyBase.MainParty.MapEvent;
				bool flag;
				if (mapEvent == null)
				{
					flag = null != null;
				}
				else
				{
					PartyBase leaderParty = mapEvent.GetLeaderParty(PartyBase.MainParty.OpponentSide);
					flag = ((leaderParty != null) ? leaderParty.MobileParty : null) != null;
				}
				if (flag)
				{
					return PartyBase.MainParty.MapEvent.GetLeaderParty(PartyBase.MainParty.OpponentSide).MobileParty;
				}
				return null;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x0600153F RID: 5439 RVA: 0x00062949 File Offset: 0x00060B49
		// (set) Token: 0x06001540 RID: 5440 RVA: 0x00062951 File Offset: 0x00060B51
		public int TransactionDebt
		{
			get
			{
				return this._transactionDebt;
			}
			private set
			{
				if (value != this._transactionDebt)
				{
					this._transactionDebt = value;
					this.TotalAmountChange(this._transactionDebt);
				}
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x00062974 File Offset: 0x00060B74
		// (set) Token: 0x06001542 RID: 5442 RVA: 0x0006297C File Offset: 0x00060B7C
		public float XpGainFromDonations
		{
			get
			{
				return this._xpGainFromDonations;
			}
			private set
			{
				if (value != this._xpGainFromDonations)
				{
					this._xpGainFromDonations = value;
					if (this._xpGainFromDonations < 0f)
					{
						this._xpGainFromDonations = 0f;
					}
					Action donationXpChange = this.DonationXpChange;
					if (donationXpChange == null)
					{
						return;
					}
					donationXpChange();
				}
			}
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x000629B8 File Offset: 0x00060BB8
		public InventoryLogic(MobileParty ownerParty, CharacterObject ownerCharacter, PartyBase merchantParty)
		{
			this._rosters = new ItemRoster[2];
			this._rostersBackup = new ItemRoster[2];
			this.OwnerParty = ownerParty;
			this.OwnerCharacter = ownerCharacter;
			this.OtherParty = merchantParty;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00062A15 File Offset: 0x00060C15
		public InventoryLogic(PartyBase merchantParty)
			: this(MobileParty.MainParty, CharacterObject.PlayerCharacter, merchantParty)
		{
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00062A28 File Offset: 0x00060C28
		public void Initialize(ItemRoster leftItemRoster, MobileParty party, bool isTrading, bool isSpecialActionsPermitted, CharacterObject initialCharacterOfRightRoster, InventoryScreenHelper.InventoryCategoryType merchantItemType, IMarketData marketData, bool useBasePrices, InventoryScreenHelper.InventoryMode inventoryMode, TextObject leftRosterName = null, TroopRoster leftMemberRoster = null, InventoryLogic.CapacityData otherSideCapacityData = null)
		{
			this.Initialize(leftItemRoster, party.ItemRoster, party.MemberRoster, isTrading, isSpecialActionsPermitted, initialCharacterOfRightRoster, merchantItemType, marketData, useBasePrices, inventoryMode, leftRosterName, leftMemberRoster, otherSideCapacityData);
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x00062A5C File Offset: 0x00060C5C
		public void Initialize(ItemRoster leftItemRoster, ItemRoster rightItemRoster, TroopRoster rightMemberRoster, bool isTrading, bool isSpecialActionsPermitted, CharacterObject initialCharacterOfRightRoster, InventoryScreenHelper.InventoryCategoryType merchantItemType, IMarketData marketData, bool useBasePrices, InventoryScreenHelper.InventoryMode inventoryMode, TextObject leftRosterName = null, TroopRoster leftMemberRoster = null, InventoryLogic.CapacityData otherSideCapacityData = null)
		{
			this.OtherSideCapacityData = otherSideCapacityData;
			this.MarketData = marketData;
			this.TransactionDebt = 0;
			this.MerchantItemType = merchantItemType;
			this.InventoryListener = new FakeInventoryListener();
			this._useBasePrices = useBasePrices;
			this.LeftRosterName = leftRosterName;
			this.IsTrading = isTrading;
			this.IsSpecialActionsPermitted = isSpecialActionsPermitted;
			this._inventoryMode = inventoryMode;
			PartyBase otherParty = this.OtherParty;
			Clan clan;
			if (otherParty == null)
			{
				clan = null;
			}
			else
			{
				MobileParty mobileParty = otherParty.MobileParty;
				clan = ((mobileParty != null) ? mobileParty.ActualClan : null);
			}
			this.IsOtherPartyFromPlayerClan = clan == Hero.MainHero.Clan;
			this.InitializeRosters(leftItemRoster, rightItemRoster, rightMemberRoster, initialCharacterOfRightRoster, leftMemberRoster);
			this._transactionHistory.Clear();
			this.InitializeCategoryAverages();
			this.CanGainXpFromDiscarding = this._inventoryMode == InventoryScreenHelper.InventoryMode.Loot || (this._inventoryMode == InventoryScreenHelper.InventoryMode.Default && this.OtherParty == null);
			this.InitializeXpGainFromDonations();
			if (this._inventoryMode == InventoryScreenHelper.InventoryMode.Warehouse)
			{
				this._workshopWarehouseBehavior = Campaign.Current.GetCampaignBehavior<IWorkshopWarehouseCampaignBehavior>();
			}
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x00062B4E File Offset: 0x00060D4E
		private void InitializeRosters(ItemRoster leftItemRoster, ItemRoster rightItemRoster, TroopRoster rightMemberRoster, CharacterObject initialCharacterOfRightRoster, TroopRoster leftMemberRoster = null)
		{
			this._rosters[0] = leftItemRoster;
			this._rosters[1] = rightItemRoster;
			this.RightMemberRoster = rightMemberRoster;
			this.LeftMemberRoster = leftMemberRoster;
			this.InitialEquipmentCharacter = initialCharacterOfRightRoster;
			this.SetCurrentStateAsInitial();
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x00062B7F File Offset: 0x00060D7F
		public int GetItemTotalPrice(ItemRosterElement itemRosterElement, int absStockChange, out int lastPrice, bool isBuying)
		{
			lastPrice = this.GetItemPrice(itemRosterElement.EquipmentElement, isBuying);
			return lastPrice;
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x00062B94 File Offset: 0x00060D94
		public void SetPlayerAcceptTraderOffer()
		{
			this._playerAcceptsTraderOffer = true;
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x00062BA0 File Offset: 0x00060DA0
		public bool DoneLogic()
		{
			if (this.IsPreviewingItem)
			{
				return false;
			}
			SettlementComponent currentSettlementComponent = this.CurrentSettlementComponent;
			MobileParty currentMobileParty = this.CurrentMobileParty;
			PartyBase partyBase = null;
			if (currentMobileParty != null)
			{
				partyBase = currentMobileParty.Party;
			}
			else if (currentSettlementComponent != null)
			{
				partyBase = currentSettlementComponent.Owner;
			}
			if (!this._playerAcceptsTraderOffer)
			{
				InventoryListener inventoryListener = this.InventoryListener;
				int? num = ((inventoryListener != null) ? new int?(inventoryListener.GetGold()) : null) + this.TotalAmount;
				int num2 = 0;
				bool flag = (num.GetValueOrDefault() < num2) & (num != null);
			}
			if (this.InventoryListener != null && this.IsTrading && this.OwnerCharacter.HeroObject.Gold - this.TotalAmount < 0)
			{
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_warning_you_dont_have_enough_money", null), 0, null, null, "");
				return false;
			}
			if (this._playerAcceptsTraderOffer)
			{
				this._playerAcceptsTraderOffer = false;
				if (this.InventoryListener != null)
				{
					int gold = this.InventoryListener.GetGold();
					this.TransactionDebt = -gold;
				}
			}
			if (this.OwnerCharacter != null && this.OwnerCharacter.HeroObject != null && this.IsTrading)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, this.OwnerCharacter.HeroObject, MathF.Min(-this.TotalAmount, this.InventoryListener.GetGold()), false);
				if (currentSettlementComponent != null && currentSettlementComponent.IsTown && this.OwnerCharacter.GetPerkValue(DefaultPerks.Trade.TrickleDown))
				{
					int num3 = 0;
					List<ValueTuple<ItemRosterElement, int>> boughtItems = this._transactionHistory.GetBoughtItems();
					int num4 = 0;
					while (boughtItems != null && num4 < boughtItems.Count)
					{
						ItemObject item = boughtItems[num4].Item1.EquipmentElement.Item;
						if (item != null && item.IsTradeGood)
						{
							num3 += boughtItems[num4].Item2;
						}
						num4++;
					}
					if (num3 >= 10000)
					{
						for (int i = 0; i < currentSettlementComponent.Settlement.Notables.Count; i++)
						{
							if (currentSettlementComponent.Settlement.Notables[i].IsMerchant)
							{
								int num5 = MathF.Floor(DefaultPerks.Trade.TrickleDown.PrimaryBonus);
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(currentSettlementComponent.Settlement.Notables[i], this.OwnerCharacter.HeroObject, num5, true);
							}
						}
					}
				}
			}
			if (this.CanGainXpFromDiscarding)
			{
				CampaignEventDispatcher.Instance.OnItemsDiscardedByPlayer(this._rosters[0]);
			}
			CampaignEventDispatcher.Instance.OnPlayerInventoryExchange(this._transactionHistory.GetBoughtItems(), this._transactionHistory.GetSoldItems(), this.IsTrading);
			if (currentSettlementComponent != null && this.InventoryListener != null && this.IsTrading)
			{
				this.InventoryListener.SetGold(this.InventoryListener.GetGold() + this.TotalAmount);
			}
			else if (((currentMobileParty != null) ? currentMobileParty.Party.LeaderHero : null) != null && this.IsTrading)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, this.CurrentMobileParty.Party.LeaderHero, this.TotalAmount, false);
				if (this.CurrentMobileParty.Party.LeaderHero.CompanionOf != null)
				{
					this.CurrentMobileParty.AddTaxGold((int)((float)this.TotalAmount * 0.1f));
				}
			}
			else if (partyBase != null && partyBase.LeaderHero == null && this.IsTrading)
			{
				GiveGoldAction.ApplyForCharacterToParty(null, partyBase, this.TotalAmount, false);
			}
			this._partyInitialEquipment = new InventoryLogic.PartyEquipment(this.OwnerParty);
			if (this.IsOtherPartyFromPlayerClan && this.LeftMemberRoster != null)
			{
				this._otherPartyInitialEquipment = new InventoryLogic.PartyEquipment(this.OtherParty.MobileParty);
			}
			return true;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x00062F4B File Offset: 0x0006114B
		public List<ValueTuple<ItemRosterElement, int>> GetBoughtItems()
		{
			return this._transactionHistory.GetBoughtItems();
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00062F58 File Offset: 0x00061158
		public List<ValueTuple<ItemRosterElement, int>> GetSoldItems()
		{
			return this._transactionHistory.GetSoldItems();
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x00062F65 File Offset: 0x00061165
		public bool CanInventoryCapacityIncrease(InventoryLogic.InventorySide side)
		{
			return this._inventoryMode != InventoryScreenHelper.InventoryMode.Warehouse || side > InventoryLogic.InventorySide.OtherInventory;
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x00062F76 File Offset: 0x00061176
		public bool GetCanItemIncreaseInventoryCapacity(ItemObject item)
		{
			return item.HasHorseComponent;
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x00062F80 File Offset: 0x00061180
		private void InitializeCategoryAverages()
		{
			if (Campaign.Current != null && Settlement.CurrentSettlement != null)
			{
				Town town = (Settlement.CurrentSettlement.IsVillage ? Settlement.CurrentSettlement.Village.Bound.Town : Settlement.CurrentSettlement.Town);
				foreach (ItemCategory itemCategory in ItemCategories.All)
				{
					float num = 0f;
					for (int i = 0; i < Town.AllTowns.Count; i++)
					{
						if (Town.AllTowns[i] != town)
						{
							num += Town.AllTowns[i].MarketData.GetPriceFactor(itemCategory);
						}
					}
					float num2 = num / (float)(Town.AllTowns.Count - 1);
					this._itemCategoryAverages.Add(itemCategory, num2);
					Debug.Print(string.Format("Average value of {0} : {1}", itemCategory.GetName(), num2), 0, Debug.DebugColor.White, 17592186044416UL);
				}
			}
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x000630A4 File Offset: 0x000612A4
		private void InitializeXpGainFromDonations()
		{
			this.XpGainFromDonations = 0f;
			bool flag = PerkHelper.PlayerHasAnyItemDonationPerk();
			bool flag2 = this._inventoryMode == InventoryScreenHelper.InventoryMode.Loot;
			if (flag && flag2)
			{
				this.XpGainFromDonations = (float)Campaign.Current.Models.ItemDiscardModel.GetXpBonusForDiscardingItems(this._rosters[0]);
			}
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x000630F4 File Offset: 0x000612F4
		private void HandleDonationOnTransferItem(ItemRosterElement rosterElement, int amount, bool isBuying, bool isSelling)
		{
			ItemObject item = rosterElement.EquipmentElement.Item;
			ItemDiscardModel itemDiscardModel = Campaign.Current.Models.ItemDiscardModel;
			if (this.CanGainXpFromDiscarding && (isSelling || isBuying) && item != null)
			{
				this.XpGainFromDonations += (float)(itemDiscardModel.GetXpBonusForDiscardingItem(item, amount) * (isSelling ? 1 : (-1)));
			}
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00063151 File Offset: 0x00061351
		public float GetAveragePriceFactorItemCategory(ItemCategory category)
		{
			if (this._itemCategoryAverages.ContainsKey(category))
			{
				return this._itemCategoryAverages[category];
			}
			return -99f;
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00063174 File Offset: 0x00061374
		public bool IsThereAnyChanges()
		{
			if (this.IsThereAnyChangeBetweenRosters(this._rosters[1], this._rostersBackup[1]) || !this._partyInitialEquipment.IsEqual(new InventoryLogic.PartyEquipment(this.OwnerParty)))
			{
				return true;
			}
			InventoryLogic.PartyEquipment otherPartyInitialEquipment = this._otherPartyInitialEquipment;
			if (otherPartyInitialEquipment == null)
			{
				return false;
			}
			PartyBase otherParty = this.OtherParty;
			return !otherPartyInitialEquipment.IsEqual(new InventoryLogic.PartyEquipment((otherParty != null) ? otherParty.MobileParty : null));
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x000631E0 File Offset: 0x000613E0
		private bool IsThereAnyChangeBetweenRosters(ItemRoster roster1, ItemRoster roster2)
		{
			if (roster1.Count != roster2.Count)
			{
				return true;
			}
			using (IEnumerator<ItemRosterElement> enumerator = roster1.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemRosterElement item = enumerator.Current;
					if (!roster2.Any<ItemRosterElement>((ItemRosterElement e) => e.IsEqualTo(item)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00063258 File Offset: 0x00061458
		public void Reset(bool fromCancel)
		{
			this.ResetLogic(fromCancel);
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00063264 File Offset: 0x00061464
		private void ResetLogic(bool fromCancel)
		{
			Debug.Print("InventoryLogic::Reset", 0, Debug.DebugColor.White, 17592186044416UL);
			for (int i = 0; i < 2; i++)
			{
				this._rosters[i].Clear();
				this._rosters[i].Add(this._rostersBackup[i]);
			}
			this.TransactionDebt = 0;
			this._transactionHistory.Clear();
			this.InitializeXpGainFromDonations();
			this._partyInitialEquipment.ResetEquipment();
			InventoryLogic.PartyEquipment otherPartyInitialEquipment = this._otherPartyInitialEquipment;
			if (otherPartyInitialEquipment != null)
			{
				otherPartyInitialEquipment.ResetEquipment();
			}
			InventoryLogic.AfterResetDelegate afterReset = this.AfterReset;
			if (afterReset != null)
			{
				afterReset(this, fromCancel);
			}
			List<TransferCommandResult> list = new List<TransferCommandResult>();
			if (!fromCancel)
			{
				this.OnAfterTransfer(list);
			}
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0006330C File Offset: 0x0006150C
		public bool CanPlayerCompleteTransaction()
		{
			InventoryLogic.CapacityData otherSideCapacityData = this.OtherSideCapacityData;
			int num = ((otherSideCapacityData != null) ? otherSideCapacityData.GetCapacity() : (-1));
			return (num == -1 || this.OtherSideCurrentWeight <= num || this.OtherSideCapacityData.CanForceTransaction()) && (!this.IsPreviewingItem || !this.IsTrading || this.TotalAmount <= 0 || (this.TotalAmount >= 0 && this.OwnerCharacter.HeroObject.Gold - this.TotalAmount >= 0));
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0006338C File Offset: 0x0006158C
		public bool CanSlaughterItem(ItemRosterElement element, InventoryLogic.InventorySide sideOfItem)
		{
			return (!this.IsTrading || this._transactionHistory.IsEmpty) && (this.IsSpecialActionsPermitted && this.IsSlaughterable(element.EquipmentElement.Item) && sideOfItem == InventoryLogic.InventorySide.PlayerInventory && element.Amount > 0) && !this._transactionHistory.GetBoughtItems().Any<ValueTuple<ItemRosterElement, int>>((ValueTuple<ItemRosterElement, int> i) => i.Item1.EquipmentElement.Item == element.EquipmentElement.Item);
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00063413 File Offset: 0x00061613
		public bool IsSlaughterable(ItemObject item)
		{
			return item.Type == ItemObject.ItemTypeEnum.Animal || item.Type == ItemObject.ItemTypeEnum.Horse;
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0006342C File Offset: 0x0006162C
		public bool CanDonateItem(ItemRosterElement element, InventoryLogic.InventorySide sideOfItem)
		{
			return Game.Current.IsDevelopmentMode && this.IsSpecialActionsPermitted && element.Amount > 0 && this.IsDonatable(element.EquipmentElement.Item) && sideOfItem == InventoryLogic.InventorySide.PlayerInventory;
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x00063474 File Offset: 0x00061674
		public bool IsDonatable(ItemObject item)
		{
			return item.Type == ItemObject.ItemTypeEnum.Arrows || item.Type == ItemObject.ItemTypeEnum.BodyArmor || item.Type == ItemObject.ItemTypeEnum.Bolts || item.Type == ItemObject.ItemTypeEnum.SlingStones || item.Type == ItemObject.ItemTypeEnum.Bow || item.Type == ItemObject.ItemTypeEnum.Bullets || item.Type == ItemObject.ItemTypeEnum.Cape || item.Type == ItemObject.ItemTypeEnum.ChestArmor || item.Type == ItemObject.ItemTypeEnum.Crossbow || item.Type == ItemObject.ItemTypeEnum.Sling || item.Type == ItemObject.ItemTypeEnum.HandArmor || item.Type == ItemObject.ItemTypeEnum.HeadArmor || item.Type == ItemObject.ItemTypeEnum.HorseHarness || item.Type == ItemObject.ItemTypeEnum.LegArmor || item.Type == ItemObject.ItemTypeEnum.Musket || item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon || item.Type == ItemObject.ItemTypeEnum.Pistol || item.Type == ItemObject.ItemTypeEnum.Polearm || item.Type == ItemObject.ItemTypeEnum.Shield || item.Type == ItemObject.ItemTypeEnum.Thrown || item.Type == ItemObject.ItemTypeEnum.TwoHandedWeapon;
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x00063563 File Offset: 0x00061763
		public void SetInventoryListener(InventoryListener inventoryListener)
		{
			this.InventoryListener = inventoryListener;
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x0006356C File Offset: 0x0006176C
		public int GetItemPrice(EquipmentElement equipmentElement, bool isBuying = false)
		{
			bool flag = !isBuying;
			bool flag2 = false;
			int num = 0;
			int num2;
			bool flag3;
			if (this._transactionHistory.GetLastTransfer(equipmentElement, out num2, out flag3) && flag3 != flag)
			{
				flag2 = true;
				num = num2;
			}
			if (this._useBasePrices)
			{
				return equipmentElement.GetBaseValue();
			}
			if (flag2)
			{
				return num;
			}
			return this.MarketData.GetPrice(equipmentElement, this.OwnerParty, flag, this.OtherParty);
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x000635CC File Offset: 0x000617CC
		public int GetCostOfItemRosterElement(ItemRosterElement itemRosterElement, InventoryLogic.InventorySide side)
		{
			bool flag = side == InventoryLogic.InventorySide.OtherInventory && this.IsTrading;
			return this.GetItemPrice(itemRosterElement.EquipmentElement, flag);
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x000635F4 File Offset: 0x000617F4
		private void OnAfterTransfer(List<TransferCommandResult> resultList)
		{
			InventoryLogic.ProcessResultListDelegate afterTransfer = this.AfterTransfer;
			if (afterTransfer != null)
			{
				afterTransfer(this, resultList);
			}
			foreach (TransferCommandResult transferCommandResult in resultList)
			{
				if (transferCommandResult.EffectedNumber > 0)
				{
					Game.Current.EventManager.TriggerEvent<InventoryTransferItemEvent>(new InventoryTransferItemEvent(transferCommandResult.EffectedItemRosterElement.EquipmentElement.Item, transferCommandResult.ResultSide == InventoryLogic.InventorySide.PlayerInventory));
				}
			}
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0006368C File Offset: 0x0006188C
		public void AddTransferCommand(TransferCommand command)
		{
			this.ProcessTransferCommand(command);
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x00063698 File Offset: 0x00061898
		public void AddTransferCommands(IEnumerable<TransferCommand> commands)
		{
			foreach (TransferCommand transferCommand in commands)
			{
				this.ProcessTransferCommand(transferCommand);
			}
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x000636E0 File Offset: 0x000618E0
		public bool CheckItemRosterHasElement(InventoryLogic.InventorySide side, ItemRosterElement rosterElement, int number)
		{
			int num = this._rosters[(int)side].FindIndexOfElement(rosterElement.EquipmentElement);
			return num != -1 && this._rosters[(int)side].GetElementCopyAtIndex(num).Amount >= number;
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00063724 File Offset: 0x00061924
		private void ProcessTransferCommand(TransferCommand command)
		{
			List<TransferCommandResult> list = this.TransferItem(ref command);
			this.OnAfterTransfer(list);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00063744 File Offset: 0x00061944
		private List<TransferCommandResult> TransferItem(ref TransferCommand transferCommand)
		{
			List<TransferCommandResult> list = new List<TransferCommandResult>();
			string text = "TransferItem Name: {0} | From: {1} To: {2} | Amount: {3}";
			object[] array = new object[4];
			int num = 0;
			ItemObject item = transferCommand.ElementToTransfer.EquipmentElement.Item;
			array[num] = ((item != null) ? item.Name.ToString() : null) ?? "null";
			array[1] = transferCommand.FromSide;
			array[2] = transferCommand.ToSide;
			array[3] = transferCommand.Amount;
			Debug.Print(string.Format(text, array), 0, Debug.DebugColor.White, 17592186044416UL);
			if (transferCommand.ElementToTransfer.EquipmentElement.Item != null && InventoryLogic.TransferIsMovementValid(ref transferCommand) && this.DoesTransferItemExist(ref transferCommand))
			{
				int num2 = 0;
				bool flag = false;
				if (!InventoryLogic.IsEquipmentSide(transferCommand.FromSide) && transferCommand.FromSide != InventoryLogic.InventorySide.None)
				{
					int num3 = this._rosters[(int)transferCommand.FromSide].FindIndexOfElement(transferCommand.ElementToTransfer.EquipmentElement);
					ItemRosterElement elementCopyAtIndex = this._rosters[(int)transferCommand.FromSide].GetElementCopyAtIndex(num3);
					flag = transferCommand.Amount == elementCopyAtIndex.Amount;
				}
				bool flag2 = this.IsSell(transferCommand.FromSide, transferCommand.ToSide);
				bool flag3 = this.IsBuy(transferCommand.FromSide, transferCommand.ToSide);
				for (int i = 0; i < transferCommand.Amount; i++)
				{
					if (InventoryLogic.IsEquipmentSide(transferCommand.ToSide) && transferCommand.ToSideEquipment[(int)transferCommand.ToEquipmentIndex].Item != null)
					{
						TransferCommand transferCommand2 = TransferCommand.Transfer(1, transferCommand.ToSide, InventoryLogic.InventorySide.PlayerInventory, new ItemRosterElement(transferCommand.ToSideEquipment[(int)transferCommand.ToEquipmentIndex], 1), transferCommand.ToEquipmentIndex, EquipmentIndex.None, transferCommand.Character);
						list.AddRange(this.TransferItem(ref transferCommand2));
					}
					EquipmentElement equipmentElement = transferCommand.ElementToTransfer.EquipmentElement;
					int itemPrice = this.GetItemPrice(equipmentElement, flag3);
					if (flag3 || flag2)
					{
						this._transactionHistory.RecordTransaction(equipmentElement, flag2, itemPrice);
					}
					if (this.IsTrading)
					{
						if (flag3)
						{
							num2 += itemPrice;
						}
						else if (flag2)
						{
							num2 -= itemPrice;
						}
					}
					if (InventoryLogic.IsEquipmentSide(transferCommand.FromSide))
					{
						ItemRosterElement itemRosterElement = new ItemRosterElement(transferCommand.FromSideEquipment[(int)transferCommand.FromEquipmentIndex], transferCommand.Amount);
						itemRosterElement.Amount--;
						transferCommand.FromSideEquipment[(int)transferCommand.FromEquipmentIndex] = itemRosterElement.EquipmentElement;
					}
					else if (transferCommand.FromSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.FromSide == InventoryLogic.InventorySide.OtherInventory)
					{
						this._rosters[(int)transferCommand.FromSide].AddToCounts(transferCommand.ElementToTransfer.EquipmentElement, -1);
					}
					if (InventoryLogic.IsEquipmentSide(transferCommand.ToSide))
					{
						ItemRosterElement elementToTransfer = transferCommand.ElementToTransfer;
						elementToTransfer.Amount = 1;
						transferCommand.ToSideEquipment[(int)transferCommand.ToEquipmentIndex] = elementToTransfer.EquipmentElement;
					}
					else if (transferCommand.ToSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.ToSide == InventoryLogic.InventorySide.OtherInventory)
					{
						this._rosters[(int)transferCommand.ToSide].AddToCounts(transferCommand.ElementToTransfer.EquipmentElement, 1);
					}
				}
				if (InventoryLogic.IsEquipmentSide(transferCommand.FromSide))
				{
					ItemRosterElement itemRosterElement2 = new ItemRosterElement(transferCommand.FromSideEquipment[(int)transferCommand.FromEquipmentIndex], transferCommand.Amount);
					int amount = itemRosterElement2.Amount;
					itemRosterElement2.Amount = amount - 1;
					list.Add(new TransferCommandResult(transferCommand.FromSide, itemRosterElement2, -transferCommand.Amount, itemRosterElement2.Amount, transferCommand.FromEquipmentIndex, transferCommand.Character));
				}
				else if (transferCommand.FromSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.FromSide == InventoryLogic.InventorySide.OtherInventory)
				{
					if (flag)
					{
						list.Add(new TransferCommandResult(transferCommand.FromSide, new ItemRosterElement(transferCommand.ElementToTransfer.EquipmentElement, 0), -transferCommand.Amount, 0, transferCommand.FromEquipmentIndex, transferCommand.Character));
					}
					else
					{
						int num4 = this._rosters[(int)transferCommand.FromSide].FindIndexOfElement(transferCommand.ElementToTransfer.EquipmentElement);
						ItemRosterElement elementCopyAtIndex2 = this._rosters[(int)transferCommand.FromSide].GetElementCopyAtIndex(num4);
						list.Add(new TransferCommandResult(transferCommand.FromSide, elementCopyAtIndex2, -transferCommand.Amount, elementCopyAtIndex2.Amount, transferCommand.FromEquipmentIndex, transferCommand.Character));
					}
				}
				if (InventoryLogic.IsEquipmentSide(transferCommand.ToSide))
				{
					ItemRosterElement elementToTransfer2 = transferCommand.ElementToTransfer;
					elementToTransfer2.Amount = 1;
					list.Add(new TransferCommandResult(transferCommand.ToSide, elementToTransfer2, 1, 1, transferCommand.ToEquipmentIndex, transferCommand.Character));
				}
				else if (transferCommand.ToSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.ToSide == InventoryLogic.InventorySide.OtherInventory)
				{
					int num5 = this._rosters[(int)transferCommand.ToSide].FindIndexOfElement(transferCommand.ElementToTransfer.EquipmentElement);
					ItemRosterElement elementCopyAtIndex3 = this._rosters[(int)transferCommand.ToSide].GetElementCopyAtIndex(num5);
					list.Add(new TransferCommandResult(transferCommand.ToSide, elementCopyAtIndex3, transferCommand.Amount, elementCopyAtIndex3.Amount, transferCommand.ToEquipmentIndex, transferCommand.Character));
				}
				this.HandleDonationOnTransferItem(transferCommand.ElementToTransfer, transferCommand.Amount, flag3, flag2);
				this.TransactionDebt += num2;
			}
			return list;
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00063C5A File Offset: 0x00061E5A
		public static bool IsEquipmentSide(InventoryLogic.InventorySide side)
		{
			return side == InventoryLogic.InventorySide.CivilianEquipment || side == InventoryLogic.InventorySide.BattleEquipment || side == InventoryLogic.InventorySide.StealthEquipment;
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00063C6A File Offset: 0x00061E6A
		private bool IsSell(InventoryLogic.InventorySide fromSide, InventoryLogic.InventorySide toSide)
		{
			return toSide == InventoryLogic.InventorySide.OtherInventory && (InventoryLogic.IsEquipmentSide(fromSide) || fromSide == InventoryLogic.InventorySide.PlayerInventory);
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x00063C7F File Offset: 0x00061E7F
		private bool IsBuy(InventoryLogic.InventorySide fromSide, InventoryLogic.InventorySide toSide)
		{
			return fromSide == InventoryLogic.InventorySide.OtherInventory && (InventoryLogic.IsEquipmentSide(toSide) || toSide == InventoryLogic.InventorySide.PlayerInventory);
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x00063C94 File Offset: 0x00061E94
		public void SlaughterItem(ItemRosterElement itemRosterElement)
		{
			List<TransferCommandResult> list = new List<TransferCommandResult>();
			EquipmentElement equipmentElement = itemRosterElement.EquipmentElement;
			int meatCount = equipmentElement.Item.HorseComponent.MeatCount;
			int hideCount = equipmentElement.Item.HorseComponent.HideCount;
			int num = this._rosters[1].AddToCounts(DefaultItems.Meat, meatCount);
			ItemRosterElement elementCopyAtIndex = this._rosters[1].GetElementCopyAtIndex(num);
			bool flag = itemRosterElement.Amount == 1;
			int num2 = this._rosters[1].AddToCounts(itemRosterElement.EquipmentElement, -1);
			if (flag)
			{
				list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, new ItemRosterElement(equipmentElement, 0), -1, 0, EquipmentIndex.None, null));
			}
			else
			{
				ItemRosterElement elementCopyAtIndex2 = this._rosters[1].GetElementCopyAtIndex(num2);
				list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex2, -1, elementCopyAtIndex2.Amount, EquipmentIndex.None, null));
			}
			list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex, meatCount, elementCopyAtIndex.Amount, EquipmentIndex.None, null));
			if (hideCount > 0)
			{
				int num3 = this._rosters[1].AddToCounts(DefaultItems.Hides, hideCount);
				ItemRosterElement elementCopyAtIndex3 = this._rosters[1].GetElementCopyAtIndex(num3);
				list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex3, hideCount, elementCopyAtIndex3.Amount, EquipmentIndex.None, null));
			}
			this.SetCurrentStateAsInitial();
			this.OnAfterTransfer(list);
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x00063DC8 File Offset: 0x00061FC8
		public void DonateItem(ItemRosterElement itemRosterElement)
		{
			List<TransferCommandResult> list = new List<TransferCommandResult>();
			int tier = (int)itemRosterElement.EquipmentElement.Item.Tier;
			int num = 100 * (tier + 1);
			InventoryLogic.InventorySide inventorySide = InventoryLogic.InventorySide.PlayerInventory;
			int num2 = this._rosters[(int)inventorySide].AddToCounts(itemRosterElement.EquipmentElement, -1);
			ItemRosterElement elementCopyAtIndex = this._rosters[(int)inventorySide].GetElementCopyAtIndex(num2);
			list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex, -1, elementCopyAtIndex.Amount, EquipmentIndex.None, null));
			if (num > 0)
			{
				TroopRosterElement randomElementWithPredicate = PartyBase.MainParty.MemberRoster.GetTroopRoster().GetRandomElementWithPredicate<TroopRosterElement>((TroopRosterElement m) => !m.Character.IsHero && m.Character.UpgradeTargets.Length != 0);
				if (randomElementWithPredicate.Character != null)
				{
					PartyBase.MainParty.MemberRoster.AddXpToTroop(randomElementWithPredicate.Character, num);
					TextObject textObject = new TextObject("{=Kwja0a4s}Added {XPAMOUNT} amount of xp to {TROOPNAME}", null);
					textObject.SetTextVariable("XPAMOUNT", num);
					textObject.SetTextVariable("TROOPNAME", randomElementWithPredicate.Character.Name.ToString());
					Debug.Print(textObject.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
					MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
				}
			}
			this.SetCurrentStateAsInitial();
			this.OnAfterTransfer(list);
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x00063EFC File Offset: 0x000620FC
		private static bool TransferIsMovementValid(ref TransferCommand transferCommand)
		{
			if (transferCommand.ElementToTransfer.EquipmentElement.IsQuestItem)
			{
				BannerComponent bannerComponent = transferCommand.ElementToTransfer.EquipmentElement.Item.BannerComponent;
				if (((bannerComponent != null) ? bannerComponent.BannerEffect : null) == null || ((transferCommand.FromSide != InventoryLogic.InventorySide.PlayerInventory || !InventoryLogic.IsEquipmentSide(transferCommand.ToSide)) && (!InventoryLogic.IsEquipmentSide(transferCommand.FromSide) || transferCommand.ToSide != InventoryLogic.InventorySide.PlayerInventory)))
				{
					return false;
				}
			}
			bool flag = false;
			if (InventoryLogic.IsEquipmentSide(transferCommand.ToSide))
			{
				InventoryScreenHelper.InventoryItemType inventoryItemTypeOfItem = InventoryScreenHelper.GetInventoryItemTypeOfItem(transferCommand.ElementToTransfer.EquipmentElement.Item);
				switch (transferCommand.ToEquipmentIndex)
				{
				case EquipmentIndex.WeaponItemBeginSlot:
				case EquipmentIndex.Weapon1:
				case EquipmentIndex.Weapon2:
				case EquipmentIndex.Weapon3:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.Weapon || inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.Shield;
					break;
				case EquipmentIndex.ExtraWeaponSlot:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.Banner;
					break;
				case EquipmentIndex.NumAllWeaponSlots:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.HeadArmor;
					break;
				case EquipmentIndex.Body:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.BodyArmor;
					break;
				case EquipmentIndex.Leg:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.LegArmor;
					break;
				case EquipmentIndex.Gloves:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.HandArmor;
					break;
				case EquipmentIndex.Cape:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.Cape;
					break;
				case EquipmentIndex.ArmorItemEndSlot:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.Horse;
					break;
				case EquipmentIndex.HorseHarness:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.HorseHarness;
					break;
				}
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00064040 File Offset: 0x00062240
		private bool DoesTransferItemExist(ref TransferCommand transferCommand)
		{
			if (transferCommand.FromSide == InventoryLogic.InventorySide.OtherInventory || transferCommand.FromSide == InventoryLogic.InventorySide.PlayerInventory)
			{
				return this.CheckItemRosterHasElement(transferCommand.FromSide, transferCommand.ElementToTransfer, transferCommand.Amount);
			}
			return transferCommand.FromSide != InventoryLogic.InventorySide.None && transferCommand.FromSideEquipment[(int)transferCommand.FromEquipmentIndex].Item != null && transferCommand.ElementToTransfer.EquipmentElement.IsEqualTo(transferCommand.FromSideEquipment[(int)transferCommand.FromEquipmentIndex]);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x000640C6 File Offset: 0x000622C6
		public void TransferOne(ItemRosterElement itemRosterElement)
		{
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x000640C8 File Offset: 0x000622C8
		public int GetElementCountOnSide(InventoryLogic.InventorySide side)
		{
			return this._rosters[(int)side].Count;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x000640D7 File Offset: 0x000622D7
		public IReadOnlyList<ItemRosterElement> GetElementsInInitialRoster(InventoryLogic.InventorySide side)
		{
			return this._rostersBackup[(int)side];
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x000640E1 File Offset: 0x000622E1
		public IReadOnlyList<ItemRosterElement> GetElementsInRoster(InventoryLogic.InventorySide side)
		{
			return this._rosters[(int)side];
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x000640EC File Offset: 0x000622EC
		private void SetCurrentStateAsInitial()
		{
			for (int i = 0; i < this._rostersBackup.Length; i++)
			{
				this._rostersBackup[i] = new ItemRoster(this._rosters[i]);
			}
			this._partyInitialEquipment = new InventoryLogic.PartyEquipment(this.OwnerParty);
			if (this.IsOtherPartyFromPlayerClan && this.LeftMemberRoster != null)
			{
				this._otherPartyInitialEquipment = new InventoryLogic.PartyEquipment(this.OtherParty.MobileParty);
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x00064158 File Offset: 0x00062358
		public ItemRosterElement? FindItemFromSide(InventoryLogic.InventorySide side, EquipmentElement item)
		{
			int num = this._rosters[(int)side].FindIndexOfElement(item);
			if (num >= 0)
			{
				return new ItemRosterElement?(this._rosters[(int)side].ElementAt<ItemRosterElement>(num));
			}
			return null;
		}

		// Token: 0x040006F7 RID: 1783
		private ItemRoster[] _rosters;

		// Token: 0x040006F8 RID: 1784
		private ItemRoster[] _rostersBackup;

		// Token: 0x040006F9 RID: 1785
		private IWorkshopWarehouseCampaignBehavior _workshopWarehouseBehavior;

		// Token: 0x040006FF RID: 1791
		public bool IsPreviewingItem;

		// Token: 0x04000700 RID: 1792
		private InventoryLogic.PartyEquipment _partyInitialEquipment;

		// Token: 0x04000701 RID: 1793
		private InventoryLogic.PartyEquipment _otherPartyInitialEquipment;

		// Token: 0x04000707 RID: 1799
		private float _xpGainFromDonations;

		// Token: 0x04000708 RID: 1800
		private int _transactionDebt;

		// Token: 0x04000709 RID: 1801
		private bool _playerAcceptsTraderOffer;

		// Token: 0x0400070A RID: 1802
		private InventoryLogic.TransactionHistory _transactionHistory = new InventoryLogic.TransactionHistory();

		// Token: 0x0400070B RID: 1803
		private Dictionary<ItemCategory, float> _itemCategoryAverages = new Dictionary<ItemCategory, float>();

		// Token: 0x0400070C RID: 1804
		private bool _useBasePrices;

		// Token: 0x0400070D RID: 1805
		public InventoryScreenHelper.InventoryCategoryType MerchantItemType = InventoryScreenHelper.InventoryCategoryType.None;

		// Token: 0x0400070E RID: 1806
		private InventoryScreenHelper.InventoryMode _inventoryMode;

		// Token: 0x02000582 RID: 1410
		public enum TransferType
		{
			// Token: 0x0400180B RID: 6155
			Neutral,
			// Token: 0x0400180C RID: 6156
			Sell,
			// Token: 0x0400180D RID: 6157
			Buy
		}

		// Token: 0x02000583 RID: 1411
		public enum InventorySide
		{
			// Token: 0x0400180F RID: 6159
			OtherInventory,
			// Token: 0x04001810 RID: 6160
			PlayerInventory,
			// Token: 0x04001811 RID: 6161
			CivilianEquipment,
			// Token: 0x04001812 RID: 6162
			BattleEquipment,
			// Token: 0x04001813 RID: 6163
			StealthEquipment,
			// Token: 0x04001814 RID: 6164
			None = -1
		}

		// Token: 0x02000584 RID: 1412
		// (Invoke) Token: 0x06005097 RID: 20631
		public delegate void AfterResetDelegate(InventoryLogic inventoryLogic, bool fromCancel);

		// Token: 0x02000585 RID: 1413
		// (Invoke) Token: 0x0600509B RID: 20635
		public delegate void TotalAmountChangeDelegate(int newTotalAmount);

		// Token: 0x02000586 RID: 1414
		// (Invoke) Token: 0x0600509F RID: 20639
		public delegate void ProcessResultListDelegate(InventoryLogic inventoryLogic, List<TransferCommandResult> results);

		// Token: 0x02000587 RID: 1415
		private class PartyEquipment
		{
			// Token: 0x17000F79 RID: 3961
			// (get) Token: 0x060050A2 RID: 20642 RVA: 0x0018F6AC File Offset: 0x0018D8AC
			// (set) Token: 0x060050A3 RID: 20643 RVA: 0x0018F6B4 File Offset: 0x0018D8B4
			public Dictionary<CharacterObject, Equipment[]> CharacterEquipments { get; private set; }

			// Token: 0x060050A4 RID: 20644 RVA: 0x0018F6BD File Offset: 0x0018D8BD
			public PartyEquipment(MobileParty party)
			{
				this.CharacterEquipments = new Dictionary<CharacterObject, Equipment[]>();
				this.InitializeCopyFrom(party);
			}

			// Token: 0x060050A5 RID: 20645 RVA: 0x0018F6D8 File Offset: 0x0018D8D8
			public void InitializeCopyFrom(MobileParty party)
			{
				this.CharacterEquipments = new Dictionary<CharacterObject, Equipment[]>();
				for (int i = 0; i < party.MemberRoster.Count; i++)
				{
					CharacterObject character = party.MemberRoster.GetElementCopyAtIndex(i).Character;
					if (character.IsHero)
					{
						this.CharacterEquipments.Add(character, new Equipment[]
						{
							new Equipment(character.FirstBattleEquipment),
							new Equipment(character.FirstCivilianEquipment),
							new Equipment(character.FirstStealthEquipment)
						});
					}
				}
			}

			// Token: 0x060050A6 RID: 20646 RVA: 0x0018F75C File Offset: 0x0018D95C
			internal void ResetEquipment()
			{
				foreach (KeyValuePair<CharacterObject, Equipment[]> keyValuePair in this.CharacterEquipments)
				{
					foreach (Equipment equipment in keyValuePair.Value)
					{
						if (equipment.IsBattle)
						{
							keyValuePair.Key.FirstBattleEquipment.FillFrom(equipment, true);
						}
						else if (equipment.IsCivilian)
						{
							keyValuePair.Key.FirstCivilianEquipment.FillFrom(equipment, true);
						}
						else if (equipment.IsStealth)
						{
							keyValuePair.Key.FirstStealthEquipment.FillFrom(equipment, true);
						}
						else
						{
							Debug.FailedAssert("Equipment type cannot be found!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Inventory\\InventoryLogic.cs", "ResetEquipment", 1206);
						}
					}
				}
			}

			// Token: 0x060050A7 RID: 20647 RVA: 0x0018F844 File Offset: 0x0018DA44
			public void SetReference(InventoryLogic.PartyEquipment partyEquipment)
			{
				this.CharacterEquipments.Clear();
				this.CharacterEquipments = partyEquipment.CharacterEquipments;
			}

			// Token: 0x060050A8 RID: 20648 RVA: 0x0018F860 File Offset: 0x0018DA60
			public bool IsEqual(InventoryLogic.PartyEquipment partyEquipment)
			{
				if (partyEquipment.CharacterEquipments.Keys.Count != this.CharacterEquipments.Keys.Count)
				{
					return false;
				}
				foreach (CharacterObject characterObject in partyEquipment.CharacterEquipments.Keys)
				{
					if (!this.CharacterEquipments.Keys.Contains(characterObject))
					{
						return false;
					}
					Equipment[] array;
					if (!this.CharacterEquipments.TryGetValue(characterObject, out array))
					{
						return false;
					}
					Equipment[] array2;
					if (!partyEquipment.CharacterEquipments.TryGetValue(characterObject, out array2) || array2.Length != array.Length)
					{
						return false;
					}
					for (int i = 0; i < array.Length; i++)
					{
						if (!array[i].IsEquipmentEqualTo(array2[i]))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x02000588 RID: 1416
		private class ItemLog : IReadOnlyCollection<int>, IEnumerable<int>, IEnumerable
		{
			// Token: 0x17000F7A RID: 3962
			// (get) Token: 0x060050A9 RID: 20649 RVA: 0x0018F94C File Offset: 0x0018DB4C
			public bool IsSelling
			{
				get
				{
					return this._isSelling;
				}
			}

			// Token: 0x17000F7B RID: 3963
			// (get) Token: 0x060050AA RID: 20650 RVA: 0x0018F954 File Offset: 0x0018DB54
			public int Count
			{
				get
				{
					return ((IReadOnlyCollection<int>)this._transactions).Count;
				}
			}

			// Token: 0x060050AB RID: 20651 RVA: 0x0018F961 File Offset: 0x0018DB61
			private void AddTransaction(int price, bool isSelling)
			{
				if (this._transactions.IsEmpty<int>())
				{
					this._isSelling = isSelling;
				}
				this._transactions.Add(price);
			}

			// Token: 0x060050AC RID: 20652 RVA: 0x0018F984 File Offset: 0x0018DB84
			private void RemoveLastTransaction()
			{
				if (!this._transactions.IsEmpty<int>())
				{
					this._transactions.RemoveAt(this._transactions.Count - 1);
					return;
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Inventory\\InventoryLogic.cs", "RemoveLastTransaction", 1286);
			}

			// Token: 0x060050AD RID: 20653 RVA: 0x0018F9D0 File Offset: 0x0018DBD0
			public void RecordTransaction(int price, bool isSelling)
			{
				if (!this._transactions.IsEmpty<int>() && isSelling != this._isSelling)
				{
					this.RemoveLastTransaction();
					return;
				}
				this.AddTransaction(price, isSelling);
			}

			// Token: 0x060050AE RID: 20654 RVA: 0x0018F9F7 File Offset: 0x0018DBF7
			public bool GetLastTransaction(out int price, out bool isSelling)
			{
				if (this._transactions.IsEmpty<int>())
				{
					price = 0;
					isSelling = false;
					return false;
				}
				price = this._transactions[this._transactions.Count - 1];
				isSelling = this._isSelling;
				return true;
			}

			// Token: 0x060050AF RID: 20655 RVA: 0x0018FA31 File Offset: 0x0018DC31
			public IEnumerator<int> GetEnumerator()
			{
				return ((IEnumerable<int>)this._transactions).GetEnumerator();
			}

			// Token: 0x060050B0 RID: 20656 RVA: 0x0018FA3E File Offset: 0x0018DC3E
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<int>)this._transactions).GetEnumerator();
			}

			// Token: 0x04001816 RID: 6166
			private List<int> _transactions = new List<int>();

			// Token: 0x04001817 RID: 6167
			private bool _isSelling;
		}

		// Token: 0x02000589 RID: 1417
		public class CapacityData
		{
			// Token: 0x060050B2 RID: 20658 RVA: 0x0018FA5E File Offset: 0x0018DC5E
			public CapacityData(Func<int> getCapacity, Func<TextObject> getCapacityExceededWarningText, Func<TextObject> getCapacityExceededHintText, bool forceTransaction = false)
			{
				this._getCapacity = getCapacity;
				this._getCapacityExceededWarningText = getCapacityExceededWarningText;
				this._getCapacityExceededHintText = getCapacityExceededHintText;
				this._forceTransaction = forceTransaction;
			}

			// Token: 0x060050B3 RID: 20659 RVA: 0x0018FA83 File Offset: 0x0018DC83
			public int GetCapacity()
			{
				Func<int> getCapacity = this._getCapacity;
				if (getCapacity == null)
				{
					return -1;
				}
				return getCapacity();
			}

			// Token: 0x060050B4 RID: 20660 RVA: 0x0018FA96 File Offset: 0x0018DC96
			public bool CanForceTransaction()
			{
				return this._forceTransaction;
			}

			// Token: 0x060050B5 RID: 20661 RVA: 0x0018FA9E File Offset: 0x0018DC9E
			public TextObject GetCapacityExceededWarningText()
			{
				Func<TextObject> getCapacityExceededWarningText = this._getCapacityExceededWarningText;
				if (getCapacityExceededWarningText == null)
				{
					return null;
				}
				return getCapacityExceededWarningText();
			}

			// Token: 0x060050B6 RID: 20662 RVA: 0x0018FAB1 File Offset: 0x0018DCB1
			public TextObject GetCapacityExceededHintText()
			{
				Func<TextObject> getCapacityExceededHintText = this._getCapacityExceededHintText;
				if (getCapacityExceededHintText == null)
				{
					return null;
				}
				return getCapacityExceededHintText();
			}

			// Token: 0x04001818 RID: 6168
			private readonly Func<int> _getCapacity;

			// Token: 0x04001819 RID: 6169
			private readonly Func<TextObject> _getCapacityExceededWarningText;

			// Token: 0x0400181A RID: 6170
			private readonly Func<TextObject> _getCapacityExceededHintText;

			// Token: 0x0400181B RID: 6171
			private readonly bool _forceTransaction;
		}

		// Token: 0x0200058A RID: 1418
		private class TransactionHistory
		{
			// Token: 0x060050B7 RID: 20663 RVA: 0x0018FAC4 File Offset: 0x0018DCC4
			internal void RecordTransaction(EquipmentElement elementToTransfer, bool isSelling, int price)
			{
				InventoryLogic.ItemLog itemLog;
				if (!this._transactionLogs.TryGetValue(elementToTransfer, out itemLog))
				{
					itemLog = new InventoryLogic.ItemLog();
					this._transactionLogs[elementToTransfer] = itemLog;
				}
				itemLog.RecordTransaction(price, isSelling);
			}

			// Token: 0x17000F7C RID: 3964
			// (get) Token: 0x060050B8 RID: 20664 RVA: 0x0018FAFC File Offset: 0x0018DCFC
			public bool IsEmpty
			{
				get
				{
					return this._transactionLogs.IsEmpty<KeyValuePair<EquipmentElement, InventoryLogic.ItemLog>>();
				}
			}

			// Token: 0x060050B9 RID: 20665 RVA: 0x0018FB09 File Offset: 0x0018DD09
			public void Clear()
			{
				this._transactionLogs.Clear();
			}

			// Token: 0x060050BA RID: 20666 RVA: 0x0018FB18 File Offset: 0x0018DD18
			public bool GetLastTransfer(EquipmentElement equipmentElement, out int lastPrice, out bool lastIsSelling)
			{
				InventoryLogic.ItemLog itemLog;
				bool flag = this._transactionLogs.TryGetValue(equipmentElement, out itemLog);
				lastPrice = 0;
				lastIsSelling = false;
				return flag && itemLog.GetLastTransaction(out lastPrice, out lastIsSelling);
			}

			// Token: 0x060050BB RID: 20667 RVA: 0x0018FB48 File Offset: 0x0018DD48
			internal List<ValueTuple<ItemRosterElement, int>> GetTransferredItems(bool isSelling)
			{
				List<ValueTuple<ItemRosterElement, int>> list = new List<ValueTuple<ItemRosterElement, int>>();
				foreach (KeyValuePair<EquipmentElement, InventoryLogic.ItemLog> keyValuePair in this._transactionLogs)
				{
					if (keyValuePair.Value.Count > 0 && !keyValuePair.Value.IsSelling == isSelling)
					{
						int num = keyValuePair.Value.Sum();
						list.Add(new ValueTuple<ItemRosterElement, int>(new ItemRosterElement(keyValuePair.Key.Item, keyValuePair.Value.Count, keyValuePair.Key.ItemModifier), num));
					}
				}
				return list;
			}

			// Token: 0x060050BC RID: 20668 RVA: 0x0018FC08 File Offset: 0x0018DE08
			internal List<ValueTuple<ItemRosterElement, int>> GetBoughtItems()
			{
				return this.GetTransferredItems(true);
			}

			// Token: 0x060050BD RID: 20669 RVA: 0x0018FC11 File Offset: 0x0018DE11
			internal List<ValueTuple<ItemRosterElement, int>> GetSoldItems()
			{
				return this.GetTransferredItems(false);
			}

			// Token: 0x0400181C RID: 6172
			private Dictionary<EquipmentElement, InventoryLogic.ItemLog> _transactionLogs = new Dictionary<EquipmentElement, InventoryLogic.ItemLog>();
		}
	}
}
