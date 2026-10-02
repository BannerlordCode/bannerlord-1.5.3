using System;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003BA RID: 954
	public class QuestsState : GameState
	{
		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06003762 RID: 14178 RVA: 0x000DFC48 File Offset: 0x000DDE48
		// (set) Token: 0x06003763 RID: 14179 RVA: 0x000DFC50 File Offset: 0x000DDE50
		public IssueBase InitialSelectedIssue { get; private set; }

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06003764 RID: 14180 RVA: 0x000DFC59 File Offset: 0x000DDE59
		// (set) Token: 0x06003765 RID: 14181 RVA: 0x000DFC61 File Offset: 0x000DDE61
		public QuestBase InitialSelectedQuest { get; private set; }

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06003766 RID: 14182 RVA: 0x000DFC6A File Offset: 0x000DDE6A
		// (set) Token: 0x06003767 RID: 14183 RVA: 0x000DFC72 File Offset: 0x000DDE72
		public JournalLogEntry InitialSelectedLog { get; private set; }

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06003768 RID: 14184 RVA: 0x000DFC7B File Offset: 0x000DDE7B
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06003769 RID: 14185 RVA: 0x000DFC7E File Offset: 0x000DDE7E
		// (set) Token: 0x0600376A RID: 14186 RVA: 0x000DFC86 File Offset: 0x000DDE86
		public IQuestsStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x000DFC8F File Offset: 0x000DDE8F
		public QuestsState()
		{
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x000DFC97 File Offset: 0x000DDE97
		public QuestsState(IssueBase initialSelectedIssue)
		{
			this.InitialSelectedIssue = initialSelectedIssue;
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x000DFCA6 File Offset: 0x000DDEA6
		public QuestsState(QuestBase initialSelectedQuest)
		{
			this.InitialSelectedQuest = initialSelectedQuest;
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x000DFCB5 File Offset: 0x000DDEB5
		public QuestsState(JournalLogEntry initialSelectedLog)
		{
			this.InitialSelectedLog = initialSelectedLog;
		}

		// Token: 0x04000F8F RID: 3983
		private IQuestsStateHandler _handler;
	}
}
