using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000152 RID: 338
	public class DefaultSettlementFoodModel : SettlementFoodModel
	{
		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06001A7A RID: 6778 RVA: 0x00086153 File Offset: 0x00084353
		public override int FoodStocksUpperLimit
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06001A7B RID: 6779 RVA: 0x0008615A File Offset: 0x0008435A
		public override int NumberOfProsperityToEatOneFood
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06001A7C RID: 6780 RVA: 0x0008615E File Offset: 0x0008435E
		public override int NumberOfMenOnGarrisonToEatOneFood
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06001A7D RID: 6781 RVA: 0x00086162 File Offset: 0x00084362
		public override int CastleFoodStockUpperLimitBonus
		{
			get
			{
				return 150;
			}
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x00086169 File Offset: 0x00084369
		public override ExplainedNumber CalculateTownFoodStocksChange(Town town, bool includeMarketStocks = true, bool includeDescriptions = false)
		{
			return this.CalculateTownFoodChangeInternal(town, includeMarketStocks, includeDescriptions);
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x00086174 File Offset: 0x00084374
		private ExplainedNumber CalculateTownFoodChangeInternal(Town town, bool includeMarketStocks, bool includeDescriptions)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			ExplainedNumber explainedNumber2 = new ExplainedNumber(0f, includeDescriptions, null);
			ExplainedNumber explainedNumber3 = new ExplainedNumber(town.Prosperity / (float)this.NumberOfProsperityToEatOneFood, false, null);
			MobileParty garrisonParty = town.GarrisonParty;
			int? num = ((garrisonParty != null) ? new int?(garrisonParty.Party.NumberOfAllMembers) : null);
			ExplainedNumber explainedNumber4 = new ExplainedNumber(((num != null) ? ((float)num.GetValueOrDefault()) : 0f) / (float)this.NumberOfMenOnGarrisonToEatOneFood, false, null);
			if (town.IsUnderSiege)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.Gourmet, town, false, ref explainedNumber4);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Medicine.TriageTent, town, false, ref explainedNumber2);
			}
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.MasterOfWarcraft, town, false, ref explainedNumber3);
			Hero governor = town.Governor;
			if (governor != null)
			{
				Settlement currentSettlement = governor.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town : null) == town)
				{
					TraitEffectHelper.ApplyTraitEffect(governor, DefaultPersonalityTraitEffects.GenerosityFoodCostEffect, ref explainedNumber4);
				}
			}
			explainedNumber2.Add(explainedNumber3.ResultNumber, this.ProsperityText, null);
			explainedNumber2.Add(explainedNumber4.ResultNumber, this.GarrisonText, null);
			town.AddEffectOfBuildings(BuildingEffectEnum.FoodConsumption, ref explainedNumber2);
			Clan ownerClan = town.Settlement.OwnerClan;
			Kingdom kingdom = ((ownerClan != null) ? ownerClan.Kingdom : null);
			if (kingdom != null && kingdom.HasPolicy(DefaultPolicies.HuntingRights))
			{
				explainedNumber.Add(2f, DefaultPolicies.HuntingRights.Name, null);
			}
			if (!town.IsUnderSiege)
			{
				int num2 = (town.IsTown ? 15 : 10);
				explainedNumber.Add((float)num2, this.LandsAroundSettlementText, null);
				foreach (Village village in town.Owner.Settlement.BoundVillages)
				{
					float num3 = 0f;
					if (village.VillageState == Village.VillageStates.Normal)
					{
						num3 = (float)((village.GetHearthLevel() + 1) * 6);
					}
					explainedNumber.Add(num3, village.Name, null);
				}
				town.AddEffectOfBuildings(BuildingEffectEnum.FoodProduction, ref explainedNumber);
			}
			else
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Roguery.DirtyFighting, town, false, ref explainedNumber);
			}
			if (includeMarketStocks)
			{
				foreach (Town.SellLog sellLog in town.SoldItems)
				{
					if (sellLog.Category.Properties == ItemCategory.Property.BonusToFoodStores)
					{
						explainedNumber.Add((float)sellLog.Number, includeDescriptions ? sellLog.Category.GetName() : null, null);
					}
				}
			}
			ExplainedNumber explainedNumber5 = new ExplainedNumber(0f, includeDescriptions, null);
			explainedNumber5.AddFromExplainedNumber(explainedNumber, null);
			explainedNumber5.SubtractFromExplainedNumber(explainedNumber2, null);
			DefaultSettlementFoodModel.GetSettlementFoodChangeDueToIssues(town, ref explainedNumber5);
			return explainedNumber5;
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x0008643C File Offset: 0x0008463C
		private static void GetSettlementFoodChangeDueToIssues(Town town, ref ExplainedNumber explainedNumber)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementFood, town.Settlement, ref explainedNumber);
		}

		// Token: 0x040008B9 RID: 2233
		private readonly TextObject ProsperityText = GameTexts.FindText("str_prosperity", null);

		// Token: 0x040008BA RID: 2234
		private readonly TextObject GarrisonText = GameTexts.FindText("str_garrison", null);

		// Token: 0x040008BB RID: 2235
		private readonly TextObject LandsAroundSettlementText = GameTexts.FindText("str_lands_around_settlement", null);

		// Token: 0x040008BC RID: 2236
		private readonly TextObject NormalVillagesText = GameTexts.FindText("str_normal_villages", null);

		// Token: 0x040008BD RID: 2237
		private readonly TextObject RaidedVillagesText = GameTexts.FindText("str_raided_villages", null);

		// Token: 0x040008BE RID: 2238
		private readonly TextObject VillagesUnderSiegeText = GameTexts.FindText("str_villages_under_siege", null);

		// Token: 0x040008BF RID: 2239
		private readonly TextObject FoodBoughtByCiviliansText = GameTexts.FindText("str_food_bought_by_civilians", null);

		// Token: 0x040008C0 RID: 2240
		private const int FoodProductionPerVillage = 10;
	}
}
