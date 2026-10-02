using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Avatar.PlayerServices;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.Diamond.Ranked;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000059 RID: 89
	public class MPLobbyPlayerBaseVM : ViewModel
	{
		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x0001975E File Offset: 0x0001795E
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x00019766 File Offset: 0x00017966
		public MPLobbyPlayerBaseVM.OnlineStatus CurrentOnlineStatus { get; private set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0001976F File Offset: 0x0001796F
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x00019777 File Offset: 0x00017977
		public PlayerId ProvidedID
		{
			get
			{
				return this._providedID;
			}
			protected set
			{
				if (this._providedID != value)
				{
					this._providedID = value;
					LobbyClient gameClient = NetworkMain.GameClient;
					this.UpdateAvatar(gameClient != null && gameClient.IsKnownPlayer(this.ProvidedID));
				}
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x000197AB File Offset: 0x000179AB
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x000197B3 File Offset: 0x000179B3
		public PlayerData PlayerData { get; private set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x000197BC File Offset: 0x000179BC
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x000197C4 File Offset: 0x000179C4
		public AnotherPlayerState State { get; protected set; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x000197CD File Offset: 0x000179CD
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x000197D5 File Offset: 0x000179D5
		public float TimeSinceLastStateUpdate { get; protected set; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x000197DE File Offset: 0x000179DE
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x000197E6 File Offset: 0x000179E6
		public PlayerStatsBase[] PlayerStats { get; private set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x000197EF File Offset: 0x000179EF
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x000197F7 File Offset: 0x000179F7
		public GameTypeRankInfo[] RankInfo { get; private set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00019800 File Offset: 0x00017A00
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x00019808 File Offset: 0x00017A08
		public string RankInfoGameTypeID { get; private set; }

		// Token: 0x060007EF RID: 2031 RVA: 0x00019814 File Offset: 0x00017A14
		public MPLobbyPlayerBaseVM(PlayerId id, string forcedName = "", Action<PlayerId> onInviteToClan = null, Action<PlayerId> onFriendRequestAnswered = null)
		{
			this.ProvidedID = id;
			this._forcedName = forcedName;
			this.SetOnInvite(null);
			this._onInviteToClan = onInviteToClan;
			this._onFriendRequestAnswered = onFriendRequestAnswered;
			this.NameHint = new HintViewModel();
			this.ExperienceHint = new HintViewModel();
			this.RatingHint = new HintViewModel();
			LobbyClient gameClient = NetworkMain.GameClient;
			this.UpdateName(gameClient != null && gameClient.IsKnownPlayer(this.ProvidedID));
			this.CanBeInvited = true;
			this.CanInviteToParty = this._onInviteToParty != null;
			this.CanInviteToClan = this._onInviteToClan != null;
			PlatformServices.Instance.CheckPermissionWithUser(Permission.ViewUserGeneratedContent, id, delegate(bool hasBannerlordIDPrivilege)
			{
				this.CanCopyID = hasBannerlordIDPrivilege;
			});
			this.IsRankInfoLoading = true;
			this.GameTypes = new MBBindingList<MPLobbyGameTypeVM>();
			this.RefreshValues();
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0001990C File Offset: 0x00017B0C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ClanInfoTitleText = new TextObject("{=j4F7tTzy}Clan", null).ToString();
			this.BadgeInfoTitleText = new TextObject("{=4PrfimcK}Badge", null).ToString();
			this.AvatarInfoTitleText = new TextObject("{=5tbWdY1j}Avatar", null).ToString();
			this.ChangeText = new TextObject("{=Ba50zU7Z}Change", null).ToString();
			this.LevelTitleText = new TextObject("{=OKUTPdaa}Level", null).ToString();
			this.GameTypeText = new TextObject("{=JPimShCw}Game Type", null).ToString();
			this.InviteToPartyHint = new HintViewModel(new TextObject("{=aZnS9ECC}Invite", null), null);
			this.InviteToClanHint = new HintViewModel(new TextObject("{=fLddxLjh}Invite to Clan", null), null);
			this.RemoveFriendHint = new HintViewModel(new TextObject("{=d7ysGcsN}Remove Friend", null), null);
			this.AcceptFriendRequestHint = new HintViewModel(new TextObject("{=BSUteZmt}Accept Friend Request", null), null);
			this.DeclineFriendRequestHint = new HintViewModel(new TextObject("{=942B3LfA}Decline Friend Request", null), null);
			this.CancelFriendRequestHint = new HintViewModel(new TextObject("{=lGbrWyEe}Cancel Friend Request", null), null);
			this.LootHint = new HintViewModel(new TextObject("{=Th8q8wC2}Loot", null), null);
			this.ClanLeaderboardHint = new HintViewModel(new TextObject("{=JdEiK70R}Clan Leaderboard", null), null);
			this.ChangeBannerlordIDHint = new HintViewModel(new TextObject("{=ozREO8ev}Change Bannerlord ID", null), null);
			this.AddFriendWithBannerlordIDHint = new HintViewModel(new TextObject("{=tC9C8TLi}Add Friend", null), null);
			this.CopyBannerlordIDHint = new HintViewModel(new TextObject("{=Pwi1YCjH}Copy Bannerlord ID", null), null);
			MBBindingList<MPLobbyPlayerStatItemVM> displayedStats = this.DisplayedStats;
			if (displayedStats != null)
			{
				displayedStats.ApplyActionOnAllItems(delegate(MPLobbyPlayerStatItemVM s)
				{
					s.RefreshValues();
				});
			}
			MPLobbyBadgeItemVM shownBadge = this.ShownBadge;
			if (shownBadge == null)
			{
				return;
			}
			shownBadge.RefreshValues();
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00019AE0 File Offset: 0x00017CE0
		public void RefreshSelectableGameTypes(bool isRankedOnly, Action<string> onRefreshed, string initialGameTypeID = "")
		{
			this.GameTypes.Clear();
			this.GameTypes.Add(new MPLobbyGameTypeVM("Skirmish", false, onRefreshed));
			this.GameTypes.Add(new MPLobbyGameTypeVM("Captain", false, onRefreshed));
			if (!isRankedOnly)
			{
				this.GameTypes.Add(new MPLobbyGameTypeVM("Duel", true, onRefreshed));
				this.GameTypes.Add(new MPLobbyGameTypeVM("TeamDeathmatch", true, onRefreshed));
				this.GameTypes.Add(new MPLobbyGameTypeVM("Siege", true, new Action<string>(this.UpdateDisplayedRankInfo)));
			}
			MPLobbyGameTypeVM mplobbyGameTypeVM = this.GameTypes.FirstOrDefault<MPLobbyGameTypeVM>((MPLobbyGameTypeVM gt) => gt.GameTypeID == initialGameTypeID);
			if (mplobbyGameTypeVM != null)
			{
				mplobbyGameTypeVM.IsSelected = true;
				return;
			}
			this.GameTypes[0].IsSelected = true;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00019BBC File Offset: 0x00017DBC
		private void UpdateForcedAvatarIndex(bool isKnownPlayer)
		{
			if (this.ProvidedID != NetworkMain.GameClient.PlayerID)
			{
				Game game = Game.Current;
				object obj;
				if (game == null)
				{
					obj = null;
				}
				else
				{
					GameStateManager gameStateManager = game.GameStateManager;
					obj = ((gameStateManager != null) ? gameStateManager.ActiveState : null);
				}
				LobbyState lobbyState = obj as LobbyState;
				bool flag;
				if (lobbyState == null)
				{
					flag = false;
				}
				else
				{
					bool? hasUserGeneratedContentPrivilege = lobbyState.HasUserGeneratedContentPrivilege;
					bool flag2 = false;
					flag = (hasUserGeneratedContentPrivilege.GetValueOrDefault() == flag2) & (hasUserGeneratedContentPrivilege != null);
				}
				if (flag)
				{
					this._forcedAvatarIndex = AvatarServices.GetForcedAvatarIndexOfPlayer(this.ProvidedID);
					return;
				}
			}
			if (!BannerlordConfig.EnableGenericAvatars || this.ProvidedID == NetworkMain.GameClient.PlayerID || isKnownPlayer)
			{
				this._forcedAvatarIndex = -1;
				return;
			}
			this._forcedAvatarIndex = AvatarServices.GetForcedAvatarIndexOfPlayer(this.ProvidedID);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00019C74 File Offset: 0x00017E74
		protected async void UpdateName(bool isKnownPlayer)
		{
			string genericName = this._genericPlayerName.ToString();
			this.Name = genericName;
			if (this.ProvidedID != NetworkMain.GameClient.PlayerID)
			{
				Game game = Game.Current;
				object obj;
				if (game == null)
				{
					obj = null;
				}
				else
				{
					GameStateManager gameStateManager = game.GameStateManager;
					obj = ((gameStateManager != null) ? gameStateManager.ActiveState : null);
				}
				LobbyState lobbyState = obj as LobbyState;
				bool flag;
				if (lobbyState == null)
				{
					flag = false;
				}
				else
				{
					bool? hasUserGeneratedContentPrivilege = lobbyState.HasUserGeneratedContentPrivilege;
					bool flag2 = false;
					flag = (hasUserGeneratedContentPrivilege.GetValueOrDefault() == flag2) & (hasUserGeneratedContentPrivilege != null);
				}
				if (flag && this.ProvidedID.ProvidedType != PlayerIdProvidedTypes.PS && this.ProvidedID.ProvidedType != PlayerIdProvidedTypes.GDK)
				{
					this.Name = genericName;
					goto IL_0279;
				}
			}
			if (this._forcedName != string.Empty && !BannerlordConfig.EnableGenericNames)
			{
				this.Name = this._forcedName;
			}
			else if (this.ProvidedID == NetworkMain.GameClient.PlayerID)
			{
				this.Name = NetworkMain.GameClient.Name;
			}
			else if (!isKnownPlayer && BannerlordConfig.EnableGenericNames)
			{
				this.Name = genericName;
			}
			else if (this.PlayerData != null)
			{
				string lastPlayerName = this.PlayerData.LastPlayerName;
				this.Name = lastPlayerName;
			}
			else if (this.ProvidedID.IsValid)
			{
				IFriendListService[] friendListServices = PlatformServices.Instance.GetFriendListServices();
				string foundName = genericName;
				for (int i = friendListServices.Length - 1; i >= 0; i--)
				{
					string text = await friendListServices[i].GetUserName(this.ProvidedID);
					if (!string.IsNullOrEmpty(text) && text != "-" && text != genericName)
					{
						foundName = text;
						break;
					}
				}
				this.Name = foundName;
				friendListServices = null;
				foundName = null;
			}
			IL_0279:
			this.NameHint.HintText = new TextObject("{=!}" + this.Name, null);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00019CB5 File Offset: 0x00017EB5
		protected void UpdateAvatar(bool isKnownPlayer)
		{
			this.UpdateForcedAvatarIndex(isKnownPlayer);
			this.Avatar = new PlayerAvatarImageIdentifierVM(this.ProvidedID, this._forcedAvatarIndex);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00019CD8 File Offset: 0x00017ED8
		public void UpdatePlayerState(AnotherPlayerData playerData)
		{
			if (playerData == null)
			{
				return;
			}
			if (playerData.PlayerState != AnotherPlayerState.NoAnswer)
			{
				this.State = playerData.PlayerState;
				this.StateText = GameTexts.FindText("str_multiplayer_lobby_state", this.State.ToString()).ToString();
			}
			this._spectatableCustomBattleId = playerData.SpectatableCustomBattleId;
			this.CanSpectate = this._spectatableCustomBattleId != null;
			this.TimeSinceLastStateUpdate = Game.Current.ApplicationTime;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00019D54 File Offset: 0x00017F54
		public virtual void UpdateWith(PlayerData playerData)
		{
			if (playerData == null)
			{
				Debug.FailedAssert("PlayerData shouldn't be null at this stage!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Friends\\MPLobbyPlayerBaseVM.cs", "UpdateWith", 283);
				return;
			}
			this.PlayerData = playerData;
			this.ProvidedID = this.PlayerData.PlayerId;
			this.UpdateNameAndAvatar(true);
			this.UpdateExperienceData();
			if (NetworkMain.GameClient != null && NetworkMain.GameClient.SupportedFeatures.SupportsFeatures(Features.Clan))
			{
				this.IsClanInfoSupported = true;
			}
			else
			{
				this.IsClanInfoSupported = false;
			}
			this.Loot = playerData.Gold;
			this.Sigil = new MPLobbySigilItemVM();
			this.Sigil.RefreshWith(playerData.Sigil);
			this.ShownBadge = new MPLobbyBadgeItemVM(BadgeManager.GetById(playerData.ShownBadgeId), null, (Badge badge) => true, null);
			this.BannerlordID = string.Format("{0}#{1}", playerData.Username, playerData.UserId);
			this.SelectedBadgeID = playerData.ShownBadgeId;
			this.StateText = "";
			this._spectatableCustomBattleId = null;
			this.CanSpectate = false;
			this._hasReceivedPlayerStats = false;
			this._isReceivingPlayerStats = false;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00019E84 File Offset: 0x00018084
		public void UpdateNameAndAvatar(bool forceUpdate = false)
		{
			bool flag = NetworkMain.GameClient.IsKnownPlayer(this.ProvidedID);
			if (this._isKnownPlayer != flag || forceUpdate)
			{
				this._isKnownPlayer = flag;
				this.UpdateAvatar(flag);
				this.UpdateName(flag);
			}
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00019EC8 File Offset: 0x000180C8
		public void OnStatusChanged(MPLobbyPlayerBaseVM.OnlineStatus status, bool isInGameStatusActive)
		{
			this.CurrentOnlineStatus = status;
			this.StateText = "";
			this.TimeSinceLastStateUpdate = 0f;
			this.CanInviteToParty = this._onInviteToParty != null && (status == MPLobbyPlayerBaseVM.OnlineStatus.InGame || (status == MPLobbyPlayerBaseVM.OnlineStatus.Online && !isInGameStatusActive));
			this.ShowLevel = status == MPLobbyPlayerBaseVM.OnlineStatus.InGame || (status == MPLobbyPlayerBaseVM.OnlineStatus.Online && !isInGameStatusActive);
			this.CanInviteToClan = this._onInviteToClan != null && status == MPLobbyPlayerBaseVM.OnlineStatus.Online;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00019F42 File Offset: 0x00018142
		public void SetOnInvite(Action<PlayerId> onInvite)
		{
			this._onInviteToParty = onInvite;
			this.CanInviteToParty = onInvite != null;
			this.RefreshValues();
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00019F5C File Offset: 0x0001815C
		public async void UpdateStats(Action onDone)
		{
			if (!this._hasReceivedPlayerStats && !this._isReceivingPlayerStats)
			{
				this._isReceivingPlayerStats = true;
				PlayerStatsBase[] array = await NetworkMain.GameClient.GetPlayerStats(this.ProvidedID);
				this.PlayerStats = array;
				this._isReceivingPlayerStats = false;
				this._hasReceivedPlayerStats = this.PlayerStats != null;
				if (this._hasReceivedPlayerStats)
				{
					Action onPlayerStatsReceived = this.OnPlayerStatsReceived;
					if (onPlayerStatsReceived != null)
					{
						onPlayerStatsReceived();
					}
					if (onDone != null)
					{
						onDone();
					}
				}
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00019FA0 File Offset: 0x000181A0
		public void UpdateExperienceData()
		{
			this.Level = this.PlayerData.Level;
			int num = PlayerDataExperience.ExperienceRequiredForLevel(this.PlayerData.Level + 1);
			float num2 = (float)this.PlayerData.ExperienceInCurrentLevel / (float)num;
			this.ExperienceRatio = (int)(num2 * 100f);
			string text = this.PlayerData.ExperienceInCurrentLevel + " / " + num;
			this.ExperienceHint.HintText = new TextObject("{=!}" + text, null);
			TextObject textObject = new TextObject("{=5Z0pvuNL}Level {LEVEL}", null);
			textObject.SetTextVariable("LEVEL", this.Level);
			this.LevelText = textObject.ToString();
			int experienceToNextLevel = this.PlayerData.ExperienceToNextLevel;
			TextObject textObject2 = new TextObject("{=NUSH5bJu}{EXPERIENCE} exp to next level", null);
			textObject2.SetTextVariable("EXPERIENCE", experienceToNextLevel);
			this.ExperienceText = textObject2.ToString();
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001A08C File Offset: 0x0001828C
		public async void UpdateRating(Action onDone)
		{
			this.IsRankInfoLoading = true;
			GameTypeRankInfo[] array = await NetworkMain.GameClient.GetGameTypeRankInfo(this.ProvidedID);
			this.RankInfo = array;
			this.IsRankInfoLoading = false;
			if (onDone != null)
			{
				onDone();
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001A0D0 File Offset: 0x000182D0
		public void UpdateDisplayedRankInfo(string gameType)
		{
			GameTypeRankInfo gameTypeRankInfo = null;
			if (gameType == "Skirmish")
			{
				GameTypeRankInfo[] rankInfo = this.RankInfo;
				GameTypeRankInfo gameTypeRankInfo2;
				if (rankInfo == null)
				{
					gameTypeRankInfo2 = null;
				}
				else
				{
					gameTypeRankInfo2 = rankInfo.FirstOrDefault<GameTypeRankInfo>((GameTypeRankInfo r) => r.GameType == "Skirmish");
				}
				gameTypeRankInfo = gameTypeRankInfo2;
				this.RankInfoGameTypeID = "Skirmish";
			}
			else if (gameType == "Captain")
			{
				GameTypeRankInfo[] rankInfo2 = this.RankInfo;
				GameTypeRankInfo gameTypeRankInfo3;
				if (rankInfo2 == null)
				{
					gameTypeRankInfo3 = null;
				}
				else
				{
					gameTypeRankInfo3 = rankInfo2.FirstOrDefault<GameTypeRankInfo>((GameTypeRankInfo r) => r.GameType == "Captain");
				}
				gameTypeRankInfo = gameTypeRankInfo3;
				this.RankInfoGameTypeID = "Captain";
			}
			if (gameTypeRankInfo != null)
			{
				RankBarInfo rankBarInfo = gameTypeRankInfo.RankBarInfo;
				this.Rating = rankBarInfo.Rating;
				this.RatingID = rankBarInfo.RankId;
				this.RatingText = MPLobbyVM.GetLocalizedRankName(this.RatingID);
				if (rankBarInfo.IsEvaluating)
				{
					TextObject textObject = new TextObject("{=Ise5gWw3}{PLAYED_GAMES} / {TOTAL_GAMES} Evaluation matches played", null);
					textObject.SetTextVariable("PLAYED_GAMES", rankBarInfo.EvaluationMatchesPlayed);
					textObject.SetTextVariable("TOTAL_GAMES", rankBarInfo.TotalEvaluationMatchesRequired);
					this.RankText = textObject.ToString();
					this.RatingRatio = MathF.Floor((float)rankBarInfo.EvaluationMatchesPlayed / (float)rankBarInfo.TotalEvaluationMatchesRequired * 100f);
				}
				else
				{
					TextObject textObject2 = new TextObject("{=BUOtUW1u}{RATING} Points", null);
					textObject2.SetTextVariable("RATING", rankBarInfo.Rating);
					this.RankText = textObject2.ToString();
					this.RatingRatio = (string.IsNullOrEmpty(rankBarInfo.NextRankId) ? 100 : MathF.Floor(rankBarInfo.ProgressPercentage));
				}
				GameTexts.SetVariable("NUMBER", this.RatingRatio.ToString("0.00"));
				this.RatingHint.HintText = GameTexts.FindText("str_NUMBER_percent", null);
			}
			else
			{
				this.Rating = 0;
				this.RatingRatio = 0;
				this.RatingID = "norank";
				this.RatingText = new TextObject("{=GXosklej}Casual", null).ToString();
				this.RankText = new TextObject("{=56FyokuX}Game mode is casual", null).ToString();
				this.RatingHint.HintText = TextObject.GetEmpty();
			}
			Action<string> onRankInfoChanged = this.OnRankInfoChanged;
			if (onRankInfoChanged != null)
			{
				onRankInfoChanged(gameType);
			}
			this.IsRankInfoCasual = gameType != "Skirmish" && gameType != "Captain";
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001A31C File Offset: 0x0001851C
		public async void UpdateClanInfo()
		{
			if (!(this.ProvidedID == PlayerId.Empty))
			{
				bool isSelfPlayer = this.ProvidedID == NetworkMain.GameClient.PlayerID;
				ClanInfo clanInfo;
				if (isSelfPlayer)
				{
					clanInfo = NetworkMain.GameClient.ClanInfo;
				}
				else
				{
					clanInfo = await NetworkMain.GameClient.GetPlayerClanInfo(this.ProvidedID);
				}
				if (clanInfo != null && (isSelfPlayer || (!isSelfPlayer && clanInfo.Players.Length != 0)))
				{
					this.ClanBanner = new BannerImageIdentifierVM(new Banner(clanInfo.Sigil), true);
					this.ClanName = clanInfo.Name;
					GameTexts.SetVariable("STR", clanInfo.Tag);
					this.ClanTag = new TextObject("{=uTXYEAOg}[{STR}]", null).ToString();
				}
				else
				{
					this.ClanBanner = new BannerImageIdentifierVM(Banner.CreateOneColoredEmptyBanner(99), false);
					this.ClanName = new TextObject("{=0DnHFlia}Not In a Clan", null).ToString();
					this.ClanTag = string.Empty;
				}
			}
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0001A358 File Offset: 0x00018558
		public void FilterStatsForGameMode(string gameModeCode)
		{
			if (this.PlayerStats == null)
			{
				return;
			}
			if (this.DisplayedStats == null)
			{
				this.DisplayedStats = new MBBindingList<MPLobbyPlayerStatItemVM>();
			}
			this.DisplayedStats.Clear();
			IEnumerable<PlayerStatsBase> enumerable = this.PlayerStats.Where<PlayerStatsBase>((PlayerStatsBase s) => s.GameType == gameModeCode);
			foreach (PlayerStatsBase playerStatsBase in enumerable)
			{
				if (gameModeCode == "Skirmish" || gameModeCode == "Captain")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=WW2N3zJf}Wins", null), playerStatsBase.WinCount));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=4nr9Km6t}Losses", null), playerStatsBase.LoseCount));
				}
				if (gameModeCode == "Skirmish")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=ab2cbidI}Total Score", null), (playerStatsBase as PlayerStatsSkirmish).Score));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=fdR3xpBS}MVP Badges", null), (playerStatsBase as PlayerStatsSkirmish).MVPs));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), playerStatsBase.AverageKillPerDeath));
				}
				else if (gameModeCode == "Captain")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=ab2cbidI}Total Score", null), (playerStatsBase as PlayerStatsCaptain).Score));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=fdR3xpBS}MVP Badges", null), (playerStatsBase as PlayerStatsCaptain).MVPs));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=9FSk2daF}Captains Killed", null), (playerStatsBase as PlayerStatsCaptain).CaptainsKilled));
				}
				else if (gameModeCode == "Siege")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=ab2cbidI}Total Score", null), (playerStatsBase as PlayerStatsSiege).Score));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=XKWGPrYt}Siege Engines Destroyed", null), (playerStatsBase as PlayerStatsSiege).SiegeEnginesDestroyed));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=7APa598U}Kills With a Siege Engine", null), (playerStatsBase as PlayerStatsSiege).SiegeEngineKills));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=FaKWQccs}Gold Gained From Objectives", null), (playerStatsBase as PlayerStatsSiege).ObjectiveGoldGained));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), playerStatsBase.AverageKillPerDeath));
				}
				else if (gameModeCode == "Duel")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=SS5WyUWR}Duels Won", null), (playerStatsBase as PlayerStatsDuel).DuelsWon));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=Iu2eFSsh}Infantry Wins", null), (playerStatsBase as PlayerStatsDuel).InfantryWins));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=wyKhcvbd}Ranged Wins", null), (playerStatsBase as PlayerStatsDuel).ArcherWins));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=qipBkhys}Cavalry Wins", null), (playerStatsBase as PlayerStatsDuel).CavalryWins));
				}
				else if (gameModeCode == "TeamDeathmatch")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=ab2cbidI}Total Score", null), (playerStatsBase as PlayerStatsTeamDeathmatch).Score));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=9ET13VOe}Average Score", null), (playerStatsBase as PlayerStatsTeamDeathmatch).AverageScore));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), playerStatsBase.AverageKillPerDeath));
				}
				if (gameModeCode != "Duel")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=FKe05WtJ}Kills", null), playerStatsBase.KillCount));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=8eZFlPVu}Deaths", null), playerStatsBase.DeathCount));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=1imGhhZl}Assists", null), playerStatsBase.AssistCount));
				}
			}
			if (enumerable.IsEmpty<PlayerStatsBase>())
			{
				if (gameModeCode == "Skirmish" || gameModeCode == "Captain")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=WW2N3zJf}Wins", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=4nr9Km6t}Losses", null), "-"));
				}
				if (gameModeCode == "Skirmish")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=ab2cbidI}Total Score", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=fdR3xpBS}MVP Badges", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), "-"));
				}
				else if (gameModeCode == "Captain")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=ab2cbidI}Total Score", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=fdR3xpBS}MVP Badges", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=9FSk2daF}Captains Killed", null), "-"));
				}
				else if (gameModeCode == "Siege")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=ab2cbidI}Total Score", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=XKWGPrYt}Siege Engines Destroyed", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=7APa598U}Kills With a Siege Engine", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=FaKWQccs}Gold Gained From Objectives", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), "-"));
				}
				else if (gameModeCode == "Duel")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=SS5WyUWR}Duels Won", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=Iu2eFSsh}Infantry Wins", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=wyKhcvbd}Ranged Wins", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=qipBkhys}Cavalry Wins", null), "-"));
				}
				else if (gameModeCode == "TeamDeathmatch")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=ab2cbidI}Total Score", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=9ET13VOe}Average Score", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), "-"));
				}
				if (gameModeCode != "Duel")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=FKe05WtJ}Kills", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=8eZFlPVu}Deaths", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=1imGhhZl}Assists", null), "-"));
				}
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001AC94 File Offset: 0x00018E94
		public void RefreshCharacterVisual()
		{
			this.CharacterVisual = new CharacterViewModel();
			BasicCharacterObject @object = MBObjectManager.Instance.GetObject<BasicCharacterObject>("mp_character");
			@object.UpdatePlayerCharacterBodyProperties(this.PlayerData.BodyProperties, this.PlayerData.Race, this.PlayerData.IsFemale);
			this.CharacterVisual.FillFrom(@object, -1, null);
			this.CharacterVisual.BodyProperties = new BodyProperties(this.PlayerData.BodyProperties.DynamicProperties, @object.BodyPropertyRange.BodyPropertyMin.StaticProperties).ToString();
			this.CharacterVisual.IsFemale = this.PlayerData.IsFemale;
			this.CharacterVisual.Race = this.PlayerData.Race;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001AD61 File Offset: 0x00018F61
		public void ExecuteSelectPlayer()
		{
			this.IsSelected = !this.IsSelected;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001AD72 File Offset: 0x00018F72
		public void ExecuteInviteToParty()
		{
			Action<PlayerId> onInviteToParty = this._onInviteToParty;
			if (onInviteToParty == null)
			{
				return;
			}
			onInviteToParty(this.ProvidedID);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001AD8A File Offset: 0x00018F8A
		public void ExecuteSpectateMatch()
		{
			if (this._spectatableCustomBattleId == null)
			{
				return;
			}
			this.SpectateMatchAsync(this._spectatableCustomBattleId.Value);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001ADAC File Offset: 0x00018FAC
		private async void SpectateMatchAsync(CustomBattleId battleId)
		{
			TaskAwaiter<bool> taskAwaiter = NetworkMain.GameClient.RequestJoinCustomGame(battleId, CustomGameJoinType.Spectator, null).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				InformationManager.ShowInquiry(new InquiryData("", GameTexts.FindText("str_couldnt_join_server", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001ADE5 File Offset: 0x00018FE5
		public void ExecuteInviteToClan()
		{
			Action<PlayerId> onInviteToClan = this._onInviteToClan;
			if (onInviteToClan == null)
			{
				return;
			}
			onInviteToClan(this.ProvidedID);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001ADFD File Offset: 0x00018FFD
		public void ExecuteKickFromParty()
		{
			if (NetworkMain.GameClient.IsInParty && NetworkMain.GameClient.IsPartyLeader)
			{
				NetworkMain.GameClient.KickPlayerFromParty(this.ProvidedID);
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001AE28 File Offset: 0x00019028
		public void ExecuteAcceptFriendRequest()
		{
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(this.ProvidedID);
			NetworkMain.GameClient.RespondToFriendRequest(this.ProvidedID, flag, true, false);
			Action<PlayerId> onFriendRequestAnswered = this._onFriendRequestAnswered;
			if (onFriendRequestAnswered != null)
			{
				onFriendRequestAnswered(this.ProvidedID);
			}
			if (this.HasNotification)
			{
				this.HasNotification = false;
			}
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001AE8C File Offset: 0x0001908C
		public void ExecuteDeclineFriendRequest()
		{
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(this.ProvidedID);
			NetworkMain.GameClient.RespondToFriendRequest(this.ProvidedID, flag, false, false);
			Action<PlayerId> onFriendRequestAnswered = this._onFriendRequestAnswered;
			if (onFriendRequestAnswered != null)
			{
				onFriendRequestAnswered(this.ProvidedID);
			}
			if (this.HasNotification)
			{
				this.HasNotification = false;
			}
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001AEF0 File Offset: 0x000190F0
		public void ExecuteCancelPendingFriendRequest()
		{
			NetworkMain.GameClient.RemoveFriend(this.ProvidedID);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001AF02 File Offset: 0x00019102
		public void ExecuteRemoveFriend()
		{
			NetworkMain.GameClient.RemoveFriend(this.ProvidedID);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001AF14 File Offset: 0x00019114
		public void ExecuteCopyBannerlordID()
		{
			Input.SetClipboardText(this.BannerlordID);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001AF24 File Offset: 0x00019124
		private void ExecuteAddFriend()
		{
			string[] array = this.BannerlordID.Split(new char[] { '#' });
			string text = array[0];
			int num;
			if (int.TryParse(array[1], out num))
			{
				bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(this.ProvidedID);
				NetworkMain.GameClient.AddFriendByUsernameAndId(text, num, flag);
			}
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001AF81 File Offset: 0x00019181
		public void ExecuteShowProfile()
		{
			Action<PlayerId> onPlayerProfileRequested = MPLobbyPlayerBaseVM.OnPlayerProfileRequested;
			if (onPlayerProfileRequested == null)
			{
				return;
			}
			onPlayerProfileRequested(this.ProvidedID);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001AF98 File Offset: 0x00019198
		private void ExecuteActivateSigilChangeInformation()
		{
			this.IsSigilChangeInformationEnabled = true;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001AFA1 File Offset: 0x000191A1
		private void ExecuteDeactivateSigilChangeInformation()
		{
			this.IsSigilChangeInformationEnabled = false;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0001AFAA File Offset: 0x000191AA
		private void ExecuteChangeSigil()
		{
			Action<PlayerId> onSigilChangeRequested = MPLobbyPlayerBaseVM.OnSigilChangeRequested;
			if (onSigilChangeRequested == null)
			{
				return;
			}
			onSigilChangeRequested(this.ProvidedID);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0001AFC1 File Offset: 0x000191C1
		private void ExecuteChangeBannerlordID()
		{
			Action<PlayerId> onBannerlordIDChangeRequested = MPLobbyPlayerBaseVM.OnBannerlordIDChangeRequested;
			if (onBannerlordIDChangeRequested == null)
			{
				return;
			}
			onBannerlordIDChangeRequested(this.ProvidedID);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001AFD8 File Offset: 0x000191D8
		private void ExecuteAddFriendWithBannerlordID()
		{
			Action<PlayerId> onAddFriendWithBannerlordIDRequested = MPLobbyPlayerBaseVM.OnAddFriendWithBannerlordIDRequested;
			if (onAddFriendWithBannerlordIDRequested == null)
			{
				return;
			}
			onAddFriendWithBannerlordIDRequested(this.ProvidedID);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001AFEF File Offset: 0x000191EF
		private void ExecuteChangeBadge()
		{
			Action<PlayerId> onBadgeChangeRequested = MPLobbyPlayerBaseVM.OnBadgeChangeRequested;
			if (onBadgeChangeRequested == null)
			{
				return;
			}
			onBadgeChangeRequested(this.ProvidedID);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0001B008 File Offset: 0x00019208
		private void ExecuteShowRankProgression()
		{
			MPLobbyGameTypeVM mplobbyGameTypeVM = this.GameTypes.FirstOrDefault<MPLobbyGameTypeVM>((MPLobbyGameTypeVM gt) => gt.IsSelected);
			if (mplobbyGameTypeVM != null && !mplobbyGameTypeVM.IsCasual)
			{
				Action<MPLobbyPlayerBaseVM> onRankProgressionRequested = MPLobbyPlayerBaseVM.OnRankProgressionRequested;
				if (onRankProgressionRequested == null)
				{
					return;
				}
				onRankProgressionRequested(this);
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001B060 File Offset: 0x00019260
		private void ExecuteShowRankLeaderboard()
		{
			MPLobbyGameTypeVM mplobbyGameTypeVM = this.GameTypes.FirstOrDefault<MPLobbyGameTypeVM>((MPLobbyGameTypeVM gt) => gt.IsSelected);
			if (mplobbyGameTypeVM != null && !mplobbyGameTypeVM.IsCasual)
			{
				Action<string> onRankLeaderboardRequested = MPLobbyPlayerBaseVM.OnRankLeaderboardRequested;
				if (onRankLeaderboardRequested == null)
				{
					return;
				}
				onRankLeaderboardRequested(mplobbyGameTypeVM.GameTypeID);
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001B0B8 File Offset: 0x000192B8
		private void ExecuteShowClanPage()
		{
			Action onClanPageRequested = MPLobbyPlayerBaseVM.OnClanPageRequested;
			if (onClanPageRequested == null)
			{
				return;
			}
			onClanPageRequested();
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001B0C9 File Offset: 0x000192C9
		private void ExecuteShowClanLeaderboard()
		{
			Action onClanLeaderboardRequested = MPLobbyPlayerBaseVM.OnClanLeaderboardRequested;
			if (onClanLeaderboardRequested == null)
			{
				return;
			}
			onClanLeaderboardRequested();
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x0001B0DA File Offset: 0x000192DA
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x0001B0E2 File Offset: 0x000192E2
		[DataSourceProperty]
		public bool CanCopyID
		{
			get
			{
				return this._canCopyID;
			}
			set
			{
				if (value != this._canCopyID)
				{
					this._canCopyID = value;
					base.OnPropertyChangedWithValue(value, "CanCopyID");
				}
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0001B100 File Offset: 0x00019300
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x0001B108 File Offset: 0x00019308
		[DataSourceProperty]
		public bool ShowLevel
		{
			get
			{
				return this._showLevel;
			}
			set
			{
				if (value != this._showLevel)
				{
					this._showLevel = value;
					base.OnPropertyChangedWithValue(value, "ShowLevel");
				}
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x0001B126 File Offset: 0x00019326
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x0001B12E File Offset: 0x0001932E
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x0001B14C File Offset: 0x0001934C
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x0001B154 File Offset: 0x00019354
		[DataSourceProperty]
		public bool HasNotification
		{
			get
			{
				return this._hasNotification;
			}
			set
			{
				if (value != this._hasNotification)
				{
					this._hasNotification = value;
					base.OnPropertyChangedWithValue(value, "HasNotification");
				}
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x0001B172 File Offset: 0x00019372
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x0001B17A File Offset: 0x0001937A
		[DataSourceProperty]
		public bool IsFriendRequest
		{
			get
			{
				return this._isFriendRequest;
			}
			set
			{
				if (value != this._isFriendRequest)
				{
					this._isFriendRequest = value;
					base.OnPropertyChangedWithValue(value, "IsFriendRequest");
				}
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x0001B198 File Offset: 0x00019398
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x0001B1A0 File Offset: 0x000193A0
		[DataSourceProperty]
		public bool IsPendingRequest
		{
			get
			{
				return this._isPendingRequest;
			}
			set
			{
				if (value != this._isPendingRequest)
				{
					this._isPendingRequest = value;
					base.OnPropertyChangedWithValue(value, "IsPendingRequest");
				}
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x0001B1BE File Offset: 0x000193BE
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x0001B1C6 File Offset: 0x000193C6
		[DataSourceProperty]
		public bool CanRemove
		{
			get
			{
				return this._canRemove;
			}
			set
			{
				if (value != this._canRemove)
				{
					this._canRemove = value;
					base.OnPropertyChangedWithValue(value, "CanRemove");
				}
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0001B1E4 File Offset: 0x000193E4
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x0001B1EC File Offset: 0x000193EC
		[DataSourceProperty]
		public bool CanBeInvited
		{
			get
			{
				return this._canBeInvited;
			}
			set
			{
				if (value != this._canBeInvited)
				{
					this._canBeInvited = value;
					base.OnPropertyChangedWithValue(value, "CanBeInvited");
				}
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x0001B20A File Offset: 0x0001940A
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x0001B212 File Offset: 0x00019412
		[DataSourceProperty]
		public bool CanInviteToParty
		{
			get
			{
				return this._canInviteToParty;
			}
			set
			{
				if (value != this._canInviteToParty)
				{
					this._canInviteToParty = value;
					base.OnPropertyChangedWithValue(value, "CanInviteToParty");
				}
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x0001B230 File Offset: 0x00019430
		// (set) Token: 0x0600082B RID: 2091 RVA: 0x0001B238 File Offset: 0x00019438
		[DataSourceProperty]
		public bool CanSpectate
		{
			get
			{
				return this._canSpectate;
			}
			set
			{
				if (value != this._canSpectate)
				{
					this._canSpectate = value;
					base.OnPropertyChangedWithValue(value, "CanSpectate");
				}
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x0001B256 File Offset: 0x00019456
		// (set) Token: 0x0600082D RID: 2093 RVA: 0x0001B25E File Offset: 0x0001945E
		[DataSourceProperty]
		public bool CanInviteToClan
		{
			get
			{
				return this._canInviteToClan;
			}
			set
			{
				if (value != this._canInviteToClan)
				{
					this._canInviteToClan = value;
					base.OnPropertyChangedWithValue(value, "CanInviteToClan");
				}
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x0001B27C File Offset: 0x0001947C
		// (set) Token: 0x0600082F RID: 2095 RVA: 0x0001B284 File Offset: 0x00019484
		[DataSourceProperty]
		public bool IsSigilChangeInformationEnabled
		{
			get
			{
				return this._isSigilChangeInformationEnabled;
			}
			set
			{
				if (value != this._isSigilChangeInformationEnabled)
				{
					this._isSigilChangeInformationEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsSigilChangeInformationEnabled");
				}
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x0001B2A2 File Offset: 0x000194A2
		// (set) Token: 0x06000831 RID: 2097 RVA: 0x0001B2AA File Offset: 0x000194AA
		[DataSourceProperty]
		public bool IsRankInfoLoading
		{
			get
			{
				return this._isRankInfoLoading;
			}
			set
			{
				if (value != this._isRankInfoLoading)
				{
					this._isRankInfoLoading = value;
					base.OnPropertyChangedWithValue(value, "IsRankInfoLoading");
				}
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x0001B2C8 File Offset: 0x000194C8
		// (set) Token: 0x06000833 RID: 2099 RVA: 0x0001B2D0 File Offset: 0x000194D0
		[DataSourceProperty]
		public bool IsRankInfoCasual
		{
			get
			{
				return this._isRankInfoCasual;
			}
			set
			{
				if (value != this._isRankInfoCasual)
				{
					this._isRankInfoCasual = value;
					base.OnPropertyChangedWithValue(value, "IsRankInfoCasual");
				}
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x0001B2EE File Offset: 0x000194EE
		// (set) Token: 0x06000835 RID: 2101 RVA: 0x0001B2F6 File Offset: 0x000194F6
		[DataSourceProperty]
		public bool IsClanInfoSupported
		{
			get
			{
				return this._isClanInfoSupported;
			}
			set
			{
				if (value != this._isClanInfoSupported)
				{
					this._isClanInfoSupported = value;
					base.OnPropertyChangedWithValue(value, "IsClanInfoSupported");
				}
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x0001B314 File Offset: 0x00019514
		// (set) Token: 0x06000837 RID: 2103 RVA: 0x0001B31C File Offset: 0x0001951C
		[DataSourceProperty]
		public bool IsBannerlordIDSupported
		{
			get
			{
				return this._isBannerlordIDSupported;
			}
			set
			{
				if (value != this._isBannerlordIDSupported)
				{
					this._isBannerlordIDSupported = value;
					base.OnPropertyChangedWithValue(value, "IsBannerlordIDSupported");
				}
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x0001B33A File Offset: 0x0001953A
		// (set) Token: 0x06000839 RID: 2105 RVA: 0x0001B342 File Offset: 0x00019542
		[DataSourceProperty]
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue(value, "Level");
				}
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x0001B360 File Offset: 0x00019560
		// (set) Token: 0x0600083B RID: 2107 RVA: 0x0001B368 File Offset: 0x00019568
		[DataSourceProperty]
		public int Rating
		{
			get
			{
				return this._rating;
			}
			set
			{
				if (value != this._rating)
				{
					this._rating = value;
					base.OnPropertyChangedWithValue(value, "Rating");
				}
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x0001B386 File Offset: 0x00019586
		// (set) Token: 0x0600083D RID: 2109 RVA: 0x0001B38E File Offset: 0x0001958E
		[DataSourceProperty]
		public int Loot
		{
			get
			{
				return this._loot;
			}
			set
			{
				if (value != this._loot)
				{
					this._loot = value;
					base.OnPropertyChangedWithValue(value, "Loot");
				}
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x0001B3AC File Offset: 0x000195AC
		// (set) Token: 0x0600083F RID: 2111 RVA: 0x0001B3B4 File Offset: 0x000195B4
		[DataSourceProperty]
		public int ExperienceRatio
		{
			get
			{
				return this._experienceRatio;
			}
			set
			{
				if (value != this._experienceRatio)
				{
					this._experienceRatio = value;
					base.OnPropertyChangedWithValue(value, "ExperienceRatio");
				}
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x0001B3D2 File Offset: 0x000195D2
		// (set) Token: 0x06000841 RID: 2113 RVA: 0x0001B3DA File Offset: 0x000195DA
		[DataSourceProperty]
		public int RatingRatio
		{
			get
			{
				return this._ratingRatio;
			}
			set
			{
				if (value != this._ratingRatio)
				{
					this._ratingRatio = value;
					base.OnPropertyChangedWithValue(value, "RatingRatio");
				}
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x0001B3F8 File Offset: 0x000195F8
		// (set) Token: 0x06000843 RID: 2115 RVA: 0x0001B400 File Offset: 0x00019600
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x0001B423 File Offset: 0x00019623
		// (set) Token: 0x06000845 RID: 2117 RVA: 0x0001B42B File Offset: 0x0001962B
		[DataSourceProperty]
		public string StateText
		{
			get
			{
				return this._stateText;
			}
			set
			{
				if (value != this._stateText)
				{
					this._stateText = value;
					base.OnPropertyChangedWithValue<string>(value, "StateText");
				}
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x0001B44E File Offset: 0x0001964E
		// (set) Token: 0x06000847 RID: 2119 RVA: 0x0001B456 File Offset: 0x00019656
		[DataSourceProperty]
		public string LevelText
		{
			get
			{
				return this._levelText;
			}
			set
			{
				if (value != this._levelText)
				{
					this._levelText = value;
					base.OnPropertyChangedWithValue<string>(value, "LevelText");
				}
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x0001B479 File Offset: 0x00019679
		// (set) Token: 0x06000849 RID: 2121 RVA: 0x0001B481 File Offset: 0x00019681
		[DataSourceProperty]
		public string LevelTitleText
		{
			get
			{
				return this._levelTitleText;
			}
			set
			{
				if (value != this._levelTitleText)
				{
					this._levelTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "LevelTitleText");
				}
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x0001B4A4 File Offset: 0x000196A4
		// (set) Token: 0x0600084B RID: 2123 RVA: 0x0001B4AC File Offset: 0x000196AC
		[DataSourceProperty]
		public string RatingText
		{
			get
			{
				return this._ratingText;
			}
			set
			{
				if (value != this._ratingText)
				{
					this._ratingText = value;
					base.OnPropertyChangedWithValue<string>(value, "RatingText");
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x0001B4CF File Offset: 0x000196CF
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x0001B4D7 File Offset: 0x000196D7
		[DataSourceProperty]
		public string GameTypeText
		{
			get
			{
				return this._gameTypeText;
			}
			set
			{
				if (value != this._gameTypeText)
				{
					this._gameTypeText = value;
					base.OnPropertyChangedWithValue<string>(value, "GameTypeText");
				}
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x0001B4FA File Offset: 0x000196FA
		// (set) Token: 0x0600084F RID: 2127 RVA: 0x0001B502 File Offset: 0x00019702
		[DataSourceProperty]
		public string RatingID
		{
			get
			{
				return this._ratingID;
			}
			set
			{
				if (value != this._ratingID)
				{
					this._ratingID = value;
					base.OnPropertyChangedWithValue<string>(value, "RatingID");
				}
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x0001B525 File Offset: 0x00019725
		// (set) Token: 0x06000851 RID: 2129 RVA: 0x0001B52D File Offset: 0x0001972D
		[DataSourceProperty]
		public string ClanName
		{
			get
			{
				return this._clanName;
			}
			set
			{
				if (value != this._clanName)
				{
					this._clanName = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanName");
				}
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x0001B550 File Offset: 0x00019750
		// (set) Token: 0x06000853 RID: 2131 RVA: 0x0001B558 File Offset: 0x00019758
		[DataSourceProperty]
		public string ClanTag
		{
			get
			{
				return this._clanTag;
			}
			set
			{
				if (value != this._clanTag)
				{
					this._clanTag = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanTag");
				}
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x0001B57B File Offset: 0x0001977B
		// (set) Token: 0x06000855 RID: 2133 RVA: 0x0001B583 File Offset: 0x00019783
		[DataSourceProperty]
		public string ChangeText
		{
			get
			{
				return this._changeText;
			}
			set
			{
				if (value != this._changeText)
				{
					this._changeText = value;
					base.OnPropertyChangedWithValue<string>(value, "ChangeText");
				}
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0001B5A6 File Offset: 0x000197A6
		// (set) Token: 0x06000857 RID: 2135 RVA: 0x0001B5AE File Offset: 0x000197AE
		[DataSourceProperty]
		public string ClanInfoTitleText
		{
			get
			{
				return this._clanInfoTitleText;
			}
			set
			{
				if (value != this._clanInfoTitleText)
				{
					this._clanInfoTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanInfoTitleText");
				}
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x0001B5D1 File Offset: 0x000197D1
		// (set) Token: 0x06000859 RID: 2137 RVA: 0x0001B5D9 File Offset: 0x000197D9
		[DataSourceProperty]
		public string BadgeInfoTitleText
		{
			get
			{
				return this._badgeInfoTitleText;
			}
			set
			{
				if (value != this._badgeInfoTitleText)
				{
					this._badgeInfoTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "BadgeInfoTitleText");
				}
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x0001B5FC File Offset: 0x000197FC
		// (set) Token: 0x0600085B RID: 2139 RVA: 0x0001B604 File Offset: 0x00019804
		[DataSourceProperty]
		public string AvatarInfoTitleText
		{
			get
			{
				return this._avatarInfoTitleText;
			}
			set
			{
				if (value != this._avatarInfoTitleText)
				{
					this._avatarInfoTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "AvatarInfoTitleText");
				}
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x0001B627 File Offset: 0x00019827
		// (set) Token: 0x0600085D RID: 2141 RVA: 0x0001B62F File Offset: 0x0001982F
		[DataSourceProperty]
		public string ExperienceText
		{
			get
			{
				return this._experienceText;
			}
			set
			{
				if (value != this._experienceText)
				{
					this._experienceText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExperienceText");
				}
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x0001B652 File Offset: 0x00019852
		// (set) Token: 0x0600085F RID: 2143 RVA: 0x0001B65A File Offset: 0x0001985A
		[DataSourceProperty]
		public string RankText
		{
			get
			{
				return this._rankText;
			}
			set
			{
				if (value != this._rankText)
				{
					this._rankText = value;
					base.OnPropertyChangedWithValue<string>(value, "RankText");
				}
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x0001B67D File Offset: 0x0001987D
		// (set) Token: 0x06000861 RID: 2145 RVA: 0x0001B685 File Offset: 0x00019885
		[DataSourceProperty]
		public string BannerlordID
		{
			get
			{
				return this._bannerlordID;
			}
			set
			{
				if (value != this._bannerlordID)
				{
					this._bannerlordID = value;
					base.OnPropertyChangedWithValue<string>(value, "BannerlordID");
				}
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x0001B6A8 File Offset: 0x000198A8
		// (set) Token: 0x06000863 RID: 2147 RVA: 0x0001B6B0 File Offset: 0x000198B0
		[DataSourceProperty]
		public string SelectedBadgeID
		{
			get
			{
				return this._selectedBadgeID;
			}
			set
			{
				if (value != this._selectedBadgeID)
				{
					this._selectedBadgeID = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedBadgeID");
				}
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x0001B6D3 File Offset: 0x000198D3
		// (set) Token: 0x06000865 RID: 2149 RVA: 0x0001B6DB File Offset: 0x000198DB
		[DataSourceProperty]
		public HintViewModel NameHint
		{
			get
			{
				return this._nameHint;
			}
			set
			{
				if (value != this._nameHint)
				{
					this._nameHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "NameHint");
				}
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x0001B6F9 File Offset: 0x000198F9
		// (set) Token: 0x06000867 RID: 2151 RVA: 0x0001B701 File Offset: 0x00019901
		[DataSourceProperty]
		public HintViewModel InviteToPartyHint
		{
			get
			{
				return this._inviteToPartyHint;
			}
			set
			{
				if (value != this._inviteToPartyHint)
				{
					this._inviteToPartyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "InviteToPartyHint");
				}
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x0001B71F File Offset: 0x0001991F
		// (set) Token: 0x06000869 RID: 2153 RVA: 0x0001B727 File Offset: 0x00019927
		[DataSourceProperty]
		public HintViewModel RemoveFriendHint
		{
			get
			{
				return this._removeFriendHint;
			}
			set
			{
				if (value != this._removeFriendHint)
				{
					this._removeFriendHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RemoveFriendHint");
				}
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x0001B745 File Offset: 0x00019945
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x0001B74D File Offset: 0x0001994D
		[DataSourceProperty]
		public HintViewModel AcceptFriendRequestHint
		{
			get
			{
				return this._acceptFriendRequestHint;
			}
			set
			{
				if (value != this._acceptFriendRequestHint)
				{
					this._acceptFriendRequestHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AcceptFriendRequestHint");
				}
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x0001B76B File Offset: 0x0001996B
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x0001B773 File Offset: 0x00019973
		[DataSourceProperty]
		public HintViewModel DeclineFriendRequestHint
		{
			get
			{
				return this._declineFriendRequestHint;
			}
			set
			{
				if (value != this._declineFriendRequestHint)
				{
					this._declineFriendRequestHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DeclineFriendRequestHint");
				}
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x0001B791 File Offset: 0x00019991
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x0001B799 File Offset: 0x00019999
		[DataSourceProperty]
		public HintViewModel CancelFriendRequestHint
		{
			get
			{
				return this._cancelFriendRequestHint;
			}
			set
			{
				if (value != this._cancelFriendRequestHint)
				{
					this._cancelFriendRequestHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CancelFriendRequestHint");
				}
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x0001B7B7 File Offset: 0x000199B7
		// (set) Token: 0x06000871 RID: 2161 RVA: 0x0001B7BF File Offset: 0x000199BF
		[DataSourceProperty]
		public HintViewModel InviteToClanHint
		{
			get
			{
				return this._inviteToClanHint;
			}
			set
			{
				if (value != this._inviteToClanHint)
				{
					this._inviteToClanHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "InviteToClanHint");
				}
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0001B7DD File Offset: 0x000199DD
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x0001B7E5 File Offset: 0x000199E5
		[DataSourceProperty]
		public HintViewModel ChangeBannerlordIDHint
		{
			get
			{
				return this._changeBannerlordIDHint;
			}
			set
			{
				if (value != this._changeBannerlordIDHint)
				{
					this._changeBannerlordIDHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ChangeBannerlordIDHint");
				}
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0001B803 File Offset: 0x00019A03
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x0001B80B File Offset: 0x00019A0B
		[DataSourceProperty]
		public HintViewModel CopyBannerlordIDHint
		{
			get
			{
				return this._copyBannerlordIDHint;
			}
			set
			{
				if (value != this._copyBannerlordIDHint)
				{
					this._copyBannerlordIDHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CopyBannerlordIDHint");
				}
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x0001B829 File Offset: 0x00019A29
		// (set) Token: 0x06000877 RID: 2167 RVA: 0x0001B831 File Offset: 0x00019A31
		[DataSourceProperty]
		public HintViewModel AddFriendWithBannerlordIDHint
		{
			get
			{
				return this._addFriendWithBannerlordIDHint;
			}
			set
			{
				if (value != this._addFriendWithBannerlordIDHint)
				{
					this._addFriendWithBannerlordIDHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AddFriendWithBannerlordIDHint");
				}
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x0001B84F File Offset: 0x00019A4F
		// (set) Token: 0x06000879 RID: 2169 RVA: 0x0001B857 File Offset: 0x00019A57
		[DataSourceProperty]
		public HintViewModel ExperienceHint
		{
			get
			{
				return this._experienceHint;
			}
			set
			{
				if (value != this._experienceHint)
				{
					this._experienceHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ExperienceHint");
				}
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x0001B875 File Offset: 0x00019A75
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x0001B87D File Offset: 0x00019A7D
		[DataSourceProperty]
		public HintViewModel RatingHint
		{
			get
			{
				return this._ratingHint;
			}
			set
			{
				if (value != this._ratingHint)
				{
					this._ratingHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RatingHint");
				}
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x0001B89B File Offset: 0x00019A9B
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x0001B8A3 File Offset: 0x00019AA3
		[DataSourceProperty]
		public HintViewModel LootHint
		{
			get
			{
				return this._lootHint;
			}
			set
			{
				if (value != this._lootHint)
				{
					this._lootHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LootHint");
				}
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0001B8C1 File Offset: 0x00019AC1
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x0001B8C9 File Offset: 0x00019AC9
		[DataSourceProperty]
		public HintViewModel SkirmishRatingHint
		{
			get
			{
				return this._skirmishRatingHint;
			}
			set
			{
				if (value != this._skirmishRatingHint)
				{
					this._skirmishRatingHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SkirmishRatingHint");
				}
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x0001B8E7 File Offset: 0x00019AE7
		// (set) Token: 0x06000881 RID: 2177 RVA: 0x0001B8EF File Offset: 0x00019AEF
		[DataSourceProperty]
		public HintViewModel CaptainRatingHint
		{
			get
			{
				return this._captainRatingHint;
			}
			set
			{
				if (value != this._captainRatingHint)
				{
					this._captainRatingHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CaptainRatingHint");
				}
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x0001B90D File Offset: 0x00019B0D
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x0001B915 File Offset: 0x00019B15
		[DataSourceProperty]
		public HintViewModel ClanLeaderboardHint
		{
			get
			{
				return this._clanLeaderboardHint;
			}
			set
			{
				if (value != this._clanLeaderboardHint)
				{
					this._clanLeaderboardHint = value;
					base.OnPropertyChanged("ClanLeaderboardHint");
				}
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x0001B932 File Offset: 0x00019B32
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x0001B93A File Offset: 0x00019B3A
		[DataSourceProperty]
		public PlayerAvatarImageIdentifierVM Avatar
		{
			get
			{
				return this._avatar;
			}
			set
			{
				if (value != this._avatar)
				{
					this._avatar = value;
					base.OnPropertyChangedWithValue<PlayerAvatarImageIdentifierVM>(value, "Avatar");
				}
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0001B958 File Offset: 0x00019B58
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x0001B960 File Offset: 0x00019B60
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanBanner
		{
			get
			{
				return this._clanBanner;
			}
			set
			{
				if (value != this._clanBanner)
				{
					this._clanBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ClanBanner");
				}
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0001B97E File Offset: 0x00019B7E
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x0001B986 File Offset: 0x00019B86
		[DataSourceProperty]
		public MPLobbySigilItemVM Sigil
		{
			get
			{
				return this._sigil;
			}
			set
			{
				if (value != this._sigil)
				{
					this._sigil = value;
					base.OnPropertyChangedWithValue<MPLobbySigilItemVM>(value, "Sigil");
				}
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0001B9A4 File Offset: 0x00019BA4
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x0001B9AC File Offset: 0x00019BAC
		[DataSourceProperty]
		public MPLobbyBadgeItemVM ShownBadge
		{
			get
			{
				return this._shownBadge;
			}
			set
			{
				if (value != this._shownBadge)
				{
					this._shownBadge = value;
					base.OnPropertyChangedWithValue<MPLobbyBadgeItemVM>(value, "ShownBadge");
				}
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0001B9CA File Offset: 0x00019BCA
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x0001B9D2 File Offset: 0x00019BD2
		[DataSourceProperty]
		public CharacterViewModel CharacterVisual
		{
			get
			{
				return this._characterVisual;
			}
			set
			{
				if (value != this._characterVisual)
				{
					this._characterVisual = value;
					base.OnPropertyChangedWithValue<CharacterViewModel>(value, "CharacterVisual");
				}
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0001B9F0 File Offset: 0x00019BF0
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x0001B9F8 File Offset: 0x00019BF8
		[DataSourceProperty]
		public MBBindingList<MPLobbyPlayerStatItemVM> DisplayedStats
		{
			get
			{
				return this._displayedStats;
			}
			set
			{
				if (value != this._displayedStats)
				{
					this._displayedStats = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyPlayerStatItemVM>>(value, "DisplayedStats");
				}
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0001BA16 File Offset: 0x00019C16
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x0001BA1E File Offset: 0x00019C1E
		[DataSourceProperty]
		public MBBindingList<MPLobbyGameTypeVM> GameTypes
		{
			get
			{
				return this._gameTypes;
			}
			set
			{
				if (value != this._gameTypes)
				{
					this._gameTypes = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyGameTypeVM>>(value, "GameTypes");
				}
			}
		}

		// Token: 0x0400039A RID: 922
		public static Action<PlayerId> OnPlayerProfileRequested;

		// Token: 0x0400039B RID: 923
		public static Action<PlayerId> OnBannerlordIDChangeRequested;

		// Token: 0x0400039C RID: 924
		public static Action<PlayerId> OnAddFriendWithBannerlordIDRequested;

		// Token: 0x0400039D RID: 925
		public static Action<PlayerId> OnSigilChangeRequested;

		// Token: 0x0400039E RID: 926
		public static Action<PlayerId> OnBadgeChangeRequested;

		// Token: 0x0400039F RID: 927
		public static Action<MPLobbyPlayerBaseVM> OnRankProgressionRequested;

		// Token: 0x040003A0 RID: 928
		public static Action<string> OnRankLeaderboardRequested;

		// Token: 0x040003A1 RID: 929
		public static Action OnClanPageRequested;

		// Token: 0x040003A2 RID: 930
		public static Action OnClanLeaderboardRequested;

		// Token: 0x040003A3 RID: 931
		private const int DefaultBannerBackgroundColorId = 99;

		// Token: 0x040003A4 RID: 932
		private CustomBattleId? _spectatableCustomBattleId;

		// Token: 0x040003A6 RID: 934
		private PlayerId _providedID;

		// Token: 0x040003A7 RID: 935
		private readonly string _forcedName = string.Empty;

		// Token: 0x040003A8 RID: 936
		private int _forcedAvatarIndex = -1;

		// Token: 0x040003AA RID: 938
		private Action<PlayerId> _onInviteToParty;

		// Token: 0x040003AB RID: 939
		private readonly Action<PlayerId> _onInviteToClan;

		// Token: 0x040003AC RID: 940
		private readonly Action<PlayerId> _onFriendRequestAnswered;

		// Token: 0x040003AF RID: 943
		private bool _isKnownPlayer;

		// Token: 0x040003B0 RID: 944
		private readonly TextObject _genericPlayerName = new TextObject("{=RN6zHak0}Player", null);

		// Token: 0x040003B1 RID: 945
		public Action OnPlayerStatsReceived;

		// Token: 0x040003B2 RID: 946
		protected bool _hasReceivedPlayerStats;

		// Token: 0x040003B3 RID: 947
		protected bool _isReceivingPlayerStats;

		// Token: 0x040003B7 RID: 951
		private const string _skirmishGameTypeID = "Skirmish";

		// Token: 0x040003B8 RID: 952
		private const string _captainGameTypeID = "Captain";

		// Token: 0x040003B9 RID: 953
		private const string _duelGameTypeID = "Duel";

		// Token: 0x040003BA RID: 954
		private const string _teamDeathmatchGameTypeID = "TeamDeathmatch";

		// Token: 0x040003BB RID: 955
		private const string _siegeGameTypeID = "Siege";

		// Token: 0x040003BC RID: 956
		public Action<string> OnRankInfoChanged;

		// Token: 0x040003BD RID: 957
		private bool _canCopyID;

		// Token: 0x040003BE RID: 958
		private bool _showLevel;

		// Token: 0x040003BF RID: 959
		private bool _isSelected;

		// Token: 0x040003C0 RID: 960
		private bool _hasNotification;

		// Token: 0x040003C1 RID: 961
		private bool _isFriendRequest;

		// Token: 0x040003C2 RID: 962
		private bool _isPendingRequest;

		// Token: 0x040003C3 RID: 963
		private bool _canRemove;

		// Token: 0x040003C4 RID: 964
		private bool _canBeInvited;

		// Token: 0x040003C5 RID: 965
		private bool _canInviteToParty;

		// Token: 0x040003C6 RID: 966
		private bool _canInviteToClan;

		// Token: 0x040003C7 RID: 967
		private bool _isSigilChangeInformationEnabled;

		// Token: 0x040003C8 RID: 968
		private bool _isRankInfoLoading;

		// Token: 0x040003C9 RID: 969
		private bool _isRankInfoCasual;

		// Token: 0x040003CA RID: 970
		private bool _isClanInfoSupported;

		// Token: 0x040003CB RID: 971
		private bool _isBannerlordIDSupported;

		// Token: 0x040003CC RID: 972
		private int _level;

		// Token: 0x040003CD RID: 973
		private int _rating;

		// Token: 0x040003CE RID: 974
		private int _loot;

		// Token: 0x040003CF RID: 975
		private int _experienceRatio;

		// Token: 0x040003D0 RID: 976
		private int _ratingRatio;

		// Token: 0x040003D1 RID: 977
		private string _name = "";

		// Token: 0x040003D2 RID: 978
		private string _stateText;

		// Token: 0x040003D3 RID: 979
		private string _levelText;

		// Token: 0x040003D4 RID: 980
		private string _levelTitleText;

		// Token: 0x040003D5 RID: 981
		private string _ratingText;

		// Token: 0x040003D6 RID: 982
		private string _gameTypeText;

		// Token: 0x040003D7 RID: 983
		private string _ratingID;

		// Token: 0x040003D8 RID: 984
		private string _clanName;

		// Token: 0x040003D9 RID: 985
		private string _clanTag;

		// Token: 0x040003DA RID: 986
		private string _changeText;

		// Token: 0x040003DB RID: 987
		private string _clanInfoTitleText;

		// Token: 0x040003DC RID: 988
		private string _badgeInfoTitleText;

		// Token: 0x040003DD RID: 989
		private string _avatarInfoTitleText;

		// Token: 0x040003DE RID: 990
		private string _experienceText;

		// Token: 0x040003DF RID: 991
		private string _rankText;

		// Token: 0x040003E0 RID: 992
		private string _bannerlordID;

		// Token: 0x040003E1 RID: 993
		private string _selectedBadgeID;

		// Token: 0x040003E2 RID: 994
		private HintViewModel _nameHint;

		// Token: 0x040003E3 RID: 995
		private HintViewModel _inviteToPartyHint;

		// Token: 0x040003E4 RID: 996
		private HintViewModel _removeFriendHint;

		// Token: 0x040003E5 RID: 997
		private HintViewModel _acceptFriendRequestHint;

		// Token: 0x040003E6 RID: 998
		private HintViewModel _declineFriendRequestHint;

		// Token: 0x040003E7 RID: 999
		private HintViewModel _cancelFriendRequestHint;

		// Token: 0x040003E8 RID: 1000
		private HintViewModel _inviteToClanHint;

		// Token: 0x040003E9 RID: 1001
		private HintViewModel _changeBannerlordIDHint;

		// Token: 0x040003EA RID: 1002
		private HintViewModel _copyBannerlordIDHint;

		// Token: 0x040003EB RID: 1003
		private HintViewModel _addFriendWithBannerlordIDHint;

		// Token: 0x040003EC RID: 1004
		private HintViewModel _experienceHint;

		// Token: 0x040003ED RID: 1005
		private HintViewModel _ratingHint;

		// Token: 0x040003EE RID: 1006
		private HintViewModel _lootHint;

		// Token: 0x040003EF RID: 1007
		private HintViewModel _skirmishRatingHint;

		// Token: 0x040003F0 RID: 1008
		private HintViewModel _captainRatingHint;

		// Token: 0x040003F1 RID: 1009
		private HintViewModel _clanLeaderboardHint;

		// Token: 0x040003F2 RID: 1010
		private PlayerAvatarImageIdentifierVM _avatar;

		// Token: 0x040003F3 RID: 1011
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x040003F4 RID: 1012
		private MPLobbySigilItemVM _sigil;

		// Token: 0x040003F5 RID: 1013
		private MPLobbyBadgeItemVM _shownBadge;

		// Token: 0x040003F6 RID: 1014
		private CharacterViewModel _characterVisual;

		// Token: 0x040003F7 RID: 1015
		private MBBindingList<MPLobbyPlayerStatItemVM> _displayedStats;

		// Token: 0x040003F8 RID: 1016
		private MBBindingList<MPLobbyGameTypeVM> _gameTypes;

		// Token: 0x040003F9 RID: 1017
		private bool _canSpectate;

		// Token: 0x02000124 RID: 292
		public enum OnlineStatus
		{
			// Token: 0x0400097C RID: 2428
			None,
			// Token: 0x0400097D RID: 2429
			InGame,
			// Token: 0x0400097E RID: 2430
			Online,
			// Token: 0x0400097F RID: 2431
			Offline
		}
	}
}
