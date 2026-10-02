using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E9 RID: 489
	public abstract class PregnancyModel : MBGameModel<PregnancyModel>
	{
		// Token: 0x06001F65 RID: 8037
		public abstract float GetDailyChanceOfPregnancyForHero(Hero hero);

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06001F66 RID: 8038
		public abstract float PregnancyDurationInDays { get; }

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06001F67 RID: 8039
		public abstract float MaternalMortalityProbabilityInLabor { get; }

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06001F68 RID: 8040
		public abstract float StillbirthProbability { get; }

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06001F69 RID: 8041
		public abstract float DeliveringFemaleOffspringProbability { get; }

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06001F6A RID: 8042
		public abstract float DeliveringTwinsProbability { get; }
	}
}
