using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x0200001F RID: 31
	public class SettlementNameplatePartyMarkersVM : ViewModel
	{
		// Token: 0x060002EC RID: 748 RVA: 0x0000CDF8 File Offset: 0x0000AFF8
		public SettlementNameplatePartyMarkersVM(Settlement settlement)
		{
			this._settlement = settlement;
			this.PartiesInSettlement = new MBBindingList<SettlementNameplatePartyMarkerItemVM>();
			this._itemComparer = new SettlementNameplatePartyMarkersVM.PartyMarkerItemComparer();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000CE20 File Offset: 0x0000B020
		private void PopulatePartyList()
		{
			this.PartiesInSettlement.Clear();
			foreach (MobileParty mobileParty in this._settlement.Parties.Where<MobileParty>((MobileParty p) => this.IsMobilePartyValid(p)))
			{
				this.PartiesInSettlement.Add(new SettlementNameplatePartyMarkerItemVM(mobileParty));
			}
			this.PartiesInSettlement.Sort(this._itemComparer);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000CEAC File Offset: 0x0000B0AC
		private bool IsMobilePartyValid(MobileParty party)
		{
			if (party.IsGarrison || party.IsMilitia)
			{
				return false;
			}
			if (party.IsMainParty && (!party.IsMainParty || Campaign.Current.IsMainHeroDisguised))
			{
				return false;
			}
			if (party.Army != null)
			{
				Army army = party.Army;
				return army != null && army.LeaderParty.IsMainParty && !Campaign.Current.IsMainHeroDisguised;
			}
			return true;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000CF1C File Offset: 0x0000B11C
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (settlement == this._settlement)
			{
				SettlementNameplatePartyMarkerItemVM settlementNameplatePartyMarkerItemVM = this.PartiesInSettlement.SingleOrDefault<SettlementNameplatePartyMarkerItemVM>((SettlementNameplatePartyMarkerItemVM p) => p.Party == party);
				if (settlementNameplatePartyMarkerItemVM != null)
				{
					this.PartiesInSettlement.Remove(settlementNameplatePartyMarkerItemVM);
				}
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000CF68 File Offset: 0x0000B168
		private void OnSettlementEntered(MobileParty partyEnteredSettlement, Settlement settlement, Hero leader)
		{
			if (settlement == this._settlement && partyEnteredSettlement != null && this.PartiesInSettlement.SingleOrDefault<SettlementNameplatePartyMarkerItemVM>((SettlementNameplatePartyMarkerItemVM p) => p.Party == partyEnteredSettlement) == null && this.IsMobilePartyValid(partyEnteredSettlement))
			{
				this.PartiesInSettlement.Add(new SettlementNameplatePartyMarkerItemVM(partyEnteredSettlement));
				this.PartiesInSettlement.Sort(this._itemComparer);
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000CFE1 File Offset: 0x0000B1E1
		private void OnMapEventEnded(MapEvent obj)
		{
			if (obj.MapEventSettlement != null && obj.MapEventSettlement == this._settlement)
			{
				this.PopulatePartyList();
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000D000 File Offset: 0x0000B200
		public void RegisterEvents()
		{
			if (!this._eventsRegistered)
			{
				this.PopulatePartyList();
				CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
				this._eventsRegistered = true;
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000D067 File Offset: 0x0000B267
		public void UnloadEvents()
		{
			if (this._eventsRegistered)
			{
				CampaignEvents.SettlementEntered.ClearListeners(this);
				CampaignEvents.OnSettlementLeftEvent.ClearListeners(this);
				CampaignEvents.MapEventEnded.ClearListeners(this);
				this.PartiesInSettlement.Clear();
				this._eventsRegistered = false;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000D0A4 File Offset: 0x0000B2A4
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000D0AC File Offset: 0x0000B2AC
		public MBBindingList<SettlementNameplatePartyMarkerItemVM> PartiesInSettlement
		{
			get
			{
				return this._partiesInSettlement;
			}
			set
			{
				if (value != this._partiesInSettlement)
				{
					this._partiesInSettlement = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementNameplatePartyMarkerItemVM>>(value, "PartiesInSettlement");
				}
			}
		}

		// Token: 0x04000171 RID: 369
		private Settlement _settlement;

		// Token: 0x04000172 RID: 370
		private bool _eventsRegistered;

		// Token: 0x04000173 RID: 371
		private SettlementNameplatePartyMarkersVM.PartyMarkerItemComparer _itemComparer;

		// Token: 0x04000174 RID: 372
		private MBBindingList<SettlementNameplatePartyMarkerItemVM> _partiesInSettlement;

		// Token: 0x0200008D RID: 141
		public class PartyMarkerItemComparer : IComparer<SettlementNameplatePartyMarkerItemVM>
		{
			// Token: 0x060006F9 RID: 1785 RVA: 0x000183F8 File Offset: 0x000165F8
			public int Compare(SettlementNameplatePartyMarkerItemVM x, SettlementNameplatePartyMarkerItemVM y)
			{
				return x.SortIndex.CompareTo(y.SortIndex);
			}
		}
	}
}
