using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000040 RID: 64
	public abstract class CampaignEventReceiver
	{
		// Token: 0x06000523 RID: 1315 RVA: 0x00022A67 File Offset: 0x00020C67
		public virtual void RemoveListeners(object o)
		{
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00022A69 File Offset: 0x00020C69
		public virtual void OnCharacterCreationIsOver()
		{
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00022A6B File Offset: 0x00020C6B
		public virtual void OnHeroLevelledUp(Hero hero, bool shouldNotify = true)
		{
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00022A6D File Offset: 0x00020C6D
		public virtual void OnHomeHideoutChanged(BanditPartyComponent banditPartyComponent, Hideout oldHomeHideout)
		{
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00022A6F File Offset: 0x00020C6F
		public virtual void OnHeroGainedSkill(Hero hero, SkillObject skill, int change = 1, bool shouldNotify = true)
		{
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00022A71 File Offset: 0x00020C71
		public virtual void OnHeroCreated(Hero hero, bool isBornNaturally = false)
		{
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00022A73 File Offset: 0x00020C73
		public virtual void OnHeroActivated(Hero hero, Hero.CharacterStates previousState)
		{
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00022A75 File Offset: 0x00020C75
		public virtual void OnHeroWounded(Hero woundedHero)
		{
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00022A77 File Offset: 0x00020C77
		public virtual void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00022A79 File Offset: 0x00020C79
		public virtual void OnQuestLogAdded(QuestBase quest, bool hideInformation)
		{
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00022A7B File Offset: 0x00020C7B
		public virtual void OnIssueLogAdded(IssueBase issue, bool hideInformation)
		{
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00022A7D File Offset: 0x00020C7D
		public virtual void OnClanTierChanged(Clan clan, bool shouldNotify = true)
		{
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00022A7F File Offset: 0x00020C7F
		public virtual void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail actionDetail, bool showNotification = true)
		{
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00022A81 File Offset: 0x00020C81
		public virtual void OnClanDefected(Clan clan, Kingdom oldKingdom, Kingdom newKingdom)
		{
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00022A83 File Offset: 0x00020C83
		public virtual void OnClanCreated(Clan clan, bool isCompanion)
		{
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00022A85 File Offset: 0x00020C85
		public virtual void OnHeroJoinedParty(Hero hero, MobileParty mobileParty)
		{
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00022A87 File Offset: 0x00020C87
		public virtual void OnKingdomDecisionAdded(KingdomDecision decision, bool isPlayerInvolved)
		{
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00022A89 File Offset: 0x00020C89
		public virtual void OnKingdomDecisionCancelled(KingdomDecision decision, bool isPlayerInvolved)
		{
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00022A8B File Offset: 0x00020C8B
		public virtual void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome chosenOutcome, bool isPlayerInvolved)
		{
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00022A8D File Offset: 0x00020C8D
		public virtual void OnHeroOrPartyTradedGold(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> recipient, ValueTuple<int, string> goldAmount, bool showNotification)
		{
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00022A8F File Offset: 0x00020C8F
		public virtual void OnHeroOrPartyGaveItem(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> receiver, ItemRosterElement itemRosterElement, bool showNotification)
		{
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00022A91 File Offset: 0x00020C91
		public virtual void OnBanditPartyRecruited(MobileParty banditParty)
		{
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00022A93 File Offset: 0x00020C93
		public virtual void OnArmyCreated(Army army)
		{
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00022A95 File Offset: 0x00020C95
		public virtual void OnPartyAttachedAnotherParty(MobileParty mobileParty)
		{
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00022A97 File Offset: 0x00020C97
		public virtual void OnNearbyPartyAddedToPlayerMapEvent(MobileParty mobileParty)
		{
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00022A99 File Offset: 0x00020C99
		public virtual void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00022A9B File Offset: 0x00020C9B
		public virtual void OnArmyGathered(Army army, IMapPoint gatheringPoint)
		{
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00022A9D File Offset: 0x00020C9D
		public virtual void OnPerkOpened(Hero hero, PerkObject perk)
		{
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00022A9F File Offset: 0x00020C9F
		public virtual void OnPerkReset(Hero hero, PerkObject perk)
		{
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00022AA1 File Offset: 0x00020CA1
		public virtual void OnPlayerTraitChanged(TraitObject trait, int previousLevel)
		{
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00022AA3 File Offset: 0x00020CA3
		public virtual void OnVillageStateChanged(Village village, Village.VillageStates oldState, Village.VillageStates newState, MobileParty raiderParty)
		{
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00022AA5 File Offset: 0x00020CA5
		public virtual void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00022AA7 File Offset: 0x00020CA7
		public virtual void OnAfterSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00022AA9 File Offset: 0x00020CA9
		public virtual void OnBeforeSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00022AAB File Offset: 0x00020CAB
		public virtual void OnMercenaryTroopChangedInTown(Town town, CharacterObject oldTroopType, CharacterObject newTroopType)
		{
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00022AAD File Offset: 0x00020CAD
		public virtual void OnMercenaryNumberChangedInTown(Town town, int oldNumber, int newNumber)
		{
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00022AAF File Offset: 0x00020CAF
		public virtual void OnAlleyOwnerChanged(Alley alley, Hero newOwner, Hero oldOwner)
		{
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00022AB1 File Offset: 0x00020CB1
		public virtual void OnAlleyClearedByPlayer(Alley alley)
		{
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00022AB3 File Offset: 0x00020CB3
		public virtual void OnAlleyOccupiedByPlayer(Alley alley, TroopRoster troops)
		{
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00022AB5 File Offset: 0x00020CB5
		public virtual void OnRomanticStateChanged(Hero hero1, Hero hero2, Romance.RomanceLevelEnum romanceLevel)
		{
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00022AB7 File Offset: 0x00020CB7
		public virtual void OnBeforeHeroesMarried(Hero hero1, Hero hero2, bool showNotification = true)
		{
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00022AB9 File Offset: 0x00020CB9
		public virtual void OnPlayerEliminatedFromTournament(int round, Town town)
		{
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00022ABB File Offset: 0x00020CBB
		public virtual void OnPlayerStartedTournamentMatch(Town town)
		{
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00022ABD File Offset: 0x00020CBD
		public virtual void OnTournamentStarted(Town town)
		{
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00022ABF File Offset: 0x00020CBF
		public virtual void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00022AC1 File Offset: 0x00020CC1
		public virtual void OnTournamentCancelled(Town town)
		{
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00022AC3 File Offset: 0x00020CC3
		public virtual void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00022AC5 File Offset: 0x00020CC5
		public virtual void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00022AC7 File Offset: 0x00020CC7
		public virtual void OnKingdomCreated(Kingdom createdKingdom)
		{
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00022AC9 File Offset: 0x00020CC9
		public virtual void OnHeroOccupationChanged(Hero hero, Occupation oldOccupation)
		{
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00022ACB File Offset: 0x00020CCB
		public virtual void OnKingdomDestroyed(Kingdom kingdom)
		{
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00022ACD File Offset: 0x00020CCD
		public virtual void CanKingdomBeDiscontinued(Kingdom kingdom, ref bool result)
		{
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00022ACF File Offset: 0x00020CCF
		public virtual void OnBarterAccepted(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00022AD1 File Offset: 0x00020CD1
		public virtual void OnBarterCanceled(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00022AD3 File Offset: 0x00020CD3
		public virtual void OnStartBattle(PartyBase attackerParty, PartyBase defenderParty, object subject, bool showNotification)
		{
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00022AD5 File Offset: 0x00020CD5
		public virtual void OnRebellionFinished(Settlement settlement, Clan oldOwnerClan)
		{
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00022AD7 File Offset: 0x00020CD7
		public virtual void TownRebelliousStateChanged(Town town, bool rebelliousState)
		{
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00022AD9 File Offset: 0x00020CD9
		public virtual void OnRebelliousClanDisbandedAtSettlement(Settlement settlement, Clan clan)
		{
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00022ADB File Offset: 0x00020CDB
		public virtual void OnItemsLooted(MobileParty mobileParty, ItemRoster items)
		{
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00022ADD File Offset: 0x00020CDD
		public virtual void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00022ADF File Offset: 0x00020CDF
		public virtual void OnMobilePartyCreated(MobileParty party)
		{
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00022AE1 File Offset: 0x00020CE1
		public virtual void OnMapInteractableCreated(IInteractablePoint interactable)
		{
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00022AE3 File Offset: 0x00020CE3
		public virtual void OnMapInteractableDestroyed(IInteractablePoint interactable)
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00022AE5 File Offset: 0x00020CE5
		public virtual void OnMobilePartyQuestStatusChanged(MobileParty party, bool isUsedByQuest)
		{
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00022AE7 File Offset: 0x00020CE7
		public virtual void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00022AE9 File Offset: 0x00020CE9
		public virtual void OnBeforeHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00022AEB File Offset: 0x00020CEB
		public virtual void OnChildEducationCompleted(Hero hero, int age)
		{
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00022AED File Offset: 0x00020CED
		public virtual void OnHeroComesOfAge(Hero hero)
		{
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00022AEF File Offset: 0x00020CEF
		public virtual void OnHeroReachesTeenAge(Hero hero)
		{
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00022AF1 File Offset: 0x00020CF1
		public virtual void OnHeroGrowsOutOfInfancy(Hero hero)
		{
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00022AF3 File Offset: 0x00020CF3
		public virtual void OnCharacterDefeated(Hero winner, Hero loser)
		{
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00022AF5 File Offset: 0x00020CF5
		public virtual void OnHeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00022AF7 File Offset: 0x00020CF7
		public virtual void OnHeroPrisonerReleased(Hero prisoner, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail, bool showNotification = true)
		{
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00022AF9 File Offset: 0x00020CF9
		public virtual void OnCharacterBecameFugitive(Hero hero, bool showNotification)
		{
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00022AFB File Offset: 0x00020CFB
		public virtual void OnPlayerMetHero(Hero hero)
		{
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00022AFD File Offset: 0x00020CFD
		public virtual void OnPlayerLearnsAboutHero(Hero hero)
		{
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00022AFF File Offset: 0x00020CFF
		public virtual void OnRenownGained(Hero hero, int gainedRenown, bool doNotNotify)
		{
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00022B01 File Offset: 0x00020D01
		public virtual void OnCrimeRatingChanged(IFaction kingdom, float deltaCrimeAmount)
		{
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00022B03 File Offset: 0x00020D03
		public virtual void OnNewCompanionAdded(Hero newCompanion)
		{
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00022B05 File Offset: 0x00020D05
		public virtual void OnAfterMissionStarted(IMission iMission)
		{
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00022B07 File Offset: 0x00020D07
		public virtual void OnGameMenuOpened(MenuCallbackArgs args)
		{
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00022B09 File Offset: 0x00020D09
		public virtual void OnVillageBecomeNormal(Village village)
		{
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00022B0B File Offset: 0x00020D0B
		public virtual void OnVillageBeingRaided(Village village)
		{
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00022B0D File Offset: 0x00020D0D
		public virtual void OnVillageLooted(Village village)
		{
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00022B0F File Offset: 0x00020D0F
		public virtual void OnAgentJoinedConversation(IAgent agent)
		{
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00022B11 File Offset: 0x00020D11
		public virtual void OnConversationEnded(IEnumerable<CharacterObject> characters)
		{
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00022B13 File Offset: 0x00020D13
		public virtual void OnMapEventEnded(MapEvent mapEvent)
		{
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00022B15 File Offset: 0x00020D15
		public virtual void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00022B17 File Offset: 0x00020D17
		public virtual void OnRansomOfferedToPlayer(Hero captiveHero)
		{
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00022B19 File Offset: 0x00020D19
		public virtual void OnPrisonersChangeInSettlement(Settlement settlement, FlattenedTroopRoster prisonerRoster, Hero prisonerHero, bool takenFromDungeon)
		{
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00022B1B File Offset: 0x00020D1B
		public virtual void OnMissionStarted(IMission mission)
		{
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00022B1D File Offset: 0x00020D1D
		public virtual void OnRansomOfferCancelled(Hero captiveHero)
		{
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00022B1F File Offset: 0x00020D1F
		public virtual void OnPeaceOfferedToPlayer(IFaction opponentFaction, int tributeAmount, int tributeDuration)
		{
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00022B21 File Offset: 0x00020D21
		public virtual void OnTradeAgreementSigned(Kingdom kingdom, Kingdom other)
		{
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00022B23 File Offset: 0x00020D23
		public virtual void OnPeaceOfferResolved(IFaction opponentFaction)
		{
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00022B25 File Offset: 0x00020D25
		public virtual void OnMarriageOfferedToPlayer(Hero suitor, Hero maiden)
		{
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00022B27 File Offset: 0x00020D27
		public virtual void OnMarriageOfferCanceled(Hero suitor, Hero maiden)
		{
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00022B29 File Offset: 0x00020D29
		public virtual void OnVassalOrMercenaryServiceOfferedToPlayer(Kingdom offeredKingdom)
		{
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00022B2B File Offset: 0x00020D2B
		public virtual void OnVassalOrMercenaryServiceOfferCanceled(Kingdom offeredKingdom)
		{
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00022B2D File Offset: 0x00020D2D
		public virtual void OnPlayerBoardGameOver(Hero opposingHero, BoardGameHelper.BoardGameState state)
		{
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00022B2F File Offset: 0x00020D2F
		public virtual void OnCommonAreaStateChanged(Alley alley, Alley.AreaState oldState, Alley.AreaState newState)
		{
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00022B31 File Offset: 0x00020D31
		public virtual void BeforeMissionOpened()
		{
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00022B33 File Offset: 0x00020D33
		public virtual void OnPartyRemoved(PartyBase party)
		{
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00022B35 File Offset: 0x00020D35
		public virtual void OnPartySizeChanged(PartyBase party)
		{
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00022B37 File Offset: 0x00020D37
		public virtual void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00022B39 File Offset: 0x00020D39
		public virtual void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00022B3B File Offset: 0x00020D3B
		public virtual void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00022B3D File Offset: 0x00020D3D
		public virtual void Tick(float dt)
		{
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00022B3F File Offset: 0x00020D3F
		public virtual void OnSessionStart(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00022B41 File Offset: 0x00020D41
		public virtual void OnAfterSessionStart(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00022B43 File Offset: 0x00020D43
		public virtual void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00022B45 File Offset: 0x00020D45
		public virtual void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00022B47 File Offset: 0x00020D47
		public virtual void OnGameEarlyLoaded(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00022B49 File Offset: 0x00020D49
		public virtual void OnPlayerTradeProfit(int profit)
		{
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00022B4B File Offset: 0x00020D4B
		public virtual void OnRulingClanChanged(Kingdom kingdom, Clan oldRulingClan)
		{
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00022B4D File Offset: 0x00020D4D
		public virtual void OnPrisonerReleased(FlattenedTroopRoster roster)
		{
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00022B4F File Offset: 0x00020D4F
		public virtual void OnGameLoadFinished()
		{
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00022B51 File Offset: 0x00020D51
		public virtual void OnPartyJoinedArmy(MobileParty mobileParty)
		{
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00022B53 File Offset: 0x00020D53
		public virtual void OnPartyRemovedFromArmy(MobileParty mobileParty)
		{
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00022B55 File Offset: 0x00020D55
		public virtual void OnArmyOverlaySetDirty()
		{
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00022B57 File Offset: 0x00020D57
		public virtual void OnPlayerDesertedBattle(int sacrificedMenCount)
		{
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00022B59 File Offset: 0x00020D59
		public virtual void OnPlayerArmyLeaderChangedBehavior()
		{
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00022B5B File Offset: 0x00020D5B
		public virtual void MissionTick(float dt)
		{
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00022B5D File Offset: 0x00020D5D
		public virtual void OnChildConceived(Hero mother)
		{
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00022B5F File Offset: 0x00020D5F
		public virtual void OnGivenBirth(Hero mother, List<Hero> aliveChildren, int stillbornCount)
		{
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00022B61 File Offset: 0x00020D61
		public virtual void OnUnitRecruited(CharacterObject character, int amount)
		{
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00022B63 File Offset: 0x00020D63
		public virtual void OnPlayerBattleEnd(MapEvent mapEvent)
		{
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00022B65 File Offset: 0x00020D65
		public virtual void OnMissionEnded(IMission mission)
		{
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00022B67 File Offset: 0x00020D67
		public virtual void TickPartialHourlyAi(MobileParty party)
		{
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00022B69 File Offset: 0x00020D69
		public virtual void QuarterDailyPartyTick(MobileParty party)
		{
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00022B6B File Offset: 0x00020D6B
		public virtual void AiHourlyTick(MobileParty party, PartyThinkParams partyThinkParams)
		{
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00022B6D File Offset: 0x00020D6D
		public virtual void HourlyTick()
		{
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00022B6F File Offset: 0x00020D6F
		public virtual void QuarterHourlyTick()
		{
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00022B71 File Offset: 0x00020D71
		public virtual void HourlyTickParty(MobileParty mobileParty)
		{
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00022B73 File Offset: 0x00020D73
		public virtual void HourlyTickSettlement(Settlement settlement)
		{
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00022B75 File Offset: 0x00020D75
		public virtual void HourlyTickClan(Clan clan)
		{
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00022B77 File Offset: 0x00020D77
		public virtual void DailyTick()
		{
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00022B79 File Offset: 0x00020D79
		public virtual void DailyTickParty(MobileParty mobileParty)
		{
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00022B7B File Offset: 0x00020D7B
		public virtual void DailyTickTown(Town town)
		{
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00022B7D File Offset: 0x00020D7D
		public virtual void DailyTickSettlement(Settlement settlement)
		{
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00022B7F File Offset: 0x00020D7F
		public virtual void DailyTickClan(Clan clan)
		{
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00022B81 File Offset: 0x00020D81
		public virtual void OnPlayerBodyPropertiesChanged()
		{
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00022B83 File Offset: 0x00020D83
		public virtual void WeeklyTick()
		{
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00022B85 File Offset: 0x00020D85
		public virtual void CollectAvailableTutorials(ref List<CampaignTutorial> tutorials)
		{
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00022B87 File Offset: 0x00020D87
		public virtual void DailyTickHero(Hero hero)
		{
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00022B89 File Offset: 0x00020D89
		public virtual void OnTutorialCompleted(string tutorial)
		{
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00022B8B File Offset: 0x00020D8B
		public virtual void OnBuildingLevelChanged(Town town, Building building, int levelChange)
		{
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00022B8D File Offset: 0x00020D8D
		public virtual void BeforeGameMenuOpened(MenuCallbackArgs args)
		{
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00022B8F File Offset: 0x00020D8F
		public virtual void AfterGameMenuInitialized(MenuCallbackArgs args)
		{
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00022B91 File Offset: 0x00020D91
		public virtual void OnBarterablesRequested(BarterData args)
		{
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00022B93 File Offset: 0x00020D93
		public virtual void OnPartyVisibilityChanged(PartyBase party)
		{
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00022B95 File Offset: 0x00020D95
		public virtual void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00022B97 File Offset: 0x00020D97
		public virtual void TrackDetected(Track track)
		{
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00022B99 File Offset: 0x00020D99
		public virtual void TrackLost(Track track)
		{
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00022B9B File Offset: 0x00020D9B
		public virtual void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00022B9D File Offset: 0x00020D9D
		public virtual void LocationCharactersSimulated()
		{
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00022B9F File Offset: 0x00020D9F
		public virtual void OnBeforePlayerAgentSpawn(ref MatrixFrame spawnFrame)
		{
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00022BA1 File Offset: 0x00020DA1
		public virtual void OnPlayerAgentSpawned()
		{
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00022BA3 File Offset: 0x00020DA3
		public virtual void OnPlayerUpgradedTroops(CharacterObject upgradeFromTroop, CharacterObject upgradeToTroop, int number)
		{
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00022BA5 File Offset: 0x00020DA5
		public virtual void OnHeroCombatHit(CharacterObject attackerTroop, CharacterObject attackedTroop, PartyBase party, WeaponComponentData usedWeapon, bool isFatal, int xp)
		{
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00022BA7 File Offset: 0x00020DA7
		public virtual void OnCharacterPortraitPopUpOpened(CharacterObject character)
		{
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00022BA9 File Offset: 0x00020DA9
		public virtual void OnCharacterPortraitPopUpClosed()
		{
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00022BAB File Offset: 0x00020DAB
		public virtual void OnPlayerStartTalkFromMenu(Hero hero)
		{
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00022BAD File Offset: 0x00020DAD
		public virtual void OnGameMenuOptionSelected(GameMenu gameMenu, GameMenuOption gameMenuOption)
		{
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00022BAF File Offset: 0x00020DAF
		public virtual void OnPlayerStartRecruitment(CharacterObject recruitTroopCharacter)
		{
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00022BB1 File Offset: 0x00020DB1
		public virtual void OnBeforePlayerCharacterChanged(Hero oldPlayer, Hero newPlayer)
		{
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00022BB3 File Offset: 0x00020DB3
		public virtual void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
		{
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00022BB5 File Offset: 0x00020DB5
		public virtual void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
		{
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00022BB7 File Offset: 0x00020DB7
		public virtual void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00022BB9 File Offset: 0x00020DB9
		public virtual void OnPlayerSiegeStarted()
		{
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00022BBB File Offset: 0x00020DBB
		public virtual void OnSiegeEventEnded(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00022BBD File Offset: 0x00020DBD
		public virtual void OnSiegeAftermathApplied(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00022BBF File Offset: 0x00020DBF
		public virtual void OnSiegeBombardmentHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, SiegeBombardTargets target)
		{
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00022BC1 File Offset: 0x00020DC1
		public virtual void OnSiegeBombardmentWallHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, bool isWallCracked)
		{
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00022BC3 File Offset: 0x00020DC3
		public virtual void OnSiegeEngineDestroyed(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType destroyedEngine)
		{
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00022BC5 File Offset: 0x00020DC5
		public virtual void OnTradeRumorIsTaken(List<TradeRumor> newRumors, Settlement sourceSettlement = null)
		{
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00022BC7 File Offset: 0x00020DC7
		public virtual void OnCheckForIssue(Hero hero)
		{
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00022BC9 File Offset: 0x00020DC9
		public virtual void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero issueSolver)
		{
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00022BCB File Offset: 0x00020DCB
		public virtual void OnTroopsDeserted(MobileParty mobileParty, TroopRoster desertedTroops)
		{
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00022BCD File Offset: 0x00020DCD
		public virtual void OnTroopRecruited(Hero recruiterHero, Settlement recruitmentSettlement, Hero recruitmentSource, CharacterObject troop, int amount)
		{
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00022BCF File Offset: 0x00020DCF
		public virtual void OnTroopGivenToSettlement(Hero giverHero, Settlement recipientSettlement, TroopRoster roster)
		{
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00022BD1 File Offset: 0x00020DD1
		public virtual void OnItemSold(PartyBase receiverParty, PartyBase payerParty, ItemRosterElement itemRosterElement, int number, Settlement currentSettlement)
		{
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00022BD3 File Offset: 0x00020DD3
		public virtual void OnCaravanTransactionCompleted(MobileParty caravanParty, Town town, List<ValueTuple<EquipmentElement, int>> itemRosterElements)
		{
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00022BD5 File Offset: 0x00020DD5
		public virtual void OnPrisonerSold(PartyBase sellerParty, PartyBase buyerParty, TroopRoster prisoners)
		{
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00022BD7 File Offset: 0x00020DD7
		public virtual void OnPartyDisbanded(MobileParty disbandParty, Settlement relatedSettlement)
		{
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00022BD9 File Offset: 0x00020DD9
		public virtual void OnPartyDisbandStarted(MobileParty disbandParty)
		{
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00022BDB File Offset: 0x00020DDB
		public virtual void OnPartyDisbandCanceled(MobileParty disbandParty)
		{
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00022BDD File Offset: 0x00020DDD
		public virtual void OnHideoutSpotted(PartyBase party, PartyBase hideoutParty)
		{
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00022BDF File Offset: 0x00020DDF
		public virtual void OnHideoutDeactivated(Settlement hideout)
		{
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00022BE1 File Offset: 0x00020DE1
		public virtual void OnHideoutBattleCompleted(BattleSideEnum winnerSide, HideoutEventComponent hideoutEventComponent, HideoutEventComponent.HideoutBattleEndState battleEndState)
		{
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00022BE3 File Offset: 0x00020DE3
		public virtual void OnPlayerInventoryExchange(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00022BE5 File Offset: 0x00020DE5
		public virtual void OnItemsDiscardedByPlayer(ItemRoster roster)
		{
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00022BE7 File Offset: 0x00020DE7
		public virtual void OnPersuasionProgressCommitted(Tuple<PersuasionOptionArgs, PersuasionOptionResult> progress)
		{
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00022BE9 File Offset: 0x00020DE9
		public virtual void OnHeroSharedFoodWithAnother(Hero supporterHero, Hero supportedHero, float influence)
		{
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00022BEB File Offset: 0x00020DEB
		public virtual void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00022BED File Offset: 0x00020DED
		public virtual void OnQuestStarted(QuestBase quest)
		{
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00022BEF File Offset: 0x00020DEF
		public virtual void OnItemProduced(ItemObject itemObject, Settlement settlement, int count)
		{
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00022BF1 File Offset: 0x00020DF1
		public virtual void OnItemConsumed(ItemObject itemObject, Settlement settlement, int count)
		{
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00022BF3 File Offset: 0x00020DF3
		public virtual void OnPartyConsumedFood(MobileParty party)
		{
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00022BF5 File Offset: 0x00020DF5
		public virtual void SiegeCompleted(Settlement siegeSettlement, MobileParty attackerParty, bool isWin, MapEvent.BattleTypes battleType)
		{
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00022BF7 File Offset: 0x00020DF7
		public virtual void AfterSiegeCompleted(Settlement siegeSettlement, MobileParty attackerParty, bool isWin, MapEvent.BattleTypes battleType)
		{
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00022BF9 File Offset: 0x00020DF9
		public virtual void SiegeEngineBuilt(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine)
		{
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00022BFB File Offset: 0x00020DFB
		public virtual void RaidCompleted(BattleSideEnum winnerSide, RaidEventComponent raidEvent)
		{
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00022BFD File Offset: 0x00020DFD
		public virtual void ForceSuppliesCompleted(BattleSideEnum winnerSide, ForceSuppliesEventComponent forceSuppliesEvent)
		{
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00022BFF File Offset: 0x00020DFF
		public virtual void ForceVolunteersCompleted(BattleSideEnum winnerSide, ForceVolunteersEventComponent forceVolunteersEvent)
		{
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00022C01 File Offset: 0x00020E01
		public virtual void OnBeforeMainCharacterDied(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00022C03 File Offset: 0x00020E03
		public virtual void OnGameOver()
		{
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00022C05 File Offset: 0x00020E05
		public virtual void OnClanDestroyed(Clan destroyedClan)
		{
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00022C07 File Offset: 0x00020E07
		public virtual void OnNewIssueCreated(IssueBase issue)
		{
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00022C09 File Offset: 0x00020E09
		public virtual void OnIssueOwnerChanged(IssueBase issue, Hero oldOwner)
		{
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00022C0B File Offset: 0x00020E0B
		public virtual void OnNewItemCrafted(ItemObject itemObject)
		{
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00022C0D File Offset: 0x00020E0D
		public virtual void OnWorkshopInitialized(Workshop workshop)
		{
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00022C0F File Offset: 0x00020E0F
		public virtual void OnWorkshopOwnerChanged(Workshop workshop, Hero oldOwner)
		{
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00022C11 File Offset: 0x00020E11
		public virtual void OnWorkshopTypeChanged(Workshop workshop)
		{
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00022C13 File Offset: 0x00020E13
		public virtual void CraftingPartUnlocked(CraftingPiece craftingPiece)
		{
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00022C15 File Offset: 0x00020E15
		public virtual void OnNewItemCrafted(ItemObject itemObject, ItemModifier overriddenItemModifier, bool isCraftingOrderItem)
		{
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00022C17 File Offset: 0x00020E17
		public virtual void OnEquipmentSmeltedByHero(Hero hero, EquipmentElement equipmentElement)
		{
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00022C19 File Offset: 0x00020E19
		public virtual void OnBeforeSave()
		{
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00022C1B File Offset: 0x00020E1B
		public virtual void OnMainPartyPrisonerRecruited(FlattenedTroopRoster roster)
		{
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00022C1D File Offset: 0x00020E1D
		public virtual void OnPrisonerTaken(FlattenedTroopRoster roster)
		{
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00022C1F File Offset: 0x00020E1F
		public virtual void OnPrisonerDonatedToSettlement(MobileParty donatingParty, FlattenedTroopRoster donatedPrisoners, Settlement donatedSettlement)
		{
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00022C21 File Offset: 0x00020E21
		public virtual void CanMoveToSettlement(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00022C23 File Offset: 0x00020E23
		public virtual void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00022C25 File Offset: 0x00020E25
		public virtual void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00022C27 File Offset: 0x00020E27
		public virtual void CanHeroBeReleased(Hero prisoner, ref bool result)
		{
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00022C29 File Offset: 0x00020E29
		public virtual void CanPlayerMeetWithHeroAfterConversation(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00022C2B File Offset: 0x00020E2B
		public virtual void CanHeroBecomePrisoner(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00022C2D File Offset: 0x00020E2D
		public virtual void CanBeGovernorOrHavePartyRole(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00022C2F File Offset: 0x00020E2F
		public virtual void OnSaveOver(bool isSuccessful, string saveName)
		{
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00022C31 File Offset: 0x00020E31
		public virtual void CollectMetadataEntries(List<KeyValuePair<string, string>> pairs)
		{
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00022C33 File Offset: 0x00020E33
		public virtual void OnSaveStarted()
		{
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00022C35 File Offset: 0x00020E35
		public virtual void CanHeroMarry(Hero hero, ref bool result)
		{
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00022C37 File Offset: 0x00020E37
		public virtual void OnHeroTeleportationRequested(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00022C39 File Offset: 0x00020E39
		public virtual void OnPartyLeaderChangeOfferCanceled(MobileParty party)
		{
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00022C3B File Offset: 0x00020E3B
		public virtual void OnPartyLeaderChanged(MobileParty mobileParty, Hero oldLeader)
		{
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00022C3D File Offset: 0x00020E3D
		public virtual void OnClanInfluenceChanged(Clan clan, float change)
		{
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00022C3F File Offset: 0x00020E3F
		public virtual void OnPlayerPartyKnockedOrKilledTroop(CharacterObject strikedTroop)
		{
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00022C41 File Offset: 0x00020E41
		public virtual void OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType incomeType, int incomeAmount)
		{
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00022C43 File Offset: 0x00020E43
		public virtual void OnClanEarnedGoldFromTribute(Clan receiverClan, IFaction payingFaction)
		{
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00022C45 File Offset: 0x00020E45
		public virtual void OnCollectLootItems(PartyBase winnerParty, ItemRoster gainedLoots)
		{
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00022C47 File Offset: 0x00020E47
		public virtual void OnLootDistributedToParty(PartyBase winnerParty, PartyBase defeatedParty, ItemRoster lootedItems)
		{
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00022C49 File Offset: 0x00020E49
		public virtual void OnPlayerJoinedTournament(Town town, bool isParticipant)
		{
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00022C4B File Offset: 0x00020E4B
		public virtual void OnConfigChanged()
		{
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00022C4D File Offset: 0x00020E4D
		public virtual void OnMobilePartyRaftStateChanged(MobileParty mobileParty)
		{
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00022C4F File Offset: 0x00020E4F
		public virtual void OnCharacterCreationInitialized(CharacterCreationManager characterCreationManager)
		{
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00022C51 File Offset: 0x00020E51
		public virtual void OnShipDestroyed(PartyBase owner, Ship ship, DestroyShipAction.ShipDestroyDetail detail)
		{
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00022C53 File Offset: 0x00020E53
		public virtual void OnShipOwnerChanged(Ship ship, PartyBase oldOwner, ChangeShipOwnerAction.ShipOwnerChangeDetail shipOwnerChangeDetail)
		{
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00022C55 File Offset: 0x00020E55
		public virtual void CanHaveUnlockedUpgradePiece(Ship ship, ChangeShipOwnerAction.ShipOwnerChangeDetail detail, ref bool canHaveUpgradePiece)
		{
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00022C57 File Offset: 0x00020E57
		public virtual void OnFigureheadUnlocked(Figurehead figurehead)
		{
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00022C59 File Offset: 0x00020E59
		public virtual void OnShipRepaired(Ship ship, Settlement repairPort)
		{
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00022C5B File Offset: 0x00020E5B
		public virtual void OnPartyLeftArmy(MobileParty party, Army army)
		{
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00022C5D File Offset: 0x00020E5D
		public virtual void OnIncidentResolved(Incident incident)
		{
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00022C5F File Offset: 0x00020E5F
		public virtual void OnPartyAddedToMapEvent(PartyBase partyBase)
		{
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00022C61 File Offset: 0x00020E61
		public virtual void OnMobilePartyNavigationStateChanged(MobileParty mobileParty)
		{
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00022C63 File Offset: 0x00020E63
		public virtual void OnMobilePartyJoinedToSiegeEvent(MobileParty mobileParty)
		{
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00022C65 File Offset: 0x00020E65
		public virtual void OnMobilePartyLeftSiegeEvent(MobileParty mobileParty)
		{
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00022C67 File Offset: 0x00020E67
		public virtual void OnBlockadeActivated(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00022C69 File Offset: 0x00020E69
		public virtual void OnBlockadeDeactivated(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00022C6B File Offset: 0x00020E6B
		public virtual void OnShipCreated(Ship ship, Settlement createdSettlement)
		{
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00022C6D File Offset: 0x00020E6D
		public virtual void OnMercenaryServiceStarted(Clan mercenaryClan, StartMercenaryServiceAction.StartMercenaryServiceActionDetails details)
		{
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00022C6F File Offset: 0x00020E6F
		public virtual void OnMercenaryServiceEnded(Clan mercenaryClan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails details)
		{
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00022C71 File Offset: 0x00020E71
		public virtual void OnAllianceStarted(Kingdom kingdom1, Kingdom kingdom2)
		{
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00022C73 File Offset: 0x00020E73
		public virtual void OnAllianceEnded(Kingdom kingdom1, Kingdom kingdom2)
		{
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00022C75 File Offset: 0x00020E75
		public virtual void OnCallToWarAgreementStarted(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00022C77 File Offset: 0x00020E77
		public virtual void OnCallToWarAgreementEnded(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00022C79 File Offset: 0x00020E79
		public virtual void OnPartyEncounter(PartyBase attacker, PartyBase defender)
		{
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00022C7B File Offset: 0x00020E7B
		public virtual void OnBloodFeudStateChanged(Clan clan, Hero executedHero, ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail detail)
		{
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00022C7D File Offset: 0x00020E7D
		public virtual void OnDeathMarkAdded(Hero hero, Hero killerHero)
		{
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00022C7F File Offset: 0x00020E7F
		public virtual void CanHeroLeadParty(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00022C81 File Offset: 0x00020E81
		public virtual void OnCraftingOrderCompleted(Town town, CraftingOrder craftingOrder, ItemObject craftedItem, Hero completerHero)
		{
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00022C83 File Offset: 0x00020E83
		public virtual void OnItemsRefined(Hero hero, Crafting.RefiningFormula refineFormula)
		{
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00022C85 File Offset: 0x00020E85
		public virtual void OnMapEventContinuityNeedsUpdate(IFaction faction)
		{
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00022C87 File Offset: 0x00020E87
		public virtual void OnHeirSelectionOver(Hero selectedHeir)
		{
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00022C89 File Offset: 0x00020E89
		public virtual void OnHeirSelectionRequested(Dictionary<Hero, int> heirApparents)
		{
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00022C8B File Offset: 0x00020E8B
		public virtual void OnMainPartyStarving()
		{
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00022C8D File Offset: 0x00020E8D
		public virtual void OnHeroGetsBusy(Hero hero, HeroGetsBusyReasons heroGetsBusyReason)
		{
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00022C8F File Offset: 0x00020E8F
		public virtual void CanHeroEquipmentBeChanged(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00022C91 File Offset: 0x00020E91
		public virtual void CanHaveCampaignIssues(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00022C93 File Offset: 0x00020E93
		public virtual void IsSettlementBusy(Settlement settlement, object asker, ref int flags)
		{
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00022C95 File Offset: 0x00020E95
		public virtual void OnHeroUnregistered(Hero hero)
		{
		}
	}
}
