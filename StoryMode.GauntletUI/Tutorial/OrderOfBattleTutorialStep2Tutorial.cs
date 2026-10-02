using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000041 RID: 65
	[Tutorial("OrderOfBattleTutorialStep2")]
	public class OrderOfBattleTutorialStep2Tutorial : TutorialItemBase
	{
		// Token: 0x06000130 RID: 304 RVA: 0x0000450B File Offset: 0x0000270B
		public OrderOfBattleTutorialStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Top;
			base.HighlightedVisualElementID = "CreateFormation";
			base.MouseRequired = false;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000452C File Offset: 0x0000272C
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000452F File Offset: 0x0000272F
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.IsOrderOfBattleOpenAndReady && TutorialHelper.IsPlayerEncounterLeader && !TutorialHelper.IsNavalMission;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00004549 File Offset: 0x00002749
		public override void OnOrderOfBattleFormationClassChanged(OrderOfBattleFormationClassChangedEvent obj)
		{
			this._playerChangedAFormationType = true;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00004552 File Offset: 0x00002752
		public override void OnOrderOfBattleFormationWeightChanged(OrderOfBattleFormationWeightChangedEvent obj)
		{
			this._playerChangedAFormationWeight = this._playerChangedAFormationType;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00004560 File Offset: 0x00002760
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerChangedAFormationType && this._playerChangedAFormationWeight;
		}

		// Token: 0x04000051 RID: 81
		private bool _playerChangedAFormationType;

		// Token: 0x04000052 RID: 82
		private bool _playerChangedAFormationWeight;
	}
}
