using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200042E RID: 1070
	public class ItemConsumptionBehavior : CampaignBehaviorBase
	{
		// Token: 0x060043EC RID: 17388 RVA: 0x0013FD38 File Offset: 0x0013DF38
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedFollowUp));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedFollowUpEnd));
			CampaignEvents.DailyTickTownEvent.AddNonSerializedListener(this, new Action<Town>(this.DailyTickTown));
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x0013FD8A File Offset: 0x0013DF8A
		private void OnNewGameCreatedFollowUp(CampaignGameStarter starter, int i)
		{
			if (i < 2)
			{
				this.MakeConsumptionAllTowns();
			}
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0013FD98 File Offset: 0x0013DF98
		private void OnNewGameCreatedFollowUpEnd(CampaignGameStarter starter)
		{
			Dictionary<ItemCategory, float> dictionary = new Dictionary<ItemCategory, float>();
			for (int i = 0; i < 10; i++)
			{
				foreach (Town town in Town.AllTowns)
				{
					ItemConsumptionBehavior.UpdateSupplyAndDemand(town);
					this.UpdateDemandShift(town, dictionary);
				}
			}
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0013FE04 File Offset: 0x0013E004
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x0013FE08 File Offset: 0x0013E008
		private void DailyTickTown(Town town)
		{
			Dictionary<ItemCategory, int> dictionary = new Dictionary<ItemCategory, int>();
			this.MakeConsumptionInTown(town, dictionary);
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x0013FE24 File Offset: 0x0013E024
		private void MakeConsumptionAllTowns()
		{
			foreach (Town town in Town.AllTowns)
			{
				this.DailyTickTown(town);
			}
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x0013FE78 File Offset: 0x0013E078
		private void MakeConsumptionInTown(Town town, Dictionary<ItemCategory, int> saleLog)
		{
			Dictionary<ItemCategory, float> dictionary = new Dictionary<ItemCategory, float>();
			this.DeleteOverproducedItems(town);
			ItemConsumptionBehavior.UpdateSupplyAndDemand(town);
			this.UpdateDemandShift(town, dictionary);
			ItemConsumptionBehavior.MakeConsumption(town, dictionary, saleLog);
			ItemConsumptionBehavior.GetFoodFromMarket(town, saleLog);
			this.UpdateSellLog(town, saleLog);
			this.UpdateTownGold(town);
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x0013FEC0 File Offset: 0x0013E0C0
		private void UpdateTownGold(Town town)
		{
			int townGoldChange = Campaign.Current.Models.SettlementEconomyModel.GetTownGoldChange(town);
			town.ChangeGold(townGoldChange);
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0013FEEC File Offset: 0x0013E0EC
		private void DeleteOverproducedItems(Town town)
		{
			ItemRoster itemRoster = town.Owner.ItemRoster;
			for (int i = itemRoster.Count - 1; i >= 0; i--)
			{
				ItemRosterElement elementCopyAtIndex = itemRoster.GetElementCopyAtIndex(i);
				ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
				int amount = elementCopyAtIndex.Amount;
				if (amount > 0 && (item.IsCraftedByPlayer || item.IsBannerItem))
				{
					itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement, -amount);
				}
				else if (elementCopyAtIndex.EquipmentElement.ItemModifier != null && MBRandom.RandomFloat < 0.05f)
				{
					itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement, -1);
				}
			}
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x0013FF90 File Offset: 0x0013E190
		private static void GetFoodFromMarket(Town town, Dictionary<ItemCategory, int> saleLog)
		{
			float foodChange = town.FoodChange;
			ValueTuple<int, int> townFoodAndMarketStocks = TownHelpers.GetTownFoodAndMarketStocks(town);
			int item = townFoodAndMarketStocks.Item1;
			int item2 = townFoodAndMarketStocks.Item2;
			if (town.IsTown && town.IsUnderSiege && foodChange < 0f && item <= 0 && item2 > 0)
			{
				ItemConsumptionBehavior.GetFoodFromMarketInternal(town, Math.Abs(MathF.Floor(foodChange)), saleLog);
			}
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x0013FFEC File Offset: 0x0013E1EC
		private void UpdateSellLog(Town town, Dictionary<ItemCategory, int> saleLog)
		{
			List<Town.SellLog> list = new List<Town.SellLog>();
			foreach (KeyValuePair<ItemCategory, int> keyValuePair in saleLog)
			{
				if (keyValuePair.Value > 0)
				{
					list.Add(new Town.SellLog(keyValuePair.Key, keyValuePair.Value));
				}
			}
			town.SetSoldItems(list);
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x00140064 File Offset: 0x0013E264
		private static void GetFoodFromMarketInternal(Town town, int amount, Dictionary<ItemCategory, int> saleLog)
		{
			ItemRoster itemRoster = town.Owner.ItemRoster;
			int num = itemRoster.Count - 1;
			while (num >= 0 && amount > 0)
			{
				ItemRosterElement elementCopyAtIndex = itemRoster.GetElementCopyAtIndex(num);
				ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
				if (item.ItemCategory.Properties == ItemCategory.Property.BonusToFoodStores)
				{
					int num2 = ((elementCopyAtIndex.Amount >= amount) ? amount : elementCopyAtIndex.Amount);
					amount -= num2;
					itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement, -num2);
					int num3 = 0;
					saleLog.TryGetValue(item.ItemCategory, out num3);
					saleLog[item.ItemCategory] = num3 + num2;
				}
				num--;
			}
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x00140110 File Offset: 0x0013E310
		private static void MakeConsumption(Town town, Dictionary<ItemCategory, float> categoryDemand, Dictionary<ItemCategory, int> saleLog)
		{
			saleLog.Clear();
			TownMarketData marketData = town.MarketData;
			ItemRoster itemRoster = town.Owner.ItemRoster;
			for (int i = itemRoster.Count - 1; i >= 0; i--)
			{
				ItemRosterElement elementCopyAtIndex = itemRoster.GetElementCopyAtIndex(i);
				ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
				int amount = elementCopyAtIndex.Amount;
				ItemCategory itemCategory = item.GetItemCategory();
				float num = categoryDemand[itemCategory];
				float num2 = Campaign.Current.Models.SettlementEconomyModel.CalculateDailySettlementBudgetForItemCategory(town, num, itemCategory);
				if (num2 > 0.01f)
				{
					int price = marketData.GetPrice(item, null, false, null);
					float num3 = num2 / (float)price;
					if (num3 > (float)amount)
					{
						num3 = (float)amount;
					}
					int num4 = MBRandom.RoundRandomized(num3);
					if (num4 > amount)
					{
						num4 = amount;
					}
					itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement, -num4);
					categoryDemand[itemCategory] = num2 - num3 * (float)price;
					town.ChangeGold(num4 * price);
					int num5 = 0;
					saleLog.TryGetValue(itemCategory, out num5);
					saleLog[itemCategory] = num5 + num4;
				}
			}
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x00140228 File Offset: 0x0013E428
		private void UpdateDemandShift(Town town, Dictionary<ItemCategory, float> categoryBudget)
		{
			TownMarketData marketData = town.MarketData;
			foreach (ItemCategory itemCategory in ItemCategories.All)
			{
				categoryBudget[itemCategory] = Campaign.Current.Models.SettlementEconomyModel.GetDailyDemandForCategory(town, itemCategory, 0);
			}
			foreach (ItemCategory itemCategory2 in ItemCategories.All)
			{
				if (itemCategory2.CanSubstitute != null)
				{
					ItemData categoryData = marketData.GetCategoryData(itemCategory2);
					ItemData categoryData2 = marketData.GetCategoryData(itemCategory2.CanSubstitute);
					if (categoryData.Supply / categoryData.Demand > categoryData2.Supply / categoryData2.Demand && categoryData2.Demand > categoryData.Demand)
					{
						float num = (categoryData2.Demand - categoryData.Demand) * itemCategory2.SubstitutionFactor;
						marketData.SetDemand(itemCategory2, categoryData.Demand + num);
						marketData.SetDemand(itemCategory2.CanSubstitute, categoryData2.Demand - num);
						float num2;
						float num3;
						if (categoryBudget.TryGetValue(itemCategory2, out num2) && categoryBudget.TryGetValue(itemCategory2.CanSubstitute, out num3))
						{
							categoryBudget[itemCategory2] = num2 + num;
							categoryBudget[itemCategory2.CanSubstitute] = num3 - num;
						}
					}
				}
			}
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x001403A4 File Offset: 0x0013E5A4
		private static void UpdateSupplyAndDemand(Town town)
		{
			TownMarketData marketData = town.MarketData;
			SettlementEconomyModel settlementEconomyModel = Campaign.Current.Models.SettlementEconomyModel;
			foreach (ItemCategory itemCategory in ItemCategories.All)
			{
				ItemData categoryData = marketData.GetCategoryData(itemCategory);
				float estimatedDemandForCategory = settlementEconomyModel.GetEstimatedDemandForCategory(town, categoryData, itemCategory);
				ValueTuple<float, float> supplyDemandForCategory = settlementEconomyModel.GetSupplyDemandForCategory(town, itemCategory, (float)categoryData.InStoreValue, estimatedDemandForCategory, categoryData.Supply, categoryData.Demand);
				float item = supplyDemandForCategory.Item1;
				float item2 = supplyDemandForCategory.Item2;
				marketData.SetSupplyDemand(itemCategory, item, item2);
			}
		}
	}
}
