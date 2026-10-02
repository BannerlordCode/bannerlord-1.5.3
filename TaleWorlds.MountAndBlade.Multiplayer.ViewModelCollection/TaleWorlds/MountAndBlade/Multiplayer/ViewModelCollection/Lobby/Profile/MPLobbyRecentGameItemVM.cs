using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x0200003E RID: 62
	public class MPLobbyRecentGameItemVM : ViewModel
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x00013854 File Offset: 0x00011A54
		// (set) Token: 0x060005E1 RID: 1505 RVA: 0x0001385C File Offset: 0x00011A5C
		public MatchHistoryData MatchInfo { get; private set; }

		// Token: 0x060005E2 RID: 1506 RVA: 0x00013865 File Offset: 0x00011A65
		public MPLobbyRecentGameItemVM(Action<MPLobbyRecentGamePlayerItemVM> onActivatePlayerActions)
		{
			this._onActivatePlayerActions = onActivatePlayerActions;
			this.PlayersA = new MBBindingList<MPLobbyRecentGamePlayerItemVM>();
			this.PlayersB = new MBBindingList<MPLobbyRecentGamePlayerItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00013890 File Offset: 0x00011A90
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.LastSeenPlayersText = new TextObject("{=NJolh9ye}Recent Games", null).ToString();
			this.Seperator = new TextObject("{=4NaOKslb}-", null).ToString();
			this.PlayersA.ApplyActionOnAllItems(delegate(MPLobbyRecentGamePlayerItemVM x)
			{
				x.RefreshValues();
			});
			this.PlayersB.ApplyActionOnAllItems(delegate(MPLobbyRecentGamePlayerItemVM x)
			{
				x.RefreshValues();
			});
			this.AbandonedHint = new HintViewModel(new TextObject("{=eQPSEUml}Abandoned", null), null);
			this.WonHint = new HintViewModel(new TextObject("{=IS4SifJG}Won", null), null);
			this.LostHint = new HintViewModel(new TextObject("{=b2aqL7T2}Lost", null), null);
			if (this.MatchInfo != null)
			{
				this.FillFrom(this.MatchInfo);
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001397C File Offset: 0x00011B7C
		public void FillFrom(MatchHistoryData match)
		{
			this.MatchInfo = match;
			this.PlayersA.Clear();
			this.PlayersB.Clear();
			this.GameMode = GameTexts.FindText("str_multiplayer_official_game_type_name", match.GameType).ToString();
			this.PlayerResultIndex = ((match.WinnerTeam == -1) ? 0 : 1);
			this.CultureA = match.Faction1;
			this.FactionNameA = MPLobbyRecentGameItemVM.GetLocalizedCultureNameFromStringID(match.Faction1);
			this.ScoreA = match.AttackerScore.ToString();
			this.CultureB = match.Faction2;
			this.FactionNameB = MPLobbyRecentGameItemVM.GetLocalizedCultureNameFromStringID(match.Faction2);
			this.ScoreB = match.DefenderScore.ToString();
			this.MatchResultIndex = ((match.DefenderScore == match.AttackerScore) ? 0 : ((match.DefenderScore > match.AttackerScore) ? 2 : 1));
			this.Date = LocalizedTextManager.GetDateFormattedByLanguage(BannerlordConfig.Language, match.MatchDate);
			foreach (PlayerInfo playerInfo in match.Players)
			{
				PlayerId playerId = PlayerId.FromString(playerInfo.PlayerId);
				if (!MultiplayerPlayerHelper.IsBlocked(playerId))
				{
					MPLobbyRecentGamePlayerItemVM mplobbyRecentGamePlayerItemVM = new MPLobbyRecentGamePlayerItemVM(playerId, match, this._onActivatePlayerActions);
					if (match.WinnerTeam != -1 && playerId == NetworkMain.GameClient.PlayerID)
					{
						this.PlayerResultIndex = ((playerInfo.TeamNo == match.WinnerTeam) ? 1 : 2);
					}
					if (playerInfo.TeamNo == 1)
					{
						this.PlayersA.Add(mplobbyRecentGamePlayerItemVM);
					}
					else
					{
						this.PlayersB.Add(mplobbyRecentGamePlayerItemVM);
					}
				}
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00013B38 File Offset: 0x00011D38
		public void OnFriendListUpdated(bool forceUpdate = false)
		{
			foreach (MPLobbyRecentGamePlayerItemVM mplobbyRecentGamePlayerItemVM in this.PlayersA)
			{
				mplobbyRecentGamePlayerItemVM.UpdateNameAndAvatar(forceUpdate);
			}
			foreach (MPLobbyRecentGamePlayerItemVM mplobbyRecentGamePlayerItemVM2 in this.PlayersB)
			{
				mplobbyRecentGamePlayerItemVM2.UpdateNameAndAvatar(forceUpdate);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00013BC0 File Offset: 0x00011DC0
		// (set) Token: 0x060005E7 RID: 1511 RVA: 0x00013BC8 File Offset: 0x00011DC8
		[DataSourceProperty]
		public string LastSeenPlayersText
		{
			get
			{
				return this._lastSeenPlayersText;
			}
			set
			{
				if (value != this._lastSeenPlayersText)
				{
					this._lastSeenPlayersText = value;
					base.OnPropertyChangedWithValue<string>(value, "LastSeenPlayersText");
				}
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x00013BEB File Offset: 0x00011DEB
		// (set) Token: 0x060005E9 RID: 1513 RVA: 0x00013BF3 File Offset: 0x00011DF3
		[DataSourceProperty]
		public MBBindingList<MPLobbyRecentGamePlayerItemVM> PlayersA
		{
			get
			{
				return this._playersA;
			}
			set
			{
				if (value != this._playersA)
				{
					this._playersA = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyRecentGamePlayerItemVM>>(value, "PlayersA");
				}
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00013C11 File Offset: 0x00011E11
		// (set) Token: 0x060005EB RID: 1515 RVA: 0x00013C19 File Offset: 0x00011E19
		[DataSourceProperty]
		public MBBindingList<MPLobbyRecentGamePlayerItemVM> PlayersB
		{
			get
			{
				return this._playersB;
			}
			set
			{
				if (value != this._playersB)
				{
					this._playersB = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyRecentGamePlayerItemVM>>(value, "PlayersB");
				}
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00013C37 File Offset: 0x00011E37
		// (set) Token: 0x060005ED RID: 1517 RVA: 0x00013C3F File Offset: 0x00011E3F
		[DataSourceProperty]
		public string CultureA
		{
			get
			{
				return this._cultureA;
			}
			set
			{
				if (value != this._cultureA)
				{
					this._cultureA = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureA");
				}
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x00013C62 File Offset: 0x00011E62
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x00013C6A File Offset: 0x00011E6A
		[DataSourceProperty]
		public string CultureB
		{
			get
			{
				return this._cultureB;
			}
			set
			{
				if (value != this._cultureB)
				{
					this._cultureB = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureB");
				}
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x00013C8D File Offset: 0x00011E8D
		// (set) Token: 0x060005F1 RID: 1521 RVA: 0x00013C95 File Offset: 0x00011E95
		[DataSourceProperty]
		public string FactionNameA
		{
			get
			{
				return this._factionNameA;
			}
			set
			{
				if (value != this._factionNameA)
				{
					this._factionNameA = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionNameA");
				}
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x00013CB8 File Offset: 0x00011EB8
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x00013CC0 File Offset: 0x00011EC0
		[DataSourceProperty]
		public string FactionNameB
		{
			get
			{
				return this._factionNameB;
			}
			set
			{
				if (value != this._factionNameB)
				{
					this._factionNameB = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionNameB");
				}
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x00013CE3 File Offset: 0x00011EE3
		// (set) Token: 0x060005F5 RID: 1525 RVA: 0x00013CEB File Offset: 0x00011EEB
		[DataSourceProperty]
		public string ScoreA
		{
			get
			{
				return this._scoreA;
			}
			set
			{
				if (value != this._scoreA)
				{
					this._scoreA = value;
					base.OnPropertyChangedWithValue<string>(value, "ScoreA");
				}
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x00013D0E File Offset: 0x00011F0E
		// (set) Token: 0x060005F7 RID: 1527 RVA: 0x00013D16 File Offset: 0x00011F16
		[DataSourceProperty]
		public string ScoreB
		{
			get
			{
				return this._scoreB;
			}
			set
			{
				if (value != this._scoreB)
				{
					this._scoreB = value;
					base.OnPropertyChangedWithValue<string>(value, "ScoreB");
				}
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x00013D39 File Offset: 0x00011F39
		// (set) Token: 0x060005F9 RID: 1529 RVA: 0x00013D41 File Offset: 0x00011F41
		[DataSourceProperty]
		public string GameMode
		{
			get
			{
				return this._gameMode;
			}
			set
			{
				if (value != this._gameMode)
				{
					this._gameMode = value;
					base.OnPropertyChangedWithValue<string>(value, "GameMode");
				}
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x00013D64 File Offset: 0x00011F64
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x00013D6C File Offset: 0x00011F6C
		[DataSourceProperty]
		public string Date
		{
			get
			{
				return this._date;
			}
			set
			{
				if (value != this._date)
				{
					this._date = value;
					base.OnPropertyChangedWithValue<string>(value, "Date");
				}
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x00013D8F File Offset: 0x00011F8F
		// (set) Token: 0x060005FD RID: 1533 RVA: 0x00013D97 File Offset: 0x00011F97
		[DataSourceProperty]
		public string Seperator
		{
			get
			{
				return this._seperator;
			}
			set
			{
				if (value != this._seperator)
				{
					this._seperator = value;
					base.OnPropertyChangedWithValue<string>(value, "Seperator");
				}
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x00013DBA File Offset: 0x00011FBA
		// (set) Token: 0x060005FF RID: 1535 RVA: 0x00013DC2 File Offset: 0x00011FC2
		[DataSourceProperty]
		public int MatchResultIndex
		{
			get
			{
				return this._matchResultIndex;
			}
			set
			{
				if (value != this._matchResultIndex)
				{
					this._matchResultIndex = value;
					base.OnPropertyChangedWithValue(value, "MatchResultIndex");
				}
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x00013DE0 File Offset: 0x00011FE0
		// (set) Token: 0x06000601 RID: 1537 RVA: 0x00013DE8 File Offset: 0x00011FE8
		[DataSourceProperty]
		public int PlayerResultIndex
		{
			get
			{
				return this._playerResultIndex;
			}
			set
			{
				if (value != this._playerResultIndex)
				{
					this._playerResultIndex = value;
					base.OnPropertyChangedWithValue(value, "PlayerResultIndex");
				}
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x00013E06 File Offset: 0x00012006
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x00013E0E File Offset: 0x0001200E
		[DataSourceProperty]
		public HintViewModel AbandonedHint
		{
			get
			{
				return this._abandonedHint;
			}
			set
			{
				if (value != this._abandonedHint)
				{
					this._abandonedHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AbandonedHint");
				}
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00013E2C File Offset: 0x0001202C
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x00013E34 File Offset: 0x00012034
		[DataSourceProperty]
		public HintViewModel WonHint
		{
			get
			{
				return this._wonHint;
			}
			set
			{
				if (value != this._wonHint)
				{
					this._wonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "WonHint");
				}
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00013E52 File Offset: 0x00012052
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x00013E5A File Offset: 0x0001205A
		[DataSourceProperty]
		public HintViewModel LostHint
		{
			get
			{
				return this._lostHint;
			}
			set
			{
				if (value != this._lostHint)
				{
					this._lostHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LostHint");
				}
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00013E78 File Offset: 0x00012078
		private static string GetLocalizedCultureNameFromStringID(string cultureID)
		{
			if (cultureID == "sturgia")
			{
				return new TextObject("{=PjO7oY16}Sturgia", null).ToString();
			}
			if (cultureID == "vlandia")
			{
				return new TextObject("{=FjwRsf1C}Vlandia", null).ToString();
			}
			if (cultureID == "battania")
			{
				return new TextObject("{=0B27RrYJ}Battania", null).ToString();
			}
			if (cultureID == "empire")
			{
				return new TextObject("{=empirefaction}Empire", null).ToString();
			}
			if (cultureID == "khuzait")
			{
				return new TextObject("{=sZLd6VHi}Khuzait", null).ToString();
			}
			if (!(cultureID == "aserai"))
			{
				Debug.FailedAssert("Unidentified culture id: " + cultureID, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Profile\\MPLobbyRecentGameItemVM.cs", "GetLocalizedCultureNameFromStringID", 384);
				return "";
			}
			return new TextObject("{=aseraifaction}Aserai", null).ToString();
		}

		// Token: 0x040002C9 RID: 713
		private readonly Action<MPLobbyRecentGamePlayerItemVM> _onActivatePlayerActions;

		// Token: 0x040002CA RID: 714
		public MBBindingList<MPLobbyRecentGamePlayerItemVM> _playersA;

		// Token: 0x040002CB RID: 715
		public MBBindingList<MPLobbyRecentGamePlayerItemVM> _playersB;

		// Token: 0x040002CC RID: 716
		private string _lastSeenPlayersText;

		// Token: 0x040002CD RID: 717
		private string _factionNameA;

		// Token: 0x040002CE RID: 718
		private string _factionNameB;

		// Token: 0x040002CF RID: 719
		private string _cultureA;

		// Token: 0x040002D0 RID: 720
		private string _cultureB;

		// Token: 0x040002D1 RID: 721
		private string _scoreA;

		// Token: 0x040002D2 RID: 722
		private string _scoreB;

		// Token: 0x040002D3 RID: 723
		private string _gameMode;

		// Token: 0x040002D4 RID: 724
		private string _date;

		// Token: 0x040002D5 RID: 725
		private string _seperator;

		// Token: 0x040002D6 RID: 726
		private int _playerResultIndex;

		// Token: 0x040002D7 RID: 727
		private int _matchResultIndex;

		// Token: 0x040002D8 RID: 728
		private HintViewModel _abandonedHint;

		// Token: 0x040002D9 RID: 729
		private HintViewModel _wonHint;

		// Token: 0x040002DA RID: 730
		private HintViewModel _lostHint;
	}
}
