using System;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x0200003B RID: 59
	public class MPLobbyProfileVM : ViewModel
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000552 RID: 1362 RVA: 0x000123FC File Offset: 0x000105FC
		// (remove) Token: 0x06000553 RID: 1363 RVA: 0x00012434 File Offset: 0x00010634
		public event Action OnFindGameRequested;

		// Token: 0x06000554 RID: 1364 RVA: 0x0001246C File Offset: 0x0001066C
		public MPLobbyProfileVM(LobbyState lobbyState, Action<MPLobbyVM.LobbyPage> onChangePageRequest, Action onOpenRecentGames)
		{
			this._onChangePageRequest = onChangePageRequest;
			this._onOpenRecentGames = onOpenRecentGames;
			this.HasUnofficialModulesLoaded = NetworkMain.GameClient.HasUnofficialModulesLoaded;
			this.PlayerInfo = new MPLobbyPlayerProfileVM(lobbyState);
			this.RecentGamesSummary = new MBBindingList<MPLobbyRecentGameItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000124BC File Offset: 0x000106BC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.FindGameText = new TextObject("{=yA45PqFc}FIND GAME", null).ToString();
			this.MatchFindNotPossibleText = new TextObject("{=BrYUHFsg}CHOOSE GAME", null).ToString();
			this.ShowMoreText = new TextObject("{=aBCi76ig}Show More", null).ToString();
			this.RecentGamesTitleText = new TextObject("{=NJolh9ye}Recent Games", null).ToString();
			this.RecentGamesSummary.ApplyActionOnAllItems(delegate(MPLobbyRecentGameItemVM r)
			{
				r.RefreshValues();
			});
			this.PlayerInfo.RefreshValues();
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001255C File Offset: 0x0001075C
		public void RefreshRecentGames(MBReadOnlyList<MatchHistoryData> recentGames)
		{
			this.RecentGamesSummary.Clear();
			IOrderedEnumerable<MatchHistoryData> orderedEnumerable = recentGames.OrderByDescending<MatchHistoryData, DateTime>((MatchHistoryData m) => m.MatchDate);
			int num = Math.Min(3, orderedEnumerable.Count<MatchHistoryData>());
			for (int i = 0; i < num; i++)
			{
				MPLobbyRecentGameItemVM mplobbyRecentGameItemVM = new MPLobbyRecentGameItemVM(null);
				mplobbyRecentGameItemVM.FillFrom(orderedEnumerable.ElementAt<MatchHistoryData>(i));
				this.RecentGamesSummary.Add(mplobbyRecentGameItemVM);
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000125D3 File Offset: 0x000107D3
		public void OnMatchSelectionChanged(string selectionInfo, bool isMatchFindPossible)
		{
			this.SelectionInfoText = selectionInfo;
			this.IsMatchFindPossible = isMatchFindPossible;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x000125E3 File Offset: 0x000107E3
		public void UpdatePlayerData(PlayerData playerData, bool updateStatistics = true, bool updateRating = true)
		{
			this.PlayerInfo.UpdatePlayerData(playerData, updateStatistics, updateRating);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000125F3 File Offset: 0x000107F3
		public void OnPlayerNameUpdated(string playerName)
		{
			MPLobbyPlayerProfileVM playerInfo = this.PlayerInfo;
			if (playerInfo == null)
			{
				return;
			}
			playerInfo.OnPlayerNameUpdated(playerName);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00012606 File Offset: 0x00010806
		public void OnNotificationReceived(LobbyNotification notification)
		{
			if (notification.Type == NotificationType.BadgeEarned)
			{
				this.HasBadgeNotification = true;
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00012617 File Offset: 0x00010817
		private void ExecuteFindGame()
		{
			if (this.IsMatchFindPossible)
			{
				Action onFindGameRequested = this.OnFindGameRequested;
				if (onFindGameRequested == null)
				{
					return;
				}
				onFindGameRequested();
				return;
			}
			else
			{
				Action<MPLobbyVM.LobbyPage> onChangePageRequest = this._onChangePageRequest;
				if (onChangePageRequest == null)
				{
					return;
				}
				onChangePageRequest(MPLobbyVM.LobbyPage.Matchmaking);
				return;
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00012643 File Offset: 0x00010843
		private void ExecuteOpenMatchmaking()
		{
			Action<MPLobbyVM.LobbyPage> onChangePageRequest = this._onChangePageRequest;
			if (onChangePageRequest == null)
			{
				return;
			}
			onChangePageRequest(MPLobbyVM.LobbyPage.Matchmaking);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00012656 File Offset: 0x00010856
		private void ExecuteOpenRecentGames()
		{
			Action onOpenRecentGames = this._onOpenRecentGames;
			if (onOpenRecentGames == null)
			{
				return;
			}
			onOpenRecentGames();
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00012668 File Offset: 0x00010868
		public void OnClanInfoChanged()
		{
			this.PlayerInfo.OnClanInfoChanged();
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00012675 File Offset: 0x00010875
		private void OnEnabledChanged()
		{
			MPLobbyPlayerProfileVM playerInfo = this.PlayerInfo;
			if (((playerInfo != null) ? playerInfo.Player : null) != null)
			{
				PlatformServices.Instance.CheckPermissionWithUser(Permission.ViewUserGeneratedContent, this.PlayerInfo.Player.ProvidedID, delegate(bool hasBannerlordIDPrivilege)
				{
					this.PlayerInfo.Player.IsBannerlordIDSupported = hasBannerlordIDPrivilege;
				});
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x000126B2 File Offset: 0x000108B2
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x000126BA File Offset: 0x000108BA
		[DataSourceProperty]
		public bool IsMatchFindPossible
		{
			get
			{
				return this._isMatchFindPossible;
			}
			set
			{
				if (value != this._isMatchFindPossible)
				{
					this._isMatchFindPossible = value;
					base.OnPropertyChangedWithValue(value, "IsMatchFindPossible");
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x000126D8 File Offset: 0x000108D8
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x000126E0 File Offset: 0x000108E0
		[DataSourceProperty]
		public bool HasUnofficialModulesLoaded
		{
			get
			{
				return this._hasUnofficialModulesLoaded;
			}
			set
			{
				if (value != this._hasUnofficialModulesLoaded)
				{
					this._hasUnofficialModulesLoaded = value;
					base.OnPropertyChangedWithValue(value, "HasUnofficialModulesLoaded");
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x000126FE File Offset: 0x000108FE
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x00012706 File Offset: 0x00010906
		[DataSourceProperty]
		public string ShowMoreText
		{
			get
			{
				return this._showMoreText;
			}
			set
			{
				if (value != this._showMoreText)
				{
					this._showMoreText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShowMoreText");
				}
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00012729 File Offset: 0x00010929
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x00012731 File Offset: 0x00010931
		[DataSourceProperty]
		public string FindGameText
		{
			get
			{
				return this._findGameText;
			}
			set
			{
				if (value != this._findGameText)
				{
					this._findGameText = value;
					base.OnPropertyChangedWithValue<string>(value, "FindGameText");
				}
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00012754 File Offset: 0x00010954
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x0001275C File Offset: 0x0001095C
		[DataSourceProperty]
		public string MatchFindNotPossibleText
		{
			get
			{
				return this._matchFindNotPossibleText;
			}
			set
			{
				if (value != this._matchFindNotPossibleText)
				{
					this._matchFindNotPossibleText = value;
					base.OnPropertyChangedWithValue<string>(value, "MatchFindNotPossibleText");
				}
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0001277F File Offset: 0x0001097F
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x00012787 File Offset: 0x00010987
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
					this.OnEnabledChanged();
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x000127AB File Offset: 0x000109AB
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x000127B3 File Offset: 0x000109B3
		[DataSourceProperty]
		public string SelectionInfoText
		{
			get
			{
				return this._selectionInfoText;
			}
			set
			{
				if (value != this._selectionInfoText)
				{
					this._selectionInfoText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectionInfoText");
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x000127D6 File Offset: 0x000109D6
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x000127DE File Offset: 0x000109DE
		[DataSourceProperty]
		public string RecentGamesTitleText
		{
			get
			{
				return this._recentGamesTitleText;
			}
			set
			{
				if (value != this._recentGamesTitleText)
				{
					this._recentGamesTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "RecentGamesTitleText");
				}
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00012801 File Offset: 0x00010A01
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x00012809 File Offset: 0x00010A09
		[DataSourceProperty]
		public bool HasBadgeNotification
		{
			get
			{
				return this._hasBadgeNotification;
			}
			set
			{
				if (value != this._hasBadgeNotification)
				{
					this._hasBadgeNotification = value;
					base.OnPropertyChangedWithValue(value, "HasBadgeNotification");
				}
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00012827 File Offset: 0x00010A27
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x0001282F File Offset: 0x00010A2F
		[DataSourceProperty]
		public MBBindingList<MPLobbyRecentGameItemVM> RecentGamesSummary
		{
			get
			{
				return this._recentGamesSummary;
			}
			set
			{
				if (value != this._recentGamesSummary)
				{
					this._recentGamesSummary = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyRecentGameItemVM>>(value, "RecentGamesSummary");
				}
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001284D File Offset: 0x00010A4D
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x00012855 File Offset: 0x00010A55
		[DataSourceProperty]
		public MPLobbyPlayerProfileVM PlayerInfo
		{
			get
			{
				return this._playerInfo;
			}
			set
			{
				if (value != this._playerInfo)
				{
					this._playerInfo = value;
					base.OnPropertyChangedWithValue<MPLobbyPlayerProfileVM>(value, "PlayerInfo");
				}
			}
		}

		// Token: 0x0400028C RID: 652
		private readonly Action<MPLobbyVM.LobbyPage> _onChangePageRequest;

		// Token: 0x0400028D RID: 653
		private readonly Action _onOpenRecentGames;

		// Token: 0x0400028E RID: 654
		private bool _isEnabled;

		// Token: 0x0400028F RID: 655
		private bool _isMatchFindPossible;

		// Token: 0x04000290 RID: 656
		private bool _hasUnofficialModulesLoaded;

		// Token: 0x04000291 RID: 657
		private bool _hasBadgeNotification;

		// Token: 0x04000292 RID: 658
		private string _showMoreText;

		// Token: 0x04000293 RID: 659
		private string _findGameText;

		// Token: 0x04000294 RID: 660
		private string _matchFindNotPossibleText;

		// Token: 0x04000295 RID: 661
		private string _selectionInfoText;

		// Token: 0x04000296 RID: 662
		private string _recentGamesTitleText;

		// Token: 0x04000297 RID: 663
		private MBBindingList<MPLobbyRecentGameItemVM> _recentGamesSummary;

		// Token: 0x04000298 RID: 664
		private MPLobbyPlayerProfileVM _playerInfo;
	}
}
