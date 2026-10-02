using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000014 RID: 20
	[Tutorial("RecruitmentTutorialStep1")]
	public class RecruitmentStep1Tutorial : TutorialItemBase
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00002B2B File Offset: 0x00000D2B
		public RecruitmentStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "storymode_tutorial_village_recruit";
			base.MouseRequired = true;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002B4C File Offset: 0x00000D4C
		public override bool IsConditionsMetForCompletion()
		{
			return this._recruitmentOpened;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002B54 File Offset: 0x00000D54
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._recruitmentOpened = obj.NewContext == TutorialContexts.RecruitmentWindow;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002B65 File Offset: 0x00000D65
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002B68 File Offset: 0x00000D68
		public override bool IsConditionsMetForActivation()
		{
			return !TutorialHelper.IsCharacterPopUpWindowOpen && TutorialHelper.CurrentContext == TutorialContexts.MapWindow && TutorialHelper.PlayerCanRecruit && !Settlement.CurrentSettlement.MapFaction.IsAtWarWith(MobileParty.MainParty.MapFaction);
		}

		// Token: 0x04000017 RID: 23
		private bool _recruitmentOpened;
	}
}
