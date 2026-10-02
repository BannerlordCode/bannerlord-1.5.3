using System;
using SandBox.GauntletUI.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000039 RID: 57
	[Tutorial("EncyclopediaClansTutorial")]
	public class EncyclopediaClansTutorial : EncyclopediaPageTutorialBase
	{
		// Token: 0x0600010F RID: 271 RVA: 0x00003F68 File Offset: 0x00002168
		public EncyclopediaClansTutorial()
			: base(EncyclopediaPages.Clan, EncyclopediaPages.ListClans)
		{
		}
	}
}
