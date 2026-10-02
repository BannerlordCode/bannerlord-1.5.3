using System;
using SandBox.BoardGames;
using SandBox.BoardGames.MissionLogics;
using SandBox.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.BoardGame
{
	// Token: 0x02000066 RID: 102
	public class BoardGameVM : ViewModel
	{
		// Token: 0x06000656 RID: 1622 RVA: 0x000172EC File Offset: 0x000154EC
		public BoardGameVM()
		{
			this._missionBoardGameHandler = Mission.Current.GetMissionBehavior<MissionBoardGameLogic>();
			this.BoardGameType = this._missionBoardGameHandler.CurrentBoardGame.ToString();
			this.IsGameUsingDice = this._missionBoardGameHandler.RequiresDiceRolling();
			this.DiceResult = "-";
			this.Instructions = new BoardGameInstructionsVM(this._missionBoardGameHandler.CurrentBoardGame);
			this.RefreshValues();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00017368 File Offset: 0x00015568
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RollDiceText = GameTexts.FindText("str_roll_dice", null).ToString();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.ForfeitText = GameTexts.FindText("str_forfeit", null).ToString();
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x000173BD File Offset: 0x000155BD
		public void Activate()
		{
			this.SwitchTurns();
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x000173C5 File Offset: 0x000155C5
		public void DiceRoll(int roll)
		{
			this.DiceResult = roll.ToString();
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000173D4 File Offset: 0x000155D4
		public void SwitchTurns()
		{
			this.IsPlayersTurn = this._missionBoardGameHandler.Board.PlayerTurn == PlayerTurn.PlayerOne || this._missionBoardGameHandler.Board.PlayerTurn == PlayerTurn.PlayerOneWaiting;
			this.TurnOwnerText = (this.IsPlayersTurn ? GameTexts.FindText("str_your_turn", null).ToString() : GameTexts.FindText("str_opponents_turn", null).ToString());
			this.DiceResult = "-";
			this.CanRoll = this.IsPlayersTurn && this.IsGameUsingDice;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00017461 File Offset: 0x00015661
		public void ExecuteRoll()
		{
			if (this.CanRoll)
			{
				this._missionBoardGameHandler.RollDice();
				this.CanRoll = false;
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00017480 File Offset: 0x00015680
		public void ExecuteForfeit()
		{
			if (this._missionBoardGameHandler.Board.IsReady && this._missionBoardGameHandler.IsGameInProgress)
			{
				TextObject textObject = new TextObject("{=azJulvrp}{?IS_BETTING}You are going to lose {BET_AMOUNT}{GOLD_ICON} if you forfeit.{newline}{?}{\\?}Do you really want to forfeit?", null);
				textObject.SetTextVariable("IS_BETTING", (this._missionBoardGameHandler.BetAmount > 0) ? 1 : 0);
				textObject.SetTextVariable("BET_AMOUNT", this._missionBoardGameHandler.BetAmount);
				textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
				textObject.SetTextVariable("newline", "{=!}\n");
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_forfeit", null).ToString(), textObject.ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), new Action(this._missionBoardGameHandler.ForfeitGame), null, "", 0f, null, null, null), true, false);
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00017578 File Offset: 0x00015778
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM rollDiceKey = this.RollDiceKey;
			if (rollDiceKey == null)
			{
				return;
			}
			rollDiceKey.OnFinalize();
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x00017590 File Offset: 0x00015790
		// (set) Token: 0x0600065F RID: 1631 RVA: 0x00017598 File Offset: 0x00015798
		[DataSourceProperty]
		public BoardGameInstructionsVM Instructions
		{
			get
			{
				return this._instructions;
			}
			set
			{
				if (value != this._instructions)
				{
					this._instructions = value;
					base.OnPropertyChangedWithValue<BoardGameInstructionsVM>(value, "Instructions");
				}
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x000175B6 File Offset: 0x000157B6
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x000175BE File Offset: 0x000157BE
		[DataSourceProperty]
		public bool CanRoll
		{
			get
			{
				return this._canRoll;
			}
			set
			{
				if (value != this._canRoll)
				{
					this._canRoll = value;
					base.OnPropertyChangedWithValue(value, "CanRoll");
				}
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x000175DC File Offset: 0x000157DC
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x000175E4 File Offset: 0x000157E4
		[DataSourceProperty]
		public bool IsPlayersTurn
		{
			get
			{
				return this._isPlayersTurn;
			}
			set
			{
				if (value != this._isPlayersTurn)
				{
					this._isPlayersTurn = value;
					base.OnPropertyChangedWithValue(value, "IsPlayersTurn");
				}
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x00017602 File Offset: 0x00015802
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x0001760A File Offset: 0x0001580A
		[DataSourceProperty]
		public bool IsGameUsingDice
		{
			get
			{
				return this._isGameUsingDice;
			}
			set
			{
				if (value != this._isGameUsingDice)
				{
					this._isGameUsingDice = value;
					base.OnPropertyChangedWithValue(value, "IsGameUsingDice");
				}
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x00017628 File Offset: 0x00015828
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x00017630 File Offset: 0x00015830
		[DataSourceProperty]
		public string DiceResult
		{
			get
			{
				return this._diceResult;
			}
			set
			{
				if (value != this._diceResult)
				{
					this._diceResult = value;
					base.OnPropertyChangedWithValue<string>(value, "DiceResult");
				}
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x00017653 File Offset: 0x00015853
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x0001765B File Offset: 0x0001585B
		[DataSourceProperty]
		public string RollDiceText
		{
			get
			{
				return this._rollDiceText;
			}
			set
			{
				if (value != this._rollDiceText)
				{
					this._rollDiceText = value;
					base.OnPropertyChangedWithValue<string>(value, "RollDiceText");
				}
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x0001767E File Offset: 0x0001587E
		// (set) Token: 0x0600066B RID: 1643 RVA: 0x00017686 File Offset: 0x00015886
		[DataSourceProperty]
		public string TurnOwnerText
		{
			get
			{
				return this._turnOwnerText;
			}
			set
			{
				if (value != this._turnOwnerText)
				{
					this._turnOwnerText = value;
					base.OnPropertyChangedWithValue<string>(value, "TurnOwnerText");
				}
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x000176A9 File Offset: 0x000158A9
		// (set) Token: 0x0600066D RID: 1645 RVA: 0x000176B1 File Offset: 0x000158B1
		[DataSourceProperty]
		public string BoardGameType
		{
			get
			{
				return this._boardGameType;
			}
			set
			{
				if (value != this._boardGameType)
				{
					this._boardGameType = value;
					base.OnPropertyChangedWithValue<string>(value, "BoardGameType");
				}
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x000176D4 File Offset: 0x000158D4
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x000176DC File Offset: 0x000158DC
		[DataSourceProperty]
		public string CloseText
		{
			get
			{
				return this._closeText;
			}
			set
			{
				if (value != this._closeText)
				{
					this._closeText = value;
					base.OnPropertyChangedWithValue<string>(value, "CloseText");
				}
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x000176FF File Offset: 0x000158FF
		// (set) Token: 0x06000671 RID: 1649 RVA: 0x00017707 File Offset: 0x00015907
		[DataSourceProperty]
		public string ForfeitText
		{
			get
			{
				return this._forfeitText;
			}
			set
			{
				if (value != this._forfeitText)
				{
					this._forfeitText = value;
					base.OnPropertyChangedWithValue<string>(value, "ForfeitText");
				}
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001772A File Offset: 0x0001592A
		public void SetRollDiceKey(HotKey key)
		{
			this.RollDiceKey = InputKeyItemVM.CreateFromHotKey(key, false);
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x00017739 File Offset: 0x00015939
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x00017741 File Offset: 0x00015941
		[DataSourceProperty]
		public InputKeyItemVM RollDiceKey
		{
			get
			{
				return this._rollDiceKey;
			}
			set
			{
				if (value != this._rollDiceKey)
				{
					this._rollDiceKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RollDiceKey");
				}
			}
		}

		// Token: 0x04000326 RID: 806
		private readonly MissionBoardGameLogic _missionBoardGameHandler;

		// Token: 0x04000327 RID: 807
		private BoardGameInstructionsVM _instructions;

		// Token: 0x04000328 RID: 808
		private string _turnOwnerText;

		// Token: 0x04000329 RID: 809
		private string _boardGameType;

		// Token: 0x0400032A RID: 810
		private bool _isGameUsingDice;

		// Token: 0x0400032B RID: 811
		private bool _isPlayersTurn;

		// Token: 0x0400032C RID: 812
		private bool _canRoll;

		// Token: 0x0400032D RID: 813
		private string _diceResult;

		// Token: 0x0400032E RID: 814
		private string _rollDiceText;

		// Token: 0x0400032F RID: 815
		private string _closeText;

		// Token: 0x04000330 RID: 816
		private string _forfeitText;

		// Token: 0x04000331 RID: 817
		private InputKeyItemVM _rollDiceKey;
	}
}
