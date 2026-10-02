using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000145 RID: 325
	public class DefaultPartyWageModel : PartyWageModel
	{
		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06001A21 RID: 6689 RVA: 0x0008325D File Offset: 0x0008145D
		public override int MaxWagePaymentLimit
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x00083264 File Offset: 0x00081464
		public override int GetCharacterWage(CharacterObject character)
		{
			int num;
			switch (character.Tier)
			{
			case 0:
				num = 1;
				break;
			case 1:
				num = 2;
				break;
			case 2:
				num = 3;
				break;
			case 3:
				num = 5;
				break;
			case 4:
				num = 8;
				break;
			case 5:
				num = 12;
				break;
			case 6:
				num = 17;
				break;
			default:
				num = 23;
				break;
			}
			if (character.Occupation == Occupation.Mercenary)
			{
				num = (int)((float)num * 1.5f);
			}
			return num;
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x000832D4 File Offset: 0x000814D4
		public override ExplainedNumber GetTotalWage(MobileParty mobileParty, TroopRoster troopRoster, bool includeDescriptions = false)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			Hero hero = null;
			bool flag = !mobileParty.HasPerk(DefaultPerks.Steward.AidCorps, out hero, false);
			int num7 = 0;
			int num8 = 0;
			for (int i = 0; i < troopRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = troopRoster.GetElementCopyAtIndex(i);
				CharacterObject character = elementCopyAtIndex.Character;
				if (!flag)
				{
					int number = elementCopyAtIndex.Number;
					int woundedNumber = elementCopyAtIndex.WoundedNumber;
				}
				else
				{
					int number2 = elementCopyAtIndex.Number;
				}
				if (character.IsHero)
				{
					bool flag2 = mobileParty.IsMainParty && character.HeroObject.Clan == Clan.PlayerClan && character.HeroObject.Occupation == Occupation.Lord;
					Hero heroObject = elementCopyAtIndex.Character.HeroObject;
					Clan clan = character.HeroObject.Clan;
					if (heroObject != ((clan != null) ? clan.Leader : null) && !flag2)
					{
						if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Steward.PaidInPromise))
						{
							int num9 = MathF.Round((float)character.TroopWage * (1f + DefaultPerks.Steward.PaidInPromise.PrimaryBonus));
							num += num9;
						}
						else
						{
							num += character.TroopWage;
						}
					}
				}
				else
				{
					int num10 = character.TroopWage * elementCopyAtIndex.Number;
					num += num10;
					if (character.Culture.IsBandit)
					{
						num6 += num10;
					}
					if (character.IsInfantry)
					{
						num2 += num10;
					}
					if (character.IsMounted)
					{
						num3 += num10;
					}
					if (character.Occupation == Occupation.CaravanGuard)
					{
						num7 += num10;
					}
					if (character.Occupation == Occupation.Mercenary)
					{
						num8 += num10;
					}
					if (character.IsRanged)
					{
						num4 += num10;
						if (character.Tier >= 4)
						{
							num5 += num10;
						}
					}
				}
			}
			if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Roguery.DeepPockets))
			{
				num -= num6;
				ExplainedNumber explainedNumber = new ExplainedNumber((float)num6, false, null);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.DeepPockets, mobileParty.CurrentBattleEnvironment, mobileParty.LeaderHero.CharacterObject, false, ref explainedNumber);
				num += (int)explainedNumber.ResultNumber;
			}
			if (num5 > 0)
			{
				num -= num5;
				ExplainedNumber explainedNumber2 = new ExplainedNumber((float)num5, false, null);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Crossbow.PickedShots, mobileParty, true, ref explainedNumber2);
				num += (int)explainedNumber2.ResultNumber;
			}
			ExplainedNumber explainedNumber3 = new ExplainedNumber((float)num, includeDescriptions, null);
			explainedNumber3.LimitMin(0f);
			ExplainedNumber explainedNumber4 = new ExplainedNumber(1f, false, null);
			if (mobileParty.IsGarrison)
			{
				Settlement currentSettlement = mobileParty.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town : null) != null)
				{
					if (mobileParty.CurrentSettlement.IsFortification)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.MilitaryTradition, mobileParty.CurrentSettlement.Town, false, ref explainedNumber3);
						PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.Berserker, mobileParty.CurrentSettlement.Town, false, ref explainedNumber3);
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.DrillSergant, mobileParty.CurrentSettlement.Town, false, ref explainedNumber3);
						float num11 = (float)num2 / explainedNumber3.BaseNumber;
						this.CalculatePartialGarrisonWageReduction(num11, mobileParty, DefaultPerks.Polearm.StandardBearer, ref explainedNumber3, true);
						float num12 = (float)num4 / explainedNumber3.BaseNumber;
						this.CalculatePartialGarrisonWageReduction(num12, mobileParty, DefaultPerks.Crossbow.PeasantLeader, ref explainedNumber3, true);
						float num13 = (float)num3 / explainedNumber3.BaseNumber;
						this.CalculatePartialGarrisonWageReduction(num13, mobileParty, DefaultPerks.Riding.CavalryTactics, ref explainedNumber3, true);
					}
					if (mobileParty.CurrentSettlement.IsCastle)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.HunterClan, mobileParty.CurrentSettlement.Town, false, ref explainedNumber3);
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.StiffUpperLip, mobileParty.CurrentSettlement.Town, false, ref explainedNumber3);
					}
					FeatHelper.ApplyCultureFeat(mobileParty.CurrentSettlement.Owner.Culture, DefaultCulturalFeats.EmpireGarrisonWageFeat, ref explainedNumber3);
					mobileParty.CurrentSettlement.Town.AddEffectOfBuildings(BuildingEffectEnum.GarrisonWageReduction, ref explainedNumber4);
				}
			}
			float num14 = ((mobileParty.LeaderHero != null && mobileParty.LeaderHero.Clan.Kingdom != null && !mobileParty.LeaderHero.Clan.IsUnderMercenaryService && mobileParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.MilitaryCoronae)) ? 0.1f : 0f);
			Hero hero2 = null;
			if (mobileParty.HasPerk(DefaultPerks.Trade.SwordForBarter, out hero2, true))
			{
				float num15 = (float)num7 / explainedNumber3.BaseNumber;
				if (num15 > 0f)
				{
					float num16 = DefaultPerks.Trade.SwordForBarter.SecondaryBonus * num15;
					explainedNumber3.AddFactor(num16, DefaultPerks.Trade.SwordForBarter.Name);
				}
			}
			Hero hero3 = null;
			if (mobileParty.HasPerk(DefaultPerks.Steward.Contractors, out hero3, false))
			{
				float num17 = (float)num8 / explainedNumber3.BaseNumber;
				if (num17 > 0f)
				{
					float baseNumber = explainedNumber3.BaseNumber;
					float primaryBonus = DefaultPerks.Steward.Contractors.PrimaryBonus;
					explainedNumber3.AddFactor(DefaultPerks.Steward.Contractors.PrimaryBonus * num17, DefaultPerks.Steward.Contractors.Name);
				}
			}
			Hero hero4 = null;
			if (mobileParty.HasPerk(DefaultPerks.Trade.MercenaryConnections, out hero4, true))
			{
				float num18 = (float)num8 / explainedNumber3.BaseNumber;
				if (num18 > 0f)
				{
					float baseNumber2 = explainedNumber3.BaseNumber;
					float secondaryBonus = DefaultPerks.Trade.MercenaryConnections.SecondaryBonus;
					explainedNumber3.AddFactor(DefaultPerks.Trade.MercenaryConnections.SecondaryBonus * num18, DefaultPerks.Trade.MercenaryConnections.Name);
				}
			}
			explainedNumber3.AddFactor(num14, DefaultPolicies.MilitaryCoronae.Name);
			explainedNumber3.AddFactor(explainedNumber4.ResultNumber - 1f, this._buildingEffects);
			FeatHelper.ApplyCultureFeat(mobileParty.Party, DefaultCulturalFeats.AseraiIncreasedWageFeat, ref explainedNumber3);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.Frugal, mobileParty, true, ref explainedNumber3);
			if (mobileParty.Army != null)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.EfficientCampaigner, mobileParty, false, ref explainedNumber3);
			}
			if (mobileParty.SiegeEvent != null && mobileParty.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(mobileParty.Party, MapEvent.BattleTypes.Siege))
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.MasterOfWarcraft, mobileParty, true, ref explainedNumber3);
			}
			if (mobileParty.EffectiveQuartermaster != null)
			{
				PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Steward.PriceOfLoyalty, mobileParty.CurrentBattleEnvironment, mobileParty.EffectiveQuartermaster.CharacterObject, DefaultSkills.Steward, true, ref explainedNumber3, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
			}
			if (mobileParty.CurrentSettlement != null)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Trade.ContentTrades, mobileParty, false, ref explainedNumber3);
			}
			if (mobileParty.LeaderHero != null)
			{
				TraitEffectHelper.ApplyTraitEffect(mobileParty.LeaderHero, DefaultPersonalityTraitEffects.GenerosityUpkeepReductionEffect, ref explainedNumber3);
			}
			else if (mobileParty.IsGarrison)
			{
				Settlement currentSettlement2 = mobileParty.CurrentSettlement;
				Hero hero5;
				if (currentSettlement2 == null)
				{
					hero5 = null;
				}
				else
				{
					Town town = currentSettlement2.Town;
					hero5 = ((town != null) ? town.Governor : null);
				}
				Hero hero6 = hero5;
				if (hero6 != null && hero6.CurrentSettlement == mobileParty.CurrentSettlement)
				{
					TraitEffectHelper.ApplyTraitEffect(hero6, DefaultPersonalityTraitEffects.GenerosityUpkeepReductionEffect, ref explainedNumber3);
				}
			}
			return explainedNumber3;
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0008393C File Offset: 0x00081B3C
		private void CalculatePartialGarrisonWageReduction(float troopRatio, MobileParty mobileParty, PerkObject perk, ref ExplainedNumber garrisonWageReductionMultiplier, bool isSecondaryEffect)
		{
			if (troopRatio > 0f && mobileParty.CurrentSettlement.Town.Governor != null && PerkHelper.GetPerkValueForTown(perk, mobileParty.CurrentSettlement.Town))
			{
				garrisonWageReductionMultiplier.AddFactor(isSecondaryEffect ? (perk.SecondaryBonus * troopRatio) : (perk.PrimaryBonus * troopRatio), perk.Name);
			}
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0008399C File Offset: 0x00081B9C
		public override ExplainedNumber GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false)
		{
			ExplainedNumber explainedNumber;
			if (troop.Level <= 1)
			{
				explainedNumber = new ExplainedNumber(10f, false, null);
			}
			else if (troop.Level <= 6)
			{
				explainedNumber = new ExplainedNumber(20f, false, null);
			}
			else if (troop.Level <= 11)
			{
				explainedNumber = new ExplainedNumber(50f, false, null);
			}
			else if (troop.Level <= 16)
			{
				explainedNumber = new ExplainedNumber(100f, false, null);
			}
			else if (troop.Level <= 21)
			{
				explainedNumber = new ExplainedNumber(200f, false, null);
			}
			else if (troop.Level <= 26)
			{
				explainedNumber = new ExplainedNumber(400f, false, null);
			}
			else if (troop.Level <= 31)
			{
				explainedNumber = new ExplainedNumber(600f, false, null);
			}
			else if (troop.Level <= 36)
			{
				explainedNumber = new ExplainedNumber(1000f, false, null);
			}
			else
			{
				explainedNumber = new ExplainedNumber(1500f, false, null);
			}
			if (troop.Equipment.Horse.Item != null && !withoutItemCost)
			{
				if (troop.Level < 26)
				{
					explainedNumber.Add(150f, null, null);
				}
				else
				{
					explainedNumber.Add(500f, null, null);
				}
			}
			bool flag = troop.Occupation == Occupation.Mercenary || troop.Occupation == Occupation.Gangster || troop.Occupation == Occupation.CaravanGuard;
			if (flag)
			{
				explainedNumber.Add(explainedNumber.BaseNumber * 2f, null, null);
			}
			if (buyerHero != null)
			{
				MobileParty partyBelongedTo = buyerHero.PartyBelongedTo;
				if (partyBelongedTo != null)
				{
					if (troop.Tier >= 2)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Throwing.HeadHunter, partyBelongedTo, false, ref explainedNumber);
					}
					if (troop.IsInfantry)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.OneHanded.ChinkInTheArmor, partyBelongedTo, false, ref explainedNumber);
						PerkHelper.AddPerkBonusForParty(DefaultPerks.TwoHanded.ShowOfStrength, partyBelongedTo, false, ref explainedNumber);
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Polearm.HardyFrontline, partyBelongedTo, false, ref explainedNumber);
					}
					else if (troop.IsRanged)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.RenownedArcher, partyBelongedTo, false, ref explainedNumber);
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Crossbow.Piercer, partyBelongedTo, false, ref explainedNumber);
					}
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.Frugal, partyBelongedTo, false, ref explainedNumber);
				}
				if (troop.IsMounted)
				{
					FeatHelper.ApplyCultureFeat(buyerHero.Culture, DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat, ref explainedNumber);
				}
				if (flag)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Trade.SwordForBarter, BattleEnvironment.Any, buyerHero.CharacterObject, true, ref explainedNumber);
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Charm.SlickNegotiator, BattleEnvironment.Any, buyerHero.CharacterObject, true, ref explainedNumber);
				}
			}
			explainedNumber.LimitMin(1f);
			return explainedNumber;
		}

		// Token: 0x040008AC RID: 2220
		private readonly TextObject _cultureText = GameTexts.FindText("str_culture", null);

		// Token: 0x040008AD RID: 2221
		private readonly TextObject _buildingEffects = GameTexts.FindText("str_building_effects", null);

		// Token: 0x040008AE RID: 2222
		private const float MercenaryWageFactor = 1.5f;
	}
}
