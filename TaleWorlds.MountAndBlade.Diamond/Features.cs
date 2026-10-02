using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000118 RID: 280
	[Flags]
	public enum Features
	{
		// Token: 0x04000279 RID: 633
		None = 0,
		// Token: 0x0400027A RID: 634
		Matchmaking = 1,
		// Token: 0x0400027B RID: 635
		CustomGame = 2,
		// Token: 0x0400027C RID: 636
		Party = 4,
		// Token: 0x0400027D RID: 637
		Clan = 8,
		// Token: 0x0400027E RID: 638
		BannerlordFriendList = 16,
		// Token: 0x0400027F RID: 639
		TextChat = 32,
		// Token: 0x04000280 RID: 640
		All = -1
	}
}
