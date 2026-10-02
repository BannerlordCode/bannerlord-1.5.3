using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000026 RID: 38
	[Tutorial("GetQuestTutorial")]
	public class QuestScreenTutorial : TutorialItemBase
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00003687 File Offset: 0x00001887
		public QuestScreenTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "quest";
			base.MouseRequired = true;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000036A8 File Offset: 0x000018A8
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000036AB File Offset: 0x000018AB
		public override bool IsConditionsMetForActivation()
		{
			return Mission.Current == null && TutorialHelper.CurrentContext == TutorialContexts.QuestsScreen;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000036BF File Offset: 0x000018BF
		public override bool IsConditionsMetForCompletion()
		{
			return this._contextChangedToQuestsScreen;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000036C7 File Offset: 0x000018C7
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._contextChangedToQuestsScreen = obj.NewContext == TutorialContexts.QuestsScreen;
		}

		// Token: 0x04000035 RID: 53
		private bool _contextChangedToQuestsScreen;
	}
}
