using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000199 RID: 409
	public abstract class ShipCostModel : MBGameModel<ShipCostModel>
	{
		// Token: 0x06001CDD RID: 7389
		public abstract float GetShipTradeValue(Ship ship, PartyBase seller, PartyBase buyer);

		// Token: 0x06001CDE RID: 7390
		public abstract float GetShipRepairCost(Ship ship, PartyBase owner);

		// Token: 0x06001CDF RID: 7391
		public abstract int GetShipUpgradePieceCost(Ship ship, ShipUpgradePiece piece, PartyBase owner);

		// Token: 0x06001CE0 RID: 7392
		public abstract float GetShipSellingPenalty();
	}
}
