using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E4 RID: 484
	public abstract class DailyTroopXpBonusModel : MBGameModel<DailyTroopXpBonusModel>
	{
		// Token: 0x06001F44 RID: 8004
		public abstract int CalculateDailyTroopXpBonus(Town town);

		// Token: 0x06001F45 RID: 8005
		public abstract float CalculateGarrisonXpBonusMultiplier(Town town);
	}
}
