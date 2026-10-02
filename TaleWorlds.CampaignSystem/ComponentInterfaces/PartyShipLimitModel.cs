using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C6 RID: 454
	public abstract class PartyShipLimitModel : MBGameModel<PartyShipLimitModel>
	{
		// Token: 0x06001E75 RID: 7797
		public abstract int GetIdealShipNumber(MobileParty mobileParty);

		// Token: 0x06001E76 RID: 7798
		public abstract int GetIdealShipNumber(Clan clan);

		// Token: 0x06001E77 RID: 7799
		public abstract float GetShipPriority(MobileParty mobileParty, Ship ship, bool isSelling);
	}
}
