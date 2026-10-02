using System;
using System.Collections.Generic;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000390 RID: 912
	public class RecentPlayersFriendListService : BannerlordFriendListService, IFriendListService
	{
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x060034A3 RID: 13475 RVA: 0x000DA109 File Offset: 0x000D8309
		bool IFriendListService.IncludeInAllFriends
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x060034A4 RID: 13476 RVA: 0x000DA10C File Offset: 0x000D830C
		bool IFriendListService.CanInvitePlayersToPlatformSession
		{
			get
			{
				return PlatformServices.InvitationServices != null;
			}
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000DA116 File Offset: 0x000D8316
		TextObject IFriendListService.GetServiceLocalizedName()
		{
			return new TextObject("{=XvSRoOzM}Recently Played Players", null);
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000DA123 File Offset: 0x000D8323
		string IFriendListService.GetServiceCodeName()
		{
			return "RecentlyPlayedPlayers";
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x000DA12A File Offset: 0x000D832A
		IEnumerable<PlayerId> IFriendListService.GetAllFriends()
		{
			return RecentPlayersManager.GetPlayersOrdered();
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x000DA131 File Offset: 0x000D8331
		FriendListServiceType IFriendListService.GetFriendListServiceType()
		{
			return FriendListServiceType.RecentPlayers;
		}
	}
}
