using System;
using System.Linq;
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
	// Token: 0x02000155 RID: 341
	public class DefaultSettlementMilitiaModel : SettlementMilitiaModel
	{
		// Token: 0x06001AAB RID: 6827 RVA: 0x00086F80 File Offset: 0x00085180
		public override int MilitiaToSpawnAfterSiege(Town town)
		{
			return 2 * (45 + MBRandom.RandomInt(10));
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x00086F8E File Offset: 0x0008518E
		public override ExplainedNumber CalculateMilitiaChange(Settlement settlement, bool includeDescriptions = false)
		{
			return DefaultSettlementMilitiaModel.CalculateMilitiaChangeInternal(settlement, includeDescriptions);
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00086F98 File Offset: 0x00085198
		public override ExplainedNumber CalculateVeteranMilitiaSpawnChance(Settlement settlement)
		{
			ExplainedNumber explainedNumber = default(ExplainedNumber);
			Town town = null;
			if (settlement.IsFortification)
			{
				town = settlement.Town;
			}
			else if (settlement.IsVillage)
			{
				Settlement tradeBound = settlement.Village.TradeBound;
				if (((tradeBound != null) ? tradeBound.Town : null) != null)
				{
					town = settlement.Village.TradeBound.Town;
				}
			}
			if (town != null)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Leadership.CitizenMilitia, town, true, ref explainedNumber);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Polearm.Drills, town, true, ref explainedNumber);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.SevenVeterans, town, false, ref explainedNumber);
			}
			FeatHelper.ApplyCultureFeat(settlement.OwnerClan.Culture, DefaultCulturalFeats.BattanianMilitiaFeat, ref explainedNumber);
			if (settlement.IsFortification)
			{
				settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.MilitiaVeterancyChance, ref explainedNumber);
			}
			if (settlement.OwnerClan.Kingdom != null && settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.LandGrantsForVeteran))
			{
				explainedNumber.AddFactor(0.1f, null);
			}
			return explainedNumber;
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x00087087 File Offset: 0x00085287
		public override void CalculateMilitiaSpawnRate(Settlement settlement, out float meleeTroopRate, out float rangedTroopRate)
		{
			meleeTroopRate = 0.5f;
			rangedTroopRate = 1f - meleeTroopRate;
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x0008709C File Offset: 0x0008529C
		private static ExplainedNumber CalculateMilitiaChangeInternal(Settlement settlement, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			if (settlement.IsVillage && settlement.Village.VillageState != Village.VillageStates.Normal)
			{
				return explainedNumber;
			}
			float militia = settlement.Militia;
			if (settlement.IsFortification)
			{
				explainedNumber.Add(2f, DefaultSettlementMilitiaModel.BaseText, null);
			}
			else if (settlement.IsVillage)
			{
				explainedNumber.Add(0.5f, DefaultSettlementMilitiaModel.BaseText, null);
			}
			float num = -militia * 0.025f;
			explainedNumber.Add(num, DefaultSettlementMilitiaModel.RetiredText, null);
			if (settlement.IsVillage)
			{
				float num2 = settlement.Village.Hearth / 400f;
				explainedNumber.Add(num2, DefaultSettlementMilitiaModel.FromHearthsText, null);
			}
			else if (settlement.IsFortification)
			{
				float num3 = settlement.Town.Prosperity / 1000f;
				explainedNumber.Add(num3, DefaultSettlementMilitiaModel.FromProsperityText, null);
				if (settlement.Town.InRebelliousState)
				{
					float num4 = MBMath.Map(settlement.Town.Loyalty, 0f, (float)Campaign.Current.Models.SettlementLoyaltyModel.RebelliousStateStartLoyaltyThreshold, (float)Campaign.Current.Models.SettlementLoyaltyModel.MilitiaBoostPercentage, 0f);
					float num5 = MathF.Abs(num3 * (num4 * 0.01f));
					explainedNumber.Add(num5, DefaultSettlementMilitiaModel.LowLoyaltyText, null);
				}
			}
			if (settlement.IsTown)
			{
				int num6 = settlement.Town.SoldItems.Sum<Town.SellLog>(delegate(Town.SellLog x)
				{
					if (x.Category.Properties != ItemCategory.Property.BonusToMilitia)
					{
						return 0;
					}
					return x.Number;
				});
				if (num6 > 0)
				{
					explainedNumber.Add(0.2f * (float)num6, DefaultSettlementMilitiaModel.MilitiaFromMarketText, null);
				}
				if (settlement.OwnerClan.Kingdom != null)
				{
					if (settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Serfdom) && settlement.IsTown)
					{
						explainedNumber.Add(-1f, DefaultPolicies.Serfdom.Name, null);
					}
					if (settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Cantons))
					{
						explainedNumber.Add(1f, DefaultPolicies.Cantons.Name, null);
					}
				}
				FeatHelper.ApplyCultureFeat(settlement.OwnerClan.Culture, DefaultCulturalFeats.BattanianMilitiaFeat, ref explainedNumber);
			}
			if (settlement.IsCastle || settlement.IsTown)
			{
				settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.Militia, ref explainedNumber);
				if (settlement.IsCastle && settlement.Town.InRebelliousState)
				{
					settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.MilitiaReduction, ref explainedNumber);
				}
				DefaultSettlementMilitiaModel.GetSettlementMilitiaChangeDueToPolicies(settlement, ref explainedNumber);
				DefaultSettlementMilitiaModel.GetSettlementMilitiaChangeDueToPerks(settlement, ref explainedNumber);
				DefaultSettlementMilitiaModel.GetSettlementMilitiaChangeDueToIssues(settlement, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x00087330 File Offset: 0x00085530
		private static void GetSettlementMilitiaChangeDueToPerks(Settlement settlement, ref ExplainedNumber result)
		{
			if (settlement.Town != null && settlement.Town.Governor != null)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.SwiftStrike, settlement.Town, false, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Polearm.KeepAtBay, settlement.Town, false, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.MerryMen, settlement.Town, false, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Crossbow.LongShots, settlement.Town, false, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Throwing.SlingingCompetitions, settlement.Town, false, ref result);
				if (settlement.IsUnderSiege)
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Roguery.ArmsDealer, settlement.Town, false, ref result);
				}
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.SevenVeterans, settlement.Town, false, ref result);
			}
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x000873E8 File Offset: 0x000855E8
		private static void GetSettlementMilitiaChangeDueToPolicies(Settlement settlement, ref ExplainedNumber result)
		{
			Kingdom kingdom = settlement.OwnerClan.Kingdom;
			if (kingdom != null && kingdom.ActivePolicies.Contains(DefaultPolicies.Citizenship))
			{
				result.Add(1f, DefaultPolicies.Citizenship.Name, null);
			}
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x0008742C File Offset: 0x0008562C
		private static void GetSettlementMilitiaChangeDueToIssues(Settlement settlement, ref ExplainedNumber result)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementMilitia, settlement, ref result);
		}

		// Token: 0x040008CF RID: 2255
		private static readonly TextObject BaseText = new TextObject("{=militarybase}Base", null);

		// Token: 0x040008D0 RID: 2256
		private static readonly TextObject FromHearthsText = new TextObject("{=ecdZglky}From Hearths", null);

		// Token: 0x040008D1 RID: 2257
		private static readonly TextObject FromProsperityText = new TextObject("{=cTmiNAlI}From Prosperity", null);

		// Token: 0x040008D2 RID: 2258
		private static readonly TextObject RetiredText = new TextObject("{=gHnfFi1s}Retired", null);

		// Token: 0x040008D3 RID: 2259
		private static readonly TextObject MilitiaFromMarketText = new TextObject("{=7ve3bQxg}Weapons From Market", null);

		// Token: 0x040008D4 RID: 2260
		private static readonly TextObject LowLoyaltyText = new TextObject("{=SJ2qsRdF}Low Loyalty", null);

		// Token: 0x040008D5 RID: 2261
		private static readonly TextObject CultureText = GameTexts.FindText("str_culture", null);

		// Token: 0x040008D6 RID: 2262
		private const int AutoSpawnMilitiaDayMultiplierAfterSiege = 25;

		// Token: 0x040008D7 RID: 2263
		private const int BaseFortificationMilitiaChange = 2;

		// Token: 0x040008D8 RID: 2264
		private const float BaseVillageMilitiaChange = 0.5f;
	}
}
