using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000100 RID: 256
	public class DefaultBuildingConstructionModel : BuildingConstructionModel
	{
		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001726 RID: 5926 RVA: 0x0006BE77 File Offset: 0x0006A077
		public override int TownBoostCost
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001727 RID: 5927 RVA: 0x0006BE7E File Offset: 0x0006A07E
		public override int TownBoostBonus
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001728 RID: 5928 RVA: 0x0006BE82 File Offset: 0x0006A082
		public override int CastleBoostCost
		{
			get
			{
				return 250;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001729 RID: 5929 RVA: 0x0006BE89 File Offset: 0x0006A089
		public override int CastleBoostBonus
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x0006BE90 File Offset: 0x0006A090
		public override ExplainedNumber CalculateDailyConstructionPower(Town town, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateDailyConstructionPowerInternal(town, ref explainedNumber, false);
			return explainedNumber;
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x0006BEB8 File Offset: 0x0006A0B8
		public override int CalculateDailyConstructionPowerWithoutBoost(Town town)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			return this.CalculateDailyConstructionPowerInternal(town, ref explainedNumber, true);
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x0006BEE0 File Offset: 0x0006A0E0
		public override int GetBoostAmount(Town town)
		{
			object obj = (town.IsCastle ? this.CastleBoostBonus : this.TownBoostBonus);
			float num = 0f;
			if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Steward.Relocation))
			{
				num += DefaultPerks.Steward.Relocation.SecondaryBonus;
			}
			if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Trade.SpringOfGold))
			{
				num += DefaultPerks.Trade.SpringOfGold.SecondaryBonus;
			}
			object obj2 = obj;
			return obj2 + (int)(obj2 * num);
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x0006BF60 File Offset: 0x0006A160
		public override int GetBoostCost(Town town)
		{
			float num = (float)(town.IsCastle ? this.CastleBoostCost : this.TownBoostCost);
			if (town.Governor != null)
			{
				Settlement currentSettlement = town.Governor.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town : null) == town)
				{
					float traitEffectBonus = TraitEffectHelper.GetTraitEffectBonus(town.Governor, DefaultPersonalityTraitEffects.GenerosityTownProjectEffect);
					num *= 1f - traitEffectBonus;
				}
			}
			return MathF.Round(num);
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x0006BFC8 File Offset: 0x0006A1C8
		private int CalculateDailyConstructionPowerInternal(Town town, ref ExplainedNumber result, bool omitBoost = false)
		{
			float num = town.Prosperity * 0.01f;
			result.Add(num, GameTexts.FindText("str_prosperity", null), null);
			if (!omitBoost && town.BoostBuildingProcess > 0)
			{
				int boostCost = Campaign.Current.Models.BuildingConstructionModel.GetBoostCost(town);
				int num2 = Campaign.Current.Models.BuildingConstructionModel.GetBoostAmount(town);
				float num3 = MathF.Min(1f, (float)town.BoostBuildingProcess / (float)boostCost);
				float num4 = 0f;
				if (town.IsTown && town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Engineering.Clockwork))
				{
					num4 += DefaultPerks.Engineering.Clockwork.SecondaryBonus;
				}
				num2 += MathF.Round((float)num2 * num4);
				result.Add((float)num2 * num3, DefaultBuildingConstructionModel.BoostText, null);
			}
			if (town.Governor != null)
			{
				Settlement currentSettlement = town.Governor.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town : null) == town)
				{
					SkillHelper.AddSkillBonusForTown(DefaultSkillEffects.TownProjectBuildingBonus, town, ref result);
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.ForcedLabor, town, false, ref result);
				}
			}
			if (town.Governor != null)
			{
				Settlement currentSettlement2 = town.Governor.CurrentSettlement;
				if (((currentSettlement2 != null) ? currentSettlement2.Town : null) == town && !town.BuildingsInProgress.IsEmpty<Building>())
				{
					if (town.Governor.GetPerkValue(DefaultPerks.Steward.ForcedLabor) && town.Settlement.Party.PrisonRoster.TotalManCount > 0)
					{
						float num5 = MathF.Min(0.3f, (float)town.Settlement.Party.PrisonRoster.TotalManCount / 3f * DefaultPerks.Steward.ForcedLabor.SecondaryBonus);
						result.AddFactor(num5, DefaultPerks.Steward.ForcedLabor.Name);
					}
					if (town.IsCastle)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Engineering.MilitaryPlanner, town, false, ref result);
					}
					else if (town.IsTown)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Engineering.Carpenters, town, false, ref result);
					}
					Building building = town.BuildingsInProgress.Peek();
					if (building.BuildingType == DefaultBuildingTypes.SettlementFortifications || building.BuildingType == DefaultBuildingTypes.CastleBarracks || building.BuildingType == DefaultBuildingTypes.SettlementBarracks)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Engineering.Stonecutters, town, true, ref result);
					}
				}
			}
			int num6 = town.SoldItems.Sum<Town.SellLog>(delegate(Town.SellLog x)
			{
				if (x.Category.Properties != ItemCategory.Property.BonusToProduction)
				{
					return 0;
				}
				return x.Number;
			});
			if (num6 > 0)
			{
				result.Add(0.25f * (float)num6, DefaultBuildingConstructionModel.ProductionFromMarketText, null);
			}
			BuildingType buildingType = (town.BuildingsInProgress.IsEmpty<Building>() ? null : town.BuildingsInProgress.Peek().BuildingType);
			if (buildingType != null && buildingType.IsMilitaryProject)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.Confidence, town, false, ref result);
			}
			if (buildingType == DefaultBuildingTypes.SettlementMarketplace)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Trade.SelfMadeMan, town, false, ref result);
			}
			town.AddEffectOfBuildings(BuildingEffectEnum.ConstructionPerDay, ref result);
			if (town.Loyalty >= 75f)
			{
				float num7 = MBMath.Map(town.Loyalty, 75f, 100f, 0f, 0.2f);
				result.AddFactor(num7, DefaultBuildingConstructionModel.HighLoyaltyBonusText);
			}
			else if (town.Loyalty > 25f && town.Loyalty <= 50f)
			{
				float num8 = MBMath.Map(town.Loyalty, 25f, 50f, 0.5f, 0f);
				result.AddFactor(-num8, DefaultBuildingConstructionModel.LowLoyaltyPenaltyText);
			}
			else if (town.Loyalty <= 25f)
			{
				result.LimitMax(0f, DefaultBuildingConstructionModel.VeryLowLoyaltyPenaltyText);
			}
			if (town.Loyalty > 25f)
			{
				FeatHelper.ApplyCultureFeat(town.OwnerClan.Culture, DefaultCulturalFeats.BattanianConstructionFeat, ref result);
			}
			result.LimitMin(0f);
			return (int)result.ResultNumber;
		}

		// Token: 0x040007B2 RID: 1970
		private const float HammerMultiplier = 0.01f;

		// Token: 0x040007B3 RID: 1971
		private const int VeryLowLoyaltyValue = 25;

		// Token: 0x040007B4 RID: 1972
		private const float MediumLoyaltyValue = 50f;

		// Token: 0x040007B5 RID: 1973
		private const float HighLoyaltyValue = 75f;

		// Token: 0x040007B6 RID: 1974
		private const float HighestLoyaltyValue = 100f;

		// Token: 0x040007B7 RID: 1975
		private static readonly TextObject ProductionFromMarketText = new TextObject("{=vaZDJGMx}Construction from Market", null);

		// Token: 0x040007B8 RID: 1976
		private static readonly TextObject BoostText = new TextObject("{=yX1RycON}Boost from Reserve", null);

		// Token: 0x040007B9 RID: 1977
		private static readonly TextObject HighLoyaltyBonusText = new TextObject("{=aSniKUJv}High Loyalty", null);

		// Token: 0x040007BA RID: 1978
		private static readonly TextObject LowLoyaltyPenaltyText = new TextObject("{=SJ2qsRdF}Low Loyalty", null);

		// Token: 0x040007BB RID: 1979
		private static readonly TextObject VeryLowLoyaltyPenaltyText = new TextObject("{=CcQzFnpN}Very Low Loyalty", null);

		// Token: 0x040007BC RID: 1980
		private readonly TextObject CultureText = GameTexts.FindText("str_culture", null);
	}
}
