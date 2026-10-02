using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000115 RID: 277
	public enum DisconnectType
	{
		// Token: 0x0400026A RID: 618
		QuitFromGame,
		// Token: 0x0400026B RID: 619
		TimedOut,
		// Token: 0x0400026C RID: 620
		KickedByHost,
		// Token: 0x0400026D RID: 621
		KickedByPoll,
		// Token: 0x0400026E RID: 622
		BannedByPoll,
		// Token: 0x0400026F RID: 623
		Inactivity,
		// Token: 0x04000270 RID: 624
		DisconnectedFromLobby,
		// Token: 0x04000271 RID: 625
		GameEnded,
		// Token: 0x04000272 RID: 626
		ServerNotResponding,
		// Token: 0x04000273 RID: 627
		KickedDueToFriendlyDamage,
		// Token: 0x04000274 RID: 628
		PlayStateMismatch,
		// Token: 0x04000275 RID: 629
		Unknown
	}
}
