using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E0 RID: 480
	public abstract class HeroDeathProbabilityCalculationModel : MBGameModel<HeroDeathProbabilityCalculationModel>
	{
		// Token: 0x06001F35 RID: 7989
		public abstract float CalculateHeroDeathProbability(Hero hero);
	}
}
