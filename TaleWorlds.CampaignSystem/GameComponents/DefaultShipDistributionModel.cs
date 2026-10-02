using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200015C RID: 348
	public class DefaultShipDistributionModel : ShipDistributionModel
	{
		// Token: 0x06001B08 RID: 6920 RVA: 0x0008962B File Offset: 0x0008782B
		public override bool CanPartyTakeShip(PartyBase party, Ship ship)
		{
			return false;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0008962E File Offset: 0x0008782E
		public override bool CanSendShipToParty(Ship ship, MobileParty mobileParty)
		{
			return false;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x00089631 File Offset: 0x00087831
		public override float GetScoreForPartyShipComposition(MobileParty party, MBReadOnlyList<Ship> shipsToConsider)
		{
			return 0f;
		}
	}
}
