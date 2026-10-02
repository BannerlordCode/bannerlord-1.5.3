using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200041F RID: 1055
	public interface IEmptyClanPartiesCampaignBehavior
	{
		// Token: 0x0600433C RID: 17212
		void TransferCachedLordPartyToNewPartyForPlayerClan(Hero cachedPartyLeader, PartyBase newParty);

		// Token: 0x0600433D RID: 17213
		void DisbandCachedLordPartyForPlayerClan(Hero hero);

		// Token: 0x0600433E RID: 17214
		int GetShipCountForCachedLordPartyForPlayerClan(Hero hero);

		// Token: 0x0600433F RID: 17215
		MBReadOnlyList<Ship> GetShipsForCachedLordPartyForPlayerClan(Hero hero);

		// Token: 0x06004340 RID: 17216
		MBReadOnlyList<Hero> GetEmptyClanPartyLeaders();
	}
}
