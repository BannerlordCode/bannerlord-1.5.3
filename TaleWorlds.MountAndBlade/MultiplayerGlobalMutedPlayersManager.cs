using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000324 RID: 804
	public static class MultiplayerGlobalMutedPlayersManager
	{
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002DEC RID: 11756 RVA: 0x000B2AA4 File Offset: 0x000B0CA4
		private static List<PlayerId> _mutedPlayers
		{
			get
			{
				if (MultiplayerGlobalMutedPlayersManager._mutedPlayersInternal == null)
				{
					MultiplayerGlobalMutedPlayersManager._mutedPlayersInternal = new List<PlayerId>();
				}
				return MultiplayerGlobalMutedPlayersManager._mutedPlayersInternal;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002DED RID: 11757 RVA: 0x000B2ABC File Offset: 0x000B0CBC
		public static List<PlayerId> MutedPlayers
		{
			get
			{
				return MultiplayerGlobalMutedPlayersManager._mutedPlayers;
			}
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000B2AC3 File Offset: 0x000B0CC3
		public static void MutePlayer(PlayerId playerId)
		{
			MultiplayerGlobalMutedPlayersManager._mutedPlayers.Add(playerId);
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000B2AD0 File Offset: 0x000B0CD0
		public static void UnmutePlayer(PlayerId playerId)
		{
			MultiplayerGlobalMutedPlayersManager._mutedPlayers.Remove(playerId);
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000B2ADE File Offset: 0x000B0CDE
		public static bool IsUserMuted(PlayerId playerId)
		{
			return MultiplayerGlobalMutedPlayersManager._mutedPlayers.Contains(playerId);
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000B2AEB File Offset: 0x000B0CEB
		public static void ClearMutedPlayers()
		{
			MultiplayerGlobalMutedPlayersManager._mutedPlayers.Clear();
		}

		// Token: 0x04001220 RID: 4640
		private static List<PlayerId> _mutedPlayersInternal;
	}
}
