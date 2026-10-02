using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006C RID: 108
	public class MPLobbyClanInviteFriendsPopupVM : ViewModel
	{
		// Token: 0x06000A9A RID: 2714 RVA: 0x00020AB8 File Offset: 0x0001ECB8
		public MPLobbyClanInviteFriendsPopupVM(Func<MBBindingList<MPLobbyPlayerBaseVM>> getAllFriends)
		{
			this._getAllFriends = getAllFriends;
			this.OnlineFriends = new MBBindingList<MPLobbyPlayerBaseVM>();
			this.RefreshValues();
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00020AD8 File Offset: 0x0001ECD8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=v4hVLpap}Invite Players to Clan", null).ToString();
			this.InviteText = new TextObject("{=aZnS9ECC}Invite", null).ToString();
			this.CloseText = new TextObject("{=yQtzabbe}Close", null).ToString();
			this.SelectPlayersText = new TextObject("{=ZAejS7WF}Select players to invite to your clan", null).ToString();
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00020B44 File Offset: 0x0001ED44
		public void Open()
		{
			if (NetworkMain.GameClient.ClanID == Guid.Empty || NetworkMain.GameClient.ClanInfo == null)
			{
				return;
			}
			IEnumerable<PlayerId> enumerable = NetworkMain.GameClient.ClanInfo.Players.Select<ClanPlayer, PlayerId>((ClanPlayer c) => c.PlayerId);
			this.OnlineFriends.Clear();
			using (IEnumerator<MPLobbyPlayerBaseVM> enumerator = this._getAllFriends().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MPLobbyPlayerBaseVM onlineFriend = enumerator.Current;
					if (!enumerable.Contains(onlineFriend.ProvidedID) && !this.OnlineFriends.Any<MPLobbyPlayerBaseVM>((MPLobbyPlayerBaseVM f) => f.ProvidedID == onlineFriend.ProvidedID))
					{
						this.OnlineFriends.Add(onlineFriend);
					}
				}
			}
			this.IsEnabled = true;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00020C40 File Offset: 0x0001EE40
		private void ExecuteSendInvitation()
		{
			foreach (MPLobbyPlayerBaseVM mplobbyPlayerBaseVM in this.OnlineFriends)
			{
				if (mplobbyPlayerBaseVM.IsSelected)
				{
					mplobbyPlayerBaseVM.ExecuteInviteToClan();
				}
			}
			this.ExecuteClosePopup();
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00020C9C File Offset: 0x0001EE9C
		private void ResetSelection()
		{
			foreach (MPLobbyPlayerBaseVM mplobbyPlayerBaseVM in this.OnlineFriends)
			{
				mplobbyPlayerBaseVM.IsSelected = false;
			}
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00020CE8 File Offset: 0x0001EEE8
		public void ExecuteClosePopup()
		{
			if (this.IsEnabled)
			{
				this.ResetSelection();
				this.IsEnabled = false;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x00020CFF File Offset: 0x0001EEFF
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x00020D07 File Offset: 0x0001EF07
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
					base.OnPropertyChanged("IsEnabled");
				}
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x00020D24 File Offset: 0x0001EF24
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x00020D2C File Offset: 0x0001EF2C
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChanged("TitleText");
				}
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x00020D4E File Offset: 0x0001EF4E
		// (set) Token: 0x06000AA5 RID: 2725 RVA: 0x00020D56 File Offset: 0x0001EF56
		[DataSourceProperty]
		public string InviteText
		{
			get
			{
				return this._inviteText;
			}
			set
			{
				if (value != this._inviteText)
				{
					this._inviteText = value;
					base.OnPropertyChanged("InviteText");
				}
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x00020D78 File Offset: 0x0001EF78
		// (set) Token: 0x06000AA7 RID: 2727 RVA: 0x00020D80 File Offset: 0x0001EF80
		[DataSourceProperty]
		public string CloseText
		{
			get
			{
				return this._closeText;
			}
			set
			{
				if (value != this._closeText)
				{
					this._closeText = value;
					base.OnPropertyChanged("CloseText");
				}
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00020DA2 File Offset: 0x0001EFA2
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x00020DAA File Offset: 0x0001EFAA
		[DataSourceProperty]
		public string SelectPlayersText
		{
			get
			{
				return this._selectPlayersText;
			}
			set
			{
				if (value != this._selectPlayersText)
				{
					this._selectPlayersText = value;
					base.OnPropertyChanged("SelectPlayersText");
				}
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x00020DCC File Offset: 0x0001EFCC
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x00020DD4 File Offset: 0x0001EFD4
		[DataSourceProperty]
		public MBBindingList<MPLobbyPlayerBaseVM> OnlineFriends
		{
			get
			{
				return this._onlineFriends;
			}
			set
			{
				if (value != this._onlineFriends)
				{
					this._onlineFriends = value;
					base.OnPropertyChanged("OnlineFriends");
				}
			}
		}

		// Token: 0x040004D1 RID: 1233
		private Func<MBBindingList<MPLobbyPlayerBaseVM>> _getAllFriends;

		// Token: 0x040004D2 RID: 1234
		private bool _isEnabled;

		// Token: 0x040004D3 RID: 1235
		private string _titleText;

		// Token: 0x040004D4 RID: 1236
		private string _inviteText;

		// Token: 0x040004D5 RID: 1237
		private string _closeText;

		// Token: 0x040004D6 RID: 1238
		private string _selectPlayersText;

		// Token: 0x040004D7 RID: 1239
		private MBBindingList<MPLobbyPlayerBaseVM> _onlineFriends;
	}
}
