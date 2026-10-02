using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleWorlds.AchievementSystem;
using TaleWorlds.ActivitySystem;
using TaleWorlds.Diamond;
using TaleWorlds.Diamond.AccessProvider.Test;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.PlayerServices;
using TaleWorlds.PlayerServices.Avatar;

namespace TaleWorlds.PlatformService
{
	// Token: 0x02000013 RID: 19
	public class TestPlatformServices : IPlatformServices
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00002A38 File Offset: 0x00000C38
		public TestPlatformServices(string userName)
		{
			this._userName = userName;
			this._loginAccessProvider = new TestLoginAccessProvider();
			ILoginAccessProvider loginAccessProvider = this._loginAccessProvider;
			loginAccessProvider.Initialize(this._userName, null);
			this._playerId = loginAccessProvider.GetPlayerId();
			this._testFriendListService = new TestFriendListService(userName, this._playerId);
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00002A8F File Offset: 0x00000C8F
		string IPlatformServices.ProviderName
		{
			get
			{
				return "Test";
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00002A98 File Offset: 0x00000C98
		string IPlatformServices.UserId
		{
			get
			{
				return this._playerId.ToString();
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00002AB9 File Offset: 0x00000CB9
		PlayerId IPlatformServices.PlayerId
		{
			get
			{
				return this._playerId;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00002AC1 File Offset: 0x00000CC1
		string IPlatformServices.UserDisplayName
		{
			get
			{
				return this._userName;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00002AC9 File Offset: 0x00000CC9
		IReadOnlyCollection<PlayerId> IPlatformServices.BlockedUsers
		{
			get
			{
				return new List<PlayerId>();
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00002AD0 File Offset: 0x00000CD0
		bool IPlatformServices.IsPermanentMuteAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00002AD3 File Offset: 0x00000CD3
		bool IPlatformServices.Initialize(IFriendListService[] additionalFriendListServices)
		{
			return false;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00002AD6 File Offset: 0x00000CD6
		void IPlatformServices.Terminate()
		{
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00002AD8 File Offset: 0x00000CD8
		bool IPlatformServices.IsPlayerProfileCardAvailable(PlayerId providedId)
		{
			return false;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00002ADB File Offset: 0x00000CDB
		bool IPlatformServices.UserLoggedIn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00002ADE File Offset: 0x00000CDE
		void IPlatformServices.LoginUser()
		{
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00002AE0 File Offset: 0x00000CE0
		void IPlatformServices.ShowPlayerProfileCard(PlayerId providedId)
		{
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00002AE2 File Offset: 0x00000CE2
		Task<AvatarData> IPlatformServices.GetUserAvatar(PlayerId providedId)
		{
			return Task.FromResult<AvatarData>(null);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00002AEA File Offset: 0x00000CEA
		IFriendListService[] IPlatformServices.GetFriendListServices()
		{
			return new IFriendListService[] { this._testFriendListService };
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00002AFB File Offset: 0x00000CFB
		IAchievementService IPlatformServices.GetAchievementService()
		{
			return new TestAchievementService();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00002B02 File Offset: 0x00000D02
		IActivityService IPlatformServices.GetActivityService()
		{
			return new TestActivityService();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00002B09 File Offset: 0x00000D09
		Task<bool> IPlatformServices.ShowOverlayForWebPage(string url)
		{
			return Task.FromResult<bool>(false);
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060000CC RID: 204 RVA: 0x00002B14 File Offset: 0x00000D14
		// (remove) Token: 0x060000CD RID: 205 RVA: 0x00002B4C File Offset: 0x00000D4C
		public event Action<AvatarData> OnAvatarUpdated;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060000CE RID: 206 RVA: 0x00002B84 File Offset: 0x00000D84
		// (remove) Token: 0x060000CF RID: 207 RVA: 0x00002BBC File Offset: 0x00000DBC
		public event Action<string> OnNameUpdated;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060000D0 RID: 208 RVA: 0x00002BF4 File Offset: 0x00000DF4
		// (remove) Token: 0x060000D1 RID: 209 RVA: 0x00002C2C File Offset: 0x00000E2C
		public event Action<bool, TextObject> OnSignInStateUpdated;

		// Token: 0x060000D2 RID: 210 RVA: 0x00002C64 File Offset: 0x00000E64
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

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060000D3 RID: 211 RVA: 0x00002CE8 File Offset: 0x00000EE8
		// (remove) Token: 0x060000D4 RID: 212 RVA: 0x00002D20 File Offset: 0x00000F20
		public event Action OnBlockedUserListUpdated;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060000D5 RID: 213 RVA: 0x00002D58 File Offset: 0x00000F58
		// (remove) Token: 0x060000D6 RID: 214 RVA: 0x00002D90 File Offset: 0x00000F90
		public event Action<string> OnTextEnteredFromPlatform;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060000D7 RID: 215 RVA: 0x00002DC8 File Offset: 0x00000FC8
		// (remove) Token: 0x060000D8 RID: 216 RVA: 0x00002E00 File Offset: 0x00001000
		public event Action OnTextCanceledFromPlatform;

		// Token: 0x060000D9 RID: 217 RVA: 0x00002E35 File Offset: 0x00001035
		public void Tick(float dt)
		{
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00002E37 File Offset: 0x00001037
		PlatformInitParams IPlatformServices.GetInitParams()
		{
			return new PlatformInitParams();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00002E3E File Offset: 0x0000103E
		public void ActivateFriendList()
		{
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00002E40 File Offset: 0x00001040
		Task<ILoginAccessProvider> IPlatformServices.CreateLobbyClientLoginProvider()
		{
			return Task.FromResult<ILoginAccessProvider>(this._loginAccessProvider);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00002E4D File Offset: 0x0000104D
		void IPlatformServices.CheckPrivilege(Privilege privilege, bool displayResolveUI, PrivilegeResult callback)
		{
			callback(true);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00002E56 File Offset: 0x00001056
		void IPlatformServices.CheckPermissionWithUser(Permission privilege, PlayerId targetPlayerId, PermissionResult callback)
		{
			callback(true);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00002E5F File Offset: 0x0000105F
		Task<bool> IPlatformServices.VerifyString(string content)
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00002E67 File Offset: 0x00001067
		void IPlatformServices.GetPlatformId(PlayerId playerId, Action<object> callback)
		{
			callback(0);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00002E75 File Offset: 0x00001075
		void IPlatformServices.ShowRestrictedInformation()
		{
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00002E77 File Offset: 0x00001077
		void IPlatformServices.OnFocusGained()
		{
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00002E79 File Offset: 0x00001079
		bool IPlatformServices.RegisterPermissionChangeEvent(PlayerId targetPlayerId, Permission permission, PermissionChanged callback)
		{
			return true;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00002E7C File Offset: 0x0000107C
		bool IPlatformServices.UnregisterPermissionChangeEvent(PlayerId targetPlayerId, Permission permission, PermissionChanged callback)
		{
			return true;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00002E7F File Offset: 0x0000107F
		bool IPlatformServices.ShowGamepadTextInput(string descriptionText, string existingText, uint maxLine, bool isObfuscated)
		{
			return false;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00002E82 File Offset: 0x00001082
		bool IPlatformServices.UsePlatformInvitationService(PlayerId targetPlayerId)
		{
			return false;
		}

		// Token: 0x04000038 RID: 56
		private readonly string _userName;

		// Token: 0x04000039 RID: 57
		private readonly PlayerId _playerId;

		// Token: 0x0400003A RID: 58
		private TestLoginAccessProvider _loginAccessProvider;

		// Token: 0x0400003B RID: 59
		private TestFriendListService _testFriendListService;
	}
}
