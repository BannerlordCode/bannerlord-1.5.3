using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006F RID: 111
	public class MPLobbyClanLeaderboardVM : ViewModel
	{
		// Token: 0x06000ACD RID: 2765 RVA: 0x00021273 File Offset: 0x0001F473
		public MPLobbyClanLeaderboardVM()
		{
			this.ClanItems = new MBBindingList<MPLobbyClanItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0002128C File Offset: 0x0001F48C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.LeaderboardText = new TextObject("{=vGF5S2hE}Leaderboard", null).ToString();
			this.ClansText = new TextObject("{=bfQLwMUp}Clans", null).ToString();
			this.NameText = new TextObject("{=PDdh1sBj}Name", null).ToString();
			this.GamesWonText = new TextObject("{=dxlkHhw5}Games Won", null).ToString();
			this.GamesLostText = new TextObject("{=BrjpmaJH}Games Lost", null).ToString();
			this.NextText = new TextObject("{=Rvr1bcu8}Next", null).ToString();
			this.PreviousText = new TextObject("{=WXAaWZVf}Previous", null).ToString();
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00021350 File Offset: 0x0001F550
		private async void LoadClanLeaderboard()
		{
			this.IsDataLoading = true;
			ClanLeaderboardInfo clanLeaderboardInfo = await NetworkMain.GameClient.GetClanLeaderboardInfo();
			if (((clanLeaderboardInfo != null) ? clanLeaderboardInfo.ClanEntries : null) != null)
			{
				this._clans = clanLeaderboardInfo.ClanEntries;
			}
			else
			{
				this._clans = new ClanLeaderboardEntry[0];
			}
			this.SortController = new MPLobbyClanLeaderboardSortControllerVM(ref this._clans, new Action(this.OnClansSorted));
			this.GoToPage(0);
			this.IsDataLoading = false;
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00021389 File Offset: 0x0001F589
		private void OnClansSorted()
		{
			this.GoToPage(0);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00021394 File Offset: 0x0001F594
		private void GoToPage(int pageNumber)
		{
			int num = pageNumber * 30;
			if (this._clans == null || num > this._clans.Length - 1)
			{
				return;
			}
			this.ClanItems.Clear();
			int num2 = num;
			while (num2 < num + 30 && num2 != this._clans.Length)
			{
				ClanLeaderboardEntry clanLeaderboardEntry = this._clans[num2];
				this.ClanItems.Add(new MPLobbyClanItemVM(clanLeaderboardEntry.Name, clanLeaderboardEntry.Tag, clanLeaderboardEntry.Sigil, clanLeaderboardEntry.WinCount, clanLeaderboardEntry.LossCount, num2 + 1, clanLeaderboardEntry.ClanId.Equals(NetworkMain.GameClient.ClanID)));
				num2++;
			}
			this._currentPageNumber = pageNumber;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0002143A File Offset: 0x0001F63A
		private void ExecuteGoToNextPage()
		{
			if (this._currentPageNumber + 1 <= this._clans.Length / 30)
			{
				this.GoToPage(this._currentPageNumber + 1);
				return;
			}
			this.GoToPage(0);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00021467 File Offset: 0x0001F667
		private void ExecuteGoToPreviousPage()
		{
			if (this._currentPageNumber > 0)
			{
				this.GoToPage(this._currentPageNumber - 1);
				return;
			}
			this.GoToPage(this._clans.Length / 30);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00021492 File Offset: 0x0001F692
		public void ExecuteOpenPopup()
		{
			this.IsEnabled = true;
			this.LoadClanLeaderboard();
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000214A1 File Offset: 0x0001F6A1
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x000214AA File Offset: 0x0001F6AA
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x000214B2 File Offset: 0x0001F6B2
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

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x000214D0 File Offset: 0x0001F6D0
		// (set) Token: 0x06000AD9 RID: 2777 RVA: 0x000214D8 File Offset: 0x0001F6D8
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
				}
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x000214F6 File Offset: 0x0001F6F6
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x000214FE File Offset: 0x0001F6FE
		[DataSourceProperty]
		public string LeaderboardText
		{
			get
			{
				return this._leaderboardText;
			}
			set
			{
				if (value != this._leaderboardText)
				{
					this._leaderboardText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaderboardText");
				}
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x00021521 File Offset: 0x0001F721
		// (set) Token: 0x06000ADD RID: 2781 RVA: 0x00021529 File Offset: 0x0001F729
		[DataSourceProperty]
		public string ClansText
		{
			get
			{
				return this._clansText;
			}
			set
			{
				if (value != this._clansText)
				{
					this._clansText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClansText");
				}
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x0002154C File Offset: 0x0001F74C
		// (set) Token: 0x06000ADF RID: 2783 RVA: 0x00021554 File Offset: 0x0001F754
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x00021577 File Offset: 0x0001F777
		// (set) Token: 0x06000AE1 RID: 2785 RVA: 0x0002157F File Offset: 0x0001F77F
		[DataSourceProperty]
		public string GamesWonText
		{
			get
			{
				return this._gamesWonText;
			}
			set
			{
				if (value != this._gamesWonText)
				{
					this._gamesWonText = value;
					base.OnPropertyChangedWithValue<string>(value, "GamesWonText");
				}
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x000215A2 File Offset: 0x0001F7A2
		// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x000215AA File Offset: 0x0001F7AA
		[DataSourceProperty]
		public string GamesLostText
		{
			get
			{
				return this._gamesLostText;
			}
			set
			{
				if (value != this._gamesLostText)
				{
					this._gamesLostText = value;
					base.OnPropertyChangedWithValue<string>(value, "GamesLostText");
				}
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x000215CD File Offset: 0x0001F7CD
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x000215D5 File Offset: 0x0001F7D5
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

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x000215F8 File Offset: 0x0001F7F8
		// (set) Token: 0x06000AE7 RID: 2791 RVA: 0x00021600 File Offset: 0x0001F800
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

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00021623 File Offset: 0x0001F823
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x0002162B File Offset: 0x0001F82B
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

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0002164E File Offset: 0x0001F84E
		// (set) Token: 0x06000AEB RID: 2795 RVA: 0x00021656 File Offset: 0x0001F856
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanItemVM> ClanItems
		{
			get
			{
				return this._clanItems;
			}
			set
			{
				if (value != this._clanItems)
				{
					this._clanItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClanItemVM>>(value, "ClanItems");
				}
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x00021674 File Offset: 0x0001F874
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x0002167C File Offset: 0x0001F87C
		[DataSourceProperty]
		public MPLobbyClanLeaderboardSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<MPLobbyClanLeaderboardSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x040004ED RID: 1261
		private ClanLeaderboardEntry[] _clans;

		// Token: 0x040004EE RID: 1262
		private const int _clansPerPage = 30;

		// Token: 0x040004EF RID: 1263
		private int _currentPageNumber;

		// Token: 0x040004F0 RID: 1264
		private bool _isEnabled;

		// Token: 0x040004F1 RID: 1265
		private bool _isDataLoading;

		// Token: 0x040004F2 RID: 1266
		private string _leaderboardText;

		// Token: 0x040004F3 RID: 1267
		private string _clansText;

		// Token: 0x040004F4 RID: 1268
		private string _nameText;

		// Token: 0x040004F5 RID: 1269
		private string _gamesWonText;

		// Token: 0x040004F6 RID: 1270
		private string _gamesLostText;

		// Token: 0x040004F7 RID: 1271
		private string _nextText;

		// Token: 0x040004F8 RID: 1272
		private string _previousText;

		// Token: 0x040004F9 RID: 1273
		private string _closeText;

		// Token: 0x040004FA RID: 1274
		private MBBindingList<MPLobbyClanItemVM> _clanItems;

		// Token: 0x040004FB RID: 1275
		private MPLobbyClanLeaderboardSortControllerVM _sortController;
	}
}
