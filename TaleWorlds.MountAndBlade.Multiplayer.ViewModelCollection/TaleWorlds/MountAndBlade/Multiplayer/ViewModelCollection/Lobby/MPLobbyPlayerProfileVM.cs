using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x0200002F RID: 47
	public class MPLobbyPlayerProfileVM : ViewModel
	{
		// Token: 0x0600037B RID: 891 RVA: 0x0000CB04 File Offset: 0x0000AD04
		public MPLobbyPlayerProfileVM(LobbyState lobbyState)
		{
			this._lobbyState = lobbyState;
			this.Player = new MPLobbyPlayerBaseVM(PlayerId.Empty, "", null, null);
			MPLobbyPlayerBaseVM player = this.Player;
			player.OnRankInfoChanged = (Action<string>)Delegate.Combine(player.OnRankInfoChanged, new Action<string>(this.OnPlayerRankInfoChanged));
			this.RefreshValues();
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000CB64 File Offset: 0x0000AD64
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.StatsTitleText = new TextObject("{=GmU1to3Y}Statistics", null).ToString();
			MPLobbyPlayerBaseVM player = this.Player;
			if (player == null)
			{
				return;
			}
			player.RefreshValues();
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000CBB3 File Offset: 0x0000ADB3
		public override void OnFinalize()
		{
			base.OnFinalize();
			MPLobbyPlayerBaseVM player = this.Player;
			player.OnRankInfoChanged = (Action<string>)Delegate.Remove(player.OnRankInfoChanged, new Action<string>(this.OnPlayerRankInfoChanged));
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000CBE4 File Offset: 0x0000ADE4
		public async void SetPlayerID(PlayerId playerID)
		{
			this.IsEnabled = true;
			this.IsDataLoading = true;
			this._activePlayerID = playerID;
			PlayerData playerData = await NetworkMain.GameClient.GetAnotherPlayerData(playerID);
			this._activePlayerData = playerData;
			if (this._activePlayerData != null)
			{
				IPlatformServices instance = PlatformServices.Instance;
				if (instance != null)
				{
					instance.CheckPrivilege(Privilege.Chat, true, delegate(bool result)
					{
						if (!result)
						{
							PlatformServices.Instance.ShowRestrictedInformation();
						}
					});
				}
				PlatformServices.Instance.CheckPermissionWithUser(Permission.ViewUserGeneratedContent, this._activePlayerID, delegate(bool hasPermission)
				{
					this.Player.IsBannerlordIDSupported = hasPermission;
				});
				await this._lobbyState.UpdateHasUserGeneratedContentPrivilege(true);
				this.Player.UpdateWith(this._activePlayerData);
				if (NetworkMain.GameClient.SupportedFeatures.SupportsFeatures(Features.Clan))
				{
					this.Player.UpdateClanInfo();
				}
				this.Player.RefreshCharacterVisual();
				this.Player.UpdateStats(new Action(this.OnStatsReceived));
				this.Player.UpdateRating(new Action(this.OnRatingReceived));
			}
			else
			{
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=bhQiSzOU}Profile is not available", null).ToString(), new TextObject("{=goQ0MZhr}This player does not have an active Bannerlord player profile.", null).ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), null, new Action(this.ExecuteClosePopup), null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000CC28 File Offset: 0x0000AE28
		public void OpenWith(PlayerId playerID)
		{
			PlatformServices.Instance.CheckPermissionWithUser(Permission.ViewUserGeneratedContent, playerID, delegate(bool hasBannerlordIDPrivilege)
			{
				this.Player.IsBannerlordIDSupported = hasBannerlordIDPrivilege;
				this.SetPlayerID(playerID);
			});
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000CC68 File Offset: 0x0000AE68
		public void UpdatePlayerData(PlayerData playerData, bool updateStatistics = false, bool updateRating = false)
		{
			this.IsDataLoading = true;
			this._activePlayerID = playerData.PlayerId;
			MPLobbyPlayerBaseVM player = this.Player;
			if (player != null)
			{
				player.UpdateWith(playerData);
			}
			if (updateStatistics)
			{
				this.Player.UpdateStats(new Action(this.OnStatsReceived));
			}
			if (updateRating)
			{
				this.Player.UpdateRating(new Action(this.OnRatingReceived));
			}
			this.Player.UpdateExperienceData();
			this.Player.RefreshSelectableGameTypes(false, new Action<string>(this.Player.UpdateDisplayedRankInfo), "");
			this.IsDataLoading = false;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000CD02 File Offset: 0x0000AF02
		private void OnPlayerRankInfoChanged(string gameType)
		{
			this.Player.FilterStatsForGameMode(gameType);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000CD10 File Offset: 0x0000AF10
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
			this.IsDataLoading = false;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000CD20 File Offset: 0x0000AF20
		public void OnClanInfoChanged()
		{
			if (this.Player.ProvidedID == NetworkMain.GameClient.PlayerID)
			{
				this.Player.UpdateClanInfo();
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000CD49 File Offset: 0x0000AF49
		private void OnStatsReceived()
		{
			this._isStatsReceived = true;
			this.CheckAndUpdateStatsAndRatingData();
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000CD58 File Offset: 0x0000AF58
		private void OnRatingReceived()
		{
			this._isRatingReceived = true;
			this.CheckAndUpdateStatsAndRatingData();
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000CD67 File Offset: 0x0000AF67
		public void OnPlayerNameUpdated(string playerName)
		{
			MPLobbyPlayerBaseVM player = this.Player;
			if (player == null)
			{
				return;
			}
			player.UpdateNameAndAvatar(true);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000CD7C File Offset: 0x0000AF7C
		private void CheckAndUpdateStatsAndRatingData()
		{
			if (this._isRatingReceived && this._isStatsReceived)
			{
				this.Player.UpdateExperienceData();
				this.Player.RefreshSelectableGameTypes(false, new Action<string>(this.Player.UpdateDisplayedRankInfo), "");
				this.IsDataLoading = false;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0000CDCD File Offset: 0x0000AFCD
		// (set) Token: 0x06000389 RID: 905 RVA: 0x0000CDD5 File Offset: 0x0000AFD5
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

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000CDF3 File Offset: 0x0000AFF3
		// (set) Token: 0x0600038B RID: 907 RVA: 0x0000CDFB File Offset: 0x0000AFFB
		[DataSourceProperty]
		public bool IsDataLoading
		{
			get
			{
				return this._isDataLoading;
			}
			set
			{
				if (value != this._isDataLoading)
				{
					this._isDataLoading = value;
					base.OnPropertyChangedWithValue(value, "IsDataLoading");
				}
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0000CE19 File Offset: 0x0000B019
		// (set) Token: 0x0600038D RID: 909 RVA: 0x0000CE21 File Offset: 0x0000B021
		[DataSourceProperty]
		public string StatsTitleText
		{
			get
			{
				return this._statsTitleText;
			}
			set
			{
				if (value != this._statsTitleText)
				{
					this._statsTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "StatsTitleText");
				}
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000CE44 File Offset: 0x0000B044
		// (set) Token: 0x0600038F RID: 911 RVA: 0x0000CE4C File Offset: 0x0000B04C
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

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000CE6F File Offset: 0x0000B06F
		// (set) Token: 0x06000391 RID: 913 RVA: 0x0000CE77 File Offset: 0x0000B077
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM Player
		{
			get
			{
				return this._player;
			}
			set
			{
				if (value != this._player)
				{
					this._player = value;
					base.OnPropertyChangedWithValue<MPLobbyPlayerBaseVM>(value, "Player");
				}
			}
		}

		// Token: 0x040001C3 RID: 451
		private readonly LobbyState _lobbyState;

		// Token: 0x040001C4 RID: 452
		private PlayerId _activePlayerID;

		// Token: 0x040001C5 RID: 453
		private PlayerData _activePlayerData;

		// Token: 0x040001C6 RID: 454
		private bool _isStatsReceived;

		// Token: 0x040001C7 RID: 455
		private bool _isRatingReceived;

		// Token: 0x040001C8 RID: 456
		private bool _isEnabled;

		// Token: 0x040001C9 RID: 457
		private bool _isDataLoading;

		// Token: 0x040001CA RID: 458
		private string _statsTitleText;

		// Token: 0x040001CB RID: 459
		private string _closeText;

		// Token: 0x040001CC RID: 460
		private MPLobbyPlayerBaseVM _player;
	}
}
