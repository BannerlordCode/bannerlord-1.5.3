using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000463 RID: 1123
	public class SettlementVariablesBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004897 RID: 18583 RVA: 0x00169A5C File Offset: 0x00167C5C
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x00169A75 File Offset: 0x00167C75
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x00169A78 File Offset: 0x00167C78
		private void HourlyTickSettlement(Settlement settlement)
		{
			if (settlement.LastAttackerParty != null && settlement.Party.MapEvent == null && settlement.Party.SiegeEvent == null && settlement.LastThreatTime.ElapsedDaysUntilNow > this._resetLastAttackerPartyAsDays)
			{
				settlement.LastAttackerParty = null;
			}
		}

		// Token: 0x04001483 RID: 5251
		private float _resetLastAttackerPartyAsDays = 1f;
	}
}
