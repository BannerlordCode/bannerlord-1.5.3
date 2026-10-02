using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000124 RID: 292
	public class DefaultHeroDeathProbabilityCalculationModel : HeroDeathProbabilityCalculationModel
	{
		// Token: 0x060018BF RID: 6335 RVA: 0x00078814 File Offset: 0x00076A14
		public override float CalculateHeroDeathProbability(Hero hero)
		{
			return this.CalculateHeroDeathProbabilityInternal(hero);
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x00078820 File Offset: 0x00076A20
		private float CalculateHeroDeathProbabilityInternal(Hero hero)
		{
			float num = 0f;
			if (!CampaignOptions.IsLifeDeathCycleDisabled)
			{
				int becomeOldAge = Campaign.Current.Models.AgeModel.BecomeOldAge;
				int num2 = Campaign.Current.Models.AgeModel.MaxAge - 1;
				if (hero.Age > (float)becomeOldAge)
				{
					if (hero.Age < (float)num2)
					{
						float num3 = 0.3f * ((hero.Age - (float)becomeOldAge) / (float)(num2 - becomeOldAge));
						float num4 = 1f - MathF.Pow(1f - num3, 1f / (float)CampaignTime.DaysInYear);
						num += num4;
					}
					else if (hero.Age >= (float)num2)
					{
						num += 1f;
					}
				}
			}
			return num;
		}
	}
}
