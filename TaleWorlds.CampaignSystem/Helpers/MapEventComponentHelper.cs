using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Helpers
{
	// Token: 0x02000015 RID: 21
	public static class MapEventComponentHelper
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00009610 File Offset: 0x00007810
		public static void AddInsideSettlementParties(MapEvent mapEvent)
		{
			List<PartyBase> list = new List<PartyBase>();
			foreach (PartyBase partyBase in mapEvent.MapEventSettlement.GetInvolvedPartiesForEventType(mapEvent.EventType))
			{
				if (partyBase != PartyBase.MainParty)
				{
					MobileParty mobileParty = partyBase.MobileParty;
					if (((mobileParty != null) ? mobileParty.AttachedTo : null) != MobileParty.MainParty)
					{
						list.Add(partyBase);
					}
				}
			}
			foreach (PartyBase partyBase2 in list)
			{
				if (mapEvent.CanPartyJoinBattle(partyBase2, BattleSideEnum.Defender))
				{
					partyBase2.MapEventSide = mapEvent.DefenderSide;
				}
				else if (mapEvent.CanPartyJoinBattle(partyBase2, BattleSideEnum.Attacker))
				{
					partyBase2.MapEventSide = mapEvent.AttackerSide;
				}
				else if (partyBase2.MobileParty != null && !partyBase2.MobileParty.IsGarrison && !partyBase2.MobileParty.IsMilitia)
				{
					LeaveSettlementAction.ApplyForParty(partyBase2.MobileParty);
				}
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000972C File Offset: 0x0000792C
		public static void AddNearbyPartiesToPlayerMapEvent(MapEvent mapEvent)
		{
			List<MobileParty> list = new List<MobileParty>();
			List<MobileParty> list2 = new List<MobileParty>();
			foreach (MapEventParty mapEventParty in mapEvent.PartiesOnSide(mapEvent.PlayerSide))
			{
				if (mapEventParty.Party.IsMobile)
				{
					list.Add(mapEventParty.Party.MobileParty);
				}
			}
			foreach (MapEventParty mapEventParty2 in mapEvent.PartiesOnSide(mapEvent.PlayerSide.GetOppositeSide()))
			{
				if (mapEventParty2.Party.IsMobile)
				{
					list2.Add(mapEventParty2.Party.MobileParty);
				}
			}
			PlayerEncounter.Current.FindAllNpcPartiesWhoWillJoinEvent(list, list2);
			foreach (MobileParty mobileParty in list)
			{
				mapEvent.GetMapEventSide(mapEvent.PlayerSide).AddNearbyPartyToPlayerMapEvent(mobileParty);
			}
			foreach (MobileParty mobileParty2 in list2)
			{
				mapEvent.GetMapEventSide(mapEvent.PlayerSide.GetOppositeSide()).AddNearbyPartyToPlayerMapEvent(mobileParty2);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000098B8 File Offset: 0x00007AB8
		public static void PlayerEncounterDoWaitCommon(MapEvent mapEvent, CampaignBattleResult campaignBattleResult, out PlayerEncounterState nextEncounterState, out bool stateHandled)
		{
			nextEncounterState = PlayerEncounter.Current.EncounterState;
			stateHandled = false;
			if (campaignBattleResult != null && campaignBattleResult.BattleResolved)
			{
				if (campaignBattleResult.PlayerVictory)
				{
					if (mapEvent != null)
					{
						mapEvent.SetOverrideWinner(PartyBase.MainParty.Side);
					}
				}
				else if (mapEvent != null)
				{
					mapEvent.SetOverrideWinner(PartyBase.MainParty.OpponentSide);
				}
				nextEncounterState = PlayerEncounterState.PrepareResults;
				return;
			}
			if (PlayerEncounter.Current.BattleSimulation != null && (PlayerEncounter.BattleState == BattleState.AttackerVictory || PlayerEncounter.BattleState == BattleState.DefenderVictory))
			{
				if (mapEvent.WinningSide == PlayerEncounter.Current.PlayerSide && PlayerEncounter.Battle.RetreatingSide == BattleSideEnum.None)
				{
					PlayerEncounter.EnemySurrender = true;
				}
				else
				{
					bool totalManCount = MobileParty.MainParty.MemberRoster.TotalManCount != 0;
					int totalWounded = MobileParty.MainParty.MemberRoster.TotalWounded;
					if ((totalManCount ? 1 : 0) - totalWounded == 0)
					{
						PlayerEncounter.PlayerSurrender = true;
					}
				}
				nextEncounterState = PlayerEncounterState.PrepareResults;
				return;
			}
			if (mapEvent != null && PlayerEncounter.PlayerSurrender && mapEvent.HasWinner)
			{
				nextEncounterState = PlayerEncounterState.PrepareResults;
				return;
			}
			stateHandled = true;
			if (PlayerEncounter.Current.IsJoinedBattle && Campaign.Current.CurrentMenuContext != null && Campaign.Current.CurrentMenuContext.GameMenu.StringId == "join_encounter")
			{
				PlayerEncounter.LeaveBattle();
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000099D9 File Offset: 0x00007BD9
		public static void OnPlayerEncounterContinueBattleCommon(MapEvent mapEvent, CampaignBattleResult campaignBattleResult, out PlayerEncounterState nextEncounterState, out bool stateHandled)
		{
			nextEncounterState = PlayerEncounter.Current.EncounterState;
			mapEvent.ApplyGainedVariablesOnPlayerBattleContinues();
			mapEvent.SetOverrideWinner(BattleSideEnum.None);
			stateHandled = true;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000099F8 File Offset: 0x00007BF8
		public static void OnPlayerEncounterContinueNavalBattleCommon(MapEvent mapEvent, CampaignBattleResult campaignBattleResult, out PlayerEncounterState nextEncounterState)
		{
			MapEventSide mapEventSide = mapEvent.GetMapEventSide(mapEvent.PlayerSide);
			MapEventSide otherSide = mapEventSide.OtherSide;
			if (otherSide.Parties.Sum<MapEventParty>((MapEventParty x) => x.Ships.Count) == 0)
			{
				Debug.FailedAssert("This case should not be called anymore, make sure this is intended", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "OnPlayerEncounterContinueNavalBattleCommon", 4748);
				Debug.Print("Player side wins according to the strength ratio.", 0, Debug.DebugColor.White, 17592186044416UL);
				if (mapEvent != null)
				{
					mapEvent.SetOverrideWinner(mapEvent.PlayerSide);
				}
				PlayerEncounter.EnemySurrender = true;
				nextEncounterState = PlayerEncounterState.PrepareResults;
				return;
			}
			if (mapEventSide.Parties.Sum<MapEventParty>((MapEventParty x) => x.Ships.Count) == 0)
			{
				Debug.FailedAssert("This case should not be called anymore, make sure this is intended", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "OnPlayerEncounterContinueNavalBattleCommon", 4757);
				Debug.Print("Other side wins according to the strength ratio.", 0, Debug.DebugColor.White, 17592186044416UL);
				if (mapEvent != null)
				{
					mapEvent.SetOverrideWinner(otherSide.MissionSide);
				}
				nextEncounterState = PlayerEncounterState.PrepareResults;
				return;
			}
			nextEncounterState = PlayerEncounter.Current.EncounterState;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00009B08 File Offset: 0x00007D08
		public static bool CheckIfBattleShouldContinueAfterBattleMissionCommonCondition(MapEvent mapEvent, CampaignBattleResult campaignBattleResult)
		{
			MapEventSide mapEventSide = mapEvent.GetMapEventSide(mapEvent.PlayerSide);
			if (PlayerEncounter.PlayerSurrender || campaignBattleResult == null || campaignBattleResult.EnemyRetreated)
			{
				return false;
			}
			bool flag = !mapEvent.CheckIfOneSideHasLost();
			if (mapEvent.DefeatedSide != BattleSideEnum.None)
			{
				MapEventSide mapEventSide2 = mapEvent.GetMapEventSide(mapEvent.DefeatedSide);
				bool flag2 = campaignBattleResult.PlayerDefeat || campaignBattleResult.PlayerVictory || campaignBattleResult.EnemyPulledBack;
				bool flag3 = mapEventSide2.GetTotalHealthyTroopCountOfSide() + mapEventSide2.GetTotalHealthyHeroCountOfSide() >= 1;
				flag = flag2 && flag3;
			}
			return flag && !mapEventSide.IsSurrendered;
		}
	}
}
