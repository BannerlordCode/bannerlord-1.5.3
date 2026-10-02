using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000433 RID: 1075
	public class JournalLogsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600440E RID: 17422 RVA: 0x00140460 File Offset: 0x0013E660
		public override void RegisterEvents()
		{
			CampaignEvents.OnQuestStartedEvent.AddNonSerializedListener(this, new Action<QuestBase>(this.OnQuestStarted));
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			CampaignEvents.OnIssueUpdatedEvent.AddNonSerializedListener(this, new Action<IssueBase, IssueBase.IssueUpdateDetails, Hero>(this.OnIssueUpdated));
			CampaignEvents.IssueLogAddedEvent.AddNonSerializedListener(this, new Action<IssueBase, bool>(this.OnIssueLogAdded));
			CampaignEvents.QuestLogAddedEvent.AddNonSerializedListener(this, new Action<QuestBase, bool>(this.OnQuestLogAdded));
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x001404E0 File Offset: 0x0013E6E0
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x001404E4 File Offset: 0x0013E6E4
		private void OnIssueLogAdded(IssueBase issue, bool hideInformation)
		{
			JournalLogEntry journalLogEntry = this.GetRelatedLog(issue);
			if (journalLogEntry == null)
			{
				journalLogEntry = this.CreateRelatedLog(issue);
				LogEntry.AddLogEntry(journalLogEntry);
			}
			journalLogEntry.Update(this.GetEntries(issue), IssueBase.IssueUpdateDetails.None);
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x00140518 File Offset: 0x0013E718
		private void OnQuestLogAdded(QuestBase quest, bool hideInformation)
		{
			JournalLogEntry journalLogEntry = this.GetRelatedLog(quest);
			if (journalLogEntry == null)
			{
				journalLogEntry = this.CreateRelatedLog(quest);
				LogEntry.AddLogEntry(journalLogEntry);
			}
			journalLogEntry.Update(this.GetEntries(quest), IssueBase.IssueUpdateDetails.None);
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x0014054C File Offset: 0x0013E74C
		private void OnQuestStarted(QuestBase quest)
		{
			JournalLogEntry journalLogEntry = this.GetRelatedLog(quest);
			if (journalLogEntry == null)
			{
				journalLogEntry = this.CreateRelatedLog(quest);
				LogEntry.AddLogEntry(journalLogEntry);
			}
			journalLogEntry.Update(this.GetEntries(quest), IssueBase.IssueUpdateDetails.None);
			LogEntry.AddLogEntry(new IssueQuestStartLogEntry(journalLogEntry.RelatedHero));
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x00140590 File Offset: 0x0013E790
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			JournalLogEntry journalLogEntry = this.GetRelatedLog(quest);
			if (journalLogEntry == null)
			{
				journalLogEntry = this.CreateRelatedLog(quest);
				LogEntry.AddLogEntry(journalLogEntry);
			}
			journalLogEntry.Update(this.GetEntries(quest), detail);
			LogEntry.AddLogEntry(new IssueQuestLogEntry(journalLogEntry.RelatedHero, journalLogEntry.Antagonist, detail));
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x001405DC File Offset: 0x0013E7DC
		private void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero issueSolver)
		{
			if (issueSolver == Hero.MainHero)
			{
				JournalLogEntry journalLogEntry = this.GetRelatedLog(issue);
				if (journalLogEntry == null)
				{
					journalLogEntry = this.CreateRelatedLog(issue);
					LogEntry.AddLogEntry(journalLogEntry);
				}
				journalLogEntry.Update(this.GetEntries(issue), details);
			}
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x00140618 File Offset: 0x0013E818
		private JournalLogEntry CreateRelatedLog(IssueBase issue)
		{
			if (issue.IssueQuest != null)
			{
				return new JournalLogEntry(issue.IssueQuest.Title, issue.IssueQuest.QuestGiver, null, issue.IssueQuest.IsSpecialQuest, new MBObjectBase[] { issue, issue.IssueQuest });
			}
			return new JournalLogEntry(issue.Title, issue.IssueOwner, issue.CounterOfferHero, false, new MBObjectBase[] { issue });
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x0014068C File Offset: 0x0013E88C
		private JournalLogEntry CreateRelatedLog(QuestBase quest)
		{
			IssueBase issueOfQuest = IssueManager.GetIssueOfQuest(quest);
			if (issueOfQuest != null)
			{
				return this.CreateRelatedLog(issueOfQuest);
			}
			return new JournalLogEntry(quest.Title, quest.QuestGiver, null, quest.IsSpecialQuest, new MBObjectBase[] { quest });
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x001406D0 File Offset: 0x0013E8D0
		private JournalLogEntry GetRelatedLog(IssueBase issue)
		{
			return Campaign.Current.LogEntryHistory.FindLastGameActionLog<JournalLogEntry>((JournalLogEntry x) => x.IsRelatedTo(issue));
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x00140708 File Offset: 0x0013E908
		private JournalLogEntry GetRelatedLog(QuestBase quest)
		{
			IssueBase issueOfQuest = IssueManager.GetIssueOfQuest(quest);
			if (issueOfQuest != null)
			{
				return this.GetRelatedLog(issueOfQuest);
			}
			return Campaign.Current.LogEntryHistory.FindLastGameActionLog<JournalLogEntry>((JournalLogEntry x) => x.IsRelatedTo(quest));
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x00140754 File Offset: 0x0013E954
		private MBReadOnlyList<JournalLog> GetEntries(IssueBase issue)
		{
			if (issue.IssueQuest == null)
			{
				return issue.JournalEntries;
			}
			MBList<JournalLog> mblist = issue.JournalEntries.ToMBList<JournalLog>();
			JournalLog journalLog = issue.IssueQuest.JournalEntries.FirstOrDefault<JournalLog>();
			if (journalLog != null)
			{
				int i;
				for (i = 0; i < mblist.Count; i++)
				{
					if (mblist[i].LogTime > journalLog.LogTime)
					{
						i--;
						break;
					}
				}
				mblist.InsertRange(i, issue.IssueQuest.JournalEntries);
			}
			return mblist;
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x001407D4 File Offset: 0x0013E9D4
		private MBReadOnlyList<JournalLog> GetEntries(QuestBase quest)
		{
			IssueBase issueOfQuest = IssueManager.GetIssueOfQuest(quest);
			if (issueOfQuest != null)
			{
				return this.GetEntries(issueOfQuest);
			}
			return quest.JournalEntries;
		}
	}
}
