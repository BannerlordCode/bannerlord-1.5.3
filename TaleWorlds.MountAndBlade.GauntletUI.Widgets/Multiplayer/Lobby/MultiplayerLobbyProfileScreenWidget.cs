using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000AA RID: 170
	public class MultiplayerLobbyProfileScreenWidget : Widget
	{
		// Token: 0x06000905 RID: 2309 RVA: 0x00019E1C File Offset: 0x0001801C
		public MultiplayerLobbyProfileScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00019E28 File Offset: 0x00018028
		public void LobbyStateChanged(bool isSearchRequested, bool isSearching, bool isMatchmakingEnabled, bool isCustomBattleEnabled, bool isPartyLeader, bool isInParty)
		{
			this.FindGameButton.IsEnabled = !this.HasUnofficialModulesLoaded && isMatchmakingEnabled && !isSearchRequested && (isPartyLeader || !isInParty);
			this.FindGameButton.IsVisible = !this.HasUnofficialModulesLoaded && !isSearching;
			this.SelectionInfo.IsEnabled = !this.HasUnofficialModulesLoaded && isMatchmakingEnabled;
			this.SelectionInfo.IsVisible = !this.HasUnofficialModulesLoaded && !isSearching;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00019EA9 File Offset: 0x000180A9
		private void OnSubpageIndexChange()
		{
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x00019EAB File Offset: 0x000180AB
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x00019EB3 File Offset: 0x000180B3
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

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x00019ED7 File Offset: 0x000180D7
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x00019EDF File Offset: 0x000180DF
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

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x00019EFD File Offset: 0x000180FD
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x00019F05 File Offset: 0x00018105
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

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00019F23 File Offset: 0x00018123
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x00019F2B File Offset: 0x0001812B
		[Editor(false)]
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
					base.OnPropertyChanged(value, "HasUnofficialModulesLoaded");
				}
			}
		}

		// Token: 0x04000413 RID: 1043
		private ButtonWidget _findGameButton;

		// Token: 0x04000414 RID: 1044
		private Widget _selectionInfo;

		// Token: 0x04000415 RID: 1045
		private int _selectedModeIndex;

		// Token: 0x04000416 RID: 1046
		private bool _hasUnofficialModulesLoaded;
	}
}
