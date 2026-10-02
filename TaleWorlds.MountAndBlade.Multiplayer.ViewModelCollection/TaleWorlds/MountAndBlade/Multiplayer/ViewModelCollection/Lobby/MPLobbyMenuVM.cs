using System;
using System.Threading.Tasks;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x0200002D RID: 45
	public class MPLobbyMenuVM : ViewModel
	{
		// Token: 0x06000346 RID: 838 RVA: 0x0000C601 File Offset: 0x0000A801
		public MPLobbyMenuVM(LobbyState lobbyState, Action<bool> setNavigationRestriction, Func<Task> onQuit)
		{
			this._lobbyState = lobbyState;
			this._setNavigationRestriction = setNavigationRestriction;
			this._onQuit = onQuit;
			this.RefreshValues();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000C624 File Offset: 0x0000A824
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.HomeText = new TextObject("{=hometab}Home", null).ToString();
			this.MatchmakingText = new TextObject("{=playgame}Play", null).ToString();
			this.ProfileText = new TextObject("{=0647tsif}Profile", null).ToString();
			this.ArmoryText = new TextObject("{=kG0xuyfE}Armory", null).ToString();
			this.PreviousPageInputKey = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToPreviousTab"), true);
			this.NextPageInputKey = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToNextTab"), true);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000C6CF File Offset: 0x0000A8CF
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.PreviousPageInputKey.OnFinalize();
			this.NextPageInputKey.OnFinalize();
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000C6ED File Offset: 0x0000A8ED
		public void SetPage(MPLobbyVM.LobbyPage lobbyPage)
		{
			this.PageIndex = (int)lobbyPage;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000C6F6 File Offset: 0x0000A8F6
		private void ExecuteHome()
		{
			this._lobbyState.OnActivateHome();
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000C703 File Offset: 0x0000A903
		private void ExecuteMatchmaking()
		{
			this._lobbyState.OnActivateMatchmaking();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000C710 File Offset: 0x0000A910
		private void ExecuteCustomServer()
		{
			this._lobbyState.OnActivateCustomServer();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000C71D File Offset: 0x0000A91D
		private void ExecuteArmory()
		{
			this._lobbyState.OnActivateArmory();
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000C72A File Offset: 0x0000A92A
		private void ExecuteOptions()
		{
			this._lobbyState.OnActivateOptions();
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000C737 File Offset: 0x0000A937
		private void ExecuteProfile()
		{
			this._lobbyState.OnActivateProfile();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000C744 File Offset: 0x0000A944
		public async void ExecuteExit()
		{
			Func<Task> onQuit = this._onQuit;
			await ((onQuit != null) ? onQuit() : null);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000C77D File Offset: 0x0000A97D
		public void OnSupportedFeaturesRefreshed(SupportedFeatures supportedFeatures)
		{
			this.IsMatchmakingSupported = supportedFeatures.SupportsFeatures(Features.Matchmaking);
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000C78C File Offset: 0x0000A98C
		// (set) Token: 0x06000353 RID: 851 RVA: 0x0000C794 File Offset: 0x0000A994
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

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000C7B2 File Offset: 0x0000A9B2
		// (set) Token: 0x06000355 RID: 853 RVA: 0x0000C7BA File Offset: 0x0000A9BA
		[DataSourceProperty]
		public bool HasProfileNotification
		{
			get
			{
				return this._hasProfileNotification;
			}
			set
			{
				if (value != this._hasProfileNotification)
				{
					this._hasProfileNotification = value;
					base.OnPropertyChangedWithValue(value, "HasProfileNotification");
				}
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
		// (set) Token: 0x06000357 RID: 855 RVA: 0x0000C7E0 File Offset: 0x0000A9E0
		[DataSourceProperty]
		public bool IsClanSupported
		{
			get
			{
				return this._isClanSupported;
			}
			set
			{
				if (value != this._isClanSupported)
				{
					this._isClanSupported = value;
					base.OnPropertyChangedWithValue(value, "IsClanSupported");
				}
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000C7FE File Offset: 0x0000A9FE
		// (set) Token: 0x06000359 RID: 857 RVA: 0x0000C806 File Offset: 0x0000AA06
		[DataSourceProperty]
		public bool IsMatchmakingSupported
		{
			get
			{
				return this._isMatchmakingSupported;
			}
			set
			{
				if (value != this._isMatchmakingSupported)
				{
					this._isMatchmakingSupported = value;
					base.OnPropertyChangedWithValue(value, "IsMatchmakingSupported");
				}
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0000C824 File Offset: 0x0000AA24
		// (set) Token: 0x0600035B RID: 859 RVA: 0x0000C82C File Offset: 0x0000AA2C
		[DataSourceProperty]
		public int PageIndex
		{
			get
			{
				return this._pageIndex;
			}
			set
			{
				if (value != this._pageIndex)
				{
					this._pageIndex = value;
					base.OnPropertyChangedWithValue(value, "PageIndex");
				}
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000C84A File Offset: 0x0000AA4A
		// (set) Token: 0x0600035D RID: 861 RVA: 0x0000C852 File Offset: 0x0000AA52
		[DataSourceProperty]
		public string HomeText
		{
			get
			{
				return this._homeText;
			}
			set
			{
				if (value != this._homeText)
				{
					this._homeText = value;
					base.OnPropertyChangedWithValue<string>(value, "HomeText");
				}
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000C875 File Offset: 0x0000AA75
		// (set) Token: 0x0600035F RID: 863 RVA: 0x0000C87D File Offset: 0x0000AA7D
		[DataSourceProperty]
		public string MatchmakingText
		{
			get
			{
				return this._matchmakingText;
			}
			set
			{
				if (value != this._matchmakingText)
				{
					this._matchmakingText = value;
					base.OnPropertyChangedWithValue<string>(value, "MatchmakingText");
				}
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000C8A0 File Offset: 0x0000AAA0
		// (set) Token: 0x06000361 RID: 865 RVA: 0x0000C8A8 File Offset: 0x0000AAA8
		[DataSourceProperty]
		public string ProfileText
		{
			get
			{
				return this._profileText;
			}
			set
			{
				if (value != this._profileText)
				{
					this._profileText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProfileText");
				}
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000C8CB File Offset: 0x0000AACB
		// (set) Token: 0x06000363 RID: 867 RVA: 0x0000C8D3 File Offset: 0x0000AAD3
		[DataSourceProperty]
		public string ArmoryText
		{
			get
			{
				return this._armoryText;
			}
			set
			{
				if (value != this._armoryText)
				{
					this._armoryText = value;
					base.OnPropertyChangedWithValue<string>(value, "ArmoryText");
				}
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0000C8F6 File Offset: 0x0000AAF6
		// (set) Token: 0x06000365 RID: 869 RVA: 0x0000C8FE File Offset: 0x0000AAFE
		[DataSourceProperty]
		public InputKeyItemVM PreviousPageInputKey
		{
			get
			{
				return this._previousPageInputKey;
			}
			set
			{
				if (value != this._previousPageInputKey)
				{
					this._previousPageInputKey = value;
					base.OnPropertyChanged("PreviousPageInputKey");
				}
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000C91B File Offset: 0x0000AB1B
		// (set) Token: 0x06000367 RID: 871 RVA: 0x0000C923 File Offset: 0x0000AB23
		[DataSourceProperty]
		public InputKeyItemVM NextPageInputKey
		{
			get
			{
				return this._nextPageInputKey;
			}
			set
			{
				if (value != this._nextPageInputKey)
				{
					this._nextPageInputKey = value;
					base.OnPropertyChanged("NextPageInputKey");
				}
			}
		}

		// Token: 0x040001AF RID: 431
		private LobbyState _lobbyState;

		// Token: 0x040001B0 RID: 432
		private readonly Action<bool> _setNavigationRestriction;

		// Token: 0x040001B1 RID: 433
		private readonly Func<Task> _onQuit;

		// Token: 0x040001B2 RID: 434
		private bool _isEnabled;

		// Token: 0x040001B3 RID: 435
		private bool _hasProfileNotification;

		// Token: 0x040001B4 RID: 436
		private bool _isClanSupported;

		// Token: 0x040001B5 RID: 437
		private bool _isMatchmakingSupported;

		// Token: 0x040001B6 RID: 438
		private int _pageIndex;

		// Token: 0x040001B7 RID: 439
		private string _homeText;

		// Token: 0x040001B8 RID: 440
		private string _matchmakingText;

		// Token: 0x040001B9 RID: 441
		private string _profileText;

		// Token: 0x040001BA RID: 442
		private string _armoryText;

		// Token: 0x040001BB RID: 443
		private InputKeyItemVM _previousPageInputKey;

		// Token: 0x040001BC RID: 444
		private InputKeyItemVM _nextPageInputKey;
	}
}
