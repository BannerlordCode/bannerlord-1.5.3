using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using Storymode.Missions;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000028 RID: 40
	[Tutorial("StealthCrouchTutorial")]
	public class StealthCrouchTutorial : TutorialItemBase
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x0000377A File Offset: 0x0000197A
		public StealthCrouchTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003789 File Offset: 0x00001989
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000378C File Offset: 0x0000198C
		public override bool IsConditionsMetForActivation()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForActivation(SneakIntoTheVillaMissionController.MissionState.Crouch);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003794 File Offset: 0x00001994
		public override bool IsConditionsMetForCompletion()
		{
			return Agent.Main != null && Agent.Main.CrouchMode && SneakIntoTheVillaMissionController.Instance != null;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000037B3 File Offset: 0x000019B3
		public override bool IsConditionsMetForVisibility()
		{
			return base.IsConditionsMetForVisibility() && SneakIntoTheVillaMissionController.Instance != null;
		}
	}
}
