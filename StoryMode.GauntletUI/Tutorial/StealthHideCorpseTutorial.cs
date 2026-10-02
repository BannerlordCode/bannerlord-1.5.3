using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using Storymode.Missions;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200002E RID: 46
	[Tutorial("StealthHideCorpseTutorial")]
	public class StealthHideCorpseTutorial : TutorialItemBase
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x0000391A File Offset: 0x00001B1A
		public StealthHideCorpseTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00003929 File Offset: 0x00001B29
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000392C File Offset: 0x00001B2C
		public override bool IsConditionsMetForActivation()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForActivation(SneakIntoTheVillaMissionController.MissionState.HideCorpse);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00003935 File Offset: 0x00001B35
		public override bool IsConditionsMetForCompletion()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForCompletion(SneakIntoTheVillaMissionController.MissionState.HideCorpse) || (SneakIntoTheVillaMissionController.Instance != null && SneakIntoTheVillaMissionController.Instance.IsMainAgentDraggingTargetBody());
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00003955 File Offset: 0x00001B55
		public override bool IsConditionsMetForVisibility()
		{
			return base.IsConditionsMetForVisibility() && SneakIntoTheVillaMissionController.Instance != null;
		}
	}
}
