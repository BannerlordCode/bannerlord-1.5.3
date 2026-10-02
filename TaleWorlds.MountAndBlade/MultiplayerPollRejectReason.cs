using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C2 RID: 706
	public enum MultiplayerPollRejectReason
	{
		// Token: 0x04000F83 RID: 3971
		NotEnoughPlayersToOpenPoll,
		// Token: 0x04000F84 RID: 3972
		HasOngoingPoll,
		// Token: 0x04000F85 RID: 3973
		TooManyPollRequests,
		// Token: 0x04000F86 RID: 3974
		KickPollTargetNotSynced,
		// Token: 0x04000F87 RID: 3975
		Count
	}
}
