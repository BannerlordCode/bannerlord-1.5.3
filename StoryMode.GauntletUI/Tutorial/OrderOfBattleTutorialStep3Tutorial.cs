using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000042 RID: 66
	[Tutorial("OrderOfBattleTutorialStep3")]
	public class OrderOfBattleTutorialStep3Tutorial : TutorialItemBase
	{
		// Token: 0x06000136 RID: 310 RVA: 0x00004572 File Offset: 0x00002772
		public OrderOfBattleTutorialStep3Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Center;
			base.HighlightedVisualElementID = "AssignCaptain";
			base.MouseRequired = false;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00004593 File Offset: 0x00002793
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00004596 File Offset: 0x00002796
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.IsOrderOfBattleOpenAndReady && !TutorialHelper.IsPlayerEncounterLeader && TutorialHelper.CanPlayerAssignHimselfToFormation && !TutorialHelper.IsNavalMission;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000045B7 File Offset: 0x000027B7
		public override void OnOrderOfBattleHeroAssignedToFormation(OrderOfBattleHeroAssignedToFormationEvent obj)
		{
			if (!TutorialHelper.IsPlayerEncounterLeader)
			{
				this._playerAssignedACaptainToFormationInOoB = obj.AssignedHero == Agent.Main;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000045D3 File Offset: 0x000027D3
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerAssignedACaptainToFormationInOoB;
		}

		// Token: 0x04000053 RID: 83
		private bool _playerAssignedACaptainToFormationInOoB;
	}
}
