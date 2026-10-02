using System;
using SandBox.GauntletUI.Tutorial;
using TaleWorlds.CampaignSystem;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200004A RID: 74
	[Tutorial("StartingBloodFeudTutorial")]
	public class StartingBloodFeudTutorial : BloodFeudExecutionTutorialBase
	{
		// Token: 0x06000164 RID: 356 RVA: 0x000048F2 File Offset: 0x00002AF2
		protected override bool IsRelevantToBloodFeudState(Hero victimHero)
		{
			return victimHero.Clan != null && !victimHero.Clan.HasBloodFeudWithPlayer;
		}
	}
}
