using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Matchmaking
{
	// Token: 0x020000AE RID: 174
	public class MultiplayerLobbyMatchmakingScreenWidget : Widget
	{
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0001A557 File Offset: 0x00018757
		// (set) Token: 0x06000939 RID: 2361 RVA: 0x0001A55F File Offset: 0x0001875F
		public MultiplayerLobbyCustomServerScreenWidget CustomServerParentWidget { get; set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x0001A568 File Offset: 0x00018768
		// (set) Token: 0x0600093B RID: 2363 RVA: 0x0001A570 File Offset: 0x00018770
		public MultiplayerLobbyCustomServerScreenWidget PremadeMatchesParentWidget { get; set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0001A579 File Offset: 0x00018779
		private MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages _selectedMode
		{
			get
			{
				return (MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages)this.SelectedModeIndex;
			}
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0001A581 File Offset: 0x00018781
		public MultiplayerLobbyMatchmakingScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0001A58C File Offset: 0x0001878C
		public void LobbyStateChanged(bool isSearchRequested, bool isSearching, bool isMatchmakingEnabled, bool isCustomBattleEnabled, bool isPartyLeader, bool isInParty)
		{
			this._latestIsSearchRequested = isSearchRequested;
			this._latestIsSearching = isSearching;
			this._latestIsMatchmakingEnabled = isMatchmakingEnabled;
			this._latestIsCustomBattleEnabled = isCustomBattleEnabled;
			this._latestIsPartyLeader = isPartyLeader;
			this._latestIsInParty = isInParty;
			if (this.CustomServerParentWidget != null)
			{
				this.CustomServerParentWidget.IsInParty = isInParty;
				this.CustomServerParentWidget.IsPartyLeader = isPartyLeader;
			}
			if (this.PremadeMatchesParentWidget != null)
			{
				this.PremadeMatchesParentWidget.IsInParty = isInParty;
				this.PremadeMatchesParentWidget.IsPartyLeader = isPartyLeader;
			}
			this.UpdateStates();
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0001A610 File Offset: 0x00018810
		private void UpdateStates()
		{
			this.FindGameButton.IsEnabled = ((this._selectedMode != MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.CustomGame && this._latestIsMatchmakingEnabled && this.IsMatchFindPossible) || (this._selectedMode == MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.CustomGame && this.IsCustomGameFindEnabled)) && (this._latestIsPartyLeader || !this._latestIsInParty) && !this._latestIsSearchRequested;
			this.FindGameButton.IsVisible = !this._latestIsSearching && ((this._latestIsCustomBattleEnabled && this._selectedMode == MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.CustomGame) || (this._latestIsMatchmakingEnabled && this._selectedMode == MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.QuickPlay));
			this.SelectionInfo.IsEnabled = this._latestIsMatchmakingEnabled;
			this.SelectionInfo.IsVisible = !this._latestIsSearching && !this._latestIsCustomBattleEnabled;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0001A6DC File Offset: 0x000188DC
		private void OnSubpageIndexChange()
		{
			this.FindGameButton.IsVisible = !this._latestIsSearching && ((this._latestIsCustomBattleEnabled && this._selectedMode == MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.CustomGame) || (this._latestIsMatchmakingEnabled && this._selectedMode == MultiplayerLobbyMatchmakingScreenWidget.MatchmakingSubPages.QuickPlay));
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x0001A71C File Offset: 0x0001891C
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x0001A724 File Offset: 0x00018924
		[Editor(false)]
		public bool IsMatchFindPossible
		{
			get
			{
				return this._isMatchFindPossible;
			}
			set
			{
				if (this._isMatchFindPossible != value)
				{
					this._isMatchFindPossible = value;
					base.OnPropertyChanged(value, "IsMatchFindPossible");
					this.UpdateStates();
				}
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x0001A748 File Offset: 0x00018948
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x0001A750 File Offset: 0x00018950
		[Editor(false)]
		public bool IsCustomGameFindEnabled
		{
			get
			{
				return this._isCustomGameFindEnabled;
			}
			set
			{
				if (this._isCustomGameFindEnabled != value)
				{
					this._isCustomGameFindEnabled = value;
					base.OnPropertyChanged(value, "IsCustomGameFindEnabled");
					this.UpdateStates();
				}
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x0001A774 File Offset: 0x00018974
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x0001A77C File Offset: 0x0001897C
		[Editor(false)]
		public int SelectedModeIndex
		{
			get
			{
				return this._selectedModeIndex;
			}
			set
			{
				if (this._selectedModeIndex != value)
				{
					this._selectedModeIndex = value;
					base.OnPropertyChanged(value, "SelectedModeIndex");
					this.OnSubpageIndexChange();
				}
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x0001A7A0 File Offset: 0x000189A0
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x0001A7A8 File Offset: 0x000189A8
		[Editor(false)]
		public ButtonWidget FindGameButton
		{
			get
			{
				return this._findGameButton;
			}
			set
			{
				if (this._findGameButton != value)
				{
					this._findGameButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "FindGameButton");
				}
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x0001A7C6 File Offset: 0x000189C6
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x0001A7CE File Offset: 0x000189CE
		[Editor(false)]
		public Widget SelectionInfo
		{
			get
			{
				return this._selectionInfo;
			}
			set
			{
				if (this._selectionInfo != value)
				{
					this._selectionInfo = value;
					base.OnPropertyChanged<Widget>(value, "SelectionInfo");
				}
			}
		}

		// Token: 0x0400042A RID: 1066
		private bool _latestIsSearchRequested;

		// Token: 0x0400042B RID: 1067
		private bool _latestIsSearching;

		// Token: 0x0400042C RID: 1068
		private bool _latestIsMatchmakingEnabled;

		// Token: 0x0400042D RID: 1069
		private bool _latestIsCustomBattleEnabled;

		// Token: 0x0400042E RID: 1070
		private bool _latestIsPartyLeader;

		// Token: 0x0400042F RID: 1071
		private bool _latestIsInParty;

		// Token: 0x04000430 RID: 1072
		private ButtonWidget _findGameButton;

		// Token: 0x04000431 RID: 1073
		private Widget _selectionInfo;

		// Token: 0x04000432 RID: 1074
		private int _selectedModeIndex;

		// Token: 0x04000433 RID: 1075
		private bool _isMatchFindPossible;

		// Token: 0x04000434 RID: 1076
		private bool _isCustomGameFindEnabled;

		// Token: 0x020001BC RID: 444
		private enum MatchmakingSubPages
		{
			// Token: 0x04000A23 RID: 2595
			QuickPlay,
			// Token: 0x04000A24 RID: 2596
			CustomGame,
			// Token: 0x04000A25 RID: 2597
			CustomGameList,
			// Token: 0x04000A26 RID: 2598
			PremadeMatchList,
			// Token: 0x04000A27 RID: 2599
			Default
		}
	}
}
