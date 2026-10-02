using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010C RID: 268
	public class DefaultClanPoliticsModel : ClanPoliticsModel
	{
		// Token: 0x060017B0 RID: 6064 RVA: 0x0006F624 File Offset: 0x0006D824
		public override ExplainedNumber CalculateInfluenceChange(Clan clan, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateInfluenceChangeInternal(clan, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x0006F64C File Offset: 0x0006D84C
		private void CalculateInfluenceChangeInternal(Clan clan, ref ExplainedNumber influenceChange)
		{
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Charm.ImmortalCharm, BattleEnvironment.Any, clan.Leader.CharacterObject, true, ref influenceChange);
			if (clan.IsUnderMercenaryService)
			{
				int num = MathF.Ceiling(clan.Influence * (1f / Campaign.Current.Models.ClanFinanceModel.RevenueSmoothenFraction()));
				influenceChange.Add((float)(-(float)num), DefaultClanPoliticsModel._mercenaryStr, null);
			}
			float num2 = 0f;
			foreach (WarPartyComponent warPartyComponent in clan.WarPartyComponents)
			{
				MobileParty mobileParty = warPartyComponent.MobileParty;
				if (mobileParty.Army != null && mobileParty.Army.LeaderParty != mobileParty && mobileParty.LeaderHero != null && mobileParty.Army.LeaderParty.LeaderHero != null && warPartyComponent.Clan != mobileParty.Army.LeaderParty.LeaderHero.Clan)
				{
					num2 += Campaign.Current.Models.ArmyManagementCalculationModel.DailyBeingAtArmyInfluenceAward(mobileParty);
				}
			}
			influenceChange.Add(num2, DefaultClanPoliticsModel._armyMemberStr, null);
			if (clan.MapFaction.Leader == clan.Leader && clan.MapFaction.IsKingdomFaction)
			{
				influenceChange.Add(3f, DefaultClanPoliticsModel._kingBonusStr, null);
			}
			foreach (Settlement settlement in clan.Settlements)
			{
				if (settlement.IsFortification)
				{
					settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.Influence, ref influenceChange);
				}
			}
			if (clan == Clan.PlayerClan && clan.MapFaction.MainHeroCrimeRating > 0f)
			{
				int num3 = (int)(clan.MapFaction.MainHeroCrimeRating * -0.5f);
				influenceChange.Add((float)num3, DefaultClanPoliticsModel._crimeStr, null);
			}
			float num4 = 0f;
			foreach (Hero hero in clan.SupporterNotables)
			{
				if (hero.CurrentSettlement != null)
				{
					float influenceBonusToClan = Campaign.Current.Models.NotablePowerModel.GetInfluenceBonusToClan(hero);
					num4 += influenceBonusToClan;
				}
			}
			if (num4 > 0f)
			{
				influenceChange.Add(num4, DefaultClanPoliticsModel._supporterStr, null);
			}
			if (clan.Kingdom != null && !clan.IsUnderMercenaryService)
			{
				this.CalculateInfluenceChangeDueToPolicies(clan, ref influenceChange);
			}
			this.CalculateInfluenceChangeDueToIssues(clan, ref influenceChange);
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x0006F8DC File Offset: 0x0006DADC
		private void CalculateInfluenceChangeDueToIssues(Clan clan, ref ExplainedNumber influenceChange)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectOfClan(DefaultIssueEffects.ClanInfluence, clan, ref influenceChange);
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x0006F8FC File Offset: 0x0006DAFC
		private void CalculateInfluenceChangeDueToPolicies(Clan clan, ref ExplainedNumber influenceChange)
		{
			if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.FeudalInheritance))
			{
				influenceChange.Add(0.1f * (float)clan.Settlements.Count, DefaultPolicies.FeudalInheritance.Name, null);
			}
			if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Serfdom))
			{
				influenceChange.Add(0.2f * (float)clan.Settlements.Count<Settlement>((Settlement t) => t.IsVillage), DefaultPolicies.Serfdom.Name, null);
			}
			if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.LordsPrivyCouncil) && clan.Tier >= 5)
			{
				influenceChange.Add(0.5f, DefaultPolicies.LordsPrivyCouncil.Name, null);
			}
			if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Senate) && clan.Tier >= 3)
			{
				influenceChange.Add(0.5f, DefaultPolicies.Senate.Name, null);
			}
			if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.NobleRetinues) && clan.Tier >= 5)
			{
				influenceChange.Add(-1f, DefaultPolicies.NobleRetinues.Name, null);
			}
			if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Bailiffs))
			{
				int num = clan.Settlements.Count<Settlement>((Settlement settlement) => settlement.IsTown && settlement.Town.Security > 60f);
				influenceChange.Add((float)num, DefaultPolicies.Bailiffs.Name, null);
			}
			if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.CouncilOfTheCommons))
			{
				float num2 = 0f;
				foreach (Settlement settlement2 in clan.Settlements)
				{
					num2 += (float)settlement2.Notables.Count * 0.1f;
				}
				influenceChange.Add(num2, DefaultPolicies.CouncilOfTheCommons.Name, null);
			}
			if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.TrialByJury))
			{
				influenceChange.Add(-1f, DefaultPolicies.TrialByJury.Name, null);
			}
			if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Lawspeakers))
			{
				float num3 = ((clan.Leader.GetSkillValue(DefaultSkills.Charm) > 100) ? 1f : (-1f));
				influenceChange.Add(num3, DefaultPolicies.Lawspeakers.Name, null);
			}
			if (clan == clan.Kingdom.RulingClan)
			{
				if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.SacredMajesty))
				{
					influenceChange.Add(3f, DefaultPolicies.SacredMajesty.Name, null);
				}
				if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Marshals))
				{
					influenceChange.Add(-1f, DefaultPolicies.Marshals.Name, null);
					return;
				}
			}
			else
			{
				if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.SacredMajesty))
				{
					influenceChange.Add(-0.5f, DefaultPolicies.SacredMajesty.Name, null);
				}
				if (clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.RoyalGuard))
				{
					influenceChange.Add(-0.2f, DefaultPolicies.RoyalGuard.Name, null);
				}
			}
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0006FC54 File Offset: 0x0006DE54
		public override float CalculateSupportForPolicyInClan(Clan clan, PolicyObject policy)
		{
			float num = 0f;
			float num2 = 1f;
			float num3 = (float)clan.Leader.GetTraitLevel(DefaultTraits.Authoritarian) * policy.AuthoritarianWeight * num2;
			float num4 = (float)clan.Leader.GetTraitLevel(DefaultTraits.Egalitarian) * policy.EgalitarianWeight * num2;
			float num5 = (float)clan.Leader.GetTraitLevel(DefaultTraits.Oligarchic) * policy.OligarchicWeight * num2;
			float num6;
			float num7;
			float num8;
			if (clan.Tier == 1)
			{
				num6 = policy.EgalitarianWeight;
				num7 = 0f;
				num8 = 0f;
			}
			else if (clan.Tier == 2)
			{
				num6 = policy.EgalitarianWeight;
				num7 = 0f;
				num8 = 0f;
			}
			else if (clan.Tier == 3)
			{
				num6 = policy.EgalitarianWeight;
				num7 = 0f;
				num8 = 0f;
			}
			else if (clan.Tier == 4)
			{
				num6 = 0f;
				num7 = policy.OligarchicWeight;
				num8 = 0f;
			}
			else if (clan.Tier == 5)
			{
				num6 = 0f;
				num7 = policy.OligarchicWeight;
				num8 = 0f;
			}
			else
			{
				num6 = 0f;
				num7 = policy.OligarchicWeight;
				num8 = 0f;
			}
			float num9 = 0f;
			if (clan.Kingdom.RulingClan == clan)
			{
				if (clan.Leader.GetTraitLevel(DefaultTraits.Oligarchic) > 0 || clan.Leader.GetTraitLevel(DefaultTraits.Egalitarian) > 0)
				{
					num9 = -0.5f;
				}
				else if (clan.Leader.GetTraitLevel(DefaultTraits.Authoritarian) > 0)
				{
					num9 = 1f;
				}
			}
			return MathF.Clamp(num + (num3 + num4 + num5 + num6 + num7 + num8 + num9), -2f, 2f);
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x0006FE0F File Offset: 0x0006E00F
		public override float CalculateRelationshipChangeWithSponsor(Clan clan, Clan sponsorClan)
		{
			return MathF.Lerp(-2f, 2f, MathF.Clamp((float)clan.Leader.GetRelation(sponsorClan.Leader) / 60f, 0f, 1f), 1E-05f);
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0006FE4C File Offset: 0x0006E04C
		public override int GetInfluenceRequiredToOverrideKingdomDecision(DecisionOutcome popularOption, DecisionOutcome overridingOption, KingdomDecision decision)
		{
			float totalSupportPoints = popularOption.TotalSupportPoints;
			float num = overridingOption.TotalSupportPoints;
			float num2 = 0f;
			if (decision.Kingdom.RulingClan == Clan.PlayerClan)
			{
				if (totalSupportPoints == num + 1f)
				{
					num += 1f;
					num2 += (float)decision.GetInfluenceCostOfSupport(Clan.PlayerClan, Supporter.SupportWeights.SlightlyFavor);
				}
				else if (totalSupportPoints == num + 2f)
				{
					num += 2f;
					num2 += (float)decision.GetInfluenceCostOfSupport(Clan.PlayerClan, Supporter.SupportWeights.StronglyFavor);
				}
				else if (totalSupportPoints > num + 2f)
				{
					num += 3f;
					num2 += (float)decision.GetInfluenceCostOfSupport(Clan.PlayerClan, Supporter.SupportWeights.FullyPush);
				}
			}
			if (totalSupportPoints > num)
			{
				float num3 = (totalSupportPoints - num) / 3f * (float)decision.GetInfluenceCostOfSupport(decision.Kingdom.RulingClan, Supporter.SupportWeights.FullyPush) * 1.4f;
				if (decision.Kingdom.RulingClan != Clan.PlayerClan)
				{
					num3 *= 0.8f;
				}
				num2 += num3;
			}
			num2 = (float)(5 * (int)(num2 / 5f));
			if (totalSupportPoints > num && decision.Kingdom.ActivePolicies.Contains(DefaultPolicies.RoyalPrivilege))
			{
				num2 *= 0.8f;
			}
			return (int)num2;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0006FF60 File Offset: 0x0006E160
		public override bool CanHeroBeGovernor(Hero hero)
		{
			return hero.IsActive && !hero.IsChild && !hero.IsHumanPlayerCharacter && !hero.IsPartyLeader && !hero.IsFugitive && !hero.IsReleased && !hero.IsTraveling && !hero.IsPrisoner && hero.CanBeGovernorOrHavePartyRole() && !hero.IsSpecial && !hero.IsTemplate;
		}

		// Token: 0x040007F1 RID: 2033
		private static readonly TextObject _supporterStr = new TextObject("{=RzFyGnWJ}Supporters", null);

		// Token: 0x040007F2 RID: 2034
		private static readonly TextObject _crimeStr = new TextObject("{=MvxW9rmf}Criminal", null);

		// Token: 0x040007F3 RID: 2035
		private static readonly TextObject _armyMemberStr = new TextObject("{=XAdBVsXV}Clan members in an army", null);

		// Token: 0x040007F4 RID: 2036
		private static readonly TextObject _townProjectStr = new TextObject("{=8Yb3IVvb}Settlement Buildings", null);

		// Token: 0x040007F5 RID: 2037
		private static readonly TextObject _courtshipPerkStr = new TextObject("{=zgzDwZKZ}Courtship from clan parties", null);

		// Token: 0x040007F6 RID: 2038
		private static readonly TextObject _mercenaryStr = new TextObject("{=qcaaJLhx}Mercenary Contract", null);

		// Token: 0x040007F7 RID: 2039
		private static readonly TextObject _kingBonusStr = new TextObject("{=JNS46jsG}King bonus", null);
	}
}
