using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F8 RID: 1016
	public class CaravansCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06003DCC RID: 15820 RVA: 0x00102AF8 File Offset: 0x00100CF8
		private int MaxNumberOfItemsToBuyFromSingleCategory
		{
			get
			{
				return Campaign.Current.Models.CaravanModel.MaxNumberOfItemsToBuyFromSingleCategory;
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06003DCD RID: 15821 RVA: 0x00102B0E File Offset: 0x00100D0E
		public ITradeAgreementsCampaignBehavior TradeAgreementsCampaignBehavior
		{
			get
			{
				if (this._tradeAgreementsBehavior == null)
				{
					this._tradeAgreementsBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
				}
				return this._tradeAgreementsBehavior;
			}
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x00102B2E File Offset: 0x00100D2E
		private float GetDistanceLimitVeryFarAsDaysForNavigationType(bool isNavalCaravan)
		{
			if (!isNavalCaravan)
			{
				return this._defaultCaravanVeryFarCache;
			}
			return this._navalCaravanVeryFarCache;
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x00102B40 File Offset: 0x00100D40
		private float GetDistanceLimitFarAsDaysForNavigationType(bool isNavalCaravan)
		{
			return this.GetDistanceLimitVeryFarAsDaysForNavigationType(isNavalCaravan) * 0.75f;
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x00102B4F File Offset: 0x00100D4F
		private float GetDistanceLimitMediumAsDaysForNavigationType(bool isNavalCaravan)
		{
			return this.GetDistanceLimitVeryFarAsDaysForNavigationType(isNavalCaravan) * 0.5f;
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x00102B5E File Offset: 0x00100D5E
		private float GetDistanceLimitCloseAsDaysForNavigationType(bool isNavalCaravan)
		{
			return this.GetDistanceLimitVeryFarAsDaysForNavigationType(isNavalCaravan) * 0.25f;
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x00102B70 File Offset: 0x00100D70
		public CaravansCampaignBehavior()
		{
			this._tradeActionLogPool = new CaravansCampaignBehavior.TradeActionLogPool(4096);
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x00102C20 File Offset: 0x00100E20
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyTickParty));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEndEvent));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.OnMobilePartyCreated));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnLootDistributedToPartyEvent.AddNonSerializedListener(this, new Action<PartyBase, PartyBase, ItemRoster>(this.OnLootDistributedToParty));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x00102D6F File Offset: 0x00100F6F
		private void OnKingdomDestroyed(Kingdom destroyedKingdom)
		{
			if (this._prohibitedKingdomsForPlayerCaravans.Contains(destroyedKingdom))
			{
				this._prohibitedKingdomsForPlayerCaravans.Remove(destroyedKingdom);
			}
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x00102D8C File Offset: 0x00100F8C
		private void OnGameLoadFinished()
		{
			this.CreatePriceDataCache();
			foreach (MobileParty mobileParty in MobileParty.AllCaravanParties)
			{
				if ((!mobileParty.IsActive || !mobileParty.IsReady) && this._caravanLastHomeTownVisitTime.ContainsKey(mobileParty))
				{
					this._caravanLastHomeTownVisitTime.Remove(mobileParty);
				}
			}
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x00102E08 File Offset: 0x00101008
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			for (int i = 0; i < siegeEvent.BesiegedSettlement.Parties.Count; i++)
			{
				if (siegeEvent.BesiegedSettlement.Parties[i].IsCaravan)
				{
					siegeEvent.BesiegedSettlement.Parties[i].SetMoveModeHold();
				}
			}
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x00102E5E File Offset: 0x0010105E
		private void OnLootDistributedToParty(PartyBase winnerParty, PartyBase defeatedParty, ItemRoster lootedItems)
		{
			if (winnerParty.IsMobile && defeatedParty.IsMobile && defeatedParty.MobileParty.IsCaravan)
			{
				SkillLevelingManager.OnLoot(winnerParty.MobileParty, defeatedParty.MobileParty, lootedItems, true);
			}
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x00102E90 File Offset: 0x00101090
		private void OnNewGameCreatedPartialFollowUpEndEvent(CampaignGameStarter obj)
		{
			for (int i = 0; i < 2; i++)
			{
				foreach (Hero hero in Hero.AllAliveHeroes)
				{
					if (hero.Clan != Clan.PlayerClan && Campaign.Current.Models.CaravanModel.CanHeroCreateCaravan(hero))
					{
						this.SpawnCaravan(hero, true);
					}
				}
				this.UpdateAverageValues();
				this.DoInitialTradeRuns();
			}
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x00102F20 File Offset: 0x00101120
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<MobileParty, CampaignTime>>("_tradeRumorTakenCaravans", ref this._tradeRumorTakenCaravans);
			dataStore.SyncData<Dictionary<MobileParty, CampaignTime>>("_lootedCaravans", ref this._lootedCaravans);
			dataStore.SyncData<Dictionary<MobileParty, CaravansCampaignBehavior.PlayerInteraction>>("_interactedCaravans", ref this._interactedCaravans);
			dataStore.SyncData<Dictionary<MobileParty, List<CaravansCampaignBehavior.TradeActionLog>>>("_tradeActionLogs", ref this._tradeActionLogs);
			dataStore.SyncData<Dictionary<MobileParty, CampaignTime>>("_caravanLastHomeTownVisitTime", ref this._caravanLastHomeTownVisitTime);
			dataStore.SyncData<List<Kingdom>>("_prohibitedKingdomsForPlayerCaravans", ref this._prohibitedKingdomsForPlayerCaravans);
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x00102F9C File Offset: 0x0010119C
		private void DoInitialTradeRuns()
		{
			foreach (MobileParty mobileParty in MobileParty.AllCaravanParties)
			{
				Settlement currentSettlement = mobileParty.CurrentSettlement;
				Town town = ((currentSettlement != null) ? currentSettlement.Town : null);
				MobileParty.NavigationType navigationType = (mobileParty.HasNavalNavigationCapability ? MobileParty.NavigationType.Naval : MobileParty.NavigationType.Default);
				List<ValueTuple<Town, float>> list = new List<ValueTuple<Town, float>>();
				foreach (Town town2 in Town.AllTowns)
				{
					if (town2.Settlement.HasPort || mobileParty.HasLandNavigationCapability)
					{
						bool flag = navigationType == MobileParty.NavigationType.Naval;
						float num;
						if (mobileParty.CurrentSettlement != null)
						{
							num = Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty.CurrentSettlement, town2.Settlement, flag, flag, navigationType);
						}
						else
						{
							float num2;
							num = Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty, town2.Settlement, flag, navigationType, out num2);
						}
						list.Add(new ValueTuple<Town, float>(town2, 1f / MathF.Pow(num, 1.5f)));
					}
				}
				Town town3 = MBRandom.ChooseWeighted<Town>(list);
				if (town3 != null && town != null)
				{
					this.CreatePriceDataCache();
					if (MBRandom.RandomFloat < 0.5f)
					{
						this.SellGoods(mobileParty, town, 0.7f, false);
						this.BuyGoods(mobileParty, town);
						this.SellGoods(mobileParty, town3, 0.7f, false);
					}
					else
					{
						this.SellGoods(mobileParty, town3, 0.7f, false);
						this.BuyGoods(mobileParty, town3);
						this.SellGoods(mobileParty, town, 0.7f, false);
					}
				}
			}
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x0010317C File Offset: 0x0010137C
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.CacheVeryFarDistances();
			this.AddDialogs(campaignGameStarter);
			this.UpdateAverageValues();
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x00103194 File Offset: 0x00101394
		private void CacheVeryFarDistances()
		{
			MobileParty.NavigationType navigationType = MobileParty.NavigationType.Naval;
			float num = 20f;
			float num2 = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(navigationType) * num;
			float num3 = Campaign.Current.EstimatedAverageCaravanPartyNavalSpeed * (float)CampaignTime.HoursInDay;
			this._navalCaravanVeryFarCache = num2 / num3;
			navigationType = MobileParty.NavigationType.Default;
			num = 5f;
			num2 = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(navigationType) * num;
			num3 = Campaign.Current.EstimatedAverageCaravanPartySpeed * (float)CampaignTime.HoursInDay;
			this._defaultCaravanVeryFarCache = num2 / num3;
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x00103204 File Offset: 0x00101404
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			foreach (PartyBase partyBase in mapEvent.InvolvedParties)
			{
				if (partyBase.IsMobile && partyBase.MobileParty.IsCaravan && mapEvent.IsWinnerSide(partyBase.Side))
				{
					if (partyBase.MobileParty.HasNavalNavigationCapability)
					{
						this.DiscardShips(partyBase.MobileParty);
					}
					MobileParty mobileParty = partyBase.MobileParty;
					int numberOfPackAnimals = mobileParty.ItemRoster.NumberOfPackAnimals;
					int numberOfLivestockAnimals = mobileParty.ItemRoster.NumberOfLivestockAnimals;
					int numberOfMounts = mobileParty.ItemRoster.NumberOfMounts;
					int totalManCount = mobileParty.MemberRoster.TotalManCount;
					if (partyBase.MobileParty.HasLandNavigationCapability && (float)(numberOfPackAnimals + numberOfLivestockAnimals + numberOfMounts) > (float)totalManCount * 1.2f)
					{
						int num2;
						for (int i = numberOfPackAnimals + numberOfLivestockAnimals + numberOfMounts; i > totalManCount; i -= num2)
						{
							int num = 10000;
							ItemRosterElement itemRosterElement = partyBase.MobileParty.ItemRoster[0];
							foreach (ItemRosterElement itemRosterElement2 in partyBase.MobileParty.ItemRoster)
							{
								if (itemRosterElement2.EquipmentElement.Item.IsMountable || itemRosterElement2.EquipmentElement.Item.ItemCategory == DefaultItemCategories.PackAnimal || itemRosterElement2.EquipmentElement.Item.IsAnimal)
								{
									int itemValue = itemRosterElement2.EquipmentElement.ItemValue;
									if (itemValue < num)
									{
										num = itemValue;
										itemRosterElement = itemRosterElement2;
									}
								}
							}
							num2 = MathF.Min(itemRosterElement.Amount, MathF.Max(1, i - totalManCount));
							mobileParty.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement, -num2);
						}
					}
					int inventoryCapacity = mobileParty.InventoryCapacity;
					float totalWeightCarried = mobileParty.TotalWeightCarried;
					float num3 = 0f;
					if (totalWeightCarried - num3 > (float)inventoryCapacity)
					{
						while (totalWeightCarried - num3 > (float)inventoryCapacity)
						{
							int num4 = 10000;
							ItemRosterElement itemRosterElement3 = partyBase.MobileParty.ItemRoster[0];
							foreach (ItemRosterElement itemRosterElement4 in partyBase.MobileParty.ItemRoster)
							{
								if (!itemRosterElement4.EquipmentElement.Item.IsMountable)
								{
									int itemValue2 = itemRosterElement4.EquipmentElement.ItemValue;
									if (itemValue2 < num4)
									{
										num4 = itemValue2;
										itemRosterElement3 = itemRosterElement4;
									}
								}
							}
							int num5 = MathF.Ceiling((totalWeightCarried - num3 - (float)inventoryCapacity) / itemRosterElement3.EquipmentElement.Weight);
							int num6 = Math.Max(1, Math.Min(itemRosterElement3.Amount, num5));
							float weight = itemRosterElement3.EquipmentElement.Weight;
							mobileParty.ItemRoster.AddToCounts(itemRosterElement3.EquipmentElement, -num6);
							num3 += weight * (float)num6;
						}
					}
				}
			}
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x0010355C File Offset: 0x0010175C
		public void SpawnCaravan(Hero hero, bool initialSpawn = false)
		{
			bool flag = Campaign.Current.Models.CaravanModel.GetEliteCaravanSpawnChance(hero) > hero.RandomFloat();
			PartyTemplateObject randomElementWithPredicate = (flag ? hero.Culture.EliteCaravanPartyTemplates : hero.Culture.CaravanPartyTemplates).GetRandomElementWithPredicate<PartyTemplateObject>((PartyTemplateObject x) => x.ShipHulls.Count == 0 != hero.CurrentSettlement.HasPort);
			bool isNaval = randomElementWithPredicate.ShipHulls.Any<ShipTemplateStack>();
			Settlement settlement = hero.HomeSettlement ?? hero.BornSettlement;
			Settlement settlement2;
			if (settlement == null)
			{
				settlement2 = Town.AllTowns.GetRandomElementWithPredicate<Town>((Town x) => x.Settlement.HasPort == isNaval).Settlement;
			}
			else if (settlement.IsTown)
			{
				settlement2 = settlement;
			}
			else if (settlement.IsVillage)
			{
				settlement2 = settlement.Village.TradeBound ?? Town.AllTowns.GetRandomElementWithPredicate<Town>((Town x) => x.Settlement.HasPort == isNaval).Settlement;
			}
			else
			{
				settlement2 = Town.AllTowns.GetRandomElementWithPredicate<Town>((Town x) => x.Settlement.HasPort == isNaval).Settlement;
			}
			MobileParty mobileParty = CaravanPartyComponent.CreateCaravanParty(hero, settlement2, randomElementWithPredicate, initialSpawn, null, null, flag);
			if (!initialSpawn)
			{
				hero.AddPower((float)Campaign.Current.Models.CaravanModel.GetPowerChangeAfterCaravanCreation(hero, mobileParty));
			}
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x001036C4 File Offset: 0x001018C4
		private void UpdateAverageValues()
		{
			Dictionary<ItemCategory, ValueTuple<float, int>> dictionary = new Dictionary<ItemCategory, ValueTuple<float, int>>();
			foreach (ItemObject itemObject in Items.All)
			{
				if (itemObject.IsReady)
				{
					ValueTuple<float, int> valueTuple;
					dictionary.TryGetValue(itemObject.ItemCategory, out valueTuple);
					dictionary[itemObject.ItemCategory] = new ValueTuple<float, int>(valueTuple.Item1 + (float)MathF.Min(500, itemObject.Value), valueTuple.Item2 + 1);
				}
			}
			this._packAnimalCategoryIndex = -1;
			for (int i = 0; i < ItemCategories.All.Count; i++)
			{
				ItemCategory itemCategory = ItemCategories.All[i];
				ValueTuple<float, int> valueTuple2;
				bool flag = dictionary.TryGetValue(itemCategory, out valueTuple2);
				this._averageValuesCached[itemCategory] = (flag ? (valueTuple2.Item1 / (float)valueTuple2.Item2) : 1f);
				if (itemCategory == DefaultItemCategories.PackAnimal)
				{
					this._packAnimalCategoryIndex = i;
				}
			}
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x001037D0 File Offset: 0x001019D0
		private void CreatePriceDataCache()
		{
			foreach (ItemCategory itemCategory in ItemCategories.All)
			{
				float num = 0f;
				float num2 = 1000f;
				float num3 = 0f;
				float num4 = 1000f;
				foreach (Town town in Town.AllTowns)
				{
					float itemCategoryPriceIndex = town.GetItemCategoryPriceIndex(itemCategory);
					num += itemCategoryPriceIndex;
					if (itemCategoryPriceIndex < num2)
					{
						num2 = itemCategoryPriceIndex;
					}
					if (town.Settlement.HasPort)
					{
						num3 += itemCategoryPriceIndex;
						if (itemCategoryPriceIndex < num4)
						{
							num4 = itemCategoryPriceIndex;
						}
					}
				}
				float num5 = num / (float)Town.AllTowns.Count;
				float num6 = num3 / (float)Town.AllTowns.Count<Town>((Town x) => x.Settlement.HasPort);
				this._priceDictionary[itemCategory] = new CaravansCampaignBehavior.PriceIndexData(num5, num2);
				this._coastalPriceDictionary[itemCategory] = new CaravansCampaignBehavior.PriceIndexData(num6, num4);
			}
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x00103910 File Offset: 0x00101B10
		public void DailyTick()
		{
			this.DeleteExpiredTradeRumorTakenCaravans();
			this.DeleteExpiredLootedCaravans();
			this.CreatePriceDataCache();
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x00103924 File Offset: 0x00101B24
		private void DailyTickHero(Hero hero)
		{
			if (hero != Hero.MainHero && MBRandom.RandomFloat < 0.75f && Campaign.Current.Models.CaravanModel.CanHeroCreateCaravan(hero))
			{
				this.SpawnCaravan(hero, false);
			}
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x0010395C File Offset: 0x00101B5C
		private void DeleteExpiredTradeRumorTakenCaravans()
		{
			List<MobileParty> list = new List<MobileParty>();
			foreach (KeyValuePair<MobileParty, CampaignTime> keyValuePair in this._tradeRumorTakenCaravans)
			{
				if (CampaignTime.Now - keyValuePair.Value >= CampaignTime.Days(1f))
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (MobileParty mobileParty in list)
			{
				this._tradeRumorTakenCaravans.Remove(mobileParty);
			}
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x00103A24 File Offset: 0x00101C24
		private void DeleteExpiredLootedCaravans()
		{
			List<MobileParty> list = new List<MobileParty>();
			foreach (KeyValuePair<MobileParty, CampaignTime> keyValuePair in this._lootedCaravans)
			{
				if (CampaignTime.Now - keyValuePair.Value >= CampaignTime.Days(10f))
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (MobileParty mobileParty in list)
			{
				this._lootedCaravans.Remove(mobileParty);
			}
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x00103AEC File Offset: 0x00101CEC
		private Town GetDestinationForMobileParty(MobileParty party)
		{
			Settlement targetSettlement = party.TargetSettlement;
			if (targetSettlement == null)
			{
				return null;
			}
			return targetSettlement.Town;
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x00103B00 File Offset: 0x00101D00
		public void HourlyTickParty(MobileParty mobileParty)
		{
			if (!Campaign.Current.GameStarted)
			{
				return;
			}
			if (mobileParty.IsCaravan)
			{
				bool flag = false;
				float randomFloat = MBRandom.RandomFloat;
				if (mobileParty.MapEvent == null && !mobileParty.IsInRaftState && mobileParty.IsPartyTradeActive && !mobileParty.Ai.DoNotMakeNewDecisions && mobileParty.DefaultBehavior != AiBehavior.MoveToNearestLandOrPort)
				{
					if (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsFortification)
					{
						if ((!mobileParty.CurrentSettlement.IsUnderSiege || (!mobileParty.CurrentSettlement.SiegeEvent.IsBlockadeActive && mobileParty.HasNavalNavigationCapability)) && mobileParty.ShortTermBehavior != AiBehavior.FleeToPoint && !mobileParty.Ai.IsAlerted && (mobileParty.IsCurrentlyUsedByAQuest || randomFloat < 0.33333334f))
						{
							float num = ((mobileParty.MemberRoster.TotalManCount > 0) ? ((float)mobileParty.MemberRoster.TotalWounded / (float)mobileParty.MemberRoster.TotalManCount) : 1f);
							float num2 = 1f;
							if ((double)num > 0.4)
							{
								num2 = 0f;
							}
							else if ((double)num > 0.2)
							{
								num2 = 0.1f;
							}
							else if ((double)num > 0.1)
							{
								num2 = 0.2f;
							}
							else if ((double)num > 0.05)
							{
								num2 = 0.3f;
							}
							else if ((double)num > 0.025)
							{
								num2 = 0.4f;
							}
							float randomFloat2 = MBRandom.RandomFloat;
							if (num2 > randomFloat2)
							{
								flag = true;
							}
						}
					}
					else
					{
						Town destinationForMobileParty = this.GetDestinationForMobileParty(mobileParty);
						flag = destinationForMobileParty == null || (destinationForMobileParty.IsUnderSiege && (!mobileParty.HasNavalNavigationCapability || destinationForMobileParty.Settlement.SiegeEvent.IsBlockadeActive)) || !this.CanTradeWith(mobileParty.MapFaction, destinationForMobileParty.MapFaction);
					}
					if (flag)
					{
						if (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsTown)
						{
							Town town = mobileParty.CurrentSettlement.Town;
							this.BuyGoods(mobileParty, town);
						}
						MobileParty.NavigationType navigationType;
						bool flag2;
						bool flag3;
						Town town2 = this.ThinkNextDestination(mobileParty, out navigationType, out flag2, out flag3);
						if (town2 != null)
						{
							SetPartyAiAction.GetActionForVisitingSettlement(mobileParty, town2.Settlement, navigationType, flag2, flag3);
						}
					}
				}
			}
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x00103D30 File Offset: 0x00101F30
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			Town town = settlement.Town;
			if (Campaign.Current.GameStarted && mobileParty != null && town != null && mobileParty.IsCaravan && mobileParty.IsPartyTradeActive && mobileParty.IsActive)
			{
				if (mobileParty.DefaultBehavior == AiBehavior.MoveToNearestLandOrPort)
				{
					mobileParty.SetMoveModeHold();
				}
				if (mobileParty.CaravanPartyComponent.CanHaveNavalNavigationCapability)
				{
					this.AdjustConvoyShips(mobileParty, town);
					this.RefillConvoyTroops(mobileParty);
				}
				if (Campaign.Current.GameStarted)
				{
					List<CaravansCampaignBehavior.TradeActionLog> list;
					if (this._tradeActionLogs.TryGetValue(mobileParty, out list))
					{
						for (int i = list.Count - 1; i >= 0; i--)
						{
							CaravansCampaignBehavior.TradeActionLog tradeActionLog = list[i];
							if (tradeActionLog.BoughtTime.ElapsedDaysUntilNow > 7f)
							{
								list.RemoveAt(i);
								this._tradeActionLogPool.ReleaseLog(tradeActionLog);
							}
						}
					}
					this.SellGoods(mobileParty, town, 1.1f, false);
				}
				if (mobileParty.HomeSettlement == settlement)
				{
					this._caravanLastHomeTownVisitTime[mobileParty] = CampaignTime.Now;
				}
			}
			if (mobileParty != null && mobileParty.IsCaravan && mobileParty.HasLandNavigationCapability && settlement.IsTown && settlement.Town.Governor != null && settlement.Town.Governor.GetPerkValue(DefaultPerks.Trade.Tollgates))
			{
				int num = MathF.Round(DefaultPerks.Trade.Tollgates.SecondaryBonus);
				settlement.Town.TradeTaxAccumulated += num;
			}
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x00103E94 File Offset: 0x00102094
		private void DiscardShips(MobileParty convoy)
		{
			MBList<ValueTuple<Ship, float>> mblist = new MBList<ValueTuple<Ship, float>>();
			foreach (Ship ship in convoy.Ships)
			{
				mblist.Add(new ValueTuple<Ship, float>(ship, this.GetShipPriority(convoy, ship, false)));
			}
			mblist = mblist.OrderByDescending<ValueTuple<Ship, float>, float>((ValueTuple<Ship, float> x) => x.Item2).ToMBList<ValueTuple<Ship, float>>();
			int idealShipNumber = Campaign.Current.Models.PartyShipLimitModel.GetIdealShipNumber(convoy);
			for (int i = 0; i < mblist.Count; i++)
			{
				Ship item = mblist[i].Item1;
				float item2 = mblist[i].Item2;
				if (i >= idealShipNumber || (item2 == -3.4028235E+38f && convoy.Ships.Count > 1))
				{
					DestroyShipAction.ApplyByDiscard(item);
				}
			}
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x00103F94 File Offset: 0x00102194
		private void RefillConvoyTroops(MobileParty convoy)
		{
			PartyTemplateObject randomCaravanTemplate = CaravanHelper.GetRandomCaravanTemplate(convoy.Owner.Culture, convoy.CaravanPartyComponent.IsElite, false);
			int totalManCount = convoy.MemberRoster.TotalManCount;
			int num = convoy.Party.PartySizeLimit - totalManCount;
			if (num > 0)
			{
				int num2 = randomCaravanTemplate.Stacks.Sum<PartyTemplateStack>((PartyTemplateStack x) => x.MaxValue);
				float num3 = (float)num / (float)num2;
				foreach (PartyTemplateStack partyTemplateStack in randomCaravanTemplate.Stacks)
				{
					CharacterObject character = partyTemplateStack.Character;
					int num4 = MathF.Floor((float)partyTemplateStack.MaxValue * num3);
					num -= num4;
					convoy.MemberRoster.AddToCounts(character, num4, false, 0, 0, true, -1);
				}
				if (num > 0)
				{
					List<ValueTuple<int, float>> list = new List<ValueTuple<int, float>>();
					for (int i = 0; i < randomCaravanTemplate.Stacks.Count; i++)
					{
						PartyTemplateStack partyTemplateStack2 = randomCaravanTemplate.Stacks[i];
						float num5 = (float)(partyTemplateStack2.MaxValue + partyTemplateStack2.MinValue) / 2f;
						list.Add(new ValueTuple<int, float>(i, num5));
					}
					for (int j = 0; j < num; j++)
					{
						int num6 = MBRandom.ChooseWeighted<int>(list);
						CharacterObject character2 = randomCaravanTemplate.Stacks[num6].Character;
						convoy.MemberRoster.AddToCounts(character2, 1, false, 0, 0, true, -1);
					}
				}
			}
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x00104124 File Offset: 0x00102324
		private void AdjustConvoyShips(MobileParty caravan, Town town)
		{
			if (town.AvailableShips.Count > 0 && caravan.PartyTradeGold > 10000 && caravan.Ships.Count < Campaign.Current.Models.PartyShipLimitModel.GetIdealShipNumber(caravan))
			{
				this.BuyShips(caravan, town);
			}
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x00104178 File Offset: 0x00102378
		private void BuyShips(MobileParty caravan, Town town)
		{
			MBList<ValueTuple<Ship, float>> mblist = new MBList<ValueTuple<Ship, float>>();
			bool flag = false;
			if (caravan.CaravanPartyComponent.IsElite)
			{
				flag = true;
				for (int i = 0; i < caravan.Ships.Count; i++)
				{
					if (caravan.Ships[i].ShipHull.Type == ShipHull.ShipType.Medium)
					{
						flag = false;
						break;
					}
				}
			}
			for (int j = town.AvailableShips.Count - 1; j >= 0; j--)
			{
				Ship ship = town.AvailableShips[j];
				if (ship.ShipHull.Type == ShipHull.ShipType.Light || (ship.ShipHull.Type == ShipHull.ShipType.Medium && flag))
				{
					mblist.Add(new ValueTuple<Ship, float>(ship, this.GetShipPriority(caravan, ship, false)));
				}
			}
			mblist = mblist.OrderByDescending<ValueTuple<Ship, float>, float>((ValueTuple<Ship, float> x) => x.Item2).ToMBList<ValueTuple<Ship, float>>();
			int idealShipNumber = Campaign.Current.Models.PartyShipLimitModel.GetIdealShipNumber(caravan);
			int num = 0;
			while (num < mblist.Count && caravan.Ships.Count < idealShipNumber)
			{
				Ship item = mblist[num].Item1;
				if ((float)caravan.PartyTradeGold * 0.5f >= Campaign.Current.Models.ShipCostModel.GetShipTradeValue(item, town.Settlement.Party, caravan.Party) && (item.ShipHull.Type != ShipHull.ShipType.Medium || flag))
				{
					ChangeShipOwnerAction.ApplyByTrade(caravan.Party, item);
					if (item.ShipHull.Type == ShipHull.ShipType.Medium)
					{
						flag = false;
					}
				}
				num++;
			}
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x00104312 File Offset: 0x00102512
		private float GetShipPriority(MobileParty convoy, Ship ship, bool isForSelling)
		{
			return Campaign.Current.Models.PartyShipLimitModel.GetShipPriority(convoy, ship, isForSelling);
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x0010432C File Offset: 0x0010252C
		public void OnSettlementLeft(MobileParty mobileParty, Settlement settlement)
		{
			if (mobileParty != null && mobileParty != MobileParty.MainParty && (mobileParty.IsCaravan || mobileParty.IsLordParty))
			{
				int num = mobileParty.InventoryCapacity;
				float num2 = mobileParty.TotalWeightCarried;
				Town town = (settlement.IsTown ? settlement.Town : (settlement.IsVillage ? settlement.Village.Bound.Town : null));
				if (town != null)
				{
					float num3 = 1.1f;
					while (num2 > (float)num)
					{
						this.SellGoods(mobileParty, town, num3, true);
						num3 -= 0.02f;
						if (num3 < 0.75f)
						{
							break;
						}
						num = mobileParty.InventoryCapacity;
						num2 = mobileParty.TotalWeightCarried;
					}
				}
			}
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x001043CC File Offset: 0x001025CC
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (mobileParty.IsCaravan)
			{
				if (this._interactedCaravans.ContainsKey(mobileParty))
				{
					this._interactedCaravans.Remove(mobileParty);
				}
				List<CaravansCampaignBehavior.TradeActionLog> list;
				if (this._tradeActionLogs.TryGetValue(mobileParty, out list))
				{
					this._tradeActionLogs.Remove(mobileParty);
					for (int i = 0; i < list.Count; i++)
					{
						CaravansCampaignBehavior.TradeActionLog tradeActionLog = list[i];
						this._tradeActionLogPool.ReleaseLog(tradeActionLog);
					}
				}
				if (this._caravanLastHomeTownVisitTime.ContainsKey(mobileParty))
				{
					this._caravanLastHomeTownVisitTime.Remove(mobileParty);
				}
			}
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x00104459 File Offset: 0x00102659
		private void OnMobilePartyCreated(MobileParty mobileParty)
		{
			if (mobileParty.IsCaravan)
			{
				this._caravanLastHomeTownVisitTime.Add(mobileParty, CampaignTime.Now);
			}
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x00104474 File Offset: 0x00102674
		private Town ThinkNextDestination(MobileParty caravanParty, out MobileParty.NavigationType bestNavigationType, out bool isFromPort, out bool isTargetingPort)
		{
			this.RefreshTotalValueOfItemsAtCategoryForParty(caravanParty);
			Town town = this.FindNextDestinationForCaravan(caravanParty, true, out bestNavigationType, out isFromPort, out isTargetingPort);
			if (town == null)
			{
				town = this.FindNextDestinationForCaravan(caravanParty, false, out bestNavigationType, out isFromPort, out isTargetingPort);
			}
			return town;
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x001044A8 File Offset: 0x001026A8
		private Town FindNextDestinationForCaravan(MobileParty caravanParty, bool distanceCut, out MobileParty.NavigationType bestNavigationType, out bool isFromPort, out bool isTargetingPort)
		{
			float num = 0f;
			Town town = null;
			bestNavigationType = MobileParty.NavigationType.None;
			isTargetingPort = false;
			float num2 = caravanParty.TotalWeightCarried / (float)caravanParty.InventoryCapacity;
			num2 = MBMath.Map(num2, 0f, 1f, 0f, 0.9f);
			CampaignTime campaignTime;
			this._caravanLastHomeTownVisitTime.TryGetValue(caravanParty, out campaignTime);
			bool hasNavalNavigationCapability = caravanParty.HasNavalNavigationCapability;
			foreach (Town town2 in Town.AllTowns)
			{
				if (town2.Owner.Settlement != caravanParty.CurrentSettlement && (!town2.IsUnderSiege || (!town2.Settlement.SiegeEvent.IsBlockadeActive && hasNavalNavigationCapability)) && this.CanTradeWith(caravanParty.MapFaction, town2.MapFaction) && (town2.Settlement.HasPort || !hasNavalNavigationCapability) && (!town2.Settlement.Parties.Contains(MobileParty.MainParty) || !MobileParty.MainParty.MapFaction.IsAtWarWith(caravanParty.MapFaction)))
				{
					MobileParty.NavigationType navigationType;
					bool flag;
					float tradeScoreForTown = this.GetTradeScoreForTown(caravanParty, town2, campaignTime, num2, distanceCut, out navigationType, out flag);
					if (tradeScoreForTown > num)
					{
						num = tradeScoreForTown;
						town = town2;
						isTargetingPort = flag;
						bestNavigationType = navigationType;
					}
				}
			}
			isFromPort = isTargetingPort && caravanParty.CurrentSettlement != null;
			return town;
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x00104618 File Offset: 0x00102818
		private void AdjustVeryFarAddition(bool isNavalCaravan, float distanceAsDays, float minimumAddition, ref float veryFarAddition)
		{
			float distanceLimitVeryFarAsDaysForNavigationType = this.GetDistanceLimitVeryFarAsDaysForNavigationType(isNavalCaravan);
			if (distanceAsDays > distanceLimitVeryFarAsDaysForNavigationType)
			{
				veryFarAddition += (distanceAsDays - distanceLimitVeryFarAsDaysForNavigationType) * minimumAddition * 4f;
			}
			float distanceLimitFarAsDaysForNavigationType = this.GetDistanceLimitFarAsDaysForNavigationType(isNavalCaravan);
			if (distanceAsDays > distanceLimitFarAsDaysForNavigationType)
			{
				veryFarAddition += (distanceAsDays - distanceLimitFarAsDaysForNavigationType) * minimumAddition * 3f;
			}
			float distanceLimitMediumAsDaysForNavigationType = this.GetDistanceLimitMediumAsDaysForNavigationType(isNavalCaravan);
			if (distanceAsDays > distanceLimitMediumAsDaysForNavigationType)
			{
				veryFarAddition += (distanceAsDays - distanceLimitMediumAsDaysForNavigationType) * minimumAddition * 2f;
			}
			float distanceLimitCloseAsDaysForNavigationType = this.GetDistanceLimitCloseAsDaysForNavigationType(isNavalCaravan);
			if (distanceAsDays > distanceLimitCloseAsDaysForNavigationType)
			{
				veryFarAddition += (distanceAsDays - distanceLimitCloseAsDaysForNavigationType) * minimumAddition;
			}
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x00104698 File Offset: 0x00102898
		private float GetTradeScoreForTown(MobileParty caravanParty, Town town, CampaignTime lastHomeVisitTimeOfCaravan, float caravanFullness, bool distanceCut, out MobileParty.NavigationType bestNavigationType, out bool isTargetingPort)
		{
			bool hasNavalNavigationCapability = caravanParty.HasNavalNavigationCapability;
			isTargetingPort = hasNavalNavigationCapability;
			float num;
			bool flag;
			AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(caravanParty, town.Settlement, isTargetingPort, out bestNavigationType, out num, out flag);
			if (bestNavigationType == MobileParty.NavigationType.None)
			{
				bestNavigationType = MobileParty.NavigationType.None;
				isTargetingPort = false;
				return -1f;
			}
			float num2 = num / ((hasNavalNavigationCapability ? Campaign.Current.EstimatedAverageCaravanPartyNavalSpeed : Campaign.Current.EstimatedAverageCaravanPartySpeed) * (float)CampaignTime.HoursInDay);
			float num3 = 0f;
			this.AdjustVeryFarAddition(hasNavalNavigationCapability, num2, 0.15f, ref num3);
			float elapsedDaysUntilNow = lastHomeVisitTimeOfCaravan.ElapsedDaysUntilNow;
			bool flag2 = elapsedDaysUntilNow > this.GetDistanceLimitVeryFarAsDaysForNavigationType(hasNavalNavigationCapability);
			if (flag2)
			{
				float num4 = num / ((hasNavalNavigationCapability ? Campaign.Current.EstimatedAverageCaravanPartyNavalSpeed : Campaign.Current.EstimatedAverageCaravanPartySpeed) * (float)CampaignTime.HoursInDay);
				this.AdjustVeryFarAddition(hasNavalNavigationCapability, num4, ((elapsedDaysUntilNow - 1f) * MathF.Sqrt(elapsedDaysUntilNow - 1f) - 1f) * 0.008f, ref num3);
			}
			ExplainedNumber explainedNumber = default(ExplainedNumber);
			town.AddEffectOfBuildings(BuildingEffectEnum.CaravanAccessibility, ref explainedNumber);
			float num5 = Math.Max(1f, explainedNumber.ResultNumber);
			float distanceLimitVeryFarAsDaysForNavigationType = this.GetDistanceLimitVeryFarAsDaysForNavigationType(hasNavalNavigationCapability);
			float num6 = num2 + num3;
			if (distanceCut && (town.Owner.Settlement != caravanParty.HomeSettlement || !flag2) && num6 > distanceLimitVeryFarAsDaysForNavigationType)
			{
				bestNavigationType = MobileParty.NavigationType.None;
				isTargetingPort = false;
				return -1f;
			}
			float num7 = (hasNavalNavigationCapability ? MathF.Max(0.1f, 1f - num6 / (2f * distanceLimitVeryFarAsDaysForNavigationType)) : (1f / num6));
			float num8 = 1f;
			if (caravanParty.HomeSettlement == town.Owner.Settlement)
			{
				num8 = 1f + elapsedDaysUntilNow * 0.1f * (elapsedDaysUntilNow * 0.1f);
				if (num7 < 0.5f)
				{
					num7 = 0.5f;
				}
			}
			TownMarketData marketData = town.MarketData;
			float num9 = 1.1f;
			float num10 = 0f;
			for (int i = 0; i < caravanParty.Party.ItemRoster.Count; i++)
			{
				ItemObject item = caravanParty.ItemRoster.GetElementCopyAtIndex(i).EquipmentElement.Item;
				float num11 = num9 - MathF.Sqrt((float)MathF.Min(this._totalValueOfItemsAtCategory[item.ItemCategory], 5000) / 5000f) * 0.2f;
				num10 += this.CalculateTownSellScoreForCategory(caravanParty, marketData, i, num11);
			}
			num10 *= (hasNavalNavigationCapability ? 0.5f : 0.3f) + caravanFullness;
			float num12 = 0f;
			for (int j = 0; j < ItemCategories.All.Count; j++)
			{
				ItemCategory itemCategory = ItemCategories.All[j];
				if (itemCategory.IsTradeGood || itemCategory.IsAnimal)
				{
					num12 += this.CalculateTownBuyScoreForCategory(marketData, j, caravanParty);
				}
			}
			num12 *= MathF.Max(0.1f, 1f - 2f * (caravanFullness - (hasNavalNavigationCapability ? 0.5f : 0.3f) * MathF.Min(num10, 1000f) / 1000f));
			num12 = MathF.Min(num12, (float)((int)(0.5f * (float)caravanParty.PartyTradeGold)));
			float num13 = ((caravanParty.IsCurrentlyUsedByAQuest && town.Settlement == caravanParty.HomeSettlement && caravanParty.Position.Distance(caravanParty.HomeSettlement.Position) < Campaign.Current.Models.EncounterModel.NeededMaximumDistanceForEncounteringTown * 5f) ? 0.1f : 1f);
			float num14 = 1f;
			float num15 = ((town.Security >= 75f) ? (1f + MathF.Clamp((town.Security - 75f) * 0.002f, 0f, 0.05f)) : 1f);
			float num16 = ((caravanParty.Owner != null) ? caravanParty.Owner.RandomFloat(1f, 1.03f) : 1f);
			float num17 = 1f;
			TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement;
			if (this.TradeAgreementsCampaignBehavior != null && caravanParty.MapFaction.IsKingdomFaction && town.MapFaction.IsKingdomFaction && this.TradeAgreementsCampaignBehavior.HasTradeAgreement((Kingdom)caravanParty.MapFaction, (Kingdom)town.MapFaction, out tradeAgreement))
			{
				num17 = (hasNavalNavigationCapability ? 1.5f : 2f);
			}
			return (num10 + num12) * num7 * num17 * num8 * num13 * num14 * num15 * num16 * num5;
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x00104AEC File Offset: 0x00102CEC
		private float CalculateTownSellScoreForCategory(MobileParty party, TownMarketData marketData, int i, float limitValue)
		{
			ItemRosterElement itemRosterElement = party.Party.ItemRoster[i];
			ItemCategory itemCategory = itemRosterElement.EquipmentElement.Item.ItemCategory;
			CaravansCampaignBehavior.PriceIndexData priceIndexData;
			this.GetCategoryPriceData(itemCategory, party, out priceIndexData);
			float num = marketData.GetPriceFactor(itemCategory) - priceIndexData.AverageBuySellPriceIndex * limitValue;
			if (num > 0f)
			{
				int num2 = ((itemRosterElement.EquipmentElement.Item.ItemCategory != DefaultItemCategories.PackAnimal || !party.HasLandNavigationCapability) ? itemRosterElement.Amount : MathF.Max(0, itemRosterElement.Amount - party.MemberRoster.TotalManCount));
				float num3 = ((itemCategory.Properties == ItemCategory.Property.BonusToFoodStores) ? 1.1f : 1f);
				return num * num3 * (float)MathF.Min(4000, itemRosterElement.EquipmentElement.Item.Value * num2);
			}
			return 0f;
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x00104BD6 File Offset: 0x00102DD6
		private void SetPlayerInteraction(MobileParty mobileParty, CaravansCampaignBehavior.PlayerInteraction interaction)
		{
			if (this._interactedCaravans.ContainsKey(mobileParty))
			{
				this._interactedCaravans[mobileParty] = interaction;
				return;
			}
			this._interactedCaravans.Add(mobileParty, interaction);
		}

		// Token: 0x06003DF6 RID: 15862 RVA: 0x00104C04 File Offset: 0x00102E04
		private CaravansCampaignBehavior.PlayerInteraction GetPlayerInteraction(MobileParty mobileParty)
		{
			CaravansCampaignBehavior.PlayerInteraction playerInteraction;
			if (this._interactedCaravans.TryGetValue(mobileParty, out playerInteraction))
			{
				return playerInteraction;
			}
			return CaravansCampaignBehavior.PlayerInteraction.None;
		}

		// Token: 0x06003DF7 RID: 15863 RVA: 0x00104C24 File Offset: 0x00102E24
		private float CalculateTownBuyScoreForCategory(TownMarketData marketData, int categoryIndex, MobileParty mobileParty)
		{
			ItemCategory itemCategory = ItemCategories.All[categoryIndex];
			CaravansCampaignBehavior.PriceIndexData priceIndexData;
			this.GetCategoryPriceData(itemCategory, mobileParty, out priceIndexData);
			float priceFactor = marketData.GetPriceFactor(itemCategory);
			float num = priceIndexData.AverageBuySellPriceIndex / priceFactor;
			float num2 = num * num - 1.1f;
			if (num2 > 0f)
			{
				return MathF.Min(MathF.Sqrt(this._averageValuesCached[itemCategory]) * 3f * num2, 0.3f * (float)marketData.GetCategoryData(itemCategory).InStoreValue);
			}
			return 0f;
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x00104CA0 File Offset: 0x00102EA0
		private bool GetCategoryPriceData(ItemCategory category, MobileParty mobileParty, out CaravansCampaignBehavior.PriceIndexData priceIndex)
		{
			bool flag = true;
			if (!(this.ShouldPartyUseCoastalPrices(mobileParty) ? this._coastalPriceDictionary : this._priceDictionary).TryGetValue(category, out priceIndex))
			{
				flag = false;
				priceIndex = new CaravansCampaignBehavior.PriceIndexData(1f, 1f);
			}
			return flag;
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x00104CE8 File Offset: 0x00102EE8
		private void RefreshTotalValueOfItemsAtCategoryForParty(MobileParty caravanParty)
		{
			this._totalValueOfItemsAtCategory.Clear();
			for (int i = 0; i < caravanParty.ItemRoster.Count; i++)
			{
				ItemRosterElement elementCopyAtIndex = caravanParty.ItemRoster.GetElementCopyAtIndex(i);
				ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
				int num = elementCopyAtIndex.Amount * (item.Value + 10);
				int num2;
				if (this._totalValueOfItemsAtCategory.TryGetValue(item.ItemCategory, out num2))
				{
					this._totalValueOfItemsAtCategory[item.ItemCategory] = num2 + num;
				}
				else
				{
					this._totalValueOfItemsAtCategory.Add(item.ItemCategory, num);
				}
			}
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x00104D84 File Offset: 0x00102F84
		private bool ShouldPartyUseCoastalPrices(MobileParty mobileParty)
		{
			return mobileParty.IsCaravan && mobileParty.CaravanPartyComponent.CanHaveNavalNavigationCapability;
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x00104D9C File Offset: 0x00102F9C
		private void SellGoodsInternal(MobileParty mobileParty, Town town, bool sellHorses, List<ValueTuple<EquipmentElement, int>> soldItems, float priceIndexSellLimit = 1.1f, bool toLoseWeight = false)
		{
			int itemAverageWeight = Campaign.Current.Models.InventoryCapacityModel.GetItemAverageWeight();
			this.RefreshTotalValueOfItemsAtCategoryForParty(mobileParty);
			for (int i = mobileParty.ItemRoster.Count - 1; i >= 0; i--)
			{
				int num = (int)((float)mobileParty.ItemRoster.NumberOfPackAnimals - (float)mobileParty.Party.NumberOfAllMembers * 0.6f);
				int num2 = (int)((float)mobileParty.ItemRoster.NumberOfLivestockAnimals - (float)mobileParty.Party.NumberOfAllMembers * 0.6f);
				ItemRosterElement elementCopyAtIndex = mobileParty.ItemRoster.GetElementCopyAtIndex(i);
				ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
				CaravansCampaignBehavior.PriceIndexData priceIndexData;
				if (this.GetCategoryPriceData(item.GetItemCategory(), mobileParty, out priceIndexData) && sellHorses == (item.HasHorseComponent || item.ItemCategory == DefaultItemCategories.PackAnimal) && (!toLoseWeight || !item.HasHorseComponent || !mobileParty.HasLandNavigationCapability))
				{
					bool flag = item.ItemCategory == DefaultItemCategories.PackAnimal;
					if (!flag || num > 0 || !mobileParty.HasLandNavigationCapability)
					{
						float priceFactor = town.MarketData.GetPriceFactor(item.ItemCategory);
						float num3 = priceFactor / priceIndexData.AverageBuySellPriceIndex;
						float num4 = priceIndexSellLimit - (Campaign.Current.GameStarted ? (MathF.Sqrt((float)MathF.Min(this._totalValueOfItemsAtCategory[item.ItemCategory], 5000) / 5000f) * 0.4f) : 0f);
						bool flag2 = num > 0 && flag;
						bool flag3 = num2 > 0 && item.HorseComponent != null && item.HorseComponent.IsLiveStock;
						if (num3 >= num4 || (mobileParty.HasLandNavigationCapability && (flag3 || flag2)))
						{
							float num5 = 0.8f * priceIndexData.AverageBuySellPriceIndex + 0.2f * priceIndexData.MinBuySellPriceIndex;
							if (priceFactor >= num5 * num4 || (mobileParty.HasLandNavigationCapability && (flag3 || flag2)))
							{
								float num6 = priceFactor - num5 * num4;
								float demand = town.MarketData.GetDemand(item.ItemCategory);
								float num7 = Campaign.Current.Models.SettlementEconomyModel.CalculateDailySettlementBudgetForItemCategory(town, demand, item.ItemCategory) + (float)(2 * item.Value);
								int itemPrice = town.GetItemPrice(item, mobileParty, true);
								float num8 = ((item.ItemCategory == DefaultItemCategories.PackAnimal) ? 1.5f : 1f);
								float num9 = (mobileParty.HasNavalNavigationCapability ? 5f : 3f);
								float num10 = num7 * num6 * num3 * num8 * num9;
								if (num10 > 0f || flag3 || flag2)
								{
									int num11 = MBRandom.RoundRandomized(num10 / (float)itemPrice);
									if (mobileParty.HasLandNavigationCapability)
									{
										if (flag2)
										{
											num11 = num;
										}
										else if (flag3)
										{
											num11 = num2;
										}
									}
									int amount = elementCopyAtIndex.Amount;
									if (num11 > amount)
									{
										num11 = amount;
									}
									if (num11 * itemPrice > town.Gold)
									{
										num11 = town.Gold / itemPrice;
									}
									if (toLoseWeight && mobileParty.TotalWeightCarried - (float)(num11 * itemAverageWeight) < (float)mobileParty.InventoryCapacity)
									{
										num11 = (int)((mobileParty.TotalWeightCarried - (float)mobileParty.InventoryCapacity) / (float)itemAverageWeight + 0.99f);
									}
									if (num11 > elementCopyAtIndex.Amount)
									{
										num11 = elementCopyAtIndex.Amount;
									}
									if (num11 * itemPrice > town.Gold)
									{
										num11 = town.Gold / itemPrice;
									}
									if (num11 > 0)
									{
										soldItems.Add(new ValueTuple<EquipmentElement, int>(elementCopyAtIndex.EquipmentElement, num11));
										if (Campaign.Current.GameStarted)
										{
											this.OnSellItems(mobileParty, elementCopyAtIndex, town);
										}
										SellItemsAction.Apply(mobileParty.Party, town.Owner, elementCopyAtIndex, num11, town.Owner.Settlement);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x00105140 File Offset: 0x00103340
		private void SellGoods(MobileParty mobileParty, Town town, float priceIndexSellLimit = 1.1f, bool toLoseWeight = false)
		{
			this.RefreshTotalValueOfItemsAtCategoryForParty(mobileParty);
			List<ValueTuple<EquipmentElement, int>> list = new List<ValueTuple<EquipmentElement, int>>();
			this.SellGoodsInternal(mobileParty, town, false, list, priceIndexSellLimit, toLoseWeight);
			this.SellGoodsInternal(mobileParty, town, true, list, priceIndexSellLimit, toLoseWeight);
			if (!list.IsEmpty<ValueTuple<EquipmentElement, int>>() && mobileParty.IsCaravan)
			{
				CampaignEventDispatcher.Instance.OnCaravanTransactionCompleted(mobileParty, town, list);
			}
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x00105194 File Offset: 0x00103394
		private void OnSellItems(MobileParty caravanParty, ItemRosterElement rosterElement, Town soldTown)
		{
			int itemPrice = soldTown.GetItemPrice(rosterElement.EquipmentElement.Item, caravanParty, true);
			List<CaravansCampaignBehavior.TradeActionLog> list;
			if (this._tradeActionLogs.TryGetValue(caravanParty, out list))
			{
				foreach (CaravansCampaignBehavior.TradeActionLog tradeActionLog in list)
				{
					if (tradeActionLog.ItemRosterElement.EquipmentElement.Item == rosterElement.EquipmentElement.Item && itemPrice > tradeActionLog.SellPrice)
					{
						tradeActionLog.OnSellAction(soldTown.Settlement, itemPrice);
					}
				}
			}
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x00105244 File Offset: 0x00103444
		private void BuyGoods(MobileParty caravanParty, Town town)
		{
			CaravansCampaignBehavior.<>c__DisplayClass80_0 CS$<>8__locals1 = new CaravansCampaignBehavior.<>c__DisplayClass80_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.town = town;
			CS$<>8__locals1.caravanParty = caravanParty;
			List<ValueTuple<EquipmentElement, int>> list = new List<ValueTuple<EquipmentElement, int>>();
			float capacityFactor = this.CalculateCapacityFactor(CS$<>8__locals1.caravanParty);
			float budgetFactor = this.CalculateBudgetFactor(CS$<>8__locals1.caravanParty);
			this.RefreshTotalValueOfItemsAtCategoryForParty(CS$<>8__locals1.caravanParty);
			MBList<ItemCategory> mblist = ItemCategories.All.OrderByDescending<ItemCategory, float>((ItemCategory x) => CS$<>8__locals1.<>4__this.CalculateBuyValue(x, CS$<>8__locals1.town, CS$<>8__locals1.caravanParty, budgetFactor, capacityFactor)).ToMBList<ItemCategory>();
			int num = (CS$<>8__locals1.caravanParty.HasNavalNavigationCapability ? 10 : 5);
			for (int i = 0; i < num; i++)
			{
				this.BuyCategory(CS$<>8__locals1.caravanParty, CS$<>8__locals1.town, mblist[i], budgetFactor, capacityFactor, list);
			}
			if (CS$<>8__locals1.caravanParty.HasNavalNavigationCapability)
			{
				this.BuyCategory(CS$<>8__locals1.caravanParty, CS$<>8__locals1.town, DefaultItemCategories.Grain, budgetFactor, capacityFactor, list);
				this.BuyCategory(CS$<>8__locals1.caravanParty, CS$<>8__locals1.town, DefaultItemCategories.Fish, budgetFactor, capacityFactor, list);
			}
			else if ((float)(CS$<>8__locals1.caravanParty.ItemRoster.NumberOfPackAnimals + CS$<>8__locals1.caravanParty.ItemRoster.NumberOfLivestockAnimals) < (float)CS$<>8__locals1.caravanParty.Party.NumberOfAllMembers * 2f && CS$<>8__locals1.caravanParty.ItemRoster.NumberOfPackAnimals < CS$<>8__locals1.caravanParty.Party.NumberOfAllMembers && this._packAnimalCategoryIndex >= 0 && CS$<>8__locals1.caravanParty.PartyTradeGold > 1000)
			{
				this.BuyCategory(CS$<>8__locals1.caravanParty, CS$<>8__locals1.town, DefaultItemCategories.PackAnimal, budgetFactor, capacityFactor, list);
			}
			if (!list.IsEmpty<ValueTuple<EquipmentElement, int>>())
			{
				CampaignEventDispatcher.Instance.OnCaravanTransactionCompleted(CS$<>8__locals1.caravanParty, CS$<>8__locals1.town, list);
			}
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x00105495 File Offset: 0x00103695
		private float CalculateBudgetFactor(MobileParty caravanParty)
		{
			return 0.1f + MathF.Clamp((float)caravanParty.PartyTradeGold / 5000f, 0f, 1f);
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x001054BC File Offset: 0x001036BC
		private float CalculateCapacityFactor(MobileParty caravanParty)
		{
			float num = caravanParty.TotalWeightCarried / ((float)caravanParty.InventoryCapacity + 1f);
			num *= 0.9f;
			return 1.1f - MathF.Clamp(num, 0f, 1f);
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x001054FC File Offset: 0x001036FC
		private void BuyCategory(MobileParty caravanParty, Town town, ItemCategory category, float budgetFactor, float capacityFactor, List<ValueTuple<EquipmentElement, int>> boughtItems)
		{
			float num = this.CalculateBuyValue(category, town, caravanParty, budgetFactor, capacityFactor);
			if (num < 7f)
			{
				return;
			}
			if (caravanParty.TotalWeightCarried / (float)caravanParty.InventoryCapacity > 0.9f && !category.IsAnimal)
			{
				return;
			}
			if (town.MarketData.GetCategoryData(category).InStore == 0)
			{
				return;
			}
			float num2 = MathF.Min(MathF.Min((float)caravanParty.PartyTradeGold * 0.5f, num * 1.5f), (float)Campaign.Current.Models.CaravanModel.GetMaxGoldToSpendOnOneItemCategory(caravanParty, category));
			if (!Campaign.Current.GameStarted)
			{
				num2 *= 0.5f;
			}
			float num3 = num2;
			int num4 = 0;
			Predicate<ItemObject> <>9__0;
			for (;;)
			{
				int num5 = 0;
				int num6 = (int)(MBRandom.RandomFloat * (float)town.Owner.ItemRoster.Count);
				ItemRoster itemRoster = town.Owner.ItemRoster;
				Predicate<ItemObject> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (ItemObject x) => x.ItemCategory == category);
				}
				int num7 = itemRoster.FindIndexFirstAfterXthElement(predicate, num6);
				if (num7 < 0)
				{
					break;
				}
				ItemRosterElement rosterElement = town.Owner.ItemRoster.GetElementCopyAtIndex(num7);
				ItemObject item = rosterElement.EquipmentElement.Item;
				int itemPrice = town.GetItemPrice(item, caravanParty, false);
				int num8 = MBRandom.RoundRandomized(num3 / (float)itemPrice);
				if (num8 > rosterElement.Amount)
				{
					num8 = rosterElement.Amount;
				}
				if (num8 > this.MaxNumberOfItemsToBuyFromSingleCategory)
				{
					num8 = this.MaxNumberOfItemsToBuyFromSingleCategory;
				}
				if ((!category.IsAnimal || !caravanParty.HasLandNavigationCapability) && caravanParty.TotalWeightCarried + (float)num8 * item.Weight > (float)caravanParty.InventoryCapacity)
				{
					num8 = (int)(((float)caravanParty.InventoryCapacity * 0.9f - caravanParty.TotalWeightCarried) / item.Weight);
				}
				if (caravanParty.HasLandNavigationCapability && rosterElement.EquipmentElement.Item.HorseComponent != null && (rosterElement.EquipmentElement.Item.HorseComponent.IsLiveStock || rosterElement.EquipmentElement.Item.HorseComponent.IsPackAnimal))
				{
					int numberOfPackAnimals = caravanParty.ItemRoster.NumberOfPackAnimals;
					int numberOfLivestockAnimals = caravanParty.ItemRoster.NumberOfLivestockAnimals;
					if (rosterElement.EquipmentElement.Item.HorseComponent.IsLiveStock && (float)(numberOfLivestockAnimals + num8) > (float)caravanParty.Party.NumberOfAllMembers * 0.6f)
					{
						num8 = (int)((float)caravanParty.Party.NumberOfAllMembers * 0.6f) - numberOfLivestockAnimals;
					}
					else if (rosterElement.EquipmentElement.Item.HorseComponent.IsPackAnimal && numberOfPackAnimals + num8 > caravanParty.Party.NumberOfAllMembers)
					{
						num8 = caravanParty.Party.NumberOfAllMembers - numberOfPackAnimals;
					}
				}
				if (num8 > 0)
				{
					SellItemsAction.Apply(town.Owner, caravanParty.Party, rosterElement, num8, town.Owner.Settlement);
					int num9 = boughtItems.FindIndex((ValueTuple<EquipmentElement, int> x) => x.Item1.IsEqualTo(rosterElement.EquipmentElement));
					if (num9 == -1)
					{
						boughtItems.Add(new ValueTuple<EquipmentElement, int>(rosterElement.EquipmentElement, -num8));
					}
					else
					{
						boughtItems[num9] = new ValueTuple<EquipmentElement, int>(rosterElement.EquipmentElement, -num8 + boughtItems[num9].Item2);
					}
					num4 += num8;
					num3 -= (float)(num8 * itemPrice + 1);
					num5 = num8 * itemPrice;
					Town destinationForMobileParty = this.GetDestinationForMobileParty(caravanParty);
					if (caravanParty.LastVisitedSettlement != null && destinationForMobileParty != null && Campaign.Current.GameStarted)
					{
						List<CaravansCampaignBehavior.TradeActionLog> list;
						if (!this._tradeActionLogs.TryGetValue(caravanParty, out list))
						{
							list = new List<CaravansCampaignBehavior.TradeActionLog>();
							this._tradeActionLogs.Add(caravanParty, list);
						}
						int itemPrice2 = town.GetItemPrice(rosterElement.EquipmentElement, caravanParty, false);
						list.Add(this._tradeActionLogPool.CreateNewLog(town.Settlement, itemPrice2, rosterElement));
					}
				}
				if (num3 <= 0f || num5 <= 0 || num4 >= Campaign.Current.Models.CaravanModel.MaxNumberOfItemsToBuyFromSingleCategory)
				{
					return;
				}
			}
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x00105958 File Offset: 0x00103B58
		private int CaravanTotalValue(MobileParty caravanParty)
		{
			float num = 0f;
			for (int i = 0; i < caravanParty.ItemRoster.Count; i++)
			{
				ItemRosterElement itemRosterElement = caravanParty.ItemRoster[i];
				num += this.GetGlobalItemSellPrice(itemRosterElement.EquipmentElement.Item, caravanParty) * (float)itemRosterElement.Amount;
			}
			return (int)num + caravanParty.PartyTradeGold;
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x001059BC File Offset: 0x00103BBC
		private float CalculateBuyValue(ItemCategory category, Town town, MobileParty caravanParty, float budgetFactor, float capacityFactor)
		{
			if (!category.IsTradeGood && !category.IsAnimal)
			{
				return 0f;
			}
			CaravansCampaignBehavior.PriceIndexData priceIndexData;
			if (!this.GetCategoryPriceData(category, caravanParty, out priceIndexData))
			{
				return 0f;
			}
			if (town.MarketData.GetItemCountOfCategory(category) == 0)
			{
				return 0f;
			}
			float num = 0f;
			if (Campaign.Current.GameStarted && this._totalValueOfItemsAtCategory.ContainsKey(category))
			{
				num = MathF.Sqrt((float)MathF.Min(this._totalValueOfItemsAtCategory[category], 5000) / 5000f) * 0.4f;
			}
			float itemCategoryPriceIndex = town.GetItemCategoryPriceIndex(category);
			float averageBuySellPriceIndex = priceIndexData.AverageBuySellPriceIndex;
			float num2 = averageBuySellPriceIndex * (1f - num) - itemCategoryPriceIndex;
			if (num2 < 0f)
			{
				return 0f;
			}
			float demand = town.MarketData.GetDemand(category);
			float num3 = 0.1f * MathF.Pow(demand, 0.5f);
			float num4 = num2 * this._averageValuesCached[category];
			float num5 = num2 * 200f;
			float num6 = averageBuySellPriceIndex / itemCategoryPriceIndex;
			float num7 = ((category.Properties == ItemCategory.Property.BonusToFoodStores) ? 1.1f : 1f);
			return ((category == DefaultItemCategories.PackAnimal && caravanParty.HasLandNavigationCapability) ? 1.5f : 1f) * num7 * num6 * num3 * (num4 * budgetFactor + num5 * capacityFactor);
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x00105B04 File Offset: 0x00103D04
		private float GetGlobalItemSellPrice(ItemObject item, MobileParty mobileParty)
		{
			CaravansCampaignBehavior.PriceIndexData priceIndexData;
			if (!this.GetCategoryPriceData(item.ItemCategory, mobileParty, out priceIndexData))
			{
				return 1f;
			}
			return priceIndexData.AverageBuySellPriceIndex * (float)item.Value;
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x00105B38 File Offset: 0x00103D38
		private List<Kingdom> GetSuitableKingdomsAsTradePartnerForPlayerCaravans()
		{
			List<Kingdom> list = new List<Kingdom>();
			foreach (Kingdom kingdom in Campaign.Current.Kingdoms)
			{
				if (!kingdom.IsEliminated)
				{
					list.Add(kingdom);
				}
			}
			return list;
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x00105BA0 File Offset: 0x00103DA0
		private List<Kingdom> GetSuitableKingdomsForHomeSettlement()
		{
			List<Kingdom> list = new List<Kingdom>();
			foreach (Kingdom kingdom in this.GetSuitableKingdomsAsTradePartnerForPlayerCaravans())
			{
				if (this.GetSuitableHomeSettlementsForKingdom(kingdom).Count > 0)
				{
					list.Add(kingdom);
				}
			}
			return list;
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x00105C0C File Offset: 0x00103E0C
		private List<Settlement> GetSuitableHomeSettlementsForKingdom(Kingdom kingdom)
		{
			List<Settlement> list = new List<Settlement>();
			MobileParty conversationParty = MobileParty.ConversationParty;
			bool hasNavalNavigationCapability = conversationParty.HasNavalNavigationCapability;
			foreach (Settlement settlement in kingdom.Settlements)
			{
				if (settlement.IsTown && settlement != conversationParty.HomeSettlement && (!hasNavalNavigationCapability || settlement.HasPort))
				{
					list.Add(settlement);
				}
			}
			return list;
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x00105C94 File Offset: 0x00103E94
		protected void AddDialogs(CampaignGameStarter starter)
		{
			starter.AddPlayerLine("caravan_companion_talk_start", "hero_main_options", "caravan_companion_talk_start", "{=q0RY0dQG}We need to talk business.", new ConversationSentence.OnConditionDelegate(this.companion_is_caravan_leader_on_condition), null, 100, null, null);
			starter.AddDialogLine("caravan_companion_talk_start_reply", "caravan_companion_talk_start", "caravan_companion_talk_start_reply", "{=9RiXgPc1}Certainly. What do you need to know?", null, null, 100, null);
			starter.AddPlayerLine("caravan_companion_change_home_settlement", "caravan_companion_talk_start_reply", "caravan_companion_ask_change_home_settlement", "{=dMQ1u6l2}I would like you to start trading out of a different town.", new ConversationSentence.OnConditionDelegate(this.caravan_companion_change_home_settlement_on_condition), new ConversationSentence.OnConsequenceDelegate(this.caravan_companion_change_home_settlement_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.caravan_companion_change_home_settlement_clickable_condition), null);
			starter.AddPlayerLine("caravan_companion_prohibit_kingdoms", "caravan_companion_talk_start_reply", "caravan_companion_prohibit_kingdoms_selected", "{=5LhfbFpX}Let's discuss our trade partners.", new ConversationSentence.OnConditionDelegate(this.caravan_companion_prohibit_kingdoms_on_condition), new ConversationSentence.OnConsequenceDelegate(this.caravan_companion_prohibit_kingdoms_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.caravan_companion_prohibit_kingdoms_clickable_condition), null);
			starter.AddPlayerLine("caravan_companion_trade_rumors", "caravan_companion_talk_start_reply", "caravan_companion_ask_trade_rumors", "{=oMuxr3X6}What news of the markets? Any good deals to be had?", null, null, 100, null, null);
			starter.AddDialogLine("caravan_companion_ask_trade_rumors", "caravan_companion_ask_trade_rumors", "caravan_companion_anything_else", "{=sC4ZLZ8x}{COMMENT}", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_ask_trade_rumors_on_consequence), 100, null);
			starter.AddDialogLine("caravan_companion_talk_player_thank", "caravan_companion_anything_else", "caravan_companion_talk_end", "{=DQBaaC0e}Is there anything else?", null, null, 100, null);
			starter.AddPlayerLine("caravan_companion_talk_not_leave", "caravan_companion_talk_end", "lord_pretalk", "{=i2FwKPmC}Yes, I wanted to talk about something else..", null, null, 100, null, null);
			starter.AddPlayerLine("caravan_companion_talk_leave", "caravan_companion_talk_end", "close_window", "{=1IJouNaM}Carry on, then. Farewell.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_player_leave_encounter_on_consequence), 100, null, null);
			starter.AddPlayerLine("caravan_companion_nevermind", "caravan_companion_talk_start_reply", "lord_pretalk", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			starter.AddDialogLine("caravan_companion_ask_change_home_settlement", "caravan_companion_ask_change_home_settlement", "caravan_companion_ask_change_home_settlement_2", "{=8oJYCDbO}Certainly. We currently trade out of {HOME_SETTLEMENT}. In which realm shall we make our new home?", new ConversationSentence.OnConditionDelegate(this.caravan_companion_ask_change_home_settlement_2_on_condition), null, 100, null);
			starter.AddRepeatablePlayerLine("caravan_companion_ask_change_home_settlement_2", "caravan_companion_ask_change_home_settlement_2", "caravan_companion_ask_change_home_settlement_3", "{=!}{KINGDOM_NAME}", "{=bKqka5Uj}I am thinking of a different realm.", "caravan_companion_ask_change_home_settlement", new ConversationSentence.OnConditionDelegate(this.caravan_companion_ask_change_home_settlement_3_on_condition), new ConversationSentence.OnConsequenceDelegate(this.caravan_companion_ask_change_home_settlement_3_on_consequence), 100, null);
			starter.AddDialogLine("caravan_companion_ask_change_home_settlement_3", "caravan_companion_ask_change_home_settlement_3", "caravan_companion_ask_change_home_settlement_4", "{=SuQSy6g2}And which town there did you have in mind?", null, null, 100, null);
			starter.AddRepeatablePlayerLine("caravan_companion_ask_change_home_settlement_4", "caravan_companion_ask_change_home_settlement_4", "caravan_companion_change_home_settlement_end", "{=!}{SETTLEMENT.NAME}", "{=aE4JAqn0}I am thinking of a different settlement.", "caravan_companion_ask_change_home_settlement_3", new ConversationSentence.OnConditionDelegate(this.caravan_companion_ask_change_home_settlement_4_on_condition), new ConversationSentence.OnConsequenceDelegate(this.caravan_companion_ask_change_home_settlement_4_on_consequence), 100, null);
			starter.AddPlayerLine("caravan_companion_ask_change_home_settlement_cancel", "caravan_companion_ask_change_home_settlement_2", "lord_pretalk", "{=PznWhAdU}Actually, never mind.", null, null, 100, null, null);
			starter.AddPlayerLine("caravan_companion_ask_change_home_settlement_cancel_2", "caravan_companion_ask_change_home_settlement_4", "lord_pretalk", "{=PznWhAdU}Actually, never mind.", null, null, 100, null, null);
			starter.AddDialogLine("caravan_companion_change_home_settlement_end", "caravan_companion_change_home_settlement_end", "caravan_companion_anything_else", "{=cWwYmaw7}Understood. {NEW_HOME_SETTLEMENT_LINE}", new ConversationSentence.OnConditionDelegate(this.caravan_companion_change_home_settlement_end_on_condition), null, 100, null);
			starter.AddDialogLine("caravan_companion_prohibit_kingdoms_selected", "caravan_companion_prohibit_kingdoms_selected", "caravan_companion_prohibit_kingdoms_selected_2", "{=cK0yctTy}Certainly. {DESPITE_WAR}", null, null, 100, null);
			starter.AddRepeatablePlayerLine("caravan_companion_prohibit_kingdoms_selected_2", "caravan_companion_prohibit_kingdoms_selected_2", "caravan_companion_prohibit_kingdoms_selected", "{=!}{CONTINUE_OR_STOP_TRADE}", "{=bKqka5Uj}I am thinking of a different realm.", "caravan_companion_prohibit_kingdoms_selected", new ConversationSentence.OnConditionDelegate(this.caravan_companion_prohibit_kingdoms_selected_2_on_condition), new ConversationSentence.OnConsequenceDelegate(this.caravan_companion_prohibit_kingdoms_selected_2_on_consequence), 100, null);
			starter.AddPlayerLine("caravan_companion_prohibit_kingdoms_selected_cancel", "caravan_companion_prohibit_kingdoms_selected_2", "lord_pretalk", "{=FM7YZaOa}Alright, that is all.", null, null, 100, null, null);
			starter.AddDialogLine("player_caravan_talk_start", "start", "player_caravan_talk_start", "{=BsVXQEhj}How may I help you?", new ConversationSentence.OnConditionDelegate(this.player_caravan_talk_start_on_condition), null, 100, null);
			starter.AddPlayerLine("player_caravan_trade_rumors", "player_caravan_talk_start", "player_caravan_ask_trade_rumors", "{=shNl2Npf}What news of the markets?", null, null, 100, null, null);
			starter.AddDialogLine("player_caravan_ask_trade_rumors", "player_caravan_ask_trade_rumors", "player_caravan_anything_else", "{=sC4ZLZ8x}{COMMENT}", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_ask_trade_rumors_on_consequence), 100, null);
			starter.AddDialogLine("player_caravan_talk_player_thank", "player_caravan_anything_else", "player_caravan_talk_end", "{=DQBaaC0e}Is there anything else?", null, null, 100, null);
			starter.AddPlayerLine("player_caravan_talk_not_leave", "player_caravan_talk_end", "start", "{=i2FwKPmC}Yes, I wanted to talk about something else..", null, null, 100, null, null);
			starter.AddPlayerLine("player_caravan_talk_leave", "player_caravan_talk_end", "close_window", "{=1IJouNaM}Carry on, then. Farewell.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_player_leave_encounter_on_consequence), 100, null, null);
			starter.AddPlayerLine("player_caravan_nevermind", "player_caravan_talk_start", "close_window", "{=D33fIGQe}Never mind.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_player_leave_encounter_on_consequence), 100, null, null);
			starter.AddDialogLine("caravan_hero_leader_talk_start", "start", "caravan_talk", "{=!}{CARAVAN_GREETING}", new ConversationSentence.OnConditionDelegate(this.caravan_start_talk_on_condition), null, 100, null);
			starter.AddDialogLine("caravan_pretalk", "caravan_pretalk", "caravan_talk", "{=3cBfSJOI}Is there anything else?[ib:normal]", null, null, 100, null);
			starter.AddPlayerLine("caravan_buy_products", "caravan_talk", "caravan_player_trade", "{=t0UGXPV4}I'm interested in trading. What kind of products do you have?", new ConversationSentence.OnConditionDelegate(this.caravan_buy_products_on_condition), null, 100, null, null);
			starter.AddPlayerLine("caravan_trade_rumors", "caravan_talk", "caravan_ask_trade_rumors", "{=b5Ucatkb}Tell me about your journeys. What news of the markets?", new ConversationSentence.OnConditionDelegate(this.caravan_ask_trade_rumors_on_condition), null, 100, null, null);
			starter.AddDialogLine("caravan_ask_trade_rumors", "caravan_ask_trade_rumors", "caravan_trade_rumors_player_answer", "{=sC4ZLZ8x}{COMMENT}", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_ask_trade_rumors_on_consequence), 100, null);
			starter.AddPlayerLine("caravan_trade_rumors_player_answer", "caravan_trade_rumors_player_answer", "caravan_talk_player_thank", "{=ha7EmrU9}Thank you for that information.", null, null, 100, null, null);
			starter.AddDialogLine("caravan_talk_player_thank", "caravan_talk_player_thank", "caravan_talk", "{=BQuVWKvq}You're welcome. Is there anything we need to discuss?", null, null, 100, null);
			starter.AddPlayerLine("caravan_loot", "caravan_talk", "caravan_loot_talk", "{=WOBy5UfY}Hand over your goods, or die!", new ConversationSentence.OnConditionDelegate(this.caravan_loot_on_condition), null, 100, new ConversationSentence.OnClickableConditionDelegate(this.caravan_loot_on_clickable_condition), null);
			starter.AddPlayerLine("caravan_talk_leave", "caravan_talk", "close_window", "{=1IJouNaM}Carry on, then. Farewell.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_talk_leave_on_consequence), 100, null, null);
			starter.AddDialogLine("caravan_player_trade_end", "caravan_player_trade", "caravan_pretalk", "{=tlLDHAIu}Very well. A pleasure doing business with you.[rf:convo_relaxed_happy][ib:demure]", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_player_trade_end_on_condition), null, 100, null);
			starter.AddDialogLine("caravan_player_trade_end_response", "caravan_player_trade_response", "close_window", "{=2g2FhKb5}Farewell.", null, null, 100, null);
			starter.AddDialogLine("caravan_fight", "caravan_loot_talk", "caravan_do_not_bribe", "{=!}{CARAVAN_DEFIANCE}", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_not_bribe_on_condition), null, 100, null);
			starter.AddPlayerLine("player_decided_to_fight", "caravan_do_not_bribe", "close_window", "{=EhxS7NQ4}So be it. Attack!", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_fight_on_consequence), 100, null, null);
			starter.AddPlayerLine("player_decided_to_not_fight_1", "caravan_do_not_bribe", "close_window", "{=bfPsE9M1}You must have misunderstood me. Go in peace.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_talk_leave_on_consequence), 100, null, null);
			starter.AddDialogLine("caravan_accepted_to_give_some_goods", "caravan_loot_talk", "caravan_give_some_goods", "{=dMc3SjOK}We can pay you. {TAKE_MONEY_AND_PRODUCT_STRING}[rf:idle_angry][ib:nervous]", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_give_goods_on_condition), null, 100, null);
			starter.AddPlayerLine("player_decided_to_take_some_goods", "caravan_give_some_goods", "caravan_end_talk_bribe", "{=0Pd84h4W}I'll accept that.", null, null, 100, delegate(out TextObject explanation)
			{
				if (!Hero.MainHero.MapFaction.IsAtWarWith(MobileParty.ConversationParty.MapFaction))
				{
					explanation = new TextObject("{=nbgyyif6}This action may start a war.", null);
				}
				else
				{
					explanation = null;
				}
				return true;
			}, null);
			starter.AddPlayerLine("player_decided_to_take_everything", "caravan_give_some_goods", "player_wants_everything", "{=QZ6IcCIm}I want everything you've got.", null, null, 100, delegate(out TextObject explanation)
			{
				if (!Hero.MainHero.MapFaction.IsAtWarWith(MobileParty.ConversationParty.MapFaction))
				{
					explanation = new TextObject("{=nbgyyif6}This action may start a war.", null);
				}
				else
				{
					explanation = null;
				}
				return true;
			}, null);
			starter.AddPlayerLine("player_decided_to_not_fight_2", "caravan_give_some_goods", "close_window", "{=bfPsE9M1}You must have misunderstood me. Go in peace.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_talk_leave_on_consequence), 100, null, null);
			starter.AddDialogLine("caravan_fight_no_surrender", "player_wants_everything", "close_window", "{=3JfCwL31}You will have to fight us first![rf:idle_angry][ib:aggressive]", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_not_surrender_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_fight_on_consequence), 100, null);
			starter.AddDialogLine("caravan_accepted_to_give_everything", "player_wants_everything", "player_decision_to_take_prisoners", "{=hbtbSag8}We can't fight you. We surrender. Please don't hurt us. Take what you want.[if:idle_angry][ib:nervous]", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_give_goods_on_condition), null, 100, null);
			starter.AddPlayerLine("player_do_not_take_prisoners", "player_decision_to_take_prisoners", "caravan_end_talk_surrender", "{=6kaia5qP}Give me all your wares!", null, null, 100, delegate(out TextObject explanation)
			{
				if (!Hero.MainHero.MapFaction.IsAtWarWith(MobileParty.ConversationParty.MapFaction))
				{
					explanation = new TextObject("{=nbgyyif6}This action may start a war.", null);
				}
				else
				{
					explanation = null;
				}
				return true;
			}, null);
			starter.AddPlayerLine("player_decided_to_take_prisoner", "player_decision_to_take_prisoners", "caravan_taken_prisoner_warning_check", "{=1gv0AVUN}You are my prisoners now.", null, null, 100, delegate(out TextObject explanation)
			{
				if (!Hero.MainHero.MapFaction.IsAtWarWith(MobileParty.ConversationParty.MapFaction))
				{
					explanation = new TextObject("{=nbgyyif6}This action may start a war.", null);
				}
				else
				{
					explanation = null;
				}
				return true;
			}, null);
			starter.AddPlayerLine("player_decided_to_force_fight", "player_decision_to_take_prisoners", "caravan_force_start_encounter", "{=ha53qb7v}Don't bother pleading for your lives. At them, lads!", null, null, 100, delegate(out TextObject explanation)
			{
				if (!Hero.MainHero.MapFaction.IsAtWarWith(MobileParty.ConversationParty.MapFaction))
				{
					explanation = new TextObject("{=nbgyyif6}This action may start a war.", null);
				}
				else
				{
					explanation = null;
				}
				return true;
			}, null);
			starter.AddDialogLine("caravan_force_fight_encounter", "caravan_force_start_encounter", "close_window", "{=yoWl6w1I}Heaven will avenge us, you butcher!", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_fight_forced_on_consequence), 100, null);
			starter.AddDialogLine("caravan_warn_player_to_take_prisoner", "caravan_taken_prisoner_warning_check", "caravan_taken_prisoner_warning_answer", "{=NuYzgBZB}You are going too far. The {KINGDOM} won't stand for the destruction of its caravans.", new ConversationSentence.OnConditionDelegate(this.conversation_warn_player_on_condition), null, 100, null);
			starter.AddDialogLine("caravan_do_not_warn_player", "caravan_taken_prisoner_warning_check", "close_window", "{=BvytaDUJ}Heaven protect us from the likes of you.", null, delegate
			{
				Campaign.Current.ConversationManager.ConversationEndOneShot += this.player_take_prisoner_consequence;
			}, 100, null);
			starter.AddPlayerLine("player_decided_to_take_prisoner_continue", "caravan_taken_prisoner_warning_answer", "close_window", "{=WVkc4UgX}Continue.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_took_prisoner_on_consequence), 100, null, null);
			starter.AddPlayerLine("player_decided_to_take_prisoner_leave", "caravan_taken_prisoner_warning_answer", "caravan_loot_talk", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			starter.AddDialogLine("caravan_bribery_leave", "caravan_end_talk_bribe", "close_window", "{=uPwKhAps}Can we leave now?", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_looted_leave_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_looted_leave_on_consequence), 100, null);
			starter.AddDialogLine("caravan_surrender_leave", "caravan_end_talk_surrender", "close_window", "{=uPwKhAps}Can we leave now?", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_looted_leave_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_surrender_leave_on_consequence), 100, null);
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x00106681 File Offset: 0x00104881
		private bool caravan_companion_change_home_settlement_on_condition()
		{
			return !MobileParty.ConversationParty.IsCurrentlyUsedByAQuest;
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x00106692 File Offset: 0x00104892
		private bool caravan_companion_change_home_settlement_clickable_condition(out TextObject explanation)
		{
			if (this.GetSuitableKingdomsForHomeSettlement().Count == 0)
			{
				explanation = new TextObject("{=MQM5SxdI}There is no suitable kingdom to select a new home settlement from.", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x001066B4 File Offset: 0x001048B4
		private void caravan_companion_change_home_settlement_on_consequence()
		{
			ConversationSentence.SetObjectsToRepeatOver(this.GetSuitableKingdomsForHomeSettlement(), 5);
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x001066C4 File Offset: 0x001048C4
		private bool caravan_companion_ask_change_home_settlement_2_on_condition()
		{
			MobileParty conversationParty = MobileParty.ConversationParty;
			string text = "HOME_SETTLEMENT";
			Settlement homeSettlement = conversationParty.HomeSettlement;
			MBTextManager.SetTextVariable(text, (homeSettlement != null) ? homeSettlement.Name : null, false);
			return true;
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x001066F8 File Offset: 0x001048F8
		private bool caravan_companion_ask_change_home_settlement_3_on_condition()
		{
			Kingdom kingdom = ConversationSentence.CurrentProcessedRepeatObject as Kingdom;
			if (kingdom != null)
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("KINGDOM_NAME", kingdom.Name);
				return true;
			}
			return false;
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x0010672C File Offset: 0x0010492C
		private void caravan_companion_ask_change_home_settlement_3_on_consequence()
		{
			Kingdom kingdom = ConversationSentence.SelectedRepeatObject as Kingdom;
			ConversationSentence.SetObjectsToRepeatOver(this.GetSuitableHomeSettlementsForKingdom(kingdom), 5);
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x00106754 File Offset: 0x00104954
		private bool caravan_companion_ask_change_home_settlement_4_on_condition()
		{
			Settlement settlement = ConversationSentence.CurrentProcessedRepeatObject as Settlement;
			if (settlement != null)
			{
				StringHelpers.SetSettlementProperties("SETTLEMENT", settlement, null, true);
				return true;
			}
			return false;
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x00106780 File Offset: 0x00104980
		private void caravan_companion_ask_change_home_settlement_4_on_consequence()
		{
			Settlement settlement = ConversationSentence.SelectedRepeatObject as Settlement;
			StringHelpers.SetSettlementProperties("SETTLEMENT", settlement, null, false);
			MobileParty.ConversationParty.CaravanPartyComponent.ChangeHomeSettlement(settlement);
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x001067B8 File Offset: 0x001049B8
		private bool caravan_companion_change_home_settlement_end_on_condition()
		{
			MobileParty conversationParty = MobileParty.ConversationParty;
			TextObject textObject = TextObject.GetEmpty();
			if (conversationParty.HomeSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				textObject = new TextObject("{=Qyqurdka}Although it is inside enemy territory, our new home is {SETTLEMENT_NAME}.", null);
			}
			else
			{
				textObject = new TextObject("{=JIPKLzsx}Our new home is {SETTLEMENT_NAME}.", null);
			}
			textObject.SetTextVariable("SETTLEMENT_NAME", conversationParty.HomeSettlement.Name);
			MBTextManager.SetTextVariable("NEW_HOME_SETTLEMENT_LINE", textObject, false);
			return true;
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x0010682B File Offset: 0x00104A2B
		private bool caravan_companion_prohibit_kingdoms_on_condition()
		{
			return !MobileParty.ConversationParty.IsCurrentlyUsedByAQuest;
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x0010683C File Offset: 0x00104A3C
		private void caravan_companion_prohibit_kingdoms_on_consequence()
		{
			ConversationSentence.SetObjectsToRepeatOver(this.GetSuitableKingdomsAsTradePartnerForPlayerCaravans(), 5);
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x0010684A File Offset: 0x00104A4A
		private bool caravan_companion_prohibit_kingdoms_clickable_condition(out TextObject explanation)
		{
			if (this.GetSuitableKingdomsAsTradePartnerForPlayerCaravans().Count == 0)
			{
				explanation = new TextObject("{=vUmhym4n}There is no suitable kingdom to discuss as a trade partner.", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x0010686C File Offset: 0x00104A6C
		private bool caravan_companion_prohibit_kingdoms_selected_2_on_condition()
		{
			Kingdom kingdom = ConversationSentence.CurrentProcessedRepeatObject as Kingdom;
			if (kingdom != null)
			{
				bool flag = this._prohibitedKingdomsForPlayerCaravans.Contains(kingdom);
				TextObject textObject = TextObject.GetEmpty();
				if (flag)
				{
					textObject = new TextObject("{=1QBbbq4h}Let's continue trading with {KINGDOM_NAME}.", null);
				}
				else
				{
					textObject = new TextObject("{=KsFOH8vo}Let's stop trading with {KINGDOM_NAME}.", null);
				}
				textObject.SetTextVariable("KINGDOM_NAME", kingdom.Name);
				ConversationSentence.SelectedRepeatLine.SetTextVariable("CONTINUE_OR_STOP_TRADE", textObject);
				return true;
			}
			return false;
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x001068DC File Offset: 0x00104ADC
		private void caravan_companion_prohibit_kingdoms_selected_2_on_consequence()
		{
			Kingdom kingdom = ConversationSentence.SelectedRepeatObject as Kingdom;
			bool flag = this._prohibitedKingdomsForPlayerCaravans.Contains(kingdom);
			TextObject textObject = TextObject.GetEmpty();
			if (flag)
			{
				this._prohibitedKingdomsForPlayerCaravans.Remove(kingdom);
			}
			else
			{
				this._prohibitedKingdomsForPlayerCaravans.Add(kingdom);
				if (kingdom.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					textObject = new TextObject("{=y9sgoggj}We are currently at war with {KINGDOM_NAME}, and we shall not start trading with them even if we make peace.", null);
					textObject.SetTextVariable("KINGDOM_NAME", kingdom.Name);
				}
			}
			MBTextManager.SetTextVariable("DESPITE_WAR", textObject, false);
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x00106964 File Offset: 0x00104B64
		private void conversation_caravan_fight_forced_on_consequence()
		{
			this.SetPlayerInteraction(MobileParty.ConversationParty, CaravansCampaignBehavior.PlayerInteraction.Hostile);
			BeHostileAction.ApplyEncounterHostileAction(MobileParty.MainParty.Party, MobileParty.ConversationParty.Party);
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x0010698C File Offset: 0x00104B8C
		private bool companion_is_caravan_leader_on_condition()
		{
			return Hero.OneToOneConversationHero != null && MobileParty.ConversationParty != null && MobileParty.ConversationParty.Party.Owner == Hero.MainHero && MobileParty.ConversationParty.IsCaravan && (Hero.OneToOneConversationHero.IsPlayerCompanion || Hero.OneToOneConversationHero.Clan == Clan.PlayerClan);
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x001069EC File Offset: 0x00104BEC
		private bool player_caravan_talk_start_on_condition()
		{
			return Hero.OneToOneConversationHero == null && MobileParty.ConversationParty != null && MobileParty.ConversationParty.Party.Owner == Hero.MainHero && MobileParty.ConversationParty.IsCaravan && PartyBase.MainParty.Side == BattleSideEnum.Attacker;
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x00106A38 File Offset: 0x00104C38
		private void player_take_prisoner_consequence()
		{
			if (MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerEncounter.EncounteredMobileParty.MapFaction))
			{
				this.conversation_caravan_took_prisoner_on_consequence();
			}
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x00106A5C File Offset: 0x00104C5C
		private bool conversation_warn_player_on_condition()
		{
			IFaction mapFaction = MobileParty.ConversationParty.MapFaction;
			MBTextManager.SetTextVariable("KINGDOM", mapFaction.IsKingdomFaction ? ((Kingdom)mapFaction).FormalName : mapFaction.Name, false);
			return PlayerEncounter.Current != null && !PlayerEncounter.LeaveEncounter && !MobileParty.MainParty.MapFaction.IsAtWarWith(MobileParty.ConversationParty.MapFaction);
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x00106AC8 File Offset: 0x00104CC8
		private bool caravan_start_talk_on_condition()
		{
			if (MobileParty.ConversationParty == null || !MobileParty.ConversationParty.IsCaravan)
			{
				return false;
			}
			CaravansCampaignBehavior.PlayerInteraction playerInteraction = this.GetPlayerInteraction(MobileParty.ConversationParty);
			this.SetPlayerInteraction(MobileParty.ConversationParty, CaravansCampaignBehavior.PlayerInteraction.Friendly);
			if (playerInteraction == CaravansCampaignBehavior.PlayerInteraction.Hostile)
			{
				MBTextManager.SetTextVariable("CARAVAN_GREETING", "{=L7AN6ybY}What do you want with us now?", false);
			}
			else if (playerInteraction != CaravansCampaignBehavior.PlayerInteraction.None)
			{
				MBTextManager.SetTextVariable("CARAVAN_GREETING", "{=Z5kqbeyu}Greetings, once again. Is there anything else?", false);
			}
			else if (CharacterObject.OneToOneConversationCharacter.IsHero && PartyBase.MainParty.Side == BattleSideEnum.Attacker && MobileParty.ConversationParty.Party.Owner != Hero.MainHero)
			{
				StringHelpers.SetCharacterProperties("LEADER", CharacterObject.OneToOneConversationCharacter, null, false);
				MBTextManager.SetTextVariable("CARAVAN_GREETING", "{=afVsbikp}Greetings, traveller. How may we help you?", false);
			}
			else
			{
				MBTextManager.SetTextVariable("HOMETOWN", MobileParty.ConversationParty.HomeSettlement.EncyclopediaLinkWithName, false);
				StringHelpers.SetCharacterProperties("MERCHANT", MobileParty.ConversationParty.Party.Owner.CharacterObject, null, false);
				StringHelpers.SetCharacterProperties("PROTECTOR", MobileParty.ConversationParty.HomeSettlement.OwnerClan.Leader.CharacterObject, null, false);
				TextObject textObject = new TextObject("{=FpUybbSk}Greetings. This caravan is owned by {MERCHANT.LINK}. We trade under the protection of {PROTECTOR.LINK}, master of {HOMETOWN}. How may we help you?[if:convo_normal]", null);
				if (MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCurrentlyAtSea)
				{
					textObject = new TextObject("{=yGttYe7g}Greetings. This ship is owned by {MERCHANT.LINK}. We sail under the protection of {PROTECTOR.LINK}, master of {HOMETOWN}. How may we help you?[if:convo_normal]", null);
				}
				MBTextManager.SetTextVariable("CARAVAN_GREETING", textObject, false);
			}
			return true;
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x00106C24 File Offset: 0x00104E24
		private bool caravan_loot_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan && MobileParty.ConversationParty.Party.MapFaction != Hero.MainHero.MapFaction && MobileParty.ConversationParty.Party.Owner != Hero.MainHero;
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x00106C7C File Offset: 0x00104E7C
		private bool caravan_loot_on_clickable_condition(out TextObject explanation)
		{
			if (this._lootedCaravans.ContainsKey(MobileParty.ConversationParty))
			{
				explanation = new TextObject("{=il2khBNl}You just looted this party.", null);
				return false;
			}
			int num;
			ItemRoster itemRoster;
			this.BribeAmount(MobileParty.ConversationParty.Party, out num, out itemRoster);
			bool flag = num > 0;
			bool flag2 = !itemRoster.IsEmpty<ItemRosterElement>();
			if (flag)
			{
				if (flag2)
				{
					TextObject textObject = ((itemRoster.Count == 1) ? GameTexts.FindText("str_LEFT_RIGHT", null) : GameTexts.FindText("str_LEFT_comma_RIGHT", null));
					TextObject textObject2 = GameTexts.FindText("str_looted_party_have_money", null);
					textObject2.SetTextVariable("MONEY", num);
					textObject2.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject2.SetTextVariable("ITEM_LIST", textObject);
					for (int i = 0; i < itemRoster.Count; i++)
					{
						ItemRosterElement elementCopyAtIndex = itemRoster.GetElementCopyAtIndex(i);
						TextObject textObject3 = GameTexts.FindText("str_offered_item_list", null);
						textObject3.SetTextVariable("COUNT", elementCopyAtIndex.Amount);
						textObject3.SetTextVariable("ITEM", elementCopyAtIndex.EquipmentElement.Item.Name);
						textObject.SetTextVariable("LEFT", textObject3);
						if (itemRoster.Count == 1)
						{
							textObject.SetTextVariable("RIGHT", TextObject.GetEmpty());
						}
						else if (itemRoster.Count - 2 > i)
						{
							TextObject textObject4 = GameTexts.FindText("str_LEFT_comma_RIGHT", null);
							textObject.SetTextVariable("RIGHT", textObject4);
							textObject = textObject4;
						}
						else
						{
							TextObject textObject5 = GameTexts.FindText("str_LEFT_ONLY", null);
							textObject.SetTextVariable("RIGHT", textObject5);
							textObject = textObject5;
						}
					}
					MBTextManager.SetTextVariable("TAKE_MONEY_AND_PRODUCT_STRING", textObject2, false);
				}
				else
				{
					TextObject textObject6 = GameTexts.FindText("str_looted_party_have_money_but_no_item", null);
					textObject6.SetTextVariable("MONEY", num);
					textObject6.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					MBTextManager.SetTextVariable("TAKE_MONEY_AND_PRODUCT_STRING", textObject6, false);
				}
			}
			else if (flag2)
			{
				TextObject textObject7 = ((itemRoster.Count == 1) ? GameTexts.FindText("str_LEFT_RIGHT", null) : GameTexts.FindText("str_LEFT_comma_RIGHT", null));
				TextObject textObject8 = GameTexts.FindText("str_looted_party_have_no_money", null);
				textObject8.SetTextVariable("ITEM_LIST", textObject7);
				for (int j = 0; j < itemRoster.Count; j++)
				{
					ItemRosterElement elementCopyAtIndex2 = itemRoster.GetElementCopyAtIndex(j);
					TextObject textObject9 = GameTexts.FindText("str_offered_item_list", null);
					textObject9.SetTextVariable("COUNT", elementCopyAtIndex2.Amount);
					textObject9.SetTextVariable("ITEM", elementCopyAtIndex2.EquipmentElement.Item.Name);
					textObject7.SetTextVariable("LEFT", textObject9);
					if (itemRoster.Count == 1)
					{
						textObject7.SetTextVariable("RIGHT", TextObject.GetEmpty());
					}
					else if (itemRoster.Count - 2 > j)
					{
						TextObject textObject10 = GameTexts.FindText("str_LEFT_comma_RIGHT", null);
						textObject7.SetTextVariable("RIGHT", textObject10);
						textObject7 = textObject10;
					}
					else
					{
						TextObject textObject11 = GameTexts.FindText("str_LEFT_ONLY", null);
						textObject7.SetTextVariable("RIGHT", textObject11);
						textObject7 = textObject11;
					}
				}
				MBTextManager.SetTextVariable("TAKE_MONEY_AND_PRODUCT_STRING", textObject8, false);
			}
			if (!flag && !flag2)
			{
				explanation = new TextObject("{=pbRwAjUN}They seem to have no valuable goods.", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x00106FB8 File Offset: 0x001051B8
		private bool caravan_buy_products_on_condition()
		{
			if (MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan)
			{
				if (MobileParty.ConversationParty.IsInRaftState)
				{
					return false;
				}
				for (int i = 0; i < MobileParty.ConversationParty.ItemRoster.Count; i++)
				{
					if (MobileParty.ConversationParty.ItemRoster.GetElementNumber(i) > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x00107016 File Offset: 0x00105216
		private void caravan_player_leave_encounter_on_consequence()
		{
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x0010701E File Offset: 0x0010521E
		private bool caravan_ask_trade_rumors_on_condition()
		{
			return !MobileParty.ConversationParty.IsInRaftState;
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x00107030 File Offset: 0x00105230
		private void caravan_ask_trade_rumors_on_consequence()
		{
			Town destinationForMobileParty = this.GetDestinationForMobileParty(MobileParty.ConversationParty);
			if (MobileParty.ConversationParty.LastVisitedSettlement != null && destinationForMobileParty != null && MobileParty.ConversationParty.LastVisitedSettlement != destinationForMobileParty.Settlement)
			{
				List<ValueTuple<CaravansCampaignBehavior.TradeActionLog, float>> list = new List<ValueTuple<CaravansCampaignBehavior.TradeActionLog, float>>();
				List<CaravansCampaignBehavior.TradeActionLog> list2;
				if (this._tradeActionLogs.TryGetValue(MobileParty.ConversationParty, out list2))
				{
					foreach (CaravansCampaignBehavior.TradeActionLog tradeActionLog in list2)
					{
						float profitRate = tradeActionLog.ProfitRate;
						if (profitRate > 1.2f && tradeActionLog.SoldSettlement != null && tradeActionLog.SoldSettlement != tradeActionLog.BoughtSettlement)
						{
							list.Add(new ValueTuple<CaravansCampaignBehavior.TradeActionLog, float>(tradeActionLog, profitRate));
						}
					}
				}
				if (list.Count <= 0)
				{
					MBTextManager.SetTextVariable("COMMENT", GameTexts.FindText("str_caravan_trade_comment_no_profit", null), false);
					return;
				}
				CaravansCampaignBehavior.TradeActionLog tradeActionLog2 = MBRandom.ChooseWeighted<CaravansCampaignBehavior.TradeActionLog>(list);
				MBTextManager.SetTextVariable("ITEM_NAME", tradeActionLog2.ItemRosterElement.EquipmentElement.Item.Name, false);
				MBTextManager.SetTextVariable("SETTLEMENT", tradeActionLog2.BoughtSettlement.EncyclopediaLinkWithName, false);
				MBTextManager.SetTextVariable("DESTINATION", tradeActionLog2.SoldSettlement.EncyclopediaLinkWithName, false);
				MBTextManager.SetTextVariable("BUY_COST", tradeActionLog2.BuyPrice);
				MBTextManager.SetTextVariable("SELL_COST", tradeActionLog2.SellPrice);
				MBTextManager.SetTextVariable("COMMENT", GameTexts.FindText("str_caravan_trade_comment", null), false);
				if (!this._tradeRumorTakenCaravans.ContainsKey(MobileParty.ConversationParty) || (this._tradeRumorTakenCaravans.ContainsKey(MobileParty.ConversationParty) && CampaignTime.Now - this._tradeRumorTakenCaravans[MobileParty.ConversationParty] >= CampaignTime.Days(1f)))
				{
					List<TradeRumor> list3 = new List<TradeRumor>();
					list3.Add(new TradeRumor(destinationForMobileParty.Owner.Settlement, tradeActionLog2.ItemRosterElement.EquipmentElement.Item, destinationForMobileParty.GetItemPrice(tradeActionLog2.ItemRosterElement.EquipmentElement.Item, null, false), destinationForMobileParty.GetItemPrice(tradeActionLog2.ItemRosterElement.EquipmentElement.Item, null, true), 10));
					Town town = MobileParty.ConversationParty.LastVisitedSettlement.Town;
					if (town != null)
					{
						list3.Add(new TradeRumor(town.Owner.Settlement, tradeActionLog2.ItemRosterElement.EquipmentElement.Item, town.GetItemPrice(tradeActionLog2.ItemRosterElement.EquipmentElement.Item, null, false), town.GetItemPrice(tradeActionLog2.ItemRosterElement.EquipmentElement.Item, null, true), 10));
					}
					if (list3.Count > 0)
					{
						CampaignEventDispatcher.Instance.OnTradeRumorIsTaken(list3, null);
						if (this._tradeRumorTakenCaravans.ContainsKey(MobileParty.ConversationParty) && CampaignTime.Now - this._tradeRumorTakenCaravans[MobileParty.ConversationParty] >= CampaignTime.Days(1f))
						{
							this._tradeRumorTakenCaravans[MobileParty.ConversationParty] = CampaignTime.Now;
							return;
						}
						this._tradeRumorTakenCaravans.Add(MobileParty.ConversationParty, CampaignTime.Now);
						return;
					}
				}
			}
			else
			{
				MBTextManager.SetTextVariable("COMMENT", new TextObject("{=TEUVTPIa}Well, we've been resting in town for a while, so our information is probably quite out of date.", null), false);
			}
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x00107398 File Offset: 0x00105598
		private void caravan_talk_leave_on_consequence()
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x001073A7 File Offset: 0x001055A7
		private bool conversation_caravan_player_trade_end_on_condition()
		{
			if (MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan)
			{
				InventoryScreenHelper.OpenTradeWithCaravanOrAlleyParty(MobileParty.ConversationParty, InventoryScreenHelper.InventoryCategoryType.None);
			}
			return true;
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x001073C8 File Offset: 0x001055C8
		private bool conversation_caravan_not_bribe_on_condition()
		{
			if (MobileParty.ConversationParty == null || !MobileParty.ConversationParty.IsCaravan)
			{
				return false;
			}
			if (MobileParty.ConversationParty.IsInRaftState)
			{
				return false;
			}
			if (MobileParty.ConversationParty.IsCurrentlyAtSea)
			{
				MBTextManager.SetTextVariable("CARAVAN_DEFIANCE", "{=jfKTahGa}If you want our wares you'll have to take this ship, you damned pirate![rf: idle_angry][ib: aggressive]", false);
			}
			else
			{
				MBTextManager.SetTextVariable("CARAVAN_DEFIANCE", "{=QNaKmkt9}We're paid to guard this caravan. If you want to rob it, it's going to be over our dead bodies![rf: idle_angry][ib: aggressive]", false);
			}
			return !this.IsBribeFeasible();
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x0010742F File Offset: 0x0010562F
		private bool conversation_caravan_not_surrender_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan && !MobileParty.ConversationParty.IsInRaftState && !this.IsSurrenderFeasible();
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x0010745D File Offset: 0x0010565D
		private void conversation_caravan_fight_on_consequence()
		{
			this.SetPlayerInteraction(MobileParty.ConversationParty, CaravansCampaignBehavior.PlayerInteraction.Hostile);
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, MobileParty.ConversationParty.Party);
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x0010747F File Offset: 0x0010567F
		private bool conversation_caravan_give_goods_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan;
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x00107494 File Offset: 0x00105694
		private bool conversation_caravan_looted_leave_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan;
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x001074AC File Offset: 0x001056AC
		private void conversation_caravan_looted_leave_on_consequence()
		{
			int num;
			ItemRoster itemRoster;
			this.BribeAmount(MobileParty.ConversationParty.Party, out num, out itemRoster);
			GiveGoldAction.ApplyForPartyToCharacter(MobileParty.ConversationParty.Party, Hero.MainHero, num, false);
			if (!itemRoster.IsEmpty<ItemRosterElement>())
			{
				for (int i = itemRoster.Count - 1; i >= 0; i--)
				{
					PartyBase party = MobileParty.ConversationParty.Party;
					PartyBase party2 = Hero.MainHero.PartyBelongedTo.Party;
					ItemRosterElement itemRosterElement = itemRoster[i];
					GiveItemAction.ApplyForParties(party, party2, in itemRosterElement);
				}
			}
			BeHostileAction.ApplyMinorCoercionHostileAction(PartyBase.MainParty, MobileParty.ConversationParty.Party);
			this._lootedCaravans.Add(MobileParty.ConversationParty, CampaignTime.Now);
			this.SetPlayerInteraction(MobileParty.ConversationParty, CaravansCampaignBehavior.PlayerInteraction.Hostile);
			SkillLevelingManager.OnLoot(MobileParty.MainParty, MobileParty.ConversationParty, itemRoster, false);
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x00107574 File Offset: 0x00105774
		private void conversation_caravan_surrender_leave_on_consequence()
		{
			ItemRoster itemRoster = new ItemRoster(MobileParty.ConversationParty.ItemRoster);
			bool flag = false;
			for (int i = 0; i < itemRoster.Count; i++)
			{
				if (itemRoster.GetElementNumber(i) > 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				InventoryScreenHelper.OpenScreenAsLoot(new Dictionary<PartyBase, ItemRoster> { 
				{
					PartyBase.MainParty,
					itemRoster
				} });
				MobileParty.ConversationParty.ItemRoster.Clear();
			}
			int num = MathF.Max(MobileParty.ConversationParty.PartyTradeGold, 0);
			if (num > 0)
			{
				GiveGoldAction.ApplyForPartyToCharacter(MobileParty.ConversationParty.Party, Hero.MainHero, num, false);
			}
			BeHostileAction.ApplyMajorCoercionHostileAction(PartyBase.MainParty, MobileParty.ConversationParty.Party);
			this._lootedCaravans.Add(MobileParty.ConversationParty, CampaignTime.Now);
			SkillLevelingManager.OnLoot(MobileParty.MainParty, MobileParty.ConversationParty, itemRoster, false);
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x00107648 File Offset: 0x00105848
		private void conversation_caravan_took_prisoner_on_consequence()
		{
			MobileParty encounteredMobileParty = PlayerEncounter.EncounteredMobileParty;
			ItemRoster itemRoster = new ItemRoster(encounteredMobileParty.ItemRoster);
			bool flag = false;
			for (int i = 0; i < itemRoster.Count; i++)
			{
				if (itemRoster.GetElementNumber(i) > 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				InventoryScreenHelper.OpenScreenAsLoot(new Dictionary<PartyBase, ItemRoster> { 
				{
					PartyBase.MainParty,
					itemRoster
				} });
				encounteredMobileParty.ItemRoster.Clear();
			}
			int num = MathF.Max(encounteredMobileParty.PartyTradeGold, 0);
			if (num > 0)
			{
				GiveGoldAction.ApplyForPartyToCharacter(encounteredMobileParty.Party, Hero.MainHero, num, false);
			}
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, encounteredMobileParty.Party);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (TroopRosterElement troopRosterElement in encounteredMobileParty.MemberRoster.GetTroopRoster())
			{
				troopRoster.AddToCounts(troopRosterElement.Character, troopRosterElement.Number, false, 0, 0, true, -1);
			}
			PartyScreenHelper.OpenScreenAsLoot(TroopRoster.CreateDummyTroopRoster(), troopRoster, encounteredMobileParty.Name, troopRoster.TotalManCount, null);
			SkillLevelingManager.OnLoot(MobileParty.MainParty, encounteredMobileParty, itemRoster, false);
			DestroyPartyAction.Apply(MobileParty.MainParty.Party, encounteredMobileParty);
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x0010778C File Offset: 0x0010598C
		private bool IsBribeFeasible()
		{
			float resultNumber = Campaign.Current.Models.EncounterModel.GetBribeChance(MobileParty.ConversationParty, MobileParty.MainParty).ResultNumber;
			return MobileParty.ConversationParty.Party.RandomFloatWithSeed(5U, 1f) <= resultNumber;
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x001077DC File Offset: 0x001059DC
		private bool IsSurrenderFeasible()
		{
			float surrenderChance = Campaign.Current.Models.EncounterModel.GetSurrenderChance(MobileParty.ConversationParty, MobileParty.MainParty);
			return MobileParty.ConversationParty.Party.RandomFloatWithSeed(7U, 1f) <= surrenderChance;
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x00107824 File Offset: 0x00105A24
		private void BribeAmount(PartyBase party, out int gold, out ItemRoster items)
		{
			int num = 0;
			ItemRoster itemRoster = new ItemRoster();
			bool flag = false;
			for (int i = 0; i < MobileParty.ConversationParty.ItemRoster.Count; i++)
			{
				num += MobileParty.ConversationParty.ItemRoster.GetElementUnitCost(i) * MobileParty.ConversationParty.ItemRoster.GetElementNumber(i);
				flag = true;
			}
			num += MobileParty.ConversationParty.PartyTradeGold;
			int num2 = MathF.Min((int)((float)num * 0.05f), 2000);
			int num3 = MathF.Min(MobileParty.ConversationParty.PartyTradeGold, num2);
			if (num3 < num2 && flag)
			{
				for (int j = 0; j < MobileParty.ConversationParty.ItemRoster.Count; j++)
				{
					ItemRosterElement elementCopyAtIndex = MobileParty.ConversationParty.ItemRoster.GetElementCopyAtIndex(j);
					if (elementCopyAtIndex.EquipmentElement.ItemValue * elementCopyAtIndex.Amount >= num2 - num3)
					{
						if (elementCopyAtIndex.EquipmentElement.Item.Type == ItemObject.ItemTypeEnum.Goods)
						{
							if (!itemRoster.IsEmpty<ItemRosterElement>())
							{
								itemRoster.Clear();
							}
							itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement.Item, elementCopyAtIndex.Amount);
							break;
						}
						if (itemRoster.IsEmpty<ItemRosterElement>())
						{
							itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement.Item, elementCopyAtIndex.Amount);
						}
					}
				}
				if (itemRoster.IsEmpty<ItemRosterElement>())
				{
					int num4 = num2 - num3;
					int num5 = 0;
					while (num5 < MobileParty.ConversationParty.ItemRoster.Count && num4 > 0)
					{
						ItemRosterElement randomElement = MobileParty.ConversationParty.ItemRoster.GetRandomElement<ItemRosterElement>();
						num4 -= randomElement.Amount * randomElement.EquipmentElement.ItemValue;
						itemRoster.AddToCounts(randomElement.EquipmentElement.Item, randomElement.Amount);
						num5++;
					}
				}
			}
			gold = num3;
			items = itemRoster;
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x00107A08 File Offset: 0x00105C08
		private bool CanTradeWith(IFaction caravanFaction, IFaction targetFaction)
		{
			Kingdom kingdom;
			return !caravanFaction.IsAtWarWith(targetFaction) && (caravanFaction != Hero.MainHero.MapFaction || (kingdom = targetFaction as Kingdom) == null || !this._prohibitedKingdomsForPlayerCaravans.Contains(kingdom));
		}

		// Token: 0x04001327 RID: 4903
		private const float InventoryFullnessGoal = 0.9f;

		// Token: 0x04001328 RID: 4904
		private const float AverageCaravanWaitAtSettlement = 3f;

		// Token: 0x04001329 RID: 4905
		private const float ProfitRateRumorThreshold = 1.2f;

		// Token: 0x0400132A RID: 4906
		private const float ReferenceBudgetValue = 5000f;

		// Token: 0x0400132B RID: 4907
		private const float HighSecurityThreshold = 75f;

		// Token: 0x0400132C RID: 4908
		private const float MustDiscardPriorityValue = -3.4028235E+38f;

		// Token: 0x0400132D RID: 4909
		private const float CaravanTradeAgreementBonus = 2f;

		// Token: 0x0400132E RID: 4910
		private const float ConvoyTradeAgreementBonus = 1.5f;

		// Token: 0x0400132F RID: 4911
		private float _navalCaravanVeryFarCache = -1f;

		// Token: 0x04001330 RID: 4912
		private float _defaultCaravanVeryFarCache = -1f;

		// Token: 0x04001331 RID: 4913
		private ITradeAgreementsCampaignBehavior _tradeAgreementsBehavior;

		// Token: 0x04001332 RID: 4914
		private Dictionary<MobileParty, CampaignTime> _tradeRumorTakenCaravans = new Dictionary<MobileParty, CampaignTime>();

		// Token: 0x04001333 RID: 4915
		private Dictionary<MobileParty, CampaignTime> _caravanLastHomeTownVisitTime = new Dictionary<MobileParty, CampaignTime>();

		// Token: 0x04001334 RID: 4916
		private Dictionary<MobileParty, CampaignTime> _lootedCaravans = new Dictionary<MobileParty, CampaignTime>();

		// Token: 0x04001335 RID: 4917
		private Dictionary<MobileParty, CaravansCampaignBehavior.PlayerInteraction> _interactedCaravans = new Dictionary<MobileParty, CaravansCampaignBehavior.PlayerInteraction>();

		// Token: 0x04001336 RID: 4918
		private Dictionary<MobileParty, List<CaravansCampaignBehavior.TradeActionLog>> _tradeActionLogs = new Dictionary<MobileParty, List<CaravansCampaignBehavior.TradeActionLog>>();

		// Token: 0x04001337 RID: 4919
		private CaravansCampaignBehavior.TradeActionLogPool _tradeActionLogPool;

		// Token: 0x04001338 RID: 4920
		private List<Kingdom> _prohibitedKingdomsForPlayerCaravans = new List<Kingdom>();

		// Token: 0x04001339 RID: 4921
		private int _packAnimalCategoryIndex = -1;

		// Token: 0x0400133A RID: 4922
		private readonly Dictionary<ItemCategory, float> _averageValuesCached = new Dictionary<ItemCategory, float>();

		// Token: 0x0400133B RID: 4923
		private readonly Dictionary<ItemCategory, CaravansCampaignBehavior.PriceIndexData> _priceDictionary = new Dictionary<ItemCategory, CaravansCampaignBehavior.PriceIndexData>();

		// Token: 0x0400133C RID: 4924
		private readonly Dictionary<ItemCategory, CaravansCampaignBehavior.PriceIndexData> _coastalPriceDictionary = new Dictionary<ItemCategory, CaravansCampaignBehavior.PriceIndexData>();

		// Token: 0x0400133D RID: 4925
		private readonly Dictionary<ItemCategory, int> _totalValueOfItemsAtCategory = new Dictionary<ItemCategory, int>();

		// Token: 0x02000808 RID: 2056
		public class CaravansCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060066BB RID: 26299 RVA: 0x001D0E9E File Offset: 0x001CF09E
			public CaravansCampaignBehaviorTypeDefiner()
				: base(60000)
			{
			}

			// Token: 0x060066BC RID: 26300 RVA: 0x001D0EAB File Offset: 0x001CF0AB
			protected override void DefineEnumTypes()
			{
				base.AddEnumDefinition(typeof(CaravansCampaignBehavior.PlayerInteraction), 1, null);
			}

			// Token: 0x060066BD RID: 26301 RVA: 0x001D0EBF File Offset: 0x001CF0BF
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<MobileParty, CaravansCampaignBehavior.PlayerInteraction>));
				base.ConstructContainerDefinition(typeof(List<CaravansCampaignBehavior.TradeActionLog>));
				base.ConstructContainerDefinition(typeof(Dictionary<MobileParty, List<CaravansCampaignBehavior.TradeActionLog>>));
			}

			// Token: 0x060066BE RID: 26302 RVA: 0x001D0EF1 File Offset: 0x001CF0F1
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(CaravansCampaignBehavior.TradeActionLog), 2, null);
			}
		}

		// Token: 0x02000809 RID: 2057
		private enum PlayerInteraction
		{
			// Token: 0x040020D6 RID: 8406
			None,
			// Token: 0x040020D7 RID: 8407
			Friendly,
			// Token: 0x040020D8 RID: 8408
			TradedWith,
			// Token: 0x040020D9 RID: 8409
			Hostile
		}

		// Token: 0x0200080A RID: 2058
		private struct PriceIndexData
		{
			// Token: 0x060066BF RID: 26303 RVA: 0x001D0F05 File Offset: 0x001CF105
			public PriceIndexData(float averageBuySellPriceIndex, float minBuySellPriceIndex)
			{
				this.AverageBuySellPriceIndex = averageBuySellPriceIndex;
				this.MinBuySellPriceIndex = minBuySellPriceIndex;
			}

			// Token: 0x040020DA RID: 8410
			internal readonly float AverageBuySellPriceIndex;

			// Token: 0x040020DB RID: 8411
			internal readonly float MinBuySellPriceIndex;
		}

		// Token: 0x0200080B RID: 2059
		internal class TradeActionLog
		{
			// Token: 0x1700159F RID: 5535
			// (get) Token: 0x060066C0 RID: 26304 RVA: 0x001D0F15 File Offset: 0x001CF115
			public float ProfitRate
			{
				get
				{
					return (float)this.SellPrice / (float)this.BuyPrice;
				}
			}

			// Token: 0x060066C1 RID: 26305 RVA: 0x001D0F26 File Offset: 0x001CF126
			public void OnSellAction(Settlement soldSettlement, int sellPrice)
			{
				this.SellPrice = sellPrice;
				this.SoldSettlement = soldSettlement;
			}

			// Token: 0x060066C2 RID: 26306 RVA: 0x001D0F36 File Offset: 0x001CF136
			public void Reset()
			{
				this.BoughtSettlement = null;
				this.SoldSettlement = null;
				this.SellPrice = 0;
				this.BuyPrice = 0;
			}

			// Token: 0x060066C3 RID: 26307 RVA: 0x001D0F54 File Offset: 0x001CF154
			internal static void AutoGeneratedStaticCollectObjectsTradeActionLog(object o, List<object> collectedObjects)
			{
				((CaravansCampaignBehavior.TradeActionLog)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060066C4 RID: 26308 RVA: 0x001D0F62 File Offset: 0x001CF162
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.BoughtSettlement);
				ItemRosterElement.AutoGeneratedStaticCollectObjectsItemRosterElement(this.ItemRosterElement, collectedObjects);
				collectedObjects.Add(this.SoldSettlement);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.BoughtTime, collectedObjects);
			}

			// Token: 0x060066C5 RID: 26309 RVA: 0x001D0F9E File Offset: 0x001CF19E
			internal static object AutoGeneratedGetMemberValueBoughtSettlement(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).BoughtSettlement;
			}

			// Token: 0x060066C6 RID: 26310 RVA: 0x001D0FAB File Offset: 0x001CF1AB
			internal static object AutoGeneratedGetMemberValueBuyPrice(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).BuyPrice;
			}

			// Token: 0x060066C7 RID: 26311 RVA: 0x001D0FBD File Offset: 0x001CF1BD
			internal static object AutoGeneratedGetMemberValueSellPrice(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).SellPrice;
			}

			// Token: 0x060066C8 RID: 26312 RVA: 0x001D0FCF File Offset: 0x001CF1CF
			internal static object AutoGeneratedGetMemberValueItemRosterElement(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).ItemRosterElement;
			}

			// Token: 0x060066C9 RID: 26313 RVA: 0x001D0FE1 File Offset: 0x001CF1E1
			internal static object AutoGeneratedGetMemberValueSoldSettlement(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).SoldSettlement;
			}

			// Token: 0x060066CA RID: 26314 RVA: 0x001D0FEE File Offset: 0x001CF1EE
			internal static object AutoGeneratedGetMemberValueBoughtTime(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).BoughtTime;
			}

			// Token: 0x040020DC RID: 8412
			[SaveableField(0)]
			public Settlement BoughtSettlement;

			// Token: 0x040020DD RID: 8413
			[SaveableField(1)]
			public int BuyPrice;

			// Token: 0x040020DE RID: 8414
			[SaveableField(2)]
			public int SellPrice;

			// Token: 0x040020DF RID: 8415
			[SaveableField(3)]
			public ItemRosterElement ItemRosterElement;

			// Token: 0x040020E0 RID: 8416
			[SaveableField(4)]
			public Settlement SoldSettlement;

			// Token: 0x040020E1 RID: 8417
			[SaveableField(5)]
			public CampaignTime BoughtTime;
		}

		// Token: 0x0200080C RID: 2060
		internal class TradeActionLogPool
		{
			// Token: 0x170015A0 RID: 5536
			// (get) Token: 0x060066CC RID: 26316 RVA: 0x001D1008 File Offset: 0x001CF208
			public int Size
			{
				get
				{
					Stack<CaravansCampaignBehavior.TradeActionLog> stack = this._stack;
					if (stack == null)
					{
						return 0;
					}
					return stack.Count;
				}
			}

			// Token: 0x170015A1 RID: 5537
			// (get) Token: 0x060066CD RID: 26317 RVA: 0x001D101B File Offset: 0x001CF21B
			private int MaxSize { get; }

			// Token: 0x060066CE RID: 26318 RVA: 0x001D1024 File Offset: 0x001CF224
			public TradeActionLogPool(int size)
			{
				this.MaxSize = size;
				this._stack = new Stack<CaravansCampaignBehavior.TradeActionLog>(size);
				for (int i = 0; i < size; i++)
				{
					this._stack.Push(new CaravansCampaignBehavior.TradeActionLog());
				}
			}

			// Token: 0x060066CF RID: 26319 RVA: 0x001D1068 File Offset: 0x001CF268
			public CaravansCampaignBehavior.TradeActionLog CreateNewLog(Settlement boughtSettlement, int buyPrice, ItemRosterElement itemRosterElement)
			{
				CaravansCampaignBehavior.TradeActionLog tradeActionLog = ((this._stack.Count > 0) ? this._stack.Pop() : new CaravansCampaignBehavior.TradeActionLog());
				tradeActionLog.BoughtSettlement = boughtSettlement;
				tradeActionLog.BuyPrice = buyPrice;
				tradeActionLog.ItemRosterElement = itemRosterElement;
				tradeActionLog.BoughtTime = CampaignTime.Now;
				return tradeActionLog;
			}

			// Token: 0x060066D0 RID: 26320 RVA: 0x001D10B5 File Offset: 0x001CF2B5
			public void ReleaseLog(CaravansCampaignBehavior.TradeActionLog log)
			{
				log.Reset();
				if (this._stack.Count < this.MaxSize)
				{
					this._stack.Push(log);
				}
			}

			// Token: 0x060066D1 RID: 26321 RVA: 0x001D10DC File Offset: 0x001CF2DC
			public override string ToString()
			{
				return string.Format("TrackPool: {0}", this.Size);
			}

			// Token: 0x040020E3 RID: 8419
			private Stack<CaravansCampaignBehavior.TradeActionLog> _stack;
		}
	}
}
