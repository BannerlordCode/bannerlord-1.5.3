using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D3 RID: 467
	public abstract class SettlementValueModel : MBGameModel<SettlementValueModel>
	{
		// Token: 0x06001EC0 RID: 7872
		public abstract Settlement FindMostSuitableHomeSettlement(Clan clan);

		// Token: 0x06001EC1 RID: 7873
		public abstract float CalculateSettlementValueForFaction(Settlement settlement, IFaction faction);

		// Token: 0x06001EC2 RID: 7874
		public abstract float CalculateSettlementBaseValue(Settlement settlement);

		// Token: 0x06001EC3 RID: 7875
		public abstract float CalculateSettlementValueForEnemyHero(Settlement settlement, Hero hero);
	}
}
