using System;
using StoryMode.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace StoryMode.GameComponents
{
	// Token: 0x02000042 RID: 66
	public class StoryModeGenericXpModel : GenericXpModel
	{
		// Token: 0x0600044B RID: 1099 RVA: 0x000193EB File Offset: 0x000175EB
		public override float GetXpMultiplier(Hero hero)
		{
			if (((hero != null) ? hero.CurrentSettlement : null) != null && hero.CurrentSettlement.IsTrainingField())
			{
				return 0f;
			}
			return base.BaseModel.GetXpMultiplier(hero);
		}
	}
}
