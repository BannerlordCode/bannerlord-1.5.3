using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019B RID: 411
	public abstract class ShipDistributionModel : MBGameModel<ShipDistributionModel>
	{
		// Token: 0x06001CE4 RID: 7396
		public abstract float GetScoreForPartyShipComposition(MobileParty party, MBReadOnlyList<Ship> shipsToConsider);

		// Token: 0x06001CE5 RID: 7397
		public abstract bool CanSendShipToParty(Ship ship, MobileParty mobileParty);

		// Token: 0x06001CE6 RID: 7398
		public abstract bool CanPartyTakeShip(PartyBase party, Ship ship);
	}
}
