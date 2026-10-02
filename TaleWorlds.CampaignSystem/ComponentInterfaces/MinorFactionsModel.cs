using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AF RID: 431
	public abstract class MinorFactionsModel : MBGameModel<MinorFactionsModel>
	{
		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06001D75 RID: 7541
		public abstract float DailyMinorFactionHeroSpawnChance { get; }

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001D76 RID: 7542
		public abstract int MinorFactionHeroLimit { get; }

		// Token: 0x06001D77 RID: 7543
		public abstract int GetMercenaryAwardFactorToJoinKingdom(Clan mercenaryClan, Kingdom kingdom, bool neededAmountForClanToJoinCalculation = false);
	}
}
