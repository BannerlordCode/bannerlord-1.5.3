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
	// Token: 0x020003C6 RID: 966
	public interface ISkillLevelingManager
	{
		// Token: 0x06003883 RID: 14467
		void OnCombatHit(CharacterObject affectorCharacter, CharacterObject affectedCharacter, CharacterObject captain, Hero commander, float speedBonusFromMovement, float shotDifficulty, WeaponComponentData affectorWeapon, float hitPointRatio, CombatXpModel.MissionTypeEnum missionType, bool isAffectorMounted, bool isTeamKill, bool isAffectorUnderCommand, float damageAmount, bool isFatal, bool isSiegeEngineHit, bool isHorseCharge, bool isSneakAttack);

		// Token: 0x06003884 RID: 14468
		void OnSiegeEngineDestroyed(MobileParty party, SiegeEngineType destroyedSiegeEngine);

		// Token: 0x06003885 RID: 14469
		void OnSimulationCombatKill(CharacterObject affectorCharacter, CharacterObject affectedCharacter, PartyBase affectorParty, PartyBase commanderParty);

		// Token: 0x06003886 RID: 14470
		void OnTradeProfitMade(PartyBase party, int tradeProfit);

		// Token: 0x06003887 RID: 14471
		void OnTradeProfitMade(Hero hero, int tradeProfit);

		// Token: 0x06003888 RID: 14472
		void OnSettlementProjectFinished(Settlement settlement);

		// Token: 0x06003889 RID: 14473
		void OnSettlementGoverned(Hero governor, Settlement settlement);

		// Token: 0x0600388A RID: 14474
		void OnInfluenceSpent(Hero hero, float amountSpent);

		// Token: 0x0600388B RID: 14475
		void OnGainRelation(Hero hero, Hero gainedRelationWith, float relationChange, ChangeRelationAction.ChangeRelationDetail detail = ChangeRelationAction.ChangeRelationDetail.Default);

		// Token: 0x0600388C RID: 14476
		void OnTroopRecruited(Hero hero, int amount, int tier);

		// Token: 0x0600388D RID: 14477
		void OnBribeGiven(int amount);

		// Token: 0x0600388E RID: 14478
		void OnWarehouseProduction(EquipmentElement production);

		// Token: 0x0600388F RID: 14479
		void OnAIPartyLootCasualties(int goldAmount, Hero winnerPartyLeader, PartyBase defeatedParty);

		// Token: 0x06003890 RID: 14480
		void OnBanditsRecruited(MobileParty mobileParty, CharacterObject bandit, int count);

		// Token: 0x06003891 RID: 14481
		void OnMainHeroReleasedFromCaptivity(float captivityTime);

		// Token: 0x06003892 RID: 14482
		void OnMainHeroTortured();

		// Token: 0x06003893 RID: 14483
		void OnMainHeroDisguised(bool isNotCaught);

		// Token: 0x06003894 RID: 14484
		void OnRaid(MobileParty attackerParty, ItemRoster lootedItems);

		// Token: 0x06003895 RID: 14485
		void OnLoot(MobileParty attackerParty, MobileParty forcedParty, ItemRoster lootedItems, bool attacked);

		// Token: 0x06003896 RID: 14486
		void OnPrisonerSell(MobileParty mobileParty, in TroopRoster prisonerRoster);

		// Token: 0x06003897 RID: 14487
		void OnSurgeryApplied(MobileParty party, bool surgerySuccess, int troopTier);

		// Token: 0x06003898 RID: 14488
		void OnTacticsUsed(MobileParty party, float xp);

		// Token: 0x06003899 RID: 14489
		void OnHideoutSpotted(MobileParty party, PartyBase spottedParty);

		// Token: 0x0600389A RID: 14490
		void OnTrackDetected(Track track);

		// Token: 0x0600389B RID: 14491
		void OnTravelOnFoot(Hero hero);

		// Token: 0x0600389C RID: 14492
		void OnTravelOnHorse(Hero hero);

		// Token: 0x0600389D RID: 14493
		void OnTravelOnWater(MobileParty party);

		// Token: 0x0600389E RID: 14494
		void OnHeroHealedWhileWaiting(Hero hero, int healingAmount);

		// Token: 0x0600389F RID: 14495
		void OnRegularTroopHealedWhileWaiting(MobileParty mobileParty, int healedTroopCount, float averageTier);

		// Token: 0x060038A0 RID: 14496
		void OnLeadingArmy(MobileParty mobileParty);

		// Token: 0x060038A1 RID: 14497
		void OnSieging(MobileParty mobileParty);

		// Token: 0x060038A2 RID: 14498
		void OnSiegeEngineBuilt(MobileParty mobileParty, SiegeEngineType siegeEngine);

		// Token: 0x060038A3 RID: 14499
		void OnUpgradeTroops(PartyBase party, CharacterObject troop, CharacterObject upgrade, int numberOfTroops);

		// Token: 0x060038A4 RID: 14500
		void OnPersuasionSucceeded(Hero targetHero, SkillObject skill, PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient);

		// Token: 0x060038A5 RID: 14501
		void OnPrisonBreakEnd(Hero prisonerHero, bool isSucceeded);

		// Token: 0x060038A6 RID: 14502
		void OnWallBreached(MobileParty party);

		// Token: 0x060038A7 RID: 14503
		void OnForceVolunteers(MobileParty attackerParty, PartyBase forcedParty);

		// Token: 0x060038A8 RID: 14504
		void OnForceSupplies(MobileParty attackerParty, ItemRoster lootedItems, bool attacked);

		// Token: 0x060038A9 RID: 14505
		void OnAIPartiesTravel(Hero hero, bool isCaravanParty, TerrainType currentTerrainType);

		// Token: 0x060038AA RID: 14506
		void OnTraverseTerrain(MobileParty mobileParty, TerrainType currentTerrainType);

		// Token: 0x060038AB RID: 14507
		void OnBattleEnded(PartyBase party, CharacterObject troop, int excessXp);

		// Token: 0x060038AC RID: 14508
		void OnFoodConsumed(MobileParty mobileParty, bool wasStarving);

		// Token: 0x060038AD RID: 14509
		void OnAlleyCleared(Alley alley);

		// Token: 0x060038AE RID: 14510
		void OnDailyAlleyTick(Alley alley, Hero alleyLeader);

		// Token: 0x060038AF RID: 14511
		void OnBoardGameWonAgainstLord(Hero lord, BoardGameHelper.AIDifficulty difficulty, bool extraXpGain);

		// Token: 0x060038B0 RID: 14512
		void OnShipDamaged(Ship ship, float rawDamage, float finalDamage);

		// Token: 0x060038B1 RID: 14513
		void OnShipRepaired(Ship ship, float repairedHitPoints);

		// Token: 0x060038B2 RID: 14514
		void OnHideoutMissionEnd(bool isSucceeded);

		// Token: 0x060038B3 RID: 14515
		void OnHideoutClearedAsGhost();

		// Token: 0x060038B4 RID: 14516
		void OnHighMorale(MobileParty mobileParty);
	}
}
