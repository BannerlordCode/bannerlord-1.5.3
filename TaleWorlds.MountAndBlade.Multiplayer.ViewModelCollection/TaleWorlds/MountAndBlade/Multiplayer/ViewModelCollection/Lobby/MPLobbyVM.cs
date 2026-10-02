using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.AfterBattle;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Authentication;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.OfficialGame;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Popup;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000032 RID: 50
	public class MPLobbyVM : ViewModel
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000D15D File Offset: 0x0000B35D
		private PlayerId _partyLeaderId
		{
			get
			{
				return NetworkMain.GameClient.PlayersInParty.Find((PartyPlayerInLobbyClient p) => p.IsPartyLeader).PlayerId;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0000D192 File Offset: 0x0000B392
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x0000D19A File Offset: 0x0000B39A
		public MPLobbyVM.LobbyPage CurrentPage { get; private set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000D1A3 File Offset: 0x0000B3A3
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x0000D1AB File Offset: 0x0000B3AB
		public List<MPLobbyVM.LobbyPage> DisallowedPages { get; private set; }

		// Token: 0x060003B4 RID: 948 RVA: 0x0000D1B4 File Offset: 0x0000B3B4
		public MPLobbyVM(LobbyState lobbyState, Action<BasicCharacterObject> onOpenFacegen, Action onForceCloseFacegen, Action onLogout, Action<bool> onBusyStateChanged, Action<KeyOptionVM> onKeybindRequest, Func<string> getContinueKeyText, Action<bool> setNavigationRestriction)
		{
			this.CurrentPage = MPLobbyVM.LobbyPage.NotAssigned;
			this._onForceCloseFacegen = onForceCloseFacegen;
			this._onLogout = onLogout;
			this._onBusyStateChanged = onBusyStateChanged;
			this._onKeybindRequest = onKeybindRequest;
			this._setNavigationRestriction = setNavigationRestriction;
			this._lobbyState = lobbyState;
			this._lobbyClient = this._lobbyState.LobbyClient;
			this._partySuggestionQueue = new MBQueue<MPLobbyPartyPlayerSuggestionPopupVM.PlayerPartySuggestionData>();
			this._partyActionQueue = new ConcurrentQueue<ValueTuple<MPLobbyVM.PartyActionType, PlayerId, PartyRemoveReason>>();
			this.BlockerState = new MPLobbyBlockerStateVM(this._setNavigationRestriction);
			this.Menu = new MPLobbyMenuVM(lobbyState, this._setNavigationRestriction, new Func<Task>(this.RequestExit));
			bool isAbleToSearchForGame = NetworkMain.GameClient.IsAbleToSearchForGame;
			this.IsMatchmakingEnabled = !isAbleToSearchForGame;
			this.IsMatchmakingEnabled = isAbleToSearchForGame;
			this.IsCustomGameFindEnabled = NetworkMain.GameClient.IsCustomBattleAvailable;
			this.IsPartyLeader = NetworkMain.GameClient.IsPartyLeader;
			this.IsInParty = NetworkMain.GameClient.IsInParty;
			this.Login = new MPAuthenticationVM(lobbyState);
			this.Rejoin = new MPLobbyRejoinVM(new Action<MPLobbyVM.LobbyPage>(this.OnChangePageRequest));
			this.Home = new MPLobbyHomeVM(lobbyState.NewsManager, new Action<MPLobbyVM.LobbyPage>(this.OnChangePageRequest));
			this.Profile = new MPLobbyProfileVM(lobbyState, new Action<MPLobbyVM.LobbyPage>(this.OnChangePageRequest), new Action(this.ExecuteOpenRecentGames));
			this.Matchmaking = new MPMatchmakingVM(lobbyState, new Action<MPLobbyVM.LobbyPage>(this.OnChangePageRequest), new Action<string, bool>(this.OnMatchSelectionChanged), new Action<bool>(this.OnGameFindStateChanged));
			this.Armory = new MPArmoryVM(onOpenFacegen, new Action<MPArmoryCosmeticItemBaseVM>(this.OnItemObtainRequested), getContinueKeyText);
			this.Friends = new MPLobbyFriendsVM();
			this.GameSearch = new MPLobbyGameSearchVM();
			this.PlayerProfile = new MPLobbyPlayerProfileVM(lobbyState);
			this.AfterBattlePopup = new MPAfterBattlePopupVM(getContinueKeyText);
			this.PartyInvitationPopup = new MPLobbyPartyInvitationPopupVM();
			this.PartyJoinRequestPopup = new MPLobbyPartyJoinRequestPopupVM();
			this.PartyPlayerSuggestionPopup = new MPLobbyPartyPlayerSuggestionPopupVM();
			this.QueryPopup = new MPLobbyQueryPopupVM();
			this.InformationPopup = new MPLobbyInformationPopup();
			this.Options = new MPOptionsVM(false, new Action(this.ExecuteShowBrightness), new Action(this.ExecuteShowExposure), this._onKeybindRequest);
			this.BrightnessPopup = new BrightnessOptionVM(null);
			this.ExposurePopup = new ExposureOptionVM(null);
			this.BannerlordIDChangePopup = new MPLobbyBannerlordIDChangePopup();
			this.BannerlordIDAddFriendPopup = new MPLobbyBannerlordIDAddFriendPopupVM();
			this.Clan = new MPLobbyClanVM(new Action(this.OpenInviteClanMemberPopup));
			this.ClanCreationPopup = new MPLobbyClanCreationPopupVM();
			this.ClanCreationInformationPopup = new MPLobbyClanCreationInformationVM(new Action(this.OpenClanCreationPopup));
			this.ClanInvitationPopup = new MPLobbyClanInvitationPopupVM();
			this.ClanMatchmakingRequestPopup = new MPLobbyClanMatchmakingRequestPopupVM();
			this.ClanInviteFriendsPopup = new MPLobbyClanInviteFriendsPopupVM(new Func<MBBindingList<MPLobbyPlayerBaseVM>>(this.Friends.GetAllFriends));
			this.ClanLeaderboardPopup = new MPLobbyClanLeaderboardVM();
			this.CosmeticObtainPopup = new MPCosmeticObtainPopupVM(new Action<string, int>(this.OnItemObtained), getContinueKeyText);
			this.ChangeSigilPopup = new MPLobbyHomeChangeSigilPopupVM(new Action<MPLobbyCosmeticSigilItemVM>(this.OnItemObtainRequested));
			this.BadgeProgressionInformation = new MPLobbyBadgeProgressInformationVM(getContinueKeyText);
			this.BadgeSelectionPopup = new MPLobbyBadgeSelectionPopupVM(new Action(this.OnBadgeNotificationRead), new Action(this.OnBadgeSelectionUpdated), new Action<MPLobbyAchievementBadgeGroupVM>(this.OnBadgeProgressInfoRequested));
			this.RecentGames = new MPLobbyRecentGamesVM();
			this.RankProgressInformation = new MPLobbyRankProgressInformationVM(getContinueKeyText);
			this.RankLeaderboard = new MPLobbyRankLeaderboardVM(lobbyState);
			this.Home.OnFindGameRequested += this.AutoFindGameRequested;
			this.Profile.OnFindGameRequested += this.AutoFindGameRequested;
			MPLobbyRejoinVM rejoin = this.Rejoin;
			rejoin.OnRejoinRequested = (Action)Delegate.Combine(rejoin.OnRejoinRequested, new Action(this.OnRejoinRequested));
			if (this._lobbyClient.LoggedIn)
			{
				this.SetPage(MPLobbyVM.LobbyPage.Home, MPMatchmakingVM.MatchmakingSubPages.Default);
			}
			this.DisallowedPages = new List<MPLobbyVM.LobbyPage>();
			InformationManager.ClearAllMessages();
			this._lobbyState.RegisterForCustomServerAction(new Func<GameServerEntry, List<CustomServerAction>>(this.OnServerActionRequested));
			LobbyState lobbyState2 = this._lobbyState;
			lobbyState2.OnUserGeneratedContentPrivilegeUpdated = (Action<bool>)Delegate.Combine(lobbyState2.OnUserGeneratedContentPrivilegeUpdated, new Action<bool>(this.OnUserGeneratedContentPrivilegeUpdated));
			this.InitializeCallbacks();
			this.RefreshValues();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000D5D4 File Offset: 0x0000B7D4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.BlockerState.RefreshValues();
			this.Login.RefreshValues();
			this.Rejoin.RefreshValues();
			this.Menu.RefreshValues();
			this.Home.RefreshValues();
			this.Matchmaking.RefreshValues();
			this.Armory.RefreshValues();
			this.Profile.RefreshValues();
			this.Friends.RefreshValues();
			this.GameSearch.RefreshValues();
			this.PlayerProfile.RefreshValues();
			this.Options.RefreshValues();
			this.QueryPopup.RefreshValues();
			this.AfterBattlePopup.RefreshValues();
			this.PartyInvitationPopup.RefreshValues();
			this.PartyJoinRequestPopup.RefreshValues();
			this.PartyPlayerSuggestionPopup.RefreshValues();
			this.Clan.RefreshValues();
			this.ClanCreationPopup.RefreshValues();
			this.ClanCreationInformationPopup.RefreshValues();
			this.ClanInvitationPopup.RefreshValues();
			this.ClanMatchmakingRequestPopup.RefreshValues();
			this.ClanInviteFriendsPopup.RefreshValues();
			this.ClanLeaderboardPopup.RefreshValues();
			this.BannerlordIDChangePopup.RefreshValues();
			this.BannerlordIDAddFriendPopup.RefreshValues();
			this.CosmeticObtainPopup.RefreshValues();
			this.ChangeSigilPopup.RefreshValues();
			this.BadgeProgressionInformation.RefreshValues();
			this.BadgeSelectionPopup.RefreshValues();
			this.RecentGames.RefreshValues();
			this.RankProgressInformation.RefreshValues();
			this.RankLeaderboard.RefreshValues();
			this.BrightnessPopup.RefreshValues();
			this.ExposurePopup.RefreshValues();
			this.RefreshPlayerData(NetworkMain.GameClient.PlayerData);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000D778 File Offset: 0x0000B978
		private void OnUserGeneratedContentPrivilegeUpdated(bool hasPrivilege)
		{
			bool? cachedHasUserGeneratedContentPrivilege = this._cachedHasUserGeneratedContentPrivilege;
			if (!((cachedHasUserGeneratedContentPrivilege.GetValueOrDefault() == hasPrivilege) & (cachedHasUserGeneratedContentPrivilege != null)))
			{
				this.OnFriendListUpdated(true);
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000D7AC File Offset: 0x0000B9AC
		private List<CustomServerAction> OnServerActionRequested(GameServerEntry arg)
		{
			GameServerEntry arg2 = arg;
			if (arg2 != null && !arg2.IsOfficial)
			{
				return new List<CustomServerAction>
				{
					new CustomServerAction(delegate
					{
						this.OnShowPlayerProfile(arg.HostId);
					}, arg, GameTexts.FindText("str_mp_scoreboard_context_viewprofile", null).ToString())
				};
			}
			return null;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000D818 File Offset: 0x0000BA18
		public void CreateInputKeyVisuals(HotKey cancelInputKey, HotKey doneInputKey, HotKey previousInputKey, HotKey nextInputKey, HotKey firstInputKey, HotKey lastInputKey)
		{
			this.BannerlordIDAddFriendPopup.SetCancelInputKey(cancelInputKey);
			this.BannerlordIDAddFriendPopup.SetDoneInputKey(doneInputKey);
			this.BannerlordIDChangePopup.SetCancelInputKey(cancelInputKey);
			this.BannerlordIDChangePopup.SetDoneInputKey(doneInputKey);
			this.RankLeaderboard.SetCancelInputKey(cancelInputKey);
			this.RankLeaderboard.SetPreviousInputKey(previousInputKey);
			this.RankLeaderboard.SetNextInputKey(nextInputKey);
			this.RankLeaderboard.SetFirstInputKey(firstInputKey);
			this.RankLeaderboard.SetLastInputKey(lastInputKey);
			this.ChangeSigilPopup.SetCancelInputKey(cancelInputKey);
			this.ChangeSigilPopup.SetDoneInputKey(doneInputKey);
			this.ClanCreationPopup.SetCancelInputKey(cancelInputKey);
			this.Clan.ClanOverview.ChangeSigilPopup.SetCancelInputKey(cancelInputKey);
			this.Clan.ClanOverview.ChangeSigilPopup.SetDoneInputKey(doneInputKey);
			this.Clan.ClanOverview.ChangeFactionPopup.SetCancelInputKey(cancelInputKey);
			this.Clan.ClanOverview.ChangeFactionPopup.SetDoneInputKey(doneInputKey);
			this.Clan.ClanOverview.SendAnnouncementPopup.SetCancelInputKey(cancelInputKey);
			this.Clan.ClanOverview.SendAnnouncementPopup.SetDoneInputKey(doneInputKey);
			this.Clan.ClanOverview.SetClanInformationPopup.SetCancelInputKey(doneInputKey);
			this.Clan.ClanOverview.SetClanInformationPopup.SetDoneInputKey(doneInputKey);
			this.BadgeSelectionPopup.SetCancelInputKey(cancelInputKey);
			this.Login.SetDoneInputKey(doneInputKey);
			this.Login.SetCancelInputKey(cancelInputKey);
			this.CosmeticObtainPopup.SetDoneInputKey(doneInputKey);
			this.QueryPopup.SetDoneInputKey(doneInputKey);
			this.QueryPopup.SetCancelInputKey(cancelInputKey);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000D9B0 File Offset: 0x0000BBB0
		public override void OnFinalize()
		{
			this.FinalizeCallbacks();
			this.Clan.ClanOverview.ChangeSigilPopup.OnFinalize();
			this.Clan.ClanOverview.ChangeFactionPopup.OnFinalize();
			this.Clan.ClanOverview.SendAnnouncementPopup.OnFinalize();
			this.BannerlordIDAddFriendPopup.OnFinalize();
			this.BannerlordIDChangePopup.OnFinalize();
			this.RankLeaderboard.OnFinalize();
			this.ChangeSigilPopup.OnFinalize();
			this.ClanCreationPopup.OnFinalize();
			this.BadgeSelectionPopup.OnFinalize();
			this.BadgeProgressionInformation.OnFinalize();
			this.CosmeticObtainPopup.OnFinalize();
			InformationManager.ClearAllMessages();
			this.Login.OnFinalize();
			this.Armory.OnFinalize();
			this.Matchmaking.OnFinalize();
			this.Friends.OnFinalize();
			this.Home.OnFindGameRequested -= this.AutoFindGameRequested;
			this.Profile.OnFindGameRequested -= this.AutoFindGameRequested;
			if (this._lobbyState != null)
			{
				this._lobbyState.UnregisterForCustomServerAction(new Func<GameServerEntry, List<CustomServerAction>>(this.OnServerActionRequested));
				LobbyState lobbyState = this._lobbyState;
				lobbyState.OnUserGeneratedContentPrivilegeUpdated = (Action<bool>)Delegate.Remove(lobbyState.OnUserGeneratedContentPrivilegeUpdated, new Action<bool>(this.OnUserGeneratedContentPrivilegeUpdated));
			}
			MPLobbyRejoinVM rejoin = this.Rejoin;
			rejoin.OnRejoinRequested = (Action)Delegate.Remove(rejoin.OnRejoinRequested, new Action(this.OnRejoinRequested));
			this.Menu.OnFinalize();
			this.Home.OnFinalize();
			this._lobbyState = null;
			base.OnFinalize();
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000DB4C File Offset: 0x0000BD4C
		private void InitializeCallbacks()
		{
			MPLobbyPlayerBaseVM.OnPlayerProfileRequested = new Action<PlayerId>(this.OnShowPlayerProfile);
			MPLobbyPlayerBaseVM.OnBannerlordIDChangeRequested = new Action<PlayerId>(this.OnBannerlordIDChangeRequested);
			MPLobbyPlayerBaseVM.OnAddFriendWithBannerlordIDRequested = new Action<PlayerId>(this.OnAddFriendWithBannerlordIDRequested);
			MPLobbyPlayerBaseVM.OnSigilChangeRequested = new Action<PlayerId>(this.OnSigilChangeRequested);
			MPLobbyPlayerBaseVM.OnBadgeChangeRequested = new Action<PlayerId>(this.OnBadgeChangeRequested);
			MPLobbyPlayerBaseVM.OnRankProgressionRequested = new Action<MPLobbyPlayerBaseVM>(this.OnRankProgressionRequested);
			MPLobbyPlayerBaseVM.OnRankLeaderboardRequested = new Action<string>(this.OnRankLeaderboardRequested);
			MPLobbyPlayerBaseVM.OnClanPageRequested = new Action(this.OnClanPageRequested);
			MPLobbyPlayerBaseVM.OnClanLeaderboardRequested = new Action(this.OnClanLeaderboardRequested);
			MPArmoryCosmeticItemBaseVM.OnPurchaseRequested += this.OnItemObtainRequested;
			MPAnnouncementItemVM.OnInspect += this.OnAnnouncementInspected;
			MPCustomGameVM.OnMapCheckingStateChanged += this.OnCustomGameMapCheckingStateChanged;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000DC28 File Offset: 0x0000BE28
		private void FinalizeCallbacks()
		{
			MPLobbyPlayerBaseVM.OnPlayerProfileRequested = null;
			MPLobbyPlayerBaseVM.OnBannerlordIDChangeRequested = null;
			MPLobbyPlayerBaseVM.OnAddFriendWithBannerlordIDRequested = null;
			MPLobbyPlayerBaseVM.OnSigilChangeRequested = null;
			MPLobbyPlayerBaseVM.OnBadgeChangeRequested = null;
			MPLobbyPlayerBaseVM.OnRankProgressionRequested = null;
			MPLobbyPlayerBaseVM.OnRankLeaderboardRequested = null;
			MPLobbyPlayerBaseVM.OnClanPageRequested = null;
			MPLobbyPlayerBaseVM.OnClanLeaderboardRequested = null;
			MPArmoryCosmeticItemBaseVM.OnPurchaseRequested -= this.OnItemObtainRequested;
			MPAnnouncementItemVM.OnInspect -= this.OnAnnouncementInspected;
			MPCustomGameVM.OnMapCheckingStateChanged -= this.OnCustomGameMapCheckingStateChanged;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000DC9E File Offset: 0x0000BE9E
		public void OnActivate()
		{
			this._isRejoining = false;
			this._isDisconnecting = false;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000DCAE File Offset: 0x0000BEAE
		public void OnDeactivate()
		{
			this._isRejoining = false;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
		public void OnTick(float dt)
		{
			this.IsLoggedIn = NetworkMain.GameClient.LoggedIn;
			this.Login.OnTick(dt);
			this.Friends.OnTick(dt);
			this.Armory.OnTick(dt);
			this.Home.OnTick(dt);
			this.PartyInvitationPopup.OnTick(dt);
			this.PartyJoinRequestPopup.OnTick(dt);
			this.UpdateBlockerState();
			if (this.IsSearchingGame)
			{
				this._playerCountInQueueTimer += dt;
				if (this._playerCountInQueueTimer >= 10f)
				{
					this.UpdatePlayerCountInQueue();
					this._playerCountInQueueTimer = 0f;
				}
			}
			else
			{
				this._playerCountInQueueTimer = 10f;
			}
			if (!this.PartyPlayerSuggestionPopup.IsEnabled && !this._partySuggestionQueue.IsEmpty<MPLobbyPartyPlayerSuggestionPopupVM.PlayerPartySuggestionData>() && !this._lobbyClient.IsPartyFull)
			{
				MPLobbyPartyPlayerSuggestionPopupVM.PlayerPartySuggestionData playerPartySuggestionData = this._partySuggestionQueue.Dequeue();
				this.PartyPlayerSuggestionPopup.OpenWith(playerPartySuggestionData);
			}
			else if (!this._partySuggestionQueue.IsEmpty<MPLobbyPartyPlayerSuggestionPopupVM.PlayerPartySuggestionData>() && this._lobbyClient.IsPartyFull)
			{
				this._partySuggestionQueue.Clear();
			}
			ValueTuple<MPLobbyVM.PartyActionType, PlayerId, PartyRemoveReason> valueTuple;
			if (this._partyActionQueue.TryDequeue(out valueTuple))
			{
				MPLobbyVM.PartyActionType item = valueTuple.Item1;
				PlayerId item2 = valueTuple.Item2;
				PartyRemoveReason item3 = valueTuple.Item3;
				switch (item)
				{
				case MPLobbyVM.PartyActionType.Add:
					this.HandlePlayerAddedToParty(item2);
					break;
				case MPLobbyVM.PartyActionType.Remove:
					this.HandlePlayerRemovedFromParty(item2, item3);
					break;
				case MPLobbyVM.PartyActionType.AssignLeader:
					this.HandlePlayerAssignedPartyLeader(item2);
					break;
				}
			}
			if (this._playerDataToRefreshWith != null && !NetworkMain.GameClient.IsRefreshingPlayerData)
			{
				this.RefreshPlayerDataInternal();
			}
			else if (this._playerDataToRefreshWith == null && NetworkMain.GameClient.IsRefreshingPlayerData)
			{
				this._playerDataToRefreshWith = null;
				NetworkMain.GameClient.IsRefreshingPlayerData = false;
			}
			this.Matchmaking.OnTick(dt);
			this.GameSearch.OnTick(dt);
			this.Armory.Cosmetics.OnTick(dt);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000DE8C File Offset: 0x0000C08C
		public void OnConfirm()
		{
			if (this.Login.IsEnabled)
			{
				if (this.Login.CanTryLogin)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.Login.ExecuteLogin();
					return;
				}
			}
			else
			{
				if (this.Options.IsEnabled)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.Options.ExecuteApply();
					return;
				}
				if (this.QueryPopup.IsEnabled)
				{
					if (this.QueryPopup.IsInquiry)
					{
						SoundEvent.PlaySound2D("event:/ui/default");
						this.QueryPopup.ExecuteAccept();
						return;
					}
				}
				else
				{
					if (this.ExposurePopup.Visible)
					{
						SoundEvent.PlaySound2D("event:/ui/default");
						this.ExposurePopup.ExecuteConfirm();
						return;
					}
					if (this.BrightnessPopup.Visible)
					{
						SoundEvent.PlaySound2D("event:/ui/default");
						this.BrightnessPopup.ExecuteConfirm();
						return;
					}
					if (this.ChangeSigilPopup.IsEnabled && !this.CosmeticObtainPopup.IsEnabled)
					{
						SoundEvent.PlaySound2D("event:/ui/default");
						this.ChangeSigilPopup.ExecuteChangeSigil();
						return;
					}
					if (this.CosmeticObtainPopup.IsEnabled)
					{
						if (this.CosmeticObtainPopup.ObtainState == 0 && this.CosmeticObtainPopup.CanObtain)
						{
							SoundEvent.PlaySound2D("event:/ui/multiplayer/shop_purchase_proceed");
							this.CosmeticObtainPopup.ExecuteAction();
							return;
						}
					}
					else
					{
						if (this.BannerlordIDAddFriendPopup.IsSelected)
						{
							SoundEvent.PlaySound2D("event:/ui/default");
							this.BannerlordIDAddFriendPopup.ExecuteTryAddFriend();
							return;
						}
						if (this.BannerlordIDChangePopup.IsSelected)
						{
							SoundEvent.PlaySound2D("event:/ui/default");
							this.BannerlordIDChangePopup.ExecuteApply();
							return;
						}
						if (this.Clan.ClanOverview.ChangeSigilPopup.IsSelected)
						{
							SoundEvent.PlaySound2D("event:/ui/default");
							this.Clan.ClanOverview.ChangeSigilPopup.ExecuteChangeSigil();
							return;
						}
						if (this.Clan.ClanOverview.ChangeFactionPopup.IsSelected)
						{
							SoundEvent.PlaySound2D("event:/ui/default");
							this.Clan.ClanOverview.ChangeFactionPopup.ExecuteChangeFaction();
							return;
						}
						if (this.Clan.ClanOverview.SetClanInformationPopup.IsSelected)
						{
							SoundEvent.PlaySound2D("event:/ui/default");
							this.Clan.ClanOverview.SetClanInformationPopup.ExecuteSend();
							return;
						}
						if (this.Clan.ClanOverview.SendAnnouncementPopup.IsSelected)
						{
							SoundEvent.PlaySound2D("event:/ui/default");
							this.Clan.ClanOverview.SendAnnouncementPopup.ExecuteSend();
						}
					}
				}
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000E10C File Offset: 0x0000C30C
		public async void OnEscape()
		{
			if (!this._waitingForEscapeResult)
			{
				this._waitingForEscapeResult = true;
				if (this.CurrentPage == MPLobbyVM.LobbyPage.Authentication)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					if (this.HasNoPopupOpen())
					{
						this.Login.ExecuteExit();
					}
					else
					{
						this.ForceClosePopups();
					}
				}
				else if (this.QueryPopup.IsEnabled)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.QueryPopup.ExecuteDecline();
				}
				else if (this.ExposurePopup.Visible)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.ExposurePopup.ExecuteCancel();
				}
				else if (this.BrightnessPopup.Visible)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.BrightnessPopup.ExecuteCancel();
				}
				else if (this.ClanCreationPopup.IsEnabled)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.ClanCreationPopup.ExecuteClosePopup();
				}
				else if (this.CosmeticObtainPopup.IsEnabled)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.CosmeticObtainPopup.ExecuteClosePopup();
				}
				else if (this.Friends.IsPlayerActionsActive)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.Friends.IsPlayerActionsActive = false;
				}
				else if (this.RankLeaderboard.IsPlayerActionsActive)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.RankLeaderboard.IsPlayerActionsActive = false;
				}
				else if (this.RecentGames.IsPlayerActionsActive)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.RecentGames.IsPlayerActionsActive = false;
				}
				else if (this.Armory.IsManagingTaunts)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					if (this.Armory.Cosmetics.SelectedTauntItem != null || this.Armory.Cosmetics.SelectedTauntSlot != null)
					{
						this.Armory.ExecuteClearTauntSelection();
					}
					else
					{
						this.Armory.ExecuteToggleManageTauntsState();
					}
				}
				else if (NetworkMain.GameClient.IsRefreshingPlayerData)
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_exit", null).ToString(), new TextObject("{=usLhlY2j}Please wait until player data is downloaded.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
				}
				else if (this.HasAnyContextMenuOpen())
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.ForceCloseContextMenus();
				}
				else if (this.HasNoPopupOpen())
				{
					SoundEvent.PlaySound2D("event:/ui/sort");
					await this.RequestExit();
				}
				else
				{
					SoundEvent.PlaySound2D("event:/ui/default");
					this.ForceClosePopups();
				}
				this._waitingForEscapeResult = false;
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000E145 File Offset: 0x0000C345
		public bool HasAnyContextMenuOpen()
		{
			return this.Clan.ClanRoster.IsMemberActionsActive || this.Friends.IsPlayerActionsActive || this.RankLeaderboard.IsPlayerActionsActive || this.RecentGames.IsPlayerActionsActive;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000E180 File Offset: 0x0000C380
		public void ForceCloseContextMenus()
		{
			this.Clan.ClanRoster.IsMemberActionsActive = false;
			this.Friends.IsPlayerActionsActive = false;
			this.RankLeaderboard.IsPlayerActionsActive = false;
			this.RecentGames.IsPlayerActionsActive = false;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000E1B8 File Offset: 0x0000C3B8
		public bool HasNoPopupOpen()
		{
			return !this.Clan.IsEnabled && !this.Clan.ClanOverview.ChangeFactionPopup.IsSelected && !this.Clan.ClanOverview.ChangeSigilPopup.IsSelected && !this.Clan.ClanOverview.SendAnnouncementPopup.IsSelected && !this.Clan.ClanOverview.SetClanInformationPopup.IsSelected && !this.PartyInvitationPopup.IsEnabled && !this.PartyJoinRequestPopup.IsEnabled && !this.PartyPlayerSuggestionPopup.IsEnabled && !this.ClanInvitationPopup.IsEnabled && !this.ClanMatchmakingRequestPopup.IsEnabled && !this.BannerlordIDChangePopup.IsSelected && !this.BannerlordIDAddFriendPopup.IsSelected && !this.CosmeticObtainPopup.IsEnabled && !this.AfterBattlePopup.IsEnabled && !this.ClanCreationPopup.IsEnabled && !this.ClanCreationInformationPopup.IsEnabled && !this.ClanInviteFriendsPopup.IsEnabled && !this.ClanLeaderboardPopup.IsEnabled && !this.BadgeProgressionInformation.IsEnabled && !this.BadgeSelectionPopup.IsEnabled && !this.ChangeSigilPopup.IsEnabled && !this.RecentGames.IsEnabled && !this.PlayerProfile.IsEnabled && !this.RankProgressInformation.IsEnabled && !this.RankLeaderboard.IsEnabled && !this.ExposurePopup.Visible && !this.BrightnessPopup.Visible && !this.QueryPopup.IsEnabled;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000E394 File Offset: 0x0000C594
		private void ForceClosePopups()
		{
			this.Clan.ExecuteClosePopup();
			this.Clan.ClanOverview.ChangeFactionPopup.ExecuteClosePopup();
			this.Clan.ClanOverview.ChangeSigilPopup.ExecuteClosePopup();
			this.Clan.ClanOverview.SendAnnouncementPopup.ExecuteClosePopup();
			this.Clan.ClanOverview.SetClanInformationPopup.ExecuteClosePopup();
			this.PartyInvitationPopup.Close();
			this.PartyJoinRequestPopup.Close();
			this.PartyPlayerSuggestionPopup.Close();
			this.ClanInvitationPopup.Close();
			this.ClanMatchmakingRequestPopup.Close();
			this.BannerlordIDChangePopup.ExecuteClosePopup();
			this.BannerlordIDAddFriendPopup.ExecuteClosePopup();
			this.CosmeticObtainPopup.ExecuteClosePopup();
			this.AfterBattlePopup.ExecuteClose();
			this.ClanCreationPopup.ExecuteClosePopup();
			this.ClanCreationInformationPopup.ExecuteClosePopup();
			this.ClanInviteFriendsPopup.ExecuteClosePopup();
			this.ClanLeaderboardPopup.ExecuteClosePopup();
			this.BadgeProgressionInformation.ExecuteClosePopup();
			this.BadgeSelectionPopup.ExecuteClosePopup();
			this.ChangeSigilPopup.ExecuteClosePopup();
			this.RecentGames.ExecuteClosePopup();
			this.PlayerProfile.ExecuteClosePopup();
			this.RankProgressInformation.ExecuteClosePopup();
			this.RankLeaderboard.ExecuteClosePopup();
			this.ExposurePopup.ExecuteCancel();
			this.BrightnessPopup.ExecuteCancel();
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000E4F4 File Offset: 0x0000C6F4
		public async Task RequestExit()
		{
			if (NetworkMain.GameClient.CurrentState != LobbyClient.State.WaitingToJoinCustomGame && NetworkMain.GameClient.CurrentState != LobbyClient.State.WaitingToJoinPremadeGame && NetworkMain.GameClient.CurrentState != LobbyClient.State.Connected && NetworkMain.GameClient.CurrentState != LobbyClient.State.SessionRequested && NetworkMain.GameClient.CurrentState != LobbyClient.State.Working)
			{
				while (NetworkMain.GameClient.CurrentState == LobbyClient.State.RequestingToSearchBattle || NetworkMain.GameClient.CurrentState == LobbyClient.State.RequestingToCancelSearchBattle)
				{
					await Task.Yield();
				}
				if (NetworkMain.GameClient.CurrentState == LobbyClient.State.SearchingBattle)
				{
					if (!NetworkMain.GameClient.IsInParty || NetworkMain.GameClient.IsPartyLeader)
					{
						NetworkMain.GameClient.CancelFindGame();
					}
				}
				else if (!this._isDisconnecting)
				{
					if (Input.IsGamepadActive)
					{
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=exitMenuOption}Exit", null).ToString(), new TextObject("{=NMh61YLB}Are you sure you want to exit?", null).ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), new Action(this.OnExit), null, "", 0f, null, null, null), false, false);
					}
					else
					{
						this.OnExit();
					}
				}
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000E539 File Offset: 0x0000C739
		private void OnExit()
		{
			this._isDisconnecting = true;
			Action<bool> setNavigationRestriction = this._setNavigationRestriction;
			if (setNavigationRestriction != null)
			{
				setNavigationRestriction(true);
			}
			Action onLogout = this._onLogout;
			if (onLogout != null)
			{
				onLogout();
			}
			NetworkMain.GameClient.Logout(null);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000E570 File Offset: 0x0000C770
		private async void UpdatePlayerCountInQueue()
		{
			LobbyClient gameClient = NetworkMain.GameClient;
			if (gameClient.Connected && this.GameSearch.CustomGameMode != MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				MatchmakingWaitTimeStats matchmakingWaitTimeStats = await gameClient.GetMatchmakingWaitTimes();
				(from t in this.Matchmaking.QuickplayGameTypes
					where t.IsSelected
					select t.Type).ToArray<string>();
				this.GameSearch.UpdateData(matchmakingWaitTimeStats, null);
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000E5A9 File Offset: 0x0000C7A9
		public void ConnectionStateUpdated(bool isAuthenticated)
		{
			if (isAuthenticated)
			{
				this.RefreshRecentGames();
				this.Friends.OnStateActivate();
			}
			else
			{
				this.PartyInvitationPopup.Close();
				this.PartyJoinRequestPopup.Close();
				this.ClanInvitationPopup.Close();
			}
			this.OnSearchBattleCanceled();
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
		public void ShowOptionsChangedInquiry(Action onAccept = null, Action onDecline = null)
		{
			this.QueryPopup.ShowInquiry(new TextObject("{=Rov73lC3}Unsaved Changes", null), new TextObject("{=u0HNU5pA}You have unsaved changes. Do you want to apply them before you continue?", null), delegate
			{
				this.Options.ExecuteApply();
				Action onAccept2 = onAccept;
				if (onAccept2 == null)
				{
					return;
				}
				onAccept2();
			}, delegate
			{
				this.Options.ForceCancel();
				Action onDecline2 = onDecline;
				if (onDecline2 == null)
				{
					return;
				}
				onDecline2();
			});
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000E64C File Offset: 0x0000C84C
		public void SetPage(MPLobbyVM.LobbyPage lobbyPage, MPMatchmakingVM.MatchmakingSubPages matchmakingSubPage = MPMatchmakingVM.MatchmakingSubPages.Default)
		{
			if (this.CurrentPage != lobbyPage)
			{
				if (lobbyPage != MPLobbyVM.LobbyPage.Authentication && this.CurrentPage == MPLobbyVM.LobbyPage.Options && this.Options.IsOptionsChanged())
				{
					this.ShowOptionsChangedInquiry(null, null);
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				bool flag8 = false;
				bool flag9 = false;
				switch (lobbyPage)
				{
				case MPLobbyVM.LobbyPage.Authentication:
					flag2 = true;
					break;
				case MPLobbyVM.LobbyPage.Rejoin:
					flag3 = true;
					break;
				case MPLobbyVM.LobbyPage.Options:
					flag = true;
					flag7 = true;
					break;
				case MPLobbyVM.LobbyPage.Home:
					flag = true;
					flag8 = true;
					flag4 = true;
					break;
				case MPLobbyVM.LobbyPage.Armory:
					flag = true;
					flag8 = true;
					flag6 = true;
					break;
				case MPLobbyVM.LobbyPage.Matchmaking:
					flag = true;
					flag8 = true;
					flag5 = true;
					this.Matchmaking.TrySetMatchmakingSubPage(matchmakingSubPage);
					break;
				case MPLobbyVM.LobbyPage.Profile:
					flag = true;
					flag9 = true;
					flag8 = true;
					break;
				}
				this._isDisconnecting = false;
				this.Menu.IsEnabled = flag;
				this.Login.IsEnabled = flag2;
				this.Rejoin.IsEnabled = flag3;
				this.Home.IsEnabled = flag4;
				this.Matchmaking.IsEnabled = flag5;
				this.Armory.IsEnabled = flag6;
				this.Options.IsEnabled = flag7;
				this.Friends.IsEnabled = flag8;
				this.Profile.IsEnabled = flag9;
				this.IsArmoryActive = this.Armory.IsEnabled;
				if (this.CurrentPage == MPLobbyVM.LobbyPage.Matchmaking)
				{
					this.Matchmaking.TrySetMatchmakingSubPage(MPMatchmakingVM.MatchmakingSubPages.QuickPlay);
				}
				else if (this.CurrentPage == MPLobbyVM.LobbyPage.Profile)
				{
					this.RefreshRecentGames();
				}
				this.CurrentPage = lobbyPage;
				this._setNavigationRestriction(this.CurrentPage == MPLobbyVM.LobbyPage.Authentication);
				this.Menu.SetPage(lobbyPage);
				if (lobbyPage != MPLobbyVM.LobbyPage.Profile)
				{
					this.Menu.HasProfileNotification = this.BadgeSelectionPopup.HasNotifications;
					return;
				}
				this.Menu.HasProfileNotification = false;
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000E804 File Offset: 0x0000CA04
		public void RefreshRecentGames()
		{
			MBReadOnlyList<MatchHistoryData> entries = MultiplayerLocalDataManager.Instance.MatchHistory.GetEntries();
			if (entries != null)
			{
				this.Profile.RefreshRecentGames(entries);
				this.RecentGames.RefreshData(entries);
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000E83C File Offset: 0x0000CA3C
		public void OnDisconnected()
		{
			if (this.CurrentPage != MPLobbyVM.LobbyPage.Authentication)
			{
				this.SetPage(MPLobbyVM.LobbyPage.Authentication, MPMatchmakingVM.MatchmakingSubPages.Default);
			}
			this.Friends.UpdateCanInviteOtherPlayersToParty();
			this.IsPartyLeader = false;
			this.ForceClosePopups();
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000E867 File Offset: 0x0000CA67
		private void AutoFindGameRequested()
		{
			this.Matchmaking.ExecuteAutoFindGame();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000E874 File Offset: 0x0000CA74
		public void OnServerStatusReceived(ServerStatus serverStatus)
		{
			this.Matchmaking.OnServerStatusReceived(serverStatus);
			this.IsMatchmakingEnabled = NetworkMain.GameClient.IsAbleToSearchForGame;
			this.IsCustomGameFindEnabled = NetworkMain.GameClient.IsCustomBattleAvailable;
			if (!NetworkMain.GameClient.IsAbleToSearchForGame && this.CurrentPage == MPLobbyVM.LobbyPage.Matchmaking && this.Matchmaking.CurrentSubPage == MPMatchmakingVM.MatchmakingSubPages.QuickPlay)
			{
				this.SetPage(MPLobbyVM.LobbyPage.Home, MPMatchmakingVM.MatchmakingSubPages.Default);
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=aO3avkK9}Matchmaking Disabled", null).ToString(), new TextObject("{=5baU17n0}Matchmaking feature is currently disabled", null).ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
			}
			if (!serverStatus.IsCustomBattleEnabled && this.CurrentPage == MPLobbyVM.LobbyPage.Matchmaking && (this.Matchmaking.CurrentSubPage == MPMatchmakingVM.MatchmakingSubPages.CustomGameList || this.Matchmaking.CurrentSubPage == MPMatchmakingVM.MatchmakingSubPages.CustomGame))
			{
				this.SetPage(MPLobbyVM.LobbyPage.Home, MPMatchmakingVM.MatchmakingSubPages.Default);
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=i0OEsfLt}Custom Battle Disabled", null).ToString(), new TextObject("{=F7dBjl83}Custom Battle feature is currently disabled", null).ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000E9A8 File Offset: 0x0000CBA8
		public void OnRejoinBattleRequestAnswered(bool isSuccessful)
		{
			if (isSuccessful && this._isRejoinRequested)
			{
				this._isRejoining = true;
				return;
			}
			this.SetPage(MPLobbyVM.LobbyPage.Home, MPMatchmakingVM.MatchmakingSubPages.Default);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000E9C5 File Offset: 0x0000CBC5
		public void OnRequestedToSearchBattle()
		{
			this.IsSearchGameRequested = true;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000E9D0 File Offset: 0x0000CBD0
		public void OnUpdateFindingGame(MatchmakingWaitTimeStats matchmakingWaitTimeStats, string[] gameTypeInfo)
		{
			if (!this.IsSearchingGame)
			{
				this.IsSearchGameRequested = false;
				this.IsSearchingGame = true;
				this.GameSearch.SetEnabled(true);
				this.Armory.SetCanOpenFacegen(false);
				this.Matchmaking.OnFindingGame();
			}
			if (this.IsSearchingGame)
			{
				Action onForceCloseFacegen = this._onForceCloseFacegen;
				if (onForceCloseFacegen != null)
				{
					onForceCloseFacegen();
				}
			}
			if (gameTypeInfo == null)
			{
				this.Matchmaking.GetSelectedGameTypesInfo(out gameTypeInfo);
			}
			this.GameSearch.UpdateData(matchmakingWaitTimeStats, gameTypeInfo);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000EA50 File Offset: 0x0000CC50
		public void OnPremadeGameCreated()
		{
			this.IsSearchingGame = true;
			this.Matchmaking.OnFindingGame();
			this.GameSearch.UpdatePremadeGameData();
			this.GameSearch.SetEnabled(true);
			this.Clan.ClanOverview.AreActionButtonsEnabled = false;
			this.Armory.SetCanOpenFacegen(false);
			Action onForceCloseFacegen = this._onForceCloseFacegen;
			if (onForceCloseFacegen != null)
			{
				onForceCloseFacegen();
			}
			this.SetPage(MPLobbyVM.LobbyPage.Profile, MPMatchmakingVM.MatchmakingSubPages.Default);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000EABC File Offset: 0x0000CCBC
		public void OnRequestedToCancelSearchBattle()
		{
			this.GameSearch.OnRequestedToCancelSearchBattle();
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000EACC File Offset: 0x0000CCCC
		private void HandlePlayerRemovedFromParty(PlayerId playerID, PartyRemoveReason reason)
		{
			this.Friends.OnPlayerRemovedFromParty(playerID);
			if (NetworkMain.GameClient.ClanHomeInfo == null || !NetworkMain.GameClient.ClanHomeInfo.IsInClan)
			{
				this.RefreshClanInfo();
			}
			if (this.ClanCreationPopup.IsEnabled)
			{
				this.ClanCreationPopup.ExecuteClosePopup();
			}
			if (playerID == NetworkMain.GameClient.PlayerData.PlayerId && reason != PartyRemoveReason.DeclinedInvitation)
			{
				this.IsPartyLeader = false;
				this.IsInParty = false;
				TextObject textObject = GameTexts.FindText("str_youve_been_removed", null);
				if (reason == PartyRemoveReason.Left)
				{
					textObject = GameTexts.FindText("str_left_party", null);
				}
				else if (reason == PartyRemoveReason.Disband)
				{
					textObject = GameTexts.FindText("str_party_disbanded", null);
				}
				else if (reason == PartyRemoveReason.JoinRequestDeclined)
				{
					textObject = GameTexts.FindText("str_party_join_declined", null);
				}
				else if (reason == PartyRemoveReason.NoPlatformPermission)
				{
					textObject = GameTexts.FindText("str_party_join_permission_failed", null);
				}
				InformationManager.ShowInquiry(new InquiryData(string.Empty, textObject.ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), string.Empty, null, null, "", 0f, null, null, null), false, false);
			}
			this.GameSearch.UpdateCanCancel();
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000EBEC File Offset: 0x0000CDEC
		private void HandlePlayerAddedToParty(PlayerId playerID)
		{
			this.Friends.OnPlayerAddedToParty(playerID);
			if (NetworkMain.GameClient.ClanHomeInfo == null || !NetworkMain.GameClient.ClanHomeInfo.IsInClan)
			{
				this.RefreshClanInfo();
			}
			this.IsInParty = NetworkMain.GameClient.IsInParty;
			this.IsPartyLeader = NetworkMain.GameClient.IsPartyLeader;
			this.GameSearch.UpdateCanCancel();
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000EC53 File Offset: 0x0000CE53
		public void OnPlayerRemovedFromParty(PlayerId playerId, PartyRemoveReason reason)
		{
			this._partyActionQueue.Enqueue(new ValueTuple<MPLobbyVM.PartyActionType, PlayerId, PartyRemoveReason>(MPLobbyVM.PartyActionType.Remove, playerId, reason));
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000EC68 File Offset: 0x0000CE68
		public void OnPlayerAddedToParty(PlayerId playerId)
		{
			this._partyActionQueue.Enqueue(new ValueTuple<MPLobbyVM.PartyActionType, PlayerId, PartyRemoveReason>(MPLobbyVM.PartyActionType.Add, playerId, PartyRemoveReason.Kicked));
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000EC7D File Offset: 0x0000CE7D
		public void OnPlayerAssignedPartyLeader(PlayerId newPartyLeaderId)
		{
			this._partyActionQueue.Enqueue(new ValueTuple<MPLobbyVM.PartyActionType, PlayerId, PartyRemoveReason>(MPLobbyVM.PartyActionType.AssignLeader, newPartyLeaderId, PartyRemoveReason.Kicked));
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000EC94 File Offset: 0x0000CE94
		private void HandlePlayerAssignedPartyLeader(PlayerId playerID)
		{
			this.Friends.OnPlayerAssignedPartyLeader();
			this.IsInParty = NetworkMain.GameClient.IsInParty;
			this.IsPartyLeader = NetworkMain.GameClient.IsPartyLeader;
			this.GameSearch.UpdateCanCancel();
			this.Friends.UpdateCanInviteOtherPlayersToParty();
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000ECE2 File Offset: 0x0000CEE2
		public void OnPlayerSuggestedToParty(PlayerId playerId, string playerName, PlayerId suggestingPlayerId, string suggestingPlayerName)
		{
			this._partySuggestionQueue.Enqueue(new MPLobbyPartyPlayerSuggestionPopupVM.PlayerPartySuggestionData(playerId, playerName, suggestingPlayerId, suggestingPlayerName));
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000ECFC File Offset: 0x0000CEFC
		public void OnPlayerNameUpdated(string playerName)
		{
			MPLobbyHomeVM home = this.Home;
			if (home != null)
			{
				home.OnPlayerNameUpdated(playerName);
			}
			MPLobbyProfileVM profile = this.Profile;
			if (profile != null)
			{
				profile.OnPlayerNameUpdated(playerName);
			}
			MPLobbyClanVM clan = this.Clan;
			if (clan != null)
			{
				clan.OnPlayerNameUpdated(playerName);
			}
			MPLobbyClanCreationInformationVM clanCreationInformationPopup = this.ClanCreationInformationPopup;
			if (clanCreationInformationPopup == null)
			{
				return;
			}
			clanCreationInformationPopup.OnPlayerNameUpdated();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000ED50 File Offset: 0x0000CF50
		public void OnSearchBattleCanceled()
		{
			this.IsSearchGameRequested = false;
			this.IsSearchingGame = false;
			this.Clan.ClanOverview.AreActionButtonsEnabled = true;
			this.GameSearch.SetEnabled(false);
			this.Armory.SetCanOpenFacegen(true);
			this.Matchmaking.OnCancelFindingGame();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000EDA0 File Offset: 0x0000CFA0
		private async void RefreshPlayerDataInternal()
		{
			NetworkMain.GameClient.IsRefreshingPlayerData = true;
			if (NetworkMain.GameClient.Connected)
			{
				await NetworkMain.GameClient.GetCosmeticsInfo();
				this.Armory.RefreshPlayerData(this._playerDataToRefreshWith);
				this.Friends.Player.UpdateWith(this._playerDataToRefreshWith);
				this.BadgeSelectionPopup.RefreshPlayerData(this._playerDataToRefreshWith);
				this.Matchmaking.RefreshPlayerData(this._playerDataToRefreshWith);
			}
			if (NetworkMain.GameClient.Connected)
			{
				await NetworkMain.GameClient.GetClanHomeInfo();
				this.Home.RefreshPlayerData(this._playerDataToRefreshWith, true);
				this.Profile.UpdatePlayerData(this._playerDataToRefreshWith, true, true);
			}
			this._playerDataToRefreshWith = null;
			NetworkMain.GameClient.IsRefreshingPlayerData = false;
			this.RefreshSupportedFeatures();
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000EDD9 File Offset: 0x0000CFD9
		public void RefreshPlayerData(PlayerData playerData)
		{
			if (playerData != null)
			{
				if (this._playerDataToRefreshWith != playerData)
				{
					this._playerDataToRefreshWith = playerData;
					return;
				}
			}
			else
			{
				this.SetPage(MPLobbyVM.LobbyPage.Authentication, MPMatchmakingVM.MatchmakingSubPages.Default);
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000EDF8 File Offset: 0x0000CFF8
		public void RefreshSupportedFeatures()
		{
			SupportedFeatures supportedFeatures = this._lobbyClient.SupportedFeatures;
			this.Matchmaking.OnSupportedFeaturesRefreshed(supportedFeatures);
			this.Menu.OnSupportedFeaturesRefreshed(supportedFeatures);
			this.Friends.OnSupportedFeaturesRefreshed(supportedFeatures);
			if (!this.Menu.IsMatchmakingSupported)
			{
				this.DisallowedPages.Add(MPLobbyVM.LobbyPage.Matchmaking);
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000EE50 File Offset: 0x0000D050
		private void UpdateBlockerState()
		{
			LobbyClient.State currentState = this._lobbyClient.CurrentState;
			bool flag = NetworkMain.GameClient.IsRefreshingPlayerData || this._isDisconnecting || this._isRejoining || this._isStartingGameFind || this._isCustomGameCheckingForMaps || currentState == LobbyClient.State.AtBattle || currentState == LobbyClient.State.QuittingFromBattle || currentState == LobbyClient.State.WaitingToRegisterCustomGame || currentState == LobbyClient.State.HostingCustomGame || currentState == LobbyClient.State.WaitingToJoinCustomGame || currentState == LobbyClient.State.InCustomGame || this.Matchmaking.CustomServer.IsSearchingGamesToSpectate;
			if (flag && !this.BlockerState.IsEnabled)
			{
				TextObject textObject = new TextObject("{=Rc95Kq8r}Please wait...", null);
				this.BlockerState.OnLobbyStateIsBlocker(textObject);
				return;
			}
			if (!flag && this.BlockerState.IsEnabled)
			{
				this.BlockerState.OnLobbyStateNotBlocker();
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000EF0B File Offset: 0x0000D10B
		private void OnChangePageRequest(MPLobbyVM.LobbyPage page)
		{
			this.SetPage(page, MPMatchmakingVM.MatchmakingSubPages.Default);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000EF15 File Offset: 0x0000D115
		private void OnMatchSelectionChanged(string selectionInfo, bool isMatchFindPossible)
		{
			this.Home.OnMatchSelectionChanged(selectionInfo, isMatchFindPossible);
			this.Profile.OnMatchSelectionChanged(selectionInfo, isMatchFindPossible);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000EF31 File Offset: 0x0000D131
		private void OnGameFindStateChanged(bool isStartingGameFind)
		{
			this._isStartingGameFind = isStartingGameFind;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000EF3A File Offset: 0x0000D13A
		private void OnShowPlayerProfile(PlayerId playerID)
		{
			this.PlayerProfile.OpenWith(playerID);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000EF48 File Offset: 0x0000D148
		private void ExecuteShowBrightness()
		{
			this.BrightnessPopup.Visible = true;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000EF56 File Offset: 0x0000D156
		private void ExecuteShowExposure()
		{
			this.ExposurePopup.Visible = true;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000EF64 File Offset: 0x0000D164
		private void OpenClanCreationPopup()
		{
			this.ClanCreationPopup.ExecuteOpenPopup();
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000EF71 File Offset: 0x0000D171
		private void CloseClanCreationPopup()
		{
			this.ClanCreationPopup.ExecuteClosePopup();
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000EF7E File Offset: 0x0000D17E
		private void ExecuteOpenRecentGames()
		{
			this.RefreshRecentGames();
			this.RecentGames.ExecuteOpenPopup();
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000EF91 File Offset: 0x0000D191
		private void OpenInviteClanMemberPopup()
		{
			this.ClanInviteFriendsPopup.Open();
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000EF9E File Offset: 0x0000D19E
		private void OnBadgeProgressInfoRequested(MPLobbyAchievementBadgeGroupVM achivementGroup)
		{
			this.BadgeProgressionInformation.OpenWith(achivementGroup);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000EFAC File Offset: 0x0000D1AC
		private void OnBadgeNotificationRead()
		{
			this.Profile.HasBadgeNotification = this.BadgeSelectionPopup.HasNotifications;
			this.Menu.HasProfileNotification = this.BadgeSelectionPopup.HasNotifications;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000EFDC File Offset: 0x0000D1DC
		private void OnBadgeSelectionUpdated()
		{
			PlayerData playerData = NetworkMain.GameClient.PlayerData;
			if (playerData != null)
			{
				this.Home.RefreshPlayerData(playerData, false);
				this.Profile.UpdatePlayerData(playerData, false, false);
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000F012 File Offset: 0x0000D212
		private void OnBadgeChangeRequested(PlayerId playerID)
		{
			if (playerID == NetworkMain.GameClient.PlayerID)
			{
				this.BadgeSelectionPopup.Open();
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000F031 File Offset: 0x0000D231
		private void OnRankProgressionRequested(MPLobbyPlayerBaseVM player)
		{
			this.RankProgressInformation.OpenWith(player);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000F03F File Offset: 0x0000D23F
		private void OnRankLeaderboardRequested(string gameMode)
		{
			this.RankLeaderboard.OpenWith(gameMode);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000F04D File Offset: 0x0000D24D
		private void OnClanPageRequested()
		{
			if (NetworkMain.GameClient.IsInClan)
			{
				this.Clan.ExecuteOpenPopup();
				return;
			}
			this.ClanCreationInformationPopup.RefreshWith(NetworkMain.GameClient.ClanHomeInfo);
			this.ClanCreationInformationPopup.ExecuteOpenPopup();
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000F087 File Offset: 0x0000D287
		private void OnClanLeaderboardRequested()
		{
			this.ClanLeaderboardPopup.ExecuteOpenPopup();
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000F094 File Offset: 0x0000D294
		public void OnNotificationsReceived(LobbyNotification[] notifications)
		{
			List<LobbyNotification> list = notifications.Where<LobbyNotification>((LobbyNotification n) => n.Type == NotificationType.FriendRequest).ToList<LobbyNotification>();
			this.Friends.OnFriendRequestNotificationsReceived(list);
			foreach (LobbyNotification lobbyNotification in notifications)
			{
				if (lobbyNotification.Type == NotificationType.ClanAnnouncement)
				{
					this.Clan.OnNotificationReceived(lobbyNotification);
				}
				else if (lobbyNotification.Type == NotificationType.BadgeEarned)
				{
					this.Profile.OnNotificationReceived(lobbyNotification);
					this.BadgeSelectionPopup.OnNotificationReceived(lobbyNotification);
					if (!this.Profile.IsEnabled)
					{
						this.Menu.HasProfileNotification = this.BadgeSelectionPopup.HasNotifications;
					}
				}
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000F146 File Offset: 0x0000D346
		private void OnAddFriendWithBannerlordIDRequested(PlayerId playerID)
		{
			if (playerID == NetworkMain.GameClient.PlayerID)
			{
				this.BannerlordIDAddFriendPopup.ExecuteOpenPopup();
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000F165 File Offset: 0x0000D365
		private void OnBannerlordIDChangeRequested(PlayerId playerID)
		{
			if (playerID == NetworkMain.GameClient.PlayerID)
			{
				this.BannerlordIDChangePopup.ExecuteOpenPopup();
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000F184 File Offset: 0x0000D384
		private void OnItemObtainRequested(MPLobbyCosmeticSigilItemVM sigilItem)
		{
			this.CosmeticObtainPopup.OpenWith(sigilItem);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000F194 File Offset: 0x0000D394
		private void OnItemObtainRequested(MPArmoryCosmeticItemBaseVM cosmeticItem)
		{
			MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
			if ((mparmoryCosmeticClothingItemVM = cosmeticItem as MPArmoryCosmeticClothingItemVM) != null)
			{
				this.CosmeticObtainPopup.OpenWith(mparmoryCosmeticClothingItemVM);
				return;
			}
			MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM;
			if ((mparmoryCosmeticTauntItemVM = cosmeticItem as MPArmoryCosmeticTauntItemVM) != null)
			{
				this.CosmeticObtainPopup.OpenWith(mparmoryCosmeticTauntItemVM, this.Armory.HeroPreview.HeroVisual);
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000F1DE File Offset: 0x0000D3DE
		private void OnAnnouncementInspected(MPAnnouncementItemVM announcement)
		{
			this.InformationPopup.ShowInformation(announcement.Title, announcement.Description);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000F1F7 File Offset: 0x0000D3F7
		private void OnCustomGameMapCheckingStateChanged(bool isCheckingForMaps)
		{
			this._isCustomGameCheckingForMaps = isCheckingForMaps;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000F200 File Offset: 0x0000D400
		private void OnItemObtained(string cosmeticID, int finalLoot)
		{
			this.Armory.Cosmetics.OnItemObtained(cosmeticID, finalLoot);
			this.ChangeSigilPopup.OnLootUpdated(finalLoot);
			this.Home.Player.Loot = finalLoot;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000F231 File Offset: 0x0000D431
		private void OnSigilChangeRequested(PlayerId playerID)
		{
			if (playerID == NetworkMain.GameClient.PlayerID)
			{
				this.ChangeSigilPopup.Open();
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000F250 File Offset: 0x0000D450
		public void OnSigilChanged(int iconID)
		{
			this.Home.Player.Sigil.RefreshWith(iconID);
			MPLobbyPlayerBaseVM player = this.Profile.PlayerInfo.Player;
			if (player != null)
			{
				player.Sigil.RefreshWith(iconID);
			}
			Banner banner = Banner.CreateOneColoredEmptyBanner(0);
			BannerData bannerData = new BannerData(iconID, 0, 0, new Vec2(512f, 512f), new Vec2(764f, 764f), false, false, 0f);
			banner.AddIconData(bannerData);
			NetworkMain.GameClient.PlayerData.Sigil = banner.Serialize();
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000F2E5 File Offset: 0x0000D4E5
		public void OnClanCreationFinished()
		{
			this.ClanCreationPopup.IsWaiting = false;
			this.ClanCreationPopup.HasCreationStarted = false;
			this.ClanInvitationPopup.Close();
			this.ClanCreationPopup.ExecuteClosePopup();
			this.RefreshClanInfo();
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000F31B File Offset: 0x0000D51B
		public void OnEnableGenericAvatarsChanged()
		{
			this.OnFriendListUpdated(true);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000F324 File Offset: 0x0000D524
		public void OnEnableGenericNamesChanged()
		{
			this.OnFriendListUpdated(true);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000F330 File Offset: 0x0000D530
		public void OnFriendListUpdated(bool forceUpdate = false)
		{
			this._lobbyClient.FriendIDs.Clear();
			foreach (IFriendListService friendListService in PlatformServices.Instance.GetFriendListServices())
			{
				if (friendListService.IncludeInAllFriends)
				{
					this._lobbyClient.FriendIDs.AddRange(friendListService.GetAllFriends());
				}
			}
			this.Friends.OnFriendListUpdated(forceUpdate);
			this.RecentGames.OnFriendListUpdated(forceUpdate);
			MPLobbyClanCreationInformationVM clanCreationInformationPopup = this.ClanCreationInformationPopup;
			if (clanCreationInformationPopup != null)
			{
				clanCreationInformationPopup.OnFriendListUpdated(forceUpdate);
			}
			this.Home.Player.UpdateNameAndAvatar(forceUpdate);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000F3C4 File Offset: 0x0000D5C4
		public void OnClanInfoChanged()
		{
			this.Clan.OnClanInfoChanged();
			this.Friends.OnClanInfoChanged();
			this.Home.OnClanInfoChanged();
			this.Profile.OnClanInfoChanged();
			this.PlayerProfile.OnClanInfoChanged();
			if (this.Clan.IsEnabled && !NetworkMain.GameClient.ClanHomeInfo.IsInClan)
			{
				this.SetPage(MPLobbyVM.LobbyPage.Home, MPMatchmakingVM.MatchmakingSubPages.Default);
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000F430 File Offset: 0x0000D630
		private async void RefreshClanInfo()
		{
			await NetworkMain.GameClient.GetClanHomeInfo();
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000F461 File Offset: 0x0000D661
		private void OnRejoinRequested()
		{
			this._isRejoinRequested = true;
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000F46A File Offset: 0x0000D66A
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x0000F472 File Offset: 0x0000D672
		[DataSourceProperty]
		public bool IsLoggedIn
		{
			get
			{
				return this._isLoggedIn;
			}
			set
			{
				if (value != this._isLoggedIn)
				{
					this._isLoggedIn = value;
					base.OnPropertyChangedWithValue(value, "IsLoggedIn");
				}
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000F490 File Offset: 0x0000D690
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x0000F498 File Offset: 0x0000D698
		[DataSourceProperty]
		public BrightnessOptionVM BrightnessPopup
		{
			get
			{
				return this._brightnessPopup;
			}
			set
			{
				if (value != this._brightnessPopup)
				{
					this._brightnessPopup = value;
					base.OnPropertyChangedWithValue<BrightnessOptionVM>(value, "BrightnessPopup");
				}
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0000F4B6 File Offset: 0x0000D6B6
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x0000F4BE File Offset: 0x0000D6BE
		[DataSourceProperty]
		public ExposureOptionVM ExposurePopup
		{
			get
			{
				return this._exposurePopup;
			}
			set
			{
				if (value != this._exposurePopup)
				{
					this._exposurePopup = value;
					base.OnPropertyChangedWithValue<ExposureOptionVM>(value, "ExposurePopup");
				}
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0000F4DC File Offset: 0x0000D6DC
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x0000F4E4 File Offset: 0x0000D6E4
		[DataSourceProperty]
		public bool IsArmoryActive
		{
			get
			{
				return this._isArmoryActive;
			}
			set
			{
				if (value != this._isArmoryActive)
				{
					this._isArmoryActive = value;
					base.OnPropertyChangedWithValue(value, "IsArmoryActive");
				}
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0000F502 File Offset: 0x0000D702
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x0000F50A File Offset: 0x0000D70A
		[DataSourceProperty]
		public bool IsSearchGameRequested
		{
			get
			{
				return this._isSearchGameRequested;
			}
			set
			{
				if (value != this._isSearchGameRequested)
				{
					this._isSearchGameRequested = value;
					base.OnPropertyChangedWithValue(value, "IsSearchGameRequested");
				}
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000F528 File Offset: 0x0000D728
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x0000F530 File Offset: 0x0000D730
		[DataSourceProperty]
		public bool IsInParty
		{
			get
			{
				return this._isInParty;
			}
			set
			{
				if (value != this._isInParty)
				{
					this._isInParty = value;
					base.OnPropertyChangedWithValue(value, "IsInParty");
				}
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0000F54E File Offset: 0x0000D74E
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x0000F556 File Offset: 0x0000D756
		[DataSourceProperty]
		public bool IsSearchingGame
		{
			get
			{
				return this._isSearchingGame;
			}
			set
			{
				if (value != this._isSearchingGame)
				{
					this._isSearchingGame = value;
					base.OnPropertyChangedWithValue(value, "IsSearchingGame");
				}
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0000F574 File Offset: 0x0000D774
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x0000F57C File Offset: 0x0000D77C
		[DataSourceProperty]
		public bool IsMatchmakingEnabled
		{
			get
			{
				return this._isMatchmakingEnabled;
			}
			set
			{
				if (value != this._isMatchmakingEnabled)
				{
					this._isMatchmakingEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsMatchmakingEnabled");
				}
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000F59A File Offset: 0x0000D79A
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x0000F5A2 File Offset: 0x0000D7A2
		[DataSourceProperty]
		public bool IsCustomGameFindEnabled
		{
			get
			{
				return this._isCustomGameFindEnabled;
			}
			set
			{
				if (value != this._isCustomGameFindEnabled)
				{
					this._isCustomGameFindEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsCustomGameFindEnabled");
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x0000F5C8 File Offset: 0x0000D7C8
		[DataSourceProperty]
		public bool IsPartyLeader
		{
			get
			{
				return this._isPartyLeader;
			}
			set
			{
				if (value != this._isPartyLeader)
				{
					this._isPartyLeader = value;
					base.OnPropertyChangedWithValue(value, "IsPartyLeader");
				}
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0000F5E6 File Offset: 0x0000D7E6
		// (set) Token: 0x06000419 RID: 1049 RVA: 0x0000F5EE File Offset: 0x0000D7EE
		[DataSourceProperty]
		public MPLobbyBlockerStateVM BlockerState
		{
			get
			{
				return this._blockerState;
			}
			set
			{
				if (value != this._blockerState)
				{
					this._blockerState = value;
					base.OnPropertyChangedWithValue<MPLobbyBlockerStateVM>(value, "BlockerState");
				}
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0000F60C File Offset: 0x0000D80C
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x0000F614 File Offset: 0x0000D814
		[DataSourceProperty]
		public MPLobbyMenuVM Menu
		{
			get
			{
				return this._menu;
			}
			set
			{
				if (value != this._menu)
				{
					this._menu = value;
					base.OnPropertyChangedWithValue<MPLobbyMenuVM>(value, "Menu");
				}
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0000F632 File Offset: 0x0000D832
		// (set) Token: 0x0600041D RID: 1053 RVA: 0x0000F63A File Offset: 0x0000D83A
		[DataSourceProperty]
		public MPAuthenticationVM Login
		{
			get
			{
				return this._login;
			}
			set
			{
				if (value != this._login)
				{
					this._login = value;
					base.OnPropertyChangedWithValue<MPAuthenticationVM>(value, "Login");
				}
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0000F658 File Offset: 0x0000D858
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x0000F660 File Offset: 0x0000D860
		[DataSourceProperty]
		public MPLobbyRejoinVM Rejoin
		{
			get
			{
				return this._rejoin;
			}
			set
			{
				if (value != this._rejoin)
				{
					this._rejoin = value;
					base.OnPropertyChangedWithValue<MPLobbyRejoinVM>(value, "Rejoin");
				}
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000F67E File Offset: 0x0000D87E
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x0000F686 File Offset: 0x0000D886
		[DataSourceProperty]
		public MPLobbyFriendsVM Friends
		{
			get
			{
				return this._friends;
			}
			set
			{
				if (value != this._friends)
				{
					this._friends = value;
					base.OnPropertyChangedWithValue<MPLobbyFriendsVM>(value, "Friends");
				}
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0000F6A4 File Offset: 0x0000D8A4
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x0000F6AC File Offset: 0x0000D8AC
		[DataSourceProperty]
		public MPLobbyHomeVM Home
		{
			get
			{
				return this._home;
			}
			set
			{
				if (value != this._home)
				{
					this._home = value;
					base.OnPropertyChangedWithValue<MPLobbyHomeVM>(value, "Home");
				}
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x0000F6D2 File Offset: 0x0000D8D2
		[DataSourceProperty]
		public MPMatchmakingVM Matchmaking
		{
			get
			{
				return this._matchmaking;
			}
			set
			{
				if (value != this._matchmaking)
				{
					this._matchmaking = value;
					base.OnPropertyChangedWithValue<MPMatchmakingVM>(value, "Matchmaking");
				}
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000F6F0 File Offset: 0x0000D8F0
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x0000F6F8 File Offset: 0x0000D8F8
		[DataSourceProperty]
		public MPArmoryVM Armory
		{
			get
			{
				return this._armory;
			}
			set
			{
				if (value != this._armory)
				{
					this._armory = value;
					base.OnPropertyChangedWithValue<MPArmoryVM>(value, "Armory");
				}
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x0000F716 File Offset: 0x0000D916
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x0000F71E File Offset: 0x0000D91E
		[DataSourceProperty]
		public MPLobbyGameSearchVM GameSearch
		{
			get
			{
				return this._gameSearch;
			}
			set
			{
				if (value != this._gameSearch)
				{
					this._gameSearch = value;
					base.OnPropertyChangedWithValue<MPLobbyGameSearchVM>(value, "GameSearch");
				}
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0000F73C File Offset: 0x0000D93C
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x0000F744 File Offset: 0x0000D944
		[DataSourceProperty]
		public MPLobbyPlayerProfileVM PlayerProfile
		{
			get
			{
				return this._playerProfile;
			}
			set
			{
				if (value != this._playerProfile)
				{
					this._playerProfile = value;
					base.OnPropertyChangedWithValue<MPLobbyPlayerProfileVM>(value, "PlayerProfile");
				}
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x0000F762 File Offset: 0x0000D962
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x0000F76A File Offset: 0x0000D96A
		[DataSourceProperty]
		public MPAfterBattlePopupVM AfterBattlePopup
		{
			get
			{
				return this._afterBattlePopup;
			}
			set
			{
				if (value != this._afterBattlePopup)
				{
					this._afterBattlePopup = value;
					base.OnPropertyChangedWithValue<MPAfterBattlePopupVM>(value, "AfterBattlePopup");
				}
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x0000F788 File Offset: 0x0000D988
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x0000F790 File Offset: 0x0000D990
		[DataSourceProperty]
		public MPLobbyPartyInvitationPopupVM PartyInvitationPopup
		{
			get
			{
				return this._partyInvitationPopup;
			}
			set
			{
				if (value != this._partyInvitationPopup)
				{
					this._partyInvitationPopup = value;
					base.OnPropertyChangedWithValue<MPLobbyPartyInvitationPopupVM>(value, "PartyInvitationPopup");
				}
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x0000F7AE File Offset: 0x0000D9AE
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x0000F7B6 File Offset: 0x0000D9B6
		[DataSourceProperty]
		public MPLobbyPartyJoinRequestPopupVM PartyJoinRequestPopup
		{
			get
			{
				return this._partyJoinRequestPopup;
			}
			set
			{
				if (value != this._partyJoinRequestPopup)
				{
					this._partyJoinRequestPopup = value;
					base.OnPropertyChangedWithValue<MPLobbyPartyJoinRequestPopupVM>(value, "PartyJoinRequestPopup");
				}
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0000F7D4 File Offset: 0x0000D9D4
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x0000F7DC File Offset: 0x0000D9DC
		[DataSourceProperty]
		public MPLobbyInformationPopup InformationPopup
		{
			get
			{
				return this._informationPopup;
			}
			set
			{
				if (value != this._informationPopup)
				{
					this._informationPopup = value;
					base.OnPropertyChangedWithValue<MPLobbyInformationPopup>(value, "InformationPopup");
				}
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0000F7FA File Offset: 0x0000D9FA
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x0000F802 File Offset: 0x0000DA02
		[DataSourceProperty]
		public MPLobbyQueryPopupVM QueryPopup
		{
			get
			{
				return this._queryPopup;
			}
			set
			{
				if (value != this._queryPopup)
				{
					this._queryPopup = value;
					base.OnPropertyChangedWithValue<MPLobbyQueryPopupVM>(value, "QueryPopup");
				}
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000F820 File Offset: 0x0000DA20
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x0000F828 File Offset: 0x0000DA28
		[DataSourceProperty]
		public MPLobbyPartyPlayerSuggestionPopupVM PartyPlayerSuggestionPopup
		{
			get
			{
				return this._partyPlayerSuggestionPopup;
			}
			set
			{
				if (value != this._partyPlayerSuggestionPopup)
				{
					this._partyPlayerSuggestionPopup = value;
					base.OnPropertyChanged("PartyPlayerSuggestionPopup");
				}
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0000F845 File Offset: 0x0000DA45
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x0000F84D File Offset: 0x0000DA4D
		[DataSourceProperty]
		public MPOptionsVM Options
		{
			get
			{
				return this._options;
			}
			set
			{
				if (value != this._options)
				{
					this._options = value;
					base.OnPropertyChangedWithValue<MPOptionsVM>(value, "Options");
				}
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x0000F86B File Offset: 0x0000DA6B
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x0000F873 File Offset: 0x0000DA73
		[DataSourceProperty]
		public MPLobbyProfileVM Profile
		{
			get
			{
				return this._profile;
			}
			set
			{
				if (value != this._profile)
				{
					this._profile = value;
					base.OnPropertyChangedWithValue<MPLobbyProfileVM>(value, "Profile");
				}
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0000F891 File Offset: 0x0000DA91
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x0000F899 File Offset: 0x0000DA99
		[DataSourceProperty]
		public MPLobbyClanVM Clan
		{
			get
			{
				return this._clan;
			}
			set
			{
				if (value != this._clan)
				{
					this._clan = value;
					base.OnPropertyChanged("Clan");
				}
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x0000F8B6 File Offset: 0x0000DAB6
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x0000F8BE File Offset: 0x0000DABE
		[DataSourceProperty]
		public MPLobbyClanCreationPopupVM ClanCreationPopup
		{
			get
			{
				return this._clanCreationPopup;
			}
			set
			{
				if (value != this._clanCreationPopup)
				{
					this._clanCreationPopup = value;
					base.OnPropertyChanged("ClanCreationPopup");
				}
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0000F8DB File Offset: 0x0000DADB
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x0000F8E3 File Offset: 0x0000DAE3
		[DataSourceProperty]
		public MPLobbyClanCreationInformationVM ClanCreationInformationPopup
		{
			get
			{
				return this._clanCreationInformationPopup;
			}
			set
			{
				if (value != this._clanCreationInformationPopup)
				{
					this._clanCreationInformationPopup = value;
					base.OnPropertyChanged("ClanCreationInformationPopup");
				}
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x0000F900 File Offset: 0x0000DB00
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x0000F908 File Offset: 0x0000DB08
		[DataSourceProperty]
		public MPLobbyClanInvitationPopupVM ClanInvitationPopup
		{
			get
			{
				return this._clanInvitationPopup;
			}
			set
			{
				if (value != this._clanInvitationPopup)
				{
					this._clanInvitationPopup = value;
					base.OnPropertyChanged("ClanInvitationPopup");
				}
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x0000F925 File Offset: 0x0000DB25
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x0000F92D File Offset: 0x0000DB2D
		[DataSourceProperty]
		public MPLobbyClanMatchmakingRequestPopupVM ClanMatchmakingRequestPopup
		{
			get
			{
				return this._clanMatchmakingRequestPopup;
			}
			set
			{
				if (value != this._clanMatchmakingRequestPopup)
				{
					this._clanMatchmakingRequestPopup = value;
					base.OnPropertyChanged("ClanMatchmakingRequestPopup");
				}
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0000F94A File Offset: 0x0000DB4A
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x0000F952 File Offset: 0x0000DB52
		[DataSourceProperty]
		public MPLobbyClanInviteFriendsPopupVM ClanInviteFriendsPopup
		{
			get
			{
				return this._clanInviteFriendsPopup;
			}
			set
			{
				if (value != this._clanInviteFriendsPopup)
				{
					this._clanInviteFriendsPopup = value;
					base.OnPropertyChanged("ClanInviteFriendsPopup");
				}
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0000F96F File Offset: 0x0000DB6F
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x0000F977 File Offset: 0x0000DB77
		[DataSourceProperty]
		public MPLobbyClanLeaderboardVM ClanLeaderboardPopup
		{
			get
			{
				return this._clanLeaderboardPopup;
			}
			set
			{
				if (value != this._clanLeaderboardPopup)
				{
					this._clanLeaderboardPopup = value;
					base.OnPropertyChanged("ClanLeaderboardPopup");
				}
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0000F994 File Offset: 0x0000DB94
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0000F99C File Offset: 0x0000DB9C
		[DataSourceProperty]
		public MPCosmeticObtainPopupVM CosmeticObtainPopup
		{
			get
			{
				return this._cosmeticObtainPopup;
			}
			set
			{
				if (value != this._cosmeticObtainPopup)
				{
					this._cosmeticObtainPopup = value;
					base.OnPropertyChangedWithValue<MPCosmeticObtainPopupVM>(value, "CosmeticObtainPopup");
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0000F9BA File Offset: 0x0000DBBA
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x0000F9C2 File Offset: 0x0000DBC2
		[DataSourceProperty]
		public MPLobbyBannerlordIDAddFriendPopupVM BannerlordIDAddFriendPopup
		{
			get
			{
				return this._bannerlordIDAddFriendPopup;
			}
			set
			{
				if (value != this._bannerlordIDAddFriendPopup)
				{
					this._bannerlordIDAddFriendPopup = value;
					base.OnPropertyChanged("BannerlordIDAddFriendPopup");
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x0000F9DF File Offset: 0x0000DBDF
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x0000F9E7 File Offset: 0x0000DBE7
		[DataSourceProperty]
		public MPLobbyBannerlordIDChangePopup BannerlordIDChangePopup
		{
			get
			{
				return this._bannerlordIDChangePopup;
			}
			set
			{
				if (value != this._bannerlordIDChangePopup)
				{
					this._bannerlordIDChangePopup = value;
					base.OnPropertyChanged("BannerlordIDChangePopup");
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0000FA04 File Offset: 0x0000DC04
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x0000FA0C File Offset: 0x0000DC0C
		[DataSourceProperty]
		public MPLobbyBadgeProgressInformationVM BadgeProgressionInformation
		{
			get
			{
				return this._badgeProgressionInformation;
			}
			set
			{
				if (value != this._badgeProgressionInformation)
				{
					this._badgeProgressionInformation = value;
					base.OnPropertyChangedWithValue<MPLobbyBadgeProgressInformationVM>(value, "BadgeProgressionInformation");
				}
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0000FA2A File Offset: 0x0000DC2A
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x0000FA32 File Offset: 0x0000DC32
		[DataSourceProperty]
		public MPLobbyBadgeSelectionPopupVM BadgeSelectionPopup
		{
			get
			{
				return this._badgeSelectionPopup;
			}
			set
			{
				if (value != this._badgeSelectionPopup)
				{
					this._badgeSelectionPopup = value;
					base.OnPropertyChangedWithValue<MPLobbyBadgeSelectionPopupVM>(value, "BadgeSelectionPopup");
				}
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0000FA50 File Offset: 0x0000DC50
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x0000FA58 File Offset: 0x0000DC58
		[DataSourceProperty]
		public MPLobbyHomeChangeSigilPopupVM ChangeSigilPopup
		{
			get
			{
				return this._changeSigilPopup;
			}
			set
			{
				if (value != this._changeSigilPopup)
				{
					this._changeSigilPopup = value;
					base.OnPropertyChangedWithValue<MPLobbyHomeChangeSigilPopupVM>(value, "ChangeSigilPopup");
				}
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0000FA76 File Offset: 0x0000DC76
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x0000FA7E File Offset: 0x0000DC7E
		[DataSourceProperty]
		public MPLobbyRecentGamesVM RecentGames
		{
			get
			{
				return this._recentGames;
			}
			set
			{
				if (value != this._recentGames)
				{
					this._recentGames = value;
					base.OnPropertyChangedWithValue<MPLobbyRecentGamesVM>(value, "RecentGames");
				}
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0000FA9C File Offset: 0x0000DC9C
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
		[DataSourceProperty]
		public MPLobbyRankProgressInformationVM RankProgressInformation
		{
			get
			{
				return this._rankProgressInformation;
			}
			set
			{
				if (value != this._rankProgressInformation)
				{
					this._rankProgressInformation = value;
					base.OnPropertyChangedWithValue<MPLobbyRankProgressInformationVM>(value, "RankProgressInformation");
				}
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0000FAC2 File Offset: 0x0000DCC2
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x0000FACA File Offset: 0x0000DCCA
		[DataSourceProperty]
		public MPLobbyRankLeaderboardVM RankLeaderboard
		{
			get
			{
				return this._rankLeaderboard;
			}
			set
			{
				if (value != this._rankLeaderboard)
				{
					this._rankLeaderboard = value;
					base.OnPropertyChangedWithValue<MPLobbyRankLeaderboardVM>(value, "RankLeaderboard");
				}
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000FAE8 File Offset: 0x0000DCE8
		public static string GetLocalizedGameTypesString(string[] gameTypes)
		{
			if (gameTypes.Length == 0)
			{
				return GameTexts.FindText("str_multiplayer_official_game_type_name", "None").ToString();
			}
			string text = "";
			for (int i = 0; i < gameTypes.Length; i++)
			{
				text += GameTexts.FindText("str_multiplayer_official_game_type_name", gameTypes[i]).ToString();
				if (i != gameTypes.Length - 1)
				{
					text += ", ";
				}
			}
			return text;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000FB50 File Offset: 0x0000DD50
		public static string GetLocalizedRankName(string rankID)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(rankID);
			if (num <= 1440162539U)
			{
				if (num <= 1082301734U)
				{
					if (num != 616112491U)
					{
						if (num != 1048746496U)
						{
							if (num == 1082301734U)
							{
								if (rankID == "bronze3")
								{
									return new TextObject("{=Xsq7z0PG}Bronze III", null).ToString();
								}
							}
						}
						else if (rankID == "bronze1")
						{
							return new TextObject("{=CacPs8hA}Bronze I", null).ToString();
						}
					}
					else if (rankID == "general")
					{
						return new TextObject("{=mprankgeneral}General", null).ToString();
					}
				}
				else if (num <= 1099079353U)
				{
					if (num != 1087912039U)
					{
						if (num == 1099079353U)
						{
							if (rankID == "bronze2")
							{
								return new TextObject("{=e3IeNR9W}Bronze II", null).ToString();
							}
						}
					}
					else if (rankID == "conqueror")
					{
						return new TextObject("{=wwbIcqsq}Conqueror", null).ToString();
					}
				}
				else if (num != 1412953442U)
				{
					if (num == 1440162539U)
					{
						if (rankID == "captain")
						{
							return new TextObject("{=F70rOpkK}Captain", null).ToString();
						}
					}
				}
				else if (rankID == "sergeant")
				{
					return new TextObject("{=g9VIbA9s}Sergeant", null).ToString();
				}
			}
			else if (num <= 2260997974U)
			{
				if (num != 2166136261U)
				{
					if (num != 2244220355U)
					{
						if (num == 2260997974U)
						{
							if (rankID == "silver2")
							{
								return new TextObject("{=zpAamvDv}Silver II", null).ToString();
							}
						}
					}
					else if (rankID == "silver1")
					{
						return new TextObject("{=DUrxKsJj}Silver I", null).ToString();
					}
				}
				else if (rankID != null)
				{
					if (rankID.Length == 0)
					{
						return new TextObject("{=E3Bqugs0}Unranked", null).ToString();
					}
				}
			}
			else if (num <= 3320148192U)
			{
				if (num != 2277775593U)
				{
					if (num == 3320148192U)
					{
						if (rankID == "gold3")
						{
							return new TextObject("{=0FVlAbbJ}Gold III", null).ToString();
						}
					}
				}
				else if (rankID == "silver3")
				{
					return new TextObject("{=HGqvwRJt}Silver III", null).ToString();
				}
			}
			else if (num != 3336925811U)
			{
				if (num == 3353703430U)
				{
					if (rankID == "gold1")
					{
						return new TextObject("{=2faDtaGz}Gold I", null).ToString();
					}
				}
			}
			else if (rankID == "gold2")
			{
				return new TextObject("{=9hJtoWot}Gold II", null).ToString();
			}
			return string.Empty;
		}

		// Token: 0x040001DA RID: 474
		private LobbyClient _lobbyClient;

		// Token: 0x040001DB RID: 475
		private LobbyState _lobbyState;

		// Token: 0x040001DC RID: 476
		private const float PlayerCountInQueueTimerInterval = 10f;

		// Token: 0x040001DD RID: 477
		private float _playerCountInQueueTimer;

		// Token: 0x040001DF RID: 479
		private PlayerData _playerDataToRefreshWith;

		// Token: 0x040001E0 RID: 480
		private MBQueue<MPLobbyPartyPlayerSuggestionPopupVM.PlayerPartySuggestionData> _partySuggestionQueue;

		// Token: 0x040001E1 RID: 481
		private ConcurrentQueue<ValueTuple<MPLobbyVM.PartyActionType, PlayerId, PartyRemoveReason>> _partyActionQueue;

		// Token: 0x040001E2 RID: 482
		private bool _waitingForEscapeResult;

		// Token: 0x040001E3 RID: 483
		private bool _isCustomGameCheckingForMaps;

		// Token: 0x040001E4 RID: 484
		private bool _isDisconnecting;

		// Token: 0x040001E5 RID: 485
		private bool _isRejoinRequested;

		// Token: 0x040001E6 RID: 486
		private bool _isRejoining;

		// Token: 0x040001E7 RID: 487
		private bool _isStartingGameFind;

		// Token: 0x040001E8 RID: 488
		private bool? _cachedHasUserGeneratedContentPrivilege;

		// Token: 0x040001E9 RID: 489
		private readonly Action _onForceCloseFacegen;

		// Token: 0x040001EA RID: 490
		private readonly Action _onLogout;

		// Token: 0x040001EB RID: 491
		private readonly Action<bool> _onBusyStateChanged;

		// Token: 0x040001EC RID: 492
		private readonly Action<KeyOptionVM> _onKeybindRequest;

		// Token: 0x040001ED RID: 493
		private readonly Action<bool> _setNavigationRestriction;

		// Token: 0x040001EE RID: 494
		private const string _defaultSound = "event:/ui/default";

		// Token: 0x040001EF RID: 495
		private const string _tabSound = "event:/ui/tab";

		// Token: 0x040001F0 RID: 496
		private const string _sortSound = "event:/ui/sort";

		// Token: 0x040001F1 RID: 497
		private const string _purchaseSound = "event:/ui/multiplayer/shop_purchase_proceed";

		// Token: 0x040001F2 RID: 498
		private bool _isLoggedIn;

		// Token: 0x040001F3 RID: 499
		private bool _isArmoryActive;

		// Token: 0x040001F4 RID: 500
		private bool _isSearchGameRequested;

		// Token: 0x040001F5 RID: 501
		private bool _isSearchingGame;

		// Token: 0x040001F6 RID: 502
		private bool _isMatchmakingEnabled;

		// Token: 0x040001F7 RID: 503
		private bool _isCustomGameFindEnabled;

		// Token: 0x040001F8 RID: 504
		private bool _isPartyLeader;

		// Token: 0x040001F9 RID: 505
		private bool _isInParty;

		// Token: 0x040001FA RID: 506
		private MPLobbyBlockerStateVM _blockerState;

		// Token: 0x040001FB RID: 507
		private MPLobbyMenuVM _menu;

		// Token: 0x040001FC RID: 508
		private MPAuthenticationVM _login;

		// Token: 0x040001FD RID: 509
		private MPLobbyRejoinVM _rejoin;

		// Token: 0x040001FE RID: 510
		private MPLobbyFriendsVM _friends;

		// Token: 0x040001FF RID: 511
		private MPLobbyHomeVM _home;

		// Token: 0x04000200 RID: 512
		private MPMatchmakingVM _matchmaking;

		// Token: 0x04000201 RID: 513
		private MPArmoryVM _armory;

		// Token: 0x04000202 RID: 514
		private MPLobbyGameSearchVM _gameSearch;

		// Token: 0x04000203 RID: 515
		private MPLobbyPlayerProfileVM _playerProfile;

		// Token: 0x04000204 RID: 516
		private MPAfterBattlePopupVM _afterBattlePopup;

		// Token: 0x04000205 RID: 517
		private MPLobbyPartyInvitationPopupVM _partyInvitationPopup;

		// Token: 0x04000206 RID: 518
		private MPLobbyPartyJoinRequestPopupVM _partyJoinRequestPopup;

		// Token: 0x04000207 RID: 519
		private MPLobbyInformationPopup _informationPopup;

		// Token: 0x04000208 RID: 520
		private MPLobbyQueryPopupVM _queryPopup;

		// Token: 0x04000209 RID: 521
		private MPLobbyPartyPlayerSuggestionPopupVM _partyPlayerSuggestionPopup;

		// Token: 0x0400020A RID: 522
		private MPOptionsVM _options;

		// Token: 0x0400020B RID: 523
		private MPLobbyProfileVM _profile;

		// Token: 0x0400020C RID: 524
		private BrightnessOptionVM _brightnessPopup;

		// Token: 0x0400020D RID: 525
		private ExposureOptionVM _exposurePopup;

		// Token: 0x0400020E RID: 526
		private MPLobbyClanVM _clan;

		// Token: 0x0400020F RID: 527
		private MPLobbyClanCreationPopupVM _clanCreationPopup;

		// Token: 0x04000210 RID: 528
		private MPLobbyClanCreationInformationVM _clanCreationInformationPopup;

		// Token: 0x04000211 RID: 529
		private MPLobbyClanInvitationPopupVM _clanInvitationPopup;

		// Token: 0x04000212 RID: 530
		private MPLobbyClanMatchmakingRequestPopupVM _clanMatchmakingRequestPopup;

		// Token: 0x04000213 RID: 531
		private MPLobbyClanInviteFriendsPopupVM _clanInviteFriendsPopup;

		// Token: 0x04000214 RID: 532
		private MPLobbyClanLeaderboardVM _clanLeaderboardPopup;

		// Token: 0x04000215 RID: 533
		private MPCosmeticObtainPopupVM _cosmeticObtainPopup;

		// Token: 0x04000216 RID: 534
		private MPLobbyBannerlordIDChangePopup _bannerlordIDChangePopup;

		// Token: 0x04000217 RID: 535
		private MPLobbyBannerlordIDAddFriendPopupVM _bannerlordIDAddFriendPopup;

		// Token: 0x04000218 RID: 536
		private MPLobbyBadgeProgressInformationVM _badgeProgressionInformation;

		// Token: 0x04000219 RID: 537
		private MPLobbyBadgeSelectionPopupVM _badgeSelectionPopup;

		// Token: 0x0400021A RID: 538
		private MPLobbyHomeChangeSigilPopupVM _changeSigilPopup;

		// Token: 0x0400021B RID: 539
		private MPLobbyRecentGamesVM _recentGames;

		// Token: 0x0400021C RID: 540
		private MPLobbyRankProgressInformationVM _rankProgressInformation;

		// Token: 0x0400021D RID: 541
		private MPLobbyRankLeaderboardVM _rankLeaderboard;

		// Token: 0x020000DD RID: 221
		public enum LobbyPage
		{
			// Token: 0x04000887 RID: 2183
			NotAssigned,
			// Token: 0x04000888 RID: 2184
			Authentication,
			// Token: 0x04000889 RID: 2185
			Rejoin,
			// Token: 0x0400088A RID: 2186
			Options,
			// Token: 0x0400088B RID: 2187
			Home,
			// Token: 0x0400088C RID: 2188
			Armory,
			// Token: 0x0400088D RID: 2189
			Matchmaking,
			// Token: 0x0400088E RID: 2190
			Profile,
			// Token: 0x0400088F RID: 2191
			HotkeySelectablePageBegin = 3,
			// Token: 0x04000890 RID: 2192
			HotkeySelectablePageEnd = 7
		}

		// Token: 0x020000DE RID: 222
		private enum PartyActionType
		{
			// Token: 0x04000892 RID: 2194
			Add,
			// Token: 0x04000893 RID: 2195
			Remove,
			// Token: 0x04000894 RID: 2196
			AssignLeader
		}
	}
}
