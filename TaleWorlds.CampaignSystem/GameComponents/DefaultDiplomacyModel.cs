using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000117 RID: 279
	public class DefaultDiplomacyModel : DiplomacyModel
	{
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x0600180B RID: 6155 RVA: 0x00071B6E File Offset: 0x0006FD6E
		public override int MinimumRelationWithConversationCharacterToJoinKingdom
		{
			get
			{
				return -10;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600180C RID: 6156 RVA: 0x00071B72 File Offset: 0x0006FD72
		public override int GiftingTownRelationshipBonus
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x0600180D RID: 6157 RVA: 0x00071B76 File Offset: 0x0006FD76
		public override int GiftingCastleRelationshipBonus
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x0600180E RID: 6158 RVA: 0x00071B7A File Offset: 0x0006FD7A
		public override int MaxRelationLimit
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x0600180F RID: 6159 RVA: 0x00071B7E File Offset: 0x0006FD7E
		public override int MinRelationLimit
		{
			get
			{
				return -100;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x00071B82 File Offset: 0x0006FD82
		public override int MaxNeutralRelationLimit
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001811 RID: 6161 RVA: 0x00071B86 File Offset: 0x0006FD86
		public override int MinNeutralRelationLimit
		{
			get
			{
				return -50;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x00071B8A File Offset: 0x0006FD8A
		public override float WarDeclarationScorePenaltyAgainstTradePartners
		{
			get
			{
				return 0.7f;
			}
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00071B91 File Offset: 0x0006FD91
		public override float GetStrengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom(Kingdom kingdomToJoin)
		{
			return kingdomToJoin.CurrentTotalStrength * 0.05f;
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00071BA0 File Offset: 0x0006FDA0
		public override float GetClanStrength(Clan clan)
		{
			float num = 0f;
			foreach (Hero hero in clan.Heroes)
			{
				num += this.GetHeroCommandingStrengthForClan(hero);
			}
			float num2 = clan.Influence * 1.2f;
			float num3 = (float)clan.Settlements.Count * 4f;
			return num + num2 + num3;
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00071C24 File Offset: 0x0006FE24
		public override float GetHeroCommandingStrengthForClan(Hero hero)
		{
			if (!hero.IsAlive)
			{
				return 0f;
			}
			float num = (float)hero.GetSkillValue(DefaultSkills.Tactics) * 1f;
			float num2 = (float)hero.GetSkillValue(DefaultSkills.Steward) * 1f;
			float num3 = (float)hero.GetSkillValue(DefaultSkills.Trade) * 1f;
			float num4 = (float)hero.GetSkillValue(DefaultSkills.Leadership) * 1f;
			float num5 = (float)((hero.GetTraitLevel(DefaultTraits.Commander) > 0) ? 300 : 0);
			float num6 = (float)hero.Gold * 0.1f;
			float num7 = ((hero.PartyBelongedTo != null) ? (5f * hero.PartyBelongedTo.Party.CalculateCurrentStrength()) : 0f);
			float num8 = 0f;
			if (hero.Clan.Leader == hero)
			{
				num8 += 500f;
			}
			float num9 = 0f;
			if (hero.Father == hero.Clan.Leader || hero.Clan.Leader.Father == hero || hero.Mother == hero.Clan.Leader || hero.Clan.Leader.Mother == hero)
			{
				num9 += 100f;
			}
			float num10 = 0f;
			if (hero.IsNoncombatant)
			{
				num10 -= 250f;
			}
			float num11 = 0f;
			if (hero.GovernorOf != null)
			{
				num11 -= 250f;
			}
			float num12 = num5 + num + num2 + num3 + num4 + num6 + num7 + num8 + num9 + num10 + num11;
			if (num12 <= 0f)
			{
				return 0f;
			}
			return num12;
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x00071DB8 File Offset: 0x0006FFB8
		public override float GetHeroGoverningStrengthForClan(Hero hero)
		{
			if (hero.IsAlive)
			{
				float num = (float)hero.GetSkillValue(DefaultSkills.Tactics) * 0.3f;
				float num2 = (float)hero.GetSkillValue(DefaultSkills.Charm) * 0.9f;
				float num3 = (float)hero.GetSkillValue(DefaultSkills.Engineering) * 0.8f;
				float num4 = (float)hero.GetSkillValue(DefaultSkills.Steward) * 2f;
				float num5 = (float)hero.GetSkillValue(DefaultSkills.Trade) * 1.2f;
				float num6 = (float)hero.GetSkillValue(DefaultSkills.Leadership) * 1f;
				float num7 = (float)((hero.GetTraitLevel(DefaultTraits.Honor) > 0) ? 100 : 0);
				float num8 = (float)MathF.Min(100000, hero.Gold) * 0.005f;
				float num9 = 0f;
				if (hero.Spouse == hero.Clan.Leader)
				{
					num9 += 1000f;
				}
				if (hero.Father == hero.Clan.Leader || hero.Clan.Leader.Father == hero || hero.Mother == hero.Clan.Leader || hero.Clan.Leader.Mother == hero)
				{
					num9 += 750f;
				}
				if (hero.Siblings.Contains(hero.Clan.Leader))
				{
					num9 += 500f;
				}
				return num7 + num + num4 + num5 + num6 + num8 + num9 + num2 + num3;
			}
			return 0f;
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x00071F28 File Offset: 0x00070128
		public override int GetEffectiveRelationChange(Hero originalHero, Hero originalGainedRelationWith, int relationChange)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)relationChange, false, null);
			if (explainedNumber.ResultNumber > 0f)
			{
				Hero hero;
				if (originalHero.IsHumanPlayerCharacter || originalGainedRelationWith.IsHumanPlayerCharacter)
				{
					hero = (originalHero.IsHumanPlayerCharacter ? originalHero : originalGainedRelationWith);
				}
				else
				{
					hero = ((MBRandom.RandomFloat < 0.5f) ? originalHero : originalGainedRelationWith);
				}
				SkillHelper.AddSkillBonusForCharacter(DefaultSkillEffects.CharmRelationBonus, hero.CharacterObject, ref explainedNumber);
				if (originalHero.IsFemale != originalGainedRelationWith.IsFemale)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Charm.InBloom, BattleEnvironment.Any, hero.CharacterObject, true, ref explainedNumber);
				}
				else
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Charm.YoungAndRespectful, BattleEnvironment.Any, hero.CharacterObject, true, ref explainedNumber);
				}
				if (originalGainedRelationWith.GetTraitLevel(DefaultTraits.Mercy) > 0)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Charm.GoodNatured, BattleEnvironment.Any, hero.CharacterObject, false, ref explainedNumber);
				}
				if (originalGainedRelationWith.GetTraitLevel(DefaultTraits.Mercy) < 0)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Charm.Tribute, BattleEnvironment.Any, hero.CharacterObject, false, ref explainedNumber);
				}
				float traitEffectBonus = TraitEffectHelper.GetTraitEffectBonus(originalHero, DefaultPersonalityTraitEffects.HonorRelationGainEffect);
				float traitEffectBonus2 = TraitEffectHelper.GetTraitEffectBonus(originalGainedRelationWith, DefaultPersonalityTraitEffects.HonorRelationGainEffect);
				float num = ((traitEffectBonus > traitEffectBonus2) ? traitEffectBonus : traitEffectBonus2);
				if (num != 0f)
				{
					explainedNumber.AddFactor(num, new TextObject("{=ENta0wCu}Personality", null));
				}
				float num2 = (originalGainedRelationWith.IsClanLeader ? TraitEffectHelper.GetTraitEffectBonus(originalHero, DefaultPersonalityTraitEffects.HonorClanLeaderRelationEffect) : 0f);
				float num3 = (originalHero.IsClanLeader ? TraitEffectHelper.GetTraitEffectBonus(originalGainedRelationWith, DefaultPersonalityTraitEffects.HonorClanLeaderRelationEffect) : 0f);
				float num4 = ((num2 < num3) ? num2 : num3);
				if (num4 != 0f)
				{
					explainedNumber.AddFactor(num4, new TextObject("{=ENta0wCu}Personality", null));
				}
			}
			return explainedNumber.RoundedResultNumber;
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x000720BC File Offset: 0x000702BC
		public override int GetInfluenceAwardForSettlementCapturer(Settlement settlement)
		{
			int num3;
			if (settlement.IsTown || settlement.IsCastle)
			{
				int num = (settlement.IsTown ? 30 : 10);
				int num2 = 0;
				foreach (Village village in settlement.BoundVillages)
				{
					num2 += this.GetInfluenceAwardForSettlementCapturer(village.Settlement);
				}
				num3 = num + num2;
			}
			else
			{
				num3 = 10;
			}
			return num3;
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x00072144 File Offset: 0x00070344
		public override float GetHourlyInfluenceAwardForBeingArmyMember(MobileParty mobileParty)
		{
			float num = mobileParty.Party.CalculateCurrentStrength();
			float num2 = 0.0001f * (20f + num);
			if (mobileParty.BesiegedSettlement != null || mobileParty.MapEvent != null)
			{
				num2 *= 2f;
			}
			return num2;
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x00072184 File Offset: 0x00070384
		public override float GetHourlyInfluenceAwardForRaidingEnemyVillage(MobileParty mobileParty)
		{
			int num = 0;
			foreach (MapEventParty mapEventParty in mobileParty.MapEvent.AttackerSide.Parties)
			{
				if (mapEventParty.Party.MobileParty != mobileParty)
				{
					MobileParty mobileParty2 = mapEventParty.Party.MobileParty;
					if (((mobileParty2 != null) ? mobileParty2.Army : null) == null || mapEventParty.Party.MobileParty.Army.LeaderParty != mobileParty)
					{
						continue;
					}
				}
				num += mapEventParty.Party.MemberRoster.TotalManCount;
			}
			return (MathF.Sqrt((float)num) + 2f) / 240f;
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x00072244 File Offset: 0x00070444
		public override float GetHourlyInfluenceAwardForBesiegingEnemyFortification(MobileParty mobileParty)
		{
			int num = 0;
			foreach (PartyBase partyBase in mobileParty.BesiegedSettlement.SiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege))
			{
				if (partyBase.MobileParty == mobileParty || (partyBase.MobileParty.Army != null && partyBase.MobileParty.Army.LeaderParty == mobileParty))
				{
					num += partyBase.MemberRoster.TotalManCount;
				}
			}
			return (MathF.Sqrt((float)num) + 2f) / 240f;
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x000722E8 File Offset: 0x000704E8
		public override float GetScoreOfClanToJoinKingdom(Clan clan, Kingdom kingdom)
		{
			if (clan.Kingdom != null && clan.Kingdom.RulingClan == clan)
			{
				return -100000000f;
			}
			int relationBetweenClans = FactionManager.GetRelationBetweenClans(kingdom.RulingClan, clan);
			int num = 0;
			int num2 = 0;
			foreach (Clan clan2 in kingdom.Clans)
			{
				int relationBetweenClans2 = FactionManager.GetRelationBetweenClans(clan, clan2);
				num += relationBetweenClans2;
				num2++;
			}
			float num3 = ((num2 > 0) ? ((float)num / (float)num2) : 0f);
			float num4 = MathF.Max(-100f, MathF.Min(100f, (float)relationBetweenClans + num3));
			float num5 = MathF.Min(2f, MathF.Max(0.33f, 1f + MathF.Sqrt(MathF.Abs(num4)) * ((num4 < 0f) ? (-0.067f) : 0.1f)));
			float num6 = 1f;
			if (kingdom.Culture == clan.Culture)
			{
				num6 += 0.15f;
			}
			else if (kingdom.Leader != Hero.MainHero)
			{
				num6 -= 0.15f;
			}
			float num7 = clan.CalculateTotalSettlementBaseValue();
			float num8 = clan.CalculateTotalSettlementValueForFaction(kingdom);
			int warPartyLimit = clan.WarPartyLimit;
			float num9 = 0f;
			float num10 = 0f;
			if (!clan.IsMinorFaction)
			{
				float num11 = 0f;
				foreach (Town town in kingdom.Fiefs)
				{
					num11 += town.Settlement.GetSettlementValueForFaction(kingdom);
				}
				int num12 = 0;
				foreach (Clan clan3 in kingdom.Clans)
				{
					if (!clan3.IsUnderMercenaryService && clan3 != clan)
					{
						num12 += clan3.WarPartyLimit;
					}
				}
				num9 = num11 / (float)(num12 + warPartyLimit);
				num10 = -((float)(num12 * num12) * 100f) + 10000f;
			}
			float num13 = num9 * MathF.Sqrt((float)warPartyLimit) * 0.15f * 0.2f;
			num13 *= num5 * num6;
			num13 += (clan.MapFaction.IsAtWarWith(kingdom) ? (num8 - num7) : 0f);
			num13 += num10;
			if (clan.Kingdom != null && clan.Kingdom.Leader == Hero.MainHero && num13 > 0f)
			{
				num13 *= 0.2f;
			}
			return num13;
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x00072594 File Offset: 0x00070794
		public override float GetScoreOfClanToLeaveKingdom(Clan clan, Kingdom kingdom)
		{
			int relationBetweenClans = FactionManager.GetRelationBetweenClans(kingdom.RulingClan, clan);
			int num = 0;
			int num2 = 0;
			foreach (Clan clan2 in kingdom.Clans)
			{
				int relationBetweenClans2 = FactionManager.GetRelationBetweenClans(clan, clan2);
				num += relationBetweenClans2;
				num2++;
			}
			float num3 = ((num2 > 0) ? ((float)num / (float)num2) : 0f);
			float num4 = MathF.Max(-100f, MathF.Min(100f, (float)relationBetweenClans + num3));
			float num5 = MathF.Min(2f, MathF.Max(0.33f, 1f + MathF.Sqrt(MathF.Abs(num4)) * ((num4 < 0f) ? (-0.067f) : 0.1f)));
			float num6 = 1f + ((kingdom.Culture == clan.Culture) ? 0.15f : ((kingdom.Leader == Hero.MainHero) ? 0f : (-0.15f)));
			float num7 = clan.CalculateTotalSettlementBaseValue();
			float num8 = clan.CalculateTotalSettlementValueForFaction(kingdom);
			int warPartyLimit = clan.WarPartyLimit;
			float num9 = 0f;
			if (!clan.IsMinorFaction)
			{
				float num10 = 0f;
				foreach (Town town in kingdom.Fiefs)
				{
					num10 += town.Settlement.GetSettlementValueForFaction(kingdom);
				}
				int num11 = 0;
				foreach (Clan clan3 in kingdom.Clans)
				{
					if (!clan3.IsUnderMercenaryService && clan3 != clan)
					{
						num11 += clan3.WarPartyLimit;
					}
				}
				num9 = num10 / (float)(num11 + warPartyLimit);
			}
			float num12 = HeroHelper.CalculateReliabilityConstant(clan.Leader, 1f);
			float num13 = (float)(CampaignTime.Now - clan.LastFactionChangeTime).ToDays;
			float num14 = 4000f * (15f - MathF.Sqrt(MathF.Min(225f, num13)));
			int num15 = 0;
			int num16 = 0;
			using (List<Town>.Enumerator enumerator2 = clan.Fiefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.IsCastle)
					{
						num16++;
					}
					else
					{
						num15++;
					}
				}
			}
			float num17 = -70000f - (float)num16 * 10000f - (float)num15 * 30000f;
			num17 /= 0.15f;
			float num18 = -num9 * MathF.Sqrt((float)warPartyLimit) * 0.15f * 0.2f + num17 * num12 + -num14;
			num18 *= num5 * num6;
			if (num5 < 1f && num7 - num8 < 0f)
			{
				num18 += num5 * (num7 - num8);
			}
			else
			{
				num18 += num7 - num8;
			}
			if (num5 < 1f)
			{
				num18 += (1f - num5) * 200000f;
			}
			if (kingdom.Leader == Hero.MainHero)
			{
				if (num18 > 0f)
				{
					num18 *= 0.2f;
				}
				else
				{
					num18 *= 5f;
				}
			}
			return num18 + ((kingdom.Leader == Hero.MainHero) ? (-(1000000f * num5)) : 0f);
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0007291C File Offset: 0x00070B1C
		public override float GetScoreOfKingdomToGetClan(Kingdom kingdom, Clan clan)
		{
			float num = MathF.Min(2f, MathF.Max(0.33f, 1f + 0.02f * (float)FactionManager.GetRelationBetweenClans(kingdom.RulingClan, clan)));
			float num2 = 1f + ((kingdom.Culture == clan.Culture) ? 1f : 0f);
			int warPartyLimit = clan.WarPartyLimit;
			float num3 = (clan.CurrentTotalStrength + 150f * (float)warPartyLimit) * 20f;
			float powerRatioToEnemies = FactionHelper.GetPowerRatioToEnemies(kingdom);
			float num4 = HeroHelper.CalculateReliabilityConstant(clan.Leader, 1f);
			float num5 = 1f / MathF.Max(0.4f, MathF.Min(2.5f, MathF.Sqrt(powerRatioToEnemies)));
			num3 *= num5;
			return (clan.CalculateTotalSettlementValueForFaction(kingdom) * 0.1f + num3) * num * num2 * num4;
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x000729F0 File Offset: 0x00070BF0
		public override float GetScoreOfKingdomToSackClan(Kingdom kingdom, Clan clan)
		{
			float num = MathF.Min(2f, MathF.Max(0.33f, 1f + 0.02f * (float)FactionManager.GetRelationBetweenClans(kingdom.RulingClan, clan)));
			float num2 = 1f + ((kingdom.Culture == clan.Culture) ? 1f : 0.5f);
			int warPartyLimit = clan.WarPartyLimit;
			float num3 = (clan.CurrentTotalStrength + 150f * (float)warPartyLimit) * 20f;
			float num4 = clan.CalculateTotalSettlementValueForFaction(kingdom);
			return 10f - 1f * num3 * num2 * num - num4;
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x00072A88 File Offset: 0x00070C88
		public override float GetScoreOfMercenaryToJoinKingdom(Clan mercenaryClan, Kingdom kingdom)
		{
			int num = ((mercenaryClan.Kingdom == kingdom) ? mercenaryClan.MercenaryAwardMultiplier : Campaign.Current.Models.MinorFactionsModel.GetMercenaryAwardFactorToJoinKingdom(mercenaryClan, kingdom, false));
			float num2 = mercenaryClan.CurrentTotalStrength + (float)mercenaryClan.WarPartyLimit * 50f;
			int mercenaryAwardFactorToJoinKingdom = Campaign.Current.Models.MinorFactionsModel.GetMercenaryAwardFactorToJoinKingdom(mercenaryClan, kingdom, true);
			if (kingdom.Leader == Hero.MainHero)
			{
				return 0f;
			}
			return (float)(num - mercenaryAwardFactorToJoinKingdom) * num2 * 0.5f;
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x00072B0C File Offset: 0x00070D0C
		public override float GetScoreOfMercenaryToLeaveKingdom(Clan mercenaryClan, Kingdom kingdom)
		{
			float num = 0.005f * MathF.Min(200f, mercenaryClan.LastFactionChangeTime.ElapsedDaysUntilNow);
			return 10000f * num - 5000f - this.GetScoreOfMercenaryToJoinKingdom(mercenaryClan, kingdom);
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x00072B50 File Offset: 0x00070D50
		public override float GetScoreOfKingdomToHireMercenary(Kingdom kingdom, Clan mercenaryClan)
		{
			int num = 0;
			foreach (Clan clan in kingdom.Clans)
			{
				num += clan.WarPartyLimit;
			}
			float num2 = (float)((num < 12) ? ((12 - num) * 100) : 0);
			int count = kingdom.Settlements.Count;
			int num3 = ((count < 40) ? ((40 - count) * 30) : 0);
			return num2 + (float)num3;
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x00072BD8 File Offset: 0x00070DD8
		public override float GetScoreOfKingdomToSackMercenary(Kingdom kingdom, Clan mercenaryClan)
		{
			float num = (((float)kingdom.Leader.Gold > 20000f) ? (MathF.Sqrt((float)kingdom.Leader.Gold / 20000f) - 1f) : (-1f));
			int relationBetweenClans = FactionManager.GetRelationBetweenClans(kingdom.RulingClan, mercenaryClan);
			float num2 = MathF.Min(5f, FactionHelper.GetPowerRatioToEnemies(kingdom));
			return (MathF.Min(2f + (float)relationBetweenClans / 100f - num2, num) * -1f - 0.1f) * 50f * mercenaryClan.CurrentTotalStrength * 5f;
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x00072C70 File Offset: 0x00070E70
		public override float GetScoreOfDeclaringPeaceForClan(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace, Clan evaluatingClan, out TextObject reason, bool includeReason = false)
		{
			reason = null;
			if (includeReason)
			{
				reason = this.GetReasonForDeclaringPeace(factionDeclaresPeace, factionDeclaredPeace, evaluatingClan);
			}
			float num = DefaultDiplomacyModel.GetExposureScoreToOtherFaction(factionDeclaresPeace, factionDeclaredPeace);
			if (num.ApproximatelyEqualsTo(-3.4028235E+38f, 1E-05f))
			{
				return 10000000f;
			}
			num = MathF.Min(num * 1.4f, DefaultDiplomacyModel.GetExposureScoreToOtherFaction(factionDeclaredPeace, factionDeclaresPeace));
			float num2;
			float num3;
			DefaultDiplomacyModel.GetBenefitAndRiskScoreForPeace(factionDeclaresPeace, factionDeclaredPeace, evaluatingClan, out num2, out num3);
			DefaultDiplomacyModel.UpdateOurBenefitMinusOurRiskBasedOnEvaluatingFaction(evaluatingClan, ref num2, ref num3);
			num3 = DefaultDiplomacyModel.ApplyWarProgressToRiskScore(factionDeclaresPeace, factionDeclaredPeace, num3);
			num2 *= DefaultDiplomacyModel.GetWarScale(factionDeclaresPeace, factionDeclaredPeace);
			float relationScore = DefaultDiplomacyModel.GetRelationScore(factionDeclaresPeace, factionDeclaredPeace, evaluatingClan);
			float sameCultureTownScore = DefaultDiplomacyModel.GetSameCultureTownScore(factionDeclaresPeace, factionDeclaredPeace);
			float allianceFactorForDeclaringPeace = Campaign.Current.Models.AllianceModel.GetAllianceFactorForDeclaringPeace(factionDeclaresPeace, factionDeclaredPeace);
			return (sameCultureTownScore + num2 * num - num3 * allianceFactorForDeclaringPeace + relationScore) * -1f;
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x00072D28 File Offset: 0x00070F28
		public override float GetScoreOfDeclaringPeace(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace)
		{
			float num = DefaultDiplomacyModel.GetExposureScoreToOtherFaction(factionDeclaresPeace, factionDeclaredPeace);
			if (num.ApproximatelyEqualsTo(-3.4028235E+38f, 1E-05f))
			{
				return 10000000f;
			}
			num = MathF.Min(num * 1.4f, DefaultDiplomacyModel.GetExposureScoreToOtherFaction(factionDeclaredPeace, factionDeclaresPeace));
			float num2;
			float num3;
			DefaultDiplomacyModel.GetBenefitAndRiskScoreForPeace(factionDeclaresPeace, factionDeclaredPeace, factionDeclaresPeace.Leader.Clan, out num2, out num3);
			num3 = DefaultDiplomacyModel.ApplyWarProgressToRiskScore(factionDeclaresPeace, factionDeclaredPeace, num3);
			num2 *= DefaultDiplomacyModel.GetWarScale(factionDeclaresPeace, factionDeclaredPeace);
			float allianceFactorForDeclaringPeace = Campaign.Current.Models.AllianceModel.GetAllianceFactorForDeclaringPeace(factionDeclaresPeace, factionDeclaredPeace);
			return (DefaultDiplomacyModel.GetSameCultureTownScore(factionDeclaresPeace, factionDeclaredPeace) + num2 * num - num3 * allianceFactorForDeclaringPeace) * -1f;
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x00072DC0 File Offset: 0x00070FC0
		private TextObject GetReasonForDeclaringPeace(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace, Clan evaluatingClan)
		{
			if (DefaultDiplomacyModel.GetExposureScoreToOtherFaction(factionDeclaresPeace, factionDeclaredPeace).ApproximatelyEqualsTo(-3.4028235E+38f, 1E-05f))
			{
				return new TextObject("{=i0h0LKa0}Our borders are far from those of the enemy. It is too arduous to pursue this war.", null);
			}
			DefaultDiplomacyModel.WarStats warStats = DefaultDiplomacyModel.CalculateWarStatsForPeace(factionDeclaresPeace, factionDeclaredPeace, evaluatingClan);
			DefaultDiplomacyModel.WarStats warStats2 = DefaultDiplomacyModel.CalculateWarStatsForPeace(factionDeclaredPeace, factionDeclaresPeace, evaluatingClan);
			float num;
			float num2;
			DefaultDiplomacyModel.GetBenefitAndRiskScoreForPeace(factionDeclaresPeace, factionDeclaredPeace, evaluatingClan, out num, out num2);
			float num3 = DefaultDiplomacyModel.ApplyWarProgressToRiskScore(factionDeclaresPeace, factionDeclaredPeace, num2);
			TextObject textObject;
			if (num - num2 > 0f)
			{
				if (num - num3 < 0f)
				{
					textObject = new TextObject("{=QQtJobYP}We need time to recover from the hardships of war.", null);
				}
				else
				{
					textObject = new TextObject("{=vwjs6EjJ}On balance, the gains we stand to make are not worth the costs and risks.", null);
				}
			}
			else if (warStats.Strength < warStats2.Strength)
			{
				textObject = new TextObject("{=JOe3BC41}The {ENEMY_KINGDOM_INFORMAL_NAME} is currently more powerful than us. We need time to build up our strength.", null);
			}
			else if (warStats.Strength > warStats2.Strength && warStats.Strength < warStats2.Strength + warStats.TotalStrengthOfEnemies)
			{
				textObject = new TextObject("{=vwjs6EjJ}On balance, the gains we stand to make are not worth the costs and risks.", null);
			}
			else if (warStats.Strength < warStats2.Strength + warStats.TotalStrengthOfEnemies)
			{
				textObject = new TextObject("{=nuqv4GAA}We have too many enemies. We need to make peace with at least some of them.", null);
			}
			else
			{
				textObject = new TextObject("{=HqJSNG3M}Our realm is currently doing well, but we stand to lose this wealth if we go on fighting.", null);
			}
			if (!TextObject.IsNullOrEmpty(textObject))
			{
				textObject.SetTextVariable("ENEMY_KINGDOM_INFORMAL_NAME", factionDeclaredPeace.InformalName);
			}
			return textObject;
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x00072EF0 File Offset: 0x000710F0
		public override ExplainedNumber GetWarProgressScore(IFaction factionDeclaresWar, IFaction factionDeclaredWar, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			StanceLink stanceWith = factionDeclaresWar.GetStanceWith(factionDeclaredWar);
			if (!stanceWith.IsAtWar)
			{
				return explainedNumber;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (Town town in factionDeclaredWar.Fiefs)
			{
				if (town.IsTown)
				{
					num++;
				}
				else if (town.IsCastle)
				{
					num2++;
				}
				num3 += town.Villages.Count;
			}
			int num4 = factionDeclaredWar.WarPartyComponents.Sum<WarPartyComponent>((WarPartyComponent x) => x.Party.NumberOfAllMembers);
			int num5 = factionDeclaredWar.Fiefs.Sum<Town>(delegate(Town x)
			{
				MobileParty garrisonParty = x.GarrisonParty;
				if (garrisonParty == null)
				{
					return 0;
				}
				return garrisonParty.Party.NumberOfAllMembers;
			}) + num4;
			int casualties = stanceWith.GetCasualties(factionDeclaredWar);
			int successfulTownSieges = stanceWith.GetSuccessfulTownSieges(factionDeclaresWar);
			int num6 = stanceWith.GetSuccessfulSieges(factionDeclaresWar) - successfulTownSieges;
			int successfulRaids = stanceWith.GetSuccessfulRaids(factionDeclaresWar);
			int casualties2 = stanceWith.GetCasualties(factionDeclaresWar);
			int successfulTownSieges2 = stanceWith.GetSuccessfulTownSieges(factionDeclaredWar);
			int num7 = stanceWith.GetSuccessfulSieges(factionDeclaredWar) - successfulTownSieges2;
			int successfulRaids2 = stanceWith.GetSuccessfulRaids(factionDeclaredWar);
			float num8 = Math.Max(0f, (float)(casualties - casualties2) / (float)Math.Max(1, num5 * 4) * 500f);
			float num9 = Math.Max(0f, (float)(successfulTownSieges - successfulTownSieges2) / (float)Math.Max(1, num + successfulTownSieges - successfulTownSieges2) * 1000f);
			float num10 = Math.Max(0f, (float)(num6 - num7) / (float)Math.Max(1, num2 + num6 - num7) * 500f);
			float num11 = Math.Max(0f, (float)(successfulRaids - successfulRaids2) / (float)Math.Max(1, num3) * 250f);
			explainedNumber.Add(num8, new TextObject("{=FKe05WtJ}Kills", null), null);
			explainedNumber.Add(num9, new TextObject("{=bVa5jNbd}Town Sieges", null), null);
			explainedNumber.Add(num10, new TextObject("{=Sdu2FmgY}Castle Sieges", null), null);
			explainedNumber.Add(num11, new TextObject("{=w6E2lb09}Raids", null), null);
			explainedNumber.LimitMin(0f);
			explainedNumber.LimitMax(750f, null);
			return explainedNumber;
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x00073140 File Offset: 0x00071340
		private static float GetWarScale(IFaction factionDeclaresWar, IFaction factionDeclaredWar)
		{
			StanceLink stanceWith = factionDeclaresWar.GetStanceWith(factionDeclaredWar);
			if (!stanceWith.IsAtWar)
			{
				return 1f;
			}
			int casualties = stanceWith.GetCasualties(factionDeclaredWar);
			int casualties2 = stanceWith.GetCasualties(factionDeclaresWar);
			int num = MathF.Max(1, (int)stanceWith.WarStartDate.ElapsedDaysUntilNow);
			if (num <= 20)
			{
				return 1f;
			}
			float num2 = (float)MathF.Max(casualties + casualties2, 1) / (20f * MathF.Pow((float)num, 1.5f));
			if (num2 >= 1f || num2 <= 0f)
			{
				return 1f;
			}
			return num2;
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x000731D0 File Offset: 0x000713D0
		public override float GetScoreOfDeclaringWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar, Clan evaluatingClan, out TextObject reason, bool includeReason = false)
		{
			reason = null;
			if (includeReason)
			{
				reason = this.GetReasonForDeclaringWar(factionDeclaresWar, factionDeclaredWar, evaluatingClan);
			}
			if (factionDeclaresWar.CurrentTotalStrength <= 500f || factionDeclaresWar.WarPartyComponents.Count < 2)
			{
				return -10000000f;
			}
			float exposureScoreToOtherFaction = DefaultDiplomacyModel.GetExposureScoreToOtherFaction(factionDeclaresWar, factionDeclaredWar);
			if (exposureScoreToOtherFaction.ApproximatelyEqualsTo(-3.4028235E+38f, 1E-05f))
			{
				return -10000000f;
			}
			float num;
			float num2;
			DefaultDiplomacyModel.GetBenefitAndRiskScoreForWar(factionDeclaresWar, factionDeclaredWar, evaluatingClan, out num, out num2);
			float relationScore = DefaultDiplomacyModel.GetRelationScore(factionDeclaresWar, factionDeclaredWar, evaluatingClan);
			float sameCultureTownScore = DefaultDiplomacyModel.GetSameCultureTownScore(factionDeclaresWar, factionDeclaredWar);
			float allianceFactorForDeclaringWar = Campaign.Current.Models.AllianceModel.GetAllianceFactorForDeclaringWar(factionDeclaresWar, factionDeclaredWar);
			float tradeAgreementFactor = DefaultDiplomacyModel.GetTradeAgreementFactor(factionDeclaresWar, factionDeclaredWar);
			return sameCultureTownScore + num * exposureScoreToOtherFaction * allianceFactorForDeclaringWar * tradeAgreementFactor - num2 + relationScore;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x00073280 File Offset: 0x00071480
		private static float GetTradeAgreementFactor(IFaction factionDeclaresWar, IFaction factionDeclaredWar)
		{
			float num = 1f;
			if (factionDeclaresWar.IsKingdomFaction && factionDeclaredWar.IsKingdomFaction)
			{
				Kingdom kingdom = (Kingdom)factionDeclaresWar;
				Kingdom kingdom2 = (Kingdom)factionDeclaredWar;
				if (!kingdom2.IsAllyWith(kingdom))
				{
					ITradeAgreementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
					TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement;
					if (campaignBehavior != null && campaignBehavior.HasTradeAgreement(kingdom, kingdom2, out tradeAgreement))
					{
						num *= Campaign.Current.Models.DiplomacyModel.WarDeclarationScorePenaltyAgainstTradePartners;
					}
				}
			}
			return num;
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x000732F0 File Offset: 0x000714F0
		private TextObject GetReasonForDeclaringWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar, Clan evaluatingClan)
		{
			DefaultDiplomacyModel.WarStats warStats = DefaultDiplomacyModel.CalculateWarStatsForWar(factionDeclaresWar, factionDeclaredWar, evaluatingClan);
			DefaultDiplomacyModel.WarStats warStats2 = DefaultDiplomacyModel.CalculateWarStatsForWar(factionDeclaredWar, factionDeclaresWar, evaluatingClan);
			float num;
			float num2;
			DefaultDiplomacyModel.GetBenefitAndRiskScoreForWar(factionDeclaresWar, factionDeclaredWar, evaluatingClan, out num, out num2);
			float relationScore = DefaultDiplomacyModel.GetRelationScore(factionDeclaresWar, factionDeclaredWar, evaluatingClan);
			float sameCultureTownScore = DefaultDiplomacyModel.GetSameCultureTownScore(factionDeclaresWar, factionDeclaredWar);
			if (factionDeclaresWar.CurrentTotalStrength <= 500f || factionDeclaresWar.WarPartyComponents.Count < 2)
			{
				return new TextObject("{=JOe3BC41}The {ENEMY_KINGDOM_INFORMAL_NAME} is currently more powerful than us. We need time to build up our strength.", null);
			}
			if (DefaultDiplomacyModel.GetExposureScoreToOtherFaction(factionDeclaresWar, factionDeclaredWar).ApproximatelyEqualsTo(-3.4028235E+38f, 1E-05f))
			{
				return new TextObject("{=i0h0LKa0}Our borders are far from those of the enemy. It is too arduous to pursue this war.", null);
			}
			TextObject textObject;
			if (num - num2 > 0f)
			{
				if (relationScore > num - num2 && relationScore > sameCultureTownScore)
				{
					textObject = new TextObject("{=dov3iRlt}{ENEMY_RULER.NAME} of the {ENEMY_KINGDOM_INFORMAL_NAME} is vile and dangerous. We must deal with {?ENEMY_RULER.GENDER}her{?}him{\\?} before it is too late.", null);
				}
				else if (sameCultureTownScore > num - num2)
				{
					textObject = new TextObject("{=79lEPn1u}The {ENEMY_KINGDOM_INFORMAL_NAME} have occupied our ancestral lands and they oppress our kinfolk.", null);
				}
				else if (warStats.Strength > warStats2.Strength)
				{
					textObject = new TextObject("{=az3K3j4C}Right now we are stronger than the {ENEMY_KINGDOM_INFORMAL_NAME}. We should strike while we can.", null);
				}
				else
				{
					textObject = new TextObject("{=1aQAmENB}The {ENEMY_KINGDOM_INFORMAL_NAME} may be strong, but their lands are rich and ripe for the taking.", null);
				}
			}
			else if (relationScore > sameCultureTownScore)
			{
				textObject = new TextObject("{=dov3iRlt}{ENEMY_RULER.NAME} of the {ENEMY_KINGDOM_INFORMAL_NAME} is vile and dangerous. We must deal with {?ENEMY_RULER.GENDER}her{?}him{\\?} before it is too late.", null);
			}
			else
			{
				textObject = new TextObject("{=79lEPn1u}The {ENEMY_KINGDOM_INFORMAL_NAME} have occupied our ancestral lands and they oppress our kinfolk.", null);
			}
			if (!TextObject.IsNullOrEmpty(textObject))
			{
				textObject.SetTextVariable("ENEMY_KINGDOM_INFORMAL_NAME", factionDeclaredWar.InformalName);
				textObject.SetCharacterProperties("ENEMY_RULER", factionDeclaredWar.Leader.CharacterObject, false);
			}
			return textObject;
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0007343C File Offset: 0x0007163C
		private static float ApplyWarProgressToRiskScore(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace, float riskScore)
		{
			float resultNumber = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(factionDeclaresPeace, factionDeclaredPeace, false).ResultNumber;
			float resultNumber2 = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(factionDeclaredPeace, factionDeclaresPeace, false).ResultNumber;
			float num = MathF.Abs(resultNumber2 - resultNumber);
			if (num < 75f)
			{
				riskScore *= MBMath.Map(num, 0f, 75f, 0.5f, 1f);
			}
			else if (resultNumber2 > resultNumber)
			{
				float num2 = (resultNumber2 - resultNumber + 650f) / 650f;
				riskScore *= num2;
			}
			return riskScore;
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x000734D8 File Offset: 0x000716D8
		private static void GetBenefitAndRiskScoreForPeace(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace, IFaction evaluatingFaction, out float benefitScore, out float riskScore)
		{
			DefaultDiplomacyModel.WarStats warStats = DefaultDiplomacyModel.CalculateWarStatsForPeace(factionDeclaresPeace, factionDeclaredPeace, evaluatingFaction);
			DefaultDiplomacyModel.WarStats warStats2 = DefaultDiplomacyModel.CalculateWarStatsForPeace(factionDeclaredPeace, factionDeclaresPeace, evaluatingFaction);
			benefitScore = DefaultDiplomacyModel.CalculateBenefitScore(warStats, warStats2);
			riskScore = DefaultDiplomacyModel.CalculateRiskScore(warStats, warStats2);
			riskScore = MathF.Min(warStats2.ValueOfSettlements * 0.75f, riskScore);
			benefitScore = MathF.Min(warStats.ValueOfSettlements * 1.5f, benefitScore);
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x00073538 File Offset: 0x00071738
		private static void GetBenefitAndRiskScoreForWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar, IFaction evaluatingFaction, out float benefitScore, out float riskScore)
		{
			DefaultDiplomacyModel.WarStats warStats = DefaultDiplomacyModel.CalculateWarStatsForWar(factionDeclaresWar, factionDeclaredWar, evaluatingFaction);
			DefaultDiplomacyModel.WarStats warStats2 = DefaultDiplomacyModel.CalculateWarStatsForWar(factionDeclaredWar, factionDeclaresWar, evaluatingFaction);
			benefitScore = DefaultDiplomacyModel.CalculateBenefitScore(warStats, warStats2);
			riskScore = DefaultDiplomacyModel.CalculateRiskScore(warStats, warStats2);
			DefaultDiplomacyModel.ApplyTributeEffectToBenefitScoreForWar(factionDeclaresWar, factionDeclaredWar, evaluatingFaction, ref benefitScore);
			DefaultDiplomacyModel.UpdateOurBenefitMinusOurRiskBasedOnEvaluatingFaction(evaluatingFaction, ref benefitScore, ref riskScore);
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0007357C File Offset: 0x0007177C
		private static void ApplyTributeEffectToBenefitScoreForWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar, IFaction evaluatingFaction, ref float benefitScore)
		{
			StanceLink stanceWith = factionDeclaresWar.GetStanceWith(factionDeclaredWar);
			if (stanceWith.GetRemainingTributePaymentCount() == 0)
			{
				return;
			}
			int dailyTributeToPay = stanceWith.GetDailyTributeToPay(factionDeclaresWar);
			int dailyTributeToPay2 = stanceWith.GetDailyTributeToPay(factionDeclaredWar);
			if (dailyTributeToPay == 0 && dailyTributeToPay2 == 0)
			{
				return;
			}
			bool flag = stanceWith.GetDailyTributeToPay(evaluatingFaction.MapFaction) > 0 && evaluatingFaction.MapFaction == factionDeclaresWar;
			if (dailyTributeToPay > 0)
			{
				float num = factionDeclaresWar.Fiefs.Sum<Town>((Town x) => x.Prosperity) + 1f;
				float num2 = 1f + (float)dailyTributeToPay / num;
				benefitScore = (flag ? (benefitScore * num2) : (benefitScore / num2));
				return;
			}
			if (dailyTributeToPay2 > 0)
			{
				float num3 = factionDeclaredWar.Fiefs.Sum<Town>((Town x) => x.Prosperity) + 1f;
				float num4 = 1f + (float)dailyTributeToPay2 / num3;
				benefitScore = (flag ? (benefitScore * num4) : (benefitScore / num4));
			}
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x00073678 File Offset: 0x00071878
		public override float GetScoreOfLettingPartyGo(MobileParty party, MobileParty partyToLetGo)
		{
			float num = 0f;
			for (int i = 0; i < partyToLetGo.ItemRoster.Count; i++)
			{
				ItemRosterElement elementCopyAtIndex = partyToLetGo.ItemRoster.GetElementCopyAtIndex(i);
				num += (float)(elementCopyAtIndex.Amount * elementCopyAtIndex.EquipmentElement.GetBaseValue());
			}
			float num2 = 0f;
			for (int j = 0; j < party.ItemRoster.Count; j++)
			{
				ItemRosterElement elementCopyAtIndex2 = party.ItemRoster.GetElementCopyAtIndex(j);
				num2 += (float)(elementCopyAtIndex2.Amount * elementCopyAtIndex2.EquipmentElement.GetBaseValue());
			}
			float num3 = 0f;
			foreach (TroopRosterElement troopRosterElement in party.MemberRoster.GetTroopRoster())
			{
				num3 += MathF.Min(1000f, 10f * (float)troopRosterElement.Character.Level * MathF.Sqrt((float)troopRosterElement.Character.Level));
			}
			float num4 = 0f;
			foreach (TroopRosterElement troopRosterElement2 in partyToLetGo.MemberRoster.GetTroopRoster())
			{
				num4 += MathF.Min(1000f, 10f * (float)troopRosterElement2.Character.Level * MathF.Sqrt((float)troopRosterElement2.Character.Level));
			}
			float num5 = 0f;
			foreach (TroopRosterElement troopRosterElement3 in partyToLetGo.MemberRoster.GetTroopRoster())
			{
				if (troopRosterElement3.Character.IsHero)
				{
					num5 += 500f;
				}
				num5 += (float)Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(troopRosterElement3.Character, partyToLetGo.LeaderHero) * 0.3f;
			}
			float num6 = (party.IsPartyTradeActive ? ((float)party.PartyTradeGold) : 0f);
			num6 += ((party.LeaderHero != null) ? ((float)party.PartyTradeGold * 0.15f) : 0f);
			float num7 = (partyToLetGo.IsPartyTradeActive ? ((float)partyToLetGo.PartyTradeGold) : 0f);
			num6 += ((partyToLetGo.LeaderHero != null) ? ((float)partyToLetGo.PartyTradeGold * 0.15f) : 0f);
			float num8 = num4 + 10000f;
			if (partyToLetGo.BesiegedSettlement != null)
			{
				num8 += 20000f;
			}
			return -1000f + 0.01999998f * num3 - 0.98f * num8 - 0.98f * num7 + 0.01999998f * num6 + 0.98f * num5 + (num2 * 0.01999998f - 0.98f * num);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x00073974 File Offset: 0x00071B74
		public override float GetValueOfHeroForFaction(Hero examinedHero, IFaction targetFaction, bool forMarriage = false)
		{
			return this.GetHeroCommandingStrengthForClan(examinedHero) * 10f;
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x00073983 File Offset: 0x00071B83
		public override int GetRelationCostOfExpellingClanFromKingdom()
		{
			return -20;
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x00073987 File Offset: 0x00071B87
		public override int GetInfluenceCostOfSupportingClan()
		{
			return 50;
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0007398C File Offset: 0x00071B8C
		public override int GetInfluenceCostOfExpellingClan(Clan proposingClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(200f, false, null);
			this.GetPerkEffectsOnKingdomDecisionInfluenceCost(proposingClan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x000739BC File Offset: 0x00071BBC
		public override int GetInfluenceCostOfProposingPeace(Clan proposingClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(100f, false, null);
			this.GetPerkEffectsOnKingdomDecisionInfluenceCost(proposingClan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x000739EC File Offset: 0x00071BEC
		public override int GetInfluenceCostOfProposingWar(Clan proposingClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(200f, false, null);
			if (proposingClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.WarTax) && proposingClan == proposingClan.Kingdom.RulingClan)
			{
				explainedNumber.AddFactor(1f, null);
			}
			this.GetPerkEffectsOnKingdomDecisionInfluenceCost(proposingClan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x00073A4E File Offset: 0x00071C4E
		public override int GetInfluenceValueOfSupportingClan()
		{
			return this.GetInfluenceCostOfSupportingClan() / 4;
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x00073A58 File Offset: 0x00071C58
		public override int GetRelationValueOfSupportingClan()
		{
			return 1;
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x00073A5C File Offset: 0x00071C5C
		public override int GetInfluenceCostOfAnnexation(Clan proposingClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(200f, false, null);
			if (proposingClan.Kingdom != null)
			{
				if (proposingClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.FeudalInheritance))
				{
					explainedNumber.AddFactor(1f, null);
				}
				if (proposingClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.PrecarialLandTenure) && proposingClan == proposingClan.Kingdom.RulingClan)
				{
					explainedNumber.AddFactor(-0.5f, null);
				}
			}
			this.GetPerkEffectsOnKingdomDecisionInfluenceCost(proposingClan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x00073AEA File Offset: 0x00071CEA
		public override int GetInfluenceCostOfChangingLeaderOfArmy()
		{
			return 30;
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00073AF0 File Offset: 0x00071CF0
		public override int GetInfluenceCostOfDisbandingArmy()
		{
			int num = 30;
			if (Clan.PlayerClan.Kingdom != null && Clan.PlayerClan == Clan.PlayerClan.Kingdom.RulingClan)
			{
				num /= 2;
			}
			return num;
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x00073B27 File Offset: 0x00071D27
		public override int GetRelationCostOfDisbandingArmy(bool isLeaderParty)
		{
			if (!isLeaderParty)
			{
				return -1;
			}
			return -4;
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x00073B30 File Offset: 0x00071D30
		public override int GetInfluenceCostOfPolicyProposalAndDisavowal(Clan proposerClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(100f, false, null);
			this.GetPerkEffectsOnKingdomDecisionInfluenceCost(proposerClan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x00073B60 File Offset: 0x00071D60
		public override int GetInfluenceCostOfAbandoningArmy()
		{
			return 2;
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x00073B63 File Offset: 0x00071D63
		private void GetPerkEffectsOnKingdomDecisionInfluenceCost(Clan proposingClan, ref ExplainedNumber cost)
		{
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Charm.Firebrand, BattleEnvironment.Any, proposingClan.Leader.CharacterObject, true, ref cost);
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x00073B7E File Offset: 0x00071D7E
		private int GetBaseRelationBetweenHeroes(Hero hero1, Hero hero2)
		{
			return CharacterRelationManager.GetHeroRelation(hero1, hero2);
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00073B87 File Offset: 0x00071D87
		public override int GetBaseRelation(Hero hero1, Hero hero2)
		{
			return this.GetBaseRelationBetweenHeroes(hero1, hero2);
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x00073B94 File Offset: 0x00071D94
		public override int GetEffectiveRelation(Hero hero1, Hero hero2)
		{
			Hero hero3;
			Hero hero4;
			this.GetHeroesForEffectiveRelation(hero1, hero2, out hero3, out hero4);
			if (hero3 == null || hero4 == null)
			{
				return 0;
			}
			int baseRelationBetweenHeroes = this.GetBaseRelationBetweenHeroes(hero3, hero4);
			this.GetPersonalityEffects(ref baseRelationBetweenHeroes, hero1, hero4);
			return MBMath.ClampInt(baseRelationBetweenHeroes, this.MinRelationLimit, this.MaxRelationLimit);
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x00073BDC File Offset: 0x00071DDC
		public override void GetHeroesForEffectiveRelation(Hero hero1, Hero hero2, out Hero effectiveHero1, out Hero effectiveHero2)
		{
			effectiveHero1 = ((hero1.Clan != null) ? hero1.Clan.Leader : hero1);
			effectiveHero2 = ((hero2.Clan != null) ? hero2.Clan.Leader : hero2);
			if (effectiveHero1 == effectiveHero2 || (hero1.IsPlayerCompanion && hero2.IsHumanPlayerCharacter) || (hero2.IsPlayerCompanion && hero1.IsHumanPlayerCharacter))
			{
				effectiveHero1 = hero1;
				effectiveHero2 = hero2;
			}
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x00073C48 File Offset: 0x00071E48
		public override int GetRelationChangeAfterClanLeaderIsDead(Hero deadLeader, Hero relationHero)
		{
			return (int)((float)CharacterRelationManager.GetHeroRelation(deadLeader, relationHero) * 0.7f);
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x00073C5C File Offset: 0x00071E5C
		public override int GetRelationChangeAfterVotingInSettlementOwnerPreliminaryDecision(Hero supporter, bool hasHeroVotedAgainstOwner)
		{
			int num;
			if (hasHeroVotedAgainstOwner)
			{
				num = -20;
				if (supporter.Culture.HasFeat(DefaultCulturalFeats.SturgianDecisionPenaltyFeat))
				{
					num += (int)((float)num * DefaultCulturalFeats.SturgianDecisionPenaltyFeat.EffectBonus);
				}
			}
			else
			{
				num = 5;
			}
			return num;
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x00073C97 File Offset: 0x00071E97
		private void GetPersonalityEffects(ref int effectiveRelation, Hero hero1, Hero effectiveHero2)
		{
			this.GetTraitEffect(ref effectiveRelation, hero1, effectiveHero2, DefaultTraits.Honor, 2);
			this.GetTraitEffect(ref effectiveRelation, hero1, effectiveHero2, DefaultTraits.Valor, 1);
			this.GetTraitEffect(ref effectiveRelation, hero1, effectiveHero2, DefaultTraits.Mercy, 1);
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x00073CC8 File Offset: 0x00071EC8
		private void GetTraitEffect(ref int effectiveRelation, Hero hero1, Hero effectiveHero2, TraitObject trait, int effectMagnitude)
		{
			int traitLevel = hero1.GetTraitLevel(trait);
			int traitLevel2 = effectiveHero2.GetTraitLevel(trait);
			int num = traitLevel * traitLevel2;
			if (num > 0)
			{
				effectiveRelation += effectMagnitude;
				return;
			}
			if (num < 0)
			{
				effectiveRelation -= effectMagnitude;
			}
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x00073D00 File Offset: 0x00071F00
		public override int GetCharmExperienceFromRelationGain(Hero hero, float relationChange, ChangeRelationAction.ChangeRelationDetail detail)
		{
			float num = 20f;
			if (detail != ChangeRelationAction.ChangeRelationDetail.Emissary)
			{
				if (!hero.IsNotable)
				{
					if (hero.MapFaction != null && hero.MapFaction.Leader == hero)
					{
						num *= 30f;
					}
					else if (hero.Clan != null && hero.Clan.Leader == hero)
					{
						num *= 20f;
					}
				}
			}
			else if (!hero.IsNotable)
			{
				if (hero.MapFaction != null && hero.MapFaction.Leader == hero)
				{
					num *= 30f;
				}
				else if (hero.Clan != null && hero.Clan.Leader == hero)
				{
					num *= 20f;
				}
				else
				{
					num *= 10f;
				}
			}
			else
			{
				num *= 20f;
			}
			return MathF.Round(num * relationChange);
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x00073DC4 File Offset: 0x00071FC4
		public override uint GetNotificationColor(ChatNotificationType notificationType)
		{
			switch (notificationType)
			{
			case ChatNotificationType.Default:
				return 10066329U;
			case ChatNotificationType.PlayerFactionPositive:
				return 2284902U;
			case ChatNotificationType.PlayerClanPositive:
				return 3407803U;
			case ChatNotificationType.PlayerFactionNegative:
				return 14509602U;
			case ChatNotificationType.PlayerClanNegative:
				return 16750899U;
			case ChatNotificationType.Civilian:
				return 10053324U;
			case ChatNotificationType.PlayerClanCivilian:
				return 15623935U;
			case ChatNotificationType.PlayerFactionCivilian:
				return 11163101U;
			case ChatNotificationType.Neutral:
				return 12303291U;
			case ChatNotificationType.PlayerFactionIndirectPositive:
				return 12298820U;
			case ChatNotificationType.PlayerFactionIndirectNegative:
				return 13382502U;
			case ChatNotificationType.PlayerClanPolitical:
				return 6745855U;
			case ChatNotificationType.PlayerFactionPolitical:
				return 5614301U;
			case ChatNotificationType.Political:
				return 6724044U;
			default:
				return 13369548U;
			}
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x00073E6A File Offset: 0x0007206A
		public override float DenarsToInfluence()
		{
			return 0.002f;
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x00073E71 File Offset: 0x00072071
		public override float GetDecisionMakingThreshold(IFaction consideringFaction)
		{
			return Campaign.Current.Models.DiplomacyModel.GetValueOfSettlementsForFaction(consideringFaction) / 6f;
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x00073E8E File Offset: 0x0007208E
		public override bool CanSettlementBeGifted(Settlement settlementToGift)
		{
			return settlementToGift.Town != null && !settlementToGift.Town.IsOwnerUnassigned;
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x00073EA8 File Offset: 0x000720A8
		public override float GetValueOfSettlementsForFaction(IFaction faction)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (Town town in faction.Fiefs)
			{
				if (town.IsTown)
				{
					num += 2000f;
				}
				else
				{
					num += 1000f;
				}
				num += town.Prosperity * 0.33f;
				num2 += (float)town.Villages.Count * 300f;
			}
			num *= 50f;
			num += num2 * 25f;
			num = DefaultDiplomacyModel.AdjustValueOfSettlements(num);
			return num;
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x00073F58 File Offset: 0x00072158
		public override IEnumerable<BarterGroup> GetBarterGroups()
		{
			return new BarterGroup[]
			{
				new GoldBarterGroup(),
				new ItemBarterGroup(),
				new PrisonerBarterGroup(),
				new FiefBarterGroup(),
				new OtherBarterGroup(),
				new DefaultsBarterGroup()
			};
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x00073F90 File Offset: 0x00072190
		public override bool IsPeaceSuitable(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace)
		{
			if (factionDeclaresPeace.IsEliminated || factionDeclaredPeace.IsEliminated)
			{
				return false;
			}
			float scoreOfDeclaringPeace = Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringPeace(factionDeclaresPeace, factionDeclaredPeace);
			float scoreOfDeclaringPeace2 = Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringPeace(factionDeclaredPeace, factionDeclaresPeace);
			float valueOfSettlementsForFaction = Campaign.Current.Models.DiplomacyModel.GetValueOfSettlementsForFaction(factionDeclaresPeace);
			float num;
			if (scoreOfDeclaringPeace2 > 0f)
			{
				num = scoreOfDeclaringPeace2 - scoreOfDeclaringPeace;
			}
			else
			{
				num = Campaign.Current.Models.DiplomacyModel.GetDecisionMakingThreshold(factionDeclaredPeace) - scoreOfDeclaringPeace2;
			}
			return num <= valueOfSettlementsForFaction || factionDeclaresPeace.GetStanceWith(factionDeclaredPeace).WarStartDate.ElapsedDaysUntilNow >= 150f;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0007403C File Offset: 0x0007223C
		public override int GetDailyTributeToPay(Clan factionToPay, Clan factionToReceive, out int tributeDurationInDays)
		{
			float scoreOfDeclaringPeace = Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringPeace(factionToReceive.MapFaction, factionToPay.MapFaction);
			float scoreOfDeclaringPeace2 = Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringPeace(factionToPay.MapFaction, factionToReceive.MapFaction);
			float valueOfSettlementsForFaction = Campaign.Current.Models.DiplomacyModel.GetValueOfSettlementsForFaction(factionToPay.MapFaction);
			float num;
			if (scoreOfDeclaringPeace > 0f)
			{
				num = scoreOfDeclaringPeace - scoreOfDeclaringPeace2;
			}
			else
			{
				num = Campaign.Current.Models.DiplomacyModel.GetDecisionMakingThreshold(factionToReceive.MapFaction) - scoreOfDeclaringPeace;
			}
			float resultNumber = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(factionToPay.MapFaction, factionToReceive.MapFaction, false).ResultNumber;
			float resultNumber2 = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(factionToReceive.MapFaction, factionToPay.MapFaction, false).ResultNumber;
			float num2 = MathF.Abs(resultNumber - resultNumber2);
			if (resultNumber > resultNumber2)
			{
				tributeDurationInDays = 0;
				return 0;
			}
			float num3 = num / (valueOfSettlementsForFaction + 1f);
			if (num2 < 75f)
			{
				num3 = 0.05f;
			}
			else
			{
				num3 /= 2f;
				if (num3 < 0.05f)
				{
					num3 = 0f;
				}
				else if (num3 > 0.05f && num3 < 0.1f)
				{
					num3 = 0.05f;
				}
				else if (num3 > 0.1f && num3 < 0.15f)
				{
					num3 = 0.1f;
				}
				else
				{
					num3 = 0.15f;
				}
			}
			int num4 = (int)(num3 * factionToPay.MapFaction.Fiefs.Sum<Town>((Town x) => x.Prosperity) * 0.35f);
			num4 = 10 * (num4 / 10);
			tributeDurationInDays = ((num4 == 0) ? 0 : 100);
			return num4;
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x00074203 File Offset: 0x00072403
		public override bool IsClanEligibleToBecomeRuler(Clan clan)
		{
			return !clan.IsEliminated && clan.Leader.IsAlive && !clan.IsUnderMercenaryService;
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x00074228 File Offset: 0x00072428
		public override DiplomacyModel.DiplomacyStance? GetShallowDiplomaticStance(IFaction faction1, IFaction faction2)
		{
			if (faction1.IsBanditFaction != faction2.IsBanditFaction)
			{
				return new DiplomacyModel.DiplomacyStance?(DiplomacyModel.DiplomacyStance.War);
			}
			return null;
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x00074253 File Offset: 0x00072453
		public override DiplomacyModel.DiplomacyStance GetDefaultDiplomaticStance(IFaction faction1, IFaction faction2)
		{
			if (this.IsAtConstantWar(faction1, faction2))
			{
				return DiplomacyModel.DiplomacyStance.War;
			}
			return DiplomacyModel.DiplomacyStance.Neutral;
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x00074264 File Offset: 0x00072464
		public override bool IsAtConstantWar(IFaction faction1, IFaction faction2)
		{
			if (((faction1.IsOutlaw && faction1.IsMinorFaction && faction2.IsKingdomFaction) || (faction2.IsOutlaw && faction2.IsMinorFaction && faction1.IsKingdomFaction)) && faction1.Culture == faction2.Culture)
			{
				return true;
			}
			DiplomacyModel.DiplomacyStance? shallowDiplomaticStance = Campaign.Current.Models.DiplomacyModel.GetShallowDiplomaticStance(faction1, faction2);
			DiplomacyModel.DiplomacyStance diplomacyStance = DiplomacyModel.DiplomacyStance.War;
			return (shallowDiplomaticStance.GetValueOrDefault() == diplomacyStance) & (shallowDiplomaticStance != null);
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x000742E4 File Offset: 0x000724E4
		private static DefaultDiplomacyModel.WarStats CalculateWarStatsForPeace(IFaction faction, IFaction targetFaction, IFaction evaluatingFaction)
		{
			float num = 0f;
			float num2 = 0f;
			bool flag = evaluatingFaction.MapFaction == faction.MapFaction;
			float num3 = faction.WarPartyComponents.Sum<WarPartyComponent>((WarPartyComponent x) => x.Party.EstimatedStrength);
			float num4;
			if (!flag)
			{
				num4 = faction.Fiefs.Sum<Town>(delegate(Town x)
				{
					MobileParty garrisonParty = x.GarrisonParty;
					if (garrisonParty == null)
					{
						return 0f;
					}
					return garrisonParty.Party.EstimatedStrength;
				}) * 0.7f;
			}
			else
			{
				num4 = faction.Fiefs.Sum<Town>(delegate(Town x)
				{
					MobileParty garrisonParty2 = x.GarrisonParty;
					if (garrisonParty2 == null)
					{
						return 0f;
					}
					return garrisonParty2.Party.EstimatedStrength;
				});
			}
			float num5 = num4;
			if (faction.IsKingdomFaction)
			{
				foreach (Clan clan in ((Kingdom)faction).Clans)
				{
					if (!clan.IsUnderMercenaryService)
					{
						int partyLimitForTier = Campaign.Current.Models.ClanTierModel.GetPartyLimitForTier(clan, clan.Tier);
						num2 += (float)(partyLimitForTier * 64);
					}
				}
			}
			num += num5 + Math.Max(num3, num2);
			float num6 = 0f;
			IEnumerable<IFaction> factionsAtWarWith = faction.FactionsAtWarWith;
			Func<IFaction, bool> <>9__3;
			Func<IFaction, bool> func;
			if ((func = <>9__3) == null)
			{
				func = (<>9__3 = (IFaction x) => x != targetFaction);
			}
			foreach (IFaction faction2 in factionsAtWarWith.Where<IFaction>(func))
			{
				float num7 = 0f;
				if (!faction2.IsBanditFaction && (!faction2.IsMinorFaction || faction2.Leader == Hero.MainHero) && faction2.IsKingdomFaction)
				{
					int num8 = 0;
					foreach (Clan clan2 in ((Kingdom)faction2).Clans.Where<Clan>((Clan x) => !x.IsUnderMercenaryService))
					{
						num8 += Campaign.Current.Models.ClanTierModel.GetPartyLimitForTier(clan2, clan2.Tier);
					}
					num7 += (float)(num8 * 64);
					float num9 = faction2.WarPartyComponents.Sum<WarPartyComponent>((WarPartyComponent x) => x.Party.EstimatedStrength);
					num6 += Math.Max(num9, num7);
				}
			}
			return new DefaultDiplomacyModel.WarStats
			{
				Strength = num,
				ValueOfSettlements = Campaign.Current.Models.DiplomacyModel.GetValueOfSettlementsForFaction(faction),
				TotalStrengthOfEnemies = (flag ? (num6 * 0.6f) : num6)
			};
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x00074614 File Offset: 0x00072814
		private static DefaultDiplomacyModel.WarStats CalculateWarStatsForWar(IFaction faction, IFaction targetFaction, IFaction evaluatingFaction)
		{
			float num = faction.WarPartyComponents.Sum<WarPartyComponent>((WarPartyComponent x) => x.Party.EstimatedStrength);
			float num2 = faction.Fiefs.Sum<Town>(delegate(Town x)
			{
				MobileParty garrisonParty = x.GarrisonParty;
				if (garrisonParty == null)
				{
					return 0f;
				}
				return garrisonParty.Party.EstimatedStrength;
			});
			float num3 = 0f;
			float num4 = 0f;
			bool flag = evaluatingFaction.MapFaction == faction.MapFaction;
			if (faction.IsKingdomFaction)
			{
				foreach (Clan clan in ((Kingdom)faction).Clans)
				{
					if (!clan.IsUnderMercenaryService)
					{
						int partyLimitForTier = Campaign.Current.Models.ClanTierModel.GetPartyLimitForTier(clan, clan.Tier);
						num4 += (float)(partyLimitForTier * 64);
					}
				}
			}
			num3 += num2 + Math.Max(num, num4);
			float num5 = 0f;
			float num6 = 0f;
			IEnumerable<IFaction> factionsAtWarWith = faction.FactionsAtWarWith;
			Func<IFaction, bool> <>9__2;
			Func<IFaction, bool> func;
			if ((func = <>9__2) == null)
			{
				func = (<>9__2 = (IFaction x) => x.MapFaction != targetFaction.MapFaction);
			}
			foreach (IFaction faction2 in factionsAtWarWith.Where<IFaction>(func))
			{
				if (!faction2.IsBanditFaction && (!faction2.IsMinorFaction || faction2.Leader == Hero.MainHero) && faction2.IsKingdomFaction)
				{
					int num7 = 0;
					foreach (Clan clan2 in ((Kingdom)faction2).Clans.Where<Clan>((Clan x) => !x.IsUnderMercenaryService))
					{
						num7 += Campaign.Current.Models.ClanTierModel.GetPartyLimitForTier(clan2, clan2.Tier);
					}
					num6 += (float)(num7 * 64);
					float num8 = faction2.WarPartyComponents.Sum<WarPartyComponent>((WarPartyComponent x) => x.Party.EstimatedStrength);
					float num9 = faction2.Fiefs.Sum<Town>(delegate(Town x)
					{
						MobileParty garrisonParty2 = x.GarrisonParty;
						if (garrisonParty2 == null)
						{
							return 0f;
						}
						return garrisonParty2.Party.EstimatedStrength;
					}) + Math.Max(num6, num8);
					num5 += num9;
				}
			}
			return new DefaultDiplomacyModel.WarStats
			{
				Strength = num3,
				ValueOfSettlements = Campaign.Current.Models.DiplomacyModel.GetValueOfSettlementsForFaction(faction),
				TotalStrengthOfEnemies = (flag ? (num5 * 1.1f) : num5)
			};
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x00074940 File Offset: 0x00072B40
		private static float GetExposureScoreToOtherFaction(IFaction factionDeclaresWar, IFaction factionDeclaredWar)
		{
			HashSet<Settlement> hashSet = new HashSet<Settlement>();
			float num = 0f;
			float num2 = 0f;
			if (factionDeclaresWar.Fiefs.Count == 0 || factionDeclaredWar.Fiefs.Count == 0)
			{
				return 1f;
			}
			foreach (Town town in factionDeclaresWar.Fiefs)
			{
				foreach (Settlement settlement in town.GetNeighborFortifications(MobileParty.NavigationType.All))
				{
					if (settlement.MapFaction != factionDeclaresWar && !hashSet.Contains(settlement))
					{
						if (settlement.MapFaction == factionDeclaredWar)
						{
							num2 += 1f;
						}
						num += 1f;
						hashSet.Add(settlement);
					}
				}
			}
			if (num2 < 1f)
			{
				return float.MinValue;
			}
			return 0.8f + num2 / num;
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x00074A48 File Offset: 0x00072C48
		private static float CalculateBenefitScore(DefaultDiplomacyModel.WarStats faction1Stats, DefaultDiplomacyModel.WarStats faction2Stats)
		{
			float num = MathF.Clamp(faction2Stats.ValueOfSettlements, 10000f, 10000000f);
			float num2 = (faction2Stats.Strength + faction1Stats.TotalStrengthOfEnemies) / faction1Stats.Strength;
			float num3 = MathF.Clamp(1f / (1f + num2 * num2), 0.1f, 0.9f);
			return num * num3;
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x00074AA0 File Offset: 0x00072CA0
		private static float CalculateRiskScore(DefaultDiplomacyModel.WarStats faction1Stats, DefaultDiplomacyModel.WarStats faction2Stats)
		{
			float num = MathF.Clamp(faction1Stats.ValueOfSettlements, 10000f, 10000000f);
			float num2 = faction1Stats.Strength / (faction2Stats.Strength + faction1Stats.TotalStrengthOfEnemies);
			float num3 = MathF.Clamp(1f / (1f + num2 * num2), 0.1f, 0.9f);
			return num * num3;
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x00074AF8 File Offset: 0x00072CF8
		private static float AdjustValueOfSettlements(float valueOfSettlements)
		{
			if (valueOfSettlements <= 2000000f)
			{
				return valueOfSettlements + 10000f;
			}
			return (valueOfSettlements - 2000000f) * 0.5f + 10000f + 2000000f;
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x00074B24 File Offset: 0x00072D24
		private static float GetRelationScore(IFaction factionDeclaresWar, IFaction factionDeclaredWar, IFaction evaluatingFaction)
		{
			float relationWithClan = (float)factionDeclaresWar.Leader.Clan.GetRelationWithClan(factionDeclaredWar.Leader.Clan);
			int relationWithClan2 = evaluatingFaction.Leader.Clan.GetRelationWithClan(factionDeclaredWar.Leader.Clan);
			float num = (relationWithClan + (float)relationWithClan2) / 2f;
			float num2 = 0f;
			if (num < 0f)
			{
				if (factionDeclaresWar.CurrentTotalStrength > factionDeclaredWar.CurrentTotalStrength * 2f)
				{
					num2 = -250f * num;
				}
				else
				{
					float num3 = factionDeclaresWar.CurrentTotalStrength / (2f * factionDeclaredWar.CurrentTotalStrength);
					num2 = -250f * (num3 * num3) * num;
				}
			}
			return num2;
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x00074BC0 File Offset: 0x00072DC0
		private static float GetSameCultureTownScore(IFaction factionDeclaresWar, IFaction factionDeclaredWar)
		{
			float num = factionDeclaredWar.Settlements.Sum<Settlement>(delegate(Settlement s)
			{
				if (s.Culture != factionDeclaresWar.Culture || !s.IsFortification)
				{
					return 0f;
				}
				return s.Town.Prosperity * 0.5f * 50f;
			});
			float num2 = MathF.Min(100000f, num);
			return 0.3f * num2;
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x00074C08 File Offset: 0x00072E08
		private static void UpdateOurBenefitMinusOurRiskBasedOnEvaluatingFaction(IFaction evaluatingFaction, ref float ourBenefit, ref float ourRisk)
		{
			if (ourBenefit.ApproximatelyEqualsTo(ourRisk, 1E-05f))
			{
				return;
			}
			if (!evaluatingFaction.IsKingdomFaction && evaluatingFaction.Leader != evaluatingFaction.MapFaction.Leader)
			{
				bool flag = ourBenefit > ourRisk;
				if (flag && evaluatingFaction.Leader.GetTraitLevel(DefaultTraits.Valor) != 0)
				{
					ourBenefit *= 1f - 0.05f * (float)MathF.Min(2, MathF.Max(-2, evaluatingFaction.Leader.GetTraitLevel(DefaultTraits.Valor)));
					return;
				}
				if (!flag && evaluatingFaction.Leader.GetTraitLevel(DefaultTraits.Calculating) != 0)
				{
					ourRisk *= 1f + 0.05f * (float)MathF.Min(2, MathF.Max(-2, evaluatingFaction.Leader.GetTraitLevel(DefaultTraits.Calculating)));
				}
			}
		}

		// Token: 0x04000802 RID: 2050
		private const int DailyValueFactorForTributes = 70;

		// Token: 0x04000803 RID: 2051
		private const float ProsperityValueFactor = 50f;

		// Token: 0x04000804 RID: 2052
		private const float StrengthFactor = 50f;

		// Token: 0x04000805 RID: 2053
		private const float DenarsToInfluenceValue = 0.002f;

		// Token: 0x04000806 RID: 2054
		private const float RulingClanToJoinOtherKingdomScore = -100000000f;

		// Token: 0x04000807 RID: 2055
		private const float MinStrengthRequiredForFactionToConsiderWar = 500f;

		// Token: 0x04000808 RID: 2056
		private const int MinWarPartyRequiredToConsiderWar = 2;

		// Token: 0x04000809 RID: 2057
		private const float ClanRichnessEffectMultiplier = 0.15f;

		// Token: 0x0400080A RID: 2058
		private const float MaxBenefitValue = 10000000f;

		// Token: 0x0400080B RID: 2059
		private const float MeaningfulBenefitValue = 2000000f;

		// Token: 0x0400080C RID: 2060
		private const float MinBenefitValue = 10000f;

		// Token: 0x0400080D RID: 2061
		private const float DefaultRelationMultiplierForScoreOfWar = -250f;

		// Token: 0x0400080E RID: 2062
		private const float SameCultureTownMultiplier = 0.3f;

		// Token: 0x0400080F RID: 2063
		private const float MaxAcceptableProsperityValue = 100000f;

		// Token: 0x020005B2 RID: 1458
		private struct WarStats
		{
			// Token: 0x040018C1 RID: 6337
			public float Strength;

			// Token: 0x040018C2 RID: 6338
			public float ValueOfSettlements;

			// Token: 0x040018C3 RID: 6339
			public float TotalStrengthOfEnemies;
		}
	}
}
