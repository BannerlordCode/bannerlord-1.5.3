using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200013A RID: 314
	public enum MBLoginErrorCode
	{
		// Token: 0x04000395 RID: 917
		None,
		// Token: 0x04000396 RID: 918
		CouldNotLogin,
		// Token: 0x04000397 RID: 919
		VersionMismatch,
		// Token: 0x04000398 RID: 920
		IncorrectPassword,
		// Token: 0x04000399 RID: 921
		FamilyShareNotAllowed,
		// Token: 0x0400039A RID: 922
		BannedFromGame,
		// Token: 0x0400039B RID: 923
		NoAuthenticationToken,
		// Token: 0x0400039C RID: 924
		AuthTokenExpired,
		// Token: 0x0400039D RID: 925
		BannedFromHostingServers,
		// Token: 0x0400039E RID: 926
		CustomBattleServerIncompatibleVersion,
		// Token: 0x0400039F RID: 927
		ReachedMaxNumberofCustomBattleServers,
		// Token: 0x040003A0 RID: 928
		CouldNotDestroyOldSession,
		// Token: 0x040003A1 RID: 929
		LoggingInDisabled
	}
}
