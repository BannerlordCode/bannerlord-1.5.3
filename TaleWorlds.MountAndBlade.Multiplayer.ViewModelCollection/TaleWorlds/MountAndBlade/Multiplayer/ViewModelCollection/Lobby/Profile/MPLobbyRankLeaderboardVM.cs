using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x0200003C RID: 60
	public class MPLobbyRankLeaderboardVM : ViewModel
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x00012888 File Offset: 0x00010A88
		public MPLobbyRankLeaderboardVM(LobbyState lobbyState)
		{
			this._lobbyState = lobbyState;
			this.LeaderboardPlayers = new MBBindingList<MPLobbyLeaderboardPlayerItemVM>();
			this.PlayerActions = new MBBindingList<StringPairItemWithActionVM>();
			this.FirstPageHint = new HintViewModel(GameTexts.FindText("str_first_page", null), null);
			this.LastPageHint = new HintViewModel(GameTexts.FindText("str_last_page", null), null);
			this.PreviousPageHint = new HintViewModel(GameTexts.FindText("str_previous", null), null);
			this.NextPageHint = new HintViewModel(GameTexts.FindText("str_next", null), null);
			this.RefreshValues();
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001292B File Offset: 0x00010B2B
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CloseText = new TextObject("{=yQstzabbe}Close", null).ToString();
			this.NoDataAvailableText = this._noDataAvailableTextObject.ToString();
			this.RefreshTitleText();
			this.RefreshCurrentPageText();
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00012966 File Offset: 0x00010B66
		private void RefreshCurrentPageText()
		{
			this.CurrentPageText = GameTexts.FindText("str_LEFT_over_RIGHT", null).SetTextVariable("LEFT", this.CurrentPageIndex + 1).SetTextVariable("RIGHT", this.TotalPageCount)
				.ToString();
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x000129A0 File Offset: 0x00010BA0
		private void RefreshButtonsDisabled()
		{
			this.IsPreviousPageAvailable = !this.IsDataLoading && this.CurrentPageIndex > 0;
			this.IsNextPageAvailable = !this.IsDataLoading && this.CurrentPageIndex < this.TotalPageCount - 1;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x000129E0 File Offset: 0x00010BE0
		private async void LoadDataForPage(int pageIndex)
		{
			this.IsDataLoading = true;
			this.LeaderboardPlayers.Clear();
			int startIndex = 100 * pageIndex;
			PlayerLeaderboardData[] array = await NetworkMain.GameClient.GetRankedLeaderboard(this._currentGameType, startIndex, 100);
			PlayerLeaderboardData[] leaderboardPlayerInfos = array;
			await this._lobbyState.UpdateHasUserGeneratedContentPrivilege(true);
			if (leaderboardPlayerInfos != null && leaderboardPlayerInfos.Length != 0)
			{
				for (int i = 0; i < leaderboardPlayerInfos.Length; i++)
				{
					MPLobbyLeaderboardPlayerItemVM mplobbyLeaderboardPlayerItemVM = new MPLobbyLeaderboardPlayerItemVM(i + 1 + startIndex, leaderboardPlayerInfos[i], new Action<MPLobbyLeaderboardPlayerItemVM>(this.ActivatePlayerActions));
					this.LeaderboardPlayers.Add(mplobbyLeaderboardPlayerItemVM);
				}
			}
			this.IsDataLoading = false;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00012A24 File Offset: 0x00010C24
		public async void OpenWith(string gameType)
		{
			this._currentGameType = gameType;
			this.RefreshTitleText();
			this.CurrentPageIndex = 0;
			this.HasData = false;
			this.IsEnabled = true;
			this.IsDataLoading = true;
			this.LeaderboardPlayers.Clear();
			int num = await NetworkMain.GameClient.GetRankedLeaderboardCount(gameType);
			this.TotalPageCount = (num + 100 - 1) / 100;
			this.HasData = num > 0;
			if (this.HasData)
			{
				this.LoadDataForPage(0);
			}
			else
			{
				this.IsDataLoading = false;
			}
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00012A65 File Offset: 0x00010C65
		public void ExecuteLoadFirstPage()
		{
			if (this.IsPreviousPageAvailable)
			{
				this.CurrentPageIndex = 0;
				this.LoadDataForPage(this.CurrentPageIndex);
			}
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00012A84 File Offset: 0x00010C84
		public void ExecuteLoadPreviousPage()
		{
			if (this.IsPreviousPageAvailable)
			{
				int currentPageIndex = this.CurrentPageIndex;
				this.CurrentPageIndex = currentPageIndex - 1;
				this.LoadDataForPage(this.CurrentPageIndex);
			}
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00012AB8 File Offset: 0x00010CB8
		public void ExecuteLoadNextPage()
		{
			if (this.IsNextPageAvailable)
			{
				int currentPageIndex = this.CurrentPageIndex;
				this.CurrentPageIndex = currentPageIndex + 1;
				this.LoadDataForPage(this.CurrentPageIndex);
			}
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00012AE9 File Offset: 0x00010CE9
		public void ExecuteLoadLastPage()
		{
			if (this.IsNextPageAvailable)
			{
				this.CurrentPageIndex = this.TotalPageCount - 1;
				this.LoadDataForPage(this.CurrentPageIndex);
			}
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00012B0D File Offset: 0x00010D0D
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00012B18 File Offset: 0x00010D18
		private void RefreshTitleText()
		{
			if (string.IsNullOrEmpty(this._currentGameType))
			{
				this.TitleText = new TextObject("{=vGF5S2hE}Leaderboard", null).ToString();
				return;
			}
			this.TitleText = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).SetTextVariable("LEFT", new TextObject("{=vGF5S2hE}Leaderboard", null).ToString()).SetTextVariable("RIGHT", GameTexts.FindText("str_multiplayer_official_game_type_name", this._currentGameType).ToString())
				.ToString();
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00012B98 File Offset: 0x00010D98
		public void ActivatePlayerActions(MPLobbyLeaderboardPlayerItemVM playerVM)
		{
			this.PlayerActions.Clear();
			if (playerVM.ProvidedID != NetworkMain.GameClient.PlayerID)
			{
				bool flag = false;
				FriendInfo[] friendInfos = NetworkMain.GameClient.FriendInfos;
				for (int i = 0; i < friendInfos.Length; i++)
				{
					if (friendInfos[i].Id == playerVM.ProvidedID)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteRequestFriendship), new TextObject("{=UwkpJq9N}Add As Friend", null).ToString(), "RequestFriendship", playerVM));
				}
				else
				{
					this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteTerminateFriendship), new TextObject("{=2YIVRuRa}Remove From Friends", null).ToString(), "TerminateFriendship", playerVM));
				}
				MultiplayerPlayerContextMenuHelper.AddLobbyViewProfileOptions(playerVM, this.PlayerActions);
				StringPairItemWithActionVM stringPairItemWithActionVM = new StringPairItemWithActionVM(new Action<object>(this.ExecuteReport), GameTexts.FindText("str_mp_scoreboard_context_report", null).ToString(), "Report", playerVM);
				if (MultiplayerReportPlayerManager.IsPlayerReportedOverLimit(playerVM.ProvidedID))
				{
					stringPairItemWithActionVM.IsEnabled = false;
					stringPairItemWithActionVM.Hint.HintText = new TextObject("{=klkYFik9}You've already reported this player.", null);
				}
				this.PlayerActions.Add(stringPairItemWithActionVM);
			}
			this.IsPlayerActionsActive = false;
			this.IsPlayerActionsActive = this.PlayerActions.Count > 0;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00012CEC File Offset: 0x00010EEC
		private void ExecuteRequestFriendship(object playerObj)
		{
			PlayerId providedID = (playerObj as MPLobbyLeaderboardPlayerItemVM).ProvidedID;
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(providedID);
			NetworkMain.GameClient.AddFriend(providedID, flag);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00012D2A File Offset: 0x00010F2A
		private void ExecuteTerminateFriendship(object playerObj)
		{
			NetworkMain.GameClient.RemoveFriend((playerObj as MPLobbyLeaderboardPlayerItemVM).ProvidedID);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00012D44 File Offset: 0x00010F44
		private void ExecuteReport(object playerObj)
		{
			MultiplayerReportPlayerManager.RequestReportPlayer(Guid.Empty.ToString(), (playerObj as MPLobbyLeaderboardPlayerItemVM).ProvidedID, (playerObj as MPLobbyLeaderboardPlayerItemVM).Name, false);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00012D80 File Offset: 0x00010F80
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM previousInputKey = this.PreviousInputKey;
			if (previousInputKey != null)
			{
				previousInputKey.OnFinalize();
			}
			InputKeyItemVM nextInputKey = this.NextInputKey;
			if (nextInputKey != null)
			{
				nextInputKey.OnFinalize();
			}
			InputKeyItemVM firstInputKey = this.FirstInputKey;
			if (firstInputKey != null)
			{
				firstInputKey.OnFinalize();
			}
			InputKeyItemVM lastInputKey = this.LastInputKey;
			if (lastInputKey == null)
			{
				return;
			}
			lastInputKey.OnFinalize();
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00012DE7 File Offset: 0x00010FE7
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00012DF6 File Offset: 0x00010FF6
		public void SetPreviousInputKey(HotKey hotKey)
		{
			this.PreviousInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00012E05 File Offset: 0x00011005
		public void SetNextInputKey(HotKey hotKey)
		{
			this.NextInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00012E14 File Offset: 0x00011014
		public void SetFirstInputKey(HotKey hotKey)
		{
			this.FirstInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00012E23 File Offset: 0x00011023
		public void SetLastInputKey(HotKey hotKey)
		{
			this.LastInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x00012E32 File Offset: 0x00011032
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x00012E3A File Offset: 0x0001103A
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChanged("CancelInputKey");
				}
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x00012E57 File Offset: 0x00011057
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x00012E5F File Offset: 0x0001105F
		[DataSourceProperty]
		public InputKeyItemVM PreviousInputKey
		{
			get
			{
				return this._previousInputKey;
			}
			set
			{
				if (value != this._previousInputKey)
				{
					this._previousInputKey = value;
					base.OnPropertyChanged("PreviousInputKey");
				}
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x00012E7C File Offset: 0x0001107C
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x00012E84 File Offset: 0x00011084
		[DataSourceProperty]
		public InputKeyItemVM NextInputKey
		{
			get
			{
				return this._nextInputKey;
			}
			set
			{
				if (value != this._nextInputKey)
				{
					this._nextInputKey = value;
					base.OnPropertyChanged("NextInputKey");
				}
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00012EA1 File Offset: 0x000110A1
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x00012EA9 File Offset: 0x000110A9
		[DataSourceProperty]
		public InputKeyItemVM FirstInputKey
		{
			get
			{
				return this._firstInputKey;
			}
			set
			{
				if (value != this._firstInputKey)
				{
					this._firstInputKey = value;
					base.OnPropertyChanged("FirstInputKey");
				}
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00012EC6 File Offset: 0x000110C6
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x00012ECE File Offset: 0x000110CE
		[DataSourceProperty]
		public InputKeyItemVM LastInputKey
		{
			get
			{
				return this._lastInputKey;
			}
			set
			{
				if (value != this._lastInputKey)
				{
					this._lastInputKey = value;
					base.OnPropertyChanged("LastInputKey");
				}
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00012EEB File Offset: 0x000110EB
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x00012EF3 File Offset: 0x000110F3
		[DataSourceProperty]
		public int CurrentPageIndex
		{
			get
			{
				return this._currentPageIndex;
			}
			set
			{
				if (value != this._currentPageIndex)
				{
					this._currentPageIndex = value;
					base.OnPropertyChangedWithValue(value, "CurrentPageIndex");
					this.RefreshCurrentPageText();
					this.RefreshButtonsDisabled();
				}
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00012F1D File Offset: 0x0001111D
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x00012F25 File Offset: 0x00011125
		[DataSourceProperty]
		public int TotalPageCount
		{
			get
			{
				return this._totalPageCount;
			}
			set
			{
				if (value != this._totalPageCount)
				{
					this._totalPageCount = value;
					base.OnPropertyChangedWithValue(value, "TotalPageCount");
					this.RefreshCurrentPageText();
					this.RefreshButtonsDisabled();
				}
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00012F4F File Offset: 0x0001114F
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x00012F57 File Offset: 0x00011157
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

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00012F75 File Offset: 0x00011175
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x00012F7D File Offset: 0x0001117D
		[DataSourceProperty]
		public bool IsDataLoading
		{
			get
			{
				return this._isDataLoading;
			}
			set
			{
				if (value != this._isDataLoading)
				{
					this._isDataLoading = value;
					base.OnPropertyChangedWithValue(value, "IsDataLoading");
					this.RefreshButtonsDisabled();
				}
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00012FA1 File Offset: 0x000111A1
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x00012FA9 File Offset: 0x000111A9
		[DataSourceProperty]
		public bool HasData
		{
			get
			{
				return this._hasData;
			}
			set
			{
				if (value != this._hasData)
				{
					this._hasData = value;
					base.OnPropertyChangedWithValue(value, "HasData");
				}
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00012FC7 File Offset: 0x000111C7
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x00012FCF File Offset: 0x000111CF
		[DataSourceProperty]
		public bool IsPlayerActionsActive
		{
			get
			{
				return this._isPlayerActionsActive;
			}
			set
			{
				if (value != this._isPlayerActionsActive)
				{
					this._isPlayerActionsActive = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerActionsActive");
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00012FED File Offset: 0x000111ED
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x00012FF5 File Offset: 0x000111F5
		[DataSourceProperty]
		public bool IsPreviousPageAvailable
		{
			get
			{
				return this._isPreviousPageAvailable;
			}
			set
			{
				if (value != this._isPreviousPageAvailable)
				{
					this._isPreviousPageAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsPreviousPageAvailable");
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00013013 File Offset: 0x00011213
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x0001301B File Offset: 0x0001121B
		[DataSourceProperty]
		public bool IsNextPageAvailable
		{
			get
			{
				return this._isNextPageAvailable;
			}
			set
			{
				if (value != this._isNextPageAvailable)
				{
					this._isNextPageAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsNextPageAvailable");
				}
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00013039 File Offset: 0x00011239
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x00013041 File Offset: 0x00011241
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

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00013064 File Offset: 0x00011264
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x0001306C File Offset: 0x0001126C
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

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x0001308F File Offset: 0x0001128F
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00013097 File Offset: 0x00011297
		[DataSourceProperty]
		public string NoDataAvailableText
		{
			get
			{
				return this._noDataAvailableText;
			}
			set
			{
				if (value != this._noDataAvailableText)
				{
					this._noDataAvailableText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoDataAvailableText");
				}
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x000130BA File Offset: 0x000112BA
		// (set) Token: 0x060005AE RID: 1454 RVA: 0x000130C2 File Offset: 0x000112C2
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

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x000130E5 File Offset: 0x000112E5
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x000130ED File Offset: 0x000112ED
		[DataSourceProperty]
		public MBBindingList<MPLobbyLeaderboardPlayerItemVM> LeaderboardPlayers
		{
			get
			{
				return this._leaderboardPlayers;
			}
			set
			{
				if (value != this._leaderboardPlayers)
				{
					this._leaderboardPlayers = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyLeaderboardPlayerItemVM>>(value, "LeaderboardPlayers");
				}
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x0001310B File Offset: 0x0001130B
		// (set) Token: 0x060005B2 RID: 1458 RVA: 0x00013113 File Offset: 0x00011313
		[DataSourceProperty]
		public MBBindingList<StringPairItemWithActionVM> PlayerActions
		{
			get
			{
				return this._playerActions;
			}
			set
			{
				if (value != this._playerActions)
				{
					this._playerActions = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemWithActionVM>>(value, "PlayerActions");
				}
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00013131 File Offset: 0x00011331
		// (set) Token: 0x060005B4 RID: 1460 RVA: 0x00013139 File Offset: 0x00011339
		[DataSourceProperty]
		public HintViewModel PreviousPageHint
		{
			get
			{
				return this._previousPageHint;
			}
			set
			{
				if (value != this._previousPageHint)
				{
					this._previousPageHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PreviousPageHint");
				}
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00013157 File Offset: 0x00011357
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x0001315F File Offset: 0x0001135F
		[DataSourceProperty]
		public HintViewModel NextPageHint
		{
			get
			{
				return this._nextPageHint;
			}
			set
			{
				if (value != this._nextPageHint)
				{
					this._nextPageHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "NextPageHint");
				}
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x0001317D File Offset: 0x0001137D
		// (set) Token: 0x060005B8 RID: 1464 RVA: 0x00013185 File Offset: 0x00011385
		[DataSourceProperty]
		public HintViewModel FirstPageHint
		{
			get
			{
				return this._firstPageHint;
			}
			set
			{
				if (value != this._firstPageHint)
				{
					this._firstPageHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FirstPageHint");
				}
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x000131A3 File Offset: 0x000113A3
		// (set) Token: 0x060005BA RID: 1466 RVA: 0x000131AB File Offset: 0x000113AB
		[DataSourceProperty]
		public HintViewModel LastPageHint
		{
			get
			{
				return this._lastPageHint;
			}
			set
			{
				if (value != this._lastPageHint)
				{
					this._lastPageHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LastPageHint");
				}
			}
		}

		// Token: 0x04000299 RID: 665
		private const int PlayerItemsPerPage = 100;

		// Token: 0x0400029A RID: 666
		private string _currentGameType;

		// Token: 0x0400029B RID: 667
		private readonly LobbyState _lobbyState;

		// Token: 0x0400029C RID: 668
		private readonly TextObject _noDataAvailableTextObject = new TextObject("{=vw6Va7ho}There are currently no players in the leaderboard.", null);

		// Token: 0x0400029D RID: 669
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400029E RID: 670
		private InputKeyItemVM _previousInputKey;

		// Token: 0x0400029F RID: 671
		private InputKeyItemVM _nextInputKey;

		// Token: 0x040002A0 RID: 672
		private InputKeyItemVM _firstInputKey;

		// Token: 0x040002A1 RID: 673
		private InputKeyItemVM _lastInputKey;

		// Token: 0x040002A2 RID: 674
		private int _currentPageIndex;

		// Token: 0x040002A3 RID: 675
		private int _totalPageCount;

		// Token: 0x040002A4 RID: 676
		private bool _isEnabled;

		// Token: 0x040002A5 RID: 677
		private bool _isDataLoading;

		// Token: 0x040002A6 RID: 678
		private bool _hasData;

		// Token: 0x040002A7 RID: 679
		private bool _isPlayerActionsActive;

		// Token: 0x040002A8 RID: 680
		private bool _isPreviousPageAvailable;

		// Token: 0x040002A9 RID: 681
		private bool _isNextPageAvailable;

		// Token: 0x040002AA RID: 682
		private string _titleText;

		// Token: 0x040002AB RID: 683
		private string _closeText;

		// Token: 0x040002AC RID: 684
		private string _noDataAvailableText;

		// Token: 0x040002AD RID: 685
		private string _currentPageText;

		// Token: 0x040002AE RID: 686
		private MBBindingList<MPLobbyLeaderboardPlayerItemVM> _leaderboardPlayers;

		// Token: 0x040002AF RID: 687
		private MBBindingList<StringPairItemWithActionVM> _playerActions;

		// Token: 0x040002B0 RID: 688
		private HintViewModel _firstPageHint;

		// Token: 0x040002B1 RID: 689
		private HintViewModel _lastPageHint;

		// Token: 0x040002B2 RID: 690
		private HintViewModel _previousPageHint;

		// Token: 0x040002B3 RID: 691
		private HintViewModel _nextPageHint;
	}
}
