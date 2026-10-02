using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C4 RID: 1220
	public static class ChangeOwnerOfSettlementAction
	{
		// Token: 0x06004D28 RID: 19752 RVA: 0x00186634 File Offset: 0x00184834
		private static void ApplyInternal(Settlement settlement, Hero newOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			Clan ownerClan = settlement.OwnerClan;
			Hero hero = ((ownerClan != null) ? ownerClan.Leader : null);
			if (settlement.Town != null)
			{
				settlement.Town.IsOwnerUnassigned = false;
			}
			if (settlement.IsFortification)
			{
				settlement.Town.OwnerClan = newOwner.Clan;
			}
			if (settlement.IsFortification)
			{
				if (detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege && settlement.Town.GarrisonParty != null)
				{
					DestroyPartyAction.Apply(capturerHero.PartyBelongedTo.Party, settlement.Town.GarrisonParty);
				}
				if (settlement.Town.GarrisonParty == null)
				{
					settlement.AddGarrisonParty();
				}
				ChangeGovernorAction.RemoveGovernorOfIfExists(settlement.Town);
			}
			settlement.Party.SetVisualAsDirty();
			foreach (Village village in settlement.BoundVillages)
			{
				village.Settlement.Party.SetVisualAsDirty();
				if (village.VillagerPartyComponent != null && newOwner != null)
				{
					foreach (MobileParty mobileParty in MobileParty.All)
					{
						if (mobileParty.MapEvent == null && mobileParty != MobileParty.MainParty && mobileParty.ShortTermTargetParty == village.VillagerPartyComponent.MobileParty && !mobileParty.MapFaction.IsAtWarWith(newOwner.MapFaction))
						{
							mobileParty.SetMoveModeHold();
						}
					}
				}
			}
			bool flag = (detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege || detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByClanDestruction || detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByLeaveFaction) && settlement.IsFortification;
			if (newOwner != null)
			{
				IFaction mapFaction = newOwner.MapFaction;
				if (settlement.Party.MapEvent != null && !settlement.Party.MapEvent.AttackerSide.LeaderParty.MapFaction.IsAtWarWith(mapFaction) && settlement.Party.MapEvent.Winner == null)
				{
					settlement.Party.MapEvent.DiplomaticallyFinished = true;
					foreach (WarPartyComponent warPartyComponent in settlement.MapFaction.WarPartyComponents)
					{
						MobileParty mobileParty2 = warPartyComponent.MobileParty;
						if (mobileParty2.DefaultBehavior == AiBehavior.DefendSettlement && mobileParty2.TargetSettlement == settlement && mobileParty2.CurrentSettlement == null)
						{
							mobileParty2.SetMoveModeHold();
						}
					}
					settlement.Party.MapEvent.Update();
				}
				foreach (Clan clan in Clan.NonBanditFactions)
				{
					if (mapFaction == null || (clan.Kingdom == null && !clan.IsAtWarWith(mapFaction)) || (clan.Kingdom != null && !clan.Kingdom.IsAtWarWith(mapFaction)))
					{
						foreach (WarPartyComponent warPartyComponent2 in clan.WarPartyComponents)
						{
							MobileParty mobileParty3 = warPartyComponent2.MobileParty;
							if (mobileParty3.BesiegedSettlement != settlement && (mobileParty3.DefaultBehavior == AiBehavior.RaidSettlement || mobileParty3.DefaultBehavior == AiBehavior.BesiegeSettlement || mobileParty3.DefaultBehavior == AiBehavior.AssaultSettlement) && mobileParty3.TargetSettlement == settlement)
							{
								Army army = mobileParty3.Army;
								if (army != null)
								{
									army.FinishArmyObjective();
								}
								mobileParty3.SetMoveModeHold();
							}
						}
					}
				}
			}
			CampaignEventDispatcher.Instance.OnSettlementOwnerChanged(settlement, flag, newOwner, hero, capturerHero, detail);
		}

		// Token: 0x06004D29 RID: 19753 RVA: 0x001869CC File Offset: 0x00184BCC
		public static void ApplyByDefault(Hero hero, Settlement settlement)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, hero, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.Default);
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x001869D7 File Offset: 0x00184BD7
		public static void ApplyByKingDecision(Hero hero, Settlement settlement)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, hero, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByKingDecision);
			if (settlement.Town != null)
			{
				settlement.Town.IsOwnerUnassigned = false;
			}
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x001869F6 File Offset: 0x00184BF6
		public static void ApplyBySiege(Hero newOwner, Hero capturerHero, Settlement settlement)
		{
			if (settlement.Town != null)
			{
				settlement.Town.LastCapturedBy = capturerHero.Clan;
			}
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, newOwner, capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege);
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x00186A1A File Offset: 0x00184C1A
		public static void ApplyByLeaveFaction(Hero hero, Settlement settlement)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, hero, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByLeaveFaction);
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x00186A25 File Offset: 0x00184C25
		public static void ApplyByBarter(Hero hero, Settlement settlement)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, hero, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByBarter);
		}

		// Token: 0x06004D2E RID: 19758 RVA: 0x00186A30 File Offset: 0x00184C30
		public static void ApplyByRebellion(Hero hero, Settlement settlement)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, hero, hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByRebellion);
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x00186A3B File Offset: 0x00184C3B
		public static void ApplyByDestroyClan(Settlement settlement, Hero newOwner)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, newOwner, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByClanDestruction);
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x00186A46 File Offset: 0x00184C46
		public static void ApplyByGift(Settlement settlement, Hero newOwner)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, newOwner, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByGift);
		}

		// Token: 0x020008DB RID: 2267
		public enum ChangeOwnerOfSettlementDetail
		{
			// Token: 0x04002655 RID: 9813
			Default,
			// Token: 0x04002656 RID: 9814
			BySiege,
			// Token: 0x04002657 RID: 9815
			ByBarter,
			// Token: 0x04002658 RID: 9816
			ByLeaveFaction,
			// Token: 0x04002659 RID: 9817
			ByKingDecision,
			// Token: 0x0400265A RID: 9818
			ByGift,
			// Token: 0x0400265B RID: 9819
			ByRebellion,
			// Token: 0x0400265C RID: 9820
			ByClanDestruction
		}
	}
}
