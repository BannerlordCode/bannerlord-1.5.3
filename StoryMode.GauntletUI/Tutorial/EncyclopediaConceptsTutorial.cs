using System;
using SandBox.GauntletUI.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200003A RID: 58
	[Tutorial("EncyclopediaConceptsTutorial")]
	public class EncyclopediaConceptsTutorial : EncyclopediaPageTutorialBase
	{
		// Token: 0x06000110 RID: 272 RVA: 0x00003F73 File Offset: 0x00002173
		public EncyclopediaConceptsTutorial()
			: base(EncyclopediaPages.Concept, EncyclopediaPages.ListConcepts)
		{
		}
	}
}
