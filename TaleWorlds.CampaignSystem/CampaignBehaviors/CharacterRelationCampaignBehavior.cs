using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003FB RID: 1019
	public class CharacterRelationCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003F6F RID: 16239 RVA: 0x0011094C File Offset: 0x0010EB4C
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.UpdateFriendshipAndEnemies));
			CampaignEvents.RaidCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, RaidEventComponent>(this.OnRaidCompleted));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnPrisonerDonatedToSettlementEvent.AddNonSerializedListener(this, new Action<MobileParty, FlattenedTroopRoster, Settlement>(this.OnPrisonerDonatedToSettlement));
			CampaignEvents.HeroRelationChanged.AddNonSerializedListener(this, new Action<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero>(this.OnHeroRelationChanged));
			CampaignEvents.BeforeHeroesMarried.AddNonSerializedListener(this, new Action<Hero, Hero, bool>(CharacterRelationCampaignBehavior.OnHeroesMarried));
			CampaignEvents.OnHeroUnregisteredEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroUnregistered));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x00110A56 File Offset: 0x0010EC56
		private void OnHeroUnregistered(Hero hero)
		{
			Campaign.Current.CharacterRelationManager.RemoveHero(hero);
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x00110A68 File Offset: 0x0010EC68
		private void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			if (relationChange > 0)
			{
				SkillLevelingManager.OnGainRelation(originalHero, effectiveHeroGainedRelationWith, (float)relationChange, detail);
			}
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x00110A7C File Offset: 0x0010EC7C
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.HasWinner)
			{
				MapEventSide winnerSide = mapEvent.Winner;
				MapEventSide otherSide = winnerSide.OtherSide;
				if (mapEvent.EventType == MapEvent.BattleTypes.FieldBattle || mapEvent.EventType == MapEvent.BattleTypes.Siege || mapEvent.EventType == MapEvent.BattleTypes.SiegeOutside)
				{
					bool flag = false;
					foreach (MapEventParty mapEventParty in otherSide.Parties)
					{
						if (mapEventParty.Party.IsMobile && mapEventParty.Party.MobileParty.IsLordParty)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						Hero leaderHero = winnerSide.LeaderParty.LeaderHero;
						if (leaderHero != null && leaderHero.GetPerkValue(DefaultPerks.Charm.Oratory))
						{
							Hero randomElementWithPredicate = Hero.AllAliveHeroes.GetRandomElementWithPredicate<Hero>(delegate(Hero x)
							{
								if (x.IsActive && x.IsNotable)
								{
									Settlement currentSettlement = x.CurrentSettlement;
									return ((currentSettlement != null) ? currentSettlement.MapFaction : null) == winnerSide.LeaderParty.MapFaction;
								}
								return false;
							});
							if (randomElementWithPredicate != null)
							{
								int num = (int)DefaultPerks.Charm.Oratory.SecondaryBonus;
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(winnerSide.LeaderParty.LeaderHero, randomElementWithPredicate, num, true);
							}
						}
						Hero leaderHero2 = winnerSide.LeaderParty.LeaderHero;
						if (leaderHero2 != null && leaderHero2.GetPerkValue(DefaultPerks.Charm.Warlord))
						{
							Hero randomElementWithPredicate2 = winnerSide.LeaderParty.MapFaction.AliveLords.GetRandomElementWithPredicate<Hero>((Hero x) => x != winnerSide.LeaderParty.LeaderHero);
							if (randomElementWithPredicate2 != null)
							{
								int num2 = (int)DefaultPerks.Charm.Warlord.SecondaryBonus;
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(winnerSide.LeaderParty.LeaderHero, randomElementWithPredicate2, num2, true);
							}
						}
					}
				}
				int num3 = winnerSide.CalculateTotalContribution();
				if (num3 > 0)
				{
					MBReadOnlyList<MapEventParty> parties = winnerSide.Parties;
					List<MobileParty> list = new List<MobileParty>();
					List<MobileParty> list2 = new List<MobileParty>();
					foreach (MapEventParty mapEventParty2 in parties)
					{
						PartyBase party = mapEventParty2.Party;
						if (party.IsMobile)
						{
							if (party.MobileParty.IsVillager)
							{
								list.Add(party.MobileParty);
							}
							else if (party.MobileParty.IsCaravan)
							{
								list2.Add(party.MobileParty);
							}
						}
					}
					foreach (MapEventParty mapEventParty3 in parties)
					{
						PartyBase party2 = mapEventParty3.Party;
						if (party2.LeaderHero != null)
						{
							float num4 = (float)mapEventParty3.ContributionToBattle / (float)num3;
							if (num4 > 0f)
							{
								if (mapEvent.EventType == MapEvent.BattleTypes.Raid && winnerSide.MissionSide == BattleSideEnum.Defender && mapEvent.MapEventSettlement.Notables.Count > 0)
								{
									ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mapEvent.MapEventSettlement.Notables.GetRandomElement<Hero>(), party2.LeaderHero, 5, true);
								}
								foreach (MobileParty mobileParty in list)
								{
									if (mobileParty.HomeSettlement.OwnerClan != party2.LeaderHero.Clan && !mobileParty.HomeSettlement.OwnerClan.IsEliminated && !party2.LeaderHero.Clan.IsEliminated)
									{
										int num5 = MBRandom.RoundRandomized(4f * num4);
										if (num5 > 0)
										{
											ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mobileParty.HomeSettlement.OwnerClan.Leader, party2.LeaderHero.Clan.Leader, num5, true);
										}
										int num6 = MBRandom.RoundRandomized(2f * num4);
										if (num6 > 0)
										{
											foreach (Hero hero in mobileParty.HomeSettlement.Notables)
											{
												ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, party2.LeaderHero.Clan.Leader, num6, true);
											}
										}
									}
								}
								foreach (MobileParty mobileParty2 in list2)
								{
									if (mobileParty2.HomeSettlement != null && mobileParty2.HomeSettlement.OwnerClan != null && party2.LeaderHero != null && mobileParty2.HomeSettlement.OwnerClan.Leader.Clan != party2.LeaderHero.Clan && mobileParty2.Party.Owner != null && mobileParty2.Party.Owner != Hero.MainHero && mobileParty2.Party.Owner.IsAlive && party2.LeaderHero.Clan.Leader != null && party2.LeaderHero.Clan.Leader.IsAlive && !mobileParty2.IsCurrentlyUsedByAQuest)
									{
										int num7 = MBRandom.RoundRandomized(6f * num4);
										ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mobileParty2.Party.Owner, party2.LeaderHero.Clan.Leader, num7, true);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x00111028 File Offset: 0x0010F228
		private void OnPrisonerDonatedToSettlement(MobileParty donatingParty, FlattenedTroopRoster donatedPrisoners, Settlement donatedSettlement)
		{
			if (donatingParty.IsMainParty)
			{
				foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in donatedPrisoners)
				{
					if (flattenedTroopRosterElement.Troop.IsHero)
					{
						float num = Campaign.Current.Models.PrisonerDonationModel.CalculateRelationGainAfterHeroPrisonerDonate(donatingParty.Party, flattenedTroopRosterElement.Troop.HeroObject, donatedSettlement);
						if (num != 0f)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, donatedSettlement.OwnerClan.Leader, (int)num, true);
						}
					}
				}
			}
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x001110C8 File Offset: 0x0010F2C8
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x001110CC File Offset: 0x0010F2CC
		private void UpdateFriendshipAndEnemies(CampaignGameStarter campaignGameStarter)
		{
			List<Hero> list = new List<Hero>(Hero.AllAliveHeroes.Count + Hero.DeadOrDisabledHeroes.Count);
			foreach (Hero hero in Campaign.Current.AliveHeroes)
			{
				if (hero.IsLord && hero != Hero.MainHero && hero.MapFaction != null)
				{
					IFaction mapFaction = hero.MapFaction;
					if (((mapFaction != null) ? mapFaction.Leader : null) != Hero.MainHero)
					{
						list.Add(hero);
					}
				}
			}
			foreach (Hero hero2 in Campaign.Current.DeadOrDisabledHeroes)
			{
				if (hero2.IsLord && hero2 != Hero.MainHero && hero2.MapFaction != null)
				{
					IFaction mapFaction2 = hero2.MapFaction;
					if (((mapFaction2 != null) ? mapFaction2.Leader : null) != Hero.MainHero)
					{
						list.Add(hero2);
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				Hero hero3 = list[i];
				for (int j = i + 1; j < list.Count; j++)
				{
					Hero hero4 = list[j];
					if ((!hero4.IsDead || !(hero4.DeathDay < hero3.BirthDay)) && (!hero3.IsDead || !(hero3.DeathDay < hero4.BirthDay)))
					{
						float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(hero3.MapFaction.FactionMidSettlement, hero4.MapFaction.FactionMidSettlement, false, false, MobileParty.NavigationType.All);
						float num = 1f / (2f + 5f * (distance / Campaign.Current.Models.MapDistanceModel.GetMaximumDistanceBetweenTwoConnectedSettlements(MobileParty.NavigationType.All)));
						if (hero3 == hero3.MapFaction.Leader || hero4 == hero4.MapFaction.Leader)
						{
							num = MathF.Sqrt(num);
						}
						if ((hero3.Clan != null && hero3.Clan.Tier >= 5 && hero4.Clan != null && hero4.Clan.Tier >= 5) || (hero3.IsKingdomLeader && hero4.IsKingdomLeader))
						{
							num = 1f;
						}
						if (MBRandom.RandomFloat < num)
						{
							float num2 = 0f;
							int num3 = HeroHelper.NPCPersonalityClashWithNPC(hero3, hero4);
							if (hero3.IsKingdomLeader && hero4.IsKingdomLeader)
							{
								if (hero3.Culture == hero4.Culture)
								{
									hero3.SetPersonalRelation(hero4, MathF.Round(MBRandom.RandomFloatRanged(35f, 100f)) * -1);
									goto IL_03A3;
								}
								if (num3 == 0)
								{
									num2 = (float)(((double)MBRandom.RandomFloat > 0.5) ? MathF.Round(MBRandom.RandomFloatRanged(-100f, -35f)) : MathF.Round(MBRandom.RandomFloatRanged(35f, 100f)));
								}
								else
								{
									for (int k = 0; k < 4; k++)
									{
										num2 += MBRandom.RandomFloat * 2f - 1f;
									}
									num2 = MBMath.ClampFloat(num2 * 30f, -100f, 100f);
								}
							}
							else
							{
								for (int l = 0; l < 4; l++)
								{
									num2 += MBRandom.RandomFloat * 2f - 1f;
								}
								num2 = MBMath.ClampFloat(num2 * 30f, -100f, 100f);
							}
							if (num3 == 0)
							{
								hero3.SetPersonalRelation(hero4, MathF.Round(num2));
							}
							else if (num3 < 0)
							{
								hero3.SetPersonalRelation(hero4, MathF.Abs(MathF.Round(num2)) * -1);
							}
							else
							{
								hero3.SetPersonalRelation(hero4, MathF.Abs(MathF.Round(num2)));
							}
						}
					}
					IL_03A3:;
				}
			}
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x001114C0 File Offset: 0x0010F6C0
		private void DailyTickParty(MobileParty mobileParty)
		{
			if (mobileParty.LeaderHero != null)
			{
				Settlement currentSettlement = mobileParty.CurrentSettlement;
				if (currentSettlement != null && currentSettlement.IsTown && mobileParty.CurrentSettlement.SiegeEvent == null)
				{
					if (mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Medicine.BestMedicine))
					{
						Hero randomElementWithPredicate = mobileParty.CurrentSettlement.Notables.GetRandomElementWithPredicate<Hero>((Hero x) => x.Age >= 40f && x.IsAlive);
						if (randomElementWithPredicate != null)
						{
							int num = (int)DefaultPerks.Medicine.BestMedicine.SecondaryBonus;
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mobileParty.LeaderHero, randomElementWithPredicate, num, true);
						}
					}
					if (mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Medicine.GoodLogdings))
					{
						Hero randomElement = TownHelpers.GetHeroesInSettlement(mobileParty.CurrentSettlement, (Hero x) => x.Age >= 40f && x != mobileParty.LeaderHero && x.IsLord).GetRandomElement<Hero>();
						if (randomElement != null)
						{
							int num2 = (int)DefaultPerks.Medicine.GoodLogdings.SecondaryBonus;
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mobileParty.LeaderHero, randomElement, num2, true);
						}
					}
				}
				if (mobileParty.Army != null && MBRandom.RandomFloat < DefaultPerks.Charm.Parade.SecondaryBonus && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Charm.Parade))
				{
					Func<TroopRosterElement, bool> <>9__3;
					MobileParty randomElementWithPredicate2 = mobileParty.Army.Parties.GetRandomElementWithPredicate<MobileParty>(delegate(MobileParty x)
					{
						List<TroopRosterElement> troopRoster = x.MemberRoster.GetTroopRoster();
						Func<TroopRosterElement, bool> func;
						if ((func = <>9__3) == null)
						{
							func = (<>9__3 = (TroopRosterElement y) => y.Character.IsHero && y.Character.Occupation == Occupation.Lord && y.Character.HeroObject != mobileParty.LeaderHero);
						}
						return troopRoster.AnyQ<TroopRosterElement>(func);
					});
					if (randomElementWithPredicate2 != null)
					{
						CharacterObject character = randomElementWithPredicate2.MemberRoster.GetTroopRoster().GetRandomElementWithPredicate<TroopRosterElement>((TroopRosterElement x) => x.Character.IsHero && x.Character.Occupation == Occupation.Lord && x.Character.HeroObject != mobileParty.LeaderHero).Character;
						Hero hero = ((character != null) ? character.HeroObject : null);
						if (hero != null)
						{
							int num3 = 1;
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(mobileParty.LeaderHero, hero, num3, true);
						}
					}
				}
			}
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x00111694 File Offset: 0x0010F894
		private void DailyTick()
		{
			if (Settlement.CurrentSettlement != null && Hero.MainHero.GetPerkValue(DefaultPerks.Charm.ForgivableGrievances) && MBRandom.RandomFloat < DefaultPerks.Charm.ForgivableGrievances.SecondaryBonus)
			{
				MBList<Hero> mblist = new MBList<Hero>();
				foreach (Hero hero in SettlementHelper.GetAllHeroesOfSettlement(Settlement.CurrentSettlement, true))
				{
					if (!hero.IsHumanPlayerCharacter && hero.GetRelationWithPlayer() < 0f)
					{
						mblist.Add(hero);
					}
				}
				if (mblist.Count > 0)
				{
					ChangeRelationAction.ApplyPlayerRelation(mblist.GetRandomElement<Hero>(), 1, true, true);
				}
			}
			SettlementLoyaltyModel settlementLoyaltyModel = Campaign.Current.Models.SettlementLoyaltyModel;
			SettlementSecurityModel settlementSecurityModel = Campaign.Current.Models.SettlementSecurityModel;
			bool flag = false;
			bool flag2 = false;
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsTown)
				{
					if (settlement.Town.Security >= (float)settlementSecurityModel.ThresholdForNotableRelationBonus)
					{
						using (List<Hero>.Enumerator enumerator3 = settlement.Notables.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Hero hero2 = enumerator3.Current;
								if ((hero2.IsArtisan || hero2.IsMerchant) && MBRandom.RandomFloat < 0.05f)
								{
									ChangeRelationAction.ApplyRelationChangeBetweenHeroes(settlement.OwnerClan.Leader, hero2, settlementSecurityModel.DailyNotableRelationBonus, false);
									flag2 = flag2 || settlement.OwnerClan.Leader.IsHumanPlayerCharacter;
								}
							}
							continue;
						}
					}
					if (settlement.Town.Security >= (float)settlementSecurityModel.ThresholdForNotableRelationPenalty)
					{
						continue;
					}
					foreach (Hero hero3 in settlement.Notables)
					{
						if ((hero3.IsArtisan || hero3.IsMerchant) && MBRandom.RandomFloat < 0.05f)
						{
							hero3.AddPower((float)settlementSecurityModel.DailyNotablePowerPenalty);
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(settlement.OwnerClan.Leader, hero3, settlementSecurityModel.DailyNotableRelationPenalty, false);
						}
					}
					using (List<Hero>.Enumerator enumerator3 = settlement.Notables.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							Hero hero4 = enumerator3.Current;
							if (hero4.IsGangLeader && MBRandom.RandomFloat < 0.05f)
							{
								hero4.AddPower((float)settlementSecurityModel.DailyNotablePowerBonus);
							}
						}
						continue;
					}
				}
				if (settlement.IsVillage && settlement.Village.Bound.Town.Loyalty >= settlementLoyaltyModel.ThresholdForNotableRelationBonus)
				{
					foreach (Hero hero5 in settlement.Notables)
					{
						if ((hero5.IsHeadman || hero5.IsRuralNotable) && MBRandom.RandomFloat < 0.05f)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(settlement.OwnerClan.Leader, hero5, settlementLoyaltyModel.DailyNotableRelationBonus, false);
							flag = flag || settlement.OwnerClan.Leader.IsHumanPlayerCharacter;
						}
					}
				}
			}
			if (flag2)
			{
				InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=ME5hmllb}Your relation with notables in some of your settlements increased due to high security", null).ToString()));
			}
			if (flag)
			{
				InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=0h5BrVdA}Your relation with notables in some of your settlements increased due to high loyalty", null).ToString()));
			}
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x00111AAC File Offset: 0x0010FCAC
		public void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if ((detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege || detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByBarter) && oldOwner != null && oldOwner.MapFaction != null && oldOwner.MapFaction.Leader != oldOwner && oldOwner.IsAlive && oldOwner.MapFaction.Leader != Hero.MainHero)
			{
				float value = settlement.GetValue(null, true);
				int num = (int)((1f + MathF.Max(1f, MathF.Sqrt(value / 100000f))) * ((newOwner.MapFaction != oldOwner.MapFaction) ? 1f : 0.5f));
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(oldOwner, oldOwner.MapFaction.Leader, -num, false);
				if (capturerHero != null && capturerHero.Clan != capturerHero.MapFaction.Leader.Clan)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(capturerHero, capturerHero.MapFaction.Leader, num / 2, false);
				}
				if (oldOwner.Clan != null && settlement != null)
				{
					ChangeClanInfluenceAction.Apply(oldOwner.Clan, (float)(settlement.IsTown ? (-50) : (-25)));
				}
			}
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x00111BC4 File Offset: 0x0010FDC4
		private void OnRaidCompleted(BattleSideEnum winnerSide, RaidEventComponent raidEvent)
		{
			MapEvent mapEvent = raidEvent.MapEvent;
			PartyBase leaderParty = mapEvent.AttackerSide.LeaderParty;
			Hero hero = ((leaderParty != null) ? leaderParty.LeaderHero : null);
			PartyBase leaderParty2 = mapEvent.DefenderSide.LeaderParty;
			if (leaderParty == null || leaderParty.MapFaction == mapEvent.MapEventSettlement.MapFaction)
			{
				return;
			}
			if (winnerSide == BattleSideEnum.Attacker && hero != null && leaderParty2 != null && leaderParty2.IsSettlement && leaderParty2.Settlement.IsVillage && leaderParty2.Settlement.OwnerClan != Clan.PlayerClan)
			{
				int num = -MathF.Ceiling(6f * raidEvent.RaidDamage);
				int num2 = -MathF.Ceiling(6f * raidEvent.RaidDamage * 0.5f);
				if (num < 0)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, leaderParty2.Settlement.OwnerClan.Leader, num, true);
				}
				if (num2 < 0)
				{
					foreach (Hero hero2 in leaderParty2.Settlement.Notables)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, hero2, num2, true);
					}
				}
			}
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x00111CFC File Offset: 0x0010FEFC
		private static void OnHeroesMarried(Hero firstHero, Hero secondHero, bool showNotification)
		{
			ChangeRelationAction.ApplyRelationChangeBetweenHeroes(firstHero, secondHero, 30, false);
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x00111D08 File Offset: 0x0010FF08
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveKingdom)
			{
				int num = ((detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion) ? (-40) : (-20));
				Hero leader = clan.Leader;
				foreach (Clan clan2 in oldKingdom.Clans)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(leader, clan2.Leader, num, true);
				}
			}
		}

		// Token: 0x0400135B RID: 4955
		private const int RelationPenaltyFactor = 6;

		// Token: 0x0400135C RID: 4956
		private const int RelationIncreaseBetweenHeroesAfterMarriage = 30;

		// Token: 0x0400135D RID: 4957
		private const int RelationChangeForLeavingWithRebellion = -40;

		// Token: 0x0400135E RID: 4958
		private const int RelationChangeForLeaveKingdom = -20;

		// Token: 0x0400135F RID: 4959
		private const int RaidDefenseRelationGainWithVillageNotable = 5;

		// Token: 0x04001360 RID: 4960
		private const float ChanceForRelationChange = 0.05f;
	}
}
