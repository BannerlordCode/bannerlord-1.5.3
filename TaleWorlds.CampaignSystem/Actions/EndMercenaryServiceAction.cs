using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004D8 RID: 1240
	public static class EndMercenaryServiceAction
	{
		// Token: 0x06004D8A RID: 19850 RVA: 0x00187E1A File Offset: 0x0018601A
		private static void Apply(Clan clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails details)
		{
			clan.EndMercenaryService(details == EndMercenaryServiceAction.EndMercenaryServiceActionDetails.ApplyByLeavingKingdom);
			CampaignEventDispatcher.Instance.OnMercenaryServiceEnded(clan, details);
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x00187E32 File Offset: 0x00186032
		public static void EndByDefault(Clan clan)
		{
			EndMercenaryServiceAction.Apply(clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails.ApplyByDefault);
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x00187E3B File Offset: 0x0018603B
		public static void EndByLeavingKingdom(Clan clan)
		{
			EndMercenaryServiceAction.Apply(clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails.ApplyByLeavingKingdom);
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x00187E44 File Offset: 0x00186044
		public static void EndByBecomingVassal(Clan clan)
		{
			EndMercenaryServiceAction.Apply(clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails.ApplyByBecomingVassal);
		}

		// Token: 0x020008E4 RID: 2276
		public enum EndMercenaryServiceActionDetails
		{
			// Token: 0x04002684 RID: 9860
			ApplyByDefault,
			// Token: 0x04002685 RID: 9861
			ApplyByLeavingKingdom,
			// Token: 0x04002686 RID: 9862
			ApplyByBecomingVassal
		}
	}
}
