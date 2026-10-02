using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000080 RID: 128
	public class CampaignTutorial
	{
		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x00051684 File Offset: 0x0004F884
		public TextObject Description
		{
			get
			{
				return GameTexts.FindText("str_campaign_tutorial_description", this.TutorialTypeId);
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x00051696 File Offset: 0x0004F896
		public TextObject Title
		{
			get
			{
				return GameTexts.FindText("str_campaign_tutorial_title", this.TutorialTypeId);
			}
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x000516A8 File Offset: 0x0004F8A8
		public CampaignTutorial(string tutorialType, int priority)
		{
			this.TutorialTypeId = tutorialType;
			this.Priority = priority;
		}

		// Token: 0x040004BB RID: 1211
		public readonly string TutorialTypeId;

		// Token: 0x040004BC RID: 1212
		public readonly int Priority;
	}
}
