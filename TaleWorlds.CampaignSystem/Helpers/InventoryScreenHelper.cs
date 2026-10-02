using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000026 RID: 38
	public static class InventoryScreenHelper
	{
		// Token: 0x0600014B RID: 331 RVA: 0x0000FEF0 File Offset: 0x0000E0F0
		public static InventoryState GetActiveInventoryState()
		{
			GameStateManager gameStateManager = GameStateManager.Current;
			InventoryState inventoryState;
			if ((inventoryState = ((gameStateManager != null) ? gameStateManager.ActiveState : null) as InventoryState) != null)
			{
				return inventoryState;
			}
			Debug.FailedAssert("GetActiveInventoryState requested but the active state is not InventoryState!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "GetActiveInventoryState", 9217);
			return null;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000FF33 File Offset: 0x0000E133
		public static void PlayerAcceptTradeOffer()
		{
			InventoryState activeInventoryState = InventoryScreenHelper.GetActiveInventoryState();
			if (activeInventoryState == null)
			{
				return;
			}
			InventoryLogic inventoryLogic = activeInventoryState.InventoryLogic;
			if (inventoryLogic == null)
			{
				return;
			}
			inventoryLogic.SetPlayerAcceptTraderOffer();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000FF4E File Offset: 0x0000E14E
		public static void CloseScreen(bool fromCancel)
		{
			InventoryScreenHelper.CloseInventoryPresentation(fromCancel);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000FF58 File Offset: 0x0000E158
		private static void CloseInventoryPresentation(bool fromCancel)
		{
			InventoryState activeInventoryState = InventoryScreenHelper.GetActiveInventoryState();
			InventoryLogic inventoryLogic = ((activeInventoryState != null) ? activeInventoryState.InventoryLogic : null);
			if (fromCancel && inventoryLogic != null)
			{
				inventoryLogic.Reset(fromCancel);
			}
			if (inventoryLogic != null && inventoryLogic.DoneLogic())
			{
				Action doneLogicExtrasDelegate = activeInventoryState.DoneLogicExtrasDelegate;
				if (doneLogicExtrasDelegate != null)
				{
					doneLogicExtrasDelegate();
				}
				activeInventoryState.DoneLogicExtrasDelegate = null;
				activeInventoryState.InventoryLogic = null;
				Game.Current.GameStateManager.PopState(0);
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000FFC0 File Offset: 0x0000E1C0
		private static void OpenInventoryPresentation(TextObject leftRosterName, Action doneLogicExtrasDelegate = null)
		{
			ItemRoster itemRoster = new ItemRoster();
			if (Game.Current.CheatMode)
			{
				TestCommonBase baseInstance = TestCommonBase.BaseInstance;
				if (baseInstance == null || !baseInstance.IsTestEnabled)
				{
					MBReadOnlyList<ItemObject> objectTypeList = Game.Current.ObjectManager.GetObjectTypeList<ItemObject>();
					for (int num = 0; num != objectTypeList.Count; num++)
					{
						ItemObject itemObject = objectTypeList[num];
						itemRoster.AddToCounts(itemObject, 10);
					}
				}
			}
			InventoryState inventoryState = Game.Current.GameStateManager.CreateState<InventoryState>();
			InventoryLogic inventoryLogic = new InventoryLogic(null);
			inventoryLogic.Initialize(itemRoster, MobileParty.MainParty, false, true, CharacterObject.PlayerCharacter, InventoryScreenHelper.InventoryCategoryType.None, InventoryScreenHelper.GetCurrentMarketDataForPlayer(), false, inventoryState.InventoryMode, leftRosterName, null, null);
			inventoryState.InventoryLogic = inventoryLogic;
			inventoryState.DoneLogicExtrasDelegate = doneLogicExtrasDelegate;
			Game.Current.GameStateManager.PushState(inventoryState, 0);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00010088 File Offset: 0x0000E288
		private static IMarketData GetCurrentMarketDataForPlayer()
		{
			IMarketData marketData = null;
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				Settlement settlement = MobileParty.MainParty.CurrentSettlement;
				if (settlement == null)
				{
					Town town = SettlementHelper.FindNearestTownToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, null);
					settlement = ((town != null) ? town.Settlement : null);
				}
				if (settlement != null)
				{
					if (settlement.IsVillage)
					{
						marketData = settlement.Village.MarketData;
					}
					else if (settlement.IsTown)
					{
						marketData = settlement.Town.MarketData;
					}
				}
			}
			if (marketData == null)
			{
				marketData = new FakeMarketData();
			}
			return marketData;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00010104 File Offset: 0x0000E304
		public static void OpenScreenAsInventoryOfSubParty(MobileParty rightParty, MobileParty leftParty, Action doneLogicExtrasDelegate)
		{
			InventoryState inventoryState = Game.Current.GameStateManager.CreateState<InventoryState>();
			Hero leaderHero = rightParty.LeaderHero;
			InventoryLogic inventoryLogic = new InventoryLogic(rightParty, (leaderHero != null) ? leaderHero.CharacterObject : null, leftParty.Party);
			InventoryLogic inventoryLogic2 = inventoryLogic;
			ItemRoster itemRoster = leftParty.ItemRoster;
			ItemRoster itemRoster2 = rightParty.ItemRoster;
			TroopRoster memberRoster = rightParty.MemberRoster;
			bool flag = false;
			bool flag2 = false;
			Hero leaderHero2 = rightParty.LeaderHero;
			inventoryLogic2.Initialize(itemRoster, itemRoster2, memberRoster, flag, flag2, (leaderHero2 != null) ? leaderHero2.CharacterObject : null, InventoryScreenHelper.InventoryCategoryType.None, InventoryScreenHelper.GetCurrentMarketDataForPlayer(), false, inventoryState.InventoryMode, null, null, null);
			inventoryState.InventoryLogic = inventoryLogic;
			inventoryState.DoneLogicExtrasDelegate = doneLogicExtrasDelegate;
			Game.Current.GameStateManager.PushState(inventoryState, 0);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0001019C File Offset: 0x0000E39C
		public static void OpenScreenAsInventoryForCraftedItemDecomposition(MobileParty party, CharacterObject character, Action doneLogicExtrasDelegate)
		{
			InventoryState inventoryState = Game.Current.GameStateManager.CreateState<InventoryState>();
			InventoryLogic inventoryLogic = new InventoryLogic(null);
			inventoryLogic.Initialize(new ItemRoster(), party.ItemRoster, party.MemberRoster, false, false, character, InventoryScreenHelper.InventoryCategoryType.None, InventoryScreenHelper.GetCurrentMarketDataForPlayer(), false, inventoryState.InventoryMode, null, null, null);
			inventoryState.InventoryLogic = inventoryLogic;
			inventoryState.DoneLogicExtrasDelegate = doneLogicExtrasDelegate;
			Game.Current.GameStateManager.PushState(inventoryState, 0);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0001020C File Offset: 0x0000E40C
		public static void OpenScreenAsInventoryOf(MobileParty party, CharacterObject character)
		{
			InventoryState inventoryState = Game.Current.GameStateManager.CreateState<InventoryState>();
			InventoryLogic inventoryLogic = new InventoryLogic(null);
			inventoryLogic.Initialize(new ItemRoster(), party.ItemRoster, party.MemberRoster, false, true, character, InventoryScreenHelper.InventoryCategoryType.None, InventoryScreenHelper.GetCurrentMarketDataForPlayer(), false, inventoryState.InventoryMode, null, null, null);
			inventoryState.InventoryLogic = inventoryLogic;
			Game.Current.GameStateManager.PushState(inventoryState, 0);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00010272 File Offset: 0x0000E472
		public static void OpenScreenAsInventoryOf(PartyBase rightParty, PartyBase leftParty)
		{
			Hero leaderHero = rightParty.LeaderHero;
			InventoryScreenHelper.OpenScreenAsInventoryOf(rightParty, leftParty, (leaderHero != null) ? leaderHero.CharacterObject : null, null, null, null);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00010290 File Offset: 0x0000E490
		public static void OpenScreenAsInventoryOf(PartyBase rightParty, PartyBase leftParty, CharacterObject character, TextObject leftRosterName = null, InventoryLogic.CapacityData capacityData = null, Action doneLogicExtrasDelegate = null)
		{
			InventoryState inventoryState = Game.Current.GameStateManager.CreateState<InventoryState>();
			InventoryLogic inventoryLogic = new InventoryLogic(leftParty);
			inventoryLogic.Initialize(leftParty.ItemRoster, rightParty.ItemRoster, rightParty.MemberRoster, false, false, character, InventoryScreenHelper.InventoryCategoryType.None, InventoryScreenHelper.GetCurrentMarketDataForPlayer(), false, InventoryScreenHelper.InventoryMode.Default, leftRosterName, leftParty.MemberRoster, capacityData);
			inventoryState.InventoryLogic = inventoryLogic;
			inventoryState.DoneLogicExtrasDelegate = doneLogicExtrasDelegate;
			Game.Current.GameStateManager.PushState(inventoryState, 0);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00010300 File Offset: 0x0000E500
		public static void OpenScreenAsInventory(Action doneLogicExtrasDelegate = null)
		{
			InventoryScreenHelper.OpenInventoryPresentation(new TextObject("{=02c5bQSM}Discard", null), doneLogicExtrasDelegate);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00010314 File Offset: 0x0000E514
		public static void OpenScreenAsLoot(Dictionary<PartyBase, ItemRoster> itemRostersToLoot)
		{
			ItemRoster itemRoster = itemRostersToLoot[PartyBase.MainParty];
			InventoryState inventoryState = Game.Current.GameStateManager.CreateState<InventoryState>();
			inventoryState.InventoryMode = InventoryScreenHelper.InventoryMode.Loot;
			InventoryLogic inventoryLogic = new InventoryLogic(null);
			inventoryLogic.Initialize(itemRoster, MobileParty.MainParty.ItemRoster, MobileParty.MainParty.MemberRoster, false, true, CharacterObject.PlayerCharacter, InventoryScreenHelper.InventoryCategoryType.None, InventoryScreenHelper.GetCurrentMarketDataForPlayer(), false, inventoryState.InventoryMode, GameTexts.FindText("str_loot", null), null, null);
			inventoryState.InventoryLogic = inventoryLogic;
			Game.Current.GameStateManager.PushState(inventoryState, 0);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000103A0 File Offset: 0x0000E5A0
		public static void OpenScreenAsStash(ItemRoster stash)
		{
			InventoryState inventoryState = Game.Current.GameStateManager.CreateState<InventoryState>();
			inventoryState.InventoryMode = InventoryScreenHelper.InventoryMode.Stash;
			InventoryLogic inventoryLogic = new InventoryLogic(null);
			inventoryLogic.Initialize(stash, MobileParty.MainParty, false, false, CharacterObject.PlayerCharacter, InventoryScreenHelper.InventoryCategoryType.None, InventoryScreenHelper.GetCurrentMarketDataForPlayer(), false, inventoryState.InventoryMode, new TextObject("{=nZbaYvVx}Stash", null), null, null);
			inventoryState.InventoryLogic = inventoryLogic;
			Game.Current.GameStateManager.PushState(inventoryState, 0);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00010410 File Offset: 0x0000E610
		public static void OpenScreenAsWarehouse(ItemRoster stash, InventoryLogic.CapacityData otherSideCapacity)
		{
			InventoryState inventoryState = Game.Current.GameStateManager.CreateState<InventoryState>();
			inventoryState.InventoryMode = InventoryScreenHelper.InventoryMode.Warehouse;
			InventoryLogic inventoryLogic = new InventoryLogic(null);
			inventoryLogic.Initialize(stash, MobileParty.MainParty, false, false, CharacterObject.PlayerCharacter, InventoryScreenHelper.InventoryCategoryType.None, InventoryScreenHelper.GetCurrentMarketDataForPlayer(), false, inventoryState.InventoryMode, new TextObject("{=anTRftmb}Warehouse", null), null, otherSideCapacity);
			inventoryState.InventoryLogic = inventoryLogic;
			Game.Current.GameStateManager.PushState(inventoryState, 0);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00010480 File Offset: 0x0000E680
		public static void OpenScreenAsReceiveItems(ItemRoster items, TextObject leftRosterName, Action doneLogicDelegate = null)
		{
			InventoryState inventoryState = Game.Current.GameStateManager.CreateState<InventoryState>();
			InventoryLogic inventoryLogic = new InventoryLogic(null);
			inventoryLogic.Initialize(items, MobileParty.MainParty.ItemRoster, MobileParty.MainParty.MemberRoster, false, true, CharacterObject.PlayerCharacter, InventoryScreenHelper.InventoryCategoryType.None, InventoryScreenHelper.GetCurrentMarketDataForPlayer(), false, inventoryState.InventoryMode, leftRosterName, null, null);
			inventoryState.InventoryLogic = inventoryLogic;
			inventoryState.DoneLogicExtrasDelegate = doneLogicDelegate;
			Game.Current.GameStateManager.PushState(inventoryState, 0);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000104F8 File Offset: 0x0000E6F8
		public static void OpenTradeWithCaravanOrAlleyParty(MobileParty caravan, InventoryScreenHelper.InventoryCategoryType merchantItemType = InventoryScreenHelper.InventoryCategoryType.None)
		{
			InventoryState inventoryState = Game.Current.GameStateManager.CreateState<InventoryState>();
			inventoryState.InventoryMode = InventoryScreenHelper.InventoryMode.Trade;
			InventoryLogic inventoryLogic = new InventoryLogic(caravan.Party);
			inventoryLogic.Initialize(caravan.Party.ItemRoster, PartyBase.MainParty.ItemRoster, PartyBase.MainParty.MemberRoster, true, true, CharacterObject.PlayerCharacter, merchantItemType, InventoryScreenHelper.GetCurrentMarketDataForPlayer(), false, inventoryState.InventoryMode, null, null, null);
			inventoryLogic.SetInventoryListener(new InventoryScreenHelper.CaravanInventoryListener(caravan));
			inventoryState.InventoryLogic = inventoryLogic;
			Game.Current.GameStateManager.PushState(inventoryState, 0);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00010588 File Offset: 0x0000E788
		public static void ActivateTradeWithCurrentSettlement()
		{
			InventoryScreenHelper.OpenScreenAsTrade(Settlement.CurrentSettlement.ItemRoster, Settlement.CurrentSettlement.SettlementComponent, InventoryScreenHelper.InventoryCategoryType.None, null);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000105A8 File Offset: 0x0000E7A8
		public static void OpenScreenAsTrade(ItemRoster leftRoster, SettlementComponent settlementComponent, InventoryScreenHelper.InventoryCategoryType merchantItemType = InventoryScreenHelper.InventoryCategoryType.None, Action doneLogicExtrasDelegate = null)
		{
			InventoryState inventoryState = Game.Current.GameStateManager.CreateState<InventoryState>();
			inventoryState.InventoryMode = InventoryScreenHelper.InventoryMode.Trade;
			InventoryLogic inventoryLogic = new InventoryLogic(settlementComponent.Owner);
			inventoryLogic.Initialize(leftRoster, PartyBase.MainParty.ItemRoster, PartyBase.MainParty.MemberRoster, true, true, CharacterObject.PlayerCharacter, merchantItemType, InventoryScreenHelper.GetCurrentMarketDataForPlayer(), false, inventoryState.InventoryMode, null, null, null);
			inventoryLogic.SetInventoryListener(new InventoryScreenHelper.MerchantInventoryListener(settlementComponent));
			inventoryState.InventoryLogic = inventoryLogic;
			inventoryState.DoneLogicExtrasDelegate = doneLogicExtrasDelegate;
			Game.Current.GameStateManager.PushState(inventoryState, 0);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00010638 File Offset: 0x0000E838
		public static InventoryScreenHelper.InventoryItemType GetInventoryItemTypeOfItem(ItemObject item)
		{
			if (item != null)
			{
				switch (item.ItemType)
				{
				case ItemObject.ItemTypeEnum.Horse:
					return InventoryScreenHelper.InventoryItemType.Horse;
				case ItemObject.ItemTypeEnum.OneHandedWeapon:
				case ItemObject.ItemTypeEnum.TwoHandedWeapon:
				case ItemObject.ItemTypeEnum.Polearm:
				case ItemObject.ItemTypeEnum.Arrows:
				case ItemObject.ItemTypeEnum.Bolts:
				case ItemObject.ItemTypeEnum.SlingStones:
				case ItemObject.ItemTypeEnum.Bow:
				case ItemObject.ItemTypeEnum.Crossbow:
				case ItemObject.ItemTypeEnum.Sling:
				case ItemObject.ItemTypeEnum.Thrown:
				case ItemObject.ItemTypeEnum.Pistol:
				case ItemObject.ItemTypeEnum.Musket:
				case ItemObject.ItemTypeEnum.Bullets:
					return InventoryScreenHelper.InventoryItemType.Weapon;
				case ItemObject.ItemTypeEnum.Shield:
					return InventoryScreenHelper.InventoryItemType.Shield;
				case ItemObject.ItemTypeEnum.Goods:
					return InventoryScreenHelper.InventoryItemType.Goods;
				case ItemObject.ItemTypeEnum.HeadArmor:
					return InventoryScreenHelper.InventoryItemType.HeadArmor;
				case ItemObject.ItemTypeEnum.BodyArmor:
					return InventoryScreenHelper.InventoryItemType.BodyArmor;
				case ItemObject.ItemTypeEnum.LegArmor:
					return InventoryScreenHelper.InventoryItemType.LegArmor;
				case ItemObject.ItemTypeEnum.HandArmor:
					return InventoryScreenHelper.InventoryItemType.HandArmor;
				case ItemObject.ItemTypeEnum.Animal:
					return InventoryScreenHelper.InventoryItemType.Animal;
				case ItemObject.ItemTypeEnum.Book:
					return InventoryScreenHelper.InventoryItemType.Book;
				case ItemObject.ItemTypeEnum.Cape:
					return InventoryScreenHelper.InventoryItemType.Cape;
				case ItemObject.ItemTypeEnum.HorseHarness:
					return InventoryScreenHelper.InventoryItemType.HorseHarness;
				case ItemObject.ItemTypeEnum.Banner:
					return InventoryScreenHelper.InventoryItemType.Banner;
				}
			}
			return InventoryScreenHelper.InventoryItemType.None;
		}

		// Token: 0x02000514 RID: 1300
		public enum InventoryMode
		{
			// Token: 0x04001640 RID: 5696
			Default,
			// Token: 0x04001641 RID: 5697
			Trade,
			// Token: 0x04001642 RID: 5698
			Loot,
			// Token: 0x04001643 RID: 5699
			Stash,
			// Token: 0x04001644 RID: 5700
			Warehouse
		}

		// Token: 0x02000515 RID: 1301
		// (Invoke) Token: 0x06004E92 RID: 20114
		public delegate void InventoryFinishDelegate();

		// Token: 0x02000516 RID: 1302
		[Flags]
		public enum InventoryItemType
		{
			// Token: 0x04001646 RID: 5702
			None = 0,
			// Token: 0x04001647 RID: 5703
			Weapon = 1,
			// Token: 0x04001648 RID: 5704
			Shield = 2,
			// Token: 0x04001649 RID: 5705
			HeadArmor = 4,
			// Token: 0x0400164A RID: 5706
			BodyArmor = 8,
			// Token: 0x0400164B RID: 5707
			LegArmor = 16,
			// Token: 0x0400164C RID: 5708
			HandArmor = 32,
			// Token: 0x0400164D RID: 5709
			Horse = 64,
			// Token: 0x0400164E RID: 5710
			HorseHarness = 128,
			// Token: 0x0400164F RID: 5711
			Goods = 256,
			// Token: 0x04001650 RID: 5712
			Book = 512,
			// Token: 0x04001651 RID: 5713
			Animal = 1024,
			// Token: 0x04001652 RID: 5714
			Cape = 2048,
			// Token: 0x04001653 RID: 5715
			Banner = 4096,
			// Token: 0x04001654 RID: 5716
			HorseCategory = 192,
			// Token: 0x04001655 RID: 5717
			Armors = 2108,
			// Token: 0x04001656 RID: 5718
			Equipable = 6399,
			// Token: 0x04001657 RID: 5719
			All = 4095
		}

		// Token: 0x02000517 RID: 1303
		public enum InventoryCategoryType
		{
			// Token: 0x04001659 RID: 5721
			None = -1,
			// Token: 0x0400165A RID: 5722
			All,
			// Token: 0x0400165B RID: 5723
			Armors,
			// Token: 0x0400165C RID: 5724
			Weapon,
			// Token: 0x0400165D RID: 5725
			Shield,
			// Token: 0x0400165E RID: 5726
			HorseCategory,
			// Token: 0x0400165F RID: 5727
			Goods,
			// Token: 0x04001660 RID: 5728
			CategoryTypeAmount
		}

		// Token: 0x02000518 RID: 1304
		private class CaravanInventoryListener : InventoryListener
		{
			// Token: 0x06004E95 RID: 20117 RVA: 0x0018BF52 File Offset: 0x0018A152
			public CaravanInventoryListener(MobileParty caravan)
			{
				this._caravan = caravan;
			}

			// Token: 0x06004E96 RID: 20118 RVA: 0x0018BF61 File Offset: 0x0018A161
			public override int GetGold()
			{
				return this._caravan.PartyTradeGold;
			}

			// Token: 0x06004E97 RID: 20119 RVA: 0x0018BF6E File Offset: 0x0018A16E
			public override TextObject GetTraderName()
			{
				if (this._caravan.LeaderHero == null)
				{
					return this._caravan.Name;
				}
				return this._caravan.LeaderHero.Name;
			}

			// Token: 0x06004E98 RID: 20120 RVA: 0x0018BF99 File Offset: 0x0018A199
			public override void SetGold(int gold)
			{
				this._caravan.PartyTradeGold = gold;
			}

			// Token: 0x06004E99 RID: 20121 RVA: 0x0018BFA7 File Offset: 0x0018A1A7
			public override PartyBase GetOppositeParty()
			{
				return this._caravan.Party;
			}

			// Token: 0x06004E9A RID: 20122 RVA: 0x0018BFB4 File Offset: 0x0018A1B4
			public override void OnTransaction()
			{
				throw new NotImplementedException();
			}

			// Token: 0x04001661 RID: 5729
			private MobileParty _caravan;
		}

		// Token: 0x02000519 RID: 1305
		private class MerchantInventoryListener : InventoryListener
		{
			// Token: 0x06004E9B RID: 20123 RVA: 0x0018BFBB File Offset: 0x0018A1BB
			public MerchantInventoryListener(SettlementComponent settlementComponent)
			{
				this._settlementComponent = settlementComponent;
			}

			// Token: 0x06004E9C RID: 20124 RVA: 0x0018BFCA File Offset: 0x0018A1CA
			public override TextObject GetTraderName()
			{
				return this._settlementComponent.Owner.Name;
			}

			// Token: 0x06004E9D RID: 20125 RVA: 0x0018BFDC File Offset: 0x0018A1DC
			public override PartyBase GetOppositeParty()
			{
				return this._settlementComponent.Owner;
			}

			// Token: 0x06004E9E RID: 20126 RVA: 0x0018BFE9 File Offset: 0x0018A1E9
			public override int GetGold()
			{
				return this._settlementComponent.Gold;
			}

			// Token: 0x06004E9F RID: 20127 RVA: 0x0018BFF6 File Offset: 0x0018A1F6
			public override void SetGold(int gold)
			{
				this._settlementComponent.ChangeGold(gold - this._settlementComponent.Gold);
			}

			// Token: 0x06004EA0 RID: 20128 RVA: 0x0018C010 File Offset: 0x0018A210
			public override void OnTransaction()
			{
				throw new NotImplementedException();
			}

			// Token: 0x04001662 RID: 5730
			private SettlementComponent _settlementComponent;
		}
	}
}
