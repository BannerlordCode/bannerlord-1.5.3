using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame
{
	// Token: 0x02000061 RID: 97
	public class MPCustomGameSortControllerVM : ViewModel
	{
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x0001CD90 File Offset: 0x0001AF90
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x0001CD98 File Offset: 0x0001AF98
		public MPCustomGameSortControllerVM.CustomServerSortOption? CurrentSortOption { get; private set; }

		// Token: 0x0600091C RID: 2332 RVA: 0x0001CDA4 File Offset: 0x0001AFA4
		public MPCustomGameSortControllerVM(ref MBBindingList<MPCustomGameItemVM> listToControl, MPCustomGameVM.CustomGameMode customGameMode)
		{
			this._listToControl = listToControl;
			this.IsPremadeMatchesList = customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame;
			this.IsPingInfoAvailable = MPCustomGameVM.IsPingInfoAvailable && !this.IsPremadeMatchesList;
			this._numberOfSortOptions = 11;
			this._sortComparers = new MPCustomGameSortControllerVM.ItemComparer[this._numberOfSortOptions];
			for (MPCustomGameSortControllerVM.CustomServerSortOption customServerSortOption = MPCustomGameSortControllerVM.CustomServerSortOption.Name; customServerSortOption < MPCustomGameSortControllerVM.CustomServerSortOption.SortOptionsEndExclusive; customServerSortOption++)
			{
				MPCustomGameSortControllerVM.ItemComparer sortComparer = this.GetSortComparer(customServerSortOption);
				if (sortComparer != null)
				{
					this._sortComparers[(int)customServerSortOption] = sortComparer;
				}
				else
				{
					Debug.FailedAssert("No valid comparer for custom server sort option: " + customServerSortOption.ToString(), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\CustomGame\\MPCustomGameSortControllerVM.cs", ".ctor", 59);
				}
			}
			this.RefreshValues();
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001CE4C File Offset: 0x0001B04C
		private MPCustomGameSortControllerVM.ItemComparer GetSortComparer(MPCustomGameSortControllerVM.CustomServerSortOption option)
		{
			switch (option)
			{
			case MPCustomGameSortControllerVM.CustomServerSortOption.SortOptionsBeginExclusive:
			case MPCustomGameSortControllerVM.CustomServerSortOption.SortOptionsEndExclusive:
				return null;
			case MPCustomGameSortControllerVM.CustomServerSortOption.Name:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.ServerNameComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.GameType:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.GameTypeComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.PlayerCount:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.PlayerCountComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.PasswordProtection:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.PasswordComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.FirstFaction:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.FirstFactionComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.SecondFaction:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.SecondFactionComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.Region:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.RegionComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.PremadeMatchType:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.PremadeMatchTypeComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.Host:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.HostComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.Ping:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.PingComparer();
			case MPCustomGameSortControllerVM.CustomServerSortOption.Favorite:
				return this._sortComparers[(int)option] ?? new MPCustomGameSortControllerVM.FavoriteComparer();
			default:
				return null;
			}
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0001CF63 File Offset: 0x0001B163
		public void InitializeWithSortState(MPCustomGameSortControllerVM.CustomServerSortOption? sortOption, MPCustomGameSortControllerVM.SortState sortState = MPCustomGameSortControllerVM.SortState.Default)
		{
			this.SetSortOption(sortOption);
			this.CurrentSortState = (int)sortState;
			this.SortByCurrentState();
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0001CF7C File Offset: 0x0001B17C
		private void SetSortOption(MPCustomGameSortControllerVM.CustomServerSortOption? sortOption)
		{
			MPCustomGameSortControllerVM.CustomServerSortOption? currentSortOption = this.CurrentSortOption;
			MPCustomGameSortControllerVM.CustomServerSortOption? customServerSortOption = sortOption;
			if ((currentSortOption.GetValueOrDefault() == customServerSortOption.GetValueOrDefault()) & (currentSortOption != null == (customServerSortOption != null)))
			{
				return;
			}
			this.CurrentSortOption = sortOption;
			this.RefreshSelectedStates();
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001CFC4 File Offset: 0x0001B1C4
		public void SortByCurrentState()
		{
			if (this.CurrentSortOption == null)
			{
				return;
			}
			MPCustomGameSortControllerVM.ItemComparer sortComparer = this.GetSortComparer(this.CurrentSortOption.Value);
			MPCustomGameSortControllerVM.SortState currentSortState = (MPCustomGameSortControllerVM.SortState)this.CurrentSortState;
			if (sortComparer != null)
			{
				sortComparer.SetSortMode(currentSortState != MPCustomGameSortControllerVM.SortState.Descending);
				this._listToControl.Sort(sortComparer);
			}
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0001D01C File Offset: 0x0001B21C
		private void SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption option)
		{
			MPCustomGameSortControllerVM.CustomServerSortOption? currentSortOption = this.CurrentSortOption;
			if (!((option == currentSortOption.GetValueOrDefault()) & (currentSortOption != null)))
			{
				this.CurrentSortState = 1;
			}
			else
			{
				this.CurrentSortState = (this.CurrentSortState + 1) % 3;
			}
			this.SetSortOption(new MPCustomGameSortControllerVM.CustomServerSortOption?(option));
			this.SortByCurrentState();
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001D06F File Offset: 0x0001B26F
		public void ExecuteSortByFavorites()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.Favorite);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001D079 File Offset: 0x0001B279
		public void ExecuteSortByServerName()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.Name);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001D082 File Offset: 0x0001B282
		public void ExecuteSortByGameType()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.GameType);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001D08B File Offset: 0x0001B28B
		public void ExecuteSortByPlayerCount()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.PlayerCount);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001D094 File Offset: 0x0001B294
		public void ExecuteSortByPassword()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.PasswordProtection);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001D09D File Offset: 0x0001B29D
		public void ExecuteSortByFirstFaction()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.FirstFaction);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001D0A6 File Offset: 0x0001B2A6
		public void ExecuteSortBySecondFaction()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.SecondFaction);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001D0AF File Offset: 0x0001B2AF
		public void ExecuteSortByRegion()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.Region);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001D0B8 File Offset: 0x0001B2B8
		public void ExecuteSortByPremadeMatchType()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.PremadeMatchType);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001D0C1 File Offset: 0x0001B2C1
		public void ExecuteSortByHost()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.Host);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001D0CA File Offset: 0x0001B2CA
		public void ExecuteSortByPing()
		{
			this.SortWithOptionAux(MPCustomGameSortControllerVM.CustomServerSortOption.Ping);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001D0D4 File Offset: 0x0001B2D4
		private void RefreshSelectedStates()
		{
			MPCustomGameSortControllerVM.CustomServerSortOption? customServerSortOption = this.CurrentSortOption;
			MPCustomGameSortControllerVM.CustomServerSortOption customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.Favorite;
			this.IsFavoritesSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.Name;
			this.IsServerNameSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.GameType;
			this.IsGameTypeSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.PlayerCount;
			this.IsPlayerCountSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.PasswordProtection;
			this.IsPasswordSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.FirstFaction;
			this.IsFirstFactionSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.SecondFaction;
			this.IsSecondFactionSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.Region;
			this.IsRegionSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.PremadeMatchType;
			this.IsPremadeMatchTypeSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.Host;
			this.IsHostSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
			customServerSortOption = this.CurrentSortOption;
			customServerSortOption2 = MPCustomGameSortControllerVM.CustomServerSortOption.Ping;
			this.IsPingSelected = (customServerSortOption.GetValueOrDefault() == customServerSortOption2) & (customServerSortOption != null);
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x0001D24E File Offset: 0x0001B44E
		// (set) Token: 0x0600092F RID: 2351 RVA: 0x0001D256 File Offset: 0x0001B456
		[DataSourceProperty]
		public bool IsPremadeMatchesList
		{
			get
			{
				return this._isPremadeMatchesList;
			}
			set
			{
				if (value != this._isPremadeMatchesList)
				{
					this._isPremadeMatchesList = value;
					base.OnPropertyChanged("IsPremadeMatchesList");
				}
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0001D273 File Offset: 0x0001B473
		// (set) Token: 0x06000931 RID: 2353 RVA: 0x0001D27B File Offset: 0x0001B47B
		[DataSourceProperty]
		public bool IsPingInfoAvailable
		{
			get
			{
				return this._isPingInfoAvailable;
			}
			set
			{
				if (value != this._isPingInfoAvailable)
				{
					this._isPingInfoAvailable = value;
					base.OnPropertyChanged("IsPingInfoAvailable");
				}
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x0001D298 File Offset: 0x0001B498
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x0001D2A0 File Offset: 0x0001B4A0
		[DataSourceProperty]
		public int CurrentSortState
		{
			get
			{
				return this._currentSortState;
			}
			set
			{
				if (value != this._currentSortState)
				{
					this._currentSortState = value;
					base.OnPropertyChanged("CurrentSortState");
				}
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x0001D2BD File Offset: 0x0001B4BD
		// (set) Token: 0x06000935 RID: 2357 RVA: 0x0001D2C5 File Offset: 0x0001B4C5
		[DataSourceProperty]
		public bool IsFavoritesSelected
		{
			get
			{
				return this._isFavoritesSelected;
			}
			set
			{
				if (value != this._isFavoritesSelected)
				{
					this._isFavoritesSelected = value;
					base.OnPropertyChanged("IsFavoritesSelected");
				}
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x0001D2E2 File Offset: 0x0001B4E2
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x0001D2EA File Offset: 0x0001B4EA
		[DataSourceProperty]
		public bool IsServerNameSelected
		{
			get
			{
				return this._isServerNameSelected;
			}
			set
			{
				if (value != this._isServerNameSelected)
				{
					this._isServerNameSelected = value;
					base.OnPropertyChanged("IsServerNameSelected");
				}
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0001D307 File Offset: 0x0001B507
		// (set) Token: 0x06000939 RID: 2361 RVA: 0x0001D30F File Offset: 0x0001B50F
		[DataSourceProperty]
		public bool IsPasswordSelected
		{
			get
			{
				return this._isPasswordSelected;
			}
			set
			{
				if (value != this._isPasswordSelected)
				{
					this._isPasswordSelected = value;
					base.OnPropertyChanged("IsPasswordSelected");
				}
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x0001D32C File Offset: 0x0001B52C
		// (set) Token: 0x0600093B RID: 2363 RVA: 0x0001D334 File Offset: 0x0001B534
		[DataSourceProperty]
		public bool IsPlayerCountSelected
		{
			get
			{
				return this._isPlayerCountSelected;
			}
			set
			{
				if (value != this._isPlayerCountSelected)
				{
					this._isPlayerCountSelected = value;
					base.OnPropertyChanged("IsPlayerCountSelected");
				}
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0001D351 File Offset: 0x0001B551
		// (set) Token: 0x0600093D RID: 2365 RVA: 0x0001D359 File Offset: 0x0001B559
		[DataSourceProperty]
		public bool IsFirstFactionSelected
		{
			get
			{
				return this._isFirstFactionSelected;
			}
			set
			{
				if (value != this._isFirstFactionSelected)
				{
					this._isFirstFactionSelected = value;
					base.OnPropertyChanged("IsFirstFactionSelected");
				}
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x0001D376 File Offset: 0x0001B576
		// (set) Token: 0x0600093F RID: 2367 RVA: 0x0001D37E File Offset: 0x0001B57E
		[DataSourceProperty]
		public bool IsGameTypeSelected
		{
			get
			{
				return this._isGameTypeSelected;
			}
			set
			{
				if (value != this._isGameTypeSelected)
				{
					this._isGameTypeSelected = value;
					base.OnPropertyChanged("IsGameTypeSelected");
				}
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x0001D39B File Offset: 0x0001B59B
		// (set) Token: 0x06000941 RID: 2369 RVA: 0x0001D3A3 File Offset: 0x0001B5A3
		[DataSourceProperty]
		public bool IsSecondFactionSelected
		{
			get
			{
				return this._isSecondFactionSelected;
			}
			set
			{
				if (value != this._isSecondFactionSelected)
				{
					this._isSecondFactionSelected = value;
					base.OnPropertyChanged("IsSecondFactionSelected");
				}
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x0001D3C0 File Offset: 0x0001B5C0
		// (set) Token: 0x06000943 RID: 2371 RVA: 0x0001D3C8 File Offset: 0x0001B5C8
		[DataSourceProperty]
		public bool IsRegionSelected
		{
			get
			{
				return this._isRegionSelected;
			}
			set
			{
				if (value != this._isRegionSelected)
				{
					this._isRegionSelected = value;
					base.OnPropertyChanged("IsRegionSelected");
				}
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x0001D3E5 File Offset: 0x0001B5E5
		// (set) Token: 0x06000945 RID: 2373 RVA: 0x0001D3ED File Offset: 0x0001B5ED
		[DataSourceProperty]
		public bool IsPremadeMatchTypeSelected
		{
			get
			{
				return this._isPremadeMatchTypeSelected;
			}
			set
			{
				if (value != this._isPremadeMatchTypeSelected)
				{
					this._isPremadeMatchTypeSelected = value;
					base.OnPropertyChanged("IsPremadeMatchTypeSelected");
				}
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x0001D40A File Offset: 0x0001B60A
		// (set) Token: 0x06000947 RID: 2375 RVA: 0x0001D412 File Offset: 0x0001B612
		[DataSourceProperty]
		public bool IsHostSelected
		{
			get
			{
				return this._isHostSelected;
			}
			set
			{
				if (value != this._isHostSelected)
				{
					this._isHostSelected = value;
					base.OnPropertyChanged("IsHostSelected");
				}
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x0001D42F File Offset: 0x0001B62F
		// (set) Token: 0x06000949 RID: 2377 RVA: 0x0001D437 File Offset: 0x0001B637
		[DataSourceProperty]
		public bool IsPingSelected
		{
			get
			{
				return this._isPingSelected;
			}
			set
			{
				if (value != this._isPingSelected)
				{
					this._isPingSelected = value;
					base.OnPropertyChanged("IsPingSelected");
				}
			}
		}

		// Token: 0x0400043A RID: 1082
		private MBBindingList<MPCustomGameItemVM> _listToControl;

		// Token: 0x0400043C RID: 1084
		private readonly MPCustomGameSortControllerVM.ItemComparer[] _sortComparers;

		// Token: 0x0400043D RID: 1085
		private readonly int _numberOfSortOptions;

		// Token: 0x0400043E RID: 1086
		private bool _isPremadeMatchesList;

		// Token: 0x0400043F RID: 1087
		private bool _isPingInfoAvailable;

		// Token: 0x04000440 RID: 1088
		private int _currentSortState;

		// Token: 0x04000441 RID: 1089
		private bool _isFavoritesSelected;

		// Token: 0x04000442 RID: 1090
		private bool _isServerNameSelected;

		// Token: 0x04000443 RID: 1091
		private bool _isGameTypeSelected;

		// Token: 0x04000444 RID: 1092
		private bool _isPlayerCountSelected;

		// Token: 0x04000445 RID: 1093
		private bool _isPasswordSelected;

		// Token: 0x04000446 RID: 1094
		private bool _isFirstFactionSelected;

		// Token: 0x04000447 RID: 1095
		private bool _isSecondFactionSelected;

		// Token: 0x04000448 RID: 1096
		private bool _isRegionSelected;

		// Token: 0x04000449 RID: 1097
		private bool _isPremadeMatchTypeSelected;

		// Token: 0x0400044A RID: 1098
		private bool _isHostSelected;

		// Token: 0x0400044B RID: 1099
		private bool _isPingSelected;

		// Token: 0x02000133 RID: 307
		public enum SortState
		{
			// Token: 0x040009C0 RID: 2496
			Default,
			// Token: 0x040009C1 RID: 2497
			Ascending,
			// Token: 0x040009C2 RID: 2498
			Descending
		}

		// Token: 0x02000134 RID: 308
		public enum CustomServerSortOption
		{
			// Token: 0x040009C4 RID: 2500
			SortOptionsBeginExclusive = -1,
			// Token: 0x040009C5 RID: 2501
			Name,
			// Token: 0x040009C6 RID: 2502
			GameType,
			// Token: 0x040009C7 RID: 2503
			PlayerCount,
			// Token: 0x040009C8 RID: 2504
			PasswordProtection,
			// Token: 0x040009C9 RID: 2505
			FirstFaction,
			// Token: 0x040009CA RID: 2506
			SecondFaction,
			// Token: 0x040009CB RID: 2507
			Region,
			// Token: 0x040009CC RID: 2508
			PremadeMatchType,
			// Token: 0x040009CD RID: 2509
			Host,
			// Token: 0x040009CE RID: 2510
			Ping,
			// Token: 0x040009CF RID: 2511
			Favorite,
			// Token: 0x040009D0 RID: 2512
			SortOptionsEndExclusive
		}

		// Token: 0x02000135 RID: 309
		private abstract class ItemComparer : IComparer<MPCustomGameItemVM>
		{
			// Token: 0x060012B7 RID: 4791 RVA: 0x0003B5AA File Offset: 0x000397AA
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x060012B8 RID: 4792
			public abstract int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y);

			// Token: 0x040009D1 RID: 2513
			protected bool _isAscending;
		}

		// Token: 0x02000136 RID: 310
		private class ServerNameComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x060012BA RID: 4794 RVA: 0x0003B5BB File Offset: 0x000397BB
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.NameText.CompareTo(x.NameText) * -1;
				}
				return y.NameText.CompareTo(x.NameText);
			}
		}

		// Token: 0x02000137 RID: 311
		private class GameTypeComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x060012BC RID: 4796 RVA: 0x0003B5F2 File Offset: 0x000397F2
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.GameTypeText.CompareTo(x.GameTypeText) * -1;
				}
				return y.GameTypeText.CompareTo(x.GameTypeText);
			}
		}

		// Token: 0x02000138 RID: 312
		private class PlayerCountComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x060012BE RID: 4798 RVA: 0x0003B62C File Offset: 0x0003982C
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.PlayerCount.CompareTo(x.PlayerCount) * -1;
				}
				return y.PlayerCount.CompareTo(x.PlayerCount);
			}
		}

		// Token: 0x02000139 RID: 313
		private class PasswordComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x060012C0 RID: 4800 RVA: 0x0003B674 File Offset: 0x00039874
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.IsPasswordProtected.CompareTo(x.IsPasswordProtected) * -1;
				}
				return y.IsPasswordProtected.CompareTo(x.IsPasswordProtected);
			}
		}

		// Token: 0x0200013A RID: 314
		private class FirstFactionComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x060012C2 RID: 4802 RVA: 0x0003B6BC File Offset: 0x000398BC
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.FirstFactionName.CompareTo(x.FirstFactionName) * -1;
				}
				return y.FirstFactionName.CompareTo(x.FirstFactionName);
			}
		}

		// Token: 0x0200013B RID: 315
		private class SecondFactionComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x060012C4 RID: 4804 RVA: 0x0003B6F3 File Offset: 0x000398F3
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.SecondFactionName.CompareTo(x.SecondFactionName) * -1;
				}
				return y.SecondFactionName.CompareTo(x.SecondFactionName);
			}
		}

		// Token: 0x0200013C RID: 316
		private class RegionComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x060012C6 RID: 4806 RVA: 0x0003B72A File Offset: 0x0003992A
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.RegionName.CompareTo(x.RegionName) * -1;
				}
				return y.RegionName.CompareTo(x.RegionName);
			}
		}

		// Token: 0x0200013D RID: 317
		private class PremadeMatchTypeComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x060012C8 RID: 4808 RVA: 0x0003B761 File Offset: 0x00039961
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (this._isAscending)
				{
					return y.PremadeMatchTypeText.CompareTo(x.PremadeMatchTypeText) * -1;
				}
				return y.PremadeMatchTypeText.CompareTo(x.PremadeMatchTypeText);
			}
		}

		// Token: 0x0200013E RID: 318
		private class HostComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x060012CA RID: 4810 RVA: 0x0003B798 File Offset: 0x00039998
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				if (!(y.HostText == x.HostText))
				{
					string hostText = y.HostText;
					return ((hostText != null) ? hostText.CompareTo(x.HostText) : (-1)) * (this._isAscending ? (-1) : 1);
				}
				return 0;
			}
		}

		// Token: 0x0200013F RID: 319
		private class PingComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x060012CC RID: 4812 RVA: 0x0003B7DC File Offset: 0x000399DC
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				int num = (this._isAscending ? (-1) : 1);
				if (y.PingText == x.PingText)
				{
					return 0;
				}
				if (y.PingText == "-" || y.PingText == null)
				{
					return num;
				}
				if (x.PingText == "-" || x.PingText == null)
				{
					return num * -1;
				}
				return (int)(long.Parse(y.PingText) - long.Parse(x.PingText)) * num;
			}
		}

		// Token: 0x02000140 RID: 320
		private class FavoriteComparer : MPCustomGameSortControllerVM.ItemComparer
		{
			// Token: 0x060012CE RID: 4814 RVA: 0x0003B868 File Offset: 0x00039A68
			public override int Compare(MPCustomGameItemVM x, MPCustomGameItemVM y)
			{
				return y.IsFavorite.CompareTo(x.IsFavorite) * (this._isAscending ? (-1) : 1);
			}
		}
	}
}
