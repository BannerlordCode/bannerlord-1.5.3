using System;
using TaleWorlds.Library;
using TaleWorlds.Library.NewsManager;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home
{
	// Token: 0x02000050 RID: 80
	public class MPLobbyHomeVM : ViewModel
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060006F3 RID: 1779 RVA: 0x000163C0 File Offset: 0x000145C0
		// (remove) Token: 0x060006F4 RID: 1780 RVA: 0x000163F8 File Offset: 0x000145F8
		public event Action OnFindGameRequested;

		// Token: 0x060006F5 RID: 1781 RVA: 0x00016430 File Offset: 0x00014630
		public MPLobbyHomeVM(NewsManager newsManager, Action<MPLobbyVM.LobbyPage> onChangePageRequest)
		{
			this._onChangePageRequest = onChangePageRequest;
			this.HasUnofficialModulesLoaded = NetworkMain.GameClient.HasUnofficialModulesLoaded;
			this.Player = new MPLobbyPlayerBaseVM(NetworkMain.GameClient.PlayerID, "", null, null);
			this.News = new MPNewsVM(newsManager);
			this.IsNewsAvailable = true;
			this.Announcements = new MPAnnouncementsVM(this.IsNewsAvailable ? new float?(30f) : null);
			this.RefreshValues();
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x000164B8 File Offset: 0x000146B8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.FindGameText = new TextObject("{=yA45PqFc}FIND GAME", null).ToString();
			this.MatchFindNotPossibleText = new TextObject("{=BrYUHFsg}CHOOSE GAME", null).ToString();
			this.OpenProfileText = new TextObject("{=aBCi76ig}Show More", null).ToString();
			this.Player.RefreshValues();
			this.News.RefreshValues();
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00016523 File Offset: 0x00014723
		public void OnTick(float dt)
		{
			this.Announcements.OnTick(dt);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00016531 File Offset: 0x00014731
		public void RefreshPlayerData(PlayerData playerData, bool updateRating = true)
		{
			this.Player.UpdateWith(playerData);
			if (updateRating)
			{
				this.Player.UpdateRating(new Action(this.OnRatingReceived));
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00016559 File Offset: 0x00014759
		private void OnRatingReceived()
		{
			this.Player.RefreshSelectableGameTypes(true, new Action<string>(this.Player.UpdateDisplayedRankInfo), "");
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001657D File Offset: 0x0001477D
		public void OnMatchSelectionChanged(string selectionInfo, bool isMatchFindPossible)
		{
			this.SelectionInfoText = selectionInfo;
			this.IsMatchFindPossible = isMatchFindPossible;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001658D File Offset: 0x0001478D
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

		// Token: 0x060006FC RID: 1788 RVA: 0x000165B9 File Offset: 0x000147B9
		private void ExecuteOpenMatchmaking()
		{
			Action<MPLobbyVM.LobbyPage> onChangePageRequest = this._onChangePageRequest;
			if (onChangePageRequest == null)
			{
				return;
			}
			onChangePageRequest(MPLobbyVM.LobbyPage.Matchmaking);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000165CC File Offset: 0x000147CC
		private void ExecuteOpenProfile()
		{
			Action<MPLobbyVM.LobbyPage> onChangePageRequest = this._onChangePageRequest;
			if (onChangePageRequest == null)
			{
				return;
			}
			onChangePageRequest(MPLobbyVM.LobbyPage.Profile);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x000165DF File Offset: 0x000147DF
		public void OnClanInfoChanged()
		{
			this.Player.UpdateClanInfo();
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x000165EC File Offset: 0x000147EC
		public void OnPlayerNameUpdated(string playerName)
		{
			this.Player.UpdateNameAndAvatar(true);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000165FA File Offset: 0x000147FA
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.News.OnFinalize();
			this.News = null;
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x00016614 File Offset: 0x00014814
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x0001661C File Offset: 0x0001481C
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

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001663A File Offset: 0x0001483A
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x00016642 File Offset: 0x00014842
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

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x00016660 File Offset: 0x00014860
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x00016668 File Offset: 0x00014868
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

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x00016686 File Offset: 0x00014886
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0001668E File Offset: 0x0001488E
		[DataSourceProperty]
		public bool IsNewsAvailable
		{
			get
			{
				return this._isNewsAvailable;
			}
			set
			{
				if (value != this._isNewsAvailable)
				{
					this._isNewsAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsNewsAvailable");
				}
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x000166AC File Offset: 0x000148AC
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x000166B4 File Offset: 0x000148B4
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

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x000166D7 File Offset: 0x000148D7
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x000166DF File Offset: 0x000148DF
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

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x00016702 File Offset: 0x00014902
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0001670A File Offset: 0x0001490A
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

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001672D File Offset: 0x0001492D
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x00016735 File Offset: 0x00014935
		[DataSourceProperty]
		public string OpenProfileText
		{
			get
			{
				return this._openProfileText;
			}
			set
			{
				if (value != this._openProfileText)
				{
					this._openProfileText = value;
					base.OnPropertyChangedWithValue<string>(value, "OpenProfileText");
				}
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00016758 File Offset: 0x00014958
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x00016760 File Offset: 0x00014960
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM Player
		{
			get
			{
				return this._player;
			}
			set
			{
				if (value != this._player)
				{
					this._player = value;
					base.OnPropertyChangedWithValue<MPLobbyPlayerBaseVM>(value, "Player");
				}
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001677E File Offset: 0x0001497E
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x00016786 File Offset: 0x00014986
		[DataSourceProperty]
		public MPNewsVM News
		{
			get
			{
				return this._news;
			}
			set
			{
				if (value != this._news)
				{
					this._news = value;
					base.OnPropertyChangedWithValue<MPNewsVM>(value, "News");
				}
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x000167A4 File Offset: 0x000149A4
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x000167AC File Offset: 0x000149AC
		[DataSourceProperty]
		public MPAnnouncementsVM Announcements
		{
			get
			{
				return this._announcements;
			}
			set
			{
				if (value != this._announcements)
				{
					this._announcements = value;
					base.OnPropertyChangedWithValue<MPAnnouncementsVM>(value, "Announcements");
				}
			}
		}

		// Token: 0x04000340 RID: 832
		private const float _announcementUpdateIntervalInSeconds = 30f;

		// Token: 0x04000341 RID: 833
		private readonly Action<MPLobbyVM.LobbyPage> _onChangePageRequest;

		// Token: 0x04000343 RID: 835
		private bool _isEnabled;

		// Token: 0x04000344 RID: 836
		private bool _isMatchFindPossible;

		// Token: 0x04000345 RID: 837
		private bool _hasUnofficialModulesLoaded;

		// Token: 0x04000346 RID: 838
		private bool _isNewsAvailable;

		// Token: 0x04000347 RID: 839
		private string _findGameText;

		// Token: 0x04000348 RID: 840
		private string _matchFindNotPossibleText;

		// Token: 0x04000349 RID: 841
		private string _selectionInfoText;

		// Token: 0x0400034A RID: 842
		private string _openProfileText;

		// Token: 0x0400034B RID: 843
		private MPLobbyPlayerBaseVM _player;

		// Token: 0x0400034C RID: 844
		private MPNewsVM _news;

		// Token: 0x0400034D RID: 845
		private MPAnnouncementsVM _announcements;
	}
}
