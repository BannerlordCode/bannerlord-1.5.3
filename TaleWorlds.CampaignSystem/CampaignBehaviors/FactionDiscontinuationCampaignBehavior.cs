using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200040E RID: 1038
	public class FactionDiscontinuationCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600422C RID: 16940 RVA: 0x0012C54C File Offset: 0x0012A74C
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.DailyTickClan));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x0012C5B8 File Offset: 0x0012A7B8
		public void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (this._independentClans.ContainsKey(newOwner.Clan))
			{
				this._independentClans.Remove(newOwner.Clan);
			}
			if (this.CanClanBeDiscontinued(oldOwner.Clan))
			{
				this.AddIndependentClan(oldOwner.Clan);
			}
			Kingdom kingdom = oldOwner.Clan.Kingdom;
			if (kingdom != null && this.CanKingdomBeDiscontinued(kingdom))
			{
				this.DiscontinueKingdom(kingdom);
			}
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x0012C628 File Offset: 0x0012A828
		public void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (newKingdom == null)
			{
				if (this.CanClanBeDiscontinued(clan))
				{
					this.AddIndependentClan(clan);
				}
			}
			else if (this._independentClans.ContainsKey(clan))
			{
				this._independentClans.Remove(clan);
			}
			if (detail != ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByKingdomDestruction && oldKingdom != null && this.CanKingdomBeDiscontinued(oldKingdom))
			{
				this.DiscontinueKingdom(oldKingdom);
			}
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x0012C680 File Offset: 0x0012A880
		private void DailyTickClan(Clan clan)
		{
			if (this._independentClans.ContainsKey(clan) && this._independentClans[clan].IsPast)
			{
				this.DiscontinueClan(clan);
			}
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x0012C6B8 File Offset: 0x0012A8B8
		private bool CanKingdomBeDiscontinued(Kingdom kingdom)
		{
			bool flag = !kingdom.IsEliminated && kingdom != Clan.PlayerClan.Kingdom && kingdom.Settlements.IsEmpty<Settlement>();
			if (flag)
			{
				CampaignEventDispatcher.Instance.CanKingdomBeDiscontinued(kingdom, ref flag);
			}
			return flag;
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x0012C6FC File Offset: 0x0012A8FC
		private void DiscontinueKingdom(Kingdom kingdom)
		{
			foreach (Clan clan in new List<Clan>(kingdom.Clans))
			{
				this.FinalizeMapEvents(clan);
				ChangeKingdomAction.ApplyByLeaveByKingdomDestruction(clan, true);
			}
			kingdom.RulingClan = null;
			DestroyKingdomAction.Apply(kingdom);
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x0012C768 File Offset: 0x0012A968
		private void FinalizeMapEvents(Clan clan)
		{
			foreach (WarPartyComponent warPartyComponent in clan.WarPartyComponents.ToList<WarPartyComponent>())
			{
				if ((warPartyComponent != null) & warPartyComponent.Party.IsActive)
				{
					if (warPartyComponent.MobileParty.MapEvent != null)
					{
						warPartyComponent.MobileParty.MapEvent.FinalizeEvent();
					}
					if (warPartyComponent.MobileParty.SiegeEvent != null)
					{
						warPartyComponent.MobileParty.SiegeEvent.FinalizeSiegeEvent();
					}
				}
			}
			foreach (Settlement settlement in clan.Settlements)
			{
				if (settlement.Party.MapEvent != null)
				{
					settlement.Party.MapEvent.FinalizeEvent();
				}
				if (settlement.Party.SiegeEvent != null)
				{
					settlement.Party.SiegeEvent.FinalizeSiegeEvent();
				}
			}
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x0012C87C File Offset: 0x0012AA7C
		private bool CanClanBeDiscontinued(Clan clan)
		{
			return clan.Kingdom == null && !clan.IsRebelClan && !clan.IsBanditFaction && !clan.IsMinorFaction && clan != Clan.PlayerClan && clan.Settlements.IsEmpty<Settlement>();
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x0012C8B3 File Offset: 0x0012AAB3
		private void DiscontinueClan(Clan clan)
		{
			DestroyClanAction.Apply(clan);
			this._independentClans.Remove(clan);
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x0012C8C8 File Offset: 0x0012AAC8
		private void AddIndependentClan(Clan clan)
		{
			if (!this._independentClans.ContainsKey(clan))
			{
				this._independentClans.Add(clan, CampaignTime.DaysFromNow(28f));
			}
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x0012C8EE File Offset: 0x0012AAEE
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Clan, CampaignTime>>("_independentClans", ref this._independentClans);
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x0012C904 File Offset: 0x0012AB04
		private void OnGameLoadFinished()
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.5.0", 0)))
			{
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (!kingdom.IsEliminated && this.CanKingdomBeDiscontinued(kingdom))
					{
						this.DiscontinueKingdom(kingdom);
					}
				}
			}
		}

		// Token: 0x040013CF RID: 5071
		private const float SurvivalDurationForIndependentClanInDays = 28f;

		// Token: 0x040013D0 RID: 5072
		private Dictionary<Clan, CampaignTime> _independentClans = new Dictionary<Clan, CampaignTime>();
	}
}
