using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Quests
{
	// Token: 0x02000025 RID: 37
	public class QuestStageVM : ViewModel
	{
		// Token: 0x0600024A RID: 586 RVA: 0x00013A50 File Offset: 0x00011C50
		public QuestStageVM(JournalLog log, string dateString, bool isLastStage, Action onLogNotified, QuestStageTaskVM stageTask = null)
		{
			this.StageTask = new QuestStageTaskVM(TextObject.GetEmpty(), 0, 0, LogType.None);
			this._onLogNotified = onLogNotified;
			string text = log.LogText.ToString();
			GameTexts.SetVariable("ENTRY", text);
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this.DateText = dateString;
			this.DescriptionText = log.LogText.ToString();
			this.IsLastStage = isLastStage;
			this.Log = log;
			this.UpdateIsNew();
			if (stageTask != null)
			{
				this.StageTask = stageTask;
				this.StageTask.IsValid = true;
				this.HasATask = true;
				this.IsTaskCompleted = this.StageTask.CurrentProgress == this.StageTask.TargetProgress;
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00013B10 File Offset: 0x00011D10
		public QuestStageVM(JournalLog log, string description, string dateString, bool isLastStage, Action onLogNotified)
		{
			this.Log = log;
			this.StageTask = new QuestStageTaskVM(TextObject.GetEmpty(), 0, 0, LogType.None);
			this._onLogNotified = onLogNotified;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this.DateText = dateString;
			this.DescriptionText = description;
			this.IsLastStage = isLastStage;
			this.UpdateIsNew();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00013B71 File Offset: 0x00011D71
		public void ExecuteResetUpdated()
		{
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00013B73 File Offset: 0x00011D73
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00013B85 File Offset: 0x00011D85
		public void UpdateIsNew()
		{
			if (this.Log != null)
			{
				this.IsNew = this._viewDataTracker.UnExaminedQuestLogs.Any<JournalLog>((JournalLog l) => l == this.Log);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00013BB1 File Offset: 0x00011DB1
		// (set) Token: 0x06000250 RID: 592 RVA: 0x00013BB9 File Offset: 0x00011DB9
		[DataSourceProperty]
		public string DateText
		{
			get
			{
				return this._dateText;
			}
			set
			{
				if (value != this._dateText)
				{
					this._dateText = value;
					base.OnPropertyChangedWithValue<string>(value, "DateText");
				}
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00013BDC File Offset: 0x00011DDC
		// (set) Token: 0x06000252 RID: 594 RVA: 0x00013BE4 File Offset: 0x00011DE4
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00013C07 File Offset: 0x00011E07
		// (set) Token: 0x06000254 RID: 596 RVA: 0x00013C0F File Offset: 0x00011E0F
		[DataSourceProperty]
		public bool HasATask
		{
			get
			{
				return this._hasATask;
			}
			set
			{
				if (value != this._hasATask)
				{
					this._hasATask = value;
					base.OnPropertyChangedWithValue(value, "HasATask");
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000255 RID: 597 RVA: 0x00013C2D File Offset: 0x00011E2D
		// (set) Token: 0x06000256 RID: 598 RVA: 0x00013C35 File Offset: 0x00011E35
		[DataSourceProperty]
		public bool IsNew
		{
			get
			{
				return this._isNew;
			}
			set
			{
				if (value != this._isNew)
				{
					this._isNew = value;
					base.OnPropertyChangedWithValue(value, "IsNew");
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00013C53 File Offset: 0x00011E53
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00013C5B File Offset: 0x00011E5B
		[DataSourceProperty]
		public bool IsLastStage
		{
			get
			{
				return this._isLastStage;
			}
			set
			{
				if (value != this._isLastStage)
				{
					this._isLastStage = value;
					base.OnPropertyChangedWithValue(value, "IsLastStage");
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000259 RID: 601 RVA: 0x00013C79 File Offset: 0x00011E79
		// (set) Token: 0x0600025A RID: 602 RVA: 0x00013C81 File Offset: 0x00011E81
		[DataSourceProperty]
		public bool IsTaskCompleted
		{
			get
			{
				return this._isTaskCompleted;
			}
			set
			{
				if (value != this._isTaskCompleted)
				{
					this._isTaskCompleted = value;
					base.OnPropertyChangedWithValue(value, "IsTaskCompleted");
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00013C9F File Offset: 0x00011E9F
		// (set) Token: 0x0600025C RID: 604 RVA: 0x00013CA7 File Offset: 0x00011EA7
		[DataSourceProperty]
		public QuestStageTaskVM StageTask
		{
			get
			{
				return this._stageTask;
			}
			set
			{
				if (value != this._stageTask)
				{
					this._stageTask = value;
					base.OnPropertyChangedWithValue<QuestStageTaskVM>(value, "StageTask");
				}
			}
		}

		// Token: 0x0400010A RID: 266
		public readonly JournalLog Log;

		// Token: 0x0400010B RID: 267
		private readonly Action _onLogNotified;

		// Token: 0x0400010C RID: 268
		private readonly IViewDataTracker _viewDataTracker;

		// Token: 0x0400010D RID: 269
		private string _descriptionText;

		// Token: 0x0400010E RID: 270
		private string _dateText;

		// Token: 0x0400010F RID: 271
		private bool _hasATask;

		// Token: 0x04000110 RID: 272
		private bool _isNew;

		// Token: 0x04000111 RID: 273
		private bool _isTaskCompleted;

		// Token: 0x04000112 RID: 274
		private bool _isLastStage;

		// Token: 0x04000113 RID: 275
		private QuestStageTaskVM _stageTask;
	}
}
