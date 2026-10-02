using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Friend
{
	// Token: 0x020000B2 RID: 178
	public class MultiplayerLobbyFriendsPanelWidget : Widget
	{
		// Token: 0x06000969 RID: 2409 RVA: 0x0001ABC0 File Offset: 0x00018DC0
		public MultiplayerLobbyFriendsPanelWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001ABC9 File Offset: 0x00018DC9
		private void OnShowListTogglePropertyChanged(PropertyOwnerObject owner, string propertyName, bool value)
		{
			if (propertyName == "IsSelected")
			{
				this.FriendsListPanel.IsVisible = this.ShowListToggle.IsSelected;
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001ABEE File Offset: 0x00018DEE
		private void IsForcedOpenUpdated()
		{
			this.FriendsListPanel.IsVisible = this.IsForcedOpen;
			this.ShowListToggle.IsSelected = this.IsForcedOpen;
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x0001AC12 File Offset: 0x00018E12
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x0001AC1A File Offset: 0x00018E1A
		[Editor(false)]
		public bool IsForcedOpen
		{
			get
			{
				return this._isForcedOpen;
			}
			set
			{
				if (this._isForcedOpen != value)
				{
					this._isForcedOpen = value;
					base.OnPropertyChanged(value, "IsForcedOpen");
					this.IsForcedOpenUpdated();
				}
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x0001AC3E File Offset: 0x00018E3E
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x0001AC46 File Offset: 0x00018E46
		[Editor(false)]
		public Widget FriendsListPanel
		{
			get
			{
				return this._friendsListPanel;
			}
			set
			{
				if (this._friendsListPanel != value)
				{
					this._friendsListPanel = value;
					base.OnPropertyChanged<Widget>(value, "FriendsListPanel");
				}
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x0001AC64 File Offset: 0x00018E64
		// (set) Token: 0x06000971 RID: 2417 RVA: 0x0001AC6C File Offset: 0x00018E6C
		[Editor(false)]
		public ToggleStateButtonWidget ShowListToggle
		{
			get
			{
				return this._showListToggle;
			}
			set
			{
				if (this._showListToggle != value)
				{
					if (this._showListToggle != null)
					{
						this._showListToggle.boolPropertyChanged -= this.OnShowListTogglePropertyChanged;
					}
					this._showListToggle = value;
					if (this._showListToggle != null)
					{
						this._showListToggle.boolPropertyChanged += this.OnShowListTogglePropertyChanged;
					}
					base.OnPropertyChanged<ToggleStateButtonWidget>(value, "ShowListToggle");
				}
			}
		}

		// Token: 0x0400043E RID: 1086
		private bool _isForcedOpen;

		// Token: 0x0400043F RID: 1087
		private Widget _friendsListPanel;

		// Token: 0x04000440 RID: 1088
		private ToggleStateButtonWidget _showListToggle;
	}
}
