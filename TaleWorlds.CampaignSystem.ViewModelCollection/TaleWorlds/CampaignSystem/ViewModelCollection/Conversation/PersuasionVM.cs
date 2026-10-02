using System;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Conversation
{
	// Token: 0x02000120 RID: 288
	public class PersuasionVM : ViewModel
	{
		// Token: 0x06001A48 RID: 6728 RVA: 0x00063ACB File Offset: 0x00061CCB
		public PersuasionVM(ConversationManager manager)
		{
			this.PersuasionProgress = new MBBindingList<BoolItemWithActionVM>();
			this._manager = manager;
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x00063AE8 File Offset: 0x00061CE8
		public void OnPersuasionProgress(Tuple<PersuasionOptionArgs, PersuasionOptionResult> selectedOption)
		{
			this.ProgressText = "";
			string text = null;
			string text2 = null;
			switch (selectedOption.Item2)
			{
			case PersuasionOptionResult.CriticalFailure:
				text = new TextObject("{=ocSW4WA2}Critical Fail!", null).ToString();
				text2 = "<a style=\"Conversation.Persuasion.Negative\"><b>{TEXT}</b></a>";
				break;
			case PersuasionOptionResult.Failure:
			case PersuasionOptionResult.Miss:
				text = new TextObject("{=JYOcl7Ox}Ineffective!", null).ToString();
				text2 = "<a style=\"Conversation.Persuasion.Neutral\"><b>{TEXT}</b></a>";
				break;
			case PersuasionOptionResult.Success:
				text = new TextObject("{=3F0y3ugx}Success!", null).ToString();
				text2 = "<a style=\"Conversation.Persuasion.Positive\"><b>{TEXT}</b></a>";
				break;
			case PersuasionOptionResult.CriticalSuccess:
				text = new TextObject("{=4U9EnZt5}Critical Success!", null).ToString();
				text2 = "<a style=\"Conversation.Persuasion.Positive\"><b>{TEXT}</b></a>";
				break;
			}
			this.ProgressText = text2.Replace("{TEXT}", text);
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x00063B9B File Offset: 0x00061D9B
		public override void RefreshValues()
		{
			base.RefreshValues();
			PersuasionOptionVM currentPersuasionOption = this.CurrentPersuasionOption;
			if (currentPersuasionOption == null)
			{
				return;
			}
			currentPersuasionOption.RefreshValues();
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x00063BB3 File Offset: 0x00061DB3
		public void SetCurrentOption(PersuasionOptionVM option)
		{
			if (this.CurrentPersuasionOption != option)
			{
				this.CurrentPersuasionOption = option;
			}
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x00063BC8 File Offset: 0x00061DC8
		public void RefreshPersusasion()
		{
			this.CurrentCritFailChance = 0;
			this.CurrentFailChance = 0;
			this.CurrentCritSuccessChance = 0;
			this.CurrentSuccessChance = 0;
			this.IsPersuasionActive = ConversationManager.GetPersuasionIsActive();
			this.PersuasionProgress.Clear();
			this.PersuasionHint = new BasicTooltipViewModel();
			if (this.IsPersuasionActive)
			{
				int num = (int)ConversationManager.GetPersuasionProgress();
				int num2 = (int)ConversationManager.GetPersuasionGoalValue();
				for (int i = 1; i <= num2; i++)
				{
					bool flag = i <= num;
					this.PersuasionProgress.Add(new BoolItemWithActionVM(null, flag, null));
				}
				if (this.CurrentPersuasionOption != null)
				{
					this.CurrentCritFailChance = this._currentPersuasionOption.CritFailChance;
					this.CurrentFailChance = this._currentPersuasionOption.FailChance;
					this.CurrentCritSuccessChance = this._currentPersuasionOption.CritSuccessChance;
					this.CurrentSuccessChance = this._currentPersuasionOption.SuccessChance;
				}
				this.PersuasionHint = new BasicTooltipViewModel(() => this.GetPersuasionTooltip());
			}
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x00063CB5 File Offset: 0x00061EB5
		private string GetPersuasionTooltip()
		{
			if (ConversationManager.GetPersuasionIsActive())
			{
				GameTexts.SetVariable("CURRENT_PROGRESS", (int)ConversationManager.GetPersuasionProgress());
				GameTexts.SetVariable("TARGET_PROGRESS", (int)ConversationManager.GetPersuasionGoalValue());
				return GameTexts.FindText("str_persuasion_tooltip", null).ToString();
			}
			return "";
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x00063CF4 File Offset: 0x00061EF4
		private void RefreshChangeValues()
		{
			float num;
			float num2;
			float num3;
			this._manager.GetPersuasionChanceValues(out num, out num2, out num3);
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06001A4F RID: 6735 RVA: 0x00063D12 File Offset: 0x00061F12
		// (set) Token: 0x06001A50 RID: 6736 RVA: 0x00063D1A File Offset: 0x00061F1A
		[DataSourceProperty]
		public BasicTooltipViewModel PersuasionHint
		{
			get
			{
				return this._persuasionHint;
			}
			set
			{
				if (this._persuasionHint != value)
				{
					this._persuasionHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "PersuasionHint");
				}
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06001A51 RID: 6737 RVA: 0x00063D38 File Offset: 0x00061F38
		// (set) Token: 0x06001A52 RID: 6738 RVA: 0x00063D40 File Offset: 0x00061F40
		[DataSourceProperty]
		public string ProgressText
		{
			get
			{
				return this._progressText;
			}
			set
			{
				if (this._progressText != value)
				{
					this._progressText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressText");
				}
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06001A53 RID: 6739 RVA: 0x00063D63 File Offset: 0x00061F63
		// (set) Token: 0x06001A54 RID: 6740 RVA: 0x00063D6B File Offset: 0x00061F6B
		[DataSourceProperty]
		public MBBindingList<BoolItemWithActionVM> PersuasionProgress
		{
			get
			{
				return this._persuasionProgress;
			}
			set
			{
				if (value != this._persuasionProgress)
				{
					this._persuasionProgress = value;
					base.OnPropertyChangedWithValue<MBBindingList<BoolItemWithActionVM>>(value, "PersuasionProgress");
				}
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06001A55 RID: 6741 RVA: 0x00063D89 File Offset: 0x00061F89
		// (set) Token: 0x06001A56 RID: 6742 RVA: 0x00063D91 File Offset: 0x00061F91
		[DataSourceProperty]
		public bool IsPersuasionActive
		{
			get
			{
				return this._isPersuasionActive;
			}
			set
			{
				if (value != this._isPersuasionActive)
				{
					if (value)
					{
						this.RefreshChangeValues();
					}
					this._isPersuasionActive = value;
					base.OnPropertyChangedWithValue(value, "IsPersuasionActive");
				}
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06001A57 RID: 6743 RVA: 0x00063DB8 File Offset: 0x00061FB8
		// (set) Token: 0x06001A58 RID: 6744 RVA: 0x00063DC0 File Offset: 0x00061FC0
		[DataSourceProperty]
		public int CurrentSuccessChance
		{
			get
			{
				return this._currentSuccessChance;
			}
			set
			{
				if (this._currentSuccessChance != value)
				{
					this._currentSuccessChance = value;
					base.OnPropertyChangedWithValue(value, "CurrentSuccessChance");
				}
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06001A59 RID: 6745 RVA: 0x00063DDE File Offset: 0x00061FDE
		// (set) Token: 0x06001A5A RID: 6746 RVA: 0x00063DE6 File Offset: 0x00061FE6
		[DataSourceProperty]
		public PersuasionOptionVM CurrentPersuasionOption
		{
			get
			{
				return this._currentPersuasionOption;
			}
			set
			{
				if (this._currentPersuasionOption != value)
				{
					this._currentPersuasionOption = value;
					base.OnPropertyChangedWithValue<PersuasionOptionVM>(value, "CurrentPersuasionOption");
				}
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x00063E04 File Offset: 0x00062004
		// (set) Token: 0x06001A5C RID: 6748 RVA: 0x00063E0C File Offset: 0x0006200C
		[DataSourceProperty]
		public int CurrentFailChance
		{
			get
			{
				return this._currentFailChance;
			}
			set
			{
				if (this._currentFailChance != value)
				{
					this._currentFailChance = value;
					base.OnPropertyChangedWithValue(value, "CurrentFailChance");
				}
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x00063E2A File Offset: 0x0006202A
		// (set) Token: 0x06001A5E RID: 6750 RVA: 0x00063E32 File Offset: 0x00062032
		[DataSourceProperty]
		public int CurrentCritSuccessChance
		{
			get
			{
				return this._currentCritSuccessChance;
			}
			set
			{
				if (this._currentCritSuccessChance != value)
				{
					this._currentCritSuccessChance = value;
					base.OnPropertyChangedWithValue(value, "CurrentCritSuccessChance");
				}
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06001A5F RID: 6751 RVA: 0x00063E50 File Offset: 0x00062050
		// (set) Token: 0x06001A60 RID: 6752 RVA: 0x00063E58 File Offset: 0x00062058
		[DataSourceProperty]
		public int CurrentCritFailChance
		{
			get
			{
				return this._currentCritFailChance;
			}
			set
			{
				if (this._currentCritFailChance != value)
				{
					this._currentCritFailChance = value;
					base.OnPropertyChangedWithValue(value, "CurrentCritFailChance");
				}
			}
		}

		// Token: 0x04000C08 RID: 3080
		internal const string PositiveText = "<a style=\"Conversation.Persuasion.Positive\"><b>{TEXT}</b></a>";

		// Token: 0x04000C09 RID: 3081
		internal const string NegativeText = "<a style=\"Conversation.Persuasion.Negative\"><b>{TEXT}</b></a>";

		// Token: 0x04000C0A RID: 3082
		internal const string NeutralText = "<a style=\"Conversation.Persuasion.Neutral\"><b>{TEXT}</b></a>";

		// Token: 0x04000C0B RID: 3083
		private ConversationManager _manager;

		// Token: 0x04000C0C RID: 3084
		private MBBindingList<BoolItemWithActionVM> _persuasionProgress;

		// Token: 0x04000C0D RID: 3085
		private bool _isPersuasionActive;

		// Token: 0x04000C0E RID: 3086
		private int _currentCritFailChance;

		// Token: 0x04000C0F RID: 3087
		private int _currentFailChance;

		// Token: 0x04000C10 RID: 3088
		private int _currentSuccessChance;

		// Token: 0x04000C11 RID: 3089
		private int _currentCritSuccessChance;

		// Token: 0x04000C12 RID: 3090
		private string _progressText;

		// Token: 0x04000C13 RID: 3091
		private PersuasionOptionVM _currentPersuasionOption;

		// Token: 0x04000C14 RID: 3092
		private BasicTooltipViewModel _persuasionHint;
	}
}
