using System;
using SandBox.GauntletUI.Tutorial;
using TaleWorlds.CampaignSystem;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200004B RID: 75
	[Tutorial("ContinuingBloodFeudTutorial")]
	public class ContinuingBloodFeudTutorial : BloodFeudExecutionTutorialBase
	{
		// Token: 0x06000166 RID: 358 RVA: 0x00004914 File Offset: 0x00002B14
		protected override bool IsRelevantToBloodFeudState(Hero victimHero)
		{
			return victimHero.Clan != null && victimHero.Clan.HasBloodFeudWithPlayer;
		}
	}
}
