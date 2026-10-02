using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200013F RID: 319
	public class DefaultPartySizeLimitModel : PartySizeLimitModel
	{
		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060019DC RID: 6620 RVA: 0x00080EB7 File Offset: 0x0007F0B7
		public override int MinimumNumberOfVillagersAtVillagerParty
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x00080FAC File Offset: 0x0007F1AC
		public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			if (!party.IsMobile)
			{
				return explainedNumber;
			}
			if (party.MobileParty.IsGarrison)
			{
				return this.CalculateGarrisonPartySizeLimit(party.MobileParty.GarrisonPartyComponent.Settlement, includeDescriptions);
			}
			if (party.MobileParty.IsPatrolParty)
			{
				return this.CalculatePatrolPartySizeLimit(party.MobileParty, includeDescriptions);
			}
			return this.CalculateMobilePartyMemberSizeLimit(party.MobileParty, includeDescriptions);
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x00081020 File Offset: 0x0007F220
		private ExplainedNumber CalculatePatrolPartySizeLimit(MobileParty mobileParty, bool includeDescriptions)
		{
			new ExplainedNumber(10f, includeDescriptions, null);
			foreach (Building building in mobileParty.HomeSettlement.Town.Buildings)
			{
				if (building.BuildingType == DefaultBuildingTypes.SettlementGuardHouse)
				{
					return new ExplainedNumber((float)this.GetPatrolPartySizeLimitFromGuardHouseLevel(building.CurrentLevel), includeDescriptions, null);
				}
			}
			return new ExplainedNumber(0f, includeDescriptions, null);
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x000810B8 File Offset: 0x0007F2B8
		private int GetPatrolPartySizeLimitFromGuardHouseLevel(int level)
		{
			return 10 + 5 * level;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x000810C0 File Offset: 0x0007F2C0
		public override ExplainedNumber GetPartyPrisonerSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			if (party.IsSettlement)
			{
				return this.CalculateSettlementPartyPrisonerSizeLimitInternal(party.Settlement, includeDescriptions);
			}
			return this.CalculateMobilePartyPrisonerSizeLimitInternal(party, includeDescriptions);
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x000810E0 File Offset: 0x0007F2E0
		private ExplainedNumber CalculateMobilePartyMemberSizeLimit(MobileParty party, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(20f, includeDescriptions, this._baseSizeText);
			if (party.LeaderHero != null && party.LeaderHero.Clan != null && !party.IsCaravan)
			{
				this.CalculateBaseMemberSize(party.LeaderHero, party.MapFaction, party.ActualClan, ref explainedNumber);
				SkillHelper.AddSkillBonusForParty(DefaultSkillEffects.StewardPartySizeBonus, party, ref explainedNumber);
				if (DefaultPartySizeLimitModel._addAdditionalPartySizeAsCheat && party.IsMainParty && Game.Current.CheatMode)
				{
					explainedNumber.Add(5000f, new TextObject("{=!}Additional size from extra party cheat", null), null);
				}
			}
			else if (party.IsCaravan)
			{
				if (party.Party.Owner == Hero.MainHero)
				{
					int num = (party.CaravanPartyComponent.IsElite ? 30 : 10);
					if (party.CaravanPartyComponent.CanHaveNavalNavigationCapability)
					{
						num = (party.CaravanPartyComponent.IsElite ? 46 : 33);
					}
					explainedNumber.Add((float)num, this._randomSizeBonusTemporary, null);
				}
				else
				{
					Hero owner = party.Party.Owner;
					if (owner != null && owner.IsNotable)
					{
						explainedNumber.Add((float)(10 * ((party.Party.Owner.Power < 100f) ? 1 : ((party.Party.Owner.Power < 200f) ? 2 : 3))), this._randomSizeBonusTemporary, null);
					}
				}
			}
			else if (party.IsVillager)
			{
				explainedNumber.Add(40f, this._randomSizeBonusTemporary, null);
			}
			if (party.IsCurrentlyAtSea)
			{
				foreach (Ship ship in party.Ships)
				{
					explainedNumber.AddFactor(ship.CrewCapacityBonusFactor, ship.Name);
				}
			}
			return explainedNumber;
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x000812C4 File Offset: 0x0007F4C4
		public override ExplainedNumber CalculateGarrisonPartySizeLimit(Settlement settlement, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(200f, includeDescriptions, this._baseSizeText);
			SkillHelper.AddSkillBonusForCharacter(DefaultSkillEffects.LeadershipGarrisonSizeBonus, settlement.OwnerClan.Leader.CharacterObject, ref explainedNumber);
			if (settlement.IsTown)
			{
				explainedNumber.Add(200f, this._townBonusText, null);
			}
			this.AddGarrisonOwnerPerkEffects(settlement, ref explainedNumber);
			this.AddSettlementProjectBonuses(settlement, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00081330 File Offset: 0x0007F530
		private ExplainedNumber CalculateSettlementPartyPrisonerSizeLimitInternal(Settlement settlement, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(60f, includeDescriptions, this._baseSizeText);
			if (settlement.Town != null)
			{
				explainedNumber.Add(40f, this._baseSizeText, null);
			}
			this.AddSettlementProjectPrisonerBonuses(settlement, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x00081378 File Offset: 0x0007F578
		private ExplainedNumber CalculateMobilePartyPrisonerSizeLimitInternal(PartyBase party, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(10f, includeDescriptions, this._baseSizeText);
			explainedNumber.Add((float)this.GetCurrentPartySizeEffect(party), this._currentPartySizeBonusText, null);
			this.AddMobilePartyLeaderPrisonerSizePerkEffects(party, ref explainedNumber);
			if (DefaultPartySizeLimitModel._addAdditionalPrisonerSizeAsCheat && party.IsMobile && party.MobileParty.IsMainParty && Game.Current.CheatMode)
			{
				explainedNumber.Add(5000f, new TextObject("{=!}Additional size from extra prisoner cheat", null), null);
			}
			return explainedNumber;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x000813F8 File Offset: 0x0007F5F8
		private void AddMobilePartyLeaderPrisonerSizePerkEffects(PartyBase party, ref ExplainedNumber result)
		{
			PerkHelper.AddPerkBonusForParty(DefaultPerks.TwoHanded.Terror, party.MobileParty, false, ref result);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.Stamina, party.MobileParty, false, ref result);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.Manhunter, party.MobileParty, false, ref result);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.VantagePoint, party.MobileParty, false, ref result);
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x00081451 File Offset: 0x0007F651
		private void AddGarrisonOwnerPerkEffects(Settlement currentSettlement, ref ExplainedNumber result)
		{
			if (currentSettlement != null && currentSettlement.IsFortification)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.CorpsACorps, currentSettlement.Town, false, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Leadership.VeteransRespect, currentSettlement.Town, true, ref result);
			}
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x00081484 File Offset: 0x0007F684
		public override int GetNextClanTierPartySizeEffectChangeForHero(Hero hero)
		{
			int tierEffectInternal = this.GetTierEffectInternal(hero.Clan.Tier, hero.Clan.Leader == hero);
			return this.GetTierEffectInternal(hero.Clan.Tier + 1, hero.Clan.Leader == hero) - tierEffectInternal;
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x000814D4 File Offset: 0x0007F6D4
		private int GetTierEffectInternal(int tier, bool isHeroClanLeader)
		{
			if (tier < 1)
			{
				return 0;
			}
			if (isHeroClanLeader)
			{
				return 25 * tier;
			}
			return 15 * tier;
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x000814E8 File Offset: 0x0007F6E8
		public override int GetAssumedPartySizeForLordParty(Hero leaderHero, IFaction partyMapFaction, Clan actualClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(20f, false, this._baseSizeText);
			if (leaderHero != null && leaderHero.Clan != null)
			{
				this.CalculateBaseMemberSize(leaderHero, partyMapFaction, actualClan, ref explainedNumber);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.StewardPartySizeBonus, ref explainedNumber, leaderHero.GetSkillValue(DefaultSkills.Steward));
			}
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x0008153D File Offset: 0x0007F73D
		public override int GetClanTierPartySizeEffectForHero(Hero hero)
		{
			return this.GetTierEffectInternal(hero.Clan.Tier, hero.Clan.Leader == hero);
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0008155E File Offset: 0x0007F75E
		private void AddSettlementProjectBonuses(Settlement settlement, ref ExplainedNumber result)
		{
			if (settlement != null && settlement.IsFortification)
			{
				settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.GarrisonCapacity, ref result);
			}
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x00081578 File Offset: 0x0007F778
		private void AddSettlementProjectPrisonerBonuses(Settlement settlement, ref ExplainedNumber result)
		{
			if (settlement != null && settlement.IsFortification)
			{
				settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.PrisonCapacity, ref result);
			}
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x00081593 File Offset: 0x0007F793
		private int GetCurrentPartySizeEffect(PartyBase party)
		{
			return party.NumberOfHealthyMembers / 2;
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x000815A0 File Offset: 0x0007F7A0
		private void CalculateBaseMemberSize(Hero partyLeader, IFaction partyMapFaction, Clan actualClan, ref ExplainedNumber result)
		{
			if (partyMapFaction != null && partyMapFaction.IsKingdomFaction && partyLeader.MapFaction.Leader == partyLeader)
			{
				result.Add(20f, this._factionLeaderText, null);
			}
			if (partyLeader.PartyBelongedTo != null)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.OneHanded.Prestige, partyLeader.PartyBelongedTo, false, ref result);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.TwoHanded.Hope, partyLeader.PartyBelongedTo, false, ref result);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.ImposingStature, partyLeader.PartyBelongedTo, false, ref result);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.MerryMen, partyLeader.PartyBelongedTo, true, ref result);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.HordeLeader, partyLeader.PartyBelongedTo, true, ref result);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.MountedScouts, partyLeader.PartyBelongedTo, false, ref result);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.Authority, partyLeader.PartyBelongedTo, false, ref result);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.UpliftingSpirit, partyLeader.PartyBelongedTo, false, ref result);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.TalentMagnet, partyLeader.PartyBelongedTo, true, ref result);
			}
			PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Leadership.UltimateLeader, BattleEnvironment.Any, partyLeader.CharacterObject, DefaultSkills.Leadership, true, ref result, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
			if (actualClan != null)
			{
				Hero leader = actualClan.Leader;
				bool? flag = ((leader != null) ? new bool?(leader.GetPerkValue(DefaultPerks.Leadership.LeaderOfMasses)) : null);
				bool flag2 = true;
				if ((flag.GetValueOrDefault() == flag2) & (flag != null))
				{
					int num = 0;
					using (List<Settlement>.Enumerator enumerator = actualClan.Settlements.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.IsTown)
							{
								num++;
							}
						}
					}
					float num2 = (float)num * DefaultPerks.Leadership.LeaderOfMasses.PrimaryBonus;
					if (num2 > 0f)
					{
						result.Add(num2, DefaultPerks.Leadership.LeaderOfMasses.Name, null);
					}
				}
			}
			if (partyLeader.Clan.Leader == partyLeader)
			{
				if (partyLeader.Clan.Tier >= 5 && partyMapFaction.IsKingdomFaction && ((Kingdom)partyMapFaction).ActivePolicies.Contains(DefaultPolicies.NobleRetinues))
				{
					result.Add(40f, DefaultPolicies.NobleRetinues.Name, null);
				}
				if (partyMapFaction.IsKingdomFaction && partyMapFaction.Leader == partyLeader && ((Kingdom)partyMapFaction).ActivePolicies.Contains(DefaultPolicies.RoyalGuard))
				{
					result.Add(60f, DefaultPolicies.RoyalGuard.Name, null);
				}
			}
			result.Add((float)Campaign.Current.Models.PartySizeLimitModel.GetClanTierPartySizeEffectForHero(partyLeader), this._clanTierText, null);
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00081838 File Offset: 0x0007FA38
		private float GetPartySizeRatioForSize(PartyTemplateObject partyTemplate, int desiredSize)
		{
			int num = partyTemplate.Stacks.Sum<PartyTemplateStack>((PartyTemplateStack s) => s.MinValue);
			int num2 = partyTemplate.Stacks.Sum<PartyTemplateStack>((PartyTemplateStack s) => s.MaxValue);
			float num3;
			if (desiredSize < num)
			{
				num3 = (float)desiredSize / (float)num - 1f;
			}
			else if (num <= desiredSize && desiredSize <= num2)
			{
				num3 = (float)(desiredSize - num) / (float)(num2 - num);
			}
			else
			{
				num3 = (float)desiredSize / (float)num2;
			}
			return num3;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x000818D0 File Offset: 0x0007FAD0
		private float GetInitialPartySizeRatioForMobileParty(MobileParty party, PartyTemplateObject partyTemplate)
		{
			float num;
			if (party.IsBandit)
			{
				if (!partyTemplate.ShipHulls.IsEmpty<ShipTemplateStack>())
				{
					num = ((MBRandom.RandomFloat < 0.4f) ? MBRandom.RandomFloatRanged(0f, 0.33f) : MBRandom.RandomFloatRanged(0.66f, 1f));
				}
				else
				{
					float playerProgress = Campaign.Current.PlayerProgress;
					float num2 = 0.4f + 0.8f * playerProgress;
					float num3 = MBRandom.RandomFloatRanged(0.2f, 0.8f);
					num = num2 * num3;
				}
			}
			else if (party.IsCaravan && party.Owner == Hero.MainHero)
			{
				num = 1f;
			}
			else if (party.IsPatrolParty)
			{
				num = 1f;
			}
			else
			{
				num = party.RandomFloat();
			}
			return num;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0008198C File Offset: 0x0007FB8C
		public override int GetIdealVillagerPartySize(Village village)
		{
			float num = 0f;
			foreach (ValueTuple<ItemObject, float> valueTuple in village.VillageType.Productions)
			{
				float resultNumber = Campaign.Current.Models.VillageProductionCalculatorModel.CalculateDailyProductionAmount(village, valueTuple.Item1).ResultNumber;
				num += resultNumber;
			}
			float num2 = ((num > 10f) ? (40f * (1f - (MathF.Min(40f, num) - 10f) / 60f)) : 40f);
			return this.MinimumNumberOfVillagersAtVillagerParty + (int)(village.Hearth / num2);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00081A54 File Offset: 0x0007FC54
		public override TroopRoster FindAppropriateInitialRosterForMobileParty(MobileParty party, PartyTemplateObject partyTemplate)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			float initialPartySizeRatioForMobileParty = this.GetInitialPartySizeRatioForMobileParty(party, partyTemplate);
			for (int i = 0; i < partyTemplate.Stacks.Count; i++)
			{
				int minValue = partyTemplate.Stacks[i].MinValue;
				int maxValue = partyTemplate.Stacks[i].MaxValue;
				int num;
				if (initialPartySizeRatioForMobileParty <= 0f)
				{
					num = minValue;
				}
				else if (initialPartySizeRatioForMobileParty <= 1f)
				{
					num = MBRandom.RoundRandomized((float)minValue + (float)(maxValue - minValue) * initialPartySizeRatioForMobileParty);
				}
				else
				{
					Debug.FailedAssert("initialPartySizeRatio should not be above 1", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultPartySizeLimitModel.cs", "FindAppropriateInitialRosterForMobileParty", 494);
					num = maxValue;
				}
				if (party.IsVillager)
				{
					Village village = party.VillagerPartyComponent.Village;
					Settlement bound = village.Bound;
					bool flag;
					if (bound == null)
					{
						flag = null != null;
					}
					else
					{
						Town town = bound.Town;
						flag = ((town != null) ? town.Governor : null) != null;
					}
					if (flag && village.Bound.Town.Governor.GetPerkValue(DefaultPerks.Scouting.VillageNetwork))
					{
						num = MathF.Round((float)num * (1f + DefaultPerks.Scouting.VillageNetwork.SecondaryBonus));
					}
				}
				if (num > 0)
				{
					CharacterObject character = partyTemplate.Stacks[i].Character;
					troopRoster.AddToCounts(character, num, false, 0, 0, true, -1);
				}
			}
			return troopRoster;
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x00081B90 File Offset: 0x0007FD90
		public override List<Ship> FindAppropriateInitialShipsForMobileParty(MobileParty party, PartyTemplateObject partyTemplate)
		{
			List<Ship> list = new List<Ship>();
			float initialPartySizeRatioForMobileParty = this.GetInitialPartySizeRatioForMobileParty(party, partyTemplate);
			if (partyTemplate.ShipHulls != null && partyTemplate.ShipHulls.Count > 0)
			{
				foreach (ShipTemplateStack shipTemplateStack in partyTemplate.ShipHulls)
				{
					int minValue = shipTemplateStack.MinValue;
					int maxValue = shipTemplateStack.MaxValue;
					int num;
					if (initialPartySizeRatioForMobileParty <= 0f)
					{
						num = MBRandom.RoundRandomized(Math.Max(0f, (float)minValue + (float)minValue * initialPartySizeRatioForMobileParty));
					}
					else if (initialPartySizeRatioForMobileParty <= 1f)
					{
						num = MBRandom.RoundRandomized((float)minValue + (float)(maxValue - minValue) * initialPartySizeRatioForMobileParty);
					}
					else
					{
						num = MBRandom.RoundRandomized((float)maxValue * initialPartySizeRatioForMobileParty);
					}
					for (int i = 0; i < num; i++)
					{
						list.Add(new Ship(shipTemplateStack.ShipHull));
					}
				}
			}
			return list;
		}

		// Token: 0x0400086B RID: 2155
		private const int BaseMobilePartySize = 20;

		// Token: 0x0400086C RID: 2156
		private const int BaseMobilePartyPrisonerSize = 10;

		// Token: 0x0400086D RID: 2157
		private const int BaseSettlementPrisonerSize = 60;

		// Token: 0x0400086E RID: 2158
		private const int BaseTownPrisonerSizeBonus = 40;

		// Token: 0x0400086F RID: 2159
		private const int BaseGarrisonPartySize = 200;

		// Token: 0x04000870 RID: 2160
		private const int BasePatrolPartySize = 10;

		// Token: 0x04000871 RID: 2161
		private const int TownGarrisonSizeBonus = 200;

		// Token: 0x04000872 RID: 2162
		private const int AdditionalPartySizeForCheat = 5000;

		// Token: 0x04000873 RID: 2163
		private const int OneVillagerPerHearth = 40;

		// Token: 0x04000874 RID: 2164
		private const int AdditionalPartySizeLimitPerTier = 15;

		// Token: 0x04000875 RID: 2165
		private const int AdditionalPartySizeLimitForLeaderPerTier = 25;

		// Token: 0x04000876 RID: 2166
		private readonly TextObject _leadershipSkillLevelBonusText = GameTexts.FindText("str_leadership_skill_level_bonus", null);

		// Token: 0x04000877 RID: 2167
		private readonly TextObject _leadershipPerkUltimateLeaderBonusText = GameTexts.FindText("str_leadership_perk_bonus", null);

		// Token: 0x04000878 RID: 2168
		private readonly TextObject _wallLevelBonusText = GameTexts.FindText("str_map_tooltip_wall_level", null);

		// Token: 0x04000879 RID: 2169
		private readonly TextObject _baseSizeText = GameTexts.FindText("str_base_size", null);

		// Token: 0x0400087A RID: 2170
		private readonly TextObject _clanTierText = GameTexts.FindText("str_clan_tier_bonus", null);

		// Token: 0x0400087B RID: 2171
		private readonly TextObject _renownText = GameTexts.FindText("str_renown_bonus", null);

		// Token: 0x0400087C RID: 2172
		private readonly TextObject _clanLeaderText = GameTexts.FindText("str_clan_leader_bonus", null);

		// Token: 0x0400087D RID: 2173
		private readonly TextObject _factionLeaderText = GameTexts.FindText("str_faction_leader_bonus", null);

		// Token: 0x0400087E RID: 2174
		private readonly TextObject _leaderLevelText = GameTexts.FindText("str_leader_level_bonus", null);

		// Token: 0x0400087F RID: 2175
		private readonly TextObject _townBonusText = GameTexts.FindText("str_town_bonus", null);

		// Token: 0x04000880 RID: 2176
		private readonly TextObject _minorFactionText = GameTexts.FindText("str_minor_faction_bonus", null);

		// Token: 0x04000881 RID: 2177
		private readonly TextObject _currentPartySizeBonusText = GameTexts.FindText("str_current_party_size_bonus", null);

		// Token: 0x04000882 RID: 2178
		private readonly TextObject _randomSizeBonusTemporary = new TextObject("{=hynFV8jC}Extra size bonus (Perk-like Effect)", null);

		// Token: 0x04000883 RID: 2179
		private static bool _addAdditionalPartySizeAsCheat;

		// Token: 0x04000884 RID: 2180
		private static bool _addAdditionalPrisonerSizeAsCheat;

		// Token: 0x020005C6 RID: 1478
		private enum LimitType
		{
			// Token: 0x0400190A RID: 6410
			MobilePartySizeLimit,
			// Token: 0x0400190B RID: 6411
			GarrisonPartySizeLimit,
			// Token: 0x0400190C RID: 6412
			PrisonerSizeLimit
		}
	}
}
