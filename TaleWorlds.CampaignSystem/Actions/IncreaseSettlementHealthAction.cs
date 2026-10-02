using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004DF RID: 1247
	public static class IncreaseSettlementHealthAction
	{
		// Token: 0x06004DAF RID: 19887 RVA: 0x00188744 File Offset: 0x00186944
		private static void ApplyInternal(Settlement settlement, float percentage)
		{
			settlement.SettlementHitPoints += percentage;
			settlement.SettlementHitPoints = ((settlement.SettlementHitPoints > 1f) ? 1f : settlement.SettlementHitPoints);
			if (settlement.SettlementHitPoints >= 1f && settlement.IsVillage && settlement.Village.VillageState != Village.VillageStates.Normal)
			{
				ChangeVillageStateAction.ApplyBySettingToNormal(settlement);
				settlement.Militia += 20f;
			}
		}

		// Token: 0x06004DB0 RID: 19888 RVA: 0x001887B9 File Offset: 0x001869B9
		public static void Apply(Settlement settlement, float percentage)
		{
			IncreaseSettlementHealthAction.ApplyInternal(settlement, percentage);
		}
	}
}
