using System;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace StoryMode.GameComponents
{
	// Token: 0x02000043 RID: 67
	public class StoryModeHeroDeathProbabilityCalculationModel : HeroDeathProbabilityCalculationModel
	{
		// Token: 0x0600044D RID: 1101 RVA: 0x00019422 File Offset: 0x00017622
		public override float CalculateHeroDeathProbability(Hero hero)
		{
			if (hero == StoryModeHeroes.ElderBrother && !StoryModeManager.Current.MainStoryLine.IsCompleted)
			{
				return 0f;
			}
			return base.BaseModel.CalculateHeroDeathProbability(hero);
		}
	}
}
