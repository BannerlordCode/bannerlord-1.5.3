using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000130 RID: 304
	public enum CustomGameJoinResponse
	{
		// Token: 0x04000363 RID: 867
		Success,
		// Token: 0x04000364 RID: 868
		IncorrectPlayerState,
		// Token: 0x04000365 RID: 869
		ServerCapacityIsFull,
		// Token: 0x04000366 RID: 870
		ErrorOnGameServer,
		// Token: 0x04000367 RID: 871
		GameServerAccessError,
		// Token: 0x04000368 RID: 872
		CustomGameServerNotAvailable,
		// Token: 0x04000369 RID: 873
		CustomGameServerFinishing,
		// Token: 0x0400036A RID: 874
		IncorrectPassword,
		// Token: 0x0400036B RID: 875
		PlayerBanned,
		// Token: 0x0400036C RID: 876
		HostReplyTimedOut,
		// Token: 0x0400036D RID: 877
		NoPlayerDataFound,
		// Token: 0x0400036E RID: 878
		UnspecifiedError,
		// Token: 0x0400036F RID: 879
		NoPlayersCanJoin,
		// Token: 0x04000370 RID: 880
		AlreadyRequestedWaitingForServerResponse,
		// Token: 0x04000371 RID: 881
		RequesterIsNotPartyLeader,
		// Token: 0x04000372 RID: 882
		NotAllPlayersReady,
		// Token: 0x04000373 RID: 883
		NotAllPlayersModulesMatchWithServer,
		// Token: 0x04000374 RID: 884
		SpectatorCapacityIsFull,
		// Token: 0x04000375 RID: 885
		SpectatorsNotAllowed
	}
}
