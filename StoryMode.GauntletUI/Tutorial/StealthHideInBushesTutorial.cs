using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using Storymode.Missions;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200002A RID: 42
	[Tutorial("StealthHideInBushesTutorial")]
	public class StealthHideInBushesTutorial : TutorialItemBase
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00003814 File Offset: 0x00001A14
		public StealthHideInBushesTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003823 File Offset: 0x00001A23
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003826 File Offset: 0x00001A26
		public override bool IsConditionsMetForActivation()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForActivation(SneakIntoTheVillaMissionController.MissionState.HideInBushes);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000382E File Offset: 0x00001A2E
		public override bool IsConditionsMetForCompletion()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForCompletion(SneakIntoTheVillaMissionController.MissionState.HideInBushes);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003836 File Offset: 0x00001A36
		public override bool IsConditionsMetForVisibility()
		{
			return base.IsConditionsMetForVisibility() && SneakIntoTheVillaMissionController.Instance != null;
		}
	}
}
