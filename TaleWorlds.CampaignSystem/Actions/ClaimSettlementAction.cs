using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004CD RID: 1229
	public static class ClaimSettlementAction
	{
		// Token: 0x06004D56 RID: 19798 RVA: 0x0018726D File Offset: 0x0018546D
		private static void ApplyInternal(Hero claimant, Settlement claimedSettlement)
		{
			ClaimSettlementAction.ImpactRelations(claimant, claimedSettlement);
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x00187278 File Offset: 0x00185478
		private static void ImpactRelations(Hero claimant, Settlement claimedSettlement)
		{
			if (claimedSettlement.OwnerClan.Leader != null)
			{
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(claimant, claimedSettlement.OwnerClan.Leader, -50, false);
				if (!claimedSettlement.OwnerClan.IsMapFaction)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(claimant, claimedSettlement.OwnerClan.Leader, -20, false);
				}
			}
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x001872C7 File Offset: 0x001854C7
		public static void Apply(Hero claimant, Settlement claimedSettlement)
		{
			ClaimSettlementAction.ApplyInternal(claimant, claimedSettlement);
		}
	}
}
