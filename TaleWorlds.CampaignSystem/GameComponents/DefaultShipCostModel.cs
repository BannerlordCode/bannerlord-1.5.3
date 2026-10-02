using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200015B RID: 347
	public class DefaultShipCostModel : ShipCostModel
	{
		// Token: 0x06001B03 RID: 6915 RVA: 0x0008960B File Offset: 0x0008780B
		public override float GetShipTradeValue(Ship ship, PartyBase seller, PartyBase buyer)
		{
			return 0f;
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x00089612 File Offset: 0x00087812
		public override float GetShipRepairCost(Ship ship, PartyBase owner)
		{
			return 0f;
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x00089619 File Offset: 0x00087819
		public override int GetShipUpgradePieceCost(Ship ship, ShipUpgradePiece piece, PartyBase owner)
		{
			return 0;
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x0008961C File Offset: 0x0008781C
		public override float GetShipSellingPenalty()
		{
			return 0f;
		}
	}
}
