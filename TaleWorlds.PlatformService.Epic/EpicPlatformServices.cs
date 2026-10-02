using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epic.OnlineServices;
using Epic.OnlineServices.Achievements;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.Friends;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.Presence;
using Epic.OnlineServices.Stats;
using Epic.OnlineServices.UserInfo;
using Newtonsoft.Json;
using TaleWorlds.AchievementSystem;
using TaleWorlds.ActivitySystem;
using TaleWorlds.Avatar.PlayerServices;
using TaleWorlds.Diamond;
using TaleWorlds.Diamond.AccessProvider.Epic;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.PlayerServices;
using TaleWorlds.PlayerServices.Avatar;

namespace TaleWorlds.PlatformService.Epic
{
	// Token: 0x02000005 RID: 5
	public class EpicPlatformServices : IPlatformServices
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002397 File Offset: 0x00000597
		public string UserId
		{
			get
			{
				if (this._epicAccountId == null)
				{
					return "";
				}
				return this._epicAccountId.ToString();
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000023B8 File Offset: 0x000005B8
		string IPlatformServices.UserDisplayName
		{
			get
			{
				return this._epicUserName;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000023C0 File Offset: 0x000005C0
		IReadOnlyCollection<PlayerId> IPlatformServices.BlockedUsers
		{
			get
			{
				return new List<PlayerId>();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000023C7 File Offset: 0x000005C7
		bool IPlatformServices.IsPermanentMuteAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000023CA File Offset: 0x000005CA
		public EpicPlatformServices(PlatformInitParams initParams)
		{
			this._initParams = initParams;
			AvatarServices.AddAvatarService(PlayerIdProvidedTypes.Epic, new EpicPlatformAvatarService());
			this._epicFriendListService = new EpicFriendListService(this);
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000028 RID: 40 RVA: 0x00002408 File Offset: 0x00000608
		// (remove) Token: 0x06000029 RID: 41 RVA: 0x00002440 File Offset: 0x00000640
		public event Action<AvatarData> OnAvatarUpdated;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600002A RID: 42 RVA: 0x00002478 File Offset: 0x00000678
		// (remove) Token: 0x0600002B RID: 43 RVA: 0x000024B0 File Offset: 0x000006B0
		public event Action<string> OnNameUpdated;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600002C RID: 44 RVA: 0x000024E8 File Offset: 0x000006E8
		// (remove) Token: 0x0600002D RID: 45 RVA: 0x00002520 File Offset: 0x00000720
		public event Action<bool, TextObject> OnSignInStateUpdated;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600002E RID: 46 RVA: 0x00002558 File Offset: 0x00000758
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x00002590 File Offset: 0x00000790
		public event Action OnBlockedUserListUpdated;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000030 RID: 48 RVA: 0x000025C8 File Offset: 0x000007C8
		// (remove) Token: 0x06000031 RID: 49 RVA: 0x00002600 File Offset: 0x00000800
		public event Action<string> OnTextEnteredFromPlatform;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000032 RID: 50 RVA: 0x00002638 File Offset: 0x00000838
		// (remove) Token: 0x06000033 RID: 51 RVA: 0x00002670 File Offset: 0x00000870
		public event Action OnTextCanceledFromPlatform;

		// Token: 0x06000034 RID: 52 RVA: 0x000026A8 File Offset: 0x000008A8
		public bool Initialize(IFriendListService[] additionalFriendListServices)
		{
			this._friendListServices = new IFriendListService[additionalFriendListServices.Length + 1];
			this._friendListServices[0] = this._epicFriendListService;
			for (int i = 0; i < additionalFriendListServices.Length; i++)
			{
				this._friendListServices[i + 1] = additionalFriendListServices[i];
			}
			string text = (string)this._initParams["PlatformInterface"];
			long num;
			if (!long.TryParse(text, out num))
			{
				this._initFailReason = new TextObject("{=BJ1626h7}Epic platform initialization failed: {FAILREASON}.", null);
				this._initFailReason.SetTextVariable("FAILREASON (Platform Interface Handle)", text);
				Debug.Print("Epic PlatformInterface.Initialize Failed (Platform Interface Handle):" + text, 0, Debug.DebugColor.White, 17592186044416UL);
				return false;
			}
			IntPtr intPtr = new IntPtr(num);
			this._platform = new PlatformInterface(intPtr);
			AddNotifyFriendsUpdateOptions addNotifyFriendsUpdateOptions = default(AddNotifyFriendsUpdateOptions);
			this._platform.GetFriendsInterface().AddNotifyFriendsUpdate(ref addNotifyFriendsUpdateOptions, null, delegate(ref OnFriendsUpdateInfo callbackInfo)
			{
				this._epicFriendListService.UserStatusChanged(EpicPlatformServices.EpicAccountIdToPlayerId(callbackInfo.TargetUserId));
			});
			this._epicAccountId = EpicAccountId.FromString((string)this._initParams["EpicUserId"]);
			this._epicUserName = (string)this._initParams["EpicUserName"];
			if (this._platform.GetAuthInterface() == null)
			{
				Console.WriteLine("ERROR: Failed to get Auth interface!");
				this._initFailReason = new TextObject("{=BJ1626h7}Failed to get Auth interface!.", null);
				Debug.Print("Failed to get Auth interface!", 0, Debug.DebugColor.White, 17592186044416UL);
				return false;
			}
			return this.Connect();
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000035 RID: 53 RVA: 0x0000281D File Offset: 0x00000A1D
		private string ExchangeCode
		{
			get
			{
				return (string)this._initParams["ExchangeCode"];
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002834 File Offset: 0x00000A34
		private void Dummy()
		{
			if (this.OnAvatarUpdated != null)
			{
				this.OnAvatarUpdated(null);
			}
			if (this.OnNameUpdated != null)
			{
				this.OnNameUpdated(null);
			}
			if (this.OnSignInStateUpdated != null)
			{
				this.OnSignInStateUpdated(false, null);
			}
			if (this.OnBlockedUserListUpdated != null)
			{
				this.OnBlockedUserListUpdated();
			}
			if (this.OnTextEnteredFromPlatform != null)
			{
				this.OnTextEnteredFromPlatform(null);
			}
			if (this.OnTextCanceledFromPlatform != null)
			{
				this.OnTextCanceledFromPlatform();
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000028B8 File Offset: 0x00000AB8
		private void RefreshConnection(ref AuthExpirationCallbackInfo clientData)
		{
			try
			{
				this.Connect();
			}
			catch (Exception ex)
			{
				Debug.Print("RefreshConnection:" + ex.Message + " " + Environment.StackTrace, 5, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000290C File Offset: 0x00000B0C
		private bool Connect()
		{
			bool failed = false;
			CopyUserAuthTokenOptions copyUserAuthTokenOptions = default(CopyUserAuthTokenOptions);
			Token? token;
			this._platform.GetAuthInterface().CopyUserAuthToken(ref copyUserAuthTokenOptions, this._epicAccountId, ref token);
			if (token == null)
			{
				this._initFailReason = new TextObject("{=oGIdsL8h}Could not retrieve token", null);
				return false;
			}
			this._accessToken = token.Value.AccessToken;
			this._platform.GetConnectInterface().RemoveNotifyAuthExpiration(this._refreshConnectionCallbackId);
			LoginOptions loginOptions = default(LoginOptions);
			Credentials credentials = default(Credentials);
			credentials.Token = this._accessToken;
			credentials.Type = 0;
			loginOptions.Credentials = new Credentials?(credentials);
			LoginOptions loginOptions2 = loginOptions;
			OnCreateUserCallback <>9__1;
			this._platform.GetConnectInterface().Login(ref loginOptions2, null, delegate(ref LoginCallbackInfo data)
			{
				if (data.ResultCode == 3)
				{
					CreateUserOptions createUserOptions = default(CreateUserOptions);
					createUserOptions.ContinuanceToken = data.ContinuanceToken;
					CreateUserOptions createUserOptions2 = createUserOptions;
					ConnectInterface connectInterface = this._platform.GetConnectInterface();
					object obj = null;
					OnCreateUserCallback onCreateUserCallback;
					if ((onCreateUserCallback = <>9__1) == null)
					{
						onCreateUserCallback = (<>9__1 = delegate(ref CreateUserCallbackInfo res)
						{
							if (res.ResultCode != null)
							{
								failed = true;
								return;
							}
							this._localUserId = res.LocalUserId;
						});
					}
					connectInterface.CreateUser(ref createUserOptions2, obj, onCreateUserCallback);
					return;
				}
				if (data.ResultCode != null)
				{
					failed = true;
					return;
				}
				this._localUserId = data.LocalUserId;
			});
			while (this._localUserId == null && !failed)
			{
				this._platform.Tick();
			}
			if (failed)
			{
				this._initFailReason = new TextObject("{=KoKdRd1u}Could not login to Epic", null);
				return false;
			}
			AddNotifyAuthExpirationOptions addNotifyAuthExpirationOptions = default(AddNotifyAuthExpirationOptions);
			this._refreshConnectionCallbackId = this._platform.GetConnectInterface().AddNotifyAuthExpiration(ref addNotifyAuthExpirationOptions, token, new OnAuthExpirationCallback(this.RefreshConnection));
			this.QueryStats();
			this.QueryDefinitions();
			return true;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002A74 File Offset: 0x00000C74
		public void Terminate()
		{
			if (this._platform != null)
			{
				this._platform.Release();
				this._platform = null;
				PlatformInterface.Shutdown();
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002A9C File Offset: 0x00000C9C
		public void Tick(float dt)
		{
			if (this._platform != null)
			{
				this._platform.Tick();
				this.ProcessIngestStatsQueue();
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002ABD File Offset: 0x00000CBD
		string IPlatformServices.ProviderName
		{
			get
			{
				return "Epic";
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002AC4 File Offset: 0x00000CC4
		PlayerId IPlatformServices.PlayerId
		{
			get
			{
				return EpicPlatformServices.EpicAccountIdToPlayerId(this._epicAccountId);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002AD1 File Offset: 0x00000CD1
		bool IPlatformServices.IsPlayerProfileCardAvailable(PlayerId providedId)
		{
			return false;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002AD4 File Offset: 0x00000CD4
		void IPlatformServices.ShowPlayerProfileCard(PlayerId providedId)
		{
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002AD6 File Offset: 0x00000CD6
		bool IPlatformServices.UserLoggedIn
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002ADD File Offset: 0x00000CDD
		void IPlatformServices.LoginUser()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002AE4 File Offset: 0x00000CE4
		Task<AvatarData> IPlatformServices.GetUserAvatar(PlayerId providedId)
		{
			return Task.FromResult<AvatarData>(null);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002AEC File Offset: 0x00000CEC
		Task<bool> IPlatformServices.ShowOverlayForWebPage(string url)
		{
			return Task.FromResult<bool>(false);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002AF4 File Offset: 0x00000CF4
		Task<ILoginAccessProvider> IPlatformServices.CreateLobbyClientLoginProvider()
		{
			return Task.FromResult<ILoginAccessProvider>(new EpicLoginAccessProvider(this._platform, this._epicAccountId, this._epicUserName, this._accessToken, this._initFailReason));
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002B1E File Offset: 0x00000D1E
		PlatformInitParams IPlatformServices.GetInitParams()
		{
			return this._initParams;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002B26 File Offset: 0x00000D26
		IAchievementService IPlatformServices.GetAchievementService()
		{
			return new EpicAchievementService(this);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002B2E File Offset: 0x00000D2E
		IActivityService IPlatformServices.GetActivityService()
		{
			return new TestActivityService();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002B35 File Offset: 0x00000D35
		void IPlatformServices.CheckPrivilege(Privilege privilege, bool displayResolveUI, PrivilegeResult callback)
		{
			callback(true);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002B3E File Offset: 0x00000D3E
		void IPlatformServices.CheckPermissionWithUser(Permission privilege, PlayerId targetPlayerId, PermissionResult callback)
		{
			callback(true);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002B47 File Offset: 0x00000D47
		bool IPlatformServices.RegisterPermissionChangeEvent(PlayerId targetPlayerId, Permission permission, PermissionChanged callback)
		{
			return false;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002B4A File Offset: 0x00000D4A
		bool IPlatformServices.UnregisterPermissionChangeEvent(PlayerId targetPlayerId, Permission permission, PermissionChanged callback)
		{
			return false;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002B4D File Offset: 0x00000D4D
		void IPlatformServices.ShowRestrictedInformation()
		{
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002B4F File Offset: 0x00000D4F
		Task<bool> IPlatformServices.VerifyString(string content)
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002B57 File Offset: 0x00000D57
		void IPlatformServices.OnFocusGained()
		{
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002B59 File Offset: 0x00000D59
		void IPlatformServices.GetPlatformId(PlayerId playerId, Action<object> callback)
		{
			callback(EpicPlatformServices.PlayerIdToEpicAccountId(playerId));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002B68 File Offset: 0x00000D68
		internal async Task<string> GetUserName(PlayerId providedId)
		{
			string text;
			if (!providedId.IsValid || providedId.ProvidedType != PlayerIdProvidedTypes.Epic)
			{
				text = null;
			}
			else
			{
				EpicAccountId epicAccountId = EpicPlatformServices.PlayerIdToEpicAccountId(providedId);
				UserInfoData? userInfoData = await this.GetUserInfo(epicAccountId);
				if (userInfoData == null)
				{
					text = "";
				}
				else
				{
					text = userInfoData.Value.DisplayName;
				}
			}
			return text;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002BB8 File Offset: 0x00000DB8
		internal async Task<bool> GetUserOnlineStatus(PlayerId providedId)
		{
			EpicAccountId targetUserId = EpicPlatformServices.PlayerIdToEpicAccountId(providedId);
			await this.GetUserInfo(targetUserId);
			Info? info = await this.GetUserPresence(targetUserId);
			bool flag;
			if (info == null)
			{
				flag = false;
			}
			else
			{
				flag = info.Value.Status == 1;
			}
			return flag;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002C08 File Offset: 0x00000E08
		internal async Task<bool> IsPlayingThisGame(PlayerId providedId)
		{
			Info? info = await this.GetUserPresence(EpicPlatformServices.PlayerIdToEpicAccountId(providedId));
			bool flag;
			if (info == null)
			{
				flag = false;
			}
			else
			{
				flag = info.Value.ProductId == "6372ed7350f34ffc9ace219dff4b9f40";
			}
			return flag;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002C58 File Offset: 0x00000E58
		internal Task<PlayerId> GetUserWithName(string name)
		{
			TaskCompletionSource<PlayerId> tsc = new TaskCompletionSource<PlayerId>();
			QueryUserInfoByDisplayNameOptions queryUserInfoByDisplayNameOptions = default(QueryUserInfoByDisplayNameOptions);
			queryUserInfoByDisplayNameOptions.LocalUserId = this._epicAccountId;
			queryUserInfoByDisplayNameOptions.DisplayName = name;
			QueryUserInfoByDisplayNameOptions queryUserInfoByDisplayNameOptions2 = queryUserInfoByDisplayNameOptions;
			this._platform.GetUserInfoInterface().QueryUserInfoByDisplayName(ref queryUserInfoByDisplayNameOptions2, null, delegate(ref QueryUserInfoByDisplayNameCallbackInfo callbackInfo)
			{
				if (callbackInfo.ResultCode == null)
				{
					PlayerId playerId = EpicPlatformServices.EpicAccountIdToPlayerId(callbackInfo.TargetUserId);
					tsc.SetResult(playerId);
					return;
				}
				throw new Exception("Could not retrieve player from EOS");
			});
			return tsc.Task;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002CC4 File Offset: 0x00000EC4
		internal IEnumerable<PlayerId> GetAllFriends()
		{
			List<PlayerId> friends = new List<PlayerId>();
			bool? success = null;
			QueryFriendsOptions queryFriendsOptions = default(QueryFriendsOptions);
			queryFriendsOptions.LocalUserId = this._epicAccountId;
			QueryFriendsOptions queryFriendsOptions2 = queryFriendsOptions;
			this._platform.GetFriendsInterface().QueryFriends(ref queryFriendsOptions2, null, delegate(ref QueryFriendsCallbackInfo callbackInfo)
			{
				if (callbackInfo.ResultCode == null)
				{
					GetFriendsCountOptions getFriendsCountOptions = default(GetFriendsCountOptions);
					getFriendsCountOptions.LocalUserId = this._epicAccountId;
					GetFriendsCountOptions getFriendsCountOptions2 = getFriendsCountOptions;
					int friendsCount = this._platform.GetFriendsInterface().GetFriendsCount(ref getFriendsCountOptions2);
					for (int i = 0; i < friendsCount; i++)
					{
						GetFriendAtIndexOptions getFriendAtIndexOptions = default(GetFriendAtIndexOptions);
						getFriendAtIndexOptions.LocalUserId = this._epicAccountId;
						getFriendAtIndexOptions.Index = i;
						GetFriendAtIndexOptions getFriendAtIndexOptions2 = getFriendAtIndexOptions;
						EpicAccountId friendAtIndex = this._platform.GetFriendsInterface().GetFriendAtIndex(ref getFriendAtIndexOptions2);
						friends.Add(EpicPlatformServices.EpicAccountIdToPlayerId(friendAtIndex));
					}
					success = new bool?(true);
					return;
				}
				success = new bool?(false);
			});
			while (success == null)
			{
				this._platform.Tick();
				Task.Delay(5);
			}
			return friends;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002D54 File Offset: 0x00000F54
		public void QueryDefinitions()
		{
			AchievementsInterface achievementsInterface = this._platform.GetAchievementsInterface();
			QueryDefinitionsOptions queryDefinitionsOptions = default(QueryDefinitionsOptions);
			queryDefinitionsOptions.LocalUserId = this._localUserId;
			QueryDefinitionsOptions queryDefinitionsOptions2 = queryDefinitionsOptions;
			achievementsInterface.QueryDefinitions(ref queryDefinitionsOptions2, null, delegate(ref OnQueryDefinitionsCompleteCallbackInfo data)
			{
			});
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002DAA File Offset: 0x00000FAA
		internal bool SetStat(string name, int value)
		{
			this._ingestStatsQueue.Add(new EpicPlatformServices.IngestStatsQueueItem
			{
				Name = name,
				Value = value
			});
			return true;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002DCC File Offset: 0x00000FCC
		internal Task<int> GetStat(string name)
		{
			StatsInterface statsInterface = this._platform.GetStatsInterface();
			CopyStatByNameOptions copyStatByNameOptions = default(CopyStatByNameOptions);
			copyStatByNameOptions.Name = name;
			copyStatByNameOptions.TargetUserId = this._localUserId;
			CopyStatByNameOptions copyStatByNameOptions2 = copyStatByNameOptions;
			Stat? stat;
			if (statsInterface.CopyStatByName(ref copyStatByNameOptions2, ref stat) == null)
			{
				return Task.FromResult<int>(stat.Value.Value);
			}
			return Task.FromResult<int>(-1);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002E30 File Offset: 0x00001030
		internal Task<int[]> GetStats(string[] names)
		{
			List<int> list = new List<int>();
			foreach (string text in names)
			{
				list.Add(this.GetStat(text).Result);
			}
			return Task.FromResult<int[]>(list.ToArray());
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002E74 File Offset: 0x00001074
		private void ProcessIngestStatsQueue()
		{
			if (!this._writingStats && DateTime.Now.Subtract(this._statsLastWrittenOn).TotalSeconds > 5.0 && this._ingestStatsQueue.Count > 0)
			{
				this._statsLastWrittenOn = DateTime.Now;
				this._writingStats = true;
				StatsInterface statsInterface = this._platform.GetStatsInterface();
				List<IngestData> stats = new List<IngestData>();
				while (this._ingestStatsQueue.Count > 0)
				{
					EpicPlatformServices.IngestStatsQueueItem ingestStatsQueueItem;
					if (this._ingestStatsQueue.TryTake(out ingestStatsQueueItem))
					{
						List<IngestData> stats2 = stats;
						IngestData ingestData = default(IngestData);
						ingestData.StatName = ingestStatsQueueItem.Name;
						ingestData.IngestAmount = ingestStatsQueueItem.Value;
						stats2.Add(ingestData);
					}
				}
				IngestStatOptions ingestStatOptions = default(IngestStatOptions);
				ingestStatOptions.Stats = stats.ToArray();
				ingestStatOptions.LocalUserId = this._localUserId;
				ingestStatOptions.TargetUserId = this._localUserId;
				IngestStatOptions ingestStatOptions2 = ingestStatOptions;
				statsInterface.IngestStat(ref ingestStatOptions2, null, delegate(ref IngestStatCompleteCallbackInfo data)
				{
					if (data.ResultCode != null)
					{
						foreach (IngestData ingestData2 in stats)
						{
							this._ingestStatsQueue.Add(new EpicPlatformServices.IngestStatsQueueItem
							{
								Name = ingestData2.StatName,
								Value = ingestData2.IngestAmount
							});
						}
					}
					this.QueryStats();
					this._writingStats = false;
				});
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002F9E File Offset: 0x0000119E
		private static PlayerId EpicAccountIdToPlayerId(EpicAccountId epicAccountId)
		{
			return new PlayerId(3, epicAccountId.ToString());
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002FAC File Offset: 0x000011AC
		private static EpicAccountId PlayerIdToEpicAccountId(PlayerId playerId)
		{
			byte[] array = new ArraySegment<byte>(playerId.ToByteArray(), 16, 16).ToArray<byte>();
			Guid guid = new Guid(array);
			return EpicAccountId.FromString(guid.ToString("N"));
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002FF4 File Offset: 0x000011F4
		private Task<UserInfoData?> GetUserInfo(EpicAccountId targetUserId)
		{
			TaskCompletionSource<UserInfoData?> tsc = new TaskCompletionSource<UserInfoData?>();
			QueryUserInfoOptions queryUserInfoOptions = default(QueryUserInfoOptions);
			queryUserInfoOptions.LocalUserId = this._epicAccountId;
			queryUserInfoOptions.TargetUserId = targetUserId;
			QueryUserInfoOptions queryUserInfoOptions2 = queryUserInfoOptions;
			this._platform.GetUserInfoInterface().QueryUserInfo(ref queryUserInfoOptions2, null, delegate(ref QueryUserInfoCallbackInfo callbackInfo)
			{
				if (callbackInfo.ResultCode == null)
				{
					CopyUserInfoOptions copyUserInfoOptions = default(CopyUserInfoOptions);
					copyUserInfoOptions.LocalUserId = this._epicAccountId;
					copyUserInfoOptions.TargetUserId = targetUserId;
					CopyUserInfoOptions copyUserInfoOptions2 = copyUserInfoOptions;
					UserInfoData? userInfoData;
					this._platform.GetUserInfoInterface().CopyUserInfo(ref copyUserInfoOptions2, ref userInfoData);
					tsc.SetResult(userInfoData);
					return;
				}
				tsc.SetResult(null);
			});
			return tsc.Task;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003070 File Offset: 0x00001270
		private Task<Info?> GetUserPresence(EpicAccountId targetUserId)
		{
			TaskCompletionSource<Info?> tsc = new TaskCompletionSource<Info?>();
			QueryPresenceOptions queryPresenceOptions = default(QueryPresenceOptions);
			queryPresenceOptions.LocalUserId = this._epicAccountId;
			queryPresenceOptions.TargetUserId = targetUserId;
			QueryPresenceOptions queryPresenceOptions2 = queryPresenceOptions;
			this._platform.GetPresenceInterface().QueryPresence(ref queryPresenceOptions2, null, delegate(ref QueryPresenceCallbackInfo callbackInfo)
			{
				if (callbackInfo.ResultCode != null)
				{
					tsc.SetResult(null);
					return;
				}
				HasPresenceOptions hasPresenceOptions = default(HasPresenceOptions);
				hasPresenceOptions.LocalUserId = this._epicAccountId;
				hasPresenceOptions.TargetUserId = targetUserId;
				HasPresenceOptions hasPresenceOptions2 = hasPresenceOptions;
				if (this._platform.GetPresenceInterface().HasPresence(ref hasPresenceOptions2))
				{
					CopyPresenceOptions copyPresenceOptions = default(CopyPresenceOptions);
					copyPresenceOptions.LocalUserId = this._epicAccountId;
					copyPresenceOptions.TargetUserId = targetUserId;
					CopyPresenceOptions copyPresenceOptions2 = copyPresenceOptions;
					Info? info;
					this._platform.GetPresenceInterface().CopyPresence(ref copyPresenceOptions2, ref info);
					tsc.SetResult(info);
					return;
				}
				tsc.SetResult(null);
			});
			return tsc.Task;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000030EC File Offset: 0x000012EC
		private void QueryStats()
		{
			QueryStatsOptions queryStatsOptions = default(QueryStatsOptions);
			queryStatsOptions.LocalUserId = this._localUserId;
			queryStatsOptions.TargetUserId = this._localUserId;
			QueryStatsOptions queryStatsOptions2 = queryStatsOptions;
			this._platform.GetStatsInterface().QueryStats(ref queryStatsOptions2, null, delegate(ref OnQueryStatsCompleteCallbackInfo data)
			{
			});
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000314F File Offset: 0x0000134F
		IFriendListService[] IPlatformServices.GetFriendListServices()
		{
			return this._friendListServices;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003157 File Offset: 0x00001357
		public bool ShowGamepadTextInput(string descriptionText, string existingText, uint maxChars, bool isObfuscated)
		{
			return false;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000315A File Offset: 0x0000135A
		bool IPlatformServices.UsePlatformInvitationService(PlayerId targetPlayerId)
		{
			return false;
		}

		// Token: 0x04000009 RID: 9
		private EpicAccountId _epicAccountId;

		// Token: 0x0400000A RID: 10
		private ProductUserId _localUserId;

		// Token: 0x0400000B RID: 11
		private string _accessToken;

		// Token: 0x0400000C RID: 12
		private string _epicUserName;

		// Token: 0x0400000D RID: 13
		private PlatformInterface _platform;

		// Token: 0x0400000E RID: 14
		private PlatformInitParams _initParams;

		// Token: 0x0400000F RID: 15
		private EpicFriendListService _epicFriendListService;

		// Token: 0x04000010 RID: 16
		private IFriendListService[] _friendListServices;

		// Token: 0x04000017 RID: 23
		private TextObject _initFailReason;

		// Token: 0x04000018 RID: 24
		private ulong _refreshConnectionCallbackId;

		// Token: 0x04000019 RID: 25
		private ConcurrentBag<EpicPlatformServices.IngestStatsQueueItem> _ingestStatsQueue = new ConcurrentBag<EpicPlatformServices.IngestStatsQueueItem>();

		// Token: 0x0400001A RID: 26
		private bool _writingStats;

		// Token: 0x0400001B RID: 27
		private DateTime _statsLastWrittenOn = DateTime.MinValue;

		// Token: 0x0400001C RID: 28
		private const int MinStatsWriteInterval = 5;

		// Token: 0x02000007 RID: 7
		private class IngestStatsQueueItem
		{
			// Token: 0x17000011 RID: 17
			// (get) Token: 0x06000078 RID: 120 RVA: 0x0000337A File Offset: 0x0000157A
			// (set) Token: 0x06000079 RID: 121 RVA: 0x00003382 File Offset: 0x00001582
			public string Name { get; set; }

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x0600007A RID: 122 RVA: 0x0000338B File Offset: 0x0000158B
			// (set) Token: 0x0600007B RID: 123 RVA: 0x00003393 File Offset: 0x00001593
			public int Value { get; set; }
		}

		// Token: 0x02000008 RID: 8
		private class EpicAuthErrorResponse
		{
			// Token: 0x17000013 RID: 19
			// (get) Token: 0x0600007D RID: 125 RVA: 0x000033A4 File Offset: 0x000015A4
			// (set) Token: 0x0600007E RID: 126 RVA: 0x000033AC File Offset: 0x000015AC
			[JsonProperty("errorCode")]
			public string ErrorCode { get; set; }

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x0600007F RID: 127 RVA: 0x000033B5 File Offset: 0x000015B5
			// (set) Token: 0x06000080 RID: 128 RVA: 0x000033BD File Offset: 0x000015BD
			[JsonProperty("errorMessage")]
			public string ErrorMessage { get; set; }

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x06000081 RID: 129 RVA: 0x000033C6 File Offset: 0x000015C6
			// (set) Token: 0x06000082 RID: 130 RVA: 0x000033CE File Offset: 0x000015CE
			[JsonProperty("numericErrorCode")]
			public int NumericErrorCode { get; set; }

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x06000083 RID: 131 RVA: 0x000033D7 File Offset: 0x000015D7
			// (set) Token: 0x06000084 RID: 132 RVA: 0x000033DF File Offset: 0x000015DF
			[JsonProperty("error_description")]
			public string ErrorDescription { get; set; }

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x06000085 RID: 133 RVA: 0x000033E8 File Offset: 0x000015E8
			// (set) Token: 0x06000086 RID: 134 RVA: 0x000033F0 File Offset: 0x000015F0
			[JsonProperty("error")]
			public string Error { get; set; }
		}

		// Token: 0x02000009 RID: 9
		private class EpicAuthResponse
		{
			// Token: 0x17000018 RID: 24
			// (get) Token: 0x06000088 RID: 136 RVA: 0x00003401 File Offset: 0x00001601
			// (set) Token: 0x06000089 RID: 137 RVA: 0x00003409 File Offset: 0x00001609
			[JsonProperty("access_token")]
			public string AccessToken { get; set; }

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x0600008A RID: 138 RVA: 0x00003412 File Offset: 0x00001612
			// (set) Token: 0x0600008B RID: 139 RVA: 0x0000341A File Offset: 0x0000161A
			[JsonProperty("expires_in")]
			public int ExpiresIn { get; set; }

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x0600008C RID: 140 RVA: 0x00003423 File Offset: 0x00001623
			// (set) Token: 0x0600008D RID: 141 RVA: 0x0000342B File Offset: 0x0000162B
			[JsonProperty("expires_at")]
			public DateTime ExpiresAt { get; set; }

			// Token: 0x1700001B RID: 27
			// (get) Token: 0x0600008E RID: 142 RVA: 0x00003434 File Offset: 0x00001634
			// (set) Token: 0x0600008F RID: 143 RVA: 0x0000343C File Offset: 0x0000163C
			[JsonProperty("token_type")]
			public string TokenType { get; set; }

			// Token: 0x1700001C RID: 28
			// (get) Token: 0x06000090 RID: 144 RVA: 0x00003445 File Offset: 0x00001645
			// (set) Token: 0x06000091 RID: 145 RVA: 0x0000344D File Offset: 0x0000164D
			[JsonProperty("refresh_token")]
			public string RefreshToken { get; set; }

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x06000092 RID: 146 RVA: 0x00003456 File Offset: 0x00001656
			// (set) Token: 0x06000093 RID: 147 RVA: 0x0000345E File Offset: 0x0000165E
			[JsonProperty("refresh_expires")]
			public int RefreshExpires { get; set; }

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x06000094 RID: 148 RVA: 0x00003467 File Offset: 0x00001667
			// (set) Token: 0x06000095 RID: 149 RVA: 0x0000346F File Offset: 0x0000166F
			[JsonProperty("refresh_expires_at")]
			public DateTime RefreshExpiresAt { get; set; }

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x06000096 RID: 150 RVA: 0x00003478 File Offset: 0x00001678
			// (set) Token: 0x06000097 RID: 151 RVA: 0x00003480 File Offset: 0x00001680
			[JsonProperty("account_id")]
			public string AccountId { get; set; }

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x06000098 RID: 152 RVA: 0x00003489 File Offset: 0x00001689
			// (set) Token: 0x06000099 RID: 153 RVA: 0x00003491 File Offset: 0x00001691
			[JsonProperty("client_id")]
			public string ClientId { get; set; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x0600009A RID: 154 RVA: 0x0000349A File Offset: 0x0000169A
			// (set) Token: 0x0600009B RID: 155 RVA: 0x000034A2 File Offset: 0x000016A2
			[JsonProperty("internal_client")]
			public bool InternalClient { get; set; }

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x0600009C RID: 156 RVA: 0x000034AB File Offset: 0x000016AB
			// (set) Token: 0x0600009D RID: 157 RVA: 0x000034B3 File Offset: 0x000016B3
			[JsonProperty("client_service")]
			public string ClientService { get; set; }

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x0600009E RID: 158 RVA: 0x000034BC File Offset: 0x000016BC
			// (set) Token: 0x0600009F RID: 159 RVA: 0x000034C4 File Offset: 0x000016C4
			[JsonProperty("displayName")]
			public string DisplayName { get; set; }

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x060000A0 RID: 160 RVA: 0x000034CD File Offset: 0x000016CD
			// (set) Token: 0x060000A1 RID: 161 RVA: 0x000034D5 File Offset: 0x000016D5
			[JsonProperty("app")]
			public string App { get; set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x060000A2 RID: 162 RVA: 0x000034DE File Offset: 0x000016DE
			// (set) Token: 0x060000A3 RID: 163 RVA: 0x000034E6 File Offset: 0x000016E6
			[JsonProperty("in_app_id")]
			public string InAppId { get; set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x060000A4 RID: 164 RVA: 0x000034EF File Offset: 0x000016EF
			// (set) Token: 0x060000A5 RID: 165 RVA: 0x000034F7 File Offset: 0x000016F7
			[JsonProperty("device_id")]
			public string DeviceId { get; set; }

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003500 File Offset: 0x00001700
			// (set) Token: 0x060000A7 RID: 167 RVA: 0x00003508 File Offset: 0x00001708
			[JsonProperty("product_id")]
			public string ProductId { get; set; }
		}
	}
}
