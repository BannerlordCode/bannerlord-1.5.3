using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Friend
{
	// Token: 0x020000B0 RID: 176
	public class MultiplayerLobbyFriendGroupToggleWidget : ToggleButtonWidget
	{
		// Token: 0x06000951 RID: 2385 RVA: 0x0001A8ED File Offset: 0x00018AED
		public MultiplayerLobbyFriendGroupToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0001A8F6 File Offset: 0x00018AF6
		protected override void OnClick(Widget widget)
		{
			base.OnClick(widget);
			this.UpdateCollapseIndicator();
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0001A905 File Offset: 0x00018B05
		protected override void RefreshState()
		{
			base.RefreshState();
			Widget titleContainer = this.TitleContainer;
			if (titleContainer == null)
			{
				return;
			}
			titleContainer.SetState(base.CurrentState);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0001A923 File Offset: 0x00018B23
		private void CollapseIndicatorUpdated()
		{
			this.CollapseIndicator.AddState("Collapsed");
			this.CollapseIndicator.AddState("Expanded");
			this.UpdateCollapseIndicator();
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0001A94B File Offset: 0x00018B4B
		private void UpdateCollapseIndicator()
		{
			if (base.WidgetToClose != null && this.CollapseIndicator != null)
			{
				if (base.WidgetToClose.IsVisible)
				{
					this.CollapseIndicator.SetState("Expanded");
					return;
				}
				this.CollapseIndicator.SetState("Collapsed");
			}
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0001A98B File Offset: 0x00018B8B
		private void PlayerCountUpdated()
		{
			if (this.PlayerCountText == null)
			{
				return;
			}
			this.PlayerCountText.Text = "(" + this.PlayerCount + ")";
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0001A9BB File Offset: 0x00018BBB
		private void InitialClosedStateUpdated()
		{
			base.IsSelected = !this.InitialClosedState;
			this.CollapseIndicatorUpdated();
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x0001A9D2 File Offset: 0x00018BD2
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x0001A9DA File Offset: 0x00018BDA
		[Editor(false)]
		public Widget CollapseIndicator
		{
			get
			{
				return this._collapseIndicator;
			}
			set
			{
				if (this._collapseIndicator != value)
				{
					this._collapseIndicator = value;
					base.OnPropertyChanged<Widget>(value, "CollapseIndicator");
					this.CollapseIndicatorUpdated();
				}
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x0001A9FE File Offset: 0x00018BFE
		// (set) Token: 0x0600095B RID: 2395 RVA: 0x0001AA06 File Offset: 0x00018C06
		[Editor(false)]
		public Widget TitleContainer
		{
			get
			{
				return this._titleContainer;
			}
			set
			{
				if (this._titleContainer != value)
				{
					this._titleContainer = value;
					base.OnPropertyChanged<Widget>(value, "TitleContainer");
				}
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0001AA24 File Offset: 0x00018C24
		// (set) Token: 0x0600095D RID: 2397 RVA: 0x0001AA2C File Offset: 0x00018C2C
		[Editor(false)]
		public TextWidget PlayerCountText
		{
			get
			{
				return this._playerCountText;
			}
			set
			{
				if (this._playerCountText != value)
				{
					this._playerCountText = value;
					base.OnPropertyChanged<TextWidget>(value, "PlayerCountText");
				}
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0001AA4A File Offset: 0x00018C4A
		// (set) Token: 0x0600095F RID: 2399 RVA: 0x0001AA52 File Offset: 0x00018C52
		[Editor(false)]
		public int PlayerCount
		{
			get
			{
				return this._playerCount;
			}
			set
			{
				if (this._playerCount != value)
				{
					this._playerCount = value;
					base.OnPropertyChanged(value, "PlayerCount");
					this.PlayerCountUpdated();
				}
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0001AA76 File Offset: 0x00018C76
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x0001AA7E File Offset: 0x00018C7E
		[Editor(false)]
		public bool InitialClosedState
		{
			get
			{
				return this._initialClosedState;
			}
			set
			{
				if (this._initialClosedState != value)
				{
					this._initialClosedState = value;
					base.OnPropertyChanged(value, "InitialClosedState");
					this.InitialClosedStateUpdated();
				}
			}
		}

		// Token: 0x04000437 RID: 1079
		private Widget _collapseIndicator;

		// Token: 0x04000438 RID: 1080
		private Widget _titleContainer;

		// Token: 0x04000439 RID: 1081
		private TextWidget _playerCountText;

		// Token: 0x0400043A RID: 1082
		private int _playerCount;

		// Token: 0x0400043B RID: 1083
		private bool _initialClosedState;
	}
}
