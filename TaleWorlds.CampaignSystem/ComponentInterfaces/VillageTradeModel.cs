using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000204 RID: 516
	public abstract class VillageTradeModel : MBGameModel<VillageTradeModel>
	{
		// Token: 0x06002029 RID: 8233
		public abstract float TradeBoundDistanceLimitAsDays(MobileParty.NavigationType navigationType);

		// Token: 0x0600202A RID: 8234
		public abstract Settlement GetTradeBoundToAssignForVillage(Village village);
	}
}
