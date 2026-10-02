using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C8 RID: 200
	public class MultiplayerTeamStatsSidePanel : ListPanel
	{
		// Token: 0x06000A9E RID: 2718 RVA: 0x0001DDF8 File Offset: 0x0001BFF8
		public MultiplayerTeamStatsSidePanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0001DE04 File Offset: 0x0001C004
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.ScrollablePanel == null || this.RowList == null)
			{
				return;
			}
			if (this.FollowedPlayerToken == this._handledFollowedPlayerToken)
			{
				return;
			}
			MultiplayerTeamStatsSideRowWidget multiplayerTeamStatsSideRowWidget = this.FindFollowedRow();
			if (multiplayerTeamStatsSideRowWidget == null)
			{
				return;
			}
			this._handledFollowedPlayerToken = this.FollowedPlayerToken;
			ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters(0f, 0f, 0f, 0f, -1f, 0.5f, 0.3f);
			this.ScrollablePanel.ScrollToChild(multiplayerTeamStatsSideRowWidget, autoScrollParameters);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0001DE88 File Offset: 0x0001C088
		private MultiplayerTeamStatsSideRowWidget FindFollowedRow()
		{
			for (int i = 0; i < this.RowList.ChildCount; i++)
			{
				MultiplayerTeamStatsSideRowWidget multiplayerTeamStatsSideRowWidget;
				if ((multiplayerTeamStatsSideRowWidget = this.RowList.GetChild(i) as MultiplayerTeamStatsSideRowWidget) != null && multiplayerTeamStatsSideRowWidget.IsFollowed)
				{
					return multiplayerTeamStatsSideRowWidget;
				}
			}
			return null;
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0001DECB File Offset: 0x0001C0CB
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x0001DED3 File Offset: 0x0001C0D3
		[Editor(false)]
		public int FollowedPlayerToken
		{
			get
			{
				return this._followedPlayerToken;
			}
			set
			{
				if (this._followedPlayerToken != value)
				{
					this._followedPlayerToken = value;
					base.OnPropertyChanged(value, "FollowedPlayerToken");
				}
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0001DEF1 File Offset: 0x0001C0F1
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x0001DEF9 File Offset: 0x0001C0F9
		[Editor(false)]
		public ScrollablePanel ScrollablePanel
		{
			get
			{
				return this._scrollablePanel;
			}
			set
			{
				if (this._scrollablePanel != value)
				{
					this._scrollablePanel = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "ScrollablePanel");
				}
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x0001DF17 File Offset: 0x0001C117
		// (set) Token: 0x06000AA6 RID: 2726 RVA: 0x0001DF1F File Offset: 0x0001C11F
		[Editor(false)]
		public Widget RowList
		{
			get
			{
				return this._rowList;
			}
			set
			{
				if (this._rowList != value)
				{
					this._rowList = value;
					base.OnPropertyChanged<Widget>(value, "RowList");
				}
			}
		}

		// Token: 0x040004D9 RID: 1241
		private int _handledFollowedPlayerToken;

		// Token: 0x040004DA RID: 1242
		private ScrollablePanel _scrollablePanel;

		// Token: 0x040004DB RID: 1243
		private Widget _rowList;

		// Token: 0x040004DC RID: 1244
		private int _followedPlayerToken;
	}
}
