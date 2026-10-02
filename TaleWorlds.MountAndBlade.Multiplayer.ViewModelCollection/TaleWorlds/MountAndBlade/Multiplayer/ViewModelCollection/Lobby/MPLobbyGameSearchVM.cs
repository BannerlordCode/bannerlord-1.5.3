using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x0200002B RID: 43
	public class MPLobbyGameSearchVM : ViewModel
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000BFDD File Offset: 0x0000A1DD
		// (set) Token: 0x06000319 RID: 793 RVA: 0x0000BFE5 File Offset: 0x0000A1E5
		public MPCustomGameVM.CustomGameMode CustomGameMode { get; private set; }

		// Token: 0x0600031A RID: 794 RVA: 0x0000BFEE File Offset: 0x0000A1EE
		public MPLobbyGameSearchVM()
		{
			this.GameTypesText = new TextObject("{=cK5DE88I}N/A", null).ToString();
			this.RefreshValues();
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000C028 File Offset: 0x0000A228
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.CustomGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				this.TitleText = new TextObject("{=dkPL25g9}Waiting for an opponent team", null).ToString();
				this.GameTypesText = "";
				this.ShowStats = false;
			}
			else
			{
				this.TitleText = new TextObject("{=FD7EQDmW}Looking for game", null).ToString();
				this.ShowStats = true;
			}
			GameTexts.SetVariable("STR1", "");
			GameTexts.SetVariable("STR2", new TextObject("{=mFMPj9zg}Searching for matches", null));
			this.CurrentWaitingTimeDescription = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			GameTexts.SetVariable("STR2", new TextObject("{=18yFEEIL}Estimated wait time", null));
			this.AverageWaitingTimeDescription = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			this.CancelText = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
			this.PracticeText = new TextObject("{=cjBboOaH}Practice while waiting", null).ToString();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000C11C File Offset: 0x0000A31C
		public void OnTick(float dt)
		{
			if (this.IsEnabled)
			{
				this._waitingTimeElapsed += dt;
				this.CurrentWaitingTime = this.SecondsToString(this._waitingTimeElapsed);
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000C146 File Offset: 0x0000A346
		public void SetEnabled(bool enabled)
		{
			this.IsEnabled = enabled;
			if (enabled)
			{
				this.CanCancelSearch = true;
				this.CanEnterPracticeBattle = false;
				this._waitingTimeElapsed = 0f;
			}
			this.RefreshValues();
			if (this.CustomGameMode != MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				this.UpdateCanCancel();
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000C180 File Offset: 0x0000A380
		public void UpdateData(MatchmakingWaitTimeStats matchmakingWaitTimeStats, string[] gameTypeInfo)
		{
			this.ShowStats = true;
			this.CustomGameMode = MPCustomGameVM.CustomGameMode.CustomServer;
			this.TitleText = new TextObject("{=FD7EQDmW}Looking for game", null).ToString();
			int num = 0;
			string[] array = this.GameTypesText.Replace(" ", "").Split(new char[] { ',' });
			foreach (string text in array)
			{
				WaitTimeStatType waitTimeStatType = WaitTimeStatType.SoloDuo;
				if (NetworkMain.GameClient.PlayersInParty.Count >= 3 && NetworkMain.GameClient.PlayersInParty.Count <= 5)
				{
					waitTimeStatType = WaitTimeStatType.Party;
				}
				else if (NetworkMain.GameClient.IsPartyFull)
				{
					waitTimeStatType = WaitTimeStatType.Premade;
				}
				num += matchmakingWaitTimeStats.GetWaitTime(MultiplayerMain.GetUserCurrentRegion(), text, waitTimeStatType);
			}
			this.AverageWaitingTime = this.SecondsToString((float)(num / array.Length));
			if (gameTypeInfo != null)
			{
				this.GameTypesText = MPLobbyVM.GetLocalizedGameTypesString(gameTypeInfo);
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000C25C File Offset: 0x0000A45C
		public void UpdatePremadeGameData()
		{
			this.ShowStats = false;
			this.CustomGameMode = MPCustomGameVM.CustomGameMode.PremadeGame;
			this.TitleText = new TextObject("{=dkPL25g9}Waiting for an opponent team", null).ToString();
			this.GameTypesText = "";
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000C28D File Offset: 0x0000A48D
		public void OnJoinPremadeGameRequestSuccessful()
		{
			this.TitleText = new TextObject("{=5coyTZOI}Game is starting!", null).ToString();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000C2A5 File Offset: 0x0000A4A5
		public void OnRequestedToCancelSearchBattle()
		{
			this.CanCancelSearch = false;
			this.CanEnterPracticeBattle = false;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000C2B5 File Offset: 0x0000A4B5
		public void UpdateCanCancel()
		{
			this.CanCancelSearch = !NetworkMain.GameClient.IsInParty || NetworkMain.GameClient.IsPartyLeader;
			this.CanEnterPracticeBattle = false;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000C2DD File Offset: 0x0000A4DD
		private void ExecuteCancel()
		{
			if (this.CustomGameMode != MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				NetworkMain.GameClient.CancelFindGame();
				return;
			}
			NetworkMain.GameClient.CancelCreatingPremadeGame();
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000C300 File Offset: 0x0000A500
		private string SecondsToString(float seconds)
		{
			return TimeSpan.FromSeconds((double)seconds).ToString((seconds >= 3600f) ? this._longTimeTextFormat : this._shortTimeTextFormat);
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000C332 File Offset: 0x0000A532
		// (set) Token: 0x06000326 RID: 806 RVA: 0x0000C33A File Offset: 0x0000A53A
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

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000C358 File Offset: 0x0000A558
		// (set) Token: 0x06000328 RID: 808 RVA: 0x0000C360 File Offset: 0x0000A560
		[DataSourceProperty]
		public bool CanEnterPracticeBattle
		{
			get
			{
				return this._canEnterPracticeBattle;
			}
			set
			{
				if (value != this._canEnterPracticeBattle)
				{
					this._canEnterPracticeBattle = value;
					base.OnPropertyChangedWithValue(value, "CanEnterPracticeBattle");
				}
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000C37E File Offset: 0x0000A57E
		// (set) Token: 0x0600032A RID: 810 RVA: 0x0000C386 File Offset: 0x0000A586
		[DataSourceProperty]
		public bool CanCancelSearch
		{
			get
			{
				return this._canCancelSearch;
			}
			set
			{
				if (value != this._canCancelSearch)
				{
					this._canCancelSearch = value;
					base.OnPropertyChangedWithValue(value, "CanCancelSearch");
				}
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000C3A4 File Offset: 0x0000A5A4
		// (set) Token: 0x0600032C RID: 812 RVA: 0x0000C3AC File Offset: 0x0000A5AC
		[DataSourceProperty]
		public bool ShowStats
		{
			get
			{
				return this._showStats;
			}
			set
			{
				if (value != this._showStats)
				{
					this._showStats = value;
					base.OnPropertyChangedWithValue(value, "ShowStats");
				}
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000C3CA File Offset: 0x0000A5CA
		// (set) Token: 0x0600032E RID: 814 RVA: 0x0000C3D2 File Offset: 0x0000A5D2
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

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000C3F5 File Offset: 0x0000A5F5
		// (set) Token: 0x06000330 RID: 816 RVA: 0x0000C3FD File Offset: 0x0000A5FD
		[DataSourceProperty]
		public string GameTypesText
		{
			get
			{
				return this._gameTypesText;
			}
			set
			{
				if (value != this._gameTypesText)
				{
					this._gameTypesText = value;
					base.OnPropertyChangedWithValue<string>(value, "GameTypesText");
				}
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000C420 File Offset: 0x0000A620
		// (set) Token: 0x06000332 RID: 818 RVA: 0x0000C428 File Offset: 0x0000A628
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000C44B File Offset: 0x0000A64B
		// (set) Token: 0x06000334 RID: 820 RVA: 0x0000C453 File Offset: 0x0000A653
		[DataSourceProperty]
		public string PracticeText
		{
			get
			{
				return this._practiceText;
			}
			set
			{
				if (value != this._practiceText)
				{
					this._practiceText = value;
					base.OnPropertyChangedWithValue<string>(value, "PracticeText");
				}
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000C476 File Offset: 0x0000A676
		// (set) Token: 0x06000336 RID: 822 RVA: 0x0000C47E File Offset: 0x0000A67E
		[DataSourceProperty]
		public string AverageWaitingTime
		{
			get
			{
				return this._averageWaitingTime;
			}
			set
			{
				if (value != this._averageWaitingTime)
				{
					this._averageWaitingTime = value;
					base.OnPropertyChangedWithValue<string>(value, "AverageWaitingTime");
				}
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000C4A1 File Offset: 0x0000A6A1
		// (set) Token: 0x06000338 RID: 824 RVA: 0x0000C4A9 File Offset: 0x0000A6A9
		[DataSourceProperty]
		public string AverageWaitingTimeDescription
		{
			get
			{
				return this._averageWaitingTimeDescription;
			}
			set
			{
				if (value != this._averageWaitingTimeDescription)
				{
					this._averageWaitingTimeDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "AverageWaitingTimeDescription");
				}
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000C4CC File Offset: 0x0000A6CC
		// (set) Token: 0x0600033A RID: 826 RVA: 0x0000C4D4 File Offset: 0x0000A6D4
		[DataSourceProperty]
		public string CurrentWaitingTime
		{
			get
			{
				return this._currentWaitingTime;
			}
			set
			{
				if (value != this._currentWaitingTime)
				{
					this._currentWaitingTime = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWaitingTime");
				}
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000C4F7 File Offset: 0x0000A6F7
		// (set) Token: 0x0600033C RID: 828 RVA: 0x0000C4FF File Offset: 0x0000A6FF
		[DataSourceProperty]
		public string CurrentWaitingTimeDescription
		{
			get
			{
				return this._currentWaitingTimeDescription;
			}
			set
			{
				if (value != this._currentWaitingTimeDescription)
				{
					this._currentWaitingTimeDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWaitingTimeDescription");
				}
			}
		}

		// Token: 0x0400019B RID: 411
		private float _waitingTimeElapsed;

		// Token: 0x0400019C RID: 412
		private string _shortTimeTextFormat = "mm\\:ss";

		// Token: 0x0400019D RID: 413
		private string _longTimeTextFormat = "hh\\:mm\\:ss";

		// Token: 0x0400019E RID: 414
		private bool _isEnabled;

		// Token: 0x0400019F RID: 415
		private bool _canCancelSearch;

		// Token: 0x040001A0 RID: 416
		private bool _canEnterPracticeBattle;

		// Token: 0x040001A1 RID: 417
		private bool _showStats;

		// Token: 0x040001A2 RID: 418
		private string _titleText;

		// Token: 0x040001A3 RID: 419
		private string _gameTypesText;

		// Token: 0x040001A4 RID: 420
		private string _cancelText;

		// Token: 0x040001A5 RID: 421
		private string _practiceText;

		// Token: 0x040001A6 RID: 422
		private string _averageWaitingTime;

		// Token: 0x040001A7 RID: 423
		private string _averageWaitingTimeDescription;

		// Token: 0x040001A8 RID: 424
		private string _currentWaitingTime;

		// Token: 0x040001A9 RID: 425
		private string _currentWaitingTimeDescription;
	}
}
