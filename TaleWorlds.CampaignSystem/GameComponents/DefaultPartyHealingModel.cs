using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200013A RID: 314
	public class DefaultPartyHealingModel : PartyHealingModel
	{
		// Token: 0x060019B0 RID: 6576 RVA: 0x0007FB24 File Offset: 0x0007DD24
		public override float GetSurgeryChance(PartyBase party)
		{
			MobileParty mobileParty = party.MobileParty;
			int? num;
			if (mobileParty == null)
			{
				num = null;
			}
			else
			{
				Hero effectiveSurgeon = mobileParty.EffectiveSurgeon;
				num = ((effectiveSurgeon != null) ? new int?(effectiveSurgeon.GetSkillValue(DefaultSkills.Medicine)) : null);
			}
			int num2 = num ?? 0;
			return 0.0015f * (float)num2;
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x0007FB88 File Offset: 0x0007DD88
		public override float GetSiegeBombardmentHitSurgeryChance(PartyBase party)
		{
			float num = 0f;
			Hero hero = null;
			if (party != null && party.IsMobile && party.MobileParty.HasPerk(DefaultPerks.Medicine.SiegeMedic, out hero, false))
			{
				num += DefaultPerks.Medicine.SiegeMedic.PrimaryBonus;
			}
			return num;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x0007FBCC File Offset: 0x0007DDCC
		public override float GetSurvivalChance(PartyBase party, CharacterObject character, DamageTypes damageType, bool canDamageKillEvenIfBlunt, PartyBase enemyParty = null)
		{
			if ((damageType == DamageTypes.Blunt && !canDamageKillEvenIfBlunt) || (character.IsHero && CampaignOptions.BattleDeath == CampaignOptions.Difficulty.VeryEasy) || (character.IsPlayerCharacter && CampaignOptions.BattleDeath == CampaignOptions.Difficulty.Easy))
			{
				return 1f;
			}
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			float num;
			if (((party != null) ? party.MobileParty : null) != null)
			{
				MobileParty mobileParty = party.MobileParty;
				this.AddSurgeonSurvivalBonus(mobileParty, ref explainedNumber);
				Hero hero = null;
				if (((enemyParty != null) ? enemyParty.MobileParty : null) != null && enemyParty.MobileParty.HasPerk(DefaultPerks.Medicine.DoctorsOath, out hero, false))
				{
					DefaultPartyHealingModel.AddDoctorsOathSkillBonusForParty(enemyParty.MobileParty, ref explainedNumber);
					SkillLevelingManager.OnSurgeryApplied(enemyParty.MobileParty, false, character.Tier);
				}
				explainedNumber.Add((float)character.Level * 0.02f, null, null);
				if (!character.IsHero && party.MapEvent != null && character.Tier < 3)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.PhysicianOfPeople, party.MobileParty, false, ref explainedNumber);
				}
				if (character.IsHero)
				{
					explainedNumber.Add(character.GetTotalArmorSum(Equipment.EquipmentType.Battle) * 0.01f, null, null);
					explainedNumber.Add(character.Age * -0.01f, null, null);
					explainedNumber.AddFactor(50f, null);
				}
				ExplainedNumber explainedNumber2 = new ExplainedNumber(1f / explainedNumber.ResultNumber, false, null);
				if (character.IsHero)
				{
					if (party.IsMobile)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.CheatDeath, party.MobileParty, false, ref explainedNumber2);
					}
					if (character.HeroObject.Clan == Clan.PlayerClan)
					{
						float clanMemberDeathChanceMultiplier = Campaign.Current.Models.DifficultyModel.GetClanMemberDeathChanceMultiplier();
						if (!clanMemberDeathChanceMultiplier.ApproximatelyEqualsTo(0f, 1E-05f))
						{
							explainedNumber2.AddFactor(clanMemberDeathChanceMultiplier, GameTexts.FindText("str_game_difficulty", null));
						}
					}
				}
				num = 1f - MBMath.ClampFloat(explainedNumber2.ResultNumber, 0f, 1f);
			}
			else if (character.IsHero && character.HeroObject.IsPrisoner)
			{
				num = 1f - character.Age * 0.0035f;
			}
			else if (explainedNumber.ResultNumber.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				num = 0f;
			}
			else
			{
				num = 1f - 1f / explainedNumber.ResultNumber;
			}
			return num;
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x0007FE08 File Offset: 0x0007E008
		public override int GetSkillXpFromHealingTroop(PartyBase party)
		{
			return 5;
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x0007FE0C File Offset: 0x0007E00C
		public override ExplainedNumber GetDailyHealingForRegulars(PartyBase party, bool isPrisoners, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			if (isPrisoners)
			{
				explainedNumber.Add(1f, null, null);
			}
			else if (party != null && party.IsMobile)
			{
				MobileParty mobileParty = party.MobileParty;
				if (party.IsStarving || (mobileParty.IsGarrison && mobileParty.CurrentSettlement.IsStarving))
				{
					if (mobileParty.IsGarrison)
					{
						if (SettlementHelper.IsGarrisonStarving(mobileParty.CurrentSettlement))
						{
							int num = MBRandom.RoundRandomized((float)party.MemberRoster.TotalRegulars * 0.1f);
							explainedNumber.Add((float)(-(float)num), DefaultPartyHealingModel._starvingText, null);
						}
					}
					else
					{
						int totalRegulars = party.MemberRoster.TotalRegulars;
						explainedNumber.Add((float)(-(float)totalRegulars) * 0.25f, DefaultPartyHealingModel._starvingText, null);
					}
				}
				else
				{
					explainedNumber.Add(5f, null, null);
					if (mobileParty.IsGarrison)
					{
						if (mobileParty.CurrentSettlement.IsTown)
						{
							SkillHelper.AddSkillBonusForTown(DefaultSkillEffects.GovernorHealingRateBonus, mobileParty.CurrentSettlement.Town, ref explainedNumber);
						}
					}
					else
					{
						SkillHelper.AddSkillBonusForParty(DefaultSkillEffects.HealingRateBonusForRegulars, mobileParty, ref explainedNumber);
					}
					if (!mobileParty.IsGarrison && !mobileParty.IsMilitia)
					{
						if (!mobileParty.IsMoving)
						{
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.TriageTent, mobileParty, true, ref explainedNumber);
						}
						else
						{
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.WalkItOff, mobileParty, true, ref explainedNumber);
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.WalkItOff, mobileParty, true, ref explainedNumber);
						}
					}
					if (mobileParty.Morale >= Campaign.Current.Models.PartyMoraleModel.HighMoraleValue)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BestMedicine, mobileParty, true, ref explainedNumber);
					}
					if (mobileParty.CurrentSettlement != null && !mobileParty.CurrentSettlement.IsHideout)
					{
						if (mobileParty.CurrentSettlement.IsFortification)
						{
							explainedNumber.Add(10f, DefaultPartyHealingModel._settlementText, null);
						}
						if (party.SiegeEvent == null && !mobileParty.CurrentSettlement.IsUnderSiege && !mobileParty.CurrentSettlement.IsRaided && !mobileParty.CurrentSettlement.IsUnderRaid)
						{
							if (mobileParty.CurrentSettlement.IsTown)
							{
								PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.PristineStreets, mobileParty, false, ref explainedNumber);
							}
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.AGoodDaysRest, mobileParty, true, ref explainedNumber);
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.GoodLogdings, mobileParty, true, ref explainedNumber);
						}
					}
					else if (!mobileParty.IsMoving && mobileParty.LastVisitedSettlement != null && mobileParty.LastVisitedSettlement.IsVillage && mobileParty.LastVisitedSettlement.Position.DistanceSquared(party.Position) < 2f && !mobileParty.LastVisitedSettlement.IsUnderRaid && !mobileParty.LastVisitedSettlement.IsRaided)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BushDoctor, mobileParty, false, ref explainedNumber);
					}
					if (mobileParty.Army != null)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Rearguard, mobileParty, true, ref explainedNumber);
					}
					Hero hero = null;
					if (party.ItemRoster.FoodVariety > 0 && mobileParty.HasPerk(DefaultPerks.Medicine.PerfectHealth, out hero, false))
					{
						float num2 = DefaultPerks.Medicine.PerfectHealth.PrimaryBonus;
						if (party.IsMobile && party.MobileParty.IsCurrentlyAtSea)
						{
							num2 *= 0.5f;
						}
						explainedNumber.AddFactor((float)mobileParty.ItemRoster.FoodVariety * num2, DefaultPerks.Medicine.PerfectHealth.Name);
					}
					Hero hero2 = null;
					if (mobileParty.HasPerk(DefaultPerks.Medicine.HelpingHands, out hero2, false))
					{
						float num3 = (float)MathF.Floor((float)party.MemberRoster.TotalManCount / 10f);
						float num4 = DefaultPerks.Medicine.HelpingHands.PrimaryBonus;
						if (mobileParty.IsCurrentlyAtSea)
						{
							num4 *= 0.5f;
						}
						float num5 = num3 * num4;
						explainedNumber.AddFactor(num5, DefaultPerks.Medicine.HelpingHands.Name);
					}
				}
				if (mobileParty.IsInRaftState)
				{
					int totalRegulars2 = party.MemberRoster.TotalRegulars;
					explainedNumber.Add((float)(-(float)totalRegulars2) * 0.25f, DefaultPartyHealingModel._raftStateText, null);
				}
			}
			return explainedNumber;
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x000801C4 File Offset: 0x0007E3C4
		public override ExplainedNumber GetDailyHealingHpForHeroes(PartyBase party, bool isPrisoners, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			if (isPrisoners)
			{
				explainedNumber.Add(20f, null, null);
			}
			else if (party == null)
			{
				explainedNumber.Add(11f, null, null);
			}
			else if (party.IsMobile)
			{
				MobileParty mobileParty = party.MobileParty;
				if (party.IsStarving && mobileParty.CurrentSettlement == null)
				{
					return new ExplainedNumber(-19f, includeDescriptions, DefaultPartyHealingModel._starvingText);
				}
				explainedNumber.Add(11f, null, null);
				if (!mobileParty.IsGarrison && !mobileParty.IsMilitia)
				{
					if (!mobileParty.IsMoving)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.TriageTent, mobileParty, true, ref explainedNumber);
					}
					else
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.WalkItOff, mobileParty, true, ref explainedNumber);
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.WalkItOff, mobileParty, true, ref explainedNumber);
					}
				}
				if (mobileParty.Morale >= Campaign.Current.Models.PartyMoraleModel.HighMoraleValue)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BestMedicine, mobileParty, true, ref explainedNumber);
				}
				if (mobileParty.CurrentSettlement != null && !mobileParty.CurrentSettlement.IsHideout)
				{
					if (mobileParty.CurrentSettlement.IsFortification)
					{
						explainedNumber.Add(8f, DefaultPartyHealingModel._settlementText, null);
					}
					if (mobileParty.CurrentSettlement.IsTown)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.PristineStreets, mobileParty, false, ref explainedNumber);
					}
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.AGoodDaysRest, mobileParty, true, ref explainedNumber);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.GoodLogdings, mobileParty, true, ref explainedNumber);
				}
				else if (!mobileParty.IsMoving && mobileParty.LastVisitedSettlement != null && mobileParty.LastVisitedSettlement.IsVillage && mobileParty.LastVisitedSettlement.Position.DistanceSquared(party.Position) < 2f && !mobileParty.LastVisitedSettlement.IsUnderRaid && !mobileParty.LastVisitedSettlement.IsRaided)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BushDoctor, mobileParty, false, ref explainedNumber);
				}
				SkillHelper.AddSkillBonusForParty(DefaultSkillEffects.HealingRateBonusForHeroes, mobileParty, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x000803A4 File Offset: 0x0007E5A4
		public override ExplainedNumber GetBattleEndHealingAmount(PartyBase party, Hero hero)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			if (hero.GetPerkValue(DefaultPerks.Medicine.PreventiveMedicine))
			{
				explainedNumber.Add(DefaultPerks.Medicine.PreventiveMedicine.SecondaryBonus * (float)(hero.MaxHitPoints - hero.HitPoints), DefaultPerks.Medicine.PreventiveMedicine.Name, null);
			}
			if (party.MapEventSide == party.MapEvent.AttackerSide)
			{
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.WalkItOff, BattleEnvironment.Any, hero.CharacterObject, false, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x00080420 File Offset: 0x0007E620
		private static void AddDoctorsOathSkillBonusForParty(MobileParty enemyParty, ref ExplainedNumber explainedNumber)
		{
			Hero effectiveRoleHolder = enemyParty.GetEffectiveRoleHolder(PartyRole.Surgeon);
			CharacterObject characterObject = ((effectiveRoleHolder != null) ? effectiveRoleHolder.CharacterObject : null) ?? SkillHelper.GetEffectivePartyLeaderForSkill(enemyParty.Party);
			if (characterObject != null)
			{
				MapEvent mapEvent = enemyParty.MapEvent;
				bool flag = mapEvent != null && mapEvent.IsPlayerMapEvent;
				int skillValue = characterObject.GetSkillValue(DefaultSkillEffects.SurgeonSurvivalBonus.EffectedSkill);
				float skillEffectValue = DefaultSkillEffects.SurgeonSurvivalBonus.GetSkillEffectValue(skillValue);
				explainedNumber.Add(skillEffectValue * (flag ? 1f : 0.1f), explainedNumber.IncludeDescriptions ? GameTexts.FindText("role", PartyRole.Surgeon.ToString()) : null, null);
			}
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x000804C0 File Offset: 0x0007E6C0
		private void AddSurgeonSurvivalBonus(MobileParty mobileParty, ref ExplainedNumber survivalDenominator)
		{
			Hero effectiveRoleHolder = mobileParty.GetEffectiveRoleHolder(PartyRole.Surgeon);
			CharacterObject characterObject = ((effectiveRoleHolder != null) ? effectiveRoleHolder.CharacterObject : null) ?? SkillHelper.GetEffectivePartyLeaderForSkill(mobileParty.Party);
			if (characterObject != null)
			{
				MapEvent mapEvent = mobileParty.MapEvent;
				bool flag = mapEvent != null && mapEvent.IsPlayerMapEvent;
				int skillValue = characterObject.GetSkillValue(DefaultSkillEffects.SurgeonSurvivalBonus.EffectedSkill);
				float skillEffectValue = DefaultSkillEffects.SurgeonSurvivalBonus.GetSkillEffectValue(skillValue);
				survivalDenominator.Add(skillEffectValue * (flag ? 1f : 0.25f), survivalDenominator.IncludeDescriptions ? GameTexts.FindText("role", PartyRole.Surgeon.ToString()) : null, null);
			}
		}

		// Token: 0x04000852 RID: 2130
		private const int StarvingEffectHeroes = -19;

		// Token: 0x04000853 RID: 2131
		private const int FortificationEffectForHeroes = 8;

		// Token: 0x04000854 RID: 2132
		private const int FortificationEffectForRegulars = 10;

		// Token: 0x04000855 RID: 2133
		private const int BaseDailyHealingForHeroes = 11;

		// Token: 0x04000856 RID: 2134
		private const int DailyHealingForPrisonerHeroes = 20;

		// Token: 0x04000857 RID: 2135
		private const int DailyHealingForPrisonerRegulars = 1;

		// Token: 0x04000858 RID: 2136
		private const int BaseDailyHealingForTroops = 5;

		// Token: 0x04000859 RID: 2137
		private const int SkillEXPFromHealingTroops = 5;

		// Token: 0x0400085A RID: 2138
		private const float StarvingWoundedEffectRatio = 0.25f;

		// Token: 0x0400085B RID: 2139
		private const float StarvingWoundedEffectRatioForGarrison = 0.1f;

		// Token: 0x0400085C RID: 2140
		private const float DriftingWoundedEffectRatio = 0.25f;

		// Token: 0x0400085D RID: 2141
		private const float AISurgeonSurvivalMultiplier = 0.25f;

		// Token: 0x0400085E RID: 2142
		private const float DoctorsOathMultiplier = 0.1f;

		// Token: 0x0400085F RID: 2143
		private static readonly TextObject _starvingText = new TextObject("{=jZYUdkXF}Starving", null);

		// Token: 0x04000860 RID: 2144
		private static readonly TextObject _settlementText = new TextObject("{=M0Gpl0dH}In Settlement", null);

		// Token: 0x04000861 RID: 2145
		private static readonly TextObject _raftStateText = new TextObject("{=dNJLG7O5}Stranded at sea", null);
	}
}
