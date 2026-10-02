using System;
using System.Collections.Generic;
using StoryMode.Missions;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.ViewModelCollection.Missions
{
	// Token: 0x02000003 RID: 3
	public class TrainingFieldObjectivesVM : ViewModel
	{
		// Token: 0x0600001A RID: 26 RVA: 0x000024F0 File Offset: 0x000006F0
		public TrainingFieldObjectivesVM()
		{
			this.ObjectiveItems = new MBBindingList<TrainingFieldObjectiveItemVM>();
			this._dummyObjective = TrainingFieldObjectiveItemVM.CreateDummy();
			this.RefreshValues();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002514 File Offset: 0x00000714
		public override void RefreshValues()
		{
			base.RefreshValues();
			string keyHyperlinkText = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("Generic", 4), 1f);
			GameTexts.SetVariable("LEAVE_KEY", keyHyperlinkText);
			GameTexts.SetVariable("newline", "\n");
			this.LeaveAnyTimeText = GameTexts.FindText("str_leave_training_field", null).ToString();
			this.ObjectiveItems.ApplyActionOnAllItems(delegate(TrainingFieldObjectiveItemVM o)
			{
				o.RefreshValues();
			});
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002598 File Offset: 0x00000798
		public void UpdateObjectivesWith(List<TrainingFieldMissionController.TutorialObjective> objectives)
		{
			this.ObjectiveItems.Clear();
			foreach (TrainingFieldMissionController.TutorialObjective tutorialObjective in objectives)
			{
				TrainingFieldObjectiveItemVM trainingFieldObjectiveItemVM = TrainingFieldObjectiveItemVM.CreateFromObjective(tutorialObjective);
				this.ObjectiveItems.Add(trainingFieldObjectiveItemVM);
				if (tutorialObjective.IsActive)
				{
					this.ActiveObjective = trainingFieldObjectiveItemVM;
				}
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000260C File Offset: 0x0000080C
		public void UpdateCurrentObjectiveExplanationText(TextObject currentObjectiveText)
		{
			if (this.ActiveObjective == null)
			{
				this.ActiveObjective = this._dummyObjective;
			}
			this.CurrentObjectiveExplanationText = ((currentObjectiveText != null) ? currentObjectiveText.ToString() : null) ?? "";
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000263D File Offset: 0x0000083D
		public void UpdateCurrentMouseObjective(TrainingFieldMissionController.MouseObjectives currentMouseObjective, TrainingFieldMissionController.ObjectivePerformingType currentObjectivePerformingType)
		{
			TrainingFieldObjectiveItemVM activeObjective = this.ActiveObjective;
			if (activeObjective == null)
			{
				return;
			}
			activeObjective.UpdateObjective(currentMouseObjective, currentObjectivePerformingType);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002651 File Offset: 0x00000851
		public void UpdateTimerText(string timerText)
		{
			this.TimerText = timerText;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000265A File Offset: 0x0000085A
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002662 File Offset: 0x00000862
		[DataSourceProperty]
		public string LeaveAnyTimeText
		{
			get
			{
				return this._leaveAnyTimeText;
			}
			set
			{
				if (value != this._leaveAnyTimeText)
				{
					this._leaveAnyTimeText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaveAnyTimeText");
				}
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002685 File Offset: 0x00000885
		// (set) Token: 0x06000023 RID: 35 RVA: 0x0000268D File Offset: 0x0000088D
		[DataSourceProperty]
		public string CurrentObjectiveExplanationText
		{
			get
			{
				return this._currentObjectiveExplanationText;
			}
			set
			{
				if (value != this._currentObjectiveExplanationText)
				{
					this._currentObjectiveExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentObjectiveExplanationText");
				}
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000026B0 File Offset: 0x000008B0
		// (set) Token: 0x06000025 RID: 37 RVA: 0x000026B8 File Offset: 0x000008B8
		[DataSourceProperty]
		public string TimerText
		{
			get
			{
				return this._timerText;
			}
			set
			{
				if (value != this._timerText)
				{
					this._timerText = value;
					base.OnPropertyChangedWithValue<string>(value, "TimerText");
				}
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000026DB File Offset: 0x000008DB
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000026E3 File Offset: 0x000008E3
		[DataSourceProperty]
		public TrainingFieldObjectiveItemVM ActiveObjective
		{
			get
			{
				return this._activeObjective;
			}
			set
			{
				if (value != this._activeObjective)
				{
					this._activeObjective = value;
					base.OnPropertyChangedWithValue<TrainingFieldObjectiveItemVM>(value, "ActiveObjective");
				}
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002701 File Offset: 0x00000901
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002709 File Offset: 0x00000909
		[DataSourceProperty]
		public MBBindingList<TrainingFieldObjectiveItemVM> ObjectiveItems
		{
			get
			{
				return this._objectiveItems;
			}
			set
			{
				if (value != this._objectiveItems)
				{
					this._objectiveItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<TrainingFieldObjectiveItemVM>>(value, "ObjectiveItems");
				}
			}
		}

		// Token: 0x0400000E RID: 14
		private TrainingFieldObjectiveItemVM _dummyObjective;

		// Token: 0x0400000F RID: 15
		private string _leaveAnyTimeText;

		// Token: 0x04000010 RID: 16
		private string _currentObjectiveExplanationText;

		// Token: 0x04000011 RID: 17
		private string _timerText;

		// Token: 0x04000012 RID: 18
		private TrainingFieldObjectiveItemVM _activeObjective;

		// Token: 0x04000013 RID: 19
		private MBBindingList<TrainingFieldObjectiveItemVM> _objectiveItems;
	}
}
