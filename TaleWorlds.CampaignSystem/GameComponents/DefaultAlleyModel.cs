using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F5 RID: 245
	public class DefaultAlleyModel : AlleyModel
	{
		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001689 RID: 5769 RVA: 0x0006736E File Offset: 0x0006556E
		private CharacterObject _thug
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("gangster_1");
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x0600168A RID: 5770 RVA: 0x0006737F File Offset: 0x0006557F
		private CharacterObject _expertThug
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("gangster_2");
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x0600168B RID: 5771 RVA: 0x00067390 File Offset: 0x00065590
		private CharacterObject _masterThug
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("gangster_3");
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600168C RID: 5772 RVA: 0x000673A1 File Offset: 0x000655A1
		public override CampaignTime DestroyAlleyAfterDaysWhenLeaderIsDeath
		{
			get
			{
				return CampaignTime.Days(4f);
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600168D RID: 5773 RVA: 0x000673AD File Offset: 0x000655AD
		public override int MinimumTroopCountInPlayerOwnedAlley
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600168E RID: 5774 RVA: 0x000673B0 File Offset: 0x000655B0
		public override int MaximumTroopCountInPlayerOwnedAlley
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600168F RID: 5775 RVA: 0x000673B4 File Offset: 0x000655B4
		public override float GetDailyCrimeRatingOfAlley
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x000673BB File Offset: 0x000655BB
		public override float GetDailyXpGainForAssignedClanMember(Hero assignedHero)
		{
			return 200f;
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x000673C2 File Offset: 0x000655C2
		public override float GetDailyXpGainForMainHero()
		{
			return 40f;
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x000673C9 File Offset: 0x000655C9
		public override float GetInitialXpGainForMainHero()
		{
			return 1500f;
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x000673D0 File Offset: 0x000655D0
		public override float GetXpGainAfterSuccessfulAlleyDefenseForMainHero()
		{
			return 6000f;
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x000673D7 File Offset: 0x000655D7
		public override TroopRoster GetTroopsOfAIOwnedAlley(Alley alley)
		{
			return this.GetTroopsOfAlleyInternal(alley);
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x000673E0 File Offset: 0x000655E0
		public override TroopRoster GetTroopsOfAlleyForBattleMission(Alley alley)
		{
			TroopRoster troopsOfAlleyInternal = this.GetTroopsOfAlleyInternal(alley);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (TroopRosterElement troopRosterElement in troopsOfAlleyInternal.GetTroopRoster())
			{
				troopRoster.AddToCounts(troopRosterElement.Character, troopRosterElement.Number * 2, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x00067454 File Offset: 0x00065654
		private TroopRoster GetTroopsOfAlleyInternal(Alley alley)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			Hero owner = alley.Owner;
			if (owner.Power <= 100f)
			{
				if ((float)owner.RandomValue > 0.5f)
				{
					troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
				}
				else
				{
					troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 1, false, 0, 0, true, -1);
				}
			}
			else if (owner.Power <= 200f)
			{
				if ((float)owner.RandomValue > 0.5f)
				{
					troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 1, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 2, false, 0, 0, true, -1);
				}
				else
				{
					troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 2, false, 0, 0, true, -1);
				}
			}
			else if (owner.Power <= 300f)
			{
				if ((float)owner.RandomValue > 0.5f)
				{
					troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 2, false, 0, 0, true, -1);
				}
				else
				{
					troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 3, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 3, false, 0, 0, true, -1);
				}
			}
			else if ((float)owner.RandomValue > 0.5f)
			{
				troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._expertThug, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._masterThug, 3, false, 0, 0, true, -1);
			}
			else
			{
				troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._expertThug, 4, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._masterThug, 4, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x00067684 File Offset: 0x00065884
		public override List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>> GetClanMembersAndAvailabilityDetailsForLeadingAnAlley(Alley alley)
		{
			List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>> list = new List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>>();
			foreach (Hero hero in Clan.PlayerClan.AliveLords)
			{
				if (hero != Hero.MainHero)
				{
					list.Add(new ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>(hero, this.GetAvailability(alley, hero)));
				}
			}
			foreach (Hero hero2 in Clan.PlayerClan.Companions)
			{
				if (hero2 != Hero.MainHero && !hero2.IsDead)
				{
					list.Add(new ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>(hero2, this.GetAvailability(alley, hero2)));
				}
			}
			return list;
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x0006775C File Offset: 0x0006595C
		public override TroopRoster GetTroopsToRecruitFromAlleyDependingOnAlleyRandom(Alley alley, float random)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			if (random >= 0.5f)
			{
				return troopRoster;
			}
			Clan relatedBanditClanDependingOnAlleySettlementFaction = this.GetRelatedBanditClanDependingOnAlleySettlementFaction(alley);
			if (random > 0.3f)
			{
				troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 1, false, 0, 0, true, -1);
			}
			else if (random > 0.15f)
			{
				troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 1, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop.UpgradeTargets[0], 1, false, 0, 0, true, -1);
			}
			else if (random > 0.05f)
			{
				troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 2, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop.UpgradeTargets[0], 1, false, 0, 0, true, -1);
			}
			else
			{
				troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop.UpgradeTargets[0], 3, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x0006788C File Offset: 0x00065A8C
		public override TextObject GetDisabledReasonTextForHero(Hero hero, Alley alley, DefaultAlleyModel.AlleyMemberAvailabilityDetail detail)
		{
			switch (detail)
			{
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Available:
				return TextObject.GetEmpty();
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.AvailableWithDelay:
			{
				TextObject textObject = new TextObject("{=dgUF5awO}It will take {HOURS} {?HOURS > 1}hours{?}hour{\\?} for this clan member to arrive.", null);
				textObject.SetTextVariable("HOURS", (int)Math.Ceiling((double)Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(hero, alley.Settlement.Party).ResultNumber));
				return textObject;
			}
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughRoguerySkill:
			{
				TextObject textObject2 = GameTexts.FindText("str_character_role_disabled_tooltip", null);
				textObject2.SetTextVariable("SKILL_NAME", DefaultSkills.Roguery.Name.ToString());
				textObject2.SetTextVariable("MIN_SKILL_AMOUNT", 30);
				return textObject2;
			}
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughMercyTrait:
			{
				TextObject textObject3 = GameTexts.FindText("str_hero_needs_trait_tooltip", null);
				textObject3.SetTextVariable("TRAIT_NAME", DefaultTraits.Mercy.Name.ToString());
				textObject3.SetTextVariable("MAX_TRAIT_AMOUNT", 0);
				return textObject3;
			}
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.CanNotLeadParty:
				return new TextObject("{=qClVr2ka}This hero cannot lead a party.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlreadyAlleyLeader:
				return GameTexts.FindText("str_hero_is_already_alley_leader", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Prisoner:
				return new TextObject("{=qhRC8XWU}This hero is currently prisoner.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.SolvingIssue:
				return new TextObject("{=nT6EQGf9}This hero is currently solving an issue.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Traveling:
				return new TextObject("{=WECWpVSw}This hero is currently traveling.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Busy:
				return new TextObject("{=c9iu5lcc}This hero is currently busy.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Fugutive:
				return new TextObject("{=eZYtkDff}This hero is currently fugutive.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Governor:
				return new TextObject("{=8NI4wrqU}This hero is currently assigned as a governor.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlleyUnderAttack:
				return new TextObject("{=pdqi2qz1}You can not do this action while your alley is under attack.", null);
			default:
				return TextObject.GetEmpty();
			}
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00067A00 File Offset: 0x00065C00
		public override float GetAlleyAttackResponseTimeInDays(TroopRoster troopRoster)
		{
			float num = 0f;
			foreach (TroopRosterElement troopRosterElement in troopRoster.GetTroopRoster())
			{
				num += (((float)troopRosterElement.Character.Tier > 4f) ? 4f : ((float)troopRosterElement.Character.Tier)) * (float)troopRosterElement.Number;
			}
			return (float)Math.Min(12, 8 + (int)(num / 8f));
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x00067A98 File Offset: 0x00065C98
		private Clan GetRelatedBanditClanDependingOnAlleySettlementFaction(Alley alley)
		{
			string stringId = alley.Settlement.Culture.StringId;
			Clan clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
			if (stringId == "khuzait")
			{
				clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "steppe_bandits");
			}
			else if (stringId == "vlandia" || stringId.Contains("empire"))
			{
				clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
			}
			else if (stringId == "aserai")
			{
				clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "desert_bandits");
			}
			else if (stringId == "battania")
			{
				clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "forest_bandits");
			}
			else if (stringId == "sturgia" || stringId == "nord")
			{
				clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "sea_raiders");
			}
			return clan;
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00067C1C File Offset: 0x00065E1C
		private DefaultAlleyModel.AlleyMemberAvailabilityDetail GetAvailability(Alley alley, Hero hero)
		{
			IAlleyCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IAlleyCampaignBehavior>();
			if (alley.Owner == Hero.MainHero && campaignBehavior != null && campaignBehavior.GetIsPlayerAlleyUnderAttack(alley))
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlleyUnderAttack;
			}
			if (hero.GetSkillValue(DefaultSkills.Roguery) < 30)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughRoguerySkill;
			}
			if (hero.GetTraitLevel(DefaultTraits.Mercy) > 0)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughMercyTrait;
			}
			if (campaignBehavior != null && campaignBehavior.GetAllAssignedClanMembersForOwnedAlleys().Contains(hero))
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlreadyAlleyLeader;
			}
			if (hero.GovernorOf != null)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Governor;
			}
			if (!hero.CanLeadParty())
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.CanNotLeadParty;
			}
			if (Campaign.Current.IssueManager.IssueSolvingCompanionList.Contains(hero))
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.SolvingIssue;
			}
			if (hero.IsFugitive)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Fugutive;
			}
			if (hero.IsTraveling)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Traveling;
			}
			if (hero.IsPrisoner)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Prisoner;
			}
			if (!hero.IsActive)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Busy;
			}
			if (hero.IsPartyLeader)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Busy;
			}
			if (Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(hero, alley.Settlement.Party).BaseNumber > 0f)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.AvailableWithDelay;
			}
			return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Available;
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x00067D19 File Offset: 0x00065F19
		public override int GetDailyIncomeOfAlley(Alley alley)
		{
			return (int)(alley.Settlement.Town.Prosperity / 50f);
		}

		// Token: 0x04000771 RID: 1905
		private const int BaseResponseTimeInDays = 8;

		// Token: 0x04000772 RID: 1906
		private const int MaxResponseTimeInDays = 12;

		// Token: 0x04000773 RID: 1907
		public const int MinimumRoguerySkillNeededForLeadingAnAlley = 30;

		// Token: 0x04000774 RID: 1908
		public const int MaximumMercyTraitNeededForLeadingAnAlley = 0;

		// Token: 0x02000599 RID: 1433
		public enum AlleyMemberAvailabilityDetail
		{
			// Token: 0x0400186F RID: 6255
			Available,
			// Token: 0x04001870 RID: 6256
			AvailableWithDelay,
			// Token: 0x04001871 RID: 6257
			NotEnoughRoguerySkill,
			// Token: 0x04001872 RID: 6258
			NotEnoughMercyTrait,
			// Token: 0x04001873 RID: 6259
			CanNotLeadParty,
			// Token: 0x04001874 RID: 6260
			AlreadyAlleyLeader,
			// Token: 0x04001875 RID: 6261
			Prisoner,
			// Token: 0x04001876 RID: 6262
			SolvingIssue,
			// Token: 0x04001877 RID: 6263
			Traveling,
			// Token: 0x04001878 RID: 6264
			Busy,
			// Token: 0x04001879 RID: 6265
			Fugutive,
			// Token: 0x0400187A RID: 6266
			Governor,
			// Token: 0x0400187B RID: 6267
			AlleyUnderAttack
		}
	}
}
