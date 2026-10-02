using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C1 RID: 449
	public abstract class BattleRewardModel : MBGameModel<BattleRewardModel>
	{
		// Token: 0x06001E39 RID: 7737
		public abstract float GetBannerLootChanceFromDefeatedHero(Hero defeatedHero);

		// Token: 0x06001E3A RID: 7738
		public abstract ItemObject GetBannerRewardForWinningMapEvent(MapEvent mapEvent);

		// Token: 0x06001E3B RID: 7739
		public abstract int GetPlayerGainedRelationAmount(MapEvent mapEvent, Hero hero);

		// Token: 0x06001E3C RID: 7740
		public abstract ExplainedNumber CalculateRenownGain(PartyBase winnerParty, float renownValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, float renownMultiplierForWinnerSide, bool includeDescriptions);

		// Token: 0x06001E3D RID: 7741
		public abstract ExplainedNumber CalculateInfluenceGain(PartyBase winnerParty, float influenceValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, float influenceMultiplierForWinnerSide, bool includeDescriptions);

		// Token: 0x06001E3E RID: 7742
		public abstract ExplainedNumber CalculateMoraleGainVictory(PartyBase winnerParty, float renownValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, bool includeDescriptions);

		// Token: 0x06001E3F RID: 7743
		public abstract float CalculateMoraleChangeOnRoundVictory(PartyBase party, MapEventSide partySide, BattleSideEnum roundWinner);

		// Token: 0x06001E40 RID: 7744
		public abstract int CalculateGoldLossAfterDefeat(Hero partyLeaderHero);

		// Token: 0x06001E41 RID: 7745
		public abstract EquipmentElement GetLootedItemFromTroop(CharacterObject character, float targetValue);

		// Token: 0x06001E42 RID: 7746
		public abstract float GetExpectedLootedItemValueFromCasualty(Hero winnerPartyLeaderHero, CharacterObject casualtyCharacter);

		// Token: 0x06001E43 RID: 7747
		public abstract int CalculatePlunderedGoldAmountFromDefeatedParty(PartyBase defeatedParty);

		// Token: 0x06001E44 RID: 7748
		public abstract MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootGoldChances(MBReadOnlyList<MapEventParty> winnerParties);

		// Token: 0x06001E45 RID: 7749
		public abstract float GetMainPartyMemberScatterChance();

		// Token: 0x06001E46 RID: 7750
		public abstract float GetAITradePenalty();

		// Token: 0x06001E47 RID: 7751
		public abstract void GetCaptureMemberChancesForWinnerParties(MapEvent endedMapEvent, MBReadOnlyList<MapEventParty> winnerParties, out MBList<KeyValuePair<MapEventParty, float>> woundedMemberChances, out MBList<KeyValuePair<MapEventParty, float>> healthyMemberChances);

		// Token: 0x06001E48 RID: 7752
		public abstract MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootPrisonerChances(MBReadOnlyList<MapEventParty> winnerParties, TroopRosterElement prisonerElement);

		// Token: 0x06001E49 RID: 7753
		public abstract MBList<KeyValuePair<MapEventParty, float>> GetLootItemChancesForWinnerParties(MBReadOnlyList<MapEventParty> winnerParties, PartyBase defeatedParty);

		// Token: 0x06001E4A RID: 7754
		public abstract MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootCasualtyChances(MBReadOnlyList<MapEventParty> winnerParties, PartyBase defeatedParty);

		// Token: 0x06001E4B RID: 7755
		public abstract float CalculateShipDamageAfterDefeat(Ship ship);

		// Token: 0x06001E4C RID: 7756
		public abstract MBReadOnlyList<KeyValuePair<Ship, MapEventParty>> DistributeDefeatedPartyShipsAmongWinners(MapEvent mapEvent, MBReadOnlyList<Ship> shipsToLoot, MBReadOnlyList<MapEventParty> winnerParties);

		// Token: 0x06001E4D RID: 7757
		public abstract float GetSunkenShipMoraleEffect(PartyBase shipOwner, Ship ship);

		// Token: 0x06001E4E RID: 7758
		public abstract float GetShipSiegeEngineHitMoraleEffect(Ship ship, SiegeEngineType siegeEngineType);

		// Token: 0x06001E4F RID: 7759
		public abstract Figurehead GetFigureheadLoot(MBReadOnlyList<MapEventParty> defeatedParties, PartyBase defeatedSideLeaderParty);

		// Token: 0x06001E50 RID: 7760
		public abstract MBReadOnlyList<MapEventParty> GetWinnerPartiesThatCanPlunderGoldFromShips(MBReadOnlyList<MapEventParty> winnerParties);

		// Token: 0x06001E51 RID: 7761
		public abstract bool CanTroopBeTakenPrisoner(CharacterObject troop);
	}
}
