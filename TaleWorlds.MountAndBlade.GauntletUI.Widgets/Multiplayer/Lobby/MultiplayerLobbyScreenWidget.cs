using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Friend;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Matchmaking;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000AC RID: 172
	public class MultiplayerLobbyScreenWidget : Widget
	{
		// Token: 0x06000914 RID: 2324 RVA: 0x00019FDC File Offset: 0x000181DC
		public MultiplayerLobbyScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00019FE8 File Offset: 0x000181E8
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this.IsLoggedIn)
			{
				foreach (TextureWidget textureWidget in base.GetAllChildrenOfTypeRecursive<TextureWidget>(null))
				{
					if (((textureWidget != null) ? textureWidget.TextureProvider : null) != null && !textureWidget.SetForClearNextFrame)
					{
						textureWidget.OnClearTextureProvider();
					}
				}
			}
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001A060 File Offset: 0x00018260
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.OnLobbyStateChanged();
				this._initialized = true;
			}
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0001A080 File Offset: 0x00018280
		private void OnLoggedInChanged()
		{
			if (!this._isLoggedIn)
			{
				this._stateChangeLocked = true;
				this.IsSearchGameRequested = false;
				this.IsSearchingGame = false;
				this.IsCustomBattleEnabled = false;
				this.IsMatchmakingEnabled = false;
				this.IsPartyLeader = false;
				this.IsInParty = false;
				this._stateChangeLocked = false;
				this.OnLobbyStateChanged();
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001A0D4 File Offset: 0x000182D4
		private void OnLobbyStateChanged()
		{
			if (!this._stateChangeLocked)
			{
				this.MenuWidget.LobbyStateChanged(this.IsSearchGameRequested, this.IsSearchingGame, this.IsMatchmakingEnabled, this.IsCustomBattleEnabled, this.IsPartyLeader, this.IsInParty);
				this.HomeScreenWidget.LobbyStateChanged(this.IsSearchGameRequested, this.IsSearchingGame, this.IsMatchmakingEnabled, this.IsCustomBattleEnabled, this.IsPartyLeader, this.IsInParty);
				this.MatchmakingScreenWidget.LobbyStateChanged(this.IsSearchGameRequested, this.IsSearchingGame, this.IsMatchmakingEnabled, this.IsCustomBattleEnabled, this.IsPartyLeader, this.IsInParty);
				this.ProfileScreenWidget.LobbyStateChanged(this.IsSearchGameRequested, this.IsSearchingGame, this.IsMatchmakingEnabled, this.IsCustomBattleEnabled, this.IsPartyLeader, this.IsInParty);
			}
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001A1A8 File Offset: 0x000183A8
		private void HomeScreenWidgetPropertyChanged(PropertyOwnerObject owner, string property, bool value)
		{
			this.ToggleFriendListOnTabToggled(property, value);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0001A1B2 File Offset: 0x000183B2
		private void SocialScreenWidgetPropertyChanged(PropertyOwnerObject owner, string property, bool value)
		{
			this.ToggleFriendListOnTabToggled(property, value);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001A1BC File Offset: 0x000183BC
		private void ToggleFriendListOnTabToggled(string property, bool value)
		{
			if (this.FriendsPanelWidget != null && property == "IsVisible")
			{
				bool flag = value;
				if (!flag)
				{
					MultiplayerLobbyHomeScreenWidget homeScreenWidget = this.HomeScreenWidget;
					if (homeScreenWidget == null || !homeScreenWidget.IsVisible)
					{
						MultiplayerLobbyProfileScreenWidget profileScreenWidget = this.ProfileScreenWidget;
						if (profileScreenWidget == null || !profileScreenWidget.IsVisible)
						{
							goto IL_0044;
						}
					}
					flag = true;
				}
				IL_0044:
				this.FriendsPanelWidget.IsForcedOpen = flag;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x0001A219 File Offset: 0x00018419
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x0001A221 File Offset: 0x00018421
		[Editor(false)]
		public bool IsLoggedIn
		{
			get
			{
				return this._isLoggedIn;
			}
			set
			{
				if (value != this._isLoggedIn)
				{
					this._isLoggedIn = value;
					base.OnPropertyChanged(value, "IsLoggedIn");
					this.OnLoggedInChanged();
				}
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x0001A245 File Offset: 0x00018445
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x0001A24D File Offset: 0x0001844D
		[Editor(false)]
		public bool IsSearchGameRequested
		{
			get
			{
				return this._isSearchGameRequested;
			}
			set
			{
				if (this._isSearchGameRequested != value)
				{
					this._isSearchGameRequested = value;
					base.OnPropertyChanged(value, "IsSearchGameRequested");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x0001A271 File Offset: 0x00018471
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x0001A279 File Offset: 0x00018479
		[Editor(false)]
		public bool IsSearchingGame
		{
			get
			{
				return this._isSearchingGame;
			}
			set
			{
				if (this._isSearchingGame != value)
				{
					this._isSearchingGame = value;
					base.OnPropertyChanged(value, "IsSearchingGame");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x0001A29D File Offset: 0x0001849D
		// (set) Token: 0x06000923 RID: 2339 RVA: 0x0001A2A5 File Offset: 0x000184A5
		[Editor(false)]
		public bool IsCustomBattleEnabled
		{
			get
			{
				return this._isCustomBattleEnabled;
			}
			set
			{
				if (this._isCustomBattleEnabled != value)
				{
					this._isCustomBattleEnabled = value;
					base.OnPropertyChanged(value, "IsCustomBattleEnabled");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x0001A2C9 File Offset: 0x000184C9
		// (set) Token: 0x06000925 RID: 2341 RVA: 0x0001A2D1 File Offset: 0x000184D1
		[Editor(false)]
		public bool IsMatchmakingEnabled
		{
			get
			{
				return this._isMatchmakingEnabled;
			}
			set
			{
				if (this._isMatchmakingEnabled != value)
				{
					this._isMatchmakingEnabled = value;
					base.OnPropertyChanged(value, "IsMatchmakingEnabled");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x0001A2F5 File Offset: 0x000184F5
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x0001A2FD File Offset: 0x000184FD
		[Editor(false)]
		public bool IsPartyLeader
		{
			get
			{
				return this._isPartyLeader;
			}
			set
			{
				if (this._isPartyLeader != value)
				{
					this._isPartyLeader = value;
					base.OnPropertyChanged(value, "IsPartyLeader");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x0001A321 File Offset: 0x00018521
		// (set) Token: 0x06000929 RID: 2345 RVA: 0x0001A329 File Offset: 0x00018529
		[Editor(false)]
		public bool IsInParty
		{
			get
			{
				return this._isInParty;
			}
			set
			{
				if (this._isInParty != value)
				{
					this._isInParty = value;
					base.OnPropertyChanged(value, "IsInParty");
					this.OnLobbyStateChanged();
				}
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0001A34D File Offset: 0x0001854D
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x0001A355 File Offset: 0x00018555
		[Editor(false)]
		public MultiplayerLobbyMenuWidget MenuWidget
		{
			get
			{
				return this._menuWidget;
			}
			set
			{
				if (this._menuWidget != value)
				{
					this._menuWidget = value;
					base.OnPropertyChanged<MultiplayerLobbyMenuWidget>(value, "MenuWidget");
				}
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0001A373 File Offset: 0x00018573
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x0001A37C File Offset: 0x0001857C
		[Editor(false)]
		public MultiplayerLobbyHomeScreenWidget HomeScreenWidget
		{
			get
			{
				return this._homeScreenWidget;
			}
			set
			{
				if (this._homeScreenWidget != value)
				{
					if (this._homeScreenWidget != null)
					{
						this._homeScreenWidget.boolPropertyChanged -= this.HomeScreenWidgetPropertyChanged;
					}
					this._homeScreenWidget = value;
					if (this._homeScreenWidget != null)
					{
						this._homeScreenWidget.boolPropertyChanged += this.HomeScreenWidgetPropertyChanged;
					}
					base.OnPropertyChanged<MultiplayerLobbyHomeScreenWidget>(value, "HomeScreenWidget");
				}
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x0001A3E3 File Offset: 0x000185E3
		// (set) Token: 0x0600092F RID: 2351 RVA: 0x0001A3EB File Offset: 0x000185EB
		[Editor(false)]
		public MultiplayerLobbyMatchmakingScreenWidget MatchmakingScreenWidget
		{
			get
			{
				return this._matchmakingScreenWidget;
			}
			set
			{
				if (this._matchmakingScreenWidget != value)
				{
					this._matchmakingScreenWidget = value;
					base.OnPropertyChanged<MultiplayerLobbyMatchmakingScreenWidget>(value, "MatchmakingScreenWidget");
				}
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0001A409 File Offset: 0x00018609
		// (set) Token: 0x06000931 RID: 2353 RVA: 0x0001A414 File Offset: 0x00018614
		[Editor(false)]
		public MultiplayerLobbyProfileScreenWidget ProfileScreenWidget
		{
			get
			{
				return this._profileScreenWidget;
			}
			set
			{
				if (this._profileScreenWidget != value)
				{
					if (this._profileScreenWidget != null)
					{
						this._profileScreenWidget.boolPropertyChanged -= this.SocialScreenWidgetPropertyChanged;
					}
					this._profileScreenWidget = value;
					if (this._profileScreenWidget != null)
					{
						this._profileScreenWidget.boolPropertyChanged += this.SocialScreenWidgetPropertyChanged;
					}
					base.OnPropertyChanged<MultiplayerLobbyProfileScreenWidget>(value, "ProfileScreenWidget");
				}
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x0001A47B File Offset: 0x0001867B
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x0001A483 File Offset: 0x00018683
		[Editor(false)]
		public MultiplayerLobbyFriendsPanelWidget FriendsPanelWidget
		{
			get
			{
				return this._friendsPanelWidget;
			}
			set
			{
				if (this._friendsPanelWidget != value)
				{
					this._friendsPanelWidget = value;
					base.OnPropertyChanged<MultiplayerLobbyFriendsPanelWidget>(value, "FriendsPanelWidget");
				}
			}
		}

		// Token: 0x04000419 RID: 1049
		private bool _initialized;

		// Token: 0x0400041A RID: 1050
		private bool _stateChangeLocked;

		// Token: 0x0400041B RID: 1051
		private bool _isLoggedIn;

		// Token: 0x0400041C RID: 1052
		private bool _isSearchGameRequested;

		// Token: 0x0400041D RID: 1053
		private bool _isSearchingGame;

		// Token: 0x0400041E RID: 1054
		private bool _isMatchmakingEnabled;

		// Token: 0x0400041F RID: 1055
		private bool _isPartyLeader;

		// Token: 0x04000420 RID: 1056
		private bool _isInParty;

		// Token: 0x04000421 RID: 1057
		private bool _isCustomBattleEnabled;

		// Token: 0x04000422 RID: 1058
		private MultiplayerLobbyMenuWidget _menuWidget;

		// Token: 0x04000423 RID: 1059
		private MultiplayerLobbyHomeScreenWidget _homeScreenWidget;

		// Token: 0x04000424 RID: 1060
		private MultiplayerLobbyMatchmakingScreenWidget _matchmakingScreenWidget;

		// Token: 0x04000425 RID: 1061
		private MultiplayerLobbyFriendsPanelWidget _friendsPanelWidget;

		// Token: 0x04000426 RID: 1062
		private MultiplayerLobbyProfileScreenWidget _profileScreenWidget;
	}
}
