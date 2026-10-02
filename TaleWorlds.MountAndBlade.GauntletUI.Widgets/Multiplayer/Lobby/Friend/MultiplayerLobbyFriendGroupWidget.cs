using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Friend
{
	// Token: 0x020000B1 RID: 177
	public class MultiplayerLobbyFriendGroupWidget : Widget
	{
		// Token: 0x06000962 RID: 2402 RVA: 0x0001AAA2 File Offset: 0x00018CA2
		public MultiplayerLobbyFriendGroupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0001AAAB File Offset: 0x00018CAB
		private void FriendCountChanged(Widget widget)
		{
			this.Toggle.PlayerCount = this.List.ChildCount;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0001AAC3 File Offset: 0x00018CC3
		private void FriendCountChanged(Widget parentWidget, Widget addedWidget)
		{
			this.Toggle.PlayerCount = this.List.ChildCount;
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x0001AADB File Offset: 0x00018CDB
		// (set) Token: 0x06000966 RID: 2406 RVA: 0x0001AAE4 File Offset: 0x00018CE4
		[Editor(false)]
		public ListPanel List
		{
			get
			{
				return this._list;
			}
			set
			{
				if (this._list != value)
				{
					ListPanel list = this._list;
					if (list != null)
					{
						list.ItemAddEventHandlers.Remove(new Action<Widget, Widget>(this.FriendCountChanged));
					}
					ListPanel list2 = this._list;
					if (list2 != null)
					{
						list2.ItemAfterRemoveEventHandlers.Remove(new Action<Widget>(this.FriendCountChanged));
					}
					this._list = value;
					ListPanel list3 = this._list;
					if (list3 != null)
					{
						list3.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.FriendCountChanged));
					}
					ListPanel list4 = this._list;
					if (list4 != null)
					{
						list4.ItemAfterRemoveEventHandlers.Add(new Action<Widget>(this.FriendCountChanged));
					}
					base.OnPropertyChanged<ListPanel>(value, "List");
				}
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0001AB9A File Offset: 0x00018D9A
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x0001ABA2 File Offset: 0x00018DA2
		[Editor(false)]
		public MultiplayerLobbyFriendGroupToggleWidget Toggle
		{
			get
			{
				return this._toggle;
			}
			set
			{
				if (this._toggle != value)
				{
					this._toggle = value;
					base.OnPropertyChanged<MultiplayerLobbyFriendGroupToggleWidget>(value, "Toggle");
				}
			}
		}

		// Token: 0x0400043C RID: 1084
		private ListPanel _list;

		// Token: 0x0400043D RID: 1085
		private MultiplayerLobbyFriendGroupToggleWidget _toggle;
	}
}
