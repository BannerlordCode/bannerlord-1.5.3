using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000437 RID: 1079
	public class MapTrackerCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600458F RID: 17807 RVA: 0x00151398 File Offset: 0x0014F598
		public override void RegisterEvents()
		{
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.OnMobilePartyCreated));
			CampaignEvents.OnPartyRemovedEvent.AddNonSerializedListener(this, new Action<PartyBase>(this.OnPartyRemoved));
			CampaignEvents.MobilePartyQuestStatusChanged.AddNonSerializedListener(this, new Action<MobileParty, bool>(this.OnPartyQuestStatusChanged));
			CampaignEvents.ArmyCreated.AddNonSerializedListener(this, new Action<Army>(this.OnArmyCreated));
			CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
			CampaignEvents.OnPartyJoinedArmyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyJoinedArmy));
			CampaignEvents.PartyRemovedFromArmyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyRemovedFromArmy));
			CampaignEvents.OnHeroChangedClanEvent.AddNonSerializedListener(this, new Action<Hero, Clan>(this.OnHeroChangedClan));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.OnClanCreatedEvent.AddNonSerializedListener(this, new Action<Clan, bool>(this.OnCompanionClanCreated));
			CampaignEvents.OnPlayerCharacterChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero, MobileParty, bool>(this.OnPlayerCharacterChanged));
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x001514A2 File Offset: 0x0014F6A2
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x001514A4 File Offset: 0x0014F6A4
		private void OnMobilePartyCreated(MobileParty mobileParty)
		{
			Campaign.Current.MapTrackerManager.Refresh(mobileParty);
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x001514B6 File Offset: 0x0014F6B6
		private void OnPartyRemoved(PartyBase partyBase)
		{
			if (partyBase.IsMobile)
			{
				Campaign.Current.MapTrackerManager.RemoveMapTracker(partyBase.MobileParty);
				Campaign.Current.MapTrackerManager.Refresh(partyBase.MobileParty);
			}
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x001514EA File Offset: 0x0014F6EA
		private void OnPartyQuestStatusChanged(MobileParty mobileParty, bool isUsedByQuest)
		{
			Campaign.Current.MapTrackerManager.Refresh(mobileParty);
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x001514FC File Offset: 0x0014F6FC
		private void OnArmyCreated(Army army)
		{
			Campaign.Current.MapTrackerManager.Refresh(army);
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x0015150E File Offset: 0x0014F70E
		private void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayerJoining)
		{
			Campaign.Current.MapTrackerManager.ForceRemoveTracker(army);
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x00151520 File Offset: 0x0014F720
		private void OnPartyJoinedArmy(MobileParty mobileParty)
		{
			if (mobileParty == MobileParty.MainParty && mobileParty.Army != null)
			{
				Campaign.Current.MapTrackerManager.Refresh(mobileParty.Army);
			}
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x00151548 File Offset: 0x0014F748
		private void OnPartyRemovedFromArmy(MobileParty mobileParty)
		{
			if (mobileParty == MobileParty.MainParty)
			{
				for (int i = 0; i < Kingdom.All.Count; i++)
				{
					Kingdom kingdom = Kingdom.All[i];
					for (int j = 0; j < kingdom.Armies.Count; j++)
					{
						Campaign.Current.MapTrackerManager.Refresh(kingdom.Armies[j]);
					}
				}
			}
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x001515B0 File Offset: 0x0014F7B0
		private void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
			if (hero.PartyBelongedTo != null)
			{
				Campaign.Current.MapTrackerManager.Refresh(hero.PartyBelongedTo);
			}
			for (int i = 0; i < hero.OwnedCaravans.Count; i++)
			{
				Campaign.Current.MapTrackerManager.Refresh(hero.OwnedCaravans[i].MobileParty);
			}
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x00151610 File Offset: 0x0014F810
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			if (clan == Clan.PlayerClan)
			{
				Campaign.Current.MapTrackerManager.ResetTrackers();
			}
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x00151629 File Offset: 0x0014F829
		private void OnCompanionClanCreated(Clan clan, bool isCompanion)
		{
			if (isCompanion && clan.Leader.PartyBelongedTo != null)
			{
				Campaign.Current.MapTrackerManager.Refresh(clan.Leader.PartyBelongedTo);
			}
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x00151655 File Offset: 0x0014F855
		private void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
		{
			Campaign.Current.MapTrackerManager.ResetTrackers();
		}
	}
}
