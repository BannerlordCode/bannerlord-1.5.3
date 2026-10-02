using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200016C RID: 364
	public class DefaultVillageProductionCalculatorModel : VillageProductionCalculatorModel
	{
		// Token: 0x06001B9F RID: 7071 RVA: 0x0008F010 File Offset: 0x0008D210
		public override ExplainedNumber CalculateDailyProductionAmount(Village village, ItemObject item)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			if (village.VillageState == Village.VillageStates.Normal)
			{
				foreach (ValueTuple<ItemObject, float> valueTuple in village.VillageType.Productions)
				{
					ItemObject item2 = valueTuple.Item1;
					float num = valueTuple.Item2;
					if (item2 == item)
					{
						if (village.TradeBound != null)
						{
							float num2 = (float)(village.GetHearthLevel() + 1) * 0.5f;
							if (item.IsMountable && item.Tier == ItemObject.ItemTiers.Tier2 && PerkHelper.GetPerkValueForTown(DefaultPerks.Riding.Shepherd, village.TradeBound.Town) && MBRandom.RandomFloat < DefaultPerks.Riding.Shepherd.SecondaryBonus)
							{
								num += 1f;
							}
							explainedNumber.Add(num * num2, null, null);
							if (item.ItemCategory == DefaultItemCategories.Grain || item.ItemCategory == DefaultItemCategories.Olives || item.ItemCategory == DefaultItemCategories.Fish || item.ItemCategory == DefaultItemCategories.DateFruit)
							{
								PerkHelper.AddPerkBonusForTown(DefaultPerks.Trade.GranaryAccountant, village.TradeBound.Town, false, ref explainedNumber);
							}
							else if (item.ItemCategory == DefaultItemCategories.Clay || item.ItemCategory == DefaultItemCategories.Iron || item.ItemCategory == DefaultItemCategories.Cotton || item.ItemCategory == DefaultItemCategories.Silver)
							{
								PerkHelper.AddPerkBonusForTown(DefaultPerks.Trade.TradeyardForeman, village.TradeBound.Town, false, ref explainedNumber);
							}
							if (item.IsTradeGood)
							{
								PerkHelper.AddPerkBonusForTown(DefaultPerks.Athletics.Steady, village.TradeBound.Town, false, ref explainedNumber);
							}
							if (item.IsAnimal)
							{
								PerkHelper.AddPerkBonusForTown(DefaultPerks.Medicine.PerfectHealth, village.TradeBound.Town, false, ref explainedNumber);
							}
							PerkHelper.AddPerkBonusForTown(DefaultPerks.Riding.Breeder, village.TradeBound.Town, false, ref explainedNumber);
						}
						if (item.ItemCategory == DefaultItemCategories.Sheep || item.ItemCategory == DefaultItemCategories.Cow || item.ItemCategory == DefaultItemCategories.WarHorse || item.ItemCategory == DefaultItemCategories.Horse || item.ItemCategory == DefaultItemCategories.PackAnimal)
						{
							FeatHelper.ApplyCultureFeat(village.Settlement.OwnerClan.Culture, DefaultCulturalFeats.KhuzaitAnimalProductionFeat, ref explainedNumber);
						}
						if (item.ItemCategory == DefaultItemCategories.Grain)
						{
							FeatHelper.ApplyCultureFeat(village.Settlement.OwnerClan.Culture, DefaultCulturalFeats.SturgianGrainProductionFeat, ref explainedNumber);
						}
						if (village.Bound.IsFortification)
						{
							village.Bound.Town.AddEffectOfBuildings(BuildingEffectEnum.VillageProduction, ref explainedNumber);
						}
						if (village.Bound.IsCastle)
						{
							FeatHelper.ApplyCultureFeat(village.Settlement.OwnerClan.Culture, DefaultCulturalFeats.VlandianCastleVillageProductionFeat, ref explainedNumber);
						}
					}
				}
			}
			return explainedNumber;
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x0008F2D4 File Offset: 0x0008D4D4
		public override float CalculateDailyFoodProductionAmount(Village village)
		{
			if (village.VillageState != Village.VillageStates.Normal)
			{
				return 0f;
			}
			float num = (float)(village.GetHearthLevel() + 1);
			float num2;
			if (this.GetIssueEffectOnFoodProduction(village.Settlement, out num2))
			{
				num *= num2;
			}
			return num;
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x0008F310 File Offset: 0x0008D510
		private bool GetIssueEffectOnFoodProduction(Settlement settlement, out float issueEffect)
		{
			issueEffect = 1f;
			if (settlement.IsVillage)
			{
				foreach (Hero hero in SettlementHelper.GetAllHeroesOfSettlement(settlement, false))
				{
					if (hero.Issue != null && hero.MapFaction == settlement.MapFaction)
					{
						float activeIssueEffectAmount = hero.Issue.GetActiveIssueEffectAmount(DefaultIssueEffects.HalfVillageProduction);
						if (activeIssueEffectAmount != 0f)
						{
							issueEffect *= activeIssueEffectAmount;
						}
					}
				}
			}
			return !issueEffect.ApproximatelyEqualsTo(1f, 1E-05f);
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x0008F3B0 File Offset: 0x0008D5B0
		public override float CalculateProductionSpeedOfItemCategory(ItemCategory item)
		{
			float num = 0f;
			foreach (VillageType villageType in VillageType.All)
			{
				float productionPerDay = villageType.GetProductionPerDay(item);
				if (productionPerDay > num)
				{
					num = productionPerDay;
				}
			}
			return num;
		}

		// Token: 0x0400093A RID: 2362
		private readonly TextObject _cultureEffect = GameTexts.FindText("str_culture", null);
	}
}
