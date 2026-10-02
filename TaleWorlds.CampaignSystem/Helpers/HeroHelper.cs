using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x0200000D RID: 13
	public static class HeroHelper
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00005D6C File Offset: 0x00003F6C
		public static TextObject GetLastSeenText(Hero hero)
		{
			if (hero.IsDead)
			{
				return TextObject.GetEmpty();
			}
			if (hero.IsFugitive)
			{
				return GameTexts.FindText("str_last_seen_encyclopedia_entry_fugitive", null);
			}
			if (hero.IsKnownToPlayer && (hero.IsActive || hero.IsPrisoner))
			{
				hero.UpdateLastKnownClosestSettlement(HeroHelper.GetClosestSettlement(hero));
			}
			if (hero.LastKnownClosestSettlement == null || !hero.IsKnownToPlayer)
			{
				return GameTexts.FindText("str_never_seen_encyclopedia_entry", null);
			}
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			PartyBase partyBase = ((partyBelongedTo != null) ? partyBelongedTo.Party : null) ?? hero.PartyBelongedToAsPrisoner;
			if (partyBase == null)
			{
				TextObject textObject = GameTexts.FindText("str_last_seen_encyclopedia_entry", null);
				textObject.SetTextVariable("IS_IN_SETTLEMENT", (hero.LastKnownClosestSettlement == hero.CurrentSettlement) ? 1 : 0);
				textObject.SetTextVariable("SETTLEMENT", hero.LastKnownClosestSettlement.EncyclopediaLinkWithName);
				return textObject;
			}
			if (partyBase.IsMobile && partyBase.MobileParty.IsCurrentlyAtSea)
			{
				if (hero.IsPrisoner)
				{
					if (partyBase.MobileParty.Army != null)
					{
						TextObject textObject2 = GameTexts.FindText("str_last_seen_encyclopedia_entry_at_sea_in_army_as_prisoner", null);
						textObject2.SetTextVariable("ARMY_LEADER", partyBase.MobileParty.Army.LeaderParty.LeaderHero.EncyclopediaLinkWithName);
						textObject2.SetTextVariable("SETTLEMENT", hero.LastKnownClosestSettlement.EncyclopediaLinkWithName);
						return textObject2;
					}
					TextObject textObject3 = GameTexts.FindText("str_last_seen_encyclopedia_entry_at_sea_as_prisoner", null);
					textObject3.SetTextVariable("PARTY_LEADER", partyBase.LeaderHero.EncyclopediaLinkWithName);
					textObject3.SetTextVariable("SETTLEMENT", hero.LastKnownClosestSettlement.EncyclopediaLinkWithName);
					return textObject3;
				}
				else
				{
					if (partyBase.MobileParty.Army != null)
					{
						TextObject textObject4 = GameTexts.FindText("str_last_seen_encyclopedia_entry_at_sea_in_army", null);
						textObject4.SetTextVariable("ARMY_LEADER", partyBase.MobileParty.Army.LeaderParty.LeaderHero.EncyclopediaLinkWithName);
						textObject4.SetTextVariable("SETTLEMENT", hero.LastKnownClosestSettlement.EncyclopediaLinkWithName);
						return textObject4;
					}
					TextObject textObject5 = GameTexts.FindText("str_last_seen_encyclopedia_entry_at_sea", null);
					textObject5.SetTextVariable("SETTLEMENT", hero.LastKnownClosestSettlement.EncyclopediaLinkWithName);
					return textObject5;
				}
			}
			else if (hero.IsPrisoner)
			{
				if (!partyBase.IsMobile)
				{
					TextObject textObject6 = GameTexts.FindText("str_last_seen_encyclopedia_entry_as_prisoner_in_settlement", null);
					textObject6.SetTextVariable("SETTLEMENT", hero.LastKnownClosestSettlement.EncyclopediaLinkWithName);
					return textObject6;
				}
				if (partyBase.MobileParty.Army != null)
				{
					TextObject textObject7 = GameTexts.FindText("str_last_seen_encyclopedia_entry_in_army_as_prisoner", null);
					textObject7.SetTextVariable("ARMY_LEADER", partyBase.MobileParty.Army.LeaderParty.LeaderHero.EncyclopediaLinkWithName);
					textObject7.SetTextVariable("IS_IN_SETTLEMENT", (hero.LastKnownClosestSettlement == hero.CurrentSettlement) ? 1 : 0);
					textObject7.SetTextVariable("SETTLEMENT", hero.LastKnownClosestSettlement.EncyclopediaLinkWithName);
					return textObject7;
				}
				TextObject textObject8 = GameTexts.FindText("str_last_seen_encyclopedia_entry_as_prisoner", null);
				textObject8.SetTextVariable("PARTY_LEADER", partyBase.LeaderHero.EncyclopediaLinkWithName);
				textObject8.SetTextVariable("IS_IN_SETTLEMENT", (hero.LastKnownClosestSettlement == hero.CurrentSettlement) ? 1 : 0);
				textObject8.SetTextVariable("SETTLEMENT", hero.LastKnownClosestSettlement.EncyclopediaLinkWithName);
				return textObject8;
			}
			else
			{
				if (partyBase.MobileParty.Army != null)
				{
					TextObject textObject9 = GameTexts.FindText("str_last_seen_encyclopedia_entry_in_army", null);
					textObject9.SetTextVariable("ARMY_LEADER", partyBase.MobileParty.Army.LeaderParty.LeaderHero.EncyclopediaLinkWithName);
					textObject9.SetTextVariable("IS_IN_SETTLEMENT", (hero.LastKnownClosestSettlement == hero.CurrentSettlement) ? 1 : 0);
					textObject9.SetTextVariable("SETTLEMENT", hero.LastKnownClosestSettlement.EncyclopediaLinkWithName);
					return textObject9;
				}
				TextObject textObject10 = GameTexts.FindText("str_last_seen_encyclopedia_entry", null);
				textObject10.SetTextVariable("IS_IN_SETTLEMENT", (hero.LastKnownClosestSettlement == hero.CurrentSettlement) ? 1 : 0);
				textObject10.SetTextVariable("SETTLEMENT", hero.LastKnownClosestSettlement.EncyclopediaLinkWithName);
				return textObject10;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00006124 File Offset: 0x00004324
		public static Settlement GetClosestSettlement(Hero hero)
		{
			Settlement settlement = null;
			if (hero.CurrentSettlement != null)
			{
				settlement = hero.CurrentSettlement;
			}
			else
			{
				MobileParty partyBelongedTo = hero.PartyBelongedTo;
				PartyBase partyBase = ((partyBelongedTo != null) ? partyBelongedTo.Party : null) ?? hero.PartyBelongedToAsPrisoner;
				if (partyBase != null)
				{
					if (partyBase.IsSettlement)
					{
						settlement = partyBase.Settlement;
					}
					else if (partyBase.IsMobile)
					{
						MobileParty mobileParty = partyBase.MobileParty;
						float averageDistanceBetweenClosestTwoTownsWithNavigationType = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All);
						CampaignVec2 campaignVec = mobileParty.Position;
						if (campaignVec.IsValid())
						{
							float num = Campaign.MapDiagonalSquared;
							campaignVec = mobileParty.Position;
							LocatableSearchData<Settlement> locatableSearchData = Settlement.StartFindingLocatablesAroundPosition(campaignVec.ToVec2(), averageDistanceBetweenClosestTwoTownsWithNavigationType * 1.5f);
							for (Settlement settlement2 = Settlement.FindNextLocatable(ref locatableSearchData); settlement2 != null; settlement2 = Settlement.FindNextLocatable(ref locatableSearchData))
							{
								if (settlement2.IsVillage || settlement2.IsFortification)
								{
									campaignVec = settlement2.Position;
									float num2 = campaignVec.DistanceSquared(mobileParty.Position);
									if (num2 < num)
									{
										num = num2;
										settlement = settlement2;
									}
								}
							}
							if (settlement == null)
							{
								campaignVec = mobileParty.Position;
								settlement = SettlementHelper.FindNearestSettlementToPoint(in campaignVec, (Settlement x) => x.IsVillage || x.IsFortification);
							}
						}
						else
						{
							Debug.FailedAssert("Mobileparty is nowhere to be found", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "GetClosestSettlement", 2517);
						}
					}
				}
				else if (PlayerEncounter.Current != null && PlayerEncounter.Battle != null)
				{
					if (PlayerEncounter.Current.EncounterSettlementAux != null)
					{
						return PlayerEncounter.Current.EncounterSettlementAux;
					}
					BattleSideEnum otherSide = PlayerEncounter.Battle.GetOtherSide(PlayerEncounter.Battle.PlayerSide);
					if (PlayerEncounter.Battle.PartiesOnSide(otherSide).Any<MapEventParty>((MapEventParty x) => x.Party.Owner == hero))
					{
						settlement = SettlementHelper.FindNearestSettlementToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, (Settlement x) => x.IsVillage || x.IsFortification);
					}
				}
			}
			if (settlement != null && !settlement.IsVillage && !settlement.IsFortification)
			{
				settlement = SettlementHelper.FindNearestSettlementToSettlement(settlement, MobileParty.NavigationType.All, (Settlement x) => x.IsVillage || x.IsFortification);
			}
			return settlement;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00006360 File Offset: 0x00004560
		public static bool LordWillConspireWithLord(Hero lord, Hero otherLord, bool suggestingBetrayal)
		{
			Hero.OneToOneConversationHero.MapFaction.Leader.SetTextVariables();
			int num = 0;
			num += otherLord.RandomInt(-9, 11);
			num += lord.GetTraitLevel(DefaultTraits.Honor);
			if (suggestingBetrayal)
			{
				num--;
			}
			if (suggestingBetrayal && Hero.OneToOneConversationHero.Clan == Hero.OneToOneConversationHero.MapFaction.Leader.Clan)
			{
				TextObject textObject = new TextObject("{=0M6ApEr2}Surely you know that {FIRST_NAME} is {RELATIONSHIP} as well as my liege, and will always be able to count on my loyalty.", null);
				textObject.SetTextVariable("FIRST_NAME", Hero.OneToOneConversationHero.MapFaction.Leader.FirstName);
				textObject.SetTextVariable("RELATIONSHIP", ConversationHelper.HeroRefersToHero(Hero.OneToOneConversationHero, Hero.OneToOneConversationHero.MapFaction.Leader, true));
				MBTextManager.SetTextVariable("CONSPIRE_REFUSAL", textObject, false);
				return false;
			}
			if (num < 0)
			{
				if (suggestingBetrayal)
				{
					MBTextManager.SetTextVariable("CONSPIRE_REFUSAL", Campaign.Current.ConversationManager.FindMatchingTextOrNull("str_liege_support", lord.CharacterObject), false);
				}
				else
				{
					MBTextManager.SetTextVariable("CONSPIRE_REFUSAL", Campaign.Current.ConversationManager.FindMatchingTextOrNull("str_lord_intrigue_refuses", lord.CharacterObject), false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00006484 File Offset: 0x00004684
		public static bool UnderPlayerCommand(Hero hero)
		{
			return hero != null && ((hero.MapFaction != null && hero.MapFaction.Leader == Hero.MainHero) || (hero.IsNotable && hero.HomeSettlement.OwnerClan == Hero.MainHero.Clan) || hero.IsPlayerCompanion);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000064D8 File Offset: 0x000046D8
		public static TextObject GetTitleInIndefiniteCase(Hero hero)
		{
			string text = hero.MapFaction.Culture.StringId;
			if (hero.IsFemale)
			{
				text += "_f";
			}
			if (hero.MapFaction.IsKingdomFaction && hero.MapFaction.Leader == hero)
			{
				return GameTexts.FindText("str_faction_ruler", text);
			}
			return GameTexts.FindText("str_faction_official", text);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000653C File Offset: 0x0000473C
		public static TextObject GetCharacterTypeName(Hero hero)
		{
			if (hero.IsArtisan)
			{
				return GameTexts.FindText("str_charactertype_artisan", null);
			}
			if (hero.IsGangLeader)
			{
				return GameTexts.FindText("str_charactertype_gangleader", null);
			}
			if (hero.IsPreacher)
			{
				return GameTexts.FindText("str_charactertype_preacher", null);
			}
			if (hero.IsMerchant)
			{
				return GameTexts.FindText("str_charactertype_merchant", null);
			}
			if (hero.IsHeadman)
			{
				return GameTexts.FindText("str_charactertype_headman", null);
			}
			if (hero.IsRuralNotable)
			{
				return GameTexts.FindText("str_charactertype_ruralnotable", null);
			}
			if (hero.IsWanderer)
			{
				return GameTexts.FindText("str_charactertype_wanderer", null);
			}
			Clan clan = hero.Clan;
			if (clan != null && clan.IsClanTypeMercenary)
			{
				return GameTexts.FindText("str_charactertype_mercenary", null);
			}
			if (hero.IsMinorFactionHero)
			{
				return GameTexts.FindText("str_charactertype_minorfaction", null);
			}
			if (!hero.IsLord)
			{
				return GameTexts.FindText("str_charactertype_unknown", null);
			}
			if (hero.IsFemale)
			{
				return GameTexts.FindText("str_charactertype_lady", null);
			}
			return GameTexts.FindText("str_charactertype_lord", null);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000663C File Offset: 0x0000483C
		public static TextObject GetOccupiedEventReasonText(Hero hero)
		{
			TextObject textObject;
			if (!hero.CanHaveCampaignIssues())
			{
				textObject = GameTexts.FindText("str_hero_busy_issue_quest", null);
			}
			else
			{
				textObject = GameTexts.FindText("str_hero_busy", null);
			}
			return textObject;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000666C File Offset: 0x0000486C
		public static List<string> OrderHeroesOnPlayerSideByPriority(bool includeArmyLeader = false, bool includePlayerCompanions = false)
		{
			List<Hero> list = new List<Hero>();
			foreach (MapEventParty mapEventParty in MobileParty.MainParty.MapEvent.PartiesOnSide(MobileParty.MainParty.MapEvent.PlayerSide))
			{
				if (mapEventParty.Party.LeaderHero != null)
				{
					if (!includeArmyLeader)
					{
						MobileParty mobileParty = mapEventParty.Party.MobileParty;
						MobileParty mobileParty2;
						if (mobileParty == null)
						{
							mobileParty2 = null;
						}
						else
						{
							Army army = mobileParty.Army;
							mobileParty2 = ((army != null) ? army.LeaderParty : null);
						}
						if (mobileParty2 == mapEventParty.Party.MobileParty)
						{
							goto IL_0088;
						}
					}
					list.Add(mapEventParty.Party.LeaderHero);
				}
				IL_0088:
				if (mapEventParty.Party.MobileParty == MobileParty.MainParty && includePlayerCompanions)
				{
					foreach (Hero hero in Clan.PlayerClan.Companions)
					{
						if (hero.PartyBelongedTo == MobileParty.MainParty)
						{
							list.Add(hero);
						}
					}
				}
			}
			return list.OrderByDescending<Hero, int>((Hero t) => Campaign.Current.Models.EncounterModel.GetCharacterSergeantScore(t)).ToList<Hero>().ConvertAll<string>((Hero t) => t.CharacterObject.StringId);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000067E8 File Offset: 0x000049E8
		public static bool WillLordAttack()
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.PlayerSide == BattleSideEnum.Defender && (PlayerEncounter.EncounteredMobileParty == null || PlayerEncounter.EncounteredMobileParty.Ai.DoNotAttackMainPartyUntil.IsPast))
			{
				if (Hero.OneToOneConversationHero == null)
				{
					return false;
				}
				if (Campaign.Current.CurrentConversationContext == ConversationContext.FreeOrCapturePrisonerHero || Campaign.Current.CurrentConversationContext == ConversationContext.CapturedLord || Hero.OneToOneConversationHero.IsPrisoner)
				{
					return false;
				}
				PartyBase partyBase = ((Campaign.Current.ConversationManager.ConversationParty == null) ? PlayerEncounter.EncounteredParty : Campaign.Current.ConversationManager.ConversationParty.Party);
				if (partyBase.Owner != null && partyBase.LeaderHero != null && FactionManager.IsAtWarAgainstFaction(partyBase.MapFaction, Hero.MainHero.MapFaction))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000068BC File Offset: 0x00004ABC
		public static void SetPlayerSalutation()
		{
			if (Hero.OneToOneConversationHero.IsLord)
			{
				MBTextManager.SetTextVariable("PLAYER_SALUTATION", Hero.MainHero.Name, false);
				return;
			}
			if (Hero.OneToOneConversationHero.IsPlayerCompanion)
			{
				MBTextManager.SetTextVariable("PLAYER_SALUTATION", GameTexts.FindText("str_player_salutation_captain", null), false);
				return;
			}
			if (Hero.MainHero.IsFemale)
			{
				MBTextManager.SetTextVariable("PLAYER_SALUTATION", GameTexts.FindText("str_player_salutation_madame", null), false);
				return;
			}
			MBTextManager.SetTextVariable("PLAYER_SALUTATION", GameTexts.FindText("str_player_salutation_sir", null), false);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00006947 File Offset: 0x00004B47
		public static void SpawnHeroForTheFirstTime(Hero hero, Settlement spawnSettlement)
		{
			hero.BornSettlement = spawnSettlement;
			EnterSettlementAction.ApplyForCharacterOnly(hero, spawnSettlement);
			hero.ChangeState(Hero.CharacterStates.Active);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00006960 File Offset: 0x00004B60
		public static int DefaultRelation(Hero hero, Hero otherHero)
		{
			int middleAdultHoodAge = Campaign.Current.Models.AgeModel.MiddleAdultHoodAge;
			if (hero.Clan != null && hero.Clan.IsNoble && hero.Clan == otherHero.Clan)
			{
				return 40;
			}
			if (hero.MapFaction == otherHero.MapFaction && hero.CharacterObject.Culture == otherHero.CharacterObject.Culture && hero.Age > (float)middleAdultHoodAge && otherHero.Age > (float)middleAdultHoodAge && HeroHelper.NPCPersonalityClashWithNPC(hero, otherHero) > 40)
			{
				return -5;
			}
			if (hero.MapFaction == otherHero.MapFaction && hero.CharacterObject.Culture == otherHero.CharacterObject.Culture && hero.Age > (float)middleAdultHoodAge && otherHero.Age > (float)middleAdultHoodAge)
			{
				return 25;
			}
			if (hero.MapFaction == otherHero.MapFaction && hero.CharacterObject.Culture == otherHero.CharacterObject.Culture)
			{
				return 10;
			}
			return 0;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00006A57 File Offset: 0x00004C57
		public static bool IsCompanionInPlayerParty(Hero hero)
		{
			return hero != null && hero.IsPlayerCompanion && hero.PartyBelongedTo == MobileParty.MainParty;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00006A74 File Offset: 0x00004C74
		public static bool NPCPoliticalDifferencesWithNPC(Hero firstNPC, Hero secondNPC)
		{
			bool flag = firstNPC.GetTraitLevel(DefaultTraits.Egalitarian) > 0;
			bool flag2 = firstNPC.GetTraitLevel(DefaultTraits.Oligarchic) > 0;
			bool flag3 = firstNPC.GetTraitLevel(DefaultTraits.Authoritarian) > 0;
			bool flag4 = secondNPC.GetTraitLevel(DefaultTraits.Egalitarian) > 0;
			bool flag5 = secondNPC.GetTraitLevel(DefaultTraits.Oligarchic) > 0;
			bool flag6 = secondNPC.GetTraitLevel(DefaultTraits.Authoritarian) > 0;
			return flag != flag4 || flag2 != flag5 || flag3 != flag6;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00006AF0 File Offset: 0x00004CF0
		public static int NPCPersonalityClashWithNPC(Hero firstNPC, Hero secondNPC)
		{
			int num = 0;
			foreach (TraitObject traitObject in DefaultTraits.Personality)
			{
				int traitLevel = firstNPC.CharacterObject.GetTraitLevel(traitObject);
				int traitLevel2 = secondNPC.CharacterObject.GetTraitLevel(traitObject);
				if ((traitLevel > 0 && traitLevel2 < 0) || (traitLevel < 0 && traitLevel2 > 0))
				{
					num--;
				}
				if ((traitLevel > 0 && traitLevel2 > 0) || (traitLevel < 0 && traitLevel2 < 0))
				{
					num++;
				}
			}
			return num * 5;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00006B84 File Offset: 0x00004D84
		public static int TraitHarmony(Hero considerer, TraitObject trait, Hero consideree, bool sensitive)
		{
			int traitLevel = considerer.GetTraitLevel(trait);
			int traitLevel2 = consideree.GetTraitLevel(trait);
			if (traitLevel > 0 && traitLevel2 > 0)
			{
				return 3;
			}
			if (traitLevel == 0 && traitLevel2 > 0)
			{
				return 1;
			}
			if (traitLevel < 0 && traitLevel2 < 0)
			{
				return 1;
			}
			if (traitLevel > 0 && traitLevel2 < 0)
			{
				return -3;
			}
			if (traitLevel == 0 && traitLevel2 < 0)
			{
				return -1;
			}
			if (traitLevel < 0 && traitLevel2 > 0)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00006BE0 File Offset: 0x00004DE0
		public static float CalculateReliabilityConstant(Hero hero, float maxValueConstant = 1f)
		{
			int traitLevel = hero.GetTraitLevel(DefaultTraits.Honor);
			return maxValueConstant * ((2.5f + (float)MathF.Min(2, MathF.Max(-2, traitLevel))) / 5f);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00006C16 File Offset: 0x00004E16
		public static void SetPropertiesToTextObject(this Hero hero, TextObject textObject, string tagName)
		{
			StringHelpers.SetCharacterProperties(tagName, hero.CharacterObject, textObject, false);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00006C27 File Offset: 0x00004E27
		public static void SetPropertiesToTextObject(this Settlement settlement, TextObject textObject, string tagName)
		{
			StringHelpers.SetSettlementProperties(tagName, settlement, textObject, false);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00006C32 File Offset: 0x00004E32
		public static bool HeroCanRecruitFromHero(Hero buyerHero, Hero sellerHero, int index)
		{
			return index <= Campaign.Current.Models.VolunteerModel.MaximumIndexHeroCanRecruitFromHero(buyerHero, sellerHero, -101);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00006C54 File Offset: 0x00004E54
		public static List<CharacterObject> GetVolunteerTroopsOfHeroForRecruitment(Hero hero)
		{
			List<CharacterObject> list = new List<CharacterObject>();
			if (hero.IsAlive)
			{
				for (int i = 0; i < 6; i++)
				{
					list.Add(hero.VolunteerTypes[i]);
				}
			}
			return list;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00006C8C File Offset: 0x00004E8C
		public static Clan GetRandomClanForNotable(Hero notable)
		{
			float num = 0f;
			List<Clan> list = new List<Clan>();
			if (notable.IsPreacher)
			{
				num = 0.5f;
				list = Clan.NonBanditFactions.Where<Clan>((Clan x) => x.IsSect).ToList<Clan>();
			}
			if (notable.IsGangLeader)
			{
				num = 0.5f;
				list = Clan.NonBanditFactions.Where<Clan>((Clan x) => x.IsMafia).ToList<Clan>();
			}
			if (MBRandom.RandomFloat >= num)
			{
				return null;
			}
			foreach (Hero hero in notable.HomeSettlement.Notables)
			{
				if (list.Contains(hero.SupporterOf))
				{
					list.Remove(hero.SupporterOf);
				}
			}
			float num2 = 0f;
			ILookup<Clan, Settlement> lookup = Settlement.All.Where<Settlement>((Settlement x) => x.IsTown || x.IsHideout).ToLookup<Settlement, Clan>((Settlement x) => x.OwnerClan);
			foreach (Clan clan in list)
			{
				num2 += HeroHelper.GetProbabilityForClan(clan, lookup[clan], notable);
			}
			num2 *= MBRandom.RandomFloat;
			foreach (Clan clan2 in list)
			{
				num2 -= HeroHelper.GetProbabilityForClan(clan2, lookup[clan2], notable);
				if (num2 <= 0f)
				{
					return clan2;
				}
			}
			return null;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00006E90 File Offset: 0x00005090
		private static float GetProbabilityForClan(Clan clan, IEnumerable<Settlement> applicableSettlements, Hero notable)
		{
			float num = 1f;
			if (clan.Culture == notable.Culture)
			{
				num *= 3f;
			}
			float num2 = float.MaxValue;
			foreach (Settlement settlement in applicableSettlements)
			{
				float num3 = settlement.Position.DistanceSquared(notable.HomeSettlement.Position);
				if (num3 < num2)
				{
					num2 = num3;
				}
			}
			num /= num2;
			return num;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00006F18 File Offset: 0x00005118
		public static CampaignTime GetRandomBirthDayForAge(float age)
		{
			float num = MBRandom.RandomFloatRanged(0f, (float)CampaignTime.Now.GetDayOfYear);
			float num2 = (float)CampaignTime.Now.GetYear - age;
			return CampaignTime.Days(num) + CampaignTime.Years(num2);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00006F60 File Offset: 0x00005160
		public static void GetRandomDeathDayAndBirthDay(int deathAge, out CampaignTime birthday, out CampaignTime deathday)
		{
			int daysInYear = CampaignTime.DaysInYear;
			int num = MBRandom.RandomInt(daysInYear);
			birthday = CampaignTime.Years((float)(CampaignTime.Now.GetYear - deathAge - 1)) - CampaignTime.Days((float)num);
			deathday = birthday + CampaignTime.Years((float)deathAge) + CampaignTime.Days((float)MBRandom.RandomInt(daysInYear - 1));
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00006FD0 File Offset: 0x000051D0
		public static float StartRecruitingMoneyLimit(Hero hero)
		{
			if (hero.Clan == Clan.PlayerClan)
			{
				return 0f;
			}
			return 50f + ((hero.PartyBelongedTo != null) ? ((float)MathF.Min(150, hero.PartyBelongedTo.MemberRoster.TotalManCount) * 20f) : 0f);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00007028 File Offset: 0x00005228
		public static float StartRecruitingMoneyLimitForClanLeader(Hero hero)
		{
			if (hero.Clan == Clan.PlayerClan)
			{
				return 0f;
			}
			return 50f + ((hero.Clan != null && hero.Clan.Leader != null && hero.Clan.Leader.PartyBelongedTo != null) ? ((float)hero.Clan.Leader.PartyBelongedTo.TotalWage + (float)hero.Clan.Leader.PartyBelongedTo.MemberRoster.TotalManCount * 40f) : 0f);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000070B4 File Offset: 0x000052B4
		public static TextObject GetPersonalityTraitChangeName(TraitObject traitObject, Hero hero, bool isPositive)
		{
			if (DefaultTraits.Personality.Contains(traitObject))
			{
				int traitLevel = hero.GetTraitLevel(traitObject);
				string text = "str_trait_name_" + traitObject.StringId.ToLower();
				string text2 = (isPositive ? "3" : "1");
				if (traitLevel < 0)
				{
					text2 = (isPositive ? "3" : "0");
				}
				else if (traitLevel > 0)
				{
					text2 = (isPositive ? "4" : "1");
				}
				return GameTexts.FindText(text, text2);
			}
			Debug.FailedAssert("Given trait is not a personality trait!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "GetPersonalityTraitChangeName", 3084);
			return TextObject.GetEmpty();
		}
	}
}
