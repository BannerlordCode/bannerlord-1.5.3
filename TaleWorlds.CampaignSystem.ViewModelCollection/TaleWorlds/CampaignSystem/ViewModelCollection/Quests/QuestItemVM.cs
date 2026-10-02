using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Quests
{
	// Token: 0x02000022 RID: 34
	public class QuestItemVM : ViewModel
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00012EFC File Offset: 0x000110FC
		public QuestBase Quest { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00012F04 File Offset: 0x00011104
		public IssueBase Issue { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00012F0C File Offset: 0x0001110C
		public JournalLogEntry QuestLogEntry { get; }

		// Token: 0x06000202 RID: 514 RVA: 0x00012F14 File Offset: 0x00011114
		public QuestItemVM(JournalLogEntry questLogEntry, Action<QuestItemVM> onSelection, QuestsVM.QuestCompletionType completion)
		{
			this._onSelection = onSelection;
			this.QuestLogEntry = questLogEntry;
			this.Stages = new MBBindingList<QuestStageVM>();
			this._completionType = completion;
			this.IsCompleted = this._completionType > QuestsVM.QuestCompletionType.Active;
			this.IsCompletedSuccessfully = this._completionType == QuestsVM.QuestCompletionType.Successful;
			this.CompletionTypeAsInt = (int)this._completionType;
			bool flag;
			if (!this.IsCompleted)
			{
				QuestBase quest = this.Quest;
				CampaignTime? campaignTime = ((quest != null) ? new CampaignTime?(quest.QuestDueTime) : null);
				CampaignTime never = CampaignTime.Never;
				flag = campaignTime != null && (campaignTime == null || campaignTime.GetValueOrDefault() == never);
			}
			else
			{
				flag = true;
			}
			this.IsRemainingDaysHidden = flag;
			this.IsQuestGiverHeroHidden = false;
			this.IsMainQuest = questLogEntry.IsSpecial;
			foreach (JournalLog journalLog in questLogEntry.GetEntries())
			{
				this.PopulateQuestLog(journalLog, false);
			}
			this.Name = questLogEntry.Title.ToString();
			this.QuestGiverHero = new HeroVM(questLogEntry.RelatedHero, false);
			this.UpdateIsUpdated();
			this.IsTracked = false;
			this.IsTrackable = false;
			this.RefreshValues();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00013060 File Offset: 0x00011260
		public QuestItemVM(QuestBase quest, Action<QuestItemVM> onSelection)
		{
			this.Quest = quest;
			this._onSelection = onSelection;
			this.Stages = new MBBindingList<QuestStageVM>();
			this.CompletionTypeAsInt = 0;
			this.IsRemainingDaysHidden = !this.Quest.IsOngoing || this.Quest.IsRemainingTimeHidden;
			this.IsQuestGiverHeroHidden = this.Quest.QuestGiver == null;
			MBReadOnlyList<JournalLog> journalEntries = this.Quest.JournalEntries;
			for (int i = 0; i < journalEntries.Count; i++)
			{
				bool flag = i == journalEntries.Count - 1;
				JournalLog journalLog = journalEntries[i];
				this.PopulateQuestLog(journalLog, flag);
			}
			this.IsMainQuest = quest.IsSpecialQuest;
			if (!this.IsQuestGiverHeroHidden)
			{
				this.QuestGiverHero = new HeroVM(this.Quest.QuestGiver, false);
			}
			this.UpdateIsUpdated();
			this.IsTrackable = !this.Quest.IsFinalized;
			this.IsTracked = this.Quest.IsTrackEnabled;
			this.RefreshValues();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00013160 File Offset: 0x00011360
		public QuestItemVM(IssueBase issue, Action<QuestItemVM> onSelection)
		{
			this.Issue = issue;
			this._onSelection = onSelection;
			this.Stages = new MBBindingList<QuestStageVM>();
			this.IsCompleted = false;
			this.CompletionTypeAsInt = 0;
			this.IsRemainingDaysHidden = this.Issue.IsOngoingWithoutQuest;
			this.IsQuestGiverHeroHidden = false;
			this.UpdateRemainingTime(this.Issue.IssueDueTime);
			foreach (JournalLog journalLog in issue.JournalEntries)
			{
				this.PopulateQuestLog(journalLog, false);
			}
			this.Name = issue.Title.ToString();
			this.QuestGiverHero = new HeroVM(issue.IssueOwner, false);
			this.UpdateIsUpdated();
			this.IsTrackable = false;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0001323C File Offset: 0x0001143C
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.Quest != null)
			{
				this.Name = this.Quest.Title.ToString();
				this.UpdateRemainingTime(this.Quest.QuestDueTime);
			}
			else if (this.Issue != null)
			{
				this.Name = this.Issue.Title.ToString();
				this.UpdateRemainingTime(this.Issue.IssueDueTime);
			}
			else if (this.QuestLogEntry != null)
			{
				this.Name = this.QuestLogEntry.Title.ToString();
			}
			HeroVM questGiverHero = this.QuestGiverHero;
			if (questGiverHero != null)
			{
				questGiverHero.RefreshValues();
			}
			this.Stages.ApplyActionOnAllItems(delegate(QuestStageVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0001330C File Offset: 0x0001150C
		private void UpdateRemainingTime(CampaignTime dueTime)
		{
			if (this.IsRemainingDaysHidden)
			{
				this.RemainingDays = 0;
			}
			else
			{
				this.RemainingDays = (int)(dueTime - CampaignTime.Now).ToDays;
			}
			GameTexts.SetVariable("DAY_IS_PLURAL", (this.RemainingDays > 1) ? 1 : 0);
			GameTexts.SetVariable("DAY", this.RemainingDays);
			if (dueTime.ToHours - CampaignTime.Now.ToHours < (double)CampaignTime.HoursInDay)
			{
				this.RemainingDaysText = GameTexts.FindText("str_less_than_a_day", null).ToString();
				this.RemainingDaysTextCombined = GameTexts.FindText("str_less_than_a_day", null).ToString();
				return;
			}
			this.RemainingDaysText = GameTexts.FindText("str_DAY_days_capital", null).ToString();
			this.RemainingDaysTextCombined = GameTexts.FindText("str_DAY_days", null).ToString();
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000133E4 File Offset: 0x000115E4
		private void PopulateQuestLog(JournalLog log, bool isLastStage)
		{
			string text = log.GetTimeText().ToString();
			if (log.Type != LogType.Text && log.Type != LogType.None)
			{
				int num = MathF.Max(log.Range, 0);
				int num2 = ((log.Type == LogType.TwoWayContinuous) ? log.CurrentProgress : MathF.Max(log.CurrentProgress, 0));
				TextObject textObject = new TextObject("{=Pdo7PpS3}{TASK_NAME} {CURRENT_PROGRESS}/{TARGET_PROGRESS}", null);
				textObject.SetTextVariable("TASK_NAME", log.TaskName);
				textObject.SetTextVariable("CURRENT_PROGRESS", num2);
				textObject.SetTextVariable("TARGET_PROGRESS", num);
				QuestStageTaskVM questStageTaskVM = new QuestStageTaskVM(textObject, num2, num, log.Type);
				this.Stages.Add(new QuestStageVM(log, text, isLastStage, new Action(this.UpdateIsUpdated), questStageTaskVM));
				return;
			}
			this.Stages.Add(new QuestStageVM(log, log.LogText.ToString(), text, isLastStage, new Action(this.UpdateIsUpdated)));
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000134CF File Offset: 0x000116CF
		public void UpdateIsUpdated()
		{
			this.IsUpdated = this.Stages.Any<QuestStageVM>((QuestStageVM s) => s.IsNew);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00013501 File Offset: 0x00011701
		public void ExecuteSelection()
		{
			this._onSelection(this);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0001350F File Offset: 0x0001170F
		public void ExecuteToggleQuestTrack()
		{
			if (this.Quest != null)
			{
				this.Quest.ToggleTrackedObjects();
				this.IsTracked = this.Quest.IsTrackEnabled;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00013535 File Offset: 0x00011735
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0001353D File Offset: 0x0001173D
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00013560 File Offset: 0x00011760
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00013568 File Offset: 0x00011768
		[DataSourceProperty]
		public int CompletionTypeAsInt
		{
			get
			{
				return this._completionTypeAsInt;
			}
			set
			{
				if (value != this._completionTypeAsInt)
				{
					this._completionTypeAsInt = value;
					base.OnPropertyChangedWithValue(value, "CompletionTypeAsInt");
				}
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00013586 File Offset: 0x00011786
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0001358E File Offset: 0x0001178E
		[DataSourceProperty]
		public bool IsMainQuest
		{
			get
			{
				return this._isMainQuest;
			}
			set
			{
				if (value != this._isMainQuest)
				{
					this._isMainQuest = value;
					base.OnPropertyChangedWithValue(value, "IsMainQuest");
				}
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000211 RID: 529 RVA: 0x000135AC File Offset: 0x000117AC
		// (set) Token: 0x06000212 RID: 530 RVA: 0x000135B4 File Offset: 0x000117B4
		[DataSourceProperty]
		public bool IsNavalQuest
		{
			get
			{
				return this._isNavalQuest;
			}
			set
			{
				if (value != this._isNavalQuest)
				{
					this._isNavalQuest = value;
					base.OnPropertyChangedWithValue(value, "IsNavalQuest");
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000213 RID: 531 RVA: 0x000135D2 File Offset: 0x000117D2
		// (set) Token: 0x06000214 RID: 532 RVA: 0x000135DA File Offset: 0x000117DA
		[DataSourceProperty]
		public bool IsCompletedSuccessfully
		{
			get
			{
				return this._isCompletedSuccessfully;
			}
			set
			{
				if (value != this._isCompletedSuccessfully)
				{
					this._isCompletedSuccessfully = value;
					base.OnPropertyChangedWithValue(value, "IsCompletedSuccessfully");
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000215 RID: 533 RVA: 0x000135F8 File Offset: 0x000117F8
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00013600 File Offset: 0x00011800
		[DataSourceProperty]
		public bool IsCompleted
		{
			get
			{
				return this._isCompleted;
			}
			set
			{
				if (value != this._isCompleted)
				{
					this._isCompleted = value;
					base.OnPropertyChangedWithValue(value, "IsCompleted");
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0001361E File Offset: 0x0001181E
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00013626 File Offset: 0x00011826
		[DataSourceProperty]
		public bool IsUpdated
		{
			get
			{
				return this._isUpdated;
			}
			set
			{
				if (value != this._isUpdated)
				{
					this._isUpdated = value;
					base.OnPropertyChangedWithValue(value, "IsUpdated");
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00013644 File Offset: 0x00011844
		// (set) Token: 0x0600021A RID: 538 RVA: 0x0001364C File Offset: 0x0001184C
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0001366A File Offset: 0x0001186A
		// (set) Token: 0x0600021C RID: 540 RVA: 0x00013672 File Offset: 0x00011872
		[DataSourceProperty]
		public bool IsRemainingDaysHidden
		{
			get
			{
				return this._isRemainingDaysHidden;
			}
			set
			{
				if (value != this._isRemainingDaysHidden)
				{
					this._isRemainingDaysHidden = value;
					base.OnPropertyChangedWithValue(value, "IsRemainingDaysHidden");
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00013690 File Offset: 0x00011890
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00013698 File Offset: 0x00011898
		[DataSourceProperty]
		public bool IsTracked
		{
			get
			{
				return this._isTracked;
			}
			set
			{
				if (value != this._isTracked)
				{
					this._isTracked = value;
					base.OnPropertyChangedWithValue(value, "IsTracked");
				}
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600021F RID: 543 RVA: 0x000136B6 File Offset: 0x000118B6
		// (set) Token: 0x06000220 RID: 544 RVA: 0x000136BE File Offset: 0x000118BE
		[DataSourceProperty]
		public bool IsTrackable
		{
			get
			{
				return this._isTrackable;
			}
			set
			{
				if (value != this._isTrackable)
				{
					this._isTrackable = value;
					base.OnPropertyChangedWithValue(value, "IsTrackable");
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000221 RID: 545 RVA: 0x000136DC File Offset: 0x000118DC
		// (set) Token: 0x06000222 RID: 546 RVA: 0x000136E4 File Offset: 0x000118E4
		[DataSourceProperty]
		public string RemainingDaysText
		{
			get
			{
				return this._remainingDaysText;
			}
			set
			{
				if (value != this._remainingDaysText)
				{
					this._remainingDaysText = value;
					base.OnPropertyChangedWithValue<string>(value, "RemainingDaysText");
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00013707 File Offset: 0x00011907
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0001370F File Offset: 0x0001190F
		[DataSourceProperty]
		public string RemainingDaysTextCombined
		{
			get
			{
				return this._remainingDaysTextCombined;
			}
			set
			{
				if (value != this._remainingDaysTextCombined)
				{
					this._remainingDaysTextCombined = value;
					base.OnPropertyChangedWithValue<string>(value, "RemainingDaysTextCombined");
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00013732 File Offset: 0x00011932
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0001373A File Offset: 0x0001193A
		[DataSourceProperty]
		public int RemainingDays
		{
			get
			{
				return this._remainingDays;
			}
			set
			{
				if (value != this._remainingDays)
				{
					this._remainingDays = value;
					base.OnPropertyChangedWithValue(value, "RemainingDays");
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00013758 File Offset: 0x00011958
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00013760 File Offset: 0x00011960
		[DataSourceProperty]
		public HeroVM QuestGiverHero
		{
			get
			{
				return this._questGiverHero;
			}
			set
			{
				if (value != this._questGiverHero)
				{
					this._questGiverHero = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "QuestGiverHero");
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0001377E File Offset: 0x0001197E
		// (set) Token: 0x0600022A RID: 554 RVA: 0x00013786 File Offset: 0x00011986
		[DataSourceProperty]
		public bool IsQuestGiverHeroHidden
		{
			get
			{
				return this._isQuestGiverHeroHidden;
			}
			set
			{
				if (value != this._isQuestGiverHeroHidden)
				{
					this._isQuestGiverHeroHidden = value;
					base.OnPropertyChangedWithValue(value, "IsQuestGiverHeroHidden");
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000137A4 File Offset: 0x000119A4
		// (set) Token: 0x0600022C RID: 556 RVA: 0x000137AC File Offset: 0x000119AC
		[DataSourceProperty]
		public MBBindingList<QuestStageVM> Stages
		{
			get
			{
				return this._stages;
			}
			set
			{
				if (value != this._stages)
				{
					this._stages = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestStageVM>>(value, "Stages");
				}
			}
		}

		// Token: 0x040000EB RID: 235
		private readonly Action<QuestItemVM> _onSelection;

		// Token: 0x040000EC RID: 236
		private QuestsVM.QuestCompletionType _completionType;

		// Token: 0x040000ED RID: 237
		private string _name;

		// Token: 0x040000EE RID: 238
		private string _remainingDaysText;

		// Token: 0x040000EF RID: 239
		private string _remainingDaysTextCombined;

		// Token: 0x040000F0 RID: 240
		private int _remainingDays;

		// Token: 0x040000F1 RID: 241
		private int _completionTypeAsInt;

		// Token: 0x040000F2 RID: 242
		private bool _isRemainingDaysHidden;

		// Token: 0x040000F3 RID: 243
		private bool _isUpdated;

		// Token: 0x040000F4 RID: 244
		private bool _isSelected;

		// Token: 0x040000F5 RID: 245
		private bool _isCompleted;

		// Token: 0x040000F6 RID: 246
		private bool _isCompletedSuccessfully;

		// Token: 0x040000F7 RID: 247
		private bool _isTracked;

		// Token: 0x040000F8 RID: 248
		private bool _isTrackable;

		// Token: 0x040000F9 RID: 249
		private bool _isMainQuest;

		// Token: 0x040000FA RID: 250
		private bool _isNavalQuest;

		// Token: 0x040000FB RID: 251
		private HeroVM _questGiverHero;

		// Token: 0x040000FC RID: 252
		private bool _isQuestGiverHeroHidden;

		// Token: 0x040000FD RID: 253
		private MBBindingList<QuestStageVM> _stages;
	}
}
