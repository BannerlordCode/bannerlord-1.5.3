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
	// Token: 0x0200000F RID: 15
	public class NullPlatformServices : IPlatformServices
	{
		// Token: 0x06000058 RID: 88 RVA: 0x00002054 File Offset: 0x00000254
		public NullPlatformServices()
		{
			this._testFriendListService = new TestFriendListService("NULL", default(PlayerId));
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002080 File Offset: 0x00000280
		string IPlatformServices.ProviderName
		{
			get
			{
				return "Null";
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002087 File Offset: 0x00000287
		string IPlatformServices.UserId
		{
			get
			{
				return "";
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000208E File Offset: 0x0000028E
		PlayerId IPlatformServices.PlayerId
		{
			get
			{
				return PlayerId.Empty;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002095 File Offset: 0x00000295
		string IPlatformServices.UserDisplayName
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005D RID: 93 RVA: 0x0000209C File Offset: 0x0000029C
		IReadOnlyCollection<PlayerId> IPlatformServices.BlockedUsers
		{
			get
			{
				return new List<PlayerId>();
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000020A3 File Offset: 0x000002A3
		bool IPlatformServices.IsPermanentMuteAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000020A6 File Offset: 0x000002A6
		bool IPlatformServices.Initialize(IFriendListService[] additionalFriendListServices)
		{
			return false;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000020A9 File Offset: 0x000002A9
		void IPlatformServices.Terminate()
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000020AB File Offset: 0x000002AB
		bool IPlatformServices.IsPlayerProfileCardAvailable(PlayerId providedId)
		{
			return false;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000020AE File Offset: 0x000002AE
		void IPlatformServices.ShowPlayerProfileCard(PlayerId providedId)
		{
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000020B0 File Offset: 0x000002B0
		bool IPlatformServices.UserLoggedIn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000020B3 File Offset: 0x000002B3
		void IPlatformServices.LoginUser()
		{
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000020B5 File Offset: 0x000002B5
		Task<AvatarData> IPlatformServices.GetUserAvatar(PlayerId providedId)
		{
			return Task.FromResult<AvatarData>(null);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000020BD File Offset: 0x000002BD
		Task<bool> IPlatformServices.ShowOverlayForWebPage(string url)
		{
			return Task.FromResult<bool>(false);
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000067 RID: 103 RVA: 0x000020C8 File Offset: 0x000002C8
		// (remove) Token: 0x06000068 RID: 104 RVA: 0x00002100 File Offset: 0x00000300
		public event Action<PlayerId> OnUserStatusChanged;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000069 RID: 105 RVA: 0x00002138 File Offset: 0x00000338
		// (remove) Token: 0x0600006A RID: 106 RVA: 0x00002170 File Offset: 0x00000370
		public event Action<AvatarData> OnAvatarUpdated;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600006B RID: 107 RVA: 0x000021A8 File Offset: 0x000003A8
		// (remove) Token: 0x0600006C RID: 108 RVA: 0x000021E0 File Offset: 0x000003E0
		public event Action OnBlockedUserListUpdated;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600006D RID: 109 RVA: 0x00002218 File Offset: 0x00000418
		// (remove) Token: 0x0600006E RID: 110 RVA: 0x00002250 File Offset: 0x00000450
		public event Action<string> OnNameUpdated;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600006F RID: 111 RVA: 0x00002288 File Offset: 0x00000488
		// (remove) Token: 0x06000070 RID: 112 RVA: 0x000022C0 File Offset: 0x000004C0
		public event Action<bool, TextObject> OnSignInStateUpdated;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000071 RID: 113 RVA: 0x000022F8 File Offset: 0x000004F8
		// (remove) Token: 0x06000072 RID: 114 RVA: 0x00002330 File Offset: 0x00000530
		public event Action<string> OnTextEnteredFromPlatform;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000073 RID: 115 RVA: 0x00002368 File Offset: 0x00000568
		// (remove) Token: 0x06000074 RID: 116 RVA: 0x000023A0 File Offset: 0x000005A0
		public event Action OnTextCanceledFromPlatform;

		// Token: 0x06000075 RID: 117 RVA: 0x000023D8 File Offset: 0x000005D8
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
			if (this.OnUserStatusChanged != null)
			{
				this.OnUserStatusChanged(default(PlayerId));
			}
			if (this.OnSignInStateUpdated != null)
			{
				this.OnSignInStateUpdated(false, null);
			}
			if (this.OnTextEnteredFromPlatform != null)
			{
				this.OnTextEnteredFromPlatform(null);
			}
			if (this.OnTextCanceledFromPlatform != null)
			{
				this.OnTextCanceledFromPlatform();
			}
			if (this.OnBlockedUserListUpdated != null)
			{
				this.OnBlockedUserListUpdated();
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002478 File Offset: 0x00000678
		public void Tick(float dt)
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000247A File Offset: 0x0000067A
		PlatformInitParams IPlatformServices.GetInitParams()
		{
			return new PlatformInitParams();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002481 File Offset: 0x00000681
		IFriendListService[] IPlatformServices.GetFriendListServices()
		{
			return new IFriendListService[] { this._testFriendListService };
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002492 File Offset: 0x00000692
		public void ActivateFriendList()
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002494 File Offset: 0x00000694
		Task<ILoginAccessProvider> IPlatformServices.CreateLobbyClientLoginProvider()
		{
			return Task.FromResult<ILoginAccessProvider>(new TestLoginAccessProvider());
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000024A0 File Offset: 0x000006A0
		IAchievementService IPlatformServices.GetAchievementService()
		{
			return new TestAchievementService();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000024A7 File Offset: 0x000006A7
		IActivityService IPlatformServices.GetActivityService()
		{
			return new TestActivityService();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000024AE File Offset: 0x000006AE
		void IPlatformServices.CheckPrivilege(Privilege privilege, bool displayResolveUI, PrivilegeResult callback)
		{
			callback(true);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000024B7 File Offset: 0x000006B7
		void IPlatformServices.CheckPermissionWithUser(Permission privilege, PlayerId targetPlayerId, PermissionResult callback)
		{
			callback(true);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000024C0 File Offset: 0x000006C0
		void IPlatformServices.GetPlatformId(PlayerId playerId, Action<object> callback)
		{
			callback(-1);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000024CE File Offset: 0x000006CE
		Task<bool> IPlatformServices.VerifyString(string content)
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000024D6 File Offset: 0x000006D6
		void IPlatformServices.ShowRestrictedInformation()
		{
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000024D8 File Offset: 0x000006D8
		void IPlatformServices.OnFocusGained()
		{
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000024DA File Offset: 0x000006DA
		bool IPlatformServices.RegisterPermissionChangeEvent(PlayerId targetPlayerId, Permission permission, PermissionChanged Callback)
		{
			return true;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000024DD File Offset: 0x000006DD
		bool IPlatformServices.UnregisterPermissionChangeEvent(PlayerId targetPlayerId, Permission permission, PermissionChanged Callback)
		{
			return true;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000024E0 File Offset: 0x000006E0
		bool IPlatformServices.ShowGamepadTextInput(string descriptionText, string existingText, uint maxLine, bool isObfuscated)
		{
			return false;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000024E3 File Offset: 0x000006E3
		bool IPlatformServices.UsePlatformInvitationService(PlayerId targetPlayerId)
		{
			return false;
		}

		// Token: 0x0400001D RID: 29
		private TestFriendListService _testFriendListService;
	}
}
