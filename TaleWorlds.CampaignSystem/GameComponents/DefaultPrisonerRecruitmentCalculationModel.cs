using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200014B RID: 331
	public class DefaultPrisonerRecruitmentCalculationModel : PrisonerRecruitmentCalculationModel
	{
		// Token: 0x06001A45 RID: 6725 RVA: 0x000846C8 File Offset: 0x000828C8
		public override int GetConformityNeededToRecruitPrisoner(CharacterObject character)
		{
			return (character.Level + 6) * (character.Level + 6) - 10;
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x000846E0 File Offset: 0x000828E0
		public override ExplainedNumber GetConformityChangePerHour(PartyBase party, CharacterObject troopToBoost)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(10f, false, null);
			if (party.LeaderHero != null)
			{
				explainedNumber.Add((float)party.LeaderHero.GetSkillValue(DefaultSkills.Leadership) * 0.05f, null, null);
			}
			if (troopToBoost.Tier <= 3)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.FerventAttacker, party.MobileParty, false, ref explainedNumber);
			}
			if (troopToBoost.Tier >= 4)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.StoutDefender, party.MobileParty, false, ref explainedNumber);
			}
			if (troopToBoost.Occupation != Occupation.Bandit)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.LoyaltyAndHonor, party.MobileParty, false, ref explainedNumber);
			}
			if (troopToBoost.IsInfantry)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.LeadByExample, party.MobileParty, true, ref explainedNumber);
			}
			if (troopToBoost.IsRanged)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.TrustedCommander, party.MobileParty, true, ref explainedNumber);
			}
			if (troopToBoost.Occupation == Occupation.Bandit)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.Promises, party.MobileParty, false, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x000847D4 File Offset: 0x000829D4
		public override float GetPrisonerRecruitmentMoraleEffect(PartyBase party, CharacterObject character, int num)
		{
			Hero hero = null;
			Hero hero2 = null;
			CultureObject culture = character.Culture;
			Hero leaderHero = party.LeaderHero;
			bool flag = culture == ((leaderHero != null) ? leaderHero.Culture : null) && party.MobileParty != null && party.MobileParty.HasPerk(DefaultPerks.Leadership.Presence, out hero, true);
			bool flag2 = character.Occupation == Occupation.Bandit && party.MobileParty != null && party.MobileParty.HasPerk(DefaultPerks.Roguery.TwoFaced, out hero2, true);
			if (flag || flag2)
			{
				return 0f;
			}
			float num2;
			if (character.Occupation == Occupation.Bandit)
			{
				num2 = -2f;
			}
			else
			{
				num2 = -1f;
			}
			float num3 = num2 * (float)num;
			if (party.LeaderHero != null)
			{
				float traitEffectBonus = TraitEffectHelper.GetTraitEffectBonus(party.LeaderHero, DefaultPersonalityTraitEffects.HonorRecruitPenaltyReductionEffect);
				if (traitEffectBonus != 0f)
				{
					num3 *= 1f + traitEffectBonus;
				}
			}
			return num3;
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x000848A0 File Offset: 0x00082AA0
		public override bool IsPrisonerRecruitable(PartyBase party, CharacterObject character, out int conformityNeeded)
		{
			if (!character.IsRegular || character.Tier > Campaign.Current.Models.CharacterStatsModel.MaxCharacterTier || character.Tier < 2 || character.Culture.IsBandit)
			{
				conformityNeeded = 0;
				return false;
			}
			int elementXp = party.MobileParty.PrisonRoster.GetElementXp(character);
			conformityNeeded = this.GetConformityNeededToRecruitPrisoner(character);
			return elementXp >= conformityNeeded;
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x00084910 File Offset: 0x00082B10
		public override bool ShouldPartyRecruitPrisoners(PartyBase party)
		{
			return party.IsMobile && party.PartySizeLimit > party.MobileParty.MemberRoster.TotalManCount && !party.MobileParty.IsWageLimitExceeded() && !party.MobileParty.IsPatrolParty && (party.MobileParty.Morale > 30f || DefaultPrisonerRecruitmentCalculationModel.ShouldRecruitDueToPresencePerk(party.MobileParty));
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x00084978 File Offset: 0x00082B78
		public override int CalculateRecruitableNumber(PartyBase party, CharacterObject character)
		{
			if (character.IsHero || party.PrisonRoster.Count == 0 || party.PrisonRoster.TotalRegulars <= 0)
			{
				return 0;
			}
			int conformityNeededToRecruitPrisoner = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetConformityNeededToRecruitPrisoner(character);
			int elementXp = party.PrisonRoster.GetElementXp(character);
			int elementNumber = party.PrisonRoster.GetElementNumber(character);
			return MathF.Min(elementXp / conformityNeededToRecruitPrisoner, elementNumber);
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x000849E4 File Offset: 0x00082BE4
		private static bool ShouldRecruitDueToPresencePerk(MobileParty mobileParty)
		{
			Hero hero = null;
			return mobileParty.HasPerk(DefaultPerks.Leadership.Presence, out hero, true);
		}

		// Token: 0x040008B2 RID: 2226
		private const int AILordMinTierRequirementForRecruitPrisoners = 2;
	}
}
