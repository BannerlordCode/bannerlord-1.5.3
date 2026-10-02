using System;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x0200015D RID: 349
	public abstract class CharacterCreationStageBaseVM : ViewModel
	{
		// Token: 0x06002174 RID: 8564 RVA: 0x00077F1C File Offset: 0x0007611C
		protected CharacterCreationStageBaseVM(CharacterCreationManager characterCreationManager, Action affirmativeAction, TextObject affirmativeActionText, Action negativeAction, TextObject negativeActionText)
		{
			this.CharacterCreationManager = characterCreationManager;
			this._affirmativeAction = affirmativeAction;
			this._negativeAction = negativeAction;
			this._affirmativeActionText = affirmativeActionText;
			this._negativeActionText = negativeActionText;
			TextObject affirmativeActionText2 = this._affirmativeActionText;
			this.NextStageText = ((affirmativeActionText2 != null) ? affirmativeActionText2.ToString() : null);
			TextObject negativeActionText2 = this._negativeActionText;
			this.PreviousStageText = ((negativeActionText2 != null) ? negativeActionText2.ToString() : null);
		}

		// Token: 0x06002175 RID: 8565
		public abstract void OnNextStage();

		// Token: 0x06002176 RID: 8566
		public abstract void OnPreviousStage();

		// Token: 0x06002177 RID: 8567
		public abstract bool CanAdvanceToNextStage();

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x00077FBA File Offset: 0x000761BA
		// (set) Token: 0x06002179 RID: 8569 RVA: 0x00077FC2 File Offset: 0x000761C2
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x00077FE5 File Offset: 0x000761E5
		// (set) Token: 0x0600217B RID: 8571 RVA: 0x00077FED File Offset: 0x000761ED
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x00078010 File Offset: 0x00076210
		// (set) Token: 0x0600217D RID: 8573 RVA: 0x00078018 File Offset: 0x00076218
		[DataSourceProperty]
		public string SelectionText
		{
			get
			{
				return this._selectionText;
			}
			set
			{
				if (value != this._selectionText)
				{
					this._selectionText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectionText");
				}
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x0007803B File Offset: 0x0007623B
		// (set) Token: 0x0600217F RID: 8575 RVA: 0x00078043 File Offset: 0x00076243
		[DataSourceProperty]
		public string NextStageText
		{
			get
			{
				return this._nextStageText;
			}
			set
			{
				if (value != this._nextStageText)
				{
					this._nextStageText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextStageText");
				}
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x00078066 File Offset: 0x00076266
		// (set) Token: 0x06002181 RID: 8577 RVA: 0x0007806E File Offset: 0x0007626E
		[DataSourceProperty]
		public string PreviousStageText
		{
			get
			{
				return this._previousStageText;
			}
			set
			{
				if (value != this._previousStageText)
				{
					this._previousStageText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviousStageText");
				}
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x00078091 File Offset: 0x00076291
		// (set) Token: 0x06002183 RID: 8579 RVA: 0x00078099 File Offset: 0x00076299
		[DataSourceProperty]
		public int TotalStageCount
		{
			get
			{
				return this._totalStageCount;
			}
			set
			{
				if (value != this._totalStageCount)
				{
					this._totalStageCount = value;
					base.OnPropertyChangedWithValue(value, "TotalStageCount");
				}
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x000780B7 File Offset: 0x000762B7
		// (set) Token: 0x06002185 RID: 8581 RVA: 0x000780BF File Offset: 0x000762BF
		[DataSourceProperty]
		public int FurthestIndex
		{
			get
			{
				return this._furthestIndex;
			}
			set
			{
				if (value != this._furthestIndex)
				{
					this._furthestIndex = value;
					base.OnPropertyChangedWithValue(value, "FurthestIndex");
				}
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x000780DD File Offset: 0x000762DD
		// (set) Token: 0x06002187 RID: 8583 RVA: 0x000780E5 File Offset: 0x000762E5
		[DataSourceProperty]
		public int CurrentStageIndex
		{
			get
			{
				return this._currentStageIndex;
			}
			set
			{
				if (value != this._currentStageIndex)
				{
					this._currentStageIndex = value;
					base.OnPropertyChangedWithValue(value, "CurrentStageIndex");
				}
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x00078103 File Offset: 0x00076303
		// (set) Token: 0x06002189 RID: 8585 RVA: 0x0007810B File Offset: 0x0007630B
		[DataSourceProperty]
		public bool AnyItemSelected
		{
			get
			{
				return this._anyItemSelected;
			}
			set
			{
				if (value != this._anyItemSelected)
				{
					this._anyItemSelected = value;
					base.OnPropertyChangedWithValue(value, "AnyItemSelected");
				}
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x00078129 File Offset: 0x00076329
		// (set) Token: 0x0600218B RID: 8587 RVA: 0x00078131 File Offset: 0x00076331
		[DataSourceProperty]
		public bool CanAdvance
		{
			get
			{
				return this._canAdvance;
			}
			set
			{
				if (value != this._canAdvance)
				{
					this._canAdvance = value;
					base.OnPropertyChangedWithValue(value, "CanAdvance");
				}
			}
		}

		// Token: 0x04000F47 RID: 3911
		protected readonly CharacterCreationManager CharacterCreationManager;

		// Token: 0x04000F48 RID: 3912
		protected readonly Action _affirmativeAction;

		// Token: 0x04000F49 RID: 3913
		protected readonly Action _negativeAction;

		// Token: 0x04000F4A RID: 3914
		protected readonly TextObject _affirmativeActionText;

		// Token: 0x04000F4B RID: 3915
		protected readonly TextObject _negativeActionText;

		// Token: 0x04000F4C RID: 3916
		private string _title = "";

		// Token: 0x04000F4D RID: 3917
		private string _description = "";

		// Token: 0x04000F4E RID: 3918
		private string _selectionText = "";

		// Token: 0x04000F4F RID: 3919
		private string _nextStageText;

		// Token: 0x04000F50 RID: 3920
		private string _previousStageText;

		// Token: 0x04000F51 RID: 3921
		private int _totalStageCount = -1;

		// Token: 0x04000F52 RID: 3922
		private int _currentStageIndex = -1;

		// Token: 0x04000F53 RID: 3923
		private int _furthestIndex = -1;

		// Token: 0x04000F54 RID: 3924
		private bool _anyItemSelected;

		// Token: 0x04000F55 RID: 3925
		private bool _canAdvance;
	}
}
