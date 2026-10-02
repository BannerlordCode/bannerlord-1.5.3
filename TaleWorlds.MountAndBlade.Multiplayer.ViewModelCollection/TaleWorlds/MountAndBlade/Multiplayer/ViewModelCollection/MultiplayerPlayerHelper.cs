using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000014 RID: 20
	public static class MultiplayerPlayerHelper
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000059C2 File Offset: 0x00003BC2
		private static IReadOnlyCollection<PlayerId> PlatformBlocks
		{
			get
			{
				return PlatformServices.Instance.BlockedUsers;
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000059CE File Offset: 0x00003BCE
		public static bool IsBlocked(PlayerId playerID)
		{
			return PermaMuteList.IsPlayerMuted(playerID) || (MultiplayerPlayerHelper.PlatformBlocks != null && MultiplayerPlayerHelper.PlatformBlocks.Contains(playerID));
		}
	}
}
