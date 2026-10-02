using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000144 RID: 324
	public class DefaultPartyTroopUpgradeModel : PartyTroopUpgradeModel
	{
		// Token: 0x06001A18 RID: 6680 RVA: 0x00082ED4 File Offset: 0x000810D4
		public override bool CanPartyUpgradeTroopToTarget(PartyBase upgradingParty, CharacterObject upgradeableCharacter, CharacterObject upgradeTarget)
		{
			bool flag = Campaign.Current.Models.PartyTroopUpgradeModel.DoesPartyHaveRequiredItemsForUpgrade(upgradingParty, upgradeTarget);
			PerkObject perkObject;
			bool flag2 = Campaign.Current.Models.PartyTroopUpgradeModel.DoesPartyHaveRequiredPerksForUpgrade(upgradingParty, upgradeableCharacter, upgradeTarget, out perkObject);
			return Campaign.Current.Models.PartyTroopUpgradeModel.IsTroopUpgradeable(upgradingParty, upgradeableCharacter) && upgradeableCharacter.UpgradeTargets.Contains(upgradeTarget) && flag2 && flag;
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x00082F3D File Offset: 0x0008113D
		public override bool IsTroopUpgradeable(PartyBase party, CharacterObject character)
		{
			return !character.IsHero && character.UpgradeTargets.Length != 0;
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x00082F54 File Offset: 0x00081154
		public override int GetXpCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget)
		{
			if (upgradeTarget != null && characterObject.UpgradeTargets.Contains(upgradeTarget))
			{
				int tier = upgradeTarget.Tier;
				int num = 0;
				for (int i = characterObject.Tier + 1; i <= tier; i++)
				{
					if (i <= 1)
					{
						num += 100;
					}
					else if (i == 2)
					{
						num += 300;
					}
					else if (i == 3)
					{
						num += 550;
					}
					else if (i == 4)
					{
						num += 900;
					}
					else if (i == 5)
					{
						num += 1300;
					}
					else if (i == 6)
					{
						num += 1700;
					}
					else if (i == 7)
					{
						num += 2100;
					}
					else
					{
						int num2 = upgradeTarget.Level + 4;
						num += (int)(1.333f * (float)num2 * (float)num2);
					}
				}
				return num;
			}
			return 100000000;
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x00083014 File Offset: 0x00081214
		public override ExplainedNumber GetGoldCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget)
		{
			PartyWageModel partyWageModel = Campaign.Current.Models.PartyWageModel;
			int roundedResultNumber = partyWageModel.GetTroopRecruitmentCost(upgradeTarget, null, true).RoundedResultNumber;
			int roundedResultNumber2 = partyWageModel.GetTroopRecruitmentCost(characterObject, null, true).RoundedResultNumber;
			bool flag = characterObject.Occupation == Occupation.Mercenary || characterObject.Occupation == Occupation.Gangster;
			ExplainedNumber explainedNumber = new ExplainedNumber((float)(roundedResultNumber - roundedResultNumber2) / ((!flag) ? 2f : 3f), false, null);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.SoundReserves, party.MobileParty, true, ref explainedNumber);
			if (characterObject.IsRanged)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.RenownedArcher, party.MobileParty, false, ref explainedNumber);
			}
			if (characterObject.IsMounted)
			{
				FeatHelper.ApplyCultureFeat(party, DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat, ref explainedNumber);
			}
			if (flag)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.Contractors, party.MobileParty, true, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x000830E6 File Offset: 0x000812E6
		public override int GetSkillXpFromUpgradingTroops(PartyBase party, CharacterObject troop, int numberOfTroops)
		{
			return (troop.Level + 10) * numberOfTroops;
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x000830F4 File Offset: 0x000812F4
		public override bool DoesPartyHaveRequiredItemsForUpgrade(PartyBase party, CharacterObject upgradeTarget)
		{
			ItemCategory upgradeRequiresItemFromCategory = upgradeTarget.UpgradeRequiresItemFromCategory;
			if (upgradeRequiresItemFromCategory != null)
			{
				int num = 0;
				for (int i = 0; i < party.ItemRoster.Count; i++)
				{
					ItemRosterElement itemRosterElement = party.ItemRoster[i];
					if (itemRosterElement.EquipmentElement.Item.ItemCategory == upgradeRequiresItemFromCategory)
					{
						num += itemRosterElement.Amount;
					}
				}
				return num > 0;
			}
			return true;
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x00083158 File Offset: 0x00081358
		public override bool DoesPartyHaveRequiredPerksForUpgrade(PartyBase party, CharacterObject character, CharacterObject upgradeTarget, out PerkObject requiredPerk)
		{
			requiredPerk = null;
			if (character.Culture.IsBandit && !upgradeTarget.Culture.IsBandit)
			{
				requiredPerk = DefaultPerks.Leadership.VeteransRespect;
				Hero hero = null;
				return party.MobileParty.HasPerk(requiredPerk, out hero, true);
			}
			return true;
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x000831A0 File Offset: 0x000813A0
		public override float GetUpgradeChanceForTroopUpgrade(PartyBase party, CharacterObject troop, int upgradeTargetIndex)
		{
			float num = 1f;
			int num2 = troop.UpgradeTargets.Length;
			if (num2 > 1 && upgradeTargetIndex >= 0 && upgradeTargetIndex < num2)
			{
				if (party.LeaderHero != null && party.LeaderHero.PreferredUpgradeFormation != FormationClass.NumberOfAllFormations)
				{
					FormationClass preferredUpgradeFormation = party.LeaderHero.PreferredUpgradeFormation;
					if (CharacterHelper.SearchForFormationInTroopTree(troop.UpgradeTargets[upgradeTargetIndex], preferredUpgradeFormation))
					{
						num = 9999f;
					}
				}
				else
				{
					Hero leaderHero = party.LeaderHero;
					int num3 = ((leaderHero != null) ? leaderHero.RandomValue : party.Id.GetHashCode());
					int deterministicHashCode = troop.StringId.GetDeterministicHashCode();
					uint num4 = (uint)((num3 >> ((troop.Tier * 3) & 31)) ^ deterministicHashCode);
					if ((long)upgradeTargetIndex == (long)((ulong)num4 % (ulong)((long)num2)))
					{
						num = 9999f;
					}
				}
			}
			return num;
		}
	}
}
