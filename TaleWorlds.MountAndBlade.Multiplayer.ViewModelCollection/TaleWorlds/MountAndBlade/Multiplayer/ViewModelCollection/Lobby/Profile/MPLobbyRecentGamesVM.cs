using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000040 RID: 64
	public class MPLobbyRecentGamesVM : ViewModel
	{
		// Token: 0x06000612 RID: 1554 RVA: 0x00014072 File Offset: 0x00012272
		public MPLobbyRecentGamesVM()
		{
			this._games = new MBBindingList<MPLobbyRecentGameItemVM>();
			this.PlayerActions = new MBBindingList<StringPairItemWithActionVM>();
			this.NoRecentGamesFoundText = new TextObject("{=TzYWE9tA}No Recent Games Found", null).ToString();
			this.RefreshValues();
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000140AC File Offset: 0x000122AC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RecentGamesText = new TextObject("{=NJolh9ye}Recent Games", null).ToString();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.Games.ApplyActionOnAllItems(delegate(MPLobbyRecentGameItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00014118 File Offset: 0x00012318
		public void RefreshData(MBReadOnlyList<MatchHistoryData> matches)
		{
			this.Games.Clear();
			if (matches != null)
			{
				foreach (MatchHistoryData matchHistoryData in matches.OrderByDescending<MatchHistoryData, DateTime>((MatchHistoryData m) => m.MatchDate))
				{
					if (matchHistoryData != null)
					{
						MPLobbyRecentGameItemVM mplobbyRecentGameItemVM = new MPLobbyRecentGameItemVM(new Action<MPLobbyRecentGamePlayerItemVM>(this.ActivatePlayerActions));
						mplobbyRecentGameItemVM.FillFrom(matchHistoryData);
						this.Games.Add(mplobbyRecentGameItemVM);
					}
				}
			}
			this.GotItems = matches.Count > 0;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x000141C4 File Offset: 0x000123C4
		public void ActivatePlayerActions(MPLobbyRecentGamePlayerItemVM playerVM)
		{
			this.PlayerActions.Clear();
			this._currentMatchOfTheActivePlayer = playerVM.MatchOfThePlayer;
			if (playerVM.ProvidedID != NetworkMain.GameClient.PlayerID)
			{
				StringPairItemWithActionVM stringPairItemWithActionVM = new StringPairItemWithActionVM(new Action<object>(this.ExecuteReport), GameTexts.FindText("str_mp_scoreboard_context_report", null).ToString(), "Report", playerVM);
				if (MultiplayerReportPlayerManager.IsPlayerReportedOverLimit(playerVM.ProvidedID))
				{
					stringPairItemWithActionVM.IsEnabled = false;
					stringPairItemWithActionVM.Hint.HintText = new TextObject("{=klkYFik9}You've already reported this player.", null);
				}
				this.PlayerActions.Add(stringPairItemWithActionVM);
				bool flag = false;
				FriendInfo[] friendInfos = NetworkMain.GameClient.FriendInfos;
				for (int i = 0; i < friendInfos.Length; i++)
				{
					if (friendInfos[i].Id == playerVM.ProvidedID)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteRequestFriendship), new TextObject("{=UwkpJq9N}Add As Friend", null).ToString(), "RequestFriendship", playerVM));
				}
				else
				{
					this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteTerminateFriendship), new TextObject("{=2YIVRuRa}Remove From Friends", null).ToString(), "TerminateFriendship", playerVM));
				}
				MultiplayerPlayerContextMenuHelper.AddLobbyViewProfileOptions(playerVM, this.PlayerActions);
			}
			this.IsPlayerActionsActive = false;
			this.IsPlayerActionsActive = this.PlayerActions.Count > 0;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00014324 File Offset: 0x00012524
		private void ExecuteRequestFriendship(object playerObj)
		{
			MPLobbyRecentGamePlayerItemVM mplobbyRecentGamePlayerItemVM = playerObj as MPLobbyRecentGamePlayerItemVM;
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(mplobbyRecentGamePlayerItemVM.ProvidedID);
			NetworkMain.GameClient.AddFriend(mplobbyRecentGamePlayerItemVM.ProvidedID, flag);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00014368 File Offset: 0x00012568
		private void ExecuteTerminateFriendship(object memberObj)
		{
			MPLobbyRecentGamePlayerItemVM mplobbyRecentGamePlayerItemVM = memberObj as MPLobbyRecentGamePlayerItemVM;
			NetworkMain.GameClient.RemoveFriend(mplobbyRecentGamePlayerItemVM.ProvidedID);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0001438C File Offset: 0x0001258C
		private void ExecuteReport(object playerObj)
		{
			MPLobbyRecentGamePlayerItemVM mplobbyRecentGamePlayerItemVM = playerObj as MPLobbyRecentGamePlayerItemVM;
			MultiplayerReportPlayerManager.RequestReportPlayer(this._currentMatchOfTheActivePlayer.MatchId, mplobbyRecentGamePlayerItemVM.ProvidedID, mplobbyRecentGamePlayerItemVM.Name, false);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x000143BD File Offset: 0x000125BD
		public void ExecuteOpenPopup()
		{
			this.IsEnabled = true;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x000143C6 File Offset: 0x000125C6
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000143D0 File Offset: 0x000125D0
		public void OnFriendListUpdated(bool forceUpdate = false)
		{
			foreach (MPLobbyRecentGameItemVM mplobbyRecentGameItemVM in this.Games)
			{
				mplobbyRecentGameItemVM.OnFriendListUpdated(forceUpdate);
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0001441C File Offset: 0x0001261C
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x00014424 File Offset: 0x00012624
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
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00014442 File Offset: 0x00012642
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x0001444A File Offset: 0x0001264A
		[DataSourceProperty]
		public bool GotItems
		{
			get
			{
				return this._gotItems;
			}
			set
			{
				if (value != this._gotItems)
				{
					this._gotItems = value;
					base.OnPropertyChangedWithValue(value, "GotItems");
				}
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00014468 File Offset: 0x00012668
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x00014470 File Offset: 0x00012670
		[DataSourceProperty]
		public bool IsPlayerActionsActive
		{
			get
			{
				return this._isPlayerActionsActive;
			}
			set
			{
				if (value != this._isPlayerActionsActive)
				{
					this._isPlayerActionsActive = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerActionsActive");
				}
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0001448E File Offset: 0x0001268E
		// (set) Token: 0x06000623 RID: 1571 RVA: 0x00014496 File Offset: 0x00012696
		[DataSourceProperty]
		public string RecentGamesText
		{
			get
			{
				return this._recentGamesText;
			}
			set
			{
				if (value != this._recentGamesText)
				{
					this._recentGamesText = value;
					base.OnPropertyChangedWithValue<string>(value, "RecentGamesText");
				}
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x000144B9 File Offset: 0x000126B9
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x000144C1 File Offset: 0x000126C1
		[DataSourceProperty]
		public string NoRecentGamesFoundText
		{
			get
			{
				return this._noRecentGamesFoundText;
			}
			set
			{
				if (value != this._noRecentGamesFoundText)
				{
					this._noRecentGamesFoundText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoRecentGamesFoundText");
				}
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x000144E4 File Offset: 0x000126E4
		// (set) Token: 0x06000627 RID: 1575 RVA: 0x000144EC File Offset: 0x000126EC
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

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0001450F File Offset: 0x0001270F
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x00014517 File Offset: 0x00012717
		[DataSourceProperty]
		public MBBindingList<StringPairItemWithActionVM> PlayerActions
		{
			get
			{
				return this._playerActions;
			}
			set
			{
				if (value != this._playerActions)
				{
					this._playerActions = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemWithActionVM>>(value, "PlayerActions");
				}
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00014535 File Offset: 0x00012735
		// (set) Token: 0x0600062B RID: 1579 RVA: 0x0001453D File Offset: 0x0001273D
		[DataSourceProperty]
		public MBBindingList<MPLobbyRecentGameItemVM> Games
		{
			get
			{
				return this._games;
			}
			set
			{
				if (value != this._games)
				{
					this._games = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyRecentGameItemVM>>(value, "Games");
				}
			}
		}

		// Token: 0x040002E0 RID: 736
		private MatchHistoryData _currentMatchOfTheActivePlayer;

		// Token: 0x040002E1 RID: 737
		private bool _isEnabled;

		// Token: 0x040002E2 RID: 738
		private bool _gotItems;

		// Token: 0x040002E3 RID: 739
		private bool _isPlayerActionsActive;

		// Token: 0x040002E4 RID: 740
		private string _recentGamesText;

		// Token: 0x040002E5 RID: 741
		private string _noRecentGamesFoundText;

		// Token: 0x040002E6 RID: 742
		private string _closeText;

		// Token: 0x040002E7 RID: 743
		private MBBindingList<StringPairItemWithActionVM> _playerActions;

		// Token: 0x040002E8 RID: 744
		private MBBindingList<MPLobbyRecentGameItemVM> _games;
	}
}
