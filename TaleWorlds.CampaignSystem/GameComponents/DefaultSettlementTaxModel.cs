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
	// Token: 0x02000159 RID: 345
	public class DefaultSettlementTaxModel : SettlementTaxModel
	{
		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x000887A4 File Offset: 0x000869A4
		public override float SettlementCommissionRateTown
		{
			get
			{
				return 0.7f;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x000887AB File Offset: 0x000869AB
		public override float SettlementCommissionRateVillage
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x000887B2 File Offset: 0x000869B2
		public override int SettlementCommissionDecreaseSecurityThreshold
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x000887B6 File Offset: 0x000869B6
		public override int MaximumDecreaseBasedOnSecuritySecurity
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x000887BC File Offset: 0x000869BC
		public override float GetTownTaxRatio(Town town)
		{
			float num = 1f;
			if (town.Settlement.OwnerClan.Kingdom != null && town.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.CrownDuty))
			{
				num += 0.05f;
			}
			return this.SettlementCommissionRateTown * num;
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x00088814 File Offset: 0x00086A14
		public override float GetVillageTaxRatio(Village village)
		{
			float num = this.SettlementCommissionRateVillage;
			if (village.Settlement.OwnerClan.Kingdom != null && village.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.LandGrantsForVeteran))
			{
				num -= num * 0.05f;
			}
			return num;
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00088868 File Offset: 0x00086A68
		public override float GetTownCommissionChangeBasedOnSecurity(Town town, float commission)
		{
			if (town.Security < (float)this.SettlementCommissionDecreaseSecurityThreshold)
			{
				float num = MBMath.Map((float)this.SettlementCommissionDecreaseSecurityThreshold - town.Security, 0f, (float)this.SettlementCommissionDecreaseSecurityThreshold, (float)this.MaximumDecreaseBasedOnSecuritySecurity, 0f);
				commission -= commission * (num * 0.01f);
				return commission;
			}
			return commission;
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x000888C0 File Offset: 0x00086AC0
		public override ExplainedNumber CalculateTownTax(Town town, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateDailyTaxInternal(town, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x000888E8 File Offset: 0x00086AE8
		private float CalculateDailyTax(Town town, ref ExplainedNumber explainedNumber)
		{
			float prosperity = town.Prosperity;
			float num = 1f;
			if (town.Settlement.OwnerClan.Kingdom != null && town.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.CouncilOfTheCommons))
			{
				num -= 0.05f;
			}
			float num2 = 0.35f;
			float num3 = prosperity * num2 * num;
			explainedNumber.Add(num3, this.ProsperityText, null);
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x0008895C File Offset: 0x00086B5C
		private void CalculateDailyTaxInternal(Town town, ref ExplainedNumber result)
		{
			float num = this.CalculateDailyTax(town, ref result);
			this.CalculatePolicyGoldCut(town, num, ref result);
			if (PerkHelper.GetPerkValueForTown(DefaultPerks.Bow.QuickDraw, town))
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.QuickDraw, town, false, ref result);
			}
			if (town.Governor != null)
			{
				if (town.Governor.GetPerkValue(DefaultPerks.Steward.Logistician))
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.Logistician, town, false, ref result);
				}
				PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Steward.PriceOfLoyalty, BattleEnvironment.Any, town.Governor.CharacterObject, DefaultSkills.Steward, false, ref result, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus);
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Scouting.DesertBorn, town))
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Scouting.DesertBorn, town, false, ref result);
				}
			}
			if (town.IsTown)
			{
				FeatHelper.ApplyCultureFeat(town.OwnerClan.Culture, DefaultCulturalFeats.KhuzaitDecreasedTaxFeat, ref result);
			}
			this.GetSettlementTaxChangeDueToIssues(town, ref result);
			this.CalculateSettlementTaxDueToSecurity(town, ref result);
			this.CalculateSettlementTaxDueToLoyalty(town, ref result);
			this.CalculateSettlementTaxDueToBuildings(town, ref result);
			result.Clamp(0f, float.MaxValue);
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x00088A5C File Offset: 0x00086C5C
		private void CalculateSettlementTaxDueToSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			SettlementSecurityModel settlementSecurityModel = Campaign.Current.Models.SettlementSecurityModel;
			if (town.Security >= (float)settlementSecurityModel.ThresholdForTaxBoost)
			{
				settlementSecurityModel.CalculateGoldGainDueToHighSecurity(town, ref explainedNumber);
				return;
			}
			if (town.Security >= (float)settlementSecurityModel.ThresholdForHigherTaxCorruption && town.Security < (float)settlementSecurityModel.ThresholdForTaxCorruption)
			{
				settlementSecurityModel.CalculateGoldCutDueToLowSecurity(town, ref explainedNumber);
			}
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x00088AB8 File Offset: 0x00086CB8
		private void CalculateSettlementTaxDueToLoyalty(Town town, ref ExplainedNumber explainedNumber)
		{
			SettlementLoyaltyModel settlementLoyaltyModel = Campaign.Current.Models.SettlementLoyaltyModel;
			if (town.Loyalty >= (float)settlementLoyaltyModel.ThresholdForTaxBoost)
			{
				settlementLoyaltyModel.CalculateGoldGainDueToHighLoyalty(town, ref explainedNumber);
				return;
			}
			if (town.Loyalty >= (float)settlementLoyaltyModel.ThresholdForHigherTaxCorruption && town.Loyalty <= (float)settlementLoyaltyModel.ThresholdForTaxCorruption)
			{
				settlementLoyaltyModel.CalculateGoldCutDueToLowLoyalty(town, ref explainedNumber);
				return;
			}
			if (town.Loyalty < (float)settlementLoyaltyModel.ThresholdForHigherTaxCorruption)
			{
				explainedNumber.AddFactor(-1f, DefaultSettlementTaxModel.VeryLowLoyalty);
			}
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x00088B33 File Offset: 0x00086D33
		private void CalculateSettlementTaxDueToBuildings(Town town, ref ExplainedNumber result)
		{
			town.AddEffectOfBuildings(BuildingEffectEnum.TaxPerDay, ref result);
			town.AddEffectOfBuildings(BuildingEffectEnum.DenarByBoundVillageHeartPerDay, ref result);
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x00088B48 File Offset: 0x00086D48
		private void CalculatePolicyGoldCut(Town town, float rawTax, ref ExplainedNumber explainedNumber)
		{
			if (town.MapFaction.IsKingdomFaction)
			{
				Kingdom kingdom = (Kingdom)town.MapFaction;
				if (town.IsTown)
				{
					if (kingdom.ActivePolicies.Contains(DefaultPolicies.Magistrates))
					{
						explainedNumber.Add(-0.05f * rawTax, DefaultPolicies.Magistrates.Name, null);
					}
					if (kingdom.ActivePolicies.Contains(DefaultPolicies.Bailiffs))
					{
						explainedNumber.Add(-0.05f * rawTax, DefaultPolicies.Bailiffs.Name, null);
					}
					if (kingdom.ActivePolicies.Contains(DefaultPolicies.TribunesOfThePeople))
					{
						explainedNumber.Add(-0.05f * rawTax, DefaultPolicies.TribunesOfThePeople.Name, null);
					}
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.Cantons))
				{
					explainedNumber.Add(-0.1f * rawTax, DefaultPolicies.Cantons.Name, null);
				}
			}
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x00088C21 File Offset: 0x00086E21
		private void GetSettlementTaxChangeDueToIssues(Town center, ref ExplainedNumber result)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementTax, center.Owner.Settlement, ref result);
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x00088C48 File Offset: 0x00086E48
		public override int CalculateVillageTaxFromIncome(Village village, int marketIncome)
		{
			if (marketIncome == 0)
			{
				return 0;
			}
			return (int)((float)marketIncome * Campaign.Current.Models.SettlementTaxModel.GetVillageTaxRatio(village));
		}

		// Token: 0x040008EF RID: 2287
		private readonly TextObject ProsperityText = GameTexts.FindText("str_prosperity", null);

		// Token: 0x040008F0 RID: 2288
		private static readonly TextObject VeryLowLoyalty = new TextObject("{=CcQzFnpN}Very Low Loyalty", null);
	}
}
