using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000417 RID: 1047
	public class HeroSpawnCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060042B5 RID: 17077 RVA: 0x00130AD8 File Offset: 0x0012ECD8
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
			CampaignEvents.OnGovernorChangedEvent.AddNonSerializedListener(this, new Action<Town, Hero, Hero>(this.OnGovernorChanged));
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnNonBanditClanDailyTick));
			CampaignEvents.HeroComesOfAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroComesOfAge));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroDailyTick));
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x00130B9D File Offset: 0x0012ED9D
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060042B7 RID: 17079 RVA: 0x00130BA0 File Offset: 0x0012EDA0
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int i)
		{
			if (i == 0)
			{
				int heroComesOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
				foreach (Clan clan in Clan.All)
				{
					foreach (Hero hero in clan.Heroes)
					{
						if (hero.Age >= (float)heroComesOfAge && hero.IsAlive && !hero.IsDisabled)
						{
							hero.ChangeState(Hero.CharacterStates.Active);
						}
					}
				}
			}
			int num = Clan.NonBanditFactions.Count<Clan>();
			int num2 = num / 100 + ((num % 100 > i) ? 1 : 0);
			int num3 = num / 100;
			for (int j = 0; j < i; j++)
			{
				num3 += ((num % 100 > j) ? 1 : 0);
			}
			for (int k = 0; k < num2; k++)
			{
				this.TrySpawnHeroesAndParties(Clan.NonBanditFactions.ElementAt<Clan>(num3 + k), true);
			}
		}

		// Token: 0x060042B8 RID: 17080 RVA: 0x00130CCC File Offset: 0x0012EECC
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
			foreach (Clan clan in Clan.NonBanditFactions)
			{
				if (!clan.IsEliminated && clan.IsMinorFaction && clan != Clan.PlayerClan)
				{
					this.SpawnMinorFactionHeroes(clan, true);
					this.CheckAndAssignClanLeader(clan);
					clan.ConsiderAndUpdateHomeSettlement();
				}
			}
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x00130D40 File Offset: 0x0012EF40
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.IsActive)
				{
					this.AssignSettlementToHeroOnGameStart(hero);
				}
			}
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x00130D9C File Offset: 0x0012EF9C
		private void OnHeroComesOfAge(Hero hero)
		{
			if (!hero.IsDisabled && hero.HeroState != Hero.CharacterStates.Active && !hero.IsTraveling)
			{
				hero.ChangeState(Hero.CharacterStates.Active);
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(hero, hero.HomeSettlement);
			}
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x00130DCC File Offset: 0x0012EFCC
		private void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			if (!companion.IsFugitive && !companion.IsDead && detail != RemoveCompanionAction.RemoveCompanionDetail.ByTurningToLord && detail != RemoveCompanionAction.RemoveCompanionDetail.Death && companion.DeathMark == KillCharacterAction.KillCharacterActionDetail.None)
			{
				Settlement settlement = this.FindASuitableSettlementToTeleportForHero(companion, 0f);
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(companion, settlement);
			}
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x00130E10 File Offset: 0x0012F010
		private void AssignSettlementToHeroOnGameStart(Hero hero)
		{
			if (hero.CurrentSettlement == null && hero.PartyBelongedTo == null && !hero.IsSpecial && hero.GovernorOf == null)
			{
				if (MobileParty.MainParty.MemberRoster.Contains(hero.CharacterObject))
				{
					MobileParty.MainParty.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
					MobileParty.MainParty.MemberRoster.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
					return;
				}
				Settlement settlement = this.FindASuitableSettlementToTeleportForHero(hero, 0f);
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(hero, settlement);
			}
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x00130EA8 File Offset: 0x0012F0A8
		private void OnHeroDailyTick(Hero hero)
		{
			Settlement settlement = null;
			if (hero.IsFugitive || hero.IsReleased)
			{
				if (!hero.IsSpecial && (hero.IsPlayerCompanion || MBRandom.RandomFloat < 0.3f || (hero.CurrentSettlement != null && hero.CurrentSettlement.MapFaction.IsAtWarWith(hero.MapFaction))))
				{
					settlement = this.FindASuitableSettlementToTeleportForHero(hero, 0f);
				}
			}
			else if (hero.IsActive && this.CanHeroMoveToAnotherSettlement(hero))
			{
				settlement = this.FindASuitableSettlementToTeleportForHero(hero, 10f);
			}
			if (settlement != null)
			{
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(hero, settlement);
				if (!hero.IsActive)
				{
					hero.ChangeState(Hero.CharacterStates.Active);
				}
			}
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x00130F49 File Offset: 0x0012F149
		private void OnNonBanditClanDailyTick(Clan clan)
		{
			this.TrySpawnHeroesAndParties(clan, false);
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x00130F53 File Offset: 0x0012F153
		private void TrySpawnHeroesAndParties(Clan clan, bool isNewGame)
		{
			if (!clan.IsEliminated && clan != Clan.PlayerClan)
			{
				if (clan.IsMinorFaction)
				{
					this.SpawnMinorFactionHeroes(clan, false);
				}
				this.ConsiderSpawningLordParties(clan, isNewGame);
			}
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x00130F80 File Offset: 0x0012F180
		private bool CanHeroMoveToAnotherSettlement(Hero hero)
		{
			if (hero.Clan != Clan.PlayerClan && !hero.IsTemplate && hero.IsAlive && !hero.IsNotable && !hero.IsHumanPlayerCharacter && !hero.IsPartyLeader && !hero.IsPrisoner && hero.HeroState != Hero.CharacterStates.Disabled && hero.GovernorOf == null && hero.PartyBelongedTo == null && !hero.IsWanderer && hero.PartyBelongedToAsPrisoner == null && hero.CharacterObject.Occupation != Occupation.Special && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				Settlement currentSettlement = hero.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town : null) == null || (!hero.CurrentSettlement.Town.HasTournament && !hero.CurrentSettlement.IsUnderSiege))
				{
					return hero.CanMoveToSettlement();
				}
			}
			return false;
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x00131074 File Offset: 0x0012F274
		private float GetHeroPartyCommandScore(Hero hero)
		{
			return 3f * (float)hero.GetSkillValue(DefaultSkills.Tactics) + 2f * (float)hero.GetSkillValue(DefaultSkills.Leadership) + (float)hero.GetSkillValue(DefaultSkills.Scouting) + (float)hero.GetSkillValue(DefaultSkills.Steward) + (float)hero.GetSkillValue(DefaultSkills.OneHanded) + (float)hero.GetSkillValue(DefaultSkills.TwoHanded) + (float)hero.GetSkillValue(DefaultSkills.Polearm) + (float)hero.GetSkillValue(DefaultSkills.Riding) + ((hero.Clan.Leader == hero) ? 1000f : 0f) + ((hero.GovernorOf == null) ? 500f : 0f) + (float)(hero.IsNoncombatant ? (-5000) : 0);
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x00131138 File Offset: 0x0012F338
		private void ConsiderSpawningLordParties(Clan clan, bool isNewGame)
		{
			int partyLimitForTier = Campaign.Current.Models.ClanTierModel.GetPartyLimitForTier(clan, clan.Tier);
			int count = clan.WarPartyComponents.Count;
			if (count >= partyLimitForTier)
			{
				return;
			}
			int num = partyLimitForTier - count;
			for (int i = 0; i < num; i++)
			{
				Hero bestAvailableCommander = this.GetBestAvailableCommander(clan);
				if (bestAvailableCommander == null)
				{
					break;
				}
				float num2 = this.CalculateScoreToCreateParty(clan);
				if (this.GetHeroPartyCommandScore(bestAvailableCommander) + num2 > 100f)
				{
					MobileParty mobileParty = this.SpawnLordParty(bestAvailableCommander, isNewGame);
					if (mobileParty != null)
					{
						this.GiveInitialItemsToParty(mobileParty);
					}
				}
			}
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x001311C4 File Offset: 0x0012F3C4
		private float CalculateScoreToCreateParty(Clan clan)
		{
			return (float)(clan.Fiefs.Count * 100 - clan.WarPartyComponents.Count * 100) + (float)clan.Gold * 0.01f + (clan.IsMinorFaction ? 200f : 0f) + ((clan.WarPartyComponents.Count > 0) ? 0f : 200f);
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x00131230 File Offset: 0x0012F430
		private Hero GetBestAvailableCommander(Clan clan)
		{
			Hero hero = null;
			float num = 0f;
			foreach (Hero hero2 in clan.Heroes)
			{
				if (hero2.IsActive && hero2.IsAlive && hero2.PartyBelongedTo == null && hero2.PartyBelongedToAsPrisoner == null && hero2.CanLeadParty() && hero2.Age > (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && hero2.CharacterObject.Occupation == Occupation.Lord)
				{
					float heroPartyCommandScore = this.GetHeroPartyCommandScore(hero2);
					if (heroPartyCommandScore > num)
					{
						num = heroPartyCommandScore;
						hero = hero2;
					}
				}
			}
			if (hero != null)
			{
				return hero;
			}
			if (clan != Clan.PlayerClan)
			{
				foreach (Hero hero3 in clan.Heroes)
				{
					if (hero3.IsActive && hero3.IsAlive && hero3.PartyBelongedTo == null && hero3.PartyBelongedToAsPrisoner == null && hero3.Age > (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && hero3.CharacterObject.Occupation == Occupation.Lord)
					{
						float heroPartyCommandScore2 = this.GetHeroPartyCommandScore(hero3);
						if (heroPartyCommandScore2 > num)
						{
							num = heroPartyCommandScore2;
							hero = hero3;
						}
					}
				}
			}
			return hero;
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x001313A0 File Offset: 0x0012F5A0
		private MobileParty SpawnLordParty(Hero hero, bool isNewGame)
		{
			if (hero.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(hero);
			}
			Settlement settlement = SettlementHelper.GetBestSettlementToSpawnAround(hero);
			if (settlement == null || settlement.MapFaction != hero.MapFaction)
			{
				settlement = hero.MapFaction.InitialHomeSettlement;
			}
			if (settlement == null)
			{
				settlement = Settlement.All.First<Settlement>((Settlement x) => x.Culture == hero.Culture);
			}
			MobileParty mobileParty = MobilePartyHelper.SpawnLordParty(hero, settlement.GatePosition, Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Default) / 2f);
			if (isNewGame)
			{
				int num = (int)((float)(mobileParty.Party.PartySizeLimit - mobileParty.MemberRoster.TotalManCount) * MBRandom.RandomFloatRanged(0.75f, 0.9f));
				PartyTemplateObject defaultPartyTemplate = mobileParty.LordPartyComponent.Owner.Clan.DefaultPartyTemplate;
				List<ValueTuple<CharacterObject, float>> list = new List<ValueTuple<CharacterObject, float>>();
				foreach (PartyTemplateStack partyTemplateStack in defaultPartyTemplate.Stacks)
				{
					list.Add(new ValueTuple<CharacterObject, float>(partyTemplateStack.Character, (float)(partyTemplateStack.MinValue + partyTemplateStack.MaxValue) / 2f));
				}
				for (int i = 0; i < num; i++)
				{
					CharacterObject characterObject = MBRandom.ChooseWeighted<CharacterObject>(list);
					mobileParty.AddElementToMemberRoster(characterObject, 1, false);
				}
			}
			return mobileParty;
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x0013151C File Offset: 0x0012F71C
		private void GiveInitialItemsToParty(MobileParty heroParty)
		{
			float num = 2f * Campaign.Current.EstimatedAverageLordPartySpeed * (float)CampaignTime.HoursInDay;
			foreach (Settlement settlement in Campaign.Current.Settlements)
			{
				if (settlement.IsVillage)
				{
					float num2;
					float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(heroParty, settlement, false, heroParty.NavigationCapability, out num2);
					if (distance < num)
					{
						foreach (ValueTuple<ItemObject, float> valueTuple in settlement.Village.VillageType.Productions)
						{
							ItemObject item = valueTuple.Item1;
							float item2 = valueTuple.Item2;
							float num3 = ((item.ItemType == ItemObject.ItemTypeEnum.Horse && item.HorseComponent.IsRideable && !item.HorseComponent.IsPackAnimal) ? 7f : (item.IsFood ? 0.1f : 0f));
							float num4 = ((float)heroParty.MemberRoster.TotalManCount + 2f) / 200f;
							float num5 = 1f - distance / num;
							int num6 = MBRandom.RoundRandomized(num3 * item2 * num5 * num4);
							if (num6 > 0)
							{
								heroParty.ItemRoster.AddToCounts(item, num6);
							}
						}
					}
				}
			}
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x001316BC File Offset: 0x0012F8BC
		private void CheckAndAssignClanLeader(Clan clan)
		{
			if (clan.Leader == null || clan.Leader.IsDead)
			{
				Hero hero = clan.AliveLords.FirstOrDefault<Hero>();
				if (hero != null)
				{
					clan.SetLeader(hero);
					return;
				}
				Debug.FailedAssert("Cant find a lord to assign as leader to minor faction.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\HeroSpawnCampaignBehavior.cs", "CheckAndAssignClanLeader", 438);
			}
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x0013170E File Offset: 0x0012F90E
		private void CreateMinorFactionHeroFromTemplate(CharacterObject template, Clan faction)
		{
			Hero hero = HeroCreator.CreateSpecialHero(template, null, faction, null, Campaign.Current.GameStarted ? 19 : (-1));
			hero.ChangeState(Campaign.Current.GameStarted ? Hero.CharacterStates.Active : Hero.CharacterStates.NotSpawned);
			hero.IsMinorFactionHero = true;
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x00131748 File Offset: 0x0012F948
		private void SpawnMinorFactionHeroes(Clan clan, bool firstTime)
		{
			int num = Campaign.Current.Models.MinorFactionsModel.MinorFactionHeroLimit - clan.AliveLords.Count;
			if (num > 0)
			{
				if (firstTime)
				{
					int num2 = 0;
					while (num2 < clan.MinorFactionCharacterTemplates.Count && num > 0)
					{
						CharacterObject characterObject = clan.MinorFactionCharacterTemplates[num2];
						this.CreateMinorFactionHeroFromTemplate(characterObject, clan);
						num--;
						num2++;
					}
				}
				if (num > 0)
				{
					if (clan.MinorFactionCharacterTemplates == null || clan.MinorFactionCharacterTemplates.IsEmpty<CharacterObject>())
					{
						Debug.FailedAssert(string.Format("{0} templates are empty!", clan.Name), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\HeroSpawnCampaignBehavior.cs", "SpawnMinorFactionHeroes", 475);
						return;
					}
					for (int i = 0; i < num; i++)
					{
						if (MBRandom.RandomFloat < Campaign.Current.Models.MinorFactionsModel.DailyMinorFactionHeroSpawnChance)
						{
							CharacterObject randomElementInefficiently = clan.MinorFactionCharacterTemplates.GetRandomElementInefficiently<CharacterObject>();
							this.CreateMinorFactionHeroFromTemplate(randomElementInefficiently, clan);
						}
					}
				}
			}
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x00131830 File Offset: 0x0012FA30
		private void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
			if (oldGovernor != null && oldGovernor.Clan != null)
			{
				foreach (Hero hero in oldGovernor.Clan.Heroes)
				{
					hero.UpdateHomeSettlement();
				}
			}
			if (newGovernor != null && newGovernor.Clan != null && (oldGovernor == null || newGovernor.Clan != oldGovernor.Clan))
			{
				foreach (Hero hero2 in newGovernor.Clan.Heroes)
				{
					hero2.UpdateHomeSettlement();
				}
			}
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x001318F0 File Offset: 0x0012FAF0
		private Settlement FindASuitableSettlementToTeleportForHero(Hero hero, float minimumScore = 0f)
		{
			Settlement settlement;
			if (hero.IsNotable)
			{
				Debug.FailedAssert("Notables should not use here anymore, make sure this case is intended", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\HeroSpawnCampaignBehavior.cs", "FindASuitableSettlementToTeleportForHero", 519);
				settlement = hero.BornSettlement;
			}
			else
			{
				List<ValueTuple<Settlement, float>> list = new List<ValueTuple<Settlement, float>>(hero.MapFaction.Fiefs.Count);
				foreach (Town town in hero.MapFaction.Fiefs)
				{
					float moveScoreForHero = this.GetMoveScoreForHero(hero, town);
					list.Add(new ValueTuple<Settlement, float>(town.Settlement, (moveScoreForHero >= minimumScore) ? moveScoreForHero : 0f));
				}
				settlement = MBRandom.ChooseWeighted<Settlement>(list);
				if (settlement == null)
				{
					List<Settlement> list2 = new List<Settlement>();
					List<Settlement> list3 = new List<Settlement>();
					foreach (Town town2 in Town.AllFiefs)
					{
						if (town2.MapFaction.IsAtWarWith(hero.MapFaction))
						{
							if (town2.IsTown)
							{
								list3.Add(town2.Settlement);
							}
						}
						else
						{
							list2.Add(town2.Settlement);
						}
					}
					foreach (Settlement settlement2 in list2)
					{
						float moveScoreForHero2 = this.GetMoveScoreForHero(hero, settlement2.Town);
						list.Add(new ValueTuple<Settlement, float>(settlement2, (moveScoreForHero2 >= minimumScore) ? moveScoreForHero2 : 0f));
					}
					settlement = MBRandom.ChooseWeighted<Settlement>(list);
					if (settlement == null)
					{
						list.Clear();
						foreach (Settlement settlement3 in list3)
						{
							float moveScoreForHero3 = this.GetMoveScoreForHero(hero, settlement3.Town);
							list.Add(new ValueTuple<Settlement, float>(settlement3, (moveScoreForHero3 >= minimumScore) ? moveScoreForHero3 : 0f));
						}
						settlement = MBRandom.ChooseWeighted<Settlement>(list);
					}
				}
			}
			return settlement;
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x00131B20 File Offset: 0x0012FD20
		private float GetMoveScoreForHero(Hero hero, Town fief)
		{
			Clan clan = hero.Clan;
			float num = 1E-06f;
			if (!fief.IsUnderSiege && !fief.MapFaction.IsAtWarWith(hero.MapFaction) && (!fief.IsCastle || hero.Occupation != Occupation.Wanderer || hero.IsPlayerCompanion))
			{
				num = (DiplomacyHelper.IsSameFactionAndNotEliminated(fief.MapFaction, hero.MapFaction) ? 0.01f : 1E-05f);
				if (fief.MapFaction == hero.MapFaction)
				{
					num += 10f;
					if (fief.IsTown)
					{
						num += 100f;
					}
					if (fief.OwnerClan == clan)
					{
						num += (fief.IsTown ? 500f : 100f);
					}
					if (fief.HasTournament)
					{
						num += 400f;
					}
				}
				foreach (Hero hero2 in fief.Settlement.HeroesWithoutParty)
				{
					if (clan != null && hero2.Clan == clan)
					{
						num += (fief.IsTown ? 100f : 10f);
					}
				}
				if (hero.IsFugitive)
				{
					Settlement homeSettlement = hero.HomeSettlement;
					if (((homeSettlement != null) ? homeSettlement.Town : null) == fief)
					{
						num += 100f;
					}
				}
				if (fief.Settlement.IsStarving)
				{
					num *= 0.1f;
				}
				if (hero.CurrentSettlement == fief.Settlement)
				{
					num *= 3f;
				}
			}
			return num;
		}

		// Token: 0x040013E0 RID: 5088
		private const float MinimumScoreForSafeSettlement = 10f;
	}
}
