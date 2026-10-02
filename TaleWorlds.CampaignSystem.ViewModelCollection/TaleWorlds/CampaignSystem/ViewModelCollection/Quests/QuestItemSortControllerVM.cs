using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Quests
{
	// Token: 0x02000021 RID: 33
	public class QuestItemSortControllerVM : ViewModel
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00012DF7 File Offset: 0x00010FF7
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x00012DFF File Offset: 0x00010FFF
		public QuestItemSortControllerVM.QuestItemSortOption? CurrentSortOption { get; private set; }

		// Token: 0x060001F8 RID: 504 RVA: 0x00012E08 File Offset: 0x00011008
		public QuestItemSortControllerVM(ref MBBindingList<QuestItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._dateStartedComparer = new QuestItemSortControllerVM.QuestItemDateStartedComparer();
			this._lastUpdatedComparer = new QuestItemSortControllerVM.QuestItemLastUpdatedComparer();
			this._timeDueComparer = new QuestItemSortControllerVM.QuestItemTimeDueComparer();
			this.IsThereAnyQuest = this._listToControl.Count > 0;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00012E58 File Offset: 0x00011058
		private void ExecuteSortByDateStarted()
		{
			this._listToControl.Sort(this._dateStartedComparer);
			this.CurrentSortOption = new QuestItemSortControllerVM.QuestItemSortOption?(QuestItemSortControllerVM.QuestItemSortOption.DateStarted);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00012E77 File Offset: 0x00011077
		private void ExecuteSortByLastUpdated()
		{
			this._listToControl.Sort(this._lastUpdatedComparer);
			this.CurrentSortOption = new QuestItemSortControllerVM.QuestItemSortOption?(QuestItemSortControllerVM.QuestItemSortOption.LastUpdated);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00012E96 File Offset: 0x00011096
		private void ExecuteSortByTimeDue()
		{
			this._listToControl.Sort(this._timeDueComparer);
			this.CurrentSortOption = new QuestItemSortControllerVM.QuestItemSortOption?(QuestItemSortControllerVM.QuestItemSortOption.TimeDue);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00012EB5 File Offset: 0x000110B5
		public void SortByOption(QuestItemSortControllerVM.QuestItemSortOption sortOption)
		{
			if (sortOption == QuestItemSortControllerVM.QuestItemSortOption.DateStarted)
			{
				this.ExecuteSortByDateStarted();
				return;
			}
			if (sortOption == QuestItemSortControllerVM.QuestItemSortOption.LastUpdated)
			{
				this.ExecuteSortByLastUpdated();
				return;
			}
			if (sortOption == QuestItemSortControllerVM.QuestItemSortOption.TimeDue)
			{
				this.ExecuteSortByTimeDue();
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00012ED6 File Offset: 0x000110D6
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00012EDE File Offset: 0x000110DE
		[DataSourceProperty]
		public bool IsThereAnyQuest
		{
			get
			{
				return this._isThereAnyQuest;
			}
			set
			{
				if (value != this._isThereAnyQuest)
				{
					this._isThereAnyQuest = value;
					base.OnPropertyChangedWithValue(value, "IsThereAnyQuest");
				}
			}
		}

		// Token: 0x040000E2 RID: 226
		private MBBindingList<QuestItemVM> _listToControl;

		// Token: 0x040000E3 RID: 227
		private QuestItemSortControllerVM.QuestItemDateStartedComparer _dateStartedComparer;

		// Token: 0x040000E4 RID: 228
		private QuestItemSortControllerVM.QuestItemLastUpdatedComparer _lastUpdatedComparer;

		// Token: 0x040000E5 RID: 229
		private QuestItemSortControllerVM.QuestItemTimeDueComparer _timeDueComparer;

		// Token: 0x040000E7 RID: 231
		private bool _isThereAnyQuest;

		// Token: 0x02000192 RID: 402
		public enum QuestItemSortOption
		{
			// Token: 0x040010C1 RID: 4289
			DateStarted,
			// Token: 0x040010C2 RID: 4290
			LastUpdated,
			// Token: 0x040010C3 RID: 4291
			TimeDue
		}

		// Token: 0x02000193 RID: 403
		private abstract class QuestItemComparerBase : IComparer<QuestItemVM>
		{
			// Token: 0x060023DB RID: 9179
			public abstract int Compare(QuestItemVM x, QuestItemVM y);

			// Token: 0x060023DC RID: 9180 RVA: 0x0007F52C File Offset: 0x0007D72C
			protected JournalLog GetJournalLogAt(QuestItemVM questItem, QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex logIndex)
			{
				if (questItem.Quest == null && questItem.Stages.Count > 0)
				{
					int num = ((logIndex == QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.First) ? 0 : (questItem.Stages.Count - 1));
					return questItem.Stages[num].Log;
				}
				if (questItem.Quest != null && questItem.Quest.JournalEntries.Count > 0)
				{
					int num2 = ((logIndex == QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.First) ? 0 : (questItem.Quest.JournalEntries.Count - 1));
					return questItem.Quest.JournalEntries[num2];
				}
				return null;
			}

			// Token: 0x020002FE RID: 766
			protected enum JournalLogIndex
			{
				// Token: 0x04001459 RID: 5209
				First,
				// Token: 0x0400145A RID: 5210
				Last
			}
		}

		// Token: 0x02000194 RID: 404
		private class QuestItemDateStartedComparer : QuestItemSortControllerVM.QuestItemComparerBase
		{
			// Token: 0x060023DE RID: 9182 RVA: 0x0007F5C4 File Offset: 0x0007D7C4
			public override int Compare(QuestItemVM first, QuestItemVM second)
			{
				JournalLog journalLogAt = base.GetJournalLogAt(first, QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.First);
				JournalLog journalLogAt2 = base.GetJournalLogAt(second, QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.First);
				if (journalLogAt != null && journalLogAt2 != null)
				{
					return journalLogAt.LogTime.CompareTo(journalLogAt2.LogTime);
				}
				if (journalLogAt == null && journalLogAt2 != null)
				{
					return -1;
				}
				if (journalLogAt != null && journalLogAt2 == null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x02000195 RID: 405
		private class QuestItemLastUpdatedComparer : QuestItemSortControllerVM.QuestItemComparerBase
		{
			// Token: 0x060023E0 RID: 9184 RVA: 0x0007F618 File Offset: 0x0007D818
			public override int Compare(QuestItemVM first, QuestItemVM second)
			{
				JournalLog journalLogAt = base.GetJournalLogAt(first, QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.Last);
				JournalLog journalLogAt2 = base.GetJournalLogAt(second, QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.Last);
				if (journalLogAt != null && journalLogAt2 != null)
				{
					return journalLogAt2.LogTime.CompareTo(journalLogAt.LogTime);
				}
				if (journalLogAt == null && journalLogAt2 != null)
				{
					return -1;
				}
				if (journalLogAt != null && journalLogAt2 == null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x02000196 RID: 406
		private class QuestItemTimeDueComparer : QuestItemSortControllerVM.QuestItemComparerBase
		{
			// Token: 0x060023E2 RID: 9186 RVA: 0x0007F66C File Offset: 0x0007D86C
			public override int Compare(QuestItemVM first, QuestItemVM second)
			{
				CampaignTime campaignTime = CampaignTime.Now;
				CampaignTime campaignTime2 = CampaignTime.Now;
				if (first.Quest != null)
				{
					campaignTime = first.Quest.QuestDueTime;
				}
				if (second.Quest != null)
				{
					campaignTime2 = second.Quest.QuestDueTime;
				}
				return campaignTime.CompareTo(campaignTime2);
			}
		}
	}
}
