using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F6 RID: 1014
	public class CampaignWarManagerBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003DA9 RID: 15785 RVA: 0x00101F36 File Offset: 0x00100136
		public override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventEnded));
			CampaignEvents.RaidCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, RaidEventComponent>(this.OnRaidCompleted));
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x00101F68 File Offset: 0x00100168
		private void OnRaidCompleted(BattleSideEnum winnerSide, RaidEventComponent raidEvent)
		{
			if (raidEvent.AttackerSide.LeaderParty.MapFaction != null && !raidEvent.AttackerSide.LeaderParty.MapFaction.IsBanditFaction && raidEvent.DefenderSide.LeaderParty.MapFaction != null && !raidEvent.DefenderSide.LeaderParty.MapFaction.IsBanditFaction)
			{
				IFaction mapFaction = raidEvent.AttackerSide.MapFaction;
				IFaction mapFaction2 = raidEvent.DefenderSide.MapFaction;
				if (mapFaction.MapFaction != mapFaction2.MapFaction)
				{
					StanceLink stanceWith = mapFaction.GetStanceWith(mapFaction2);
					if (raidEvent.MapEventSettlement != null && raidEvent.BattleState == BattleState.AttackerVictory && raidEvent.MapEventSettlement.IsVillage && raidEvent.MapEventSettlement.Village.VillageState == Village.VillageStates.Looted)
					{
						int num;
						if (mapFaction == stanceWith.Faction1)
						{
							StanceLink stanceLink = stanceWith;
							num = stanceLink.SuccessfulRaids1;
							stanceLink.SuccessfulRaids1 = num + 1;
							return;
						}
						StanceLink stanceLink2 = stanceWith;
						num = stanceLink2.SuccessfulRaids2;
						stanceLink2.SuccessfulRaids2 = num + 1;
					}
				}
			}
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x0010205C File Offset: 0x0010025C
		private void MapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.AttackerSide.LeaderParty.MapFaction != null && !mapEvent.AttackerSide.LeaderParty.MapFaction.IsBanditFaction && mapEvent.DefenderSide.LeaderParty.MapFaction != null && !mapEvent.DefenderSide.LeaderParty.MapFaction.IsBanditFaction)
			{
				IFaction mapFaction = mapEvent.AttackerSide.MapFaction;
				IFaction mapFaction2 = mapEvent.DefenderSide.MapFaction;
				if (mapFaction.MapFaction != mapFaction2.MapFaction)
				{
					StanceLink stanceWith = mapFaction.GetStanceWith(mapFaction2);
					stanceWith.TroopCasualties1 += ((stanceWith.Faction1 == mapFaction) ? mapEvent.AttackerSide.TroopCasualties : mapEvent.DefenderSide.TroopCasualties);
					stanceWith.TroopCasualties2 += ((stanceWith.Faction2 == mapFaction) ? mapEvent.AttackerSide.TroopCasualties : mapEvent.DefenderSide.TroopCasualties);
					stanceWith.ShipCasualties1 += ((stanceWith.Faction1 == mapFaction) ? mapEvent.AttackerSide.ShipCasualties : mapEvent.DefenderSide.ShipCasualties);
					stanceWith.ShipCasualties2 += ((stanceWith.Faction2 == mapFaction) ? mapEvent.AttackerSide.ShipCasualties : mapEvent.DefenderSide.ShipCasualties);
					if (mapEvent.MapEventSettlement != null && mapEvent.MapEventSettlement.IsFortification)
					{
						if (mapEvent.EventType == MapEvent.BattleTypes.Siege && mapEvent.BattleState == BattleState.AttackerVictory)
						{
							this.IncreaseSuccessfulSiegeCount(mapFaction, stanceWith, mapEvent);
							return;
						}
						if (mapEvent.EventType == MapEvent.BattleTypes.SallyOut && mapEvent.BattleState == BattleState.DefenderVictory)
						{
							this.IncreaseSuccessfulSiegeCount(mapFaction2, stanceWith, mapEvent);
						}
					}
				}
			}
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x001021FC File Offset: 0x001003FC
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x00102200 File Offset: 0x00100400
		private void IncreaseSuccessfulSiegeCount(IFaction faction, StanceLink stance, MapEvent mapEvent)
		{
			if (faction == stance.Faction1)
			{
				int num = stance.SuccessfulSieges1;
				stance.SuccessfulSieges1 = num + 1;
				if (mapEvent.MapEventSettlement.IsTown)
				{
					num = stance.SuccessfulTownSieges1;
					stance.SuccessfulTownSieges1 = num + 1;
					return;
				}
			}
			else
			{
				int num = stance.SuccessfulSieges2;
				stance.SuccessfulSieges2 = num + 1;
				if (mapEvent.MapEventSettlement.IsTown)
				{
					num = stance.SuccessfulTownSieges2;
					stance.SuccessfulTownSieges2 = num + 1;
				}
			}
		}
	}
}
