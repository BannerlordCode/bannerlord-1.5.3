using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000075 RID: 117
	public class MPLobbyClanVM : ViewModel
	{
		// Token: 0x06000BA3 RID: 2979 RVA: 0x00023121 File Offset: 0x00021321
		public MPLobbyClanVM(Action openInviteClanMemberPopup)
		{
			this.ClanOverview = new MPLobbyClanOverviewVM(openInviteClanMemberPopup);
			this.ClanRoster = new MPLobbyClanRosterVM();
			this._activeNotifications = new List<LobbyNotification>();
			this.TrySetClanSubPage(MPLobbyClanVM.ClanSubPages.Overview);
			this.RefreshValues();
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00023158 File Offset: 0x00021358
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.ClanOverview.RefreshValues();
			this.ClanRoster.RefreshValues();
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002318C File Offset: 0x0002138C
		private void OnIsEnabledChanged()
		{
			if (this.IsEnabled)
			{
				this.TrySetClanSubPage(MPLobbyClanVM.ClanSubPages.Overview);
				foreach (LobbyNotification lobbyNotification in this._activeNotifications)
				{
					NetworkMain.GameClient.MarkNotificationAsRead(lobbyNotification.Id);
				}
				this._activeNotifications.Clear();
			}
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00023204 File Offset: 0x00021404
		public async void OnClanInfoChanged()
		{
			ClanHomeInfo clanHomeInfo = NetworkMain.GameClient.ClanHomeInfo;
			if (clanHomeInfo == null)
			{
				Debug.FailedAssert("Retrieved clan home info is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Clan\\MPLobbyClanVM.cs", "OnClanInfoChanged", 65);
			}
			else
			{
				await this.ClanOverview.RefreshClanInformation(clanHomeInfo);
				this.ClanRoster.RefreshClanInformation(clanHomeInfo);
				if (!clanHomeInfo.IsInClan)
				{
					this.ExecuteClosePopup();
				}
			}
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0002323D File Offset: 0x0002143D
		private void ExecuteChangeEnabledSubPage(int subpageIndex)
		{
			this.TrySetClanSubPage((MPLobbyClanVM.ClanSubPages)subpageIndex);
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00023248 File Offset: 0x00021448
		public async void TrySetClanSubPage(MPLobbyClanVM.ClanSubPages newPage)
		{
			this.ClanOverview.IsSelected = false;
			this.ClanRoster.IsSelected = false;
			this._currentSubPage = newPage;
			this.SelectedSubPageIndex = (int)newPage;
			if (newPage == MPLobbyClanVM.ClanSubPages.Overview)
			{
				this.ClanOverview.IsSelected = true;
				await this.ClanOverview.RefreshClanInformation(NetworkMain.GameClient.ClanHomeInfo);
			}
			else if (newPage == MPLobbyClanVM.ClanSubPages.Roster)
			{
				this.ClanRoster.IsSelected = true;
				this.ClanRoster.RefreshClanInformation(NetworkMain.GameClient.ClanHomeInfo);
			}
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00023289 File Offset: 0x00021489
		public void OnNotificationReceived(LobbyNotification notification)
		{
			if (this.IsEnabled)
			{
				NetworkMain.GameClient.MarkNotificationAsRead(notification.Id);
				return;
			}
			this._activeNotifications.Add(notification);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x000232B0 File Offset: 0x000214B0
		public void OnPlayerNameUpdated(string playerName)
		{
			MPLobbyClanRosterVM clanRoster = this.ClanRoster;
			if (clanRoster == null)
			{
				return;
			}
			clanRoster.OnPlayerNameUpdated(playerName);
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x000232C3 File Offset: 0x000214C3
		public void ExecuteOpenPopup()
		{
			this.IsEnabled = true;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x000232CC File Offset: 0x000214CC
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x000232D5 File Offset: 0x000214D5
		// (set) Token: 0x06000BAE RID: 2990 RVA: 0x000232DD File Offset: 0x000214DD
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
					this.OnIsEnabledChanged();
				}
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00023300 File Offset: 0x00021500
		// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x00023308 File Offset: 0x00021508
		[DataSourceProperty]
		public int SelectedSubPageIndex
		{
			get
			{
				return this._selectedSubPageIndex;
			}
			set
			{
				if (value != this._selectedSubPageIndex)
				{
					this._selectedSubPageIndex = value;
					base.OnPropertyChanged("SelectedSubPageIndex");
				}
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00023325 File Offset: 0x00021525
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x0002332D File Offset: 0x0002152D
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
					base.OnPropertyChangedWithValue<string>(value, "CloseText");
				}
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x00023350 File Offset: 0x00021550
		// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x00023358 File Offset: 0x00021558
		[DataSourceProperty]
		public MPLobbyClanOverviewVM ClanOverview
		{
			get
			{
				return this._clanOverview;
			}
			set
			{
				if (value != this._clanOverview)
				{
					this._clanOverview = value;
					base.OnPropertyChanged("ClanOverview");
				}
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x00023375 File Offset: 0x00021575
		// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x0002337D File Offset: 0x0002157D
		[DataSourceProperty]
		public MPLobbyClanRosterVM ClanRoster
		{
			get
			{
				return this._clanRoster;
			}
			set
			{
				if (value != this._clanRoster)
				{
					this._clanRoster = value;
					base.OnPropertyChanged("ClanRoster");
				}
			}
		}

		// Token: 0x04000544 RID: 1348
		private MPLobbyClanVM.ClanSubPages _currentSubPage;

		// Token: 0x04000545 RID: 1349
		private List<LobbyNotification> _activeNotifications;

		// Token: 0x04000546 RID: 1350
		private bool _isEnabled;

		// Token: 0x04000547 RID: 1351
		private int _selectedSubPageIndex;

		// Token: 0x04000548 RID: 1352
		private string _closeText;

		// Token: 0x04000549 RID: 1353
		private MPLobbyClanOverviewVM _clanOverview;

		// Token: 0x0400054A RID: 1354
		private MPLobbyClanRosterVM _clanRoster;

		// Token: 0x02000167 RID: 359
		public enum ClanSubPages
		{
			// Token: 0x04000A43 RID: 2627
			Overview,
			// Token: 0x04000A44 RID: 2628
			Roster
		}
	}
}
