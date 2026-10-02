using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000326 RID: 806
	public static class NetworkMain
	{
		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002E2C RID: 11820 RVA: 0x000B3160 File Offset: 0x000B1360
		// (set) Token: 0x06002E2D RID: 11821 RVA: 0x000B3167 File Offset: 0x000B1367
		public static LobbyClient GameClient { get; private set; }

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002E2E RID: 11822 RVA: 0x000B316F File Offset: 0x000B136F
		// (set) Token: 0x06002E2F RID: 11823 RVA: 0x000B3176 File Offset: 0x000B1376
		public static CommunityClient CommunityClient { get; private set; }

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002E30 RID: 11824 RVA: 0x000B317E File Offset: 0x000B137E
		// (set) Token: 0x06002E31 RID: 11825 RVA: 0x000B3185 File Offset: 0x000B1385
		public static CustomBattleServer CustomBattleServer { get; private set; }

		// Token: 0x06002E32 RID: 11826 RVA: 0x000B318D File Offset: 0x000B138D
		public static void SetPeers(LobbyClient gameClient, CommunityClient communityClient, CustomBattleServer customBattleServer)
		{
			NetworkMain.GameClient = gameClient;
			NetworkMain.CommunityClient = communityClient;
			NetworkMain.CustomBattleServer = customBattleServer;
		}
	}
}
