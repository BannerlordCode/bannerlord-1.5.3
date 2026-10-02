using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace Helpers
{
	// Token: 0x02000014 RID: 20
	public static class MapEventHelper
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x000090DC File Offset: 0x000072DC
		public static bool GetRaidContext(MapEvent mapEvent, out BattleSideEnum raiderSide, out bool raiderHasSeaPresence, out bool raiderHasLandPresence, out bool villageFactionSideHasSeaPresence, out bool villageFactionSideHasLandPresence, out bool wasEverInLootingPhase)
		{
			raiderSide = BattleSideEnum.None;
			raiderHasSeaPresence = false;
			raiderHasLandPresence = false;
			villageFactionSideHasSeaPresence = false;
			villageFactionSideHasLandPresence = false;
			wasEverInLootingPhase = false;
			if (mapEvent == null || mapEvent.MapEventSettlement == null || !mapEvent.MapEventSettlement.IsVillage)
			{
				return false;
			}
			IFaction mapFaction = mapEvent.MapEventSettlement.MapFaction;
			raiderSide = (mapEvent.AttackerSide.LeaderParty.MapFaction.IsAtWarWith(mapFaction) ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
			BattleSideEnum otherSide = mapEvent.GetOtherSide(raiderSide);
			MBReadOnlyList<MapEventParty> mbreadOnlyList = mapEvent.PartiesOnSide(raiderSide);
			for (int i = 0; i < mbreadOnlyList.Count; i++)
			{
				PartyBase party = mbreadOnlyList[i].Party;
				if (party.IsMobile && party.NumberOfHealthyMembers > 0)
				{
					if (party.MobileParty.IsCurrentlyAtSea)
					{
						raiderHasSeaPresence = true;
					}
					else
					{
						raiderHasLandPresence = true;
					}
				}
				if (raiderHasSeaPresence & raiderHasLandPresence)
				{
					break;
				}
			}
			MBReadOnlyList<MapEventParty> mbreadOnlyList2 = mapEvent.PartiesOnSide(otherSide);
			for (int j = 0; j < mbreadOnlyList2.Count; j++)
			{
				PartyBase party2 = mbreadOnlyList2[j].Party;
				if (party2.IsMobile && party2.NumberOfHealthyMembers > 0)
				{
					if (party2.MobileParty.IsCurrentlyAtSea)
					{
						villageFactionSideHasSeaPresence = true;
					}
					else
					{
						villageFactionSideHasLandPresence = true;
					}
				}
				if (villageFactionSideHasSeaPresence & villageFactionSideHasLandPresence)
				{
					break;
				}
			}
			wasEverInLootingPhase = mapEvent.WasEverInLootingPhase || (mapEvent == MapEvent.PlayerMapEvent && PlayerEncounter.Current != null && PlayerEncounter.Current.InterruptedWhileLooting);
			return true;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00009234 File Offset: 0x00007434
		public static bool IsNavalRaid(MapEvent mapEvent)
		{
			BattleSideEnum battleSideEnum;
			bool flag;
			bool flag2;
			bool flag3;
			bool flag4;
			bool flag5;
			return MapEventHelper.GetRaidContext(mapEvent, out battleSideEnum, out flag, out flag2, out flag3, out flag4, out flag5) && ((!flag5 && flag) || (flag5 && flag3 && (!flag || flag2 || flag4)));
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00009274 File Offset: 0x00007474
		public static PartyBase GetSallyOutDefenderLeader()
		{
			PartyBase partyBase;
			if (MobileParty.MainParty.CurrentSettlement.Town.GarrisonParty != null)
			{
				partyBase = MobileParty.MainParty.CurrentSettlement.Town.GarrisonParty.MapEvent.DefenderSide.LeaderParty;
			}
			else
			{
				PartyBase party = MobileParty.MainParty.CurrentSettlement.Party;
				if (((party != null) ? party.MapEvent : null) != null)
				{
					partyBase = MobileParty.MainParty.CurrentSettlement.Party.MapEvent.DefenderSide.LeaderParty;
				}
				else
				{
					partyBase = MobileParty.MainParty.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Party;
				}
			}
			return partyBase;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000931C File Offset: 0x0000751C
		public static bool CanMainPartyLeaveBattleCommonCondition()
		{
			return MobileParty.MainParty.MapEvent.PlayerSide != BattleSideEnum.Defender || (MobileParty.MainParty.SiegeEvent != null && !MobileParty.MainParty.SiegeEvent.BesiegerCamp.IsBesiegerSideParty(MobileParty.MainParty) && MobileParty.MainParty.CurrentSettlement == null);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00009372 File Offset: 0x00007572
		public static PartyBase GetEncounteredPartyBase(PartyBase attackerParty, PartyBase defenderParty)
		{
			if (attackerParty == PartyBase.MainParty || defenderParty == PartyBase.MainParty)
			{
				if (attackerParty != PartyBase.MainParty)
				{
					return attackerParty;
				}
				return defenderParty;
			}
			else
			{
				if (defenderParty.MapEvent == null)
				{
					return attackerParty;
				}
				return defenderParty;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000939C File Offset: 0x0000759C
		public static void OnConversationEnd()
		{
			if (PlayerEncounter.Current != null && ((PlayerEncounter.EncounteredMobileParty != null && PlayerEncounter.EncounteredMobileParty.MapFaction != null && !PlayerEncounter.EncounteredMobileParty.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction)) || (PlayerEncounter.EncounteredParty != null && PlayerEncounter.EncounteredParty.MapFaction != null && !PlayerEncounter.EncounteredParty.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))))
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00009414 File Offset: 0x00007614
		public static FlattenedTroopRoster GetPriorityListForHideoutMission(List<MobileParty> partyList, out int firstPhaseTroopCount)
		{
			int num = partyList.SumQ<MobileParty>((MobileParty x) => x.Party.MemberRoster.TotalHealthyCount);
			firstPhaseTroopCount = MathF.Min(MathF.Floor((float)num * Campaign.Current.Models.BanditDensityModel.SpawnPercentageForFirstFightInHideoutMission), Campaign.Current.Models.BanditDensityModel.NumberOfMaximumTroopCountForFirstFightInHideout);
			int num2 = num - firstPhaseTroopCount;
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(num);
			foreach (MobileParty mobileParty in partyList)
			{
				flattenedTroopRoster.Add(mobileParty.Party.MemberRoster.GetTroopRoster());
			}
			flattenedTroopRoster.RemoveIf((FlattenedTroopRosterElement x) => x.IsWounded);
			int count = flattenedTroopRoster.RemoveIf((FlattenedTroopRosterElement x) => x.Troop.IsHero || x.Troop.Culture.BanditBoss == x.Troop).ToList<FlattenedTroopRosterElement>().Count;
			int num3 = 0;
			int num4 = num2 - count;
			if (num4 > 0)
			{
				IEnumerable<FlattenedTroopRosterElement> selectedRegularTroops = flattenedTroopRoster.OrderByDescending<FlattenedTroopRosterElement, int>((FlattenedTroopRosterElement x) => x.Troop.Level).Take<FlattenedTroopRosterElement>(num4);
				flattenedTroopRoster.RemoveIf((FlattenedTroopRosterElement x) => selectedRegularTroops.Contains(x));
				num3 += selectedRegularTroops.Count<FlattenedTroopRosterElement>();
			}
			Debug.Print("Picking bandit troops for hideout mission...", 0, Debug.DebugColor.Yellow, 256UL);
			Debug.Print("- First phase troop count: " + firstPhaseTroopCount, 0, Debug.DebugColor.Yellow, 256UL);
			Debug.Print("- Second phase boss troop count: " + count, 0, Debug.DebugColor.Yellow, 256UL);
			Debug.Print("- Second phase regular troop count: " + num3, 0, Debug.DebugColor.Yellow, 256UL);
			return flattenedTroopRoster;
		}
	}
}
