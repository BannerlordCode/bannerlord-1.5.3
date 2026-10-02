using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004E3 RID: 1251
	public static class LiftSiegeAction
	{
		// Token: 0x06004DC3 RID: 19907 RVA: 0x001893DF File Offset: 0x001875DF
		private static void ApplyInternal(MobileParty side1Party, Settlement settlement)
		{
			settlement.SiegeEvent.BesiegerCamp.RemoveAllSiegeParties();
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x001893F1 File Offset: 0x001875F1
		public static void GetGameAction(MobileParty side1Party)
		{
			LiftSiegeAction.ApplyInternal(side1Party, side1Party.BesiegedSettlement);
		}
	}
}
