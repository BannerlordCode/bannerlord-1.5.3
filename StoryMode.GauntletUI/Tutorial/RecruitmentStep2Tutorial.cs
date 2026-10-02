using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000015 RID: 21
	[Tutorial("RecruitmentTutorialStep2")]
	public class RecruitmentStep2Tutorial : TutorialItemBase
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00002B9E File Offset: 0x00000D9E
		public RecruitmentStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "AvailableTroops";
			base.MouseRequired = true;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002BBF File Offset: 0x00000DBF
		public override bool IsConditionsMetForCompletion()
		{
			return this._recruitedTroopCount >= 4;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002BCD File Offset: 0x00000DCD
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.RecruitmentWindow;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002BD0 File Offset: 0x00000DD0
		public override void OnPlayerRecruitedUnit(CharacterObject obj, int count)
		{
			this._recruitedTroopCount += count;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002BE0 File Offset: 0x00000DE0
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.PlayerCanRecruit && TutorialHelper.CurrentContext == TutorialContexts.RecruitmentWindow;
		}

		// Token: 0x04000018 RID: 24
		private int _recruitedTroopCount;
	}
}
