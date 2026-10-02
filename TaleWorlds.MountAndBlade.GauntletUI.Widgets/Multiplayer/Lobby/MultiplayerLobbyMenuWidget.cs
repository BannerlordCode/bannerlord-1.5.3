using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A9 RID: 169
	public class MultiplayerLobbyMenuWidget : Widget
	{
		// Token: 0x060008FC RID: 2300 RVA: 0x00019D66 File Offset: 0x00017F66
		public MultiplayerLobbyMenuWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00019D6F File Offset: 0x00017F6F
		public void LobbyStateChanged(bool isSearchRequested, bool isSearching, bool isMatchmakingEnabled, bool isCustomBattleEnabled, bool isPartyLeader, bool isInParty)
		{
			this.MatchmakingButtonWidget.IsEnabled = isMatchmakingEnabled || isCustomBattleEnabled;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00019D80 File Offset: 0x00017F80
		private void SelectedItemIndexChanged()
		{
			if (this.MenuItemListPanel == null)
			{
				return;
			}
			this.MenuItemListPanel.IntValue = this.SelectedItemIndex - 3;
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00019D9E File Offset: 0x00017F9E
		// (set) Token: 0x06000900 RID: 2304 RVA: 0x00019DA6 File Offset: 0x00017FA6
		[Editor(false)]
		public int SelectedItemIndex
		{
			get
			{
				return this._selectedItemIndex;
			}
			set
			{
				if (this._selectedItemIndex != value)
				{
					this._selectedItemIndex = value;
					base.OnPropertyChanged(value, "SelectedItemIndex");
					this.SelectedItemIndexChanged();
				}
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x00019DCA File Offset: 0x00017FCA
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x00019DD2 File Offset: 0x00017FD2
		[Editor(false)]
		public ListPanel MenuItemListPanel
		{
			get
			{
				return this._menuItemListPanel;
			}
			set
			{
				if (this._menuItemListPanel != value)
				{
					this._menuItemListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "MenuItemListPanel");
					this.SelectedItemIndexChanged();
				}
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00019DF6 File Offset: 0x00017FF6
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x00019DFE File Offset: 0x00017FFE
		[Editor(false)]
		public ButtonWidget MatchmakingButtonWidget
		{
			get
			{
				return this._matchmakingButtonWidget;
			}
			set
			{
				if (this._matchmakingButtonWidget != value)
				{
					this._matchmakingButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "MatchmakingButtonWidget");
				}
			}
		}

		// Token: 0x04000410 RID: 1040
		private int _selectedItemIndex;

		// Token: 0x04000411 RID: 1041
		private ListPanel _menuItemListPanel;

		// Token: 0x04000412 RID: 1042
		private ButtonWidget _matchmakingButtonWidget;
	}
}
