using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x0200001C RID: 28
	public static class SkillHelper
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x0000D18C File Offset: 0x0000B38C
		public static void AddSkillBonusForSkillLevel(SkillEffect skillEffect, ref ExplainedNumber explainedNumber, int skillLevel)
		{
			float skillEffectValue = skillEffect.GetSkillEffectValue(skillLevel);
			SkillHelper.AddToStat(ref explainedNumber, skillEffect.IncrementType, skillEffectValue, explainedNumber.IncludeDescriptions ? GameTexts.FindText("role", skillEffect.Role.ToString()) : null);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000D1D8 File Offset: 0x0000B3D8
		public static void AddSkillBonusForParty(SkillEffect skillEffect, MobileParty party, ref ExplainedNumber explainedNumber)
		{
			CharacterObject characterObject;
			if (skillEffect.Role == PartyRole.PartyLeader && party.LeaderHero != null)
			{
				Hero leaderHero = party.LeaderHero;
				characterObject = ((leaderHero != null) ? leaderHero.CharacterObject : null);
			}
			else if (party.GetEffectiveRoleHolder(skillEffect.Role) != null)
			{
				characterObject = party.GetEffectiveRoleHolder(skillEffect.Role).CharacterObject;
			}
			else
			{
				characterObject = SkillHelper.GetEffectivePartyLeaderForSkill(party.Party);
			}
			if (characterObject != null)
			{
				int skillValue = characterObject.GetSkillValue(skillEffect.EffectedSkill);
				float skillEffectValue = skillEffect.GetSkillEffectValue(skillValue);
				SkillHelper.AddToStat(ref explainedNumber, skillEffect.IncrementType, skillEffectValue, explainedNumber.IncludeDescriptions ? GameTexts.FindText("role", skillEffect.Role.ToString()) : null);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000D28C File Offset: 0x0000B48C
		public static void AddSkillBonusForTown(SkillEffect skillEffect, Town town, ref ExplainedNumber explainedNumber)
		{
			CharacterObject characterObject = null;
			if (skillEffect.Role == PartyRole.ClanLeader)
			{
				Clan ownerClan = town.Owner.Settlement.OwnerClan;
				characterObject = ((ownerClan != null) ? ownerClan.Leader.CharacterObject : null);
			}
			else if (skillEffect.Role == PartyRole.Governor)
			{
				Hero governor = town.Governor;
				characterObject = ((governor != null) ? governor.CharacterObject : null);
			}
			if (characterObject != null)
			{
				int skillValue = characterObject.GetSkillValue(skillEffect.EffectedSkill);
				float skillEffectValue = skillEffect.GetSkillEffectValue(skillValue);
				SkillHelper.AddToStat(ref explainedNumber, skillEffect.IncrementType, skillEffectValue, explainedNumber.IncludeDescriptions ? GameTexts.FindText("role", skillEffect.Role.ToString()) : null);
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000D334 File Offset: 0x0000B534
		public static void AddSkillBonusForCharacter(SkillEffect skillEffect, CharacterObject character, ref ExplainedNumber explainedNumber)
		{
			int skillValue = character.GetSkillValue(skillEffect.EffectedSkill);
			float skillEffectValue = skillEffect.GetSkillEffectValue(skillValue);
			SkillHelper.AddToStat(ref explainedNumber, skillEffect.IncrementType, skillEffectValue, explainedNumber.IncludeDescriptions ? GameTexts.FindText("role", skillEffect.Role.ToString()) : null);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000D38C File Offset: 0x0000B58C
		public static TextObject GetEffectDescriptionForSkillLevel(SkillEffect effect, int level)
		{
			float skillEffectValue = effect.GetSkillEffectValue(level);
			float num = ((effect.IncrementType == EffectIncrementType.AddFactor) ? (skillEffectValue * 100f) : skillEffectValue);
			effect.Description.SetTextVariable("a0", MathF.Abs(num).ToString("0.0"));
			return effect.Description;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000D3DF File Offset: 0x0000B5DF
		private static void AddToStat(ref ExplainedNumber stat, EffectIncrementType effectIncrementType, float number, TextObject text)
		{
			if (effectIncrementType == EffectIncrementType.Add)
			{
				stat.Add(number, text, null);
				return;
			}
			if (effectIncrementType == EffectIncrementType.AddFactor)
			{
				stat.AddFactor(number, text);
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000D3FA File Offset: 0x0000B5FA
		public static CharacterObject GetEffectivePartyLeaderForSkill(PartyBase party)
		{
			if (party == null)
			{
				return null;
			}
			if (party.LeaderHero != null)
			{
				return party.LeaderHero.CharacterObject;
			}
			TroopRoster memberRoster = party.MemberRoster;
			if (memberRoster == null || memberRoster.TotalManCount <= 0)
			{
				return null;
			}
			return party.MemberRoster.GetCharacterAtIndex(0);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000D43A File Offset: 0x0000B63A
		public static int GetHeroRelevantSkillValueForPartyRole(Hero hero, PartyRole role)
		{
			return hero.GetSkillValue(Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(role));
		}
	}
}
