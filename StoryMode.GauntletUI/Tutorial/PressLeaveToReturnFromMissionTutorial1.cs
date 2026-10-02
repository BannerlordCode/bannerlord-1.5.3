using System;
using System.Linq;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000018 RID: 24
	[Tutorial("PressLeaveToReturnFromMissionType1")]
	public class PressLeaveToReturnFromMissionTutorial1 : TutorialItemBase
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00002D92 File Offset: 0x00000F92
		public PressLeaveToReturnFromMissionTutorial1()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = string.Empty;
			base.MouseRequired = false;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002DB3 File Offset: 0x00000FB3
		public override bool IsConditionsMetForCompletion()
		{
			return this._changedContext;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002DBB File Offset: 0x00000FBB
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._changedContext = true;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002DC4 File Offset: 0x00000FC4
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public override bool IsConditionsMetForActivation()
		{
			string[] array = new string[] { "center", "lordshall", "tavern", "prison", "village_center", "arena" };
			return TutorialHelper.CurrentMissionLocation != null && array.Contains(TutorialHelper.CurrentMissionLocation.StringId) && TutorialHelper.PlayerIsInAnySettlement && !TutorialHelper.PlayerIsInAConversation && TutorialHelper.CurrentContext == TutorialContexts.Mission;
		}

		// Token: 0x0400001E RID: 30
		private bool _changedContext;
	}
}
