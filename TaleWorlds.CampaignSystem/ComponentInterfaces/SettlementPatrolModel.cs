using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000190 RID: 400
	public abstract class SettlementPatrolModel : MBGameModel<SettlementPatrolModel>
	{
		// Token: 0x06001C8B RID: 7307
		public abstract CampaignTime GetPatrolPartySpawnDuration(Settlement settlement, bool naval);

		// Token: 0x06001C8C RID: 7308
		public abstract bool CanSettlementHavePatrolParties(Settlement settlement, bool naval);

		// Token: 0x06001C8D RID: 7309
		public abstract PartyTemplateObject GetPartyTemplateForPatrolParty(Settlement settlement, bool naval);
	}
}
