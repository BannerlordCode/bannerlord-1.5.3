using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001EA RID: 490
	public abstract class NotablePowerModel : MBGameModel<NotablePowerModel>
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06001F6C RID: 8044
		public abstract int RegularNotableMaxPowerLevel { get; }

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06001F6D RID: 8045
		public abstract int NotableDisappearPowerLimit { get; }

		// Token: 0x06001F6E RID: 8046
		public abstract ExplainedNumber CalculateDailyPowerChangeForHero(Hero hero, bool includeDescriptions = false);

		// Token: 0x06001F6F RID: 8047
		public abstract TextObject GetPowerRankName(Hero hero);

		// Token: 0x06001F70 RID: 8048
		public abstract float GetInfluenceBonusToClan(Hero hero);

		// Token: 0x06001F71 RID: 8049
		public abstract int GetInitialPower(Hero hero);

		// Token: 0x06001F72 RID: 8050
		public abstract int GetInitialNotableSupporterCost(Hero hero);
	}
}
