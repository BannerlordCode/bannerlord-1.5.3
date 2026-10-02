using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000040 RID: 64
	[Tutorial("OrderOfBattleTutorialStep1")]
	public class OrderOfBattleTutorialStep1Tutorial : TutorialItemBase
	{
		// Token: 0x0600012B RID: 299 RVA: 0x000044BC File Offset: 0x000026BC
		public OrderOfBattleTutorialStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Center;
			base.HighlightedVisualElementID = "AssignCaptain";
			base.MouseRequired = false;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000044DD File Offset: 0x000026DD
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000044E0 File Offset: 0x000026E0
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.IsOrderOfBattleOpenAndReady && TutorialHelper.IsPlayerEncounterLeader && !TutorialHelper.IsNavalMission;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000044FA File Offset: 0x000026FA
		public override void OnOrderOfBattleHeroAssignedToFormation(OrderOfBattleHeroAssignedToFormationEvent obj)
		{
			this._playerAssignedACaptainToFormationInOoB = true;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00004503 File Offset: 0x00002703
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerAssignedACaptainToFormationInOoB;
		}

		// Token: 0x04000050 RID: 80
		private bool _playerAssignedACaptainToFormationInOoB;
	}
}
