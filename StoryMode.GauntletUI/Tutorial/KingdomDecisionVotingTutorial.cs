using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000048 RID: 72
	[Tutorial("KingdomDecisionVotingTutorial")]
	public class KingdomDecisionVotingTutorial : TutorialItemBase
	{
		// Token: 0x06000159 RID: 345 RVA: 0x0000482E File Offset: 0x00002A2E
		public KingdomDecisionVotingTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Left;
			base.HighlightedVisualElementID = "DecisionOptions";
			base.MouseRequired = false;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000484F File Offset: 0x00002A4F
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.KingdomScreen;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00004852 File Offset: 0x00002A52
		public override void OnPlayerSelectedAKingdomDecisionOption(PlayerSelectedAKingdomDecisionOptionEvent obj)
		{
			this._playerSelectedAnOption = true;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000485B File Offset: 0x00002A5B
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.IsKingdomDecisionPanelActiveAndHasOptions;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00004862 File Offset: 0x00002A62
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerSelectedAnOption;
		}

		// Token: 0x0400005E RID: 94
		private bool _playerSelectedAnOption;
	}
}
