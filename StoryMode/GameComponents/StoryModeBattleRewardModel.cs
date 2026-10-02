using System;
using System.Collections.Generic;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace StoryMode.GameComponents
{
	// Token: 0x0200003E RID: 62
	public class StoryModeBattleRewardModel : BattleRewardModel
	{
		// Token: 0x06000424 RID: 1060 RVA: 0x00018FBE File Offset: 0x000171BE
		public override int CalculateGoldLossAfterDefeat(Hero partyLeaderHero)
		{
			return base.BaseModel.CalculateGoldLossAfterDefeat(partyLeaderHero);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00018FCC File Offset: 0x000171CC
		public override ExplainedNumber CalculateInfluenceGain(PartyBase winnerParty, float influenceValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, float influenceMultiplierForWinnerSide, bool includeDescriptions)
		{
			return base.BaseModel.CalculateInfluenceGain(winnerParty, influenceValueOfBattleForWinnerSide, contributionShareOfWinnerParty, influenceMultiplierForWinnerSide, includeDescriptions);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00018FE0 File Offset: 0x000171E0
		public override float CalculateMoraleChangeOnRoundVictory(PartyBase party, MapEventSide partySide, BattleSideEnum roundWinner)
		{
			return base.BaseModel.CalculateMoraleChangeOnRoundVictory(party, partySide, roundWinner);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00018FF0 File Offset: 0x000171F0
		public override ExplainedNumber CalculateMoraleGainVictory(PartyBase winnerParty, float renownValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, bool includeDescriptions)
		{
			return base.BaseModel.CalculateMoraleGainVictory(winnerParty, renownValueOfBattleForWinnerSide, contributionShareOfWinnerParty, includeDescriptions);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00019002 File Offset: 0x00017202
		public override int CalculatePlunderedGoldAmountFromDefeatedParty(PartyBase defeatedParty)
		{
			return base.BaseModel.CalculatePlunderedGoldAmountFromDefeatedParty(defeatedParty);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00019010 File Offset: 0x00017210
		public override ExplainedNumber CalculateRenownGain(PartyBase winnerParty, float renownValueOfBattleForWinnerSide, float contributionShareOfWinnerParty, float renownMultiplierForWinnerSide, bool includeDescriptions)
		{
			if (TutorialPhase.Instance != null && !TutorialPhase.Instance.IsCompleted && winnerParty == PartyBase.MainParty)
			{
				return default(ExplainedNumber);
			}
			return base.BaseModel.CalculateRenownGain(winnerParty, renownValueOfBattleForWinnerSide, contributionShareOfWinnerParty, renownMultiplierForWinnerSide, includeDescriptions);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00019054 File Offset: 0x00017254
		public override float CalculateShipDamageAfterDefeat(Ship ship)
		{
			return 0f;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001905B File Offset: 0x0001725B
		public override MBReadOnlyList<KeyValuePair<Ship, MapEventParty>> DistributeDefeatedPartyShipsAmongWinners(MapEvent mapEvent, MBReadOnlyList<Ship> shipsToLoot, MBReadOnlyList<MapEventParty> winnerParties)
		{
			return new MBReadOnlyList<KeyValuePair<Ship, MapEventParty>>();
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00019062 File Offset: 0x00017262
		public override float GetAITradePenalty()
		{
			return base.BaseModel.GetAITradePenalty();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001906F File Offset: 0x0001726F
		public override float GetBannerLootChanceFromDefeatedHero(Hero defeatedHero)
		{
			return base.BaseModel.GetBannerLootChanceFromDefeatedHero(defeatedHero);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001907D File Offset: 0x0001727D
		public override ItemObject GetBannerRewardForWinningMapEvent(MapEvent mapEvent)
		{
			return base.BaseModel.GetBannerRewardForWinningMapEvent(mapEvent);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001908B File Offset: 0x0001728B
		public override float GetExpectedLootedItemValueFromCasualty(Hero winnerPartyLeaderHero, CharacterObject casualtyCharacter)
		{
			return base.BaseModel.GetExpectedLootedItemValueFromCasualty(winnerPartyLeaderHero, casualtyCharacter);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001909A File Offset: 0x0001729A
		public override Figurehead GetFigureheadLoot(MBReadOnlyList<MapEventParty> defeatedParties, PartyBase defeatedSideLeaderParty)
		{
			return base.BaseModel.GetFigureheadLoot(defeatedParties, defeatedSideLeaderParty);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x000190A9 File Offset: 0x000172A9
		public override MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootCasualtyChances(MBReadOnlyList<MapEventParty> winnerParties, PartyBase defeatedParty)
		{
			return base.BaseModel.GetLootCasualtyChances(winnerParties, defeatedParty);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000190B8 File Offset: 0x000172B8
		public override EquipmentElement GetLootedItemFromTroop(CharacterObject character, float targetValue)
		{
			return base.BaseModel.GetLootedItemFromTroop(character, targetValue);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000190C7 File Offset: 0x000172C7
		public override MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootGoldChances(MBReadOnlyList<MapEventParty> winnerParties)
		{
			return base.BaseModel.GetLootGoldChances(winnerParties);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000190D5 File Offset: 0x000172D5
		public override MBList<KeyValuePair<MapEventParty, float>> GetLootItemChancesForWinnerParties(MBReadOnlyList<MapEventParty> winnerParties, PartyBase defeatedParty)
		{
			return base.BaseModel.GetLootItemChancesForWinnerParties(winnerParties, defeatedParty);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000190E4 File Offset: 0x000172E4
		public override void GetCaptureMemberChancesForWinnerParties(MapEvent endedMapEvent, MBReadOnlyList<MapEventParty> winnerParties, out MBList<KeyValuePair<MapEventParty, float>> woundedMemberChances, out MBList<KeyValuePair<MapEventParty, float>> healthyMemberChances)
		{
			base.BaseModel.GetCaptureMemberChancesForWinnerParties(endedMapEvent, winnerParties, out woundedMemberChances, out healthyMemberChances);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000190F8 File Offset: 0x000172F8
		public override MBReadOnlyList<KeyValuePair<MapEventParty, float>> GetLootPrisonerChances(MBReadOnlyList<MapEventParty> winnerParties, TroopRosterElement prisonerElement)
		{
			if (StoryModeData.IsConspiracyTroop(prisonerElement.Character))
			{
				MBList<KeyValuePair<MapEventParty, float>> mblist = new MBList<KeyValuePair<MapEventParty, float>>();
				foreach (MapEventParty mapEventParty in winnerParties)
				{
					mblist.Add(new KeyValuePair<MapEventParty, float>(mapEventParty, 0f));
				}
				return mblist;
			}
			return base.BaseModel.GetLootPrisonerChances(winnerParties, prisonerElement);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00019174 File Offset: 0x00017374
		public override float GetMainPartyMemberScatterChance()
		{
			return base.BaseModel.GetMainPartyMemberScatterChance();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00019181 File Offset: 0x00017381
		public override int GetPlayerGainedRelationAmount(MapEvent mapEvent, Hero hero)
		{
			return base.BaseModel.GetPlayerGainedRelationAmount(mapEvent, hero);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00019190 File Offset: 0x00017390
		public override float GetShipSiegeEngineHitMoraleEffect(Ship ship, SiegeEngineType siegeEngineType)
		{
			return base.BaseModel.GetShipSiegeEngineHitMoraleEffect(ship, siegeEngineType);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0001919F File Offset: 0x0001739F
		public override float GetSunkenShipMoraleEffect(PartyBase shipOwner, Ship ship)
		{
			return base.BaseModel.GetSunkenShipMoraleEffect(shipOwner, ship);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000191AE File Offset: 0x000173AE
		public override MBReadOnlyList<MapEventParty> GetWinnerPartiesThatCanPlunderGoldFromShips(MBReadOnlyList<MapEventParty> winnerParties)
		{
			return base.BaseModel.GetWinnerPartiesThatCanPlunderGoldFromShips(winnerParties);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000191BC File Offset: 0x000173BC
		public override bool CanTroopBeTakenPrisoner(CharacterObject troop)
		{
			return !StoryModeData.IsConspiracyTroop(troop) && base.BaseModel.CanTroopBeTakenPrisoner(troop);
		}
	}
}
