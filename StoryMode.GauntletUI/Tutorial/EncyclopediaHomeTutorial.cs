using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000034 RID: 52
	[Tutorial("EncyclopediaHomeTutorial")]
	public class EncyclopediaHomeTutorial : TutorialItemBase
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00003E1C File Offset: 0x0000201C
		public EncyclopediaHomeTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "";
			base.MouseRequired = false;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00003E3D File Offset: 0x0000203D
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.EncyclopediaWindow;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00003E41 File Offset: 0x00002041
		public override bool IsConditionsMetForActivation()
		{
			this._isActive = GauntletTutorialSystem.Current.CurrentEncyclopediaPageContext == EncyclopediaPages.Home;
			return this._isActive;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00003E5C File Offset: 0x0000205C
		public override bool IsConditionsMetForCompletion()
		{
			return this._isActive && GauntletTutorialSystem.Current.CurrentEncyclopediaPageContext != EncyclopediaPages.Home;
		}

		// Token: 0x0400003F RID: 63
		private bool _isActive;
	}
}
