using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.BoardGame
{
	// Token: 0x02000064 RID: 100
	public class BoardGameInstructionsVM : ViewModel
	{
		// Token: 0x06000639 RID: 1593 RVA: 0x00016DB4 File Offset: 0x00014FB4
		public BoardGameInstructionsVM(CultureObject.BoardGameType boardGameType)
		{
			this._boardGameType = boardGameType;
			this.InstructionList = new MBBindingList<BoardGameInstructionVM>();
			for (int i = 0; i < this.GetNumberOfInstructions(this._boardGameType); i++)
			{
				this.InstructionList.Add(new BoardGameInstructionVM(this._boardGameType, i));
			}
			this._currentInstructionIndex = 0;
			if (this.InstructionList.Count > 0)
			{
				this.InstructionList[0].IsEnabled = true;
			}
			this.RefreshValues();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00016E34 File Offset: 0x00015034
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.InstructionsText = GameTexts.FindText("str_how_to_play", null).ToString();
			this.PreviousText = GameTexts.FindText("str_previous", null).ToString();
			this.NextText = GameTexts.FindText("str_next", null).ToString();
			this.InstructionList.ApplyActionOnAllItems(delegate(BoardGameInstructionVM x)
			{
				x.RefreshValues();
			});
			if (this._currentInstructionIndex >= 0 && this._currentInstructionIndex < this.InstructionList.Count)
			{
				TextObject textObject = new TextObject("{=hUSmlhNh}{CURRENT_PAGE}/{TOTAL_PAGES}", null);
				textObject.SetTextVariable("CURRENT_PAGE", (this._currentInstructionIndex + 1).ToString());
				textObject.SetTextVariable("TOTAL_PAGES", this.InstructionList.Count.ToString());
				this.CurrentPageText = textObject.ToString();
				this.IsPreviousButtonEnabled = this._currentInstructionIndex != 0;
				this.IsNextButtonEnabled = this._currentInstructionIndex < this.InstructionList.Count - 1;
			}
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00016F50 File Offset: 0x00015150
		public void ExecuteShowPrevious()
		{
			if (this._currentInstructionIndex > 0 && this._currentInstructionIndex < this.InstructionList.Count)
			{
				this.InstructionList[this._currentInstructionIndex].IsEnabled = false;
				this._currentInstructionIndex--;
				this.InstructionList[this._currentInstructionIndex].IsEnabled = true;
				this.RefreshValues();
			}
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00016FBC File Offset: 0x000151BC
		public void ExecuteShowNext()
		{
			if (this._currentInstructionIndex >= 0 && this._currentInstructionIndex < this.InstructionList.Count - 1)
			{
				this.InstructionList[this._currentInstructionIndex].IsEnabled = false;
				this._currentInstructionIndex++;
				this.InstructionList[this._currentInstructionIndex].IsEnabled = true;
				this.RefreshValues();
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00017029 File Offset: 0x00015229
		private int GetNumberOfInstructions(CultureObject.BoardGameType game)
		{
			switch (game)
			{
			case CultureObject.BoardGameType.Seega:
				return 4;
			case CultureObject.BoardGameType.Puluc:
				return 5;
			case CultureObject.BoardGameType.Konane:
				return 3;
			case CultureObject.BoardGameType.MuTorere:
				return 2;
			case CultureObject.BoardGameType.Tablut:
				return 4;
			case CultureObject.BoardGameType.BaghChal:
				return 4;
			default:
				return 0;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x00017058 File Offset: 0x00015258
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x00017060 File Offset: 0x00015260
		[DataSourceProperty]
		public bool IsPreviousButtonEnabled
		{
			get
			{
				return this._isPreviousButtonEnabled;
			}
			set
			{
				if (value != this._isPreviousButtonEnabled)
				{
					this._isPreviousButtonEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsPreviousButtonEnabled");
				}
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x0001707E File Offset: 0x0001527E
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x00017086 File Offset: 0x00015286
		[DataSourceProperty]
		public bool IsNextButtonEnabled
		{
			get
			{
				return this._isNextButtonEnabled;
			}
			set
			{
				if (value != this._isNextButtonEnabled)
				{
					this._isNextButtonEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsNextButtonEnabled");
				}
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x000170A4 File Offset: 0x000152A4
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x000170AC File Offset: 0x000152AC
		[DataSourceProperty]
		public string InstructionsText
		{
			get
			{
				return this._instructionsText;
			}
			set
			{
				if (value != this._instructionsText)
				{
					this._instructionsText = value;
					base.OnPropertyChangedWithValue<string>(value, "InstructionsText");
				}
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x000170CF File Offset: 0x000152CF
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x000170D7 File Offset: 0x000152D7
		[DataSourceProperty]
		public string PreviousText
		{
			get
			{
				return this._previousText;
			}
			set
			{
				if (value != this._previousText)
				{
					this._previousText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviousText");
				}
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x000170FA File Offset: 0x000152FA
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x00017102 File Offset: 0x00015302
		[DataSourceProperty]
		public string NextText
		{
			get
			{
				return this._nextText;
			}
			set
			{
				if (value != this._nextText)
				{
					this._nextText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextText");
				}
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00017125 File Offset: 0x00015325
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x0001712D File Offset: 0x0001532D
		[DataSourceProperty]
		public string CurrentPageText
		{
			get
			{
				return this._currentPageText;
			}
			set
			{
				if (value != this._currentPageText)
				{
					this._currentPageText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentPageText");
				}
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00017150 File Offset: 0x00015350
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x00017158 File Offset: 0x00015358
		[DataSourceProperty]
		public MBBindingList<BoardGameInstructionVM> InstructionList
		{
			get
			{
				return this._instructionList;
			}
			set
			{
				if (value != this._instructionList)
				{
					this._instructionList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BoardGameInstructionVM>>(value, "InstructionList");
				}
			}
		}

		// Token: 0x04000317 RID: 791
		private readonly CultureObject.BoardGameType _boardGameType;

		// Token: 0x04000318 RID: 792
		private int _currentInstructionIndex;

		// Token: 0x04000319 RID: 793
		private bool _isPreviousButtonEnabled;

		// Token: 0x0400031A RID: 794
		private bool _isNextButtonEnabled;

		// Token: 0x0400031B RID: 795
		private string _instructionsText;

		// Token: 0x0400031C RID: 796
		private string _previousText;

		// Token: 0x0400031D RID: 797
		private string _nextText;

		// Token: 0x0400031E RID: 798
		private string _currentPageText;

		// Token: 0x0400031F RID: 799
		private MBBindingList<BoardGameInstructionVM> _instructionList;
	}
}
