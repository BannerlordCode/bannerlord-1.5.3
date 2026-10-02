using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;

namespace StoryMode
{
	// Token: 0x0200000A RID: 10
	public static class StoryModeHelpers
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002AD4 File Offset: 0x00000CD4
		public static void SetPlayerSiblingsSkillsIfNeeded(Hero hero)
		{
			bool flag = false;
			foreach (SkillObject skillObject in Skills.All)
			{
				if (hero.GetSkillValue(skillObject) == 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				List<ValueTuple<SkillObject, int>> defaultSkillsForHero = Campaign.Current.Models.HeroCreationModel.GetDefaultSkillsForHero(hero);
				hero.ClearSkills();
				foreach (ValueTuple<SkillObject, int> valueTuple in defaultSkillsForHero)
				{
					hero.HeroDeveloper.SetInitialSkillLevel(valueTuple.Item1, valueTuple.Item2);
				}
				hero.HeroDeveloper.InitializeHeroDeveloper(CampaignOptions.AutoAllocateClanMemberPerks);
			}
		}
	}
}
