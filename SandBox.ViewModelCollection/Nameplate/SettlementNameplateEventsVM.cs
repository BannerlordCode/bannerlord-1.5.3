using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x0200001C RID: 28
	public class SettlementNameplateEventsVM : ViewModel
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000C3B8 File Offset: 0x0000A5B8
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000C3C0 File Offset: 0x0000A5C0
		public bool IsEventsRegistered { get; private set; }

		// Token: 0x060002BC RID: 700 RVA: 0x0000C3C9 File Offset: 0x0000A5C9
		public SettlementNameplateEventsVM(Settlement settlement)
		{
			this._settlement = settlement;
			this.EventsList = new MBBindingList<SettlementNameplateEventItemVM>();
			this.TrackQuests = new MBBindingList<QuestMarkerVM>();
			this._relatedQuests = new List<QuestBase>();
			if (settlement.IsVillage)
			{
				this.AddPrimaryProductionIcon();
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000C407 File Offset: 0x0000A607
		public void Tick()
		{
			if (this._areQuestsDirty)
			{
				this.RefreshQuestCounts();
				this._areQuestsDirty = false;
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000C41E File Offset: 0x0000A61E
		private void PopulateEventList()
		{
			if (Campaign.Current.TournamentManager.GetTournamentGame(this._settlement.Town) != null)
			{
				this.EventsList.Add(new SettlementNameplateEventItemVM(SettlementNameplateEventItemVM.SettlementEventType.Tournament));
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000C450 File Offset: 0x0000A650
		public void RegisterEvents()
		{
			if (!this.IsEventsRegistered)
			{
				this.PopulateEventList();
				CampaignEvents.TournamentStarted.AddNonSerializedListener(this, new Action<Town>(this.OnTournamentStarted));
				CampaignEvents.TournamentFinished.AddNonSerializedListener(this, new Action<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject>(this.OnTournamentFinished));
				CampaignEvents.TournamentCancelled.AddNonSerializedListener(this, new Action<Town>(this.OnTournamentCancelled));
				CampaignEvents.OnNewIssueCreatedEvent.AddNonSerializedListener(this, new Action<IssueBase>(this.OnNewIssueCreated));
				CampaignEvents.OnIssueUpdatedEvent.AddNonSerializedListener(this, new Action<IssueBase, IssueBase.IssueUpdateDetails, Hero>(this.OnIssueUpdated));
				CampaignEvents.OnQuestStartedEvent.AddNonSerializedListener(this, new Action<QuestBase>(this.OnQuestStarted));
				CampaignEvents.QuestLogAddedEvent.AddNonSerializedListener(this, new Action<QuestBase, bool>(this.OnQuestLogAdded));
				CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
				CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
				CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroTakenPrisoner));
				this.IsEventsRegistered = true;
				this.RefreshQuestCounts();
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000C578 File Offset: 0x0000A778
		public void UnloadEvents()
		{
			if (this.IsEventsRegistered)
			{
				CampaignEvents.TournamentStarted.ClearListeners(this);
				CampaignEvents.TournamentFinished.ClearListeners(this);
				CampaignEvents.TournamentCancelled.ClearListeners(this);
				CampaignEvents.OnNewIssueCreatedEvent.ClearListeners(this);
				CampaignEvents.OnIssueUpdatedEvent.ClearListeners(this);
				CampaignEvents.OnQuestStartedEvent.ClearListeners(this);
				CampaignEvents.QuestLogAddedEvent.ClearListeners(this);
				CampaignEvents.OnQuestCompletedEvent.ClearListeners(this);
				CampaignEvents.SettlementEntered.ClearListeners(this);
				CampaignEvents.OnSettlementLeftEvent.ClearListeners(this);
				CampaignEvents.HeroPrisonerTaken.ClearListeners(this);
				int num = this.EventsList.Count;
				for (int i = 0; i < num; i++)
				{
					if (this.EventsList[i].EventType != SettlementNameplateEventItemVM.SettlementEventType.Production)
					{
						this.EventsList.RemoveAt(i);
						num--;
						i--;
					}
				}
				this.IsEventsRegistered = false;
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000C650 File Offset: 0x0000A850
		private void OnTournamentStarted(Town town)
		{
			if (this._settlement.Town != null && town == this._settlement.Town)
			{
				bool flag = false;
				for (int i = 0; i < this.EventsList.Count; i++)
				{
					if (this.EventsList[i].EventType == SettlementNameplateEventItemVM.SettlementEventType.Tournament)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.EventsList.Add(new SettlementNameplateEventItemVM(SettlementNameplateEventItemVM.SettlementEventType.Tournament));
				}
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000C6BB File Offset: 0x0000A8BB
		private void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
			this.RemoveTournament(town);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000C6C4 File Offset: 0x0000A8C4
		private void OnTournamentCancelled(Town town)
		{
			this.RemoveTournament(town);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000C6D0 File Offset: 0x0000A8D0
		private void RemoveTournament(Town town)
		{
			if (this._settlement.Town != null && town == this._settlement.Town)
			{
				if (this.EventsList.Count<SettlementNameplateEventItemVM>((SettlementNameplateEventItemVM e) => e.EventType == SettlementNameplateEventItemVM.SettlementEventType.Tournament) > 0)
				{
					int num = -1;
					for (int i = 0; i < this.EventsList.Count; i++)
					{
						if (this.EventsList[i].EventType == SettlementNameplateEventItemVM.SettlementEventType.Tournament)
						{
							num = i;
							break;
						}
					}
					if (num != -1)
					{
						this.EventsList.RemoveAt(num);
						return;
					}
					Debug.FailedAssert("There should be a tournament item to remove", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Nameplate\\SettlementNameplateEventsVM.cs", "RemoveTournament", 164);
				}
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000C784 File Offset: 0x0000A984
		private void RefreshQuestCounts()
		{
			this._relatedQuests.Clear();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = Campaign.Current.IssueManager.GetNumOfActiveIssuesInSettlement(this._settlement, false);
			int numOfAvailableIssuesInSettlement = Campaign.Current.IssueManager.GetNumOfAvailableIssuesInSettlement(this._settlement);
			this.TrackQuests.Clear();
			List<QuestBase> list;
			if (Campaign.Current.QuestManager.TrackedObjects.TryGetValue(this._settlement, out list))
			{
				foreach (QuestBase questBase in list)
				{
					if (questBase.IsSpecialQuest)
					{
						if (!this.TrackQuests.Any<QuestMarkerVM>((QuestMarkerVM x) => x.IssueQuestFlag == CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest))
						{
							this.TrackQuests.Add(new QuestMarkerVM(CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest, null, null));
							this._relatedQuests.Add(questBase);
							continue;
						}
					}
					if (!this.TrackQuests.Any<QuestMarkerVM>((QuestMarkerVM x) => x.IssueQuestFlag == CampaignUIHelper.IssueQuestFlags.TrackedIssue))
					{
						this.TrackQuests.Add(new QuestMarkerVM(CampaignUIHelper.IssueQuestFlags.TrackedIssue, null, null));
						this._relatedQuests.Add(questBase);
					}
				}
			}
			List<ValueTuple<bool, QuestBase>> questsRelatedToSettlement = CampaignUIHelper.GetQuestsRelatedToSettlement(this._settlement);
			for (int i = 0; i < questsRelatedToSettlement.Count; i++)
			{
				if (questsRelatedToSettlement[i].Item1)
				{
					if (questsRelatedToSettlement[i].Item2.IsSpecialQuest)
					{
						num++;
					}
					else
					{
						num4++;
					}
				}
				else if (questsRelatedToSettlement[i].Item2.IsSpecialQuest)
				{
					num3++;
				}
				else
				{
					num2++;
				}
				this._relatedQuests.Add(questsRelatedToSettlement[i].Item2);
			}
			this.HandleIssueCount(numOfAvailableIssuesInSettlement, SettlementNameplateEventItemVM.SettlementEventType.AvailableIssue);
			this.HandleIssueCount(num4, SettlementNameplateEventItemVM.SettlementEventType.ActiveQuest);
			this.HandleIssueCount(num, SettlementNameplateEventItemVM.SettlementEventType.ActiveStoryQuest);
			this.HandleIssueCount(num2, SettlementNameplateEventItemVM.SettlementEventType.TrackedIssue);
			this.HandleIssueCount(num3, SettlementNameplateEventItemVM.SettlementEventType.TrackedStoryQuest);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000C998 File Offset: 0x0000AB98
		private void OnNewIssueCreated(IssueBase issue)
		{
			if (issue.IssueSettlement != this._settlement)
			{
				Hero issueOwner = issue.IssueOwner;
				if (((issueOwner != null) ? issueOwner.CurrentSettlement : null) != this._settlement)
				{
					return;
				}
			}
			this._areQuestsDirty = true;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000C9C9 File Offset: 0x0000ABC9
		private void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero hero)
		{
			if (issue.IssueSettlement == this._settlement && issue.IssueQuest == null)
			{
				this._areQuestsDirty = true;
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000C9E8 File Offset: 0x0000ABE8
		private void OnQuestStarted(QuestBase quest)
		{
			if (this.IsQuestRelated(quest))
			{
				this._areQuestsDirty = true;
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000C9FA File Offset: 0x0000ABFA
		private void OnQuestLogAdded(QuestBase quest, bool hideInformation)
		{
			if (this.IsQuestRelated(quest))
			{
				this._areQuestsDirty = true;
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000CA0C File Offset: 0x0000AC0C
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails details)
		{
			if (this.IsQuestRelated(quest))
			{
				this._areQuestsDirty = true;
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000CA1E File Offset: 0x0000AC1E
		private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (settlement == this._settlement)
			{
				this._areQuestsDirty = true;
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000CA30 File Offset: 0x0000AC30
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (settlement == this._settlement)
			{
				this._areQuestsDirty = true;
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000CA42 File Offset: 0x0000AC42
		private void OnHeroTakenPrisoner(PartyBase capturer, Hero prisoner)
		{
			if (prisoner.CurrentSettlement == this._settlement)
			{
				this._areQuestsDirty = true;
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000CA5C File Offset: 0x0000AC5C
		private void AddPrimaryProductionIcon()
		{
			string stringId = this._settlement.Village.VillageType.PrimaryProduction.StringId;
			string text = (stringId.Contains("camel") ? "camel" : ((stringId.Contains("horse") || stringId.Contains("mule")) ? "horse" : stringId));
			this.EventsList.Add(new SettlementNameplateEventItemVM(text));
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000CACC File Offset: 0x0000ACCC
		private void HandleIssueCount(int count, SettlementNameplateEventItemVM.SettlementEventType eventType)
		{
			SettlementNameplateEventItemVM settlementNameplateEventItemVM = this.EventsList.FirstOrDefault<SettlementNameplateEventItemVM>((SettlementNameplateEventItemVM e) => e.EventType == eventType);
			if (count > 0 && settlementNameplateEventItemVM == null)
			{
				this.EventsList.Add(new SettlementNameplateEventItemVM(eventType));
				return;
			}
			if (count == 0 && settlementNameplateEventItemVM != null)
			{
				this.EventsList.Remove(settlementNameplateEventItemVM);
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000CB30 File Offset: 0x0000AD30
		private bool IsQuestRelated(QuestBase quest)
		{
			IssueBase issueOfQuest = IssueManager.GetIssueOfQuest(quest);
			return (issueOfQuest != null && issueOfQuest.IssueSettlement == this._settlement) || this._relatedQuests.Contains(quest) || CampaignUIHelper.IsQuestRelatedToSettlement(quest, this._settlement);
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000CB71 File Offset: 0x0000AD71
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x0000CB79 File Offset: 0x0000AD79
		[DataSourceProperty]
		public MBBindingList<QuestMarkerVM> TrackQuests
		{
			get
			{
				return this._trackQuests;
			}
			set
			{
				if (value != this._trackQuests)
				{
					this._trackQuests = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestMarkerVM>>(value, "TrackQuests");
				}
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000CB97 File Offset: 0x0000AD97
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x0000CB9F File Offset: 0x0000AD9F
		public MBBindingList<SettlementNameplateEventItemVM> EventsList
		{
			get
			{
				return this._eventsList;
			}
			set
			{
				if (value != this._eventsList)
				{
					this._eventsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementNameplateEventItemVM>>(value, "EventsList");
				}
			}
		}

		// Token: 0x04000161 RID: 353
		private List<QuestBase> _relatedQuests;

		// Token: 0x04000162 RID: 354
		private Settlement _settlement;

		// Token: 0x04000163 RID: 355
		private bool _areQuestsDirty;

		// Token: 0x04000164 RID: 356
		private MBBindingList<QuestMarkerVM> _trackQuests;

		// Token: 0x04000165 RID: 357
		private MBBindingList<SettlementNameplateEventItemVM> _eventsList;
	}
}
