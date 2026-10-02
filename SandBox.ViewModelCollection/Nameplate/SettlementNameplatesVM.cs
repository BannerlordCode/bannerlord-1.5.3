using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x02000020 RID: 32
	public class SettlementNameplatesVM : ViewModel
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000D0D3 File Offset: 0x0000B2D3
		public MBReadOnlyList<SettlementNameplateVM> AllNameplates
		{
			get
			{
				return this._allNameplates;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000D0DC File Offset: 0x0000B2DC
		public SettlementNameplatesVM(Camera mapCamera, Action<CampaignVec2> fastMoveCameraToPosition)
		{
			this._allNameplates = new MBList<SettlementNameplateVM>(400);
			this._allNameplatesBySettlements = new Dictionary<Settlement, SettlementNameplateVM>(400);
			this.SmallNameplates = new MBBindingList<SettlementNameplateVM>();
			this.MediumNameplates = new MBBindingList<SettlementNameplateVM>();
			this.LargeNameplates = new MBBindingList<SettlementNameplateVM>();
			this._mapCamera = mapCamera;
			this._fastMoveCameraToPosition = fastMoveCameraToPosition;
			CampaignEvents.PartyVisibilityChangedEvent.AddNonSerializedListener(this, new Action<PartyBase>(this.OnPartyBaseVisibilityChange));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnPeaceDeclared));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangeKingdom));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStartedOnSettlement));
			CampaignEvents.OnSiegeEventEndedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventEndedOnSettlement));
			CampaignEvents.RebelliousClanDisbandedAtSettlement.AddNonSerializedListener(this, new Action<Settlement, Clan>(this.OnRebelliousClanDisbandedAtSettlement));
			CampaignEvents.OnAllianceStartedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceStarted));
			CampaignEvents.OnAllianceEndedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceEnded));
			this.UpdateNameplateAuxMTPredicate = new TWParallel.ParallelForAuxPredicate(this.UpdateNameplateAuxMT);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000D238 File Offset: 0x0000B438
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			for (int i = 0; i < this._allNameplates.Count; i++)
			{
				this._allNameplates[i].OnFinalize();
			}
			this._allNameplates.Clear();
			this._allNameplatesBySettlements.Clear();
			this.SmallNameplates.Clear();
			this.MediumNameplates.Clear();
			this.LargeNameplates.Clear();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
		public override void RefreshValues()
		{
			base.RefreshValues();
			for (int i = 0; i < this._allNameplates.Count; i++)
			{
				this._allNameplates[i].RefreshValues();
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000D2F0 File Offset: 0x0000B4F0
		public void Reset(IEnumerable<Tuple<Settlement, GameEntity>> settlements)
		{
			for (int i = 0; i < this._allNameplates.Count; i++)
			{
				this._allNameplates[i].OnFinalize();
			}
			this._allNameplates.Clear();
			this._allNameplatesBySettlements.Clear();
			this.SmallNameplates.Clear();
			this.MediumNameplates.Clear();
			this.LargeNameplates.Clear();
			this.Initialize(settlements);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000D364 File Offset: 0x0000B564
		public void Initialize(IEnumerable<Tuple<Settlement, GameEntity>> settlements)
		{
			this._allRegularSettlements = settlements.Where<Tuple<Settlement, GameEntity>>((Tuple<Settlement, GameEntity> x) => !x.Item1.IsHideout && !(x.Item1.SettlementComponent is RetirementSettlementComponent));
			this._allHideouts = settlements.Where<Tuple<Settlement, GameEntity>>((Tuple<Settlement, GameEntity> x) => x.Item1.IsHideout && !(x.Item1.SettlementComponent is RetirementSettlementComponent));
			this._allRetreats = settlements.Where<Tuple<Settlement, GameEntity>>((Tuple<Settlement, GameEntity> x) => !x.Item1.IsHideout && x.Item1.SettlementComponent is RetirementSettlementComponent);
			foreach (Tuple<Settlement, GameEntity> tuple in this._allRegularSettlements)
			{
				if (tuple.Item1.IsVisible)
				{
					SettlementNameplateVM settlementNameplateVM = new SettlementNameplateVM(tuple.Item1, tuple.Item2, this._mapCamera, this._fastMoveCameraToPosition);
					this.AddNameplate(settlementNameplateVM);
				}
			}
			foreach (Tuple<Settlement, GameEntity> tuple2 in this._allHideouts)
			{
				if (tuple2.Item1.IsVisible)
				{
					SettlementNameplateVM settlementNameplateVM2 = new SettlementNameplateVM(tuple2.Item1, tuple2.Item2, this._mapCamera, this._fastMoveCameraToPosition);
					this.AddNameplate(settlementNameplateVM2);
				}
			}
			foreach (Tuple<Settlement, GameEntity> tuple3 in this._allRetreats)
			{
				RetirementSettlementComponent retirementSettlementComponent;
				if ((retirementSettlementComponent = tuple3.Item1.SettlementComponent as RetirementSettlementComponent) != null)
				{
					if (retirementSettlementComponent.Settlement.IsVisible)
					{
						SettlementNameplateVM settlementNameplateVM3 = new SettlementNameplateVM(tuple3.Item1, tuple3.Item2, this._mapCamera, this._fastMoveCameraToPosition);
						this.AddNameplate(settlementNameplateVM3);
					}
				}
				else
				{
					Debug.FailedAssert("A settlement which is IsRetreat doesn't have a retirement component.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Nameplate\\SettlementNameplatesVM.cs", "Initialize", 136);
				}
			}
			for (int i = 0; i < this._allNameplates.Count; i++)
			{
				SettlementNameplateVM settlementNameplateVM4 = this._allNameplates[i];
				Settlement settlement = settlementNameplateVM4.Settlement;
				if (((settlement != null) ? settlement.SiegeEvent : null) != null)
				{
					SettlementNameplateVM settlementNameplateVM5 = settlementNameplateVM4;
					Settlement settlement2 = settlementNameplateVM4.Settlement;
					settlementNameplateVM5.OnSiegeEventStartedOnSettlement((settlement2 != null) ? settlement2.SiegeEvent : null);
				}
				else if (settlementNameplateVM4.Settlement.IsTown || settlementNameplateVM4.Settlement.IsCastle)
				{
					Clan ownerClan = settlementNameplateVM4.Settlement.OwnerClan;
					if (ownerClan != null && ownerClan.IsRebelClan)
					{
						settlementNameplateVM4.OnRebelliousClanFormed(settlementNameplateVM4.Settlement.OwnerClan);
					}
				}
			}
			this.RefreshRelationsOfNameplates();
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000D614 File Offset: 0x0000B814
		private void AddNameplate(SettlementNameplateVM nameplate)
		{
			this._allNameplates.Add(nameplate);
			this._allNameplatesBySettlements[nameplate.Settlement] = nameplate;
			switch (nameplate.SettlementTypeEnum)
			{
			case SettlementNameplateVM.Type.Village:
				this.SmallNameplates.Add(nameplate);
				return;
			case SettlementNameplateVM.Type.Castle:
				this.MediumNameplates.Add(nameplate);
				return;
			case SettlementNameplateVM.Type.Town:
				this.LargeNameplates.Add(nameplate);
				return;
			default:
				return;
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000D680 File Offset: 0x0000B880
		private void RemoveNameplate(SettlementNameplateVM nameplate)
		{
			this._allNameplates.Remove(nameplate);
			this._allNameplatesBySettlements.Remove(nameplate.Settlement);
			this.SmallNameplates.Remove(nameplate);
			this.MediumNameplates.Remove(nameplate);
			this.LargeNameplates.Remove(nameplate);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000D6D4 File Offset: 0x0000B8D4
		private void UpdateNameplateAuxMT(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this._allNameplates[i].UpdateNameplateMT(this._cachedCameraPosition);
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000D704 File Offset: 0x0000B904
		public void Update()
		{
			this._cachedCameraPosition = this._mapCamera.Position;
			TWParallel.For(0, this._allNameplates.Count, this.UpdateNameplateAuxMTPredicate, 16);
			for (int i = 0; i < this._allNameplates.Count; i++)
			{
				this._allNameplates[i].RefreshBindValues();
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000D764 File Offset: 0x0000B964
		private void OnSiegeEventStartedOnSettlement(SiegeEvent siegeEvent)
		{
			SettlementNameplateVM settlementNameplateVM;
			if (this._allNameplatesBySettlements.TryGetValue(siegeEvent.BesiegedSettlement, out settlementNameplateVM))
			{
				settlementNameplateVM.OnSiegeEventStartedOnSettlement(siegeEvent);
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000D790 File Offset: 0x0000B990
		private void OnSiegeEventEndedOnSettlement(SiegeEvent siegeEvent)
		{
			SettlementNameplateVM settlementNameplateVM;
			if (this._allNameplatesBySettlements.TryGetValue(siegeEvent.BesiegedSettlement, out settlementNameplateVM))
			{
				settlementNameplateVM.OnSiegeEventEndedOnSettlement(siegeEvent);
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000D7BC File Offset: 0x0000B9BC
		private void OnMapEventStartedOnSettlement(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			SettlementNameplateVM settlementNameplateVM;
			if (this._allNameplatesBySettlements.TryGetValue(mapEvent.MapEventSettlement, out settlementNameplateVM))
			{
				settlementNameplateVM.OnMapEventStartedOnSettlement(mapEvent);
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000D7E8 File Offset: 0x0000B9E8
		private void OnMapEventEndedOnSettlement(MapEvent mapEvent)
		{
			SettlementNameplateVM settlementNameplateVM;
			if (this._allNameplatesBySettlements.TryGetValue(mapEvent.MapEventSettlement, out settlementNameplateVM))
			{
				settlementNameplateVM.OnMapEventEndedOnSettlement();
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000D810 File Offset: 0x0000BA10
		private void OnPartyBaseVisibilityChange(PartyBase party)
		{
			if (party.IsSettlement)
			{
				Tuple<Settlement, GameEntity> tuple;
				if (party.Settlement.IsHideout)
				{
					tuple = this._allHideouts.SingleOrDefault<Tuple<Settlement, GameEntity>>((Tuple<Settlement, GameEntity> h) => h.Item1.Hideout == party.Settlement.Hideout);
				}
				else if (party.Settlement.SettlementComponent is RetirementSettlementComponent)
				{
					tuple = this._allRetreats.SingleOrDefault<Tuple<Settlement, GameEntity>>((Tuple<Settlement, GameEntity> h) => h.Item1.SettlementComponent as RetirementSettlementComponent == party.Settlement.SettlementComponent as RetirementSettlementComponent);
				}
				else
				{
					tuple = this._allRegularSettlements.SingleOrDefault<Tuple<Settlement, GameEntity>>((Tuple<Settlement, GameEntity> h) => h.Item1 == party.Settlement);
				}
				if (tuple != null)
				{
					SettlementNameplateVM settlementNameplateVM = null;
					if (tuple.Item1 != null)
					{
						this._allNameplatesBySettlements.TryGetValue(tuple.Item1, out settlementNameplateVM);
					}
					if (party.IsVisible && settlementNameplateVM == null)
					{
						SettlementNameplateVM settlementNameplateVM2 = new SettlementNameplateVM(tuple.Item1, tuple.Item2, this._mapCamera, this._fastMoveCameraToPosition);
						this.AddNameplate(settlementNameplateVM2);
						settlementNameplateVM2.RefreshRelationStatus();
						return;
					}
					if (!party.IsVisible && settlementNameplateVM != null)
					{
						this.RemoveNameplate(settlementNameplateVM);
					}
				}
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000D925 File Offset: 0x0000BB25
		private void OnPeaceDeclared(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			this.OnPeaceOrWarDeclared(faction1, faction2);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000D92F File Offset: 0x0000BB2F
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail arg3)
		{
			this.OnPeaceOrWarDeclared(faction1, faction2);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000D939 File Offset: 0x0000BB39
		private void OnPeaceOrWarDeclared(IFaction faction1, IFaction faction2)
		{
			if (faction1 == Hero.MainHero.MapFaction || faction1 == Hero.MainHero.Clan || faction2 == Hero.MainHero.MapFaction || faction2 == Hero.MainHero.Clan)
			{
				this.RefreshRelationsOfNameplates();
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000D975 File Offset: 0x0000BB75
		private void OnClanChangeKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			this.RefreshRelationsOfNameplates();
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000D980 File Offset: 0x0000BB80
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero previousOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			SettlementNameplateVM settlementNameplateVM = null;
			if (this._allNameplatesBySettlements.TryGetValue(settlement, out settlementNameplateVM))
			{
				settlementNameplateVM.RefreshDynamicProperties(true);
				settlementNameplateVM.RefreshRelationStatus();
				if (detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByRebellion)
				{
					settlementNameplateVM.OnRebelliousClanFormed(newOwner.Clan);
				}
				else if (previousOwner != null && previousOwner.IsRebel)
				{
					settlementNameplateVM.OnRebelliousClanDisbanded(previousOwner.Clan);
				}
			}
			for (int i = 0; i < settlement.BoundVillages.Count; i++)
			{
				Village village = settlement.BoundVillages[i];
				if (this._allNameplatesBySettlements.TryGetValue(village.Settlement, out settlementNameplateVM))
				{
					settlementNameplateVM.RefreshDynamicProperties(true);
					settlementNameplateVM.RefreshRelationStatus();
				}
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000DA1E File Offset: 0x0000BC1E
		private void OnAllianceEnded(Kingdom kingdom1, Kingdom kingdom2)
		{
			this.OnAllianceStateChanged(kingdom1, kingdom2);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000DA28 File Offset: 0x0000BC28
		private void OnAllianceStarted(Kingdom kingdom1, Kingdom kingdom2)
		{
			this.OnAllianceStateChanged(kingdom1, kingdom2);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000DA32 File Offset: 0x0000BC32
		private void OnAllianceStateChanged(Kingdom kingdom1, Kingdom kingdom2)
		{
			if (kingdom1 == Hero.MainHero.MapFaction || kingdom2 == Hero.MainHero.MapFaction)
			{
				this.RefreshRelationsOfNameplates();
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000DA54 File Offset: 0x0000BC54
		public SettlementNameplateVM GetNameplateOfSettlement(Settlement settlement)
		{
			SettlementNameplateVM settlementNameplateVM;
			if (this._allNameplatesBySettlements.TryGetValue(settlement, out settlementNameplateVM))
			{
				return settlementNameplateVM;
			}
			return null;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000DA74 File Offset: 0x0000BC74
		public void OnRebelliousClanDisbandedAtSettlement(Settlement settlement, Clan clan)
		{
			SettlementNameplateVM settlementNameplateVM;
			if (this._allNameplatesBySettlements.TryGetValue(settlement, out settlementNameplateVM))
			{
				settlementNameplateVM.OnRebelliousClanDisbanded(clan);
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000DA98 File Offset: 0x0000BC98
		public void RefreshRelationsOfNameplates()
		{
			for (int i = 0; i < this._allNameplates.Count; i++)
			{
				this._allNameplates[i].RefreshRelationStatus();
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000DACC File Offset: 0x0000BCCC
		public void RefreshDynamicPropertiesOfNameplates(bool forceUpdate)
		{
			for (int i = 0; i < this._allNameplates.Count; i++)
			{
				this._allNameplates[i].RefreshDynamicProperties(forceUpdate);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0000DB01 File Offset: 0x0000BD01
		// (set) Token: 0x06000313 RID: 787 RVA: 0x0000DB09 File Offset: 0x0000BD09
		[DataSourceProperty]
		public MBBindingList<SettlementNameplateVM> SmallNameplates
		{
			get
			{
				return this._smallNameplates;
			}
			set
			{
				if (this._smallNameplates != value)
				{
					this._smallNameplates = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementNameplateVM>>(value, "SmallNameplates");
				}
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000DB27 File Offset: 0x0000BD27
		// (set) Token: 0x06000315 RID: 789 RVA: 0x0000DB2F File Offset: 0x0000BD2F
		[DataSourceProperty]
		public MBBindingList<SettlementNameplateVM> MediumNameplates
		{
			get
			{
				return this._mediumNameplates;
			}
			set
			{
				if (this._mediumNameplates != value)
				{
					this._mediumNameplates = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementNameplateVM>>(value, "MediumNameplates");
				}
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0000DB4D File Offset: 0x0000BD4D
		// (set) Token: 0x06000317 RID: 791 RVA: 0x0000DB55 File Offset: 0x0000BD55
		[DataSourceProperty]
		public MBBindingList<SettlementNameplateVM> LargeNameplates
		{
			get
			{
				return this._largeNameplates;
			}
			set
			{
				if (this._largeNameplates != value)
				{
					this._largeNameplates = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementNameplateVM>>(value, "LargeNameplates");
				}
			}
		}

		// Token: 0x04000175 RID: 373
		private readonly Camera _mapCamera;

		// Token: 0x04000176 RID: 374
		private Vec3 _cachedCameraPosition;

		// Token: 0x04000177 RID: 375
		private readonly TWParallel.ParallelForAuxPredicate UpdateNameplateAuxMTPredicate;

		// Token: 0x04000178 RID: 376
		private readonly Action<CampaignVec2> _fastMoveCameraToPosition;

		// Token: 0x04000179 RID: 377
		private IEnumerable<Tuple<Settlement, GameEntity>> _allHideouts;

		// Token: 0x0400017A RID: 378
		private IEnumerable<Tuple<Settlement, GameEntity>> _allRetreats;

		// Token: 0x0400017B RID: 379
		private IEnumerable<Tuple<Settlement, GameEntity>> _allRegularSettlements;

		// Token: 0x0400017C RID: 380
		private MBList<SettlementNameplateVM> _allNameplates;

		// Token: 0x0400017D RID: 381
		private Dictionary<Settlement, SettlementNameplateVM> _allNameplatesBySettlements;

		// Token: 0x0400017E RID: 382
		private MBBindingList<SettlementNameplateVM> _smallNameplates;

		// Token: 0x0400017F RID: 383
		private MBBindingList<SettlementNameplateVM> _mediumNameplates;

		// Token: 0x04000180 RID: 384
		private MBBindingList<SettlementNameplateVM> _largeNameplates;
	}
}
