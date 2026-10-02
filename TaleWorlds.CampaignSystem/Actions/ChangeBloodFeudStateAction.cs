using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004BE RID: 1214
	public static class ChangeBloodFeudStateAction
	{
		// Token: 0x06004D0C RID: 19724 RVA: 0x00185C5B File Offset: 0x00183E5B
		private static void ApplyInternal(Clan clan, Hero executedHero, ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail detail)
		{
			clan.HasBloodFeudWithPlayer = detail == ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail.StartedByPlayerExecuteAHero || detail == ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail.StartedByAIExecutePlayerRelative;
			if (detail == ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail.SettledByRansomPayment)
			{
				clan.BloodFeudExecutionsDoneCount = 0;
				clan.BloodFeudExecutionsReceivedCount = 0;
			}
			CampaignEventDispatcher.Instance.OnBloodFeudStateChanged(clan, executedHero, detail);
		}

		// Token: 0x06004D0D RID: 19725 RVA: 0x00185C8D File Offset: 0x00183E8D
		public static void StartBloodFeudWithClanByPlayerExecutingAHero(Clan clan, Hero executedHero)
		{
			ChangeBloodFeudStateAction.ApplyInternal(clan, executedHero, ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail.StartedByPlayerExecuteAHero);
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x00185C97 File Offset: 0x00183E97
		public static void StartBloodFeudWithClanByAIExecutingPlayerRelative(Clan clan, Hero executedHero)
		{
			ChangeBloodFeudStateAction.ApplyInternal(clan, executedHero, ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail.StartedByAIExecutePlayerRelative);
		}

		// Token: 0x06004D0F RID: 19727 RVA: 0x00185CA1 File Offset: 0x00183EA1
		public static void SettleBloodFeudByRelationIncrease(Clan clan)
		{
			ChangeBloodFeudStateAction.ApplyInternal(clan, null, ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail.SettledByRelationIncrease);
		}

		// Token: 0x06004D10 RID: 19728 RVA: 0x00185CAB File Offset: 0x00183EAB
		public static void SettleBloodFeudByRansomPayment(Clan clan)
		{
			ChangeBloodFeudStateAction.ApplyInternal(clan, null, ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail.SettledByRansomPayment);
		}

		// Token: 0x020008D7 RID: 2263
		public enum ChangeBloodFeudActionDetail
		{
			// Token: 0x04002643 RID: 9795
			SettledByRelationIncrease,
			// Token: 0x04002644 RID: 9796
			SettledByRansomPayment,
			// Token: 0x04002645 RID: 9797
			StartedByPlayerExecuteAHero,
			// Token: 0x04002646 RID: 9798
			StartedByAIExecutePlayerRelative
		}
	}
}
