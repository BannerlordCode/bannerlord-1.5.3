using System;
using Galaxy.Api;

namespace TaleWorlds.PlatformService.GOG
{
	// Token: 0x0200000D RID: 13
	public class UserStatsAndAchievementsRetrieveListener : GlobalUserStatsAndAchievementsRetrieveListener
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000081 RID: 129 RVA: 0x0000327C File Offset: 0x0000147C
		// (remove) Token: 0x06000082 RID: 130 RVA: 0x000032B4 File Offset: 0x000014B4
		public event UserStatsAndAchievementsRetrieveListener.UserStatsAndAchievementsRetrieved OnUserStatsAndAchievementsRetrieved;

		// Token: 0x06000083 RID: 131 RVA: 0x000032EC File Offset: 0x000014EC
		public override void OnUserStatsAndAchievementsRetrieveSuccess(GalaxyID userID)
		{
			UserStatsAndAchievementsRetrieveListener.UserStatsAndAchievementsRetrieved onUserStatsAndAchievementsRetrieved = this.OnUserStatsAndAchievementsRetrieved;
			if (onUserStatsAndAchievementsRetrieved == null)
			{
				return;
			}
			onUserStatsAndAchievementsRetrieved(userID, true, null);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003314 File Offset: 0x00001514
		public override void OnUserStatsAndAchievementsRetrieveFailure(GalaxyID userID, IUserStatsAndAchievementsRetrieveListener.FailureReason failureReason)
		{
			UserStatsAndAchievementsRetrieveListener.UserStatsAndAchievementsRetrieved onUserStatsAndAchievementsRetrieved = this.OnUserStatsAndAchievementsRetrieved;
			if (onUserStatsAndAchievementsRetrieved == null)
			{
				return;
			}
			onUserStatsAndAchievementsRetrieved(userID, false, new IUserStatsAndAchievementsRetrieveListener.FailureReason?(failureReason));
		}

		// Token: 0x02000018 RID: 24
		// (Invoke) Token: 0x060000A4 RID: 164
		public delegate void UserStatsAndAchievementsRetrieved(GalaxyID userID, bool success, IUserStatsAndAchievementsRetrieveListener.FailureReason? failureReason);
	}
}
