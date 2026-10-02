using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using Storymode.Missions;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000029 RID: 41
	[Tutorial("StealthWalkSlowTutorial")]
	public class StealthWalkSlowTutorial : TutorialItemBase
	{
		// Token: 0x060000CA RID: 202 RVA: 0x000037C7 File Offset: 0x000019C7
		public StealthWalkSlowTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000037D6 File Offset: 0x000019D6
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000037D9 File Offset: 0x000019D9
		public override bool IsConditionsMetForActivation()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForActivation(SneakIntoTheVillaMissionController.MissionState.WalkSlow);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000037E1 File Offset: 0x000019E1
		public override bool IsConditionsMetForCompletion()
		{
			return Agent.Main != null && Agent.Main.WalkMode && SneakIntoTheVillaMissionController.Instance != null;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003800 File Offset: 0x00001A00
		public override bool IsConditionsMetForVisibility()
		{
			return base.IsConditionsMetForVisibility() && SneakIntoTheVillaMissionController.Instance != null;
		}
	}
}
