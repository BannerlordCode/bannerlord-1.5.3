using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamworks;
using TaleWorlds.Localization;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.PlatformService.Steam
{
	// Token: 0x02000003 RID: 3
	public class SteamFriendListService : IFriendListService
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002223 File Offset: 0x00000423
		public SteamFriendListService(SteamPlatformServices steamPlatformServices)
		{
			this._steamPlatformServices = steamPlatformServices;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000012 RID: 18 RVA: 0x00002234 File Offset: 0x00000434
		// (remove) Token: 0x06000013 RID: 19 RVA: 0x0000226C File Offset: 0x0000046C
		public event Action<PlayerId> OnUserStatusChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000014 RID: 20 RVA: 0x000022A4 File Offset: 0x000004A4
		// (remove) Token: 0x06000015 RID: 21 RVA: 0x000022DC File Offset: 0x000004DC
		public event Action<PlayerId> OnFriendRemoved;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000016 RID: 22 RVA: 0x00002314 File Offset: 0x00000514
		// (remove) Token: 0x06000017 RID: 23 RVA: 0x0000234C File Offset: 0x0000054C
		public event Action OnFriendListChanged;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002381 File Offset: 0x00000581
		bool IFriendListService.InGameStatusFetchable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002384 File Offset: 0x00000584
		bool IFriendListService.AllowsFriendOperations
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002387 File Offset: 0x00000587
		bool IFriendListService.CanInvitePlayersToPlatformSession
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000238A File Offset: 0x0000058A
		bool IFriendListService.IncludeInAllFriends
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000238D File Offset: 0x0000058D
		string IFriendListService.GetServiceCodeName()
		{
			return "Steam";
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002394 File Offset: 0x00000594
		TextObject IFriendListService.GetServiceLocalizedName()
		{
			return new TextObject("{=!}Steam", null);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000023A1 File Offset: 0x000005A1
		FriendListServiceType IFriendListService.GetFriendListServiceType()
		{
			return FriendListServiceType.Steam;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000023A4 File Offset: 0x000005A4
		IEnumerable<PlayerId> IFriendListService.GetAllFriends()
		{
			if (SteamAPI.IsSteamRunning() && this._steamPlatformServices.Initialized)
			{
				int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
				int num;
				for (int i = 0; i < friendCount; i = num)
				{
					yield return SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate).ToPlayerId();
					num = i + 1;
				}
			}
			yield break;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000023B4 File Offset: 0x000005B4
		Task<bool> IFriendListService.GetUserOnlineStatus(PlayerId providedId)
		{
			return this._steamPlatformServices.GetUserOnlineStatus(providedId);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000023C2 File Offset: 0x000005C2
		Task<bool> IFriendListService.IsPlayingThisGame(PlayerId providedId)
		{
			return this._steamPlatformServices.IsPlayingThisGame(providedId);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000023D0 File Offset: 0x000005D0
		Task<string> IFriendListService.GetUserName(PlayerId providedId)
		{
			return this._steamPlatformServices.GetUserName(providedId);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000023DE File Offset: 0x000005DE
		Task<PlayerId> IFriendListService.GetUserWithName(string name)
		{
			return this._steamPlatformServices.GetUserWithName(name);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000023EC File Offset: 0x000005EC
		internal void HandleOnUserStatusChanged(PlayerId playerId)
		{
			if (this.OnUserStatusChanged != null)
			{
				this.OnUserStatusChanged(playerId);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002404 File Offset: 0x00000604
		private void Dummy()
		{
			if (this.OnFriendRemoved != null)
			{
				this.OnFriendRemoved(default(PlayerId));
			}
			if (this.OnFriendListChanged != null)
			{
				this.OnFriendListChanged();
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002440 File Offset: 0x00000640
		IEnumerable<PlayerId> IFriendListService.GetPendingRequests()
		{
			return null;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002443 File Offset: 0x00000643
		IEnumerable<PlayerId> IFriendListService.GetReceivedRequests()
		{
			return null;
		}

		// Token: 0x04000009 RID: 9
		private SteamPlatformServices _steamPlatformServices;
	}
}
