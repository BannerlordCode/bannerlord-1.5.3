using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Quests
{
	// Token: 0x02000023 RID: 35
	public class QuestMarkerVM : ViewModel
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600022D RID: 557 RVA: 0x000137CA File Offset: 0x000119CA
		// (set) Token: 0x0600022E RID: 558 RVA: 0x000137D2 File Offset: 0x000119D2
		public TextObject QuestTitle { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600022F RID: 559 RVA: 0x000137DB File Offset: 0x000119DB
		// (set) Token: 0x06000230 RID: 560 RVA: 0x000137E3 File Offset: 0x000119E3
		public TextObject QuestHintText { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000231 RID: 561 RVA: 0x000137EC File Offset: 0x000119EC
		// (set) Token: 0x06000232 RID: 562 RVA: 0x000137F4 File Offset: 0x000119F4
		public CampaignUIHelper.IssueQuestFlags IssueQuestFlag { get; private set; }

		// Token: 0x06000233 RID: 563 RVA: 0x000137FD File Offset: 0x000119FD
		public QuestMarkerVM(CampaignUIHelper.IssueQuestFlags issueQuestFlag, TextObject questTitle = null, TextObject questHintText = null)
		{
			this.RefreshWith(issueQuestFlag, questTitle, questHintText);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00013810 File Offset: 0x00011A10
		public void RefreshWith(CampaignUIHelper.IssueQuestFlags issueQuestFlag, TextObject questTitle = null, TextObject questHintText = null)
		{
			this.IssueQuestFlag = issueQuestFlag;
			this.QuestMarkerType = (int)issueQuestFlag;
			this.QuestTitle = questTitle ?? TextObject.GetEmpty();
			this.QuestHintText = questHintText;
			if (this.QuestHintText != null)
			{
				this.QuestHint = new HintViewModel(this.QuestHintText, null);
			}
			this.IsTrackMarker = issueQuestFlag == CampaignUIHelper.IssueQuestFlags.TrackedIssue || issueQuestFlag == CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest;
			this.RefreshValues();
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0001387A File Offset: 0x00011A7A
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (!TextObject.IsNullOrEmpty(this.QuestHintText))
			{
				this.QuestHint = new HintViewModel(this.QuestHintText, null);
				return;
			}
			this.QuestHint = new HintViewModel();
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000236 RID: 566 RVA: 0x000138AD File Offset: 0x00011AAD
		// (set) Token: 0x06000237 RID: 567 RVA: 0x000138B5 File Offset: 0x00011AB5
		[DataSourceProperty]
		public bool IsTrackMarker
		{
			get
			{
				return this._isTrackMarker;
			}
			set
			{
				if (value != this._isTrackMarker)
				{
					this._isTrackMarker = value;
					base.OnPropertyChangedWithValue(value, "IsTrackMarker");
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000238 RID: 568 RVA: 0x000138D3 File Offset: 0x00011AD3
		// (set) Token: 0x06000239 RID: 569 RVA: 0x000138DB File Offset: 0x00011ADB
		[DataSourceProperty]
		public int QuestMarkerType
		{
			get
			{
				return this._questMarkerType;
			}
			set
			{
				if (value != this._questMarkerType)
				{
					this._questMarkerType = value;
					base.OnPropertyChangedWithValue(value, "QuestMarkerType");
				}
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600023A RID: 570 RVA: 0x000138F9 File Offset: 0x00011AF9
		// (set) Token: 0x0600023B RID: 571 RVA: 0x00013901 File Offset: 0x00011B01
		[DataSourceProperty]
		public HintViewModel QuestHint
		{
			get
			{
				return this._questHint;
			}
			set
			{
				if (value != this._questHint)
				{
					this._questHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "QuestHint");
				}
			}
		}

		// Token: 0x04000101 RID: 257
		private bool _isTrackMarker;

		// Token: 0x04000102 RID: 258
		private int _questMarkerType;

		// Token: 0x04000103 RID: 259
		private HintViewModel _questHint;
	}
}
