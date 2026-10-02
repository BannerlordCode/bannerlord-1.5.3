using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200013C RID: 316
	public class DefaultPartyMoraleModel : PartyMoraleModel
	{
		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060019C1 RID: 6593 RVA: 0x00080733 File Offset: 0x0007E933
		public override float HighMoraleValue
		{
			get
			{
				return 60f;
			}
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x0008073A File Offset: 0x0007E93A
		public override int GetDailyStarvationMoralePenalty(PartyBase party)
		{
			return -5;
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0008073E File Offset: 0x0007E93E
		public override int GetDailyNoWageMoralePenalty(MobileParty party)
		{
			return -3;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x00080742 File Offset: 0x0007E942
		private int GetStarvationMoralePenalty(MobileParty party)
		{
			return -30;
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x00080746 File Offset: 0x0007E946
		private int GetNoWageMoralePenalty(MobileParty party)
		{
			return -20;
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x0008074A File Offset: 0x0007E94A
		public override float GetStandardBaseMorale(PartyBase party)
		{
			return 50f;
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x00080751 File Offset: 0x0007E951
		public override float GetVictoryMoraleChange(PartyBase party)
		{
			return 20f;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x00080758 File Offset: 0x0007E958
		public override float GetDefeatMoraleChange(PartyBase party)
		{
			return -20f;
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x00080760 File Offset: 0x0007E960
		private void CalculateFoodVarietyMoraleBonus(MobileParty party, ref ExplainedNumber result)
		{
			if (!party.Party.IsStarving)
			{
				float num;
				switch (party.ItemRoster.FoodVariety)
				{
				case 0:
				case 1:
					num = -2f;
					break;
				case 2:
					num = -1f;
					break;
				case 3:
					num = 0f;
					break;
				case 4:
					num = 1f;
					break;
				case 5:
					num = 2f;
					break;
				case 6:
					num = 3f;
					break;
				case 7:
					num = 5f;
					break;
				case 8:
					num = 6f;
					break;
				case 9:
					num = 7f;
					break;
				case 10:
					num = 8f;
					break;
				case 11:
					num = 9f;
					break;
				case 12:
					num = 10f;
					break;
				default:
					num = 10f;
					break;
				}
				if (num < 0f && party.LeaderHero != null && party.LeaderHero.GetPerkValue(DefaultPerks.Steward.WarriorsDiet))
				{
					num = 0f;
				}
				if (num != 0f)
				{
					result.Add(num, this._foodBonusMoraleText, null);
					Hero hero = null;
					if (num > 0f && party.HasPerk(DefaultPerks.Steward.Gourmet, out hero, false))
					{
						if (party.IsCurrentlyAtSea)
						{
							num *= 0.5f;
						}
						result.Add(num, DefaultPerks.Steward.Gourmet.Name, null);
					}
				}
			}
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x000808A8 File Offset: 0x0007EAA8
		private void GetPartySizeMoraleEffect(MobileParty mobileParty, ref ExplainedNumber result)
		{
			if (!mobileParty.IsMilitia && !mobileParty.IsVillager)
			{
				int num = mobileParty.Party.NumberOfAllMembers - mobileParty.Party.PartySizeLimit;
				if (num > 0)
				{
					result.Add(-1f * MathF.Sqrt((float)num), this._partySizeMoraleText, null);
				}
			}
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x000808FC File Offset: 0x0007EAFC
		private static void CheckPerkEffectOnPartyMorale(MobileParty party, PerkObject perk, bool isInfoNeeded, TextObject newInfo, int perkEffect, out TextObject outNewInfo, out int outPerkEffect)
		{
			outNewInfo = newInfo;
			outPerkEffect = perkEffect;
			if (party.LeaderHero != null && party.LeaderHero.GetPerkValue(perk))
			{
				if (isInfoNeeded)
				{
					MBTextManager.SetTextVariable("EFFECT_NAME", perk.Name, false);
					MBTextManager.SetTextVariable("NUM", 10);
					MBTextManager.SetTextVariable("STR1", newInfo, false);
					MBTextManager.SetTextVariable("STR2", GameTexts.FindText("str_party_effect", null), false);
					outNewInfo = GameTexts.FindText("str_new_item_line", null);
				}
				outPerkEffect += 10;
			}
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x00080984 File Offset: 0x0007EB84
		private void GetMoraleEffectsFromPerks(MobileParty party, ref ExplainedNumber bonus)
		{
			Hero hero = null;
			if (party.HasPerk(DefaultPerks.Crossbow.PeasantLeader, out hero, false))
			{
				float num = this.CalculateTroopTierRatio(party);
				float num2 = DefaultPerks.Crossbow.PeasantLeader.PrimaryBonus * num;
				bonus.AddFactor(num2, DefaultPerks.Crossbow.PeasantLeader.Name);
			}
			Hero hero2 = null;
			Settlement currentSettlement = party.CurrentSettlement;
			if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) != null && party.HasPerk(DefaultPerks.Charm.SelfPromoter, out hero2, true))
			{
				bonus.Add(DefaultPerks.Charm.SelfPromoter.SecondaryBonus, DefaultPerks.Charm.SelfPromoter.Name, null);
			}
			Hero hero3 = null;
			if (party.HasPerk(DefaultPerks.Steward.Logistician, out hero3, false))
			{
				int num3 = 0;
				for (int i = 0; i < party.MemberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(i);
					if (elementCopyAtIndex.Character.IsMounted)
					{
						num3 += elementCopyAtIndex.Number;
					}
				}
				if (party.Party.NumberOfMounts > party.MemberRoster.TotalManCount - num3)
				{
					bonus.Add(DefaultPerks.Steward.Logistician.PrimaryBonus, DefaultPerks.Steward.Logistician.Name, null);
				}
			}
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x00080A9C File Offset: 0x0007EC9C
		private float CalculateTroopTierRatio(MobileParty party)
		{
			int totalManCount = party.MemberRoster.TotalManCount;
			float num = 0f;
			foreach (TroopRosterElement troopRosterElement in party.MemberRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.Tier <= 3)
				{
					num += (float)troopRosterElement.Number;
				}
			}
			return num / (float)totalManCount;
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x00080B1C File Offset: 0x0007ED1C
		private void GetMoraleEffectsFromSkill(MobileParty party, ref ExplainedNumber bonus)
		{
			CharacterObject effectivePartyLeaderForSkill = SkillHelper.GetEffectivePartyLeaderForSkill(party.Party);
			if (effectivePartyLeaderForSkill != null && effectivePartyLeaderForSkill.GetSkillValue(DefaultSkills.Leadership) > 0)
			{
				SkillHelper.AddSkillBonusForCharacter(DefaultSkillEffects.LeadershipMoraleBonus, effectivePartyLeaderForSkill, ref bonus);
			}
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x00080B54 File Offset: 0x0007ED54
		public override ExplainedNumber GetEffectivePartyMorale(MobileParty mobileParty, bool includeDescription = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(50f, includeDescription, null);
			ExplainedNumber explainedNumber2 = new ExplainedNumber(mobileParty.RecentEventsMorale, includeDescription, null);
			if (mobileParty.RecentEventsMorale > 0f && mobileParty.LeaderHero != null)
			{
				TraitEffectHelper.ApplyTraitEffect(mobileParty.LeaderHero, DefaultPersonalityTraitEffects.GenerosityMoraleGainEffect, ref explainedNumber2);
			}
			else if (mobileParty.RecentEventsMorale < 0f)
			{
				Army army = mobileParty.Army;
				Hero hero;
				if (army == null)
				{
					hero = null;
				}
				else
				{
					MobileParty leaderParty = army.LeaderParty;
					hero = ((leaderParty != null) ? leaderParty.LeaderHero : null);
				}
				Hero hero2 = hero ?? mobileParty.LeaderHero;
				if (hero2 != null)
				{
					TraitEffectHelper.ApplyTraitEffect(hero2, DefaultPersonalityTraitEffects.ValorLossMoraleResistEffect, ref explainedNumber2);
				}
			}
			if (mobileParty.LeaderHero != null)
			{
				TraitEffectHelper.ApplyTraitEffect(mobileParty.LeaderHero, DefaultPersonalityTraitEffects.GenerosityFlatMoraleEffect, ref explainedNumber);
			}
			explainedNumber.AddFromExplainedNumber(explainedNumber2, this._recentEventsText);
			this.GetMoraleEffectsFromSkill(mobileParty, ref explainedNumber);
			if (mobileParty.IsMilitia || mobileParty.IsGarrison)
			{
				if (mobileParty.IsMilitia)
				{
					if (mobileParty.HomeSettlement.IsStarving)
					{
						explainedNumber.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
					}
				}
				else if (SettlementHelper.IsGarrisonStarving(mobileParty.CurrentSettlement))
				{
					explainedNumber.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
				}
			}
			else if (mobileParty.Party.IsStarving)
			{
				explainedNumber.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
			}
			if (mobileParty.HasUnpaidWages > 0f)
			{
				explainedNumber.Add(mobileParty.HasUnpaidWages * (float)this.GetNoWageMoralePenalty(mobileParty), this._noWageMoraleText, null);
			}
			this.GetMoraleEffectsFromPerks(mobileParty, ref explainedNumber);
			this.CalculateFoodVarietyMoraleBonus(mobileParty, ref explainedNumber);
			this.GetPartySizeMoraleEffect(mobileParty, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x04000864 RID: 2148
		private const float BaseMoraleValue = 50f;

		// Token: 0x04000865 RID: 2149
		private readonly TextObject _recentEventsText = GameTexts.FindText("str_recent_events", null);

		// Token: 0x04000866 RID: 2150
		private readonly TextObject _starvationMoraleText = GameTexts.FindText("str_starvation_morale", null);

		// Token: 0x04000867 RID: 2151
		private readonly TextObject _noWageMoraleText = GameTexts.FindText("str_no_wage_morale", null);

		// Token: 0x04000868 RID: 2152
		private readonly TextObject _foodBonusMoraleText = GameTexts.FindText("str_food_bonus_morale", null);

		// Token: 0x04000869 RID: 2153
		private readonly TextObject _partySizeMoraleText = GameTexts.FindText("str_party_size_morale", null);
	}
}
