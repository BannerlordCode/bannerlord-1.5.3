using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using Storymode.Missions;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200002C RID: 44
	[Tutorial("StealthDarkZoneTutorial")]
	public class StealthDarkZoneTutorial : TutorialItemBase
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00003897 File Offset: 0x00001A97
		public StealthDarkZoneTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000038A6 File Offset: 0x00001AA6
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000038A9 File Offset: 0x00001AA9
		public override bool IsConditionsMetForActivation()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForActivation(SneakIntoTheVillaMissionController.MissionState.DarkZone);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000038B1 File Offset: 0x00001AB1
		public override bool IsConditionsMetForCompletion()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForCompletion(SneakIntoTheVillaMissionController.MissionState.DarkZone);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000038B9 File Offset: 0x00001AB9
		public override bool IsConditionsMetForVisibility()
		{
			return base.IsConditionsMetForVisibility() && SneakIntoTheVillaMissionController.Instance != null;
		}
	}
}
