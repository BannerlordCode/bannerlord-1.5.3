using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x0200001D RID: 29
	public static class PerkHelper
	{
		// Token: 0x060000FA RID: 250 RVA: 0x0000D458 File Offset: 0x0000B658
		public static void ClearPerksForSkill(Hero hero, SkillObject skill)
		{
			foreach (PerkObject perkObject in PerkObject.All)
			{
				if (perkObject.Skill == skill)
				{
					PerkHelper.ClearPermanentBonusesIfExists(hero, perkObject);
					hero.SetPerkValueInternal(perkObject, false);
				}
			}
			PartyBase.MainParty.MemberRoster.UpdateVersion();
			hero.HitPoints = MathF.Min(hero.HitPoints, hero.MaxHitPoints);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000D4E4 File Offset: 0x0000B6E4
		public static IEnumerable<PerkObject> GetCaptainPerksForTroopUsages(TroopUsageFlags troopUsageFlags, BattleEnvironment battleEnvironment = BattleEnvironment.Any)
		{
			List<PerkObject> list = new List<PerkObject>();
			foreach (PerkObject perkObject in PerkObject.All)
			{
				if (perkObject.PrimaryRole == PartyRole.Captain && perkObject.PrimaryTroopUsageMask != TroopUsageFlags.Undefined && perkObject.ApplicableInEnvironment(battleEnvironment, true) && (perkObject.PrimaryTroopUsageMask == TroopUsageFlags.Any || troopUsageFlags.HasAllFlags(perkObject.PrimaryTroopUsageMask)))
				{
					list.Add(perkObject);
				}
				else if (perkObject.SecondaryRole == PartyRole.Captain && perkObject.SecondaryTroopUsageMask != TroopUsageFlags.Undefined && perkObject.ApplicableInEnvironment(battleEnvironment, false) && (perkObject.SecondaryTroopUsageMask == TroopUsageFlags.Any || troopUsageFlags.HasAllFlags(perkObject.SecondaryTroopUsageMask)))
				{
					list.Add(perkObject);
				}
			}
			return list;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000D5BC File Offset: 0x0000B7BC
		public static bool PlayerHasAnyItemDonationPerk()
		{
			Hero hero;
			return MobileParty.MainParty.HasPerk(DefaultPerks.Steward.GivingHands, out hero, false) || MobileParty.MainParty.HasPerk(DefaultPerks.Steward.PaidInPromise, out hero, true);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000D5F1 File Offset: 0x0000B7F1
		public static bool AddPerkBonusForParty(PerkObject perk, MobileParty party, bool isPrimaryBonus, ref ExplainedNumber stat)
		{
			return PerkHelper.AddPerkBonusForParty(perk, party.CurrentBattleEnvironment, party, isPrimaryBonus, ref stat);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000D604 File Offset: 0x0000B804
		public static bool AddPerkBonusForParty(PerkObject perk, BattleEnvironment battleEnvironment, MobileParty party, bool isPrimaryBonus, ref ExplainedNumber stat)
		{
			Hero hero = null;
			if (party != null && party.HasPerk(perk, battleEnvironment, out hero, !isPrimaryBonus))
			{
				EffectIncrementType effectIncrementType;
				float num;
				PerkHelper.CalculateContextualPerkData(perk, battleEnvironment, isPrimaryBonus, out effectIncrementType, out num);
				PerkHelper.AddToStat(ref stat, effectIncrementType, num, perk.Name);
				return true;
			}
			return false;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000D644 File Offset: 0x0000B844
		public static bool AddPerkBonusForCharacter(PerkObject perk, BattleEnvironment battleEnvironment, CharacterObject character, bool isPrimaryBonus, ref ExplainedNumber bonuses)
		{
			if (perk.ApplicableInEnvironment(battleEnvironment, isPrimaryBonus))
			{
				if (((isPrimaryBonus && perk.PrimaryRole == PartyRole.Personal) || (!isPrimaryBonus && perk.SecondaryRole == PartyRole.Personal)) && character.GetPerkValue(perk))
				{
					EffectIncrementType effectIncrementType;
					float num;
					PerkHelper.CalculateContextualPerkData(perk, battleEnvironment, isPrimaryBonus, out effectIncrementType, out num);
					PerkHelper.AddToStat(ref bonuses, effectIncrementType, num, perk.Name);
					return true;
				}
				if (((isPrimaryBonus && perk.PrimaryRole == PartyRole.ClanLeader) || (!isPrimaryBonus && perk.SecondaryRole == PartyRole.ClanLeader)) && character.IsHero)
				{
					Clan clan = character.HeroObject.Clan;
					if (((clan != null) ? clan.Leader : null) != null && character.HeroObject.Clan.Leader.GetPerkValue(perk))
					{
						EffectIncrementType effectIncrementType2;
						float num2;
						PerkHelper.CalculateContextualPerkData(perk, battleEnvironment, isPrimaryBonus, out effectIncrementType2, out num2);
						PerkHelper.AddToStat(ref bonuses, effectIncrementType2, num2, perk.Name);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000D710 File Offset: 0x0000B910
		public static bool AddEpicPerkBonusForCharacterWithSkill(PerkObject perk, BattleEnvironment battleEnvironment, CharacterObject character, int effectiveSkill, bool isPrimaryBonus, ref ExplainedNumber bonuses, int skillRequired)
		{
			if (perk.ApplicableInEnvironment(battleEnvironment, isPrimaryBonus) && character.GetPerkValue(perk) && effectiveSkill > skillRequired)
			{
				EffectIncrementType effectIncrementType;
				float num;
				PerkHelper.CalculateContextualPerkData(perk, battleEnvironment, isPrimaryBonus, out effectIncrementType, out num);
				PerkHelper.AddToStat(ref bonuses, effectIncrementType, num * (float)(effectiveSkill - skillRequired), perk.Name);
				return true;
			}
			return false;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000D75B File Offset: 0x0000B95B
		public static bool AddEpicPerkBonusForCharacter(PerkObject perk, BattleEnvironment battleEnvironment, CharacterObject character, SkillObject skillType, bool isPrimaryBonus, ref ExplainedNumber bonuses, int skillRequired)
		{
			return PerkHelper.AddEpicPerkBonusForCharacterWithSkill(perk, battleEnvironment, character, character.GetSkillValue(skillType), isPrimaryBonus, ref bonuses, skillRequired);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000D774 File Offset: 0x0000B974
		public static bool AddPerkBonusFromCaptain(PerkObject perk, BattleEnvironment battleEnvironment, CharacterObject captainCharacter, ref ExplainedNumber bonuses)
		{
			bool flag = perk.PrimaryRole == PartyRole.Captain;
			if ((flag || perk.SecondaryRole == PartyRole.Captain) && perk.ApplicableInEnvironment(battleEnvironment, flag) && captainCharacter != null && captainCharacter.GetPerkValue(perk))
			{
				EffectIncrementType effectIncrementType;
				float num;
				PerkHelper.CalculateContextualPerkData(perk, battleEnvironment, flag, out effectIncrementType, out num);
				PerkHelper.AddToStat(ref bonuses, effectIncrementType, num, perk.Name);
				return true;
			}
			return false;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000D7CC File Offset: 0x0000B9CC
		public static bool AddPerkBonusForTown(PerkObject perk, Town town, bool isPrimaryBonus, ref ExplainedNumber bonuses)
		{
			Hero governor = town.Governor;
			if (governor != null && governor.GetPerkValue(perk) && governor.CurrentSettlement != null && governor.CurrentSettlement == town.Settlement)
			{
				PerkHelper.AddToStat(ref bonuses, isPrimaryBonus ? perk.PrimaryIncrementType : perk.SecondaryIncrementType, isPrimaryBonus ? perk.PrimaryBonus : perk.SecondaryBonus, perk.Name);
				return true;
			}
			return false;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000D834 File Offset: 0x0000BA34
		public static Hero GetHeroForTownPerk(PerkObject perk, Town town)
		{
			if (perk.PrimaryRole == PartyRole.ClanLeader || perk.SecondaryRole == PartyRole.ClanLeader)
			{
				Clan ownerClan = town.Owner.Settlement.OwnerClan;
				Hero hero = ((ownerClan != null) ? ownerClan.Leader : null);
				if (hero != null && hero.GetPerkValue(perk))
				{
					return hero;
				}
			}
			if (perk.PrimaryRole == PartyRole.Governor || perk.SecondaryRole == PartyRole.Governor)
			{
				Hero governor = town.Governor;
				if (governor != null && governor.GetPerkValue(perk) && governor.CurrentSettlement != null && governor.CurrentSettlement == town.Settlement)
				{
					return governor;
				}
			}
			return null;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000D8BC File Offset: 0x0000BABC
		public static bool GetPerkValueForTown(PerkObject perk, Town town)
		{
			if (perk.PrimaryRole == PartyRole.ClanLeader || perk.SecondaryRole == PartyRole.ClanLeader)
			{
				Clan ownerClan = town.Owner.Settlement.OwnerClan;
				Hero hero = ((ownerClan != null) ? ownerClan.Leader : null);
				if (hero != null && hero.GetPerkValue(perk))
				{
					return true;
				}
			}
			if (perk.PrimaryRole == PartyRole.Governor || perk.SecondaryRole == PartyRole.Governor)
			{
				Hero governor = town.Governor;
				if (governor != null && governor.GetPerkValue(perk) && governor.CurrentSettlement != null && governor.CurrentSettlement == town.Settlement)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000D944 File Offset: 0x0000BB44
		public static List<PerkObject> GetGovernorPerksForHero(Hero hero)
		{
			List<PerkObject> list = new List<PerkObject>();
			foreach (PerkObject perkObject in PerkObject.All)
			{
				if ((perkObject.PrimaryRole == PartyRole.Governor || perkObject.SecondaryRole == PartyRole.Governor) && hero.GetPerkValue(perkObject))
				{
					list.Add(perkObject);
				}
			}
			return list;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000D9B8 File Offset: 0x0000BBB8
		public static ValueTuple<TextObject, TextObject> GetGovernorEngineeringSkillEffectForHero(Hero governor)
		{
			if (governor != null && governor.GetSkillValue(DefaultSkills.Engineering) > 0)
			{
				SkillEffect townProjectBuildingBonus = DefaultSkillEffects.TownProjectBuildingBonus;
				int skillValue = governor.GetSkillValue(townProjectBuildingBonus.EffectedSkill);
				TextObject effectDescriptionForSkillLevel = SkillHelper.GetEffectDescriptionForSkillLevel(townProjectBuildingBonus, skillValue);
				return new ValueTuple<TextObject, TextObject>(DefaultSkills.Engineering.Name, effectDescriptionForSkillLevel);
			}
			return new ValueTuple<TextObject, TextObject>(TextObject.GetEmpty(), new TextObject("{=0rBsbw1T}No effect", null));
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000DA18 File Offset: 0x0000BC18
		private static void CalculateContextualPerkData(PerkObject perk, BattleEnvironment battleEnvironment, bool isPrimaryBonus, out EffectIncrementType incrementType, out float perkBonus)
		{
			PerkObject.EffectEnvironment effectEnvironment;
			if (isPrimaryBonus)
			{
				perkBonus = perk.PrimaryBonus;
				effectEnvironment = perk.PrimaryEffectEnvironment;
				incrementType = perk.PrimaryIncrementType;
			}
			else
			{
				perkBonus = perk.SecondaryBonus;
				effectEnvironment = perk.SecondaryEffectEnvironment;
				incrementType = perk.SecondaryIncrementType;
			}
			float num = ((effectEnvironment == PerkObject.EffectEnvironment.NavalReduced && battleEnvironment == BattleEnvironment.Naval) ? 0.5f : 1f);
			perkBonus *= num;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000DA78 File Offset: 0x0000BC78
		public static int AvailablePerkCountOfHero(Hero hero)
		{
			MBList<PerkObject> mblist = new MBList<PerkObject>();
			foreach (PerkObject perkObject in PerkObject.All)
			{
				SkillObject skill = perkObject.Skill;
				if ((float)hero.GetSkillValue(skill) >= perkObject.RequiredSkillValue && !hero.GetPerkValue(perkObject) && (perkObject.AlternativePerk == null || !hero.GetPerkValue(perkObject.AlternativePerk)) && !mblist.Contains(perkObject.AlternativePerk))
				{
					mblist.Add(perkObject);
				}
			}
			return mblist.Count;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000DB1C File Offset: 0x0000BD1C
		private static void ClearPermanentBonusesIfExists(Hero hero, PerkObject perk)
		{
			if (!hero.GetPerkValue(perk))
			{
				return;
			}
			if (perk == DefaultPerks.Crafting.VigorousSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Vigor, 1);
				return;
			}
			if (perk == DefaultPerks.Crafting.StrongSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Control, 1);
				return;
			}
			if (perk == DefaultPerks.Crafting.EnduringSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Endurance, 1);
				return;
			}
			if (perk == DefaultPerks.Crafting.WeaponMasterSmith)
			{
				hero.HeroDeveloper.RemoveFocus(DefaultSkills.OneHanded, 1);
				hero.HeroDeveloper.RemoveFocus(DefaultSkills.TwoHanded, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.Durable)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Endurance, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.Steady)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Control, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.Strong)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Vigor, 1);
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000DBF9 File Offset: 0x0000BDF9
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

		// Token: 0x04000004 RID: 4
		public const float NavalBattleEnvironmentMultiplier = 0.5f;
	}
}
