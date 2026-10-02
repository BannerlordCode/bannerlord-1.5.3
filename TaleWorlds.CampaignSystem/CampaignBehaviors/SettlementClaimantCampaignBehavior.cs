using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000462 RID: 1122
	public class SettlementClaimantCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004892 RID: 18578 RVA: 0x001697B8 File Offset: 0x001679B8
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x001697E8 File Offset: 0x001679E8
		private void DailyTickSettlement(Settlement settlement)
		{
			if (settlement.Town != null && settlement.Town.IsOwnerUnassigned && settlement.OwnerClan != null && settlement.OwnerClan.Kingdom != null)
			{
				Kingdom kingdom = settlement.OwnerClan.Kingdom;
				if (kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>((KingdomDecision x) => x is SettlementClaimantDecision) == null)
				{
					kingdom.AddDecision(new SettlementClaimantDecision(kingdom.RulingClan, settlement, null, null), true);
				}
			}
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x0016986C File Offset: 0x00167A6C
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x00169870 File Offset: 0x00167A70
		public void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (settlement.IsVillage && settlement.Party.MapEvent != null && !FactionManager.IsAtWarAgainstFaction(settlement.Party.MapEvent.AttackerSide.LeaderParty.MapFaction, newOwner.MapFaction))
			{
				settlement.Party.MapEvent.FinalizeEvent();
			}
			if (openToClaim && newOwner.MapFaction.IsKingdomFaction && (newOwner.MapFaction as Kingdom).Clans.Count > 1 && settlement.Town != null)
			{
				settlement.Town.IsOwnerUnassigned = true;
			}
			foreach (Kingdom kingdom in Kingdom.All)
			{
				foreach (KingdomDecision kingdomDecision in kingdom.UnresolvedDecisions.ToList<KingdomDecision>())
				{
					SettlementClaimantDecision settlementClaimantDecision;
					SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision;
					if ((settlementClaimantDecision = kingdomDecision as SettlementClaimantDecision) != null)
					{
						if (settlementClaimantDecision.Settlement == settlement)
						{
							kingdom.RemoveDecision(kingdomDecision);
						}
					}
					else if ((settlementClaimantPreliminaryDecision = kingdomDecision as SettlementClaimantPreliminaryDecision) != null && settlementClaimantPreliminaryDecision.Settlement == settlement && settlementClaimantPreliminaryDecision.Settlement == settlement)
					{
						kingdom.RemoveDecision(kingdomDecision);
					}
				}
			}
			if (oldOwner.Clan == Clan.PlayerClan && (newOwner == null || newOwner.Clan != Clan.PlayerClan))
			{
				foreach (ItemRosterElement itemRosterElement in settlement.Stash)
				{
					settlement.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement.Item, itemRosterElement.Amount);
				}
				settlement.Stash.Clear();
			}
		}
	}
}
