using System;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004DC RID: 1244
	public static class GatherArmyAction
	{
		// Token: 0x06004DA1 RID: 19873 RVA: 0x00188380 File Offset: 0x00186580
		private static void ApplyInternal(MobileParty leaderParty, IMapPoint gatheringPoint, float playerInvolvement = 0f)
		{
			Army army = leaderParty.Army;
			CampaignEventDispatcher.Instance.OnArmyGathered(army, gatheringPoint);
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x001883A0 File Offset: 0x001865A0
		public static void Apply(MobileParty leaderParty, IMapPoint gatheringPoint)
		{
			GatherArmyAction.ApplyInternal(leaderParty, gatheringPoint, (leaderParty == MobileParty.MainParty) ? 1f : 0f);
		}
	}
}
