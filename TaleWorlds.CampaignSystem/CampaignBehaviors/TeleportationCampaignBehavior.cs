using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000467 RID: 1127
	public class TeleportationCampaignBehavior : CampaignBehaviorBase, ITeleportationCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x060048F5 RID: 18677 RVA: 0x0016CFD1 File Offset: 0x0016B1D1
		private TextObject _partyLeaderChangeNotificationText
		{
			get
			{
				return new TextObject("{=QSaufZ9i}One of your parties has lost its leader. It will disband after a day has passed. You can assign a new clan member to lead it, if you wish to keep the party.", null);
			}
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x0016CFE0 File Offset: 0x0016B1E0
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
			CampaignEvents.HeroComesOfAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroComesOfAge));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.OnPartyDisbandStartedEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyDisbandStarted));
			CampaignEvents.OnGovernorChangedEvent.AddNonSerializedListener(this, new Action<Town, Hero, Hero>(this.OnGovernorChanged));
			CampaignEvents.OnHeroTeleportationRequestedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, MobileParty, TeleportHeroAction.TeleportationDetail>(this.OnHeroTeleportationRequested));
			CampaignEvents.OnPartyDisbandedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnPartyDisbanded));
			CampaignEvents.OnClanLeaderChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnClanLeaderChanged));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.HeroPrisonerTaken));
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x0016D101 File Offset: 0x0016B301
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<TeleportationCampaignBehavior.TeleportationData>>("_teleportationList", ref this._teleportationList);
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x0016D118 File Offset: 0x0016B318
		public bool GetTargetOfTeleportingHero(Hero teleportingHero, out bool isGovernor, out bool isPartyLeader, out IMapPoint target)
		{
			isGovernor = false;
			isPartyLeader = false;
			target = null;
			for (int i = 0; i < this._teleportationList.Count; i++)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportingHero == teleportingHero)
				{
					if (teleportationData.TargetSettlement != null)
					{
						isGovernor = teleportationData.IsGovernor;
						target = teleportationData.TargetSettlement;
						return true;
					}
					if (teleportationData.TargetParty != null)
					{
						isPartyLeader = teleportationData.IsPartyLeader;
						target = teleportationData.TargetParty;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060048F9 RID: 18681 RVA: 0x0016D194 File Offset: 0x0016B394
		public CampaignTime GetHeroArrivalTimeToDestination(Hero teleportingHero)
		{
			TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList.FirstOrDefaultQ<TeleportationCampaignBehavior.TeleportationData>((TeleportationCampaignBehavior.TeleportationData x) => x.TeleportingHero == teleportingHero);
			if (teleportationData != null)
			{
				return teleportationData.TeleportationTime;
			}
			return CampaignTime.Never;
		}

		// Token: 0x060048FA RID: 18682 RVA: 0x0016D1D8 File Offset: 0x0016B3D8
		private void HourlyTick()
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportationTime.IsPast && Campaign.Current.Models.DelayedTeleportationModel.CanPerformImmediateTeleport(teleportationData.TeleportingHero, teleportationData.TargetParty, teleportationData.TargetSettlement))
				{
					TeleportationCampaignBehavior.TeleportationData teleportationData2 = teleportationData;
					this.RemoveTeleportationData(teleportationData, false, true);
					this.ApplyImmediateTeleport(teleportationData2);
				}
			}
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x0016D254 File Offset: 0x0016B454
		private void DailyTickParty(MobileParty mobileParty)
		{
			if (mobileParty.IsActive && mobileParty.Army == null && mobileParty.MapEvent == null && mobileParty.LeaderHero != null && mobileParty.LeaderHero.IsNoncombatant && mobileParty.LeaderHero.DeathMark == KillCharacterAction.KillCharacterActionDetail.None && mobileParty.ActualClan != null && mobileParty.ActualClan != Clan.PlayerClan && mobileParty.ActualClan.Leader != mobileParty.LeaderHero && !mobileParty.IsInRaftState && (!mobileParty.IsCurrentlyAtSea || mobileParty.CurrentSettlement != null))
			{
				MBList<Hero> mblist = mobileParty.ActualClan.Heroes.WhereQ<Hero>((Hero h) => h.IsActive && h.IsCommander && h.PartyBelongedTo == null).ToMBList<Hero>();
				if (!mblist.IsEmpty<Hero>())
				{
					Hero leaderHero = mobileParty.LeaderHero;
					mobileParty.RemovePartyLeader();
					MakeHeroFugitiveAction.Apply(leaderHero, false);
					TeleportHeroAction.ApplyDelayedTeleportToPartyAsPartyLeader(mblist.GetRandomElementInefficiently<Hero>(), mobileParty);
				}
			}
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x0016D350 File Offset: 0x0016B550
		private void OnHeroComesOfAge(Hero hero)
		{
			if (hero.Clan != Clan.PlayerClan && !hero.IsNoncombatant)
			{
				foreach (WarPartyComponent warPartyComponent in hero.Clan.WarPartyComponents)
				{
					MobileParty mobileParty = warPartyComponent.MobileParty;
					if (mobileParty != null && mobileParty.Army == null && mobileParty.MapEvent == null && mobileParty.LeaderHero != null && mobileParty.LeaderHero.IsNoncombatant && (!mobileParty.IsCurrentlyAtSea || mobileParty.CurrentSettlement != null))
					{
						Hero leaderHero = mobileParty.LeaderHero;
						mobileParty.RemovePartyLeader();
						MakeHeroFugitiveAction.Apply(leaderHero, false);
						TeleportHeroAction.ApplyDelayedTeleportToPartyAsPartyLeader(hero, warPartyComponent.Party.MobileParty);
						break;
					}
				}
			}
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x0016D420 File Offset: 0x0016B620
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TargetParty != null && teleportationData.TargetParty == mobileParty)
				{
					this.RemoveTeleportationData(teleportationData, true, false);
				}
			}
		}

		// Token: 0x060048FE RID: 18686 RVA: 0x0016D46C File Offset: 0x0016B66C
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TargetSettlement != null && teleportationData.TargetSettlement == settlement && newOwner.Clan != teleportationData.TeleportingHero.Clan)
				{
					this.RemoveTeleportationData(teleportationData, true, true);
				}
			}
		}

		// Token: 0x060048FF RID: 18687 RVA: 0x0016D4CC File Offset: 0x0016B6CC
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportingHero == victim)
				{
					this.RemoveTeleportationData(teleportationData, true, true);
				}
			}
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x0016D510 File Offset: 0x0016B710
		private void OnPartyDisbandStarted(MobileParty disbandParty)
		{
			if (disbandParty.ActualClan == Clan.PlayerClan && disbandParty.LeaderHero == null && (disbandParty.IsLordParty || disbandParty.IsCaravan) && !disbandParty.IsCurrentlyAtSea)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new PartyLeaderChangeNotification(disbandParty, this._partyLeaderChangeNotificationText));
			}
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TargetParty != null && teleportationData.TargetParty == disbandParty)
				{
					this.RemoveTeleportationData(teleportationData, true, false);
				}
			}
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x0016D5A4 File Offset: 0x0016B7A4
		private void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportingHero != newGovernor && teleportationData.IsGovernor && teleportationData.TargetSettlement.Town == fortification)
				{
					teleportationData.IsGovernor = false;
				}
			}
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x0016D5FC File Offset: 0x0016B7FC
		private void OnHeroTeleportationRequested(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
			switch (detail)
			{
			case TeleportHeroAction.TeleportationDetail.ImmediateTeleportToSettlement:
			{
				for (int i = this._teleportationList.Count - 1; i >= 0; i--)
				{
					TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
					if (hero == teleportationData.TeleportingHero && teleportationData.TargetSettlement == targetSettlement)
					{
						this.RemoveTeleportationData(teleportationData, true, false);
					}
				}
				break;
			}
			case TeleportHeroAction.TeleportationDetail.ImmediateTeleportToParty:
			case TeleportHeroAction.TeleportationDetail.ImmediateTeleportToPartyAsPartyLeader:
				break;
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToSettlement:
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToSettlementAsGovernor:
				this._teleportationList.Add(new TeleportationCampaignBehavior.TeleportationData(hero, targetSettlement, detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToSettlementAsGovernor));
				return;
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToParty:
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToPartyAsPartyLeader:
				if (detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToPartyAsPartyLeader)
				{
					for (int j = this._teleportationList.Count - 1; j >= 0; j--)
					{
						TeleportationCampaignBehavior.TeleportationData teleportationData2 = this._teleportationList[j];
						if (teleportationData2.TargetParty == targetParty && teleportationData2.IsPartyLeader)
						{
							this.RemoveTeleportationData(teleportationData2, true, false);
						}
					}
				}
				this._teleportationList.Add(new TeleportationCampaignBehavior.TeleportationData(hero, targetParty, detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToPartyAsPartyLeader));
				return;
			default:
				return;
			}
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x0016D6E4 File Offset: 0x0016B8E4
		private void OnPartyDisbanded(MobileParty disbandParty, Settlement relatedSettlement)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TargetParty != null && teleportationData.TargetParty == disbandParty)
				{
					this.RemoveTeleportationData(teleportationData, true, true);
				}
			}
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x0016D730 File Offset: 0x0016B930
		private void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportingHero == newLeader && !teleportationData.IsPartyLeader)
				{
					this.RemoveTeleportationData(teleportationData, true, true);
				}
			}
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x0016D77C File Offset: 0x0016B97C
		private void HeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportingHero == prisoner)
				{
					this.RemoveTeleportationData(teleportationData, true, true);
				}
			}
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x0016D7C0 File Offset: 0x0016B9C0
		private void RemoveTeleportationData(TeleportationCampaignBehavior.TeleportationData data, bool isCanceled, bool disbandTargetParty = true)
		{
			this._teleportationList.Remove(data);
			if (isCanceled)
			{
				if (data.TeleportingHero.IsTraveling && data.TeleportingHero.DeathMark == KillCharacterAction.KillCharacterActionDetail.None)
				{
					MakeHeroFugitiveAction.Apply(data.TeleportingHero, false);
				}
				if (data.TargetParty != null)
				{
					if (data.TargetParty.ActualClan == Clan.PlayerClan)
					{
						CampaignEventDispatcher.Instance.OnPartyLeaderChangeOfferCanceled(data.TargetParty);
					}
					if (disbandTargetParty && data.TargetParty.IsActive && data.IsPartyLeader)
					{
						IDisbandPartyCampaignBehavior behavior = Campaign.Current.CampaignBehaviorManager.GetBehavior<IDisbandPartyCampaignBehavior>();
						if (behavior != null && !behavior.IsPartyWaitingForDisband(data.TargetParty))
						{
							DisbandPartyAction.StartDisband(data.TargetParty);
						}
					}
				}
			}
		}

		// Token: 0x06004907 RID: 18695 RVA: 0x0016D874 File Offset: 0x0016BA74
		private void ApplyImmediateTeleport(TeleportationCampaignBehavior.TeleportationData data)
		{
			if (data.TargetSettlement == null)
			{
				if (data.TargetParty != null)
				{
					if (data.IsPartyLeader)
					{
						TeleportHeroAction.ApplyImmediateTeleportToPartyAsPartyLeader(data.TeleportingHero, data.TargetParty);
						return;
					}
					TeleportHeroAction.ApplyImmediateTeleportToParty(data.TeleportingHero, data.TargetParty);
				}
				return;
			}
			if (data.IsGovernor)
			{
				data.TargetSettlement.Town.Governor = data.TeleportingHero;
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(data.TeleportingHero, data.TargetSettlement);
				return;
			}
			TeleportHeroAction.ApplyImmediateTeleportToSettlement(data.TeleportingHero, data.TargetSettlement);
		}

		// Token: 0x04001494 RID: 5268
		private List<TeleportationCampaignBehavior.TeleportationData> _teleportationList = new List<TeleportationCampaignBehavior.TeleportationData>();

		// Token: 0x0200089D RID: 2205
		public class TeleportationCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06006BCC RID: 27596 RVA: 0x001DB348 File Offset: 0x001D9548
			public TeleportationCampaignBehaviorTypeDefiner()
				: base(151000)
			{
			}

			// Token: 0x06006BCD RID: 27597 RVA: 0x001DB355 File Offset: 0x001D9555
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(TeleportationCampaignBehavior.TeleportationData), 1, null);
			}

			// Token: 0x06006BCE RID: 27598 RVA: 0x001DB369 File Offset: 0x001D9569
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(List<TeleportationCampaignBehavior.TeleportationData>));
			}
		}

		// Token: 0x0200089E RID: 2206
		internal class TeleportationData
		{
			// Token: 0x06006BCF RID: 27599 RVA: 0x001DB37C File Offset: 0x001D957C
			public TeleportationData(Hero teleportingHero, Settlement targetSettlement, bool isGovernor)
			{
				this.TeleportingHero = teleportingHero;
				this.TeleportationTime = CampaignTime.HoursFromNow(Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(teleportingHero, targetSettlement.Party).ResultNumber);
				this.TargetSettlement = targetSettlement;
				this.IsGovernor = isGovernor;
				this.TargetParty = null;
				this.IsPartyLeader = false;
			}

			// Token: 0x06006BD0 RID: 27600 RVA: 0x001DB3E0 File Offset: 0x001D95E0
			public TeleportationData(Hero teleportingHero, MobileParty targetParty, bool isPartyLeader)
			{
				this.TeleportingHero = teleportingHero;
				this.TeleportationTime = CampaignTime.HoursFromNow(Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(teleportingHero, targetParty.Party).ResultNumber);
				this.TargetParty = targetParty;
				this.IsPartyLeader = isPartyLeader;
				this.TargetSettlement = null;
				this.IsGovernor = false;
			}

			// Token: 0x06006BD1 RID: 27601 RVA: 0x001DB444 File Offset: 0x001D9644
			internal static void AutoGeneratedStaticCollectObjectsTeleportationData(object o, List<object> collectedObjects)
			{
				((TeleportationCampaignBehavior.TeleportationData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006BD2 RID: 27602 RVA: 0x001DB452 File Offset: 0x001D9652
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.TeleportingHero);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.TeleportationTime, collectedObjects);
				collectedObjects.Add(this.TargetSettlement);
				collectedObjects.Add(this.TargetParty);
			}

			// Token: 0x06006BD3 RID: 27603 RVA: 0x001DB489 File Offset: 0x001D9689
			internal static object AutoGeneratedGetMemberValueTeleportingHero(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).TeleportingHero;
			}

			// Token: 0x06006BD4 RID: 27604 RVA: 0x001DB496 File Offset: 0x001D9696
			internal static object AutoGeneratedGetMemberValueTeleportationTime(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).TeleportationTime;
			}

			// Token: 0x06006BD5 RID: 27605 RVA: 0x001DB4A8 File Offset: 0x001D96A8
			internal static object AutoGeneratedGetMemberValueTargetSettlement(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).TargetSettlement;
			}

			// Token: 0x06006BD6 RID: 27606 RVA: 0x001DB4B5 File Offset: 0x001D96B5
			internal static object AutoGeneratedGetMemberValueIsGovernor(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).IsGovernor;
			}

			// Token: 0x06006BD7 RID: 27607 RVA: 0x001DB4C7 File Offset: 0x001D96C7
			internal static object AutoGeneratedGetMemberValueTargetParty(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).TargetParty;
			}

			// Token: 0x06006BD8 RID: 27608 RVA: 0x001DB4D4 File Offset: 0x001D96D4
			internal static object AutoGeneratedGetMemberValueIsPartyLeader(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).IsPartyLeader;
			}

			// Token: 0x0400259A RID: 9626
			[SaveableField(1)]
			public Hero TeleportingHero;

			// Token: 0x0400259B RID: 9627
			[SaveableField(2)]
			public CampaignTime TeleportationTime;

			// Token: 0x0400259C RID: 9628
			[SaveableField(3)]
			public Settlement TargetSettlement;

			// Token: 0x0400259D RID: 9629
			[SaveableField(4)]
			public bool IsGovernor;

			// Token: 0x0400259E RID: 9630
			[SaveableField(5)]
			public MobileParty TargetParty;

			// Token: 0x0400259F RID: 9631
			[SaveableField(6)]
			public bool IsPartyLeader;
		}
	}
}
