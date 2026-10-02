using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000142 RID: 322
	public class DefaultPartyTrainingModel : PartyTrainingModel
	{
		// Token: 0x06001A0E RID: 6670 RVA: 0x00082939 File Offset: 0x00080B39
		public override int GetXpReward(CharacterObject character)
		{
			int num = character.Level + 6;
			return num * num / 3;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x00082948 File Offset: 0x00080B48
		public override ExplainedNumber GetEffectiveDailyExperience(MobileParty mobileParty, TroopRosterElement troop)
		{
			ExplainedNumber explainedNumber = default(ExplainedNumber);
			if (mobileParty.IsLordParty && !troop.Character.IsHero && (mobileParty.Army == null || mobileParty.Army.LeaderParty != MobileParty.MainParty) && mobileParty.MapEvent == null && (mobileParty.Party.Owner == null || mobileParty.Party.Owner.Clan != Clan.PlayerClan))
			{
				if (mobileParty.LeaderHero != null && mobileParty.LeaderHero == mobileParty.ActualClan.Leader)
				{
					explainedNumber.Add(15f + (float)troop.Character.Tier * 3f, null, null);
				}
				else
				{
					explainedNumber.Add(10f + (float)troop.Character.Tier * 2f, null, null);
				}
			}
			Hero hero = null;
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Leadership.CombatTips, out hero, false))
			{
				explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Leadership.CombatTips), null, null);
			}
			Hero hero2 = null;
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Leadership.RaiseTheMeek, out hero2, false) && troop.Character.Tier < 3)
			{
				explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Leadership.RaiseTheMeek), null, null);
			}
			if (mobileParty.IsGarrison)
			{
				Settlement currentSettlement = mobileParty.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town.Governor : null) != null && mobileParty.CurrentSettlement.Town.Governor.GetPerkValue(DefaultPerks.Bow.BullsEye))
				{
					explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Bow.BullsEye), null, null);
				}
			}
			Hero hero3 = null;
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Polearm.Drills, out hero3, true))
			{
				explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Polearm.Drills), null, null);
			}
			Hero hero4 = null;
			if (mobileParty.IsActive && troop.Character.IsInfantry && mobileParty.HasPerk(DefaultPerks.OneHanded.MilitaryTradition, out hero4, false))
			{
				explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.OneHanded.MilitaryTradition), null, null);
			}
			Hero hero5 = null;
			if (mobileParty.IsActive && mobileParty.IsMoving && !troop.Character.IsMounted && mobileParty.HasPerk(DefaultPerks.Athletics.WalkItOff, out hero5, true))
			{
				explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Athletics.WalkItOff), null, null);
			}
			Hero hero6 = null;
			if (mobileParty.IsActive && troop.Character.IsInfantry && mobileParty.HasPerk(DefaultPerks.Throwing.Saddlebags, out hero6, true))
			{
				explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Throwing.Saddlebags), null, null);
			}
			Hero hero7 = null;
			if (mobileParty.IsActive && !mobileParty.IsMoving && mobileParty.CurrentSettlement != null && !troop.Character.IsMounted && mobileParty.HasPerk(DefaultPerks.Athletics.AGoodDaysRest, out hero7, true))
			{
				explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Athletics.AGoodDaysRest), null, null);
			}
			Hero hero8 = null;
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Bow.Trainer, out hero8, true) && troop.Character.IsRanged)
			{
				explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Bow.Trainer), null, null);
			}
			Hero hero9 = null;
			if (mobileParty.IsActive && troop.Character.IsRanged && mobileParty.HasPerk(DefaultPerks.Crossbow.RenownMarksmen, out hero9, false))
			{
				explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Crossbow.RenownMarksmen), null, null);
			}
			if (mobileParty.IsActive && mobileParty.IsMoving)
			{
				if (mobileParty.Morale > 75f)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.ForcedMarch, mobileParty, false, ref explainedNumber);
				}
				if (mobileParty.TotalWeightCarried > (float)mobileParty.InventoryCapacity)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Unburdened, mobileParty, false, ref explainedNumber);
				}
			}
			Hero hero10 = null;
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Steward.SevenVeterans, out hero10, false) && troop.Character.Tier >= 4)
			{
				explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Steward.SevenVeterans), null, null);
			}
			Hero hero11 = null;
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Steward.DrillSergant, out hero11, false))
			{
				explainedNumber.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Steward.DrillSergant), null, null);
			}
			if (troop.Character.Culture.IsBandit)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.NoRestForTheWicked, mobileParty, true, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x00082D70 File Offset: 0x00080F70
		private int GetPerkExperiencesForTroops(PerkObject perk)
		{
			if (perk == DefaultPerks.Leadership.CombatTips || perk == DefaultPerks.Leadership.RaiseTheMeek || perk == DefaultPerks.OneHanded.MilitaryTradition || perk == DefaultPerks.Crossbow.RenownMarksmen || perk == DefaultPerks.Steward.DrillSergant)
			{
				return MathF.Round(perk.PrimaryBonus);
			}
			if (perk == DefaultPerks.Polearm.Drills || perk == DefaultPerks.Athletics.WalkItOff || perk == DefaultPerks.Athletics.AGoodDaysRest || perk == DefaultPerks.Bow.Trainer || perk == DefaultPerks.Bow.BullsEye || perk == DefaultPerks.Throwing.Saddlebags)
			{
				return MathF.Round(perk.SecondaryBonus);
			}
			return 0;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x00082DF0 File Offset: 0x00080FF0
		public override int GenerateSharedXp(CharacterObject troop, int xp, MobileParty mobileParty)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)xp, false, null);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.LeaderOfMasses, mobileParty, false, ref explainedNumber);
			if (troop.IsRegular)
			{
				if (troop.IsMounted)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.LeadByExample, mobileParty, false, ref explainedNumber);
				}
				if (troop.IsRanged)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.MakeADifference, mobileParty, false, ref explainedNumber);
				}
			}
			return (int)(explainedNumber.ResultNumber - (float)xp);
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x00082E58 File Offset: 0x00081058
		public override ExplainedNumber CalculateXpGainFromBattles(FlattenedTroopRosterElement troopRosterElement, PartyBase party)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)troopRosterElement.XpGained, false, null);
			if ((party.MapEvent.IsPlayerSimulation || !party.MapEvent.IsPlayerMapEvent) && party.IsMobile)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.TrustedCommander, party.MobileParty, false, ref explainedNumber);
			}
			return explainedNumber;
		}
	}
}
