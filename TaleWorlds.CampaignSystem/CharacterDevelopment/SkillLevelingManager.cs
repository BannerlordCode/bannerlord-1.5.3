using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003C7 RID: 967
	public static class SkillLevelingManager
	{
		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x060038B5 RID: 14517 RVA: 0x000EA839 File Offset: 0x000E8A39
		private static ISkillLevelingManager Instance
		{
			get
			{
				return Campaign.Current.SkillLevelingManager;
			}
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x000EA848 File Offset: 0x000E8A48
		public static void OnCombatHit(CharacterObject affectorCharacter, CharacterObject affectedCharacter, CharacterObject captain, Hero commander, float speedBonusFromMovement, float shotDifficulty, WeaponComponentData affectorWeapon, float hitPointRatio, CombatXpModel.MissionTypeEnum missionType, bool isAffectorMounted, bool isTeamKill, bool isAffectorUnderCommand, float damageAmount, bool isFatal, bool isSiegeEngineHit, bool isHorseCharge, bool isSneakAttack)
		{
			SkillLevelingManager.Instance.OnCombatHit(affectorCharacter, affectedCharacter, captain, commander, speedBonusFromMovement, shotDifficulty, affectorWeapon, hitPointRatio, missionType, isAffectorMounted, isTeamKill, isAffectorUnderCommand, damageAmount, isFatal, isSiegeEngineHit, isHorseCharge, isSneakAttack);
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x000EA87D File Offset: 0x000E8A7D
		public static void OnSiegeEngineDestroyed(MobileParty party, SiegeEngineType destroyedSiegeEngine)
		{
			SkillLevelingManager.Instance.OnSiegeEngineDestroyed(party, destroyedSiegeEngine);
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x000EA88B File Offset: 0x000E8A8B
		public static void OnWallBreached(MobileParty party)
		{
			SkillLevelingManager.Instance.OnWallBreached(party);
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x000EA898 File Offset: 0x000E8A98
		public static void OnSimulationCombatKill(CharacterObject affectorCharacter, CharacterObject affectedCharacter, PartyBase affectorParty, PartyBase commanderParty)
		{
			SkillLevelingManager.Instance.OnSimulationCombatKill(affectorCharacter, affectedCharacter, affectorParty, commanderParty);
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x000EA8A8 File Offset: 0x000E8AA8
		public static void OnTradeProfitMade(PartyBase party, int tradeProfit)
		{
			SkillLevelingManager.Instance.OnTradeProfitMade(party, tradeProfit);
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x000EA8B6 File Offset: 0x000E8AB6
		public static void OnTradeProfitMade(Hero hero, int tradeProfit)
		{
			SkillLevelingManager.Instance.OnTradeProfitMade(hero, tradeProfit);
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x000EA8C4 File Offset: 0x000E8AC4
		public static void OnSettlementProjectFinished(Settlement settlement)
		{
			SkillLevelingManager.Instance.OnSettlementProjectFinished(settlement);
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x000EA8D1 File Offset: 0x000E8AD1
		public static void OnSettlementGoverned(Hero governor, Settlement settlement)
		{
			SkillLevelingManager.Instance.OnSettlementGoverned(governor, settlement);
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x000EA8DF File Offset: 0x000E8ADF
		public static void OnInfluenceSpent(Hero hero, float amountSpent)
		{
			SkillLevelingManager.Instance.OnInfluenceSpent(hero, amountSpent);
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x000EA8ED File Offset: 0x000E8AED
		public static void OnGainRelation(Hero hero, Hero gainedRelationWith, float relationChange, ChangeRelationAction.ChangeRelationDetail detail = ChangeRelationAction.ChangeRelationDetail.Default)
		{
			SkillLevelingManager.Instance.OnGainRelation(hero, gainedRelationWith, relationChange, detail);
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x000EA8FD File Offset: 0x000E8AFD
		public static void OnTroopRecruited(Hero hero, int amount, int tier)
		{
			SkillLevelingManager.Instance.OnTroopRecruited(hero, amount, tier);
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x000EA90C File Offset: 0x000E8B0C
		public static void OnBribeGiven(int amount)
		{
			SkillLevelingManager.Instance.OnBribeGiven(amount);
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x000EA919 File Offset: 0x000E8B19
		public static void OnBanditsRecruited(MobileParty mobileParty, CharacterObject bandit, int count)
		{
			SkillLevelingManager.Instance.OnBanditsRecruited(mobileParty, bandit, count);
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x000EA928 File Offset: 0x000E8B28
		public static void OnMainHeroReleasedFromCaptivity(float captivityTime)
		{
			SkillLevelingManager.Instance.OnMainHeroReleasedFromCaptivity(captivityTime);
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x000EA935 File Offset: 0x000E8B35
		public static void OnMainHeroTortured()
		{
			SkillLevelingManager.Instance.OnMainHeroTortured();
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x000EA941 File Offset: 0x000E8B41
		public static void OnMainHeroDisguised(bool isNotCaught)
		{
			SkillLevelingManager.Instance.OnMainHeroDisguised(isNotCaught);
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x000EA94E File Offset: 0x000E8B4E
		public static void OnRaid(MobileParty attackerParty, ItemRoster lootedItems)
		{
			SkillLevelingManager.Instance.OnRaid(attackerParty, lootedItems);
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x000EA95C File Offset: 0x000E8B5C
		public static void OnLoot(MobileParty attackerParty, MobileParty forcedParty, ItemRoster lootedItems, bool attacked)
		{
			SkillLevelingManager.Instance.OnLoot(attackerParty, forcedParty, lootedItems, attacked);
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x000EA96C File Offset: 0x000E8B6C
		public static void OnForceVolunteers(MobileParty attackerParty, PartyBase forcedParty)
		{
			SkillLevelingManager.Instance.OnForceVolunteers(attackerParty, forcedParty);
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x000EA97A File Offset: 0x000E8B7A
		public static void OnForceSupplies(MobileParty attackerParty, ItemRoster lootedItems, bool attacked)
		{
			SkillLevelingManager.Instance.OnForceSupplies(attackerParty, lootedItems, attacked);
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x000EA989 File Offset: 0x000E8B89
		public static void OnPrisonerSell(MobileParty mobileParty, in TroopRoster prisonerRoster)
		{
			SkillLevelingManager.Instance.OnPrisonerSell(mobileParty, in prisonerRoster);
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x000EA997 File Offset: 0x000E8B97
		public static void OnSurgeryApplied(MobileParty party, bool surgerySuccess, int troopTier)
		{
			SkillLevelingManager.Instance.OnSurgeryApplied(party, surgerySuccess, troopTier);
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x000EA9A6 File Offset: 0x000E8BA6
		public static void OnTacticsUsed(MobileParty party, float xp)
		{
			SkillLevelingManager.Instance.OnTacticsUsed(party, xp);
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x000EA9B4 File Offset: 0x000E8BB4
		public static void OnHideoutSpotted(MobileParty party, PartyBase spottedParty)
		{
			SkillLevelingManager.Instance.OnHideoutSpotted(party, spottedParty);
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x000EA9C2 File Offset: 0x000E8BC2
		public static void OnTrackDetected(Track track)
		{
			SkillLevelingManager.Instance.OnTrackDetected(track);
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x000EA9CF File Offset: 0x000E8BCF
		public static void OnTravelOnFoot(Hero hero)
		{
			SkillLevelingManager.Instance.OnTravelOnFoot(hero);
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x000EA9DC File Offset: 0x000E8BDC
		public static void OnTravelOnHorse(Hero hero)
		{
			SkillLevelingManager.Instance.OnTravelOnHorse(hero);
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x000EA9E9 File Offset: 0x000E8BE9
		public static void OnTravelOnWater(MobileParty party)
		{
			SkillLevelingManager.Instance.OnTravelOnWater(party);
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x000EA9F6 File Offset: 0x000E8BF6
		public static void OnAIPartiesTravel(Hero hero, bool isCaravanParty, TerrainType currentTerrainType)
		{
			SkillLevelingManager.Instance.OnAIPartiesTravel(hero, isCaravanParty, currentTerrainType);
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x000EAA05 File Offset: 0x000E8C05
		public static void OnTraverseTerrain(MobileParty mobileParty, TerrainType currentTerrainType)
		{
			SkillLevelingManager.Instance.OnTraverseTerrain(mobileParty, currentTerrainType);
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x000EAA13 File Offset: 0x000E8C13
		public static void OnBattleEnded(PartyBase party, CharacterObject troop, int excessXp)
		{
			SkillLevelingManager.Instance.OnBattleEnded(party, troop, excessXp);
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x000EAA22 File Offset: 0x000E8C22
		public static void OnHeroHealedWhileWaiting(Hero hero, int healingAmount)
		{
			SkillLevelingManager.Instance.OnHeroHealedWhileWaiting(hero, healingAmount);
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x000EAA30 File Offset: 0x000E8C30
		public static void OnRegularTroopHealedWhileWaiting(MobileParty mobileParty, int healedTroopCount, float averageTier)
		{
			SkillLevelingManager.Instance.OnRegularTroopHealedWhileWaiting(mobileParty, healedTroopCount, averageTier);
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x000EAA3F File Offset: 0x000E8C3F
		public static void OnLeadingArmy(MobileParty mobileParty)
		{
			SkillLevelingManager.Instance.OnLeadingArmy(mobileParty);
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x000EAA4C File Offset: 0x000E8C4C
		public static void OnSieging(MobileParty mobileParty)
		{
			SkillLevelingManager.Instance.OnSieging(mobileParty);
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x000EAA59 File Offset: 0x000E8C59
		public static void OnSiegeEngineBuilt(MobileParty mobileParty, SiegeEngineType siegeEngine)
		{
			SkillLevelingManager.Instance.OnSiegeEngineBuilt(mobileParty, siegeEngine);
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x000EAA67 File Offset: 0x000E8C67
		public static void OnUpgradeTroops(PartyBase party, CharacterObject troop, CharacterObject upgrade, int numberOfTroops)
		{
			SkillLevelingManager.Instance.OnUpgradeTroops(party, troop, upgrade, numberOfTroops);
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x000EAA77 File Offset: 0x000E8C77
		public static void OnPersuasionSucceeded(Hero targetHero, SkillObject skill, PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient)
		{
			SkillLevelingManager.Instance.OnPersuasionSucceeded(targetHero, skill, difficulty, argumentDifficultyBonusCoefficient);
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x000EAA87 File Offset: 0x000E8C87
		public static void OnPrisonBreakEnd(Hero prisonerHero, bool isSucceeded)
		{
			SkillLevelingManager.Instance.OnPrisonBreakEnd(prisonerHero, isSucceeded);
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x000EAA95 File Offset: 0x000E8C95
		public static void OnFoodConsumed(MobileParty mobileParty, bool wasStarving)
		{
			SkillLevelingManager.Instance.OnFoodConsumed(mobileParty, wasStarving);
		}

		// Token: 0x060038DE RID: 14558 RVA: 0x000EAAA3 File Offset: 0x000E8CA3
		public static void OnAlleyCleared(Alley alley)
		{
			SkillLevelingManager.Instance.OnAlleyCleared(alley);
		}

		// Token: 0x060038DF RID: 14559 RVA: 0x000EAAB0 File Offset: 0x000E8CB0
		public static void OnDailyAlleyTick(Alley alley, Hero alleyLeader)
		{
			SkillLevelingManager.Instance.OnDailyAlleyTick(alley, alleyLeader);
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x000EAABE File Offset: 0x000E8CBE
		public static void OnBoardGameWonAgainstLord(Hero lord, BoardGameHelper.AIDifficulty difficulty, bool extraXpGain)
		{
			SkillLevelingManager.Instance.OnBoardGameWonAgainstLord(lord, difficulty, extraXpGain);
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x000EAACD File Offset: 0x000E8CCD
		public static void OnProductionProducedToWarehouse(EquipmentElement production)
		{
			SkillLevelingManager.Instance.OnWarehouseProduction(production);
		}

		// Token: 0x060038E2 RID: 14562 RVA: 0x000EAADA File Offset: 0x000E8CDA
		public static void OnAIPartyLootCasualties(int goldAmount, Hero winnerPartyLeader, PartyBase defeatedParty)
		{
			SkillLevelingManager.Instance.OnAIPartyLootCasualties(goldAmount, winnerPartyLeader, defeatedParty);
		}

		// Token: 0x060038E3 RID: 14563 RVA: 0x000EAAE9 File Offset: 0x000E8CE9
		public static void OnShipDamaged(Ship ship, float rawDamage, float finalDamage)
		{
			SkillLevelingManager.Instance.OnShipDamaged(ship, rawDamage, finalDamage);
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x000EAAF8 File Offset: 0x000E8CF8
		public static void OnShipRepaired(Ship ship, float repairedHitPoints)
		{
			SkillLevelingManager.Instance.OnShipRepaired(ship, repairedHitPoints);
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x000EAB06 File Offset: 0x000E8D06
		public static void OnHighMorale(MobileParty mobileParty)
		{
			SkillLevelingManager.Instance.OnHighMorale(mobileParty);
		}
	}
}
