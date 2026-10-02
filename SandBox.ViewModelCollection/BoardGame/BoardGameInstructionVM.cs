using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.BoardGame
{
	// Token: 0x02000065 RID: 101
	public class BoardGameInstructionVM : ViewModel
	{
		// Token: 0x0600064C RID: 1612 RVA: 0x00017176 File Offset: 0x00015376
		public BoardGameInstructionVM(CultureObject.BoardGameType game, int instructionIndex)
		{
			this._game = game;
			this._instructionIndex = instructionIndex;
			this.GameType = this._game.ToString();
			this.RefreshValues();
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x000171AC File Offset: 0x000153AC
		public override void RefreshValues()
		{
			base.RefreshValues();
			GameTexts.SetVariable("newline", "\n");
			this.TitleText = GameTexts.FindText("str_board_game_title", this._game.ToString() + "_" + this._instructionIndex).ToString();
			this.DescriptionText = GameTexts.FindText("str_board_game_instruction", this._game.ToString() + "_" + this._instructionIndex).ToString();
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00017244 File Offset: 0x00015444
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x0001724C File Offset: 0x0001544C
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0001726A File Offset: 0x0001546A
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x00017272 File Offset: 0x00015472
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00017295 File Offset: 0x00015495
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x0001729D File Offset: 0x0001549D
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

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x000172C0 File Offset: 0x000154C0
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x000172C8 File Offset: 0x000154C8
		[DataSourceProperty]
		public string GameType
		{
			get
			{
				return this._gameType;
			}
			set
			{
				if (value != this._gameType)
				{
					this._gameType = value;
					base.OnPropertyChangedWithValue<string>(value, "GameType");
				}
			}
		}

		// Token: 0x04000320 RID: 800
		private readonly CultureObject.BoardGameType _game;

		// Token: 0x04000321 RID: 801
		private readonly int _instructionIndex;

		// Token: 0x04000322 RID: 802
		private bool _isEnabled;

		// Token: 0x04000323 RID: 803
		private string _titleText;

		// Token: 0x04000324 RID: 804
		private string _descriptionText;

		// Token: 0x04000325 RID: 805
		private string _gameType;
	}
}
