using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamworks;
using TaleWorlds.AchievementSystem;
using TaleWorlds.ActivitySystem;
using TaleWorlds.Avatar.PlayerServices;
using TaleWorlds.Diamond;
using TaleWorlds.Diamond.AccessProvider.Steam;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.PlayerServices;
using TaleWorlds.PlayerServices.Avatar;

namespace TaleWorlds.PlatformService.Steam
{
	// Token: 0x02000006 RID: 6
	public class SteamPlatformServices : IPlatformServices
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000025B1 File Offset: 0x000007B1
		private static SteamPlatformServices Instance
		{
			get
			{
				return PlatformServices.Instance as SteamPlatformServices;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000025BD File Offset: 0x000007BD
		// (set) Token: 0x06000037 RID: 55 RVA: 0x000025C5 File Offset: 0x000007C5
		internal bool Initialized { get; private set; }

		// Token: 0x06000038 RID: 56 RVA: 0x000025CE File Offset: 0x000007CE
		public SteamPlatformServices(PlatformInitParams initParams)
		{
			this._initParams = initParams;
			AvatarServices.AddAvatarService(PlayerIdProvidedTypes.Steam, new SteamPlatformAvatarService(this));
			this._achievementService = new SteamAchievementService(this);
			this._steamFriendListService = new SteamFriendListService(this);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000039 RID: 57 RVA: 0x0000260C File Offset: 0x0000080C
		string IPlatformServices.ProviderName
		{
			get
			{
				return "Steam";
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002614 File Offset: 0x00000814
		string IPlatformServices.UserId
		{
			get
			{
				return ((ulong)SteamUser.GetSteamID()).ToString();
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002633 File Offset: 0x00000833
		PlayerId IPlatformServices.PlayerId
		{
			get
			{
				return SteamUser.GetSteamID().ToPlayerId();
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000263F File Offset: 0x0000083F
		bool IPlatformServices.UserLoggedIn
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002646 File Offset: 0x00000846
		void IPlatformServices.LoginUser()
		{
			throw new NotImplementedException();
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003E RID: 62 RVA: 0x0000264D File Offset: 0x0000084D
		string IPlatformServices.UserDisplayName
		{
			get
			{
				if (!this.Initialized)
				{
					return string.Empty;
				}
				return SteamFriends.GetPersonaName();
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002662 File Offset: 0x00000862
		IReadOnlyCollection<PlayerId> IPlatformServices.BlockedUsers
		{
			get
			{
				return new List<PlayerId>();
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002669 File Offset: 0x00000869
		bool IPlatformServices.IsPermanentMuteAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000266C File Offset: 0x0000086C
		bool IPlatformServices.Initialize(IFriendListService[] additionalFriendListServices)
		{
			this._friendListServices = new IFriendListService[additionalFriendListServices.Length + 1];
			this._friendListServices[0] = this._steamFriendListService;
			for (int i = 0; i < additionalFriendListServices.Length; i++)
			{
				this._friendListServices[i + 1] = additionalFriendListServices[i];
			}
			if (!SteamAPI.Init())
			{
				return false;
			}
			ModuleHelper.InitializePlatformModuleExtension(new SteamModuleExtension(), null);
			this.InitCallbacks();
			this._achievementService.Initialize();
			SteamUserStats.RequestCurrentStats();
			this.Initialized = true;
			return true;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000026E5 File Offset: 0x000008E5
		void IPlatformServices.Tick(float dt)
		{
			if (this.Initialized)
			{
				SteamAPI.RunCallbacks();
				this._achievementService.Tick(dt);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002700 File Offset: 0x00000900
		void IPlatformServices.Terminate()
		{
			SteamAPI.Shutdown();
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000044 RID: 68 RVA: 0x00002708 File Offset: 0x00000908
		// (remove) Token: 0x06000045 RID: 69 RVA: 0x00002740 File Offset: 0x00000940
		public event Action<AvatarData> OnAvatarUpdated;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000046 RID: 70 RVA: 0x00002778 File Offset: 0x00000978
		// (remove) Token: 0x06000047 RID: 71 RVA: 0x000027B0 File Offset: 0x000009B0
		public event Action<string> OnNameUpdated;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000048 RID: 72 RVA: 0x000027E8 File Offset: 0x000009E8
		// (remove) Token: 0x06000049 RID: 73 RVA: 0x00002820 File Offset: 0x00000A20
		public event Action<bool, TextObject> OnSignInStateUpdated;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600004A RID: 74 RVA: 0x00002858 File Offset: 0x00000A58
		// (remove) Token: 0x0600004B RID: 75 RVA: 0x00002890 File Offset: 0x00000A90
		public event Action OnBlockedUserListUpdated;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600004C RID: 76 RVA: 0x000028C8 File Offset: 0x00000AC8
		// (remove) Token: 0x0600004D RID: 77 RVA: 0x00002900 File Offset: 0x00000B00
		public event Action<string> OnTextEnteredFromPlatform;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600004E RID: 78 RVA: 0x00002938 File Offset: 0x00000B38
		// (remove) Token: 0x0600004F RID: 79 RVA: 0x00002970 File Offset: 0x00000B70
		public event Action OnTextCanceledFromPlatform;

		// Token: 0x06000050 RID: 80 RVA: 0x000029A5 File Offset: 0x00000BA5
		bool IPlatformServices.ShowGamepadTextInput(string descriptionText, string existingText, uint maxChars, bool isObfuscated)
		{
			return this.Initialized && SteamUtils.ShowGamepadTextInput(isObfuscated ? EGamepadTextInputMode.k_EGamepadTextInputModePassword : EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, descriptionText, maxChars, existingText);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000029C2 File Offset: 0x00000BC2
		bool IPlatformServices.IsPlayerProfileCardAvailable(PlayerId providedId)
		{
			return false;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000029C5 File Offset: 0x00000BC5
		void IPlatformServices.ShowPlayerProfileCard(PlayerId providedId)
		{
			SteamFriends.ActivateGameOverlayToUser("steamid", providedId.ToSteamId());
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000029D8 File Offset: 0x00000BD8
		async Task<AvatarData> IPlatformServices.GetUserAvatar(PlayerId providedId)
		{
			AvatarData avatarData;
			if (!providedId.IsValid)
			{
				avatarData = null;
			}
			else if (this._avatarCache.ContainsKey(providedId))
			{
				avatarData = this._avatarCache[providedId];
			}
			else
			{
				if (this._avatarCache.Count > 300)
				{
					this._avatarCache.Clear();
				}
				long startTime = DateTime.UtcNow.Ticks;
				CSteamID steamId = providedId.ToSteamId();
				if (SteamFriends.RequestUserInformation(steamId, false))
				{
					while (!SteamPlatformServices._avatarUpdates.Contains(steamId) && !this.TimedOut(startTime, 5000L))
					{
						await Task.Delay(5);
					}
					SteamPlatformServices._avatarUpdates.Remove(steamId);
				}
				int userAvatar = SteamFriends.GetLargeFriendAvatar(steamId);
				if (userAvatar == -1)
				{
					while (!SteamPlatformServices._avatarLoadedUpdates.Contains(steamId) && !this.TimedOut(startTime, 5000L))
					{
						await Task.Delay(5);
					}
					SteamPlatformServices._avatarLoadedUpdates.Remove(steamId);
					while (userAvatar == -1 && !this.TimedOut(startTime, 5000L))
					{
						userAvatar = SteamFriends.GetLargeFriendAvatar(steamId);
					}
				}
				if (userAvatar != -1)
				{
					uint num;
					uint num2;
					SteamUtils.GetImageSize(userAvatar, out num, out num2);
					if (num != 0U)
					{
						uint num3 = num * num2 * 4U;
						byte[] array = new byte[num3];
						if (SteamUtils.GetImageRGBA(userAvatar, array, (int)num3))
						{
							AvatarData avatarData2 = new AvatarData(array, num, num2);
							Dictionary<PlayerId, AvatarData> avatarCache = this._avatarCache;
							lock (avatarCache)
							{
								if (!this._avatarCache.ContainsKey(providedId))
								{
									this._avatarCache.Add(providedId, avatarData2);
								}
							}
							return avatarData2;
						}
					}
				}
				avatarData = null;
			}
			return avatarData;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002A25 File Offset: 0x00000C25
		public void ClearAvatarCache()
		{
			this._avatarCache.Clear();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002A34 File Offset: 0x00000C34
		private bool TimedOut(long startUTCTicks, long timeOut)
		{
			return (long)(DateTime.Now - new DateTime(startUTCTicks)).Milliseconds > timeOut;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002A60 File Offset: 0x00000C60
		internal async Task<string> GetUserName(PlayerId providedId)
		{
			string text;
			if (!providedId.IsValid || providedId.ProvidedType != PlayerIdProvidedTypes.Steam)
			{
				text = null;
			}
			else
			{
				long startTime = DateTime.UtcNow.Ticks;
				CSteamID steamId = providedId.ToSteamId();
				if (SteamFriends.RequestUserInformation(steamId, false))
				{
					while (!SteamPlatformServices._nameUpdates.Contains(steamId) && !this.TimedOut(startTime, 5000L))
					{
						await Task.Delay(5);
					}
					SteamPlatformServices._nameUpdates.Remove(steamId);
				}
				string friendPersonaName = SteamFriends.GetFriendPersonaName(steamId);
				if (!string.IsNullOrEmpty(friendPersonaName))
				{
					text = friendPersonaName;
				}
				else
				{
					text = null;
				}
			}
			return text;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002AAD File Offset: 0x00000CAD
		PlatformInitParams IPlatformServices.GetInitParams()
		{
			return this._initParams;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002AB5 File Offset: 0x00000CB5
		IAchievementService IPlatformServices.GetAchievementService()
		{
			return this._achievementService;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002ABD File Offset: 0x00000CBD
		IActivityService IPlatformServices.GetActivityService()
		{
			return new TestActivityService();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002AC4 File Offset: 0x00000CC4
		async Task<bool> IPlatformServices.ShowOverlayForWebPage(string url)
		{
			await Task.Delay(0);
			SteamFriends.ActivateGameOverlayToWebPage(url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
			return true;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002B09 File Offset: 0x00000D09
		void IPlatformServices.CheckPrivilege(Privilege privilege, bool displayResolveUI, PrivilegeResult callback)
		{
			callback(true);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002B12 File Offset: 0x00000D12
		void IPlatformServices.CheckPermissionWithUser(Permission privilege, PlayerId targetPlayerId, PermissionResult callback)
		{
			callback(true);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002B1B File Offset: 0x00000D1B
		bool IPlatformServices.RegisterPermissionChangeEvent(PlayerId targetPlayerId, Permission permission, PermissionChanged callback)
		{
			return false;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002B1E File Offset: 0x00000D1E
		bool IPlatformServices.UnregisterPermissionChangeEvent(PlayerId targetPlayerId, Permission permission, PermissionChanged callback)
		{
			return false;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002B21 File Offset: 0x00000D21
		void IPlatformServices.ShowRestrictedInformation()
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002B23 File Offset: 0x00000D23
		Task<bool> IPlatformServices.VerifyString(string content)
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002B2B File Offset: 0x00000D2B
		void IPlatformServices.GetPlatformId(PlayerId playerId, Action<object> callback)
		{
			callback(playerId.ToSteamId());
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002B3E File Offset: 0x00000D3E
		void IPlatformServices.OnFocusGained()
		{
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002B40 File Offset: 0x00000D40
		internal Task<bool> GetUserOnlineStatus(PlayerId providedId)
		{
			SteamUtils.GetAppID();
			if (SteamFriends.GetFriendPersonaState(new CSteamID(providedId.Part4)) != EPersonaState.k_EPersonaStateOffline)
			{
				return Task.FromResult<bool>(true);
			}
			return Task.FromResult<bool>(false);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002B68 File Offset: 0x00000D68
		internal Task<bool> IsPlayingThisGame(PlayerId providedId)
		{
			AppId_t appID = SteamUtils.GetAppID();
			FriendGameInfo_t friendGameInfo_t;
			if (SteamFriends.GetFriendGamePlayed(new CSteamID(providedId.Part4), out friendGameInfo_t) && friendGameInfo_t.m_gameID.AppID() == appID)
			{
				return Task.FromResult<bool>(true);
			}
			return Task.FromResult<bool>(false);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002BB4 File Offset: 0x00000DB4
		internal async Task<PlayerId> GetUserWithName(string name)
		{
			await Task.Delay(0);
			int num = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
			CSteamID csteamID = default(CSteamID);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				CSteamID friendByIndex = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
				if (SteamFriends.GetFriendPersonaName(friendByIndex).Equals(name))
				{
					csteamID = friendByIndex;
					num2++;
				}
			}
			num = SteamFriends.GetCoplayFriendCount();
			for (int j = 0; j < num; j++)
			{
				CSteamID coplayFriend = SteamFriends.GetCoplayFriend(j);
				if (SteamFriends.GetFriendPersonaName(coplayFriend).Equals(name))
				{
					csteamID = coplayFriend;
					num2++;
				}
			}
			PlayerId playerId;
			if (num2 != 1)
			{
				playerId = default(PlayerId);
			}
			else
			{
				playerId = csteamID.ToPlayerId();
			}
			return playerId;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002BFC File Offset: 0x00000DFC
		private async void OnAvatarUpdateReceived(ulong userId)
		{
			int userAvatar = -1;
			while (userAvatar == -1)
			{
				userAvatar = SteamFriends.GetLargeFriendAvatar(new CSteamID(userId));
				await Task.Delay(5);
			}
			if (userAvatar != -1)
			{
				uint num;
				uint num2;
				SteamUtils.GetImageSize(userAvatar, out num, out num2);
				if (num != 0U)
				{
					uint num3 = num * num2 * 4U;
					byte[] array = new byte[num3];
					if (SteamUtils.GetImageRGBA(userAvatar, array, (int)num3))
					{
						Action<AvatarData> onAvatarUpdated = this.OnAvatarUpdated;
						if (onAvatarUpdated != null)
						{
							onAvatarUpdated(new AvatarData(array, num, num2));
						}
					}
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002C40 File Offset: 0x00000E40
		private void OnNameUpdateReceived(PlayerId userId)
		{
			string friendPersonaName = SteamFriends.GetFriendPersonaName(userId.ToSteamId());
			if (!string.IsNullOrEmpty(friendPersonaName))
			{
				Action<string> onNameUpdated = this.OnNameUpdated;
				if (onNameUpdated == null)
				{
					return;
				}
				onNameUpdated(friendPersonaName);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002C72 File Offset: 0x00000E72
		private void Dummy()
		{
			if (this.OnSignInStateUpdated != null)
			{
				this.OnSignInStateUpdated(false, null);
			}
			if (this.OnBlockedUserListUpdated != null)
			{
				this.OnBlockedUserListUpdated();
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002C9C File Offset: 0x00000E9C
		private void InitCallbacks()
		{
			this._personaStateChangeT = Callback<PersonaStateChange_t>.Create(new Callback<PersonaStateChange_t>.DispatchDelegate(SteamPlatformServices.UserInformationUpdated));
			this._avatarImageLoadedT = Callback<AvatarImageLoaded_t>.Create(new Callback<AvatarImageLoaded_t>.DispatchDelegate(SteamPlatformServices.AvatarLoaded));
			this._gamepadTextInputDismissedT = Callback<GamepadTextInputDismissed_t>.Create(new Callback<GamepadTextInputDismissed_t>.DispatchDelegate(this.GamepadTextInputDismissed));
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002CEE File Offset: 0x00000EEE
		private static void AvatarLoaded(AvatarImageLoaded_t avatarImageLoadedT)
		{
			SteamPlatformServices._avatarLoadedUpdates.Add(avatarImageLoadedT.m_steamID);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002D00 File Offset: 0x00000F00
		private static void UserInformationUpdated(PersonaStateChange_t pCallback)
		{
			if ((pCallback.m_nChangeFlags & EPersonaChange.k_EPersonaChangeAvatar) != (EPersonaChange)0)
			{
				SteamPlatformServices._avatarUpdates.Add(new CSteamID(pCallback.m_ulSteamID));
				SteamPlatformServices.Instance.OnAvatarUpdateReceived(pCallback.m_ulSteamID);
				return;
			}
			if ((pCallback.m_nChangeFlags & EPersonaChange.k_EPersonaChangeName) != (EPersonaChange)0)
			{
				SteamPlatformServices._nameUpdates.Add(new CSteamID(pCallback.m_ulSteamID));
				SteamPlatformServices.Instance.OnNameUpdateReceived(new CSteamID(pCallback.m_ulSteamID).ToPlayerId());
				return;
			}
			if ((pCallback.m_nChangeFlags & EPersonaChange.k_EPersonaChangeGamePlayed) != (EPersonaChange)0)
			{
				SteamPlatformServices.HandleOnUserStatusChanged(new CSteamID(pCallback.m_ulSteamID).ToPlayerId());
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002D98 File Offset: 0x00000F98
		private void GamepadTextInputDismissed(GamepadTextInputDismissed_t gamepadTextInputDismissedT)
		{
			if (gamepadTextInputDismissedT.m_bSubmitted)
			{
				string text;
				SteamUtils.GetEnteredGamepadTextInput(out text, SteamUtils.GetEnteredGamepadTextLength());
				Action<string> onTextEnteredFromPlatform = this.OnTextEnteredFromPlatform;
				if (onTextEnteredFromPlatform == null)
				{
					return;
				}
				onTextEnteredFromPlatform(text);
				return;
			}
			else
			{
				Action onTextCanceledFromPlatform = this.OnTextCanceledFromPlatform;
				if (onTextCanceledFromPlatform == null)
				{
					return;
				}
				onTextCanceledFromPlatform();
				return;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002DDC File Offset: 0x00000FDC
		private static void HandleOnUserStatusChanged(PlayerId playerId)
		{
			SteamPlatformServices.Instance._steamFriendListService.HandleOnUserStatusChanged(playerId);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002DEE File Offset: 0x00000FEE
		Task<ILoginAccessProvider> IPlatformServices.CreateLobbyClientLoginProvider()
		{
			return Task.FromResult<ILoginAccessProvider>(new SteamLoginAccessProvider());
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002DFA File Offset: 0x00000FFA
		IFriendListService[] IPlatformServices.GetFriendListServices()
		{
			return this._friendListServices;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002E02 File Offset: 0x00001002
		bool IPlatformServices.UsePlatformInvitationService(PlayerId targetPlayerId)
		{
			return false;
		}

		// Token: 0x04000010 RID: 16
		private PlatformInitParams _initParams;

		// Token: 0x04000011 RID: 17
		private SteamFriendListService _steamFriendListService;

		// Token: 0x04000012 RID: 18
		private IFriendListService[] _friendListServices;

		// Token: 0x04000013 RID: 19
		public SteamAchievementService _achievementService;

		// Token: 0x0400001A RID: 26
		private Dictionary<PlayerId, AvatarData> _avatarCache = new Dictionary<PlayerId, AvatarData>();

		// Token: 0x0400001B RID: 27
		private const int CommandRequestTimeOut = 5000;

		// Token: 0x0400001C RID: 28
		private Callback<PersonaStateChange_t> _personaStateChangeT;

		// Token: 0x0400001D RID: 29
		private Callback<AvatarImageLoaded_t> _avatarImageLoadedT;

		// Token: 0x0400001E RID: 30
		private Callback<GamepadTextInputDismissed_t> _gamepadTextInputDismissedT;

		// Token: 0x0400001F RID: 31
		private static List<CSteamID> _avatarUpdates = new List<CSteamID>();

		// Token: 0x04000020 RID: 32
		private static List<CSteamID> _avatarLoadedUpdates = new List<CSteamID>();

		// Token: 0x04000021 RID: 33
		private static List<CSteamID> _nameUpdates = new List<CSteamID>();
	}
}
