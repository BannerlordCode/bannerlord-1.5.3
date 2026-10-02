using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004F0 RID: 1264
	public static class SiegeAftermathAction
	{
		// Token: 0x06004DF2 RID: 19954 RVA: 0x0018A88C File Offset: 0x00188A8C
		private static void ApplyInternal(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			CampaignEventDispatcher.Instance.OnSiegeAftermathApplied(attackerParty, settlement, aftermathType, previousSettlementOwner, partyContributions);
		}

		// Token: 0x06004DF3 RID: 19955 RVA: 0x0018A89E File Offset: 0x00188A9E
		public static void ApplyAftermath(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			SiegeAftermathAction.ApplyInternal(attackerParty, settlement, aftermathType, previousSettlementOwner, partyContributions);
		}

		// Token: 0x020008F1 RID: 2289
		public enum SiegeAftermath
		{
			// Token: 0x040026C4 RID: 9924
			Devastate,
			// Token: 0x040026C5 RID: 9925
			Pillage,
			// Token: 0x040026C6 RID: 9926
			ShowMercy
		}
	}
}
