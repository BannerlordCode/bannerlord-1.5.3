using System;
using Galaxy.Api;

namespace TaleWorlds.PlatformService.GOG
{
	// Token: 0x0200000E RID: 14
	public class StatsAndAchievementsStoreListener : GlobalStatsAndAchievementsStoreListener
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000086 RID: 134 RVA: 0x00003338 File Offset: 0x00001538
		// (remove) Token: 0x06000087 RID: 135 RVA: 0x00003370 File Offset: 0x00001570
		public event StatsAndAchievementsStoreListener.UserStatsAndAchievementsStored OnUserStatsAndAchievementsStored;

		// Token: 0x06000088 RID: 136 RVA: 0x000033A5 File Offset: 0x000015A5
		public override void OnUserStatsAndAchievementsStoreFailure(IStatsAndAchievementsStoreListener.FailureReason failureReason)
		{
			StatsAndAchievementsStoreListener.UserStatsAndAchievementsStored onUserStatsAndAchievementsStored = this.OnUserStatsAndAchievementsStored;
			if (onUserStatsAndAchievementsStored == null)
			{
				return;
			}
			onUserStatsAndAchievementsStored(false, new IStatsAndAchievementsStoreListener.FailureReason?(failureReason));
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000033C0 File Offset: 0x000015C0
		public override void OnUserStatsAndAchievementsStoreSuccess()
		{
			StatsAndAchievementsStoreListener.UserStatsAndAchievementsStored onUserStatsAndAchievementsStored = this.OnUserStatsAndAchievementsStored;
			if (onUserStatsAndAchievementsStored == null)
			{
				return;
			}
			onUserStatsAndAchievementsStored(true, null);
		}

		// Token: 0x02000019 RID: 25
		// (Invoke) Token: 0x060000A8 RID: 168
		public delegate void UserStatsAndAchievementsStored(bool success, IStatsAndAchievementsStoreListener.FailureReason? failureReason);
	}
}
