using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E9 RID: 745
	public class BannerlordFriendListService : IFriendListService
	{
		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06002B49 RID: 11081 RVA: 0x000A7104 File Offset: 0x000A5304
		// (remove) Token: 0x06002B4A RID: 11082 RVA: 0x000A713C File Offset: 0x000A533C
		public event Action<PlayerId> OnUserStatusChanged;

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06002B4B RID: 11083 RVA: 0x000A7174 File Offset: 0x000A5374
		// (remove) Token: 0x06002B4C RID: 11084 RVA: 0x000A71AC File Offset: 0x000A53AC
		public event Action<PlayerId> OnFriendRemoved;

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06002B4D RID: 11085 RVA: 0x000A71E4 File Offset: 0x000A53E4
		// (remove) Token: 0x06002B4E RID: 11086 RVA: 0x000A721C File Offset: 0x000A541C
		public event Action OnFriendListChanged;

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06002B4F RID: 11087 RVA: 0x000A7251 File Offset: 0x000A5451
		bool IFriendListService.InGameStatusFetchable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06002B50 RID: 11088 RVA: 0x000A7254 File Offset: 0x000A5454
		bool IFriendListService.AllowsFriendOperations
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06002B51 RID: 11089 RVA: 0x000A7257 File Offset: 0x000A5457
		bool IFriendListService.CanInvitePlayersToPlatformSession
		{
			get
			{
				return PlatformServices.InvitationServices != null;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06002B52 RID: 11090 RVA: 0x000A7261 File Offset: 0x000A5461
		bool IFriendListService.IncludeInAllFriends
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x000A7264 File Offset: 0x000A5464
		public BannerlordFriendListService()
		{
			this.Friends = new List<FriendInfo>();
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000A7277 File Offset: 0x000A5477
		string IFriendListService.GetServiceCodeName()
		{
			return "TaleWorlds";
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000A727E File Offset: 0x000A547E
		TextObject IFriendListService.GetServiceLocalizedName()
		{
			return new TextObject("{=!}TaleWorlds", null);
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x000A728B File Offset: 0x000A548B
		FriendListServiceType IFriendListService.GetFriendListServiceType()
		{
			return FriendListServiceType.Bannerlord;
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x000A7290 File Offset: 0x000A5490
		IEnumerable<PlayerId> IFriendListService.GetPendingRequests()
		{
			return from f in this.Friends
				where f.Status == FriendStatus.Pending
				select f.Id;
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x000A72EC File Offset: 0x000A54EC
		IEnumerable<PlayerId> IFriendListService.GetReceivedRequests()
		{
			return from f in this.Friends
				where f.Status == FriendStatus.Received
				select f.Id;
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000A7348 File Offset: 0x000A5548
		IEnumerable<PlayerId> IFriendListService.GetAllFriends()
		{
			return from f in this.Friends
				where f.Status == FriendStatus.Accepted
				select f.Id;
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x000A73A4 File Offset: 0x000A55A4
		Task<bool> IFriendListService.GetUserOnlineStatus(PlayerId providedId)
		{
			foreach (FriendInfo friendInfo in this.Friends)
			{
				if (friendInfo.Id.Equals(providedId))
				{
					return Task.FromResult<bool>(friendInfo.IsOnline);
				}
			}
			return Task.FromResult<bool>(false);
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x000A7418 File Offset: 0x000A5618
		Task<bool> IFriendListService.IsPlayingThisGame(PlayerId providedId)
		{
			return ((IFriendListService)this).GetUserOnlineStatus(providedId);
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x000A7424 File Offset: 0x000A5624
		Task<string> IFriendListService.GetUserName(PlayerId providedId)
		{
			foreach (FriendInfo friendInfo in this.Friends)
			{
				if (friendInfo.Id.Equals(providedId))
				{
					return Task.FromResult<string>(friendInfo.Name);
				}
			}
			return Task.FromResult<string>(null);
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000A7498 File Offset: 0x000A5698
		Task<PlayerId> IFriendListService.GetUserWithName(string name)
		{
			foreach (FriendInfo friendInfo in this.Friends)
			{
				if (friendInfo.Name == name)
				{
					return Task.FromResult<PlayerId>(friendInfo.Id);
				}
			}
			return Task.FromResult<PlayerId>(default(PlayerId));
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x000A7510 File Offset: 0x000A5710
		public void OnFriendListReceived(FriendInfo[] friends)
		{
			List<FriendInfo> friends2 = this.Friends;
			this.Friends = new List<FriendInfo>(friends);
			List<PlayerId> list = null;
			bool flag = false;
			using (List<FriendInfo>.Enumerator enumerator = this.Friends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FriendInfo friend = enumerator.Current;
					int num = friends2.FindIndex((FriendInfo o) => o.Id.Equals(friend.Id));
					if (num < 0)
					{
						flag = true;
					}
					else
					{
						FriendInfo friendInfo = friends2[num];
						friends2.RemoveAt(num);
						if (friendInfo.Status != friend.Status)
						{
							flag = true;
						}
						else if (friendInfo.IsOnline != friend.IsOnline)
						{
							if (list == null)
							{
								list = new List<PlayerId>();
							}
							list.Add(friendInfo.Id);
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				if (friends2.Count > 0)
				{
					foreach (FriendInfo friendInfo2 in friends2)
					{
						Action<PlayerId> onFriendRemoved = this.OnFriendRemoved;
						if (onFriendRemoved != null)
						{
							onFriendRemoved(friendInfo2.Id);
						}
					}
				}
				if (list != null)
				{
					foreach (PlayerId playerId in list)
					{
						Action<PlayerId> onUserStatusChanged = this.OnUserStatusChanged;
						if (onUserStatusChanged != null)
						{
							onUserStatusChanged(playerId);
						}
					}
				}
				return;
			}
			Action onFriendListChanged = this.OnFriendListChanged;
			if (onFriendListChanged == null)
			{
				return;
			}
			onFriendListChanged();
		}

		// Token: 0x04001077 RID: 4215
		protected List<FriendInfo> Friends;
	}
}
