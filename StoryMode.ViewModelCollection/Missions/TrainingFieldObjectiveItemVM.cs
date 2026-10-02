using System;
using StoryMode.Missions;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.ViewModelCollection.Missions
{
	// Token: 0x02000002 RID: 2
	public class TrainingFieldObjectiveItemVM : ViewModel
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		private TrainingFieldObjectiveItemVM()
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		private TrainingFieldObjectiveItemVM(TrainingFieldMissionController.TutorialObjective objective)
		{
			this._textObjectString = objective.GetNameString();
			this._hasBackground = objective.HasBackground;
			this.IsCompleted = objective.IsFinished;
			this.IsActive = objective.IsActive;
			this._score = objective.Score;
			this.ObjectiveItems = new MBBindingList<TrainingFieldObjectiveItemVM>();
			if (objective.SubTasks != null)
			{
				foreach (TrainingFieldMissionController.TutorialObjective tutorialObjective in objective.SubTasks)
				{
					this.ObjectiveItems.Add(TrainingFieldObjectiveItemVM.CreateFromObjective(tutorialObjective));
				}
			}
			this.ObjectiveKeys = new MBBindingList<TrainingObjectiveKeyVM>();
			this.RefreshValues();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002114 File Offset: 0x00000314
		public void UpdateObjective(TrainingFieldMissionController.MouseObjectives currentMouseObjective, TrainingFieldMissionController.ObjectivePerformingType currentObjectivePerformingType)
		{
			if (this._currentMouseObjective == currentMouseObjective && this._currentObjectivePerformingType == currentObjectivePerformingType && this._lastGamepadActive == Input.IsGamepadActive)
			{
				return;
			}
			this._currentMouseObjective = currentMouseObjective;
			this._currentObjectivePerformingType = currentObjectivePerformingType;
			this._lastGamepadActive = Input.IsGamepadActive;
			this.ObjectiveKeys.Clear();
			this.ResolveInput(this._currentMouseObjective, this._currentObjectivePerformingType, this._lastGamepadActive);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002180 File Offset: 0x00000380
		private void ResolveInput(TrainingFieldMissionController.MouseObjectives currentMouseObjective, TrainingFieldMissionController.ObjectivePerformingType currentObjectivePerformingType, bool isGamepadActive)
		{
			if (currentObjectivePerformingType == TrainingFieldMissionController.ObjectivePerformingType.None || currentMouseObjective == TrainingFieldMissionController.MouseObjectives.None)
			{
				this.IsBackgroundActive = false;
				return;
			}
			this.IsBackgroundActive = this._hasBackground;
			bool flag = this.IsAttackMovement(this._currentMouseObjective);
			TrainingObjectiveKeyVM.MovementTypes movementTypeOfObjective = this.GetMovementTypeOfObjective(this._currentMouseObjective);
			this.ArrowState = "Default";
			if (isGamepadActive)
			{
				this.ObjectiveKeys.Add(new TrainingObjectiveKeyVM(new TrainingObjectiveKeyVM.KeyInput(flag ? 9 : 10, true)));
				if (currentObjectivePerformingType != TrainingFieldMissionController.ObjectivePerformingType.AutoBlock)
				{
					this.ObjectiveKeys.Add(new TrainingObjectiveKeyVM(new TrainingObjectiveKeyVM.ControllerStickInput(movementTypeOfObjective, currentObjectivePerformingType != TrainingFieldMissionController.ObjectivePerformingType.ByLookDirection)));
					this.ArrowState = this.DecideArrowDirection(movementTypeOfObjective);
					return;
				}
			}
			else
			{
				TrainingObjectiveKeyVM.MouseClickTypes mouseClickTypes = (flag ? TrainingObjectiveKeyVM.MouseClickTypes.Left : TrainingObjectiveKeyVM.MouseClickTypes.Right);
				if (currentObjectivePerformingType == TrainingFieldMissionController.ObjectivePerformingType.ByLookDirection)
				{
					this.ObjectiveKeys.Add(new TrainingObjectiveKeyVM(new TrainingObjectiveKeyVM.MouseAndClickInput(movementTypeOfObjective, mouseClickTypes)));
					this.ArrowState = this.DecideArrowDirection(movementTypeOfObjective);
					return;
				}
				if (currentObjectivePerformingType == TrainingFieldMissionController.ObjectivePerformingType.ByMovement)
				{
					this.ObjectiveKeys.Add(new TrainingObjectiveKeyVM(new TrainingObjectiveKeyVM.KeyInput(this.GetKeyOfMovementType(movementTypeOfObjective), false)));
					this.ObjectiveKeys.Add(new TrainingObjectiveKeyVM(new TrainingObjectiveKeyVM.MouseAndClickInput(TrainingObjectiveKeyVM.MovementTypes.None, mouseClickTypes)));
					return;
				}
				if (currentObjectivePerformingType == TrainingFieldMissionController.ObjectivePerformingType.AutoBlock)
				{
					this.ObjectiveKeys.Add(new TrainingObjectiveKeyVM(new TrainingObjectiveKeyVM.MouseAndClickInput(TrainingObjectiveKeyVM.MovementTypes.None, mouseClickTypes)));
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000022A5 File Offset: 0x000004A5
		private TrainingObjectiveKeyVM.MovementTypes GetMovementTypeOfObjective(TrainingFieldMissionController.MouseObjectives mouseObjective)
		{
			switch (mouseObjective)
			{
			case TrainingFieldMissionController.MouseObjectives.AttackLeft:
			case TrainingFieldMissionController.MouseObjectives.DefendLeft:
				return TrainingObjectiveKeyVM.MovementTypes.MoveLeft;
			case TrainingFieldMissionController.MouseObjectives.AttackRight:
			case TrainingFieldMissionController.MouseObjectives.DefendRight:
				return TrainingObjectiveKeyVM.MovementTypes.MoveRight;
			case TrainingFieldMissionController.MouseObjectives.AttackUp:
			case TrainingFieldMissionController.MouseObjectives.DefendUp:
				return TrainingObjectiveKeyVM.MovementTypes.MoveUp;
			case TrainingFieldMissionController.MouseObjectives.AttackDown:
			case TrainingFieldMissionController.MouseObjectives.DefendDown:
				return TrainingObjectiveKeyVM.MovementTypes.MoveDown;
			default:
				return TrainingObjectiveKeyVM.MovementTypes.None;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022DA File Offset: 0x000004DA
		private int GetKeyOfMovementType(TrainingObjectiveKeyVM.MovementTypes movementType)
		{
			switch (movementType)
			{
			case TrainingObjectiveKeyVM.MovementTypes.MoveLeft:
				return 2;
			case TrainingObjectiveKeyVM.MovementTypes.MoveRight:
				return 3;
			case TrainingObjectiveKeyVM.MovementTypes.MoveUp:
				return 0;
			case TrainingObjectiveKeyVM.MovementTypes.MoveDown:
				return 1;
			default:
				return -1;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022FF File Offset: 0x000004FF
		private bool IsAttackMovement(TrainingFieldMissionController.MouseObjectives mouseObjective)
		{
			return mouseObjective - TrainingFieldMissionController.MouseObjectives.AttackLeft <= 3 || (mouseObjective - TrainingFieldMissionController.MouseObjectives.DefendLeft > 3 && false);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002314 File Offset: 0x00000514
		private string DecideArrowDirection(TrainingObjectiveKeyVM.MovementTypes movement)
		{
			switch (movement)
			{
			case TrainingObjectiveKeyVM.MovementTypes.MoveLeft:
				return "Left";
			case TrainingObjectiveKeyVM.MovementTypes.MoveRight:
				return "Right";
			case TrainingObjectiveKeyVM.MovementTypes.MoveUp:
				return "Up";
			case TrainingObjectiveKeyVM.MovementTypes.MoveDown:
				return "Down";
			default:
				return "Default";
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002350 File Offset: 0x00000550
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._textObjectString != "")
			{
				this.ObjectiveText = this._textObjectString;
				if (this._score != 0f)
				{
					TextObject textObject = GameTexts.FindText("str_tutorial_time_score", null);
					textObject.SetTextVariable("TIME_SCORE", this._score.ToString("0.0"));
					this.ObjectiveText += textObject.ToString();
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023CD File Offset: 0x000005CD
		public static TrainingFieldObjectiveItemVM CreateFromObjective(TrainingFieldMissionController.TutorialObjective objective)
		{
			return new TrainingFieldObjectiveItemVM(objective);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023D5 File Offset: 0x000005D5
		public static TrainingFieldObjectiveItemVM CreateDummy()
		{
			return new TrainingFieldObjectiveItemVM();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000023DC File Offset: 0x000005DC
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000023E4 File Offset: 0x000005E4
		[DataSourceProperty]
		public string ObjectiveText
		{
			get
			{
				return this._objectiveText;
			}
			set
			{
				if (value != this._objectiveText)
				{
					this._objectiveText = value;
					base.OnPropertyChangedWithValue<string>(value, "ObjectiveText");
				}
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002407 File Offset: 0x00000607
		// (set) Token: 0x0600000F RID: 15 RVA: 0x0000240F File Offset: 0x0000060F
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

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000242D File Offset: 0x0000062D
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002435 File Offset: 0x00000635
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002453 File Offset: 0x00000653
		// (set) Token: 0x06000013 RID: 19 RVA: 0x0000245B File Offset: 0x0000065B
		[DataSourceProperty]
		public bool IsBackgroundActive
		{
			get
			{
				return this._isBackgroundActive;
			}
			set
			{
				if (value != this._isBackgroundActive)
				{
					this._isBackgroundActive = value;
					base.OnPropertyChangedWithValue(value, "IsBackgroundActive");
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002479 File Offset: 0x00000679
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002481 File Offset: 0x00000681
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

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000249F File Offset: 0x0000069F
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000024A7 File Offset: 0x000006A7
		[DataSourceProperty]
		public MBBindingList<TrainingObjectiveKeyVM> ObjectiveKeys
		{
			get
			{
				return this._objectiveKeys;
			}
			set
			{
				if (value != this._objectiveKeys)
				{
					this._objectiveKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<TrainingObjectiveKeyVM>>(value, "ObjectiveKeys");
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000024C5 File Offset: 0x000006C5
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000024CD File Offset: 0x000006CD
		[DataSourceProperty]
		public string ArrowState
		{
			get
			{
				return this._arrowState;
			}
			set
			{
				if (value != this._arrowState)
				{
					this._arrowState = value;
					base.OnPropertyChangedWithValue<string>(value, "ArrowState");
				}
			}
		}

		// Token: 0x04000001 RID: 1
		private string _textObjectString;

		// Token: 0x04000002 RID: 2
		private TrainingFieldMissionController.MouseObjectives _currentMouseObjective;

		// Token: 0x04000003 RID: 3
		private TrainingFieldMissionController.ObjectivePerformingType _currentObjectivePerformingType;

		// Token: 0x04000004 RID: 4
		private bool _lastGamepadActive;

		// Token: 0x04000005 RID: 5
		private bool _hasBackground;

		// Token: 0x04000006 RID: 6
		private string _objectiveText;

		// Token: 0x04000007 RID: 7
		private string _arrowState;

		// Token: 0x04000008 RID: 8
		private bool _isCompleted;

		// Token: 0x04000009 RID: 9
		private bool _isActive;

		// Token: 0x0400000A RID: 10
		private bool _isBackgroundActive;

		// Token: 0x0400000B RID: 11
		private float _score;

		// Token: 0x0400000C RID: 12
		private MBBindingList<TrainingFieldObjectiveItemVM> _objectiveItems;

		// Token: 0x0400000D RID: 13
		private MBBindingList<TrainingObjectiveKeyVM> _objectiveKeys;
	}
}
