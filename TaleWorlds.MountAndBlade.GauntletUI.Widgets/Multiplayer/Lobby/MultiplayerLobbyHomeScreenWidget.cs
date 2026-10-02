using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A7 RID: 167
	public class MultiplayerLobbyHomeScreenWidget : Widget
	{
		// Token: 0x060008EB RID: 2283 RVA: 0x00019B8F File Offset: 0x00017D8F
		public MultiplayerLobbyHomeScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00019B98 File Offset: 0x00017D98
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				if (base.IsVisible)
				{
					base.OnPropertyChanged(true, "IsVisible");
				}
				this._initialized = true;
			}
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00019BC4 File Offset: 0x00017DC4
		public void LobbyStateChanged(bool isSearchRequested, bool isSearching, bool isMatchmakingEnabled, bool isCustomBattleEnabled, bool isPartyLeader, bool isInParty)
		{
			this.FindGameButton.IsEnabled = !this.HasUnofficialModulesLoaded && isMatchmakingEnabled && !isSearchRequested && (isPartyLeader || !isInParty);
			this.FindGameButton.IsVisible = !this.HasUnofficialModulesLoaded && !isSearching;
			this.SelectionInfo.IsEnabled = !this.HasUnofficialModulesLoaded && isMatchmakingEnabled;
			this.SelectionInfo.IsVisible = !this.HasUnofficialModulesLoaded && !isSearching;
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x00019C45 File Offset: 0x00017E45
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x00019C4D File Offset: 0x00017E4D
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

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00019C6B File Offset: 0x00017E6B
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x00019C73 File Offset: 0x00017E73
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

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00019C91 File Offset: 0x00017E91
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x00019C99 File Offset: 0x00017E99
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

		// Token: 0x04000409 RID: 1033
		private bool _initialized;

		// Token: 0x0400040A RID: 1034
		private ButtonWidget _findGameButton;

		// Token: 0x0400040B RID: 1035
		private Widget _selectionInfo;

		// Token: 0x0400040C RID: 1036
		private bool _hasUnofficialModulesLoaded;
	}
}
