using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200015E RID: 350
	[Serializable]
	public class ServerStatus
	{
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x0000F3D1 File Offset: 0x0000D5D1
		// (set) Token: 0x060009D0 RID: 2512 RVA: 0x0000F3D9 File Offset: 0x0000D5D9
		public bool IsMatchmakingEnabled { get; set; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x0000F3E2 File Offset: 0x0000D5E2
		// (set) Token: 0x060009D2 RID: 2514 RVA: 0x0000F3EA File Offset: 0x0000D5EA
		public bool IsCustomBattleEnabled { get; set; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x0000F3F3 File Offset: 0x0000D5F3
		// (set) Token: 0x060009D4 RID: 2516 RVA: 0x0000F3FB File Offset: 0x0000D5FB
		public bool IsPlayerBasedCustomBattleEnabled { get; set; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0000F404 File Offset: 0x0000D604
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x0000F40C File Offset: 0x0000D60C
		public bool IsPremadeGameEnabled { get; set; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x0000F415 File Offset: 0x0000D615
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x0000F41D File Offset: 0x0000D61D
		public bool IsTestRegionEnabled { get; set; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0000F426 File Offset: 0x0000D626
		// (set) Token: 0x060009DA RID: 2522 RVA: 0x0000F42E File Offset: 0x0000D62E
		public Announcement Announcement { get; set; }

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x0000F437 File Offset: 0x0000D637
		public ServerNotification[] ServerNotifications { get; }

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x0000F43F File Offset: 0x0000D63F
		// (set) Token: 0x060009DD RID: 2525 RVA: 0x0000F447 File Offset: 0x0000D647
		public int FriendListUpdatePeriod { get; set; }

		// Token: 0x060009DE RID: 2526 RVA: 0x0000F450 File Offset: 0x0000D650
		public ServerStatus()
		{
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0000F458 File Offset: 0x0000D658
		public ServerStatus(bool isMatchmakingEnabled, bool isCustomBattleEnabled, bool isPlayerBasedCustomBattleEnabled, bool isPremadeGameEnabled, bool isTestRegionEnabled, Announcement announcement, ServerNotification[] serverNotifications, int friendListUpdatePeriod)
		{
			this.IsMatchmakingEnabled = isMatchmakingEnabled;
			this.IsCustomBattleEnabled = isCustomBattleEnabled;
			this.IsPlayerBasedCustomBattleEnabled = isPlayerBasedCustomBattleEnabled;
			this.IsPremadeGameEnabled = isPremadeGameEnabled;
			this.IsTestRegionEnabled = isTestRegionEnabled;
			this.Announcement = announcement;
			this.ServerNotifications = serverNotifications;
			this.FriendListUpdatePeriod = friendListUpdatePeriod;
		}
	}
}
