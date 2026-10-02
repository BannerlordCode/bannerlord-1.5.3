using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F0 RID: 752
	public static class CustomGameBannedPlayerManager
	{
		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002B74 RID: 11124 RVA: 0x000A7DAD File Offset: 0x000A5FAD
		private static Dictionary<PlayerId, CustomGameBannedPlayerManager.BannedPlayer> _bannedPlayers
		{
			get
			{
				if (CustomGameBannedPlayerManager._bannedPlayersInternal == null)
				{
					CustomGameBannedPlayerManager._bannedPlayersInternal = new Dictionary<PlayerId, CustomGameBannedPlayerManager.BannedPlayer>();
				}
				return CustomGameBannedPlayerManager._bannedPlayersInternal;
			}
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x000A7DC8 File Offset: 0x000A5FC8
		public static void AddBannedPlayer(PlayerId playerId, int banDueTime)
		{
			CustomGameBannedPlayerManager._bannedPlayers[playerId] = new CustomGameBannedPlayerManager.BannedPlayer
			{
				PlayerId = playerId,
				BanDueTime = banDueTime
			};
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x000A7DFC File Offset: 0x000A5FFC
		public static bool IsUserBanned(PlayerId playerId)
		{
			return CustomGameBannedPlayerManager._bannedPlayers.ContainsKey(playerId) && CustomGameBannedPlayerManager._bannedPlayers[playerId].BanDueTime > Environment.TickCount;
		}

		// Token: 0x040010D1 RID: 4305
		private static Dictionary<PlayerId, CustomGameBannedPlayerManager.BannedPlayer> _bannedPlayersInternal;

		// Token: 0x020005D6 RID: 1494
		private struct BannedPlayer
		{
			// Token: 0x17000AA8 RID: 2728
			// (get) Token: 0x06003F57 RID: 16215 RVA: 0x000F996C File Offset: 0x000F7B6C
			// (set) Token: 0x06003F58 RID: 16216 RVA: 0x000F9974 File Offset: 0x000F7B74
			public PlayerId PlayerId { get; set; }

			// Token: 0x17000AA9 RID: 2729
			// (get) Token: 0x06003F59 RID: 16217 RVA: 0x000F997D File Offset: 0x000F7B7D
			// (set) Token: 0x06003F5A RID: 16218 RVA: 0x000F9985 File Offset: 0x000F7B85
			public int BanDueTime { get; set; }
		}
	}
}
